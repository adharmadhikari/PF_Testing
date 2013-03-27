using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pinsonault.Web.Services;
using System.Data.Services;
using PathfinderClientModel;
using System.Linq.Expressions;
using Pinsonault.Data;
using System.ServiceModel.Web;

namespace Pinsonault.Application.Nipro
{
    public class NiproDataService : PathfinderClientDataServiceBase<PathfinderClientEntities>
    {
        // This method is called only once to initialize service-wide policies.
        public static void InitializeService(IDataServiceConfiguration config)
        {
            Pinsonault.Web.Support.InitializeService(config);

            config.SetEntitySetAccessRule("ContactReportDataSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("ContactReportProductsDiscussedViewSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("PlanDocumentsViewSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("PlanInfoListViewSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("PlanSearchSet", EntitySetRights.AllRead);            
            config.SetEntitySetAccessRule("PlansSectionViewSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("PlansGridSet", EntitySetRights.AllRead);
        }

        #region Query Interceptors


        [QueryInterceptor("ContactReportDataSet")]
        public Expression<Func<ContactReportData, bool>> FilterDrillDown()
        {
            bool hasCustomFilter = false;

            List<int> ccrIDs = new List<int>();                        
            string meetingOutcomeIds = System.Web.HttpContext.Current.Request.QueryString["Meeting_Outcome_ID"];
            if (!string.IsNullOrEmpty(meetingOutcomeIds))
            {
                var ids = meetingOutcomeIds.Replace(" ", string.Empty).Split(',').Select(s => Convert.ToInt32(s));

                hasCustomFilter = true;

                ccrIDs = CurrentDataSource.ContactReportOutcomeSet.Where(Generic.GetFilterForList<ContactReportOutcome, int>(ids, "Outcome_ID")).Select(i=>i.Contact_Report_ID).ToList();
            }

            string followupIds = System.Web.HttpContext.Current.Request.QueryString["Followup_Notes_ID"];
            if (!string.IsNullOrEmpty(followupIds))
            {
                hasCustomFilter = true;

                var ids = followupIds.Replace(" ", string.Empty).Split(',').Select(s => Convert.ToInt32(s));
                var q = CurrentDataSource.ContactReportFollowupNotesSet.Where(Generic.GetFilterForList<ContactReportFollowupNotes, int>(ids, "Followup_ID")).Select(i => i.Contact_Report_ID).ToList();
                if (ccrIDs.Count > 0)
                    ccrIDs = q.Intersect(ccrIDs).ToList();
                else
                    ccrIDs = q;
            }
            

            if (ccrIDs.Count> 0)
            {             
                return Pinsonault.Data.Generic.GetFilterForList<ContactReportData, int>(ccrIDs, "Contact_Report_ID");
            }
            else
                return e => !hasCustomFilter; //if custom filters were passed but no ccr ids were found then we should not return anything

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
    }
}
