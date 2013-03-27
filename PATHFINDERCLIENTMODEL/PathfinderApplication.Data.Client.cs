using System;
using System.Data.SqlClient;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using Pinsonault.Data;
using System.Linq.Expressions;

namespace PathfinderClientModel
{
    /// <summary>
    /// Customized methods and properties for PathfinderClientEntities context.
    /// </summary>
    public partial class PathfinderClientEntities
    {
        //public IQueryable<TerritoryGeography> GetUserTerritoryStates(int userID)
        //{
        //    return from ts in TerritoryGeographySet 
        //            join ut in UserTerritorySet on ts.Territory_ID equals ut.Territory_ID
        //            where ut.User_ID == userID
        //            select ts;
        //}

        //public Boolean CheckUserAlignment(int Plan_ID, int UserID)
        //{
        //    var a = (from tp in TerritoryPlanSet 
        //           join ut in UserTerritorySet on tp.Territory_ID equals ut.Territory_ID
        //           where ut.User_ID == UserID && tp.Plan_ID == Plan_ID 
        //           select tp).FirstOrDefault();

        //    if (a != null)
        //        return true;
        //    else
        //        return false;
        //}

        /// <summary>
        /// Gets geography information about a user's territory that is used to set their default view in the dashboard.
        /// </summary>
        /// <param name="userID">ID of the user.</param>
        /// <returns>UserGeography object that contains the size of the user's territory and center point or the RegionID such as a single state.</returns>
        //public UserGeography GetUserGeography(int userID)
        //{
        //    var q = GetUserTerritoryStates(userID);

        //    //If user has more than one state get area and center point
        //    if ( q.Count() > 1 )
        //    {
        //        var a = (from sg in StateGeographySet
        //                    join ts in q on sg.State_ID equals ts.Geography_ID
        //                    group sg by ts.Territory_ID into geog
        //                    select new
        //                    {
        //                        MaxY = geog.Max(g => g.Max_Latitude),
        //                        MinY = geog.Min(g => g.Min_Latitude),
        //                        MaxX = geog.Max(g => g.Max_Longitude),
        //                        MinX = geog.Min(g => g.Min_Longitude)
        //                    }).FirstOrDefault();

        //        return UserGeography.Create(a.MinX, a.MinY, a.MaxX, a.MaxY);
        //    }
        //    else 
        //    {
        //        var a = q.FirstOrDefault();
        //        //if user has 1 state region that in region ID - center point and area are not needed because map control will zoom to correct location
        //        if(a != null)                
        //            return new UserGeography { Area = 0, CenterX = 0, CenterY = 0, RegionID = a.Geography_ID };
        //        else  //else user is national return empty geog details
        //            return new UserGeography { Area = 0, CenterX = 0, CenterY = 0, RegionID = null };
        //    }
        //}

        //public IQueryable<MapViewCoverage> GetMapViewCoverage(int ChannelID, int? DrugID)
        //{
        //    if ( DrugID != null )
        //        return MapViewCoverageSet.Where(mv => mv.Section_ID == ChannelID && mv.Drug_ID == DrugID.Value);

        //    else
        //    {
        //        return from mv in MapViewCoverageSet
        //               join mvd in MapViewCoverageDefaultSet on mv.ID equals mvd.ID 
        //                where mv.Section_ID == ChannelID
        //                select mv;
        //    }
        //}

        public IEnumerable<int> GetDODHeadQuarters()
        {
            return PlanInfoListViewSet.Where(p => p.Section_ID == 12 && p.Plan_Type_ID == 20).Select(p => p.Plan_ID);
        }

        public bool DeleteDocument(int Document_ID)
        {
            //if (HttpContext.Current.User.IsInRole("editdocuments"))
            //{
            PathfinderClientModel.PlanDocuments planDocument = PlanDocumentsSet.FirstOrDefault(c => c.Document_ID == Document_ID);
            if (planDocument != null)
            {
                planDocument.Document_Status = false;
                SaveChanges();
                return true;
            }
            //}
            return false;

        }
 
    }

}