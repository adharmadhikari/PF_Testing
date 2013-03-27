using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using Pinsonault.Application.PowerPlanRx;
using Pinsonault.Web;
using System.Net.Mail;
using Telerik.Web.UI;

public partial class createcampaign_step1_team : System.Web.UI.Page, IEditPage
{
    protected override void OnInit(EventArgs e)
    {
        dsCampaignTeam.ConnectionString = Pinsonault.Web.Session.ClientDBConnectionString;
        dsAdHocCampaignTeam.ConnectionString = Pinsonault.Web.Session.ClientDBConnectionString;
        dsFunctionArea.ConnectionString = Pinsonault.Web.Session.ClientDBConnectionString;
        dsEmpNames.ConnectionString = Pinsonault.Web.Session.ClientDBConnectionString;
        base.OnInit(e);
    }
    /// <summary>
    /// Check if the user is the AE for the campaign, if so make Campaign Team email button visible and show the Edit button
    /// Initialize the alerts as not visible 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        bool canEdit = ((MasterPage)this.Master).CanEdit;

        btnEmailTeam.Visible = canEdit;
        pnlTeamAlert.Visible = false;
        
        //pnlAdHocAlert.Visible = false;
        //if (Page.IsPostBack && canEdit)
        if (canEdit)
        {
            //Check user role then allow Edit.
            if (PPRXUser.CanCreate)
                Edit();
        }
        else
        {
            frmCampaignTeam.Visible = false;
            frmAdHoc.Visible = false;
        }
    }

    /// <summary>
    /// Used to add a new campaign team member from the drop down lists which show the RBD, RSM and RFT and the employee names
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    
    protected void btnAddToTeam_Click(object sender, EventArgs e)
    {
        int functionSelected = ((RadComboBox)frmCampaignTeam.FindControl("drpFunctionArea")).SelectedIndex;
        //string territoryID = ((RadComboBox)frmCampaignTeam.FindControl("drpEmpName")).SelectedValue;
        String User_FullName = ((RadComboBox)frmCampaignTeam.FindControl("drpEmpName")).Text;
        String territoryID = Campaign.GetUserTerritory(User_FullName);
        int campaignID = System.Convert.ToInt32(Request.QueryString["id"]);
        int userID = 0;

        if(!String.IsNullOrEmpty(territoryID))
            userID = Convert.ToInt32(territoryID);

        if ((functionSelected == 0) || (territoryID == "-- Select an Employee Name --"))
        {
            pnlTeamAlert.Visible = true;
            //pnlAdHocAlert.Visible = false;
            ((RadComboBox)frmAdHoc.FindControl("drpAdHocFunctionArea")).SelectedIndex = 0;
            ((TextBox)frmAdHoc.FindControl("txtName")).Text = "";
            ((TextBox)frmAdHoc.FindControl("txtEmail")).Text = "";
            ((TextBox)frmAdHoc.FindControl("txtPhone")).Text = "";
        }
        else
        {
            Campaign.UpdateCampaignTeam(campaignID, territoryID, userID);
            ((RadComboBox)frmCampaignTeam.FindControl("drpFunctionArea")).SelectedIndex = 0;
            ((RadComboBox)frmCampaignTeam.FindControl("drpEmpName")).SelectedIndex = 0;
            pnlTeamAlert.Visible = false;
            grvCampaignTeam.DataBind();
        }
    }

    /// <summary>
    /// Used to add an Ad Hoc campaign team member that the user will manually enter
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    
    protected void btnAddAdHoc_Click(object sender, EventArgs e)
    {
        string adHocTitleID = ((RadComboBox)frmAdHoc.FindControl("drpAdHocFunctionArea")).SelectedItem.Value;
        int titleID = System.Convert.ToInt32(adHocTitleID);
        int campaignID = System.Convert.ToInt32(Request.QueryString["id"]);
        string adHocName = ((TextBox)frmAdHoc.FindControl("txtName")).Text;
        string adHocEmail = ((TextBox)frmAdHoc.FindControl("txtEmail")).Text;
        string adHocPhone = ((TextBox)frmAdHoc.FindControl("txtPhone")).Text;
        int userID = Pinsonault.Web.Session.UserID;

        if ((titleID == 0) || (adHocName == " ") || (adHocEmail == " ") || (adHocPhone == " "))
        {
            //pnlAdHocAlert.Visible = true;
            pnlTeamAlert.Visible = false;
            ((RadComboBox)frmCampaignTeam.FindControl("drpFunctionArea")).SelectedIndex = 0;
            ((RadComboBox)frmCampaignTeam.FindControl("drpEmpName")).SelectedIndex = 0;
        }
        else
        {
            Campaign.UpdateAdHocCampaignTeam(campaignID, adHocName, titleID, adHocEmail, adHocPhone, userID);
            ((RadComboBox)frmAdHoc.FindControl("drpAdHocFunctionArea")).SelectedIndex = 0;

            //pnlAdHocAlert.Visible = false;
            grvCampaignTeam.DataBind();
            grvAdHocCampaign.DataBind();
        }

        if (((TextBox)frmAdHoc.FindControl("txtName")) != null)
        {
            ((TextBox)frmAdHoc.FindControl("txtName")).Text = "";
            ((TextBox)frmAdHoc.FindControl("txtEmail")).Text = "";
            ((TextBox)frmAdHoc.FindControl("txtPhone")).Text = "";
        }

    }

    /// <summary>
    /// Used to populate the employee name drop down according to the function area selected
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void OnFunctionAreaChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        RadComboBox funcarealist = frmCampaignTeam.FindControl("drpFunctionArea") as RadComboBox;
        funcarealist.Text = e.Text;
        funcarealist.SelectedValue = e.Value;
    }

   
    /// <summary>
    /// Redirects to the page to create and send an email for a campaign team meeting
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
   
    protected void btnEmailTeam_Click(object sender, EventArgs e)
    {
        string redirectURL = "email_campaign_team.aspx?id=" + Request.QueryString["id"];

        Response.Redirect(redirectURL);
    }

    protected override void OnPreRenderComplete(EventArgs e)
    {
        base.OnPreRenderComplete(e);
    }
    
    /// <summary>
    /// Used to hide the delete button if the member is an AE, AD or DBM
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
 
    //protected void grvCampaignTeam_RowDataBound(Object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        if (e.Row.Cells[5].Text == "13" || e.Row.Cells[5].Text == "16" || e.Row.Cells[5].Text == "20")
    //        {
    //            e.Row.Cells[4].Text = " ";
    //        }            
    //    }
    //}


    #region IEditPage Members

    public bool Save()
    { 
        if ((grvCampaignTeam.Rows.Count > 0) || (grvAdHocCampaign.Rows.Count > 0))
        {
            // if 1st time Submit, update the Phase ID = 5;  else keep the Phase ID 
            using (SqlConnection cn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("update pprx.Campaign_Mast set Current_Step_ID = case when Current_Step_ID > " + Convert.ToString((int)PhaseID.Team_Setup ) + " then Current_Step_ID else " + Convert.ToString((int)PhaseID.Team_Setup) + " end where Campaign_ID = @Campaign_ID", cn))
                {
                    cmd.Parameters.AddWithValue("@Campaign_ID", Convert.ToInt32(Request.QueryString["id"]));
                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
       
        grvCampaignTeam.DataBind();
        grvAdHocCampaign.DataBind();
        return true;
    }

    public void Reset()
    {
        pnlTeamAlert.Visible = false;
        //frmCampaignTeam.Visible = false;
        //frmAdHoc.Visible = false;

        frmCampaignTeam.Visible = true;
        frmAdHoc.Visible = true;
        dsEmpNames.SelectParameters.Clear();

        if (((RadComboBox)frmCampaignTeam.FindControl("drpFunctionArea")) != null)
        {
            ControlParameter cParam = new ControlParameter();
            cParam.Name = "Title_ID";
            cParam.ControlID = ((RadComboBox)frmCampaignTeam.FindControl("drpFunctionArea")).UniqueID;
            cParam.PropertyName = "SelectedValue";
            cParam.Type = TypeCode.Int32;
            cParam.DefaultValue = "0";

            QueryStringParameter qParam = new QueryStringParameter();
            qParam.Name = "Campaign_ID";
            qParam.QueryStringField = "id";

            // Add it to your SelectParameters collection
            dsEmpNames.SelectParameters.Add(cParam);
            dsEmpNames.SelectParameters.Add(qParam);

            dsEmpNames.SelectCommand = "usp_GetEmployeeName";
        }

        grvCampaignTeam.DataBind();
        grvCampaignTeam.Columns[5].Visible = false;
        grvAdHocCampaign.Columns[6].Visible = false;
        if (((RadComboBox)frmCampaignTeam.FindControl("drpFunctionArea")) != null)
        {
            ((RadComboBox)frmCampaignTeam.FindControl("drpFunctionArea")).SelectedIndex = 0;
            ((RadComboBox)frmCampaignTeam.FindControl("drpEmpName")).SelectedIndex = 0;
            ((RadComboBox)frmAdHoc.FindControl("drpAdHocFunctionArea")).SelectedIndex = 0;
        }

        if (((TextBox)frmAdHoc.FindControl("txtName")) != null)
        {
            ((TextBox)frmAdHoc.FindControl("txtName")).Text = "";
            ((TextBox)frmAdHoc.FindControl("txtEmail")).Text = "";
            ((TextBox)frmAdHoc.FindControl("txtPhone")).Text = "";
        }
    }

    public void Edit()
    {
        //pnlAdHocAlert.Visible = false;
        pnlTeamAlert.Visible = false;
        frmCampaignTeam.Visible = true;
        frmAdHoc.Visible = true;
        dsEmpNames.SelectParameters.Clear();

        if (((RadComboBox)frmCampaignTeam.FindControl("drpFunctionArea")) != null)
        {
            ControlParameter cParam = new ControlParameter();
            cParam.Name = "Title_ID";
            cParam.ControlID = ((RadComboBox)frmCampaignTeam.FindControl("drpFunctionArea")).UniqueID;
            cParam.PropertyName = "SelectedValue";
            cParam.Type = TypeCode.Int32;
            cParam.DefaultValue = "0";

            QueryStringParameter qParam = new QueryStringParameter();
            qParam.Name = "Campaign_ID";
            qParam.QueryStringField = "id";

            // Add it to your SelectParameters collection
            dsEmpNames.SelectParameters.Add(cParam);
            dsEmpNames.SelectParameters.Add(qParam);

            dsEmpNames.SelectCommand = "pprx.usp_GetEmployeeName";
        }

        //grvCampaignTeam.DataBind();
        //((RadComboBox)frmCampaignTeam.FindControl("drpFunctionArea")).SelectedIndex = 0;
        //((RadComboBox)frmCampaignTeam.FindControl("drpEmpName")).SelectedIndex = 0;
        //((RadComboBox)frmAdHoc.FindControl("drpAdHocFunctionArea")).SelectedIndex = 0;
        
        grvCampaignTeam.Columns[5].Visible = true;
        grvAdHocCampaign.Columns[6].Visible = true;
       
    }

    #endregion

   
}
