<%@ WebHandler Language="C#" Class="PlanDocument" %>

using System;
using System.Web;
using PathfinderClientModel;
using System.Linq;
using System.IO;
using System.Configuration;
using Pinsonault.Application.Merz;

public class PlanDocument : Pinsonault.Web.GenericHttpHandler
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
                
                using (PathfinderMerzEntities dataContext = new PathfinderMerzEntities())
                {
                    var query = from d in dataContext.BusinessPlanMedicalPolicyDocSet
                                where d.Medical_Policy_ID == docID
                                select d;

                    BusinessPlanMedicalPolicyDoc document = query.FirstOrDefault();
                    
                    if (document != null)
                    {
                        context.Response.Clear();

                        string ext = Path.GetExtension(document.File_Path);

                        context.Response.ContentType = System.Net.Mime.MediaTypeNames.Application.Octet;
                        context.Response.AppendHeader("Content-Disposition", string.Format("attachment; filename=\"{0}\"", document.Medical_Policy_Name));

                        context.Response.TransmitFile(Path.Combine(Pinsonault.Web.Support.GetClientFolder("bp_medical_policy"), Path.ChangeExtension(document.Medical_Policy_ID.ToString(), ext)));
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