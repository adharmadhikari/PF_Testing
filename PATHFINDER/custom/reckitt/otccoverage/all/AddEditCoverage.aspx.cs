using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Pinsonault.Web;
using System.Collections.Specialized;
using System.IO;
using PathfinderClientModel;
using Pathfinder;
using System.Configuration;
using Pinsonault.Application.Reckitt;

public partial class custom_reckitt_otccoverage_AddEditCoverage : PageBase
{
    private Boolean MktSegmentChanged;

    protected override void OnPreInit(EventArgs e)
    {
        this.Response.Cache.SetNoStore();

        base.OnPreInit(e);
    }

    protected override void OnInit(EventArgs e)
    {
        //dsOTCCoverage.ConnectionString = Pinsonault.Web.Session.ClientCustomEntiesConnectionString;
        //dsPlans.ConnectionString = Pinsonault.Web.Session.ClientCustomEntiesConnectionString;
           

        base.OnInit(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
           int OTCID = Convert.ToInt32(HttpContext.Current.Request.QueryString["OTCID"]);
           int PlanID = 0;
           String PlanName="";

           using ( PathfinderReckittEntities context = new PathfinderReckittEntities() )
           {
               if (HttpContext.Current.Request["LinkClicked"] == "ViewOTC")
               {
                   var otc =
                       (from d in context.OTCCoverageSet
                        where d.OTC_Coverage_Id == OTCID
                        select d).First();

                   //For selected OTCID: Get PlanID, PlanName  
                   PlanID = Convert.ToInt32(otc.Plan_ID);
                   PlanName = otc.Plan_Name.ToString();
               }

               //Load page
               if (Page.IsPostBack)
               {
                   //Store the selected values in hidden variable.
                   if (HttpContext.Current.Request["LinkClicked"] == "AddOTC")
                   {
                       if (hdnSelectedSegment.Value != rdcmbMktSegment.SelectedValue)
                       {
                           MktSegmentChanged = true;
                       }
                       //Enable/disable textareas based on associated checkboxes.
                       ResetTextareas();
                       hdnSelectedSegment.Value = rdcmbMktSegment.SelectedValue;
                   }
               }

               //Populate market segment and plan dropdowns only for adding new OTC coverage.
               if (HttpContext.Current.Request["LinkClicked"] == "AddOTC")
               {
                   Dictionary<string, PathfinderModel.ClientApplicationAccess> access = Pinsonault.Web.Session.ClientApplicationAccess;
                   //Select all available Section IDs for Todays Accounts
                   rdcmbMktSegment.DataSource = access.Where(i => i.Value.ApplicationID == 1 && i.Value.Section != null && ((i.Value.SectionID == 1) || (i.Value.SectionID == 4) || (i.Value.SectionID == 9) || (i.Value.SectionID == 11) || (i.Value.SectionID == 12))).Select(i => new { ID = i.Value.SectionID, Name = i.Value.Section.Name });
                   rdcmbMktSegment.DataBind();

                   //Set the market segment as per hidden variable.
                   if (!String.IsNullOrEmpty(hdnSelectedSegment.Value))
                   {
                       rdcmbMktSegment.SelectedValue = hdnSelectedSegment.Value;
                   }
               }

               //Updates Plan Name hidden variable.
               this.PlanNameHdn.Value = PlanName;

               if (!Page.IsPostBack)
               {
                   this.formViewOTC.Visible = true;
                   this.Msglbl.Visible = false;
                   this.CloseMsglbl.Visible = false;

                   //Depending on actions(View/Add/Update) change form mode to ReadOnly/Insert/Edit.
                   //Also, set the form header with PlanName included.
                   if (HttpContext.Current.Request["LinkClicked"] == "ViewOTC")
                   {
                       this.formViewOTC.ChangeMode(FormViewMode.ReadOnly);
                       this.titleText.Text = "OTC Coverage: " + this.PlanNameHdn.Value;
                       this.combodiv.Visible = false;

                       tblOTCCoverage otc = null;
                       otc = (from d in context.tblOTCCoverageSet
                              where d.OTC_Coverage_Id == OTCID
                              select d).FirstOrDefault();

                       ((Label)this.formViewOTC.FindControl("Products_Coverage")).Text = ConvertDBValues(otc.Product_PPI, otc.Product_Antihistamines, otc.Product_Pediatric_OTC, otc.Product_Smoking, otc.Product_Obesity, otc.Product_Other, otc.Product_Other_Text);

                       //Only Account Managers assigned to selected plan will be able to edit its OTC coverage.
                       //AEUserID: Account Manager assigned to selected plan. 
                       int otccnt = 0;
                       otccnt = (from d in context.OTCCoverageSet
                                 where d.OTC_Coverage_Id == OTCID && d.Plan_ID == PlanID && d.AE_UserID == Pinsonault.Web.Session.UserID
                                 select d).Count();

                       //If count is zero then hide "EditCoverage" button else display it.
                       if (otccnt != 0)
                           ((Button)this.formViewOTC.FindControl("ViewEditbtn")).Visible = true;
                       else
                           ((Button)this.formViewOTC.FindControl("ViewEditbtn")).Visible = false;
                   }
                   else if (HttpContext.Current.Request["LinkClicked"] == "AddOTC")
                   {
                       this.formViewOTC.ChangeMode(FormViewMode.Insert);
                       this.titleText.Text = "Add OTC Coverage";
                       this.combodiv.Visible = true;
                   }
               }
               else
               {
                   if (HttpContext.Current.Request["LinkClicked"] == "ViewOTC")
                   {
                       this.combodiv.Visible = false;
                       //If form is in edit mode...
                       if (this.formViewOTC.CurrentMode == FormViewMode.Edit)
                       {
                           this.titleText.Text = "Update OTC Coverage: " + this.PlanNameHdn.Value;
                       }
                   }
                   else if (HttpContext.Current.Request["LinkClicked"] == "AddOTC")
                       this.combodiv.Visible = true;
               }
           }
  }


    protected void ResetTextareas()
    {
            if (((CheckBox)this.formViewOTC.FindControl("Product_Other")).Checked == true)
                ((TextBox)this.formViewOTC.FindControl("Product_Other_Text")).Enabled = true;
            else
                ((TextBox)this.formViewOTC.FindControl("Product_Other_Text")).Enabled = false;

            if (((CheckBox)this.formViewOTC.FindControl("Is_CoughCold")).Checked == true)
                ((TextBox)this.formViewOTC.FindControl("Desc_CoughCold_Kits")).Enabled = true;
            else
                ((TextBox)this.formViewOTC.FindControl("Desc_CoughCold_Kits")).Enabled = false;
            
            if (((CheckBox)this.formViewOTC.FindControl("Is_Health_Fairs")).Checked  == true)
                ((TextBox)this.formViewOTC.FindControl("Desc_Health_Fairs")).Enabled = true;
            else
                ((TextBox)this.formViewOTC.FindControl("Desc_Health_Fairs")).Enabled = false;

            if (((CheckBox)this.formViewOTC.FindControl("Is_Education_Brochures")).Checked  == true)
                ((TextBox)this.formViewOTC.FindControl("Desc_Education_Brochures")).Enabled = true;
            else
                ((TextBox)this.formViewOTC.FindControl("Desc_Education_Brochures")).Enabled = false;

            if (((CheckBox)this.formViewOTC.FindControl("Is_Other")).Checked == true)
                ((TextBox)this.formViewOTC.FindControl("Desc_Other")).Enabled = true;
            else
                ((TextBox)this.formViewOTC.FindControl("Desc_Other")).Enabled = false;
    }

     protected void rdcmbMktSegment_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
     {
         rdcmbMktSegment.Text = e.Text;
         rdcmbMktSegment.SelectedValue = e.Value;
     }

     protected void rdcmbPlans_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
     {
         //If market segment is changed then reload plans dropdown
         if (MktSegmentChanged == true)
             MktSegmentChanged = false;
         else
         {
             //If market segment is not changed then change plan selection
             rdcmbPlans.Text = e.Text;
             rdcmbPlans.SelectedValue = e.Value;
         }
     }

     protected void rdcmbPlans_OnDataBound(object sender, EventArgs e)
     {
         if (rdcmbPlans != null)
         {
             //If there are no plans for selected market segment then show a message to user and hide the entry form.
             if (rdcmbPlans.Items.Count == 0)
             {
                 planlbl.Visible = false;
                 rdcmbPlans.Visible = false;
                 noplanlbl.Visible = true;
                 this.formViewOTC.Visible = false;
             }
             else
             {
                 planlbl.Visible = true;
                 rdcmbPlans.Visible = true;
                 noplanlbl.Visible = false;
                 this.formViewOTC.Visible = true;
             }
         }
     }

     //It displays checkbox and associated textarea contents in a specific format.
     protected String ShowDesc(Boolean chk, string txt, string headertxt)
     {
         if (chk == true)
             return headertxt + ":<br />" + txt + "<br /><br />";
         else
             return "";
     }

    //Radio button values: 1=Yes, 2=No, 0=Not Available else none selected.
     protected string ConvertDBValues(string val)
     {
         if (val == "1")
             return "Yes";
         else if (val == "2")
             return "No";
         else if(val == "0" )
             return "Not Available";
         else
             return "";
     }

     //For OTC_Co_Pay_Level field.
     protected string ConvertCopayLevelDBValues(string val)
     {
         if (val == "1")
             return "I";
         else if (val == "2")
             return "II";
         else if (val == "3")
             return "III";
         else
             return "";
     }

     //It displays Covered OTC Product's checkboxes and associated textarea contents in a specific format.
     protected string ConvertDBValues(bool ppi, bool Antihistamines, bool pediatricOTC, bool smoking, bool obesity, bool other, String other_text)
     {
         String retval = "";
         if (ppi == true)
             retval = string.Concat(retval, "PPI<br/>");

         if (Antihistamines == true)
             retval = string.Concat(retval, "Antihistamines<br/>");

         if (pediatricOTC == true)
             retval = string.Concat(retval, "Pediatric OTC<br/>");

         if (smoking == true)
             retval = string.Concat(retval, "Smoking<br/>");

         if (obesity == true)
             retval = string.Concat(retval, "Obesity<br/>");

         if (other == true)
             retval = string.Concat(retval, "Other(s): "+ other_text + "<br/>");
         
             return retval;
     }

     //While inserting the data into OTC_Coverage table AddData is called to set the Plan_ID, Section_ID
    //and few other fields to appropriate values.
     protected void AddData(object sender, EntityDataSourceChangingEventArgs e)
     {
             if (!String.IsNullOrEmpty(rdcmbPlans.SelectedValue))
                 ((tblOTCCoverage)e.Entity).Plan_ID = Convert.ToInt32(rdcmbPlans.SelectedValue);

             if (!String.IsNullOrEmpty(rdcmbMktSegment.SelectedValue))
                 ((tblOTCCoverage)e.Entity).Section_Id = Convert.ToInt32(rdcmbMktSegment.SelectedValue);

             ((tblOTCCoverage)e.Entity).Created_BY = Pinsonault.Web.Session.FullName;
             ((tblOTCCoverage)e.Entity).Created_DT = DateTime.UtcNow;

             //If checkbox is checked then store the textarea data associated with it.
             if (((CheckBox)this.formViewOTC.FindControl("Product_Other")).Checked)
                 ((tblOTCCoverage)e.Entity).Product_Other_Text = ((TextBox)this.formViewOTC.FindControl("Product_Other_Text")).Text;
             else
                 ((tblOTCCoverage)e.Entity).Product_Other_Text = "";

             if (((CheckBox)this.formViewOTC.FindControl("Is_CoughCold")).Checked)
                 ((tblOTCCoverage)e.Entity).Desc_CoughCold_Kits = ((TextBox)this.formViewOTC.FindControl("Desc_CoughCold_Kits")).Text;
             else
                 ((tblOTCCoverage)e.Entity).Desc_CoughCold_Kits = "";

             if (((CheckBox)this.formViewOTC.FindControl("Is_Health_Fairs")).Checked)
                 ((tblOTCCoverage)e.Entity).Desc_Health_Fairs = ((TextBox)this.formViewOTC.FindControl("Desc_Health_Fairs")).Text;
             else
                 ((tblOTCCoverage)e.Entity).Desc_Health_Fairs = "";

             if (((CheckBox)this.formViewOTC.FindControl("Is_Education_Brochures")).Checked)
                 ((tblOTCCoverage)e.Entity).Desc_Education_Brochures = ((TextBox)this.formViewOTC.FindControl("Desc_Education_Brochures")).Text;
             else
                 ((tblOTCCoverage)e.Entity).Desc_Education_Brochures = "";

             if (((CheckBox)this.formViewOTC.FindControl("Is_Other")).Checked)
                 ((tblOTCCoverage)e.Entity).Desc_Other = ((TextBox)this.formViewOTC.FindControl("Desc_Other")).Text;
             else
                 ((tblOTCCoverage)e.Entity).Desc_Other = "";
   }

     //While updating the data into OTC_Coverage table EditData is called. 
     protected void EditData(object sender, EntityDataSourceChangingEventArgs e)
     {
         ((tblOTCCoverage)e.Entity).Modified_BY = Pinsonault.Web.Session.FullName;
         ((tblOTCCoverage)e.Entity).Modified_DT = DateTime.UtcNow;

         //If checkbox is checked then store the textarea data associated with it.
         if (((CheckBox)this.formViewOTC.FindControl("Product_Other")).Checked)
             ((tblOTCCoverage)e.Entity).Product_Other_Text = ((TextBox)this.formViewOTC.FindControl("Product_Other_Text")).Text;
         else
             ((tblOTCCoverage)e.Entity).Product_Other_Text = "";

         if (((CheckBox)this.formViewOTC.FindControl("Is_CoughCold")).Checked)
             ((tblOTCCoverage)e.Entity).Desc_CoughCold_Kits = ((TextBox)this.formViewOTC.FindControl("Desc_CoughCold_Kits")).Text;
         else
             ((tblOTCCoverage)e.Entity).Desc_CoughCold_Kits = "";

         if (((CheckBox)this.formViewOTC.FindControl("Is_Health_Fairs")).Checked)
             ((tblOTCCoverage)e.Entity).Desc_Health_Fairs = ((TextBox)this.formViewOTC.FindControl("Desc_Health_Fairs")).Text;
         else
             ((tblOTCCoverage)e.Entity).Desc_Health_Fairs = "";

         if (((CheckBox)this.formViewOTC.FindControl("Is_Education_Brochures")).Checked)
             ((tblOTCCoverage)e.Entity).Desc_Education_Brochures = ((TextBox)this.formViewOTC.FindControl("Desc_Education_Brochures")).Text;
         else
             ((tblOTCCoverage)e.Entity).Desc_Education_Brochures = "";

         if (((CheckBox)this.formViewOTC.FindControl("Is_Other")).Checked)
             ((tblOTCCoverage)e.Entity).Desc_Other = ((TextBox)this.formViewOTC.FindControl("Desc_Other")).Text;
         else
             ((tblOTCCoverage)e.Entity).Desc_Other = "";

     }

     //Confirmation message is displayed after insert/update.
     protected void ConfirmMsg(object sender, EntityDataSourceChangedEventArgs e)
     {
         if (e.Exception == null)
         {
             this.formViewOTC.Visible = false;

             if (HttpContext.Current.Request["LinkClicked"] == "AddOTC")
                 this.Msglbl.Text = "<br/><b>New OTC coverage has been added successfully.</b><br/><br/>";
             else if (HttpContext.Current.Request["LinkClicked"] == "ViewOTC")
                 this.Msglbl.Text = "<br/><b>OTC Coverage has been updated successfully.</b><br/><br/>";

             this.Msglbl.Visible = true;
             this.CloseMsglbl.Visible = true;
             this.combodiv.Visible = false;

             //Calls Javascript function RefreshMyKCs() to refresh otc coverage parent grid.
             Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "RefreshOTC", "RefreshOTC();", true);
         }
         else
         {
             if (HttpContext.Current.Request["LinkClicked"] == "AddOTC")
                 this.Msglbl.Text = "<br/><b>Error while adding New OTC coverage.</b><br/><br/>";
             else if (HttpContext.Current.Request["LinkClicked"] == "UpdOTC")
                 this.Msglbl.Text = "<br/><b>Error while updating OTC Coverage.</b><br/><br/>";
             
             this.formViewOTC.Visible = false;
             this.Msglbl.Visible = true;
         }
     }

    //When clicked on 'Edit Coverage' button in readonly view mode it changes form's mode to Edit.
     protected void ViewEditbtn_Click(object sender, EventArgs e)
     {
         this.formViewOTC.ChangeMode(FormViewMode.Edit);
         this.titleText.Text = "Update OTC Coverage: " + this.PlanNameHdn.Value;
     }

}

