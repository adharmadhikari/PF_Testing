using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pinsonault.Data.Reports;
using System.Collections.Specialized;
using Pinsonault.Data;

namespace Pinsonault.Application.CustomerContactReports
{
    //GD
    public class CCRProductReportDefinition : ReportDefinition
    {
        public int ProductIndex { get; set; }
        public bool IsNational { get; set; }

        protected override QueryDefinition CreateQueryDefinition(NameValueCollection filters)
        {
            NameValueCollection newFilters = new NameValueCollection(filters);

            //Add aggregates to NameValueCollection
            newFilters.Add("__aggr", "Count(User_ID), PercentCount(User_ID)");

            if (ReportKey == "meetingactivity")
                newFilters.Add("__select", "Meeting_Activity_ID, Meeting_Activity_Name");
            else if (ReportKey == "meetingtype")
                newFilters.Add("__select", "Meeting_Type_ID, Meeting_Type_Name");

            newFilters.Remove("Products_Discussed_ID");

            string prod = filters["Products_Discussed_ID"];

            ContactReportProvider cr = new ContactReportProvider();

            string[] productId = cr.GetProductsDiscussedID(prod);

            if (productId.Length > ProductIndex)
            {
                newFilters.Add("Products_Discussed_ID", productId[ProductIndex]);

                int prodID = Convert.ToInt32(productId[ProductIndex]);

                if (!IsNational)
                {
                    using (PathfinderClientModel.PathfinderClientEntities clientContext = new PathfinderClientModel.PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
                    {
                        var displayDrugNameSet = (from drugName in clientContext.ContactReportDataSummarySet
                                                  where drugName.Products_Discussed_ID == prodID
                                                  select drugName);

                        if (displayDrugNameSet.Count() >0)
                            SectionTitle = displayDrugNameSet.FirstOrDefault().Drug_Name;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(newFilters["Is_National"])) //Check to see if report is National or Regional, if national, this comparison report is not needed
                    {
                        //Remove GeoID to get National data
                        newFilters.Remove("Geography_ID");
                        newFilters.Add("Is_National", "1");

                        using (PathfinderClientModel.PathfinderClientEntities clientContext = new PathfinderClientModel.PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
                        {
                            var displayDrugNameSet = (from drugName in clientContext.ContactReportDataSet
                                                      where drugName.Products_Discussed_ID == prodID
                                                      select drugName);

                            if(displayDrugNameSet.Count() >0)
                                SectionTitle = string.Format("{0} - National", displayDrugNameSet.FirstOrDefault().Drug_Name);
                        }
                    }
                    else
                        Visible = false;
                }
            }
            else
                Visible = false;

            return base.CreateQueryDefinition(newFilters);
        }
    }

    public class ProductsDiscussedYTDReportDefinition : ReportDefinition
    {
        protected override QueryDefinition CreateQueryDefinition(NameValueCollection filters)
        {
            NameValueCollection newFilters = new NameValueCollection(filters);

            //Add aggregates to NameValueCollection
            newFilters.Add("__aggr", "Count(User_ID), PercentCount(User_ID)");
            newFilters.Add("__select", "Drug_Name, Products_Discussed_ID");

            DateTime yearBeginDt = Convert.ToDateTime(string.Format("01/01/{0}", DateTime.Now.Year));
            DateTime today = Convert.ToDateTime(string.Format("{0}/{1}/{2}", DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Year));
            string[] contactDate = filters["Contact_Date"].Split(',');

            newFilters["Contact_Date"] = string.Format("{0},{1}", yearBeginDt.ToShortDateString(), today.ToShortDateString());

            SectionTitle = string.Format("{0} to {1}", yearBeginDt.ToShortDateString().Replace('/', '-'), today.ToShortDateString().Replace('/', '-'));

            return base.CreateQueryDefinition(newFilters);
        }
    }

    public class ProductsDiscussedTimeFrameReportDefinition : ReportDefinition
    {
        protected override QueryDefinition CreateQueryDefinition(NameValueCollection filters)
        {
            NameValueCollection newFilters = new NameValueCollection(filters);

            //Add aggregates to NameValueCollection
            newFilters.Add("__aggr", "Count(User_ID), PercentCount(User_ID)");
            newFilters.Add("__select", "Drug_Name, Products_Discussed_ID");

            DateTime yearBeginDt = Convert.ToDateTime(string.Format("01/01/{0}", DateTime.Now.Year));
            DateTime today = Convert.ToDateTime(string.Format("{0}/{1}/{2}", DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Year));
            string[] contactDate = filters["Contact_Date"].Split(',');

            //Display this report only if date range is not YTD
            if ((DateTime.Compare(yearBeginDt, Convert.ToDateTime(contactDate[0])) != 0) || (DateTime.Compare(today, Convert.ToDateTime(contactDate[1])) != 0))
                SectionTitle = string.Format("{0} to {1}", contactDate[0].Replace('/', '-'), contactDate[1].Replace('/', '-'));
            else
                Visible = false;

            return base.CreateQueryDefinition(newFilters);
        }
    }

}
