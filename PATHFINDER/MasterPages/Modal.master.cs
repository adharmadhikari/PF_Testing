using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Pinsonault.Web;

public partial class MasterPages_Modal : MasterPageBase
{
    protected override void OnLoad(EventArgs e)
    {
        Support.RegisterClientScriptAndCss(this.Page);

        base.OnLoad(e);
    }
}
