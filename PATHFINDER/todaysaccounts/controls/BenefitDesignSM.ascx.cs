using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class controls_BenefitDesignSM : System.Web.UI.UserControl
{
    public string ContainerID { get; set; }
    public string OnClientRowSelected { get; set; }
    public bool AllowSorting { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        Pinsonault.Web.Support.RegisterComponentWithClientManager(this.Page, gridSMBenefitDesg.ClientID, "{}", ContainerID);
        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "_smbdpagevars", string.Format("var gridSMBenefitDesgID = '{0}'; var gridMedDBenefitDesignID = {1}", gridSMBenefitDesg.ClientID,null), true);

        gridSMBenefitDesg.AllowSorting = AllowSorting;
        gridSMBenefitDesg.MasterTableView.AllowSorting = AllowSorting;
        if (!string.IsNullOrEmpty(OnClientRowSelected))
        {
            //uncomment following code
            //If formulary is enabled for the user then show drilldown grid else hide it.
            if ( Context.User.IsInRole("frmly_9") )
            {
                gridSMBenefitDesg.ClientSettings.Selecting.AllowRowSelect = true;
                gridSMBenefitDesg.ClientSettings.ClientEvents.OnRowSelected = OnClientRowSelected;
            }
        }

        //formularyDate.Visible = gridSMBenefitDesg.Visible;
    }
}
  
