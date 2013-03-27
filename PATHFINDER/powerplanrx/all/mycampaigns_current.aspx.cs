using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using Pinsonault.Application;

public partial class mycampaigns_current : System.Web.UI.Page
{
    
    /// <summary>
    /// Show the AE Name column if user is not an AE
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        //object titleID = Session["titleID"];
        //if ((int)titleID != 13) 
        //gridMyCampaigns.Columns[0].Visible = true;   
        dsMyCampaigns.ConnectionString = Pinsonault.Web.Session.ClientDBConnectionString;
    }


    protected override void OnInit(EventArgs e)
    {
        //change command for SAE
        //if(Impact.User.IsSAE)
        //    dsMyCampaigns.SelectCommand = "usp_GetCampaignInfo_SAE_By_Novo_User_ID";

        base.OnInit(e);
    }

    /// <summary>
    /// Parameters to send to the proc to get My Campaigns
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void OnSelecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        ((SqlParameterCollection)e.Command.Parameters).AddWithValue("@TerritoryID", Pinsonault.Web.Session.TerritoryID);
    }
}
