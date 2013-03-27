using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pinsonault.Data.Reports;
using Pinsonault.Application;
using Pinsonault.Application.Millennium;

namespace Pinsonault.Application.millennium
{
    public class PlanInformationReport : Pinsonault.Application.TodaysAccounts.PlanInformationReport
    {
        protected override void BuildReportDefinitions()
        {
            string val = FindValues("Plan_ID").FirstOrDefault();
            string channel = HttpContext.Current.Request.QueryString["channel"];
            int planID = 0;
            if ( !string.IsNullOrEmpty(val) )
                int.TryParse(val, out planID);

            //check if standard plan - if not then it is custom millennium plan.
            if (BaseObjectContext.PlanMasterSet.Count(p => p.Plan_ID == planID) == 0)
            {
                if (Convert.ToInt32(channel) == 105)
                    ReportDefinitions.Add(new ReportDefinition { ReportKey = "millennium_planinformation", Tile = "Tile3Tools_MJ", EntityTypeName = "PlansClientExport", Style = ReportStyle.List, SectionTitle = Resources.Resource.SectionTitle_PlanInfo });
                else if (Convert.ToInt32(channel) == 106)
                    ReportDefinitions.Add(new ReportDefinition { ReportKey = "millennium_planinformation", Tile = "Tile3Tools_SS", EntityTypeName = "PlansClientExport", Style = ReportStyle.List, SectionTitle = Resources.Resource.SectionTitle_PlanInfo });
                else if (Convert.ToInt32(channel) == 107)
                    ReportDefinitions.Add(new ReportDefinition { ReportKey = "millennium_planinformation", Tile = "Tile3Tools_VA", EntityTypeName = "PlansClientExport", Style = ReportStyle.List, SectionTitle = Resources.Resource.SectionTitle_PlanInfo });
                else if (Convert.ToInt32(channel) == 108)
                    ReportDefinitions.Add(new ReportDefinition { ReportKey = "millennium_planinformation", Tile = "Tile3Tools_RTA", EntityTypeName = "PlansClientExport", Style = ReportStyle.List, SectionTitle = Resources.Resource.SectionTitle_PlanInfo });
            }
            else
            {
                if (Convert.ToInt32(channel) == 105)
                    ReportDefinitions.Add(new ReportDefinition { ReportKey = "millennium_planinformation", Tile = "Tile3Tools_MJ", EntityTypeName = "PlansClientExport", Style = ReportStyle.List, SectionTitle = Resources.Resource.SectionTitle_PlanInfo });
                else if (Convert.ToInt32(channel) == 106)
                    ReportDefinitions.Add(new ReportDefinition { ReportKey = "millennium_planinformation", Tile = "Tile3Tools_SS", EntityTypeName = "PlansClientExport", Style = ReportStyle.List, SectionTitle = Resources.Resource.SectionTitle_PlanInfo });
                else if (Convert.ToInt32(channel) == 107)
                    ReportDefinitions.Add(new ReportDefinition { ReportKey = "millennium_planinformation", Tile = "Tile3Tools_VA", EntityTypeName = "PlansClientExport", Style = ReportStyle.List, SectionTitle = Resources.Resource.SectionTitle_PlanInfo });
                else if (Convert.ToInt32(channel) == 108)
                    ReportDefinitions.Add(new ReportDefinition { ReportKey = "millennium_planinformation", Tile = "Tile3Tools_RTA", EntityTypeName = "PlansClientExport", Style = ReportStyle.List, SectionTitle = Resources.Resource.SectionTitle_PlanInfo });
                else
                    base.BuildReportDefinitions();
            }
        }
    }

    public class KDMDetailsReport : Pinsonault.Application.TodaysAccounts.KeyContactsReport
    {
        protected override void BuildReportDefinitions()
        {
            string val = FindValues("Plan_ID").FirstOrDefault();
            string channel = HttpContext.Current.Request.QueryString["channel"];
            String KDMString = HttpContext.Current.Request.QueryString["_data1"];
            string KDMval = "";
            if (KDMString != null )
            {
                string[] KDM = KDMString.Split('=');
                KDMval = KDM[1];
            }

            
            int planID = 0;
            if (!string.IsNullOrEmpty(val))
                int.TryParse(val, out planID);
            if (BaseObjectContext.PlanMasterSet.Count(p => p.Plan_ID == planID) == 0)
            {
                
                if (Convert.ToInt32(channel) == 105)
                {   // For MJ include All Columns
                    ReportDefinitions.Add(new ReportDefinition { ReportKey = "millennium_kdm", Tile = "Tile3Tools, Tile3ToolsCACCMD, Tile3ToolsTitles, Tile3ToolsCredentials, Tile3ToolsCAC, Tile3Tools_Aff_MJ_Org_Name,  Tile3ToolsPCMI", EntityTypeName = "KDMReport", SectionTitle = "Key Decision Maker" });
                }
                else if (Convert.ToInt32(channel) == 106)
                {   // For SS column Credentials and CAC is not required
                    ReportDefinitions.Add(new ReportDefinition { ReportKey = "millennium_kdm", Tile = "Tile3Tools, Tile3ToolsTitles, Tile3ToolsPCMI", EntityTypeName = "KDMReport", SectionTitle = "Key Decision Maker" });
                }
                else if (Convert.ToInt32(channel) == 107)
                {   // For VA CAC is not required
                    ReportDefinitions.Add(new ReportDefinition { ReportKey = "millennium_kdm", Tile = "Tile3Tools, Tile3ToolsTitles, Tile3ToolsCredentials, Tile3ToolsSpecialty, Tile3ToolsJobFunction, Tile3ToolsPCMI", EntityTypeName = "KDMReport", SectionTitle = "Key Decision Maker" });
                }
                else if (Convert.ToInt32(channel) == 108)
                {   // For RTA Professional Customer Master ID and CAC are not required
                    ReportDefinitions.Add(new ReportDefinition { ReportKey = "millennium_kdm", Tile = "Tile3Tools, Tile3ToolsTitles, Tile3ToolsCredentials, Tile3ToolsSpecialty, Tile3ToolsJobFunction, Tile3Tools_RTA_Affiliation", EntityTypeName = "KDMReport", SectionTitle = "Key Decision Maker" });
                }
                if (KDMval != "")
                { //Loop For Corresponding Address Grid if Any Key Decision Maker is Selected
                    using (PathfinderMillenniumEntities context = new PathfinderMillenniumEntities())
                    {
                        int KDMint = Convert.ToInt32(KDMval);
                        var KDMName = (from j in context.KDMDetailSet
                                       where j.KDM_ID == KDMint
                                       select j.KDM_F_Name.ToUpper() + " " + j.KDM_L_Name.ToUpper()).First();
                    
                    var SectionName = "Address for " + KDMName;
                    ReportDefinitions.Add(new ReportDefinition { ReportKey = "millennium_kdm_Address", Tile = "Tile4Tools", EntityTypeName = "KDMAddress", SectionTitle = SectionName });
                    }
                }
            }
            else
                base.BuildReportDefinitions();
        }
    }
}