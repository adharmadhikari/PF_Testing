using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Pinsonault.Application.MarketplaceAnalytics;

public partial class marketplaceanalytics_all_toolbar : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        using (PathfinderMarketplaceAnalyticsEntities context = new PathfinderMarketplaceAnalyticsEntities(Pinsonault.Web.Session.GetClientApplicationConnectionString("MarketplaceAnalytics")))
        {
            DateTime date = context.GetDataDate();

            litDataDate.Text = string.Format("Data as of {0:MMMM yyyy}", date);
        }
    }
}
