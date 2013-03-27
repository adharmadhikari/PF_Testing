using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pinsonault.Web
{
    /// <summary>
    /// Server side representation of the Current UI State tracked by the ClientManager AJAX component.
    /// </summary>
    public class StandardReportApplicationState
    {
        public StandardReportApplicationState()
        {
            Application = 1;
            Channel = 1;

            //need to remove
            Drug = null;
        }

        public int Application { get; set; }
        public int Channel { get; set; }
        public string Module { get; set; }
        //SPH 7/27/2009 - Probably don't need to load Region which can be string or array so excluding since it is not used 
        public string Region { get; set; }
        public int? Drug { get; set; }
        public int? MarketBasket { get; set; }
        //public int? UserID { get; set; }
        public string restrictions { get; set; }

        UserGeography _geography = new UserGeography();
        public UserGeography Geography
        {
            get { return _geography; }
            set
            {
                if ( value != null )
                    _geography = value;
                else
                    _geography = new UserGeography();
            }
        }
    }
}