using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlTypes;
//using Impact.Campaign;
using Telerik.Web.UI;
using Pinsonault.Application.PowerPlanRx;

public partial class createcampaign_step1_profile : System.Web.UI.Page, IEditPage
{
    protected override void OnInit(EventArgs e)
    {
        dsCampaignInfo.ConnectionString = Pinsonault.Web.Session.ClientDBConnectionString;
        dsCampaignProductFormulary.ConnectionString = Pinsonault.Web.Session.ClientDBConnectionString;
        base.OnInit(e);
    }
    protected void Page_Load(object sender, EventArgs e)
    {        
        //check if CampaignID or id is not present in the querystring, redirect it back to previous page.
        if (string.IsNullOrEmpty(Request.QueryString["id"]))
        {
            Response.Redirect(Request.ServerVariables["HTTP_REFERER"]);
        }

        if (!Page.IsPostBack)
        {
            if (((MasterPage)this.Master).IsPageEditable)
            {
                pnlReadonly.Visible = false;
                pnlEdit.Visible = true;
            }
        }  
       
    }

    
    /// <summary>
    /// This function redirects the user to Campaign Rational selection page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtnRational_Click(object sender, EventArgs e)
    {
        string strPlanID = ((Label)frmPlanInfo.FindControl("lblPlanID")).Text;
        string strBrandID = ((Label)frmPlanInfo.FindControl("lblBrandID")).Text;
        string strRedirectURL = "Campaign_Rationale.aspx?Plan_ID=" + strPlanID + "&Brand_ID=" + strBrandID + "&id=" + Request.QueryString["id"].ToString();

        Response.Redirect(strRedirectURL);
    }


    /// <summary>
    /// For updating the campaign plan profile and Product formulary details
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public bool Save()
    {
        if (Campaign.IsCampaignActive(System.Convert.ToInt32(Request.QueryString["id"])))
        {
            UpdateCampaignPlanInfo();
            frmPlanInfoReadOnly.DataBind();
            dlProdInfoReadOnly.DataBind();
            pnlEdit.Visible = false;
            pnlReadonly.Visible = true;
        }
        return true;
    }
    /// <summary>
    /// resets the control values from datbase
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void Reset()
    {
        frmCampaignInfo.DataBind();
        frmPlanInfo.DataBind();
        pnlReadonly.Visible = true;
        pnlEdit.Visible = false;
    }

    /// <summary>
    /// For editing the campaign
    /// </summary>
    public void Edit()
    {
        pnlReadonly.Visible = false;
        pnlEdit.Visible = true;
    }

    /// <summary>
    /// For updating the campaign plan info
    /// </summary>
    private void UpdateCampaignPlanInfo()
    {
        int iCampaignID = System.Convert.ToInt32(Request.QueryString["id"]);

        bool bFormulary_Change_Status = ((RadioButton)frmPlanInfo.FindControl("radFChangeYes")).Checked;
        System.Data.SqlTypes.SqlDateTime dtFormulary_Change_Eff_Date = SqlDateTime.Null;
        string strEffDate = ((TextBox)frmPlanInfo.FindControl("txtEffectiveDate")).Text;
        if (strEffDate != "")
        { dtFormulary_Change_Eff_Date = System.Convert.ToDateTime(strEffDate); }

        bool bPlan_Participation_PT = ((RadioButton)frmPlanInfo.FindControl("radParticipationYes")).Checked;
        string strPlan_Penetrate_Region = ((TextBox)frmPlanInfo.FindControl("txtPlanPenetration")).Text;
        string strContract_Share_Goal = ((TextBox)frmPlanInfo.FindControl("txtContractShareGoal")).Text;

        string strKey_Employers = ((TextBox)frmPlanInfo.FindControl("txtKeyEmployers")).Text;
        string strAffiliated_Phys_Groups = ((TextBox)frmPlanInfo.FindControl("txtAffPhyGroups")).Text;

        string strOther_Facts1 = ((TextBox)frmPlanInfo.FindControl("txtKEOtherFacts")).Text;
        string strOther_Facts2 = ((TextBox)frmPlanInfo.FindControl("txtAffPhyGroupOtherFacts")).Text;
        string strOther_Facts3 = ((TextBox)frmPlanInfo.FindControl("txtNationalAccAffOtherfacts")).Text;
        string strOther_Facts4 = ((TextBox)frmPlanInfo.FindControl("txtPBMAffOtherFacts")).Text;
        int strPinsoUserID = Pinsonault.Web.Session.UserID;

        //update campaign plan info
        Campaign.UpdateCampaignPlanInfo(iCampaignID, bFormulary_Change_Status, dtFormulary_Change_Eff_Date,
            bPlan_Participation_PT, strKey_Employers, strAffiliated_Phys_Groups, strPlan_Penetrate_Region, strContract_Share_Goal, strOther_Facts1, strOther_Facts2, strOther_Facts3, strOther_Facts4, strPinsoUserID);

    }
}
