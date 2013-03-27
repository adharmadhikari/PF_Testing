using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class controls_BenefitDesignDoD : System.Web.UI.UserControl
{
    public string ContainerID { get; set; }
    public string OnClientRowSelected { get; set; }
    public bool AllowSorting { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        Pinsonault.Web.Support.RegisterComponentWithClientManager(this.Page, gridDoDBenefitDesg.ClientID, "{}", ContainerID);
        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "_dodbdpagevars", string.Format("var gridDoDBenefitDesgID = '{0}'; var gridMedDBenefitDesignID = {1}", gridDoDBenefitDesg.ClientID, null), true);

        gridDoDBenefitDesg.AllowSorting = AllowSorting;
        gridDoDBenefitDesg.MasterTableView.AllowSorting = AllowSorting;
        if (!string.IsNullOrEmpty(OnClientRowSelected))
        {
            //uncomment following code
            //If formulary is enabled for the user then show drilldown grid else hide it.
            if ( Context.User.IsInRole("frmly_12") )
            {
                gridDoDBenefitDesg.ClientSettings.Selecting.AllowRowSelect = true;
                gridDoDBenefitDesg.ClientSettings.ClientEvents.OnRowSelected = OnClientRowSelected;
            }
        }

        //formularyDate.Visible = gridDoDBenefitDesg.Visible;
    }
}
  
