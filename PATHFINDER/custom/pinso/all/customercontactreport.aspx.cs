using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class custom_pinso_all_customercontactreport : PageBase
{
    protected override void OnInit(EventArgs e)
    {
        gridCustomerContactReports.ClientSettings.DataBinding.Location = Pinsonault.Web.Session.ClientServiceUrl;
        //displays the logged in User name and his/her TerritoryID
        lblAcctMgr.Text = Session["FirstName"] as String;
        lblTerritoryID.Text = Session["TerritoryID"] as String;

        base.OnInit(e);
    }
    
}
