using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Dundas.Charting.WebControl;

public partial class BrandComparisonPieChart : System.Web.UI.Page
{
    protected override void OnInit(EventArgs e)
    {
        dsBrandComparison.ConnectionString = Pinsonault.Web.Session.ClientDBConnectionString;       
        base.OnInit(e);
        Chart1.DataBind();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        Pinsonault.Web.Session.CheckSessionState();
        //save chart in cetralized location
         if(!string.IsNullOrEmpty(Pinsonault.Web.Session.ClientKey))
            Chart1.Save(Path.ChangeExtension(Path.Combine(Pinsonault.Web.Support.GetClientTempFolder("charts"), "Chart1"), "jpeg"), ChartImageFormat.Jpeg);            
    }
}
