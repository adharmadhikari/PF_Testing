using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Pinsonault.Data.Reports;
using System.Data.Objects;
using Pinsonault.Data;
using PathfinderModel;
using System.Collections.Specialized;


namespace Pinsonault.Application.TodaysAccounts
{
    public class PlanInformationReport : Report
    {
        protected override void BuildReportDefinitions()
        {
            ReportDefinitions.Add(new ReportDefinition
            {
                ReportKey = "planinformation",
                ReportDefinitions = new ReportDefinition[]
                {
                    //Report Defs are nested because they share same query string data - also changing EntityTypeName based on Section_ID filter if available
                    new ReportDefinition { ReportKey="planinformation", Tile="Tile3Tools",EntityTypeName= "PlanInformation"  , Style=ReportStyle.List, SectionTitle=Resources.Resource.SectionTitle_PlanInfo},
                    new ReportDefinition { ReportKey="planinformation", Tile="Tile4Tools", EntityTypeName="PlanAddressComment", Style=ReportStyle.List, SectionTitle=Resources.Resource.SectionTitle_PlanInfoAddress},
                    new PlanInfoAffiliationsReportDefinition { ReportKey="planinformation", Tile="PBM",EntityTypeName= "PlanParentAffiliationListView", AffilTypeID=3, SectionTitle=Resources.Resource.Label_PBM },
                    new PlanInfoAffiliationsReportDefinition { ReportKey="planinformation", Tile="SPP",EntityTypeName= "PlanParentAffiliationListView", AffilTypeID=2, SectionTitle=Resources.Resource.Label_SPP }
                }
            });
        }
    }

    public class KeyContactsReport : Report
    {
        protected override void BuildReportDefinitions()
        {
            ReportDefinitions.Add(new ReportDefinition { ReportKey = "contacts", Tile = "profile", EntityTypeName = "V_KeyContacts_Plan_Details", Style = ReportStyle.List, Visible = false, SectionTitle = Resources.Resource.SectionTitle_KeyContacts });
            ReportDefinitions.Add(new ReportDefinition { ReportKey = "contacts", Tile = "profile", EntityTypeName = "V_KeyContacts_Plan_Details", Style = ReportStyle.List, SectionTitle = Resources.Resource.SectionTitle_KeyContacts });
        }
    }
    /// <summary>
    /// For exporting my key contacts list
    /// </summary>
    public class MyKeyContactsReport : Report
    {
        protected override void BuildReportDefinitions()
        {
            ReportDefinitions.Add(new ReportDefinition { ReportKey = "mykeycontacts", Tile = "profile", EntityTypeName = "KeyContact", Style = ReportStyle.Grid, SectionTitle = Resources.Resource.SectionTitle_MyKeyContacts });
        }
        protected override ObjectContext CreateObjectContext(PathfinderEntities context, bool IsCustom)
        {
            return CreateClientContext();
        }
    }

    ///<summary>
    ///For exporting Lives/Formulary Data
    ///</summary>

    public class CoveredLivesReport : Report
    {
        protected override Dictionary<string, CriteriaItem> LoadCriteriaItems(NameValueCollection filters)
        {
            //Get base items - includes Market Basket and Section (among others)
            Dictionary<string, CriteriaItem> items = base.LoadCriteriaItems(filters);
            if (filters.AllKeys.Contains("__options")) 
            {
                if (filters["__options"].Contains("Prod_Name") || filters["__options"].Contains("Formulary_Name"))
                {
                    string select = filters["__options"].Trim('{', '}', '"').Replace('"', ' ').Split(':')[1];
                    items["section_id"].Text = items["section_id"].Text + " - " + select;
                }
                //if (list.ContainsKey("formulary_name"))
                //{

                //    //CriteriaItem section = list.Values.Where(j => j.Key == "Section_ID").FirstOrDefault();
                //    //CriteriaItem formulary = list.Values.Where(j => j.Key == "Formulary_Name").FirstOrDefault();
                //    list["section_id"].Text = list["section_id"].Text + " - " + list["formulary_name"].Text;
                //    list.Remove("formulary_name");

                //}
            }
            
            return items;
        }
        protected override void BuildReportDefinitions()
        {
            string section_id = null;

            if (FindValues("Section_ID").Count() > 1)
               section_id = FindValues("Section_ID").ElementAt(1).ToString();
            
            //string section_id = FindValues("Section_ID").ElementAt(1).ToString();

            string user_id = string.Empty;
            if (!string.IsNullOrEmpty(section_id))
            {
                user_id = "frmly_" + section_id;
            }

            //Combined 
            if (HasValue("Section_ID", "99"))
            {

                IList<ReportDefinition> definitions = new List<ReportDefinition>();
                
                //Add base report definitions
                definitions.Add(new CoveredLivesReportDefinition { ReportKey = "LivesFormulary", Tile = "Tile5Tools", EntityTypeName = "PlanInformation", Style = ReportStyle.List, SectionTitle = Resources.Resource.SectionTitle_CoveredLives });
                definitions.Add(new CoveredLivesReportDefinition { ReportKey = "LivesFormulary", Tile = "LivesDistribution", EntityTypeName = "V_CoveredLives", Style = ReportStyle.Grid, SectionTitle = "Lives Distribution", Sort = "Covered_Lives_Order"});

                //Check to see which segments were selected
                string original_section = null;

                //if (!string.IsNullOrEmpty(FindValues("Original_Section").ElementAt(0)))
                original_section = FindValues("Original_Section").ElementAt(0).ToString();

                string[] sectionIDs = original_section.Split(',');

                if (sectionIDs.Contains("1"))
                    definitions.Add(new CoveredLivesReportDefinition { ReportKey = "LivesFormulary", Tile = "Commercial", EntityTypeName = "BenefitDesignCommercial", Style = ReportStyle.Grid, SectionTitle = Resources.Resource.SectionTitle_BenefitDesign + " - Commercial" });
                if (sectionIDs.Contains("6"))
                    definitions.Add(new CoveredLivesReportDefinition { ReportKey = "LivesFormulary", Tile = "ManagedMedicaid", EntityTypeName = "BenefitDesignManagedMedicaid", Style = ReportStyle.Grid, SectionTitle = Resources.Resource.SectionTitle_BenefitDesign + "-ManagedMedicaid" });
                if (sectionIDs.Contains("17"))
                    definitions.Add(new CoveredLivesReportDefinition { ReportKey = "LivesFormulary", Tile = "Medicare", EntityTypeName = "BenefitDesignMedD", Style = ReportStyle.Grid, SectionTitle = Resources.Resource.SectionTitle_BenefitDesign + " - Medicare Part D" });

                ReportDefinitions.Add(new ReportDefinition
                {
                    ReportKey = "LivesFormulary",
                    ReportDefinitions = definitions.ToArray()
                });

                //ReportDefinitions.Add(new ReportDefinition
                //{
                //    ReportKey = "LivesFormulary",
                //    ReportDefinitions = new ReportDefinition[]
                //    {
                //    new CoveredLivesReportDefinition { ReportKey = "LivesFormulary", Tile = "Tile5Tools", EntityTypeName = "PlanInformation", Style = ReportStyle.List, SectionTitle = Resources.Resource.SectionTitle_CoveredLives },
                //    new CoveredLivesReportDefinition { ReportKey = "LivesFormulary", Tile = "LivesDistribution", EntityTypeName = "V_CoveredLives", Style = ReportStyle.Grid, SectionTitle = "Lives Distribution", Sort = "Covered_Lives_Order"},
                //    new CoveredLivesReportDefinition { ReportKey = "LivesFormulary", Tile = "Commercial", EntityTypeName = "BenefitDesignCommercial", Style = ReportStyle.Grid, SectionTitle = Resources.Resource.SectionTitle_BenefitDesign + " - Commercial" },
                //    new CoveredLivesReportDefinition { ReportKey = "LivesFormulary", Tile = "ManagedMedicaid", EntityTypeName = "BenefitDesignManagedMedicaid", Style = ReportStyle.Grid, SectionTitle = Resources.Resource.SectionTitle_BenefitDesign + "-ManagedMedicaid" },
                //    new CoveredLivesReportDefinition { ReportKey = "LivesFormulary", Tile = "Medicare", EntityTypeName = "BenefitDesignMedD", Style = ReportStyle.Grid, SectionTitle = Resources.Resource.SectionTitle_BenefitDesign + " - Medicare Part D" }
                //    }
                //});
            }
            //All
            if (HasValue("Section_ID", "0"))
            {
                ReportDefinitions.Add(new ReportDefinition
                {
                    ReportKey = "LivesFormulary",
                    ReportDefinitions = new ReportDefinition[]
                        {
                        new CoveredLivesReportDefinition { ReportKey = "LivesFormulary", Tile = "Tile5Tools", EntityTypeName = "PlanInformation", Style = ReportStyle.List, SectionTitle = Resources.Resource.SectionTitle_CoveredLives },
                        new CoveredLivesReportDefinition { ReportKey = "LivesFormulary", Tile = "LivesDistribution", EntityTypeName = "V_CoveredLives", Style = ReportStyle.Grid, SectionTitle = "Lives Distribution", Sort = "Covered_Lives_Order"},
                        new CoveredLivesReportDefinition { ReportKey = "LivesFormulary", Tile = "Commercial", EntityTypeName = "BenefitDesignCommercial", Style = ReportStyle.Grid, SectionTitle = Resources.Resource.SectionTitle_BenefitDesign + " - Commercial" },
                        new CoveredLivesReportDefinition { ReportKey = "LivesFormulary", Tile = "Medicare", EntityTypeName = "BenefitDesignMedD", Style = ReportStyle.Grid, SectionTitle = Resources.Resource.SectionTitle_BenefitDesign + " - Medicare Part D" }
                        }
                });
            }
            //Commercial
            if (HasValue("Section_ID", "1"))
            {
                ReportDefinitions.Add(new ReportDefinition
                {
                    ReportKey = "LivesFormulary",
                    ReportDefinitions = new ReportDefinition[]
                    {
                    //new CoveredLivesReportDefinition { ReportKey = "LivesFormulary", Tile = "Tile5Tools", EntityTypeName = "PlanInformation", Style = ReportStyle.List, SectionTitle = Resources.Resource.SectionTitle_CoveredLives },
                    new CoveredLivesReportDefinition { ReportKey = "LivesFormulary", Tile = "LivesDistribution", EntityTypeName = "CoveredLivesCommercial", Style = ReportStyle.Grid, SectionTitle = "Lives Distribution"},
                    new CoveredLivesReportDefinition { ReportKey = "LivesFormulary", Tile = "Commercial", EntityTypeName = "BenefitDesignCommercial", Style = ReportStyle.Grid, SectionTitle = Resources.Resource.SectionTitle_BenefitDesign + " - Commercial" },
                    //new CoveredLivesReportDefinition { ReportKey = "LivesFormulary", Tile = "Medicare", EntityTypeName = "BenefitDesignMedD", Style = ReportStyle.Grid, SectionTitle = Resources.Resource.SectionTitle_BenefitDesign + " - Medicare Part D" }
                    }
                });
            }

            //DOD
            if (HasValue("Section_ID", "12"))
            {
                ReportDefinitions.Add(new ReportDefinition
                {
                    ReportKey = "LivesFormulary",
                    Visible = true,
                    ReportDefinitions = new ReportDefinition[]
                    {
                    new CoveredLivesReportDefinition { ReportKey = "LivesFormulary", Tile = "Tile5Tools", EntityTypeName = "PlanInformation", Style = ReportStyle.List,SectionTitle = Resources.Resource.SectionTitle_CoveredLives },
                    //new CoveredLivesReportDefinition { ReportKey = "LivesFormulary", Tile = "LivesDistribution", EntityTypeName = "V_CoveredLives", Style = ReportStyle.Grid, SectionTitle = "Lives Distribution"},
                    new CoveredLivesReportDefinition { ReportKey = "LivesFormulary", Tile = "Commercial", EntityTypeName = "BenefitDesign", Style = ReportStyle.Grid, SectionTitle = Resources.Resource.SectionTitle_BenefitDesign + " - DOD "}
                    }
                });
            }
            //FEP
            if (HasValue("Section_ID", "20"))
            {
                ReportDefinitions.Add(new ReportDefinition
                {
                    ReportKey = "LivesFormulary",
                    ReportDefinitions = new ReportDefinition[]
                    {
                    new CoveredLivesReportDefinition { ReportKey = "LivesFormulary", Tile = "Tile5Tools", EntityTypeName = "PlanInformation", Style = ReportStyle.List, SectionTitle = Resources.Resource.SectionTitle_CoveredLives },
                    new CoveredLivesReportDefinition { ReportKey = "LivesFormulary", Tile = "LivesDistribution", EntityTypeName = "V_CoveredLives", Style = ReportStyle.Grid, SectionTitle = "Lives Distribution", Sort = "Covered_Lives_Order"},
                    new CoveredLivesReportDefinition { ReportKey = "LivesFormulary", Tile = "Commercial", EntityTypeName = "BenefitDesignCommercial", Style = ReportStyle.Grid, SectionTitle = Resources.Resource.SectionTitle_BenefitDesign + " - Commercial" },
                    new CoveredLivesReportDefinition { ReportKey = "LivesFormulary", Tile = "Medicare", EntityTypeName = "BenefitDesignMedD", Style = ReportStyle.Grid, SectionTitle = Resources.Resource.SectionTitle_BenefitDesign + " - Medicare Part D" }
                    }
                });
            }
            //Managed Medicare
            if (HasValue("Section_ID", "6"))
            {
                ReportDefinitions.Add(new ReportDefinition
                {
                    ReportKey = "LivesFormulary",
                    ReportDefinitions = new ReportDefinition[]
                    {
                    new CoveredLivesReportDefinition { ReportKey = "LivesFormulary", Tile = "Tile5Tools", EntityTypeName = "PlanInformation", Style = ReportStyle.List, SectionTitle = Resources.Resource.SectionTitle_CoveredLives },
                    new CoveredLivesReportDefinition { ReportKey = "LivesFormulary", Tile = "LivesDistribution", EntityTypeName = "V_CoveredLives", Style = ReportStyle.Grid, SectionTitle = "Lives Distribution", Sort = "Covered_Lives_Order"},
                    //new CoveredLivesReportDefinition { ReportKey = "LivesFormulary", Tile = "Commercial", EntityTypeName = "BenefitDesignCommercial", Style = ReportStyle.Grid, SectionTitle = Resources.Resource.SectionTitle_BenefitDesign + " - Commercial" },
                    //new CoveredLivesReportDefinition { ReportKey = "LivesFormulary", Tile = "Medicare", EntityTypeName = "BenefitDesignMedD", Style = ReportStyle.Grid, SectionTitle = Resources.Resource.SectionTitle_BenefitDesign + " - Medicare Part D" }
                    
                    new CoveredLivesReportDefinition { ReportKey = "LivesFormulary", Tile = "ManagedMedicaid", EntityTypeName = "BenefitDesignManagedMedicaid", Style = ReportStyle.Grid, SectionTitle = Resources.Resource.SectionTitle_BenefitDesign + "-ManagedMedicaid" }
                    
                    }
                });
            }

            // Medicare Part-D
            if (HasValue("Section_ID", "17"))
            {
                ReportDefinitions.Add(new ReportDefinition
                  {
                      ReportKey = "LivesFormulary",
                      ReportDefinitions = new ReportDefinition[]
                    {
                        //new CoveredLivesReportDefinition { ReportKey = "LivesFormulary", Tile = "Tile5Tools", EntityTypeName = "PlanInformation", Style = ReportStyle.List, SectionTitle = Resources.Resource.SectionTitle_CoveredLives },
                        new CoveredLivesReportDefinition { ReportKey = "LivesFormulary", Tile = "LivesDistribution", EntityTypeName = "CoveredLivesMedD", Style = ReportStyle.Grid, SectionTitle = Resources.Resource.SectionTitle_CoveredLives + "Lives Distribution", Sort = "Covered_Lives_Order"},                        
                        new CoveredLivesReportDefinition { ReportKey = "LivesFormulary", Tile = "Medicare", EntityTypeName = "BenefitDesignMedD", Style = ReportStyle.Grid, SectionTitle = Resources.Resource.SectionTitle_BenefitDesign + " - Medicare Part D" }
                    }
                  });
            }
            //PBM
            if (HasValue("Section_ID", "4"))
            {
                ReportDefinitions.Add(new ReportDefinition
                {
                    ReportKey = "LivesFormulary",
                    ReportDefinitions = new ReportDefinition[]
                    {
                    new CoveredLivesReportDefinition { ReportKey = "LivesFormulary", Tile = "Tile5Tools", EntityTypeName = "PlanInformation", Style = ReportStyle.List, SectionTitle = Resources.Resource.SectionTitle_CoveredLives },
                    new CoveredLivesReportDefinition { ReportKey = "LivesFormulary", Tile = "LivesDistribution", EntityTypeName = "V_CoveredLives", Style = ReportStyle.Grid, SectionTitle = "Lives Distribution" , Sort = "Covered_Lives_Order"},
                    new CoveredLivesReportDefinition { ReportKey = "LivesFormulary", Tile = "Commercial", EntityTypeName = "BenefitDesignPBM", Style = ReportStyle.Grid, SectionTitle = Resources.Resource.SectionTitle_BenefitDesign + " - PBM" }
                    //new CoveredLivesReportDefinition { ReportKey = "LivesFormulary", Tile = "Commercial", EntityTypeName = "BenefitDesignCommercial", Style = ReportStyle.Grid, SectionTitle = Resources.Resource.SectionTitle_BenefitDesign + " - Commercial" },
                    //new CoveredLivesReportDefinition { ReportKey = "LivesFormulary", Tile = "Medicare", EntityTypeName = "BenefitDesignMedD", Style = ReportStyle.Grid, SectionTitle = Resources.Resource.SectionTitle_BenefitDesign + " - Medicare Part D" }
                    }
                });
            }
            //State Medicaid
            if (HasValue("Section_ID", "9"))
            {
                ReportDefinitions.Add(new ReportDefinition
                {
                    ReportKey = "LivesFormulary",
                    Visible = true,
                    ReportDefinitions = new ReportDefinition[]
                    {
                    new CoveredLivesReportDefinition { ReportKey = "LivesFormulary", Tile = "StateMedicaid", EntityTypeName = "PlanInfoStateMedicaid", Style = ReportStyle.List,SectionTitle = Resources.Resource.SectionTitle_CoveredLives },
                    //new CoveredLivesReportDefinition { ReportKey = "LivesFormulary", Tile = "LivesDistribution", EntityTypeName = "V_CoveredLives", Style = ReportStyle.Grid, SectionTitle = "Lives Distribution"},
                    new CoveredLivesReportDefinition { ReportKey = "LivesFormulary", Tile = "StateMedicaid-BD", EntityTypeName = "BenefitDesign", Style = ReportStyle.Grid, SectionTitle = Resources.Resource.SectionTitle_BenefitDesign}
                    }
                });
            }

            if  (!string.IsNullOrEmpty(user_id) && HttpContext.Current.User.IsInRole(user_id))
            {
                ReportDefinitions.Add(new DrilldownReportDefinition {ReportKey = "LivesFormulary",  Tile = "BD_Druglevel", EntityTypeName = "CoveredLivesDrilldown", Style = ReportStyle.Grid, SectionTitle = Resources.Resource.SectionTitle_BenefitDesign + " - Drug Level" });
            }
        }
    }

    // <summary>
    /// For exporting formulary history comparison
    /// </summary>
    public class FormularyHistoryReport : Report
    {
        protected override Dictionary<string, CriteriaItem> LoadCriteriaItems(NameValueCollection filters)
        {
            Dictionary<string, CriteriaItem> items = base.LoadCriteriaItems(filters);

            CriteriaItem item = new CriteriaItem("Formulary_Name", "Formulary");
            item.Text = filters["__options"].Trim('{', '}', '"').Replace('"', ' ').Split(':')[1].Replace("|", " ");
            items.Add(item.Key, item);

            item = new CriteriaItem("Thera_Name", "Class");
            item.Text = filters["Thera_Name"].Replace("|", " ");
            items.Add(item.Key, item);

            return items;
        }

        protected override void BuildReportDefinitions()
        {
            IList<string> tiles = new List<string>();
            
            //Add Base tiles
            tiles.Add("Tile5ToolsBase");

            //Add Tier only if State Medicaid Section is not selected
            if (FilterSets[0]["Section_ID"] != "9")
                tiles.Add("Tile5Tier");

            ReportDefinitions.Add(new FormularyHistoryReportDefinition { ReportKey = "formularyhistoryreporting_ta", Style = ReportStyle.DualHeaderGrid, Tile = string.Join(", ", tiles.ToArray()), SectionTitle = Resources.Resource.SectionTitle_HxReport });
        }
        protected override ObjectContext CreateObjectContext(PathfinderEntities context, bool IsCustom)
        {
            return CreateClientContext();
        }
    }
}
