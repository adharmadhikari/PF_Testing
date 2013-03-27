using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class custom_controls_SellSheetOrderList : System.Web.UI.UserControl
{
    public RadGrid HostedGrid
    {
        get { return gridSellSheetOrders; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        gridSellSheetOrders.ClientSettings.DataBinding.Location = "~/custom/" + Pinsonault.Web.Session.ClientKey + "/sellsheets/services/DeyDataService.svc";
    }
}
