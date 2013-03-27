using System;
using System.Collections.Specialized;
using System.Linq;
using PathfinderClientModel;
using Pinsonault.Data;
using Pinsonault.Data.Reports;
using System.Drawing;
using System.Collections.Generic;
using PathfinderModel;

namespace Pinsonault.Application.MarketplaceAnalytics
{
    public class TrendingReportDefinitionBase : ReportDefinition
    {
        public string Geography { get; set; }

        private NameValueCollection newFilters;

        MarketplaceAnalyticsProvider ma = new MarketplaceAnalyticsProvider();

        protected override QueryDefinition CreateQueryDefinition(NameValueCollection filters)
        {
            //NameValueCollection newFilters = new NameValueCollection(filters);
            newFilters = new NameValueCollection(filters);

            int geographyType = Convert.ToInt32(newFilters["Geography_Type"]);

            if (string.Compare(Geography, "National", true) == 0)
            {
                newFilters.Add("Geography", "US");
                SectionTitle = "National";
            }

            if (string.Compare(Geography, "Regional", true) == 0)
            {

                if (geographyType == 2)
                {
                    newFilters.Add("Geography", newFilters["Region_ID"]);

                    SectionTitle = ma.GetRegionName(newFilters["Region_ID"]);
                }
                else if (geographyType == 3)
                {
                    newFilters.Add("Geography", newFilters["Territory_ID"]);

                    SectionTitle = ma.GetAccountManagerName(newFilters["Territory_ID"]);
                }
                else
                    Visible = false;
            }

            if (string.Compare(Geography, "State", true) == 0)
            {
                if ((geographyType == 1 || geographyType == 2) && !string.IsNullOrEmpty(newFilters["State_ID"]))
                {
                    newFilters.Add("Geography", newFilters["State_ID"]);

                    SectionTitle = ma.GetStateName(newFilters["State_ID"]);
                }
                else
                    Visible = false;
            }

            return new TrendingQueryDefinition(EntityTypeName, newFilters);
        }

        protected override ColumnMap CreateColumnMap(System.Data.Common.DbDataRecord columnRecord)
        {
            ColumnMap map = base.CreateColumnMap(columnRecord);// new ColumnMap();

            //Check is ColumnMap is Product_Trx or Product_Mst
            if (map.PropertyName.IndexOf(string.Format("Product_{0}", newFilters["Trx_Mst"])) > -1)
            {
                IList<int> timeFrameVals = new List<int>();
                bool isMonth;
                string timeFrame;

                if (string.Compare(newFilters["Calendar_Rolling"], "Calendar", true) == 0)//It is calendar based
                {
                    if (string.IsNullOrEmpty(newFilters["Quarter_Selection"]))
                    {
                        timeFrame = newFilters["Month_Selection"]; 
                        isMonth = true;
                    }
                    else//Timeframe is quarter based
                    {
                        timeFrame = newFilters["Quarter_Selection"]; 
                        isMonth = false;
                    }
                }
                else //It is rolling based
                {
                    timeFrame = newFilters["Rolling_Selection"]; 
                    isMonth = false;
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

                List<int> timeFrames = timeFrameVals.ToList();
                //If Rolling Quarterly, reverse time frame values
                if (string.Compare(newFilters["Calendar_Rolling"], "Rolling", true) == 0)
                    timeFrames.Reverse();

                map.TranslatedName = ma.GetHeaderText(isMonth, timeFrames[Convert.ToInt32(map.TranslatedName)], newFilters);
            }

            return map;
        }
    }

    public class PrescriberTrendingReportDefinitionBase : ReportDefinition
    {
        public string Geography { get; set; }

        private NameValueCollection newFilters;

        MarketplaceAnalyticsProvider ma = new MarketplaceAnalyticsProvider();

        protected override QueryDefinition CreateQueryDefinition(NameValueCollection filters)
        {
            //NameValueCollection newFilters = new NameValueCollection(filters);
            newFilters = new NameValueCollection(filters);

            //int geographyType = Convert.ToInt32(newFilters["Geography_Type"]);

            if (string.Compare(Geography, "Region", true) == 0)
            {
                if (!string.IsNullOrEmpty(newFilters["Region_ID"]))
                {
                    newFilters.Add("Geography", newFilters["Region_ID"]);
                    newFilters.Add("Prescriber_Geography_Type", "region");
                    SectionTitle = ma.GetPrescriberGeographyName(newFilters["Region_ID"], "region");
                }
                else
                    Visible = false;
            }

            if (string.Compare(Geography, "District", true) == 0)
            {
                if(!string.IsNullOrEmpty(newFilters["District_ID"]))
                {
                    newFilters.Add("Geography", newFilters["District_ID"]);
                    newFilters.Add("Prescriber_Geography_Type", "district");
                    SectionTitle = ma.GetPrescriberGeographyName(newFilters["District_ID"], "district");
                }
                else
                    Visible = false;
            }

            if (string.Compare(Geography, "Territory", true) == 0)
            {
                if (!string.IsNullOrEmpty(newFilters["Territory_ID"]))
                {
                    newFilters.Add("Geography", newFilters["Territory_ID"]);
                    newFilters.Add("Prescriber_Geography_Type", "territory");
                    SectionTitle = ma.GetPrescriberGeographyName(newFilters["Territory_ID"], "territory");
                }
                else
                    Visible = false;
            }

            return new PrescriberTrendingQueryDefinition(EntityTypeName, newFilters);
        }

        protected override ColumnMap CreateColumnMap(System.Data.Common.DbDataRecord columnRecord)
        {
            ColumnMap map = base.CreateColumnMap(columnRecord);// new ColumnMap();

            //Check is ColumnMap is Product_Trx or Product_Mst
            if (map.PropertyName.IndexOf(string.Format("Product_{0}", newFilters["Trx_Mst"])) > -1)
            {
                IList<int> timeFrameVals = new List<int>();
                bool isMonth;
                string timeFrame;

                if (string.Compare(newFilters["Calendar_Rolling"], "Calendar", true) == 0)//It is calendar based
                {
                    if (string.IsNullOrEmpty(newFilters["Quarter_Selection"]))
                    {
                        timeFrame = newFilters["Month_Selection"];
                        isMonth = true;
                    }
                    else//Timeframe is quarter based
                    {
                        timeFrame = newFilters["Quarter_Selection"];
                        isMonth = false;
                    }
                }
                else //It is rolling based
                {
                    timeFrame = newFilters["Rolling_Selection"];
                    isMonth = false;
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

                List<int> timeFrames = timeFrameVals.ToList();
                //If Rolling Quarterly, reverse time frame values
                if (string.Compare(newFilters["Calendar_Rolling"], "Rolling", true) == 0)
                    timeFrames.Reverse();

                map.TranslatedName = ma.GetHeaderText(isMonth, timeFrames[Convert.ToInt32(map.TranslatedName)], newFilters);
            }

            return map;
        }
    }

    public class PrescriberTrendingDrilldownReportDefinitionBase : ReportDefinition
    {
        private NameValueCollection newFilters;

        MarketplaceAnalyticsProvider ma = new MarketplaceAnalyticsProvider();

        protected override QueryDefinition CreateQueryDefinition(NameValueCollection filters)
        {
            newFilters = new NameValueCollection(filters);

            if ((!string.IsNullOrEmpty(newFilters["IsDrilldown"]) && (Convert.ToBoolean(newFilters["IsDrilldown"]) == true)))
            {
                //if (string.Compare(newFilters["Selection_Clicked"], "1") == 0)
                //    newFilters.Add("Region_ID", newFilters["Region_ID"]);
                //if (string.Compare(newFilters["Selection_Clicked"], "2") == 0)
                //    newFilters.Add("District_ID", newFilters["District_ID"]);
                //if (string.Compare(newFilters["Selection_Clicked"], "3") == 0)
                //    newFilters.Add("Territory_ID", newFilters["Territory_ID"]);
                //Disable paging
                newFilters["PagingEnabled"] = "False";
                newFilters["Export"] = "True";
            }
            else
                Visible = false;

            return new PrescriberTrendingDrilldownQueryDefinition(EntityTypeName, newFilters);
        }

        protected override ColumnMap CreateColumnMap(System.Data.Common.DbDataRecord columnRecord)
        {
            ColumnMap map = base.CreateColumnMap(columnRecord);// new ColumnMap();

            //Check is ColumnMap is Product_Trx or Product_Mst
            if (map.PropertyName.IndexOf(string.Format("Product_{0}", newFilters["Trx_Mst"])) > -1)
            {
                IList<int> timeFrameVals = new List<int>();
                bool isMonth;
                string timeFrame;

                if (string.Compare(newFilters["Calendar_Rolling"], "Calendar", true) == 0)//It is calendar based
                {
                    if (string.IsNullOrEmpty(newFilters["Quarter_Selection"]))
                    {
                        timeFrame = newFilters["Month_Selection"];
                        isMonth = true;
                    }
                    else//Timeframe is quarter based
                    {
                        timeFrame = newFilters["Quarter_Selection"];
                        isMonth = false;
                    }
                }
                else //It is rolling based
                {
                    timeFrame = newFilters["Rolling_Selection"];
                    isMonth = false;
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

                map.TranslatedName = ma.GetHeaderText(isMonth, timeFrameVals[Convert.ToInt32(map.TranslatedName)], newFilters);
            }

            return map;
        }
    }

    public class PrescriberTrendingPopupReportDefinitionBase : ReportDefinition
    {
        private NameValueCollection newFilters;

        MarketplaceAnalyticsProvider ma = new MarketplaceAnalyticsProvider();

        protected override QueryDefinition CreateQueryDefinition(NameValueCollection filters)
        {
            //NameValueCollection newFilters = new NameValueCollection(filters);
            newFilters = new NameValueCollection(filters);

            //int geographyType = Convert.ToInt32(newFilters["Geography_Type"]);

            if (string.Compare(newFilters["Selection_Clicked"], "1", true) == 0)
            {
                if (!string.IsNullOrEmpty(newFilters["Region_ID"]))
                {
                    newFilters.Add("Geography", newFilters["Region_ID"]);
                    newFilters.Add("Prescriber_Geography_Type", "region");
                }
            }

            if (string.Compare(newFilters["Selection_Clicked"], "2", true) == 0)
            {
                if (!string.IsNullOrEmpty(newFilters["District_ID"]))
                {
                    newFilters.Add("Geography", newFilters["District_ID"]);
                    newFilters.Add("Prescriber_Geography_Type", "district");
                }
            }

            if (string.Compare(newFilters["Selection_Clicked"], "3", true) == 0)
            {
                if (!string.IsNullOrEmpty(newFilters["Territory_ID"]))
                {
                    newFilters.Add("Geography", newFilters["Territory_ID"]);
                    newFilters.Add("Prescriber_Geography_Type", "territory");
                }
            }

            //int geographyType = Convert.ToInt32(newFilters["Geography_Type"]);
            string val = newFilters["Physician_ID"];
            if (!string.IsNullOrEmpty(val))
            {
                // to get Plan_Name
                string prescriberName;
                using (PathfinderMarketplaceAnalyticsEntities context = new PathfinderMarketplaceAnalyticsEntities(Pinsonault.Web.Session.GetClientApplicationConnectionString("MarketplaceAnalytics")))
                {
                    //step 1: delete all the records for this business plan key contacts
                    //PlanInfo plan = new PlanInfo();
                    prescriberName = (from p in context.LkpPhysiciansSet
                                      where p.Physician_ID == val
                                      select p.Full_Name).FirstOrDefault();
                    SectionTitle = prescriberName;
                }
            }

            return new PrescriberTrendingPopupQueryDefinition(EntityTypeName, newFilters);
        }

        protected override ColumnMap CreateColumnMap(System.Data.Common.DbDataRecord columnRecord)
        {
            ColumnMap map = base.CreateColumnMap(columnRecord);// new ColumnMap();

            //Check is ColumnMap is Product_Trx or Product_Mst
            if (map.PropertyName.IndexOf(string.Format("Product_{0}", newFilters["Trx_Mst"])) > -1)
            {
                IList<int> timeFrameVals = new List<int>();
                bool isMonth;
                string timeFrame;

                if (string.Compare(newFilters["Calendar_Rolling"], "Calendar", true) == 0)//It is calendar based
                {
                    if (string.IsNullOrEmpty(newFilters["Quarter_Selection"]))
                    {
                        timeFrame = newFilters["Month_Selection"];
                        isMonth = true;
                    }
                    else//Timeframe is quarter based
                    {
                        timeFrame = newFilters["Quarter_Selection"];
                        isMonth = false;
                    }
                }
                else //It is rolling based
                {
                    timeFrame = newFilters["Rolling_Selection"];
                    isMonth = false;
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

                List<int> timeFrames = timeFrameVals.ToList();
                //If Rolling Quarterly, reverse time frame values
                if (string.Compare(newFilters["Calendar_Rolling"], "Rolling", true) == 0)
                    timeFrames.Reverse();

                map.TranslatedName = ma.GetHeaderText(isMonth, timeFrames[Convert.ToInt32(map.TranslatedName)], newFilters);
            }

            return map;
        }
    }

    // sl: Prescriber Report
    public class PrescriberReportDefinitionBase : ReportDefinition
    {
        private NameValueCollection newFilters;
        MarketplaceAnalyticsProvider ma = new MarketplaceAnalyticsProvider();

        protected override QueryDefinition CreateQueryDefinition(NameValueCollection filters)
        {
            newFilters = new NameValueCollection(filters);
            return new PrescriberQueryDefinition(filters);
        }


        protected override ColumnMap CreateColumnMap(System.Data.Common.DbDataRecord columnRecord)
        {
            ColumnMap map = base.CreateColumnMap(columnRecord);// new ColumnMap();


            // 5/16/2011:  Region/District/Territory are generic names and hierarchy
            // to get the correct Territory Level Name based on client
            int TerritoryLevelID = 0;
            if (map.TranslatedName == "Region_Name")
                TerritoryLevelID = 1;
            if (map.TranslatedName == "District_Name")
                TerritoryLevelID = 2;
            if (map.TranslatedName == "Territory_Name")
                TerritoryLevelID = 3;

            if (TerritoryLevelID > 0)
            {
                using (PathfinderMarketplaceAnalyticsEntities context = new PathfinderMarketplaceAnalyticsEntities(Pinsonault.Web.Session.GetClientApplicationConnectionString("MarketplaceAnalytics")))
                {

                    var tQ = from q in context.LkpMSTerritoryLevelsSet
                             where q.MS_Territory_Level_ID == TerritoryLevelID
                             select new { tName = q.MS_Territory_Level_Name };

                    if (tQ != null)
                    {
                        foreach (var i in tQ)
                        {
                            map.TranslatedName = i.tName;
                        }

                    }

                }
            }
            /////////////


            //Check is ColumnMap is Product_Trx or Product_Mst
            if (map.PropertyName.IndexOf(string.Format("Product_{0}", newFilters["Trx_Mst"])) > -1)
            {
                IList<int> timeFrameVals = new List<int>();
                bool isMonth = false;
                string timeFrame = "";


                if (string.IsNullOrEmpty(newFilters["Year1"]) || string.IsNullOrEmpty(newFilters["Year2"]))  // Trending
                {


                    if (string.Compare(newFilters["Calendar_Rolling"], "Calendar", true) == 0)//It is calendar based
                    {
                        if (string.IsNullOrEmpty(newFilters["Quarter_Selection"]))
                        {
                            timeFrame = newFilters["Month_Selection"];
                            isMonth = true;
                        }
                        else//Timeframe is quarter based
                        {
                            timeFrame = newFilters["Quarter_Selection"];
                            isMonth = false;
                        }
                    }
                    else //It is rolling based
                    {
                        timeFrame = newFilters["Rolling_Selection"];
                        isMonth = false;
                    }


                }
                else // Comparison
                {
                    if (newFilters["Selection_Clicked"] == "1")
                    {
                        //Selection1
                        if (string.Compare(newFilters["Calendar_Rolling"], "Calendar", true) == 0)//It is calendar based
                        {
                            if (newFilters["MonthQuarter1"] == "1") //The timeframe is quarter based
                            {
                                timeFrame = newFilters["MonthQuarterSelection1"];
                                isMonth = false;
                            }
                            else //The timeframe is month based
                            {
                                timeFrame = newFilters["MonthQuarterSelection1"];
                                isMonth = true;
                            }
                        }
                        else //The timeframe is rolling quarter based
                        {
                            timeFrame = newFilters["RollingQuarterSelection1"];
                            isMonth = false;
                        }
                    }

                    if (newFilters["Selection_Clicked"] == "2")
                    {
                        //Selection2
                        if (string.Compare(newFilters["Calendar_Rolling"], "Calendar", true) == 0)//It is calendar based
                        {
                            if (newFilters["MonthQuarter1"] == "1") //The timeframe is quarter based
                            {
                                timeFrame = newFilters["MonthQuarterSelection2"];
                                isMonth = false;
                            }
                            else //The timeframe is month based
                            {
                                timeFrame = newFilters["MonthQuarterSelection2"];
                                isMonth = true;
                            }
                        }
                        else //The timeframe is rolling quarter based
                        {
                            timeFrame = newFilters["RollingQuarterSelection2"];
                            isMonth = false;
                        }
                    }

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

                List<int> timeFrames = timeFrameVals.ToList();
                //If Rolling Quarterly, reverse time frame values
                if (string.Compare(newFilters["Calendar_Rolling"], "Rolling", true) == 0)
                    timeFrames.Reverse();

                map.TranslatedName = ma.GetHeaderText(isMonth, timeFrames[Convert.ToInt32(map.TranslatedName)], newFilters);

            }

            return map;
        }
    }


    public class PrescriberReportDefinition : PrescriberReportDefinitionBase
    {
        protected override QueryDefinition CreateQueryDefinition(NameValueCollection filters)
        {
            return base.CreateQueryDefinition(filters);
        }
    }
    // sl end


    public class TrendingDrilldownReportDefinitionBase : ReportDefinition
    {
        private NameValueCollection newFilters;

        MarketplaceAnalyticsProvider ma = new MarketplaceAnalyticsProvider();

        protected override QueryDefinition CreateQueryDefinition(NameValueCollection filters)
        {
            newFilters = new NameValueCollection(filters);

            if ((!string.IsNullOrEmpty(newFilters["IsDrilldown"]) && (Convert.ToBoolean(newFilters["IsDrilldown"]) == true)))
            {
                if (string.Compare(newFilters["Selection_Clicked"], "1") == 0)
                    newFilters.Add("Geography", "US");
                if (string.Compare(newFilters["Selection_Clicked"], "2") == 0)
                {
                    int geographyType = Convert.ToInt32(newFilters["Geography_Type"]);

                    if (geographyType == 2)
                        newFilters.Add("Geography", newFilters["Region_ID"]);
                    if (geographyType == 3)
                        newFilters.Add("Geography", newFilters["Territory_ID"]);
                }
                if (string.Compare(newFilters["Selection_Clicked"], "3") == 0)
                    newFilters.Add("Geography", newFilters["State_ID"]);
            }
            else
                Visible = false;

            return new TrendingDrilldownQueryDefinition(EntityTypeName, newFilters);
        }

        protected override ColumnMap CreateColumnMap(System.Data.Common.DbDataRecord columnRecord)
        {
            ColumnMap map = base.CreateColumnMap(columnRecord);// new ColumnMap();

            //Check is ColumnMap is Product_Trx or Product_Mst
            if (map.PropertyName.IndexOf(string.Format("Product_{0}", newFilters["Trx_Mst"])) > -1)
            {
                IList<int> timeFrameVals = new List<int>();
                bool isMonth;
                string timeFrame;

                if (string.Compare(newFilters["Calendar_Rolling"], "Calendar", true) == 0)//It is calendar based
                {
                    if (string.IsNullOrEmpty(newFilters["Quarter_Selection"]))
                    {
                        timeFrame = newFilters["Month_Selection"];
                        isMonth = true;
                    }
                    else//Timeframe is quarter based
                    {
                        timeFrame = newFilters["Quarter_Selection"];
                        isMonth = false;
                    }
                }
                else //It is rolling based
                {
                    timeFrame = newFilters["Rolling_Selection"];
                    isMonth = false;
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

                map.TranslatedName = ma.GetHeaderText(isMonth, timeFrameVals[Convert.ToInt32(map.TranslatedName)], newFilters);
            }

            return map;
        }
    }

    public class ComparisonReportDefinitionBase : ReportDefinition
    {
        public int Selection { get; set; }

        private NameValueCollection newFilters;

        protected override QueryDefinition CreateQueryDefinition(NameValueCollection filters)
        {
            newFilters = new NameValueCollection(filters);

            SectionTitle = string.Format("Selection {0}", Selection);

            newFilters.Add("Selection", Selection.ToString());

            //Add Geography
            int geographyType = Convert.ToInt32(newFilters["Geography_Type"]);

            if (geographyType == 2)
                newFilters.Add("Geography", filters["Region_ID"]);
            if (geographyType == 3)
                newFilters.Add("Geography", filters["Territory_ID"]);
            if ((geographyType == 1 || geographyType == 2) && !string.IsNullOrEmpty(newFilters["State_ID"]))
                newFilters.Add("Geography", filters["State_ID"]);
            else
                newFilters.Add("Geography", "US");

            return new ComparisonQueryDefinition(EntityTypeName, newFilters);
        }

        protected override ColumnMap CreateColumnMap(System.Data.Common.DbDataRecord columnRecord)
        {
            ColumnMap map = base.CreateColumnMap(columnRecord);// new ColumnMap();

            //Check is ColumnMap is Product_Trx or Product_Mst
            if (map.PropertyName.IndexOf(string.Format("Product_{0}", newFilters["Trx_Mst"])) > -1)
                map.TranslatedName = newFilters["Trx_Mst"];

            return map;
        }
    }

    public class ComparisonDrilldownReportDefinitionBase : ReportDefinition
    {
        private NameValueCollection newFilters;

        protected override QueryDefinition CreateQueryDefinition(NameValueCollection filters)
        {
            newFilters = new NameValueCollection(filters);

            if ((!string.IsNullOrEmpty(newFilters["IsDrilldown"]) && (Convert.ToBoolean(newFilters["IsDrilldown"]) == true)))
            {
                //Add Geography
                int geographyType = Convert.ToInt32(newFilters["Geography_Type"]);

                if (geographyType == 2)
                    newFilters.Add("Geography", filters["Region_ID"]);
                if (geographyType == 3)
                    newFilters.Add("Geography", filters["Territory_ID"]);
                if ((geographyType == 1 || geographyType == 2) && !string.IsNullOrEmpty(newFilters["State_ID"]))
                    newFilters.Add("Geography", filters["State_ID"]);
                else
                    newFilters.Add("Geography", "US");
            }
            else
                Visible = false;

            return new ComparisonDrilldownQueryDefinition(EntityTypeName, newFilters);
        }

        protected override ColumnMap CreateColumnMap(System.Data.Common.DbDataRecord columnRecord)
        {
            ColumnMap map = base.CreateColumnMap(columnRecord);// new ColumnMap();

            //Check is ColumnMap is Product_Trx or Product_Mst
            if (map.PropertyName.IndexOf(string.Format("Product_{0}", newFilters["Trx_Mst"])) > -1)
            {
                //IList<int> timeFrameVals = new List<int>();
                //bool isMonth;
                //string timeFrame;

                //if (string.Compare(newFilters["Calendar_Rolling"], "Calendar", true) == 0)//It is calendar based
                //{
                //    if (string.IsNullOrEmpty(newFilters["Quarter_Selection"]))
                //    {
                //        timeFrame = newFilters["Month_Selection"];
                //        isMonth = true;
                //    }
                //    else//Timeframe is quarter based
                //    {
                //        timeFrame = newFilters["Quarter_Selection"];
                //        isMonth = false;
                //    }
                //}
                //else //It is rolling based
                //{
                //    timeFrame = newFilters["Rolling_Selection"];
                //    isMonth = false;
                //}

                ////Add time frame values to a list for processing
                //if (timeFrame.IndexOf(',') > -1)
                //{
                //    string[] timeFrameArr = timeFrame.Split(',');

                //    foreach (string s in timeFrameArr)
                //        timeFrameVals.Add(Convert.ToInt32(s));
                //}
                //else
                //    timeFrameVals.Add(Convert.ToInt32(timeFrame));

                //MarketplaceAnalyticsProvider ma = new MarketplaceAnalyticsProvider();

                //map.TranslatedName = ma.GetHeaderText(isMonth, timeFrameVals[Convert.ToInt32(map.TranslatedName)], newFilters);

                map.TranslatedName = newFilters["Trx_Mst"];
            }

            return map;
        }
    }

    public class AffiliationReportDefinitionBase : ReportDefinition
    {
        public string Geography { get; set; }
        public bool IsPlanLevel { get; set; }

        private NameValueCollection newFilters;

        MarketplaceAnalyticsProvider ma = new MarketplaceAnalyticsProvider();

        protected override QueryDefinition CreateQueryDefinition(NameValueCollection filters)
        {
            //NameValueCollection newFilters = new NameValueCollection(filters);
            newFilters = new NameValueCollection(filters);
            //if (!string.IsNullOrEmpty(newFilters["Plan_ID"]))
            //{
            //    string[] arrPlan_Section_Segment = newFilters["Plan_ID"].Split('_');
            //    IList<int> Plan_Section_SegmentVals = new List<int>();

            //    foreach (string s in arrPlan_Section_Segment)
            //        Plan_Section_SegmentVals.Add(Convert.ToInt32(s));

            //    newFilters.Remove("Plan_ID");
            //    newFilters.Add("Plan_ID", Plan_Section_SegmentVals[0].ToString());
            //    newFilters.Add("Section_ID", Plan_Section_SegmentVals[1].ToString());
            //}

            if (string.Compare(Geography, "National", true) == 0)
            {
                newFilters.Add("Geography", "US");
                SectionTitle = "National";
                //return new TrendingQueryDefinition(EntityTypeName, newFilters);
            }
            else if (IsPlanLevel)
            {
                SectionTitle = "Plan Level";
                return new AffiliationQueryDefinition(EntityTypeName, newFilters);
            }
            //nation view same as national view for trending report
            return new TrendingQueryDefinition(EntityTypeName, newFilters);
        }

        protected override ColumnMap CreateColumnMap(System.Data.Common.DbDataRecord columnRecord)
        {
            ColumnMap map = base.CreateColumnMap(columnRecord);// new ColumnMap();

            //Check is ColumnMap is Product_Trx or Product_Mst
            if (map.PropertyName.IndexOf(string.Format("Product_{0}", newFilters["Trx_Mst"])) > -1)
            {
                IList<int> timeFrameVals = new List<int>();
                bool isMonth;
                string timeFrame;

                if (string.Compare(newFilters["Calendar_Rolling"], "Calendar", true) == 0)//It is calendar based
                {
                    if (string.IsNullOrEmpty(newFilters["Quarter_Selection"]))
                    {
                        timeFrame = newFilters["Month_Selection"];
                        isMonth = true;
                    }
                    else//Timeframe is quarter based
                    {
                        timeFrame = newFilters["Quarter_Selection"];
                        isMonth = false;
                    }
                }
                else //It is rolling based
                {
                    timeFrame = newFilters["Rolling_Selection"];
                    isMonth = false;
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

                List<int> timeFrames = timeFrameVals.ToList();
                //If Rolling Quarterly, reverse time frame values
                if (string.Compare(newFilters["Calendar_Rolling"], "Rolling", true) == 0)
                    timeFrames.Reverse();

                map.TranslatedName = ma.GetHeaderText(isMonth, timeFrames[Convert.ToInt32(map.TranslatedName)], newFilters);
            }

            return map;
        }
    }

    public class AffiliationDrilldownReportDefinitionBase : ReportDefinition
    {
        private NameValueCollection newFilters;

        MarketplaceAnalyticsProvider ma = new MarketplaceAnalyticsProvider();

        protected override QueryDefinition CreateQueryDefinition(NameValueCollection filters)
        {
            newFilters = new NameValueCollection(filters);

            //if (!string.IsNullOrEmpty(newFilters["Plan_ID"]))
            //{
            //    string[] arrPlan_Section_Segment = newFilters["Plan_ID"].Split('_');
            //    IList<int> Plan_Section_SegmentVals = new List<int>();

            //    foreach (string s in arrPlan_Section_Segment)
            //        Plan_Section_SegmentVals.Add(Convert.ToInt32(s));

            //    newFilters.Remove("Plan_ID");
            //    newFilters.Add("Plan_ID", Plan_Section_SegmentVals[0].ToString());
            //    newFilters.Add("Section_ID", Plan_Section_SegmentVals[1].ToString());
            //}

            if ((!string.IsNullOrEmpty(newFilters["IsDrilldown"]) && (Convert.ToBoolean(newFilters["IsDrilldown"]) == true)))
            {
                if (string.Compare(newFilters["Selection_Clicked"], "1") == 0)
                {
                    newFilters.Add("Geography", "US");
                    //return new TrendingDrilldownQueryDefinition(EntityTypeName, newFilters);
                }
                if (string.Compare(newFilters["Selection_Clicked"], "2") == 0)
                {
                    //newFilters.Add("Geography", newFilters["Region_ID"]);
                    return new AffiliationDrilldownQueryDefinition(EntityTypeName, newFilters);
                }
                
            }
            else
                Visible = false;

            //nation view same as national view for trending report
            return new TrendingDrilldownQueryDefinition(EntityTypeName, newFilters);
        }

        protected override ColumnMap CreateColumnMap(System.Data.Common.DbDataRecord columnRecord)
        {
            ColumnMap map = base.CreateColumnMap(columnRecord);// new ColumnMap();

            //Check is ColumnMap is Product_Trx or Product_Mst
            if (map.PropertyName.IndexOf(string.Format("Product_{0}", newFilters["Trx_Mst"])) > -1)
            {
                IList<int> timeFrameVals = new List<int>();
                bool isMonth;
                string timeFrame;

                if (string.Compare(newFilters["Calendar_Rolling"], "Calendar", true) == 0)//It is calendar based
                {
                    if (string.IsNullOrEmpty(newFilters["Quarter_Selection"]))
                    {
                        timeFrame = newFilters["Month_Selection"];
                        isMonth = true;
                    }
                    else//Timeframe is quarter based
                    {
                        timeFrame = newFilters["Quarter_Selection"];
                        isMonth = false;
                    }
                }
                else //It is rolling based
                {
                    timeFrame = newFilters["Rolling_Selection"];
                    isMonth = false;
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

                map.TranslatedName = ma.GetHeaderText(isMonth, timeFrameVals[Convert.ToInt32(map.TranslatedName)], newFilters);
            }

            return map;
        }
    }

    public class FormularyHistoryReportDefinitionBase : ReportDefinition
    {
        public string Plan { get; set; }

        private NameValueCollection newFilters;

        MarketplaceAnalyticsProvider ma = new MarketplaceAnalyticsProvider();

        protected override QueryDefinition CreateQueryDefinition(NameValueCollection filters)
        {
            newFilters = new NameValueCollection(filters);
            newFilters.Add("Plan_ID", newFilters[Plan]);
            SectionTitle = ma.GetPlanName(Convert.ToInt32(newFilters[Plan]));

            return new FormularyHistoryQueryDefinition(EntityTypeName, newFilters);
        }

        protected override ColumnMap CreateColumnMap(System.Data.Common.DbDataRecord columnRecord)
        {
            ColumnMap map = base.CreateColumnMap(columnRecord);// new ColumnMap();

            //Check is ColumnMap is Product_Trx or Product_Mst
            if (map.PropertyName.IndexOf(string.Format("Product_{0}", newFilters["Trx_Mst"])) > -1)
            {
                IList<int> timeFrameVals = new List<int>();
                bool isMonth;
                string timeFrame = newFilters["Timeframe"];

                //If Med-D, timeframe is month based
                if (Convert.ToInt32(newFilters["Section_ID"]) == 17)
                {
                    //The timeframe is month based
                    isMonth = true;
                }
                else
                {
                    //The timeframe is quarter based
                    isMonth = false;
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

                List<int> timeFrames = timeFrameVals.ToList();

                map.TranslatedName = ma.GetHeaderText(isMonth, timeFrames[Convert.ToInt32(map.TranslatedName)], newFilters);
            }

            return map;
        }
    }

    public class FormularyHistoryDrilldownReportDefinitionBase : ReportDefinition
    {
        private NameValueCollection newFilters;

        MarketplaceAnalyticsProvider ma = new MarketplaceAnalyticsProvider();

        protected override QueryDefinition CreateQueryDefinition(NameValueCollection filters)
        {
            newFilters = new NameValueCollection(filters);

            if ((!string.IsNullOrEmpty(newFilters["IsDrilldown"]) && (Convert.ToBoolean(newFilters["IsDrilldown"]) == true)))
                newFilters.Add("Plan_ID", filters[string.Format("Plan_ID{0}", newFilters["Selection_Clicked"])]);
            else
                Visible = false;

            return new FormularyHistoryDrilldownQueryDefinition(EntityTypeName, newFilters);
        }

        protected override ColumnMap CreateColumnMap(System.Data.Common.DbDataRecord columnRecord)
        {
            ColumnMap map = base.CreateColumnMap(columnRecord);// new ColumnMap();

            //Check is ColumnMap is Product_Trx or Product_Mst
            if (map.PropertyName.IndexOf(string.Format("Product_{0}", newFilters["Trx_Mst"])) > -1)
            {
                IList<int> timeFrameVals = new List<int>();
                bool isMonth;
                string timeFrame = newFilters["Timeframe"];

                //If Med-D, timeframe is month based
                if (Convert.ToInt32(newFilters["Section_ID"]) == 17)
                {
                    //The timeframe is month based
                    isMonth = true;
                }
                else
                {
                    //The timeframe is quarter based
                    isMonth = false;
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

                map.TranslatedName = ma.GetHeaderText(isMonth, timeFrameVals[Convert.ToInt32(map.TranslatedName)], newFilters);
            }

            return map;
        }
    }

    public class FormularyHistoryModalReportDefinitionBase : ReportDefinition
    {
        private NameValueCollection newFilters;

        MarketplaceAnalyticsProvider ma = new MarketplaceAnalyticsProvider();

        protected override QueryDefinition CreateQueryDefinition(NameValueCollection filters)
        {
            newFilters = new NameValueCollection(filters);
            //newFilters.Add("Plan_ID", newFilters["Plan_ID"]);
            SectionTitle = ma.GetPlanName(Convert.ToInt32(newFilters["Plan_ID"]));

            return new FormularyHistoryModalQueryDefinition(EntityTypeName, newFilters);
        }

        protected override ColumnMap CreateColumnMap(System.Data.Common.DbDataRecord columnRecord)
        {
            ColumnMap map = base.CreateColumnMap(columnRecord);// new ColumnMap();

            IList<int> timeFrameVals = new List<int>();
            bool isMonth;
            int timeFrame = Convert.ToInt32(newFilters["Timeframe"]);
            MarketplaceAnalyticsProvider ma = new MarketplaceAnalyticsProvider();

            //If Med-D, timeframe is month based
            if (Convert.ToInt32(newFilters["Section_ID"]) == 17)
            {
                //The timeframe is month based
                isMonth = true;
            }
            else
            {
                //The timeframe is quarter based
                isMonth = false;
            }

            //Add time frame values to a list for processing
            timeFrameVals.Add(timeFrame);

            List<int> timeFrames = timeFrameVals.ToList();

            string secondHeaderText = "";

            //Check is ColumnMap is Product_Trx or Product_Mst
            if (map.PropertyName.IndexOf(string.Format("Product_{0}", newFilters["Trx_Mst"])) > -1)
            {
                secondHeaderText = ma.GetHeaderText(isMonth, timeFrames[Convert.ToInt32(map.TranslatedName)], newFilters);
                map.TranslatedName = string.Format("{0} {1}", newFilters["Trx_Mst"], ma.GetHeaderText(isMonth, timeFrames[Convert.ToInt32(map.TranslatedName)], newFilters));                
            }


            //Check if Tier, PA, QL, ST CoPay or Status
            if (map.TranslatedName.IndexOf("{0}") > -1)
            {
                //Check if previous or current timeframe
                if (map.PropertyName.IndexOf("0") > -1) //Previous timeframe
                {
                    secondHeaderText = ma.GetHeaderText(isMonth, ma.GetPreviousTimeFrame(timeFrame, Convert.ToInt32(newFilters["Section_ID"])), newFilters);
                    map.TranslatedName = string.Format(map.TranslatedName, ma.GetHeaderText(isMonth, ma.GetPreviousTimeFrame(timeFrame, Convert.ToInt32(newFilters["Section_ID"])), newFilters));                    
                }
                else
                {
                    secondHeaderText = ma.GetHeaderText(isMonth, timeFrame, newFilters);
                    map.TranslatedName = string.Format(map.TranslatedName, ma.GetHeaderText(isMonth, timeFrame, newFilters));
                }
            }

            if (map.PropertyName.IndexOfAny(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' }) > -1)
            {
                int itimeframeindex = Convert.ToInt32(map.PropertyName.Substring(map.PropertyName.IndexOfAny(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' })));

                string strRepeatedHeader = "H";
                if (itimeframeindex > 0)
                    strRepeatedHeader = "R";

                string[] firstHeader = map.TranslatedName.Split(' ');

                string firstHeaderName = firstHeader[0];

                map.FirstHeaderTranslatedName = firstHeaderName;
                if (map.PropertyName.IndexOf(newFilters["Trx_Mst"]) > -1)
                    map.MergedCellSpan = 1;
                else
                    map.MergedCellSpan = 2;

                if (string.IsNullOrEmpty(secondHeaderText))
                    map.SecondHeaderTranslatedName = map.TranslatedName;
                else
                    map.SecondHeaderTranslatedName = secondHeaderText;

                map.HeaderRepeaterCell = strRepeatedHeader;
                map.DBColToCompare = null;
            }

            return map;
        }
    }


}