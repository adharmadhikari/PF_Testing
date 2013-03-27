using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class controls_header : System.Web.UI.UserControl
{
    protected override void OnLoad(EventArgs e)
    {
        if ( !Context.User.IsInRole("home") )
        {
            linkHome.Visible = false;
            homeBullet.Visible = false;
        }

        tagLine.InnerText = Pinsonault.Web.Session.ClientName;

        base.OnLoad(e);
    }
}
