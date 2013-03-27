using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class usercontent_confirmexport : PageBase
{
    protected override void OnLoad(EventArgs e)
    {
        confirmText.Text = string.Format(Resources.Resource.Message_Export_Confirmation, Pinsonault.Web.Session.ClientName);

        string customExport = Request.QueryString["exportHandler"];
        if(string.IsNullOrEmpty(customExport))
            customExport = "null";

        btnAccept.Attributes["onclick"] = string.Format("onConfirmed('{0}', '{1}', '{2}',{3})", Request.QueryString["type"], Request.QueryString["module"],Request.QueryString["channel"], customExport);
        
        
        base.OnLoad(e);
    }
}
