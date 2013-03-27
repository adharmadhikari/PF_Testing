using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using PathfinderModel;
using Pinsonault.Data;
using Pinsonault.Data.Reports;

namespace Pathfinder
{
    /// <summary>
    /// Summary description for ExporterBase
    /// </summary>
    public abstract class ExporterBase : IDisposable
    {
        public ExporterBase()
        {

            ReportSubsections = new List<ReportSubsection>();
            
            ReportDate = DateTime.Now;
            ProtectOutputFile = false;
        }
        ~ExporterBase()
        {
            Dispose(false);
        }
        
        /// <summary>
        /// Report title (Optional)
        /// </summary>
        /// <remarks>
        /// Shows up in template's header block.
        /// </remarks>
        public String Title { get; set; }

        public IList<ReportSubsection> ReportSubsections { get; private set; }

        private bool _disposed = false;

        /// <summary>
        /// Reporting date/time (Optional; Defaults date/time of instantiation.)
        /// </summary>
        /// <remarks>
        /// Shows up in template's header block.
        /// </remarks>
        public DateTime ReportDate { get; set; }

        /// <summary>
        /// Write protect the data in the output file?
        /// </summary>
        /// <remarks>
        /// Setting this will protect the document by generating and setting a random password on it when
        /// data manipulation is performed. Formatting/styling is still allowed.
        /// </remarks>
        public bool ProtectOutputFile { get; set; }


        protected ObjectContext ObjectContext { get; set; }



       /// <summary>
        /// Creates an instance of an appropriate Excel exporter and instantiates it with column mappings and data.
        /// </summary>
        /// <typeparam name="T">Specialized type of exporter to create.</typeparam>
        /// <param name="templateFile">Path to Excel template file. This is paired with the exporter logic.</param>
        /// <param name="reportKey">Report key to use (key name of report in the database lookup.)</param>
        /// <param name="tileKey">Tile key to use (key name of tile report is for.)</param>
        /// <param name="filterSet">Criteria filter set.</param>
        /// <returns></returns>
        public static ExporterBase CreateInstance<T>(String reportKey, NameValueCollection queryString) where T : ExporterBase
        {
            return CreateInstance<T>(null, reportKey, queryString);
        }

        public static ExporterBase CreateInstance<T>(Report report, String reportKey, NameValueCollection queryString) where T : ExporterBase
        {
            PathfinderModel.PathfinderEntities dataCtx = null;
            
            ExporterBase exporter = null;
            bool disposeCtx = true;
             int appID = 0;
            int channel = 0;
            int.TryParse(queryString["application"], out appID);
            int.TryParse(queryString["channel"], out channel);
            bool custom = false;

            try
            {
                dataCtx = new PathfinderModel.PathfinderEntities();
                
                //-------------------------------------------
                //check that user is authorized and determine if custom 
                ClientApplicationAccess caa = Pinsonault.Web.Session.ClientApplicationAccess.Where(ca => ca.Value.ApplicationID == appID && ca.Value.SectionID == channel).Select(ca => ca.Value).FirstOrDefault();
                if ( caa != null )
                    custom = caa.Section != null && caa.Section.Is_Custom;
                else
                    throw new ApplicationException("Access to requested data is denied.");

                int userID = Pinsonault.Web.Session.UserID;

                PathfinderModel.UserModule userModule = dataCtx.UserModuleSet.Where(m => m.User_ID == userID && m.Module_Key == reportKey && m.Section_ID == channel).FirstOrDefault();
                if ( userModule != null )
                {
                    if(string.IsNullOrEmpty(userModule.Role_Key) || HttpContext.Current.User.IsInRole(userModule.Role_Key))
                        custom = custom || userModule.Is_Custom;  //if channel or module are custom then flag is true
                    else
                        throw new ApplicationException("Access to requested data is denied.");
                }
                else
                {
                    //should not be needed since all reports are now in Client_Modules
                    //PathfinderModel.Module module = dataCtx.GetApplicationModules(Pinsonault.Web.Session.UserID, appID).Where(r => r.Module_Key == reportKey).FirstOrDefault();
                    //if ( module != null )
                    //    custom = custom || module.Is_Custom;
                    //else
                    throw new ApplicationException("Access to requested data is denied.");
                }
                //

                //SPH 6/24/2010 - ReportManager.GetReport takes care of loading correct report type - report definitions should have correct reportKey value to load columns
                //if ( custom )
                //    reportKey = string.Format("{0}_{1}", Pinsonault.Web.Session.ClientKey, reportKey);
                //------------------------------------------------------------------------------------------------------


                // Instantiate exporter
                exporter = (ExporterBase)Activator.CreateInstance<T>();

                if ( exporter != null )
                {
                    //IEnumerable<PathfinderModel.Module> reports = dataCtx.ModuleSet.Where(rpt => rpt.Module_Key == reportKey);
                    //if (reports.Count() > 0)
                    //    exporter.Title = reports.First().Name;
                    Report reportInstance = report;
                    if(reportInstance == null)
                        reportInstance = PathfinderReportManager.GetReport(dataCtx, reportKey, custom,appID);

                    exporter.Title = reportInstance.Title;

                    reportInstance.Initialize(dataCtx, queryString, custom);

                    //IList<NameValueCollection> filterSets = ExtractFilterSetsFromRequest(queryString, reportInstance.ReportDefinitions.Count);//  defs.Where(d => d.ReportKey == reportKey).Count());
                    //IList<NameValueCollection> images = ExtractImagesFromRequest(queryString);

                    exporter.ObjectContext = reportInstance.QueryContext; //reportInstance.CreateObjectContext(dataCtx, custom, reportInstance.FilterSets);

                    //Do not dispose of current context if still using (CreateObjectContextByGeography returns current because it is National or State)
                    //If client data context returned then we can dispose of dataCtx because it is no longer needed.
                    disposeCtx = !exporter.ObjectContext.Equals(dataCtx);

                    IList<ReportSubsection> subsections = reportInstance.CreateReportSections();
                    foreach ( ReportSubsection subsection in subsections )
                    {
                        if (subsection.Data.GetEnumerator().MoveNext())
                            exporter.ReportSubsections.Add(subsection);
                    }
                }               
                return exporter;
            }
            finally
            {
                if ( disposeCtx && dataCtx != null )
                    dataCtx.Dispose();
            }
        }

        public static ExporterBase CreateInstance<T>(String strReportName, DataSet ds) where T : ExporterBase
        {
            ExporterBase exporter = null;

            try
            {
                // Instantiate exporter
                exporter = (ExporterBase)Activator.CreateInstance<T>();

                if (exporter != null)
                {
                    exporter.Title = strReportName;
                }
                return exporter;
            }
            finally
            {

            }
        }

        /// <summary>
        /// Function to give the report criteria details for appending on report title
        /// </summary>
        /// <param name="subsection"></param>
        /// <param name="Title"></param>
        /// <param name="rptType"></param>
        /// <param name="ReportDate"></param>
        /// <returns></returns>
        public static string GetCriteriaDetails(ReportSubsection subsection, string Title, ReportType rptType, DateTime ReportDate)
        {
            const String divider = ";  ";
            string strLineBreak = "\n";
            if (rptType == ReportType.Print)
            {
                strLineBreak = "<br/>";
            }
            StringBuilder header = new StringBuilder();
            header.Append("Report Details" + strLineBreak);
            
            // Title/Name
            String documentTitle = String.IsNullOrEmpty(Title) ? "Untitled" : Title;

            header.Append(String.Concat("Name - ", documentTitle, strLineBreak));

            // Criteria
            if ( subsection.CriteriaItems.Count > 0 )
            {
                StringBuilder criteriaBuilder = new StringBuilder();
                                               
                foreach(CriteriaItem item in subsection.CriteriaItems.Values)
                {
                    criteriaBuilder.Append(item.Title);
                    criteriaBuilder.Append(": ");
                    criteriaBuilder.Append(item.Text);
                    criteriaBuilder.Append(divider);
                }
                if (criteriaBuilder.Length - divider.Length >= divider.Length)
                    criteriaBuilder.Remove(criteriaBuilder.Length - divider.Length, divider.Length);
                header.Append(String.Concat("Criteria - ",
                    subsection.CriteriaItems == null || subsection.CriteriaItems.Count == 0 ? "None" :
                    criteriaBuilder.ToString(), strLineBreak));
            }

            // Date
            header.Append(String.Format("Date - {0}\n", ReportDate));
            return header.ToString();
        }
        public static string GetPPRXCriteriaDetails(string Title, ReportType rptType, SortedList slCriteriaList)
        {
            const String divider = ";  ";
            string strLineBreak = "\n";
            if (rptType == ReportType.Print)
            {
                strLineBreak = "<br/>";
            }
            StringBuilder header = new StringBuilder();
            header.Append("Report Details" + strLineBreak);

            // Title/Name
            String documentTitle = String.IsNullOrEmpty(Title) ? "Untitled" : Title;

            header.Append(String.Format("Name - {0} {1}", documentTitle, strLineBreak));

            // Criteria
            if (slCriteriaList.Keys.Count > 0)
            {
                StringBuilder criteriaBuilder = new StringBuilder();

                for (int iListItem = 0; iListItem < slCriteriaList.Keys.Count; iListItem++)
                {
                    criteriaBuilder.Append(slCriteriaList.GetKey(iListItem));
                    criteriaBuilder.Append(": ");
                    criteriaBuilder.Append(slCriteriaList[slCriteriaList.GetKey(iListItem)]);
                    criteriaBuilder.Append(divider);
                    criteriaBuilder.Append(strLineBreak);
                }
                if (criteriaBuilder.Length - divider.Length >= divider.Length)
                    criteriaBuilder.Remove(criteriaBuilder.Length - divider.Length, divider.Length);
                header.Append(String.Format("Criteria - {0} {1}", criteriaBuilder.ToString(), strLineBreak));
            }
            return header.ToString();
        }

        public abstract string ExportToFile();
        public abstract void ExportToWeb(HtmlTextWriter writer);
        public abstract string ExportToFileWithFormat(string strHeader, string strReportName, DataSet dsReportSet, NameValueCollection QueryString, IList<int> MonthListDesc, int iReportType, DataTable dtReportSummary);

        public string OutputFolder { get; set; }

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            Dispose(true);
        }
        protected void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (ObjectContext != null)
                        ObjectContext.Dispose();

                    GC.SuppressFinalize(this);
                }

                _disposed = true;
            }
        }

        #endregion
    }
}