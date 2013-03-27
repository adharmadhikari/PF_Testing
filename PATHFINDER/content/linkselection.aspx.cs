using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class content_linkselection : ContentPageBase
{
    protected override void OnLoad(EventArgs e)
    {
        string links = Request.QueryString["links"];

        if ( !string.IsNullOrEmpty(links) )
        {
            string[] a = links.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            gridLinks.DataSource = a.Select(i=>new { url = i });
            gridLinks.DataBind();

        }
        

        base.OnLoad(e);
    }
}
