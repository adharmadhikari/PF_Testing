using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Services;
using System.Text;
using System.ServiceModel.Web;
using PathfinderModel;
using Pinsonault.Web.Services;

namespace Pinsonault.Application.PrescriberReporting
{
    public class PrescriberReportingDataService: PathfinderDataServiceBase<PathfinderEntities>
    {
         // This method is called only once to initialize service-wide policies.
        public static void InitializeService(IDataServiceConfiguration config)
        {
            Pinsonault.Web.Support.InitializeService(config);
        }
        
        /// <summary>
        /// Returns text that can be used in javascript to construct an array of report options available to the current user based on their client id.
        /// </summary>
        /// <returns></returns>
        [WebGet]
        public override string GetModuleOptions()
        {
            return CurrentDataSource.GetUserReportOptionsAsJSON(Pinsonault.Web.Session.UserID, Pinsonault.Web.Identifiers.PrescriberReporting);
        }
    }
}
