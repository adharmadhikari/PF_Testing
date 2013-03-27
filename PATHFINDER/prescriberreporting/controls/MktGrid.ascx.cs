using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
//using PathfinderModel;
using System.Collections;
using System.Text;
using PathfinderClientModel;
using Pinsonault.Application.MarketplaceAnalytics;
using Pinsonault.Data;
using PathfinderModel;


public partial class prescriberreporting_controls_PrescriberGrid : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        NameValueCollection queryValues = new NameValueCollection(Request.QueryString);
        string dataField = null;
        string tableName = null;
        IList<string> keys = new List<string>();
        string timeFrame;
        bool isMonth;
        string dataYear;
        int? productID = null;
        int rollup;
        IEnumerable<GenericDataRecord> g = null;

        //Add keys for query
        keys.Add("Product_ID");
        keys.Add("Drug_ID");
        keys.Add("Thera_ID");
        keys.Add("Segment_ID");
        keys.Add("Segment_Name");
        keys.Add("Geography_ID");
        keys.Add("Plan_ID");

        dataYear = queryValues["Year_Selection"];
        rollup = Convert.ToInt32(queryValues["Rollup_Type"]);

        if (!string.IsNullOrEmpty(queryValues["Product_ID"]))
        {
            if (queryValues["Product_ID"].IndexOf(',') == -1)
                productID = Convert.ToInt32(queryValues["Product_ID"]);
            else
                productID = Convert.ToInt32(queryValues["Default_Product_ID"]);
        }
        

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
                tableName = "Physician_Monthly_Territory";
            }
            else
            {
                timeFrame = queryValues["Quarter_Selection"]; //The timeframe is quarter based
                isMonth = false;
                dataField = "Data_Quarter";
                tableName = "Physician_Quarterly_Territory";
            }
        }
        else
        {
            timeFrame = queryValues["Rolling_Selection"]; //The timeframe is rolling quarter based
            isMonth = false;
            dataField = "Data_Quarter";
            tableName = "Physician_Rolling_Quarterly_Territory";
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

            //Logic for Segment column - only show if 'Combined' or 'All' option is selected
            if (queryValues["Section_ID"] != "-1")
                detailedGrid.Columns[3].Visible = false;
            if (string.IsNullOrEmpty(queryValues["Section_ID"]))
                detailedGrid.Columns[3].Visible = true;

            //Hide Lives, Tier, Co-Pay and Restrictions columns if 'Employer' or 'Other' is selected
            if (queryValues["Section_ID"] == "14" || queryValues["Section_ID"] == "8")
            {
                detailedGrid.Columns[1].Visible = false;
                detailedGrid.Columns[4].Visible = false;
                detailedGrid.Columns[5].Visible = false;
                detailedGrid.Columns[6].Visible = false;
            }

            //Get data
            //    detailedGrid.Columns[6].Visible = false;
            //    detailedGrid.Columns[7].Visible = false;
            if (string.Compare(queryValues["Selection_Clicked"], "1") == 0)
            {
                g = ma.GetData(timeFrameVals, isMonth, dataYear, queryValues["Region_ID"], tableName, dataField, keys, true, productID, queryValues);

                ma.ProcessGrid(detailedGrid, isMonth, g, timeFrameVals, queryValues);
            }
            if (string.Compare(queryValues["Selection_Clicked"], "2") == 0)
            {
                g = ma.GetData(timeFrameVals, isMonth, dataYear, queryValues["District_ID"], tableName, dataField, keys, true, productID, queryValues);

                ma.ProcessGrid(detailedGrid, isMonth, g, timeFrameVals, queryValues);
            }
            if (string.Compare(queryValues["Selection_Clicked"], "3") == 0)
            {
                g = ma.GetData(timeFrameVals, isMonth, dataYear, queryValues["Territory_ID"], tableName, dataField, keys, true, productID, queryValues);

                ma.ProcessGrid(detailedGrid, isMonth, g, timeFrameVals, queryValues);
            }
        }

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
        //ma.GroupRows(gv, 2);
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