using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pinsonault.Data.Reports;
using System.Data.Objects;
using PathfinderModel;
using PathfinderClientModel;
using Pinsonault.Data;
using System.Collections.Specialized;
 
public class PlanInformationReport : Report
{
    public const string Name = "millennium_planinformation";

    protected override void BuildReportDefinitions()
    {
        ReportDefinitions.Add(new ReportDefinition { ReportKey = "millennium_planinformation", Tile = "Tile3Tools", EntityTypeName = "PlansClientExport", Style = ReportStyle.List, SectionTitle = Pinsonault.Resources.Resource.SectionTitle_PlanInfo });
    }
}

namespace Pinsonault.Application.Millennium.CustomerContactReports
{
    public class MillenniumCustomerContactDrillDownReport : Report 
    {
        protected override void BuildReportDefinitions()
        {
            ReportDefinitions.Add(new CCRDrilldownReportDefinition { ReportKey = "MillenniumCustomerContactDrillDown", Tile = "Tile3Tools", EntityTypeName = "ContactReportData", Sort = "Plan_Name desc", SectionTitle = "Customer Contact Drill Down" });
        }
    }
    public class MeetingTopicReport : Report
    {
        protected override void BuildReportDefinitions()
        {
            ReportDefinitions.Add(new ReportDefinition
            {
                ReportKey = "meetingtopic",
                ReportDefinitions = new ReportDefinition[]
                            {
                                //Report Defs are nested because they share same query string data - also changing EntityTypeName based on Section_ID filter if available
                                new CCRProductReportDefinition { ReportKey="meetingtopic", Tile="Tile4Tools", ProductIndex=0, IsNational=true, EntityTypeName="ContactReportData", Sort="Meeting_Activity_Name"},
                                new CCRProductReportDefinition { ReportKey="meetingtopic", Tile="Tile4Tools", ProductIndex=0, IsNational=false, EntityTypeName="ContactReportData", Sort="Meeting_Activity_Name"},
                                new CCRProductReportDefinition { ReportKey="meetingtopic", Tile="Tile5Tools", ProductIndex=1, IsNational=true, EntityTypeName="ContactReportData", Sort="Meeting_Activity_Name"},
                                new CCRProductReportDefinition { ReportKey="meetingtopic", Tile="Tile5Tools", ProductIndex=1, IsNational=false, EntityTypeName="ContactReportData", Sort="Meeting_Activity_Name"},
                                new CCRProductReportDefinition { ReportKey="meetingtopic", Tile="Tile5Tools", ProductIndex=2, IsNational=true, EntityTypeName="ContactReportData", Sort="Meeting_Activity_Name"},
                                new CCRProductReportDefinition { ReportKey="meetingtopic", Tile="Tile5Tools", ProductIndex=2, IsNational=false, EntityTypeName="ContactReportData", Sort="Meeting_Activity_Name"}                                
                            }
            });

            ReportDefinitions.Add(new CCRDrilldownReportDefinition { ReportKey = "meetingtopic", Tile = "Tile5Tools", EntityTypeName = "ContactReportData", Sort = "Meeting_Activity_Name desc", SectionTitle = "Drilldown" });
        }
    }
    public class InteractionTypeReport : Report
    {
        protected override void BuildReportDefinitions()
        {
            ReportDefinitions.Add(new ReportDefinition
            {
                ReportKey = "interactiontype",
                ReportDefinitions = new ReportDefinition[]
                            {
                                //Report Defs are nested because they share same query string data - also changing EntityTypeName based on Section_ID filter if available
                                new CCRProductReportDefinition { ReportKey="interactiontype", Tile="Tile4Tools", ProductIndex=0, IsNational=true, EntityTypeName="ContactReportData", Sort="Meeting_Type_Name"},
                                new CCRProductReportDefinition { ReportKey="interactiontype", Tile="Tile4Tools", ProductIndex=0, IsNational=false, EntityTypeName="ContactReportData", Sort="Meeting_Type_Name"},
                                new CCRProductReportDefinition { ReportKey="interactiontype", Tile="Tile4Tools", ProductIndex=1, IsNational=true, EntityTypeName="ContactReportData", Sort="Meeting_Type_Name"},
                                new CCRProductReportDefinition { ReportKey="interactiontype", Tile="Tile4Tools", ProductIndex=1, IsNational=false, EntityTypeName="ContactReportData", Sort="Meeting_Type_Name"},
                                new CCRProductReportDefinition { ReportKey="interactiontype", Tile="Tile4Tools", ProductIndex=2, IsNational=true, EntityTypeName="ContactReportData", Sort="Meeting_Type_Name"},
                                new CCRProductReportDefinition { ReportKey="interactiontype", Tile="Tile4Tools", ProductIndex=2, IsNational=false, EntityTypeName="ContactReportData", Sort="Meeting_Type_Name"}                               
                            }
            });
            ReportDefinitions.Add(new CCRDrilldownReportDefinition { ReportKey = "interactiontype", Tile = "Tile5Tools", EntityTypeName = "ContactReportData", Sort = "Meeting_Type_Name desc", SectionTitle = "Drilldown" });
        }
    }
}
  