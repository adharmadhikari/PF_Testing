using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PathfinderModel;

namespace Pinsonault.Application.Millennium
{
    /// <summary>
    /// Customized methods and properties for PathfinderEntities context that relate to reports.
    /// </summary>
    public partial class PathfinderMillenniumEntities
    {
        public IEnumerable<MapGeographyData> GetMapViewCoverageExecutiveReports(int Channel, int? DrugID, int? NamRamID, int ClientID, string Restrictions)
        {
            IList<MapGeographyData> data = null;
            if (string.IsNullOrEmpty(Restrictions)) { Restrictions = ""; }

            if (DrugID != null)
            {
                switch (Restrictions)
                {
                    case "":
                        var g = from d in DrugCoverageByStateSet
                                where d.Drug_ID == DrugID.Value
                                && d.Section_ID == Channel
                                && d.Client_ID == ClientID
                                && d.User_ID == NamRamID
                                select new MapGeographyData { GeographyID = d.Geography_ID, GeographyName = d.Geography_ID, CoverageStatusID = d.Coverage_Status_ID, Enrollment = d.Enrollment != null ? d.Enrollment.Value : 0 };
                        data = g.ToList();
                        break;
                    case "PA":
                        var pa = from d in DrugCoverageByStateSet
                                 where d.Drug_ID == DrugID.Value
                                 && d.Section_ID == Channel
                                 && d.Client_ID == ClientID
                                 && d.User_ID == NamRamID
                                 select new MapGeographyData { GeographyID = d.Geography_ID, GeographyName = d.Geography_ID, CoverageStatusID = d.PA_Coverage_Status_ID, Enrollment = d.Enrollment != null ? d.Enrollment.Value : 0 };
                        data = pa.ToList();
                        break;
                    case "QL":
                        var ql = from d in DrugCoverageByStateSet
                                 where d.Drug_ID == DrugID.Value
                                 && d.Section_ID == Channel
                                 && d.Client_ID == ClientID
                                 && d.User_ID == NamRamID
                                 select new MapGeographyData { GeographyID = d.Geography_ID, GeographyName = d.Geography_ID, CoverageStatusID = d.QL_Coverage_Status_ID, Enrollment = d.Enrollment != null ? d.Enrollment.Value : 0 };
                        data = ql.ToList();
                        break;
                    case "ST":
                        var st = from d in DrugCoverageByStateSet
                                 where d.Drug_ID == DrugID.Value
                                 && d.Section_ID == Channel
                                 && d.Client_ID == ClientID
                                 && d.User_ID == NamRamID
                                 select new MapGeographyData { GeographyID = d.Geography_ID, GeographyName = d.Geography_ID, CoverageStatusID = d.ST_Coverage_Status_ID, Enrollment = d.Enrollment != null ? d.Enrollment.Value : 0 };
                        data = st.ToList();
                        break;
                    case "PA_QL":
                        var paql = from d in DrugCoverageByStateSet
                                   where d.Drug_ID == DrugID.Value
                                   && d.Section_ID == Channel
                                   && d.Client_ID == ClientID
                                   && d.User_ID == NamRamID
                                   select new MapGeographyData { GeographyID = d.Geography_ID, GeographyName = d.Geography_ID, CoverageStatusID = d.PA_QL_Coverage_Status_ID, Enrollment = d.Enrollment != null ? d.Enrollment.Value : 0 };
                        data = paql.ToList();
                        break;
                    case "PA_ST":
                        var past = from d in DrugCoverageByStateSet
                                   where d.Drug_ID == DrugID.Value
                                   && d.Section_ID == Channel
                                   && d.Client_ID == ClientID
                                   && d.User_ID == NamRamID
                                   select new MapGeographyData { GeographyID = d.Geography_ID, GeographyName = d.Geography_ID, CoverageStatusID = d.PA_ST_Coverage_Status_ID, Enrollment = d.Enrollment != null ? d.Enrollment.Value : 0 };
                        data = past.ToList();
                        break;
                    case "QL_ST":
                        var qlst = from d in DrugCoverageByStateSet
                                   where d.Drug_ID == DrugID.Value
                                   && d.Section_ID == Channel
                                   && d.Client_ID == ClientID
                                   && d.User_ID == NamRamID
                                   select new MapGeographyData { GeographyID = d.Geography_ID, GeographyName = d.Geography_ID, CoverageStatusID = d.QL_ST_Coverage_Status_ID, Enrollment = d.Enrollment != null ? d.Enrollment.Value : 0 };
                        data = qlst.ToList();
                        break;
                    case "PA_QL_ST":
                        var paqlst = from d in DrugCoverageByStateSet
                                     where d.Drug_ID == DrugID.Value
                                     && d.Section_ID == Channel
                                     && d.Client_ID == ClientID
                                     && d.User_ID == NamRamID
                                     select new MapGeographyData { GeographyID = d.Geography_ID, GeographyName = d.Geography_ID, CoverageStatusID = d.PA_QL_ST_Coverage_Status_ID, Enrollment = d.Enrollment != null ? d.Enrollment.Value : 0 };
                        data = paqlst.ToList();
                        break;

                }


            }
            else  //Get just enrollment (lives) data by state if no drug id specified
            {
                using (PathfinderModel.PathfinderEntities c = new PathfinderModel.PathfinderEntities())
                {
                    var q = from s in c.StateEnrollmentSet
                            where s.Section_ID == Channel
                            select new MapGeographyData { GeographyID = s.Geography_ID, GeographyName = s.Geography_ID, Category = "d", Enrollment = s.Enrollment != null ? s.Enrollment.Value : 0 };

                    data = q.ToList();
                }
            }

            //return results of data and default list unioned - default list fills in gaps in data
            return data.Union(MapGeographyData.DefaultData, new MapGeographyDataComparer());
        }
    }
}
