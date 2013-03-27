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

public partial class prescriberreporting_all_detailedgrid : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        NameValueCollection queryValues = new NameValueCollection(Request.QueryString);
        bool isMonth;
        string timeFrame;

        if (string.Compare(queryValues["Calendar_Rolling"], "Calendar", true) == 0)
        {
            //Calendar
            if (string.IsNullOrEmpty(queryValues["Quarter_Selection"]))
            {
                timeFrame = queryValues["Month_Selection"]; //The timeframe is month based
                isMonth = true;
            }
            else
            {
                timeFrame = queryValues["Quarter_Selection"]; //The timeframe is quarter based
                isMonth = false;
            }
        }
        else
        {
            timeFrame = queryValues["Rolling_Selection"]; //The timeframe is rolling quarter based
            isMonth = false;
        }

        IList<int> timeFrameVals = new List<int>();
        if (timeFrame.IndexOf(',') > -1)
        {
            string[] timeFrameArr = timeFrame.Split(',');

            foreach (string s in timeFrameArr)
                timeFrameVals.Add(Convert.ToInt32(s));
        }
        else
            timeFrameVals.Add(Convert.ToInt32(timeFrame));

        PrescriberTrendingDrilldownQueryDefinition qd = new PrescriberTrendingDrilldownQueryDefinition(queryValues);

        IEnumerable<GenericDataRecord> g = null;

        

        using (PathfinderMarketplaceAnalyticsEntities context = new PathfinderMarketplaceAnalyticsEntities(Pinsonault.Web.Session.GetClientApplicationConnectionString("MarketplaceAnalytics")))
        {
            //Check if page count is needed
            bool getPageCount = false;
            NameValueCollection n = null;

            if ((!string.IsNullOrEmpty(queryValues["RequestPageCount"])) && (Convert.ToBoolean(queryValues["RequestPageCount"]) == true))
            {
                n = new NameValueCollection(queryValues);
                getPageCount = true;
                n["PagingEnabled"] = "false";
            }

            g = (qd.CreateQuery(context) as IEnumerable<GenericDataRecord>).ToList();

            MarketplaceAnalyticsProvider ma = new MarketplaceAnalyticsProvider();

            ma.ProcessGrid(detailedGrid, isMonth, g, timeFrameVals, queryValues);

            if (getPageCount)
            {
                //g = ma.GetData(timeFrameVals, isMonth, dataYear, queryValues["Region_ID"], tableName, dataField, keys, true, productID, n);
                PrescriberTrendingDrilldownQueryDefinition qdCount = new PrescriberTrendingDrilldownQueryDefinition(n);

                g = (qdCount.CreateQuery(context) as IEnumerable<GenericDataRecord>).ToList();
                
            }            
        }
        gridCount.Text = g.FirstOrDefault().GetValue(0).ToString();

        //Show/Hide Geography columns based on Region/District/Territory selection
        if (string.Compare(queryValues["Selection_Clicked"], "1", true) == 0) //Region Clicked
        {
            detailedGrid.Columns[1].Visible = false; //Hide District
            detailedGrid.Columns[2].Visible = false; //Hide Territory
        }
        if (string.Compare(queryValues["Selection_Clicked"], "2", true) == 0) //Region Clicked
        {
            detailedGrid.Columns[0].Visible = false; //Hide Region
            detailedGrid.Columns[2].Visible = false; //Hide Territory
        }
        if (string.Compare(queryValues["Selection_Clicked"], "3", true) == 0) //Region Clicked
        {
            detailedGrid.Columns[0].Visible = false; //Hide Region
            detailedGrid.Columns[1].Visible = false; //Hide District
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
        ma.GroupRows(gv, 4);
        ma.GroupRows(gv, 3);
        ma.GroupRows(gv, 2);
        ma.GroupRows(gv, 1);
        ma.GroupRows(gv, 0);
        

    }
}
