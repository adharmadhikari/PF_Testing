using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Services;
using Pinsonault.Web.Services;
using System.Reflection;
using PathfinderClientModel;
using Pinsonault.Web.Data;
using System.Linq.Expressions;

/// <summary>
/// Summary description for CustomSegmentsDataService
/// </summary>
public class CustomSegmentsDataService : PathfinderClientDataServiceBase<PathfinderClientEntities>
{
    // This method is called only once to initialize service-wide policies.
    public static void InitializeService(IDataServiceConfiguration config)
    {
        Pinsonault.Web.Support.InitializeService(config);

        config.SetEntitySetAccessRule("PlanInfoListViewSet", EntitySetRights.AllRead);
        config.SetEntitySetAccessRule("PlanInfoListViewByTerritorySet", EntitySetRights.AllRead);

        config.MaxResultsPerCollection = 5000;
    }
     #region Query Interceptors
       
        [QueryInterceptor("PlanInfoListViewSet")]
        public Expression<Func<PlanInfoListView, bool>> FilterPlansByClientSections()
        {
            return GenFilterPlansByClientSections<PlanInfoListView>();
        }      

        /// <summary>
        /// for filtering the accounts by territoryid
        /// </summary>
        /// <returns></returns>
        [QueryInterceptor("PlanInfoListViewByTerritorySet")]
        public Expression<Func<PlanInfoListViewByTerritory, bool>> FilterPlansByTerritory()
        {
            string terrID = Pinsonault.Web.Session.TerritoryID;

            if (!string.IsNullOrEmpty(terrID))
                return p => p.Territory_ID == terrID;
            else //if user does not have a terr id then return false since they cannot have aligned accounts
                return p => false;
        }

        [QueryInterceptor("PlanInfoListViewByTerritorySet")]
        public Expression<Func<PlanInfoListViewByTerritory, bool>> FilterPlansByClientSectionsForTerritory()
        {
            return GenFilterPlansByClientSections<PlanInfoListViewByTerritory>();
        }

        public Expression<Func<T, bool>> GenFilterPlansByClientSections<T>()
        {
            return LINQHelper.GenFilterPlansByClientSections<T>(Pinsonault.Web.Identifiers.CustomSegments);
        }
     #endregion
   

}
    


