using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using Pinsonault.Data;
using Dundas.Charting.WebControl;
using Pinsonault.Application.MarketplaceAnalytics;

public partial class prescriberreporting_all_prescribertrending : PageBase
{
    string dataField = null;
    string tableName = null;
    IList<string> keys = new List<string>();

    protected void Page_Load(object sender, EventArgs e)
    {
        //Find Grids and Charts
        GridView grid1 = (GridView)chart.FindControl("grid1").FindControl("gridTemplate");
        GridView grid2 = (GridView)chart.FindControl("grid2").FindControl("gridTemplate");
        GridView grid3 = (GridView)chart.FindControl("grid3").FindControl("gridTemplate");

        Chart chart1 = (Chart)chart.FindControl("chartDisplay1").FindControl("chart");
        Chart chart2 = (Chart)chart.FindControl("chartDisplay2").FindControl("chart");
        Chart chart3 = (Chart)chart.FindControl("chartDisplay3").FindControl("chart");

        chart1.Visible = false;
        chart2.Visible = false;
        chart3.Visible = false;

        NameValueCollection queryValues = new NameValueCollection(Request.QueryString);

        string timeFrame;
        bool isMonth;
        string dataYear;
        //int rollup;
        IEnumerable<GenericDataRecord> g = null;
        IList<int> timeFrameVals = new List<int>();

        //Add keys for query
        keys.Add("Product_ID");
        keys.Add("Thera_ID");

        dataYear = queryValues["Year_Selection"];

        //Check which type of timeframe to query by as well as Calendar or Rolling
        if (string.Compare(queryValues["Calendar_Rolling"], "Calendar", true) == 0)
        {
            //Add year to keys
            keys.Add("Data_Year");

            //Calendar
            if (string.IsNullOrEmpty(queryValues["Quarter_Selection"]))
            {
                timeFrame = queryValues["Month_Selection"]; //The timeframe is month based
                isMonth = true;
                dataField = "Data_Month";
                tableName = "MS_Prescriber_Monthly_Territory";
            }
            else
            {
                timeFrame = queryValues["Quarter_Selection"]; //The timeframe is quarter based
                isMonth = false;
                dataField = "Data_Quarter";
                tableName = "MS_Prescriber_Quarterly_Territory";
            }
        }
        else
        {
            timeFrame = queryValues["Rolling_Selection"]; //The timeframe is rolling quarter based
            isMonth = false;
            dataField = "Data_Quarter";
            tableName = "MS_Prescriber_Rolling_Quarterly_Territory";
        }

        MarketplaceAnalyticsProvider ma = new MarketplaceAnalyticsProvider();

        if (!string.IsNullOrEmpty(timeFrame))
        {
            //Add time frame values to a list for processing
            if (timeFrame.IndexOf(',') > -1)
            {
                string[] timeFrameArr = timeFrame.Split(',');

                foreach (string s in timeFrameArr)
                    timeFrameVals.Add(Convert.ToInt32(s));
            }
            else
                timeFrameVals.Add(Convert.ToInt32(timeFrame));

            queryValues.Add("Prescriber_Geography_Type", "region");

            if (!string.IsNullOrEmpty(Request.QueryString["Region_ID"]) && Request.QueryString["Region_ID"] != "all")
            {
                
                g = ma.GetData(timeFrameVals, isMonth, dataYear, queryValues["Region_ID"], tableName, dataField, keys, false, null, queryValues);

                if (g.Count() > 0)
                {
                    chart1.Visible = true;
                    ma.ProcessGrid(grid1, isMonth, g, timeFrameVals, queryValues);
                    ma.ProcessChart(timeFrameVals, chart1, isMonth, g, "prescribertrending", dataYear, queryValues);
                }
            }


            if (!string.IsNullOrEmpty(Request.QueryString["District_ID"]) && Request.QueryString["District_ID"] != "all")
            {
                queryValues["Prescriber_Geography_Type"] = "district";
                g = ma.GetData(timeFrameVals, isMonth, dataYear, queryValues["District_ID"], tableName, dataField, keys, false, null, queryValues);

                if (g.Count() > 0)
                {
                    chart2.Visible = true;
                    ma.ProcessGrid(grid2, isMonth, g, timeFrameVals, queryValues);
                    ma.ProcessChart(timeFrameVals, chart2, isMonth, g, "prescribertrending", dataYear, queryValues);
                }
            }

            if (!string.IsNullOrEmpty(Request.QueryString["Territory_ID"]) && Request.QueryString["Territory_ID"] != "all")
            {
                queryValues["Prescriber_Geography_Type"] = "territory";
                g = ma.GetData(timeFrameVals, isMonth, dataYear, queryValues["Territory_ID"], tableName, dataField, keys, false, null, queryValues);

                if (g.Count() > 0)
                {
                    chart3.Visible = true;
                    ma.ProcessGrid(grid3, isMonth, g, timeFrameVals, queryValues);
                    ma.ProcessChart(timeFrameVals, chart3, isMonth, g, "prescribertrending", dataYear, queryValues);
                }

                if (!string.IsNullOrEmpty(queryValues["Region_ID"]))
                {
                    //Since there are 3 charts, change ratio otherwise leave defaults
                    chart1.Width = 342;
                    chart1.Height = 300;
                    chart2.Width = 342;
                    chart2.Height = 300;
                    chart3.Width = 342;
                    chart3.Height = 300;
                }
            }
        }
    }
}
