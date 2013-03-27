using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pinsonault.Data;
using Pinsonault.Data.Reports;
using Pinsonault.Application;
using PathfinderModel;
using System.Collections.Specialized;
using System.Web;


namespace Pinsonault.Application.Dey
{
    public class RestrictionsReportQueryDefinition : UserRequiredQueryDefinition
    {
        public static string SelectFields { 
            get 
            { 
                //return "Geography_ID, Section_ID, Section_Name, Drug_ID, Drug_Name"; 
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["Section_ID"]) || (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["_data0"]) && !bAllSectionsSelected))
                    return "Geography_ID, Section_ID, Section_Name, Drug_ID, Drug_Name";
                else
                    return "Geography_ID, Drug_ID, Drug_Name";
            } 
        }
        public static string SelectFieldsGrid { 
            get 
            { 
                //return "Geography_ID, Section_ID, Section_Name, Drug_ID, Drug_Name"; 
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["Section_ID"]) || (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["_data0"]) && !bAllSectionsSelected))
                    return "Geography_ID, Section_ID, Section_Name, Drug_ID, Drug_Name";
                else
                    return "Geography_ID, Drug_ID, Drug_Name";
            } 
        }
        public static string AggrFields { get { return "Sum(P1_Lives), Sum(P2_Lives), Sum(P3_Lives), Sum(P4_Lives)"; } }

        static string _expr = null;
        public static string ExprFields
        {
            get
            {
                if (string.IsNullOrEmpty(_expr))
                {
                    //string format = "{0}(Sum(T{1}_Lives)/Cast(Sum(T1_Lives)+Sum(T2_Lives)+Sum(T3_Lives)+Sum(T4_Lives)+Sum(T5_Lives)+Sum(T6_Lives)+Sum(T7_Lives)+Sum(T9_Lives) as System.Decimal)*100) as T{1}";
                    //string formatP = "{0}(Sum(P{1}_Lives)/Sqlserver.nullif(Cast(Sum(P1_Lives)+Sum(P2_Lives)+Sum(P3_Lives)+Sum(P4_Lives)+Sum(P5_Lives)+Sum(P6_Lives)+Sum(P7_Lives)as System.Decimal), 0) *100) as P{1}";
                    //string formatM = "{0}(Sum(M{1}_Lives)/Sqlserver.nullif(Cast(Sum(M1_Lives)+Sum(M2_Lives)+Sum(M3_Lives)+Sum(M4_Lives)+Sum(M5_Lives)+Sum(M6_Lives)+Sum(M7_Lives)as System.Decimal), 0) *100) as M{1}";
                    string formatP = "{0}case when (Sum(P1_Lives)+Sum(P2_Lives)+Sum(P3_Lives)+Sum(P4_Lives)) = 0 then 0 else Sum(P{1}_Lives)/Cast(Sum(P1_Lives)+Sum(P2_Lives)+Sum(P3_Lives)+Sum(P4_Lives)as System.Decimal) *100 end as P{1}";

                    //List of all possible criterias
                    StringBuilder sbP = new StringBuilder();
                    sbP.AppendFormat(formatP, "", 1)
                        .AppendFormat(formatP, ",", 2)
                        .AppendFormat(formatP, ",", 3)
                        .AppendFormat(formatP, ",", 4);

                    _expr = sbP.ToString() + ", MAX(Geography_Name) as Geography_Name";
                }
                return _expr;
            }
        }

        public RestrictionsReportQueryDefinition(string EntityTypeName, NameValueCollection queryString)
            : base(EntityTypeName, queryString)
        {
        }

        public override string Select { get { return SelectFields; } }
        public override string Aggregate
        {
            get { return AggrFields; }
        }
        public override string Expressions { get { return ExprFields; } }
        public override string Sort
        {
            get { return "Drug_Name"; }
        }
    }

    public class RestrictionsReportChartQueryDefinition : UserRequiredQueryDefinition
    {
        public static string SelectFields { 
            get 
            { 
                //return "Geography_ID, Section_ID, Section_Name, Drug_ID, Drug_Name"; 
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["Section_ID"]) || (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["_data0"]) && !bAllSectionsSelected))
                    return "Geography_ID, Section_ID, Section_Name, Drug_ID, Drug_Name";
                else
                    return "Geography_ID, Drug_ID, Drug_Name";
            } 
        }
        public static string SelectFieldsGrid 
        { 
            get 
            { 
                //return "Geography_ID, Section_ID, Section_Name, Drug_ID, Drug_Name";
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["Section_ID"]) || (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["_data0"]) && !bAllSectionsSelected))
                    return "Geography_ID, Section_ID, Section_Name, Drug_ID, Drug_Name";
                else
                    return "Geography_ID, Drug_ID, Drug_Name";
            } 
        }
        public static string AggrFields { get { return "Sum(P1_Lives), Sum(P2_Lives), Sum(P3_Lives), Sum(P4_Lives)"; } }

        static string _expr = null;
        public static string ExprFields
        {
            get
            {
                if (string.IsNullOrEmpty(_expr))
                {
                    //string format = "{0}(Sum(T{1}_Lives)/Cast(Sum(T1_Lives)+Sum(T2_Lives)+Sum(T3_Lives)+Sum(T4_Lives)+Sum(T5_Lives)+Sum(T6_Lives)+Sum(T7_Lives)+Sum(T9_Lives) as System.Decimal)*100) as T{1}";
                    //string formatP = "{0}(Sum(P{1}_Lives)/Sqlserver.nullif(Cast(Sum(P1_Lives)+Sum(P2_Lives)+Sum(P3_Lives)+Sum(P4_Lives)+Sum(P5_Lives)+Sum(P6_Lives)+Sum(P7_Lives)as System.Decimal), 0) *100) as P{1}";
                    //string formatM = "{0}(Sum(M{1}_Lives)/Sqlserver.nullif(Cast(Sum(M1_Lives)+Sum(M2_Lives)+Sum(M3_Lives)+Sum(M4_Lives)+Sum(M5_Lives)+Sum(M6_Lives)+Sum(M7_Lives)as System.Decimal), 0) *100) as M{1}";
                    string formatP = "{0}case when (Sum(P1_Lives)+Sum(P2_Lives)+Sum(P3_Lives)+Sum(P4_Lives)) = 0 then 0 else Sum(P{1}_Lives)/Cast(Sum(P1_Lives)+Sum(P2_Lives)+Sum(P3_Lives)+Sum(P4_Lives)as System.Decimal) *100 end as P{1}";

                    //List of all possible criterias
                    StringBuilder sbP = new StringBuilder();
                    sbP.AppendFormat(formatP, "", 1)
                        .AppendFormat(formatP, ",", 2)
                        .AppendFormat(formatP, ",", 3)
                        .AppendFormat(formatP, ",", 4);


                    _expr = sbP.ToString() + ", MAX(Geography_Name) as Geography_Name";
                }
                return _expr;
            }
        }

        public RestrictionsReportChartQueryDefinition(string EntityTypeName, NameValueCollection queryString)
            : base(EntityTypeName, queryString)
        {
        }

        public override string Select { get { return SelectFields; } }
        public override string Aggregate
        {
            get { return AggrFields; }
        }
        public override string Expressions { get { return ExprFields; } }
        public override string Sort
        {
            get { return "Drug_Name"; }
        }
    }

    public class DrilldownQueryDefinition : UserRequiredQueryDefinition
    {
        public DrilldownQueryDefinition(string EntityTypeName, NameValueCollection queryString)
            : base(EntityTypeName, queryString)
        {
        }
    }
}
