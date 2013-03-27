using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Configuration;
using Pinsonault.Data;
using System.Data;
using System.Text;
using Telerik.Web.UI;
using Pinsonault.Application.PowerPlanRx;
using Pinsonault.Web;

public partial class createcampaign_step2_tactics : System.Web.UI.Page, IEditPage
{

    int _phaseID;
    int _tacticsCount;
    int _campaignID;
    protected override void OnInit(EventArgs e)
    {
        dsTacticsAvailable.ConnectionString = Pinsonault.Web.Session.ClientDBConnectionString;
        dsTacticsSelected.ConnectionString = Pinsonault.Web.Session.ClientDBConnectionString;
        base.OnInit(e);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        _phaseID = ((MasterPage)this.Master).PhaseID;
        hPhaseID.Value = _phaseID.ToString();

        divMsg.InnerHtml = "";
        divMsg.Visible = false;

        //check if CampaignID or id is not present in the querystring, redirect it back to previous page.
        if (string.IsNullOrEmpty(Request.QueryString["id"]))
        {
            Response.Redirect(Request.ServerVariables["HTTP_REFERER"]);
        }
        else
        {
            _campaignID = Convert.ToInt32(Request.QueryString["id"]);
        }

        // previous step(_phaseID == 5 Team Setup) has been completed & now Tactics begin (page should be in Edit mode)
        if (!Page.IsPostBack)
        {
            GetCampaignName();

            if (((MasterPage)this.Master).IsPageEditable && ((MasterPage)Master).CanEdit)
            {
                pnlReadOnly.Visible = false;
                pnlEdit.Visible = true;
            }
        }

        divCampaignName.InnerHtml = "Campaign Name: " + ViewState["CampaignName"];
        divCampaignName.Visible = true;
    }

    private void GetCampaignName()
    {
        // to get Campaign Name
        using (SqlConnection cn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand("select Campaign_Name from pprx.campaign_mast where Campaign_ID = @Campaign_ID", cn))
            {
                cmd.Parameters.AddWithValue("@Campaign_ID", _campaignID);
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ViewState["CampaignName"] = dr.GetString(0);
                    }
                }
                cn.Close();
            }
        }
    }


    #region IEditPage
    public bool Save()
    {
        if (Campaign.IsCampaignActive(System.Convert.ToInt32(Request.QueryString["id"])))
        {
            // validate: at least 1 Tactic must be selected
            int rowCount = rgTactics.MasterTableView.GetItems(new GridItemType[] { GridItemType.Item, GridItemType.AlternatingItem }).Length;

            if (rowCount > 0)
            {
                // only if current Phase ID = 5, update Phase ID = 6
                using (SqlConnection cn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("update pprx.Campaign_Mast set Current_Step_ID = case when Current_Step_ID > " + Convert.ToString((int)PhaseID.Tactics) + " then Current_Step_ID else " + Convert.ToString((int)PhaseID.Tactics) + " end where Campaign_ID = @Campaign_ID", cn))
                    {
                        cmd.Parameters.AddWithValue("@Campaign_ID", Convert.ToInt32(Request.QueryString["id"]));
                        cn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                EditSelectedTactics();

                //rgTacticsReadOnly.DataBind();
                tactics.TacticGrid.DataBind();

                pnlEdit.Visible = false;
                pnlReadOnly.Visible = true;

                return true;
            }

            else  // at least 1 Tactic must be selected
            {
                divMsg.InnerHtml = "Please select at least 1 Tactic!";
                divMsg.Visible = true;
                return false;
            }

        }

        return true;

    }

    private void EditSelectedTactics()
    {
        foreach (GridDataItem item in rgTactics.MasterTableView.Items)
        {

            RadTextBox rtxtQty = (RadTextBox)item.FindControl("rtxtQtyEdit");
            int _qty = Convert.ToInt32(rtxtQty.Text);
            int _tacticsID = Convert.ToInt32(item.OwnerTableView.DataKeyValues[item.ItemIndex]["Tactic_ID"]);

            UpdateQty(_qty, _tacticsID, Pinsonault.Web.Session.FullName);

        }
    }


    public void Reset()
    {
        rgTactics.DataBind();
        rgTacticsAdd.DataBind();

        tactics.TacticGrid.DataBind();
        pnlEdit.Visible = false;
        pnlReadOnly.Visible = true;
    }

    public void Edit()
    {
        pnlEdit.Visible = true;
        pnlReadOnly.Visible = false;
    }

    #endregion


    private void UpdateQty(int iQty, int iTacticsID, string strPinsoUser)
    {
        // update the Qty that is in the selected list
        using (SqlConnection cn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand("pprx.usp_Campaign_UpdateTactics", cn))
            {
                cmd.Parameters.AddWithValue("@Campaign_ID", _campaignID);
                cmd.Parameters.AddWithValue("@Tactic_ID", iTacticsID);
                cmd.Parameters.AddWithValue("@Quantity", iQty);
                cmd.Parameters.AddWithValue("@Modified_By", strPinsoUser);
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }


    /// <summary>
    /// Validation needed:
    /// After 1st time Submit Phase ID has already been changed to 6 in the 'Campaign_Info' table
    /// If Campaign Phase ID >=6  Then at least 1 tactic must be in the 'Campaign_Tactic_Details' table
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgTactics_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        _tacticsCount = SelectedTacticsCount(_campaignID);

        if (_phaseID >= (int)PhaseID.Tactics)  // means at least 1 Tactic is already saved (validation needed: can't remove all tactics)
        {

            if (_tacticsCount > 1)   // if _tacticsCount = 1: can't remove the only 1 left selected tactic
            {
                DeleteIt(e);
            }
        }
        else    // delete last row available anytime if it 's before the 1st time submit
        {
            DeleteIt(e);
        }

    }

    private void DeleteIt(GridCommandEventArgs e)
    {
        int _tacticsID;
        _tacticsID = Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Tactic_ID"].ToString());

        using (SqlConnection cn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand("pprx.usp_Campaign_Tactics_SelectedList_Delete", cn))
            {
                cmd.Parameters.AddWithValue("@Campaign_ID", _campaignID);
                cmd.Parameters.AddWithValue("@Tactic_ID", _tacticsID);
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        rgTacticsAdd.DataBind();
        rgTactics.DataBind();
        pnlEdit.Visible = true;
        pnlReadOnly.Visible = false;
    }


    public static int SelectedTacticsCount(int _campaignID)
    {
        int _count = 0;
        // to count # of Tactics in the selected list
        using (SqlConnection cn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand("select count(*) from pprx.Campaign_Tactics where Campaign_ID = @Campaign_ID", cn))
            {
                cmd.Parameters.AddWithValue("@Campaign_ID", _campaignID);
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        _count = dr.GetInt32(0);
                    }
                }
                cn.Close();

            }
        }

        return _count;
    }

    protected void rgTactics_CancelCommand(object sender, EventArgs e)
    {


    }


    protected void EditQty(object sender, EventArgs e)
    {
        EditSelectedTactics();

    }

    protected void AddTactics(object sender, EventArgs e)
    {

        foreach (GridDataItem item in rgTacticsAdd.MasterTableView.Items)
        {

            RadTextBox qty = (RadTextBox)item.FindControl("rtxtQtyAdd");

            if (qty.Text.Trim() != null && qty.Text.Trim() != "" && Convert.ToInt32(qty.Text) > 0)
            {

                int _tacticsID = Convert.ToInt32(item.OwnerTableView.DataKeyValues[item.ItemIndex]["Tactic_ID"]);
                int iQty = Convert.ToInt32(qty.Text);

                using (SqlConnection cn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("pprx.usp_Campaign_UpdateTactics", cn))
                    {
                        cmd.Parameters.AddWithValue("@Campaign_ID", _campaignID);
                        cmd.Parameters.AddWithValue("@Tactic_ID", _tacticsID);
                        cmd.Parameters.AddWithValue("@Quantity", iQty);
                        cmd.Parameters.AddWithValue("@Modified_By", Pinsonault.Web.Session.FullName);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        rgTacticsAdd.DataBind();
        rgTactics.DataBind();
        pnlEdit.Visible = true;
        pnlReadOnly.Visible = false;


    }
}

