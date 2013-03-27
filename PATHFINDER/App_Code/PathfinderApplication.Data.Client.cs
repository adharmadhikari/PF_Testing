using System;
using System.Data.SqlClient;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using Pinsonault.Web;

namespace Pinsonault.Web.Data
{

    public static class Client
    {
        public static List<String> GetSellSheetDrugs(Int32 Sell_Sheet_ID, Int32 UserID)
        {
            List<String> ssDrugs = new List<string>{};
            using (SqlConnection cn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_SellSheet_PlanSelectionDrugs", cn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    //set params
                    cmd.Parameters.AddWithValue("@Sell_Sheet_ID", Sell_Sheet_ID);
                    cmd.Parameters.AddWithValue("@User_ID", UserID);
                    cn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            //If client drug is null/empty then populate default header. 
                            if (String.IsNullOrEmpty(rdr.GetString(0)))
                                ssDrugs.Add("Your Product");
                            else
                                ssDrugs.Add(rdr.GetString(0));

                            //If competitor drug1 is null/empty then populate default header.
                            if(String.IsNullOrEmpty(rdr.GetString(1)))
                                ssDrugs.Add("Competitor1");
                            else
                                ssDrugs.Add(rdr.GetString(1));

                            //If competitor drug2 is null/empty then populate default header.
                            if(String.IsNullOrEmpty(rdr.GetString(2)))
                                ssDrugs.Add("Competitor2");
                            else
                                ssDrugs.Add(rdr.GetString(2));

                            if (String.IsNullOrEmpty(rdr.GetString(3)))
                                ssDrugs.Add("FALSE");
                            else
                                ssDrugs.Add(rdr.GetString(3));
                        }
                    }
                }
            }
            return ssDrugs; 
        }

        //public static int GetSellSheetCount(string geographyID, int? drugID, int channelID, int pageIndex, int pageSize)
        //{
        //    using ( SqlConnection cn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString) )
        //    {
        //        using (SqlCommand cmd = new SqlCommand("usp_SellSheets", cn))
        //        {
        //            cmd.CommandType = System.Data.CommandType.StoredProcedure;

        //            //set params
        //            cmd.Parameters.AddWithValue("@Geography_ID", geographyID);
        //            cmd.Parameters.AddWithValue("@Drug_ID", drugID);
        //            cmd.Parameters.AddWithValue("@Section_ID", channelID);
        //            cmd.Parameters.AddWithValue("@Page_Index", pageIndex);
        //            cmd.Parameters.AddWithValue("@Page_Size", pageSize);
        //            cmd.Parameters.AddWithValue("@returnCount", true);
        //            cn.Open();
        //            try
        //            {
        //                object o = cmd.ExecuteScalar();
        //                if (o != null)
        //                    return (int)o;
        //            }
        //            catch (Exception ex)
        //            {
        //                throw ex;
        //            }
        //        }
        //    }

        //    return 0; 
        //}
    }
}

//namespace PathfinderClientModel
//{
//    /// <summary>
//    /// Customized methods and properties for PathfinderClientEntities context.
//    /// </summary>
//    public partial class PathfinderClientEntities
//    {
//        partial void OnContextCreated()
//        {
//            this.SavingChanges += new EventHandler(OnSavingChanges);        
//        }

//        void OnSavingChanges(object sender, EventArgs e)
//        {
//            if ( string.Compare(Connection.ConnectionString, "name=PathfinderClientEntities", true) == 0 )
//                throw new ApplicationException("ConnectionString has not been properly set for ObjectContext.  The value must be explicitly set in code to reference the correct client database.  DO NOT use the default connection string provided in web.config.");
//        }

//        public IQueryable<TerritoryGeography> GetUserTerritoryStates(int userID)
//        {
//            return from ts in TerritoryGeographySet 
//                    join ut in UserTerritorySet on ts.Territory_ID equals ut.Territory_ID
//                    where ut.User_ID == userID
//                    select ts;
//        }


//        /// <summary>
//        /// Gets geography information about a user's territory that is used to set their default view in the dashboard.
//        /// </summary>
//        /// <param name="userID">ID of the user.</param>
//        /// <returns>UserGeography object that contains the size of the user's territory and center point or the RegionID such as a single state.</returns>
//        public UserGeography GetUserGeography(int userID)
//        {
//            var q = GetUserTerritoryStates(userID);

//            //If user has more than one state get area and center point
//            if ( q.Count() > 1 )
//            {
//                var a = (from sg in StateGeographySet
//                            join ts in q on sg.State_ID equals ts.Geography_ID
//                            group sg by ts.Territory_ID into geog
//                            select new
//                            {
//                                MaxY = geog.Max(g => g.Max_Latitude),
//                                MinY = geog.Min(g => g.Min_Latitude),
//                                MaxX = geog.Max(g => g.Max_Longitude),
//                                MinX = geog.Min(g => g.Min_Longitude)
//                            }).FirstOrDefault();

//                return UserGeography.Create(a.MinX, a.MinY, a.MaxX, a.MaxY);
//            }
//            else 
//            {
//                var a = q.FirstOrDefault();
//                //if user has 1 state region that in region ID - center point and area are not needed because map control will zoom to correct location
//                if(a != null)                
//                    return new UserGeography { Area = 0, CenterX = 0, CenterY = 0, RegionID = a.Geography_ID };
//                else  //else user is national return empty geog details
//                    return new UserGeography { Area = 0, CenterX = 0, CenterY = 0, RegionID = null };
//            }
//        }

//        public IQueryable<MapViewCoverage> GetMapViewCoverage(int ChannelID, int? DrugID)
//        {
//            if ( DrugID != null )
//                return MapViewCoverageSet.Where(mv => mv.Section_ID == ChannelID && mv.Drug_ID == DrugID.Value);

//            else
//            {
//                return from mv in MapViewCoverageSet
//                       join mvd in MapViewCoverageDefaultSet on mv.ID equals mvd.ID 
//                        where mv.Section_ID == ChannelID
//                        select mv;
//            }
//        }

//    }

//}