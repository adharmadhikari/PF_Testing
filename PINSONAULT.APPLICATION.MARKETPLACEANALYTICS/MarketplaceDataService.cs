using System;
using System.Data.Services;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;
using PathfinderModel;
using Pinsonault.Web.Services;

namespace Pinsonault.Application.MarketplaceAnalytics
{
    public class MarketplaceDataService : PathfinderDataServiceBase<PathfinderMarketplaceAnalyticsEntities>
    {
        // This method is called only once to initialize service-wide policies.
        public static void InitializeService(IDataServiceConfiguration config)
        {
            Pinsonault.Web.Support.InitializeService(config);

            config.MaxResultsPerCollection = 1000;

            config.SetEntitySetAccessRule("FHQuarterYearsSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("FHMonthYearsSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("ProductFormularyHistoryDataSet", EntitySetRights.AllRead);
        }

    }
}