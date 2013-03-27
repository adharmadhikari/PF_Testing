using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Text.RegularExpressions;

public partial class login : ContentPageBase
{
    protected override void OnInit(EventArgs e)
    {
        if (FormsAuthentication.RequireSSL)
        {
            string newUrl = Request.Url.AbsoluteUri;

            //make sure using "www" (also ok if other subdomain like "beta" or "demo") - however if nothing is present "www" is replacement value
            //                                                                                                          -check also will not work if domain type is not ".com"
            Regex regex = new Regex(@"://.+\..+\.com");
            if (!regex.IsMatch(newUrl))
            {
                Regex replace = new Regex(@"://");
                newUrl = replace.Replace(newUrl, "://www.", 1);
            }
            //make sure using https
            regex = new Regex(@"http://");
            newUrl = regex.Replace(newUrl, "https://", 1);

            if (string.Compare(Request.Url.AbsoluteUri, newUrl, true) != 0)
                Response.Redirect(newUrl);
        }

        base.OnInit(e);
    }
}
