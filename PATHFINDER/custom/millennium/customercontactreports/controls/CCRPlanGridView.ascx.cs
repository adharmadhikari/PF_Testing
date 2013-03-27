using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class custom_controls_CCRPlanGridView : System.Web.UI.UserControl
{
    protected override void OnLoad(EventArgs e)
    {
        gridPlans.ClientSettings.DataBinding.Location = "~/custom/" + Pinsonault.Web.Session.ClientKey + "/customercontactreports/services/MillenniumDataService.svc";

        base.OnLoad(e);
    }
}
