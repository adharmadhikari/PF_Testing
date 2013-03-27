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

public partial class marketplaceanalytics_all_formularyhistoryreporting : PageBase
{
    string dataField = null;
    string tableName = null;
    IList<string> keys = new List<string>();

    protected void Page_Load(object sender, EventArgs e)
    {
        //Find chart header labels and change text from National, Regional, State
        System.Web.UI.WebControls.Label lbl1 = (System.Web.UI.WebControls.Label)chart.FindControl("lbl1");
        System.Web.UI.WebControls.Label lbl2 = (System.Web.UI.WebControls.Label)chart.FindControl("lbl2");

        lbl1.Text = "Plan 1";
        lbl2.Text = "Plan 2";

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

        //Hardcode specific queryValues for compatibility with 'GetData' function
        queryValues.Add("Rollup_Type", "4"); //Rollup by Account
        queryValues.Add("Geography_Type", "1"); //Geography is always 'US'
        queryValues.Add("Calendar_Rolling", "Calendar"); //Timeframe is always calendar based        

        bool isMonth;
        IEnumerable<GenericDataRecord> g = null;
        IList<int> timeFrameVals = new List<int>();

        //Add keys for query
        keys.Add("Product_ID");
        keys.Add("Drug_ID");
        keys.Add("Thera_ID");
        keys.Add("Geography_ID");

        string timeFrame = queryValues["Timeframe"];

        //Add year to keys
        //keys.Add("Data_Year");

        //If Med-D, timeframe is month based
        if (queryValues["Monthly_Quarterly"] == "M")
        {
            //The timeframe is month based
            isMonth = true;
            dataField = "Data_Year_Month";
            tableName = "MS_Monthly";
        }
        else
        {
            //The timeframe is quarter based
            isMonth = false;
            dataField = "Data_Year_Quarter";
            tableName = "MS_Quarterly";
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

            //Get Data For Plan 1
            queryValues.Add("Plan_ID", queryValues["Plan_ID1"]);

            g = ma.GetData(timeFrameVals, isMonth, null, "US", tableName, dataField, keys, false, null, queryValues);

            if (g.Count() > 0)
            {
                chart1.Visible = true;
                ma.ProcessChart(timeFrameVals, chart1, isMonth, g, "formularyhistory", null, queryValues, dataField);
                ma.ProcessGrid(grid1, isMonth, g, timeFrameVals, queryValues);                 
            }
            
            //Get Data For Plan 2
            queryValues["Plan_ID"] = queryValues["Plan_ID2"];

            g = ma.GetData(timeFrameVals, isMonth, null, "US", tableName, dataField, keys, false, null, queryValues);

            if (g.Count() > 0)
            {
                chart2.Visible = true;
                ma.ProcessChart(timeFrameVals, chart2, isMonth, g, "formularyhistory", null, queryValues, dataField);
                ma.ProcessGrid(grid2, isMonth, g, timeFrameVals, queryValues);                
            }

            
        }
    }
}
