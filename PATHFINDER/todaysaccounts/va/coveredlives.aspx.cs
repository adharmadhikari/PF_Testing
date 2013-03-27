using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class todaysaccounts_va_coveredlives : PageBase 
{
    protected override void OnPreRender(EventArgs e)
    {
        Int32 pID = Convert.ToInt32(HttpContext.Current.Request.QueryString["Plan_ID"]);
        
        if (pID != 5732) 
        {
            Response.Redirect("NoData.aspx");
        }
        base.OnPreRender(e);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        //If formulary is enabled for the user then show drilldown grid else hide it.
        if (Context.User.IsInRole("frmly_11"))
        {
            CoveredLivesDrillDown1.Visible = true;
        }
        else
        {
            CoveredLivesDrillDown1.Visible = false;
        }
    }
}
