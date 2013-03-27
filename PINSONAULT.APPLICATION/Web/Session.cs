using System;
using System.Web;
using System.Web.Security;
using System.Linq;
using System.Linq.Expressions;
using Pinsonault.Web.Security;
using System.Configuration;
using System.Collections.Generic;
using System.Web.SessionState;

namespace Pinsonault.Web
{
    public class Session
    {
        private Session() { }

        /// <summary>
        /// Gets the database Client ID of the current user.
        /// </summary>
        public static int ClientID
        {
            get { return GetSessionValue<int>("ClientID"); }
        }

        /// <summary>
        /// Gets the database User ID of the current user.
        /// </summary>
        public static int UserID
        {
            get { return GetSessionValue<int>("UserID"); }
        }

        /// <summary>
        /// Gets the First Name of the current user.
        /// </summary>
        public static string FirstName
        {
            get { return GetSessionValue<string>("FirstName"); }
        }

        public static string FullName
        {
            get { return string.Format("{0} {1}", FirstName, GetSessionValue<string>("LastName")); }
        }

        public static bool Admin { get { return string.Compare(ClientKey, "admin", true)==0; } }

        /// <summary>
        /// Returns the connection string for the current client to be used with an entity data model.
        /// </summary>
        public static string ClientConnectionString
        {
            get
            {
                //if ( string.IsNullOrEmpty(Session.ClientKey) )
                //    throw new HttpException(500, "ClientConnectionString property cannot be called until session has been initialized through call to CheckSessionState.");

                //ConnectionStringSettings connectionString = ConfigurationManager.ConnectionStrings[string.Format("PathfinderClientEntities_{0}", Session.ClientKey)];
                //if ( connectionString == null )
                //    return string.Format(ConfigurationManager.ConnectionStrings["PathfinderClientEntities_Format"].ConnectionString, Session.ClientKey);

                //return connectionString.ConnectionString;
                return GetClientApplicationConnectionString(string.Empty);
            }
        }

        public static string GetClientApplicationConnectionString(string Application)
        {
            if ( string.IsNullOrEmpty(Session.ClientKey) )
                throw new HttpException(500, "Cannot return client connection string until session has been initialized through call to CheckSessionState.");

            string application = Application;
            if ( string.IsNullOrEmpty(application) )
                application = "PathfinderClient";

            string key = string.Format("{0}Entities_{1}", application, Session.ClientKey);
            ConnectionStringSettings connectionString = ConfigurationManager.ConnectionStrings[key];
            if ( connectionString == null )
            {
                key = string.Format("{0}Entities_Format", application);
                return string.Format(ConfigurationManager.ConnectionStrings[key].ConnectionString, Session.ClientKey);
            }

            return connectionString.ConnectionString;            
        }

        /// <summary>
        /// Returns the database connection string for the current client.
        /// </summary>
        public static string ClientDBConnectionString
        {
            get
            {
                if ( string.IsNullOrEmpty(Session.ClientKey) )
                    throw new HttpException(500, "ClientDBConnectionString property cannot be called until session has been initialized through call to CheckSessionState.");

                ConnectionStringSettings connectionString = ConfigurationManager.ConnectionStrings[string.Format("PathfinderClientDB_{0}", Session.ClientKey)];
                if ( connectionString == null )
                    return string.Format(ConfigurationManager.ConnectionStrings["PathfinderClientDB_Format"].ConnectionString, Session.ClientKey);

                return connectionString.ConnectionString;
            }
        }

        public static string ClientServiceUrl
        {
            get
            {
                if ( string.IsNullOrEmpty(Session.ClientKey) )
                    throw new HttpException(500, "ClientServiceUrl property cannot be called until session has been initialized through call to CheckSessionState.");

                return string.Format("~/custom/{0}/services/pathfinderdataservice.svc", Session.ClientKey);
            }

        }

        /// <summary>
        /// Gets the Client Key of the current user.  The Client Key is a unique name identifier used to locate client specific folders or database.
        /// </summary>
        public static string ClientKey
        {
            get { return GetSessionValue<string>("ClientKey"); }
        }

        /// <summary>
        /// Gets a unique value to append to query strings to make a request unique for a specific user.  This value should be used instead of a user name or ID which could be mistakenly used to 
        /// query the database for user records.
        /// </summary>
        public static string UserKey
        {
            get { return GetSessionValue<string>("UserKey"); }
        }

        /// <summary>
        /// Gets the Client name for the current user.  This is the display name for the client.
        /// </summary>
        public static string ClientName
        {
            get { return GetSessionValue<string>("ClientName"); }
        }

        /// <summary>
        /// Gets the value that indicates if the current user's client has a custom theme defined.  If this value returns True there should be a client specific folder in the App_Themes folder.
        /// </summary>
        public static bool CustomTheme
        {
            get { return GetSessionValue<bool>("CustomTheme"); }
        }

        /// <summary>
        /// Returns a list of region ids that the current user is aligned to.
        /// </summary>
        public static string[] GeographicRegions
        {
            get
            {
                Dictionary<string, string> alignments = GetSessionValue<Dictionary<string, string>>("Alignment");
                return alignments.Values.ToArray();
            }
        }

        public static string TerritoryID
        {
            get { return Session.GetSessionValue<string>("TerritoryID"); }
        }

        /// <summary>
        /// for getting the bool value indecating if the client has custom plans
        /// </summary>
        public static bool clientHasCustomPlans
        {
            get { return GetSessionValue<bool>("clientHasCustomPlans"); }
        }


        public static void SwitchTerritory(string TerritoryID)
        {
            Dictionary<string, string> territories = GetSessionValue<Dictionary<string, string>>("Territories");

            if (territories != null)
            {
                if (territories.Keys.Contains(TerritoryID))
                {
                    HttpContext.Current.Session["TerritoryID"] = TerritoryID;
                }
                else
                    throw new ApplicationException(string.Format("Requested territory {0} is not valid for the logged in user.", TerritoryID));
            }
            else
                throw new ApplicationException("Logged in user does not have any territories assigned.");
        }

        /// <summary>
        /// Checks if the specified RegionID is in the current user's set of aligned regions.
        /// </summary>
        /// <param name="RegionID">ID of region such as a state abbreviation</param>
        /// <returns></returns>
        public static bool IsInAlignment(string RegionID)
        {
            Dictionary<string, string> alignments = GetSessionValue<Dictionary<string, string>>("Alignment");

            return alignments == null || alignments.Count == 0 || alignments.ContainsKey(RegionID);
        }


        public static bool SupportFlash
        {
            get
            {
                string userAgent = HttpContext.Current.Request.UserAgent;
                if ( !string.IsNullOrEmpty(userAgent) )
                {
                    if ( userAgent.IndexOf("iPad;", StringComparison.InvariantCultureIgnoreCase) > -1 )
                    {
                        return false;
                    }                    
                }

                return true;
            }
        }

        //Returns comma separated list of additional options/values needed to handle UI on client(browser)
        public static string ClientOptions
        {
            get
            {
                ////IPrincipal user = HttpContext.Current.User;

                ////List<string> options = new List<string>();

                ////if ( user.IsInRole("ccr") )
                ////    options.Add("ccr");

                ////bool all = user.IsInRole("ta_all");
                ////if ( all || user.IsInRole("ta_pi") )
                ////    options.Add("pi");

                ////if ( all || user.IsInRole("ta_kc") )
                ////    options.Add("kc");

                ////if ( all || user.IsInRole("ta_bd") )
                ////    options.Add("bd");

                ////if ( all || user.IsInRole("ta_af") )
                ////    options.Add("af");

                ////return string.Join(",", options.ToArray());
                return "";
            }
        }

        /// <summary>
        /// Gets the current user's application access settings.  This includes Applications and available Sections.
        /// </summary>
        public static Dictionary<string, PathfinderModel.ClientApplicationAccess> ClientApplicationAccess
        {
            get { return Pinsonault.Web.Session.GetSessionValue<Dictionary<string, PathfinderModel.ClientApplicationAccess>>("ClientApplicationAccess"); }
        }

        /// <summary>
        /// Returns a value from the current Session
        /// </summary>
        /// <typeparam name="T">Return type of the Session value.</typeparam>
        /// <param name="Name">Name of the value to return.</param>
        /// <returns>Returns the value stored in session cast to the specified return type.  If the value is not found the default for the type is returned.</returns>
        public static T GetSessionValue<T>(string Name)
        {
            object value = System.Web.HttpContext.Current.Session[Name];
            if ( value != null )
                return (T)value;
            else
                return default(T);
        }

        /// <summary>
        /// Helper function that is used to make sure the current Session store is up to date with the current user's information.  If the authenticated user is valid but their session is empty it is restored from the database.
        /// </summary>
        public static void CheckSessionState()
        {
            if ( HttpContext.Current.User.Identity.IsAuthenticated && HttpContext.Current.Session["UserID"] == null )
            {
                PathfinderMembershipUser user = Membership.GetUser(HttpContext.Current.User.Identity.Name) as PathfinderMembershipUser;
                if ( user != null )
                {
                    HttpSessionState session = HttpContext.Current.Session;
                    session["UserID"] = user.UserID;
                    session["ClientID"] = user.ClientID;
                    session["ClientKey"] = user.ClientKey;
                    session["ClientName"] = user.ClientName;
                    session["CustomTheme"] = user.CustomTheme;
                    session["FirstName"] = user.FirstName;
                    session["LastName"] = user.LastName;
                    session["ClientApplicationAccess"] = user.ClientApplicationAccess;
                    session["UserKey"] = Pinsonault.Security.Security.HashValue(user.UserName + user.UserID);
                    session["Alignment"] = user.Alignments;
                    if (user.Territories.Count > 0)
                    {
                        session["TerritoryID"] = user.Territories.FirstOrDefault().Key;
                        session["Territories"] = user.Territories;
                    }
                    session["clientHasCustomPlans"] = user.ClientHasCustomPlans;
                    //session["Territories"] = user.Territories;

                    ////Getting aligments now since they are in client db and not part of user query that loads PathfinderMembershipUser
                    //try
                    //{
                    //    if ( !Pinsonault.Web.Session.Admin )
                    //    {
                    //        using ( PathfinderClientModel.PathfinderClientEntities clientContext = new PathfinderClientModel.PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString) )
                    //        {
                    //            HttpContext.Current.Session["Alignment"] = clientContext.GetUserTerritoryStates(user.UserID).ToDictionary(a => a.Geography_ID, a => a.Geography_ID);
                    //            HttpContext.Current.Session["TerritoryID"] = clientContext.UserTerritorySet.Where(t => t.User_ID == user.UserID).Select(t => t.Territory_ID).FirstOrDefault();
                    //        }
                    //    }
                    //}
                    //catch ( Exception ex )
                    //{
                    //    Support.EmailException(HttpContext.Current.User.Identity.Name, HttpContext.Current, ex);
                    //}
                }
            }
        }
    }
}