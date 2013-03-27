using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Pinsonault.Web;
using System.Collections.Specialized;
using System.IO;
using PathfinderClientModel;
using Pathfinder;
using System.Configuration;
//using Persits.PDF;
using Pinsonault.Application.Merz;
using PathfinderModel;

//If 'Business Planning' is selected then following querystring variables will be passed:
//      Plan_ID, Section_ID, Thera_ID
//If 'Business Planning Report' is selected then following querystring variables will be passed:
//      Plan_ID, Section_ID, Thera_ID, report(=2)
//On click of 'Export' link same page(businessplanning.aspx) will be called with following querystring variables:
//      Plan_ID, Section_ID, Thera_ID, report(=2), Export(=1)
public partial class custom_merz_businessplanning_all_businessplanning : InputFormBase  
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int Plan_ID = Convert.ToInt32(HttpContext.Current.Request.QueryString["Plan_ID"]);
        this.hdnExport.Value = HttpContext.Current.Request.QueryString["Export"];
       
        if (Convert.ToInt32(HttpContext.Current.Request.QueryString["report"]) == 2)
        {
            InfoLives1.ContainerID = "bpPlanInfo";
            MedicalPolicy1.ContainerID = "bpPlanInfo";
        }

        using (PathfinderMerzEntities context = new PathfinderMerzEntities())
        {
            //Check if Business_Plan_Id is already created for Thera_ID and Plan_ID combination
            String Account_Name =
                   (from p in context.PlanSearch1Set 
                    where p.ID == Plan_ID 
                    select p.Name).FirstOrDefault().ToString() ;
            //Display Account Name in the header.
            Literal1.Text = Literal1.Text + " : " + Account_Name; 
        }

        //For export to PDF, use PDFStyles else use BPMainStyles
        if (hdnExport.Value == "1")
        {
            InfoLives1.Export = true;
            MedicalPolicy1.Export = true;
            AddEditBP1.Export = true; 
        }
        else
        {
            InfoLives1.Export = false;
            MedicalPolicy1.Export = false;
            AddEditBP1.Export = false;
        }

    }

    protected override bool IsRequestValid()
    {
        return true;
    }

    protected override void OnPreRenderComplete(EventArgs e)
    {
        Int32 Thera_ID = Convert.ToInt32(HttpContext.Current.Request.QueryString["Thera_ID"]);
        
        this.hdnSection_ID.Value = HttpContext.Current.Request.QueryString["Section_ID"];

        //Store filter values in hidden variables which will be used for export to PDF from merz.js.
        this.hdnPlan_ID.Value = HttpContext.Current.Request.QueryString["Plan_ID"];
        this.hdnThera_ID.Value = HttpContext.Current.Request.QueryString["Thera_ID"];
        this.hdnreport.Value = HttpContext.Current.Request.QueryString["report"];
        this.hdnExport.Value = HttpContext.Current.Request.QueryString["Export"];
       
        FormView frmVW = ((FormView)AddEditBP1.FindControl("formVWBP"));
        String mode = ((HiddenField)AddEditBP1.FindControl("frmvmMode")).Value.ToString() ; 
        UserControl ucCV = ((UserControl)InfoLives1.FindControl("CLlivesMC"));

        //Add PDF stylesheet when exporting to PDF.
        if (hdnExport.Value == "1")
        {
            pdfStyleSheets.Visible = true;
            pdfStyleSheets.IncludeMain = false;
            pdfStyleSheets.IncludeTheme = false;
        }
        else
            pdfStyleSheets.Visible = false;

        //Show and Hide grid and textboxes based on TheraID
        if (Thera_ID == 2) //Dermatology
        {
            ((GridView)frmVW.FindControl("grdvwNeuCoverage")).Visible = false; //Neurology grid
            ((UserControl)frmVW.FindControl("DermCoverage1")).Visible = true; //Dermatology grid

            if (hdnExport.Value == "1")
            {
                frmVW.FindControl("NotesTR1").Visible = false;
                frmVW.FindControl("NotesTR2").Visible = false;
                frmVW.FindControl("AccOverviewTR1").Visible = false;
                frmVW.FindControl("AccOverviewTR2").Visible = false;
                frmVW.FindControl("CurrStatusTR1").Visible = false;
                frmVW.FindControl("CurrStatusTR2").Visible = false;
                frmVW.FindControl("MedPolicyTR1").Visible = false;
                frmVW.FindControl("MedPolicyTR2").Visible = false;
                frmVW.FindControl("PandTTR1").Visible = false;
                frmVW.FindControl("PandTTR2").Visible = false;
                frmVW.FindControl("IssuesTR1").Visible = false;
                frmVW.FindControl("IssuesTR2").Visible = false;
                frmVW.FindControl("StrategiesTR1").Visible = false;
                frmVW.FindControl("StrategiesTR2").Visible = false;
                frmVW.FindControl("TacticsTR1").Visible = false;
                frmVW.FindControl("TacticsTR2").Visible = false;

                ((System.Web.UI.HtmlControls.HtmlControl)frmVW.FindControl("NeurologyDIV")).Visible = false;
                ((System.Web.UI.HtmlControls.HtmlControl)frmVW.FindControl("DermatologyDIV")).Visible = true;
            }
            else
            {
                if (mode == "edit")
                {
                    ((System.Web.UI.HtmlControls.HtmlControl)frmVW.FindControl("NeurologyTextBoxesDiv")).Visible = false;
                    ((System.Web.UI.HtmlControls.HtmlControl)frmVW.FindControl("DermatologyTextBoxesDiv")).Visible = true;
                }
                else
                {
                    frmVW.FindControl("NotesTR1").Visible = true;
                    frmVW.FindControl("NotesTR2").Visible = true;
                    frmVW.FindControl("AccOverviewTR1").Visible = false;
                    frmVW.FindControl("AccOverviewTR2").Visible = false;
                    frmVW.FindControl("CurrStatusTR1").Visible = false;
                    frmVW.FindControl("CurrStatusTR2").Visible = false;
                    frmVW.FindControl("MedPolicyTR1").Visible = false;
                    frmVW.FindControl("MedPolicyTR2").Visible = false;
                    frmVW.FindControl("PandTTR1").Visible = false;
                    frmVW.FindControl("PandTTR2").Visible = false;
                    frmVW.FindControl("IssuesTR1").Visible = true;
                    frmVW.FindControl("IssuesTR2").Visible = true;
                    frmVW.FindControl("StrategiesTR1").Visible = true;
                    frmVW.FindControl("StrategiesTR2").Visible = true;
                    frmVW.FindControl("TacticsTR1").Visible = true;
                    frmVW.FindControl("TacticsTR2").Visible = true;

                    ((System.Web.UI.HtmlControls.HtmlControl)frmVW.FindControl("NeurologyDIV")).Visible = false;
                    ((System.Web.UI.HtmlControls.HtmlControl)frmVW.FindControl("DermatologyDIV")).Visible = false;
                }
            }
        }
        else //Neurology
        {
            ((GridView)frmVW.FindControl("grdvwNeuCoverage")).Visible = true; //Neurology grid
            ((UserControl)frmVW.FindControl("DermCoverage1")).Visible = false; //Dermatology grid

            if (hdnExport.Value == "1")
            {
                frmVW.FindControl("NotesTR1").Visible = false;
                frmVW.FindControl("NotesTR2").Visible = false;
                frmVW.FindControl("AccOverviewTR1").Visible = false;
                frmVW.FindControl("AccOverviewTR2").Visible = false;
                frmVW.FindControl("CurrStatusTR1").Visible = false;
                frmVW.FindControl("CurrStatusTR2").Visible = false;
                frmVW.FindControl("MedPolicyTR1").Visible = false;
                frmVW.FindControl("MedPolicyTR2").Visible = false;
                frmVW.FindControl("PandTTR1").Visible = false;
                frmVW.FindControl("PandTTR2").Visible = false;
                frmVW.FindControl("IssuesTR1").Visible = false;
                frmVW.FindControl("IssuesTR2").Visible = false;
                frmVW.FindControl("StrategiesTR1").Visible = false;
                frmVW.FindControl("StrategiesTR2").Visible = false;
                frmVW.FindControl("TacticsTR1").Visible = false;
                frmVW.FindControl("TacticsTR2").Visible = false;

                ((System.Web.UI.HtmlControls.HtmlControl)frmVW.FindControl("NeurologyDIV")).Visible = true;
                ((System.Web.UI.HtmlControls.HtmlControl)frmVW.FindControl("DermatologyDIV")).Visible = false;
            }
            else
            {
                if (mode == "edit")
                {
                    ((System.Web.UI.HtmlControls.HtmlControl)frmVW.FindControl("NeurologyTextBoxesDiv")).Visible = true;
                    ((System.Web.UI.HtmlControls.HtmlControl)frmVW.FindControl("DermatologyTextBoxesDiv")).Visible = false;
                }
                else
                {
                    frmVW.FindControl("NotesTR1").Visible = false;
                    frmVW.FindControl("NotesTR2").Visible = false;
                    frmVW.FindControl("AccOverviewTR1").Visible = true;
                    frmVW.FindControl("AccOverviewTR2").Visible = true;
                    frmVW.FindControl("CurrStatusTR1").Visible = true;
                    frmVW.FindControl("CurrStatusTR2").Visible = true;
                    frmVW.FindControl("MedPolicyTR1").Visible = true;
                    frmVW.FindControl("MedPolicyTR2").Visible = true;
                    frmVW.FindControl("PandTTR1").Visible = true;
                    frmVW.FindControl("PandTTR2").Visible = true;
                    frmVW.FindControl("IssuesTR1").Visible = true;
                    frmVW.FindControl("IssuesTR2").Visible = true;
                    frmVW.FindControl("StrategiesTR1").Visible = true;
                    frmVW.FindControl("StrategiesTR2").Visible = true;
                    frmVW.FindControl("TacticsTR1").Visible = true;
                    frmVW.FindControl("TacticsTR2").Visible = true;

                    ((System.Web.UI.HtmlControls.HtmlControl)frmVW.FindControl("NeurologyDIV")).Visible = false;
                    ((System.Web.UI.HtmlControls.HtmlControl)frmVW.FindControl("DermatologyDIV")).Visible = false;
                }
            } 
        }

     //Hiding and showing upper links based on formview mode and querystring variables(report and export).
     //When exporting to PDF, hide all the links
     if (hdnExport.Value == "1")
     {
         //Hide edit, save and export links
         EditBP.Visible = false;
         separator1.Visible = false;
         SaveBP.Visible = false;
         separator2.Visible = false; 
         ExportBP.Visible = false;

         //For Medicare Carrier(A & B)
         if (hdnSection_ID.Value == "2")
         {
             //Show readonly label control and hide textbox control.
             ucCV.FindControl("CLReadOnly").Visible = true;
             ucCV.FindControl("CLEdit").Visible = false;
             //synchronise Hidden varible("MedCEnrollment") from AddEditBusinessPlanning.ascx AND
             //Label control from CoveredLives_MC.ascx
             ((Label)ucCV.FindControl("MedCarrierABlbl")).Text = ((HiddenField)frmVW.FindControl("MedCEnrollment")).Value.ToString(); 
         }
         else
         {
             //Hide label and textbox controls for other sections.
             ucCV.FindControl("CLReadOnly").Visible = false;
             ucCV.FindControl("CLEdit").Visible = false;
         }
     }
     else
     {
         //For Business Planning Report(querystring variable report = 2): show only Export link.
         if (hdnreport.Value == "2")
         {
             //Hide edit and save links and show export link
            EditBP.Visible = false;
            separator1.Visible = false;
            SaveBP.Visible = false;
            separator2.Visible = false; 
            ExportBP.Visible = true;
         }
         else //For Bussiness Planning(querystring variable report = 1) do the following:
         {
             if (mode == "readonly")
             {
                //Hide Save link and Show Export link
                //Note: Hiding and showing of "Edit" link is done in OnInit code based on the user alignments.
                SaveBP.Visible = false;
                separator2.Visible = false; 
                ExportBP.Visible = true;

                //For Medicare Carrier(A & B)
                if (hdnSection_ID.Value == "2")
                {
                    //Show readonly label control and hide textbox control.
                    ucCV.FindControl("CLReadOnly").Visible = true;
                    ucCV.FindControl("CLEdit").Visible = false;
                }
                else
                {
                    //Hide label and textbox controls for other sections.
                    ucCV.FindControl("CLReadOnly").Visible = false;
                    ucCV.FindControl("CLEdit").Visible = false;
                }
             }
             else if (mode == "edit")
             {
                 //Hide Save link and Show Edit and Export
                EditBP.Visible = false;
                separator1.Visible = false;
                SaveBP.Visible = true;
                separator2.Visible = true; 
                ExportBP.Visible = true;

                 // For Medicare Carrier(A & B)
                 if (hdnSection_ID.Value == "2")
                 {
                     //Show editable textbox control and hide label control
                     ucCV.FindControl("CLReadOnly").Visible = false;
                     ucCV.FindControl("CLEdit").Visible = true;
                 }
                 else
                 {
                     //Hide label and textbox controls for other sections.
                     ucCV.FindControl("CLReadOnly").Visible = false;
                     ucCV.FindControl("CLEdit").Visible = false;
                 }
             }
         }
     }

     base.OnPreRenderComplete(e);
    }

    protected override void OnPreRender(EventArgs e)
    {
        //Set hidden section_id variable,
        //which will be used to check if section == 2(Medicare Carrier(A & B)) in Merz.js for hiding and 
        //showing MedicarePartB Enrollment(textbox/label) field.
        this.hdnSection_ID.Value = HttpContext.Current.Request.QueryString["Section_ID"];
        
        //Store filter values in hidden variables which will be used for export to PDF from merz.js.
        this.hdnPlan_ID.Value  = HttpContext.Current.Request.QueryString["Plan_ID"];
        this.hdnThera_ID.Value = HttpContext.Current.Request.QueryString["Thera_ID"];
        this.hdnreport.Value = HttpContext.Current.Request.QueryString["report"];
        this.hdnExport.Value = HttpContext.Current.Request.QueryString["Export"];

        //Get the form submit status(true if successful else false) from AddEditBusinessPlanning.ascx control 
        //and update PostBackResult.Success.
        this.PostBackResult.Success = AddEditBP1.SubmitStatus; 
        base.OnPreRender(e);
    }

    protected override void OnInit(EventArgs e)
    {   
        // check if the user has alignment for that plan
        //if yes then check if Business_Plan_Id is already created for Thera_ID and Plan_ID combination
        //if it is created already then set the hidden variable with Business_Plan_Id value
        //else create a record in Business_Planning table and get that Business_Plan_Id and 
        //then set hidden variable.
        //else don't create a record. user will be able to see just the readonly mode.
        int Plan_ID = Convert.ToInt32(HttpContext.Current.Request.QueryString["Plan_ID"]);
        int Thera_ID = Convert.ToInt32(HttpContext.Current.Request.QueryString["Thera_ID"]);
        int Section_ID = Convert.ToInt32(HttpContext.Current.Request.QueryString["Section_ID"]);
        int report = Convert.ToInt32(HttpContext.Current.Request.QueryString["report"]);
        int Export = Convert.ToInt32(HttpContext.Current.Request.QueryString["Export"]);

        Boolean UserAligned = false;
        using ( PathfinderEntities context = new PathfinderEntities()) //Pinsonault.Web.Session.ClientConnectionString) )
        {
            UserAligned = context.CheckUserAlignment(Plan_ID, Pinsonault.Web.Session.UserID); 
        }

        //If UserAligned is false then loggedin user is not allowed to update any data hence hide "Edit" button. 
        if (UserAligned == false)
        {
            EditBP.Visible = false;
            separator1.Visible = false;
            MedicalPolicy1.AlignmentStatus = false;
        }
        else
        {
            EditBP.Visible = true;
            separator1.Visible = true;

            //Hide "Medical Policy: Upload Files" link for export and for Business Planning Report.
            if (Export == 1)
                MedicalPolicy1.AlignmentStatus = false;
            else if(report == 2)
                MedicalPolicy1.AlignmentStatus = false;
            else
                MedicalPolicy1.AlignmentStatus = true; 
        }

        using (PathfinderMerzEntities context = new PathfinderMerzEntities())
        {
            //Check if Business_Plan_Id is already created for Thera_ID and Plan_ID combination
            int bpcnt =
                   (from d in context.BusinessPlansSet
                    where d.Plan_ID == Plan_ID && d.Thera_ID == Thera_ID
                    select d).Count();

            //If bpcnt == 0 then there is no record present for selected Plan_ID and Thera_ID so create one.
            if (bpcnt == 0)
            {
                //Add a record to Business_Plan table only if logged on user is aligned to selected plan.
                if (UserAligned == true)
                    //call CreateBusinessPlanRecord() to add a record in Business_Plan table.
                    BP_ID.Value = Convert.ToString(CreateBusinessPlanRecord());
                else
                    BP_ID.Value = "0";
            }
            else
            {
                //If record exists in Business_Plan table for selected Plan_ID and Thera_ID then get the Business_Plan_ID.
                int bpid =
                   (from d in context.BusinessPlansSet
                    where d.Plan_ID == Plan_ID && d.Thera_ID == Thera_ID
                    select d.Business_Plan_ID).FirstOrDefault();

                //Set the hidden variable with BP_ID
                BP_ID.Value = bpid.ToString();
            }
         
            MedicalPolicy1.BusinessPlanID = Convert.ToInt32(BP_ID.Value);
            AddEditBP1.BusinessPlanID = Convert.ToInt32(BP_ID.Value);  
        }

        //Used for exporting to PDF
        if (!String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["Export"]))
        {
            if (Convert.ToInt32(HttpContext.Current.Request.QueryString["Export"]) == 1)
            {
                //For Exporting to PDF, hide radgrid and show gridview.
                ((Telerik.Web.UI.RadGrid)MedicalPolicy1.FindControl("RadGridbppOLICY")).Visible = false;
                ((GridView)MedicalPolicy1.FindControl("grdvwMedPolicy")).Visible = true;
            }
            else
            {
                //else show radgrid and hide gridview.
                ((Telerik.Web.UI.RadGrid)MedicalPolicy1.FindControl("RadGridbppOLICY")).Visible = true;
                ((GridView)MedicalPolicy1.FindControl("grdvwMedPolicy")).Visible = false;
            }
        }
        else
        {
            //else show radgrid and hide gridview.
            ((Telerik.Web.UI.RadGrid)MedicalPolicy1.FindControl("RadGridbppOLICY")).Visible = true;
            ((GridView)MedicalPolicy1.FindControl("grdvwMedPolicy")).Visible = false;
        }
    }

    protected int CreateBusinessPlanRecord()
    {
        int Plan_ID = Convert.ToInt32(HttpContext.Current.Request.QueryString["Plan_ID"]);
        int Thera_ID = Convert.ToInt32(HttpContext.Current.Request.QueryString["Thera_ID"]);
        int Section_ID = Convert.ToInt32(HttpContext.Current.Request.QueryString["Section_ID"]);

        using (PathfinderMerzEntities context = new PathfinderMerzEntities())
        {
            BusinessPlans bplans = new BusinessPlans();
            bplans.Plan_ID = Plan_ID;
            bplans.Thera_ID = Thera_ID;
            bplans.Created_BY = Pinsonault.Web.Session.FullName;
            bplans.Created_DT = DateTime.UtcNow;
            bplans.Modified_DT = bplans.Created_DT;
            bplans.Modified_BY = bplans.Created_BY;
            context.AddToBusinessPlansSet(bplans);
            context.SaveChanges();
            
            return bplans.Business_Plan_ID;
        }
    }

}
