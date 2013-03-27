using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pinsonault.Web.Services;
using System.Data.Services;
using System.Linq.Expressions;

namespace Pinsonault.Application.Dey
{
    public class DeyDataService : PathfinderDataServiceBase<PathfinderDeyEntities>
    {
        // This method is called only once to initialize service-wide policies.
        public static void InitializeService(IDataServiceConfiguration config)
        {
            Pinsonault.Web.Support.InitializeService(config);
            config.MaxResultsPerCollection = 1000;
            config.SetEntitySetAccessRule("RestrictionsReportDrilldownSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("SellSheetRepsSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("MSTerritorySet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("SellSheetTerritorySet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("SellSheetTerritoryRepsSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("SellSheetOrdersSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("SellSheetOrdersListSet", EntitySetRights.AllRead);
        }
        [QueryInterceptor("RestrictionsReportDrilldownSet")]
        public Expression<Func<RestrictionsReportDrilldown, bool>> FilterRestrictionsReportDrilldownByOnlyPBMSelection()
        {
            //if only PBM is selected, then show all plans which have PBM name associated with it
            string onlyPBM = System.Web.HttpContext.Current.Request.QueryString["onlyPBM"];

            if (!string.IsNullOrEmpty(onlyPBM))
                return e => e.PBM_Id > 0;
            else
                return e => true;
        }


        [QueryInterceptor("RestrictionsReportDrilldownSet")]
        public Expression<Func<RestrictionsReportDrilldown, bool>> FilterRestrictionsReportDrilldownByExcludedSegment()
        {
            string excludeSegment = System.Web.HttpContext.Current.Request.QueryString["excludeSegment"];

            if (!string.IsNullOrEmpty(excludeSegment))
            {
                int Segment_id = Convert.ToInt32(excludeSegment);
                return e => e.Segment_ID != Segment_id;
            }
            else
                return e => true;
        }
    }
}
