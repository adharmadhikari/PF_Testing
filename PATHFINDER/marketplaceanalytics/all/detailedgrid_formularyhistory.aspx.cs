using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Data;
using PathfinderModel;
using System.Collections;
using Pinsonault.Application.MarketplaceAnalytics;
using Pinsonault.Data;

public partial class marketplaceanalytics_all_detailedgrid_formularyhistory : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        NameValueCollection queryValues = new NameValueCollection(Request.QueryString);
        string dataField = null;
        string tableName = null;
        IList<string> keys = new List<string>();
        bool isMonth;
        int? productID = null;
        IEnumerable<GenericDataRecord> g = null;

        //Add keys for query
        keys.Add("Product_ID");
        keys.Add("Drug_ID");
        keys.Add("Thera_ID");
        keys.Add("Segment_ID");
        keys.Add("Segment_Name");
        keys.Add("Geography_ID");
        keys.Add("Plan_ID");
        //keys.Add("Data_Year");

        //Hardcode specific queryValues for compatibility with 'GetData' function
        queryValues.Add("Rollup_Type", "4"); //Rollup by Account
        queryValues.Add("Geography_Type", "1"); //Geography is always 'US'
        queryValues.Add("Calendar_Rolling", "Calendar"); //Timeframe is always calendar based      

        string timeFrame = queryValues["Timeframe"];

        if (!string.IsNullOrEmpty(queryValues["Product_ID"]))
        {
            if (queryValues["Product_ID"].IndexOf(',') == -1)
                productID = Convert.ToInt32(queryValues["Product_ID"]);
        }

        //If Med-D, timeframe is month based
        if (Convert.ToInt32(queryValues["Section_ID"]) == 17)
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
            IList<int> timeFrameVals = new List<int>();
            if (timeFrame.IndexOf(',') > -1)
            {
                string[] timeFrameArr = timeFrame.Split(',');

                foreach (string s in timeFrameArr)
                    timeFrameVals.Add(Convert.ToInt32(s));
            }
            else
                timeFrameVals.Add(Convert.ToInt32(timeFrame));

            //Logic for Tier column - Hide if State Medicaid
            if (queryValues["Section_ID"] == "9")
                detailedGrid.Columns[4].Visible = false;

            //Check if page count is needed
            bool getPageCount = false;
            NameValueCollection n = null;

            if ((!string.IsNullOrEmpty(queryValues["RequestPageCount"])) && (Convert.ToBoolean(queryValues["RequestPageCount"]) == true))
            {
                n = new NameValueCollection(queryValues);
                getPageCount = true;
                n["PagingEnabled"] = "false";
            }

            //Get data
            if (string.Compare(queryValues["Selection_Clicked"], "1") == 0)
            {
                //Get Data For Plan 1
                queryValues.Add("Plan_ID", queryValues["Plan_ID1"]);
                n.Add("Plan_ID", queryValues["Plan_ID1"]);

                g = ma.GetData(timeFrameVals, isMonth, null, "US", tableName, dataField, keys, true, productID, queryValues);

                ma.ProcessGrid(detailedGrid, isMonth, g, timeFrameVals, queryValues);

                if (getPageCount)
                    g = ma.GetData(timeFrameVals, isMonth, null, "US", tableName, dataField, keys, true, productID, n);
            }
            if (string.Compare(queryValues["Selection_Clicked"], "2") == 0)
            {
                //Get Data For Plan 2
                queryValues["Plan_ID"] = queryValues["Plan_ID2"];
                n.Add("Plan_ID", queryValues["Plan_ID2"]);

                g = ma.GetData(timeFrameVals, isMonth, null, "US", tableName, dataField, keys, true, productID, queryValues);

                ma.ProcessGrid(detailedGrid, isMonth, g, timeFrameVals, queryValues);

                if (getPageCount)
                    g = ma.GetData(timeFrameVals, isMonth, null, "US", tableName, dataField, keys, true, productID, n);
            }            
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
        //ma.GroupRows(gv, 3);
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
    }

}
