using System;
using Dundas.Charting.WebControl;
using System.Configuration;
using System.IO;
using System.Data;
using Pinsonault.Application.PowerPlanRx;
using System.Web.UI.WebControls;


public partial class controls_DistrictRegionBrand : ChartUserControl
{
    public override Chart HostedChart
    {
        get { return ChartAll; }
    }

    public override Unit GetRenderedHeight()
    {
        return new Unit("400px");
    }

    public override Unit GetRenderedWidth()
    {
        return new Unit("800px");
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        DataTable dtComm = Campaign.GetSVBaseByRegionDistrictSegmentBrandID(Request.QueryString["reporttype"].ToString(), Request.QueryString["regionid"].ToString(), Request.QueryString["dist"].ToString(), Convert.ToInt32((Request.QueryString["brandid"]).ToString()), Convert.ToInt32((Request.QueryString["segment"]).ToString()));

        if (dtComm.Rows.Count > 0)

        {
            //saveChart(ChartAll);
            ChartAll.DataSource = dtComm;

            //Add chart title
            if (Request.QueryString["dist"].ToString() != "0")
            {
                ChartAll.Titles.Add("Top 5 " + dtComm.Rows[0]["Segment_Name"].ToString() + " Accounts in the District: " + dtComm.Rows[0]["District_Name"].ToString());

            }
            else
            {
                ChartAll.Titles.Add("Top 5 " + dtComm.Rows[0]["Segment_Name"].ToString() + " Accounts in the Region: " + dtComm.Rows[0]["Region_Name"].ToString());
            }

            
            Series series = ChartAll.Series[0];
            
            DataPoint point;

            string strXValue;
            string strYValue;
           
            //added to get only Top 5 plans in donut chart
            DataRow[] drcomm =  dtComm.Select(" sortorder = 1 ");

            //for (int i = 0; i < dtComm.Rows.Count; i++)
            for (int i = 0; i < drcomm.Length ; i++)
            {
                if (Request.QueryString["reporttype"].ToString() == "1") // Single Brand TRx
                {
                    if (Request.QueryString["dist"].ToString() != "0") // District Selected
                    {
                        strXValue = dtComm.Rows[i]["Plan_Name"].ToString() + " " + string.Format("{0:N2}", dtComm.Rows[i]["PercentBrandTrxDistrictTotal"]) + "%";
                        strYValue = dtComm.Rows[i]["PercentBrandTrxDistrictTotal"].ToString();
                        point = series.Points[series.Points.AddXY(strXValue, strYValue)];
                    }
                    else // Region
                    {
                        strXValue = dtComm.Rows[i]["Plan_Name"].ToString() + " " + string.Format("{0:N2}", dtComm.Rows[i]["PercentBrandTrxRegionTotal"]) + "%";
                        strYValue = dtComm.Rows[i]["PercentBrandTrxRegionTotal"].ToString();
                        point = series.Points[series.Points.AddXY(strXValue, strYValue)];
                    }
                }
                else // Market Basket Group TRx
                {
                    if (Request.QueryString["dist"].ToString() != "0") // District Selected
                    {
                        strXValue = dtComm.Rows[i]["Plan_Name"].ToString() + " " + string.Format("{0:N2}", dtComm.Rows[i]["PercentMBTrxDistrictTotal"]) + "%";
                        strYValue = dtComm.Rows[i]["PercentMBTrxDistrictTotal"].ToString();
                        point = series.Points[series.Points.AddXY(strXValue, strYValue)];
                    }
                    else // Region
                    {
                        strXValue = dtComm.Rows[i]["Plan_Name"].ToString() + " " + string.Format("{0:N2}", dtComm.Rows[i]["PercentMBTrxRegionTotal"]) + "%";
                        strYValue = dtComm.Rows[i]["PercentMBTrxRegionTotal"].ToString();
                        point = series.Points[series.Points.AddXY(strXValue, strYValue)];
                    }
                }            
             
                //make points as exploded to get the exploded chart
                point["Exploded"] = "true";    
 
                //provide tooltip for each point
                point.ToolTip = strXValue;  
            }
            //apply chart color
            ChartAll.ApplyPaletteColors();
           
            //color the point labels with correspond slice color
            foreach (DataPoint p in series.Points)
            {
                p.LabelBackColor = p.Color;
            }
        }
        dtComm.Dispose();
    }

    //private void saveChart(Chart chart)
    //{
    //    Pinsonault.Web.Session.CheckSessionState();
    //    //save chart in cetralized location
    //    string chartID = Guid.NewGuid().ToString();
    //    if (!string.IsNullOrEmpty(Pinsonault.Web.Session.ClientKey))
    //        chart.Save(Path.ChangeExtension(Path.Combine(Pinsonault.Web.Support.GetClientTempFolder("charts"), chartID), "jpeg"), ChartImageFormat.Jpeg);
    //}
}
