using System;
using System.Data.Services;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;
using PathfinderClientModel;
using System.Linq.Expressions;

using System.Web;
using Pinsonault.Web.Services;
using Pinsonault.Web.Data;

namespace Pinsonault.Application.TodaysAccounts
{

    //[System.ServiceModel.ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class TodaysAccountsClientDataService : PathfinderClientDataServiceBase<PathfinderClientEntities>
    {
        // This method is called only once to initialize service-wide policies.
        public static void InitializeService(IDataServiceConfiguration config)
        {
            Pinsonault.Web.Support.InitializeService(config);

            config.MaxResultsPerCollection = 1000;

            //Important: Only list Entity Sets that should be accessible through the web service.  If client side queries are not required to populate a grid or another control do not make it public     
            config.SetEntitySetAccessRule("KeyContactSearchSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("KeyContactBasicSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("PlanInfoListViewSet", EntitySetRights.AllRead);
            //config.SetEntitySetAccessRule("PlanInfoListViewMedDProductSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("PlanInfoListViewByTerritorySet", EntitySetRights.AllRead);
            //config.SetEntitySetAccessRule("PlanInfoListViewMedDProductByTerritorySet", EntitySetRights.AllRead);

            config.SetEntitySetAccessRule("ParentPlanSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("PlanSearchSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("PlanSearchCombinedSet", EntitySetRights.AllRead);
        }

        #region Query Interceptors

        [QueryInterceptor("PlanInfoListViewByTerritorySet")]
        public Expression<Func<PlanInfoListViewByTerritory, bool>> FilterPlansByTerritory()
        {
            string terrID = Pinsonault.Web.Session.TerritoryID;

            if (!string.IsNullOrEmpty(terrID))
                return p => p.Territory_ID == terrID;
            else //if user does not have a terr id then return false since they cannot have aligned accounts
                return p => false;
        }

        //[QueryInterceptor("PlanInfoListViewMedDProductByTerritorySet")]
        //public Expression<Func<PlanInfoListViewMedDProductByTerritory, bool>> FilterMedDProductsByTerritory()
        //{
        //    string terrID = Pinsonault.Web.Session.TerritoryID;

        //    if (!string.IsNullOrEmpty(terrID))
        //        return p => p.Territory_ID == terrID;
        //    else //if user does not have a terr id then return false since they cannot have aligned accounts
        //        return p => false;
        //}


        /// <summary>
        /// Filters Key Contacts by sections client has access to.
        /// </summary>
        /// <returns></returns>
        [QueryInterceptor("KeyContactSearchSet")]
        public Expression<Func<KeyContactSearch, bool>> FilterKeyContactSearchByClientSections()
        {
            Dictionary<string, PathfinderModel.ClientApplicationAccess> access = Pinsonault.Web.Session.ClientApplicationAccess;
            //Select all available Section IDs for Todays Accounts
            IEnumerable<int> q = access.Where(i => i.Value.ApplicationID == 1).Select(i => i.Value.SectionID);

            //Expression that creates a parameter for the current entity of type KeyContactSearch
            ParameterExpression entity = Expression.Parameter(typeof(KeyContactSearch), "trg");

            //Expression that returns the Section_ID property value for a single entity of type KeyContactSearch;
            Expression targetExp = Expression.Property(entity, "Section_ID");

            //Construct the necessary OR conditions for the list of regions
            Expression body = Pinsonault.Data.Generic.GetExpForList<int>(q.ToArray(), targetExp);

            return Expression.Lambda<Func<KeyContactSearch, bool>>(body, entity);
        }

        [QueryInterceptor("PlanInfoListViewSet")]
        public Expression<Func<PlanInfoListView, bool>> FilterPlansByUserRegion()
        {
            string[] regions = Pinsonault.Web.Session.GeographicRegions;
            if (regions != null && regions.Length > 0)
            {
                //Expression that creates a parameter for the current entity of type PlanInfoListView
                ParameterExpression entity = Expression.Parameter(typeof(PlanInfoListView), "trg");

                //Expression that returns the Plan_State property value for a single entity of type PlanInfoListView;
                Expression targetExp = Expression.Property(entity, "Plan_State");

                //Construct the necessary OR conditions for the list of regions
                Expression body = Pinsonault.Data.Generic.GetExpForList<string>(regions, targetExp);

                return Expression.Lambda<Func<PlanInfoListView, bool>>(body, entity);
            }

            //no regions so do not filter
            return p => true;
        }

        /// <summary>
        /// Filters Key Contacts by sections client has access to.
        /// </summary>
        /// <returns></returns>
        [QueryInterceptor("PlanInfoListViewSet")]
        public Expression<Func<PlanInfoListView, bool>> FilterPlansByClientSections()
        {
            return GenFilterPlansByClientSections<PlanInfoListView>();
        }

        [QueryInterceptor("PlanInfoListViewByTerritorySet")]
        public Expression<Func<PlanInfoListViewByTerritory, bool>> FilterPlansByClientSectionsForTerritory()
        {
            return GenFilterPlansByClientSections<PlanInfoListViewByTerritory>();
        }

        public Expression<Func<T, bool>> GenFilterPlansByClientSections<T>()
        {
            return LINQHelper.GenFilterPlansByClientSections<T>(Pinsonault.Web.Identifiers.TodaysAccounts);
        }

        //[QueryInterceptor("PlanInfoListViewMedDProductSet")]
        //public Expression<Func<PlanInfoListViewMedDProduct, bool>> FilterMedDPlansByUserRegion()
        //{
        //    string[] regions = Pinsonault.Web.Session.GeographicRegions;
        //    if (regions != null && regions.Length > 0)
        //    {
        //        //Expression that creates a parameter for the current entity of type PlanInfoListView
        //        ParameterExpression entity = Expression.Parameter(typeof(PlanInfoListViewMedDProduct), "trg");

        //        //Expression that returns the Plan_State property value for a single entity of type PlanInfoListView;
        //        Expression targetExp = Expression.Property(entity, "Plan_State");

        //        //Construct the necessary OR conditions for the list of regions
        //        Expression body = Pinsonault.Data.Generic.GetExpForList<string>(regions, targetExp);

        //        return Expression.Lambda<Func<PlanInfoListViewMedDProduct, bool>>(body, entity);
        //    }

        //    //no regions so do not filter
        //    return p => true;
        //}

        #endregion

        [WebGet]
        public int GetContactCount(string where)
        {
            if (String.IsNullOrEmpty(where))
                return CurrentDataSource.KeyContactBasicSet.Count();
            else
                return CurrentDataSource.KeyContactBasicSet.Where(where).Count();

        }

        [WebGet]
        public int GetKeyContactSearchCount(string where)
        {
            if (String.IsNullOrEmpty(where))
                return CurrentDataSource.KeyContactSearchSet.Count();
            else
                return CurrentDataSource.KeyContactSearchSet.Where(where).Count();

        }

        [WebGet]
        public int GetPlanInfoListViewCount(string where)
        {
            if (String.IsNullOrEmpty(where))
                return CurrentDataSource.PlanInfoListViewSet.Where(FilterPlansByUserRegion()).Where(FilterPlansByClientSections()).Count();
            else
                return CurrentDataSource.PlanInfoListViewSet.Where(where).Where(FilterPlansByUserRegion()).Where(FilterPlansByClientSections()).Count();
        }

        //[WebGet]
        //public int GetPlanInfoListViewMedDCount(string where)
        //{
        //    if (String.IsNullOrEmpty(where))
        //        return CurrentDataSource.PlanInfoListViewMedDProductSet.Where(FilterMedDPlansByUserRegion()).Count();
        //    else
        //        return CurrentDataSource.PlanInfoListViewMedDProductSet.Where(where).Where(FilterMedDPlansByUserRegion()).Count();
        //}

        [WebGet]
        public int GetPlanInfoListViewByTerritoryCount(string where)
        {
            if (String.IsNullOrEmpty(where))
                return CurrentDataSource.PlanInfoListViewByTerritorySet.Where(this.FilterPlansByTerritory()).Count(FilterPlansByClientSectionsForTerritory());
            else
                return CurrentDataSource.PlanInfoListViewByTerritorySet.Where(where).Where(this.FilterPlansByTerritory()).Count(FilterPlansByClientSectionsForTerritory());
        }

        //[WebGet]
        //public int GetPlanInfoListViewMedDByTerritoryCount(string where)
        //{
        //    if (String.IsNullOrEmpty(where))
        //        return CurrentDataSource.PlanInfoListViewMedDProductByTerritorySet.Where(this.FilterMedDProductsByTerritory()).Count();
        //    else
        //        return CurrentDataSource.PlanInfoListViewMedDProductByTerritorySet.Where(where).Where(this.FilterMedDProductsByTerritory()).Count();
        //}

        public static bool DeleteContact(int KC_ID, PathfinderClientModel.PathfinderClientEntities context)
        {
            if (HttpContext.Current.User.IsInRole("editcontacts"))
            {
                PathfinderClientModel.KeyContact contact = context.KeyContactSet.FirstOrDefault(c => c.KC_ID == KC_ID);
                if (contact != null)
                {
                    contact.Status = false;
                    context.SaveChanges();
                    return true;
                }
            }
            return false;

        }

    }
}