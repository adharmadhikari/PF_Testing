using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dundas.Charting.WebControl;


public partial class custom_warner_formularyhistoryreporting_controls_ChartTemplate : ChartUserControl
{
    public override Chart HostedChart
    {
        get { return this.chart; }
    }
    protected override void OnLoad(EventArgs e)
    {
        //chart.PaletteCustomColors = Pathfinder.ReportColors.Palette.Reverse().ToArray();
        chart.ChartAreas["Default"].Area3DStyle.Enable3D = true; 
        base.OnLoad(e);
    }
}
