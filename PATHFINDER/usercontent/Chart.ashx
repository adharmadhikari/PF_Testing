<%@ WebHandler Language="C#" Class="Chart" %>

using System;
using System.Web;

//public class Chart : Pinsonault.Web.GenericHttpHandler
//{
//    public override bool IsReusable
//    {
//        get { return false; }
//    }

//    protected override void InternalProcessRequest(HttpContext context)
//    {
//        string id = context.Request["id"];
//        if ( !string.IsNullOrEmpty(id) )
//        {
//            string path = System.IO.Path.Combine(Pinsonault.Web.Support.GetClientFolder("charts"), id);
//            path = System.IO.Path.ChangeExtension(path, context.Request["type"]);
//            if ( System.IO.File.Exists(path) )
//            {
//                context.Response.Cache.SetExpires(DateTime.UtcNow.AddHours(12D));
                
//                context.Response.TransmitFile(path);
//            }
//            else
//                throw new HttpException(404, string.Format("Requested chart was not found with id of {0}.", id));
//        }
//        else
//            throw new HttpException(500, "Failed to return Chart; id is required.");
//    }
//}

    
public class Chart : Pinsonault.Web.ChartHandler
{    
}