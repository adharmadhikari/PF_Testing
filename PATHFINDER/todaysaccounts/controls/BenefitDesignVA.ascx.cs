using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class todaysaccounts_controls_BenefitDesignVA : System.Web.UI.UserControl
{
    public string ContainerID { get; set; }
    public string OnClientRowSelected { get; set; }
    public bool AllowSorting { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        Pinsonault.Web.Support.RegisterComponentWithClientManager(this.Page, gridVABenefitDesg.ClientID, "{}", ContainerID);
        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "_vabdpagevars", string.Format("var gridVABenefitDesgID = '{0}'; var gridMedDBenefitDesignID = {1}", gridVABenefitDesg.ClientID, null), true);

        gridVABenefitDesg.AllowSorting = AllowSorting;
        gridVABenefitDesg.MasterTableView.AllowSorting = AllowSorting;
        if (!string.IsNullOrEmpty(OnClientRowSelected))
        {
            //uncomment following code
            //If formulary is enabled for the user then show drilldown grid else hide it.
            if ( Context.User.IsInRole("frmly_11") )
            {
                gridVABenefitDesg.ClientSettings.Selecting.AllowRowSelect = true;
                gridVABenefitDesg.ClientSettings.ClientEvents.OnRowSelected = OnClientRowSelected;
            }
        }

        //formularyDate.Visible = gridVABenefitDesg.Visible;
    }
    
}
