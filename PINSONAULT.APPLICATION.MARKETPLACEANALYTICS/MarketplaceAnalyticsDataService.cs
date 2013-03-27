using System;
using System.Data.Services;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;
using PathfinderModel;
using Pinsonault.Web.Services;

namespace Pinsonault.Application.MarketplaceAnalytics
{
    public class MarketplaceAnalyticsDataService : PathfinderDataServiceBase<PathfinderEntities>
    {
        // This method is called only once to initialize service-wide policies.
        public static void InitializeService(IDataServiceConfiguration config)
        {
            Pinsonault.Web.Support.InitializeService(config);

            config.MaxResultsPerCollection = 1000;

            config.SetEntitySetAccessRule("PlanSearchAllSet", EntitySetRights.AllRead);

        }

        /// <summary>
        /// Returns text that can be used in javascript to construct an array of report options available to the current user based on their client id.
        /// </summary>
        /// <returns></returns>
        [WebGet]
        public override string GetModuleOptions()
        {
            return CurrentDataSource.GetUserReportOptionsAsJSON(Pinsonault.Web.Session.UserID, Pinsonault.Web.Identifiers.MarketplaceAnalytics);
        }

        /// <summary>
        /// Returns text that can be used in javascript to construct an array of report options available to the current user based on their client id.
        /// </summary>
        /// <returns></returns>
        [WebGet]
        public string GetPrescriberModuleOptions()
        {
            return CurrentDataSource.GetUserReportOptionsAsJSON(Pinsonault.Web.Session.UserID, Pinsonault.Web.Identifiers.PrescriberReporting);
        }
    }
}