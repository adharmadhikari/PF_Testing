using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class MasterPage : MasterPageBase
{
    public override void PreventPostback()
    {
        form.Attributes["onsubmit"] = "return false";
    }
}
