using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using PathfinderClientModel;
using Pinsonault.Web;
using Telerik.Web.UI;
using System.Data.SqlClient;
using System.Data;


public partial class custom_alcon_activityreporting_all_activityentry : InputFormBase
{
    protected override void OnInit(EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["ActivityDate"]))
            txtActivityDate.Text = Request.QueryString["ActivityDate"];
        else
            txtActivityDate.Text = DateTime.Today.ToShortDateString();

        dsActivity.ConnectionString = Pinsonault.Web.Session.ClientDBConnectionString;
        

        base.OnInit(e);
    }

    protected override bool IsRequestValid()
    {
        return true;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, gvActivityEntry.ClientID);

        if (!IsPostBack)
        {
            dsActivity.SelectParameters.Clear();
            dsActivity.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;
            dsActivity.SelectCommand = "usp_DailyActivity_Select";

            dsActivity.SelectParameters.Add("UserID", TypeCode.Int32, Pinsonault.Web.Session.UserID.ToString());
            dsActivity.SelectParameters.Add("ActivityDate", TypeCode.DateTime, txtActivityDate.Text); 
            dsActivity.SelectParameters[0].Direction = ParameterDirection.Input;							

            gvActivityEntry.DataSourceID = "dsActivity";
            gvActivityEntry.DataBind();

        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {

        foreach (Telerik.Web.UI.GridDataItem item in gvActivityEntry.Items)
        {
         
            HiddenField hID = (HiddenField)item["un_ActivityHours"].FindControl("hTypeID");
            int _typeID = Convert.ToInt32(Request.Form[hID.UniqueID]);

            TextBox t = (TextBox) item["un_ActivityHours"].FindControl("txtHours");

            int _hours = 0;
            if (!string.IsNullOrEmpty(Request.Form[t.UniqueID]))
                _hours = Convert.ToInt32(Request.Form[t.UniqueID]);

            updateActivityHours(_typeID, _hours);
        }

        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "_resetPage", "showGrid(null)", true);
        PostBackResult.Success = true;
    }

    private void updateActivityHours(int TypeID, int Hours)
    {
        try
        {
            using (SqlConnection cn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_DailyActivity_Insert_Update", cn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UserID", Pinsonault.Web.Session.UserID);
                    cmd.Parameters.AddWithValue("@ActivityDate", Convert.ToDateTime(txtActivityDate.Text));  
                    cmd.Parameters.AddWithValue("@ActivityTypeID", TypeID);
                    cmd.Parameters.AddWithValue("@ActivityHours", Hours);

                    cn.Open();
                    cmd.ExecuteNonQuery();
                    
                    
                }
            }
            
        }
        catch
        {
            PostBackResult.Success = false;
        }
    }

}
