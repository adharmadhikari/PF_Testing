<%@ WebHandler Language="C#" Class="PlanDocument" %>

using System;
using System.Web;
using PathfinderClientModel;
using System.Linq;
using System.IO;
using System.Configuration;
using Pinsonault.Web;

public class PlanDocument : GenericHttpHandler
{
    
    //public void ProcessRequest (HttpContext context) {
    //    context.Response.ContentType = "text/plain";
    //    context.Response.Write("Hello World");
    //}
    protected override void InternalProcessRequest(HttpContext context)
    {
        string id = context.Request.QueryString["id"];
        if (!string.IsNullOrEmpty(id))
        {
            int docID = 0;
            if (Int32.TryParse(id, out docID))
            {
                using (PathfinderClientModel.PathfinderClientEntities dataContext = new PathfinderClientModel.PathfinderClientEntities(Session.ClientConnectionString))
                {
                    var query = from d in dataContext.PlanDocumentsSet
                                where d.Document_ID == docID
                                select d;

                    PlanDocuments document = query.FirstOrDefault();
                    
                    if (document != null)
                    {
                        context.Response.Clear();

                        string ext = Path.GetExtension(document.Document_Original_File);

                        context.Response.ContentType = System.Net.Mime.MediaTypeNames.Application.Octet;
                        context.Response.AppendHeader("Content-Disposition", string.Format("attachment; filename=\"{0}\"", document.Document_Original_File));

                        context.Response.WriteFile(Path.Combine(Support.GetClientFolder("CCRDocuments"), Path.ChangeExtension(document.Document_ID.ToString(), ext)));
                    }
                }
            }
        }        
    }

    public override bool IsReusable
    {
        get {
            return false;
        }
    }

}