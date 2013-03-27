using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class createcampaign_step2_summary : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        dsCampaignInfo.ConnectionString = Pinsonault.Web.Session.ClientDBConnectionString;
    }
}
