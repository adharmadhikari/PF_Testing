using System.Data.Objects;
using System.Linq;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Text;
using Pinsonault.Data;

namespace PathfinderModel
{
    /// <summary>
    /// Customized methods and properties for PathfinderEntities context that relate to reports.
    /// </summary>
    public partial class PathfinderEntities
    {
        public IQueryable<PathfinderModel.SectionReportFilter> GetReportFilters(string reportKey, int clientID, int channelID)
        {
            return from filter in ReportFilterSet.Include("FilterProperties")
                   join filterSection in SectionReportFilterSet on filter equals filterSection.ReportFilter
                    join report in ModuleSet on filterSection.Module equals report 
                    join clientReport in ClientModuleSet on report.ID equals clientReport.Module_ID 
                    where                     
                    report.Module_Key == reportKey
                    && clientReport.Client_ID == clientID
                    //&& report.Client.Client_ID == clientID
                    && filterSection.Section_ID == channelID
                   orderby filterSection.Display_Order, filterSection.Filter_ID 
                    select filterSection;
        }

        public IEnumerable<MapGeographyData> GetMapViewCoverage(List<string> Channel1, int? DrugID,int? MarketBasketID, int ClientID, string Restrictions )
        {
            IList<MapGeographyData> data = null;
            if(string.IsNullOrEmpty(Restrictions)) {Restrictions = "";}
            //string[] cha = Channel1.Split(',');
            string[] cha = Channel1.ToArray();
            int[] channelValues = new int[cha.Length];
            for (int x = 0; x < cha.Length; x++)
            {
                channelValues[x] = Convert.ToInt32(cha[x].ToString());
            }
            int Channel = channelValues[0];
            string strwhere = string.Empty;
            strwhere = " it.Section_ID in {" + Channel1 + "}";

            if (DrugID != null)
            {
               
                switch(Restrictions)
                {
                    case "":
                        data = GetCoverageMapData(ClientID, DrugID.Value, Channel1, "Coverage_Status_ID").ToList();
                        break;
                    case "PA":
                        data = GetCoverageMapData(ClientID, DrugID.Value, Channel1, "PA_Coverage_Status_ID").ToList();
                        break;
                    case "QL":
                        data = GetCoverageMapData(ClientID, DrugID.Value, Channel1, "QL_Coverage_Status_ID").ToList();
                        break;
                    case "ST":
                        data = GetCoverageMapData(ClientID, DrugID.Value, Channel1, "ST_Coverage_Status_ID").ToList();
                        break;
                    case "PA_QL":
                        data = GetCoverageMapData(ClientID, DrugID.Value, Channel1, "PA_QL_Coverage_Status_ID").ToList();
                        break;
                    case "PA_ST":
                        data = GetCoverageMapData(ClientID, DrugID.Value, Channel1, "PA_ST_Coverage_Status_ID").ToList();
                        break;
                    case "QL_ST":
                        data = GetCoverageMapData(ClientID, DrugID.Value, Channel1, "QL_ST_Coverage_Status_ID").ToList();
                        break;
                    case "PA_QL_ST":
                       data = GetCoverageMapData(ClientID, DrugID.Value, Channel1, "PA_QL_ST_Coverage_Status_ID").ToList();
                        break;

                }
               
                
            }
            else  //Get just enrollment (lives) data by state if no drug id specified
            {
                var q = from s in StateEnrollmentSet
                        where s.Section_ID == Channel
                        select new MapGeographyData { GeographyID = s.Geography_ID, GeographyName = s.Geography_ID, Category = "d", Enrollment = s.Enrollment != null ? s.Enrollment.Value : 0 };

                data = q.ToList();
            }

            //return results of data and default list unioned - default list fills in gaps in data
            return data.Union(MapGeographyData.DefaultData, new MapGeographyDataComparer());                
        }
        public IList<MapGeographyData> GetCoverageMapData(int ClientID, int DrugID, List<string> ChannelID, string str_CoverageStatusID)
        {
            
            //string[] cha = ChannelID.Split(',');
            string[] cha = ChannelID.ToArray();
            int[] channelValues = new int[cha.Length];
            for (int x = 0; x < cha.Length; x++)
            {
                channelValues[x] = Convert.ToInt32(cha[x].ToString());
            }

            List<MapGeographyData> List = new List<MapGeographyData>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["Pathfinder"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Get_GeoCoverageReportData", cn)) 
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Client_ID", ClientID);
                    cmd.Parameters.AddWithValue("@Drug_ID0", DrugID);
                    if (channelValues[0] == 4 && cha.Length == 1) { cmd.Parameters.AddWithValue("@OnlyPBM", 1); }
                    else { cmd.Parameters.AddWithValue("@OnlyPBM", 0); }
                    for (int x = 0; x < 7; x++)
                    {
                        if(x < cha.Length)
                            cmd.Parameters.AddWithValue("@Section_ID" + x, channelValues[x]);
                        else
                            cmd.Parameters.AddWithValue("@Section_ID" + x, 0);
                    }

                    cn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            string Geographic_ID = rdr["Geography_ID"].ToString();
                            int CoverageStatus_ID = Convert.ToInt32(rdr[str_CoverageStatusID].ToString());
                            int Enrollment = Convert.ToInt32(rdr["Enrollment"].ToString());
                           //// object[] data1 = new object[rdr.FieldCount];
                           //// rdr.GetValues(data1);
                            List.Add(new MapGeographyData { GeographyID = Geographic_ID, GeographyName = Geographic_ID, CoverageStatusID = CoverageStatus_ID, Enrollment = Enrollment != null ? Enrollment : 0 });
                        }
                        
                    }
                }
            }
            return List;
        }
        //public IEnumerable<MapGeographyData> GetMapViewCoverageExecutiveReports(int Channel, int? DrugID, int? NamRamID, int ClientID, string Restrictions)
        //{
        //    IList<MapGeographyData> data = null;
        //    if (string.IsNullOrEmpty(Restrictions)) { Restrictions = ""; }

        //    if (DrugID != null)
        //    {

        //        switch (Restrictions)
        //        {
        //            case "":
        //                var g = from d in DrugCoverageByStateSet
        //                        where d.Drug_ID == DrugID.Value
        //                        && d.Section_ID == Channel
        //                        && d.Client_ID == ClientID
        //                        select new MapGeographyData { GeographyID = d.Geography_ID, GeographyName = d.Geography_ID, CoverageStatusID = d.Coverage_Status_ID, Enrollment = d.Enrollment != null ? d.Enrollment.Value : 0 };
        //                data = g.ToList();
        //                break;
        //            case "PA":
        //                var pa = from d in DrugCoverageByStateSet
        //                         where d.Drug_ID == DrugID.Value
        //                         && d.Section_ID == Channel
        //                         && d.Client_ID == ClientID
        //                         select new MapGeographyData { GeographyID = d.Geography_ID, GeographyName = d.Geography_ID, CoverageStatusID = d.PA_Coverage_Status_ID, Enrollment = d.Enrollment != null ? d.Enrollment.Value : 0 };
        //                data = pa.ToList();
        //                break;
        //            case "QL":
        //                var ql = from d in DrugCoverageByStateSet
        //                         where d.Drug_ID == DrugID.Value
        //                         && d.Section_ID == Channel
        //                         && d.Client_ID == ClientID
        //                         select new MapGeographyData { GeographyID = d.Geography_ID, GeographyName = d.Geography_ID, CoverageStatusID = d.QL_Coverage_Status_ID, Enrollment = d.Enrollment != null ? d.Enrollment.Value : 0 };
        //                data = ql.ToList();
        //                break;
        //            case "ST":
        //                var st = from d in DrugCoverageByStateSet
        //                         where d.Drug_ID == DrugID.Value
        //                         && d.Section_ID == Channel
        //                         && d.Client_ID == ClientID
        //                         select new MapGeographyData { GeographyID = d.Geography_ID, GeographyName = d.Geography_ID, CoverageStatusID = d.ST_Coverage_Status_ID, Enrollment = d.Enrollment != null ? d.Enrollment.Value : 0 };
        //                data = st.ToList();
        //                break;
        //            case "PA_QL":
        //                var paql = from d in DrugCoverageByStateSet
        //                           where d.Drug_ID == DrugID.Value
        //                           && d.Section_ID == Channel
        //                           && d.Client_ID == ClientID
        //                           select new MapGeographyData { GeographyID = d.Geography_ID, GeographyName = d.Geography_ID, CoverageStatusID = d.PA_QL_Coverage_Status_ID, Enrollment = d.Enrollment != null ? d.Enrollment.Value : 0 };
        //                data = paql.ToList();
        //                break;
        //            case "PA_ST":
        //                var past = from d in DrugCoverageByStateSet
        //                           where d.Drug_ID == DrugID.Value
        //                           && d.Section_ID == Channel
        //                           && d.Client_ID == ClientID
        //                           select new MapGeographyData { GeographyID = d.Geography_ID, GeographyName = d.Geography_ID, CoverageStatusID = d.PA_ST_Coverage_Status_ID, Enrollment = d.Enrollment != null ? d.Enrollment.Value : 0 };
        //                data = past.ToList();
        //                break;
        //            case "QL_ST":
        //                var qlst = from d in DrugCoverageByStateSet
        //                           where d.Drug_ID == DrugID.Value
        //                           && d.Section_ID == Channel
        //                           && d.Client_ID == ClientID
        //                           select new MapGeographyData { GeographyID = d.Geography_ID, GeographyName = d.Geography_ID, CoverageStatusID = d.QL_ST_Coverage_Status_ID, Enrollment = d.Enrollment != null ? d.Enrollment.Value : 0 };
        //                data = qlst.ToList();
        //                break;
        //            case "PA_QL_ST":
        //                var paqlst = from d in DrugCoverageByStateSet
        //                             where d.Drug_ID == DrugID.Value
        //                             && d.Section_ID == Channel
        //                             && d.Client_ID == ClientID
        //                             select new MapGeographyData { GeographyID = d.Geography_ID, GeographyName = d.Geography_ID, CoverageStatusID = d.PA_QL_ST_Coverage_Status_ID, Enrollment = d.Enrollment != null ? d.Enrollment.Value : 0 };
        //                data = paqlst.ToList();
        //                break;

        //        }


        //    }
        //    else  //Get just enrollment (lives) data by state if no drug id specified
        //    {
        //        using (PathfinderModel.PathfinderEntities c = new PathfinderModel.PathfinderEntities())
        //        {
        //            var q = from s in c.StateEnrollmentSet
        //                    where s.Section_ID == Channel
        //                    select new MapGeographyData { GeographyID = s.Geography_ID, GeographyName = s.Geography_ID, Category = "d", Enrollment = s.Enrollment != null ? s.Enrollment.Value : 0 };

        //            data = q.ToList();
        //        }
        //    }

        //    //return results of data and default list unioned - default list fills in gaps in data
        //    return data.Union(MapGeographyData.DefaultData, new MapGeographyDataComparer());
        //}
    }


}