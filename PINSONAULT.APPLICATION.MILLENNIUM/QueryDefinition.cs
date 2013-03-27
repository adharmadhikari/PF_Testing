using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pinsonault.Data;
using Pinsonault.Data.Reports;
using System.Collections.Specialized;

namespace Pinsonault.Application.Millennium
{
    public class GeographyCoverageFormularyStatusQueryDefinition : UserRequiredQueryDefinition
    {
        public GeographyCoverageFormularyStatusQueryDefinition(string EntityTypeName, NameValueCollection queryString)
            : base(EntityTypeName, queryString)
        {

        }

        public string CoveredWithRestrictionsColumnName { get; private set; }

        public string PharmacyLivesColumnName { get; private set; }

        public static IList<string> GetRestrictionsFromRequest(NameValueCollection queryString)
        {
            List<string> restrictions = new List<string>();

            if (!string.IsNullOrEmpty(queryString["PA"]))
                restrictions.Add("PA");
            if (!string.IsNullOrEmpty(queryString["QL"]))
                restrictions.Add("QL");
            if (!string.IsNullOrEmpty(queryString["ST"]))
                restrictions.Add("ST");

            return restrictions;
        }

        protected override void Preprocess(NameValueCollection queryString)
        {
            IList<string> restrictions = GetRestrictionsFromRequest(queryString);

            if (restrictions.Count > 0)
                CoveredWithRestrictionsColumnName = string.Format("{0}_Lives", string.Join("_", restrictions.ToArray()));
            else
                CoveredWithRestrictionsColumnName = "PA_Lives"; //shouldn't happen anymore but put for safety

            //Add restrictions to options filter to display in export header
            queryString.Add("__PA", queryString["PA"]);
            queryString.Add("__QL", queryString["QL"]);
            queryString.Add("__ST", queryString["ST"]);

            queryString.Remove("PA");
            queryString.Remove("QL");
            queryString.Remove("ST");

            //set column name to get the appropriate pharmacy lives as per channel selection
            //switch(queryString["Section_ID"])
            //{
            //    case "1": //Commercial Payer
            //        PharmacyLivesColumnName = "Commercial_Pharmacy_Lives";
            //        break;
            //    case "17": //Medicare Part D
            //        PharmacyLivesColumnName = "PartD_Lives";
            //        break;
            //    case "4": //PBM ; pharmacy lives for PBM and Commercial are same 
            //        PharmacyLivesColumnName = "Commercial_Pharmacy_Lives";
            //        break;
            //    case "6": //Managed Medicaid
            //        PharmacyLivesColumnName = "Medicaid_Lives";
            //        break;
            //}
            PharmacyLivesColumnName = "Formulary_Lives";

            //if (queryString["Is_Predominant"] == "1")
            //{
            //    queryString.Remove("Is_Predominant");
            //    queryString.Add("Is_Predominant", "true");
            //}

            if (queryString["Section_ID"] == "17")
                queryString.Add("__aggr", "Sum(Formulary_Lives), Sum(F1_Lives), Sum(F3_Lives)");
            else
                queryString.Add("__aggr", "Sum(Covered_Lives), Sum(Formulary_Lives), Sum(F1_Lives), Sum(F3_Lives)");

            // CoveredWithRestrictionsColumnName: dynamic column name so it is included in __expr
            //string expr = string.Format("Sum({0}) as F2_Lives,case when Sum(F1_Lives) + Sum({0}) + Sum(F3_Lives) > 0 then Sum(F1_Lives)/Cast((Sum(F1_Lives) + Sum({0}) + Sum(F3_Lives)) as System.Decimal)*100 else 0 end as F1,case when Sum(F1_Lives) + Sum({0}) + Sum(F3_Lives) > 0 then Sum({0})/Cast((Sum(F1_Lives) + Sum({0}) + Sum(F3_Lives)) as System.Decimal)*100 else 0 end as F2, case when Sum(F1_Lives) + Sum({0}) + Sum(F3_Lives) > 0 then Sum(F3_Lives)/Cast((Sum(F1_Lives) + Sum({0}) + Sum(F3_Lives)) as System.Decimal)*100 else 0 end as F3", CoveredWithRestrictionsColumnName);
            //string expr = string.Format("Sum({0}) as F2_Lives,Sum(Pharmacy_Lives) - (Cast((Sum(F1_Lives) + Sum({0}) + Sum(F3_Lives)) as System.Decimal)) as Other_Lives,Sum(F1_Lives)/Cast(Sum(Pharmacy_Lives) as System.Decimal)*100 as F1, Sum({0})/Cast(Sum(Pharmacy_Lives) as System.Decimal)*100 as F2, Sum(F3_Lives)/Cast(Sum(Pharmacy_Lives) as System.Decimal)*100 as F3, (Sum(Pharmacy_Lives) - (Sum(F1_Lives) + Sum({0}) + Sum(F3_Lives)))/Cast(Sum(Pharmacy_Lives) as System.Decimal)*100 as F4", CoveredWithRestrictionsColumnName);


            string expr = "";

            // to force Unselected Lives 0% when all restrictions are selected
            if (string.Compare(CoveredWithRestrictionsColumnName, "PA_QL_ST_Lives", true) == 0)
                expr = string.Format("Sum({0}) as F2_Lives, MAX(Geography_Name) as Geography_Name, 0 as Other_Lives, Sum(F1_Lives)/Cast(Sum(Formulary_Lives) as System.Decimal)*100 as F1 , Sum({0})/Cast(Sum(Formulary_Lives) as System.Decimal)*100 as F2 , Sum(F3_Lives)/Cast(Sum(Formulary_Lives) as System.Decimal)*100 as F3 , 0 as F4 ,0 as F5 , Sum(Formulary_Lives)-Sum({0}) - Sum(F1_Lives) - Sum(F3_Lives) AS F5_Lives", CoveredWithRestrictionsColumnName);
            else
                expr = string.Format("Sum({0}) as F2_Lives, MAX(Geography_Name) as Geography_Name, Sum(Formulary_Lives) - (Cast((Sum(F1_Lives) + Sum({0}) + Sum(F3_Lives)) as System.Decimal)) as Other_Lives, Sum(F1_Lives)/Cast(Sum(Formulary_Lives) as System.Decimal)*100 as F1 , Sum({0})/Cast(Sum(Formulary_Lives) as System.Decimal)*100 as F2 , Sum(F3_Lives)/Cast(Sum(Formulary_Lives) as System.Decimal)*100 as F3 , (Sum(Formulary_Lives) - (Sum(F1_Lives) + Sum({0}) + Sum(F3_Lives)))/Cast(Sum(Formulary_Lives) as System.Decimal)*100 as F4 ,(Sum(Formulary_Lives)-Sum({0}) - Sum(F1_Lives) - Sum(F3_Lives))/Cast(Sum(Formulary_Lives) as System.Decimal)*100 as F5 , Sum(Formulary_Lives)-Sum({0}) - Sum(F1_Lives) - Sum(F3_Lives) AS F5_Lives", CoveredWithRestrictionsColumnName);

            queryString.Add("__expr", expr);

            //queryString.Add("__expr", " Sum(entity." + CoveredWithRestrictionsColumnName + ") as F2_Lives,(Sum(entity.F1_Lives)/Cast((Sum(entity.F1_Lives) + Sum(entity." + CoveredWithRestrictionsColumnName + ") + Sum(entity.F3_Lives)) as System.Decimal)*100) as F1,(Sum(entity." + CoveredWithRestrictionsColumnName + ")/Cast((Sum(entity.F1_Lives) + Sum(entity." + CoveredWithRestrictionsColumnName + ") + Sum(entity.F3_Lives)) as System.Decimal)*100) as F2, (Sum(entity.F3_Lives)/Cast((Sum(entity.F1_Lives) + Sum(entity." + CoveredWithRestrictionsColumnName + ") + Sum(entity.F3_Lives)) as System.Decimal)*100) as F3");
            base.Preprocess(queryString);
        }

        public override string Select
        {
            get { return "Geography_ID, Section_ID, Section_Name, Drug_ID, Drug_Name"; }
        }


        public override string Sort
        {
            get { return "Drug_Name"; }
        }

    }

}
