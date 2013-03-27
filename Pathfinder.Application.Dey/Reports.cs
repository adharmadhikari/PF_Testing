using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pinsonault.Data.Reports;
using System.Data.Objects;
using PathfinderModel;
using Pinsonault.Data;
using System.Collections.Specialized;

namespace Pinsonault.Application.Dey
{
    //Provides helper functionality for exporting RestrictionsReport
    public class ReportUtility
    {
        //Loads custom criteria items for Standard Reports
        public Dictionary<string, CriteriaItem> LoadCriteriaItems(NameValueCollection filters, Dictionary<string, CriteriaItem> items)
        {
            CriteriaItem item = new CriteriaItem(null, null);
            string restriction = "PA";

            //Add Restrictions
            if (!string.IsNullOrEmpty(filters["PA"]))
            {
                //item = new CriteriaItem("PA", "PA Restriction");
                //item.Text = "Yes";
                //items.Add(item.Key, item);
                restriction = "PA";
            }

            if (!string.IsNullOrEmpty(filters["QL"]))
            {
                //item = new CriteriaItem("QL", "QL Restriction");
                //item.Text = "Yes";
                //items.Add(item.Key, item);
                restriction = "QL";
            }

            if (!string.IsNullOrEmpty(filters["ST"]))
            {
                //item = new CriteriaItem("ST", "Step Therapy");
                //item.Text = "Yes";
                //items.Add(item.Key, item);
                restriction = "ST";
            }

            if (!string.IsNullOrEmpty(filters["Criteria_ID"]))
            {
                using (PathfinderEntities context = new PathfinderEntities())
                {
                    int criteriaID = Convert.ToInt32(filters["Criteria_ID"]);
                    var criteriaName = (from d in context.RestrictionCriteriaDetailsSet
                                      where d.Criteria_ID == criteriaID &&
                                      d.Restriction_ID == restriction
                                      select d.Criteria_Name).Distinct();

                    item = new CriteriaItem("Criteria_ID", "Criteria");
                    item.Text = criteriaName.FirstOrDefault().ToString();
                    items.Add(item.Key, item);
                }
            }

            if (filters["Section_ID"] == "1")//Commerical 
            {
                //Add Account Type
                if (!string.IsNullOrEmpty(filters["Class_Partition"]))
                {
                    item = new CriteriaItem("Class_Partition", "Account Type");
                    if (filters["Class_Partition"] == "1")
                        item.Text = "Parent/Regional Independent";
                    else
                        item.Text = "Regional Affiliate/Regional Independent";
                    items.Add(item.Key, item);
                }
            }

            if (filters["Section_ID"] == "17")//Medicare Part D 
            {
                string strRegionTypeID = "0";
                if (!string.IsNullOrEmpty(filters["Region_Type_ID"]))
                    strRegionTypeID = filters["Region_Type_ID"];

                switch (strRegionTypeID)
                {
                    case "0":
                        {
                            //Add Account Type
                            if (!string.IsNullOrEmpty(filters["Class_Partition"]))
                            {
                                item = new CriteriaItem("Class_Partition", "Account Type");
                                if (filters["Class_Partition"] == "1")
                                    item.Text = "Parent";
                                else
                                    item.Text = "MA/PDP";
                                items.Add(item.Key, item);
                            }
                        }
                        break;
                    case "6":
                        {
                            item = new CriteriaItem("Region_Type_ID", "Account Type");
                            item.Text = "MA Regions";
                            items.Add(item.Key, item);
                            if (items.ContainsKey("geography_id"))
                            {                               
                                using (PathfinderEntities context = new PathfinderEntities())
                                {
                                    string strWhere = " it.Region_ID in {" + filters["Geography_ID"] + "}";
                                    var regionName = (from d in context.MARegionsSet.Where(strWhere)
                                                      select d).ToList().Select(d => string.Format("{0}", d.Region_Name.ToString()));

                                    //Comma separate individual record's data.
                                    items.Remove("geography_id");
                                    item = new CriteriaItem("Geography_ID", "Geography");
                                    item.Text = string.Join(", ", regionName.ToArray());
                                    items.Add(item.Key, item);
                                }
                            }
                            
                        }
                        break;
                    case "7":
                        {
                            item = new CriteriaItem("Region_Type_ID", "Account Type");
                            item.Text = "PDP Regions";
                            items.Add(item.Key, item);
                            if (items.ContainsKey("geography_id"))
                            {
                                using (PathfinderEntities context = new PathfinderEntities())
                                {
                                    string strWhere = " it.Region_ID in {" + filters["Geography_ID"] + "}";
                                    var regionName = (from d in context.PDPRegionsSet.Where(strWhere)
                                                      select d).ToList().Select(d => string.Format("{0}", d.Region_Name.ToString()));

                                    //Comma separate individual record's data.
                                    items.Remove("geography_id");
                                    item = new CriteriaItem("Geography_ID", "Geography");
                                    item.Text = string.Join(", ", regionName.ToArray());
                                    items.Add(item.Key, item);
                                }
                            }
                        }
                        break;

                }
            }
            //for getting the Account Manager or Geography name in filter criteria
            if (!string.IsNullOrEmpty(filters["geography_id"]) && !string.IsNullOrEmpty(filters["rcbGeographyType"]))
            {               
                if (filters["geography_id"].ToString() != "US" )
                {                   
                    //Regional
                    if (filters["rcbGeographyType"].ToString() == "2")
                    {
                        using (PathfinderClientModel.PathfinderClientEntities context = new PathfinderClientModel.PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
                        {
                            string Territory_ID = filters["geography_id"].ToString();
                            //get the territory name for selected Geography_ID

                            var q = (from p in context.TerritorySet
                                     where p.ID == Territory_ID
                                     select p.Name).FirstOrDefault();

                            item = new CriteriaItem("Geography", "Region");
                            item.Text = q;
                            items.Add(item.Key, item);
                            items.Remove("geography_id");
                        }
                    }
                    //State
                    else if (filters["rcbGeographyType"].ToString() == "3")
                    {
                        using (PathfinderClientModel.PathfinderClientEntities context = new PathfinderClientModel.PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
                        {
                            string Territory_ID = filters["geography_id"].ToString();
                            //get the state name for selected Geography_ID

                            var q = (from p in context.StateSet
                                     where p.ID == Territory_ID
                                     select p.Name).FirstOrDefault();
                            
                            item = new CriteriaItem("Geography", "State");
                            item.Text = q;
                            items.Add(item.Key, item);
                            items.Remove("geography_id");
                        }
                    }
                    //Account Manager
                    else if (filters["rcbGeographyType"].ToString() == "4")
                    {
                        using (PathfinderClientModel.PathfinderClientEntities context = new PathfinderClientModel.PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
                        {
                            string Territory_ID = filters["geography_id"].ToString();
                            //get the AM name for selected Geography_ID

                            var q = (from p in context.AccountManagersByTerritorySet
                                     where p.Territory_ID == Territory_ID
                                     select p.FullName).FirstOrDefault();

                            item = new CriteriaItem("Account_Manager", "Account Manager");
                            item.Text = q;
                            items.Add(item.Key, item);
                            items.Remove("geography_id");
                        }
                    }
                
                }
                
            }
            //return results
            return items;
        }  
    }

    public class DeyRestrictionsReport : Report
    {
        protected override Dictionary<string, CriteriaItem> LoadCriteriaItems(NameValueCollection filters)
        {
            //Get base items - includes Market Basket and Section (among others)
            Dictionary<string, CriteriaItem> items = base.LoadCriteriaItems(filters);

            ReportUtility sru = new ReportUtility();
            return sru.LoadCriteriaItems(filters, items);
        }

        protected override void BuildReportDefinitions()
        {
            IList<string> tiles = new List<string>();
            tiles.Add("Tile3Tools");

            if (FilterSets.Count > 0 && !string.IsNullOrEmpty(FilterSets[0]["Section_ID"]))
                tiles.Add("Tile3ToolsSection");

            ReportDefinitions.Add(new ReportDefinition
            {
                ReportKey = "deyrestrictionsreport",
                ReportDefinitions = new ReportDefinition[]
                                {
                                    new RestrictionsReportNationalDefinition { ReportKey="deyrestrictionsreport", Tile= string.Join(", ", tiles.ToArray()), EntityTypeName="RestrictionsReportSummary", Sort="Drug_Name asc", SectionTitle="QL Restrictions Report National" },
                                    new RestrictionsReportDefinition { ReportKey="deyrestrictionsreport", Tile= string.Join(", ", tiles.ToArray()), EntityTypeName="RestrictionsReportSummary", Sort="Drug_Name asc", SectionTitle="QL Restrictions Report" }
                                }
            });
            ReportDefinitions.Add(new DrilldownReportDefinition { ReportKey = "deyrestrictionsreport", Tile = "Tile4Tools", EntityTypeName = "RestrictionsReportDrilldown", Sort = "Formulary_Lives desc", SectionTitle = "QL Restrictions Report Drill Down" });
        }

        protected override ObjectContext CreateObjectContext(PathfinderEntities context, bool IsCustom)
        {
            return CreateDeyContext();
        }

        protected ObjectContext CreateDeyContext()
        {
            return new PathfinderDeyEntities();
        }
    }    
}
