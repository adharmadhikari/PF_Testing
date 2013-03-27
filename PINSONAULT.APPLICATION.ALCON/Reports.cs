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

namespace Pinsonault.Application.Alcon.CustomerContactReports
{   
    public class CustomerContactDrilldownReport : Report
    {
        protected override void BuildReportDefinitions()
        {
            ReportDefinitions.Add(new CCRDrilldownReportDefinition { ReportKey = "customercontactdrilldown", Tile = "Tile3Tools", EntityTypeName = "ContactReportDrilldown", Sort = "Plan_Name desc", SectionTitle = "Customer Contact Drill Down" });
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
                                new CCRProductReportDefinition { ReportKey="meetingtype", Tile="Tile4Tools", ProductIndex=0, IsNational=true, EntityTypeName="ContactReportDataSummaryEx", Sort="Meeting_Type_Name"},
                                new CCRProductReportDefinition { ReportKey="meetingtype", Tile="Tile4Tools", ProductIndex=0, IsNational=false, EntityTypeName="ContactReportDataSummaryEx", Sort="Meeting_Type_Name"},
                                new CCRProductReportDefinition { ReportKey="meetingtype", Tile="Tile4Tools", ProductIndex=1, IsNational=true, EntityTypeName="ContactReportDataSummaryEx", Sort="Meeting_Type_Name"},
                                new CCRProductReportDefinition { ReportKey="meetingtype", Tile="Tile4Tools", ProductIndex=1, IsNational=false, EntityTypeName="ContactReportDataSummaryEx", Sort="Meeting_Type_Name"},
                                new CCRProductReportDefinition { ReportKey="meetingtype", Tile="Tile4Tools", ProductIndex=2, IsNational=true, EntityTypeName="ContactReportDataSummaryEx", Sort="Meeting_Type_Name"},
                                new CCRProductReportDefinition { ReportKey="meetingtype", Tile="Tile4Tools", ProductIndex=2, IsNational=false, EntityTypeName="ContactReportDataSummaryEx", Sort="Meeting_Type_Name"}                               
                            }
            });
            ReportDefinitions.Add(new CCRDrilldownReportDefinition { ReportKey = "meetingtype_alcon", Tile = "Tile5Tools", EntityTypeName = "ContactReportDataEx", Sort = "Meeting_Type_Name desc", SectionTitle = "Drilldown" });
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
                                new ProductsDiscussedYTDReportDefinition { ReportKey="productsdiscussed", Tile="Tile4Tools", EntityTypeName="ContactReportDataSummaryEx", Sort="Drug_Name"},
                                new ProductsDiscussedTimeFrameReportDefinition { ReportKey="productsdiscussed", Tile="Tile4Tools", EntityTypeName="ContactReportDataSummaryEx", Sort="Drug_Name"},
                            }
            });

            ReportDefinitions.Add(new CCRDrilldownReportDefinition { ReportKey = "productsdiscussed_alcon", Tile = "Tile5Tools", EntityTypeName = "ContactReportDataEx", Sort = "Plan_Name asc", SectionTitle = "Drilldown" });
        }
    }   

    public class documentuploadsearch : Report
    {
        protected override void BuildReportDefinitions()
        {
            ReportDefinitions.Add(new ReportDefinition { ReportKey = "DocumentUploadSearch", Tile = "Tile3Tools", EntityTypeName = "PlanDocumentsView", SectionTitle = "DocumentUploadSearch" });
        }
    }
}
namespace Pinsonault.Application.Alcon.ActivityReporting
{
    public class ActivityReporting : Report
    {
        protected override void BuildReportDefinitions()
        {
            ReportDefinitions.Add(new ActivityReportingDefinition { ReportKey = "activityreporting", Tile = "Tile4Tools", EntityTypeName = "ActivityReportingDetails", Sort = "Activity_Type_Name desc", SectionTitle = "Activity Summary" });
            ReportDefinitions.Add(new ReportDefinition { ReportKey = "activityreporting", Tile = "Tile5Tools", EntityTypeName = "ActivityReportingDetails", Sort = "Territory_Name desc", SectionTitle = "Drilldown" });
        }

        protected override Dictionary<string, CriteriaItem> LoadCriteriaItems(NameValueCollection filters)
        {
            //Get base items - includes Market Basket and Section (among others)
            Dictionary<string, CriteriaItem> items = base.LoadCriteriaItems(filters);
            
            CriteriaItem item = new CriteriaItem("Meeting_Activity_ID", "Activity Type Selection");
            using (PathfinderAlconEntities context = new PathfinderAlconEntities())
            {
                if (filters["Activity_Type_ID"] != null)
                {
                    string[] maArr = filters["Activity_Type_ID"].Split(',');
                    string[] maNames = new string[maArr.Length];

                    for (int x = 0; x < maArr.Count(); x++)
                    {
                        Int32 aid = Convert.ToInt32(maArr[x]);
                        var maQuery = context.DailyActivityTypeSet.Where(a => a.Activity_Type_ID == aid).Select(a => a.Activity_Type_Name);
                        maNames[x] = maQuery.FirstOrDefault().ToString();
                    }

                    item.Text = string.Join(", ", maNames);
                    items.Add(item.Key, item);
                }
            }            

            string[] dates = filters["Activity_Date"].Split(',');

            item = new CriteriaItem("Activity_Date_From", "From");
            item.Text = dates[0];
            items.Add(item.Key, item);

            item = new CriteriaItem("Activity_Date_To", "To");
            item.Text = dates[1];
            items.Add(item.Key, item);

            using (PathfinderClientEntities context = new PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
            {
                item = new CriteriaItem("User_ID", "Account Manager");
                if (!string.IsNullOrEmpty(filters["User_ID"]))
                {
                    string[] amArr = filters["User_ID"].Split(',');
                    string[] amNames = new string[amArr.Length];

                    for (int x = 0; x < amArr.Count(); x++)
                    {
                        Int32 uid = Convert.ToInt32(amArr[x]);
                        var actMgrQuery = context.AccountManagerSet.Where(a => a.User_ID == uid).Select(a => a.FullName);
                        amNames[x] = actMgrQuery.FirstOrDefault().ToString();
                    }

                    item.Text = string.Join(", ", amNames);
                }
                else
                    item.Text = "All";

                items.Add(item.Key, item);

                item = new CriteriaItem("Territory_ID", "Geography");
                if (!string.IsNullOrEmpty(filters["Territory_ID"]))
                {
                    string[] tArr = filters["Territory_ID"].Split(',');
                    string[] tNames = new string[tArr.Length];

                    for (int x = 0; x < tArr.Count(); x++)
                    {
                        string tid = tArr[x];
                        var actMgrQuery = context.TerritorySet.Where(a => a.ID == tid).Select(a => a.Name);
                        tNames[x] = actMgrQuery.FirstOrDefault().ToString();
                    }

                    item.Text = string.Join(", ", tNames);
                }
                else
                    item.Text = "All";

                items.Add(item.Key, item);
            }

            //return results
            return items;
        }
    }

    public class ActivityReportingDrilldown : Report
    {
        protected override void BuildReportDefinitions()
        {
            ReportDefinitions.Add(new ReportDefinition { ReportKey = "activityreporting", Tile = "Tile5Tools", EntityTypeName = "ActivityReportingDetails", Sort = "Territory_Name desc", SectionTitle = "Activity Reporting Drilldown" });
        }

        protected override Dictionary<string, CriteriaItem> LoadCriteriaItems(NameValueCollection filters)
        {
            //Get base items - includes Market Basket and Section (among others)
            Dictionary<string, CriteriaItem> items = base.LoadCriteriaItems(filters);

            CriteriaItem item = new CriteriaItem("Meeting_Activity_ID", "Activity Type Selection");
            using (PathfinderAlconEntities context = new PathfinderAlconEntities())
            {
                if (filters["Activity_Type_ID"] != null)
                {

                    string[] maArr = filters["Activity_Type_ID"].Split(',');
                    string[] maNames = new string[maArr.Length];

                    for (int x = 0; x < maArr.Count(); x++)
                    {
                        Int32 aid = Convert.ToInt32(maArr[x]);
                        var maQuery = context.DailyActivityTypeSet.Where(a => a.Activity_Type_ID == aid).Select(a => a.Activity_Type_Name);
                        maNames[x] = maQuery.FirstOrDefault().ToString();
                    }

                    item.Text = string.Join(", ", maNames);
                    items.Add(item.Key, item);
                }
            }

            string[] dates = filters["Activity_Date"].Split(',');

            item = new CriteriaItem("Activity_Date_From", "From");
            item.Text = dates[0];
            items.Add(item.Key, item);

            item = new CriteriaItem("Activity_Date_To", "To");
            item.Text = dates[1];
            items.Add(item.Key, item);

            using (PathfinderClientEntities context = new PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
            {
                item = new CriteriaItem("User_ID", "Account Manager");
                if (!string.IsNullOrEmpty(filters["User_ID"]))
                {
                    string[] amArr = filters["User_ID"].Split(',');
                    string[] amNames = new string[amArr.Length];

                    for (int x = 0; x < amArr.Count(); x++)
                    {
                        Int32 uid = Convert.ToInt32(amArr[x]);
                        var actMgrQuery = context.AccountManagerSet.Where(a => a.User_ID == uid).Select(a => a.FullName);
                        amNames[x] = actMgrQuery.FirstOrDefault().ToString();
                    }

                    item.Text = string.Join(", ", amNames);
                }
                else
                    item.Text = "All";

                items.Add(item.Key, item);

                item = new CriteriaItem("Territory_ID", "Geography");
                if (!string.IsNullOrEmpty(filters["Territory_ID"]))
                {
                    string[] tArr = filters["Territory_ID"].Split(',');
                    string[] tNames = new string[tArr.Length];

                    for (int x = 0; x < tArr.Count(); x++)
                    {
                        string tid = tArr[x];
                        var actMgrQuery = context.TerritorySet.Where(a => a.ID == tid).Select(a => a.Name);
                        tNames[x] = actMgrQuery.FirstOrDefault().ToString();
                    }

                    item.Text = string.Join(", ", tNames);
                }
                else
                    item.Text = "All";

                items.Add(item.Key, item);
            }

            //return results
            return items;
        }
    }
}
namespace Pinsonault.Application.Alcon.SellSheetReporting
{
    public class FormularySellSheetReport : Report
    {       
        protected override Dictionary<string, CriteriaItem> LoadCriteriaItems(NameValueCollection filters)
        {   
            if (FilterSets.Count > 0)
                filters = FilterSets[0];
            CriteriaItem item = new CriteriaItem(null, null);
            if (filters.GetValues("Thera_ID") != null)
            {
                filters.Add("Market_Basket_ID", filters["Thera_ID"]);
            }

            Dictionary<string, CriteriaItem> items = base.LoadCriteriaItems(filters);
            using (PathfinderEntities context = new PathfinderEntities())
            {
                //add Title

                if (filters.GetValues("Title_ID") != null)
                {
                    string title = filters["Title_ID"].ToString();

                    item = new CriteriaItem("Title_ID", "Title");
                    item.Text = title;
                    items.Add(item.Key, item);                    
                }
            }
            
            return items;
        }
        protected override void BuildReportDefinitions()
        {            
            ReportDefinitions.Add(new SellSheetReportDefinition { ReportKey = "formularysellsheetreport", Tile = "Tile3Tools", EntityTypeName = "SellSheetReport", SectionTitle = "Sell Sheet Report" });
        }    
    }
}
