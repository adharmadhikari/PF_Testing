using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Dundas.Charting.WebControl;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Data.Objects;
using System.Collections.Specialized;
using System.Runtime.Serialization.Json;
using PathfinderModel;
using Pathfinder;
using Pinsonault.Data.Reports;


public class CoveredLives
{
    public int Lives { get; set; }
    public string Segment { get; set; }
    public int? Id { get; set; }
    public int Totals { get; set; }
    public int isLIS { get; set; }
   
}

public partial class standardreports_controls_CoveredLivesChart : ChartUserControl
{
    public override Chart HostedChart
    {
        get { return this.ChartCoveredLives; }
    }

    protected override void OnLoad(EventArgs e)
    {
        ProcessChart();
        base.OnLoad(e);
    }

    public void ProcessChart()
    {


        using (PathfinderModel.PathfinderEntities context = new PathfinderModel.PathfinderEntities())
        {            
            IList<CoveredLives> lives = getData(context);

            ChartCoveredLives.Attributes["_title"] = Resources.Resource.Label_Covered_Lives_Report;//for exporter

            Series series = ChartCoveredLives.Series[0];
            series.CustomAttributes = "DrawingStyle=LightToDark";
            series.CustomAttributes = "DrawingStyle=Cylinder";
            int index = 0;
            DataPoint point;

            //foreach ( DataPoint point in series.Points )
            foreach (CoveredLives cl in lives)
            {
                point = series.Points[series.Points.AddXY(cl.Segment, cl.Lives)];
                point.Color = ReportColors.StandardReports.GetColor(index % 6);               
                point.Href = string.Format("javascript:showCoveredLivesDetails({0}, '{1}')", cl.Id, cl.isLIS);

                ChartCoveredLives.Legends[0].CustomItems.Add(new LegendItem { Separator = LegendSeparatorType.ThickLine, SeparatorColor = Color.Transparent, Color = point.Color, Name = point.AxisLabel, BorderStyle = ChartDashStyle.NotSet, ShadowOffset = 0 });
                index++;
            }

        }

     }


    IList<CoveredLives> getData(PathfinderModel.PathfinderEntities context)
    {        
        var q = context.CoveredLivesSummarySet.Where(t => t.MarketSegmentId != 100 && t.MarketSegmentId != 101 && t.MarketSegmentId != 102
                                                            ).OrderBy(o => o.sortorder);
        
        return q.Select (r => new CoveredLives
            {
                Lives = (int)(r.TotalLives/1000000),
                Segment = r.MarketSegmentName,
                Id = r.MarketSegmentId,
                Totals = (int)r.TotalLives,
                isLIS = r.Is_LIS
            }).ToList();


    }

}
