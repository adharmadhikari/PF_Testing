using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Collections.Specialized;
using Pinsonault.Data;
using Pinsonault.Data.Reports;

namespace Pinsonault.Data.Reports
{
    /// <summary>
    /// Summary description for QueryDefinition
    /// </summary>

    public class UserRequiredQueryDefinition : QueryDefinition
    {
        public UserRequiredQueryDefinition(string EntityTypeName, NameValueCollection queryString)
            : base(EntityTypeName, queryString)
        {
        }

        bool OtherRestrictions { get; set; }
        public static bool bAllSectionsSelected { get; set; }

        protected override void Preprocess(NameValueCollection queryString)
        {
            OtherRestrictions = !string.IsNullOrEmpty(queryString["_others"]);
            bAllSectionsSelected = string.IsNullOrEmpty(queryString["Section_ID"]);

            base.Preprocess(queryString);
        }        
    }
}

namespace Pinsonault.Application.StandardReports 
{
    public class FormularyStatusQueryDefinition : UserRequiredQueryDefinition
    {
        public FormularyStatusQueryDefinition(string EntityTypeName, NameValueCollection queryString)
            : base(EntityTypeName, queryString)
        {
            
        }

        public string CoveredWithRestrictionsColumnName { get; private set; }

        public string PharmacyLivesColumnName { get; private set; }

        public static IList<string> GetRestrictionsFromRequest(NameValueCollection queryString)
        {
            List<string> restrictions = new List<string>();

            if ( !string.IsNullOrEmpty(queryString["PA"]) )
                restrictions.Add("PA");
            if ( !string.IsNullOrEmpty(queryString["QL"]) )
                restrictions.Add("QL");
            if ( !string.IsNullOrEmpty(queryString["ST"]) )
                restrictions.Add("ST");

            return restrictions;
        }

        protected override void Preprocess(NameValueCollection queryString)
        {
            IList<string> restrictions = GetRestrictionsFromRequest(queryString);
            
            if ( restrictions.Count > 0 )
                CoveredWithRestrictionsColumnName = string.Format("{0}_Lives", string.Join("_", restrictions.ToArray()));
            else
                CoveredWithRestrictionsColumnName = ""; //no restrictions selected

            //Add restrictions to options filter to display in export header
            queryString.Add("__PA", queryString["PA"]);
            queryString.Add("__QL", queryString["QL"]);
            queryString.Add("__ST", queryString["ST"]);

            queryString.Remove("PA");
            queryString.Remove("QL");
            queryString.Remove("ST");
           
            PharmacyLivesColumnName = "Formulary_Lives";          

            //if ( queryString["Section_ID"] == "17" ) 
                queryString.Add("__aggr", "Sum(Formulary_Lives), Sum(F1_Lives), Sum(F3_Lives)");                
            //else
            //    queryString.Add("__aggr", "Sum(Covered_Lives), Sum(Formulary_Lives), Sum(F1_Lives), Sum(F3_Lives)");                

            string expr = "";

            // to force Unselected Lives 0% when all restrictions are selected
            if (string.IsNullOrEmpty(CoveredWithRestrictionsColumnName))
                expr = "0 as F2_Lives, MAX(Geography_Name) as Geography_Name, Sum(Formulary_Lives) - (Cast((Sum(F1_Lives) + 0 + Sum(F3_Lives)) as System.Decimal)) as Other_Lives, Sum(F1_Lives)/Cast(Sum(Formulary_Lives) as System.Decimal)*100 as F1 , 0 as F2 , Sum(F3_Lives)/Cast(Sum(Formulary_Lives) as System.Decimal)*100 as F3 , (Sum(Formulary_Lives) - (Sum(F1_Lives) + 0 + Sum(F3_Lives)))/Cast(Sum(Formulary_Lives) as System.Decimal)*100 as F4 ,(Sum(Formulary_Lives)-0 - Sum(F1_Lives) - Sum(F3_Lives))/Cast(Sum(Formulary_Lives) as System.Decimal)*100 as F5 , Sum(Formulary_Lives)-0 - Sum(F1_Lives) - Sum(F3_Lives) AS F5_Lives";
            else if (string.Compare(CoveredWithRestrictionsColumnName, "PA_QL_ST_Lives", true) == 0)
                expr = string.Format("Sum({0}) as F2_Lives, MAX(Geography_Name) as Geography_Name, 0 as Other_Lives, Sum(F1_Lives)/Cast(Sum(Formulary_Lives) as System.Decimal)*100 as F1 , Sum({0})/Cast(Sum(Formulary_Lives) as System.Decimal)*100 as F2 , Sum(F3_Lives)/Cast(Sum(Formulary_Lives) as System.Decimal)*100 as F3 , 0 as F4 ,0 as F5 , Sum(Formulary_Lives)-Sum({0}) - Sum(F1_Lives) - Sum(F3_Lives) AS F5_Lives", CoveredWithRestrictionsColumnName);
            else
                expr = string.Format("Sum({0}) as F2_Lives, MAX(Geography_Name) as Geography_Name, Sum(Formulary_Lives) - (Cast((Sum(F1_Lives) + Sum({0}) + Sum(F3_Lives)) as System.Decimal)) as Other_Lives, Sum(F1_Lives)/Cast(Sum(Formulary_Lives) as System.Decimal)*100 as F1 , Sum({0})/Cast(Sum(Formulary_Lives) as System.Decimal)*100 as F2 , Sum(F3_Lives)/Cast(Sum(Formulary_Lives) as System.Decimal)*100 as F3 , (Sum(Formulary_Lives) - (Sum(F1_Lives) + Sum({0}) + Sum(F3_Lives)))/Cast(Sum(Formulary_Lives) as System.Decimal)*100 as F4 ,(Sum(Formulary_Lives)-Sum({0}) - Sum(F1_Lives) - Sum(F3_Lives))/Cast(Sum(Formulary_Lives) as System.Decimal)*100 as F5 , Sum(Formulary_Lives)-Sum({0}) - Sum(F1_Lives) - Sum(F3_Lives) AS F5_Lives", CoveredWithRestrictionsColumnName);
            
            queryString.Add("__expr", expr);

            //queryString.Add("__expr", " Sum(entity." + CoveredWithRestrictionsColumnName + ") as F2_Lives,(Sum(entity.F1_Lives)/Cast((Sum(entity.F1_Lives) + Sum(entity." + CoveredWithRestrictionsColumnName + ") + Sum(entity.F3_Lives)) as System.Decimal)*100) as F1,(Sum(entity." + CoveredWithRestrictionsColumnName + ")/Cast((Sum(entity.F1_Lives) + Sum(entity." + CoveredWithRestrictionsColumnName + ") + Sum(entity.F3_Lives)) as System.Decimal)*100) as F2, (Sum(entity.F3_Lives)/Cast((Sum(entity.F1_Lives) + Sum(entity." + CoveredWithRestrictionsColumnName + ") + Sum(entity.F3_Lives)) as System.Decimal)*100) as F3");
            base.Preprocess(queryString);
        }

        public override string Select
        {
            //if section selected is 0 then include all.
           
            get 
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["Section_ID"]) || (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["_data0"]) && !bAllSectionsSelected))
                    return "Geography_ID, Section_ID, Section_Name, Drug_ID, Drug_Name";
                else
                    return "Geography_ID, Drug_ID, Drug_Name";
            }
        }


        public override string Sort
        {
            get { return "Drug_Name"; }
        }

    }

    public class TierCoverageQueryDefinition : UserRequiredQueryDefinition
    {
        public static string SelectFields 
        { 
            get 
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["Section_ID"]) || (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["_data0"]) && !bAllSectionsSelected))
                    return "Geography_ID, Section_ID, Section_Name, Drug_ID, Drug_Name"; 
                else
                    return "Geography_ID, Drug_ID, Drug_Name"; 
            } 
        }
       // public static string AggrFields { get { return "Sum(Pharmacy_Lives), Sum(T1_Lives), Sum(T2_Lives), Sum(T3_Lives), Sum(T4_Lives), Sum(T5_Lives), Sum(T6_Lives), Sum(T7_Lives), Sum(T9_Lives)"; } }
        public static string AggrFields { get { return "Sum(Formulary_Lives), Sum(T1_Lives), Sum(T2_Lives), Sum(T3_Lives), Sum(T4_Lives), Sum(T5_Lives), Sum(T6_Lives), Sum(T7_Lives),Sum(T20_Lives),Sum(T21_Lives),Sum(T22_Lives), Sum(T0_Lives)"; } }

        static string _expr = null;
        public static string ExprFields
        {
            get
            {
                if (string.IsNullOrEmpty(_expr))
                {
                    //string format = "{0}(Sum(T{1}_Lives)/Cast(Sum(T1_Lives)+Sum(T2_Lives)+Sum(T3_Lives)+Sum(T4_Lives)+Sum(T5_Lives)+Sum(T6_Lives)+Sum(T7_Lives)+Sum(T9_Lives) as System.Decimal)*100) as T{1}";
                    string format = "{0}(Sum(T{1}_Lives)/Cast(Sum(T1_Lives)+Sum(T2_Lives)+Sum(T3_Lives)+Sum(T4_Lives)+Sum(T5_Lives)+Sum(T6_Lives)+Sum(T7_Lives)+Sum(T20_Lives)+Sum(T21_Lives)+Sum(T22_Lives)+Sum(T0_Lives) as System.Decimal)*100) as T{1}";
                   
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat(format, "", 1)          //1T
                        .AppendFormat(format, ",", 2)       //2T
                        .AppendFormat(format, ",", 3)       //3T
                        .AppendFormat(format, ",", 4)       //4T    
                        .AppendFormat(format, ",", 5)       //5T previously S
                        .AppendFormat(format, ",", 6)       //6T previously M
                        .AppendFormat(format, ",", 7)       //NC
                        .AppendFormat(format, ",", 20)      //M.
                        .AppendFormat(format, ",", 21)     //S
                         .AppendFormat(format, ",", 22)     //PC
                        .AppendFormat(format, ",", 0);     //0T


                    _expr = sb.ToString() + ", MAX(Geography_Name) as Geography_Name";
                }
                return _expr;
            }
        }

        public TierCoverageQueryDefinition(string EntityTypeName, NameValueCollection queryString)
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

    #region formulary coverage summary
    public class FormularyCoverageQueryDefinition : UserRequiredQueryDefinition
    {
        public static string SelectFields {
            get 
        {
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["Section_ID"]) || (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["_data0"]) && !bAllSectionsSelected))
                return "Geography_ID, Section_ID, Section_Name, Drug_ID, Drug_Name";
            else
                return "Geography_ID, Drug_ID, Drug_Name";
        } 
        }

        public static string AggrFields { get { return "Sum(Formulary_Lives), Sum(P), Sum(NP), Sum(NC), Sum(M), Sum(SP), Sum(NM)"; } } //exclude NA

        static string _expr = null;
        public static string ExprFields
        {
            get
            {
                if (string.IsNullOrEmpty(_expr))
                {                    
                    string format = "{0}(Sum({1})/Cast(Sum(P)+ Sum(NP)+ Sum(NC)+ Sum(M)+ Sum(SP) + Sum(NM) as System.Decimal)*100) as {1}_Percent";

                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat(format, "", "P")          //Preferred
                        .AppendFormat(format, ",", "NP")       //Non Preferred
                        .AppendFormat(format, ",", "NC")       //Not Covered
                        .AppendFormat(format, ",", "M")       //Medical    
                        .AppendFormat(format, ",", "SP")      //specialty
                         .AppendFormat(format, ",", "NM");    //Not Managed

                    _expr = sb.ToString() + ", MAX(Geography_Name) as Geography_Name";
                }
                return _expr;
            }
        }

        public FormularyCoverageQueryDefinition(string EntityTypeName, NameValueCollection queryString)
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
    #endregion

    public class DrilldownQueryDefinition : UserRequiredQueryDefinition
    {
        public DrilldownQueryDefinition(string EntityTypeName, NameValueCollection queryString)
            : base(EntityTypeName, queryString)
        {
        }
    }
}
