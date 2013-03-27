using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Objects;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Pathfinder;
using log4net;

namespace Pinsonault.Web
{
    public class ExportFile : GenericHttpHandler
    {
        private static ILog log = LogManager.GetLogger(typeof(ExportFile));

        private void ShowError(HttpResponse response, String message)
        {
            response.StatusCode = 500;
            response.ContentType = "text/html";
            response.Write(
                String.Concat(
                    "<h1>An error occurred while exporting the data.</h1>",
                    "<pre>", message, "</pre>"));
        }

        protected virtual void RenderExportExcel(HttpResponse response, string report, NameValueCollection queryString)
        {
            try
            {
                using (ExporterBase exporter = ExporterBase.CreateInstance<StandardExcelExporter>(report, queryString))
                {
                    if (exporter.ReportSubsections.Count > 0)
                    {
                        //SPH 6/15/2010 - As per Tony's request the exports will not be protected
                        //exporter.ProtectOutputFile = true;
                        String outputFile = exporter.ExportToFile();

                        // Send response data and headers
                        response.ContentType = "application/x-excel";
                        response.AddHeader("Content-Disposition",
                            String.Format("attachment; filename=\"{0}-{1}.xls\"",
                                report, Guid.NewGuid().ToString()));
                        //response.AddHeader("Pragma", "no-cache");
                        response.TransmitFile(outputFile);

                        // Direct calls to File.Delete can cause hangs.
                        AppShutdownExecutor.Enqueue(
                            delegate()
                            {
                                File.Delete(outputFile);
                            }
                        );
                    }
                    else
                    //show a message that no records found
                    {                        
                        response.StatusCode = 200;
                        response.ContentType = "text/html";                       
                        response.Write(String.Concat("<script language='javascript'>window.resizeTo(500,300);</script><title>Export Information</title>No Records found."));
                    }
                }
            }
#if DEBUG
        //catch (Exception ex)
        //{
        //    ShowError(response, ex.Message);
        //}
#endif
            finally { }
        }

        protected override void InternalProcessRequest(HttpContext context)
        {
            HttpRequest request = context.Request;
            String report = request["report"];

            if ( string.Compare(report, "geographiccoverage", true) == 0 )
                report = "formularystatus";
            else if ( string.Compare(report, "formularydrilldown", true) == 0 )
            {
                //VERIFY USER HAS PERMISSION TO EXPORT FORMULARY DRILL DOWN DATA
                if ( !HttpContext.Current.User.IsInRole("sr_fdx") )
                    throw new HttpException(403, "User is not authorized to export data.");
            }

            RenderExportExcel(context.Response, report, context.Request.QueryString);
        }

        public override bool IsReusable
        {
            get { return false; }
        }
    }
}