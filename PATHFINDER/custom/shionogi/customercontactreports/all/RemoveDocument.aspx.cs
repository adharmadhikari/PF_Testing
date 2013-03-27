using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PathfinderClientModel;
public partial class custom_pinso_customercontactreports_all_RemoveDocument : PageBase
{
    protected override void OnInit(EventArgs e)
    {
        dsPlanDocument.ConnectionString = Pinsonault.Web.Session.ClientConnectionString;
        titleText.Text = "Remove Selected Business Document";
        base.OnInit(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.formRemoveDocument.Visible = true;
        this.Msglbl.Visible = false;
    }
    protected void Yesbtn_Click(object sender, EventArgs e)
    {
        //Update Document_Status to false It doesn't delete the Document physically from the Server
        using ( PathfinderClientEntities context = new PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString) )
        {
            if (context.DeleteDocument(System.Convert.ToInt32(Page.Request.QueryString["DocumentID"])))
            {
                this.formRemoveDocument.Visible = false;
                this.Msglbl.Text = "<div>Selected Business Document has been removed successfully.</div>";
                this.Msglbl.Visible = true;

                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "RefreshBusinessDocs", "RefreshBusinessDocs();", true);
            }
            
        }

     }
}
