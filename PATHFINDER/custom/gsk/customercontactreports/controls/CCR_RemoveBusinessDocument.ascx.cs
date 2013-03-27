using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PathfinderClientModel;
using System.IO;

public partial class custom_controls_CCR_RemoveBusinessDocument : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //this.formRemoveDocument.Visible = true;
        this.Msglbl.Visible = false;
    }
    protected void Yesbtn_Click(object sender, EventArgs e)
    {
        //Update Document_Status to false It doesn't delete the Document physically from the Server
        using (PathfinderClientEntities context = new PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
        {
            if (context.DeleteDocument(System.Convert.ToInt32(Page.Request.QueryString["DocumentID"])))
            {
                //this.formRemoveDocument.Visible = false;
                this.Msglbl.Text = "<div>Selected Business Document has been removed successfully.</div>";
                this.Msglbl.Visible = true;

                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "RefreshBusinessDocs", "RefreshBusinessDocs();", true);
            }

        }
        string filename = Page.Request.QueryString["DocumentID"].ToString();
        string folderPath = Pinsonault.Web.Support.GetClientFolder("CCRDocuments");

        //TODO: need to create deleted folder in client folder and move this code in application support
        string strDeleteFolder = Path.Combine(folderPath, "Deleted");

        var file = (from f in Directory.GetFiles(folderPath) where Path.GetFileNameWithoutExtension(f).Equals(filename) select f).FirstOrDefault();

        if (File.Exists(file))
            File.Move(file, file.Replace(folderPath, strDeleteFolder));
    }
}
