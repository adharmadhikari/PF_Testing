using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using PathfinderClientModel;
using PathfinderModel;
using Pinsonault.Web; 

public partial class campaigns_current : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        dsCampaigns.ConnectionString = Pinsonault.Web.Session.ClientDBConnectionString;
    }
}
