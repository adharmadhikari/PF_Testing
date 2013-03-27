using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PathfinderModel;

public partial class marketplaceanalytics_controls_FilterFHRTimeFrame : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Timeframe.ClientID, null, "moduleOptionsContainer");
    }
}
