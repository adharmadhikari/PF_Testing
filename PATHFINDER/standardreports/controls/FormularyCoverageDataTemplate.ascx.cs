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

public partial class standardreports_controls_FormularyCoverageDataTemplate : System.Web.UI.UserControl
{
    //public override Telerik.Web.UI.RadGrid HostedGrid
    public RadGrid HostedGrid
    {
        get { return gridformularycoverage; }
    }

    public string GridLabel
    {
        get { return geoName.Text; }
        set { geoName.Text = value; }
    }
    //protected void Page_Load(object sender, EventArgs e)
    //{
    //    if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["Section_ID"]))
    //    {
    //        string[] clientdatakeys = { "Geography_ID", "Section_ID", "Drug_ID" };
    //        gridformularycoverage.MasterTableView.ClientDataKeyNames = clientdatakeys;
    //        gridformularycoverage.Rebind();

    //    }
    //}

    protected override void OnLoad(EventArgs e)
    {
        if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["Section_ID"]))
        {
            string[] clientdatakeys = { "Geography_ID", "Section_ID", "Drug_ID" };
            gridformularycoverage.MasterTableView.ClientDataKeyNames = clientdatakeys;
            gridformularycoverage.Rebind();

        }

        string val = Request.QueryString["Section_ID"];
        GridColumn col = gridformularycoverage.MasterTableView.Columns.FindByUniqueNameSafe("Formulary_Lives");
     
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
            //case "4":
            //    col.HeaderText = "Lives";
            //    break;                           
        }
        
        
        
        base.OnLoad(e);
    }
}
