using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Runtime.Serialization.Json;
using System.IO;
using System;
using PathfinderModel;
using System.Web.Caching;
using System.Runtime.Serialization;
using System.Data.Services;
using System.Reflection;
using System.Collections.Specialized;

namespace Pinsonault.Web
{
    public class Identifiers
    {
        public const int TodaysAccounts = 1;
        public const int MarketplaceAnalytics = 2;
        public const int StandardReports = 3;
        public const int FormularySellSheets = 8;
        public const int CustomerContactReports = 9;
        public const int BusinessPlanning = 11;
        public const int FormularyHistoryReporting = 14;
        public const int PrescriberReporting = 15;
        public const int ActivityReporting = 16;
        public const int SellSheetReporting = 17;
        public const int ExecutiveReports = 18;
        public const int Profile = 19;
        public const int CustomSegments = 20;
        public const int RestrictionsReport = 21;
    }


    [DataContract]
    public class GenericListItem
    {
        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public string Name { get; set; }
    }

    /// <summary>
    /// Pinsonault.Web.Support contains utility functions and configuration properties for the application.
    /// </summary>
    public class Support
    {
        private Support() { }

        const string DEFAULT_SKIN = "pathfinder";

        const int DEFAULT_MAX_ALLOWED_RECORDS = 50;
        const int DEFAULT_PAGE_CACHE_DURATION = 15; //MINUTES

        static int _maxAllowedRecords = 0;
        static int _pageCacheDuration = 0;

        static string FromEmail = "";
        static string EmailSubject = "";

        static Support()
        {
            try
            {
                //cache common script variables
                using (PathfinderEntities context = new PathfinderEntities())
                {
                    string script = getStateScript(context);
                    if (!string.IsNullOrEmpty(script))
                        HttpContext.Current.Cache.Insert("states", script, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromHours(12.0), new System.Web.Caching.CacheItemUpdateCallback(statesCacheItemUpdateCallback));

                }

                //REMOVE SOMEDAY
                Type type = Type.GetType("System.Data.Services.DataServiceBehavior, System.Data.Services, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", false, true);
                if (type != null)
                {
                    PropertyInfo propInfo = type.GetProperty("MaxProtocolVersion", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                    if (propInfo != null)
                    {
                        HasDataServicePatch = true;
                    }
                }
                //REMOVE SOMEDAY
            }
            catch
            {
                //don't want an exception in static constructor
            }

            //try
            //{
            //    string list = ConfigurationManager.AppSettings["drugListTables"];
            //    if (!string.IsNullOrEmpty(list))
            //    {
            //        SqlCacheDependencyAdmin.EnableNotifications(ConfigurationManager.ConnectionStrings["PathfinderOwner"].ConnectionString);
            //        SqlCacheDependencyAdmin.EnableTableForNotifications(ConfigurationManager.ConnectionStrings["PathfinderOwner"].ConnectionString, list.Replace(" ", "").Split(','));
            //    }
            //}
            //catch (Exception ex)
            //{
            //}
        }

        /// <summary>
        /// Gets the theme to use for the current user.  If the current user does not require a custom theme the default of "pathfinder" is returned.
        /// </summary>
        public static string Theme
        {
            get
            {
                if (Pinsonault.Web.Session.CustomTheme)
                    return Pinsonault.Web.Session.ClientKey;
                else
                    return DEFAULT_SKIN;
            }
        }

        /// <summary>
        /// Main folder for storing uploaded forms and documents.
        /// </summary>
        public static string RootFolder
        {
            get { return ConfigurationManager.AppSettings["rootFolder"]; }
        }

        /// <summary>
        /// Forms folder for storing uploaded forms.
        /// </summary>
        public static string FormsFolder
        {
            get { return ConfigurationManager.AppSettings["formsFolder"]; }
        }

        /// <summary>
        /// Folder for storing temporary files such as excel exports.
        /// </summary>
        public static string TempFolder
        {
            get { return Path.Combine(RootFolder, "temp"); }
        }

        /// <summary>
        /// Main folder for a client.  This is a combination of the RootFolder and the client's key value.
        /// </summary>
        public static string ClientFolder
        {
            get { return Path.Combine(RootFolder, string.Format(@"custom\{0}", Pinsonault.Web.Session.ClientKey)); }
        }

        /// <summary>
        /// Returns a general use folder which is any documents accessible to all clients.  
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public static string GetApplicationFolder(string Name)
        {
            return Path.Combine(RootFolder, Name);
        }

        /// <summary>
        /// Returns the forms folder.  
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public static string GetFormsFolder(string Name)
        {
            return Path.Combine(FormsFolder, Name);
        }

        /// <summary>
        /// Returns a client specific sub-folder.
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public static string GetClientFolder(string Name)
        {
            return Path.Combine(ClientFolder, Name);
        }

        public static string ClientTempFolder
        {
            get { return Path.Combine(RootFolder, string.Format(@"temp\custom\{0}", Pinsonault.Web.Session.ClientKey)); }
        }

        public static string GetClientTempFolder(string Name)
        {
            return Path.Combine(ClientTempFolder, Name);
        }

        /// <summary>
        /// Returns the maximum number of records a data service can return in a single request.
        /// </summary>
        public static int MaxAllowedRecords
        {
            get
            {
                if (_maxAllowedRecords == 0)
                {
                    string val = ConfigurationManager.AppSettings["MaxAllowedRecords"];
                    if (!int.TryParse(val, out _maxAllowedRecords))
                        _maxAllowedRecords = DEFAULT_MAX_ALLOWED_RECORDS;
                }

                return _maxAllowedRecords;
            }
        }

        /// <summary>
        /// Number of minutes a standard page should be cached.
        /// </summary>
        public static int PageCacheDuration
        {
            get
            {
                if (_pageCacheDuration == 0)
                {
                    string val = ConfigurationManager.AppSettings["DefaultPageCacheDuration"];
                    if (!int.TryParse(val, out _pageCacheDuration))
                        _pageCacheDuration = DEFAULT_PAGE_CACHE_DURATION;
                }

                return _pageCacheDuration;
            }
        }

        /// <summary>
        /// Returns the configured Admin email address for submitting notifications such as errors.
        /// </summary>
        public static string AdminEmail
        {
            get { return ConfigurationManager.AppSettings["AdminEmail"]; }
        }

        /// <summary>
        /// Returns the user's email address used for sending notifications from Pathfinder.
        /// </summary>
        public static string UserEmail
        {
            get { return HttpContext.Current.User.Identity.Name; }
        }

        /// <summary>
        /// Returns the configured Customer Support email address for submitting user requests.
        /// </summary>
        public static string CustomerSupportEmail
        {
            get { return ConfigurationManager.AppSettings["CustomerSupportEmail"]; }
        }

        /// <summary>
        /// Generic email account that is used for sending notifications from the web application.
        /// </summary>
        public static string ApplicationEmail
        {
            get { return ConfigurationManager.AppSettings["ApplicationEmail"]; }
        }

        /// <summary>
        /// Returns the applications base path.  Typically this is output to the browser so client scripts can properly construct urls.
        /// </summary>
        public static string BasePath
        {
            get
            {
                string path = HttpContext.Current.Request.ApplicationPath;
                if (string.Compare(path, "/") == 0)
                    return "";

                return path;
            }
        }

        /// <summary>
        /// Execute routines that need to run before application can be used.
        /// </summary>
        public static void ApplicationStartup()
        {
            //create client folders
            using (PathfinderEntities context = new PathfinderEntities())
            {
                var q = from client in context.ClientSet
                        where client.Status == true
                        select client;

                string folder;
                string clientFolder;
                string baseFolder = Path.Combine(Support.RootFolder, "custom");
                string baseFolder2 = Path.Combine(Support.RootFolder, "temp\\custom");
                foreach (Client client in q)
                {
                    //client folder - critical data
                    clientFolder = Path.Combine(baseFolder, client.Client_Key);
                    if (!Directory.Exists(clientFolder))
                        Directory.CreateDirectory(clientFolder);

                    //client temp folders
                    clientFolder = Path.Combine(baseFolder2, client.Client_Key);
                    if (!Directory.Exists(clientFolder))
                        Directory.CreateDirectory(clientFolder);
                    
                    folder = Path.Combine(clientFolder, "charts");
                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);
                    else
                    {
                        CleanFolder(folder, 3);
                    }
                }
            }
        }

        static void CleanFolder(string folder, int daysToExpire)
        {
            foreach (string file in Directory.GetFiles(folder))
            {
                if (File.GetLastWriteTime(file) < DateTime.Now.Subtract(new TimeSpan(daysToExpire, 0, 0, 0, 0)))
                    File.Delete(file);
            }
        }

        /// <summary>
        /// Simple helper function for sending SMTP email messages.
        /// </summary>
        /// <param name="From">The email address that the message is being sent from.</param>
        /// <param name="To">The email address of the recipient(s).  Multiple addresses should be separated by commas.</param>
        /// <param name="Subject">Subject of the message.</param>
        /// <param name="Body">Body of the message.</param>
        /// <param name="IsHTML">Indicates if the body of the message contains HTML markup.</param>
        /// <returns></returns>
        public static bool SendEmail(string From, string To, string Subject, string Body, bool IsHTML)
        {
            try
            {
                CorrectFromEmail(From, Subject);
                From = FromEmail;
                Subject = EmailSubject;

                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(ConfigurationManager.AppSettings["SmtpHost"]);
                System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage(From, To, Subject, Body);
                msg.IsBodyHtml = IsHTML;
                smtp.Send(msg);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Simple helper function for sending SMTP email messages with attachments.
        /// </summary>
        /// <param name="From">The email address that the message is being sent from.</param>
        /// <param name="To">The email address of the recipient(s).  Multiple addresses should be separated by commas.</param>
        /// <param name="Subject">Subject of the message.</param>
        /// <param name="Body">Body of the message.</param>
        /// <param name="IsHTML">Indicates if the body of the message contains HTML markup.</param>
        /// <param name="Attachments">A list of file paths to attach to the email.</param>
        /// <returns></returns>
        public static bool SendAttachmentEmail(string From, string To, string Subject, string Body, bool IsHTML, List<string> attachments)
        {
            CorrectFromEmail(From, Subject);
            From = FromEmail;
            Subject = EmailSubject;

            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage(From, To, Subject, Body);

            try
            {
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(ConfigurationManager.AppSettings["SmtpHost"]);

                msg.IsBodyHtml = IsHTML;

                foreach (string attachment in attachments)
                    msg.Attachments.Add(new System.Net.Mail.Attachment(attachment));

                smtp.Send(msg);

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (msg != null) msg.Dispose();
            }
        }

        /// <summary>
        /// Simple helper function for sending SMTP email messages with attachments.
        /// </summary>
        /// <param name="From">The email address that the message is being sent from.</param>
        /// <param name="To">The email address of the recipient(s).  Multiple addresses should be separated by commas.</param>
        /// <param name="CC">The email address of the cc recipient(s).  Multiple addresses should be separated by commas..</param>
        /// <param name="Subject">Subject of the message.</param>
        /// <param name="Body">Body of the message.</param>
        /// <param name="IsHTML">Indicates if the body of the message contains HTML markup.</param>
        /// <param name="Attachments">A list of file paths to attach to the email.</param>
        /// <returns></returns>
        public static bool SendAttachmentEmail(string From, string To, string CC, string Subject, string Body, bool IsHTML, List<string> attachments)
        {
            CorrectFromEmail(From, Subject);
            From = FromEmail;
            Subject = EmailSubject;

            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage(From, To, Subject, Body);

            try
            {
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(ConfigurationManager.AppSettings["SmtpHost"]);

                msg.IsBodyHtml = IsHTML;
                msg.CC.Add(CC);

                foreach (string attachment in attachments)
                    msg.Attachments.Add(new System.Net.Mail.Attachment(attachment));

                smtp.Send(msg);

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (msg != null) msg.Dispose();
            }
        }

        public static void CorrectFromEmail(string From, string Subject)
        {
            //change from email if it doesn't contain customer support email or Application email because emails can be sent from only registered emails as per company policy
            if (!From.Contains(CustomerSupportEmail) && !From.Contains(ApplicationEmail))
            {
                FromEmail = CustomerSupportEmail;
                EmailSubject = string.Format("{0} - from - {1}", Subject, UserEmail);
            }
            else
            {
                FromEmail = From;
                EmailSubject = Subject;
            }
        }

        /// <summary>
        /// Splits a website data field value when more than one url is stored in a single field.  Values should be separated by a semi-colan.
        /// </summary>
        /// <param name="website"></param>
        /// <returns></returns>
        public static string ParseWebsiteLink(string website)
        {
            return ParseWebsiteLink(website, 0);
        }

        public static string ParseWebsiteLink(string website, int maxLength)
        {
            if (!string.IsNullOrEmpty(website))
            {
                StringBuilder sb = new StringBuilder();

                string[] a = website.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                string site;
                string siteText;
                foreach (string item in a)
                {
                    site = item.Trim().Replace("http://", "").Replace("https://", "");
                    siteText = site;
                    if (maxLength > 3 && siteText.Length > maxLength)
                        siteText = string.Format("{0}...", siteText.Substring(0, maxLength - 3));

                    if (!string.IsNullOrEmpty(site))
                        sb.AppendFormat("{0}<a target='_blank' href='http://{1}'>{2}</a>", (sb.Length > 0 ? "&nbsp;&bull;&nbsp;" : ""), site, siteText);
                }
                return sb.ToString();
            }

            return string.Empty;
        }

        /// <summary>
        /// Emails customer support exception details so they can notify team that an error has occurred in production.
        /// </summary>
        /// <param name="ex">Exception that has occurred.</param>
        public static void EmailException(string UserName, HttpContext Context) //string RequestedUrl, Exception ex)
        {
            EmailException(UserName, Context, Context.Error);
        }

        public static void EmailException(string UserName, HttpContext Context, Exception Exception) //string RequestedUrl, Exception ex)
        {
            try
            {
                string email = Pinsonault.Web.Support.AdminEmail;
                if (!string.IsNullOrEmpty(email))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine("An unexpected error has occurred in the PathfinderRx application.");

                    sb.AppendLine();

                    sb.AppendFormat("User: {0}", UserName);
                    sb.AppendLine();
                    sb.AppendFormat("Url: {0}", Context.Request.Url.ToString());
                    sb.AppendLine();
                    sb.AppendLine();
                    NameValueCollection col = Context.Request.Form;
                    if (col.Count > 0)
                    {
                        sb.AppendLine();
                        sb.AppendLine("Form Data:".PadRight(100, '-'));
                        sb.AppendLine();
                        foreach (string key in col.Keys)
                        {
                            sb.AppendFormat("{0}: {1}", key, col[key]);
                            sb.AppendLine();
                        }
                        sb.AppendLine();
                        sb.AppendLine();
                    }


                    sb.AppendLine();
                    sb.AppendLine("Exception Details:".PadRight(100, '-'));
                    sb.AppendLine();

                    Exception exception = Exception;
                    while (exception != null)
                    {
                        sb.AppendLine("Exception:");
                        sb.AppendLine(exception.Message);
                        sb.AppendLine("Stack Trace:");
                        sb.AppendLine(exception.StackTrace);
                        sb.AppendLine();
                        exception = exception.InnerException;
                    }

                    sb.AppendLine();
                    sb.AppendLine();

                    col = Context.Request.ServerVariables;
                    if (col.Count > 0)
                    {
                        sb.AppendLine();
                        sb.AppendLine("Server Variables:".PadRight(100, '-'));
                        sb.AppendLine();

                        foreach (string key in col.Keys)
                        {
                            sb.AppendFormat("{0}: {1}", key, col[key]);
                            sb.AppendLine();
                        }
                    }

                    Pinsonault.Web.Support.SendEmail(ApplicationEmail, email, string.Format("PathfinderRx web site error [{0}]", HttpContext.Current.Request.Url.Host), sb.ToString(), false);
                }
            }
            catch { /*eat error since this is called from an exception - what else can we do at this point if this fails? */}
        }

        /// <summary>
        /// Registers an AJAX component with the current instance of clientManager during partial page updates.  This allows the clientManager to cleanup components loaded from a partial page update if that part of the page is reloaded with a new page.
        /// </summary>
        /// <param name="Page">Current page handling the request.</param>
        /// <param name="ID">Client ID of the component to register.</param>
        public static void RegisterComponentWithClientManager(Page Page, string ID)
        {
            RegisterComponentWithClientManager(Page, ID, null, null);
        }

        /// <summary>
        /// Registers an AJAX component with the current instance of clientManager during partial page updates.  This allows the clientManager to cleanup components loaded from a partial page update if that part of the page is reloaded with a new page.
        /// </summary>
        /// <param name="Page">Current page handling the request.</param>
        /// <param name="ID">Client ID of the component to register.</param>
        /// <param name="Data">Optional extra data to register with the component.  Typically this is used for adding static filters.  The format of the parameter must be JSON.</param>
        public static void RegisterComponentWithClientManager(Page Page, string ID, string Data)
        {
            RegisterComponentWithClientManager(Page, ID, Data, null);
        }

        /// <summary>
        /// Registers an AJAX component with the current instance of clientManager during partial page updates.  This allows the clientManager to cleanup components loaded from a partial page update if that part of the page is reloaded with a new page.
        /// </summary>
        /// <param name="Page">Current page handling the request.</param>
        /// <param name="ID">Client ID of the component to register.</param>
        /// <param name="Data">Optional extra data to register with the component.  Typically this is used for adding static filters.  The format of the parameter must be JSON.</param>
        /// <param name="ContainerID">Optional value that specifies what container is hosting the control.  If Null or Empty the value will default to "section2".</param>
        public static void RegisterComponentWithClientManager(Page Page, string ID, string Data, string ContainerID)
        {
            string data = Data;
            if (string.IsNullOrEmpty(data))
                data = "{}";

            if (string.IsNullOrEmpty(ContainerID))
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), ID, string.Format("window.top.clientManager.registerComponent('{0}', {1});", ID, data), true);
            else
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), ID, string.Format("window.top.clientManager.registerComponent('{0}', {1}, false, '{2}');", ID, data, ContainerID), true);
        }


        /// <summary>
        /// Outputs script variables to the web page for menu and list options.  
        /// </summary>
        /// <param name="Page">Current page handling the request.</param>
        public static void RegisterScriptVariables(Page Page)
        {
            PathfinderModel.PathfinderEntities context = null;
            PathfinderClientModel.PathfinderClientEntities contextClient = null;

            try
            {

                context = new PathfinderModel.PathfinderEntities();
                contextClient = new PathfinderClientModel.PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString);
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "_startUpTime", "var _startUp = new Date();", true);
                RegisterApplicationMenuOptionVariables(Page, context);
                RegisterClientScriptAndCss(Page);
                RegisterDrugListVariable(Page, context);
                RegisterUserAlignment(Page, context);
                RegisterStatesAndRegions(Page, context, Pinsonault.Web.Session.ClientID);
                RegisterClientOptions(Page, context, Pinsonault.Web.Session.ClientID);

                Page.ClientScript.RegisterClientScriptBlock(typeof(Page)
                                , "_modules"
                                , string.Format("var userModules = {0};{1}{2}{3}"
                                    , "{}"
                                    , (HttpContext.Current.User.IsInRole("ta") ? string.Format("userModules[{0}]={1};", Pinsonault.Web.Identifiers.TodaysAccounts, context.GetUserModulesAsJSON(Pinsonault.Web.Session.UserID, Pinsonault.Web.Identifiers.TodaysAccounts, false)) : "")                                    
                    //this is going to look odd but Modules from TA and SR are loaded in a slightly different manor and use different query - SHOULD LOOK INTO STREAMLINING THIS HACK
                                    , (HttpContext.Current.User.IsInRole("sr") ? string.Format("userModules[{0}] = {1};", Pinsonault.Web.Identifiers.StandardReports, context.GetUserReportOptionsAsJSON(Pinsonault.Web.Session.UserID, Pinsonault.Web.Identifiers.StandardReports)) : "")
                                    , (HttpContext.Current.User.IsInRole("cs") ? string.Format("userModules[{0}]={1};", Pinsonault.Web.Identifiers.CustomSegments, context.GetUserModulesAsJSON(Pinsonault.Web.Session.UserID, Pinsonault.Web.Identifiers.CustomSegments, false)) : ""))
                                    
                , true);
            }
            finally
            {
                if (context != null) context.Dispose();
                if (contextClient != null) contextClient.Dispose();
            }
        }

        public static void RegisterGenericListVariable(Page Page, GenericListItem[] list, string variableName)
        {
            Type[] types = { typeof(GenericListItem[]), typeof(GenericListItem) };

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(GenericListItem), "root", types);
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, list);
                string script = string.Format("var {0} = {1};", variableName, UTF8Encoding.UTF8.GetString(ms.ToArray()));

                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), variableName, script, true);
            }
        }


        /// <summary>
        /// Outputs script variable for a user's geography.  If the user is not aligned to any territory then a default geography or null is output.
        /// </summary>
        /// <param name="Page"></param>
        /// <param name="context"></param>
        public static void RegisterUserAlignment(Page Page, PathfinderModel.PathfinderEntities context)
        {
            UserGeography userGeography = null;

            if (!Pinsonault.Web.Session.Admin)
                userGeography = context.GetUserGeography(Pinsonault.Web.Session.UserID);

            string script;

            if (userGeography != null)
                script = string.Format("var userGeography = {0}CenterX:{1},CenterY:{2},Area:{3},RegionID:{4}{5};", "{", userGeography.CenterX, userGeography.CenterY, userGeography.Area, userGeography.RegionID != null ? string.Format("'{0}'", userGeography.RegionID) : "null", "}");
            else
                script = "var userGeography = null;";

            Page.ClientScript.RegisterClientScriptBlock(typeof(UserGeography), "_usergeography", script, true);

        }

        /// <summary>
        /// Outputs script variables for a listing of states, regions (top level territories), and region geographies (area & center point (lat/long)).
        /// </summary>
        /// <param name="Page"></param>
        /// <param name="context"></param>
        public static void RegisterStatesAndRegions(Page Page, PathfinderModel.PathfinderEntities context, int ClientID)//, PathfinderClientModel.PathfinderClientEntities contextClient)
        {
            //dsTerritories.ConnectionString = Pinsonault.Web.Session.ClientConnectionString;
            DataContractJsonSerializer serializer;

            string statesScript = HttpContext.Current.Cache["states"] as string;
            if (statesScript == null)
            {
                if (!Pinsonault.Web.Session.Admin)
                    statesScript = getStateScript(context);

                if (!string.IsNullOrEmpty(statesScript))
                {
                    //cache states
                    HttpContext.Current.Cache.Insert("states", statesScript, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromHours(12.0), new System.Web.Caching.CacheItemUpdateCallback(statesCacheItemUpdateCallback));
                }
                else
                {
                    //don't cache empty list
                    statesScript = "var statesList = [];";
                }
            }

            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "_states", statesScript, true);

            //get Territory List and territory states

            serializer = new DataContractJsonSerializer(typeof(Dictionary<string, string>));
            Dictionary<string, string> territories = null;

            if (!Pinsonault.Web.Session.Admin) //get regions (User_Level null (old) or User_Level == 2)
                territories = context.TerritorySet.Where(t => (t.User_Level == null || t.User_Level == 2) && t.Client_ID == ClientID).Select(t => new { ID = t.Territory_ID, Name = t.Territory_Name }).ToDictionary(s => s.ID, s => s.Name);

            if (territories != null && territories.Count > 0)
            {
                //StringBuilder terrGeo = new StringBuilder("{");
                //string geoIDs;
                //foreach ( PathfinderClientModel.Territory terr in contextClient.TerritorySet.Include("TerritoryGeographies").Where(t => t.Area > 0).OrderBy(t => t.ID) )
                //{
                //    geoIDs = String.Join(",", terr.TerritoryGeographies.Select(tg => tg.Geography_ID).ToArray());
                //    terrGeo.AppendFormat("{0}\"{1}\":{2}\"Area\":{3},\"CenterX\":{4},\"CenterY\":{5},\"Regions\":\"{6}\"{7}", (terrGeo.Length > 1 ? "," : ""), terr.ID, "{", terr.Area, terr.Center_X, terr.Center_Y, geoIDs, "}");
                //}
                //terrGeo.Append("}");

                using (MemoryStream ms = new MemoryStream())
                {
                    serializer.WriteObject(ms, territories);

                    //Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "_regions", string.Format("var regionsList = {0};var regionsGeographyList = {1};", System.Text.UTF8Encoding.UTF8.GetString(ms.ToArray()), terrGeo.ToString()), true);
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "_regions", string.Format("var regionsList = {0};", System.Text.UTF8Encoding.UTF8.GetString(ms.ToArray())), true);
                }
            }
            else
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "_regions", "var regionsList = [];var regionsGeographyList = {};", true);
            }
        }
        public static void RegisterClientOptions(Page Page, PathfinderModel.PathfinderEntities context, int ClientID)
        {
            string clientOptionScript = "var clientHasCustomPlans = false;";
            var p = (from q in context.ClientSet where q.Client_Key == Pinsonault.Web.Session.ClientKey select q.Has_Custom_Plans).FirstOrDefault();
            if (p == true)
                clientOptionScript = "var clientHasCustomPlans = true;";
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "clientOption", clientOptionScript, true);

        }

        public static void RegisterClientScriptAndCss(Page Page)
        {
            Dictionary<string, PathfinderModel.ClientApplicationAccess> clientApplicationAccess = Pinsonault.Web.Session.ClientApplicationAccess;

            if (clientApplicationAccess != null)
            {
                bool hasCustomApp = clientApplicationAccess.Count(ca => ca.Value.Application.Is_Custom) > 0;
                bool hasAdmin = clientApplicationAccess.Count(ca => ca.Value.Application.App_Key == "admin") > 0;

                //output custom client script if custom app
                if ((hasCustomApp && Pinsonault.Web.Session.ClientID != 1) || (hasAdmin && Pinsonault.Web.Session.Admin))
                {
                    ScriptManager.GetCurrent(Page).Scripts.Add(new ScriptReference(string.Format("~/custom/{0}/content/scripts/{0}.js", Pinsonault.Web.Session.ClientKey)));

                    System.Web.UI.HtmlControls.HtmlLink css = new System.Web.UI.HtmlControls.HtmlLink() { Href = string.Format(string.Format("~/custom/{0}/content/styles/{0}.css", Pinsonault.Web.Session.ClientKey)) };
                    css.Attributes["type"] = "text/css";
                    css.Attributes["rel"] = "stylesheet";
                    Page.Header.Controls.Add(css);
                }
            }
        }

        /// <summary>
        /// Outputs two variables to the web page that contain menu options for available applications and sections.  The application options variable will be named appMenuItems and the section options variable will be named sectionMenuItems.
        /// </summary>
        /// <param name="Page">Current page handling the request.</param>
        static void RegisterApplicationMenuOptionVariables(Page Page, PathfinderModel.PathfinderEntities context)
        {

            //PathfinderService service = new PathfinderService();

            //app script output is is var appMenuItems = [{ID:1,Name:"Todays Account's"},{ID:2,Name:"Today's Analytics"}, ...];
            StringBuilder appMenuScript = new StringBuilder("var appMenuItems = [");

            //app script output is is var sectionMenuItems = {1:{1:{Name:"Commercial Payers"},4:{Name:"PBM"},...},2:{1:{Name:"Commercial Payers"},4:{Name:"PBM"},...},...};
            StringBuilder channelMenuScript = new StringBuilder("var channelMenuItems = {");

            Dictionary<string, PathfinderModel.ClientApplicationAccess> clientApplicationAccess = Pinsonault.Web.Session.ClientApplicationAccess;

            int lastAppID = -1;
            int x = 0; //for fixing section order in chrome and ie9
            bool addComma = false;
            bool hasCustomApp = false;
            bool hasAdmin = clientApplicationAccess.Count(ca => ca.Value.Application.App_Key == "admin") > 0;

            Section section;

            foreach (KeyValuePair<string, PathfinderModel.ClientApplicationAccess> ca in clientApplicationAccess.OrderBy(ca => ca.Value.ApplicationID).ThenBy(ca => ca.Value.Section.Sort_Order))
            {
                if (ca.Value.Display_In_Menu)
                {
                    if (lastAppID != ca.Value.ApplicationID)
                    {
                        addComma = false;

                        if (lastAppID != -1) //close previous section settings
                        {
                            channelMenuScript.Append("},");
                        }

                        hasCustomApp = hasCustomApp || ca.Value.Application.Is_Custom;

                        appMenuScript.Append("{");
                        appMenuScript.AppendFormat("ID:{0},Name:\"{1}\",Custom:{2},App_Folder:\"{3}\"", ca.Value.Application.ID, ca.Value.Application.Name, ca.Value.Application.Is_Custom.ToString().ToLower(), ca.Value.Application.App_Folder);
                        appMenuScript.Append("},");


                        channelMenuScript.AppendFormat("{0}:", ca.Value.ApplicationID);
                        //channelMenuScript.Append("{0:{ID:0,Name:");
                        //channelMenuScript.AppendFormat("\"{0}\"", Resources.Resource.Label_ListItem_All);
                        //channelMenuScript.Append("}");
                    }

                    lastAppID = ca.Value.ApplicationID;

                    if (addComma)
                        channelMenuScript.Append(",");
                    else
                        channelMenuScript.Append("{");


                    section = ca.Value.Section;
                    if (section != null) //don't add missing sections
                    {
                        //channelMenuScript.AppendFormat("{0}:", ca.Value.SectionID);
                        channelMenuScript.AppendFormat("{0}:", x);
                        channelMenuScript.Append("{");
                        channelMenuScript.AppendFormat("ID:{0},Name:\"{1}\",Custom:{2},Folder:\"{3}\"", ca.Value.SectionID, section.Name, section.Is_Custom.ToString().ToLower(), section.Section_Folder);
                        channelMenuScript.Append("}");
                    }
                    else
                    {
                        channelMenuScript.Append("0:{ID:0,Name:");
                        channelMenuScript.AppendFormat("\"{0}\",Folder:\"all\"", "All");
                        channelMenuScript.Append("}");
                    }

                    addComma = true;
                    x++;
                }
            }

            appMenuScript.Remove(appMenuScript.Length - 1, 1); //remove extra comma
            appMenuScript.Append("];");

            if (lastAppID > -1)
            {
                channelMenuScript.Append("}};");
            }
            else
                channelMenuScript.Append("};");

            //merge scripts
            appMenuScript.AppendLine();
            appMenuScript.Append(channelMenuScript.ToString());

            //output
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "appMenu", appMenuScript.ToString(), true);
        }

        static AggregateCacheDependency GetDrugListCacheDependency()
        {
            AggregateCacheDependency dependency = null;

            try
            {
                string list = ConfigurationManager.AppSettings["drugListTables"];
                if (!string.IsNullOrEmpty(list))
                {
                    dependency = new AggregateCacheDependency();
                    foreach (string table in list.Split(','))
                    {                       
                        dependency.Add(new SqlCacheDependency("pathfinderMaster", table.Trim()));
                    }
                }
            }
            catch (Exception ex)
            {
                //probably not enabled on machine 
                EmailException(Support.ApplicationEmail, HttpContext.Current, ex);
            }

            return dependency;
        }

        /// <summary>
        /// Outputs a variable for available drugs organized by market basket.  The name of the variable will be drugListOptions.
        /// </summary>
        /// <param name="Page">Current page handling the request.</param>
        static void RegisterDrugListVariable(Page Page, PathfinderModel.PathfinderEntities context)
        {
            //Output Drug List
            //{TID:[{ID:0,Name:"DrugName"},...],TID2:[{ID:1,Name:"DrugName1"},...]}

            int clientID = Pinsonault.Web.Session.ClientID;

            string cacheKey = string.Format("{0}_druglist", Pinsonault.Web.Session.ClientID);
            string script = HttpContext.Current.Cache[cacheKey] as string;

            if (string.IsNullOrEmpty(script))
            {
                script = getDrugListScript(clientID, context);

                TimeSpan slidingExpiration = Cache.NoSlidingExpiration;
                AggregateCacheDependency dependency = GetDrugListCacheDependency();
                if (dependency == null)
                    slidingExpiration = TimeSpan.FromHours(1.0);

                HttpContext.Current.Cache.Insert(cacheKey, script, dependency, System.Web.Caching.Cache.NoAbsoluteExpiration, slidingExpiration, new System.Web.Caching.CacheItemUpdateCallback(drugListCacheItemUpdateCallback));
            }

            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "drugListOptions", script, true);
            //
        }

        /// <summary>
        /// Fixes up urls that start with ~ and puts the applications relative root path in its place.
        /// This can be used independently of a Page context.
        /// </summary>
        /// <param name="originalUrl">URL to translate</param>
        /// <returns>Expanded URL</returns>
        public static string ResolveUrl(string originalUrl)
        {
            if (originalUrl == null)
                return null;

            // Absolute path - just return
            if (originalUrl.IndexOf("://") != -1)
                return originalUrl;

            // Fix up image path for ~ root app dir directory
            if (originalUrl.StartsWith("~"))
            {
                string newUrl = "";
                if (HttpContext.Current != null)
                    newUrl = HttpContext.Current.Request.ApplicationPath +
                          originalUrl.Substring(1).Replace("//", "/");
                else
                    // Not context: assume current directory is the base directory
                    throw new ArgumentException("Invalid URL: Relative URL not allowed.");

                // Just to be sure fix up any double slashes
                return newUrl;
            }

            return originalUrl;
        }

        public static void RegisterTierScriptVariable(Page Page)
        {
            string tierSet = HttpContext.Current.Cache["tierSet"] as string;

            if (tierSet == null)
            {

                tierSet = getTierScript();

                HttpContext.Current.Cache.Insert("tierSet", tierSet, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromHours(12.0), new System.Web.Caching.CacheItemUpdateCallback(tierSetCacheItemUpdateCallback));
            }

            Page.ClientScript.RegisterStartupScript(typeof(Page), "tierSetScript", tierSet, true);
        }

        public static string GetDataUpdateDateByKey(string key)
        {
            return GetDataUpdateDateByKey(key, null);
        }

        public static string GetDataUpdateDateByKey(string key, string format)
        {
            string appliedFormat = string.IsNullOrEmpty(format) ? "{0:MMMM dd, yyyy}" : format;

            Dictionary<string, DataUpdateDates> dates = Pinsonault.Web.Support.DataUpdateDates;
            if (dates != null && dates.ContainsKey(key))
                return string.Format(appliedFormat, dates[key].Update_Date);

            return "";
        }

        public static Dictionary<string, DataUpdateDates> DataUpdateDates
        {
            get
            {
                Dictionary<string, DataUpdateDates> returnVal = null;

                object val = HttpContext.Current.Cache["dataUpdateDates"];
                if (val == null)
                {
                    val = getDataUpdateDates();

                    HttpContext.Current.Cache.Insert("dataUpdateDates", val, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromHours(2.0), new System.Web.Caching.CacheItemUpdateCallback(dataUpdateDatesCacheItemUpdateCallback));

                    returnVal = (Dictionary<string, DataUpdateDates>)val;
                }
                else
                    returnVal = (Dictionary<string, DataUpdateDates>)val;

                return returnVal;
            }
        }

        static void dataUpdateDatesCacheItemUpdateCallback(string key, CacheItemUpdateReason reason, out object expensiveObject, out CacheDependency dependency, out DateTime absoluteExpiration, out TimeSpan slidingExpiration)
        {
            absoluteExpiration = Cache.NoAbsoluteExpiration;
            slidingExpiration = TimeSpan.FromHours(2.0);
            dependency = null;
            expensiveObject = null;
            expensiveObject = getDataUpdateDates();
        }

        static Dictionary<string, DataUpdateDates> getDataUpdateDates()
        {
            using (PathfinderEntities context = new PathfinderEntities())
            {
                return context.DataUpdateDatesSet.ToDictionary(i => i.Data_Item);
            }
        }

        /// <summary>
        /// Web cache callback function that reloads state script after the cached item times out.  Do not call this directly in code!
        /// </summary>
        /// <param name="key"></param>
        /// <param name="reason"></param>
        /// <param name="expensiveObject"></param>
        /// <param name="dependency"></param>
        /// <param name="absoluteExpiration"></param>
        /// <param name="slidingExpiration"></param>
        static void statesCacheItemUpdateCallback(string key, CacheItemUpdateReason reason, out object expensiveObject, out CacheDependency dependency, out DateTime absoluteExpiration, out TimeSpan slidingExpiration)
        {
            absoluteExpiration = Cache.NoAbsoluteExpiration;
            slidingExpiration = TimeSpan.FromHours(12.0);
            dependency = null;
            expensiveObject = null;

            using (PathfinderEntities context = new PathfinderEntities())
            {
                expensiveObject = getStateScript(context);
            }
        }

        /// <summary>
        /// Returns javascript for outputting script variable for states list.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        static string getStateScript(PathfinderEntities context)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Dictionary<string, string>));
            Dictionary<string, string> states = context.StateSet.OrderBy(s => s.Name).ToDictionary(s => s.ID, s => s.Name);

            if (states.Count > 0)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    serializer.WriteObject(ms, states);

                    return string.Format("var statesList = {0};", System.Text.UTF8Encoding.UTF8.GetString(ms.ToArray()));
                }
            }

            return null;
        }

        /// <summary>
        /// Web cache callback function that reloads tier set script after the cached item times out.  Do not call this directly in code!
        /// </summary>
        /// <param name="key"></param>
        /// <param name="reason"></param>
        /// <param name="expensiveObject"></param>
        /// <param name="dependency"></param>
        /// <param name="absoluteExpiration"></param>
        /// <param name="slidingExpiration"></param>
        static void tierSetCacheItemUpdateCallback(string key, CacheItemUpdateReason reason, out object expensiveObject, out CacheDependency dependency, out DateTime absoluteExpiration, out TimeSpan slidingExpiration)
        {
            absoluteExpiration = Cache.NoAbsoluteExpiration;
            slidingExpiration = TimeSpan.FromHours(12.0);
            dependency = null;
            expensiveObject = getTierScript();
        }

        /// <summary>
        /// Returns a script variable that contains available list of tiers.
        /// </summary>
        /// <returns></returns>
        static string getTierScript()
        {
            StringBuilder sb = new StringBuilder("[{");
            sb.AppendFormat("id:-1,text:\"{0}\"", "All Tiers");
            sb.Append("}");

            using (PathfinderEntities context = new PathfinderEntities())
            {
                foreach (Tier tier in context.TierSet)
                {
                    sb.Append(",");

                    sb.AppendFormat("{0}id:{1},text:\"{2}\"{3}", "{", tier.ID, tier.Name, "}");
                }
            }
            sb.Append("]");

            return string.Format("var _tierSet={0};", sb.ToString());
        }

        static void drugListCacheItemUpdateCallback(string key, CacheItemUpdateReason reason, out object expensiveObject, out CacheDependency dependency, out DateTime absoluteExpiration, out TimeSpan slidingExpiration)
        {
            absoluteExpiration = Cache.NoAbsoluteExpiration;

            dependency = GetDrugListCacheDependency();            
            if (dependency == null)
                slidingExpiration = TimeSpan.FromHours(1.0);
            else
                slidingExpiration = TimeSpan.Zero;
            
            expensiveObject = null;

            int clientID = 0;

            string[] parts = key.Split(new char[] { '_' }, 2);

            if (Int32.TryParse(parts[0], out clientID))
            {
                if (clientID > 0)
                {
                    using (PathfinderEntities context = new PathfinderEntities())
                    {
                        expensiveObject = getDrugListScript(clientID, context);
                    }
                }
            }

        }

        static string getDrugListScript(int clientID, PathfinderModel.PathfinderEntities context)
        {
            StringBuilder sb = new StringBuilder("var drugListOptions = {");
            StringBuilder sb2 = new StringBuilder("var marketBasketListOptions = [");

            int currentTheraID = 0;

            IQueryable<DrugListEntry> drugs = context.GetUserDrugList(clientID);

            bool hasDrugs = false;

            foreach (DrugListEntry drug in drugs)
            {
                if (drug.TherapeuticClassID != currentTheraID)
                {
                    if (currentTheraID > 0)
                    {
                        sb.Remove(sb.Length - 1, 1);
                        sb.Append("]");
                    }
                    sb.AppendFormat("{0}{1}:[", (currentTheraID > 0 ? "," : ""), drug.TherapeuticClassID);

                    currentTheraID = drug.TherapeuticClassID;

                    sb2.Append("{ID:");
                    sb2.AppendFormat("{0},Name:\"{1}\"", drug.TherapeuticClassID, drug.TherapeuticClassName);
                    sb2.Append("},");
                }

                sb.Append("{ID:");
                sb.AppendFormat("{0},Name:\"{1}\",Selected:{2}", drug.ID, drug.Name, drug.Selected.ToString().ToLower());
                sb.Append("},");

                hasDrugs = true;
            }

            if (hasDrugs)
            {
                sb.Remove(sb.Length - 1, 1);
                sb.Append("]};");

                sb2.Remove(sb2.Length - 1, 1);
                sb2.Append("];");
            }
            else
            {
                sb.Append("};");
                sb2.Append("];");
            }

            sb2.Append(sb.ToString());

            return sb2.ToString();
        }

        public static void InitializeService(IDataServiceConfiguration config)
        {
            config.SetEntitySetAccessRule("*", EntitySetRights.None);
            config.SetServiceOperationAccessRule("*", ServiceOperationRights.AllRead);

            //PUT THIS LINE BACK AFTER DATA SERVICE PATCH IS INSTALLED
            //((DataServiceConfiguration)config).DataServiceBehavior.MaxProtocolVersion = System.Data.Services.Common.DataServiceProtocolVersion.V2;
            //USE THIS LINE FOR NOW TO DYNAMICALLY DETECT PATCH AND NOT REQUIRE CODE UPDATE
            setMaxProtocolVersion(config);


            config.MaxExpandCount = 0;
            config.MaxExpandDepth = 0;

            config.UseVerboseErrors = System.Web.HttpContext.Current.IsDebuggingEnabled;

            config.MaxResultsPerCollection = Pinsonault.Web.Support.MaxAllowedRecords;
        }

        public static bool HasDataServicePatch { get; internal set; }

        static void setMaxProtocolVersion(IDataServiceConfiguration config)
        {
            Type type = Type.GetType("System.Data.Services.DataServiceConfiguration, System.Data.Services, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", false, true);
            if (type != null)
            {
                PropertyInfo propInfo = type.GetProperty("DataServiceBehavior", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                if (propInfo != null)
                {
                    object o = propInfo.GetValue(config, null);
                    if (o != null)
                    {
                        type = Type.GetType("System.Data.Services.DataServiceBehavior, System.Data.Services, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", false, true);
                        if (type != null)
                        {
                            propInfo = type.GetProperty("MaxProtocolVersion", BindingFlags.Instance | BindingFlags.Public);
                            if (propInfo != null)
                                propInfo.SetValue(o, 2, null);
                        }
                    }
                }
            }

        }
    }
}