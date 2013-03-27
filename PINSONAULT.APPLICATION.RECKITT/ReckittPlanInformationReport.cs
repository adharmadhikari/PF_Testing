using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pinsonault.Data.Reports;

namespace Pinsonault.Application.Reckitt
{
    public class PlanInformationReport : Pinsonault.Application.TodaysAccounts.PlanInformationReport
    {
        protected override void BuildReportDefinitions()
        {
            string val = FindValues("Plan_ID").FirstOrDefault();
            int planID = 0;
            if ( !string.IsNullOrEmpty(val) )
                int.TryParse(val, out planID);

            //check if standard plan - if not then it is custom reckitt plan.
            if ( BaseObjectContext.PlanMasterSet.Count(p => p.Plan_ID == planID) == 0 )
                ReportDefinitions.Add(new ReportDefinition { ReportKey = "reckitt_planinformation", Tile = "Tile3Tools", EntityTypeName = "PlansClient", Style = ReportStyle.List, SectionTitle = Resources.Resource.SectionTitle_PlanInfo });
            else
                base.BuildReportDefinitions();
        }
    }
}