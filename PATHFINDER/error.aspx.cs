using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Default2 : System.Web.UI.Page
{
    protected override void OnLoad(EventArgs e)
    {
        supportEmail.NavigateUrl = string.Format("mailto:{0}", Pinsonault.Web.Support.CustomerSupportEmail);

        Pinsonault.Web.Support.EmailException(Context.User.Identity.Name, Context);

        base.OnLoad(e);
    } 
}
