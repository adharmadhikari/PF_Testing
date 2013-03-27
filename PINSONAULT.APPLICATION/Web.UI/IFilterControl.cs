using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Pinsonault.Web.UI
{
    /// <summary>
    /// Optional interface to implement if a report filter control can handle a default value specified by the database.  It is up to the implementing control to determine how this value is applied.
    /// </summary>
    public interface IFilterControl
    {
        string DefaultValue { get; set; }
    }
}