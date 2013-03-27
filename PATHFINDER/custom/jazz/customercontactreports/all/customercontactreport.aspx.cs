using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class custom_jazz_customercontactreports_all_customercontactreport : PageBase
{
    protected override void OnError(EventArgs e)
    {
        base.OnError(e);
    }
    protected override void OnLoad(EventArgs e)
    {
        lblUserID.Text = Pinsonault.Web.Session.UserID.ToString();
        base.OnLoad(e);
    }
}
