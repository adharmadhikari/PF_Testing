using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using Excel = Microsoft.Office.Interop.Excel; // Microsoft.Office.Interop.Excel, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71E9BCE111E9429C
using System.Data.Objects;
using PathfinderModel;
using System.Data.SqlClient;
using System.Configuration;
using log4net;

namespace Pathfinder
{
    /// <summary>
    /// Excel exporter.
    /// This class is thread safe.
    /// Use a concrete implementation such as <cref>StandardExcelExporter</cref>.
    /// </summary>
    public abstract class ExcelExporter : ExporterBase, IDisposable 
    {
        /// <summary>
        /// COM ProgID to use for instantiating Excel.
        /// </summary>
        private static readonly string EXCEL_PROGID = "Excel.Application";

        private static readonly ILog log = LogManager.GetLogger(typeof(ExcelExporter));

        private bool _disposed = false;

        /// <summary>
        /// Internal pool of Excel resources.
        /// This should not be used directly. Use the _pool instead.
        /// </summary>
        private static Excel.Application[] _resources;

        /// <summary>
        /// Resource pool representing _resources.
        /// Ensures that only one thread can access a given resource.
        /// </summary>
        private static readonly ResourcePool<Excel.Application> _pool;

        private volatile Excel.Application _excelApp;

        static ExcelExporter()
        {
            log.Info("Starting up...");
            DateTime startTime = DateTime.Now;

            // Determine maximum number of Excel processes to start.
            int poolSize = int.Parse(ConfigurationManager.AppSettings["ExcelPoolSize"]);
            if (poolSize < 1)
            {
                log.Error(String.Format("ExcelAppPoolSize has an invalid value of {0:d}", poolSize));
                throw new ArgumentOutOfRangeException("ExcelPoolSize (in the AppSettings section of web.config) must be > 0");
            }

            // Start Excel processes.
            log.Info(String.Format("Starting {0:d} processes in the Excel pool...", poolSize));
            _resources = new Excel.Application[poolSize];
            for (int i = 0; i < _resources.Length; ++i)
                _resources[i] = CreateAppInstance();

            // Start pool.
            _pool = new ResourcePool<Excel.Application>(_resources);

            TimeSpan interval = startTime - DateTime.Now;
            log.Info(String.Format("Startup completed in {0}.", interval));
        }

        ~ExcelExporter()
        {
            Dispose(true);
        }

        /// <summary>
        /// Release current ExcelApp back to pool.
        /// </summary>
        protected void ReleaseExcelApp()
        {
            if (_excelApp != null)
            {
                _pool.Release(_excelApp);
                _excelApp = null;
                log.Debug("Released ExcelApp.");
            }
        }

        /// <summary>
        /// Excel application.
        /// </summary>
        /// <remarks>
        /// This represents the Excel process handle, and one will be spawned if it doesn't already exist.
        /// Since creating a new process is spendy, this should remain alive as long as possible and created on demand.
        /// </remarks>
        protected Excel.Application ExcelApp
        {
            get
            {
                log.Debug("ExcelApp requested. Acquiring...");
                Excel.Application app = _excelApp != null ? _excelApp : _pool.Acquire(true);
                log.Debug("Acquired ExcelApp.");

                if (app == null)
                {
                    log.Warn("ExcelApp was null. Starting process...");
                    app = CreateAppInstance();
                }

                if (app != null)
                {
                    try
                    {
                        // Invoke arbitrary COM method to ensure process is still alive.
                        String version = app.Version;
                    }
                    catch (COMException)
                    {
                        // The Excel process likely croaked. Restart it.
                        log.Error("ExcelApp croaked. Reinitiailizing...");
                        DestroyAppInstance(app);
                        app = CreateAppInstance();
                    }
                }

                if (app == null)
                    log.Error("ExcelApp could not be reinitialized.");

                _excelApp = app;
                return app;
            }
        }

        /*
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowThreadProcessId(int hWnd, out int processId);
        */

        /// <summary>
        /// Creates a new Excel process.
        /// Does not place it into the pool. That should be done by the caller.
        /// </summary>
        /// <returns></returns>
        private static Excel.Application CreateAppInstance()
        {
            log.Debug("Creating Excel instance...");
            Excel.Application app = null;

            // Start the Excel process.
            if ((app = (Excel.Application)Activator.CreateInstance(
                   Type.GetTypeFromProgID(EXCEL_PROGID))) != null)
            {
                // Record PID to log.
                /*
                int processId = 0;
                GetWindowThreadProcessId(app.Hwnd, out processId);
                if (processId != 0)
                    log.DebugFormat("Created Excel process with pid={0:d}, hwnd={1:d}.", processId, app.Hwnd);
                */

                // Disable dialog alerts.
                app.DisplayAlerts = false;
            }

            AppShutdownExecutor.Enqueue(
                delegate()
                {
                    DestroyAppInstance(app);
                }
            );

            return app;
        }

        /// <summary>
        /// Destroys an existing Excel process.
        /// Does not remove it from the pool. That should be done by the caller.
        /// </summary>
        /// <param name="instance"></param>
        private static void DestroyAppInstance(Excel.Application instance)
        {
            if (instance == null)
                return;

            log.Debug("Destroying Excel instance...");

            // Re-entrancy allowed on Monitor.
            lock (instance)
            {
                // Kill that sumbitch.
                instance.Quit();
                Marshal.ReleaseComObject(instance);
            }
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
        }
        protected void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                // If instance had an unreleased ExcelApp, release it back to pool.
                ReleaseExcelApp();

                if (disposing)
                {
                    base.Dispose(disposing);
                    GC.SuppressFinalize(this);
                }

                _disposed = true;
            }
        }
    }
}
