using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class accessbasedselling_all_terrgrid : PageBase
{
    protected override void OnInit(EventArgs e)
    {
        dsData.ConnectionString = Pinsonault.Web.Session.ClientConnectionString;
        base.OnInit(e);
    }
}
