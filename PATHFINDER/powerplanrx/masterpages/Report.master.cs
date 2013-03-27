using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Report : System.Web.UI.MasterPage
{
    protected override void OnInit(EventArgs e)
    {
        HttpContext.Current.Response.AddHeader("p3p", "CP=\"IDC DSP COR ADM DEVi TAIi PSA PSD IVAi IVDi CONi HIS OUR IND CNT\"");

        //check session

        Pinsonault.Web.Session.CheckSessionState();

    }
    protected void Page_Load(object sender, EventArgs e)
    {

    }
}
