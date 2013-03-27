using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Pinsonault.Application.PowerPlanRx;
//using Impact.Utility;
using System.Net.Mail;
using System.Collections.Generic;

public partial class Campaign_Rationale : System.Web.UI.Page
{
    protected override void OnInit(EventArgs e)
    {
        dsPlanBrandInfo.ConnectionString = Pinsonault.Web.Session.ClientDBConnectionString;
        dsRationale.ConnectionString = Pinsonault.Web.Session.ClientDBConnectionString;
        base.OnInit(e);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        //check if Plan_ID and Band_ID is not present in the querystring, redirect it back to the previous page.
        if ((string.IsNullOrEmpty(Request.QueryString["Plan_ID"]) || string.IsNullOrEmpty(Request.QueryString["Brand_ID"])) && (string.IsNullOrEmpty(Request.QueryString["id"])))
        {
            Response.Redirect(Request.ServerVariables["HTTP_REFERER"]);
        }
        //Check if the user can change the rationale or not             
        //bool bIsUserAE = UserTitle.CurrentUserIsAccountExecutive;
        bool bIsUserAE = User.IsInRole("pprx_CVA");
        bool bActiveCampaign = true;

        pnlAlert.Visible = false;
        pnlRationale.Visible = true;

        //if CampaignID is not present in the querystring i.e. a new request for creating campaign and button should be enabled if user is AE
        if (!string.IsNullOrEmpty(Request.QueryString["id"]))
        {
            int iCampaignID = Convert.ToInt32(Request.QueryString["id"]);
            ViewState.Add("CampaignID", iCampaignID);
            bActiveCampaign = Campaign.IsCampaignActive(System.Convert.ToInt32(Request.QueryString["id"]));
        }
        else
        {
            //check if campaign exists
            int iCampaignID = Campaign.GetExistingActiveCampaignID(Request.QueryString["Plan_ID"].ToString(), Convert.ToInt32(Request.QueryString["Brand_ID"]), Session["territoryID"].ToString());
            if (iCampaignID > 0)
            {
                //Show alert
                ViewState.Add("CampaignID", iCampaignID);
                pnlAlert.Visible = true;
                pnlRationale.Visible = false;
            }
        }
        pnlRationale.Enabled = (bIsUserAE && bActiveCampaign); 
        
    }

    protected void OnSubmit(object sender, EventArgs e)
    {
        Save();
    }

    protected void OnReset(object sender, EventArgs e)
    {
        Reset();
    }
    /// <summary>
    /// for redirecting the user to Campaign profile step 1 page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void Save()
    {
        try
        {
            string strTerritoryID = Session["territoryID"].ToString();
            string strUserID = Session["UserID"].ToString();
            string strRationals = "";
            int iCampaign_ID;
            IList<Int32> arRationale = new List<Int32>();

            foreach (DataListItem item in dlrationale.Items)
            {
                CheckBox chkRationale = (CheckBox)item.FindControl("chkRationale");
                CheckBox chkRationaleID = (CheckBox)item.FindControl("chkRationaleID");

                if (chkRationale.Checked)
                {
                    strRationals += chkRationale.Text + ",";
                    arRationale.Add(Convert.ToInt32(chkRationaleID.Text));
                }
            }
            if (strRationals != "")
            {
                strRationals = strRationals.TrimEnd(',');
                //check if CampaignID is not present in query string, create a campaign
                if (string.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    string strPlanID = Request.QueryString["Plan_ID"];
                    int iBrandID = System.Convert.ToInt32(Request.QueryString["Brand_ID"]);
                    int iSegmentID = System.Convert.ToInt32(Request.QueryString["SegmentID"]);
                    //insert the campaign info and create a campaign
                    iCampaign_ID = Campaign.CreateCampaign(strPlanID, iBrandID, strTerritoryID, strUserID, iSegmentID);

                    Campaign.DeleteCampaignRationalsByCampaignID(iCampaign_ID); //delete all rationales for campaign
                    //now insert the selected rationales
                    foreach (int rationale in arRationale)
                        Campaign.InsertCampaignRationale(iCampaign_ID, rationale);

                    //archive campaign data for opportunity assessment

                    //TODO: review if this step is required or not
                    //Campaign.InsertCampaignOpportunityData(iCampaign_ID, Convert.ToInt32(Request.QueryString["SegmentID"]), Session["territoryID"].ToString(), Request.QueryString["BrandList"].ToString(), Session["fullName"].ToString());
                }
                //CampaignID is present in querystring, update the Campaign Rationale
                else
                {
                    iCampaign_ID = System.Convert.ToInt32(Request.QueryString["id"].ToString());
                    if (Campaign.IsCampaignActive(iCampaign_ID))
                    {
                        //delete all rationales for campaign
                        Campaign.DeleteCampaignRationalsByCampaignID(iCampaign_ID); 
                        //now insert the selected rationales
                        foreach (int rationale in arRationale)
                            Campaign.InsertCampaignRationale(iCampaign_ID, rationale);

                        //update modification details in campaign mast
                        if (Campaign.UpdateCampaignModifiedDtByCampaign(iCampaign_ID, strUserID) == 0)
                            iCampaign_ID = 0;
                    }
                }
                if (iCampaign_ID > 0)
                {
                    //Response.Redirect("createcampaign_step1_profile.aspx?id=" + iCampaign_ID);
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "redirect", string.Format("window.parent.parent.location='createcampaign_step1_profile.aspx?id={0}';", iCampaign_ID), true);
                }
                else
                {
                    lblErrorMessage.Visible = true;
                }
            }
            else
            {
                lblReqdRationals.Visible = true;
            }
        }
        catch
        {
            lblErrorMessage.Visible = true;
        }


    }
    /// <summary>
    /// for resetting the page values
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void Reset()
    {
        dlrationale.DataBind();
    }
    protected void NotifyAdmin(object sender, EventArgs e)
    {
        divButtons.Visible = false;

        try
        {
            string strToMailAddress = "";
            if (ConfigurationManager.AppSettings["SendEmail"] == "true")
                strToMailAddress = Utility.GetAdminEmail();
            else
                strToMailAddress = ConfigurationManager.AppSettings["CustomerSupportEmail"];

            string strFromEmailAddress = Utility.GetUserEmail((int)Session["UserID"]);

            string strEmailSubject = Resources.Resource.EMailSubject_CampaignCreateRequest;
            string strEmailBody = Resources.Resource.EMailBody_CampaignCreateRequest;

            strEmailBody = strEmailBody + System.Environment.NewLine + "CampaignID = " + ViewState["CampaignID"] + System.Environment.NewLine +
                        Resources.Resource.EMailSignature_CampaignCreateRequest + System.Environment.NewLine + Session["FirstName"].ToString();

            Utility.SendEMail(strToMailAddress, strFromEmailAddress, strEmailSubject, strEmailBody, MailPriority.High);

            lblMessage.Text = "Your request has been processed successfully.";
        }
        catch
        {
            lblMessage.Text = "Your request could not process this time. Please try later.";
        }
    }
    protected void UpdateCampaign(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Request.QueryString["id"]))
        {
            Response.Redirect(Request.Url.ToString() + "&id=" + ViewState["CampaignID"]);
        }
    }
}
