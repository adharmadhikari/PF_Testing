using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class todaysaccounts_controls_VAKeyContactsListGrid : System.Web.UI.UserControl
{
    public string OnClientRowSelected { get; set; }
    public string OnClientDataBinding { get; set; }
    public string ContainerID { get; set; }

    protected override void OnLoad(EventArgs e)
    {
        //Pinsonault.Web.Support.RegisterComponentWithClientManager(this.Page, gridKeyContacts.ClientID, "{}", ContainerID);

        gridKeyContacts.ClientSettings.ClientEvents.OnRowSelected = OnClientRowSelected;
        gridKeyContacts.ClientSettings.ClientEvents.OnDataBinding = OnClientDataBinding;

        base.OnLoad(e);
    }
}
