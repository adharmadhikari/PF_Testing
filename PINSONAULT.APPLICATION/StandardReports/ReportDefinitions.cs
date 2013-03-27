using System;
using System.Collections.Specialized;
using System.Linq;
using PathfinderClientModel;
using PathfinderModel;
using Pinsonault.Data;
using Pinsonault.Data.Reports;
using System.Drawing;
using System.Data.Common;
using Pinsonault.Web;


namespace Pinsonault.Application.StandardReports
{
    public class TierCoverageReportDefinitionBase : ReportDefinition
    {
        protected override QueryDefinition CreateQueryDefinition(NameValueCollection filters)
        {
            return new TierCoverageQueryDefinition(EntityTypeName, filters);
        }
    }

    public class TierCoverageReportDefinition : TierCoverageReportDefinitionBase
    {

        protected override QueryDefinition CreateQueryDefinition(NameValueCollection filters)
        {
            if (!String.IsNullOrEmpty(filters["Selected_Section_ID"]))
            {
                filters.Remove("Section_ID");

                if (filters["Selected_Section_ID"] != "0")
                    filters.Add("Section_ID", filters["Selected_Section_ID"]);
            }

            string geoID = filters["Geography_ID"];
            if ( string.Compare(filters["Geography_ID"], "US", true) != 0 )
            {
                SectionTitle = string.Format("{0} {1}", SectionTitle, geoID);
            }

            return base.CreateQueryDefinition(filters);
        }
    }

    public class TierCoverageNationalReportDefinition : TierCoverageReportDefinitionBase
    {
        protected override QueryDefinition CreateQueryDefinition(NameValueCollection filters)
        {
            NameValueCollection newFilters = new NameValueCollection(filters);

            if (!String.IsNullOrEmpty(newFilters["Selected_Section_ID"]))
            {
                newFilters.Remove("Section_ID");

                if (filters["Selected_Section_ID"] != "0")
                    newFilters.Add("Section_ID", newFilters["Selected_Section_ID"]);
            }

            //Force US if state or region present
            if ( string.Compare(filters["Geography_ID"], "US", true) != 0 )
                newFilters["Geography_ID"] = "US";
            else
                Visible = false; //if already US then hide this one and show regular version

            return base.CreateQueryDefinition(newFilters);
        }
    }


    //SL
    public class FormularyStatusReportDefinition : ReportDefinition
    {
        protected override QueryDefinition CreateQueryDefinition(NameValueCollection filters)
        {
            if (!String.IsNullOrEmpty(filters["Selected_Section_ID"]))
            {
                filters.Remove("Section_ID");

                if (filters["Selected_Section_ID"] != "0")
                    filters.Add("Section_ID", filters["Selected_Section_ID"]);
            }

            return new FormularyStatusQueryDefinition("ReportsFormularyStatusSummary", filters);
        }
    }

    public class FormularyStatusNationalReportDefinition : FormularyStatusReportDefinition
    {
        protected override QueryDefinition CreateQueryDefinition(NameValueCollection filters)
        {
            NameValueCollection newFilters = new NameValueCollection(filters);

            if (!String.IsNullOrEmpty(newFilters["Selected_Section_ID"]))
            {
                newFilters.Remove("Section_ID");

                if (filters["Selected_Section_ID"] != "0")
                    newFilters.Add("Section_ID", newFilters["Selected_Section_ID"]);
            }

            //Force US if state or region present
            if ( string.Compare(filters["Geography_ID"], "US", true) != 0 )
                newFilters["Geography_ID"] = "US";
            else
                Visible = false; //if already US then hide this one and show regular version

            return base.CreateQueryDefinition(newFilters);
        }
    }

    /// <summary>
    /// for formulary coverage report export
    /// </summary>
    public class FormularyCoverageReportDefinition : ReportDefinition
    {
        protected override QueryDefinition CreateQueryDefinition(NameValueCollection filters)
        {
            if (!String.IsNullOrEmpty(filters["Selected_Section_ID"]))
            {
                filters.Remove("Section_ID");

                if (filters["Selected_Section_ID"] != "0")
                    filters.Add("Section_ID", filters["Selected_Section_ID"]);
            }

            return new FormularyCoverageQueryDefinition("FormularyCoverageSummary", filters);
        }

    }
    /// <summary>
    /// for formulary coverage report export- national
    /// </summary>
    public class FormularyCoverageNationalReportDefinition : FormularyCoverageReportDefinition
    {
        protected override QueryDefinition CreateQueryDefinition(NameValueCollection filters)
        {
            if (!String.IsNullOrEmpty(filters["Selected_Section_ID"]))
            {
                filters.Remove("Section_ID");

                if (filters["Selected_Section_ID"] != "0")
                    filters.Add("Section_ID", filters["Selected_Section_ID"]);
            }

            //Force US if state or region present
            if (string.Compare(filters["Geography_ID"], "US", true) != 0)
                filters["Geography_ID"] = "US";
            else
                Visible = false; //if already US then hide this one and show regular version

            return base.CreateQueryDefinition(filters);
        }

    }

    /// <summary>
    /// used for Formulary Status, Lives Distribution and Tier Coverage Report
    /// </summary>
    public class DrilldownReportDefinition : ReportDefinition
    {
        protected override QueryDefinition CreateQueryDefinition(NameValueCollection filters)
        {
            return new DrilldownQueryDefinition(this.EntityTypeName, filters);
        }
    }

    /// <summary>
    /// used for Formulary Drilldown Report
    /// </summary>
    public class FrmlyDrilldownReportDefinition : ReportDefinition
    {
        protected override QueryDefinition CreateQueryDefinition(NameValueCollection filters)
        {
            ////Remove the Section_ID filter which is passed 
            ////and add new Section_ID filter which is passed in ReportDefinition.
            //if (!string.IsNullOrEmpty(filters["Section_ID"]))
            //{
            //    if (!String.IsNullOrEmpty(FDDRepSectionID))
            //    {
            //        filters.Remove("Section_ID");
            //        filters.Add("Section_ID", FDDRepSectionID);
            //    }
            //}
            //else //If all option is selected
            //{
            //    filters.Add("Section_ID", FDDRepSectionID);
            //}
            return new DrilldownQueryDefinition(this.EntityTypeName, filters);
        }
    }

}

