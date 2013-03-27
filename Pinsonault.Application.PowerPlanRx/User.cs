using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;

namespace Pinsonault.Application.PowerPlanRx
{
    /// <summary>
    /// Summary description for User
    /// </summary>
    public static class PPRXUser
    {
        public static bool CanCreate
        {
            get
            {
                return HttpContext.Current.User.IsInRole("pprx_CVA");                 
            }
        }

        public static bool CanApprove
        {
            get 
            { 
                return (HttpContext.Current.User.IsInRole("pprx_CVA") || HttpContext.Current.User.IsInRole("pprx_VA")); 
            }
        }
    }
}