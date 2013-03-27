using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Collections.Specialized;

namespace Pathfinder
{
    /// <summary>
    /// Summary description for HtmlExporter
    /// </summary>
    public class HtmlExporter : ExporterBase 
    {

        public override string ExportToFile()
        {
            throw new NotImplementedException();
        }

        public override void ExportToWeb(HtmlTextWriter writer)
        {
            writer.Write("Finish Me!");
        }
        public override string ExportToFileWithFormat(string strHeader, string strReportName, DataSet dsReportSet, NameValueCollection QueryString, IList<int> MonthListDesc, int iReportType, DataTable dtReportSummary)
        {
            return string.Empty;
        }
    }
}