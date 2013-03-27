using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pinsonault.Data.Reports;
using System.Data.Objects;
using PathfinderModel;
using Pinsonault.Data;
using System.Collections.Specialized;
using System.Web;

namespace Pinsonault.Application.StandardReports
{
    //Provides helper functionality for exporting Standard Reports
    public class SRUtility
    {
        //Loads custom criteria items for Standard Reports
        public Dictionary<string, CriteriaItem> LoadCriteriaItems(NameValueCollection filters, Dictionary<string, CriteriaItem> items)
        {
            CriteriaItem item = new CriteriaItem(null, null);

            //Add Restrictions
            if (!string.IsNullOrEmpty(filters["__PA"]))
            {
                item = new CriteriaItem("PA", "PA Restriction");
                item.Text = "Yes";
                items.Add(item.Key, item);
            }

            if (!string.IsNullOrEmpty(filters["__QL"]))
            {
                item = new CriteriaItem("QL", "QL Restriction");
                item.Text = "Yes";
                items.Add(item.Key, item);
            }

            if (!string.IsNullOrEmpty(filters["__ST"]))
            {
                item = new CriteriaItem("ST", "Step Therapy");
                item.Text = "Yes";
                items.Add(item.Key, item);
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
                if (!string.IsNullOrEmpty(filters["Segment_ID"]))
                {
                    item = new CriteriaItem("Segment_ID", "Account Sub Type");
                    if (filters["Segment_ID"] == "8")
                    {
                        item.Text = "LIS";

                        items.Add(item.Key, item);
                    }
                }
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

    public class TierCoverageReport : Report
    {
        protected override Dictionary<string, CriteriaItem> LoadCriteriaItems(NameValueCollection filters)
        {
            //Get base items - includes Market Basket and Section (among others)
            Dictionary<string, CriteriaItem> items = base.LoadCriteriaItems(filters);

            SRUtility sru = new SRUtility();
            return sru.LoadCriteriaItems(filters, items);
        }

        protected override void BuildReportDefinitions()
        {
            string tile3 = "Tile3Tools";
            string tile4 = "Tile4Tools";

            if (FilterSets.Count > 0)
            {
                if (!String.IsNullOrEmpty(FilterSets[0]["Selected_Section_ID"]))
                {
                    FilterSets[0].Remove("Section_ID");

                    if (FilterSets[0]["Selected_Section_ID"] != "0")
                        FilterSets[0].Add("Section_ID", FilterSets[0]["Selected_Section_ID"]);
                }
            }

            if (FilterSets.Count > 0 && !string.IsNullOrEmpty(FilterSets[0]["Section_ID"]))
                tile3 = "Tile3Tools, Tile3ToolsSection";

            ReportDefinitions.Add(new ReportDefinition
            {
                ReportKey = "tiercoveragecomparison",
                ReportDefinitions = new ReportDefinition[]
                                {
                                    new TierCoverageNationalReportDefinition { ReportKey="tiercoveragecomparison", Tile=tile3, EntityTypeName="ReportsTierCoverage", Sort="Drug_Name asc", SectionTitle=Resources.Resource.SectionTitle_TierCoverage + " National" },
                                    new TierCoverageReportDefinition { ReportKey="tiercoveragecomparison", Tile=tile3, EntityTypeName="ReportsTierCoverage", Sort="Drug_Name asc", SectionTitle=Resources.Resource.SectionTitle_TierCoverage }
                                }
            });
            ReportDefinitions.Add(new DrilldownReportDefinition { ReportKey = "tiercoveragecomparison", Tile = tile4, EntityTypeName = "ReportsTierCoverageDrilldown", Sort = "Formulary_Lives desc", SectionTitle = Resources.Resource.SectionTitle_TierCoverage + " Drill Down" });
        }

        protected override ObjectContext CreateObjectContext(PathfinderEntities context, bool IsCustom)
        {
            ////if user ID is present then it is an account manager search and lookup must be done from client database
            //if ( HasValue(FilterSets, "User_ID") )
            //{
            //    //Also need to fix up entity type names 
            //    bool nationOrState = IsGeographyNationOrState(context, FindValues(FilterSets, "Geography_ID").FirstOrDefault());                
            //    //string entityName = nationOrState ? "TierCoverageByUser" : "TierCoverageByUserAndRegion";

            //    string entityName = "ReportsTierCoverage";
            //    foreach ( ReportDefinition def in ReportDefinitions[0].ReportDefinitions )
            //    {
            //        def.EntityTypeName = entityName;
            //    }
            //    //ReportDefinitions[1].EntityTypeName = "TierCoverageDrillDownByUser";
            //    ReportDefinitions[1].EntityTypeName = "ReportsTierCoverageDrilldown";

            //return CreateClientContext();
            return new PathfinderModel.PathfinderEntities();
            //}
            //else
            //    return base.CreateObjectContext(context, IsCustom);
        }
    }

    public class CoveredLivesReport : Report
    {
        protected override void BuildReportDefinitions()
        {
            ReportDefinitions.Add(new ReportDefinition { ReportKey = "coveredlives", Tile = "Tile3Tools", RequiresFilters = false, EntityTypeName = "CoveredLivesSummary", Sort = "sortorder", SectionTitle = Resources.Resource.SectionTitle_CoveredLives });
            ReportDefinitions.Add(new ReportDefinition { ReportKey = "coveredlives", Tile = "Tile4Tools", EntityTypeName = "CoveredLivesListView", Sort = "Total_Covered Desc", SectionTitle = Resources.Resource.SectionTitle_CoveredLives + " Drill Down" });
        }
    }

    public class AffiliationFormularyReport : Report
    {
        protected override void BuildReportDefinitions()
        {
            string tile_used = "Tile14"; //default tile for CP and PBM
            
            if (HasValue("Section_ID", "17"))
            {
                tile_used = "Tile17";
            }

            string tile_used_child = tile_used;
            if (HasValue("Section_ID", "4"))
            {
                tile_used_child = "Tile4";
            }

            ReportDefinitions.Add(
                new ReportDefinition()
                {
                    ReportDefinitions = new ReportDefinition[]
                    {                        
                        new ReportDefinition { ReportKey = "affiliationsformulary", Tile = tile_used, EntityTypeName = "AffiliationsFormularyParentPlan", Sort = "Plan_Name", SectionTitle = Resources.Resource.SectionTitle_ParentPlan },
                        new ReportDefinition { ReportKey = "affiliationsformulary", Tile = tile_used_child, EntityTypeName = "AffiliationsFormulary", Sort = "Plan_Name", SectionTitle = Resources.Resource.SectionTitle_ChildPlan }                        
                      }
                });




        }

        protected override ObjectContext CreateObjectContext(PathfinderEntities context, bool IsCustom)
        {            
            return new PathfinderModel.PathfinderEntities();
        }
    }

    public class LivesDistributionReport : Report
    {
        protected override Dictionary<string, CriteriaItem> LoadCriteriaItems(NameValueCollection filters)
        {
            //Get base items - includes Market Basket and Section (among others)
            Dictionary<string, CriteriaItem> items = base.LoadCriteriaItems(filters);

            SRUtility sru = new SRUtility();
            return sru.LoadCriteriaItems(filters, items);
        }

        protected override void BuildReportDefinitions()
        {
            string tile = "TileAllTools";
            Boolean isAllSelected = false;

            if (HasValue(FilterSets, "Section_ID"))
            {
                string[] sectionID = FilterSets[0]["Section_ID"].Split(',');
                for (Int32 i = 0; i < sectionID.Length; i++)
                {
                    if (sectionID[i] == "1")
                        tile += ",Tile3Tools";
                    else if (sectionID[i] == "6")
                        tile += ",Tile6Tools";
                    else if (sectionID[i] == "17")
                        tile += ",Tile17Tools";
                    else if (sectionID[i] == "4")
                        tile += ",Tile4Tools";
                    else if (sectionID[i] == "9")
                        tile += ",Tile9Tools";
                }
            }
            else
            {
                tile += ",Tile3Tools,Tile6Tools,Tile17Tools,Tile4Tools,Tile9Tools";
                isAllSelected = true;
            }
            
            if(isAllSelected)
                ReportDefinitions.Add(new DrilldownReportDefinition { ReportKey = "livesdistribution", Tile = tile, EntityTypeName = "LivesDistributionByAll", SectionTitle = Resources.Resource.SectionTitle_LivesDistributionReport });
            else
                ReportDefinitions.Add(new DrilldownReportDefinition { ReportKey = "livesdistribution", Tile = tile, EntityTypeName = "LivesDistribution", SectionTitle = Resources.Resource.SectionTitle_LivesDistributionReport });

        }

        protected override ObjectContext CreateObjectContext(PathfinderEntities context, bool IsCustom)
        {
            //return CreateClientContext();
            return new PathfinderModel.PathfinderEntities();
        }
    }

    public class FormularyStatusReport : Report
    {
        protected override Dictionary<string, CriteriaItem> LoadCriteriaItems(NameValueCollection filters)
        {
            //Get base items - includes Market Basket and Section (among others)
            Dictionary<string, CriteriaItem> items = base.LoadCriteriaItems(filters);

            SRUtility sru = new SRUtility();
            return sru.LoadCriteriaItems(filters, items);            
        }

        protected override void BuildReportDefinitions()
        {
            //ReportDefinitions.Add(new ReportDefinition { ReportKey = "formularystatus", Tile = "Tile3Tools", EntityTypeName = "ReportsFormularyStatus", Sort = "Drug_Name", SectionTitle = Resources.Resource.SectionTitle_FormularyStatus });
            string tiles = "";

            if (FilterSets.Count > 0)
            {
                if (!String.IsNullOrEmpty(FilterSets[0]["Selected_Section_ID"]))
                {
                    FilterSets[0].Remove("Section_ID");

                    if (FilterSets[0]["Selected_Section_ID"] != "0")
                        FilterSets[0].Add("Section_ID", FilterSets[0]["Selected_Section_ID"]);
                }
            }

            if (FilterSets.Count > 0 && !string.IsNullOrEmpty(FilterSets[0]["Section_ID"]))
            {
                string[] sectionID = FilterSets[0]["Section_ID"].Split(',');
                if (sectionID.Length == 1)
                {
                    if (HasValue("Section_ID", "1"))
                        tiles = "Tile3, Tile3CL"; //Commercial Payer
                    else if (HasValue("Section_ID", "4"))
                        tiles = "Tile3, Tile3PBM"; //PBM
                    else if (HasValue("Section_ID", "17"))
                        tiles = "Tile3, Tile3MedD"; //Medicare Part D
                    else if (HasValue("Section_ID", "6"))
                        tiles = "Tile3, Tile3MM"; //Managed Medicaid
                    else
                        tiles = "Tile3, Tile3CL";
                }
                else
                    tiles = "Tile3, Tile3_section, Tile3CL";
            }
            else
                tiles = "Tile3, Tile3CL";


            //changes for report title change
            string strReportTitle = Resources.Resource.SectionTitle_FormularyStatus;
            string strReportKey = "formularystatus";
            
            if (System.Web.HttpContext.Current.Request.QueryString["report"] == "geographiccoverage")
            {
                strReportTitle = "Geographic Coverage";
                ReportDefinitions.Add(new ReportDefinition
                {
                    ReportKey = strReportKey,
                    ReportDefinitions = new ReportDefinition[]
                                {
                                    new FormularyStatusNationalReportDefinition{ReportKey=strReportKey, Tile = tiles, EntityTypeName="ReportsFormularyStatusSummary", Sort = "Drug_Name asc", SectionTitle=strReportTitle + " National",Visible=false },
                                    new FormularyStatusReportDefinition { ReportKey=strReportKey, Tile = tiles, EntityTypeName="ReportsFormularyStatusSummary", Sort = "Drug_Name asc", SectionTitle=strReportTitle , Visible = false },
                                }
                });
             
                ReportDefinitions.Add(new DrilldownReportDefinition { ReportKey = strReportKey, Tile = "Tile4Tools", EntityTypeName = "ReportsFormularyStatusDrilldown", Sort = "Formulary_Lives desc", SectionTitle = strReportTitle + " Drill Down" });
            }
            else
            {
                ReportDefinitions.Add(new ReportDefinition
                {
                    ReportKey = strReportKey,
                    ReportDefinitions = new ReportDefinition[]
                                {
                                    new FormularyStatusNationalReportDefinition{ReportKey=strReportKey, Tile = tiles, EntityTypeName="ReportsFormularyStatusSummary", Sort = "Drug_Name asc", SectionTitle=strReportTitle + " National" },
                                    new FormularyStatusReportDefinition { ReportKey=strReportKey, Tile = tiles, EntityTypeName="ReportsFormularyStatusSummary", Sort = "Drug_Name asc", SectionTitle=strReportTitle },
                                }
                });


                ReportDefinitions.Add(new DrilldownReportDefinition { ReportKey = strReportKey, Tile = "Tile4Tools", EntityTypeName = "ReportsFormularyStatusDrilldown", Sort = "Formulary_Lives desc", SectionTitle = strReportTitle + " Drill Down" });
            }
        }

        protected override ObjectContext CreateObjectContext(PathfinderEntities context, bool IsCustom)
        {
            //ReportDefinitions[0].EntityTypeName = "ReportsFormularyStatus";
            //ReportDefinitions[1].EntityTypeName = "ReportsFormularyStatusDrilldown";
            //return CreateClientContext();
            return new PathfinderModel.PathfinderEntities();

            //return base.CreateObjectContext(context, IsCustom, filterSets);

        }

    }

    public class FormularyDrillDownReport : Report
    {
        protected override Dictionary<string, CriteriaItem> LoadCriteriaItems(NameValueCollection filters)
        {
            //Get base items - includes Market Basket and Section (among others)
            Dictionary<string, CriteriaItem> items = base.LoadCriteriaItems(filters);

            SRUtility sru = new SRUtility();
            return sru.LoadCriteriaItems(filters, items);
        }

        //For formularly drill down report
        //It checks if the user is allowed to see selected section/s based on User-Roles.
        //And returns only valid sections.
        protected string[] CheckFDDRoles(String FilterSectionIDs)
        {
            List<string> vldSectionList = new List<string>();

            bool cp = HttpContext.Current.User.IsInRole("frmly_1");
            bool medd = HttpContext.Current.User.IsInRole("frmly_17");
            bool sm = HttpContext.Current.User.IsInRole("frmly_9");
            bool dod = HttpContext.Current.User.IsInRole("frmly_12");
            bool pbm = HttpContext.Current.User.IsInRole("frmly_4");
            bool mm = HttpContext.Current.User.IsInRole("frmly_6");
            bool va = HttpContext.Current.User.IsInRole("frmly_11");

            if (!string.IsNullOrEmpty(FilterSectionIDs))
            {
                //Multiple sections are selected.
                if (FilterSets[0]["Section_ID"].ToString().IndexOf(",") > 0)
                {
                    //Split the data by comma 
                    string[] arrIDs = FilterSets[0]["Section_ID"].ToString().Split(new Char[] { ',' });
                    ReportDefinition fddRepDef = new ReportDefinition { };
                    foreach (string ids in arrIDs)
                    {
                        if ((ids == "1") && (cp))    //Commercial
                            vldSectionList.Add("1");
                        else if ((ids == "17") && (medd))     //Medicare Part D
                            vldSectionList.Add("17");
                        else if ((ids == "6") && (mm))  //Managed Medicaid
                            vldSectionList.Add("6");
                        else if ((ids == "9") && (sm)) //State Medicaid
                            vldSectionList.Add("9");
                        else if ((ids == "4") && (pbm)) //PBM
                            vldSectionList.Add("4");
                        else if ((ids == "11") && (va))     //VA
                            vldSectionList.Add("11");
                        else if ((ids == "12") && (dod))     //DoD
                            vldSectionList.Add("12");
                    }
                }
                else //For single section selection.
                {
                    String ids = FilterSets[0]["Section_ID"].ToString();
                    if ((ids == "1") && (cp))    //Commercial
                        vldSectionList.Add("1");
                    else if ((ids == "17") && (medd))     //Medicare Part D
                        vldSectionList.Add("17");
                    else if ((ids == "6") && (mm))  //Managed Medicaid
                        vldSectionList.Add("6");
                    else if ((ids == "9") && (sm)) //State Medicaid
                        vldSectionList.Add("9");
                    else if ((ids == "4") && (pbm)) //PBM
                        vldSectionList.Add("4");
                    else if ((ids == "11") && (va))     //VA
                        vldSectionList.Add("11");
                    else if ((ids == "12") && (dod))     //DoD
                        vldSectionList.Add("12");
                }
            }
            else //For All option
            {
                if (cp)    //Commercial
                    vldSectionList.Add("1");
                if (dod) //DoD
                    vldSectionList.Add("12");
                if (mm)  //Managed Medicaid
                    vldSectionList.Add("6");
                if (medd)     //Medicare Part D
                    vldSectionList.Add("17");
                if (sm) //State Medicaid
                    vldSectionList.Add("9");
                if (pbm) //PBM
                    vldSectionList.Add("4");
                if (va)  //VA
                    vldSectionList.Add("11");
            }

            return vldSectionList.ToArray();
        }

        protected override void BuildReportDefinitions()
        {
            string tile = "Tile3Tools, Tile3Commercial";
            //if (HasValue("Section_ID", "17"))    //Medicare Part D
            //    tile = "Tile3MedD";
            //else if (HasValue("Section_ID", "6")) //Managed Medicaid
            //    tile = "Tile3Tools, Tile3MM";
            //else if (HasValue("Section_ID", "9"))
            //    tile = "Tile3SM";   //State Medicaid
            //else if (HasValue("Section_ID", "4"))
            //    tile = "Tile3Tools, Tile3PBM";   //PBM
            //ReportDefinitions.Add(new DrilldownReportDefinition { ReportKey = "formularydrilldown", Tile = tile, EntityTypeName = "ReportsFormularyDrilldown", Sort = "Plan_Name, Section_Name, Drug_Name", SectionTitle = "Formulary Drill Down" });

            if (FilterSets.Count > 0)
            {
                string[] arrValidSections = new string[] { };

                ///Checks if the user is allowed to see selected section/s based on User-Roles.
                if (!string.IsNullOrEmpty(FilterSets[0]["Section_ID"])) //Single or multiple sections selected
                    arrValidSections = CheckFDDRoles(FilterSets[0]["Section_ID"]);
                else //All option is selected
                    arrValidSections = CheckFDDRoles("");

                if (arrValidSections.Length > 0)
                {
                    //ReportDefinition fddRepDef = new ReportDefinition { };
                    //foreach (string ids in arrValidSections)
                    //{
                    //    string fddTile = "";
                    //    string fddTitle = "";
                    //    string fddSectionID = "";
                    //    if (ids == "1")    //Commercial
                    //    {
                    //        fddTitle = "Formulary Drill Down - CP";
                    //        fddTile = "Tile3Tools, Tile3Commercial";
                    //        fddSectionID = "1";
                    //    }
                    //    else if (ids == "12")    //DoD
                    //    {
                    //        fddTitle = "Formulary Drill Down - DoD";
                    //        fddTile = "Tile3VADoD";
                    //        fddSectionID = "12";
                    //    }
                    //    else if (ids == "6") //Managed Medicaid
                    //    {
                    //        fddTitle = "Formulary Drill Down - MM";
                    //        fddTile = "Tile3Tools, Tile3MM";
                    //        fddSectionID = "6";
                    //    }
                    //    else if (ids == "17")    //Medicare Part D
                    //    {
                    //        fddTitle = "Formulary Drill Down - MedD";
                    //        fddTile = "Tile3MedD";
                    //        fddSectionID = "17";
                    //    }
                    //    else if (ids == "9") //State Medicaid
                    //    {
                    //        fddTitle = "Formulary Drill Down - SM";
                    //        fddTile = "Tile3SM";   //State Medicaid
                    //        fddSectionID = "9";
                    //    }
                    //    else if (ids == "4") //PBM
                    //    {
                    //        fddTitle = "Formulary Drill Down - PBM";
                    //        fddTile = "Tile3Tools, Tile3PBM";   //PBM
                    //        fddSectionID = "4";
                    //    }
                    //    else if (ids == "11")    //VA
                    //    {
                    //        fddTitle = "Formulary Drill Down - VA";
                    //        fddTile = "Tile3VADoD";
                    //        fddSectionID = "11";
                    //    }

                    //    fddRepDef.ReportDefinitions.Add(new FrmlyDrilldownReportDefinition { ReportKey = "formularydrilldown", Tile = fddTile, EntityTypeName = "ReportsFormularyDrilldown", Sort = "Plan_Name, Drug_Name", SectionTitle = fddTitle, FDDRepSectionID = fddSectionID });
                    //}

                    //ReportDefinitions.Add(new ReportDefinition
                    //{
                    //    ReportKey = "formularydrilldown",
                    //    ReportDefinitions = fddRepDef.ReportDefinitions
                    //});
                    ReportDefinitions.Add(new FrmlyDrilldownReportDefinition { ReportKey = "formularydrilldown", Tile = tile, EntityTypeName = "ReportsFormularyDrilldown", Sort = "Plan_Name, Section_Name, Drug_Name", SectionTitle = "Formulary Drill Down" });
                }
            }
        }

        protected override ObjectContext CreateObjectContext(PathfinderEntities context, bool IsCustom)
        {
           //return CreateClientContext();           
            return new PathfinderModel.PathfinderEntities();
        }
    }

    public class FormularyCoverageReport : Report
    {
        protected override Dictionary<string, CriteriaItem> LoadCriteriaItems(NameValueCollection filters)
        {
            //Get base items - includes Market Basket and Section (among others)
            Dictionary<string, CriteriaItem> items = base.LoadCriteriaItems(filters);

            SRUtility sru = new SRUtility();
            return sru.LoadCriteriaItems(filters, items);
        }

        protected override void BuildReportDefinitions()
        {
            string tiles = "";             //"Tile3";

            if (FilterSets.Count > 0)
            {
                if (!String.IsNullOrEmpty(FilterSets[0]["Selected_Section_ID"]))
                {
                    FilterSets[0].Remove("Section_ID");

                    if (FilterSets[0]["Selected_Section_ID"] != "0")
                        FilterSets[0].Add("Section_ID", FilterSets[0]["Selected_Section_ID"]);
                }
            }


            //sl 6/28/2012  updated by adding "Lives" column to Summary 

            //if (FilterSets.Count > 0 && !string.IsNullOrEmpty(FilterSets[0]["Section_ID"]))
            //    tiles = "Tile3, Tile3Section";

            if (FilterSets.Count > 0 && !string.IsNullOrEmpty(FilterSets[0]["Section_ID"]))
            {
                string[] sectionID = FilterSets[0]["Section_ID"].Split(',');
                if (sectionID.Length == 1)
                {
                    if (HasValue("Section_ID", "1"))
                        tiles = "Tile3, Tile3CL"; //Commercial Payer
                    else if (HasValue("Section_ID", "4"))
                        tiles = "Tile3, Tile3PBM"; //PBM
                    else if (HasValue("Section_ID", "17"))
                        tiles = "Tile3, Tile3MedD"; //Medicare Part D
                    else if (HasValue("Section_ID", "6"))
                        tiles = "Tile3, Tile3MM"; //Managed Medicaid
                    else
                        tiles = "Tile3, Tile3CL";
                }
                else
                    tiles = "Tile3, Tile3_section, Tile3CL";
            }
            else
                tiles = "Tile3, Tile3CL";

            

            //changes for report title change
            string strReportTitle = Resources.Resource.SectionTitle_FormularyCoverage;
            string strReportKey = "formularycoverage";

            ReportDefinitions.Add(new ReportDefinition
            {
                ReportKey = strReportKey,
                ReportDefinitions = new ReportDefinition[]
                                {
                                    new FormularyCoverageNationalReportDefinition{ReportKey=strReportKey, Tile = tiles, EntityTypeName="FormularyCoverageSummary", Sort = "Drug_Name asc", SectionTitle=strReportTitle + " National" },
                                    new FormularyCoverageReportDefinition { ReportKey=strReportKey, Tile = tiles, EntityTypeName="FormularyCoverageSummary", Sort = "Drug_Name asc", SectionTitle=strReportTitle },
                                }
            });

            ReportDefinitions.Add(new DrilldownReportDefinition { ReportKey = strReportKey, Tile = "Tile4Tools", EntityTypeName = "FormularyCoverageDrilldown", Sort = "Formulary_Lives desc", SectionTitle = strReportTitle + " Drill Down" });

        }

        protected override ObjectContext CreateObjectContext(PathfinderEntities context, bool IsCustom)
        {
            //ReportDefinitions[0].EntityTypeName = "ReportsFormularyStatus";
            //ReportDefinitions[1].EntityTypeName = "ReportsFormularyStatusDrilldown";
            //return CreateClientContext();
            return new PathfinderModel.PathfinderEntities();

            //return base.CreateObjectContext(context, IsCustom, filterSets);

        }

    }

}
