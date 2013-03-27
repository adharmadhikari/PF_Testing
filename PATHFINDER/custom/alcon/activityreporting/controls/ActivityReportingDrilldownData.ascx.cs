using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class custom_controls_CCRDrilldownData : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        gridDrillDown.ClientSettings.DataBinding.Location = "~/custom/" + Pinsonault.Web.Session.ClientKey + "/activityreporting/services/AlconDataService.svc";
    }
}
