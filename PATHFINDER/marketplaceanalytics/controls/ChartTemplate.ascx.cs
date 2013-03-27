using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dundas.Charting.WebControl;

public partial class marketplaceanalytics_controls_ChartTemplate : ChartUserControl
{
    public override Chart HostedChart
    {
        get { return chart; }
    }

    //For iPad and non-flash browser support
    public override Unit GetRenderedHeight()
    {
        return new Unit("210px");            
    }

    public override Unit GetRenderedWidth()
    {
        int chartCount = ChartUserControlManager.Current.GetVisibleChartCount();

        string pixels = chartCount == 1 ? "415px" : chartCount == 2 ? "365px" : "235px";

        return new Unit(pixels);
    }
}
