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

namespace Pinsonault.Application.Alcon
{
    public class AlconDataService : PathfinderClientDataServiceBase<PathfinderClientEntities>
    {
        // This method is called only once to initialize service-wide policies.
        public static void InitializeService(IDataServiceConfiguration config)
        {
            Pinsonault.Web.Support.InitializeService(config);
            //config.SetEntitySetAccessRule("PlanListSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("ContactReportDataSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("ContactReportProductsDiscussedViewSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("PlanDocumentsViewSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("PlanInfoListViewSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("PlanSearchSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("PlansSectionViewSet", EntitySetRights.AllRead);
            //config.SetEntitySetAccessRule("PlansGridSet", EntitySetRights.AllRead);
        }

        #region Query Interceptors

        //[QueryInterceptor("PlanListSet")]
        //public Expression<Func<PlanList, bool>> FilterPlanByUser()
        //{
        //    int userID = Pinsonault.Web.Session.UserID;
        //    return e => e.AE_UserID == userID;
        //}
        [QueryInterceptor("ContactReportDataSet")]
        public Expression<Func<ContactReportData, bool>> FilterDrillDown()
        {
            bool hasCustomFilter = false;

            List<int> ccrIDs = new List<int>();
        
         if (ccrIDs.Count > 0)
            {
                return Pinsonault.Data.Generic.GetFilterForList<ContactReportData, int>(ccrIDs, "Contact_Report_ID");
            }
            else
                return e => !hasCustomFilter; //if custom filters were passed but no ccr ids were found then we should not return anything

        }
        //[QueryInterceptor("PlansGridSet")]
        //public Expression<Func<PlansGrid, bool>> FilterPlanByUser()
        //{
        //    int userID = Pinsonault.Web.Session.UserID;
        //    return e => e.AE_UserID == userID;
        //}

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
        public string GetActivityReportingModuleOptions()
        {
            using (PathfinderModel.PathfinderEntities context = new PathfinderModel.PathfinderEntities())
            {
                return context.GetUserReportOptionsAsJSON(Pinsonault.Web.Session.UserID, Pinsonault.Web.Identifiers.ActivityReporting);
            }
        }

        [WebGet]
        public int CreateSellSheet(string Created)
        {
            using (PathfinderClientEntities context = new PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
            {
                DateTime created;
                if (DateTime.TryParse(Created, out created))
                {
                    SellSheet sellSheet = new SellSheet();
                    sellSheet.Sell_Sheet_Name = string.Format("Draft - {0:d}", created);
                    sellSheet.Status_ID = 1;
                    sellSheet.Current_Step = "classandtemplateselection";
                    sellSheet.Territory_ID = Pinsonault.Web.Session.TerritoryID;
                    sellSheet.Include_Territory_Name = true;
                    //Set type = "Tier Status" by default
                    sellSheet.Type_ID = 1;
                    sellSheet.Created_BY = Pinsonault.Web.Session.UserID.ToString();//Pinsonault.Web.Session.FullName;
                    sellSheet.Created_DT = created;// DateTime.UtcNow;
                    sellSheet.Modified_DT = sellSheet.Created_DT;
                    sellSheet.Modified_BY = sellSheet.Created_BY;
                    context.AddToSellSheetSet(sellSheet);
                    context.SaveChanges();

                    return sellSheet.Sell_Sheet_ID;
                }
            }

            return 0;
        }

        [WebGet]
        public bool check_plantype(int planid,int productid)
        {

            bool united = false;
            using (PathfinderAlconEntities context = new PathfinderAlconEntities())
            {
                var u = (from a in context.V_Sell_Sheet_Plan_List_United
                         where a.Plan_Id == planid && a.Product_Id == productid
                         select a).FirstOrDefault();
                if (u != null)
                    united = true;
            }

            return united;
        }
        [WebGet]
        public bool check_template(int theraid, int templateid)
        {

            bool valid = false;
            using (PathfinderClientEntities context = new PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
            {
                var u = (from a in context.TheraDrugTemplateSet
                         where a.Thera_ID == theraid && a.Template_ID == templateid
                         select a).FirstOrDefault();
                if (u != null)
                    valid = true;
            }

            return valid;
        }
    }
}