using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PathfinderClientModel;
using Pinsonault.Web;
using System.IO;
using System.Data;
using Pinsonault.Application.Reckitt;
using PathfinderModel;

public partial class custom_reckitt_businessplanning_all_businessplanning : InputFormBase  
{
    protected override bool IsRequestValid()
    {
        return true;
    }
   
    protected void Page_Load(object sender, EventArgs e)
    {

        int SegmentID = Convert.ToInt32(Request.QueryString["Segment_ID"]);
        
        if (string.IsNullOrEmpty(Request.QueryString["Plan_ID"]))
        {
            throw new HttpException(500, "Plan name is required.");
        }
        if (SegmentID == 17)
        {
            coveredLives.ShowTotalCoveredLives = false;
            coveredLives.ShowPharmLives = false;
            coveredLives.CoveredLivesEntitySet = "CoveredLivesMedDSet";
        }
        else if (SegmentID == 9)
        { 
            pnlStateMedicaid.Visible = true;
            pnlCoveredLives.Visible = false;
        }

        if (!Page.IsPostBack)
        {
            bool isValidUser = false;
            int iBusinessPlanID = 0;
            int iBPCount = 0;

            int iPlanID = Convert.ToInt32(Request.QueryString["Plan_ID"]);

            using (PathfinderModel.PathfinderEntities clientContext = new PathfinderModel.PathfinderEntities())//(Pinsonault.Web.Session.ClientConnectionString))
            {
                isValidUser = clientContext.CheckUserAlignment(iPlanID, Pinsonault.Web.Session.UserID);
            }
            
            //check if business plan id exists or not, if not and if isValidUser = true -- create a business plannid for a plan 
            using (PathfinderReckittEntities context = new PathfinderReckittEntities())
            {
                iBPCount = (from bp in context.BusinessPlanSet
                            where bp.Plan_ID == iPlanID
                            select bp).Count();

                if (iBPCount > 0)
                {
                    var BusinessPlanID = (from ibp in context.BusinessPlanSet
                                          where ibp.Plan_ID == iPlanID
                                          select ibp).First();
                    iBusinessPlanID = BusinessPlanID.Business_Plan_ID;
                }
                
                ShowForm(isValidUser,iBPCount);
                if (isValidUser && iBPCount == 0)
                {
                    //insert the record in Business_Plans
                    BusinessPlan bp = new BusinessPlan();
                    bp.Plan_ID = iPlanID;
                    bp.O_Cough_Cold_Kits_YN = false;
                    bp.O_Education_Brochures_YN = false;
                    bp.O_Health_Fairs_YN = false;
                    bp.O_Other_YN = false;
                    bp.Created_BY = Pinsonault.Web.Session.FullName;
                    bp.Created_DT = DateTime.UtcNow;
                    context.AddToBusinessPlanSet(bp);
                    context.SaveChanges();
                    iBusinessPlanID = bp.Business_Plan_ID;
                }
                
                //assign the hidden field value for business plan id
                hdnBPID.Value = iBusinessPlanID.ToString();
            }
        }
    }

     //show the save button with form editable and Goals button
                  
    private void ShowForm(bool isValidUser, int iBPCount)
    {
        //hide buttons of user is not assigned to plan
        divButtons.Visible = isValidUser;
        btnGoals.Visible = isValidUser;
        Edit.Visible = isValidUser;
        Save.Visible = isValidUser;

        if (isValidUser)
        {
            if (iBPCount == 0)
            {
                frmBPAccountSummary.ChangeMode(FormViewMode.Edit);
            
                foreach (GridViewRow row in grvKeyContacts.Rows)
                {
                    ((CheckBox)row.FindControl("chkSelect")).Enabled = true;
                }
                ToggleEditSaveButton(false);               
            }
            else
            {
                frmBPAccountSummary.ChangeMode(FormViewMode.ReadOnly);
             
                foreach (GridViewRow row in grvKeyContacts.Rows)
                {
                    ((CheckBox)row.FindControl("chkSelect")).Enabled = false;
                }
                ToggleEditSaveButton(true);              
            }
        }
        else
        {            
            if (iBPCount == 0)
            {
                frmBPAccountSummary.ChangeMode(FormViewMode.ReadOnly);
               
                foreach (GridViewRow row in grvKeyContacts.Rows)
                {
                    ((CheckBox)row.FindControl("chkSelect")).Enabled = false;
                }
                divButtons.Visible = false;
            }
            else
            {
                divButtons.Visible = true;
                Edit.Visible = false;
                Save.Visible = false;
            }            
        }               
    }

    protected void EditBusinessPlan(object sender, EventArgs e)
    {
        frmBPAccountSummary.ChangeMode(FormViewMode.Edit);
     
        foreach (GridViewRow row in grvKeyContacts.Rows)
        {
            ((CheckBox)row.FindControl("chkSelect")).Enabled = true;
        }
        PostBackResult.Success = true;
        ToggleEditSaveButton(false);
    }
  
    private void ToggleEditSaveButton(bool bShowEditButton)
    {
        Save.Visible = !bShowEditButton;
        Edit.Visible = bShowEditButton;
    }
    protected void UpdateAccountSummary(object sender, EntityDataSourceChangingEventArgs e)
    {
        ((BusinessPlan)e.Entity).Modified_BY = Pinsonault.Web.Session.FullName;
        ((BusinessPlan)e.Entity).Modified_DT = DateTime.UtcNow;
        frmBPAccountSummary.ChangeMode(FormViewMode.ReadOnly);
        
    }    
    protected bool ConvertDBvalues(string strdbvalue)
    {
        if (string.IsNullOrEmpty(strdbvalue)) 
            return false;
        else 
            return Convert.ToBoolean(strdbvalue);        
    }
    protected string ShowCheckBoxValue(Boolean chk, string headertxt)
    {
        if (chk == true)
            return headertxt ;
        else
            return "";
    }
    protected string ConvertOTC_DBValues(string val)
    {
        string strReturnVal = "";
         switch(val)
        {                 
            case  "1":
                 strReturnVal = "Yes";
                 break;
             case "2":
                 strReturnVal = "No";
                 break;
             case  "0":
                 strReturnVal =  "Not Available";
                 break;
             default:
                 strReturnVal = "";
                 break;
        }
         return strReturnVal;
    }
    protected string ConvertSchedulePeriodValues(string val)
    {
        string strReturnVal = "";
        switch (val)
        {
            case "1":
                strReturnVal = "0-1 months";
                break;
            case "2":
                strReturnVal = "2-3 months";
                break;
            case "3":
                strReturnVal = "4-6 months";
                break;
            case "4":
                strReturnVal = "7-12 months";
                break;
            case "5":
                strReturnVal = "Not Available";
                break;
            default:
                strReturnVal = "";
                break;
        }
        return strReturnVal;
    }

    protected string ConvertLineBreaks(String input)
    {
        if (!String.IsNullOrEmpty(input))
        {
            //Replace carrige return/linefeed with <BR/>.
            input = input.Replace("\r\n", "<BR/>");
            //Replace registered trademark character(®) with &reg;
            return input.Replace("®", "&reg;");
        }
        else
            return "";
    }
        
    #region "KeyContacts"

    protected void OnSelectingContacts(object sender, EntityDataSourceSelectingEventArgs e)
    {
        if (Convert.ToInt32(Request.QueryString["Segment_ID"]) == 12)
        {
            e.DataSource.WhereParameters.Clear();
            string strwhere = "it.Plan_ID in {" + DODHeadquarters + "}";
            e.DataSource.AutoGenerateWhereClause = false;
            e.DataSource.Where = strwhere;           
        }       
    }
    public string DODHeadquarters
    {
        get
        {
            using (PathfinderEntities context = new PathfinderEntities())
            {
                List<string> list = new List<string>();

                foreach (int id in context.GetDODHeadQuarters())
                {
                    list.Add(id.ToString());
                }

                string ids = string.Join(",", list.ToArray());
                if (!string.IsNullOrEmpty(ids))
                    return ids;

                return "0";
            }
        }
    }
   
    protected void UpdateKeyContacts(object sender, EventArgs e)
    {
        int BPID = Convert.ToInt32(hdnBPID.Value);
        
        using (PathfinderReckittEntities context = new PathfinderReckittEntities())
        {   
            //step 1: delete all the records for this business plan key contacts
                tblBusinessPlanKeyContacts kcOld = new tblBusinessPlanKeyContacts();
                int iKCCount = (from KC in context.tblBusinessPlanKeyContactsSet 
                            where KC.Business_Plan_ID == BPID 
                            select KC).Count();

                if (iKCCount > 0)
                {
                    var ssKCToDeleteQuery = from kcDelete in context.tblBusinessPlanKeyContactsSet
                                             where kcDelete.Business_Plan_ID == BPID
                                             select kcDelete;
                    foreach (var kcToDelete in ssKCToDeleteQuery.Select(d => d))
                    {
                        context.DeleteObject(kcToDelete);                        
                    }                    
                    context.SaveChanges();
                }
                

            //step 2:insert all the selected records in business plan contacts          
           
            foreach(GridViewRow row in grvKeyContacts.Rows)
            {
                //check if the record is selected
                CheckBox chkSelectedKC = (CheckBox)row.FindControl("chkSelect");
                Label lblKCID = (Label)row.FindControl("lblKCID");
                Label lblKCTypeID = (Label)row.FindControl("lblKCTypeID");
                if (chkSelectedKC.Checked)
                {
                   // kc.Business_Plan_ID = BPID;
                    tblBusinessPlanKeyContacts kc = new tblBusinessPlanKeyContacts();
                    kc.BusinessPlan = context.BusinessPlanSet.FirstOrDefault(s => s.Business_Plan_ID == BPID);
                    kc.Key_Contact_ID = Convert.ToInt32(lblKCID.Text);
                    kc.Key_Contact_Type_ID = Convert.ToInt32(lblKCTypeID.Text);
                    context.AddTotblBusinessPlanKeyContactsSet(kc);
                    context.SaveChanges();
                }
            }
        }
    
        foreach (GridViewRow row in grvKeyContacts.Rows)
        {
            ((CheckBox)row.FindControl("chkSelect")).Enabled = false;
        }
        frmBPAccountSummary.ChangeMode(FormViewMode.ReadOnly);
        ToggleEditSaveButton(true);
        
    }
    # endregion
}
