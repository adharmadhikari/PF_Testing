using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class todaysaccounts_managedmedicaid_coveredlives : PageBase
{
    protected override void OnLoad(EventArgs e)
    {
        //If formulary is enabled for the user then show drilldown grid else hide it.
        if ( Context.User.IsInRole("frmly_6") )
        {
            CoveredLivesDrillDown1.Visible = true;
        }
        else
        {
            CoveredLivesDrillDown1.Visible = false;
        }
        base.OnLoad(e);
    }

}