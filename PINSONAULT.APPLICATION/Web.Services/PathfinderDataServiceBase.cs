using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Services;
using System.ServiceModel.Web;
using System.Data.Objects;
using System.Diagnostics;

namespace Pinsonault.Web.Services
{
    /// <summary>
    /// Summary description for PathfinderDataServiceBase
    /// </summary>
    [System.ServiceModel.ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class PathfinderDataServiceBase<T> : DataService<T> where T : ObjectContext
    {
        public PathfinderDataServiceBase()
        {
        }

        protected override T CreateDataSource()
        {
            Session.CheckSessionState();

            return base.CreateDataSource();
        }

        protected override void HandleException(HandleExceptionArgs args)
        {
            Support.EmailException(HttpContext.Current.User.Identity.Name, HttpContext.Current, args.Exception);
         
            base.HandleException(args);
        }

        /// <summary>
        /// Determines if the service should try to block requests that do not originate from a web browser.  
        /// </summary>
        protected virtual bool BlockNonBrowserRequests
        {
            get { return true; }
        }

        /// <summary>
        /// Determines if the service should prevent requests from web pages that are not located on the same host server.
        /// </summary>
        protected virtual bool VerifyHost
        {
            get { return true; }
        }

        protected override void OnStartProcessingRequest(ProcessRequestArgs args)
        {
            if ( !HttpContext.Current.IsDebuggingEnabled )
            {
                //Attempts to prevent requests if UserAgent or UrlReferrer are null because it is not a valid browser request        
                if ( BlockNonBrowserRequests && (HttpContext.Current.Request.UserAgent == null || HttpContext.Current.Request.UrlReferrer == null) )
                {
                    try { EventLog.WriteEntry("Pathfinder", "Data Service request failed because the User-Agent and/or Referring URL could not be verified.", EventLogEntryType.Warning); }
                    catch { }
                    //exception message is vague for users in case they see them but they shouldn't anyway
                    throw new HttpException(500, string.Format("Request Failed: Please contact Pinsonault support with error code {0}", 900501));
                }

                //Attempts to prevent requests if referrer and service are not on same host
                if ( VerifyHost && (HttpContext.Current.Request.UrlReferrer == null ||
                                            string.Compare(HttpContext.Current.Request.UrlReferrer.Host, HttpContext.Current.Request.Url.Host, true) != 0) )
                {
                    try { EventLog.WriteEntry("Pathfinder", "Data Service request failed because the host of the Referring URL does not match the data service's host name.", EventLogEntryType.Warning); }
                    catch { }
                    //exception message is vague for users in case they see them but they shouldn't anyway
                    throw new HttpException(500, string.Format("Request Failed: Please contact Pinsonault support with error code {0}", 900502));
                }
            }

            base.OnStartProcessingRequest(args);
        }

        [WebGet]
        public virtual string GetModuleOptions()
        {
            return "[]";
        }
    }

    public class PathfinderClientDataServiceBase<T> : PathfinderDataServiceBase<T> where T : ObjectContext
    {
        protected override T CreateDataSource()
        {
            T dataSource = base.CreateDataSource();
            dataSource.Connection.ConnectionString = Session.ClientConnectionString;

            return dataSource;
        }
    }
}