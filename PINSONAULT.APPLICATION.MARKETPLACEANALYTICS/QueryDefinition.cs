using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Data.Objects;
using System.Collections;
using System.Reflection;
using Pinsonault.Data;

namespace Pinsonault.Application.MarketplaceAnalytics
{
    /// <summary>
    /// Provides information on how to generate a dynamic query based on an entity 
    /// </summary>
    /// 

    public class PrescriberTrendingQueryDefinition : QueryDefinition
    {
        public PrescriberTrendingQueryDefinition(NameValueCollection queryString)
            : base(queryString)
        {
        }

        public PrescriberTrendingQueryDefinition(string EntityTypeName, NameValueCollection queryString)
            : base(EntityTypeName, queryString)
        {
        }

        public override IEnumerable CreateQuery(ObjectContext context)
        {
            return CreateMarketplaceQuery();
        }

        IEnumerable CreateMarketplaceQuery()
        {
            Pinsonault.Application.MarketplaceAnalytics.MarketplaceAnalyticsProvider ma = new Pinsonault.Application.MarketplaceAnalytics.MarketplaceAnalyticsProvider();
            NameValueCollection queryValues = this.Values;
            IList<string> keys = new List<string>();
            IList<int> timeFrameVals = new List<int>();
            string timeFrame = "";
            bool isMonth = false;
            string dataYear = "";
            string dataField = null;
            string tableName = null;

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

            //Add time frame values to a list for processing
            if (timeFrame.IndexOf(',') > -1)
            {
                string[] timeFrameArr = timeFrame.Split(',');

                foreach (string s in timeFrameArr)
                    timeFrameVals.Add(Convert.ToInt32(s));
            }
            else
                timeFrameVals.Add(Convert.ToInt32(timeFrame));
       
            return ma.GetData(timeFrameVals, isMonth, dataYear, queryValues["Geography"], tableName, dataField, keys, false, null, queryValues);
        }
    }

    public class PrescriberTrendingDrilldownQueryDefinition : QueryDefinition
    {
        public PrescriberTrendingDrilldownQueryDefinition(NameValueCollection queryString)
            : base(queryString)
        {
        }

        public PrescriberTrendingDrilldownQueryDefinition(string EntityTypeName, NameValueCollection queryString)
            : base(EntityTypeName, queryString)
        {
        }

        public override IEnumerable CreateQuery(ObjectContext context)
        {
            return CreateMarketplaceQuery();
        }

        IEnumerable CreateMarketplaceQuery()
        {
            Pinsonault.Application.MarketplaceAnalytics.MarketplaceAnalyticsProvider ma = new Pinsonault.Application.MarketplaceAnalytics.MarketplaceAnalyticsProvider();
            NameValueCollection queryValues = this.Values;
            IList<string> keys = new List<string>();
            IList<int> timeFrameVals = new List<int>();
            string timeFrame = "";
            bool isMonth = false;
            string dataYear = "";
            string dataField = null;
            string tableName = null;
            int? productID = null;


            //Add keys for query
            keys.Add("Physician_Name");
            keys.Add("Physician_ID");
            keys.Add("Product_ID");
            keys.Add("Product_Name");
            keys.Add("Thera_ID");
            //keys.Add("Plan_ID");
            //keys.Add("Plan_Name");
            //keys.Add("Segment_ID");
            keys.Add("Region_ID");
            keys.Add("District_ID");
            keys.Add("Territory_ID");

            keys.Add("Region_Name");
            keys.Add("District_Name");
            keys.Add("Territory_Name");

            dataYear = queryValues["Year_Selection"];

            if (!string.IsNullOrEmpty(queryValues["Product_ID"]))
            {
                if (queryValues["Product_ID"].IndexOf(',') == -1)
                    productID = Convert.ToInt32(queryValues["Product_ID"]);
            }

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

            //Add time frame values to a list for processing
            if (timeFrame.IndexOf(',') > -1)
            {
                string[] timeFrameArr = timeFrame.Split(',');

                foreach (string s in timeFrameArr)
                    timeFrameVals.Add(Convert.ToInt32(s));
            }
            else
                timeFrameVals.Add(Convert.ToInt32(timeFrame));

            

            return ma.GetDataPrescriberTrending(timeFrameVals, timeFrame, isMonth, dataYear, tableName, dataField, keys, queryValues);
        }
    }

    public class PrescriberTrendingPopupQueryDefinition : QueryDefinition
    {
        public PrescriberTrendingPopupQueryDefinition(NameValueCollection queryString)
            : base(queryString)
        {
        }

        public PrescriberTrendingPopupQueryDefinition(string EntityTypeName, NameValueCollection queryString)
            : base(EntityTypeName, queryString)
        {
        }

        public override IEnumerable CreateQuery(ObjectContext context)
        {
            return CreateMarketplaceQuery();
        }

        IEnumerable CreateMarketplaceQuery()
        {
            Pinsonault.Application.MarketplaceAnalytics.MarketplaceAnalyticsProvider ma = new Pinsonault.Application.MarketplaceAnalytics.MarketplaceAnalyticsProvider();
            NameValueCollection queryValues = this.Values;
            IList<string> keys = new List<string>();
            IList<int> timeFrameVals = new List<int>();
            string timeFrame;
            bool isMonth;
            string dataYear;
            string dataField = null;
            string tableName = null;
            int rollup;
            int? productID = null;

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

            //Add time frame values to a list for processing
            if (timeFrame.IndexOf(',') > -1)
            {
                string[] timeFrameArr = timeFrame.Split(',');

                foreach (string s in timeFrameArr)
                    timeFrameVals.Add(Convert.ToInt32(s));
            }
            else
                timeFrameVals.Add(Convert.ToInt32(timeFrame));


            return ma.GetData(timeFrameVals, isMonth, dataYear, queryValues["Geography"], tableName, dataField, keys, true, productID, queryValues);
        }
    }
   

    // sl: Prescriber 
    public class PrescriberQueryDefinition : QueryDefinition
    {
        public PrescriberQueryDefinition(NameValueCollection queryString)
            : base(queryString)
        {
        }

        public PrescriberQueryDefinition(string EntityTypeName, NameValueCollection queryString)
            : base(EntityTypeName, queryString)
        {
        }

        public override IEnumerable CreateQuery(ObjectContext context)
        {
            return CreateMarketplaceQuery();
        }

        IEnumerable CreateMarketplaceQuery()
        {
            Pinsonault.Application.MarketplaceAnalytics.MarketplaceAnalyticsProvider ma = new Pinsonault.Application.MarketplaceAnalytics.MarketplaceAnalyticsProvider();
            NameValueCollection queryValues = this.Values;
            IList<string> keys = new List<string>();
            IList<int> timeFrameVals = new List<int>();
            string timeFrame="";
            bool isMonth=false;
            string dataYear="";
            string dataField = null;
            string tableName = null;
            string topN_tableName = null;
            int? productID = null;


            // Comparison Report
            bool isCalendar;
            //string year1, year2;
            //string monthQuarter1, monthQuarter2;
            //string monthQuarterSelection1, monthQuarterSelection2;
            ////

            //Add keys for query
            keys.Add("Physician_Name");
            keys.Add("Physician_ID");
            keys.Add("Product_ID");
            keys.Add("Product_Name");
            keys.Add("Plan_ID");
            keys.Add("Plan_Name");
            keys.Add("Segment_ID");
            //keys.Add("Region_ID");
            //keys.Add("District_ID");
            //keys.Add("Territory_ID");

            keys.Add("Region_Name");
            keys.Add("District_Name");
            keys.Add("Territory_Name");
           

            if (string.IsNullOrEmpty(queryValues["Year1"]) || string.IsNullOrEmpty(queryValues["Year2"]))  // Trending
            {

                dataYear = queryValues["Year_Selection"];

                if (!string.IsNullOrEmpty(queryValues["Product_ID"]))
                {
                    if (queryValues["Product_ID"].IndexOf(',') == -1)
                        productID = Convert.ToInt32(queryValues["Product_ID"]);
                }

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
                        tableName = "Physician_Monthly_Territory";
                        topN_tableName = "MS_Monthly_Base_Prescribers";
                    }
                    else
                    {
                        timeFrame = queryValues["Quarter_Selection"]; //The timeframe is quarter based
                        isMonth = false;
                        dataField = "Data_Quarter";
                        tableName = "Physician_Quarterly_Territory";
                        topN_tableName = "MS_Quarterly_Base_Prescribers";
                    }
                }
                else
                {
                    timeFrame = queryValues["Rolling_Selection"]; //The timeframe is rolling quarter based
                    isMonth = false;
                    dataField = "Data_Quarter";
                    tableName = "Physician_Rolling_Quarterly_Territory";
                    topN_tableName = "MS_Rolling_Quarterly_Base_Prescribers";
                }

                //Add time frame values to a list for processing
                if (timeFrame.IndexOf(',') > -1)
                {
                    string[] timeFrameArr = timeFrame.Split(',');

                    foreach (string s in timeFrameArr)
                        timeFrameVals.Add(Convert.ToInt32(s));
                }
                else
                    timeFrameVals.Add(Convert.ToInt32(timeFrame));

                //return ma.GetDataPrescriber(timeFrameVals, timeFrame, isMonth, dataYear, tableName, topN_tableName, dataField, keys, queryValues);
                
            }
            else  // Comparison
            {
                //year1 = queryValues["Year1"];
                //year2 = queryValues["Year2"];
 
                //monthQuarter1 = queryValues["MonthQuarter1"];
                //monthQuarter2 = queryValues["MonthQuarter2"];
                
                //monthQuarterSelection1 = queryValues["MonthQuarterSelection1"];
                //monthQuarterSelection2 = queryValues["MonthQuarterSelection2"];

                //Check which type of timeframe to query by as well as Calendar or Rolling
                if (string.Compare(queryValues["Calendar_Rolling"], "Calendar", true) == 0)
                {
                    isCalendar = true;

                    //Add year to keys
                    keys.Add("Data_Year");
                }
                else
                    isCalendar = false;



                if (queryValues["Selection_Clicked"] == "1")
                {
                    //Selection 1
                    if (isCalendar)
                    {
                        if (queryValues["MonthQuarter1"] == "1") //It is a quarter
                        {
                            //timeFrameVals.Add(Convert.ToInt32(queryValues["MonthQuarterSelection1"])); //The timeframe is quarter based
                            isMonth = false;
                            dataField = "Data_Quarter";
                            tableName = "Physician_Quarterly_Territory";
                            topN_tableName = "MS_Quarterly_Base_Prescribers";
                        }
                        else
                        {
                            //timeFrameVals.Add(Convert.ToInt32(queryValues["MonthQuarterSelection1"])); //The timeframe is month based
                            isMonth = true;
                            dataField = "Data_Month";
                            tableName = "Physician_Monthly_Territory";
                            topN_tableName = "MS_Monthly_Base_Prescribers";
                        }

                        timeFrameVals.Add(Convert.ToInt32(queryValues["MonthQuarterSelection1"]));
                        timeFrame = queryValues["MonthQuarterSelection1"];
                            
                    }
                    else
                    {
                        timeFrameVals.Add(Convert.ToInt32(queryValues["RollingQuarterSelection1"])); //The timeframe is rolling quarter based
                        timeFrame = queryValues["RollingQuarterSelection1"];
                        isMonth = false;
                        dataField = "Data_Quarter";
                        tableName = "Physician_Rolling_Quarterly_Territory";
                        topN_tableName = "MS_Rolling_Quarterly_Base_Prescribers";
                    }

                    dataYear = queryValues["Year1"].ToString();

                    //return ma.GetDataPrescriber(timeFrameVals, timeFrame, isMonth, dataYear, tableName, topN_tableName, dataField, keys, queryValues);
                
                }
                if (queryValues["Selection_Clicked"] == "2")
                {
                    //Selection 2
                    if (isCalendar)
                    {
                        if (queryValues["MonthQuarter2"] == "1") //It is a quarter
                        {
                            //timeFrameVals.Add(Convert.ToInt32(queryValues["MonthQuarterSelection2"])); //The timeframe is quarter based
                            isMonth = false;
                            dataField = "Data_Quarter";
                            tableName = "Physician_Quarterly_Territory";
                            topN_tableName = "MS_Quarterly_Base_Prescribers";
                        }
                        else
                        {
                            //timeFrameVals.Add(Convert.ToInt32(queryValues["MonthQuarterSelection2"])); //The timeframe is month based
                            isMonth = true;
                            dataField = "Data_Month";
                            tableName = "Physician_Monthly_Territory";
                            topN_tableName = "MS_Monthly_Base_Prescribers";
                        }

                        timeFrameVals.Add(Convert.ToInt32(queryValues["MonthQuarterSelection2"]));
                        timeFrame = queryValues["MonthQuarterSelection2"];
                    }
                    else
                    {
                        timeFrameVals.Add(Convert.ToInt32(queryValues["RollingQuarterSelection2"])); //The timeframe is rolling quarter based
                        timeFrame = queryValues["RollingQuarterSelection2"];
                        isMonth = false;
                        dataField = "Data_Quarter";
                        tableName = "Physician_Rolling_Quarterly_Territory";
                        topN_tableName = "MS_Rolling_Quarterly_Base_Prescribers";
                    }

                    dataYear = queryValues["Year2"].ToString();
           
                }
            }
            return ma.GetDataPrescriber(timeFrameVals, timeFrame, isMonth, dataYear, tableName, topN_tableName, dataField, keys, queryValues);
        }
    }

    //sl end

    public class TrendingQueryDefinition : QueryDefinition
    {
        public TrendingQueryDefinition(NameValueCollection queryString)
            : base(queryString)
        {
        }

        public TrendingQueryDefinition(string EntityTypeName, NameValueCollection queryString)
            : base(EntityTypeName, queryString)
        {
        }

        public override IEnumerable CreateQuery(ObjectContext context)
        {
            return CreateMarketplaceQuery();
        }

        IEnumerable CreateMarketplaceQuery()
        {
            Pinsonault.Application.MarketplaceAnalytics.MarketplaceAnalyticsProvider ma = new Pinsonault.Application.MarketplaceAnalytics.MarketplaceAnalyticsProvider();
            NameValueCollection queryValues = this.Values;
            IList<string> keys = new List<string>();
            IList<int> timeFrameVals = new List<int>();
            string timeFrame;
            bool isMonth;
            string dataYear;
            string dataField = null;
            string tableName = null;
            int rollup;

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

            //Add time frame values to a list for processing
            if (timeFrame.IndexOf(',') > -1)
            {
                string[] timeFrameArr = timeFrame.Split(',');

                foreach (string s in timeFrameArr)
                    timeFrameVals.Add(Convert.ToInt32(s));
            }
            else
                timeFrameVals.Add(Convert.ToInt32(timeFrame));

            //Never use _By_Territory table if Geography is "US"
            if (string.Compare(queryValues["Geography"], "US", true) == 0)
                tableName = tableName.Replace("_By_Territory", "");

            return ma.GetData(timeFrameVals, isMonth, dataYear, queryValues["Geography"], tableName, dataField, keys, false, null, queryValues);
        }
    }

    public class TrendingDrilldownQueryDefinition : QueryDefinition
    {
        public TrendingDrilldownQueryDefinition(NameValueCollection queryString)
            : base(queryString)
        {
        }

        public TrendingDrilldownQueryDefinition(string EntityTypeName, NameValueCollection queryString)
            : base(EntityTypeName, queryString)
        {
        }

        public override IEnumerable CreateQuery(ObjectContext context)
        {
            return CreateMarketplaceQuery();
        }

        IEnumerable CreateMarketplaceQuery()
        {
            Pinsonault.Application.MarketplaceAnalytics.MarketplaceAnalyticsProvider ma = new Pinsonault.Application.MarketplaceAnalytics.MarketplaceAnalyticsProvider();
            NameValueCollection queryValues = this.Values;            
            IList<string> keys = new List<string>();
            IList<int> timeFrameVals = new List<int>();
            string timeFrame;
            bool isMonth;
            string dataYear;
            string dataField = null;
            string tableName = null;
            int? productID = null;
            int rollup;

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

            int geographyType = Convert.ToInt32(queryValues["Geography_Type"]);

            if (!string.IsNullOrEmpty(queryValues["Product_ID"]))
            {
                if (queryValues["Product_ID"].IndexOf(',') == -1)
                    productID = Convert.ToInt32(queryValues["Product_ID"]);
            }

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

            //Add time frame values to a list for processing
            if (timeFrame.IndexOf(',') > -1)
            {
                string[] timeFrameArr = timeFrame.Split(',');

                foreach (string s in timeFrameArr)
                    timeFrameVals.Add(Convert.ToInt32(s));
            }
            else
                timeFrameVals.Add(Convert.ToInt32(timeFrame));

            //Disable paging
            queryValues["PagingEnabled"] = "False";
            queryValues["Export"] = "True";

            //Never use _By_Territory table if Geography is "US"
            if (string.Compare(queryValues["Geography"], "US", true) == 0)
                tableName = tableName.Replace("_By_Territory", "");

            return ma.GetData(timeFrameVals, isMonth, dataYear, queryValues["Geography"], tableName, dataField, keys, true, productID, queryValues);
        }
    }

    public class ComparisonQueryDefinition : QueryDefinition
    {
        public ComparisonQueryDefinition(NameValueCollection queryString)
            : base(queryString)
        {
        }

        public ComparisonQueryDefinition(string EntityTypeName, NameValueCollection queryString)
            : base(EntityTypeName, queryString)
        {
        }

        public override IEnumerable CreateQuery(ObjectContext context)
        {
            return CreateMarketplaceQuery();
        }

        IEnumerable CreateMarketplaceQuery()
        {
            Pinsonault.Application.MarketplaceAnalytics.MarketplaceAnalyticsProvider ma = new Pinsonault.Application.MarketplaceAnalytics.MarketplaceAnalyticsProvider();
            NameValueCollection queryValues = this.Values;
            IList<string> keys = new List<string>();
            IList<int> timeFrame = new List<int>();
            bool isMonth = false;
            bool isCalendar;
            string dataField = null;
            string tableName = null;
            string dataYear = "";
            int rollup = Convert.ToInt32(queryValues["Rollup_Type"]);

            int geographyType = Convert.ToInt32(queryValues["Geography_Type"]);

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

            if (Convert.ToInt32(queryValues["Selection"]) == 1)
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

                dataYear = queryValues["Year1"].ToString();
            }
            if (Convert.ToInt32(queryValues["Selection"]) == 2)
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

                dataYear = queryValues["Year2"].ToString();
            }

            string geographyID = "US";

            if (geographyType == 2)
                geographyID = queryValues["Region_ID"];
            if (geographyType == 3)
                geographyID = queryValues["Territory_ID"];
            if ((geographyType == 1 || geographyType == 2) && !string.IsNullOrEmpty(queryValues["State_ID"]))
                geographyID = queryValues["State_ID"];

            return ma.GetData(timeFrame, isMonth, dataYear, geographyID, tableName, dataField, keys, false, null, queryValues);
        }
    }

    public class ComparisonDrilldownQueryDefinition : QueryDefinition
    {
        public ComparisonDrilldownQueryDefinition(NameValueCollection queryString)
            : base(queryString)
        {
        }

        public ComparisonDrilldownQueryDefinition(string EntityTypeName, NameValueCollection queryString)
            : base(EntityTypeName, queryString)
        {
        }

        public override IEnumerable CreateQuery(ObjectContext context)
        {
            return CreateMarketplaceQuery();
        }

        IEnumerable CreateMarketplaceQuery()
        {
            Pinsonault.Application.MarketplaceAnalytics.MarketplaceAnalyticsProvider ma = new Pinsonault.Application.MarketplaceAnalytics.MarketplaceAnalyticsProvider();
            NameValueCollection queryValues = this.Values;
            string dataField = null;
            string tableName = null;
            IList<string> keys = new List<string>();
            IList<int> timeFrame = new List<int>();
            bool isMonth = false;
            bool isCalendar;
            string year1, year2;//, year3;
            string monthQuarter1, monthQuarter2;//, monthQuarter3;
            string monthQuarterSelection1, monthQuarterSelection2;//, monthQuarterSelection3;
            string dataYear = "";
            int? productID = null;
            int rollup = Convert.ToInt32(queryValues["Rollup_Type"]);

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

            monthQuarter1 = queryValues["MonthQuarter1"];
            monthQuarter2 = queryValues["MonthQuarter2"];

            monthQuarterSelection1 = queryValues["MonthQuarterSelection1"];
            monthQuarterSelection2 = queryValues["MonthQuarterSelection2"];

            int geographyType = Convert.ToInt32(queryValues["Geography_Type"]);

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

            string geographyID = "US";

            if (geographyType == 2)
                geographyID = queryValues["Region_ID"];
            if (geographyType == 3)
                geographyID = queryValues["Territory_ID"];
            if ((geographyType == 1 || geographyType == 2) && !string.IsNullOrEmpty(queryValues["State_ID"]))
                geographyID = queryValues["State_ID"];

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

                dataYear = queryValues["Year1"].ToString();
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

                dataYear = queryValues["Year2"].ToString();
            }
           

            //Disable paging
            queryValues["PagingEnabled"] = "False";
            queryValues["Export"] = "True";

            return ma.GetData(timeFrame, isMonth, dataYear, geographyID, tableName, dataField, keys, true, productID, queryValues);
        }
    }
    /// <summary>
    /// For affiliations report (plan level) summary
    /// </summary>
    public class AffiliationQueryDefinition : QueryDefinition
    {
        public AffiliationQueryDefinition(NameValueCollection queryString)
            : base(queryString)
        {
        }

        public AffiliationQueryDefinition(string EntityTypeName, NameValueCollection queryString)
            : base(EntityTypeName, queryString)
        {
        }

        public override IEnumerable CreateQuery(ObjectContext context)
        {
            return CreateMarketplaceQuery();
        }

        IEnumerable CreateMarketplaceQuery()
        {
            Pinsonault.Application.MarketplaceAnalytics.MarketplaceAnalyticsProvider ma = new Pinsonault.Application.MarketplaceAnalytics.MarketplaceAnalyticsProvider();
            NameValueCollection queryValues = this.Values;
            IList<string> keys = new List<string>();
            IList<int> timeFrameVals = new List<int>();
            string timeFrame;
            bool isMonth;
            string dataYear;
            string dataField = null;
            string tableName = null;

            //Add keys for query
            keys.Add("Product_ID");           

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
                    tableName = "V_MS_Monthly_PlanSummary";
                }
                else
                {
                    timeFrame = queryValues["Quarter_Selection"]; //The timeframe is quarter based
                    isMonth = false;
                    dataField = "Data_Quarter";
                    tableName = "V_MS_Quarterly_PlanSummary";
                }
            }
            else
            {
                timeFrame = queryValues["Rolling_Selection"]; //The timeframe is rolling quarter based
                isMonth = false;
                dataField = "Data_Quarter";
                tableName = "V_MS_Rolling_Quart_PlanSummary";
            }

            //Add time frame values to a list for processing
            if (timeFrame.IndexOf(',') > -1)
            {
                string[] timeFrameArr = timeFrame.Split(',');

                foreach (string s in timeFrameArr)
                    timeFrameVals.Add(Convert.ToInt32(s));
            }
            else
                timeFrameVals.Add(Convert.ToInt32(timeFrame));
            //get plan level data
            return ma.GetPlanData(timeFrameVals, isMonth, dataYear,tableName, dataField, keys, false, null, queryValues);
        }
    }
    /// <summary>
    /// For affiliations report (plan level) Drilldown
    /// </summary>
    public class AffiliationDrilldownQueryDefinition : QueryDefinition
    {
        public AffiliationDrilldownQueryDefinition(NameValueCollection queryString)
            : base(queryString)
        {
        }

        public AffiliationDrilldownQueryDefinition(string EntityTypeName, NameValueCollection queryString)
            : base(EntityTypeName, queryString)
        {
        }

        public override IEnumerable CreateQuery(ObjectContext context)
        {
            return CreateMarketplaceQuery();
        }

        IEnumerable CreateMarketplaceQuery()
        {
            Pinsonault.Application.MarketplaceAnalytics.MarketplaceAnalyticsProvider ma = new Pinsonault.Application.MarketplaceAnalytics.MarketplaceAnalyticsProvider();
            NameValueCollection queryValues = this.Values;            
            IList<string> keys = new List<string>();
            IList<int> timeFrameVals = new List<int>();
            string timeFrame;
            bool isMonth;
            string dataYear;
            string dataField = null;
            string tableName = null;
            int? productID = null;

            //Add keys for query
            keys.Add("Product_ID");
            keys.Add("Drug_ID");
            keys.Add("Thera_ID");
            keys.Add("Segment_ID");
            keys.Add("Segment_Name");            
            keys.Add("Geography_ID");          
            keys.Add("Plan_ID");
            //keys.Add("MB_TRx");

            dataYear = queryValues["Year_Selection"];

            if (!string.IsNullOrEmpty(queryValues["Product_ID"]))
            {
                if (queryValues["Product_ID"].IndexOf(',') == -1)
                    productID = Convert.ToInt32(queryValues["Product_ID"]);
            }

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
                    tableName = "V_MS_Monthly_PlanAffil"; 
                }
                else
                {
                    timeFrame = queryValues["Quarter_Selection"]; //The timeframe is quarter based
                    isMonth = false;
                    dataField = "Data_Quarter";
                    tableName = "V_MS_Quarterly_PlanAffil"; 
                }
            }
            else
            {
                timeFrame = queryValues["Rolling_Selection"]; //The timeframe is rolling quarter based
                isMonth = false;
                dataField = "Data_Quarter";
                tableName = "V_MS_Rolling_Quart_PlanAffil"; 
            }

            //Add time frame values to a list for processing
            if (timeFrame.IndexOf(',') > -1)
            {
                string[] timeFrameArr = timeFrame.Split(',');

                foreach (string s in timeFrameArr)
                    timeFrameVals.Add(Convert.ToInt32(s));
            }
            else
                timeFrameVals.Add(Convert.ToInt32(timeFrame));

            //Logic for Mkt Trx column - only show if 1 timeframe/Trx is selected
            if ((string.Compare(queryValues["Trx_Mst"], "trx", true) != 0) || (timeFrameVals.Count > 1))
                keys.Remove("MB_TRx");

            //Disable paging
            queryValues["PagingEnabled"] = "False";
            queryValues["Export"] = "True";

            return ma.GetPlanData(timeFrameVals, isMonth, dataYear, tableName, dataField, keys, true, productID, queryValues);
        }
    }

    public class FormularyHistoryQueryDefinition : QueryDefinition
    {
        public FormularyHistoryQueryDefinition(NameValueCollection queryString)
            : base(queryString)
        {
        }

        public FormularyHistoryQueryDefinition(string EntityTypeName, NameValueCollection queryString)
            : base(EntityTypeName, queryString)
        {
        }

        public override IEnumerable CreateQuery(ObjectContext context)
        {
            return CreateMarketplaceQuery();
        }

        IEnumerable CreateMarketplaceQuery()
        {
            Pinsonault.Application.MarketplaceAnalytics.MarketplaceAnalyticsProvider ma = new Pinsonault.Application.MarketplaceAnalytics.MarketplaceAnalyticsProvider();
            NameValueCollection queryValues = this.Values;
            IList<string> keys = new List<string>();
            IList<int> timeFrameVals = new List<int>();
            string timeFrame;
            bool isMonth;
            string dataField = null;
            string tableName = null;

            //Add keys for query
            keys.Add("Product_ID");
            keys.Add("Drug_ID");
            keys.Add("Thera_ID");
            keys.Add("Geography_ID");

            timeFrame = queryValues["Timeframe"];

            //Hardcode specific queryValues for compatibility with 'GetData' function
            queryValues.Add("Rollup_Type", "4"); //Rollup by Account
            queryValues.Add("Geography_Type", "1"); //Geography is always 'US'
            queryValues.Add("Calendar_Rolling", "Calendar"); //Timeframe is always calendar based

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

            //Add time frame values to a list for processing
            if (timeFrame.IndexOf(',') > -1)
            {
                string[] timeFrameArr = timeFrame.Split(',');

                foreach (string s in timeFrameArr)
                    timeFrameVals.Add(Convert.ToInt32(s));
            }
            else
                timeFrameVals.Add(Convert.ToInt32(timeFrame));

            return ma.GetData(timeFrameVals, isMonth, null, "US", tableName, dataField, keys, false, null, queryValues);
        }
    }

    public class FormularyHistoryDrilldownQueryDefinition : QueryDefinition
    {
        public FormularyHistoryDrilldownQueryDefinition(NameValueCollection queryString)
            : base(queryString)
        {
        }

        public FormularyHistoryDrilldownQueryDefinition(string EntityTypeName, NameValueCollection queryString)
            : base(EntityTypeName, queryString)
        {
        }

        public override IEnumerable CreateQuery(ObjectContext context)
        {
            return CreateMarketplaceQuery();
        }

        IEnumerable CreateMarketplaceQuery()
        {
            Pinsonault.Application.MarketplaceAnalytics.MarketplaceAnalyticsProvider ma = new Pinsonault.Application.MarketplaceAnalytics.MarketplaceAnalyticsProvider();
            NameValueCollection queryValues = this.Values;
            IList<string> keys = new List<string>();
            IList<int> timeFrameVals = new List<int>();
            bool isMonth;
            string dataField = null;
            string tableName = null;
            int? productID = null;

            //Add keys for query
            keys.Add("Product_ID");
            keys.Add("Drug_ID");
            keys.Add("Thera_ID");
            keys.Add("Segment_ID");
            keys.Add("Segment_Name");
            keys.Add("Geography_ID");
            keys.Add("Plan_ID");

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
            

            //Add time frame values to a list for processing
            if (timeFrame.IndexOf(',') > -1)
            {
                string[] timeFrameArr = timeFrame.Split(',');

                foreach (string s in timeFrameArr)
                    timeFrameVals.Add(Convert.ToInt32(s));
            }
            else
                timeFrameVals.Add(Convert.ToInt32(timeFrame));

            //Disable paging
            queryValues["PagingEnabled"] = "False";
            queryValues["Export"] = "True";

            return ma.GetData(timeFrameVals, isMonth, null, "US", tableName, dataField, keys, true, productID, queryValues);
        }
    }

    public class FormularyHistoryModalQueryDefinition : QueryDefinition
    {
        public FormularyHistoryModalQueryDefinition(NameValueCollection queryString)
            : base(queryString)
        {
        }

        public FormularyHistoryModalQueryDefinition(string EntityTypeName, NameValueCollection queryString)
            : base(EntityTypeName, queryString)
        {
        }

        public override IEnumerable CreateQuery(ObjectContext context)
        {
            return CreateMarketplaceQuery();
        }

        IEnumerable CreateMarketplaceQuery()
        {
            Pinsonault.Application.MarketplaceAnalytics.MarketplaceAnalyticsProvider ma = new Pinsonault.Application.MarketplaceAnalytics.MarketplaceAnalyticsProvider();
            NameValueCollection queryValues = this.Values;
            IList<string> keys = new List<string>();
            IList<int> timeFrameVals = new List<int>();
            string timeFrame;
            bool isMonth;
            string dataField = null;
            string tableName = null;

            //Add keys for query
            keys.Add("Product_ID");
            keys.Add("Drug_ID");
            keys.Add("Thera_ID");
            keys.Add("Geography_ID");

            timeFrame = queryValues["Timeframe"];

            //Hardcode specific queryValues for compatibility with 'GetData' function
            queryValues.Add("Rollup_Type", "4"); //Rollup by Account
            queryValues.Add("Geography_Type", "1"); //Geography is always 'US'
            queryValues.Add("Calendar_Rolling", "Calendar"); //Timeframe is always calendar based

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

            //Add time frame values to a list for processing
            if (timeFrame.IndexOf(',') > -1)
            {
                string[] timeFrameArr = timeFrame.Split(',');

                foreach (string s in timeFrameArr)
                    timeFrameVals.Add(Convert.ToInt32(s));
            }
            else
                timeFrameVals.Add(Convert.ToInt32(timeFrame));

            return ma.GetFHData(timeFrameVals, isMonth, tableName, dataField, keys, queryValues);
        }
    }
}
