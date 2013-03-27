using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;
using PathfinderModel ;
using PathfinderClientModel;
using System.Data.Objects;
using Pinsonault.Data.Reports;
using Pinsonault.Data;

namespace Pinsonault.Application.MarketplaceAnalytics
{
    public class TrendingReport : Report
    {
        protected override Dictionary<string, CriteriaItem> LoadCriteriaItems(NameValueCollection filters)
        {
            MarketplaceAnalyticsProvider ma = new MarketplaceAnalyticsProvider();

            //Get base items - includes Market Basket and Section (among others)
            Dictionary<string, CriteriaItem> items = base.LoadCriteriaItems(filters);

            //return results
            return ma.LoadCriteriaItems(filters, items, QueryContext, "trending");
        }

        protected override ObjectContext CreateObjectContext(PathfinderEntities context, bool IsCustom)
        {
            return CreateClientContext();
        }

        protected override void BuildReportDefinitions()
        {
            string trxMst = FilterSets[0]["Trx_Mst"];
            //bool isCalendar = false;
            string monthQtr;
            string timeFrame;
            IList<int> timeFrameVals = new List<int>();
            IList<string> tiles = new List<string>();

            //Check which type of timeframe to query by as well as Calendar or Rolling
            if (string.Compare(FilterSets[0]["Calendar_Rolling"], "Calendar", true) == 0)
            {
                //isCalendar = true;

                if (!string.IsNullOrEmpty(FilterSets[0]["Quarter_Selection"]))
                {
                    timeFrame = FilterSets[0]["Quarter_Selection"];
                    monthQtr = "Qtr";
                }
                else
                {
                    timeFrame = FilterSets[0]["Month_Selection"];
                    monthQtr = "Month";
                }
            }
            else
            {
                //isCalendar = false;
                monthQtr = "Qtr";
                timeFrame = FilterSets[0]["Rolling_Selection"];
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

                //Add tiles for summary
                //Base tile for summary
                tiles.Add("Tile3ToolsBase");

                ////Tiles based on timeframe
                ////Reverse timeframe values if rolling
                //if (string.Compare(FilterSets[0]["Calendar_Rolling"], "Rolling", true) == 0)
                //{
                //    for (int x = timeFrameVals.Count; x > 0; x--)
                //    {
                //        tiles.Add(string.Format("Tile3Tools{0}{1}{2}", trxMst, monthQtr, x-1));
                //    }
                //}
                //else
                //{
                if (HttpContext.Current.User.IsInRole("mpa_decimal"))
                {
                    for (int x = 0; x < timeFrameVals.Count; x++)
                    {
                        tiles.Add(string.Format("Tile3ToolsDecimal{0}{1}{2}", trxMst, monthQtr, x));
                    }
                }
                else
                {
                    for (int x = 0; x < timeFrameVals.Count; x++)
                    {
                        tiles.Add(string.Format("Tile3Tools{0}{1}{2}", trxMst, monthQtr, x));
                    }
                }
                //}

                ReportDefinitions.Add(new ReportDefinition
                {
                    ReportKey = "trending",
                    ReportDefinitions = new ReportDefinition[]
                {
                    //Report Defs are nested because they share same query string data - also changing EntityTypeName based on Section_ID filter if available
                    new TrendingReportDefinitionBase { ReportKey="trending", Tile= string.Join(", ", tiles.ToArray()), Geography="National"},
                    new TrendingReportDefinitionBase { ReportKey="trending", Tile= string.Join(", ", tiles.ToArray()), Geography="Regional"},
                    new TrendingReportDefinitionBase { ReportKey="trending", Tile= string.Join(", ", tiles.ToArray()), Geography="State"}                              
                }
                });

                //Clear tiles and add tiles for Drilldown
                tiles.Clear();
                tiles.Add("Tile4ToolsBase");

                //Show Lives, Tier, Co-Pay and Restrictions columns if 'Employer' or 'Other' is NOT selected
                if (FilterSets[0]["Section_ID"] != "14" && FilterSets[0]["Section_ID"] != "8")
                    tiles.Add("Tile4ToolsOptional");

                //Show this column if not State Medicaid (SM). Used for showing Tier if Section is not SM
                if (FilterSets[0]["Section_ID"] != "9")
                    tiles.Add("Tile4ToolsOptionalNotSM");

                //Tiles based on timeframe
                if (HttpContext.Current.User.IsInRole("mpa_decimal"))
                {
                    for (int x = 0; x < timeFrameVals.Count; x++)
                    {
                        tiles.Add(string.Format("Tile3ToolsDecimal{0}{1}{2}", trxMst, monthQtr, x));
                    }
                }
                else
                {
                    for (int x = 0; x < timeFrameVals.Count; x++)
                    {
                        tiles.Add(string.Format("Tile3Tools{0}{1}{2}", trxMst, monthQtr, x));
                    }
                }

                ReportDefinitions.Add(new TrendingDrilldownReportDefinitionBase { ReportKey = "trending", Tile = string.Join(", ", tiles.ToArray()), SectionTitle = "Trending Report Drilldown" });
            }
        }
    }

    public class PrescriberTrendingReport : Report
    {
        protected override Dictionary<string, CriteriaItem> LoadCriteriaItems(NameValueCollection filters)
        {
            MarketplaceAnalyticsProvider ma = new MarketplaceAnalyticsProvider();

            //Get base items - includes Market Basket and Section (among others)
            Dictionary<string, CriteriaItem> items = base.LoadCriteriaItems(filters);

            //return results
            return ma.LoadCriteriaItems(filters, items, QueryContext, "prescribertrending");
        }

        protected override ObjectContext CreateObjectContext(PathfinderEntities context, bool IsCustom)
        {
            return CreateClientContext();
        }

        protected override void BuildReportDefinitions()
        {
            string trxMst = FilterSets[0]["Trx_Mst"];
            string monthQtr;
            string timeFrame;
            IList<int> timeFrameVals = new List<int>();
            IList<string> tiles = new List<string>();

            //Check which type of timeframe to query by as well as Calendar or Rolling
            if (string.Compare(FilterSets[0]["Calendar_Rolling"], "Calendar", true) == 0)
            {
                if (!string.IsNullOrEmpty(FilterSets[0]["Quarter_Selection"]))
                {
                    timeFrame = FilterSets[0]["Quarter_Selection"];
                    monthQtr = "Qtr";
                }
                else
                {
                    timeFrame = FilterSets[0]["Month_Selection"];
                    monthQtr = "Month";
                }
            }
            else
            {
                monthQtr = "Qtr";
                timeFrame = FilterSets[0]["Rolling_Selection"];
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

                //Add tiles for summary
                //Base tile for summary
                tiles.Add("Tile3ToolsBase");

                if (HttpContext.Current.User.IsInRole("mpa_decimal"))
                {
                    for (int x = 0; x < timeFrameVals.Count; x++)
                    {
                        tiles.Add(string.Format("Tile3ToolsDecimal{0}{1}{2}", trxMst, monthQtr, x));
                    }
                }
                else
                {
                    for (int x = 0; x < timeFrameVals.Count; x++)
                    {
                        tiles.Add(string.Format("Tile3Tools{0}{1}{2}", trxMst, monthQtr, x));
                    }
                }

                ReportDefinitions.Add(new ReportDefinition
                {
                    ReportKey = "prescribertrending",
                    ReportDefinitions = new ReportDefinition[]
                {
                    //Report Defs are nested because they share same query string data - also changing EntityTypeName based on Section_ID filter if available
                    new PrescriberTrendingReportDefinitionBase { ReportKey="prescribertrending", Tile= string.Join(", ", tiles.ToArray()), Geography="Region"},
                    new PrescriberTrendingReportDefinitionBase { ReportKey="prescribertrending", Tile= string.Join(", ", tiles.ToArray()), Geography="District"},
                    new PrescriberTrendingReportDefinitionBase { ReportKey="prescribertrending", Tile= string.Join(", ", tiles.ToArray()), Geography="Territory"}                              
                }
                });

                //Clear tiles and add tiles for Drilldown
                tiles.Clear();
                tiles.Add("Tile4ToolsBase");

                //Add Geography
                if (FilterSets.Count > 1)
                    tiles.Add("Tile4ToolsBase" + FilterSets[1]["Selection_Clicked"]);

                //Tiles based on timeframe
                if (HttpContext.Current.User.IsInRole("mpa_decimal"))
                {
                    for (int x = 0; x < timeFrameVals.Count; x++)
                    {
                        tiles.Add(string.Format("Tile3ToolsDecimal{0}{1}{2}", trxMst, monthQtr, x));
                    }
                }
                else
                {
                    for (int x = 0; x < timeFrameVals.Count; x++)
                    {
                        tiles.Add(string.Format("Tile3Tools{0}{1}{2}", trxMst, monthQtr, x));
                    }
                }

                ReportDefinitions.Add(new PrescriberTrendingDrilldownReportDefinitionBase { ReportKey = "prescribertrending", Tile = string.Join(", ", tiles.ToArray()), SectionTitle = "Prescriber Trending Report Drilldown" });
            }
        }
    }

    public class PrescriberTrendingPopupReport : Report
    {
        protected override Dictionary<string, CriteriaItem> LoadCriteriaItems(NameValueCollection filters)
        {
            MarketplaceAnalyticsProvider ma = new MarketplaceAnalyticsProvider();

            //Get base items - includes Market Basket and Section (among others)
            Dictionary<string, CriteriaItem> items = base.LoadCriteriaItems(filters);

            //return results
            return ma.LoadCriteriaItems(filters, items, QueryContext, "prescribertrending");
        }

        protected override ObjectContext CreateObjectContext(PathfinderEntities context, bool IsCustom)
        {
            return CreateClientContext();
        }

        protected override void BuildReportDefinitions()
        {
            string trxMst = FilterSets[0]["Trx_Mst"];
            string monthQtr = null;
            string timeFrame;
            IList<int> timeFrameVals = new List<int>();
            IList<string> tiles = new List<string>();

            //Check which type of timeframe to query by as well as Calendar or Rolling
            if (string.Compare(FilterSets[0]["Calendar_Rolling"], "Calendar", true) == 0)
            {
                //isCalendar = true;

                if (!string.IsNullOrEmpty(FilterSets[0]["Quarter_Selection"]))
                {
                    timeFrame = FilterSets[0]["Quarter_Selection"];
                    monthQtr = "Qtr";
                }
                else
                {
                    timeFrame = FilterSets[0]["Month_Selection"];
                    monthQtr = "Month";
                }
            }
            else
            {
                //isCalendar = false;
                monthQtr = "Qtr";
                timeFrame = FilterSets[0]["Rolling_Selection"];
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

            tiles.Add("Tile3ToolsBase");
            tiles.Add("Tile4ToolsBase");

            //Show Lives, Tier, Co-Pay and Restrictions columns if 'Employer' or 'Other' is NOT selected
            if (FilterSets[0]["Section_ID"] != "14" && FilterSets[0]["Section_ID"] != "8")
                tiles.Add("Tile4ToolsOptional");

            //Show this column if not State Medicaid (SM). Used for showing Tier if Section is not SM
            if (FilterSets[0]["Section_ID"] != "9")
                tiles.Add("Tile4ToolsOptionalNotSM");

            //Tiles based on timeframe
            if (HttpContext.Current.User.IsInRole("mpa_decimal"))
            {
                for (int x = 0; x < timeFrameVals.Count; x++)
                {
                    tiles.Add(string.Format("Tile3ToolsDecimal{0}{1}{2}", trxMst, monthQtr, x));
                }
            }
            else
            {
                for (int x = 0; x < timeFrameVals.Count; x++)
                {
                    tiles.Add(string.Format("Tile3Tools{0}{1}{2}", trxMst, monthQtr, x));
                }
            }

            ReportDefinitions.Add(new PrescriberTrendingPopupReportDefinitionBase { ReportKey = "prescribertrendingpopup", Tile = string.Join(", ", tiles.ToArray()), SectionTitle = "Prescriber Trending Popup Report" });
        }
    }

    // sl: Prescriber Report
    public class PrescriberReport : Report
    {
        protected override Dictionary<string, CriteriaItem> LoadCriteriaItems(NameValueCollection filters)
        {
            MarketplaceAnalyticsProvider ma = new MarketplaceAnalyticsProvider();

            //Get base items - includes Market Basket and Section (among others)
            Dictionary<string, CriteriaItem> items = base.LoadCriteriaItems(filters);

            //return results
            return ma.LoadCriteriaItems(filters, items, QueryContext,"prescribers");
        }

        protected override ObjectContext CreateObjectContext(PathfinderEntities context, bool IsCustom)
        {
            return CreateClientContext();
        }

        protected override void BuildReportDefinitions()
        {
            string trxMst = FilterSets[0]["Trx_Mst"];
            bool isCalendar = false;
            string monthQtr=null;
            string timeFrame;
            IList<int> timeFrameVals = new List<int>();
            IList<string> tiles = new List<string>();


            if (string.IsNullOrEmpty(FilterSets[0]["Year1"]) || string.IsNullOrEmpty(FilterSets[0]["Year2"]))  // Trending
            {

                //Check which type of timeframe to query by as well as Calendar or Rolling
                if (string.Compare(FilterSets[0]["Calendar_Rolling"], "Calendar", true) == 0)
                {
                    //isCalendar = true;

                    if (!string.IsNullOrEmpty(FilterSets[0]["Quarter_Selection"]))
                    {
                        timeFrame = FilterSets[0]["Quarter_Selection"];
                        monthQtr = "Qtr";
                    }
                    else
                    {
                        timeFrame = FilterSets[0]["Month_Selection"];
                        monthQtr = "Month";
                    }
                }
                else
                {
                    //isCalendar = false;
                    monthQtr = "Qtr";
                    timeFrame = FilterSets[0]["Rolling_Selection"];
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

            }
            else //Comparison
            {
                if (string.Compare(FilterSets[0]["Calendar_Rolling"], "Calendar", true) == 0)
                    isCalendar = true;
                else
                    isCalendar = false;



                if (FilterSets[0]["Selection_Clicked"] == "1")
                {
                    //Selection1
                    if (isCalendar)
                    {
                        if (FilterSets[0]["MonthQuarter1"] == "1") //The timeframe is quarter based
                        {
                            timeFrameVals.Add(Convert.ToInt32(FilterSets[0]["MonthQuarterSelection1"]));
                            monthQtr = "Qtr";
                        }
                        else //The timeframe is month based
                        {
                            timeFrameVals.Add(Convert.ToInt32(FilterSets[0]["MonthQuarterSelection1"]));
                            monthQtr = "Month";
                        }
                    }
                    else //The timeframe is rolling quarter based
                    {
                        timeFrameVals.Add(Convert.ToInt32(FilterSets[0]["RollingQuarterSelection1"]));
                        monthQtr = "Qtr";
                    }
                }

                if (FilterSets[0]["Selection_Clicked"] == "2")
                {
                    //Selection2
                    if (isCalendar)
                    {
                        if (FilterSets[0]["MonthQuarter1"] == "1") //The timeframe is quarter based
                        {
                            timeFrameVals.Add(Convert.ToInt32(FilterSets[0]["MonthQuarterSelection2"]));
                            monthQtr = "Qtr";
                        }
                        else //The timeframe is month based
                        {
                            timeFrameVals.Add(Convert.ToInt32(FilterSets[0]["MonthQuarterSelection2"]));
                            monthQtr = "Month";
                        }
                    }
                    else //The timeframe is rolling quarter based
                    {
                        timeFrameVals.Add(Convert.ToInt32(FilterSets[0]["RollingQuarterSelection2"]));
                        monthQtr = "Qtr";
                    }

                }

            }


            // Prescriber Grid
            tiles.Clear();
            tiles.Add("PrescriberBase");
            tiles.Add("Tile3ToolsBase");

            ////Add MBTrx only if MST and 1 timeframe selected

            //if ((string.Compare(trxMst, "trx", true) == 0) && timeFrameVals.Count == 1)
            //    tiles.Add("Tile4MB_Trx");


            ////Add Segment name only if Combined channel is selected
            //if (FilterSets[0]["Section_ID"] == "-1")
            //    tiles.Add("Tile4Segment_Name");

            //Tiles based on timeframe
            if (HttpContext.Current.User.IsInRole("mpa_decimal"))
            {
                for (int x = 0; x < timeFrameVals.Count; x++)
                {
                    tiles.Add(string.Format("Tile3ToolsDecimal{0}{1}{2}", trxMst, monthQtr, x));
                }
            }
            else
            {
                for (int x = 0; x < timeFrameVals.Count; x++)
                {
                    tiles.Add(string.Format("Tile3Tools{0}{1}{2}", trxMst, monthQtr, x));
                }
            }

            ReportDefinitions.Add(new PrescriberReportDefinitionBase { ReportKey = "prescribers", Tile = string.Join(", ", tiles.ToArray()), SectionTitle = "Prescriber Report" });

        }
    }
    // sl end

    public class ComparisonReport : Report
    {
        protected override Dictionary<string, CriteriaItem> LoadCriteriaItems(NameValueCollection filters)
        {
            MarketplaceAnalyticsProvider ma = new MarketplaceAnalyticsProvider();

            //Get base items - includes Market Basket and Section (among others)
            Dictionary<string, CriteriaItem> items = base.LoadCriteriaItems(filters);

            //return results
            return ma.LoadCriteriaItems(filters, items, QueryContext, "comparison");
        }

        protected override ObjectContext CreateObjectContext(PathfinderEntities context, bool IsCustom)
        {
            return CreateClientContext();
        }

        protected override void BuildReportDefinitions()
        {
            string trxMst = FilterSets[0]["Trx_Mst"];
            bool isCalendar = false;
            string monthQtr;
            IList<int> timeFrameVals1 = new List<int>();
            IList<int> timeFrameVals2 = new List<int>();
            IList<int> timeFrameVals3 = new List<int>();
            IList<string> tiles1 = new List<string>();
            IList<string> tiles2 = new List<string>();
            IList<string> tiles3 = new List<string>();

            if (string.Compare(FilterSets[0]["Calendar_Rolling"], "Calendar", true) == 0)
                isCalendar = true;
            else
                isCalendar = false;

            //SELECTION 1//
            if (isCalendar)
            {
                if (FilterSets[0]["MonthQuarter1"] == "1") //The timeframe is quarter based
                {
                    timeFrameVals1.Add(Convert.ToInt32(FilterSets[0]["MonthQuarterSelection1"]));
                    monthQtr = "Qtr";
                }
                else //The timeframe is month based
                {
                    timeFrameVals1.Add(Convert.ToInt32(FilterSets[0]["MonthQuarterSelection1"]));
                    monthQtr = "Month";
                }
            }
            else //The timeframe is rolling quarter based
            {
                timeFrameVals1.Add(Convert.ToInt32(FilterSets[0]["RollingQuarterSelection1"]));
                monthQtr = "Qtr";
            }

            //Add tiles for summary
            //Base tile for summary
            tiles1.Add("Tile3ToolsBase");

            //Tiles based on timeframe
            if (HttpContext.Current.User.IsInRole("mpa_decimal"))
            {
                for (int x = 0; x < timeFrameVals1.Count; x++)
                {
                    tiles1.Add(string.Format("Tile3ToolsDecimal{0}{1}{2}", trxMst, monthQtr, x));
                }
            }
            else
            {
                for (int x = 0; x < timeFrameVals1.Count; x++)
                {
                    tiles1.Add(string.Format("Tile3Tools{0}{1}{2}", trxMst, monthQtr, x));
                }
            }

            //SELECTION 2//
            if (isCalendar)
            {
                if (FilterSets[0]["MonthQuarter2"] == "1") //The timeframe is quarter based
                {
                    timeFrameVals2.Add(Convert.ToInt32(FilterSets[0]["MonthQuarterSelection2"]));
                    monthQtr = "Qtr";
                }
                else //The timeframe is month based
                {
                    timeFrameVals2.Add(Convert.ToInt32(FilterSets[0]["MonthQuarterSelection2"]));
                    monthQtr = "Month";
                }
            }
            else //The timeframe is rolling quarter based
            {
                timeFrameVals2.Add(Convert.ToInt32(FilterSets[0]["RollingQuarterSelection2"]));
                monthQtr = "Qtr";
            }

            //Add tiles for summary
            //Base tile for summary
            tiles2.Add("Tile3ToolsBase");

            //Tiles based on timeframe
            if (HttpContext.Current.User.IsInRole("mpa_decimal"))
            {
                for (int x = 0; x < timeFrameVals2.Count; x++)
                {
                    tiles2.Add(string.Format("Tile3ToolsDecimal{0}{1}{2}", trxMst, monthQtr, x));
                }
            }
            else
            {
                for (int x = 0; x < timeFrameVals2.Count; x++)
                {
                    tiles2.Add(string.Format("Tile3Tools{0}{1}{2}", trxMst, monthQtr, x));
                }
            }

            //SELECTION 3//
            if (isCalendar)
            {
                if (FilterSets[0]["MonthQuarter3"] == "1") //The timeframe is quarter based
                {
                    timeFrameVals3.Add(Convert.ToInt32(FilterSets[0]["MonthQuarterSelection3"]));
                    monthQtr = "Qtr";
                }
                else //The timeframe is month based
                {
                    timeFrameVals3.Add(Convert.ToInt32(FilterSets[0]["MonthQuarterSelection3"]));
                    monthQtr = "Month";
                }
            }
            else //The timeframe is rolling quarter based
            {
                timeFrameVals3.Add(Convert.ToInt32(FilterSets[0]["RollingQuarterSelection3"]));
                monthQtr = "Qtr";
            }

            //Add tiles for summary
            //Base tile for summary
            tiles3.Add("Tile3ToolsBase");

            //Tiles based on timeframe
            if (HttpContext.Current.User.IsInRole("mpa_decimal"))
            {
                for (int x = 0; x < timeFrameVals3.Count; x++)
                {
                    tiles3.Add(string.Format("Tile3ToolsDecimal{0}{1}{2}", trxMst, monthQtr, x));
                }
            }
            else
            {
                for (int x = 0; x < timeFrameVals3.Count; x++)
                {
                    tiles3.Add(string.Format("Tile3Tools{0}{1}{2}", trxMst, monthQtr, x));
                }
            }

            ReportDefinitions.Add(new ReportDefinition
            {
                ReportKey = "comparison",
                ReportDefinitions = new ReportDefinition[]
                {
                    //Report Defs are nested because they share same query string data - also changing EntityTypeName based on Section_ID filter if available
                    new ComparisonReportDefinitionBase { ReportKey="comparison", Tile= string.Join(", ", tiles1.ToArray()), Selection=1},
                    new ComparisonReportDefinitionBase { ReportKey="comparison", Tile= string.Join(", ", tiles2.ToArray()), Selection=2}//,
                    //new ComparisonReportDefinitionBase { ReportKey="comparison", Tile= string.Join(", ", tiles3.ToArray()), Selection=3}                              
                }
            });


            //DRILLDOWN
            if (FilterSets.Count > 1)
            {
                //Clear tiles and add tiles for Drilldown
                tiles1.Clear();
                tiles1.Add("Tile4ToolsBase");

                //Show Lives, Tier, Co-Pay and Restrictions columns if 'Employer' or 'Other' is NOT selected
                if (FilterSets[0]["Section_ID"] != "14" && FilterSets[0]["Section_ID"] != "8")
                    tiles1.Add("Tile4ToolsOptional");

                //Show this column if not State Medicaid (SM). Used for showing Tier if Section is not SM
                if (FilterSets[0]["Section_ID"] != "9")
                    tiles1.Add("Tile4ToolsOptionalNotSM");

                //if (string.Compare(trxMst, "trx", true) == 0)
                //    tiles1.Add("Tile4ToolsTrxSum");
                //else
                //    tiles1.Add("Tile4ToolsMstSum");

                timeFrameVals1.Clear();

                //SELECTION 1
                if (FilterSets[1]["Selection_Clicked"] == "1")
                {
                    if (isCalendar)
                    {
                        if (FilterSets[0]["MonthQuarter1"] == "1") //The timeframe is quarter based
                        {
                            timeFrameVals1.Add(Convert.ToInt32(FilterSets[0]["MonthQuarterSelection1"]));
                            monthQtr = "Qtr";
                        }
                        else //The timeframe is month based
                        {
                            timeFrameVals1.Add(Convert.ToInt32(FilterSets[0]["MonthQuarterSelection1"]));
                            monthQtr = "Month";
                        }
                    }
                    else //The timeframe is rolling quarter based
                    {
                        timeFrameVals1.Add(Convert.ToInt32(FilterSets[0]["RollingQuarterSelection1"]));
                        monthQtr = "Qtr";
                    }
                }
                //SELECTION 2
                if (FilterSets[1]["Selection_Clicked"] == "2")
                {
                    if (isCalendar)
                    {
                        if (FilterSets[0]["MonthQuarter2"] == "1") //The timeframe is quarter based
                        {
                            timeFrameVals1.Add(Convert.ToInt32(FilterSets[0]["MonthQuarterSelection2"]));
                            monthQtr = "Qtr";
                        }
                        else //The timeframe is month based
                        {
                            timeFrameVals1.Add(Convert.ToInt32(FilterSets[0]["MonthQuarterSelection2"]));
                            monthQtr = "Month";
                        }
                    }
                    else //The timeframe is rolling quarter based
                    {
                        timeFrameVals1.Add(Convert.ToInt32(FilterSets[0]["RollingQuarterSelection2"]));
                        monthQtr = "Qtr";
                    }
                }
                ////SELECTION 3
                //if (FilterSets[1]["Selection_Clicked"] == "3")
                //{
                //    if (isCalendar)
                //    {
                //        if (FilterSets[0]["MonthQuarter3"] == "1") //The timeframe is quarter based
                //        {
                //            timeFrameVals1.Add(Convert.ToInt32(FilterSets[0]["MonthQuarterSelection3"]));
                //            monthQtr = "Qtr";
                //        }
                //        else //The timeframe is month based
                //        {
                //            timeFrameVals1.Add(Convert.ToInt32(FilterSets[0]["MonthQuarterSelection3"]));
                //            monthQtr = "Month";
                //        }
                //    }
                //    else //The timeframe is rolling quarter based
                //    {
                //        timeFrameVals1.Add(Convert.ToInt32(FilterSets[0]["RollingQuarterSelection3"]));
                //        monthQtr = "Qtr";
                //    }
                //}

                //Tiles based on timeframe
                if (HttpContext.Current.User.IsInRole("mpa_decimal"))
                {
                    for (int x = 0; x < timeFrameVals1.Count; x++)
                    {
                        tiles1.Add(string.Format("Tile3ToolsDecimal{0}{1}{2}", trxMst, monthQtr, x));
                    }
                }
                else
                {
                    for (int x = 0; x < timeFrameVals1.Count; x++)
                    {
                        tiles1.Add(string.Format("Tile3Tools{0}{1}{2}", trxMst, monthQtr, x));
                    }
                }

                //Add MBTrx only if MST and 1 timeframe selected
                //if ((string.Compare(trxMst, "trx", true) == 0) && timeFrameVals1.Count == 1)
                //    tiles1.Add("Tile4MB_Trx");

                //Add Segment name only if Combined channel is selected
                if (FilterSets[0]["Section_ID"] == "-1")
                    tiles1.Add("Tile4Segment_Name");

                ReportDefinitions.Add(new ComparisonDrilldownReportDefinitionBase { ReportKey = "comparison", Tile = string.Join(", ", tiles1.ToArray()), SectionTitle = "Comparison Report Drilldown" });
            }
        }
    }

    public class AffiliationReport : Report
    {
        protected override Dictionary<string, CriteriaItem> LoadCriteriaItems(NameValueCollection filters)
        {
            MarketplaceAnalyticsProvider ma = new MarketplaceAnalyticsProvider();

            //Get base items - includes Market Basket and Section (among others)
            Dictionary<string, CriteriaItem> items = base.LoadCriteriaItems(filters);

            //return results
            return ma.LoadCriteriaItems(filters, items, QueryContext,"affiliation");
        }

        protected override ObjectContext CreateObjectContext(PathfinderEntities context, bool IsCustom)
        {
            return CreateClientContext();
        }

        protected override void BuildReportDefinitions()
        {

            string trxMst = FilterSets[0]["Trx_Mst"];
            //bool isCalendar = false;
            string monthQtr;
            string timeFrame;
            IList<int> timeFrameVals = new List<int>();
            IList<string> tiles = new List<string>();

            //Check which type of timeframe to query by as well as Calendar or Rolling
            if (string.Compare(FilterSets[0]["Calendar_Rolling"], "Calendar", true) == 0)
            {
                //isCalendar = true;

                if (!string.IsNullOrEmpty(FilterSets[0]["Quarter_Selection"]))
                {
                    timeFrame = FilterSets[0]["Quarter_Selection"];
                    monthQtr = "Qtr";
                }
                else
                {
                    timeFrame = FilterSets[0]["Month_Selection"];
                    monthQtr = "Month";
                }
            }
            else
            {
                //isCalendar = false;
                monthQtr = "Qtr";
                timeFrame = FilterSets[0]["Rolling_Selection"];
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

                //Add tiles for summary
                //Base tile for summary
                tiles.Add("Tile3ToolsBase");

                if (HttpContext.Current.User.IsInRole("mpa_decimal"))
                {
                    for (int x = 0; x < timeFrameVals.Count; x++)
                    {
                        tiles.Add(string.Format("Tile3ToolsDecimal{0}{1}{2}", trxMst, monthQtr, x));
                    }
                }
                else
                {
                    for (int x = 0; x < timeFrameVals.Count; x++)
                    {
                        tiles.Add(string.Format("Tile3Tools{0}{1}{2}", trxMst, monthQtr, x));
                    }
                }

                //summary report
                ReportDefinitions.Add(new ReportDefinition
                {
                    ReportKey = "affiliations",
                    ReportDefinitions = new ReportDefinition[]
                {
                    //Report Defs are nested because they share same query string data - also changing EntityTypeName based on Section_ID filter if available
                    new AffiliationReportDefinitionBase { ReportKey="affiliations", Tile= string.Join(", ", tiles.ToArray()), Geography="National" , IsPlanLevel = false},
                    
                    //change for plan level report
                    new AffiliationReportDefinitionBase { ReportKey="affiliations", Tile= string.Join(", ", tiles.ToArray()), Geography="", IsPlanLevel = true}                              
                }
                });

                //Clear tiles and add tiles for Drilldown
                tiles.Clear();
                tiles.Add("Tile4ToolsBase");

                //Add MBTrx only if MST and 1 timeframe selected
                //if ((string.Compare(trxMst, "trx", true) == 0) && timeFrameVals.Count == 1)
                //    tiles.Add("Tile4MB_Trx");

                //Add Segment name only if Combined channel is selected
                if (FilterSets[0]["Section_ID"] == "-1")
                    tiles.Add("Tile4Segment_Name");

                //Always set drilldown plan id selection to summary plan id
                if (FilterSets.Count > 1)
                    FilterSets[1]["Plan_ID"] = FilterSets[0]["Plan_ID"];

                //Tiles based on timeframe
                if (HttpContext.Current.User.IsInRole("mpa_decimal"))
                {
                    for (int x = 0; x < timeFrameVals.Count; x++)
                    {
                        tiles.Add(string.Format("Tile3ToolsDecimal{0}{1}{2}", trxMst, monthQtr, x));
                    }
                }
                else
                {
                    for (int x = 0; x < timeFrameVals.Count; x++)
                    {
                        tiles.Add(string.Format("Tile3Tools{0}{1}{2}", trxMst, monthQtr, x));
                    }
                }

                ReportDefinitions.Add(new AffiliationDrilldownReportDefinitionBase { ReportKey = "affiliations", Tile = string.Join(", ", tiles.ToArray()), SectionTitle = "Affiliation Report Drilldown" });
            }
        }
    }

    public class FormularyHistoryReport : Report
    {
        protected override Dictionary<string, CriteriaItem> LoadCriteriaItems(NameValueCollection filters)
        {
            MarketplaceAnalyticsProvider ma = new MarketplaceAnalyticsProvider();

            //Get base items - includes Market Basket and Section (among others)
            Dictionary<string, CriteriaItem> items = base.LoadCriteriaItems(filters);
            filters.Add("Calendar_Rolling", "Calendar");

            //return results
            return ma.LoadCriteriaItems(filters, items, QueryContext, "formularyhistory");
        }

        protected override ObjectContext CreateObjectContext(PathfinderEntities context, bool IsCustom)
        {
            return CreateClientContext();
        }

        protected override void BuildReportDefinitions()
        {
            string trxMst = FilterSets[0]["Trx_Mst"];
            string monthQtr;
            string timeFrame;
            IList<int> timeFrameVals = new List<int>();
            IList<string> tiles = new List<string>();
            
            timeFrame = FilterSets[0]["Timeframe"];

            //If Med-D, timeframe is month based
            if (Convert.ToInt32(FilterSets[0]["Section_ID"]) == 17)
            {
                //The timeframe is month based
                monthQtr = "Month";
            }
            else
            {
                //The timeframe is quarter based
                monthQtr = "Qtr";
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

                //Add tiles for summary
                //Base tile for summary
                tiles.Add("Tile3ToolsBase");

                if (HttpContext.Current.User.IsInRole("mpa_decimal"))
                {
                    for (int x = 0; x < timeFrameVals.Count; x++)
                    {
                        tiles.Add(string.Format("Tile3ToolsDecimal{0}{1}{2}", trxMst, monthQtr, x));
                    }
                }
                else
                {
                    for (int x = 0; x < timeFrameVals.Count; x++)
                    {
                        tiles.Add(string.Format("Tile3Tools{0}{1}{2}", trxMst, monthQtr, x));
                    }
                }

                ReportDefinitions.Add(new ReportDefinition
                {
                    ReportKey = "formularyhistory",
                    ReportDefinitions = new ReportDefinition[]
                {
                    //Report Defs are nested because they share same query string data - also changing EntityTypeName based on Section_ID filter if available
                    new FormularyHistoryReportDefinitionBase { ReportKey="formularyhistory", Tile= string.Join(", ", tiles.ToArray()), Plan="Plan_ID1"},
                    new FormularyHistoryReportDefinitionBase { ReportKey="formularyhistory", Tile= string.Join(", ", tiles.ToArray()), Plan="Plan_ID2"},
                }
                });

                //Clear tiles and add tiles for Drilldown
                tiles.Clear();
                tiles.Add("Tile4ToolsBase");

                //Tiles based on timeframe
                if (HttpContext.Current.User.IsInRole("mpa_decimal"))
                {
                    for (int x = 0; x < timeFrameVals.Count; x++)
                    {
                        tiles.Add(string.Format("Tile3ToolsDecimal{0}{1}{2}", trxMst, monthQtr, x));
                    }
                }
                else
                {
                    for (int x = 0; x < timeFrameVals.Count; x++)
                    {
                        tiles.Add(string.Format("Tile3Tools{0}{1}{2}", trxMst, monthQtr, x));
                    }
                }

                ReportDefinitions.Add(new FormularyHistoryDrilldownReportDefinitionBase { ReportKey = "formularyhistory", Tile = string.Join(", ", tiles.ToArray()), SectionTitle = "Formulary History Report Drilldown" });
            }
        }
    }

    public class FormularyHistoryModal : Report
    {
        protected override Dictionary<string, CriteriaItem> LoadCriteriaItems(NameValueCollection filters)
        {
            MarketplaceAnalyticsProvider ma = new MarketplaceAnalyticsProvider();

            //Get base items - includes Market Basket and Section (among others)
            Dictionary<string, CriteriaItem> items = base.LoadCriteriaItems(filters);
            filters.Add("Calendar_Rolling", "Calendar");

            //return results
            return ma.LoadCriteriaItems(filters, items, QueryContext, "formularyhistory");
        }

        protected override ObjectContext CreateObjectContext(PathfinderEntities context, bool IsCustom)
        {
            return CreateClientContext();
        }

        protected override void BuildReportDefinitions()
        {
            string trxMst = FilterSets[0]["Trx_Mst"];
            string monthQtr;
            string timeFrame;
            IList<int> timeFrameVals = new List<int>();
            IList<string> tiles = new List<string>();

            timeFrame = FilterSets[0]["Timeframe"];

            //If Med-D, timeframe is month based
            if (Convert.ToInt32(FilterSets[0]["Section_ID"]) == 17)
            {
                //The timeframe is month based
                monthQtr = "Month";
            }
            else
            {
                //The timeframe is quarter based
                monthQtr = "Qtr";
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

                //Add tiles for summary
                //Base tile for summary
                tiles.Add("Tile3ToolsBase");

                if (HttpContext.Current.User.IsInRole("mpa_decimal"))
                {
                    for (int x = 0; x < timeFrameVals.Count; x++)
                    {
                        tiles.Add(string.Format("Tile3ToolsDecimal{0}{1}{2}", trxMst, monthQtr, x));
                    }
                }
                else
                {
                    for (int x = 0; x < timeFrameVals.Count; x++)
                    {
                        tiles.Add(string.Format("Tile3Tools{0}{1}{2}", trxMst, monthQtr, x));
                    }
                }

                //Add Tier only if State Medicaid Section is not selected
                if (FilterSets[0]["Section_ID"] != "9")
                    tiles.Add("Tile3Tier");

                ReportDefinitions.Add(new ReportDefinition
                {
                    ReportKey = "formularyhistorymarketplacemodal",
                    Style = ReportStyle.DualHeaderGrid,
                    ReportDefinitions = new ReportDefinition[]
                {
                    //Report Defs are nested because they share same query string data - also changing EntityTypeName based on Section_ID filter if available
                    new FormularyHistoryModalReportDefinitionBase { ReportKey="formularyhistorymarketplacemodal", Style = ReportStyle.DualHeaderGrid, Tile= string.Join(", ", tiles.ToArray())},
                }
                });
            }
        }
    }
}