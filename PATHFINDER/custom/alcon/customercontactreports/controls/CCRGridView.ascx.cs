using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class custom_controls_CCRGridView : System.Web.UI.UserControl
{

    protected void Page_Load(object sender, EventArgs e)
    {
        //Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, gridCCReports.ClientID);
        gridCCReports.ClientSettings.DataBinding.Location = "~/custom/" + Pinsonault.Web.Session.ClientKey + "/customercontactreports/services/PathfinderDataService.svc";
    }

}
