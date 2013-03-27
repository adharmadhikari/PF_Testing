using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using PathfinderModel;
using Pinsonault.Application.Alcon;
using Pinsonault.Web;

public partial class custom_Alcon_customercontactreports_Controls_documenttype : System.Web.UI.UserControl
{
    protected override void OnPreRender(EventArgs e)
    {
        //creates the drop down list for Document type
        using (PathfinderAlconEntities context = new PathfinderAlconEntities())
        {
            //var n = from p in context.DocumentTypeSet
            //        select p.Document_Type_Name;
            
            var q = (from p in context.DocumentTypeSet
                     orderby p.Document_Type_Name
                     select p).ToList().Select(p => new GenericListItem { ID = p.Document_Type_ID.ToString(), Name = p.Document_Type_Name });
            if (q != null)
            {
                //List<GenericListItem> list = q.ToList();
                //list.Insert(0, new GenericListItem { ID = "0", Name = "All" });
                Pinsonault.Web.Support.RegisterGenericListVariable(this.Page, q.ToArray(), "DocumentType");
            }
        }
        base.OnPreRender(e);
    }


    protected override void OnLoad(EventArgs e)
    {
        Pinsonault.Web.Support.RegisterTierScriptVariable(this.Page);
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Document_Type_ID.ClientID, null, "moduleOptionsContainer");
       
        //Document_Type_ID.OnClientLoad = "CreateDocTypeList";
        base.OnLoad(e);
    }
}
