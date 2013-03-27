using System.Data.Services;
using PathfinderClientModel;
using Pinsonault.Web.Services;

public class AccessBasedSellilngDataService : PathfinderDataServiceBase<PathfinderClientEntities>
{
    // This method is called only once to initialize service-wide policies.
    public static void InitializeService(IDataServiceConfiguration config)
    {
        Pinsonault.Web.Support.InitializeService(config);

        //Important: Only list Entity Sets that should be accessible through the web service.  If client side queries are not required to populate a grid or another control do not make it public     
        config.SetEntitySetAccessRule("ABSSummaryViewSet", EntitySetRights.AllRead);
        config.SetEntitySetAccessRule("ABSDetailViewSet", EntitySetRights.AllRead);
        config.SetEntitySetAccessRule("ABSComparativeAnalysysSet", EntitySetRights.AllRead);
        //
    }
}
