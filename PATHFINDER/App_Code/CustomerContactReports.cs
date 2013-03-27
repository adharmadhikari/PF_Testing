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





namespace Pinsonault.Application.CustomerContactReports
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
                                //new MeetingActivityDrilldownReportDefinition { ReportKey="meetingactivity", Tile="Tile5Tools", EntityTypeName="ContactReportData", Sort="Meeting_Activity_Name desc", SectionTitle="Drilldown"}
                            }
            });

            //temp fix for filtering meeting outcome AND FOLLOWUP NOTES FROM EXCEL EXPORT IN CASE OF ALCON
            if(Pinsonault.Web.Session.ClientKey == "alcon")
                ReportDefinitions.Add(new ReportDefinition { ReportKey = "meetingactivity_alcon", Tile = "Tile5Tools", EntityTypeName = "ContactReportData", Sort = "Meeting_Activity_Name desc", SectionTitle = "Drilldown" });
            else
                ReportDefinitions.Add(new ReportDefinition { ReportKey = "meetingactivity", Tile = "Tile5Tools", EntityTypeName = "ContactReportData", Sort = "Meeting_Activity_Name desc", SectionTitle = "Drilldown" });
        }

        protected override ObjectContext CreateObjectContext(PathfinderEntities context, bool IsCustom)
        {
            return CreateClientContext();
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
                //ReportDefinitions.Add(new ReportDefinition { ReportKey = "meetingtype", Tile = "Tile5Tools", EntityTypeName = "ContactReportData", Sort = "Meeting_Type_Name desc", SectionTitle = "Drilldown" });   
                //temp fix for filtering meeting outcome AND FOLLOWUP NOTES FROM EXCEL EXPORT IN CASE OF ALCON
                if (Pinsonault.Web.Session.ClientKey == "alcon")
                    ReportDefinitions.Add(new ReportDefinition { ReportKey = "meetingtype_alcon", Tile = "Tile5Tools", EntityTypeName = "ContactReportData", Sort = "Meeting_Type_Name desc", SectionTitle = "Drilldown" });   
                else
                    ReportDefinitions.Add(new ReportDefinition { ReportKey = "meetingtype", Tile = "Tile5Tools", EntityTypeName = "ContactReportData", Sort = "Meeting_Type_Name desc", SectionTitle = "Drilldown" });    
        }

        protected override ObjectContext CreateObjectContext(PathfinderEntities context, bool IsCustom)
        {
            return CreateClientContext();
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

            //temp fix for filtering meeting outcome AND FOLLOWUP NOTES FROM EXCEL EXPORT IN CASE OF ALCON
            if (Pinsonault.Web.Session.ClientKey == "alcon")                
                ReportDefinitions.Add(new ReportDefinition { ReportKey = "productsdiscussed_alcon", Tile = "Tile5Tools", EntityTypeName = "ContactReportData", Sort = "Plan_Name asc", SectionTitle = "Drilldown" });
            else
                ReportDefinitions.Add(new ReportDefinition { ReportKey = "productsdiscussed", Tile = "Tile5Tools", EntityTypeName = "ContactReportData", Sort = "Plan_Name asc", SectionTitle = "Drilldown" });
        }


        protected override ObjectContext CreateObjectContext(PathfinderEntities context, bool IsCustom)
        {
            return CreateClientContext();
        }
    }

    public class CustomerContactDrilldownReport : Report
    {
        protected override void BuildReportDefinitions()
        {           
            ReportDefinitions.Add(new ReportDefinition { ReportKey = "customcontactdrilldown", Tile = "Tile3Tools", EntityTypeName = "ContactReportData", Sort = "Plan_Name desc", SectionTitle = "Customer Contact Drill Down" });
        }


        protected override ObjectContext CreateObjectContext(PathfinderEntities context, bool IsCustom)
        {            
            Type customContextType = Type.GetType(string.Format("Pinsonault.Application.{0}.Pathfinder{0}Entities,Pinsonault.Application.{0}", Pinsonault.Web.Session.ClientKey), false, true);
           
            if (customContextType != null)
            {
                return customContextType.GetConstructor(Type.EmptyTypes).Invoke(null) as ObjectContext;
            }
            else //fall back to regular client model although it might not work
                return CreateClientContext();
        }
    }

    /// <summary>
    /// For export of individual customer contact reports from Edit CCR screen
    /// </summary>
    public class CustomerContactEntryReport : Report
    {
        protected override void BuildReportDefinitions()
        {
            //PathfinderClientModel.CustomerContactEntryDetails 
            ReportDefinitions.Add(new ReportDefinition { ReportKey = "singlecustomercontactreport", Tile = "Tile3Tools", EntityTypeName = "CustomerContactEntryDetails", Style = ReportStyle.List, Visible = false, SectionTitle = "Customer Contact Report" });
            ReportDefinitions.Add(new ReportDefinition { ReportKey = "singlecustomercontactreport", Tile = "Tile3Tools", EntityTypeName = "CustomerContactEntryDetails", Style = ReportStyle.List, SectionTitle = "Customer Contact Report" });           
        }

        protected override ObjectContext CreateObjectContext(PathfinderEntities context, bool IsCustom)
        {
            return CreateClientContext();
        }
    }

}
