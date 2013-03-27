using System;
using System.Data.Services;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;
using PathfinderModel;
using System.Linq.Expressions;

using System.Web;
using Pinsonault.Web.Services;
using Pinsonault.Web.Data;
using System.Collections.Specialized;

namespace Pinsonault.Application.TodaysAccounts
{
    public class TodaysAccountsDataService : PathfinderDataServiceBase<PathfinderEntities>
    {
        // This method is called only once to initialize service-wide policies.
        public static void InitializeService(IDataServiceConfiguration config)
        {
            Pinsonault.Web.Support.InitializeService(config);

            //Important: Only list Entity Sets that should be accessible through the web service.  If client side queries are not required to populate a grid or another control do not make it public     
            config.SetEntitySetAccessRule("CoveredLivesDrilldownSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("KeyContactBasicSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("PlanAffiliationKeyContactSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("PlanAffiliationListViewSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("PlanAffiliationListView_MedDProductsSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("PlanAffiliationListView_DodSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("PlanMasterSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("PlanInfoListViewSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("PlanInfoListViewByTerritorySet", EntitySetRights.AllRead);
            //config.SetEntitySetAccessRule("PlanInfoListViewAllSet", EntitySetRights.AllRead);
            //config.SetEntitySetAccessRule("PlanInfoListViewAllByTerritorySet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("PlanFormularyPDLLinksSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("UserModuleSet", EntitySetRights.AllRead);


            //
        }

        #region Interceptors

        [QueryInterceptor("CoveredLivesDrilldownSet")]
        public Expression<Func<CoveredLivesDrilldown, bool>> FilterCoveredLivesDrillDownByRole()
        {
            bool cp = HttpContext.Current.User.IsInRole("frmly_1");
            bool cb = HttpContext.Current.User.IsInRole("frmly_99");
            bool medd = HttpContext.Current.User.IsInRole("frmly_17");
            bool sm = HttpContext.Current.User.IsInRole("frmly_9");
            bool dod = HttpContext.Current.User.IsInRole("frmly_12");
            bool pbm = HttpContext.Current.User.IsInRole("frmly_4");
            bool mm = HttpContext.Current.User.IsInRole("frmly_6");
            bool va = HttpContext.Current.User.IsInRole("frmly_11");


            //*Section_ID 20 (FEP) is being treated as Section_ID 1 (CP) and using that security role
            return f => (((f.Section_ID == 1 || f.Section_ID == 20) && f.Segment_ID == 1 && cp)
                || (f.Segment_ID == 2 && medd)
                || (f.Segment_ID == 8 && medd)
                || (f.Section_ID == 4 && pbm)
                || (f.Section_ID == 6 && mm)
                || (f.Section_ID == 9 && sm)
                || (f.Section_ID == 12 && dod)
                || (f.Section_ID == 11 && va)
                || (f.Section_ID == 99 && cb)
                );
        }

        [QueryInterceptor("PlanInfoListViewSet")]
        public Expression<Func<PlanInfoListView, bool>> FilterPlansByUserRegion()
        {
            //NameValueCollection nvc = HttpContext.Current.Request.QueryString;
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
        /// Filters plans by client sections
        /// </summary>
        /// <returns></returns>
        [QueryInterceptor("PlanInfoListViewSet")]
        public Expression<Func<PlanInfoListView, bool>> FilterPlansByClientSections()
        {
            return GenFilterPlansByClientSections<PlanInfoListView>();
        }

        /// <summary>
        /// If Commercial Section, filter only pure commercial business
        /// </summary>
        /// <returns></returns>
        [QueryInterceptor("PlanInfoListViewSet")]
        public Expression<Func<PlanInfoListView, bool>> FilterPureCommercial()
        {
            int sectionID = Pinsonault.Data.Generic.ParseServiceRequestForIdentifier<int>(HttpContext.Current.Request.QueryString["$filter"], "Section_ID");

            //Filter where plans have commercial business to get pure commercial only is section is one
            if (sectionID == 1)
                return p => p.Has_Commercial_Business == true;
            else
                return p => true;
        }

        //[QueryInterceptor("PlanInfoListViewAllSet")]
        //public Expression<Func<PlanInfoListViewAll, bool>> FilterPlansByUserRegionAll()
        //{
        //    //NameValueCollection nvc = HttpContext.Current.Request.QueryString;
        //    string[] regions = Pinsonault.Web.Session.GeographicRegions;
        //    if (regions != null && regions.Length > 0)
        //    {
        //        //Expression that creates a parameter for the current entity of type PlanInfoListView
        //        ParameterExpression entity = Expression.Parameter(typeof(PlanInfoListView), "trg");

        //        //Expression that returns the Plan_State property value for a single entity of type PlanInfoListView;
        //        Expression targetExp = Expression.Property(entity, "Plan_State");

        //        //Construct the necessary OR conditions for the list of regions
        //        Expression body = Pinsonault.Data.Generic.GetExpForList<string>(regions, targetExp);

        //        return Expression.Lambda<Func<PlanInfoListViewAll, bool>>(body, entity);
        //    }

        //    //no regions so do not filter
        //    return p => true;
        //}

        /// <summary>
        /// Filters plans by client sections
        /// </summary>
        /// <returns></returns>
        //[QueryInterceptor("PlanInfoListViewAllSet")]
        //public Expression<Func<PlanInfoListViewAll, bool>> FilterPlansByClientSectionsAll()
        //{
        //    return GenFilterPlansByClientSections<PlanInfoListViewAll>();
        //}

        [QueryInterceptor("PlanInfoListViewByTerritorySet")]
        public Expression<Func<PlanInfoListViewByTerritory, bool>> FilterMyPlansByClientID()
        {
            int clientID = Pinsonault.Web.Session.ClientID;
            return p => p.Client_ID == clientID;
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

        /// <summary>
        /// If Commercial Section, filter only pure commercial business
        /// </summary>
        /// <returns></returns>
        [QueryInterceptor("PlanInfoListViewByTerritorySet")]
        public Expression<Func<PlanInfoListViewByTerritory, bool>> FilterPureCommercialForTerritory()
        {
            int sectionID = Pinsonault.Data.Generic.ParseServiceRequestForIdentifier<int>(HttpContext.Current.Request.QueryString["$filter"], "Section_ID");

            //Filter where plans have commercial business to get pure commercial only is section is one
            if (sectionID == 1)
                return p => p.Has_Commercial_Business == true;
            else
                return p => true;
        }

        //[QueryInterceptor("PlanInfoListViewAllByTerritorySet")]
        //public Expression<Func<PlanInfoListViewAllByTerritory, bool>> FilterMyPlansByClientIDAll()
        //{
        //    int clientID = Pinsonault.Web.Session.ClientID;
        //    return p => p.Client_ID == clientID;
        //}

        /// <summary>
        /// for filtering the accounts by territoryid
        /// </summary>
        /// <returns></returns>
        //[QueryInterceptor("PlanInfoListViewAllByTerritorySet")]
        //public Expression<Func<PlanInfoListViewAllByTerritory, bool>> FilterPlansByTerritoryAll()
        //{
        //    string terrID = Pinsonault.Web.Session.TerritoryID;

        //    if (!string.IsNullOrEmpty(terrID))
        //        return p => p.Territory_ID == terrID;
        //    else //if user does not have a terr id then return false since they cannot have aligned accounts
        //        return p => false;
        //}

        //[QueryInterceptor("PlanInfoListViewAllByTerritorySet")]
        //public Expression<Func<PlanInfoListViewAllByTerritory, bool>> FilterPlansByClientSectionsForTerritoryAll()
        //{
        //    return GenFilterPlansByClientSections<PlanInfoListViewAllByTerritory>();
        //}

        [QueryInterceptor("UserModuleSet")]
        public Expression<Func<UserModule, bool>> FilterUserModuleByUser()
        {
            int userID = Pinsonault.Web.Session.UserID;
            return um => um.User_ID == userID;
        }

        #endregion

        [WebGet]
        public int GetPlanInfoListViewByTerritoryCount(string where)
        {
            if (String.IsNullOrEmpty(where))
                return CurrentDataSource.PlanInfoListViewByTerritorySet.Where(this.FilterPlansByTerritory()).Count(FilterPlansByClientSectionsForTerritory());
            else
                return CurrentDataSource.PlanInfoListViewByTerritorySet.Where(where).Where(this.FilterPlansByTerritory()).Count(FilterPlansByClientSectionsForTerritory());
        }

        public Expression<Func<T, bool>> GenFilterPlansByClientSections<T>()
        {
            return LINQHelper.GenFilterPlansByClientSections<T>(Pinsonault.Web.Identifiers.TodaysAccounts);
        }

        [WebGet]
        public int GetContactCount(string where)
        {
            if (String.IsNullOrEmpty(where))
                return CurrentDataSource.KeyContactBasicSet.Count();
            else
                return CurrentDataSource.KeyContactBasicSet.Where(where).Count();
        }

        [WebGet]
        public int GetVAContactCount(string where)
        {
            if (String.IsNullOrEmpty(where))
                return CurrentDataSource.PlanAffiliationKeyContactSet.Count();
            else
                return CurrentDataSource.PlanAffiliationKeyContactSet.Where(where).Count();
        }

        [WebGet]
        public int GetAffiliationCount(string where)
        {
            if (string.IsNullOrEmpty(where))
                return CurrentDataSource.PlanAffiliationListViewSet.Count();
            else
                return CurrentDataSource.PlanAffiliationListViewSet.Where(where).Count();
        }
        [WebGet]
        public int GetMedDAffiliationCount(string where)
        {
            if (string.IsNullOrEmpty(where))
                return CurrentDataSource.PlanAffiliationListView_MedDProductsSet.Count();
            else
                return CurrentDataSource.PlanAffiliationListView_MedDProductsSet.Where(where).Count();
        }
        [WebGet]
        public int GetDodAffiliationCount(string where)
        {
            if (string.IsNullOrEmpty(where))
                return CurrentDataSource.PlanAffiliationListView_DodSet.Count();
            else
                return CurrentDataSource.PlanAffiliationListView_DodSet.Where(where).Count();
        }

        [WebGet]
        public int GetCoveredLivesDrilldownCount(string where)
        {
            if (string.IsNullOrEmpty(where))
                return CurrentDataSource.CoveredLivesDrilldownSet.Count();
            else
                return CurrentDataSource.CoveredLivesDrilldownSet.Where(where).Count();
        }

        [WebGet]
        public int GetBenefitDesgCount(string where)
        {
            if (String.IsNullOrEmpty(where))
                return CurrentDataSource.BenefitDesignSet.Count();
            else
                return CurrentDataSource.BenefitDesignSet.Where(where).Count();
        }


        [WebGet]
        public string GetFormularyPDLLink(string param)
        {
            int planID = 0;
            int segmentID = 0;
            int formularyID = 0;
            int productID = 0;

            if (!string.IsNullOrEmpty(param))
            {
                string[] vals = param.Split(',');
                planID = Convert.ToInt32(vals[0]);
                segmentID = Convert.ToInt32(vals[1]);
                formularyID = Convert.ToInt32(vals[2]);
                productID = Convert.ToInt32(vals[3]);
            }

            string pdlLink;
            using (PathfinderModel.PathfinderEntities ctx = new PathfinderModel.PathfinderEntities())
            {
                if (productID == 0) // other than Medicare Part-D
                {
                    pdlLink = (from f in ctx.PlanFormularyPDLLinksSet
                               where (f.Plan_ID == planID
                               && f.Segment_ID == segmentID
                               && f.Formulary_ID == formularyID)
                               select f.PDL_Link).FirstOrDefault();
                }
                else //Medicare Part-D
                {
                    pdlLink = (from f in ctx.PlanFormularyPDLLinksSet
                               where (f.Plan_ID == planID
                               && f.Segment_ID == segmentID
                               && f.Formulary_ID == formularyID
                               && f.Plan_Product_ID == productID)
                               select f.PDL_Link).FirstOrDefault();
                }

                if (!string.IsNullOrEmpty(pdlLink))
                    return pdlLink;
                else
                    return "NotExist";
            }
        }

        /// <summary>
        /// Returns the current or previous month or quarter for Formulary History based on segment and 'C' or 'P' flag
        /// </summary>
        /// <param name="currentPrevious"></param>
        /// <param name="segmentID"></param>
        /// <returns></returns>
        public static string GetFormularyHistoryTimeframeName(string currentPrevious, int segmentID)
        {
            using (PathfinderModel.PathfinderEntities context = new PathfinderModel.PathfinderEntities())
            {
                string timeframeName = (from d in context.LkpFormularyDataPeriodSet
                                        where d.Current_Previous == currentPrevious &&
                                        d.Segment_ID == segmentID
                                        select d.Data_Name).FirstOrDefault();

                return timeframeName;
            }
        }

        /// <summary>
        /// Returns the current or previous month or quarter for Formulary History based on segment and 'C' or 'P' flag
        /// </summary>
        /// <param name="currentPrevious"></param>
        /// <param name="segmentID"></param>
        /// <returns></returns>
        public static int? GetPinsoFormularyID(int planID, int productID, int formularyID)
        {
            using (PathfinderModel.PathfinderEntities context = new PathfinderModel.PathfinderEntities())
            {
                int? pinsoFormularyID = (from d in context.fhrLkpPartDPinsoFormularyIDSet
                                         where d.Plan_ID == planID &&
                                         d.Product_ID == productID &&
                                         d.Formulary_ID == formularyID
                                         select d.Pinso_Formulary_ID).FirstOrDefault();

                return pinsoFormularyID;
            }
        }
    }
}