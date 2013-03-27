using System;
using Dundas.Charting.WebControl;
using System.Configuration;
using System.IO;
using System.Data;
using Pinsonault.Application.PowerPlanRx;
using System.Drawing;

public partial class controls_DistrictProfileChart : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        FontFamily fontFamily = new FontFamily("Arial");
        Font font = new Font(
           fontFamily,
           12,
           FontStyle.Bold,
           GraphicsUnit.Pixel);

        BindCommercialChart(font);
        BindPartDChart(font);
    }
    /// <summary>
    /// For binding the district profile chart for a given brand in commercial segment
    /// </summary>
    private void BindCommercialChart(Font TitleFont)
    {
        saveChart(ChartComm);

        DataTable dtComm = Campaign.GetSVBaseByDistrictSegmentBrandID(Request.QueryString["dist"].ToString(), Convert.ToInt32((Request.QueryString["brandid"]).ToString()), (int)SegmentID.Commercial);
        

        if (dtComm.Rows.Count > 0)
        {
            ChartComm.DataSource = dtComm;

            //Add chart title
            string strTitle = String.Format("Top 5 Commercial Accounts in the District: {0} - {1} {2} ", dtComm.Rows[0]["District_Name"].ToString(), dtComm.Rows[0]["Data_Month"].ToString(), dtComm.Rows[0]["Data_Year"].ToString());
            //ChartComm.Titles.Add(strTitle);
            ChartComm.Titles.Add(strTitle, Docking.Top, TitleFont, System.Drawing.Color.Black);
            Series series = ChartComm.Series[0];

            DataPoint point;

            string strXValue;
            string strYValue;

            for (int i = 0; i < dtComm.Rows.Count; i++)
            {
                strXValue = dtComm.Rows[i]["Plan_Name"].ToString() + " " + string.Format("{0:N2}", dtComm.Rows[i]["PercentBrandTrxDistrictTotal"]) + "%";
                strYValue = dtComm.Rows[i]["PercentBrandTrxDistrictTotal"].ToString();

                point = series.Points[series.Points.AddXY(strXValue, strYValue)];

                //make points as exploded to get the exploded chart
                point["Exploded"] = "true";

                //provide tooltip for each point
                point.ToolTip = strXValue;
            }
            //apply chart color
            ChartComm.ApplyPaletteColors();

            //color the point labels with correspond slice color
            foreach (DataPoint p in series.Points)
            {
                p.LabelBackColor = p.Color;
            }
        }
        dtComm.Dispose();
    }
    /// <summary>
    /// For binding the district profile chart for a given brand in commercial segment
    /// </summary>
    private void BindPartDChart(Font TitleFont)
        {

            saveChart(ChartPartD);

            DataTable dtPartD = Campaign.GetSVBaseByDistrictSegmentBrandID(Request.QueryString["dist"].ToString(), Convert.ToInt32((Request.QueryString["brandid"]).ToString()), (int)SegmentID.PartD);

            if (dtPartD.Rows.Count > 0)
            {
                ChartPartD.DataSource = dtPartD;

                //Add chart title
                string strTitle = String.Format("Top 5 Medicare Part D Accounts in the District: {0} - {1} {2} ", dtPartD.Rows[0]["District_Name"].ToString(), dtPartD.Rows[0]["Data_Month"].ToString(), dtPartD.Rows[0]["Data_Year"].ToString());
                //ChartPartD.Titles.Add(strTitle);
                ChartPartD.Titles.Add(strTitle, Docking.Top, TitleFont, System.Drawing.Color.Black);
                Series series = ChartPartD.Series[0];

                DataPoint point;

                string strXValue;
                string strYValue;

                for (int i = 0; i < dtPartD.Rows.Count; i++)
                {
                    strXValue = dtPartD.Rows[i]["Plan_Name"].ToString() + " " + string.Format("{0:N2}", dtPartD.Rows[i]["PercentBrandTrxDistrictTotal"]) + "%";
                    strYValue = dtPartD.Rows[i]["PercentBrandTrxDistrictTotal"].ToString();

                    point = series.Points[series.Points.AddXY(strXValue, strYValue)];

                    //make points as exploded to get the exploded chart
                    point["Exploded"] = "true";

                    //provide tooltip for each point
                    point.ToolTip = strXValue;
                }
                //apply chart color
                ChartPartD.ApplyPaletteColors();

                //color the point labels with correspond slice color
                foreach (DataPoint p in series.Points)
                {
                    p.LabelBackColor = p.Color;
                }

            }
            dtPartD.Dispose();

        }

    private void saveChart(Chart chart)
    {
        Pinsonault.Web.Session.CheckSessionState();
        //save chart in cetralized location
        if (!string.IsNullOrEmpty(Pinsonault.Web.Session.ClientKey))
            chart.Save(Path.ChangeExtension(Path.Combine(Pinsonault.Web.Support.GetClientTempFolder("charts"), chart.ID), "jpeg"), ChartImageFormat.Jpeg);            
    }
    
}
