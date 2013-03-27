using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPages_Popup : MasterPageBase
{
    protected override void OnLoad(EventArgs e)
    {
        imgClose.Src = string.Format("{0}/content/images/spacer.gif", Pinsonault.Web.Support.BasePath);

        base.OnLoad(e);
    }
}
