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


public partial class marketplaceanalytics_controls_PrescriberGrid : System.Web.UI.UserControl
{

    protected void Page_Load(object sender, EventArgs e)
    {

        using (PathfinderEntities context = new PathfinderEntities())
        {
            if (!context.CheckUserModule(Pinsonault.Web.Session.UserID, 2, "prescribers"))
                throw new HttpException(500, "Invalid request");
        }

            using (PathfinderMarketplaceAnalyticsEntities context = new PathfinderMarketplaceAnalyticsEntities(Pinsonault.Web.Session.GetClientApplicationConnectionString("MarketplaceAnalytics")))
            {
                PrescriberQueryDefinition qd = new PrescriberQueryDefinition(Request.QueryString);
                //IEnumerable data = qd.CreateQuery(context);

                IEnumerable<GenericDataRecord> g = null;
                g = (qd.CreateQuery(context) as IEnumerable<GenericDataRecord>).ToList();

                MarketplaceAnalyticsProvider ma = new MarketplaceAnalyticsProvider();
                NameValueCollection queryValues = new NameValueCollection(Request.QueryString);
                string timeFrame="";
                bool isMonth=false;
                IList<int> timeFrameVals = new List<int>();

                // timeFrame
                if (string.IsNullOrEmpty(queryValues["Year1"]) || string.IsNullOrEmpty(queryValues["Year2"]))  // Trending
                {
                    
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

                }
                else  //Comparison
                {
                    if (queryValues["Selection_Clicked"] == "1")
                    {
                        //Selection1
                        if (string.Compare(queryValues["Calendar_Rolling"], "Calendar", true) == 0)//It is calendar based
                        {
                            if (queryValues["MonthQuarter1"] == "1") //The timeframe is quarter based
                            {
                                timeFrame = queryValues["MonthQuarterSelection1"];
                                isMonth = false;
                            }
                            else //The timeframe is month based
                            {
                                timeFrame = queryValues["MonthQuarterSelection1"];
                                isMonth = true;
                            }
                        }
                        else //The timeframe is rolling quarter based
                        {
                            timeFrame = queryValues["RollingQuarterSelection1"];
                            isMonth = false;
                        }
                    }

                    if (queryValues["Selection_Clicked"] == "2")
                    {
                        //Selection2
                        if (string.Compare(queryValues["Calendar_Rolling"], "Calendar", true) == 0)//It is calendar based
                        {
                            if (queryValues["MonthQuarter1"] == "1") //The timeframe is quarter based
                            {
                                timeFrame = queryValues["MonthQuarterSelection2"];
                                isMonth = false;
                            }
                            else //The timeframe is month based
                            {
                                timeFrame = queryValues["MonthQuarterSelection2"];
                                isMonth = true;
                            }
                        }
                        else //The timeframe is rolling quarter based
                        {
                            timeFrame = queryValues["RollingQuarterSelection2"];
                            isMonth = false;
                        }
                    }
                }

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
                }

                ma.ProcessGridPrescriber(popupGrid, isMonth, g, timeFrameVals, queryValues);

            }
          

    }


    //public GridView HostedGrid
    //{
    //    get { return popupGrid; }
    //}
 
    protected void popupGrid_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;

        //This replaces <td> with <th> and adds the scope attribute
        gv.UseAccessibleHeader = true;

        if (gv.HeaderRow != null)
            gv.HeaderRow.TableSection = TableRowSection.TableHeader; //This will add the <thead> and <tbody> elements

    }

    protected void popupGrid_DataBound(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        MarketplaceAnalyticsProvider ma = new MarketplaceAnalyticsProvider();

        //Specify column index that needs merging
        ma.PrescriberGroupRows(gv, 4);
        ma.PrescriberGroupRows(gv, 3);
        ma.PrescriberGroupRows(gv, 2);
        ma.PrescriberGroupRows(gv, 1);
        ma.PrescriberGroupRows(gv, 0);

        //columns moved & merging added: OK
        //ma.GroupRows(gv, 4);
        //ma.GroupRows(gv, 3);
        //ma.GroupRows(gv, 2);
        //ma.GroupRows(gv, 1);
        //ma.GroupRows(gv, 0);
    }

    protected void popupGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {

            using (PathfinderMarketplaceAnalyticsEntities context = new PathfinderMarketplaceAnalyticsEntities(Pinsonault.Web.Session.GetClientApplicationConnectionString("MarketplaceAnalytics")))
            {

                // based on client, Territory_Levels are different (only 3 used out of Territory/District/Region/Area
                // table: tr.Lkp_MS_Territory_Levels

                var tQ = from q in context.LkpMSTerritoryLevelsSet
                         orderby q.MS_Territory_Level_ID
                         select new { tID = q.MS_Territory_Level_ID, tName = q.MS_Territory_Level_Name };
                if (tQ != null)
                {
                    foreach (var i in tQ)
                    {
                        try
                        {
                            //if (i.tID == 1)  //Region level
                            //    e.Row.Cells[2].Text = i.tName;

                            //if (i.tID == 2)  //District level
                            //    e.Row.Cells[3].Text = i.tName;

                            //if (i.tID == 3)  // Territory level
                            //    e.Row.Cells[4].Text = i.tName;


                            if (i.tID == 1)  //Region level
                                e.Row.Cells[0].Text = i.tName;

                            if (i.tID == 2)  //District level
                                e.Row.Cells[1].Text = i.tName;

                            if (i.tID == 3)  // Territory level
                                e.Row.Cells[2].Text = i.tName;






                        }
                        catch (Exception ex)
                        {
                            throw new HttpException(500, ex.Message);
                        }
                    }
                }
            }

        }

    }
}

