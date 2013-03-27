using System.Web;
using System.Text;
using System.Collections.Specialized;
using Pinsonault.Data;
using Pinsonault.Data.Reports;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data.Objects;
using System.Reflection;
using System;
using System.Linq;
using System.Collections;

namespace Pinsonault.Application.TodaysAccounts
{
    public class DrilldownQueryDefinition : UserRequiredQueryDefinition
    {
        public DrilldownQueryDefinition(string EntityTypeName, NameValueCollection queryString)
            : base(EntityTypeName, queryString)
        {
        }
    }

    public class FormularyHistoryQueryDefinition : QueryDefinition
    {
        public FormularyHistoryQueryDefinition(NameValueCollection queryString)
            : base(queryString)
        {
        }

        public FormularyHistoryQueryDefinition(string EntityTypeName, NameValueCollection queryString)
            : base(EntityTypeName, queryString)
        {
        }

        public override IEnumerable CreateQuery(ObjectContext context)
        {
            return CreateFHRQuery();
        }

        IEnumerable CreateFHRQuery()
        {
            //Query for records
            SqlConnection conn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString);
            SqlCommand comm = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter(comm);
            NameValueCollection queryValues = this.Values;

            comm.Connection = conn;

            //Clean Product_ID in case of query string hijack
            string[] drugIDs = queryValues["Drug_ID"].Split(',');

            for (int x = 0; x < drugIDs.Count(); x++)
            {
                int cleanID = Convert.ToInt32(drugIDs[x]);

                //Place clean id back into string array
                drugIDs[x] = cleanID.ToString();
            }

            StringBuilder sb = new StringBuilder();
            IList<string> keys = new List<string>();
            keys.Add("Plan_ID");
            keys.Add("Drug_ID");
            keys.Add("Drug_Name");

            string dataField = "Current_Previous_Flag";

            string tierQuery = ConstructFHRPivotQuery(comm, "Tier_Name", dataField, keys, drugIDs, queryValues).ToString();
            string formularyStatusQUery = ConstructFHRPivotQuery(comm, "Formulary_Status_Abbr", dataField, keys, drugIDs, queryValues).ToString();
            string paQuery = ConstructFHRPivotQuery(comm, "PA", dataField, keys, drugIDs, queryValues).ToString();
            string qlQuery = ConstructFHRPivotQuery(comm, "QL", dataField, keys, drugIDs, queryValues).ToString();
            string stQuery = ConstructFHRPivotQuery(comm, "ST", dataField, keys, drugIDs, queryValues).ToString();
            comm.Parameters.Clear();//Clear parameters from the first five queries since they are all the same
            string copayQuery = ConstructFHRPivotQuery(comm, "Co_Pay", dataField, keys, drugIDs, queryValues).ToString();

            sb.Append("SELECT * ");
            sb.Append("FROM (");
            sb.Append("SELECT tiertable.Plan_ID, ");
            sb.Append("tiertable.Drug_ID, ");
            sb.Append("tiertable.Drug_Name, ");
            sb.Append("CASE WHEN CONVERT(varchar(20),tiertable.Tier_Name0) = '0' THEN NULL ELSE tiertable.Tier_Name0 END AS Tier_Name0, ");
            sb.Append("CASE WHEN CONVERT(varchar(20),tiertable.Tier_Name1) = '0' THEN NULL ELSE tiertable.Tier_Name1 END AS Tier_Name1, ");
            sb.Append("CASE WHEN tiertable.Tier_Name0 = tiertable.Tier_Name1 THEN 0 ELSE 1 END AS TierChanged, ");
            sb.Append("CASE WHEN patable.PA0 = 1 THEN 'PA' ELSE NULL END AS PA0, ");
            sb.Append("CASE WHEN patable.PA1 = 1 THEN 'PA' ELSE NULL END AS PA1, ");
            sb.Append("CASE WHEN patable.PA0 = patable.PA1 THEN 0 ELSE 1 END AS PAChanged, ");
            sb.Append("CASE WHEN qltable.QL0 = 1 THEN 'QL' ELSE NULL END AS QL0, ");
            sb.Append("CASE WHEN qltable.QL1 = 1 THEN 'QL' ELSE NULL END AS QL1, ");
            sb.Append("CASE WHEN qltable.QL0 = qltable.QL1 THEN 0 ELSE 1 END AS QLChanged, ");
            sb.Append("CASE WHEN sttable.ST0 = 1 THEN 'ST' ELSE NULL END AS ST0, ");
            sb.Append("CASE WHEN sttable.ST1 = 1 THEN 'ST' ELSE NULL END AS ST1, ");
            sb.Append("CASE WHEN sttable.ST0 = sttable.ST1 THEN 0 ELSE 1 END AS STChanged, ");
            sb.Append("CASE WHEN CONVERT(varchar(20),copaytable.Co_Pay0) = '0' THEN NULL ELSE copaytable.Co_Pay0 END AS Co_Pay0, ");
            sb.Append("CASE WHEN CONVERT(varchar(20),copaytable.Co_Pay1) = '0' THEN NULL ELSE copaytable.Co_Pay1 END AS Co_Pay1, ");
            sb.Append("CASE WHEN copaytable.Co_Pay0 = copaytable.Co_Pay1 THEN 0 ELSE 1 END AS CopayChanged, ");
            sb.Append("CASE WHEN CONVERT(varchar(20),fstable.Formulary_Status_Abbr0) = '0' THEN NULL ELSE fstable.Formulary_Status_Abbr0 END AS Formulary_Status_Abbr0, ");
            sb.Append("CASE WHEN CONVERT(varchar(20),fstable.Formulary_Status_Abbr1) = '0' THEN NULL ELSE fstable.Formulary_Status_Abbr1 END AS Formulary_Status_Abbr1, ");
            sb.Append("CASE WHEN fstable.Formulary_Status_Abbr0 = fstable.Formulary_Status_Abbr1 THEN 0 ELSE 1 END AS FSChanged ");
            sb.Append("FROM ");
            sb.AppendFormat("(SELECT * FROM ({0}) AS tiertable) AS tiertable INNER JOIN ", tierQuery);
            sb.AppendFormat("(SELECT * FROM ({0}) AS patable) AS patable ON ", paQuery.Replace(",PA ", ", CAST(PA AS tinyint) AS PA "));
            sb.Append("tiertable.Plan_ID = patable.Plan_ID AND ");
            sb.Append("tiertable.Drug_ID = patable.Drug_ID INNER JOIN ");
            sb.AppendFormat("(SELECT * FROM ({0}) AS qltable) AS qltable ON ", qlQuery.Replace(",QL ", ", CAST(QL AS tinyint) AS QL "));
            sb.Append("tiertable.Plan_ID = qltable.Plan_ID AND ");
            sb.Append("tiertable.Drug_ID = qltable.Drug_ID INNER JOIN ");
            sb.AppendFormat("(SELECT * FROM ({0}) AS sttable) AS sttable ON ", stQuery.Replace(",ST ", ", CAST(ST AS tinyint) AS ST "));
            sb.Append("tiertable.Plan_ID = sttable.Plan_ID AND ");
            sb.Append("tiertable.Drug_ID = sttable.Drug_ID INNER JOIN ");
            sb.AppendFormat("(SELECT * FROM ({0}) AS copaytable) AS copaytable ON ", copayQuery);
            sb.Append("tiertable.Plan_ID = copaytable.Plan_ID AND ");
            sb.Append("tiertable.Drug_ID = copaytable.Drug_ID INNER JOIN ");
            sb.AppendFormat("(SELECT * FROM ({0}) AS fstable) AS fstable ON ", formularyStatusQUery);
            sb.Append("tiertable.Plan_ID = fstable.Plan_ID AND ");
            sb.Append("tiertable.Drug_ID = fstable.Drug_ID");
            sb.Append(") AS ChangeTable ORDER BY Drug_Name");

            comm.CommandText = sb.ToString();

            conn.Open();
            System.Data.Common.DbDataReader rdr = comm.ExecuteReader();

            IEnumerable<GenericDataRecord> g = Pinsonault.Data.GenericDataRecord.CreateCollection(rdr);
            conn.Close();

            return g;            
        }

        public SQLPivotQuery<int> ConstructFHRPivotQuery(SqlCommand comm, string pivotField, string dataField, IList<string> keys, string[] drugID, NameValueCollection queryVals)
        {
            Pinsonault.Data.SQLUtil.MaxFragmentLength = 50;

            SQLPivotQuery<int> query = null;

            string tableName = "V_Current_Previous_Plan_Drug_Formulary";

            if (Convert.ToInt32(queryVals["Segment_ID"]) == 2)
                tableName = "V_Current_Previous_Plan_Drug_Formulary_Part_D";
            else
                tableName = "V_Current_Previous_Plan_Drug_Formulary";

            IList<int> timeFrameVals = new List<int>(); ;
            timeFrameVals.Add(0); //Add previous time period
            timeFrameVals.Add(1); //Add current time period

            query = SQLPivotQuery<int>.Create(tableName, "PF", "fhr", keys, dataField, timeFrameVals);

            query.Pivot(SQLFunction.MAX, pivotField);

            ////
            //Add parameters and constrain query below
            ////

            //Do not pass Section ID as per new Section/Segment rules
            //if (Convert.ToInt32(queryVals["Segment_ID"]) != 2) //SectionID is not passed for MedD
            //{
            //    comm.Parameters.AddWithValue("@SectionID", queryVals["Section_ID"]);
            //    query.Where("Section_ID", "SectionID", SQLOperator.EqualTo);
            //}
            int sectionID = Convert.ToInt32(queryVals["Section_ID"]);

            if (sectionID == 1) //Commercial
            {
                comm.Parameters.AddWithValue("@SegmentID", 1);
                query.Where("Segment_ID", "SegmentID", SQLOperator.EqualTo);
            }
            else if (sectionID == 17) //Part D
            {
                comm.Parameters.AddWithValue("@SegmentID", 2);
                query.Where("Segment_ID", "SegmentID", SQLOperator.EqualTo);
            }
            else if (sectionID == 4) //PBM
            {
                comm.Parameters.AddWithValue("@SegmentID0", 1);
                comm.Parameters.AddWithValue("@SegmentID1", 4);
                query.Where("Segment_ID", "SegmentID", SQLOperator.In, 2);
            }
            else if (sectionID == 6) //Managed Medicaid
            {
                comm.Parameters.AddWithValue("@SegmentID", 6);
                query.Where("Segment_ID", "SegmentID", SQLOperator.EqualTo);
            }
            else if (sectionID == 9) //State Medicaid
            {
                comm.Parameters.AddWithValue("@SegmentID", 3);
                query.Where("Segment_ID", "SegmentID", SQLOperator.EqualTo);
            }
            else //All Else
            {
                comm.Parameters.AddWithValue("@SegmentID", queryVals["Segment_ID"]);
                query.Where("Segment_ID", "SegmentID", SQLOperator.EqualTo);
            }

            //if (Convert.ToInt32(queryVals["Section_ID"]) == 9) //State Medicaid
            //    comm.Parameters.AddWithValue("@SegmentID", 3);
            //else
            //    comm.Parameters.AddWithValue("@SegmentID", queryVals["Segment_ID"]);

            //query.Where("Segment_ID", "SegmentID", SQLOperator.EqualTo);

            query.Where("Plan_ID", "PlanID", SQLOperator.EqualTo);
            comm.Parameters.AddWithValue("@PlanID", queryVals["Plan_ID"]);

            if (Convert.ToInt32(queryVals["Segment_ID"]) == 2) 
            {
                //For Med-D, query for Pinso Formulary ID to handle Formulary ID changes from year to year
                int? pinsoFormularyID = Pinsonault.Application.TodaysAccounts.TodaysAccountsDataService.GetPinsoFormularyID(Convert.ToInt32(queryVals["Plan_ID"]), Convert.ToInt32(queryVals["Product_ID"]), Convert.ToInt32(queryVals["Formulary_ID"]));
                query.Where("Pinso_Formulary_ID", "PinsoFormularyID", SQLOperator.EqualTo);
                comm.Parameters.AddWithValue("@PinsoFormularyID", pinsoFormularyID);

                //Also query for product ID
                query.Where("Product_ID", "ProductID", SQLOperator.EqualTo);
                comm.Parameters.AddWithValue("@ProductID", queryVals["Product_ID"]);
            }
            else
            {
                query.Where("Formulary_ID", "FormularyID", SQLOperator.EqualTo);
                comm.Parameters.AddWithValue("@FormularyID", queryVals["Formulary_ID"]);
            }

            //Query Drug_ID
            for (int x = 0; x < drugID.Count(); x++)
                comm.Parameters.AddWithValue(string.Format("@DrugID{0}", x), Convert.ToInt32(drugID[x]));

            query.Where("Drug_ID", "DrugID", SQLOperator.In, drugID.Count());

            return query;
        }
    }

    #region Today's Accounts Pie Chart
    public class TodaysAccountsQueryDefinition : QueryDefinition
    {
        public static string SelectFields
        {
            get
            {
                return "Section_ID, Section_Name";
            }
        }

        public static string AggrFields { get { return "PERCENTSUM(Total_Pharmacy)"; } }
        

        public TodaysAccountsQueryDefinition(string EntityTypeName, NameValueCollection queryString)
            : base(EntityTypeName, queryString)
        {
        }

        public override string Select { get { return SelectFields; } }
        public override string Aggregate
        {
            get { return AggrFields; }
        }
        public override string Sort
        {
            get { return "Section_Name"; }
        }
    }
    #endregion
}
