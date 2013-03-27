using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dundas.Charting.WebControl;

public partial class standardreports_controls_PieChartTemplate : ChartUserControl
{
    public override Chart HostedChart
    {
        get { return Chart1; }
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
    }   
}
