using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Pinsonault.Data;
using System.Text;
using Telerik.Web.UI;

public partial class powerplanrx_controls_DistrictRegionBrandReportFilters : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        dsSegment.ConnectionString = Pinsonault.Web.Session.ClientDBConnectionString;
        dsBrand.ConnectionString = Pinsonault.Web.Session.ClientDBConnectionString;
        if (!Page.IsPostBack)
            // Fill the regions combo.
            LoadRegions();
    }
    protected void ddlRegion_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        LoadDistricts(e.Value);
    }

    protected void LoadRegions()
    {


        SqlConnection connection = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString);

        SqlDataAdapter adapter = new SqlDataAdapter("pprx.usp_GetRegionName", connection);
        DataTable dt = new DataTable();
        adapter.Fill(dt);

        ddlRegion.DataTextField = "Region_Name";
        ddlRegion.DataValueField = "Region_ID";
        ddlRegion.DataSource = dt;
        ddlRegion.DataBind();
        // Insert the first item.
        ddlRegion.Items.Insert(0, new RadComboBoxItem("-- Select a Region --"));
    }

    protected void LoadDistricts(String Region_ID)
    {

        if (ddlDistrict.SelectedValue == "0")
        {
            ddlDistrict.ClearSelection();
        }
        SqlConnection conn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString);

        //SqlDataAdapter adapter = new SqlDataAdapter("usp_GetDistrictName", connection);
        SqlCommand cmd = new SqlCommand("pprx.usp_GetDistrictName", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Region_ID", Region_ID);
        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        adapter.Fill(dt);

        ddlDistrict.DataTextField = "District_Name";
        ddlDistrict.DataValueField = "District_ID";
        ddlDistrict.DataSource = dt;
        ddlDistrict.DataBind();
        // Insert the first item.
        ddlDistrict.Items.Insert(0, new RadComboBoxItem("- Any District -"));
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string reportType = ddlReportType.SelectedValue;
        int brandID = ddlBrands.SelectedIndex;
        if (!string.IsNullOrEmpty(ddlBrands.SelectedValue))
            brandID = Convert.ToInt32(ddlBrands.SelectedValue);

        string segment = ddlSegment.SelectedValue;
       
        string retailRegionID = ddlRegion.SelectedValue;
        string districtID = ddlDistrict.SelectedValue;

        //reportviewerframe.Visible = true;
        //reportviewerframe.Attributes.Add("onLoad", "calcHeight()");
        //reportviewerframe.Attributes.Add("src", "ReportViewer.aspx?reportname=PPRX_Reports&report=DistrictRegionBrandReport&Type_ID=" + reportType + "&Brand_ID=" + brandID + "&Segment_ID=" + segment + "&Region_ID=" + retailRegionID + "&District_ID=" + districtID);

       // string redirectURL = "ReportViewer.aspx?reportname=PPRX_Reports&report=DistrictRegionBrandReport&Type_ID=" + reportType + "&Brand_ID=" + brandID + "&Segment_ID=" + segment + "&Region_ID=" + retailRegionID + "&District_ID=" + districtID;
       // Response.Redirect(redirectURL);

    }  

}