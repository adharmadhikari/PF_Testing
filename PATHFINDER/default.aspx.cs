using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : ContentPageBase
{
    protected override void OnLoad(EventArgs e)
    {

        //remove from page since it is not needed (causes eventlog error if page is not accessed from https - could fix in config but since it is not needed anyway just remove)
        ServiceReference reference = ScriptManager.GetCurrent(this).Services.Where(s => string.Compare(s.Path, "~/services/securityservice.svc", true) == 0).FirstOrDefault();
        if ( reference != null )
            ScriptManager.GetCurrent(this).Services.Remove(reference);

        base.OnLoad(e);
    }
}
