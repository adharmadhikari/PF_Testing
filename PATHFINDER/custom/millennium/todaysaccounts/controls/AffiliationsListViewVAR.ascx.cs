using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class custom_millennium_todaysaccounts_controls_AffiliationsListViewVAR : System.Web.UI.UserControl
{
    protected override void OnPreRender(EventArgs e)
    {
        gridAffiliations.ClientSettings.DataBinding.Location = "~/custom/" + Pinsonault.Web.Session.ClientKey + "/todaysaccounts/services/MillenniumDataService.svc";
        base.OnPreRender(e);
    }
}
