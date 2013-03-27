using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Dundas.Charting.WebControl;
using System.Collections.Specialized;
using System.Data;
using PathfinderModel;
using PathfinderClientModel;
using Pinsonault.Application.MarketplaceAnalytics;
using Pinsonault.Data;

public partial class marketplaceanalytics_all_comparison : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Find chart header labels and change text from National, Regional, State
        System.Web.UI.WebControls.Label lbl1 = (System.Web.UI.WebControls.Label)chart.FindControl("lbl1");
        System.Web.UI.WebControls.Label lbl2 = (System.Web.UI.WebControls.Label)chart.FindControl("lbl2");
        System.Web.UI.WebControls.Label lbl3 = (System.Web.UI.WebControls.Label)chart.FindControl("lbl3");

        lbl1.Text = "Selection 1";
        lbl2.Text = "Selection 2";
        lbl3.Text = "Selection 3";

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

        string dataField = null;
        string tableName = null;
        IList<string> keys = new List<string>();
        IList<int> timeFrame = new List<int>();
        bool isMonth;
        bool isCalendar;
        int rollup = Convert.ToInt32(queryValues["Rollup_Type"]);
        IEnumerable<GenericDataRecord> g = null;

        //Add keys for query
        keys.Add("Product_ID");
        keys.Add("Drug_ID");
        keys.Add("Thera_ID");
        keys.Add("Geography_ID");

        //Check which type of timeframe to query by as well as Calendar or Rolling
        if (string.Compare(queryValues["Calendar_Rolling"], "Calendar", true) == 0)
        {
            isCalendar = true;

            //Add year to keys
            keys.Add("Data_Year");
        }
        else
            isCalendar = false;

        MarketplaceAnalyticsProvider ma = new MarketplaceAnalyticsProvider();

        int geographyType = Convert.ToInt32(queryValues["Geography_Type"]);

        //Selection 1
        if (isCalendar)
        {
            if (queryValues["MonthQuarter1"] == "1") //It is a quarter
            {
                timeFrame.Add(Convert.ToInt32(queryValues["MonthQuarterSelection1"])); //The timeframe is quarter based
                isMonth = false;
                dataField = "Data_Quarter";

                //If Account Manager is selected with no Top X Plans, filter by Territory
                if (geographyType == 3)
                    tableName = "MS_Quarterly_By_Territory";
                else
                    tableName = "MS_Quarterly";
            }
            else
            {
                timeFrame.Add(Convert.ToInt32(queryValues["MonthQuarterSelection1"])); //The timeframe is month based
                isMonth = true;
                dataField = "Data_Month";

                //If Account Manager is selected with no Top X Plans, filter by Territory
                if (geographyType == 3)
                    tableName = "MS_Monthly_By_Territory";
                else
                    tableName = "MS_Monthly";
            }
        }
        else
        {
            timeFrame.Add(Convert.ToInt32(queryValues["RollingQuarterSelection1"])); //The timeframe is rolling quarter based
            isMonth = false;
            dataField = "Data_Quarter";

            //If Account Manager is selected with no Top X Plans, filter by Territory
            if (geographyType == 3)
                tableName = "MS_Rolling_Quarterly_By_Territory";
            else
                tableName = "MS_Rolling_Quarterly";
        }

        string geographyID = "US";

        if (geographyType == 2)
            geographyID = queryValues["Region_ID"];
        if (geographyType == 3)
            geographyID = queryValues["Territory_ID"];
        if ((geographyType == 1 || geographyType == 2) && !string.IsNullOrEmpty(queryValues["State_ID"]))
            geographyID = queryValues["State_ID"];

        g = ma.GetData(timeFrame, isMonth, queryValues["Year1"].ToString(), geographyID, tableName, dataField, keys, false, null, queryValues);

        if (g.Count() > 0)
        {
            chart1.Visible = true;
            ma.ProcessGrid(grid1, isMonth, g, timeFrame, queryValues);
            if (isCalendar)
                ma.ProcessChart(timeFrame, chart1, isMonth, g, "comparison", queryValues["Year1"].ToString(), queryValues);
            else
                ma.ProcessChart(timeFrame, chart1, isMonth, g, "comparison", null, queryValues);
        }

        //Selection 2
        timeFrame.Clear();
        if (isCalendar)
        {
            if (queryValues["MonthQuarter2"] == "1") //It is a quarter
            {
                timeFrame.Add(Convert.ToInt32(queryValues["MonthQuarterSelection2"])); //The timeframe is quarter based
                isMonth = false;
                dataField = "Data_Quarter";

                //If Account Manager is selected with no Top X Plans, filter by Territory
                if (geographyType == 3)
                    tableName = "MS_Quarterly_By_Territory";
                else
                    tableName = "MS_Quarterly";
            }
            else
            {
                timeFrame.Add(Convert.ToInt32(queryValues["MonthQuarterSelection2"])); //The timeframe is month based
                isMonth = true;
                dataField = "Data_Month";

                //If Account Manager is selected with no Top X Plans, filter by Territory
                if (geographyType == 3)
                    tableName = "MS_Monthly_By_Territory";
                else
                    tableName = "MS_Monthly";
            }
        }
        else
        {
            timeFrame.Add(Convert.ToInt32(queryValues["RollingQuarterSelection2"])); //The timeframe is rolling quarter based
            isMonth = false;
            dataField = "Data_Quarter";

            //If Account Manager is selected with no Top X Plans, filter by Territory
            if (geographyType == 3)
                tableName = "MS_Rolling_Quarterly_By_Territory";
            else
                tableName = "MS_Rolling_Quarterly";
        }

        g = ma.GetData(timeFrame, isMonth, queryValues["Year2"].ToString(), geographyID, tableName, dataField, keys, false, null, queryValues);

        if (g.Count() > 0)
        {
            chart2.Visible = true;
            ma.ProcessGrid(grid2, isMonth, g, timeFrame, queryValues);
            if (isCalendar)
                ma.ProcessChart(timeFrame, chart2, isMonth, g, "comparison", queryValues["Year2"].ToString(), queryValues);
            else
                ma.ProcessChart(timeFrame, chart2, isMonth, g, "comparison", null, queryValues);
        }

        ////Selection 3
        //timeFrame.Clear();

        //if (isCalendar)
        //{
        //    if (queryValues["MonthQuarter3"] == "1") //It is a quarter
        //    {
        //        timeFrame.Add(Convert.ToInt32(queryValues["MonthQuarterSelection3"])); //The timeframe is quarter based
        //        isMonth = false;
        //        dataField = "Data_Quarter";
        //        tableName = "MS_Quarterly";
        //    }
        //    else
        //    {
        //        timeFrame.Add(Convert.ToInt32(queryValues["MonthQuarterSelection3"])); //The timeframe is month based
        //        isMonth = true;
        //        dataField = "Data_Month";
        //        tableName = "MS_Monthly";
        //    }
        //}
        //else
        //{
        //    timeFrame.Add(Convert.ToInt32(queryValues["RollingQuarterSelection3"])); //The timeframe is rolling quarter based
        //    isMonth = false;
        //    dataField = "Data_Quarter";
        //    tableName = "MS_Rolling_Quarterly";
        //}

        ////ds.Clear();
        //g = ma.GetData(timeFrame, isMonth, queryValues["Year3"].ToString(), geographyID, tableName, dataField, keys, false, null, queryValues);

        //if (g.Count() > 0)
        //{
        //    chart3.Visible = true;
        //    ma.ProcessGrid(grid3, isMonth, g, timeFrame, queryValues);
        //    if (isCalendar)
        //        ma.ProcessChart(timeFrame, chart3, isMonth, g, "comparison", queryValues["Year3"].ToString(), queryValues);
        //    else
        //        ma.ProcessChart(timeFrame, chart3, isMonth, g, "comparison", null, queryValues);
        //}

        ////Since there are always 3 charts, change aspect ratio
        //chart1.Width = 342;
        //chart1.Height = 255;
        //chart2.Width = 342;
        //chart2.Height = 255;
        //chart3.Width = 342;
        //chart3.Height = 255;
    }
}
