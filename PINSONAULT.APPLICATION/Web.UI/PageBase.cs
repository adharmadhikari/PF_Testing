using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

/// <summary>
/// Base class for all user pages that loads a user's session if it is missing and also applies the appropriate Theme.
/// </summary>
public class PageBase: Page
{
	public PageBase()
	{
	}

    protected override void OnPreInit(EventArgs e)
    {
        Pinsonault.Web.Session.CheckSessionState();

        Theme = Pinsonault.Web.Support.Theme;

        this.Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(Pinsonault.Web.Support.PageCacheDuration));

        base.OnPreInit(e);
    }

    protected override void SavePageStateToPersistenceMedium(object state)
    {
        //ViewState has been disabled by default in the web.config however Telerik controls output gobs of viewstate via ControlState we are only saving it if explicitly requested for the page.
        if (EnableViewState)
            base.SavePageStateToPersistenceMedium(state);
    }

}
