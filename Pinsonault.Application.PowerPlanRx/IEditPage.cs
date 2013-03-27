using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Pinsonault.Application.PowerPlanRx
{
    /// <summary>
    /// Summary description for IEditPage
    /// </summary>
    public interface IEditPage
    {
        bool Save();
        void Reset();
        void Edit();
    }
}
