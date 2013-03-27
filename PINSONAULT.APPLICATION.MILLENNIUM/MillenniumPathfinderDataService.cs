using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Services;
using System.ServiceModel.Web;
using PathfinderModel;
using Pinsonault.Web.Data;
using Pinsonault.Web.Services;

namespace Pinsonault.Application.Millennium
{
    public class  MillenniumPathfinderDataService:PathfinderDataServiceBase<PathfinderEntities>
    {
         public static void InitializeService(IDataServiceConfiguration config)
        {
            Pinsonault.Web.Support.InitializeService(config);
        }
         [WebGet]
         public string GetExecutiveReportsOptions()
         {
             using (PathfinderModel.PathfinderEntities context = new PathfinderModel.PathfinderEntities())
             {
                 return context.GetUserReportOptionsAsJSON(Pinsonault.Web.Session.UserID, Pinsonault.Web.Identifiers.ExecutiveReports);
             }
         }
    }    
}
