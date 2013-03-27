using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PathfinderModel
{
    public class MapGeographyData
    {        
        public string GeographyID { get; set; }
        public string GeographyName { get; set; }
        public string Category { get; set; }
        int _coverageStatusID; 
        public int CoverageStatusID
        {
            get { return _coverageStatusID; }
            set
            {
                _coverageStatusID = value;
                Category = value.ToString();
            }
        }
        public int Enrollment { get; set; }
        public int? DrugID { get; set; }
        public int ChannelID { get; set; }

        public static List<MapGeographyData> DefaultData { get; private set;  }

        static MapGeographyData()
        {
            using (PathfinderEntities context = new PathfinderEntities())
            {
                DefaultData = context.StateSet.Select(s => new MapGeographyData { ChannelID = 0, Enrollment = 0, GeographyID = s.ID, Category="0", GeographyName = s.Name }).ToList();
            }
        }
    }

    public class MapGeographyDataComparer : IEqualityComparer<MapGeographyData>
    {
        #region IEqualityComparer<MapGeographyData> Members

        public bool Equals(MapGeographyData x, MapGeographyData y)
        {
            return string.Compare(x.GeographyID, y.GeographyID, true) == 0;
        }

        public int GetHashCode(MapGeographyData obj)
        {
            if(obj.GeographyID != null)
                return obj.GeographyID.ToUpper().GetHashCode();

            return 0;
        }

        #endregion
    }
}
