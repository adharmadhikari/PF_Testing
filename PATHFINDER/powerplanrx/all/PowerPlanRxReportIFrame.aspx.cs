using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Pinsonault.Data;
using System.Text;
using Telerik.Web.UI;

public partial class PowerPlanRxReport : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Request.QueryString["brandid"].ToString() == "0")
        //{
        //    pnlResult.Visible = false;
        //}
        //else
        //{
        //    pnlResult.Visible = true;
        //}

        ////give the export page url        
        //hdnQString.Value = string.Format("Export.aspx{0}", Request.Url.Query);
    }

    protected override void OnPreRender(EventArgs e)
    {
        //reset selected controls to previous value manually since we are not using proper postback method
        if (!Page.IsPostBack)
        {
            string val;
            
            val = Request["reporttype"];
            if (!string.IsNullOrEmpty(val)) ddlReportType.SelectedValue = val;
            val = Request["segment"];
            if (!string.IsNullOrEmpty(val)) ddlNationalAccount.SelectedValue = val;
            val = Request["dist"];
            if (!string.IsNullOrEmpty(val)) ddlDistrict.SelectedValue = val;
            val = Request["regionid"];
            if (!string.IsNullOrEmpty(val)) ddlRegion.SelectedValue = val;
            val = Request["brandid"];
            if (!string.IsNullOrEmpty(val)) ddlBrands.SelectedValue = val;

        }
        base.OnPreRender(e);
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string reportType = ddlReportType.SelectedValue;
        int brandID = ddlBrands.SelectedIndex;
        if (!string.IsNullOrEmpty(ddlBrands.SelectedValue))
            brandID = Convert.ToInt32(ddlBrands.SelectedValue);

        string segment = ddlNationalAccount.SelectedValue;
       
        string retailRegionID = ddlRegion.SelectedValue;
        string districtID = ddlDistrict.SelectedValue;
       
        reportviewerframe.Visible = true;
        reportviewerframe.Attributes.Add("onLoad", "calcHeight()");
        reportviewerframe.Attributes.Add("src", "ReportViewer.aspx?reportname=PPRX_Reports&report=DistrictRegionBrandReport&Type_ID=" + "1" + "&Brand_ID=" + "7" + "&Segment_ID=" + "1" + "&Region_ID=" + "32400000" + "&District_ID=" + "0");

    }  


    protected void OnRegionChanged(Object sender, EventArgs e)
    {
        RadComboBox distlist = ddlDistrict as RadComboBox;
        distlist.Items.Clear();
        RadComboBoxItem distname = new RadComboBoxItem();
        distname.Text = "-- Any District --";
        distname.Value = "0";
        distlist.Items.Add(distname);
    }

    protected void OnDistrictChanged(Object sender, EventArgs e)
    {
        RadComboBox terrlist = ddlTerritory as RadComboBox;
        terrlist.Items.Clear();
        RadComboBoxItem terrname = new RadComboBoxItem();
        terrname.Text = "-- Any Territory --";
        terrname.Value = "0";
        terrlist.Items.Add(terrname);
    }

}
