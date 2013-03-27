using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class custom_controls_NewSellSheetOrder : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        dsLkpShipLocations.ConnectionString = Pinsonault.Web.Session.ClientConnectionString;
        dsLkpStates.ConnectionString = Pinsonault.Web.Session.ClientConnectionString;
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, rdcmbShipLocation.ClientID);
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, rdcmbState.ClientID);

    }
}
