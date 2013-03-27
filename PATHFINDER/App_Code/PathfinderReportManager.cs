using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using Pinsonault.Application;

namespace Pinsonault.Data.Reports
{
/// <summary>
/// Summary description for PathfinderReportManager
/// </summary>
    public static class PathfinderReportManager
    {
        static Dictionary<string, Type> _reports = new Dictionary<string, Type>();

        //public PathfinderReport this[string reportKey]
        //{
        //    get { return _reports[reportKey]; }
        //}

        static void Add(string Name, Type reportType)
        {
            _reports.Add(Name.ToLower(), reportType);            
        }

        public static Report GetReport(string Name, bool CheckCustom, int AppID)
        {
            using ( PathfinderModel.PathfinderEntities context = new PathfinderModel.PathfinderEntities() )
            {
                return GetReport(context, Name, CheckCustom, AppID);
            }
        }

        public static Report GetReport(PathfinderModel.PathfinderEntities context, string Name, bool CheckCustom, int AppID)
        {
            if ( string.IsNullOrEmpty(Name) )
                throw new ArgumentException("GetReport failed due to missing Name parameter.", "Name");

            string name = Name.ToLower();

            var p = context.ClientModuleSet;
            var q = context.ModuleSet;
            PathfinderModel.ClientModule clientmodule = p.Where(cm => cm.Client_ID == Pinsonault.Web.Session.ClientID).Where(cm => cm.Modules.Module_Key == name).FirstOrDefault();
            
            string reportType = null;
            reportType = clientmodule.Report_Type_Name;

            PathfinderModel.Module module = q.Where(m => m.Module_Key == name).Where(m => m.Application.ID == AppID).FirstOrDefault();
          
            
            if ( string.IsNullOrEmpty(reportType) )
                reportType = module.Report_Type_Name;

            if ( !string.IsNullOrEmpty(reportType) )
            {
                Type type = Type.GetType(reportType, false, true);
                if ( type != null )
                {
                    Report report = type.GetConstructor(Type.EmptyTypes).Invoke(null) as Report;
                    report.Title = module.Name;
                    report.ReportKey = name;
                    //temp fix for Geographic Coverage Report
                    if (System.Web.HttpContext.Current.Request.QueryString["report"] == "geographiccoverage")
                        report.Title = "Geographic Coverage Report";
                    //Title Name for Millennium Reports, since Report Title needs to be chnaged from contacts to Key Decision Makers
                    if (System.Web.HttpContext.Current.Request.QueryString["report"] == "contacts" && Pinsonault.Web.Session.ClientID == 50)
                        report.Title = "Key Decision Maker Report";
                    return report;
                }
            }
            throw new ApplicationException(string.Format("GetReport failed because the report with name {0} could not be found or was not defined.", Name));
        }

        //static PathfinderReportManager()
        //{
        //    Add("coveredlives", typeof(StandardReports.CoveredLivesReport));
        //    Add("customercontactdrilldown", typeof(CustomerContactDrilldownReport));
        //    Add("formularydrilldown", typeof(FormularyDrillDownReport));
        //    Add("formularystatus", typeof(FormularyStatusReport));
        //    Add("contacts", typeof(KeyContactsReport));
        //    Add("meetingactivity", typeof(MeetingActivityReport));
        //    Add("meetingtype", typeof(MeetingTypeReport));
        //    Add("planinformation", typeof(PlanInformationReport));
        //    Add("productsdiscussed", typeof(ProductsDiscussedReport));
        //    Add("reckitt_planinformation", typeof(ReckittPlanInformationReport));
        //    Add("tiercoveragecomparison", typeof(TierCoverageReport));
        //    Add("reckitt_businessplanning", typeof(ReckittBusinessPlanning));
        //    Add("formularydrilldowntop10bystate", typeof(FormularyDrillDownTop10ByStateReport));
        //}
    }
}