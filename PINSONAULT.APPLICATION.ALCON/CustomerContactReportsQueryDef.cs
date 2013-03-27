using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pinsonault.Data;
using PathfinderClientModel;
using System.Collections.Specialized;

namespace Pinsonault.Application.Alcon
{
    public class CustomerContactReportsQueryDef : QueryDefinition
    {
        public CustomerContactReportsQueryDef(string EntityTypeName, NameValueCollection queryString) : base(EntityTypeName, queryString) { }
        public CustomerContactReportsQueryDef(NameValueCollection queryString) : base(queryString) { }
        
        public override string Aggregate
        {
            get { return "Count(User_ID), PercentCount(User_ID)"; }
        }

        protected override void Preprocess(NameValueCollection queryString)
        {
            base.Preprocess(queryString);
        }


        IEnumerable<int> lookupIDsToInt(string lookupIDs)
        {
            return lookupIDs.Replace(" ", "").Split(',').Select(s => Convert.ToInt32(s));
        }
    }   

    public class MeetingActivityCustomerContactReportQueryDef : CustomerContactReportsQueryDef
    {
        public MeetingActivityCustomerContactReportQueryDef(string EntityTypeName, NameValueCollection queryString) : base(EntityTypeName, queryString) { }
        public MeetingActivityCustomerContactReportQueryDef(NameValueCollection queryString) : base(queryString) { }
    
        public override string Select
        {
            get { return "Meeting_Activity_ID, Meeting_Activity_Name, Products_Discussed_ID, Drug_Name"; }
        }

        public override string Sort
        {
            get { return "Meeting_Activity_Name"; }
        }
    }

    public class MeetingTypeCustomerContactReportQueryDef : CustomerContactReportsQueryDef 
    {
        public MeetingTypeCustomerContactReportQueryDef(string EntityTypeName, NameValueCollection queryString) : base(EntityTypeName, queryString) { }
        public MeetingTypeCustomerContactReportQueryDef(NameValueCollection queryString) : base(queryString) { }
    
        public override string Select
        {
            get { return "Meeting_Type_ID, Meeting_Type_Name, Products_Discussed_ID, Drug_Name"; }
        }

        public override string Sort
        {
            get { return "Meeting_Type_Name"; }
        }
    }

    public class ProductsDiscussedCustomerContactReportQueryDef : CustomerContactReportsQueryDef
    {
        public ProductsDiscussedCustomerContactReportQueryDef(string EntityTypeName, NameValueCollection queryString) : base(EntityTypeName, queryString) { }
        public ProductsDiscussedCustomerContactReportQueryDef(NameValueCollection queryString) : base(queryString) { }     

        public override string Select
        {
            get { return "Drug_Name, Products_Discussed_ID"; }
        }

        public override string Sort
        {
            get { return "Drug_Name"; }
        }
    }
}
