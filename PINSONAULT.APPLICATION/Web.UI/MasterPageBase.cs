using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Configuration;

/// <summary>
/// Base class that all Pathfinder master pages should implement to enforce Page subclassing.
/// </summary>
public class MasterPageBase : MasterPage
{
    public virtual void PreventPostback(){}

    protected override void OnInit(EventArgs e)
    {
        if ( Page is PageBase || Page is ContentPageBase )
            base.OnInit(e);
        else
            throw new ApplicationException("Pathfinder pages must implement PageBase for authenticated user pages or ContentPageBase for non-authenticated user pages.");
    }

    protected override void OnPreRender(EventArgs e)
    {
        ScriptManager sm = ScriptManager.GetCurrent(this.Page);
        if ( sm != null )
        {
            string version = ConfigurationManager.AppSettings["appVersion"];
            if ( string.IsNullOrEmpty(version) )
                version = "x_x_x_x";
            else
                version = version.Replace(".", "_");
            foreach ( ScriptReference script in sm.Scripts )
            {
                //only mod local files
                if ( script.Path.StartsWith("~") )
                    script.Path = string.Format("{0}?{1}", script.Path, version);
            }
        }
        base.OnPreRender(e);
    }
}
