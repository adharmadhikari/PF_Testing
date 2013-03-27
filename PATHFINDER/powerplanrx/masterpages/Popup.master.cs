using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPages_Popup : System.Web.UI.MasterPage
{
    protected override void OnLoad(EventArgs e)
    {
       
    }
    protected override void OnInit(EventArgs e)
    {
        HttpContext.Current.Response.AddHeader("p3p", "CP=\"IDC DSP COR ADM DEVi TAIi PSA PSD IVAi IVDi CONi HIS OUR IND CNT\"");

        //check session   
        Pinsonault.Web.Session.CheckSessionState();
        //if (Session["pinsoUserID"] == null)
        //    Response.Redirect("~/titleSelect.aspx?ReturnUrl=" + Server.UrlEncode(Request.RawUrl));
    }
}
