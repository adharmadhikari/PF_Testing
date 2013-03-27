using System;
using System.Collections.Specialized;
using System.Linq;
using PathfinderClientModel;
using Pinsonault.Data;
using Pinsonault.Data.Reports;
using System.Collections.Generic;
using PathfinderModel;
using System.Collections;
using System.Data.Objects;
using System.Globalization;

namespace Pinsonault.Application.FormularyHistoryReporting
{   
    public class FHXReportDefinitionBase : ReportDefinition
    {      

        private NameValueCollection newFilters;

        FHXProvider fhp = new FHXProvider();

        protected override QueryDefinition CreateQueryDefinition(NameValueCollection filters)
        {
            newFilters = new NameValueCollection(filters);

            return new FHXQueryDefinition(EntityTypeName, newFilters);
        }

        protected override ColumnMap CreateColumnMap(System.Data.Common.DbDataRecord columnRecord)
        {
            ColumnMap map = base.CreateColumnMap(columnRecord);// new ColumnMap();
            IList<int> timeFrameVals = new List<int>();
            string strDBCompare = "";

            FHXProvider fhx = new FHXProvider();

            int iSectionID = Convert.ToInt32(newFilters["Section_ID"]);
            timeFrameVals.Add(Convert.ToInt32(newFilters["TimeFrame1"]));
            timeFrameVals.Add(Convert.ToInt32(newFilters["TimeFrame2"]));

            strDBCompare = fhx.GetDBCompareColumnName_CompareReport(map.PropertyName);

            if (map.PropertyName.IndexOfAny(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' }) > -1)
            //foreach (int s in timeFrameVals)
            {
                int itimeframeindex = Convert.ToInt32(map.PropertyName.Substring(map.PropertyName.IndexOfAny(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' })));

                string strTimeframename = "";

                if (newFilters["Monthly_Quarterly"] == "M")
                    strTimeframename = string.Format("{0} {1}", CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(Convert.ToInt32(timeFrameVals[itimeframeindex].ToString().Substring(4))), timeFrameVals[itimeframeindex].ToString().Substring(0, 4));
                else
                    strTimeframename = string.Format("Q{0} {1}", timeFrameVals[itimeframeindex].ToString().Substring(5), timeFrameVals[itimeframeindex].ToString().Substring(0, 4));
                
                string strRepeatedHeader = "H";
                if (itimeframeindex > 0)
                    strRepeatedHeader = "R";
                //translated name  will have : 1.Name as in DB 2. timeframe pivot count 3.timeframe value 4.flag to tell if is repeated header or not
                map.FirstHeaderTranslatedName = map.TranslatedName;
                map.MergedCellSpan = timeFrameVals.Count;
                map.SecondHeaderTranslatedName = strTimeframename;
                map.HeaderRepeaterCell = strRepeatedHeader;
                map.DBColToCompare = strDBCompare;

                //map.TranslatedName
                //map.TranslatedName = string.Format("{0}|{1}|{2}|{3}|{4}", map.TranslatedName, timeFrameVals.Count, strTimeframename, strRepeatedHeader, strDBCompare);
               
            }
            return map;
        }
    }

    public class FHXRollingReportDefinitionBase : ReportDefinition
    {

        private NameValueCollection newFilters;

        FHXProvider fhp = new FHXProvider();

        protected override QueryDefinition CreateQueryDefinition(NameValueCollection filters)
        {
            newFilters = new NameValueCollection(filters);

            return new FHXRollingQueryDefinition(EntityTypeName, newFilters);
        }

        protected override ColumnMap CreateColumnMap(System.Data.Common.DbDataRecord columnRecord)
        {
            ColumnMap map = base.CreateColumnMap(columnRecord);// new ColumnMap();
            IList<int> timeFrameVals = new List<int>();
            string strDBCompare ="";

            FHXProvider fhx = new FHXProvider();

            int iSectionID = Convert.ToInt32(newFilters["Section_ID"]);
            timeFrameVals = fhx.GetRollingTimeFrameVals(newFilters, iSectionID, true);
            strDBCompare = fhx.GetDBCompareColumnName(map.PropertyName);

            if (map.PropertyName.IndexOfAny(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' }) > -1)
                //foreach (int s in timeFrameVals)
            {
                int itimeframeindex = Convert.ToInt32(map.PropertyName.Substring(map.PropertyName.IndexOfAny(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' })));

                string strTimeframename = "";

                if (newFilters["Monthly_Quarterly"] == "M")
                    strTimeframename = string.Format("{0} {1}", CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(Convert.ToInt32(timeFrameVals[itimeframeindex].ToString().Substring(4))), timeFrameVals[itimeframeindex].ToString().Substring(0, 4));
                else
                    strTimeframename = string.Format("Q{0} {1}", timeFrameVals[itimeframeindex].ToString().Substring(5), timeFrameVals[itimeframeindex].ToString().Substring(0, 4));
                
                string strRepeatedHeader = "H";
                if (itimeframeindex > 0)
                    strRepeatedHeader = "R";
                //translated name  will have : 1.Name as in DB 2. timeframe pivot count 3.timeframe value 4.flag to tell if is repeated header or not

                map.FirstHeaderTranslatedName = map.TranslatedName;
                map.MergedCellSpan = timeFrameVals.Count;
                map.SecondHeaderTranslatedName = strTimeframename;
                map.HeaderRepeaterCell = strRepeatedHeader;

                //don't color code for last column else color code as per db flag for change
                if(itimeframeindex == timeFrameVals.Count-1)
                    map.DBColToCompare = string.Empty;
                else
                    map.DBColToCompare = strDBCompare;

                //map.TranslatedName = string.Format("{0}|{1}|{2}|{3}|{4}", map.TranslatedName, timeFrameVals.Count, strTimeframename, strRepeatedHeader, strDBCompare);
            }
            return map;
        }
    }
    public class FHXQueryDefinition : QueryDefinition
    {
        public FHXQueryDefinition(NameValueCollection queryString)
            : base(queryString)
        {
        }

        public FHXQueryDefinition(string EntityTypeName, NameValueCollection queryString)
            : base(EntityTypeName, queryString)
        {
        }

        public override IEnumerable CreateQuery(ObjectContext context)
        {
            return CreateFHXQuery();
        }

        IEnumerable CreateFHXQuery()
        {
            Pinsonault.Application.FormularyHistoryReporting.FHXProvider fhp = new Pinsonault.Application.FormularyHistoryReporting.FHXProvider();
            NameValueCollection queryValues = this.Values;
            IList<string> keys = new List<string>();
                       
            string dataField = "";
            string planLevelTableName = null;
         
            //IEnumerable<GenericDataRecord> g = null;
            IList<int> timeFrameVals = new List<int>();
            IList<int> display_optionsVals = new List<int>();

            //Add keys for query, used for inner join in pivot condition 
            keys.Add("Plan_ID");
            keys.Add("Drug_ID");
            keys.Add("Pinso_Formulary_ID");
            keys.Add("Segment_ID");
            keys.Add("Plan_Product_ID");
                      
            planLevelTableName = "V_GetFHXData_Geography";
           
            string geography_id = queryValues["Geography_ID"];

            int iSectionID = Convert.ToInt32(queryValues["Section_ID"]);
           
            timeFrameVals = fhp.GetTimeFrameVals(queryValues, iSectionID, false);            
            display_optionsVals = fhp.GetDisplayOptionList(queryValues, iSectionID);

            return fhp.GetData(timeFrameVals, geography_id, planLevelTableName, dataField, keys, queryValues, display_optionsVals);
        }
    }

    public class FHXRollingQueryDefinition : QueryDefinition
    {
        public FHXRollingQueryDefinition(NameValueCollection queryString)
            : base(queryString)
        {
        }

        public FHXRollingQueryDefinition(string EntityTypeName, NameValueCollection queryString)
            : base(EntityTypeName, queryString)
        {
        }

        public override IEnumerable CreateQuery(ObjectContext context)
        {
            return CreateFHXQuery();
        }

        IEnumerable CreateFHXQuery()
        {
            Pinsonault.Application.FormularyHistoryReporting.FHXProvider fhp = new Pinsonault.Application.FormularyHistoryReporting.FHXProvider();
            NameValueCollection queryValues = this.Values;
            IList<string> keys = new List<string>();

            string dataField = "";
            string planLevelTableName = null;

            //IEnumerable<GenericDataRecord> g = null;
            IList<int> timeFrameVals = new List<int>();
            IList<int> display_optionsVals = new List<int>();

            //Add keys for query, used for inner join in pivot condition 
            keys.Add("Plan_ID");
            keys.Add("Drug_ID");
            keys.Add("Pinso_Formulary_ID");
            keys.Add("Segment_ID");
            keys.Add("Plan_Product_ID");
                       
            planLevelTableName = "V_GetFHXData_Geography";
                       
            string geography_id = queryValues["Geography_ID"];

            int iSectionID = Convert.ToInt32(queryValues["Section_ID"]);
           
            timeFrameVals = fhp.GetRollingTimeFrameVals(queryValues, iSectionID, false);
            display_optionsVals = fhp.GetDisplayOptionList(queryValues, iSectionID);

            return fhp.GetRollingData(timeFrameVals, geography_id, planLevelTableName, dataField, keys, queryValues, display_optionsVals);
        }
    }

    public class FHXTierReportDefinitionBase : ReportDefinition
    {

        private NameValueCollection newFilters;

        FHXProvider fhp = new FHXProvider();

        protected override QueryDefinition CreateQueryDefinition(NameValueCollection filters)
        {
            newFilters = new NameValueCollection(filters);

            return new FHXTierQueryDefinition(EntityTypeName, newFilters);
        }
        protected override ColumnMap CreateColumnMap(System.Data.Common.DbDataRecord columnRecord)
        {
            ColumnMap map = base.CreateColumnMap(columnRecord);
            map.TranslatedName = fhp.GetMapTranslatedNameForTier_RestrictionsReport(map, newFilters);            
            return map;
        }
    }

    public class FHXTierQueryDefinition : QueryDefinition
    {
        public FHXTierQueryDefinition(NameValueCollection queryString)
            : base(queryString)
        {
        }

        public FHXTierQueryDefinition(string EntityTypeName, NameValueCollection queryString)
            : base(EntityTypeName, queryString)
        {
        }

        public override IEnumerable CreateQuery(ObjectContext context)
        {
            return CreateFHXTierQuery();
        }

        IEnumerable CreateFHXTierQuery()
        {
            Pinsonault.Application.FormularyHistoryReporting.FHXProvider fhp = new Pinsonault.Application.FormularyHistoryReporting.FHXProvider();
            NameValueCollection queryValues = this.Values;
            IList<string> keys = new List<string>();

            string dataField = "";
            string TierSummaryTableName = null;

            //IEnumerable<GenericDataRecord> g = null;
            IList<int> timeFrameVals = new List<int>();

            timeFrameVals.Add(Convert.ToInt32(queryValues["TimeFrame1"]));
            timeFrameVals.Add(Convert.ToInt32(queryValues["TimeFrame2"]));
                  
            //keys for Tier summary          
            keys.Add("Drug_ID");
            keys.Add("Drug_Name");
            keys.Add("Product_ID");
            keys.Add("Product_Name");
            //keys.Add("Thera_ID");
            keys.Add("Geography_ID");
            keys.Add("Tier_No");

            //If Med-D, timeframe is month based
            if (queryValues["Monthly_Quarterly"] == "M")
            {
                ////The timeframe is month based
                //isMonth = true;
                //msdataField = "TimeFrame";
                dataField = "Data_Year_Month";                
            }
            else
            {
                //The timeframe is quarter based
                //isMonth = false;
                //msdataField = "TimeFrame";
                dataField = "Data_Year_Quarter";              
            }
            TierSummaryTableName = "V_GetPlanProductFormularyHistory";

            
            string geographyID  = queryValues["Geography_ID"];

            return fhp.GetTierDataEx(timeFrameVals, geographyID, TierSummaryTableName, dataField, keys, queryValues);
        }
    }

    public class FHXTierRestrictionsTrxReportDefinitionBase : ReportDefinition
    {

        private NameValueCollection newFilters;

        FHXProvider fhp = new FHXProvider();

        protected override QueryDefinition CreateQueryDefinition(NameValueCollection filters)
        {
            newFilters = new NameValueCollection(filters);

            return new FHXTierRestrictionsTrxReportDefinition(EntityTypeName, newFilters);
        }
        protected override ColumnMap CreateColumnMap(System.Data.Common.DbDataRecord columnRecord)
        {
            ColumnMap map = base.CreateColumnMap(columnRecord);
            map.TranslatedName = fhp.GetMapTranslatedNameForTier_RestrictionsReport(map, newFilters);
            return map;
        }
    }
  
    public class FHXTierRestrictionsTrxReportDefinition : QueryDefinition
    {
        public FHXTierRestrictionsTrxReportDefinition(NameValueCollection queryString)
            : base(queryString)
        {
        }

        public FHXTierRestrictionsTrxReportDefinition(string EntityTypeName, NameValueCollection queryString)
            : base(EntityTypeName, queryString)
        {
        }

        public override IEnumerable CreateQuery(ObjectContext context)
        {
            return CreateFHXTrxQuery();
        }

        IEnumerable CreateFHXTrxQuery()
        {
            Pinsonault.Application.FormularyHistoryReporting.FHXProvider fhp = new Pinsonault.Application.FormularyHistoryReporting.FHXProvider();
            NameValueCollection queryValues = this.Values;            

            string dataField = "";
            string TrxSummaryTableName = null;

            //IEnumerable<GenericDataRecord> g = null;
            IList<int> timeFrameVals = new List<int>();

            timeFrameVals.Add(Convert.ToInt32(queryValues["TimeFrame1"]));
            timeFrameVals.Add(Convert.ToInt32(queryValues["TimeFrame2"]));

            IList<string> keys_trx = new List<string>();
            keys_trx.Add("Drug_ID");
            keys_trx.Add("Drug_Name");
            keys_trx.Add("Product_ID");
            keys_trx.Add("Product_Name");
            keys_trx.Add("Thera_ID");
            keys_trx.Add("Geography_ID");

            //If Med-D, timeframe is month based
            if (queryValues["Monthly_Quarterly"] == "M")
            {
                ////The timeframe is month based
                //isMonth = true;
                dataField = "TimeFrame";
                TrxSummaryTableName = "MS_Monthly";
            }
            else
            {
                //The timeframe is quarter based
                //isMonth = false;
                dataField = "TimeFrame";
                TrxSummaryTableName = "MS_Quarterly";
            }

            string geographyID = queryValues["Geography_ID"];

            return fhp.GetTierTRXData(timeFrameVals, geographyID, TrxSummaryTableName, dataField, keys_trx, queryValues);
        }
    }

    public class FHXTierRestrictionsDrilldownReportDefinitionBase : ReportDefinition
    {       
        private NameValueCollection newFilters;

        FHXProvider fhp = new FHXProvider();

        protected override QueryDefinition CreateQueryDefinition(NameValueCollection filters)
        {
            newFilters = new NameValueCollection(filters);            

            return new FHXTierRestrictionsDrilldownReportDefinition(EntityTypeName, newFilters);
        }
        protected override ColumnMap CreateColumnMap(System.Data.Common.DbDataRecord columnRecord)
        {
            ColumnMap map = base.CreateColumnMap(columnRecord);
            map.TranslatedName = fhp.GetMapTranslatedNameForTier_RestrictionsReport(map, newFilters);
            return map;
        }
    }
    public class FHXTierRestrictionsDrilldownReportDefinition : QueryDefinition
    {
        public FHXTierRestrictionsDrilldownReportDefinition(NameValueCollection queryString)
            : base(queryString)
        {
        }

        public FHXTierRestrictionsDrilldownReportDefinition(string EntityTypeName, NameValueCollection queryString)
            : base(EntityTypeName, queryString)
        {
        }

        public override IEnumerable CreateQuery(ObjectContext context)
        {
            return CreateFHXTierRestrictionDrilldownQuery();
        }

        IEnumerable CreateFHXTierRestrictionDrilldownQuery()
        {
            Pinsonault.Application.FormularyHistoryReporting.FHXProvider fhp = new Pinsonault.Application.FormularyHistoryReporting.FHXProvider();
            NameValueCollection queryValues = this.Values;

            string dataField = null;
            string tableName = null;

            string msdataField = null;
            string mstableName = null;

            IList<int> timeFrame = new List<int>();
            bool isMonth = false;

            IList<string> keys_drilldown = new List<string>();

            keys_drilldown.Add("Plan_ID");
            keys_drilldown.Add("Drug_ID");
            keys_drilldown.Add("Product_ID");
            keys_drilldown.Add("Product_Name");
            keys_drilldown.Add("Pinso_Formulary_ID");   //changed column name from Formulary_ID to Pinso_Formulary_ID  
            keys_drilldown.Add("Segment_ID");
            keys_drilldown.Add("Plan_Product_ID");
            keys_drilldown.Add("Plan_Name");
            keys_drilldown.Add("Plan_State_ID");
            keys_drilldown.Add("Plan_Pharmacy_Lives");
            keys_drilldown.Add("Formulary_Name");
            keys_drilldown.Add("Formulary_Lives");
            keys_drilldown.Add("Drug_Name");
            keys_drilldown.Add("Plan_Classification_ID");

            IList<string> mskeys_drilldown = new List<string>();
            mskeys_drilldown.Add("Product_ID");
            mskeys_drilldown.Add("Product_Name");
            mskeys_drilldown.Add("Drug_ID");
            mskeys_drilldown.Add("Thera_ID");
            mskeys_drilldown.Add("Segment_ID");
            mskeys_drilldown.Add("Segment_Name");
            mskeys_drilldown.Add("Geography_ID");
            mskeys_drilldown.Add("Plan_ID");

            IList<string> keys_fhr = new List<string>();
            keys_fhr.Add("Plan_ID");
            keys_fhr.Add("Drug_ID");
            keys_fhr.Add("Product_ID");
            keys_fhr.Add("Pinso_Formulary_ID");   //changed column name from Formulary_ID to Pinso_Formulary_ID  
            keys_fhr.Add("Segment_ID");
            keys_fhr.Add("Plan_Product_ID");
            keys_fhr.Add("Geography_ID");
            keys_fhr.Add("Geography_Name");

            NameValueCollection n = new NameValueCollection(queryValues);

            n["PagingEnabled"] = "false";
            n["Export"] = "True";

            if (queryValues["Monthly_Quarterly"] == "M")
            {
                //The timeframe is month based
                isMonth = true;

                msdataField = "TimeFrame";
                dataField = "Data_Year_Month";

                mstableName = "MS_Monthly";
                tableName = "V_GetPlanProductFormularyByProduct";
            }
            else
            {
                //The timeframe is quarter based
                isMonth = false;

                msdataField = "TimeFrame";
                dataField = "Data_Year_Quarter";

                mstableName = "MS_Quarterly";
                tableName = "V_GetPlanProductFormularyByProduct";
            }

            FHXProvider fhx = new FHXProvider();

            string geographyID = queryValues["Geography_ID"];          

            timeFrame.Add(Convert.ToInt32(queryValues["TimeFrame1"]));
            timeFrame.Add(Convert.ToInt32(queryValues["TimeFrame2"]));

            if (string.Compare(queryValues["Selection_Clicked"], "1") == 0)
                return fhx.GetTierOrCoverageDrilldownData(timeFrame, geographyID, tableName, dataField, keys_drilldown, n);
            else
                return fhp.GetTierTRXDrilldownData(timeFrame, geographyID, tableName, dataField, keys_fhr, n, mstableName, mskeys_drilldown, msdataField);   

        }
    }

    public class FHXRestrictionsReportDefinitionBase : ReportDefinition
    {

        private NameValueCollection newFilters;

        FHXProvider fhp = new FHXProvider();

        protected override QueryDefinition CreateQueryDefinition(NameValueCollection filters)
        {
            newFilters = new NameValueCollection(filters);

            return new FHXRestrictionsQueryDefinition(EntityTypeName, newFilters);
        }
        protected override ColumnMap CreateColumnMap(System.Data.Common.DbDataRecord columnRecord)
        {
            ColumnMap map = base.CreateColumnMap(columnRecord);
            map.TranslatedName = fhp.GetMapTranslatedNameForTier_RestrictionsReport(map, newFilters);
            return map;
        }
    }
    public class FHXRestrictionsQueryDefinition : QueryDefinition
    {
        public FHXRestrictionsQueryDefinition(NameValueCollection queryString)
            : base(queryString)
        {
        }

        public FHXRestrictionsQueryDefinition(string EntityTypeName, NameValueCollection queryString)
            : base(EntityTypeName, queryString)
        {
        }

        public override IEnumerable CreateQuery(ObjectContext context)
        {
            return CreateFHXRestrictionsQuery();
        }

        IEnumerable CreateFHXRestrictionsQuery()
        {
            Pinsonault.Application.FormularyHistoryReporting.FHXProvider fhp = new Pinsonault.Application.FormularyHistoryReporting.FHXProvider();
            NameValueCollection queryValues = this.Values;
            IList<string> keys = new List<string>();

            string dataField = "";
            string SummaryTableName = null;

            //IEnumerable<GenericDataRecord> g = null;
            IList<int> timeFrameVals = new List<int>();

            timeFrameVals.Add(Convert.ToInt32(queryValues["TimeFrame1"]));
            timeFrameVals.Add(Convert.ToInt32(queryValues["TimeFrame2"]));

            //coverage status summary query
            keys.Add("Drug_ID");
            keys.Add("Drug_Name");
            keys.Add("Product_ID");
            keys.Add("Product_Name");
            //keys.Add("Thera_ID");
            keys.Add("Geography_ID");
            keys.Add("Coverage_Status_ID");

            //If Med-D, timeframe is month based
            if (queryValues["Monthly_Quarterly"] == "M")
            {
                ////The timeframe is month based
                //isMonth = true;
                //msdataField = "TimeFrame";
                dataField = "Data_Year_Month";
            }
            else
            {
                //The timeframe is quarter based
                //isMonth = false;
                //msdataField = "TimeFrame";
                dataField = "Data_Year_Quarter";
            }
            SummaryTableName = "V_GetPlanProductFormularyHistory";


            string geographyID = queryValues["Geography_ID"];

            return fhp.GetCoverageStatusDataEx(timeFrameVals, geographyID, SummaryTableName, dataField, keys, queryValues);
        }
    }
}
