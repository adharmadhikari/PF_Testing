using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class todaysaccounts_medicarepartd_coveredlives_Section2 : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //If formulary is enabled for the user then show drilldown grid else hide it.
        if ( Context.User.IsInRole("frmly_17") )
        {
            CoveredLivesDrillDown1.Visible = true;
        }
        else
        {
            CoveredLivesDrillDown1.Visible = false;
        }
    }
}
