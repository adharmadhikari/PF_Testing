using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.Reflection;
namespace Pinsonault.Web.Utilities
{
    /// <summary>
    /// This class is for logging the exceptions
    /// </summary>
    public class EventLogLogger
    {  

            ///<summary>
            ///This function logs the error message with details in event view
            /// </summary>            
            ///<param name="strmessage">error message</param>
            ///<param name="strErrorDetails">error details from exception stack trace</param>   
            /// <remarks>
            /// strmessage = ex.Message.ToString(), where ex = exception
            /// strErrorDetails = ex.StackTrace.ToString(),where ex = exception
            /// </remarks>
            public void LogError(string strmessage,string strErrorDetails)
            {
                string strDetailedMessage = strmessage + ": " + strErrorDetails;
                LogEntry(strDetailedMessage, EventLogEntryType.Error);
                
            }

            ///<summary>
            ///This function creates an event source and logs the exception in event view
            /// </summary>
            /// <param name="message">error message</param>
            /// <param name="logType">EventLogEntryType</param>            
            private void LogEntry(string message, EventLogEntryType logType)
            {
                if (!EventLog.SourceExists("Pathfinder"))
                    EventLog.CreateEventSource("Pathfinder", "Application");
                EventLog PathfinderLog = new EventLog();
                PathfinderLog.Source = "Pathfinder";
                PathfinderLog.WriteEntry(message, logType);
            }           
    }
}
