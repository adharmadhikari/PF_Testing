using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Collections.Specialized;
using PathfinderModel;
using PathfinderClientModel;
using Pathfinder;
using Pinsonault.Data;
using System.Data.SqlClient;
using System.Data;
using Dundas.Charting.WebControl;
using Pinsonault.Data.Reports;
using Pinsonault.Application.MarketplaceAnalytics;

public partial class marketplaceanalytics_all_trending : PageBase
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
        int rollup;
        IEnumerable<GenericDataRecord> g = null;
        IList<int> timeFrameVals = new List<int>();

        //Add keys for query
        keys.Add("Product_ID");
        keys.Add("Drug_ID");
        keys.Add("Thera_ID");
        keys.Add("Geography_ID");

        dataYear = queryValues["Year_Selection"];
        rollup = Convert.ToInt32(queryValues["Rollup_Type"]);

        int geographyType = Convert.ToInt32(queryValues["Geography_Type"]);

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

                //If Account Manager is selected with no Top X Plans, filter by Territory
                if (geographyType == 3)
                    tableName = "MS_Monthly_By_Territory";
                else
                    tableName = "MS_Monthly";
            }
            else
            {
                timeFrame = queryValues["Quarter_Selection"]; //The timeframe is quarter based
                isMonth = false;
                dataField = "Data_Quarter";

                //If Account Manager is selected with no Top X Plans, filter by Territory
                if (geographyType == 3)
                    tableName = "MS_Quarterly_By_Territory";
                else
                    tableName = "MS_Quarterly";
            }
        }
        else
        {
            timeFrame = queryValues["Rolling_Selection"]; //The timeframe is rolling quarter based
            isMonth = false;
            dataField = "Data_Quarter";

            //If Account Manager is selected with no Top X Plans, filter by Territory
            if (geographyType == 3)
                tableName = "MS_Rolling_Quarterly_By_Territory";
            else
                tableName = "MS_Rolling_Quarterly";
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

            //Always get national data
            g = ma.GetData(timeFrameVals, isMonth, dataYear, "US", tableName.Replace("_By_Territory", ""), dataField, keys, false, null, queryValues);

            if (g.Count() > 0)
            {
                chart1.Visible = true;
                ma.ProcessGrid(grid1, isMonth, g, timeFrameVals, queryValues); //It is national, process only 1st chart
                ma.ProcessChart(timeFrameVals, chart1, isMonth, g, "trending", dataYear, queryValues);
            }

            

            if ((geographyType == 2) || (geographyType == 3)) //Region or Account Manager selected
            {
                string queryType = string.Empty;

                if (geographyType == 2)
                    queryType = "Region_ID";
                else
                    queryType = "Territory_ID";

                g = ma.GetData(timeFrameVals, isMonth, dataYear, queryValues[queryType], tableName, dataField, keys, false, null, queryValues);

                if (g.Count() > 0)
                {
                    chart2.Visible = true;
                    ma.ProcessGrid(grid2, isMonth, g, timeFrameVals, queryValues);
                    ma.ProcessChart(timeFrameVals, chart2, isMonth, g, "trending", dataYear, queryValues);
                }
            }

            if ((geographyType == 1 || geographyType == 2) && !string.IsNullOrEmpty(queryValues["State_ID"])) //State selected
            {
                //Get State data
                g = ma.GetData(timeFrameVals, isMonth, dataYear, queryValues["State_ID"], tableName, dataField, keys, false, null, queryValues);

                if (g.Count() > 0)
                {
                    chart3.Visible = true;
                    ma.ProcessGrid(grid3, isMonth, g, timeFrameVals, queryValues);
                    ma.ProcessChart(timeFrameVals, chart3, isMonth, g, "trending", dataYear, queryValues);
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
