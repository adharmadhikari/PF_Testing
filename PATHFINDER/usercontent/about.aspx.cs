using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class usercontent_about : PageBase
{
    protected override void OnLoad(EventArgs e)
    {
        supportEmail.NavigateUrl = string.Format("mailto:{0}", Pinsonault.Web.Support.CustomerSupportEmail);
        supportEmail.Text = Pinsonault.Web.Support.CustomerSupportEmail;

        base.OnLoad(e);
    } 

}
