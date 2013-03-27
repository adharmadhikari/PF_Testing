using System.Data.Services;
using System.ServiceModel.Web;
using System.Text;
using PathfinderClientModel;
using System.Linq;
using System.Web;
using System;
using System.Data.Objects;
using System.Linq.Expressions;
using System.Collections.Specialized;
using System.Collections.Generic;
using Pinsonault.Web.Services;

namespace Pinsonault.Application.Merz
{
    public class MerzDataService : PathfinderDataServiceBase<PathfinderMerzEntities>
    {
        // This method is called only once to initialize service-wide policies.
        public static void InitializeService(IDataServiceConfiguration config)
        {
            Pinsonault.Web.Support.InitializeService(config);
            config.MaxResultsPerCollection = 1000;
            //Important: Only list Entity Sets that should be accessible through the web service.  If client side queries are not required to populate a grid or another control do not make it public     

            config.SetEntitySetAccessRule("BusinessPlanMedicalPolicyDocSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("BusinessPlanMedicalPolicySet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("BusinessPlansMerzSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("BusinessPlansSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("PlanSearch1Set", EntitySetRights.AllRead);
        }


        #region Query Interceptors

        [QueryInterceptor("BusinessPlansMerzSet")]
        public Expression<Func<BusinessPlansMerz, bool>> FilterBusinessPlansMerzByDocumentType()
        {
            string val = System.Web.HttpContext.Current.Request.QueryString["DocumentType"];
            if (!string.IsNullOrEmpty(val))
            {
                int DocTypeID;
                if (int.TryParse(val, out DocTypeID))
                {
                    return e => (e.Document_Type_Flag & DocTypeID) == DocTypeID;
                }
            }

            return e => true; //get all - no DocumentType filtering
        }

        #endregion

        [WebGet]
        public int GetBPReportCount(string where)
        {
            if (String.IsNullOrEmpty(where))
                return CurrentDataSource.BusinessPlansMerzSet.Count();
            else
                return CurrentDataSource.BusinessPlansMerzSet.Where(where).Count();
        }

        //[WebGet]
        //public int CreateSellSheet(string Created)
        //{
        //    using ( PathfinderClientEntities context = new PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString) )
        //    {
        //        DateTime created;
        //        if ( DateTime.TryParse(Created, out created) )
        //        {
        //            //MerzSellSheet sellSheet = new MerzSellSheet();
        //            SellSheet sellSheet = new SellSheet();
        //            sellSheet.Sell_Sheet_Name = string.Format("Draft - {0:d}", created);
        //            sellSheet.Status_ID = 1;
        //            sellSheet.Current_Step = "classandtemplateselection";
        //            sellSheet.Territory_ID = Pinsonault.Web.Session.TerritoryID;
        //            sellSheet.Include_Territory_Name = true;
        //            //Set type = "Tier Status" by default
        //            sellSheet.Type_ID = 1;
        //            sellSheet.Created_BY = Pinsonault.Web.Session.FullName;
        //            sellSheet.Created_DT = DateTime.UtcNow;
        //            sellSheet.Modified_DT = sellSheet.Created_DT;
        //            sellSheet.Modified_BY = sellSheet.Created_BY;
        //            //CurrentDataSource.AddToMerzSellSheetSet(sellSheet);
        //            //CurrentDataSource.SaveChanges();
        //            context.AddToSellSheetSet(sellSheet);
        //            context.SaveChanges();

        //            return sellSheet.Sell_Sheet_ID;
        //        }
        //    }

        //    return 0;
        //}

       
    }
}