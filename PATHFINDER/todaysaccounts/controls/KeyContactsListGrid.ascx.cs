using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class todaysaccounts_controls_KeyContactsListGrid : System.Web.UI.UserControl
{
    public string OnClientRowSelected { get; set; }
    public string OnClientDataBinding { get; set; }
    public string ContainerID { get; set; }

    protected override void OnLoad(EventArgs e)
    {        
        gridWrapper.ContainerID = ContainerID;

        if ( string.IsNullOrEmpty(OnClientRowSelected) )
        {
            gridKeyContacts.ClientSettings.Selecting.AllowRowSelect = false;
            gridWrapper.PagingSelector = ".areaHeader .pagination";
        }
        else
            gridKeyContacts.ClientSettings.ClientEvents.OnRowSelected = OnClientRowSelected;
        //gridKeyContacts.ClientSettings.ClientEvents.OnDataBinding = OnClientDataBinding;
        gridWrapper.OnClientDataBinding = OnClientDataBinding;

        base.OnLoad(e);
    }
}
