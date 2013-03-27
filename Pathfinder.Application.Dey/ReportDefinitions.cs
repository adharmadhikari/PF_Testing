using System;
using System.Collections.Specialized;
using System.Linq;
using Pinsonault.Data;
using Pinsonault.Data.Reports;

namespace Pinsonault.Application.Dey
{
    public class RestrictionsReportDefinitionBase : ReportDefinition
    {
        protected override QueryDefinition CreateQueryDefinition(NameValueCollection filters)
        {
            return new RestrictionsReportQueryDefinition(EntityTypeName, filters);
        }
    }

    public class RestrictionsReportDefinition : RestrictionsReportDefinitionBase
    {
        private NameValueCollection newFilters;

        protected override QueryDefinition CreateQueryDefinition(NameValueCollection filters)
        {
            string geoID = filters["Geography_ID"];
            if ( string.Compare(filters["Geography_ID"], "US", true) != 0 )
            {
                SectionTitle = string.Format("{0} {1}", SectionTitle, geoID);
            }

            newFilters = filters;

            return base.CreateQueryDefinition(filters);
        }        
    }

    public class RestrictionsReportNationalDefinition : RestrictionsReportDefinitionBase
    {
        protected override QueryDefinition CreateQueryDefinition(NameValueCollection filters)
        {
            NameValueCollection newFilters = new NameValueCollection(filters);

            //Force US if state or region present
            if ( string.Compare(filters["Geography_ID"], "US", true) != 0 )
                newFilters["Geography_ID"] = "US";
            else
                Visible = false; //if already US then hide this one and show regular version

            return base.CreateQueryDefinition(newFilters);
        }
    }

    
    /// <summary>
    /// used for Restrictions Report
    /// </summary>
    public class DrilldownReportDefinition : ReportDefinition
    {
        protected override QueryDefinition CreateQueryDefinition(NameValueCollection filters)
        {
            //if(!string.IsNullOrEmpty(filters["Is_Predominant"]))
            //{
            //    if (filters["Is_Predominant"] == "1")
            //    {
            //        filters.Remove("Is_Predominant");
            //        filters.Add("Is_Predominant", "true");
            //    }
            //}           
            return new DrilldownQueryDefinition(this.EntityTypeName, filters);
        }
    }
}

