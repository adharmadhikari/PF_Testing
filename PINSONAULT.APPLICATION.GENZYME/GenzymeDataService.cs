using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pinsonault.Web.Services;
using System.Data.Services;
using System.Linq.Expressions;
using System.ServiceModel.Web;
using PathfinderClientModel;
using System.Web;
using System.Data.Objects;
using Pinsonault.Data;

namespace Pinsonault.Application.Genzyme
{
    public class GenzymeDataService : PathfinderDataServiceBase<PathfinderGenzymeEntities>
    {
        // This method is called only once to initialize service-wide policies.
        public static void InitializeService(IDataServiceConfiguration config)
        {
           Pinsonault.Web.Support.InitializeService(config);
           config.MaxResultsPerCollection = 1000;

            config.SetEntitySetAccessRule("ContactReportDataSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("ContactReportProductsDiscussedViewSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("PlanDocumentsViewSet", EntitySetRights.AllRead);
            //config.SetEntitySetAccessRule("PlanInfoListViewSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("PlanSearchSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("PlansSectionViewSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("PlansGridSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("ContactReportFollowupNotesSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("ContactReportOutcomeSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("ContactReportMeetingActivitySet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("ContactReportDrillDownSet", EntitySetRights.AllRead);
        }

        #region Query Interceptors
        [QueryInterceptor("ContactReportDataSet")]
        public Expression<Func<ContactReportData, bool>> FilterReports()
        {
            string strwhere = string.Empty;
            int[] ContactReportIDs = { 0 };

            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["Meeting_Activity_ID"]))
            {
                string meetingActivityIds = System.Web.HttpContext.Current.Request.QueryString["Meeting_Activity_ID"].ToString();
                strwhere = " it.Meeting_Activity_ID in {" + meetingActivityIds + "}";
            }

            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["Meeting_Outcome_ID"]))
            {
                string meetingOutcomeIds = System.Web.HttpContext.Current.Request.QueryString["Meeting_Outcome_ID"].ToString();
                if (string.IsNullOrEmpty(strwhere))
                    strwhere = " it.Outcome_ID in {" + meetingOutcomeIds + "}";
                else
                    strwhere = strwhere + " and it.Outcome_ID in {" + meetingOutcomeIds + "}";
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

        [QueryInterceptor("ContactReportDrillDownSet")]
        public Expression<Func<ContactReportDrillDown, bool>> FilterDrillDown()
        {
            string strwhere = string.Empty;
            int[] ContactReportIDs = { 0 };
            
            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["Meeting_Activity_ID"]))
            {
                string meetingActivityIds = System.Web.HttpContext.Current.Request.QueryString["Meeting_Activity_ID"].ToString();
                strwhere = " it.Meeting_Activity_ID in {" + meetingActivityIds + "}";
            }

            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["Meeting_Outcome_ID"]))
            {
                string meetingOutcomeIds = System.Web.HttpContext.Current.Request.QueryString["Meeting_Outcome_ID"].ToString();
                
                if (string.IsNullOrEmpty(strwhere))
                    strwhere = " it.Outcome_ID in {" + meetingOutcomeIds + "}";
                else
                    strwhere = strwhere + " and it.Outcome_ID in {" + meetingOutcomeIds + "}";

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

            //if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["Products_Discussed_ID"]))
            //    System.Web.HttpContext.Current.Request.QueryString.Remove("Products_Discussed_ID");

            //if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["Meeting_Activity_ID"]))
            //    System.Web.HttpContext.Current.Request.QueryString.Remove("Meeting_Activity_ID");

            if (!string.IsNullOrEmpty(strwhere))
            {
                var list = (from CRID in CurrentDataSource.ContactReportDrillDownFilterSet.Where(strwhere)
                            orderby CRID.Contact_Report_ID
                            select CRID.Contact_Report_ID).ToList().Distinct();
                if (list.Count() > 0)
                    ContactReportIDs = list.ToArray();

                return Pinsonault.Data.Generic.GetFilterForList<ContactReportDrillDown, int>(ContactReportIDs, "Contact_Report_ID");

            }
            else
                return e => true;

        }
       
        [QueryInterceptor("PlansSectionViewSet")]
        public Expression<Func<PlansSectionView, bool>> FilterPlansByTerritory()
        {
            string terrID = Pinsonault.Web.Session.TerritoryID;

            if (!string.IsNullOrEmpty(terrID))
                return p => p.Territory_ID == terrID;
            else //if user does not have a terr id then return True since they may not have plan alignments (THIS IS DIFFERENT THAN TA WHERE FALSE IS DEFAULT)
                return p => true;
        }

        [QueryInterceptor("PlansSectionViewSet")]
        public Expression<Func<PlansSectionView, bool>> FilterPlans()
        {
            return Pinsonault.Web.Data.LINQHelper.GenFilterPlansByClientSections<PlansSectionView>(Pinsonault.Web.Identifiers.TodaysAccounts);
        }

        [QueryInterceptor("PlansGridSet")]
        public Expression<Func<PlansGrid, bool>> FilterPlanByUser()
        {
            int userID = Pinsonault.Web.Session.UserID;
            return e => e.AE_UserID == userID;
        }
        #endregion

        [WebGet]
        public string GetCCRModuleOptions()
        {
            using (PathfinderModel.PathfinderEntities context = new PathfinderModel.PathfinderEntities())
            {
                return context.GetUserReportOptionsAsJSON(Pinsonault.Web.Session.UserID, Pinsonault.Web.Identifiers.CustomerContactReports);
            }
        }

        [WebGet]
        public string GetBusinessPlanningOptions()
        {
            using (PathfinderModel.PathfinderEntities context = new PathfinderModel.PathfinderEntities())
            {
                return context.GetUserReportOptionsAsJSON(Pinsonault.Web.Session.UserID, Pinsonault.Web.Identifiers.BusinessPlanning);
            }
        }

        //[WebGet]
        //public int GetPlanInfoListViewCount(string where)
        //{
        //    if (String.IsNullOrEmpty(where))
        //        return CurrentDataSource.PlanInfoListViewSet.Count();
        //    else
        //        return CurrentDataSource.PlanInfoListViewSet.Where(where).Count();
        //}
        [WebGet]
        public int GetPlanSectionViewCount(string where)
        {
            if (String.IsNullOrEmpty(where))
                return CurrentDataSource.PlansSectionViewSet.Where(FilterPlansByTerritory()).Where(FilterPlans()).Count();
            else
                return CurrentDataSource.PlansSectionViewSet.Where(where).Where(FilterPlansByTerritory()).Where(FilterPlans()).Count();
        }

        [WebGet]
        public int GetDocumentCount(string where)
        {

            if (string.IsNullOrEmpty(where))
                return CurrentDataSource.PlanDocumentsViewSet.Count();
            else
                return CurrentDataSource.PlanDocumentsViewSet.Where(where).Count();
        }

        [WebGet]
        public int GetCRDrillDownCount(string where)
        {

            if (string.IsNullOrEmpty(where))
                return CurrentDataSource.ContactReportDataSet.Count();
            else
                return CurrentDataSource.ContactReportDataSet.Where(where.Replace("%27", "'")).Count();
        }

        [WebGet]
        public int GetCCReportCount(string where)
        {
            if (string.IsNullOrEmpty(where))
                return CurrentDataSource.ContactReportProductsDiscussedViewSet.Count();
            else
                return CurrentDataSource.ContactReportProductsDiscussedViewSet.Where(where).Count();
        }

        [WebGet]
        public int GetCCRDrillDownCount(string where)
        {
            if (string.IsNullOrEmpty(where))
                return CurrentDataSource.ContactReportDrillDownSet.Count();
            else
                return CurrentDataSource.ContactReportDrillDownSet.Where(where).Count();
        }
      }
}
