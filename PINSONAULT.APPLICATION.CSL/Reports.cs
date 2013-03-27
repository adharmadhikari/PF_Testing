using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pinsonault.Data.Reports;


namespace Pinsonault.Application.CSL.CustomerContactReports
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
                                new MeetingActivityReportDefinition { ReportKey="meetingactivity", Tile="Tile4Tools", ProductIndex=0, DataContext=this.QueryContext, IsNational=true, EntityTypeName="ContactReportDataSummary", Sort="Meeting_Activity_Name"},
                                new MeetingActivityReportDefinition { ReportKey="meetingactivity", Tile="Tile4Tools", ProductIndex=0, DataContext=this.QueryContext, IsNational=false, EntityTypeName="ContactReportDataSummary", Sort="Meeting_Activity_Name"},
                                new MeetingActivityReportDefinition { ReportKey="meetingactivity", Tile="Tile4Tools", ProductIndex=1, DataContext=this.QueryContext, IsNational=true, EntityTypeName="ContactReportDataSummary", Sort="Meeting_Activity_Name"},
                                new MeetingActivityReportDefinition { ReportKey="meetingactivity", Tile="Tile4Tools", ProductIndex=1, DataContext=this.QueryContext, IsNational=false, EntityTypeName="ContactReportDataSummary", Sort="Meeting_Activity_Name"},
                                new MeetingActivityReportDefinition { ReportKey="meetingactivity", Tile="Tile4Tools", ProductIndex=2, DataContext=this.QueryContext, IsNational=true, EntityTypeName="ContactReportDataSummary", Sort="Meeting_Activity_Name"},
                                new MeetingActivityReportDefinition { ReportKey="meetingactivity", Tile="Tile4Tools", ProductIndex=2, DataContext=this.QueryContext, IsNational=false, EntityTypeName="ContactReportDataSummary", Sort="Meeting_Activity_Name"}                                
                            }
            });

            ReportDefinitions.Add(new CCRReportDefinition { ReportKey = "meetingactivity", Tile = "Tile5Tools", EntityTypeName = "ContactReportData", Sort = "Meeting_Activity_Name desc", SectionTitle = "Drilldown" });
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
                                new MeetingTypeReportDefinition { ReportKey="meetingtype", Tile="Tile4Tools", DataContext=this.QueryContext, ProductIndex=0, IsNational=true, EntityTypeName="ContactReportDataSummary", Sort="Meeting_Type_Name"},
                                new MeetingTypeReportDefinition { ReportKey="meetingtype", Tile="Tile4Tools", DataContext=this.QueryContext, ProductIndex=0, IsNational=false, EntityTypeName="ContactReportDataSummary", Sort="Meeting_Type_Name"},
                                new MeetingTypeReportDefinition { ReportKey="meetingtype", Tile="Tile4Tools", DataContext=this.QueryContext, ProductIndex=1, IsNational=true, EntityTypeName="ContactReportDataSummary", Sort="Meeting_Type_Name"},
                                new MeetingTypeReportDefinition { ReportKey="meetingtype", Tile="Tile4Tools", DataContext=this.QueryContext, ProductIndex=1, IsNational=false, EntityTypeName="ContactReportDataSummary", Sort="Meeting_Type_Name"},
                                new MeetingTypeReportDefinition { ReportKey="meetingtype", Tile="Tile4Tools", DataContext=this.QueryContext, ProductIndex=2, IsNational=true, EntityTypeName="ContactReportDataSummary", Sort="Meeting_Type_Name"},
                                new MeetingTypeReportDefinition { ReportKey="meetingtype", Tile="Tile4Tools", DataContext=this.QueryContext, ProductIndex=2, IsNational=false, EntityTypeName="ContactReportDataSummary", Sort="Meeting_Type_Name"}                               
                            }
            });
            ReportDefinitions.Add(new CCRReportDefinition { ReportKey = "meetingtype", Tile = "Tile5Tools", EntityTypeName = "ContactReportData", Sort = "Meeting_Type_Name desc", SectionTitle = "Drilldown" });
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
                                new ProductsDiscussedYTDReportDefinition { ReportKey="productsdiscussed", Tile="Tile4Tools", DataContext=this.QueryContext, EntityTypeName="ContactReportDataSummary", Sort="Drug_Name", SectionTitle="Year to Date"},
                                new ProductsDiscussedReportDefinition { ReportKey="productsdiscussed", Tile="Tile4Tools", DataContext=this.QueryContext, EntityTypeName="ContactReportDataSummary", Sort="Drug_Name", SectionTitle = "Time" },
                            }
            });

            ReportDefinitions.Add(new CCRReportDefinition { ReportKey = "productsdiscussed", Tile = "Tile5Tools", DataContext = this.QueryContext, EntityTypeName = "ContactReportData", Sort = "Plan_Name asc", SectionTitle = "Drilldown" });
        }
    }

    public class CustomerContactDrilldownReport : Report
    {
        protected override void BuildReportDefinitions()
        {
            ReportDefinitions.Add(new CCRReportDefinition { ReportKey = "customcontactdrilldown", Tile = "Tile3Tools", DataContext = this.QueryContext, EntityTypeName = "ContactReportData", Sort = "Plan_Name desc", SectionTitle = "Customer Contact Drill Down" });
        }
    }

}
