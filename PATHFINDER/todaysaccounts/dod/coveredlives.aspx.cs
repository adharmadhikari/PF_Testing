using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PathfinderClientModel; 

public partial class todaysaccounts_dod_coveredlives_Section2 : PageBase 
{
    protected override void OnPreRender(EventArgs e)
    {
        Int32 pID = Convert.ToInt32(HttpContext.Current.Request.QueryString["Plan_ID"]);

        //Plan_Id	Plan_Name
        //5860	TRICARE North (HealthNet)
        //5861	TRICARE West (Triwest Health Alliance)
        //5862	TRICARE South (Humana Military)
        //6037	TRICARE (HQ)
        //Check if the selected plan matches with the given list. if yes show the data. 
        //else display "Data not available" message.
        if ((pID != 5860) && (pID != 5861) && (pID != 5862) && (pID != 6037))
        {
            Response.Redirect("NoData.aspx");
        }
        base.OnPreRender(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //If formulary is enabled for the user then show drilldown grid else hide it.
        if ( Context.User.IsInRole("frmly_12") )
        {
            CoveredLivesDrillDown1.Visible = true;
        }
        else
        {
            CoveredLivesDrillDown1.Visible = false;
        }
    }
}
