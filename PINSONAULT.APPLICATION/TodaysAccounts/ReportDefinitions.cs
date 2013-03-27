using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PathfinderClientModel;
using Pinsonault.Data.Reports;
using Pinsonault.Data;
using System.Collections.Specialized;
using Pinsonault.Application.StandardReports;
using PathfinderModel;

namespace Pinsonault.Application.TodaysAccounts
{
    public class PlanInfoAffiliationsReportDefinition : ReportDefinition
    {
        public int AffilTypeID { get; set; }
        protected override QueryDefinition CreateQueryDefinition(NameValueCollection filters)
        {

            string value = filters["Plan_ID"];
            int id;
            if (int.TryParse(value, out id))
            {
                //Only for commercial payers- we need to pull data from PF db not client db
                //using (PathfinderClientEntities context = new PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
                //{
                //    Visible = context.PlanInfoListViewSet.Count(p => p.Plan_ID == id && p.Section_ID == 1) > 0;
                //}
                using (PathfinderEntities context = new PathfinderEntities())
                {
                    Visible = context.PlanInfoListViewSet.Count(p => p.Plan_ID == id && p.Section_ID == 1) > 0;
                }
            }
            //Affil type depends on tab
            filters["Affil_Type_ID"] = AffilTypeID.ToString();

            return base.CreateQueryDefinition(filters);
        }
        
    }

    public class CoveredLivesReportDefinition : ReportDefinition
    {
        protected override QueryDefinition CreateQueryDefinition(NameValueCollection filters)
        {
            return base.CreateQueryDefinition(filters);
        }
    }
    public class DrilldownReportDefinition : ReportDefinition
    {
        protected override QueryDefinition CreateQueryDefinition(NameValueCollection filters)
        {
           // return new DrilldownQueryDefinition(this.EntityTypeName, filters);
            return base.CreateQueryDefinition(filters);
        }
    }

    public class FormularyHistoryReportDefinition : ReportDefinition
    {
        private NameValueCollection newFilters;

        protected override QueryDefinition CreateQueryDefinition(NameValueCollection filters)
        {
            newFilters = new NameValueCollection(filters);

            return new FormularyHistoryQueryDefinition(EntityTypeName, newFilters);
        }

        protected override ColumnMap CreateColumnMap(System.Data.Common.DbDataRecord columnRecord)
        {
            ColumnMap map = base.CreateColumnMap(columnRecord);// new ColumnMap();

            string secondHeaderText = "";

            //Check if Tier, PA, QL, ST CoPay or Status
            if (map.TranslatedName.IndexOf("{0}") > -1)
            {
                int segmentID;

                if (newFilters["Section_ID"] == "9")//Set SegmentID for State Medicaid
                    segmentID = 3;
                else
                    segmentID = Convert.ToInt32(newFilters["Segment_ID"]);


                //Check if previous or current timeframe
                if (map.PropertyName.IndexOf("0") > -1) //Previous timeframe
                {
                    //Get the Previous Month name (P_M) since TA FHR is always month based
                    secondHeaderText = Pinsonault.Application.TodaysAccounts.TodaysAccountsDataService.GetFormularyHistoryTimeframeName("P_M", segmentID);
                    map.TranslatedName = string.Format(map.TranslatedName, Pinsonault.Application.TodaysAccounts.TodaysAccountsDataService.GetFormularyHistoryTimeframeName("P_M", segmentID));
                }
                else
                {
                    secondHeaderText = Pinsonault.Application.TodaysAccounts.TodaysAccountsDataService.GetFormularyHistoryTimeframeName("C", segmentID);
                    map.TranslatedName = string.Format(map.TranslatedName, Pinsonault.Application.TodaysAccounts.TodaysAccountsDataService.GetFormularyHistoryTimeframeName("C", segmentID));
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
