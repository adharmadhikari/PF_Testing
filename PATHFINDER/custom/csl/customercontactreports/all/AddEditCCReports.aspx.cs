using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections;
using PathfinderClientModel;


public partial class custom_pinso_customercontactreports_all_AddEditCCReports : PageBase
{
    protected void Page_Init(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            setComboValues();
            this.formViewCCR.Visible = true;
          
        }
        if ( Request.QueryString["LinkClicked"] == "AddCCR" )
        {
            this.formViewCCR.ChangeMode(FormViewMode.Insert);
        }
       
        dsMeetingOutcome.ConnectionString = Pinsonault.Web.Session.ClientConnectionString;
        dsFollowUpNotes.ConnectionString = Pinsonault.Web.Session.ClientConnectionString;
        dsContactReport.ConnectionString = Pinsonault.Web.Session.ClientConnectionString;
        dsProductsDiscussed.ConnectionString = Pinsonault.Web.Session.ClientConnectionString;
        dsMeetingActivity.ConnectionString = Pinsonault.Web.Session.ClientConnectionString;
        dsMeetingType.ConnectionString = Pinsonault.Web.Session.ClientConnectionString;                    
        dsPersonsMet.ConnectionString = Pinsonault.Web.Session.ClientDBConnectionString;        
    }
    protected void Page_Load(object sender, EventArgs e)
    {
      
        if (!Page.IsPostBack)
        {
            
            this.Msglbl.Visible = false;
            //setComboValues();
            Page.ClientScript.RegisterStartupScript(typeof(Page), "Update", "UpdChkSelection();", true);
            Page.ClientScript.RegisterStartupScript(typeof(Page), "UpdateFollow", "updFollowChkSelection();", true);
            Page.ClientScript.RegisterStartupScript(typeof(Page), "UpdateOutcome", "updMeetOutcomeChkSelection();", true);
            Page.ClientScript.RegisterStartupScript(typeof(Page), "UpdateKeyContacts", "updKeyContactChkSelection();", true);

            titleText.Text = String.Format("{0} Meeting - {1}", Request.QueryString["LinkClicked"].Replace("CCR", ""), Request.QueryString["PlanName"]);        
        }
        
    }
    protected void setComboValues()
    {
        // This function always the returns the selection of Products_Discussed, KeyContacts, MeetingOutcome, FollowupNotes for a Particular 
        // Contact_Report_ID 
        string strCallreportId = System.Web.HttpContext.Current.Request.QueryString["CRID"];
        string strPlanID = System.Web.HttpContext.Current.Request.QueryString["PlanID"];
        int planID = Convert.ToInt32(strPlanID);
        int Contact_Report_ID = 0;
        
          
        if (strCallreportId != null && strCallreportId != "")
        {
            Contact_Report_ID = System.Convert.ToInt32(strCallreportId);
        }
        if (Contact_Report_ID != 0)
        {
            using ( PathfinderClientEntities context = new PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString) )
            {

                // if a Contact_Report_ID already exists then retreive the values from ContactProductsDiscussed, ContactFollowupNotes, ContactOutcome
                // and ContactKeyContacts so can store the values into a hidden fields 
                //
                var ssPrdsDiscuss = (from d in context.ContactReportProductsDiscussedSet
                                    where d.Contact_Report_ID == Contact_Report_ID
                                    select d).ToList().Select(d => string.Format("{0}", d.Products_Discussed_ID.ToString()));

                //Comma separate individual record's data.
                hdnPrdsDisccused.Value = string.Join(",", ssPrdsDiscuss.ToArray());

                var ssFollowupNotes = (from f in context.ContactReportFollowupNotesSet
                                       where f.Contact_Report_ID == Contact_Report_ID
                                       select f).ToList().Select(f => string.Format("{0}", f.Followup_ID.ToString()));

                hdnFollowupNotes.Value = string.Join(",", ssFollowupNotes.ToArray());

                var ssMeetingOutcome = (from m in context.ContactReportOutcomeSet
                                        where m.Contact_Report_ID == Contact_Report_ID
                                        select m).ToList().Select(m => string.Format("{0}", m.Outcome_ID.ToString()));

                hdnMeetOutcome.Value = string.Join(",", ssMeetingOutcome.ToArray());

                var ssKeyContacts = (from k in context.ContactReportKeyContactSet
                                     where k.Contact_Report_ID == Contact_Report_ID
                                     select k).ToList().Select(k => string.Format("{0},{1}", k.Key_Contact_ID.ToString(), ((k.Is_Client_Key_Contact== true)? "1" : "0")));
                
                hdnKeyContacts.Value = string.Join("|", ssKeyContacts.ToArray());

                                    
            }
        }
        else { hdnPrdsDisccused.Value = ""; hdnMeetOutcome.Value = ""; hdnFollowupNotes.Value = ""; hdnKeyContacts.Value = ""; }
       
    }

    protected void ProcessRequest(string BtnClicked)
    {
        Page.ClientScript.RegisterStartupScript(typeof(Page), "Update", "UpdChkSelection();", true);
        Page.ClientScript.RegisterStartupScript(typeof(Page), "UpdateFollow", "updFollowChkSelection();", true);
        Page.ClientScript.RegisterStartupScript(typeof(Page), "UpdateOutcome", "updMeetOutcomeChkSelection();", true);
        Page.ClientScript.RegisterStartupScript(typeof(Page), "UpdateKeyContacts", "updKeyContactChkSelection();", true);


        //Create new a Contact Customer Report if Contact_Report_ID is null in Query String else retrieve the the Contact Report
        string msg = "";
        string strCallreportId = System.Web.HttpContext.Current.Request.QueryString["CRID"];
        int Contact_Report_ID = 0;
        if (strCallreportId != null && strCallreportId != "")
        {
            Contact_Report_ID = System.Convert.ToInt32(strCallreportId);
        }
        //if (Contact_Report_ID != 0)
        //{
        using (PathfinderClientEntities context = new PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
        {
            ContactReport ccrView = null;
            if (Contact_Report_ID != 0)
            {
                // Retreive Contact Report With Contact_Report_ID
                ccrView = context.ContactReportSet.FirstOrDefault(c => c.Contact_Report_ID == Contact_Report_ID);
                msg = "<br/><b>Customer Contact Report has been updated successfully.</b><br/><br/>";
                ccrView.Contact_Date = Convert.ToDateTime(((System.Web.UI.WebControls.TextBox)(this.formViewCCR.FindControl("rdCCRDate"))).Text);
                if (!string.IsNullOrEmpty(((System.Web.UI.WebControls.TextBox)(this.formViewCCR.FindControl("rdFollowUpDate"))).Text))
                    ccrView.Followup_Date = Convert.ToDateTime(((System.Web.UI.WebControls.TextBox)(this.formViewCCR.FindControl("rdFollowUpDate"))).Text);
                else
                    ccrView.Followup_Date = null;
                if (!string.IsNullOrEmpty(((Telerik.Web.UI.RadComboBox)(this.formViewCCR.FindControl("rdlMeetingActivity"))).SelectedValue))
                    ccrView.Meeting_Activity_ID = Convert.ToInt32(((Telerik.Web.UI.RadComboBox)(this.formViewCCR.FindControl("rdlMeetingActivity"))).SelectedValue);
                else
                    ccrView.Meeting_Activity_ID = null;
                if (!string.IsNullOrEmpty(((Telerik.Web.UI.RadComboBox)(this.formViewCCR.FindControl("rdlMeetingType"))).SelectedValue))
                    ccrView.Meeting_Type_ID = Convert.ToInt32(((Telerik.Web.UI.RadComboBox)(this.formViewCCR.FindControl("rdlMeetingType"))).SelectedValue);
                else
                    ccrView.Meeting_Type_ID = null;
                ccrView.User_ID = Pinsonault.Web.Session.UserID;
                ccrView.Client_ID = Pinsonault.Web.Session.ClientID;
                ccrView.Modified_DT = DateTime.UtcNow;
                ccrView.Modified_BY = Pinsonault.Web.Session.FullName;
            }
            else
            {
                //Create a new Contact Report
                ccrView = new ContactReport();
                ccrView.Plan_ID = System.Convert.ToInt32(System.Web.HttpContext.Current.Request.QueryString["PlanID"]);
                ccrView.Created_DT = DateTime.UtcNow;
                ccrView.Created_BY = Pinsonault.Web.Session.FullName;
                msg = "<br/><b>Customer Contact Report has been added successfully.</b><br/><br/>";

                ccrView.Contact_Date = Convert.ToDateTime(((System.Web.UI.WebControls.TextBox)(this.formViewCCR.FindControl("rdCCRDate"))).Text);
                if (!string.IsNullOrEmpty(((System.Web.UI.WebControls.TextBox)(this.formViewCCR.FindControl("rdFollowUpDate"))).Text))
                    ccrView.Followup_Date = Convert.ToDateTime(((System.Web.UI.WebControls.TextBox)(this.formViewCCR.FindControl("rdFollowUpDate"))).Text);
                else
                    ccrView.Followup_Date = null;
                if (!string.IsNullOrEmpty(((Telerik.Web.UI.RadComboBox)(this.formViewCCR.FindControl("rdlMeetingActivity"))).SelectedValue))
                {
                    string a = ((Telerik.Web.UI.RadComboBox)(this.formViewCCR.FindControl("rdlMeetingActivity"))).SelectedValue;
                    ccrView.Meeting_Activity_ID = Convert.ToInt32(((Telerik.Web.UI.RadComboBox)(this.formViewCCR.FindControl("rdlMeetingActivity"))).SelectedValue);
                }
                else
                    ccrView.Meeting_Activity_ID = null;
                if (!string.IsNullOrEmpty(((Telerik.Web.UI.RadComboBox)(this.formViewCCR.FindControl("rdlMeetingType"))).SelectedValue))
                    ccrView.Meeting_Type_ID = Convert.ToInt32(((Telerik.Web.UI.RadComboBox)(this.formViewCCR.FindControl("rdlMeetingType"))).SelectedValue);
                else
                    ccrView.Meeting_Type_ID = null;
                ccrView.User_ID = Pinsonault.Web.Session.UserID;
                ccrView.Client_ID = Pinsonault.Web.Session.ClientID;
                ccrView.Modified_DT = DateTime.UtcNow;
                ccrView.Modified_BY = Pinsonault.Web.Session.FullName;
                context.AddToContactReportSet(ccrView);

            }

            //Save the the changes made to the Contact Report
            context.SaveChanges();


            // There can be multiple Products Discussed, FollowUp Notes, Meeting Outcomes, KeyContacts  for any Report

            // get the Products_Discussed_IDs From The Contact_Report_Products_Discussed Table 
            var ssPrdsDiscuss = from d in context.ContactReportProductsDiscussedSet
                                where d.Contact_Report_ID == ccrView.Contact_Report_ID
                                select d;
            //delete everytime all the products discussed id's in Contact_Report_Products_Discussed for a particular Contact_Report_ID
            //so that if a user changes selection of products any no of times the changes always reflect the latest selection in the database
            foreach (var plan in ssPrdsDiscuss) context.DeleteObject(plan);

            var ssFollowupNotes = from f in context.ContactReportFollowupNotesSet
                                  where f.Contact_Report_ID == ccrView.Contact_Report_ID
                                  select f;

            foreach (var plan in ssFollowupNotes) context.DeleteObject(plan);


            var ssMeetingOutcome = from m in context.ContactReportOutcomeSet
                                   where m.Contact_Report_ID == ccrView.Contact_Report_ID
                                   select m;

            foreach (var plan in ssMeetingOutcome) context.DeleteObject(plan);


            var ssKeyContacts = from k in context.ContactReportKeyContactSet
                                where k.Contact_Report_ID == ccrView.Contact_Report_ID
                                select k;

            foreach (var plan in ssKeyContacts) context.DeleteObject(plan);
            context.SaveChanges();

            if (!String.IsNullOrEmpty(hdnPrdsDisccused.Value.ToString()))
            {

                if (hdnPrdsDisccused.Value.ToString().IndexOf(",") > 0)
                {
                    //Split the data by comma 
                    string[] proDisids = hdnPrdsDisccused.Value.ToString().Split(new Char[] { ',' });
                    foreach (string ids in proDisids)
                    {
                        int proID = Convert.ToInt32(ids);
                        ContactReportProductsDiscussed ccrPD = new ContactReportProductsDiscussed();
                        ccrPD.Contact_Report_ID = ccrView.Contact_Report_ID;
                        ccrPD.Products_Discussed_ID = Convert.ToInt32(proID);
                        context.AddToContactReportProductsDiscussedSet(ccrPD);
                    }

                }
                else
                {
                    string proDisid = hdnPrdsDisccused.Value.ToString();
                    ContactReportProductsDiscussed ccrPD = new ContactReportProductsDiscussed();
                    ccrPD.Contact_Report_ID = ccrView.Contact_Report_ID;
                    ccrPD.Products_Discussed_ID = Convert.ToInt32(proDisid);
                    context.AddToContactReportProductsDiscussedSet(ccrPD);
                }


            }



            if (!String.IsNullOrEmpty(hdnFollowupNotes.Value.ToString()))
            {

                if (hdnFollowupNotes.Value.ToString().IndexOf(",") > 0)
                {
                    //Split the data by comma 
                    string[] proDisids = hdnFollowupNotes.Value.ToString().Split(new Char[] { ',' });
                    foreach (string ids in proDisids)
                    {
                        int proID = Convert.ToInt32(ids);
                        ContactReportFollowupNotes ccrFollowup = new ContactReportFollowupNotes();
                        ccrFollowup.Contact_Report_ID = ccrView.Contact_Report_ID;
                        ccrFollowup.Followup_ID = Convert.ToInt32(proID);
                        context.AddToContactReportFollowupNotesSet(ccrFollowup);

                    }

                }
                else
                {
                    string proDisid = hdnFollowupNotes.Value.ToString();
                    ContactReportFollowupNotes ccrFollowup = new ContactReportFollowupNotes();
                    ccrFollowup.Contact_Report_ID = ccrView.Contact_Report_ID;
                    ccrFollowup.Followup_ID = Convert.ToInt32(proDisid);
                    context.AddToContactReportFollowupNotesSet(ccrFollowup);

                }
                //context.SaveChanges();
            }


            if (!String.IsNullOrEmpty(hdnMeetOutcome.Value.ToString()))
            {

                if (hdnMeetOutcome.Value.ToString().IndexOf(",") > 0)
                {
                    //Split the data by comma 
                    string[] proDisids = hdnMeetOutcome.Value.ToString().Split(new Char[] { ',' });
                    foreach (string ids in proDisids)
                    {
                        int proID = Convert.ToInt32(ids);
                        ContactReportOutcome ccrOutcome = new ContactReportOutcome();
                        ccrOutcome.Contact_Report_ID = ccrView.Contact_Report_ID;
                        ccrOutcome.Outcome_ID = Convert.ToInt32(proID);
                        context.AddToContactReportOutcomeSet(ccrOutcome);
                    }

                }
                else
                {
                    string proDisid = hdnMeetOutcome.Value.ToString();
                    ContactReportOutcome ccrOutcome = new ContactReportOutcome();
                    ccrOutcome.Contact_Report_ID = ccrView.Contact_Report_ID;
                    ccrOutcome.Outcome_ID = Convert.ToInt32(proDisid);
                    context.AddToContactReportOutcomeSet(ccrOutcome);

                }

            }


            if (!String.IsNullOrEmpty(hdnKeyContacts.Value.ToString()))
            {

                if (hdnKeyContacts.Value.ToString().IndexOf("|") > 0)
                {
                    //Split the data by | 
                    string[] proDisids = hdnKeyContacts.Value.ToString().Split(new Char[] { '|' });
                    foreach (string ids in proDisids)
                    {
                        string[] splitIDs = ids.Split(new Char[] { ',' });
                        int prodID = Convert.ToInt32(splitIDs[0]);
                        int iskey = Convert.ToInt32(splitIDs[1]);

                        //start- code changed, AM: 07/20/2011 
                        string FullID = string.Format("{0},{1}", prodID, iskey);
                        int PlanID = Convert.ToInt32(Request.QueryString["PlanID"]);
                        //Get KC First and Last Name
                        //get if the key contact exists or not
                        var keyContacts = from k in context.KeyContactCCRPersonsMetSet
                                          where k.Plan_ID == PlanID
                                          where k.KC_ID == prodID
                                          where k.Full_ID == FullID
                                          select k;

                        if (keyContacts.Count() > 0)
                        {
                            string strFName = keyContacts.FirstOrDefault().KC_F_Name;
                            string strLName = keyContacts.FirstOrDefault().KC_L_Name;


                            ContactReportKeyContact ccrKeyContact = new ContactReportKeyContact();

                            ccrKeyContact.Contact_Report_ID = ccrView.Contact_Report_ID;
                            ccrKeyContact.Key_Contact_ID = prodID;

                            ccrKeyContact.KC_F_Name = strFName;
                            ccrKeyContact.KC_L_Name = strLName;
                            ccrKeyContact.Is_Client_Key_Contact = Convert.ToBoolean(iskey);

                            context.AddToContactReportKeyContactSet(ccrKeyContact);
                        }
                        //end- code changed, AM: 07/20/2011 

                    }

                }
                else
                {
                    string[] splitIDs = hdnKeyContacts.Value.ToString().Split(new Char[] { ',' });
                    int prodID = Convert.ToInt32(splitIDs[0]);
                    int iskey = Convert.ToInt32(splitIDs[1]);

                    //start- code changed, AM: 07/20/2011 
                    string FullID = string.Format("{0},{1}", prodID, iskey);
                    int PlanID = Convert.ToInt32(Request.QueryString["PlanID"]);
                    //Get KC First and Last Name
                    //get if the key contact exists or not
                    var keyContacts = from k in context.KeyContactCCRPersonsMetSet
                                      where k.Plan_ID == PlanID
                                      where k.KC_ID == prodID
                                      where k.Full_ID == FullID
                                      select k;
                    if (keyContacts.Count() > 0)
                    {
                        string strFName = keyContacts.FirstOrDefault().KC_F_Name;
                        string strLName = keyContacts.FirstOrDefault().KC_L_Name;

                        ContactReportKeyContact ccrKeyContact = new ContactReportKeyContact();
                        ccrKeyContact.Contact_Report_ID = ccrView.Contact_Report_ID;
                        ccrKeyContact.Key_Contact_ID = prodID;

                        ccrKeyContact.KC_F_Name = strFName;
                        ccrKeyContact.KC_L_Name = strLName;
                        ccrKeyContact.Is_Client_Key_Contact = Convert.ToBoolean(iskey);

                        context.AddToContactReportKeyContactSet(ccrKeyContact);
                    }
                    //end- code changed, AM: 07/20/2011

                }
            }

            context.SaveChanges();


            }

            //Calls Javascript function RefreshMyKCs() to refresh my key contacts parent grid.
            //Page.ClientScript.RegisterStartupScript(typeof(Page), "Update", "UpdChkSelection();", true);
            //Page.ClientScript.RegisterStartupScript(typeof(Page), "UpdateFollow", "updFollowChkSelection();", true);
            //Page.ClientScript.RegisterStartupScript(typeof(Page), "UpdateOutcome", "updMeetOutcomeChkSelection();", true);
            //Page.ClientScript.RegisterStartupScript(typeof(Page), "UpdateKeyContacts", "updKeyContactChkSelection();", true);
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "RefreshCCRs", "RefreshCCRs();", true);
            this.Msglbl.Text = msg;
            this.Msglbl.Visible = true;
            formViewCCR.Visible = false;
        
    }
    protected void Addbtn_Click(object sender, EventArgs e)
    {
        ProcessRequest("Add");
    }
    protected void Editbtn_Click(object sender, EventArgs e)
    {
         ProcessRequest("Edit");
    }
   
}
