using System.Collections.Specialized;
using Pinsonault.Data.Reports;
using Pinsonault.Data;
using PathfinderClientModel;
using System.Linq;
using System.Data.Objects;
using System;

namespace Pinsonault.Application.CSL.CustomerContactReports
{
    public class CCRReportDefinition : ReportDefinition
    { 
        public bool IsNational { get; set; }
        public int ProductIndex { get; set; }
        public ObjectContext DataContext { get; set; }

        protected virtual QueryDefinition CreateQueryDefinition(string EntityTypeName, NameValueCollection filters)
        {
            Visible = true; //drilldown force visible
            return new CustomerContactReportsQueryDef(EntityTypeName, filters);
        }

        protected override QueryDefinition CreateQueryDefinition(NameValueCollection filters)
        {
            NameValueCollection newFilters = new NameValueCollection(filters);

            if (!string.IsNullOrEmpty(newFilters["Is_National"]))
            //{
            //    Visible = !IsNational;
            //}
            //else
                Visible = IsNational;

            string productID = filters["Products_Discussed_ID"];
            if ( !string.IsNullOrEmpty(productID) )
            {
                string[] vals = productID.Replace(" ", "").Split(',');
                if (vals.Length > 0 && vals.Length > ProductIndex)
                {
                    string val = vals[ProductIndex];
                    newFilters["Products_Discussed_ID"] = val;

                    if (DataContext != null)
                    {
                        int id = 0;
                        if (int.TryParse(val, out id))
                        {
                            SectionTitle = ((PathfinderClientEntities)DataContext).LkpProductsDiscussedSet.Where(p => p.Products_Discussed_ID == id).Select(p => p.Drug_Name).FirstOrDefault();
                            if (IsNational)
                                SectionTitle = string.Format("{0} - National", new string(SectionTitle.ToCharArray().Take(20).ToArray()));
                            else
                                SectionTitle = new string(SectionTitle.ToCharArray().Take(31).ToArray());
                        }
                    }
                }
                else
                    Visible = false;
            }

            if ( IsNational )
            {
                newFilters.Remove("Geography_ID");
                newFilters.Add("Is_National", "1");
            }
            return CreateQueryDefinition(EntityTypeName, newFilters);
        }      
    }

    public class MeetingActivityReportDefinition : CCRReportDefinition
    {
        protected override QueryDefinition CreateQueryDefinition(string EntityTypeName, NameValueCollection filters)
        {
            return new MeetingActivityCustomerContactReportQueryDef(EntityTypeName, filters);
        }
    }

    public class MeetingTypeReportDefinition : CCRReportDefinition
    {
        protected override QueryDefinition CreateQueryDefinition(string EntityTypeName, NameValueCollection filters)
        {
            return new MeetingTypeCustomerContactReportQueryDef(EntityTypeName, filters);
        }
    }
  
    public class ProductsDiscussedReportDefinition : CCRReportDefinition
    {
        protected override QueryDefinition CreateQueryDefinition(string EntityTypeName, NameValueCollection filters)
        {
            Visible = true;

            string contactDate = filters["Contact_Date"];
            if (!string.IsNullOrEmpty(contactDate))
            {
                string[] dates = contactDate.Replace("/", "-").Split(',');

                SectionTitle = string.Format("{0} to {1}", dates);
            }
            return new ProductsDiscussedCustomerContactReportQueryDef(EntityTypeName, filters);
        }
    }

    public class ProductsDiscussedYTDReportDefinition : CCRReportDefinition
    {
        protected override QueryDefinition CreateQueryDefinition(string EntityTypeName, NameValueCollection filters)
        {
            Visible = true;
            DateTime yearBeginDt = Convert.ToDateTime(string.Format("01/01/{0}", DateTime.Now.Year));
            DateTime today = Convert.ToDateTime(string.Format("{0}/{1}/{2}", DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Year));
            
            filters["Contact_Date"] = string.Format("{0},{1}", yearBeginDt.ToShortDateString(), today.ToShortDateString());
            
            return new ProductsDiscussedCustomerContactReportQueryDef(EntityTypeName, filters);
        }
    }
}
