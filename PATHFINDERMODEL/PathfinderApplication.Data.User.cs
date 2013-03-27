using System.Data.Objects;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
//using Microsoft.ApplicationBlocks.Data;
using System.Collections.Generic;
using System.Text;
using Pinsonault.Data;

namespace Pinsonault.Data
{
    public class GenericStatusItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public int StatusLevel { get; set; }
        public string Client { get; set; }
        public string Application { get; set; }
    }

    public class GenericStatusItemComparer : IEqualityComparer<GenericStatusItem>
    {

        #region IEqualityComparer<GenericStatusItem> Members

        public bool Equals(GenericStatusItem x, GenericStatusItem y)
        {
            return x.ID == y.ID;
        }

        public int GetHashCode(GenericStatusItem obj)
        {
            return obj.ID.GetHashCode();
        }

        #endregion
    }

    public static class UserUpdate
    {
        /// <summary>
        /// This function updates user logout time       
        /// </summary>
        /// <param name="iUserID">UserID</param>

        public static void UpdateUserLogout(int iUserID)
        {
            using ( SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["Pathfinder"].ConnectionString) )
            {
                using ( SqlCommand cmd = new SqlCommand("usp_UpdateUserByUserID", cn) )
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter arparam = new SqlParameter("@User_ID", iUserID);
                    cmd.Parameters.Add(arparam);

                    cn.Open();
                    cmd.ExecuteNonQuery();
                    //SqlHelper.ExecuteNonQuery(cn, System.Data.CommandType.StoredProcedure, "usp_UpdateUserByUserID", arparam);
                }
            }
        }
    }
    public class DrugDetails
    {
        /// <summary>
        /// This function gets the drug details       
        /// </summary>
        /// <param name="iPlanID">Plan_ID</param>
        /// <param name="iDrugID">Drug_ID</param>

        public DataTable GetDrugDetails(int iPlanID, int iDrugID, int iFormularyID, int iSegmentID)
        {
            DataTable table = new DataTable();
            using ( SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["Pathfinder"].ConnectionString) )
            {
                using ( SqlCommand cmd = new SqlCommand("usp_GetPlanDrugFormulary", cn) )
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@Plan_ID", iPlanID));
                    cmd.Parameters.Add(new SqlParameter("@Drug_ID", iDrugID));
                    cmd.Parameters.Add(new SqlParameter("@Formulary_ID", iFormularyID));
                    cmd.Parameters.Add(new SqlParameter("@Segment_ID", iSegmentID));

                    cn.Open();

                    using ( SqlDataReader rdr = cmd.ExecuteReader() )
                    {
                        table.Load(rdr);
                    }
                }
                //return SqlHelper.ExecuteDataset(cn, "usp_GetPlanDrugFormulary", arparam1, arparam2, arparam3, arparam4).Tables[0];             
            }
            return table;
        }
    }
}

/// <summary>
/// Summary description for Pinsonault.Web
/// </summary>
namespace PathfinderModel
{
    /// <summary>
    /// Customized methods and properties for PathfinderEntities context that related to users.
    /// </summary>
    public partial class PathfinderEntities
    {
        /// <summary>
        /// Checks if a plan is aligned to a user.
        /// </summary>
        /// <param name="PlanID">ID of the plan that is checked.</param>
        /// <param name="UserID">ID of the user that is checked.</param>
        /// <returns></returns>
        public bool CheckUserAlignment(int PlanID, int UserID)
        {
            return (from tp in TerritoryPlanSet                    
                    where
                    tp.Territory.Users.Count(u=>u.User_ID == UserID) > 0
                    && tp.Plan_ID == PlanID                    
                    select tp).Count() > 0;                 
        }

        public IQueryable<Geography> GetUserTerritoryStates(int userID)
        {
            return from g in GeographySet
                   from t in g.Territories
                   from u in t.Users
                   where u.User_ID == userID
                   select g;
        }

        ///<summary>
        ///Gets geography information about a user's territory that is used to set their default view in the dashboard.
        ///</summary>
        ///<param name="userID">ID of the user.</param>
        ///<returns>UserGeography object that contains the size of the user's territory and center point or the RegionID such as a single state.</returns>
        public UserGeography GetUserGeography(int userID)
        {
            var q = GetUserTerritoryStates(userID);

            //If user has more than one state get area and center point
            if ( q.Count() > 1 )
            {
                var a = (from sg in StateGeographySet
                            join ts in q on sg.Geography equals ts
                            group sg by ts.Geography_Type into geog
                            select new
                            {
                                MaxY = geog.Max(g => g.Max_Latitude),
                                MinY = geog.Min(g => g.Min_Latitude),
                                MaxX = geog.Max(g => g.Max_Longitude),
                                MinX = geog.Min(g => g.Min_Longitude)
                            }).FirstOrDefault();

                return UserGeography.Create(a.MinX, a.MinY, a.MaxX, a.MaxY);
            }
            else 
            {
                var a = q.FirstOrDefault();
                //if user has 1 state region that in region ID - center point and area are not needed because map control will zoom to correct location
                if(a != null)                
                    return new UserGeography { Area = 0, CenterX = 0, CenterY = 0, RegionID = a.ID };
                else  //else user is national return empty geog details
                    return new UserGeography { Area = 0, CenterX = 0, CenterY = 0, RegionID = null };
            }
        }

        public IQueryable<DrugListEntry> GetUserDrugList(int clientID)
        {
    //        //TherapeuticClassDrugSet.MergeOption = MergeOption.NoTracking;
    //        //DrugSet.MergeOption = MergeOption.NoTracking;
    //        //ClientApplicationAccessSet.MergeOption = MergeOption.NoTracking;

    //        //var q = from d in DrugSet
    //        //        join td in TherapeuticClassDrugSet on d equals td.Drug
    //        //        join ct in ClientTherapeuticClassSet on td.Thera_ID equals ct.Thera_ID
    //        //        join tc in TherapeuticClassMasterSet on ct.Thera_ID equals tc.ID          
    //        //        join cdp in ClientDrugPreferenceSet on ct equals cdp.ClientMarketBasket into g
    //        //        where ct.Client_ID == clientID
    //        //                && td.Status
    //        //                && d.Status
    //        //                && tc.Status                            
    //        //        orderby ct.Sort_Index, tc.Name, d.Name
    //        //        select new DrugListEntry { ID = d.ID, Name = d.Name, TherapeuticClassID = td.Thera_ID, TherapeuticClassName = tc.Name, Selected = g.Where(i=>i.Drug_ID == d.ID).Select(i=>i.Selected).FirstOrDefault() };

            var q = from d in DrugListEntrySet
                    where
                    d.ClientID == clientID
                    orderby d.TherapeuticClassSortOrder, d.TherapeuticClassName, d.DrugSortOrder, d.Name
                    select d;

            return q;
        }

        /// <summary>
        /// Checks if a user has access to a module.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="appID"></param>
        /// <param name="moduleKey"></param>
        /// <returns></returns>
        public bool CheckUserModule(int userID, int appID, string moduleKey)
        {
            UserModuleSet.MergeOption = MergeOption.NoTracking;
            //UserRolesSet.MergeOption = MergeOption.NoTracking;

            IList<UserRoles> roles = UserRolesSet.Include("Roles").Where(ur => ur.User_ID == userID).ToList();

            return (from m in UserModuleSet
                    where m.App_ID == appID
                    && m.User_ID == userID
                    && m.Module_Key == moduleKey 
                    select m).ToList().Count(m => string.IsNullOrEmpty(m.Role_Key) || roles.Count(r => r.Roles.Role_Key == m.Role_Key) > 0) > 0;
        }

        /// <summary>
        /// Returns Modules that are available to the user based on their configuration settings. Only displayable modules are retuned.
        /// </summary>
        /// <param name="userID">ID of current user</param>
        /// <param name="appID">ID of application to filter by</param>
        /// <returns></returns>
        public IEnumerable<UserModule> GetUserModules(int userID, int appID)
        {
            UserModuleSet.MergeOption = MergeOption.NoTracking;
            //UserRolesSet.MergeOption = MergeOption.NoTracking;

            IList<UserRoles> roles = UserRolesSet.Include("Roles").Where(ur=>ur.User_ID == userID).ToList();

            return (from m in UserModuleSet
                    where m.App_ID == appID
                    && m.User_ID == userID
                    && m.Display_In_Menu
                    orderby m.Sort_Order, m.Module_Name, m.Section_ID
                    select m).ToList().Where(m => string.IsNullOrEmpty(m.Role_Key) || roles.Count(r => r.Roles.Role_Key == m.Role_Key)>0);
        }

        public string GetUserModulesAsJSON(int userID, int appID)
        {
            return GetUserModulesAsJSON(userID, appID, true);
        }

        public string GetUserModulesAsJSON(int userID, int appID, bool includeAppID)
        {
            StringBuilder sb = new StringBuilder();

            if (includeAppID)
                sb.Append("{").Append(appID).Append(":");

            sb.Append("[");

            var q = GetUserModules(userID, appID);

            string channelComma = "";
            string moduleComma = "";
            string lastModule = null;
            bool opened = false;

            foreach (PathfinderModel.UserModule module in q)
            {
                if (string.Compare(lastModule, module.Module_Key, true) != 0)
                {
                    channelComma = "";
                    //close previous
                    if (opened)
                        sb.Append("}}");

                    sb.Append(moduleComma);

                    sb.Append("{");
                    sb.AppendFormat("\"ID\":\"{0}\",\"Name\":\"{1}\",\"Custom\":{2},\"Channels\":", module.Module_Key, module.Module_Name, module.Is_Custom.ToString().ToLower());
                    sb.Append("{");

                    opened = true;
                    moduleComma = ",";
                }

                sb.Append(channelComma);
                sb.AppendFormat("{0}:1", module.Section_ID);
                channelComma = ",";

                lastModule = module.Module_Key;
            }

            if (opened)
                sb.Append("}}");

            sb.Append("]");

            if (includeAppID)
                sb.Append("}");

            return sb.ToString();
        }

        /// <summary>
        /// Intended for Application configuration management. Returns the list of all available roles and includes information on which roles a client has enabled.  
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        public IEnumerable<GenericStatusItem> GetClientRoles(int clientID)
        {
            var q = (from cr in ClientRoleSet
                     where cr.Client_ID == clientID
                     select new GenericStatusItem { ID = cr.Role_ID, Name = cr.UserRole.Role_Name, Status = cr.Status, StatusLevel = cr.Status ? 1 : 0 }).ToList()
                    .Union(
                    from r in RolesSet
                    select new GenericStatusItem { ID = r.Role_ID, Name = r.Role_Name, Status = false, StatusLevel = -1 }, new GenericStatusItemComparer()
                    ).OrderBy(i => i.Name);

            return q;
        }

        public IEnumerable<GenericStatusItem> GetClientModules(int clientID)
        {
            return GetClientModules(clientID, null);
        }
        /// <summary>
        /// Intended for Application configuration management. Returns the list of all available modules and includes information on which modules a client has enabled.
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        public IEnumerable<GenericStatusItem> GetClientModules(int clientID, int? appID)
        {            
            var q = (from cm in ClientModuleSet
                     where cm.Client_ID == clientID 
                                && (appID == null || cm.Modules.Application.ID == appID)
                     select new GenericStatusItem { ID = cm.Module_ID, Name = cm.Modules.Name, Application = cm.Modules.Application.Name, Status = cm.Status > 0, StatusLevel = cm.Status }).ToList()
                    .Union(
                    from m in ModuleSet
                    where m.Application.ID > 0
                        && (appID == null || m.Application.ID == appID)
                    select new GenericStatusItem { ID = m.ID, Name = m.Name, Application = m.Application.Name, Status = false, StatusLevel = -1 }, new GenericStatusItemComparer()
                    ).OrderBy(i => i.Application).ThenBy(i=>i.Name);

            return q;
        }

        public IEnumerable<GenericStatusItem> GetClientChannels(int clientID, int appID)
        {
            //bool channelsEnabled = ApplicationSet.Where(a => a.ID == appID).Select(a => a.Channels_Enabled).FirstOrDefault();

            var q = (from caa in ClientApplicationAccessSet
                     where caa.ClientID == clientID && caa.ApplicationID == appID
                     select new GenericStatusItem { ID = caa.SectionID, Status = true, StatusLevel = 1, Name = caa.Section.Name }).ToList()
                   .Union(
                        from s in SectionSet
                        where s.Client.Client_ID == clientID || s.Client == null
                               // && (channelsEnabled || s.ID == 0) //only get 0 if channels are not enabled for application
                        select new GenericStatusItem { ID = s.ID, Name = s.Name, StatusLevel = 0, Status = false }, new GenericStatusItemComparer())
                    .OrderBy(i => i.Name);

            return q;
        }

        /// <summary>
        /// Intended for Application configuration management.  Returns a list of all therapeutic classes and includes information on which classes have been enabled for the specified client.
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        public IEnumerable<GenericStatusItem> GetClientMarketBaskets(int clientID)
        {
            var q = (from ctc in ClientTherapeuticClassSet
                     join t in TherapeuticClassMasterSet on ctc.TherapeuticClassMaster equals t
                     where ctc.Client_ID == clientID
                                 && t.Status
                     select new GenericStatusItem { ID = t.ID, Name = t.Name, Status = true, StatusLevel = 1 }).ToList()
                        .Union(
                        from t in TherapeuticClassMasterSet
                        where t.Status 
                       // && t.TherapeuticClassDrug.Count(tc=>tc.Status && tc.Drug.Status) > 0
                        select new GenericStatusItem { ID = t.ID, Name = t.Name, Status = false, StatusLevel = 0 }, new GenericStatusItemComparer())
                          .OrderBy(i => i.Name);

            return q;
        }

        public IQueryable<DrugListEntry> GetClientMarketBasketDrugs(int clientID, int marketBasketID)
        {
            return from d in DrugListEntrySet
                    where
                    d.ClientID == clientID
                    && d.TherapeuticClassID == marketBasketID 
                    orderby d.TherapeuticClassSortOrder, d.TherapeuticClassName, d.DrugSortOrder, d.Name
                    select d;            
        }

        /// <summary>
        /// Returns application modules assigned to a user through either their client, team, or user id. Results will be sorted by Sort_Order and Module_Name.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="appID"></param>
        /// <returns></returns>
        public IQueryable<Module> GetApplicationModules(int userID, int appID)
        {                               
            return ModuleSet //.Include("SectionReportFilters")
                                    //.Include("Sections")
                                    .Where(r => r.Application.ID == appID)                                   
                                    .OrderBy(r => r.Sort_Order)
                                    .ThenBy(r => r.Name);
        }

        public string GetUserReportOptionsAsJSON(int userID, int appID)
        {
            StringBuilder sb = new StringBuilder("[");

            var q = GetApplicationModules(userID, appID);
            int count;

            var userModules = GetUserModules(userID, appID);
                 
            foreach ( PathfinderModel.Module report in q )
            {
                //IEnumerable<int> channels = report.SectionReportFilters.Select(s => s.Section_ID).Distinct().Union(report.Sections.Select(s=>s.ID).Distinct());
                IEnumerable<int> channels = userModules.Where(um=>um.Module_ID == report.ID).Select(um => um.Section_ID).Distinct();
                
                if ( sb.Length > 1 )
                    sb.Append(",");
                sb.Append("{");
                sb.AppendFormat("\"ID\":\"{0}\",\"Name\":\"{1}\",\"Channels\":", report.Module_Key, report.Name);
                sb.Append("{");
                
                count = channels.Count();
                if ( count > 0 )
                {
                    sb.Append(string.Join(",", channels.Select(s => string.Format("{0}:1", s)).ToArray()));
                }
                else
                    sb.Append("0:0");
                sb.Append("}}");
            }
            sb.Append("]");
            return sb.ToString();
        }

        /// <summary>
        /// Returns report rank types assigned to a client
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns></returns>
        public IList<RankTypes> GetReportRankTypes(int? clientID)
        {
            if (clientID == null)
            {
                return (from rt in RankTypesSet
                        where rt.Client_ID == null
                        select rt).ToList();
            }
            else
            {
                return  (from rt in RankTypesSet
                        where rt.Client_ID == clientID
                        select rt).ToList();
            }
        }
        /// <summary>
        /// for getting DOD head quarters list
        /// </summary>
        /// <returns></returns>
        public IEnumerable<int> GetDODHeadQuarters()
        {           
             return PlanInfoListViewSet.Where(p => p.Section_ID == 12 && p.Plan_Type_ID == 20).Select(p => p.Plan_ID);
        }

    }
}

