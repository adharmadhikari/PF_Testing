using System.Data.Services;
using System.ServiceModel.Web;
using System.Text;
using PathfinderClientModel;
using System.Linq;
using System.Web;
using System;
using System.Data.Objects;
using System.Linq.Expressions;
using System.Collections.Specialized;
using System.Collections.Generic;
using Pinsonault.Web.Services;

namespace Pinsonault.Application.Sandoz
{
    public class SandozDataService : PathfinderDataServiceBase<PathfinderSandozEntities>
    {
        // This method is called only once to initialize service-wide policies.
        public static void InitializeService(IDataServiceConfiguration config)
        {
            Pinsonault.Web.Support.InitializeService(config);
            config.MaxResultsPerCollection = 1000;
            //Important: Only list Entity Sets that should be accessible through the web service.  If client side queries are not required to populate a grid or another control do not make it public     
        }


        #region Query Interceptors
        #endregion

      
        //[WebGet]
        //public int CreateSellSheet(string Created)
        //{
        //    using ( PathfinderClientEntities context = new PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString) )
        //    {
        //        DateTime created;
        //        if ( DateTime.TryParse(Created, out created) )
        //        {
        //            SellSheet sellSheet = new SellSheet();
        //            sellSheet.Sell_Sheet_Name = string.Format("Draft - {0:d}", created);
        //            sellSheet.Status_ID = 1;
        //            sellSheet.Current_Step = "classandtemplateselection";
        //            sellSheet.Territory_ID = Pinsonault.Web.Session.TerritoryID;
        //            sellSheet.Include_Territory_Name = true;
        //            //Set type = "Tier Status" by default
        //            sellSheet.Type_ID = 1;
        //            sellSheet.Created_BY = Pinsonault.Web.Session.FullName;
        //            sellSheet.Created_DT = DateTime.UtcNow;
        //            sellSheet.Modified_DT = sellSheet.Created_DT;
        //            sellSheet.Modified_BY = sellSheet.Created_BY;
        //            context.AddToSellSheetSet(sellSheet);
        //            context.SaveChanges();

        //            return sellSheet.Sell_Sheet_ID;
        //        }
        //    }

        //    return 0;
        //}

       
    }
}