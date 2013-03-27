using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Configuration;
using PathfinderClientModel;
using Pinsonault.Application.Merz;


public partial class custom_merz_businessplanning_all_upload : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Pinsonault.Web.Support.RegisterComponentWithClientManager(this.Page, DocumentType.ClientID);
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        Int32 DocID = Convert.ToInt32 (DocumentType.SelectedValue);
        String DocName = DocumentType.Text;  

        if (FileUpload1.HasFile)
        {
            //PlanDocumentsSet
            using (PathfinderMerzEntities context = new PathfinderMerzEntities())
            {
                BusinessPlanMedicalPolicyDoc planDoc = new BusinessPlanMedicalPolicyDoc();
                int BP_ID = System.Convert.ToInt32(System.Web.HttpContext.Current.Request.QueryString["BP_ID"]);
                planDoc.BusinessPlan = context.BusinessPlansSet.FirstOrDefault(b => b.Business_Plan_ID == BP_ID);
                planDoc.Medical_Policy_Name = FileUpload1.FileName;
                planDoc.File_Path = FileUpload1.FileName;
                planDoc.Upload_BY = Pinsonault.Web.Session.FullName;
                planDoc.Upload_DT = DateTime.UtcNow;
                planDoc.Med_Policy_Status_ID = 1;

                //Store selected document type.
                planDoc.BusinessPlanDocumentTypes = context.BusinessPlanDocumentTypesSet.FirstOrDefault(d => d.Document_Type_Name == DocName) ;

                context.AddToBusinessPlanMedicalPolicyDocSet(planDoc);
                context.SaveChanges();                

                string type = Path.GetExtension(FileUpload1.FileName);                
                //creating a unique filename with the Document_ID got from above and appending its 'type' of file extension
                string filename = Path.ChangeExtension(planDoc.Medical_Policy_ID.ToString(), type);
                string folderPath = Path.Combine(Pinsonault.Web.Support.GetClientFolder("bp_medical_policy"), filename);
                //saving the file to local disk
                FileUpload1.SaveAs(folderPath);


                msgLbl.Visible = true;
                msgLbl.Text = "Document uploaded to server";
                msgLbl.ForeColor = System.Drawing.Color.Red;
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "RefreshDDRs", "RefreshDDRs();", true);
            }
        }
    }
}
