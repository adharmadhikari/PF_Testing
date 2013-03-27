using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class accessbasedselling_all_tacticsandmessages : PageBase
{
    protected override void OnLoad(EventArgs e)
    {
        string q = Request.QueryString["p"];

        base.OnLoad(e);
    }
}
