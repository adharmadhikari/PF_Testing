using System;
using System.Data.Services;
using System.Linq;
using System.Linq.Expressions;
using System.ServiceModel.Web;
using PathfinderModel;
using Pinsonault.Web.Data;
using System.Text;
using Pinsonault.Web.Services;

public class PathfinderService : PathfinderDataServiceBase<PathfinderModel.PathfinderEntities>
{
    // This method is called only once to initialize service-wide policies.
    public static void InitializeService(IDataServiceConfiguration config)
    {
        Pinsonault.Web.Support.InitializeService(config);        

        ////Important: Only list Entity Sets that should be accessible through the web service.  If client side queries are not required to populate a grid or another control do not make it public     
        config.SetEntitySetAccessRule("FavoriteSet", EntitySetRights.AllRead);
        config.SetEntitySetAccessRule("fhrPlanSectionListSet", EntitySetRights.AllRead);
        config.SetEntitySetAccessRule("fhrGetFormularyDataPeriodSet", EntitySetRights.AllRead);
        config.SetEntitySetAccessRule("fhrGetSectionDisplayOptionsSet", EntitySetRights.AllRead);
        config.SetEntitySetAccessRule("PlanInfoListViewSet", EntitySetRights.AllRead);
        config.SetEntitySetAccessRule("Client_App_Drug_ListSet", EntitySetRights.AllRead);
        config.SetEntitySetAccessRule("AccountTypesSet", EntitySetRights.AllRead);
        config.SetEntitySetAccessRule("ParentPlanSet", EntitySetRights.AllRead);
        
        config.MaxResultsPerCollection = 1000;     
        ////
        //config.SetEntitySetAccessRule("ParentPlanListByAffilTypeSet", EntitySetRights.AllRead);

    }
#region
    [QueryInterceptor("FavoriteSet")]
    public Expression<Func<Favorite, bool>> FavoritesFilterByUser()
    {
        return f => f.User.User_ID == Pinsonault.Web.Session.UserID;
    }

    [QueryInterceptor("fhrGetFormularyDataPeriodSet")]
    public Expression<Func<fhrGetFormularyDataPeriod, bool>> FilterByClientID()
    {
        return e => e.Client_ID == Pinsonault.Web.Session.ClientID;
    }    
#endregion
    
    [WebInvoke]
    public void AddFavorite()
    {
        string name = System.Web.HttpContext.Current.Request.Form["Name"];
        string data = System.Web.HttpContext.Current.Request.Form["Data"];

        if ( string.IsNullOrEmpty(name) )
            name = string.Format("Favorite {0:g}", DateTime.UtcNow);
        if ( !string.IsNullOrEmpty(data) )
        {
            CurrentDataSource.AddToFavoriteSet(new Favorite { Name = name, User = CurrentDataSource.UserSet.FirstOrDefault(u => u.User_ID == Pinsonault.Web.Session.UserID), Data = data });
            CurrentDataSource.SaveChanges();
        }
    }
}
