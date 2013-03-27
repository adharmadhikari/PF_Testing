using System.Collections.Specialized;
using Pinsonault.Data.Reports;
using Pinsonault.Data;
using PathfinderClientModel;
using System.Linq;
using System.Data.Objects;
using System;

namespace Pinsonault.Application.Millennium.CustomerContactReports
{
    public class CCRDrilldownReportDefinition : ReportDefinition
    {
        protected override QueryDefinition CreateQueryDefinition(NameValueCollection filters)
        {
            NameValueCollection newFilters = new NameValueCollection(filters);

            //get contact report id
            string strwhere = string.Empty;
            int[] ContactReportIDs = null;
            if (filters.GetValues("Meeting_Activity_ID") != null)
            {
                newFilters.Remove("Meeting_Activity_ID");
                string meetingActivityIds = filters["Meeting_Activity_ID"].ToString();
                strwhere = " it.Meeting_Activity_ID in {" + meetingActivityIds + "}";
            }

            if (filters.GetValues("Products_Discussed_ID") != null)
            {
                if (ReportKey == "customcontactdrilldown")
                    newFilters.Remove("Products_Discussed_ID");

                string productsdiscussedIds = filters["Products_Discussed_ID"].ToString();
                if (string.IsNullOrEmpty(strwhere))
                    strwhere = " it.Products_Discussed_ID in {" + productsdiscussedIds + "}";
                else
                    strwhere = strwhere + " and it.Products_Discussed_ID in {" + productsdiscussedIds + "}";
            }
            if (!string.IsNullOrEmpty(strwhere))
            {
                using (PathfinderClientEntities rContext = new PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
                {
                    var list = (from CRID in rContext.ContactReportDataSet.Where(strwhere)
                                orderby CRID.Contact_Report_ID
                                select CRID.Contact_Report_ID).ToList().Distinct();
                    ContactReportIDs = list.ToArray();
                }

                //change ContactReportIDs array in to csv and add in new filters
                string strlist = ConvertArrayToString(ContactReportIDs);
                newFilters.Add("Contact_Report_ID", strlist);

            }

            return base.CreateQueryDefinition(newFilters);
        }
        private string ConvertArrayToString(int[] intArray)
        {
            string[] stringArray = Array.ConvertAll<int, string>(intArray, new Converter<int, string>(ConvertIntToString));
            string result = string.Join(",", stringArray);
            return result;
        }
        private string ConvertIntToString(int intParameter)
        { return intParameter.ToString(); }
    }

    public class CCRProductReportDefinition : ReportDefinition
    {
        public int ProductIndex { get; set; }
        public bool IsNational { get; set; }

        public string[] GetProductsDiscussedID(string prod)
        {
            if (prod.IndexOf(',') >= 0)
            {
                string[] productID = prod.Split(',');
                return productID;
            }
            else
            {
                string[] productID = { prod };
                return productID;
            }
        }

        protected override QueryDefinition CreateQueryDefinition(NameValueCollection filters)
        {
            NameValueCollection newFilters = new NameValueCollection(filters);

            //Add aggregates to NameValueCollection
            newFilters.Add("__aggr", "Count(User_ID), PercentCount(User_ID)");

            if (ReportKey == "interactiontype")
                newFilters.Add("__select", "Meeting_Type_ID, Meeting_Type_Name");
            if (ReportKey == "meetingtopic")
                newFilters.Add("__select", "Meeting_Activity_ID, Meeting_Activity_Name");

            //get contact report id
            string strwhere = string.Empty;
            int[] ContactReportIDs = null;

            if (filters.GetValues("Products_Discussed_ID") != null)
            {
                //newFilters.Remove("Products_Discussed_ID");
                string productsdiscussedIds = filters["Products_Discussed_ID"].ToString();
                if (string.IsNullOrEmpty(strwhere))
                    strwhere = " it.Products_Discussed_ID in {" + productsdiscussedIds + "}";
                else
                    strwhere = strwhere + " and it.Products_Discussed_ID in {" + productsdiscussedIds + "}";
            }

            if (filters.GetValues("Meeting_Activity_ID") != null)
            {
                newFilters.Remove("Meeting_Activity_ID");
                string meetingActivityIDs = filters["Meeting_Activity_ID"].ToString();
                if (string.IsNullOrEmpty(strwhere))
                    strwhere = " it.Meeting_Activity_ID in {" + meetingActivityIDs + "}";
                else
                    strwhere = strwhere + " and it.Meeting_Activity_ID in {" + meetingActivityIDs + "}";
            }

            if (!string.IsNullOrEmpty(strwhere))
            {
                using (Pinsonault.Application.Millennium.PathfinderMillenniumEntities MillenniumContext = new Pinsonault.Application.Millennium.PathfinderMillenniumEntities())
                {
                    var list = (from CRID in MillenniumContext.ContactReportDrillDownFilterSet.Where(strwhere)
                                orderby CRID.Contact_Report_ID
                                select CRID.Contact_Report_ID).ToList().Distinct();
                    ContactReportIDs = list.ToArray();
                }
                //change ContactReportIDs array in to csv and add in to newFilters

                newFilters.Add("Contact_Report_ID", ConvertArrayToString(ContactReportIDs));
            }

            newFilters.Remove("Products_Discussed_ID");

            string prod = filters["Products_Discussed_ID"];

            string[] productId = GetProductsDiscussedID(prod);

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
                                                  select drugName).FirstOrDefault();
                        if (displayDrugNameSet != null)
                            SectionTitle = displayDrugNameSet.Drug_Name;
                        else
                            Visible = false;
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
                                                      select drugName).FirstOrDefault();

                            SectionTitle = string.Format("{0} - National", displayDrugNameSet.Drug_Name);
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
        private string ConvertArrayToString(int[] intArray)
        {
            string[] stringArray = Array.ConvertAll<int, string>(intArray, new Converter<int, string>(ConvertIntToString));
            string result = string.Join(",", stringArray);
            return result;
        }
        private string ConvertIntToString(int intParameter)
        { return intParameter.ToString(); }
    }

    public class ProductsDiscussedYTDReportDefinition : ReportDefinition
    {
        protected override QueryDefinition CreateQueryDefinition(NameValueCollection filters)
        {
            NameValueCollection newFilters = new NameValueCollection(filters);

            //Add aggregates to NameValueCollection
            newFilters.Add("__aggr", "Count(User_ID), PercentCount(User_ID)");
            newFilters.Add("__select", "Drug_Name, Products_Discussed_ID");

            //get contact report id
            string strwhere = string.Empty;
            int[] ContactReportIDs = null;

            if (filters.GetValues("Meeting_Activity_ID") != null)
            {
                newFilters.Remove("Meeting_Activity_ID");
                string meetingActivityIDs = filters["Meeting_Activity_ID"].ToString();
                if (string.IsNullOrEmpty(strwhere))
                    strwhere = " it.Meeting_Activity_ID in {" + meetingActivityIDs + "}";
                else
                    strwhere = strwhere + " and it.Meeting_Activity_ID in {" + meetingActivityIDs + "}";
            }

            if (filters.GetValues("Products_Discussed_ID") != null)
            {
                //newFilters.Remove("Products_Discussed_ID");
                string productsdiscussedIds = filters["Products_Discussed_ID"].ToString();
                if (string.IsNullOrEmpty(strwhere))
                    strwhere = " it.Products_Discussed_ID in {" + productsdiscussedIds + "}";
                else
                    strwhere = strwhere + " and it.Products_Discussed_ID in {" + productsdiscussedIds + "}";
            }
            if (!string.IsNullOrEmpty(strwhere))
            {
                using (Pinsonault.Application.Millennium.PathfinderMillenniumEntities rContext = new Pinsonault.Application.Millennium.PathfinderMillenniumEntities())
                {
                    var list = (from CRID in rContext.ContactReportDrillDownFilterSet.Where(strwhere)
                                orderby CRID.Contact_Report_ID
                                select CRID.Contact_Report_ID).ToList().Distinct();
                    ContactReportIDs = list.ToArray();
                }
                //change ContactReportIDs array in to csv and add in to newFilters

                newFilters.Add("Contact_Report_ID", ConvertArrayToString(ContactReportIDs));
            }

            DateTime yearBeginDt = Convert.ToDateTime(string.Format("01/01/{0}", DateTime.Now.Year));
            DateTime today = Convert.ToDateTime(string.Format("{0}/{1}/{2}", DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Year));
            string[] contactDate = filters["Contact_Date"].Split(',');

            newFilters["Contact_Date"] = string.Format("{0},{1}", yearBeginDt.ToShortDateString(), today.ToShortDateString());

            SectionTitle = string.Format("{0} to {1}", yearBeginDt.ToShortDateString().Replace('/', '-'), today.ToShortDateString().Replace('/', '-'));

            return base.CreateQueryDefinition(newFilters);
        }

        private string ConvertArrayToString(int[] intArray)
        {
            string[] stringArray = Array.ConvertAll<int, string>(intArray, new Converter<int, string>(ConvertIntToString));
            string result = string.Join(",", stringArray);
            return result;
        }
        private string ConvertIntToString(int intParameter)
        { return intParameter.ToString(); }
    }

    public class ProductsDiscussedTimeFrameReportDefinition : ReportDefinition
    {
        protected override QueryDefinition CreateQueryDefinition(NameValueCollection filters)
        {
            NameValueCollection newFilters = new NameValueCollection(filters);

            //Add aggregates to NameValueCollection
            newFilters.Add("__aggr", "Count(User_ID), PercentCount(User_ID)");
            newFilters.Add("__select", "Drug_Name, Products_Discussed_ID");

            //get contact report id
            string strwhere = string.Empty;
            int[] ContactReportIDs = null;

            if (filters.GetValues("Meeting_Activity_ID") != null)
            {
                newFilters.Remove("Meeting_Activity_ID");
                string meetingActivityIDs = filters["Meeting_Activity_ID"].ToString();
                if (string.IsNullOrEmpty(strwhere))
                    strwhere = " it.Meeting_Activity_ID in {" + meetingActivityIDs + "}";
                else
                    strwhere = strwhere + " and it.Meeting_Activity_ID in {" + meetingActivityIDs + "}";
            }

            if (filters.GetValues("Products_Discussed_ID") != null)
            {
                //newFilters.Remove("Products_Discussed_ID");
                string productsdiscussedIds = filters["Products_Discussed_ID"].ToString();
                if (string.IsNullOrEmpty(strwhere))
                    strwhere = " it.Products_Discussed_ID in {" + productsdiscussedIds + "}";
                else
                    strwhere = strwhere + " and it.Products_Discussed_ID in {" + productsdiscussedIds + "}";
            }
            if (!string.IsNullOrEmpty(strwhere))
            {
                using (Pinsonault.Application.Millennium.PathfinderMillenniumEntities rContext = new Pinsonault.Application.Millennium.PathfinderMillenniumEntities())
                {
                    var list = (from CRID in rContext.ContactReportDrillDownFilterSet.Where(strwhere)
                                orderby CRID.Contact_Report_ID
                                select CRID.Contact_Report_ID).ToList().Distinct();
                    ContactReportIDs = list.ToArray();
                }
                //change ContactReportIDs array in to csv and add in to newFilters

                newFilters.Add("Contact_Report_ID", ConvertArrayToString(ContactReportIDs));
            }

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

        private string ConvertArrayToString(int[] intArray)
        {
            string[] stringArray = Array.ConvertAll<int, string>(intArray, new Converter<int, string>(ConvertIntToString));
            string result = string.Join(",", stringArray);
            return result;
        }
        private string ConvertIntToString(int intParameter)
        { return intParameter.ToString(); }
    }
}
