using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Configuration;
using PathfinderClientModel;


public partial class custom_jazz_customercontactreports_all_DocumentUpload : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if (FileUpload1.HasFile && CustomValidator1.IsValid)
        { 
            //PlanDocumentsSet
            using ( PathfinderClientEntities context = new PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString) )
            {
                string type = Path.GetExtension(FileUpload1.FileName);
                if ( String.Compare(type, ".pdf", true) == 0 ||  String.Compare(type, ".zip", true) == 0)
                {
                    PlanDocuments planDoc = new PlanDocuments();
                    planDoc.Plan_ID = System.Convert.ToInt32(System.Web.HttpContext.Current.Request.QueryString["Plan_ID"]);
                    planDoc.Document_Name = bpsName.Text;
                    planDoc.Document_Original_File = FileUpload1.FileName;
                    planDoc.Document_Date = DateTime.UtcNow;
                    planDoc.Document_Status = true;
                    planDoc.Is_Business_Plan = true;
                    planDoc.User_ID = Pinsonault.Web.Session.UserID;
                    planDoc.Modified_BY = Pinsonault.Web.Session.FullName;
                    planDoc.Created_BY = Pinsonault.Web.Session.FullName;
                    planDoc.Created_DT = DateTime.UtcNow;
                    planDoc.Modified_DT = DateTime.UtcNow;
                    context.AddToPlanDocumentsSet(planDoc);
                    context.SaveChanges();
                    
                    //string type = Path.GetExtension(FileUpload1.FileName);
                    //creating a unique filename with the Document_ID got from above and appending its 'type' of file extension
                    string filename = Path.ChangeExtension(planDoc.Document_ID.ToString(), type);
                    string folderPath = Path.Combine(Pinsonault.Web.Support.GetClientFolder("CCRDocuments"), filename);
                    //saving the file to local disk
                    FileUpload1.SaveAs(folderPath);

                    //making the text field empty after saving to the server
                    bpsName.Text = "";
                    hdnLbl.Visible = true;
                    hdnLbl.Text = "Document uploaded to server";
                     Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "RefreshDDRs", "RefreshDDRs();", true);
                }
                else
                {
                    bpsName.Text = "";
                    hdnLbl3.Visible = true;
                    hdnLbl3.Text = "Only .zip and .pdf files can be uploaded.";

                }
            }
        }
        else
        {
            bpsName.Text = "";
            hdnLbl3.Visible = true;
            hdnLbl3.Text = "The maximum file size allowed for upload is 6 MB.";
        }
    }

    protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (FileUpload1.FileBytes.Length > 6291456)
        {
            args.IsValid = false;
        }
        else
        {
            args.IsValid = true;
        }
    }

}
