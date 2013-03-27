using System.Collections.Specialized;
using Pinsonault.Data.Reports;
using Pinsonault.Data;
using PathfinderClientModel;
using System.Linq;
using System.Data.Objects;
using System;

namespace Pinsonault.Application.Alcon.CustomerContactReports
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

            if (ReportKey == "meetingtype")
                newFilters.Add("__select", "Meeting_Type_ID, Meeting_Type_Name");

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
                using (Pinsonault.Application.Alcon.PathfinderAlconEntities alconContext = new Pinsonault.Application.Alcon.PathfinderAlconEntities())
                {
                    var list = (from CRID in alconContext.ContactReportDrillDownFilterSet.Where(strwhere)
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
                using (Pinsonault.Application.Alcon.PathfinderAlconEntities rContext = new Pinsonault.Application.Alcon.PathfinderAlconEntities())
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
                using (Pinsonault.Application.Alcon.PathfinderAlconEntities rContext = new Pinsonault.Application.Alcon.PathfinderAlconEntities())
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
namespace Pinsonault.Application.Alcon.ActivityReporting
{
    public class ActivityReportingDefinition : ReportDefinition
    {
        private NameValueCollection newFilters;

        protected override QueryDefinition CreateQueryDefinition(NameValueCollection filters)
        {
            newFilters = new NameValueCollection(filters);
            return new ActivityReportingQueryDef(this.EntityTypeName, filters);
        }
    }
}

namespace Pinsonault.Application.Alcon.SellSheetReporting
{
    public class SellSheetReportDefinition : ReportDefinition
    {       
        protected override QueryDefinition CreateQueryDefinition(NameValueCollection filters)
        {
            NameValueCollection newFilters = new NameValueCollection(filters);
            
            //get SellSheetIDs 
            string strwhere = string.Empty;
            int[] SellSheetIDs = null;
            if (filters.GetValues("Drug_ID") != null)
            {
                newFilters.Remove("Drug_ID");
                string Drug_IDs = filters["Drug_ID"].ToString();
                strwhere = " it.Drug_ID in {" + Drug_IDs + "}";
            }

            if (filters.GetValues("Thera_ID") != null)
            {
                newFilters.Remove("Thera_ID");

                string Thera_IDs = filters["Thera_ID"].ToString();
                if (string.IsNullOrEmpty(strwhere))
                    strwhere = " it.Thera_ID in {" + Thera_IDs + "}";
                else
                    strwhere = strwhere + " and it.Thera_ID in {" + Thera_IDs + "}";
            }

            if (filters.GetValues("Section_ID") != null)
            {
                newFilters.Remove("Section_ID");

                string Section_IDs = filters["Section_ID"].ToString();
                if (string.IsNullOrEmpty(strwhere))
                    strwhere = " it.Section_ID in {" + Section_IDs + "}";
                else
                    strwhere = strwhere + " and it.Section_ID in {" + Section_IDs + "}";
            }
            if (filters.GetValues("Plan_ID") != null)
            {
                newFilters.Remove("Plan_ID");

                string Plan_IDs = filters["Plan_ID"].ToString();
                if (string.IsNullOrEmpty(strwhere))
                    strwhere = " it.Plan_ID in {" + Plan_IDs + "}";
                else
                    strwhere = strwhere + " and it.Plan_ID in {" + Plan_IDs + "}";
            }
            if (filters.GetValues("Title_ID") != null)
            {
                newFilters.Remove("Title_ID");

                string Title_ID = filters["Title_ID"].ToString();
                if (string.IsNullOrEmpty(strwhere))
                    strwhere = " it.Title = '" + Title_ID + "'";
                else
                    strwhere = strwhere + " and it.Title = '" + Title_ID + "'";
            }
            //filter parent territory id
            if (filters.GetValues("Geography_ID") != null)
            {
                newFilters.Remove("Geography_ID");

                string Geography_ID = filters["Geography_ID"].ToString();
                if (string.IsNullOrEmpty(strwhere))
                    strwhere = " (it.State_ID  = '" + Geography_ID + "')";
                else
                    strwhere = strwhere + " and (it.State_ID  = '" + Geography_ID + "')";
            }

            if (filters.GetValues("User_ID") != null)
            {
                newFilters.Remove("User_ID");

                string User_ID = filters["User_ID"].ToString();
                User_ID = User_ID.Replace('%',' ').Trim();
                if (string.IsNullOrEmpty(strwhere))
                    strwhere = " (it.User_ID  = " + User_ID + ")";
                else
                    strwhere = strwhere + " and (it.User_ID  = " + User_ID + ")";
            }
            if (filters.GetValues("Status_ID") != null)
            {
                newFilters.Remove("Status_ID");

                string Status_ID = filters["Status_ID"].ToString();
                if (string.IsNullOrEmpty(strwhere))
                    strwhere = " (it.Status_ID  = " + Status_ID + ")";
                else
                    strwhere = strwhere + " and (it.Status_ID  = " + Status_ID + ")";
            }

            if (!string.IsNullOrEmpty(strwhere))
            {
                using (PathfinderAlconEntities alconentity = new PathfinderAlconEntities())
                {

                    string CreatedDt = filters["Created_DT"].ToString();//HttpContext.Current.Request.QueryString["$filter"].Substring(HttpContext.Current.Request.QueryString["$filter"].IndexOf("Created_DT"));
                    string fromDt = CreatedDt.Substring(0, CreatedDt.IndexOf(','));
                    string ToDt = CreatedDt.Substring(CreatedDt.IndexOf(',')+1);
                    DateTime dtFrom = Convert.ToDateTime(fromDt);
                    DateTime dtTo = Convert.ToDateTime(ToDt);
                    int maxRecords = 500;  //temp fix to avoid Stack Overflow

                    var list = (from SSID in alconentity.SellSheetReportFilterSet.Where(strwhere)
                                .Where(SSID => SSID.Created_DT >= dtFrom).Where(SSID => SSID.Created_DT <= dtTo)
                                orderby SSID.Sell_Sheet_ID descending 
                                select SSID.Sell_Sheet_ID).ToList().Distinct().Take(maxRecords);

                    if (list.Count() > 0)
                    {
                        SellSheetIDs = list.ToArray();
                        //change SellSheetIDs array in to csv and add in new filters
                        string strlist = ConvertArrayToString(SellSheetIDs);
                        newFilters.Add("Sell_Sheet_ID", strlist);
                    }
                    else
                        newFilters.Add("Sell_Sheet_ID", "0");
                }

               

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
}

