using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pinsonault.Web.Services;
using System.Data.Services;
using System.Linq.Expressions;
using System.Web;

namespace Pinsonault.Application.Auxilium
{
    public class AuxiliumDataService : PathfinderDataServiceBase<PathfinderAuxiliumEntities>
    {
        // This method is called only once to initialize service-wide policies.
        public static void InitializeService(IDataServiceConfiguration config)
        {
            
            Pinsonault.Web.Support.InitializeService(config);
            config.MaxResultsPerCollection = 1000;
            config.SetEntitySetAccessRule("PlanListSet", EntitySetRights.AllRead);
            //used for CCR drilldown report
            config.SetEntitySetAccessRule("ContactReportDataSet", EntitySetRights.AllRead);
            //used for ccr reports - meeting activity, meeting type and product discussed report
            config.SetEntitySetAccessRule("ContactReportProductDataSet", EntitySetRights.AllRead);
        }


        #region Query Interceptors
        [QueryInterceptor("PlanListSet")]
        public Expression<Func<PlanList, bool>> FilterPlanByUser()
        {
            int userID = Pinsonault.Web.Session.UserID;
            return e => e.AE_UserID == userID;
        }
        [QueryInterceptor("ContactReportDataSet")]
        public Expression<Func<ContactReportData, bool>> FilterDrillDown()
        {
            string strwhere = string.Empty;
            int[] ContactReportIDs = { 0 };

            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["Meeting_Outcome_ID"]))
            {
                string meetingOutcomeIds = System.Web.HttpContext.Current.Request.QueryString["Meeting_Outcome_ID"].ToString();
                strwhere = " it.Outcome_ID in {" + meetingOutcomeIds + "}";
            }
            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["Followup_Notes_ID"]))
            {
                string followupIds = System.Web.HttpContext.Current.Request.QueryString["Followup_Notes_ID"].ToString();
                if (string.IsNullOrEmpty(strwhere))
                    strwhere = " it.Followup_ID in {" + followupIds + "}";
                else
                    strwhere = strwhere + " and it.Followup_ID in {" + followupIds + "}";

            }
            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["Products_Discussed_ID"]))
            {
                string productsdiscussedIds = System.Web.HttpContext.Current.Request.QueryString["Products_Discussed_ID"].ToString();
                if (string.IsNullOrEmpty(strwhere))
                    strwhere = " it.Products_Discussed_ID in {" + productsdiscussedIds + "}";
                else
                    strwhere = strwhere + " and it.Products_Discussed_ID in {" + productsdiscussedIds + "}";
            }
            if (!string.IsNullOrEmpty(strwhere))
            {
                var list = (from CRID in CurrentDataSource.ContactReportDrillDownFilterSet.Where(strwhere)
                            orderby CRID.Contact_Report_ID
                            select CRID.Contact_Report_ID).ToList().Distinct();
                if (list.Count() > 0)
                    ContactReportIDs = list.ToArray();

                return Pinsonault.Data.Generic.GetFilterForList<ContactReportData, int>(ContactReportIDs, "Contact_Report_ID");

            }
            else
                return e => true;

        }
        [QueryInterceptor("ContactReportProductDataSet")]
        public Expression<Func<ContactReportProductData, bool>> FilterReports()
        {
            string strwhere = string.Empty;
            int[] ContactReportIDs = { 0 };

            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["Meeting_Outcome_ID"]))
            {
                string meetingOutcomeIds = System.Web.HttpContext.Current.Request.QueryString["Meeting_Outcome_ID"].ToString();
                strwhere = " it.Outcome_ID in {" + meetingOutcomeIds + "}";
            }
            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["Followup_Notes_ID"]))
            {
                string followupIds = System.Web.HttpContext.Current.Request.QueryString["Followup_Notes_ID"].ToString();
                if (string.IsNullOrEmpty(strwhere))
                    strwhere = " it.Followup_ID in {" + followupIds + "}";
                else
                    strwhere = strwhere + " and it.Followup_ID in {" + followupIds + "}";

            }
            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["Products_Discussed_ID"]))
            {
                string productsdiscussedIds = System.Web.HttpContext.Current.Request.QueryString["Products_Discussed_ID"].ToString();
                if (string.IsNullOrEmpty(strwhere))
                    strwhere = " it.Products_Discussed_ID in {" + productsdiscussedIds + "}";
                else
                    strwhere = strwhere + " and it.Products_Discussed_ID in {" + productsdiscussedIds + "}";
            }
            if (!string.IsNullOrEmpty(strwhere))
            {
                var list = (from CRID in CurrentDataSource.ContactReportDrillDownFilterSet.Where(strwhere)
                            orderby CRID.Contact_Report_ID
                            select CRID.Contact_Report_ID).ToList().Distinct();
                if (list.Count() > 0)
                    ContactReportIDs = list.ToArray();

                return Pinsonault.Data.Generic.GetFilterForList<ContactReportProductData, int>(ContactReportIDs, "Contact_Report_ID");

            }
            else
                return e => true;
        }
        #endregion

       
    }
   
}
