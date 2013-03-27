using Pinsonault.Web.Services;
using PathfinderClientModel;
using System.Data.Services;

namespace Pinsonault.Application.MarketplaceAnalytics
{
    public class MarketplaceAnalyticsClientDataService : PathfinderDataServiceBase<PathfinderClientEntities>
    {
        // This method is called only once to initialize service-wide policies.
        public static void InitializeService(IDataServiceConfiguration config)
        {
            Pinsonault.Web.Support.InitializeService(config);

            config.MaxResultsPerCollection = 1000;

            config.SetEntitySetAccessRule("ParentPlanSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("PlanSearchSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("PlanSearchCombinedSet", EntitySetRights.AllRead);
            
        }

    }
}