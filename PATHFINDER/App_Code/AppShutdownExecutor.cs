using System;
using System.Collections.Generic;
using log4net;

namespace Pathfinder
{
    /// <summary>
    /// Used to enqueue delegate methods to be executed prior to application shutdown.
    /// This class is thread safe.
    /// </summary>
    public static class AppShutdownExecutor
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AppShutdownExecutor));

        public delegate void DeferredMethod();

        private static Queue<DeferredMethod> _queue = new Queue<DeferredMethod>();

        /// <summary>
        /// Do not call this directly!
        /// This should only be called from Global.asax:Application_OnEnd().
        /// </summary>
        public static void Execute()
        {
            lock (_queue)
            {
                log.Info(String.Format("Executing {0:d} deferred commands...", _queue.Count));
                while (_queue.Count > 0)
                {
                    try
                    {
                        Delegate func = _queue.Dequeue();
                        try
                        {
                            func.DynamicInvoke();
                        }
                        catch (Exception ex)
                        {
                            log.Error(String.Concat(
                                "Error executing deferred method: ", ex.Message));
                            log.Error(ex.StackTrace);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message);
                    }
                }
                log.Info("Execution batch complete.");
            }
        }

        /// <summary>
        /// Queue delegate to execute upon application shutdown.
        /// </summary>
        /// <param name="func">Delegate to execute on shutdown.</param>
        public static void Enqueue(DeferredMethod func)
        {
            lock (_queue)
            {
                _queue.Enqueue(func);
            }
        }
    }
}
