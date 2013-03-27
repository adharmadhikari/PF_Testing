using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class controls_PBMBenefitDesign : System.Web.UI.UserControl
{
    public string ContainerID { get; set; }
    public bool AllowSorting { get; set; }
    public string OnClientRowSelected { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        Pinsonault.Web.Support.RegisterComponentWithClientManager(this.Page, gridPBMBenefitDesg.ClientID, "{}", ContainerID);

        gridPBMBenefitDesg.AllowSorting = AllowSorting;
        gridPBMBenefitDesg.MasterTableView.AllowSorting = AllowSorting;

        if (!string.IsNullOrEmpty(OnClientRowSelected))
        {
            //uncomment following code
            //If formulary is enabled for the user then show drilldown grid else hide it.
            if (Context.User.IsInRole("frmly_1"))
            {
                gridPBMBenefitDesg.ClientSettings.Selecting.AllowRowSelect = true;
                gridPBMBenefitDesg.ClientSettings.ClientEvents.OnRowSelected = OnClientRowSelected;
            }
        }
    }
}

