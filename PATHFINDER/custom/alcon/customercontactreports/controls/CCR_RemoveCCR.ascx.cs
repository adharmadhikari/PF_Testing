using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PathfinderClientModel;
using Pinsonault.Application.Alcon;
using System.IO;
using Pinsonault.Data;
using System.Linq.Expressions;

public partial class custom_Alcon_customercontactreports_controls_CCR_RemoveCCR : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //this.formRemoveDocument.Visible = true;
        this.Msglbl.Visible = false;
    }
    protected void Yesbtn_Click(object sender, EventArgs e)
    {
        //Update Document_Status to false It doesn't delete the Document physically from the Server
        using (PathfinderAlconEntities context = new PathfinderAlconEntities())
        {

            if (DeleteCCR(System.Convert.ToInt32(Page.Request.QueryString["CCRID"])))
            {
                //this.formRemoveDocument.Visible = false;
                this.Msglbl.Text = "<div>Selected CCR has been removed successfully.</div>";
                this.Msglbl.Visible = true;

                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "RefreshCCR", "RefreshCCR();", true);
            }

        }
    }
        //string filename = Page.Request.QueryString["CCRID"].ToString();
        //string folderPath = Pinsonault.Web.Support.GetClientFolder("CCRDocuments");

        ////TODO: need to create deleted folder in client folder and move this code in application support
        //string strDeleteFolder = Path.Combine(folderPath, "Deleted");

        //var file = (from f in Directory.GetFiles(folderPath) where Path.GetFileNameWithoutExtension(f).Equals(filename) select f).FirstOrDefault();

        //if (File.Exists(file))
        //    File.Move(file, file.Replace(folderPath, strDeleteFolder));


    public bool DeleteCCR(int CCRID)
    {
       
        //        Pinsonault.Application.Alcon.Contact_Reports contactreport = Contact_Reports.FirstOrDefault(c => c.CCRID == CCRID);
        if (CCRID != null)
        {
            using (PathfinderAlconEntities context = new PathfinderAlconEntities())
            {
                 Contact_Reports ccrView;
                 ccrView = context.ContactReports.FirstOrDefault(c => c.Contact_Report_ID == CCRID);
                 ccrView.status = false;
                 ccrView.Modified_DT = DateTime.UtcNow;
                 ccrView.Modified_BY = Pinsonault.Web.Session.FullName;
                 context.SaveChanges();
           
            }
            
            return true;
        }

        return false;

        //    }
    }
}
