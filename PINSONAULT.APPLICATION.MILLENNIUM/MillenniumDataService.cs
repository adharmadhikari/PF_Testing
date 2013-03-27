using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Services;
using System.ServiceModel.Web;
using Pinsonault.Web.Data;
using Pinsonault.Web.Services;
using System.ServiceModel;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using Pinsonault.Web.UI;
using System.Reflection;

using Pinsonault.Data;


namespace Pinsonault.Application.Millennium
{
    public class MillenniumDataService : PathfinderDataServiceBase<PathfinderMillenniumEntities>
    {
        
        // This method is called only once to initialize service-wide policies.
        [OperationContract]
        [WebInvoke(Method = "*", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        public static void InitializeService(IDataServiceConfiguration config)
        {
            Pinsonault.Web.Support.InitializeService(config);

            config.SetEntitySetAccessRule("PlansGridSet", EntitySetRights.All);
            config.SetEntitySetAccessRule("ContactReportProductsDiscussedViewSet", EntitySetRights.All);
            config.SetEntitySetAccessRule("KDMViewSet", EntitySetRights.All);
            config.SetEntitySetAccessRule("KDMDetailSet", EntitySetRights.All);
            config.SetEntitySetAccessRule("KDMAddressSet", EntitySetRights.All);
            config.SetEntitySetAccessRule("PlanAffiliationListView_VARSet", EntitySetRights.All);
            
        }
        
        #region Query Interceptors
        [QueryInterceptor("PlansGridSet")]
        public Expression<Func<PlansGrid, bool>> FilterPlanByUser()
        {
            
            
                return e => true;
        }
        [QueryInterceptor("ContactReportProductsDiscussedViewSet")]
        public Expression<Func<ContactReportProductsDiscussedView, bool>> FilterCCRByUser()
        {
            int userID = Pinsonault.Web.Session.UserID;
            return e => e.User_ID == userID;

        }
        #endregion

        [WebGet]
        public int GetVARAffiliationCount(string where)
        {
            if (string.IsNullOrEmpty(where))
                return CurrentDataSource.PlanAffiliationListView_VARSet.Count();
            else
                return CurrentDataSource.PlanAffiliationListView_VARSet.Where(where).Count();
        }
    }
}
