using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Pinsonault.Application.PowerPlanRx;
using System.Xml.Linq;
using Telerik.Web.UI;
using Pinsonault.Web;

public partial class createcampaign_step2_approval : System.Web.UI.Page
{
    int _CampaignID;
    protected override void OnInit(EventArgs e)
    {
        SqlDataSource1.ConnectionString = Pinsonault.Web.Session.ClientDBConnectionString;
        base.OnInit(e);
    }
    protected void Page_Load(object sender, EventArgs e)
    {        
        _CampaignID =  Convert.ToInt32(Request.QueryString["id"].ToString());
    }
    
   
    protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
    {        
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = e.Item as GridDataItem;
                bool canApprove = PPRXUser.CanApprove;

                if (dataItem["Territory_ID"].Text == Session["TerritoryID"].ToString() && canApprove)
                {
                    // Only show two buttons to AE or AD themselves  if they have not yet approved or denied the campaign

                    if (dataItem["Status"].Text != "Approved" && dataItem["Status"].Text != "Denied")
                    {
                        Button AVLbtn = (Button)dataItem.FindControl("Approval_Button");
                        AVLbtn.Visible = true;

                        Button dnbtn = (Button)dataItem.FindControl("Denial_Button");
                        dnbtn.Visible = true;
                    }

                    
                }

            }
        
    }


    protected void On_Approval_Button_Click(object sender, EventArgs e)
    {
        Update_Campaign_Approval_Status(_CampaignID, Session["TerritoryID"].ToString(), "Approved");
        RadGrid1.DataBind();
    }


    protected void On_Denial_Button_Click(object sender, EventArgs e)
    {
        Update_Campaign_Approval_Status(_CampaignID, Session["TerritoryID"].ToString(), "Denied");
        RadGrid1.DataBind();
    }


    public void Update_Campaign_Approval_Status(int cid, string tid, string status)
    {
        // Create Instance of Connection and Command Object
        SqlConnection myConnection = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString);

        SqlCommand myCommand = new SqlCommand("pprx.usp_Update_Team_Approval_Status", myConnection);

        // Mark the Command as a SPROC
        myCommand.CommandType = CommandType.StoredProcedure;

        // Add Parameters to SPROC

        SqlParameter parameter_campaign_id = new SqlParameter("@Campaign_ID", SqlDbType.Int, 4);
        parameter_campaign_id.Value = cid;
        myCommand.Parameters.Add(parameter_campaign_id);

        SqlParameter parameter_territory_id = new SqlParameter("@Territory_ID", SqlDbType.VarChar, 50);
        parameter_territory_id.Value = tid;
        myCommand.Parameters.Add(parameter_territory_id);

        SqlParameter parameter_Approval_Status = new SqlParameter("@Approval_Status", SqlDbType.VarChar, 50);
        parameter_Approval_Status.Value = status;
        myCommand.Parameters.Add(parameter_Approval_Status);

        myCommand.Parameters.AddWithValue("@userName", Pinsonault.Web.Session.FullName);

        try
        {
            myConnection.Open();
            myCommand.ExecuteNonQuery();
            myConnection.Close();

           
        }
        finally 
        {
            if (myConnection != null) myConnection.Dispose();
            if (myCommand != null) myCommand.Dispose();

        }
    }
}
