using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;
using PathfinderClientModel;
using System.Data.Objects;
using Pinsonault.Data.Reports;


public class ReckittPlanInformationReport : Report
{
    public const string Name = "reckitt_planinformation";

    protected override void BuildReportDefinitions()
    {
        ReportDefinitions.Add(new ReportDefinition { ReportKey = "reckitt_planinformation", Tile = "Tile3Tools", EntityTypeName = "PlansClient", Style = ReportStyle.List, SectionTitle = Pinsonault.Resources.Resource.SectionTitle_PlanInfo });
    }
}

/// <summary>
/// Summary description for ReckittReports
/// </summary>
namespace Pinsonault.Application.Reckitt.CustomerContactReports
{
    public class MeetingActivityReport : Report
    {
        protected override void BuildReportDefinitions()
        {
            ReportDefinitions.Add(new ReportDefinition
            {
                ReportKey = "meetingactivity",
                ReportDefinitions = new ReportDefinition[]
                            {
                                //Report Defs are nested because they share same query string data - also changing EntityTypeName based on Section_ID filter if available
                                new CCRProductReportDefinition { ReportKey="meetingactivity", Tile="Tile4Tools", ProductIndex=0, IsNational=true, EntityTypeName="ContactReportDataSummary", Sort="Meeting_Activity_Name"},
                                new CCRProductReportDefinition { ReportKey="meetingactivity", Tile="Tile4Tools", ProductIndex=0, IsNational=false, EntityTypeName="ContactReportDataSummary", Sort="Meeting_Activity_Name"},
                                new CCRProductReportDefinition { ReportKey="meetingactivity", Tile="Tile4Tools", ProductIndex=1, IsNational=true, EntityTypeName="ContactReportDataSummary", Sort="Meeting_Activity_Name"},
                                new CCRProductReportDefinition { ReportKey="meetingactivity", Tile="Tile4Tools", ProductIndex=1, IsNational=false, EntityTypeName="ContactReportDataSummary", Sort="Meeting_Activity_Name"},
                                new CCRProductReportDefinition { ReportKey="meetingactivity", Tile="Tile4Tools", ProductIndex=2, IsNational=true, EntityTypeName="ContactReportDataSummary", Sort="Meeting_Activity_Name"},
                                new CCRProductReportDefinition { ReportKey="meetingactivity", Tile="Tile4Tools", ProductIndex=2, IsNational=false, EntityTypeName="ContactReportDataSummary", Sort="Meeting_Activity_Name"}                                
                            }
            });

            ReportDefinitions.Add(new CCRDrilldownReportDefinition { ReportKey = "meetingactivity", Tile = "Tile5Tools", EntityTypeName = "ContactReportProductData", Sort = "Meeting_Activity_Name desc", SectionTitle = "Drilldown" });
        }
    }

    public class MeetingTypeReport : Report
    {
        protected override void BuildReportDefinitions()
        {
            ReportDefinitions.Add(new ReportDefinition
            {
                ReportKey = "meetingtype",
                ReportDefinitions = new ReportDefinition[]
                            {
                                //Report Defs are nested because they share same query string data - also changing EntityTypeName based on Section_ID filter if available
                                new CCRProductReportDefinition { ReportKey="meetingtype", Tile="Tile4Tools", ProductIndex=0, IsNational=true, EntityTypeName="ContactReportDataSummary", Sort="Meeting_Type_Name"},
                                new CCRProductReportDefinition { ReportKey="meetingtype", Tile="Tile4Tools", ProductIndex=0, IsNational=false, EntityTypeName="ContactReportDataSummary", Sort="Meeting_Type_Name"},
                                new CCRProductReportDefinition { ReportKey="meetingtype", Tile="Tile4Tools", ProductIndex=1, IsNational=true, EntityTypeName="ContactReportDataSummary", Sort="Meeting_Type_Name"},
                                new CCRProductReportDefinition { ReportKey="meetingtype", Tile="Tile4Tools", ProductIndex=1, IsNational=false, EntityTypeName="ContactReportDataSummary", Sort="Meeting_Type_Name"},
                                new CCRProductReportDefinition { ReportKey="meetingtype", Tile="Tile4Tools", ProductIndex=2, IsNational=true, EntityTypeName="ContactReportDataSummary", Sort="Meeting_Type_Name"},
                                new CCRProductReportDefinition { ReportKey="meetingtype", Tile="Tile4Tools", ProductIndex=2, IsNational=false, EntityTypeName="ContactReportDataSummary", Sort="Meeting_Type_Name"}                               
                            }
            });
            ReportDefinitions.Add(new CCRDrilldownReportDefinition { ReportKey = "meetingtype", Tile = "Tile5Tools", EntityTypeName = "ContactReportProductData", Sort = "Meeting_Type_Name desc", SectionTitle = "Drilldown" });
        }
    }

    public class ProductsDiscussedReport : Report
    {
        protected override void BuildReportDefinitions()
        {
            ReportDefinitions.Add(new ReportDefinition
            {
                ReportKey = "productsdiscussed",
                ReportDefinitions = new ReportDefinition[]
                            {
                                //Report Defs are nested because they share same query string data - also changing EntityTypeName based on Section_ID filter if available
                                new ProductsDiscussedYTDReportDefinition { ReportKey="productsdiscussed", Tile="Tile4Tools", EntityTypeName="ContactReportDataSummary", Sort="Drug_Name"},
                                new ProductsDiscussedTimeFrameReportDefinition { ReportKey="productsdiscussed", Tile="Tile4Tools", EntityTypeName="ContactReportDataSummary", Sort="Drug_Name"},
                            }
            });

            ReportDefinitions.Add(new CCRDrilldownReportDefinition { ReportKey = "productsdiscussed", Tile = "Tile5Tools", EntityTypeName = "ContactReportProductData", Sort = "Plan_Name asc", SectionTitle = "Drilldown" });
        }
    }

    public class CustomerContactDrilldownReport : Report
    {
        protected override void BuildReportDefinitions()
        {
            ReportDefinitions.Add(new CCRDrilldownReportDefinition { ReportKey = "customcontactdrilldown", Tile = "Tile3Tools", EntityTypeName = "ContactReportData", Sort = "Plan_Name desc", SectionTitle = "Customer Contact Drill Down" });
        }
    }

}

