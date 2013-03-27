using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class todaysaccounts_controls_FHRIcon : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Hide FHR Modal if user is not in ta_fhr role
        if (!HttpContext.Current.User.IsInRole("ta_fhr"))
            fhrModalButton.Visible = false;
        else
            fhrModalButton.Visible = true;
    }
}
