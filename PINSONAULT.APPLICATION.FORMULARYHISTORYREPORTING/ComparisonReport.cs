using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using Pinsonault.Data.Reports;
using PathfinderModel;
using PathfinderClientModel;
using System.Collections.Specialized;

namespace Pinsonault.Application.FormularyHistoryReporting
{
    public class ComparisonReport:Report 
    {
        protected override Dictionary<string, CriteriaItem> LoadCriteriaItems(NameValueCollection filters)
        {
            FHXProvider fhp = new FHXProvider();
          
            Dictionary<string, CriteriaItem> items = base.LoadCriteriaItems(filters);
            filters.Add("Calendar_Rolling", "Calendar");
            //add code to get Time Frame values selected

            //return results
            return fhp.LoadCriteriaItems(filters, items, QueryContext, "coFormularyHxComparison");
        }

        protected override ObjectContext CreateObjectContext(PathfinderEntities context, bool IsCustom)
        {
            return CreateClientContext();
        }

        protected override void BuildReportDefinitions()
        {           
            string monthQtr;
           
            IList<int> timeFrameVals = new List<int>();
            IList<int> display_optionsVals = new List<int>();
            IList<string> tiles = new List<string>();

            FHXProvider fhx = new FHXProvider();           

            int iSectionID = Convert.ToInt32(FilterSets[0]["Section_ID"]);
            timeFrameVals = fhx.GetTimeFrameVals(FilterSets[0], iSectionID, true);
            display_optionsVals = fhx.GetDisplayOptionList(FilterSets[0], iSectionID);

            //If Med-D, timeframe is month based
            if (FilterSets[0]["Monthly_Quarterly"] == "M")
            {
                //The timeframe is month based
                monthQtr = "Month";
            }
            else
            {
                //The timeframe is quarter based
                monthQtr = "Qtr";
            }
            //rdTimeFrameType=1
            
            //Add tiles for summary
            //Base tile for summary
            tiles.Add("Tile3ToolsBase");

            if (iSectionID == 17)
                tiles.Add("Tile3ToolsBaseMedD");
            else
                tiles.Add("Tile3ToolsBaseAll");

            for (int x = 0; x < timeFrameVals.Count; x++)
            {
                tiles.Add(string.Format("Tile3Tools{0}{1}", monthQtr, x));
            }

            string strTileDisplay = "Tile_DisplayID";

            foreach (int s in display_optionsVals)
                tiles.Add(string.Format("{0}_{1}", strTileDisplay, s));

            ReportDefinitions.Add(new ReportDefinition
            {
                ReportKey = "coFormularyHxComparison",
                Style = ReportStyle.DualHeaderGrid,
                ReportDefinitions = new ReportDefinition[]
            {
                //Report Defs are nested because they share same query string data - also changing EntityTypeName based on Section_ID filter if available
                new FHXReportDefinitionBase { ReportKey="coFormularyHxComparison", SectionTitle = Resources.Resource.SectionTitle_HxReport, Style = ReportStyle.DualHeaderGrid, Tile= string.Join(", ", tiles.ToArray())},
                
            }
            });
                
          
        }
    }
    public class RollingReport : Report
    {
        protected override Dictionary<string, CriteriaItem> LoadCriteriaItems(NameValueCollection filters)
        {
            FHXProvider fhp = new FHXProvider();

            Dictionary<string, CriteriaItem> items = base.LoadCriteriaItems(filters);
            filters.Add("Calendar_Rolling", "Rolling");
            //add code to get Time Frame values selected

            //return results
            return fhp.LoadCriteriaItems(filters, items, QueryContext, "coFormularyHxRolling");
        }

        protected override ObjectContext CreateObjectContext(PathfinderEntities context, bool IsCustom)
        {
            return CreateClientContext();
        }

        protected override void BuildReportDefinitions()
        {
            //IList<int> timeFrameVals = new List<int>();
            IList<int> display_optionsVals = new List<int>();
            IList<string> tiles = new List<string>();

            FHXProvider fhx = new FHXProvider();

            int iSectionID = Convert.ToInt32(FilterSets[0]["Section_ID"]);
            //timeFrameVals = fhx.GetRollingTimeFrameVals(FilterSets[0], iSectionID);
           
            display_optionsVals = fhx.GetDisplayOptionList(FilterSets[0], iSectionID);
           
            tiles.Add("Tile3ToolsBase");

            if (iSectionID == 17)
                tiles.Add("Tile3ToolsBaseMedD");
            else
                tiles.Add("Tile3ToolsBaseAll");

            string strTileDisplay = "Tile_DisplayID"; 

            int iTimeFrame;
            string monthQtr = string.Empty;

            if (FilterSets[0]["Monthly_Quarterly"] == "M")
            {
                iTimeFrame = Convert.ToInt32(FilterSets[0]["TimeFrameMonth"]);
                monthQtr = "Month";
            }
            else
            {
                iTimeFrame = Convert.ToInt32(FilterSets[0]["TimeFrameQtr"]);
                monthQtr = "Q";
            }

            for (int icount = 0; icount < iTimeFrame; icount++)
            {
                foreach (int s in display_optionsVals)
                    tiles.Add(string.Format("{0}_{1}_{2}", strTileDisplay, s, icount));
            }
           

            ReportDefinitions.Add(new ReportDefinition
            {
                ReportKey = "coFormularyHxRolling",
                Style = ReportStyle.DualHeaderGrid,
                ReportDefinitions = new ReportDefinition[]
            {                
                new FHXRollingReportDefinitionBase { ReportKey="coFormularyHxRolling", SectionTitle = Resources.Resource.SectionTitle_HxReport , Style = ReportStyle.DualHeaderGrid, Tile= string.Join(", ", tiles.ToArray())},
                
            }
            });


        }
    }

    public class TierCoverage : Report
    {
        protected override Dictionary<string, CriteriaItem> LoadCriteriaItems(NameValueCollection filters)
        {
            FHXProvider fhp = new FHXProvider();
            Dictionary<string, CriteriaItem> items = base.LoadCriteriaItems(filters);
            return fhp.LoadCriteriaItems(filters, items, QueryContext, "coTierCoverageHxFormulary");
        }
        protected override ObjectContext CreateObjectContext(PathfinderEntities context, bool IsCustom)
        {
            return CreateClientContext();
        }
        protected override void BuildReportDefinitions()
        {
            string monthQtr;

            IList<int> timeFrameVals = new List<int>();
            string[] ProductIDarr = FilterSets[0]["Product_ID"].Split(',');
            IList<string> tiles = new List<string>();
            IList<string> tileTrxSummary = new List<string>();
            IList<string> tileDrillDown = new List<string>();

            FHXProvider fhx = new FHXProvider();

            int iSectionID = Convert.ToInt32(FilterSets[0]["Section_ID"]);
            timeFrameVals.Add(Convert.ToInt32(FilterSets[0]["TimeFrame1"]));
            timeFrameVals.Add(Convert.ToInt32(FilterSets[0]["TimeFrame2"]));     
          

            //If Med-D, timeframe is month based
            if (FilterSets[0]["Monthly_Quarterly"] == "M")
            {
                //The timeframe is month based
                monthQtr = "Month";
            }
            else
            {
                //The timeframe is quarter based
                monthQtr = "Qtr";
            }    
            
            //sumary tiles
            tiles.Add("Tile_Tier");   //tier summary
            tileTrxSummary.Add("Tile_TrxMst"); //trx summary

            string strTileDisplay = string.Format("Tile_Tier_{0}_Drug", FilterSets[0]["Trx_Mst"]);
            tileTrxSummary.Add(string.Format("Tile_{0}", FilterSets[0]["Trx_Mst"]));                      
            
            //summary would not be pivoted by products
            //for (int s = 0; s < ProductIDarr.Count(); s++)
            //{
                tiles.Add(string.Format("{0}{1}", strTileDisplay, 1));                
            //}

            //Drilldown tiles
            tileDrillDown.Add("Tile_DrilldownBase");
            
            if (FilterSets.Count() > 1 && string.Compare(FilterSets[1]["Selection_Clicked"], "1") == 0)
            {
                tileDrillDown.Add("Tile_DrilldownTier");               
            }
            else if (FilterSets.Count() > 1 && string.Compare(FilterSets[1]["Selection_Clicked"], "2") == 0)
            {
                tileDrillDown.Add("Tile_DrilldownBaseTrx");
                tileDrillDown.Add("Tile_DrilldownTier");
                tileDrillDown.Add("Tile_DrilldownCoPay");
                tileDrillDown.Add("Tile_DrilldownRestrictions");
                tileDrillDown.Add(string.Format("Tile_Drilldown_{0}", FilterSets[0]["Trx_Mst"]));              
            }


            ReportDefinitions.Add(new ReportDefinition
            {
                ReportKey = "coTierCoverageHxFormulary",
                Style = ReportStyle.Grid,
                ReportDefinitions = new ReportDefinition[]
                {
                    //Report Defs are nested because they share same query string data - also changing EntityTypeName based on Section_ID filter if available
                    new FHXTierReportDefinitionBase { ReportKey="coTierCoverageHxFormulary", SectionTitle = "Tier Lives Summary", Style = ReportStyle.Grid, Tile= string.Join(", ", tiles.ToArray())},
                    new FHXTierRestrictionsTrxReportDefinitionBase { ReportKey="coTierCoverageHxFormulary", SectionTitle = "Rx Volume Summary", Style = ReportStyle.Grid, Tile= string.Join(", ", tileTrxSummary.ToArray())},                               
                }
                
            });
            ReportDefinitions.Add(new FHXTierRestrictionsDrilldownReportDefinitionBase { ReportKey = "coTierCoverageHxFormulary", SectionTitle = "Drilldown Report", Style = ReportStyle.Grid, Tile = string.Join(", ", tileDrillDown.ToArray())});


        }
    }

    public class Restrictions : Report
    {
        protected override Dictionary<string, CriteriaItem> LoadCriteriaItems(NameValueCollection filters)
        {
            FHXProvider fhp = new FHXProvider();
            Dictionary<string, CriteriaItem> items = base.LoadCriteriaItems(filters);
            return fhp.LoadCriteriaItems(filters, items, QueryContext, "coRestrictionsHxFormulary");
        }
        protected override ObjectContext CreateObjectContext(PathfinderEntities context, bool IsCustom)
        {
            return CreateClientContext();
        }
        protected override void BuildReportDefinitions()
        {
            string monthQtr;

            IList<int> timeFrameVals = new List<int>();
            string[] ProductIDarr = FilterSets[0]["Product_ID"].Split(',');
            IList<string> tiles = new List<string>();
            IList<string> tileTrxSummary = new List<string>();
            IList<string> tileDrillDown = new List<string>();

            FHXProvider fhx = new FHXProvider();

            int iSectionID = Convert.ToInt32(FilterSets[0]["Section_ID"]);
            timeFrameVals.Add(Convert.ToInt32(FilterSets[0]["TimeFrame1"]));
            timeFrameVals.Add(Convert.ToInt32(FilterSets[0]["TimeFrame2"]));


            //If Med-D, timeframe is month based
            if (FilterSets[0]["Monthly_Quarterly"] == "M")
            {
                //The timeframe is month based
                monthQtr = "Month";
            }
            else
            {
                //The timeframe is quarter based
                monthQtr = "Qtr";
            }

            //sumary tiles
            tiles.Add("Tile_Restriction");   //coverage summary
            tileTrxSummary.Add("Tile_TrxMst"); //trx summary

            string strTileDisplay = string.Format("Tile_Tier_{0}_Drug", FilterSets[0]["Trx_Mst"]);
            tileTrxSummary.Add(string.Format("Tile_{0}", FilterSets[0]["Trx_Mst"]));

            //for (int s = 0; s < ProductIDarr.Count(); s++)
            //{
                //tiles.Add(string.Format("{0}{1}", strTileDisplay, s + 1));
            tiles.Add(string.Format("{0}{1}", strTileDisplay,1));
            //}

            //Drilldown tiles
            tileDrillDown.Add("Tile_DrilldownBase");

            if (FilterSets.Count() > 1 && string.Compare(FilterSets[1]["Selection_Clicked"], "1") == 0)
            {
                tileDrillDown.Add("Tile_DrilldownCoverage");
            }
            else if (FilterSets.Count() > 1 && string.Compare(FilterSets[1]["Selection_Clicked"], "2") == 0)
            {
                tileDrillDown.Add("Tile_DrilldownBaseTrx");
                tileDrillDown.Add("Tile_DrilldownTier");
                tileDrillDown.Add("Tile_DrilldownCoPay");
                tileDrillDown.Add("Tile_DrilldownRestrictions");
                tileDrillDown.Add(string.Format("Tile_Drilldown_{0}", FilterSets[0]["Trx_Mst"]));
            }


            ReportDefinitions.Add(new ReportDefinition
            {
                ReportKey = "coTierCoverageHxFormulary",
                Style = ReportStyle.Grid,
                ReportDefinitions = new ReportDefinition[]
                {
                    //Report Defs are nested because they share same query string data - also changing EntityTypeName based on Section_ID filter if available
                    new FHXRestrictionsReportDefinitionBase { ReportKey="coRestrictionsHxFormulary", SectionTitle = "Coverage Lives Summary", Style = ReportStyle.Grid, Tile= string.Join(", ", tiles.ToArray())},
                    new FHXTierRestrictionsTrxReportDefinitionBase { ReportKey="coRestrictionsHxFormulary", SectionTitle = "Rx Volume Summary", Style = ReportStyle.Grid, Tile= string.Join(", ", tileTrxSummary.ToArray())},                               
                }

            });
            ReportDefinitions.Add(new FHXTierRestrictionsDrilldownReportDefinitionBase { ReportKey = "coRestrictionsHxFormulary", SectionTitle = "Drilldown Report", Style = ReportStyle.Grid, Tile = string.Join(", ", tileDrillDown.ToArray()) });


        }
    }
}
