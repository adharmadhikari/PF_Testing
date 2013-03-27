using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using PathfinderClientModel;
using Pinsonault.Web;
using System.IO;
using Telerik.Web.UI;

public partial class standardreports_controls_TierCoverageDataTemplate : System.Web.UI.UserControl
{
    //public override Telerik.Web.UI.RadGrid HostedGrid
    public RadGrid HostedGrid
    {  
        get { return gridtiercoverage; }
    }

    public string GridLabel
    {
        get { return geoName.Text; }
        set { geoName.Text = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        string val = HttpContext.Current.Request.QueryString["Section_ID"];
        if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["Section_ID"]))
        {
            string[] clientdatakeys = { "Geography_ID", "Section_ID", "Drug_ID" };
            gridtiercoverage.MasterTableView.ClientDataKeyNames = clientdatakeys;
            gridtiercoverage.Rebind();

        }
        GridColumn col = gridtiercoverage.MasterTableView.Columns.FindByUniqueNameSafe("Formulary_Lives");

        switch (val)
        {
            case "17":
                col.HeaderText = "Medicare Part D Lives";
                break;
            case "1":
                col.HeaderText = "Commercial Pharmacy Lives";
                break;
            case "6":
                col.HeaderText = "Managed Medicaid Lives";
                break;
        }
    }

}
