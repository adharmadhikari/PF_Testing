using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Configuration;
//using PathfinderClientModel;
using Pinsonault.Application.Alcon;
using Pinsonault.Web;


public partial class custom_pinso_customercontactreports_all_DocumentUpload : PageBase
{ 

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["size"]))
        {
            hdnLbl3.Text = "Maximum size for upload is 6 MB.";
            hdnLbl3.Visible = true;
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "RefreshDDRs", "RefreshDDRs();", true);
        }
    }    
    
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if (FileUpload1.HasFile && CustomValidator1.IsValid)
        {
            //PlanDocumentsSet
            using (PathfinderAlconEntities context = new PathfinderAlconEntities())
            {

                string type = Path.GetExtension(FileUpload1.FileName);
                if (String.Compare(type, ".pdf", true) == 0 || String.Compare(type, ".zip", true) == 0)
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
                   
                    planDoc.Document_Type_ID =Convert.ToInt32(ddltype.SelectedValue);
                    context.AddToPlanDocumentsSet(planDoc);
                    context.SaveChanges();

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

    private void ProcessRequestPage(HttpContext context)
    {
        //base.ProcessRequest(context);     
        //1 MB = 1048576 bytes, so 6 MB = 6291456 bytes
        int maxRequestLength = 6291456;
        //PathfinderApplication.Support.CheckFileUploadSize(context, maxRequestLength);

        //This code is used to check the request length of the page and if the request length is greater than 
        //MaxRequestLength then retrun to the same page with extra query string value action=exception

        if (context.Request.ContentLength > maxRequestLength)
        {
            IServiceProvider provider = (IServiceProvider)context;
            HttpWorkerRequest workerRequest = (HttpWorkerRequest)provider.GetService(typeof(HttpWorkerRequest));

            // Check if body contains data
            if (workerRequest.HasEntityBody())
            {
                // get the total body length
                int requestLength = workerRequest.GetTotalEntityBodyLength();
                // Get the initial bytes loaded
                int initialBytes = 0;
                if (workerRequest.GetPreloadedEntityBody() != null)
                    initialBytes = workerRequest.GetPreloadedEntityBody().Length;
                if (!workerRequest.IsEntireEntityBodyIsPreloaded())
                {
                    byte[] buffer = new byte[512000];
                    // Set the received bytes to initial bytes before start reading
                    int receivedBytes = initialBytes;
                    while (requestLength - receivedBytes >= initialBytes)
                    {
                        // Read another set of bytes
                        initialBytes = workerRequest.ReadEntityBody(buffer, buffer.Length);

                        // Update the received bytes
                        receivedBytes += initialBytes;
                    }
                    initialBytes = workerRequest.ReadEntityBody(buffer, requestLength - receivedBytes);
                }
            }
            // Redirect the user to the same page with querystring size=exception. 
            context.Response.Redirect(this.Request.Url.LocalPath + "?size=exception");
        }

    }
    protected override void OnError(EventArgs e)
    {        
        ProcessRequestPage(this.Context);
    }
}

