using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Data;
using PathfinderModel;
using Pinsonault.Application.MarketplaceAnalytics;
using Pinsonault.Data;

public partial class marketplaceanalytics_all_detailedgridcomparison : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        NameValueCollection queryValues = new NameValueCollection(Request.QueryString);
        string dataField = null;
        string tableName = null;
        IList<string> keys = new List<string>();
        IList<int> timeFrame = new List<int>();
        bool isMonth = false;
        bool isCalendar;
        string year1, year2, year3;
        string monthQuarter1, monthQuarter2, monthQuarter3;
        string monthQuarterSelection1, monthQuarterSelection2, monthQuarterSelection3;
        int? productID = null;
        int rollup = Convert.ToInt32(queryValues["Rollup_Type"]);
        IEnumerable<GenericDataRecord> g = null;

        //Add keys for query
        keys.Add("Product_ID");
        keys.Add("Drug_ID");
        keys.Add("Thera_ID");
        keys.Add("Segment_ID");
        keys.Add("Segment_Name");
        keys.Add("Geography_ID");
        keys.Add("Plan_ID");

        year1 = queryValues["Year1"];
        year2 = queryValues["Year2"];
        year3 = queryValues["Year3"];

        monthQuarter1 = queryValues["MonthQuarter1"];
        monthQuarter2 = queryValues["MonthQuarter2"];
        monthQuarter3 = queryValues["MonthQuarter3"];

        monthQuarterSelection1 = queryValues["MonthQuarterSelection1"];
        monthQuarterSelection2 = queryValues["MonthQuarterSelection2"];
        monthQuarterSelection3 = queryValues["MonthQuarterSelection3"];

        //Check which type of timeframe to query by as well as Calendar or Rolling
        if (string.Compare(queryValues["Calendar_Rolling"], "Calendar", true) == 0)
        {
            isCalendar = true;

            //Add year to keys
            keys.Add("Data_Year");
        }
        else
            isCalendar = false;

        if (!string.IsNullOrEmpty(queryValues["Product_ID"]))
        {
            if (queryValues["Product_ID"].IndexOf(',') == -1)
                productID = Convert.ToInt32(queryValues["Product_ID"]);
        }
       
        MarketplaceAnalyticsProvider ma = new MarketplaceAnalyticsProvider();

        string geographyID = "US";

        int geographyType = Convert.ToInt32(queryValues["Geography_Type"]);

        if (geographyType == 2)
            geographyID = queryValues["Region_ID"];
        if (geographyType == 3)
            geographyID = queryValues["Territory_ID"];
        if ((geographyType == 1 || geographyType == 2) && !string.IsNullOrEmpty(queryValues["State_ID"]))
            geographyID = queryValues["State_ID"];


        //Logic for Segment column - only show if 'Combined' option is selected
        if (queryValues["Section_ID"] != "-1")
            detailedGrid.Columns[3].Visible = false;

        //Hide Lives, Tier, Co-Pay and Restrictions columns if 'Employer' or 'Other' is selected
        if (queryValues["Section_ID"] == "14" || queryValues["Section_ID"] == "8")
        {
            detailedGrid.Columns[2].Visible = false;
            detailedGrid.Columns[5].Visible = false;
            detailedGrid.Columns[6].Visible = false;
            detailedGrid.Columns[7].Visible = false;
        }

        //Check if page count is needed
        bool getPageCount = false;
        NameValueCollection n = null;

        if ((!string.IsNullOrEmpty(queryValues["RequestPageCount"])) && (Convert.ToBoolean(queryValues["RequestPageCount"]) == true))
        {
            n = new NameValueCollection(queryValues);
            getPageCount = true;
            n["PagingEnabled"] = "false";
        }

        if (queryValues["Selection_Clicked"] == "1")
        {
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

            g = ma.GetData(timeFrame, isMonth, queryValues["Year1"].ToString(), geographyID, tableName, dataField, keys, true, productID, queryValues);

            if (g.Count() > 0)
                ma.ProcessGrid(detailedGrid, isMonth, g, timeFrame, queryValues);

            if (getPageCount)
                g = ma.GetData(timeFrame, isMonth, queryValues["Year1"].ToString(), geographyID, tableName, dataField, keys, true, productID, n);
        }
        if (queryValues["Selection_Clicked"] == "2")
        {
            //Selection 2
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

            g = ma.GetData(timeFrame, isMonth, queryValues["Year2"].ToString(), geographyID, tableName, dataField, keys, true, productID, queryValues);

            if (g.Count() > 0)
                ma.ProcessGrid(detailedGrid, isMonth, g, timeFrame, queryValues);

            if (getPageCount)
                g = ma.GetData(timeFrame, isMonth, queryValues["Year2"].ToString(), geographyID, tableName, dataField, keys, true, productID, n);
        }
        if (queryValues["Selection_Clicked"] == "3")
        {
            //Selection 3
            if (isCalendar)
            {
                if (queryValues["MonthQuarter3"] == "1") //It is a quarter
                {
                    timeFrame.Add(Convert.ToInt32(queryValues["MonthQuarterSelection3"])); //The timeframe is quarter based
                    isMonth = false;
                    dataField = "Data_Quarter";
                    tableName = "MS_Quarterly";
                }
                else
                {
                    timeFrame.Add(Convert.ToInt32(queryValues["MonthQuarterSelection3"])); //The timeframe is month based
                    isMonth = true;
                    dataField = "Data_Month";
                    tableName = "MS_Monthly";
                }
            }
            else
            {
                timeFrame.Add(Convert.ToInt32(queryValues["RollingQuarterSelection3"])); //The timeframe is rolling quarter based
                isMonth = false;
                dataField = "Data_Quarter";
                tableName = "MS_Rolling_Quarterly";
            }

            g = ma.GetData(timeFrame, isMonth, queryValues["Year3"].ToString(), geographyID, tableName, dataField, keys, true, productID, queryValues);

            if (g.Count() > 0)
                ma.ProcessGrid(detailedGrid, isMonth, g, timeFrame, queryValues);

            if (getPageCount)
                g = ma.GetData(timeFrame, isMonth, queryValues["Year3"].ToString(), geographyID, tableName, dataField, keys, true, productID, n);
        }

        gridCount.Text = g.FirstOrDefault().GetValue(0).ToString();
    }

    protected void detailedGrid_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;

        //This replaces <td> with <th> and adds the scope attribute
        gv.UseAccessibleHeader = true;

        //This will add the <thead> and <tbody> elements
        GridViewRow hdr = gv.HeaderRow;

        if (hdr != null)
            gv.HeaderRow.TableSection = TableRowSection.TableHeader;
    }

    protected void detailedGrid_DataBound(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;

        MarketplaceAnalyticsProvider ma = new MarketplaceAnalyticsProvider();

        //Specify column index that needs merging
        ma.GroupRows(gv, 3);
        ma.GroupRows(gv, 2);
        ma.GroupRows(gv, 1);
        ma.GroupRows(gv, 0);
    }

    protected string CheckRestriction(object PA, object QL, object ST)
    {
        IList<string> restrictions = new List<string>();

        if ((!string.IsNullOrEmpty(PA.ToString())))
            restrictions.Add("PA");
        if ((!string.IsNullOrEmpty(QL.ToString())))
            restrictions.Add("QL");
        if ((!string.IsNullOrEmpty(ST.ToString())))
            restrictions.Add("ST");

        string concatRestrictions = "";

        if (restrictions.Count > 0)
            concatRestrictions = string.Join(",", restrictions.ToArray());

        return concatRestrictions;
        //if ((!string.IsNullOrEmpty(restriction.ToString()) && Convert.ToBoolean(restriction) == true))
        //    return true;
        //else
        //    return false;
    }

}
