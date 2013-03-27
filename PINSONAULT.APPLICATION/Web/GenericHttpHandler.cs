using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace Pinsonault.Web
{
    /// <summary>
    /// Summary description for GenericHttpHandler
    /// </summary>
    public abstract class GenericHttpHandler : IHttpHandler, IRequiresSessionState
    {
        public GenericHttpHandler()
        {
        }

        public abstract bool IsReusable { get; }

        public void ProcessRequest(HttpContext context)
        {
            Pinsonault.Web.Session.CheckSessionState();

            InternalProcessRequest(context);
        }

        protected abstract void InternalProcessRequest(HttpContext context);
        
    }
}