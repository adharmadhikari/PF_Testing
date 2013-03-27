using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using PathfinderModel;
using PathfinderClientModel;
using Pinsonault.Data;
using System.Data.SqlClient;
using System.Data;
using Dundas.Charting.WebControl;
using Pinsonault.Data.Reports;
using System.Text;
using System.Data.Common;
using Pinsonault.Application;
using System.Data.Objects;

/// <summary>
/// Provides helper functionality for Marketplace Analytics
/// </summary>

namespace Pinsonault.Application.MarketplaceAnalytics
{
    public class MarketplaceAnalyticsProvider
    {
        int rollup = 0;
        int[] planIDarr = null;
        string segmentID = "1";
        string trxMst;
        List<int> timeFrame = null;
        public bool intervalFlag = true;           

        //Used to construct basic pivot query
        public SQLPivotQuery<int> ConstructMAPivotQuery(SqlCommand comm, string dataYear, string geographyID, IList<int> timeFrameVals, string tableName, string dataField, IList<string> keys, bool isDetailed, int? productID, NameValueCollection queryVals)
        {
            Pinsonault.Data.SQLUtil.MaxFragmentLength = 50;

            SQLPivotQuery<int> query = null;

            trxMst = queryVals["Trx_Mst"];

            if (string.IsNullOrEmpty(trxMst))
                trxMst = "Trx";

            timeFrame = timeFrameVals.ToList();

            //Check if query is for summary or detailed grid
            if (isDetailed)
            {
                query = SQLPivotQuery<int>.Create(tableName, string.Format("PF_{0}", Pinsonault.Web.Session.ClientKey), "tr", keys, dataField, timeFrame)
                .Where("Thera_ID", "MarketBasketID", SQLOperator.EqualTo);

                //Query can only be restrained by product id for detailed grid
                if (productID != null)
                {
                    query.Where("Product_ID", "ProductID", SQLOperator.EqualTo);
                    comm.Parameters.Add("@ProductID", SqlDbType.Int).Value = productID;
                }
            }
            else
            {
                //If Rolling Quarterly, reverse time frame values
                if (string.Compare(queryVals["Calendar_Rolling"], "Rolling", true) == 0)
                    timeFrame.Reverse();

                query = SQLPivotQuery<int>.Create(tableName, null, "tr", keys, dataField, timeFrame)
                .Where("Thera_ID", "MarketBasketID", SQLOperator.EqualTo);
            }

            //Add year only if Calendar type query
            if ((string.Compare(queryVals["Calendar_Rolling"], "Calendar", true) == 0) && !(string.IsNullOrEmpty(dataYear)))
            {
                query.Where("Data_Year", "DataYear", SQLOperator.EqualTo);
                comm.Parameters.Add("@DataYear", SqlDbType.Int).Value = Convert.ToInt32(dataYear);
            }

            //Check if it is TRX, MST or Total MB_Trx or Total MB_Nrx
            if ((!string.IsNullOrEmpty(queryVals["Is_MB_Trx"])) && Convert.ToBoolean(queryVals["Is_MB_Trx"]))
                query.Pivot(SQLFunction.AVG, "MB_TRx");
            else if ((!string.IsNullOrEmpty(queryVals["Is_MB_Nrx"])) && Convert.ToBoolean(queryVals["Is_MB_Nrx"]))
                query.Pivot(SQLFunction.AVG, "MB_NRx");
            else
            {
                if (string.Compare(trxMst, "trx", true) == 0)
                    query.Pivot(SQLFunction.SUM, "Product_Trx");
                else if (string.Compare(trxMst, "nrx", true) == 0)
                {
                    query.Pivot(SQLFunction.SUM, "Product_Nrx");
                }
                else if (string.Compare(trxMst, "msn", true) == 0)
                {
                    query.Pivot(SQLFunction.SUM, "Product_Nrx");
                    query.Pivot(SQLFunction.SUM, "MB_Nrx");
                }
                else
                {
                    query.Pivot(SQLFunction.SUM, "Product_Trx");
                    query.Pivot(SQLFunction.SUM, "MB_Trx");
                }
            }

            ////
            //Add parameters and constrain query below
            ////

            //Market Basket
            comm.Parameters.Add("@MarketBasketID", SqlDbType.Int).Value = Convert.ToInt32(queryVals["Market_Basket_ID"]);

            //Check if Channel is Combined (Commercial + Part D)
            //string segmentID = "1";
            if (!string.IsNullOrEmpty(queryVals["Section_ID"]))
            {
                if (queryVals["Section_ID"] == "-1") //Channel is Combined - use segment ID of 1 and 2
                {
                    //Get Commercial & Medicare (Section ID 1 or 17; Segment ID 1 or 2)
                    comm.Parameters.Add("@SectionID0", SqlDbType.Int).Value = 1;
                    comm.Parameters.Add("@SectionID1", SqlDbType.Int).Value = 17;

                    query.Where("Section_ID", "SectionID", SQLOperator.In, 2);

                    comm.Parameters.Add("@SegmentID0", SqlDbType.Int).Value = 2;
                    comm.Parameters.Add("@SegmentID1", SqlDbType.Int).Value = 1;

                    query.Where("Segment_ID", "SegmentID", SQLOperator.In, 2);

                    segmentID = "1,2";
                }
                else if (queryVals["Section_ID"] == "8") //Channel is Other - include Other as well as DOD, FEP and Medical Groups
                {
                    comm.Parameters.Add("@SectionID0", SqlDbType.Int).Value = 8;//Other
                    comm.Parameters.Add("@SectionID1", SqlDbType.Int).Value = 12;//DOD
                    comm.Parameters.Add("@SectionID2", SqlDbType.Int).Value = 20;//FEP
                    comm.Parameters.Add("@SectionID3", SqlDbType.Int).Value = 18;//Medical Groups

                    query.Where("Section_ID", "SectionID", SQLOperator.In, 4);

                    comm.Parameters.Add("@SegmentID", SqlDbType.Int).Value = 1;

                    query.Where("Segment_ID", "SegmentID", SQLOperator.EqualTo);
                }
                else //Channel is not Combined
                {
                    //Query Seqment_ID: Segment_ID is 2 for Med-D, 3 for State Medicaid or 1 for all else
                    if (Convert.ToInt32(queryVals["Section_ID"]) == 17) //Med-D, Section ID is not passed in case of Med-D
                    {
                        comm.Parameters.Add("@SegmentID", SqlDbType.Int).Value = 2;
                        segmentID = "2";
                    }
                    else
                    {
                        comm.Parameters.Add("@SectionID", SqlDbType.Int).Value = Convert.ToInt32(queryVals["Section_ID"]);

                        query.Where("Section_ID", "SectionID", SQLOperator.EqualTo);

                        if (Convert.ToInt32(queryVals["Section_ID"]) == 9) //State Medicaid
                        {
                            comm.Parameters.Add("@SegmentID", SqlDbType.Int).Value = 3;
                            segmentID = "3";
                        }
                        else
                            comm.Parameters.Add("@SegmentID", SqlDbType.Int).Value = 1; //All Else                        
                    }

                    query.Where("Segment_ID", "SegmentID", SQLOperator.EqualTo);
                }
            }

            //Geography parameters - only add if not selecting by Account Manager/Territory
            int geographyType = Convert.ToInt32(queryVals["Geography_Type"]);

            if (geographyType == 1 || geographyType == 2 || (geographyType == 3 && string.Compare(geographyID, "US", true) == 0) || (geographyType == 0 && string.Compare(geographyID, "US", true) == 0))
            {
                query.Where("Geography_ID", "GeographyID", SQLOperator.EqualTo);
                comm.Parameters.Add("@GeographyID", SqlDbType.VarChar).Value = geographyID;
            }

            //Region/Territory/District Paramaters (For prescriber trending report)
            string prescriberGeographyType = queryVals["Prescriber_Geography_Type"];

            if (!string.IsNullOrEmpty(prescriberGeographyType))
            {
                SqlParameter p = new SqlParameter();
                p.SqlDbType = SqlDbType.VarChar;
                p.Size = 50;

                if (string.Compare(prescriberGeographyType, "region", true) == 0)
                {
                    p.SqlValue = geographyID;
                    p.ParameterName = "@RegionID";
                    query.Where("Region_ID", "RegionID", SQLOperator.EqualTo);
                    comm.Parameters.Add(p);
                }
                if (string.Compare(prescriberGeographyType, "district", true) == 0)
                {
                    p.SqlValue = geographyID;
                    p.ParameterName = "@DistrictID";
                    query.Where("District_ID", "DistrictID", SQLOperator.EqualTo);
                    comm.Parameters.Add(p);
                }
                if (string.Compare(prescriberGeographyType, "territory", true) == 0)
                {
                    p.SqlValue = geographyID;
                    p.ParameterName = "@TerritoryID";
                    query.Where("Territory_ID", "TerritoryID", SQLOperator.EqualTo);
                    comm.Parameters.Add(p);
                }
            }

            if (!string.IsNullOrEmpty(queryVals["Physician_ID"]))
            {
                SqlParameter p = new SqlParameter();
                p.SqlDbType = SqlDbType.VarChar;
                p.Size = 25;
                p.SqlValue = queryVals["Physician_ID"];
                p.ParameterName = "@PhysicianID";
                query.Where("Physician_ID", "PhysicianID", SQLOperator.EqualTo);
                comm.Parameters.Add(p);
            }

            //Account Manager (only query if Rollup is not Top 10/20 or rollup != 2 or 3)
            if (geographyType == 3 && (string.Compare(geographyID, "US", true) == -1))
            {
                comm.Parameters.Add("@TerritoryID", SqlDbType.VarChar).Value = queryVals["Territory_ID"];

                query.Where("Territory_ID", "TerritoryID", SQLOperator.EqualTo);
            }

            //Add drugs to query only if not selecting one product (for drilldown)
            if (productID == null)
            {
                string drugIDs = queryVals["Product_ID"];
                string[] drugIDarr = drugIDs.Split(',');

                for (int x = 0; x < drugIDarr.Count(); x++)
                    comm.Parameters.Add(string.Format("@ProductID{0}", x), SqlDbType.Int).Value = Convert.ToInt32(drugIDarr[x]);

                query.Where("Product_ID", "ProductID", SQLOperator.In, drugIDarr.Count());
            }

            //Check Rollup type
            rollup = Convert.ToInt32(queryVals["Rollup_Type"]);
            
            //Rollup By Top 10 or Top 20
            if (rollup == 2 || rollup == 3)
            {
                using (PathfinderMarketplaceAnalyticsEntities context = new PathfinderMarketplaceAnalyticsEntities(Pinsonault.Web.Session.GetClientApplicationConnectionString("MarketplaceAnalytics")))
                {
                    int sectionID = Convert.ToInt32(queryVals["Section_ID"]);
                    int theraID = Convert.ToInt32(queryVals["Market_Basket_ID"]);
                    int take = (rollup == 2) ? 10 : 20; //Take top 10 is rollup is 2, or else take top 20 if rollup is 3

                    //Check if Top X by Territory (Account Manager)
                    if (geographyType == 3 && string.Compare(geographyID, "US", true) == -1)
                        planIDarr = context.GetTopPlansByMarketshareTerritory(take, sectionID, theraID, queryVals["Territory_ID"]).ToArray();
                    else if (geographyType == 0)//Prescriber Trending Report
                        planIDarr = GetTopPlansByPhysicians(tableName, queryVals, prescriberGeographyType, geographyID, dataField, dataYear, timeFrameVals);
                    else
                        planIDarr = context.GetTopPlansByMarketshare(take, sectionID, geographyID, theraID).ToArray();
                }

                if (planIDarr.Count() > 0)
                {
                    for (int x = 0; x < planIDarr.Count(); x++)
                        comm.Parameters.Add(string.Format("@PlanID{0}", x), SqlDbType.Int).Value = Convert.ToInt32(planIDarr[x]);
                    
                    query.Where("Plan_ID", "PlanID", SQLOperator.In, planIDarr.Count());
                }
            }

            //Rollup By Top 50 or Top 100 for Prescriber Trending
            if (rollup == 5 || rollup == 6)
            {
                IList<string> physIDArr = GetTopPhysicians(tableName, queryVals, prescriberGeographyType, geographyID, dataField, dataYear, timeFrameVals);

                if (physIDArr.Count() > 0)
                {
                    for (int x = 0; x < physIDArr.Count(); x++)
                        comm.Parameters.Add(string.Format("@PhysicianID{0}", x), SqlDbType.VarChar).Value = physIDArr[x];

                    query.Where("Physician_ID", "PhysicianID", SQLOperator.In, physIDArr.Count());
                }                
            }

            //Rollup By Account Name
            if (rollup == 4)
            {
                string planID = queryVals["Plan_ID"];

                if (!string.IsNullOrEmpty(planID))
                {
                    string[] planIDarr = planID.Split(',');

                    if (planIDarr.Count() > 0)
                    {
                        for (int x = 0; x < planIDarr.Count(); x++)
                            comm.Parameters.Add(string.Format("@PlanID{0}", x), SqlDbType.Int).Value = Convert.ToInt32(planIDarr[x]);

                        query.Where("Plan_ID", "PlanID", SQLOperator.In, planIDarr.Count());
                    }
                }
            }

            

            return query;
        }

        //Used to construct basic pivot query
        public SQLPivotQuery<int> ConstructFHRPivotQuery(SqlCommand comm, string pivotField, int timeFrame, string tableName, string dataField, IList<string> keys, string productID, NameValueCollection queryVals)
        {
            Pinsonault.Data.SQLUtil.MaxFragmentLength = 35;

            SQLPivotQuery<int> query = null;

            IList<int> timeFrameVals = new List<int>();;
            timeFrameVals.Add(GetPreviousTimeFrame(timeFrame, Convert.ToInt32(queryVals["Section_ID"]))); //Add previous time period
            timeFrameVals.Add(Convert.ToInt32(timeFrame)); //Add current time period

            query = SQLPivotQuery<int>.Create(tableName, string.Format("PF_{0}", Pinsonault.Web.Session.ClientKey), "fhr", keys, dataField, timeFrameVals);

            query.Pivot(SQLFunction.MAX, pivotField);

            ////
            //Add parameters and constrain query below
            ////

            //Query Seqment_ID: Segment_ID is 2 for Med-D, 3 for State Medicaid or 1 for all else
            if (Convert.ToInt32(queryVals["Section_ID"]) == 17) //Med-D, Section ID is not passed in case of Med-D
                comm.Parameters.Add("@SegmentID", SqlDbType.Int).Value = 2;
            else
            {
                comm.Parameters.Add("@SectionID", SqlDbType.Int).Value = Convert.ToInt32(queryVals["Section_ID"]);
                query.Where("Section_ID", "SectionID", SQLOperator.EqualTo);

                if (Convert.ToInt32(queryVals["Section_ID"]) == 9) //State Medicaid
                    comm.Parameters.Add("@SegmentID", SqlDbType.Int).Value = 3;
                else
                    comm.Parameters.Add("@SegmentID", SqlDbType.Int).Value = 1; //All Else
            }

            query.Where("Segment_ID", "SegmentID", SQLOperator.EqualTo);

            //Query Plan_ID
            query.Where("Plan_ID", "PlanID", SQLOperator.EqualTo);
            comm.Parameters.Add("@PlanID", SqlDbType.Int).Value = Convert.ToInt32(queryVals["Plan_ID"]);

            //Query Product_ID
            string[] prodIDarr = productID.Split(',');

            for (int x = 0; x < prodIDarr.Count(); x++)
                comm.Parameters.Add(string.Format("@ProductID{0}", x), SqlDbType.Int).Value = Convert.ToInt32(prodIDarr[x]);    

            query.Where("Product_ID", "ProductID", SQLOperator.In, prodIDarr.Count());

            return query;
        }

        public IEnumerable<GenericDataRecord> GetData(IList<int> timeFrameVals, bool isMonth, string dataYear, string geographyID, string tableName, string dataField, IList<string> keys, bool isDetailed, int? productID, NameValueCollection queryVals)
        {
            //Query for records
            SqlConnection conn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString);
            SqlCommand comm = new SqlCommand();

            comm.CommandText = ConstructMAPivotQuery(comm, dataYear, geographyID, timeFrameVals, tableName, dataField, keys, isDetailed, productID, queryVals).IncludeSum().ToString();

            comm.Connection = conn;

            try
            {
                //Clean Product_ID in case of query string hijack
                string[] productIDs = queryVals["Product_ID"].Split(',');

                for (int x = 0; x < productIDs.Count(); x++)
                {
                    int cleanID = Convert.ToInt32(productIDs[x]);

                    //Place clean id back into string array
                    productIDs[x] = cleanID.ToString();
                }

                if (isDetailed)//Detailed view logic
                {
                    StringBuilder sb = new StringBuilder();

                    if (rollup == 2 || rollup == 3) //Wrap query for detail view and Top X Plans
                    {
                        //Get top plans
                        string[] planIDstr = new string[planIDarr.Length];

                        for (int i = 0; i < planIDarr.Length; i++)
                            planIDstr[i] = planIDarr[i].ToString();

                        string topPlanIDs = string.Join(",", planIDstr);

                        sb.Append("SELECT TOP 100 PERCENT ");
                        sb.Append("1 AS [SortCol],");
                        sb.Append("T1.*, ");
                        sb.Append("PF.dbo.Plan_Drug_Formulary.Tier_No, ");
                        sb.Append("PF.dbo.Plan_Drug_Formulary.Co_Pay, ");
                        sb.Append("CASE PF.dbo.Plan_Drug_Formulary.PA WHEN 'True' THEN 'PA' WHEN 'False' THEN '' ELSE '' END AS PA, ");
                        sb.Append("CASE PF.dbo.Plan_Drug_Formulary.QL WHEN 'True' THEN 'QL' WHEN 'False' THEN '' ELSE '' END AS QL, ");
                        sb.Append("CASE PF.dbo.Plan_Drug_Formulary.ST WHEN 'True' THEN 'ST' WHEN 'False' THEN '' ELSE '' END AS ST, ");
                        sb.Append("PF.dbo.Lkp_Tiers.Tier_Name, ");
                        sb.Append("p.Plan_Name, ");
                        if (Convert.ToInt32(queryVals["Section_ID"]) == 9)
                        {
                            sb.Append("p.Medicaid_Enrollment-p.Medicaid_Mcare_Enrollment as Total_Covered, ");
                        }
                        else
                        {
                            if (segmentID == "3")
                            {
                                sb.Append("p.Medicaid_Enrollment as Total_Covered, ");
                            }
                            else if (segmentID == "1")
                            {
                                sb.Append("p.Total_Pharmacy-p.Total_Medicare_PartD as Total_Covered, ");
                            }
                            else if (segmentID == "2")
                            {
                                sb.Append("p.Total_Medicare_PartD as Total_Covered, ");
                            }
                            else 
                            { 
                                sb.Append("p.Total_Pharmacy as Total_Covered, "); 
                            }
                        }
                        
                        sb.Append("dbo.Drug_Products.Product_Name, ");
                        sb.Append("dbo.Drug_Products.Product_ID, ");
                        sb.Append("PF.dbo.Lkp_Geographies.Geography_Name ");
                        sb.Append("from pf.dbo.plans p cross join dbo.Drug_Products ");
                        //Change the name of the inner query 'Product_ID' column since we are selecting the column from dbo.Drug_Products
                        sb.AppendFormat("left outer join ({0}) as T1 ", comm.CommandText.Replace("pivotTable0.Product_ID,", "pivotTable0.Product_ID AS 'Prod_ID',"));
                        sb.Append("on T1.Plan_ID = p.Plan_ID and ");
                        sb.Append("T1.Prod_ID = dbo.Drug_Products.Product_ID ");
                        sb.Append("left join PF.dbo.Plan_Drug_Formulary on ");
                        sb.Append("PF.dbo.Plan_Drug_Formulary.Plan_ID = p.Plan_ID and ");
                        sb.Append("PF.dbo.Plan_Drug_Formulary.Drug_ID = dbo.Drug_Products.Drug_ID and ");
                        sb.Append("PF.dbo.Plan_Drug_Formulary.Is_Predominant = 1 and ");
                        sb.AppendFormat("PF.dbo.Plan_Drug_Formulary.Segment_ID in ({0})", segmentID);
                        sb.Append("left outer join PF.dbo.Lkp_Tiers on ");
                        sb.Append("PF.dbo.Lkp_Tiers.Tier_Id = PF.dbo.Plan_Drug_Formulary.Tier_No ");
                        sb.Append("inner join PF.dbo.Lkp_Geographies on ");
                        sb.Append("PF.dbo.Lkp_Geographies.Geography_ID = p.Plan_State ");
                        sb.Append("where ");
                        sb.AppendFormat("dbo.Drug_Products.Product_ID in ({0})", string.Join(",", productIDs));

                        if (topPlanIDs.Length > 0)
                            sb.AppendFormat(" and p.Plan_ID in ({0})", topPlanIDs);
                        else
                            sb.Append(" and p.Plan_ID in (-1)");
                    }
                    else //Wrap query for detailed view and All plans
                    {
                        sb.Append("SELECT TOP 100 PERCENT ");
                        sb.Append("1 AS [SortCol],");
                        sb.Append("T1.*, ");
                        sb.Append("PF.dbo.Plan_Drug_Formulary.Tier_No, ");
                        sb.Append("PF.dbo.Plan_Drug_Formulary.Co_Pay, ");
                        sb.Append("CASE PF.dbo.Plan_Drug_Formulary.PA WHEN 'True' THEN 'PA' WHEN 'False' THEN '' ELSE '' END AS PA, ");
                        sb.Append("CASE PF.dbo.Plan_Drug_Formulary.QL WHEN 'True' THEN 'QL' WHEN 'False' THEN '' ELSE '' END AS QL, ");
                        sb.Append("CASE PF.dbo.Plan_Drug_Formulary.ST WHEN 'True' THEN 'ST' WHEN 'False' THEN '' ELSE '' END AS ST, ");
                        sb.Append("PF.dbo.Lkp_Tiers.Tier_Name, ");
                        sb.Append("p.Plan_Name, ");
                        //if (segmentID != "3")
                        //    sb.Append("p.Total_Covered, ");
                        //else
                        //    sb.Append("p.Medicaid_Enrollment as Total_Covered, ");
                        if (Convert.ToInt32(queryVals["Section_ID"]) == 9)
                        {
                            sb.Append("p.Medicaid_Enrollment-p.Medicaid_Mcare_Enrollment as Total_Covered, ");
                        }
                        else
                        {
                            if (segmentID == "3")
                            {
                                sb.Append("p.Medicaid_Enrollment as Total_Covered, ");
                            }
                            else if (segmentID == "1")
                            {
                                sb.Append("p.Total_Pharmacy-p.Total_Medicare_PartD as Total_Covered, ");
                            }
                            else if (segmentID == "2")
                            {
                                sb.Append("p.Total_Medicare_PartD as Total_Covered, ");
                            }
                            else { sb.Append("p.Total_Pharmacy as Total_Covered, "); }
                        }
                       
                        sb.Append("dbo.Drug_Products.Product_Name, ");
                        sb.Append("PF.dbo.Lkp_Geographies.Geography_Name ");
                        sb.Append(" from PF.dbo.Plan_Drug_Formulary right outer join ");
                        sb.AppendFormat("({0}) as T1 ", comm.CommandText);
                        sb.Append("on PF.dbo.Plan_Drug_Formulary.Plan_ID = T1.Plan_ID and ");
                        sb.Append("PF.dbo.Plan_Drug_Formulary.Drug_ID = T1.Drug_ID and ");
                        //added for avoiding duplicate rows with same plan and trx volume having different tier
                        sb.Append("PF.dbo.Plan_Drug_Formulary.Segment_ID = T1.Segment_ID and ");
                        if (!string.IsNullOrEmpty(segmentID))
                            sb.AppendFormat("PF.dbo.Plan_Drug_Formulary.Segment_ID in ({0})", segmentID);
                        sb.Append(" and PF.dbo.Plan_Drug_Formulary.Is_Predominant = 1 ");
                        sb.Append("inner join pf.dbo.plans p on ");
                        sb.Append("p.Plan_ID = T1.Plan_ID ");
                        sb.Append("left join dbo.Drug_Products on ");
                        sb.Append("dbo.Drug_Products.Product_ID = T1.Product_ID ");
                        sb.Append("inner join PF.dbo.Lkp_Geographies on ");
                        sb.Append("PF.dbo.Lkp_Geographies.Geography_ID = p.Plan_State");
                        sb.Append(" left outer join PF.dbo.Lkp_Tiers on ");
                        sb.Append("PF.dbo.Plan_Drug_Formulary.Tier_No = PF.dbo.Lkp_Tiers.Tier_Id ");
                    }

                    comm.CommandText = sb.ToString();
                }
                else //Summary view logic
                {
                    StringBuilder sb = new StringBuilder();

                    //Show Product in summary view when Top X Plan is selected even though no data is available - applies only to Top X Plans
                    if (rollup == 2 || rollup == 3)
                    {
                        sb.Append("SELECT TOP 100 PERCENT ");
                        sb.Append("T1.*, ");
                        sb.Append("dbo.Drug_Products.Product_ID, ");
                        sb.Append("dbo.Drug_Products.Product_Name ");
                        //Change the name of the inner query 'Product_ID' column since we are selecting the column from dbo.Drug_Products
                        sb.AppendFormat("from ({0}) as T1 ", comm.CommandText.Replace("pivotTable0.Product_ID,", "pivotTable0.Product_ID AS 'Prod_ID',"));
                        sb.Append("right join dbo.Drug_Products on ");
                        sb.Append("dbo.Drug_Products.Product_ID = T1.Prod_ID ");
                        sb.AppendFormat("where dbo.Drug_Products.Product_ID in ({0}) ", string.Join(",", productIDs));
                        sb.Append("order by Product_Name");
                    }
                    else
                    {
                        //Wrap query for summary view joins
                        sb.Append("SELECT TOP 100 PERCENT ");
                        sb.Append("T1.*, ");
                        sb.Append("dbo.Drug_Products.Product_Name ");
                        sb.AppendFormat("from ({0}) as T1 ", comm.CommandText);
                        sb.Append("left join dbo.Drug_Products on ");
                        sb.Append("dbo.Drug_Products.Product_ID = T1.Product_ID ");
                        sb.Append("order by Product_Name");
                    }

                    comm.CommandText = sb.ToString();
                }                

                //Wrap query for MST calculations
                if (string.Compare(trxMst, "mst", true) == 0)
                {
                    IList<string> colsTrxToSum = new List<string>();
                    IList<string> colsMBTrxToSum = new List<string>();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT TOP 100 PERCENT *, ");

                    for (int x = 0; x < timeFrame.Count; x++)
                    {
                        sb.AppendFormat("Product_Mst{0} = CASE ISNULL(MB_Trx{0}, 0) WHEN 0 THEN 0.000 ELSE (Product_Trx{0} / MB_Trx{0}) * 100 END,", x);

                        colsTrxToSum.Add(string.Format("Product_Trx{0}", x));
                        colsMBTrxToSum.Add(string.Format("MB_Trx{0}", x));
                    }

                    sb.AppendFormat("Product_Mst_Sum = CASE({0}) WHEN 0 THEN 0.000 ELSE (({1})/({2})) END ", string.Join(" + ", colsMBTrxToSum.ToArray()), string.Join(" + ", colsTrxToSum.ToArray()), string.Join(" + ", colsMBTrxToSum.ToArray()));
                    sb.AppendFormat("FROM ({0}) AS T2 ORDER BY Product_Name", comm.CommandText);

                    //Remove Drug_ID from WHERE clause in case Product is in results
                    comm.CommandText = sb.ToString().Replace("and pivotTable0.Drug_ID = pivotTable1.Drug_ID", "");
                }

                //wrap query for MSN calculation
                #region MSN calculation
                //Wrap query for MST calculations
                if (string.Compare(trxMst, "msn", true) == 0)
                {
                    IList<string> colsNrxToSum = new List<string>();
                    IList<string> colsMBNrxToSum = new List<string>();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT TOP 100 PERCENT *, ");

                    for (int x = 0; x < timeFrame.Count; x++)
                    {
                        sb.AppendFormat("Product_Msn{0} = CASE ISNULL(MB_Nrx{0}, 0) WHEN 0 THEN 0.000 ELSE (Product_Nrx{0} / MB_Nrx{0}) * 100 END,", x);

                        colsNrxToSum.Add(string.Format("Product_Nrx{0}", x));
                        colsMBNrxToSum.Add(string.Format("MB_Nrx{0}", x));
                    }

                    sb.AppendFormat("Product_Msn_Sum = CASE({0}) WHEN 0 THEN 0.000 ELSE (({1})/({2})) END ", string.Join(" + ", colsMBNrxToSum.ToArray()), string.Join(" + ", colsNrxToSum.ToArray()), string.Join(" + ", colsMBNrxToSum.ToArray()));
                    sb.AppendFormat("FROM ({0}) AS T2 ORDER BY Product_Name", comm.CommandText);

                    //Remove Drug_ID from WHERE clause in case Product is in results
                    comm.CommandText = sb.ToString().Replace("and pivotTable0.Drug_ID = pivotTable1.Drug_ID", "");
                }

                #endregion

                #region detailed trx and total mb calculations
                //Wrap query to obtain Total MB only for TRX and detailed view
                if (string.Compare(trxMst, "trx", true) == 0 && isDetailed)
                {
                    SqlCommand mbTrxComm = new SqlCommand();

                    StringBuilder sb = new StringBuilder();

                    sb.Append("SELECT TOP 100 PERCENT * FROM (");
                    sb.Append("SELECT TOP 100 PERCENT ");
                    sb.Append("2 AS [SortCol],");

                    //Change column name if selecting Top X Plans (to match unioned query)
                    if (rollup == 2 || rollup == 3)
                        sb.Append("T_MB.Product_ID AS 'Prod_ID', ");
                    else
                        sb.Append("T_MB.Product_ID, ");

                    sb.Append("T_MB.Drug_ID, ");
                    sb.Append("T_MB.Thera_ID, ");
                    sb.Append("T_MB.Segment_ID, ");
                    sb.Append("T_MB.Segment_Name, ");
                    sb.Append("T_MB.Geography_ID, ");
                    sb.Append("T_MB.Plan_ID, ");

                    //Only add Data_Year if not Rolling Quarter selection
                    if ((string.Compare(queryVals["Calendar_Rolling"], "Calendar", true) == 0) && !(string.IsNullOrEmpty(dataYear)))
                        sb.Append("T_MB.Data_Year, ");

                    //Change column names of dynamic fields to match column names of unioned query
                    for (int x = 0; x < timeFrameVals.Count; x++)
                        sb.AppendFormat("T_MB.MB_TRx{0} AS [Product_Trx{0}], ", x);

                    sb.Append("T_MB.MB_TRx_Sum AS [Product_Trx_Sum], ");
                    sb.Append("T_MB.Tier_No, ");
                    sb.Append("T_MB.Co_Pay, ");
                    sb.Append("T_MB.PA, ");
                    sb.Append("T_MB.QL, ");
                    sb.Append("T_MB.ST, ");
                    sb.Append("T_MB.Tier_Name, ");
                    sb.Append("T_MB.Plan_Name, ");
                    sb.Append("T_MB.Total_Covered, ");
                    sb.Append("T_MB.Product_Name, ");

                    //Add column name if selecting Top X Plans (to match unioned query)
                    if (rollup == 2 || rollup == 3)
                        sb.Append("0 AS [Product_ID], ");

                    sb.Append("T_MB.Geography_Name ");
                    sb.Append("FROM ( ");
                    sb.Append("SELECT TOP 100 PERCENT ");
                    sb.Append("0 AS [Product_ID], ");
                    sb.Append("0 AS [Drug_ID], ");
                    sb.Append("T1.*, ");
                    sb.Append("NULL AS [Tier_No], ");
                    sb.Append("'' AS [Co_Pay], ");
                    sb.Append("'' AS [PA], ");
                    sb.Append("'' AS [QL], ");
                    sb.Append("'' AS [ST], ");
                    sb.Append("'' AS [Tier_Name], ");
                    sb.Append("p.Plan_Name, ");
                    //sb.Append("p.Total_Covered, ");
                    if (Convert.ToInt32(queryVals["Section_ID"]) == 9)
                    {
                        sb.Append("p.Medicaid_Enrollment-p.Medicaid_Mcare_Enrollment as Total_Covered, ");
                    }
                    else
                    {
                        if (segmentID == "3")
                        {
                            sb.Append("p.Medicaid_Enrollment as Total_Covered, ");
                        }
                        else if (segmentID == "1")
                        {
                            sb.Append("p.Total_Pharmacy-p.Total_Medicare_PartD as Total_Covered, ");
                        }
                        else if (segmentID == "2")
                        {
                            sb.Append("p.Total_Medicare_PartD as Total_Covered, ");
                        }
                        else { sb.Append("p.Total_Pharmacy as Total_Covered, "); }
                    }
                    sb.Append("'Total MB' AS [Product_Name], ");
                    sb.Append("PF.dbo.Lkp_Geographies.Geography_Name ");
                    sb.Append("from ");
                    sb.Append("PF.dbo.Plan_Drug_Formulary ");
                    sb.Append("right outer join (");
                                        
                    //Remove Product_ID, Drug_ID and MB_Trx keys to prevent duplicates (selected as zero above to preserve column names)
                    IList<string> mbTrxKeys = new List<string>();
                    
                    foreach (string s in keys)
                        mbTrxKeys.Add(s);

                    mbTrxKeys.Remove("Product_ID");
                    mbTrxKeys.Remove("Drug_ID");
                    mbTrxKeys.Remove("MB_Trx");

                    //Add flag in queryVals to pivot query on MB_Trx in 'ConstructMAPivotQuery'
                    NameValueCollection mbQueryVals = new NameValueCollection(queryVals);
                    mbQueryVals.Add("Is_MB_Trx", "true");

                    string mbTrxQuery = ConstructMAPivotQuery(mbTrxComm, dataYear, geographyID, timeFrameVals, tableName, dataField, mbTrxKeys, isDetailed, productID, mbQueryVals).IncludeSum().ToString();
                    
                    //Add MB Trx pivot query
                    sb.Append(mbTrxQuery);
                    sb.Append(") as T1 on ");
                    sb.Append("PF.dbo.Plan_Drug_Formulary.Is_Predominant = 1 inner join ");
                    sb.Append("pf.dbo.plans p on p.Plan_ID = T1.Plan_ID inner join ");
                    sb.Append("PF.dbo.Lkp_Geographies on PF.dbo.Lkp_Geographies.Geography_ID = p.Plan_State ");
                    sb.Append(") AS T_MB ");
                    sb.Append("UNION ");

                    //Add existing pivot query
                    sb.Append(comm.CommandText);
                    sb.Append(") as TableX ");
                    sb.Append("ORDER BY Plan_Name, Product_Name, SortCol");

                    comm.CommandText = sb.ToString();
                }

                # endregion

                #region Detailed NRx calculation
                //Wrap query to obtain Total NRx only for NRX and detailed view
                if (string.Compare(trxMst, "nrx", true) == 0 && isDetailed)
                {
                    SqlCommand mbNrxComm = new SqlCommand();

                    StringBuilder sb = new StringBuilder();

                    sb.Append("SELECT TOP 100 PERCENT * FROM (");
                    sb.Append("SELECT TOP 100 PERCENT ");
                    sb.Append("2 AS [SortCol],");

                    //Change column name if selecting Top X Plans (to match unioned query)
                    if (rollup == 2 || rollup == 3)
                        sb.Append("T_MB.Product_ID AS 'Prod_ID', ");
                    else
                        sb.Append("T_MB.Product_ID, ");

                    sb.Append("T_MB.Drug_ID, ");
                    sb.Append("T_MB.Thera_ID, ");
                    sb.Append("T_MB.Segment_ID, ");
                    sb.Append("T_MB.Segment_Name, ");
                    sb.Append("T_MB.Geography_ID, ");
                    sb.Append("T_MB.Plan_ID, ");

                    //Only add Data_Year if not Rolling Quarter selection
                    if ((string.Compare(queryVals["Calendar_Rolling"], "Calendar", true) == 0) && !(string.IsNullOrEmpty(dataYear)))
                        sb.Append("T_MB.Data_Year, ");

                    //Change column names of dynamic fields to match column names of unioned query
                    for (int x = 0; x < timeFrameVals.Count; x++)
                        sb.AppendFormat("T_MB.MB_NRx{0} AS [Product_Nrx{0}], ", x);

                    sb.Append("T_MB.MB_NRx_Sum AS [Product_Nrx_Sum], ");
                    sb.Append("T_MB.Tier_No, ");
                    sb.Append("T_MB.Co_Pay, ");
                    sb.Append("T_MB.PA, ");
                    sb.Append("T_MB.QL, ");
                    sb.Append("T_MB.ST, ");
                    sb.Append("T_MB.Tier_Name, ");
                    sb.Append("T_MB.Plan_Name, ");
                    sb.Append("T_MB.Total_Covered, ");
                    sb.Append("T_MB.Product_Name, ");

                    //Add column name if selecting Top X Plans (to match unioned query)
                    if (rollup == 2 || rollup == 3)
                        sb.Append("0 AS [Product_ID], ");

                    sb.Append("T_MB.Geography_Name ");
                    sb.Append("FROM ( ");
                    sb.Append("SELECT TOP 100 PERCENT ");
                    sb.Append("0 AS [Product_ID], ");
                    sb.Append("0 AS [Drug_ID], ");
                    sb.Append("T1.*, ");
                    sb.Append("NULL AS [Tier_No], ");
                    sb.Append("'' AS [Co_Pay], ");
                    sb.Append("'' AS [PA], ");
                    sb.Append("'' AS [QL], ");
                    sb.Append("'' AS [ST], ");
                    sb.Append("'' AS [Tier_Name], ");
                    sb.Append("p.Plan_Name, ");
                    //sb.Append("p.Total_Covered, ");
                    if (Convert.ToInt32(queryVals["Section_ID"]) == 9)
                    {
                        sb.Append("p.Medicaid_Enrollment-p.Medicaid_Mcare_Enrollment as Total_Covered, ");
                    }
                    else
                    {
                        if (segmentID == "3")
                        {
                            sb.Append("p.Medicaid_Enrollment as Total_Covered, ");
                        }
                        else if (segmentID == "1")
                        {
                            sb.Append("p.Total_Pharmacy-p.Total_Medicare_PartD as Total_Covered, ");
                        }
                        else if (segmentID == "2")
                        {
                            sb.Append("p.Total_Medicare_PartD as Total_Covered, ");
                        }
                        else { sb.Append("p.Total_Pharmacy as Total_Covered, "); }
                    }
                    sb.Append("'Total MB' AS [Product_Name], ");
                    sb.Append("PF.dbo.Lkp_Geographies.Geography_Name ");
                    sb.Append("from ");
                    sb.Append("PF.dbo.Plan_Drug_Formulary ");
                    sb.Append("right outer join (");

                    //Remove Product_ID, Drug_ID and MB_Nrx keys to prevent duplicates (selected as zero above to preserve column names)
                    IList<string> mbNrxKeys = new List<string>();

                    foreach (string s in keys)
                        mbNrxKeys.Add(s);

                    mbNrxKeys.Remove("Product_ID");
                    mbNrxKeys.Remove("Drug_ID");
                    mbNrxKeys.Remove("MB_Nrx");

                    //Add flag in queryVals to pivot query on MB_Nrx in 'ConstructMAPivotQuery'
                    NameValueCollection mbQueryVals = new NameValueCollection(queryVals);
                    mbQueryVals.Add("Is_MB_Nrx", "true");

                    string mbNrxQuery = ConstructMAPivotQuery(mbNrxComm, dataYear, geographyID, timeFrameVals, tableName, dataField, mbNrxKeys, isDetailed, productID, mbQueryVals).IncludeSum().ToString();

                    //Add MB Trx pivot query
                    sb.Append(mbNrxQuery);
                    sb.Append(") as T1 on ");
                    sb.Append("PF.dbo.Plan_Drug_Formulary.Is_Predominant = 1 inner join ");
                    sb.Append("pf.dbo.plans p on p.Plan_ID = T1.Plan_ID inner join ");
                    sb.Append("PF.dbo.Lkp_Geographies on PF.dbo.Lkp_Geographies.Geography_ID = p.Plan_State ");
                    sb.Append(") AS T_MB ");
                    sb.Append("UNION ");

                    //Add existing pivot query
                    sb.Append(comm.CommandText);
                    sb.Append(") as TableX ");
                    sb.Append("ORDER BY Plan_Name, Product_Name, SortCol");

                    comm.CommandText = sb.ToString();
                }


                //end of nrx calculation
                #endregion


                //Wrap query for paging
                string pagingEnabled = queryVals["PagingEnabled"];
                if (!string.IsNullOrEmpty(pagingEnabled))
                {
                    if (Convert.ToBoolean(pagingEnabled) == true)
                    {
                        int startPage = Convert.ToInt32(queryVals["StartPage"]);
                        int totalPerPage = Convert.ToInt32(queryVals["TotalPerPage"]);

                        int rowNumber = (startPage * totalPerPage) - totalPerPage;

                        StringBuilder sb = new StringBuilder();
                        sb.AppendFormat("SELECT TOP {0} * FROM ", totalPerPage);
                        sb.Append("(SELECT ");
                        sb.Append("ROW_NUMBER() OVER (Order By Geography_Name ASC, Plan_Name ASC, SortCol ASC, Product_Name ASC, Segment_Name ASC, Tier_Name ASC) ");
                        sb.Append("AS RowNumber, * ");
                        sb.AppendFormat("FROM ({0}) AS Results2) AS Results WHERE RowNumber > {1} ORDER BY RowNumber, Product_Name, Segment_Name", comm.CommandText, rowNumber);

                        comm.CommandText = sb.ToString();
                    }
                    else if(string.IsNullOrEmpty(queryVals["Export"]))//Only select count if not export
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendFormat("SELECT COUNT(0) AS 'RowCount' FROM ({0}) AS FullCount", comm.CommandText);

                        comm.CommandText = sb.ToString();
                    }
                }

                conn.Open();
                DbDataReader rdr = comm.ExecuteReader();
                IEnumerable<GenericDataRecord> g = Pinsonault.Data.GenericDataRecord.CreateCollection(rdr);
                conn.Close();

                return g;
            }
            catch (Exception x)
            {
                throw x;
            }
            finally
            {
                if (conn != null) conn.Dispose();
                if (comm != null) comm.Dispose();
            }
        }

        public IEnumerable<GenericDataRecord> GetDataPrescriber(IList<int> timeFrameVals, string strTimeFrame, bool isMonth, string dataYear, string tableName, string topN_tableName, string dataField, IList<string> keys, NameValueCollection queryVals)
        {
            //Query for records
            SqlConnection conn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString);
            SqlCommand comm = new SqlCommand();
            Pinsonault.Data.SQLUtil.MaxFragmentLength = 40;
            SQLPivotQuery<int> query = null;

            string trxMst = queryVals["Trx_Mst"];

            if (string.IsNullOrEmpty(trxMst))
                trxMst = "Trx";

            List<int> timeFrame = timeFrameVals.ToList();

            //If Rolling Quarterly, reverse time frame values
            if (string.Compare(queryVals["Calendar_Rolling"], "Rolling", true) == 0)
                timeFrame.Reverse();

            query = SQLPivotQuery<int>.Create(tableName, null, "tr", keys, dataField, timeFrame);

            query.Where("Plan_ID", "PlanID", SQLOperator.EqualTo);
            comm.Parameters.Add("@PlanID", SqlDbType.Int).Value = Convert.ToInt32(queryVals["Plan_ID"]);    
            
            //Add year only if Calendar type query
            if (string.Compare(queryVals["Calendar_Rolling"], "Calendar", true) == 0)
            {
                query.Where("Data_Year", "DataYear", SQLOperator.EqualTo);
                comm.Parameters.Add("@DataYear", SqlDbType.Int).Value = Convert.ToInt32(dataYear);    
            }

            //Check if it is TRX or MST
            if (string.Compare(trxMst, "trx", true) == 0)
                query.Pivot(SQLFunction.SUM, "Product_Trx");
            else if (string.Compare(trxMst, "nrx", true) == 0)
            {
                query.Pivot(SQLFunction.SUM, "Product_Nrx");
            }
            else if (string.Compare(trxMst, "msn", true) == 0)
            {
                query.Pivot(SQLFunction.SUM, "Product_Msn");                
            }
            else
                query.Pivot(SQLFunction.SUM, "Product_Mst");

            ////
            //Add parameters and constrain query below
            ////

            //Check if Channel is Combined (Commercial + Part D)
            string segmentID = "1";
            if (!string.IsNullOrEmpty(queryVals["qSegment_ID"]))
            {
                comm.Parameters.Add("@SegmentID", SqlDbType.Int).Value = Convert.ToInt32(queryVals["qSegment_ID"]);    
                query.Where("Segment_ID", "SegmentID", SQLOperator.EqualTo);
            }
            else
            {
                if (queryVals["Section_ID"] == "-1") //Channel is Combined - use segment ID of 1 and 2
                {
                    comm.Parameters.Add("@SegmentID0", SqlDbType.Int).Value = 2;    
                    comm.Parameters.Add("@SegmentID1", SqlDbType.Int).Value = 1;    

                    query.Where("Segment_ID", "SegmentID", SQLOperator.In, 2);
                    segmentID = "1,2";
                }
                else //Channel is not Combined
                {
                    //Query Seqment_ID: Segment_ID is 2 for Med-D, 3 for State Medicaid or 1 for all else
                    if (Convert.ToInt32(queryVals["Section_ID"]) == 17) //Med-D
                    {
                        comm.Parameters.Add("@SegmentID", SqlDbType.Int).Value = 2; 
                        segmentID = "2";
                    }
                    else if (Convert.ToInt32(queryVals["Section_ID"]) == 9) //State Medicaid
                    {
                        comm.Parameters.Add("@SegmentID", SqlDbType.Int).Value = 3; 
                        segmentID = "3";
                    }
                    else
                        comm.Parameters.Add("@SegmentID", SqlDbType.Int).Value = 1;  //All Else

                    query.Where("Segment_ID", "SegmentID", SQLOperator.EqualTo);
                }
            }

            //Region
            string RegionID = "";
            if (!string.IsNullOrEmpty(queryVals["Phy_Region_ID"]) && queryVals["Phy_Region_ID"] != "all")
            {
                RegionID = queryVals["Phy_Region_ID"];
                comm.Parameters.Add("@RegionID", SqlDbType.VarChar).Value = queryVals["Phy_Region_ID"]; 
                query.Where("Region_Name", "RegionID", SQLOperator.EqualTo);
            }

            //District
            string DistrictID = "";
            if (!string.IsNullOrEmpty(queryVals["Phy_District_ID"]) && queryVals["Phy_District_ID"] != "all")
            {
                DistrictID = queryVals["Phy_District_ID"];
                comm.Parameters.Add("@DistrictID", SqlDbType.VarChar).Value = queryVals["Phy_District_ID"]; 
                query.Where("District_Name", "DistrictID", SQLOperator.EqualTo);
            }

            //Territory
            string TerritoryID = "";
            if (!string.IsNullOrEmpty(queryVals["Phy_Territory_ID"]) && queryVals["Phy_Territory_ID"] != "all")
            {
                TerritoryID = queryVals["Phy_Territory_ID"];
                comm.Parameters.Add("@TerritoryID", SqlDbType.VarChar).Value = queryVals["Phy_Territory_ID"]; 
                query.Where("Territory_Name", "TerritoryID", SQLOperator.EqualTo);
            }

            string drugIDs = queryVals["Product_ID"];
            string[] drugIDarr = drugIDs.Split(',');

            for (int x = 0; x < drugIDarr.Count(); x++)
                comm.Parameters.Add(string.Format("@ProductID{0}", x), SqlDbType.Int).Value = Convert.ToInt32(drugIDarr[x]); 

            query.Where("Product_ID", "ProductID", SQLOperator.In, drugIDarr.Count());

            comm.CommandText = query.IncludeSum().ToString();
            comm.Connection = conn;

            // TopN
            int topN;
            if (!string.IsNullOrEmpty(queryVals["topN"]))
                topN = Convert.ToInt32(queryVals["topN"]);
            else
                topN = 100;  //as default

            // rcbProduct
            int selectedProduct;
            if (!string.IsNullOrEmpty(queryVals["SelectedProduct"]))
                selectedProduct = Convert.ToInt32(queryVals["SelectedProduct"]);
            else if (!string.IsNullOrEmpty(queryVals["Default_Product_ID"]) && queryVals["Default_Product_ID"] != "0")
                selectedProduct = Convert.ToInt32(queryVals["Default_Product_ID"]);  //as default
            else
                selectedProduct = Convert.ToInt32(drugIDarr[0]);

            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT TOP 100 PERCENT ");
                sb.Append("T1.*,T2.Trx_Or_Mst_Total ");
                sb.AppendFormat("from ({0}) as T1 ", comm.CommandText);

                // to select TopN
                StringBuilder sb2 = new StringBuilder();
                StringBuilder where = new StringBuilder();
                where.Append(" Plan_ID= @PlanID ");
                if (string.Compare(queryVals["Calendar_Rolling"], "Calendar", true) == 0)
                    where.Append(" and Data_Year = @DataYear ");

                // use strTimeFrame,  can't use timeFrame: List<int> 
                where.AppendFormat(" and {0} in ({1})", dataField, strTimeFrame);
                where.AppendFormat(" and Segment_ID in ({0})", segmentID);

                where.Append(" and Product_ID = @selectedProduct ");
                comm.Parameters.Add("@selectedProduct", SqlDbType.Int).Value = Convert.ToInt32(selectedProduct); 

                sb2.AppendFormat("(SELECT TOP {0} ", topN);

                //Check if it is TRX or MST
                if (string.Compare(trxMst, "trx", true) == 0)
                    sb2.AppendFormat(" Phys_IMS_ID as Physician_ID, sum([Product_Trx]) as Trx_Or_Mst_Total ");
                else if (string.Compare(trxMst, "nrx", true) == 0)
                {
                    sb2.AppendFormat(" Phys_IMS_ID as Physician_ID, sum([Product_Nrx]) as Trx_Or_Mst_Total ");
                }
                else if (string.Compare(trxMst, "msn", true) == 0)
                {
                    sb2.AppendFormat(" Phys_IMS_ID as Physician_ID, sum([Product_Msn]) as Trx_Or_Mst_Total ");
                }
                else
                    sb2.AppendFormat(" Phys_IMS_ID as Physician_ID, sum([Product_Mst]) as Trx_Or_Mst_Total ");

                sb2.AppendFormat("from tr.{0} ", topN_tableName);
                sb2.AppendFormat("where {0} ", where.ToString());
                sb2.AppendFormat("group by Phys_IMS_ID order by Trx_or_Mst_Total desc, Physician_ID) as T2 ");
                sb2.AppendFormat("on T1.Physician_ID = T2.Physician_ID ");

                sb.AppendFormat("inner join {0} ", sb2.ToString());
                //sb.AppendFormat(" order by Trx_Or_Mst_Total desc,Physician_Name, Physician_ID, Product_Name ");
                // 5/24/2011 updated
                sb.AppendFormat(" order by Trx_Or_Mst_Total desc, Region_Name, District_Name, Territory_Name, Physician_Name, Physician_ID, Product_Name ");
                
                comm.CommandText = sb.ToString();
                conn.Open();
                DbDataReader rdr = comm.ExecuteReader();
                IEnumerable<GenericDataRecord> g = Pinsonault.Data.GenericDataRecord.CreateCollection(rdr);

                conn.Close();

                return g;
            }
            catch (Exception x)
            {
                throw x;
            }
            finally
            {
                if (conn != null) conn.Dispose();
                if (comm != null) comm.Dispose();
            }
        }

        public IEnumerable<GenericDataRecord> GetDataPrescriberTrending(IList<int> timeFrameVals, string strTimeFrame, bool isMonth, string dataYear, string tableName, string dataField, IList<string> keys, NameValueCollection queryVals)
        {
            //Query for records
            SqlConnection conn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString);
            SqlCommand comm = new SqlCommand();
            Pinsonault.Data.SQLUtil.MaxFragmentLength = 50;
            SQLPivotQuery<int> query = null;

            string trxMst = queryVals["Trx_Mst"];

            if (string.IsNullOrEmpty(trxMst))
                trxMst = "Trx";

            List<int> timeFrame = timeFrameVals.ToList();

            //If Rolling Quarterly, reverse time frame values
            if (string.Compare(queryVals["Calendar_Rolling"], "Rolling", true) == 0)
                timeFrame.Reverse();

            query = SQLPivotQuery<int>.Create(tableName, null, "tr", keys, dataField, timeFrame);

            if (!string.IsNullOrEmpty(queryVals["Plan_ID"]))
            {
                query.Where("Plan_ID", "PlanID", SQLOperator.EqualTo);
                comm.Parameters.Add("@PlanID", SqlDbType.Int).Value = Convert.ToInt32(queryVals["Plan_ID"]); 
            }

            //Filter on Market Basket only for Prescriber Trending
            if (!string.IsNullOrEmpty(queryVals["Market_Basket_ID"]))
            {
                query.Where("Thera_ID", "TheraID", SQLOperator.EqualTo);
                comm.Parameters.Add("@TheraID", SqlDbType.Int).Value = Convert.ToInt32(queryVals["Market_Basket_ID"]); 
            }

            //Add year only if Calendar type query
            if (string.Compare(queryVals["Calendar_Rolling"], "Calendar", true) == 0)
            {
                query.Where("Data_Year", "DataYear", SQLOperator.EqualTo);
                comm.Parameters.Add("@DataYear", SqlDbType.Int).Value = dataYear; 
            }

            //Check if it is TRX or MST
            if (string.Compare(trxMst, "trx", true) == 0)
                query.Pivot(SQLFunction.SUM, "Product_Trx");
            else if (string.Compare(trxMst, "nrx", true) == 0)
            {
                query.Pivot(SQLFunction.SUM, "Product_Nrx");
            }
            else if (string.Compare(trxMst, "msn", true) == 0)
            {
                query.Pivot(SQLFunction.SUM, "Product_Msn");                
            }
            else
                query.Pivot(SQLFunction.SUM, "Product_Mst");

            ////
            //Add parameters and constrain query below
            ////

            //Check if Channel is Combined (Commercial + Part D)
            string segmentID = "1";
            if (!string.IsNullOrEmpty(queryVals["qSegment_ID"]))
            {
                comm.Parameters.Add("@SegmentID", SqlDbType.Int).Value = Convert.ToInt32(queryVals["qSegment_ID"]); 
                query.Where("Segment_ID", "SegmentID", SQLOperator.EqualTo);
            }
            else
            {
                if (!string.IsNullOrEmpty(queryVals["Section_ID"]))
                {
                    if (queryVals["Section_ID"] == "-1") //Channel is Combined - use segment ID of 1 and 2
                    {
                        comm.Parameters.Add("@SegmentID0", SqlDbType.Int).Value = 2;
                        comm.Parameters.Add("@SegmentID1", SqlDbType.Int).Value = 1;

                        query.Where("Segment_ID", "SegmentID", SQLOperator.In, 2);
                        segmentID = "1,2";
                    }
                    else //Channel is not Combined
                    {
                        //Query Seqment_ID: Segment_ID is 2 for Med-D, 3 for State Medicaid or 1 for all else
                        if (Convert.ToInt32(queryVals["Section_ID"]) == 17) //Med-D
                        {
                            comm.Parameters.Add("@SegmentID", SqlDbType.Int).Value = 2;
                            segmentID = "2";
                        }
                        else if (Convert.ToInt32(queryVals["Section_ID"]) == 9) //State Medicaid
                        {
                            comm.Parameters.Add("@SegmentID", SqlDbType.Int).Value = 3;
                            segmentID = "3";
                        }
                        else
                            comm.Parameters.Add("@SegmentID", SqlDbType.Int).Value = 1; //All Else

                        query.Where("Segment_ID", "SegmentID", SQLOperator.EqualTo);
                    }
                }
            }


            //Region, District, Territory for Prescriber Trending
            if (!string.IsNullOrEmpty(queryVals["Selection_Clicked"]))
            {
                string prescriberGeographyType = string.Empty;
                string geographyID = string.Empty;
                if (string.Compare(queryVals["Selection_Clicked"], "1", true) == 0 && queryVals["Region_ID"] != "all")
                {
                    prescriberGeographyType = "region";
                    geographyID = queryVals["Region_ID"];
                    comm.Parameters.Add("@RegionID", SqlDbType.VarChar).Value = queryVals["Region_ID"];
                    query.Where("Region_ID", "RegionID", SQLOperator.EqualTo);
                }
                if (string.Compare(queryVals["Selection_Clicked"], "2", true) == 0 && queryVals["District_ID"] != "all")
                {
                    prescriberGeographyType = "district";
                    geographyID = queryVals["District_ID"];
                    comm.Parameters.Add("@DistrictID", SqlDbType.VarChar).Value = queryVals["District_ID"];
                    query.Where("District_ID", "DistrictID", SQLOperator.EqualTo);
                }
                if (string.Compare(queryVals["Selection_Clicked"], "3", true) == 0 && queryVals["Territory_ID"] != "all")
                {
                    prescriberGeographyType = "territory";
                    geographyID = queryVals["Territory_ID"];
                    comm.Parameters.Add("@TerritoryID", SqlDbType.VarChar).Value = queryVals["Territory_ID"];
                    query.Where("Territory_ID", "TerritoryID", SQLOperator.EqualTo);
                }

                //Check Rollup type
                rollup = Convert.ToInt32(queryVals["Rollup_Type"]);

                if (rollup == 5 || rollup == 6)
                {
                    IList<string> physIDArr = GetTopPhysicians(tableName, queryVals, prescriberGeographyType, geographyID, dataField, dataYear, timeFrameVals);

                    if (physIDArr.Count() > 0)
                    {
                        for (int x = 0; x < physIDArr.Count(); x++)
                            comm.Parameters.Add(string.Format("@PhysicianID{0}", x), SqlDbType.VarChar).Value = physIDArr[x];

                        query.Where("Physician_ID", "PhysicianID", SQLOperator.In, physIDArr.Count());
                    }
                }
            }


            string drugIDs = queryVals["Product_ID"];
            string[] drugIDarr = drugIDs.Split(',');

            for (int x = 0; x < drugIDarr.Count(); x++)
                comm.Parameters.Add(string.Format("@ProductID{0}", x), SqlDbType.Int).Value = Convert.ToInt32(drugIDarr[x]);

            query.Where("Product_ID", "ProductID", SQLOperator.In, drugIDarr.Count());

            comm.CommandText = query.IncludeSum().ToString();
            comm.Connection = conn;

            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT TOP 100 PERCENT ");
                sb.Append("T1.* ");
                sb.AppendFormat("from ({0}) as T1 ", comm.CommandText);
                sb.AppendFormat(" order by Physician_Name, Physician_ID, Product_Name ");

                comm.CommandText = sb.ToString();

                //Wrap query for paging
                string pagingEnabled = queryVals["PagingEnabled"];
                if (!string.IsNullOrEmpty(pagingEnabled))
                {
                    if (Convert.ToBoolean(pagingEnabled) == true)
                    {
                        int startPage = Convert.ToInt32(queryVals["StartPage"]);
                        int totalPerPage = Convert.ToInt32(queryVals["TotalPerPage"]);

                        int rowNumber = (startPage * totalPerPage) - totalPerPage;

                        StringBuilder sb3 = new StringBuilder();
                        sb3.AppendFormat("SELECT TOP {0} * FROM ", totalPerPage);
                        sb3.Append("(SELECT ");
                        sb3.Append("ROW_NUMBER() OVER (Order By Physician_Name, Physician_ID, Product_Name) ");
                        sb3.Append("AS RowNumber, * ");
                        sb3.AppendFormat("FROM ({0}) AS Results2) AS Results WHERE RowNumber > {1} ORDER BY RowNumber, Physician_Name, Physician_ID, Product_Name", comm.CommandText, rowNumber);

                        comm.CommandText = sb3.ToString();
                    }
                    else if (string.IsNullOrEmpty(queryVals["Export"]))//Only select count if not export
                    {
                        StringBuilder sb3 = new StringBuilder();
                        sb3.AppendFormat("SELECT COUNT(0) AS 'RowCount' FROM ({0}) AS FullCount", comm.CommandText);

                        comm.CommandText = sb3.ToString();
                    }
                }

                conn.Open();
                DbDataReader rdr = comm.ExecuteReader();
                IEnumerable<GenericDataRecord> g = Pinsonault.Data.GenericDataRecord.CreateCollection(rdr);

                conn.Close();

                return g;
            }
            catch (Exception x)
            {
                throw x;
            }
            finally
            {
                if (conn != null) conn.Dispose();
                if (comm != null) comm.Dispose();
            }
        }

        public IEnumerable<GenericDataRecord> GetFHData(IList<int> timeFrameVals, bool isMonth, string tableName, string dataField, IList<string> keys, NameValueCollection queryVals)
        {
            //Query for records
            SqlConnection conn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString);
            SqlCommand comm = new SqlCommand();

            comm.CommandText = ConstructMAPivotQuery(comm, null, "US", timeFrameVals, tableName, dataField, keys, false, null, queryVals).ToString();

            comm.Connection = conn;

            try
            {
                //Clean Product_ID in case of query string hijack
                string[] productIDs = queryVals["Product_ID"].Split(',');

                for (int x = 0; x < productIDs.Count(); x++)
                {
                    int cleanID = Convert.ToInt32(productIDs[x]);

                    //Place clean id back into string array
                    productIDs[x] = cleanID.ToString();
                }                

                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT ") ;
                sb.Append("mbTable.*, ");
                sb.Append("fhTable.Plan_ID, ");
                //sb.Append("fhTable.Formulary_ID, ");
                sb.Append("fhTable.Product_Name, ");
                sb.Append("CASE WHEN CONVERT(varchar(20),fhTable.Tier_Name0) = '0' THEN NULL ELSE fhTable.Tier_Name0 END AS Tier_Name0, ");
                sb.Append("CASE WHEN CONVERT(varchar(20),fhTable.Tier_Name1) = '0' THEN NULL ELSE fhTable.Tier_Name1 END AS Tier_Name1, ");
                sb.Append("fhTable.TierChanged, ");
                sb.Append("CASE WHEN fhTable.PA0 = 1 THEN 'PA' ELSE NULL END AS PA0, ");
                sb.Append("CASE WHEN fhTable.PA1 = 1 THEN 'PA' ELSE NULL END AS PA1, ");
                sb.Append("fhTable.PAChanged, ");
                sb.Append("CASE WHEN fhTable.QL0 = 1 THEN 'QL' ELSE NULL END AS QL0, ");
                sb.Append("CASE WHEN fhTable.QL1 = 1 THEN 'QL' ELSE NULL END AS QL1, ");
                sb.Append("fhTable.QLChanged, ");
                sb.Append("CASE WHEN fhTable.ST0 = 1 THEN 'ST' ELSE NULL END AS ST0, ");
                sb.Append("CASE WHEN fhTable.ST1 = 1 THEN 'ST' ELSE NULL END AS ST1, ");
                sb.Append("fhTable.STChanged, ");
                sb.Append("CASE WHEN CONVERT(varchar(20),fhTable.Co_Pay0) = '0' THEN NULL ELSE fhTable.Co_Pay0 END AS Co_Pay0, ");
                sb.Append("CASE WHEN CONVERT(varchar(20),fhTable.Co_Pay1) = '0' THEN NULL ELSE fhTable.Co_Pay1 END AS Co_Pay1, ");
                sb.Append("fhTable.CopayChanged, ");
                sb.Append("CASE WHEN CONVERT(varchar(20),fhTable.Formulary_Status_Abbr0) = '0' THEN NULL ELSE fhTable.Formulary_Status_Abbr0 END AS Formulary_Status_Abbr0, ");
                sb.Append("CASE WHEN CONVERT(varchar(20),fhTable.Formulary_Status_Abbr1) = '0' THEN NULL ELSE fhTable.Formulary_Status_Abbr1 END AS Formulary_Status_Abbr1, ");
                sb.Append("fhTable.FSChanged ");
                sb.Append("FROM (");

                //Wrap query for MST calculations
                if (string.Compare(queryVals["Trx_Mst"], "mst", true) == 0)
                {
                    IList<string> colsTrxToSum = new List<string>();
                    IList<string> colsMBTrxToSum = new List<string>();
                    StringBuilder mstsb = new StringBuilder();
                    mstsb.Append("SELECT TOP 100 PERCENT *, ");

                    for (int x = 0; x < timeFrame.Count; x++)
                    {
                        mstsb.AppendFormat("Product_Mst{0} = CASE ISNULL(MB_Trx{0}, 0) WHEN 0 THEN 0.000 ELSE (Product_Trx{0} / MB_Trx{0}) * 100 END,", x);

                        colsTrxToSum.Add(string.Format("Product_Trx{0}", x));
                        colsMBTrxToSum.Add(string.Format("MB_Trx{0}", x));
                    }

                    mstsb.AppendFormat("Product_Mst_Sum = CASE({0}) WHEN 0 THEN 0.000 ELSE (({1})/({2})) END ", string.Join(" + ", colsMBTrxToSum.ToArray()), string.Join(" + ", colsTrxToSum.ToArray()), string.Join(" + ", colsMBTrxToSum.ToArray()));
                    mstsb.AppendFormat("FROM ({0}) AS T2", comm.CommandText);

                    //Remove Drug_ID from WHERE clause in case Product is in results
                    comm.CommandText = mstsb.ToString().Replace("and pivotTable0.Drug_ID = pivotTable1.Drug_ID", "");
                }

                sb.Append(comm.CommandText);
                sb.Append(") AS mbTable INNER JOIN(");

                keys.Clear();
                keys.Add("Plan_ID");
                keys.Add("Product_ID");
                keys.Add("Product_Name");

                string tierQuery = ConstructFHRPivotQuery(comm, "Tier_Name", timeFrameVals[0], "V_ProductFormularyHistoryDataModal", dataField, keys, string.Join(",",productIDs), queryVals).ToString();
                string formularyStatusQUery = ConstructFHRPivotQuery(comm, "Formulary_Status_Abbr", timeFrameVals[0], "V_ProductFormularyHistoryDataModal", dataField, keys, string.Join(",", productIDs), queryVals).ToString();
                string paQuery = ConstructFHRPivotQuery(comm, "PA", timeFrameVals[0], "V_ProductFormularyHistoryDataModal", dataField, keys, string.Join(",", productIDs), queryVals).ToString();
                string qlQuery = ConstructFHRPivotQuery(comm, "QL", timeFrameVals[0], "V_ProductFormularyHistoryDataModal", dataField, keys, string.Join(",", productIDs), queryVals).ToString();
                string stQuery = ConstructFHRPivotQuery(comm, "ST", timeFrameVals[0], "V_ProductFormularyHistoryDataModal", dataField, keys, string.Join(",", productIDs), queryVals).ToString();
                
                //Clear parameters from all above queries since they are all duplicates
                comm.Parameters.Clear();

                //Add parameters for base MB query that are not present in FHR query
                comm.Parameters.Add("@GeographyID", SqlDbType.VarChar).Value = "US";
                comm.Parameters.Add("@PlanID0", SqlDbType.Int).Value = Convert.ToInt32(queryVals["Plan_ID"]);
                comm.Parameters.Add("@MarketBasketID", SqlDbType.Int).Value = Convert.ToInt32(queryVals["Market_Basket_ID"]);

                string copayQuery = ConstructFHRPivotQuery(comm, "Co_Pay", timeFrameVals[0], "V_ProductFormularyHistoryDataModal", dataField, keys, string.Join(",", productIDs), queryVals).ToString();

                //StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * ");
                sb.Append("FROM (");
                sb.Append("SELECT tiertable.Plan_ID, ");
                sb.Append("tiertable.Product_ID, ");
                sb.Append("tiertable.Product_Name, ");
                sb.Append("tiertable.Tier_Name0, ");
                sb.Append("tiertable.Tier_Name1, ");
                sb.Append("CASE WHEN tiertable.Tier_Name0 = tiertable.Tier_Name1 THEN 0 ELSE 1 END AS TierChanged, ");
                sb.Append("patable.PA0, ");
                sb.Append("patable.PA1, ");
                sb.Append("CASE WHEN patable.PA0 = patable.PA1 THEN 0 ELSE 1 END AS PAChanged, ");
                sb.Append("qltable.QL0, ");
                sb.Append("qltable.QL1, ");
                sb.Append("CASE WHEN qltable.QL0 = qltable.QL1 THEN 0 ELSE 1 END AS QLChanged, ");
                sb.Append("sttable.ST0, ");
                sb.Append("sttable.ST1, ");
                sb.Append("CASE WHEN sttable.ST0 = sttable.ST1 THEN 0 ELSE 1 END AS STChanged, ");
                sb.Append("copaytable.Co_Pay0, ");
                sb.Append("copaytable.Co_Pay1, ");
                sb.Append("CASE WHEN copaytable.Co_Pay0 = copaytable.Co_Pay1 THEN 0 ELSE 1 END AS CopayChanged, ");
                sb.Append("fstable.Formulary_Status_Abbr0, ");
                sb.Append("fstable.Formulary_Status_Abbr1, ");
                sb.Append("CASE WHEN fstable.Formulary_Status_Abbr0 = fstable.Formulary_Status_Abbr1 THEN 0 ELSE 1 END AS FSChanged ");
                sb.Append("FROM ");
                sb.AppendFormat("(SELECT * FROM ({0}) AS tiertable) AS tiertable INNER JOIN ", tierQuery);
                sb.AppendFormat("(SELECT * FROM ({0}) AS patable) AS patable ON ", paQuery.Replace(",PA ", ", CAST(PA AS tinyint) AS PA "));
                sb.Append("tiertable.Plan_ID = patable.Plan_ID AND ");
                sb.Append("tiertable.Product_ID = patable.Product_ID INNER JOIN ");
                sb.AppendFormat("(SELECT * FROM ({0}) AS qltable) AS qltable ON ", qlQuery.Replace(",QL ", ", CAST(QL AS tinyint) AS QL "));
                sb.Append("tiertable.Plan_ID = qltable.Plan_ID AND ");                
                sb.Append("tiertable.Product_ID = qltable.Product_ID INNER JOIN ");                
                sb.AppendFormat("(SELECT * FROM ({0}) AS sttable) AS sttable ON ", stQuery.Replace(",ST ", ", CAST(ST AS tinyint) AS ST "));
                sb.Append("tiertable.Plan_ID = sttable.Plan_ID AND ");                
                sb.Append("tiertable.Product_ID = sttable.Product_ID INNER JOIN ");                
                sb.AppendFormat("(SELECT * FROM ({0}) AS copaytable) AS copaytable ON ", copayQuery);
                sb.Append("tiertable.Plan_ID = copaytable.Plan_ID AND ");                
                sb.Append("tiertable.Product_ID = copaytable.Product_ID INNER JOIN ");                
                sb.AppendFormat("(SELECT * FROM ({0}) AS fstable) AS fstable ON ", formularyStatusQUery);
                sb.Append("tiertable.Plan_ID = fstable.Plan_ID AND ");
                sb.Append("tiertable.Product_ID = fstable.Product_ID");
                sb.Append(") AS ChangeTable");

                sb.Append(") AS fhTable ON mbTable.Product_ID = fhTable.Product_ID ORDER BY Product_Name");

                comm.CommandText = sb.ToString();

                conn.Open();
                DbDataReader rdr = comm.ExecuteReader();

                IEnumerable<GenericDataRecord> g = Pinsonault.Data.GenericDataRecord.CreateCollection(rdr);
                conn.Close();

                return g;
            }
            catch (Exception x)
            {
                throw x;
            }
            finally
            {
                if (conn != null) conn.Dispose();
                if (comm != null) comm.Dispose();
            }
        }

        public SQLPivotQuery<int> GetPivotPlanDataQuery(SqlCommand comm, string dataYear, IList<int> timeFrameVals, string tableName, string dataField, IList<string> keys, bool isDetailed, int? productID, NameValueCollection queryVals)
        {
            //Query for records
            SqlConnection conn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString);
           
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(comm);
            int rollup = Convert.ToInt32(queryVals["Rollup_Type"]);

            SQLPivotQuery<int> query = null;

            string trxMst = queryVals["Trx_Mst"];

            string segmentID = "1";

            if (string.IsNullOrEmpty(trxMst))
                trxMst = "Trx";

            List<int> timeFrame = timeFrameVals.ToList();

            //Check if query is for summary or detailed grid
            if (isDetailed)
            {

                query = SQLPivotQuery<int>.Create(tableName, string.Format("PF_{0}", Pinsonault.Web.Session.ClientKey), "tr", keys, dataField, timeFrame);
                //.Where("Thera_ID", "MarketBasketID", SQLOperator.EqualTo);

                //Query can only be restrained by product id for detailed grid
                if (productID != null)
                {
                    query.Where("Product_ID", "ProductID", SQLOperator.EqualTo);
                    comm.Parameters.Add("@ProductID", SqlDbType.Int).Value = productID;
                }
            }
            else
            {
                //If Rolling Quarterly, reverse time frame values
                if (string.Compare(queryVals["Calendar_Rolling"], "Rolling", true) == 0)
                    timeFrame.Reverse();

                query = SQLPivotQuery<int>.Create(tableName, null, "tr", keys, dataField, timeFrame);
            }


            //Add year only if Calendar type query
            if (string.Compare(queryVals["Calendar_Rolling"], "Calendar", true) == 0)
            {
                query.Where("Data_Year", "DataYear", SQLOperator.EqualTo);
                comm.Parameters.Add("@DataYear", SqlDbType.Int).Value = dataYear;
            }
            if (!string.IsNullOrEmpty(queryVals["Plan_ID"]))
            {
                query.Where("Parent_ID", "Plan_ID", SQLOperator.EqualTo);
                comm.Parameters.Add("@Plan_ID", SqlDbType.Int).Value = Convert.ToInt32(queryVals["Plan_ID"]);
            }

            //Check if it is TRX or MST   
            if ((!string.IsNullOrEmpty(queryVals["Is_MB_Trx"])) && Convert.ToBoolean(queryVals["Is_MB_Trx"]))
                query.Pivot(SQLFunction.AVG, "MB_TRx");
            else if ((!string.IsNullOrEmpty(queryVals["Is_MB_Nrx"])) && Convert.ToBoolean(queryVals["Is_MB_Nrx"]))
                query.Pivot(SQLFunction.AVG, "MB_NRx");
            else
                query.Pivot(SQLFunction.SUM, "MB_Trx");           

            //add section and segment in query
            if (queryVals["Section_ID"] == "-1") //Channel is Combined - use segment ID of 1 and 2
            {
                comm.Parameters.Add("@SegmentID0", SqlDbType.Int).Value = 2;
                comm.Parameters.Add("@SegmentID1", SqlDbType.Int).Value = 1;

                query.Where("Segment_ID", "SegmentID", SQLOperator.In, 2);

                segmentID = "1,2";
            }
            else
            {
                if (Convert.ToInt32(queryVals["Section_ID"]) == 17) //Med-D, Section ID is not passed in case of Med-D
                {
                    comm.Parameters.Add("@SegmentID", SqlDbType.Int).Value = 2;
                    segmentID = "2";
                }
                else
                {
                    if (Convert.ToInt32(queryVals["Section_ID"]) == 9) //State Medicaid
                    {
                        comm.Parameters.Add("@SegmentID", SqlDbType.Int).Value = 3;
                        segmentID = "3";
                    }
                    else
                        comm.Parameters.Add("@SegmentID", SqlDbType.Int).Value = 1; //All Else
                }

                query.Where("Segment_ID", "SegmentID", SQLOperator.EqualTo);
            }

            //Add drugs to query only if not selecting one product (for drilldown)
            if (productID == null)
            {
                string drugIDs = queryVals["Product_ID"];
                string[] drugIDarr = drugIDs.Split(',');

                for (int x = 0; x < drugIDarr.Count(); x++)
                    comm.Parameters.Add(string.Format("@ProductID{0}", x), SqlDbType.Int).Value = Convert.ToInt32(drugIDarr[x]);

                query.Where("Product_ID", "ProductID", SQLOperator.In, drugIDarr.Count());
            }

            return query;
        }

        public IEnumerable<GenericDataRecord> GetPlanData(IList<int> timeFrameVals, bool isMonth, string dataYear, string tableName, string dataField, IList<string> keys, bool isDetailed, int? productID, NameValueCollection queryVals)
        {
            //Query for records
            SqlConnection conn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString);
            SqlCommand comm = new SqlCommand();
            int rollup = Convert.ToInt32(queryVals["Rollup_Type"]);
            SQLPivotQuery<int> query = null;
            string trxMst = queryVals["Trx_Mst"];
            string segmentID = "1";

            if (string.IsNullOrEmpty(trxMst))
                trxMst = "Trx";

            List<int> timeFrame = timeFrameVals.ToList();

            //Check if query is for summary or detailed grid
            if (isDetailed)
            {
                query = SQLPivotQuery<int>.Create(tableName, string.Format("PF_{0}", Pinsonault.Web.Session.ClientKey), "tr", keys, dataField, timeFrame);

                //Query can only be restrained by product id for detailed grid
                if (productID != null)
                {
                    query.Where("Product_ID", "ProductID", SQLOperator.EqualTo);
                    comm.Parameters.Add("@ProductID", SqlDbType.Int).Value = productID;
                }
            }
            else
            {
                //If Rolling Quarterly, reverse time frame values
                if (string.Compare(queryVals["Calendar_Rolling"], "Rolling", true) == 0)
                    timeFrame.Reverse();

                query = SQLPivotQuery<int>.Create(tableName, null, "tr", keys, dataField, timeFrame);
            }


            //Add year only if Calendar type query
            if (string.Compare(queryVals["Calendar_Rolling"], "Calendar", true) == 0)
            {
                query.Where("Data_Year", "DataYear", SQLOperator.EqualTo);
                comm.Parameters.Add("@DataYear", SqlDbType.Int).Value = dataYear;
            }
            if (!string.IsNullOrEmpty(queryVals["Plan_ID"]))
            {
                query.Where("Parent_ID", "Plan_ID", SQLOperator.EqualTo);
                comm.Parameters.Add("@Plan_ID", SqlDbType.Int).Value = Convert.ToInt32(queryVals["Plan_ID"]);
            }

            //Check if it is TRX or MST or nrx or msn
            if (string.Compare(trxMst, "trx", true) == 0)
                query.Pivot(SQLFunction.SUM, "Product_Trx");
            else if (string.Compare(trxMst, "nrx", true) == 0)
            {
                query.Pivot(SQLFunction.SUM, "Product_Nrx");
            }
            else if (string.Compare(trxMst, "msn", true) == 0)
            {
                query.Pivot(SQLFunction.SUM, "Product_Nrx");
                query.Pivot(SQLFunction.SUM, "MB_Nrx");
            }
            else
            {
                query.Pivot(SQLFunction.SUM, "Product_Trx");
                query.Pivot(SQLFunction.SUM, "MB_Trx");
            }

            //add section and segment in query
            if (queryVals["Section_ID"] == "-1") //Channel is Combined - use segment ID of 1 and 2
            {
                comm.Parameters.Add("@SegmentID0", SqlDbType.Int).Value = 2;
                comm.Parameters.Add("@SegmentID1", SqlDbType.Int).Value = 1;

                query.Where("Segment_ID", "SegmentID", SQLOperator.In, 2);

                segmentID = "1,2";
            }
            else
            {
                if (Convert.ToInt32(queryVals["Section_ID"]) == 17) //Med-D, Section ID is not passed in case of Med-D
                {
                    comm.Parameters.Add("@SegmentID", SqlDbType.Int).Value = 2;
                    segmentID = "2";
                }
                else
                {
                    if (Convert.ToInt32(queryVals["Section_ID"]) == 9) //State Medicaid
                    {
                        comm.Parameters.Add("@SegmentID", SqlDbType.Int).Value = 3;
                        segmentID = "3";
                    }
                    else
                        comm.Parameters.Add("@SegmentID", SqlDbType.Int).Value = 1; //All Else
                }

                query.Where("Segment_ID", "SegmentID", SQLOperator.EqualTo);
            }

            //Add drugs to query only if not selecting one product (for drilldown)
            if (productID == null)
            {
                string drugIDs = queryVals["Product_ID"];
                string[] drugIDarr = drugIDs.Split(',');

                for (int x = 0; x < drugIDarr.Count(); x++)
                    comm.Parameters.Add(string.Format("@ProductID{0}", x), SqlDbType.Int).Value = Convert.ToInt32(drugIDarr[x]);

                query.Where("Product_ID", "ProductID", SQLOperator.In, drugIDarr.Count());
            }

            comm.CommandText = query.IncludeSum().ToString();

            comm.Connection = conn;

            try
            {
                if (isDetailed)//Detailed view logic
                {
                    StringBuilder sb = new StringBuilder();

                    //for all plans
                    sb.Append("SELECT TOP 100 PERCENT ");
                    sb.Append("1 AS [SortCol],");
                    sb.Append("T1.*, ");
                    sb.Append("PF.dbo.Plan_Drug_Formulary.Tier_No, ");
                    sb.Append("PF.dbo.Plan_Drug_Formulary.Co_Pay, ");
                    sb.Append("CASE PF.dbo.Plan_Drug_Formulary.PA WHEN 'True' THEN 'PA' WHEN 'False' THEN '' ELSE '' END AS PA, ");
                    sb.Append("CASE PF.dbo.Plan_Drug_Formulary.QL WHEN 'True' THEN 'QL' WHEN 'False' THEN '' ELSE '' END AS QL, ");
                    sb.Append("CASE PF.dbo.Plan_Drug_Formulary.ST WHEN 'True' THEN 'ST' WHEN 'False' THEN '' ELSE '' END AS ST, ");
                    sb.Append("PF.dbo.Lkp_Tiers.Tier_Name, ");
                    sb.Append("PF.dbo.Plans.Plan_Name, ");
                    //sb.Append("PF.dbo.Plans.Total_Covered, ");
                    if (Convert.ToInt32(queryVals["Section_ID"]) == 9)
                    {
                        sb.Append("PF.dbo.Plans.Medicaid_Enrollment - PF.dbo.Plans.Medicaid_Mcare_Enrollment as Total_Covered, ");
                    }
                    else
                    {
                        if (segmentID == "3")
                        {
                            sb.Append("PF.dbo.Plans.Medicaid_Enrollment as Total_Covered, ");
                        }
                        else if (segmentID == "1")
                        {
                            sb.Append("PF.dbo.Plans.Total_Pharmacy - PF.dbo.Plans.Total_Medicare_PartD as Total_Covered, ");
                        }
                        else if (segmentID == "2")
                        {
                            sb.Append("PF.dbo.Plans.Total_Medicare_PartD as Total_Covered, ");
                        }
                        else { sb.Append("PF.dbo.Plans.Total_Pharmacy as Total_Covered, "); }
                    }
                    sb.Append("dbo.Drug_Products.Product_Name, ");
                    sb.Append("PF.dbo.Lkp_Geographies.Geography_Name ");
                    sb.Append("from PF.dbo.Lkp_Tiers ");
                    sb.Append("inner Join PF.dbo.Plan_Drug_Formulary on ");
                    sb.Append("PF.dbo.Plan_Drug_Formulary.Tier_No = PF.dbo.Lkp_Tiers.Tier_Id and ");
                    sb.AppendFormat("PF.dbo.Plan_Drug_Formulary.Segment_ID in ({0}) right outer join ", segmentID);
                    sb.AppendFormat("({0}) as T1 ", comm.CommandText);
                    sb.Append("on PF.dbo.Plan_Drug_Formulary.Plan_ID = T1.Plan_ID and ");
                    sb.Append("PF.dbo.Plan_Drug_Formulary.Drug_ID = T1.Drug_ID and ");
                    //added for avoiding duplicate rows with same plan and trx volume having different tier
                    sb.Append("PF.dbo.Plan_Drug_Formulary.Segment_ID = T1.Segment_ID and ");
                    sb.Append("PF.dbo.Plan_Drug_Formulary.Is_Predominant = 1 ");
                    sb.Append("inner join PF.dbo.Plans on ");
                    sb.Append("PF.dbo.Plans.Plan_ID = T1.Plan_ID ");
                    sb.Append("left join dbo.Drug_Products on ");
                    sb.Append("dbo.Drug_Products.Product_ID = T1.Product_ID ");
                    sb.Append("inner join PF.dbo.Lkp_Geographies on ");
                    sb.Append("PF.dbo.Lkp_Geographies.Geography_ID = PF.dbo.Plans.Plan_State");

                    comm.CommandText = sb.ToString();
                }
                else //Summary view logic
                {
                    StringBuilder sb = new StringBuilder();

                    //Wrap query for summary view joins
                    sb.Append("SELECT TOP 100 PERCENT ");
                    sb.Append("T1.*, ");
                    sb.Append("dbo.Drug_Products.Product_Name ");
                    sb.AppendFormat("from ({0}) as T1 ", comm.CommandText);
                    sb.Append("left join dbo.Drug_Products on ");
                    sb.Append("dbo.Drug_Products.Product_ID = T1.Product_ID ");
                    sb.Append("order by Product_Name");

                    comm.CommandText = sb.ToString();
                }

                //Wrap query for MST calculations
                if (string.Compare(trxMst, "mst", true) == 0)
                {
                    IList<string> colsTrxToSum = new List<string>();
                    IList<string> colsMBTrxToSum = new List<string>();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT TOP 100 PERCENT *, ");

                    for (int x = 0; x < timeFrame.Count; x++)
                    {
                        sb.AppendFormat("Product_Mst{0} = CASE ISNULL(MB_Trx{0}, 0) WHEN 0 THEN 0.000 ELSE (Product_Trx{0} / MB_Trx{0}) * 100 END,", x);

                        colsTrxToSum.Add(string.Format("Product_Trx{0}", x));
                        colsMBTrxToSum.Add(string.Format("MB_Trx{0}", x));
                    }

                    sb.AppendFormat("Product_Mst_Sum = CASE({0}) WHEN 0 THEN 0.000 ELSE (({1})/({2})) END ", string.Join(" + ", colsMBTrxToSum.ToArray()), string.Join(" + ", colsTrxToSum.ToArray()), string.Join(" + ", colsMBTrxToSum.ToArray()));
                    sb.AppendFormat("FROM ({0}) AS T2", comm.CommandText);

                    //Remove Drug_ID from WHERE clause in case Product is in results
                    comm.CommandText = sb.ToString().Replace("and pivotTable0.Drug_ID = pivotTable1.Drug_ID", "");
                }

                //Wrap query for MSN calculations
                #region
                if (string.Compare(trxMst, "msn", true) == 0)
                {
                    IList<string> colsNrxToSum = new List<string>();
                    IList<string> colsMBNrxToSum = new List<string>();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT TOP 100 PERCENT *, ");

                    for (int x = 0; x < timeFrame.Count; x++)
                    {
                        sb.AppendFormat("Product_Msn{0} = CASE ISNULL(MB_Nrx{0}, 0) WHEN 0 THEN 0.000 ELSE (Product_Nrx{0} / MB_Nrx{0}) * 100 END,", x);

                        colsNrxToSum.Add(string.Format("Product_Nrx{0}", x));
                        colsMBNrxToSum.Add(string.Format("MB_Nrx{0}", x));
                    }

                    sb.AppendFormat("Product_Msn_Sum = CASE({0}) WHEN 0 THEN 0.000 ELSE (({1})/({2})) END ", string.Join(" + ", colsMBNrxToSum.ToArray()), string.Join(" + ", colsNrxToSum.ToArray()), string.Join(" + ", colsMBNrxToSum.ToArray()));
                    sb.AppendFormat("FROM ({0}) AS T2", comm.CommandText);

                    //Remove Drug_ID from WHERE clause in case Product is in results
                    comm.CommandText = sb.ToString().Replace("and pivotTable0.Drug_ID = pivotTable1.Drug_ID", "");
                }
                #endregion

                //for adding one more row for Total Market Basket Trx for each plan and segment

                #region detailed trx view - total mb calculation
                //Wrap query to obtain Total MB only for TRX and detailed view
                if (string.Compare(trxMst, "trx", true) == 0 && isDetailed)
                {
                    SqlCommand mbTrxComm = new SqlCommand();

                    StringBuilder sb = new StringBuilder();

                    sb.Append("SELECT TOP 100 PERCENT * FROM (");
                    sb.Append("SELECT TOP 100 PERCENT ");
                    sb.Append("2 AS [SortCol],");

                    //Change column name if selecting Top X Plans (to match unioned query)
                    if (rollup > 1)
                        sb.Append("T_MB.Product_ID AS 'Prod_ID', ");
                    else
                        sb.Append("T_MB.Product_ID, ");

                    sb.Append("T_MB.Drug_ID, ");
                    sb.Append("T_MB.Thera_ID, ");
                    sb.Append("T_MB.Segment_ID, ");
                    sb.Append("T_MB.Segment_Name, ");
                    sb.Append("T_MB.Geography_ID, ");
                    sb.Append("T_MB.Plan_ID, ");

                    //Only add Data_Year if not Rolling Quarter selection
                    if (string.Compare(queryVals["Calendar_Rolling"], "Calendar", true) == 0)
                        sb.Append("T_MB.Data_Year, ");

                    //Change column names of dynamic fields to match column names of unioned query
                    for (int x = 0; x < timeFrameVals.Count; x++)
                        sb.AppendFormat("T_MB.MB_TRx{0} AS [Product_Trx{0}], ", x);

                    sb.Append("T_MB.MB_TRx_Sum AS [Product_Trx_Sum], ");
                    sb.Append("T_MB.Tier_No, ");
                    sb.Append("T_MB.Co_Pay, ");
                    sb.Append("T_MB.PA, ");
                    sb.Append("T_MB.QL, ");
                    sb.Append("T_MB.ST, ");
                    sb.Append("T_MB.Tier_Name, ");
                    sb.Append("T_MB.Plan_Name, ");
                    sb.Append("T_MB.Total_Covered, ");
                    sb.Append("T_MB.Product_Name, ");

                    //Add column name if selecting Top X Plans (to match unioned query)
                    if (rollup > 1)
                        sb.Append("0 AS [Product_ID], ");

                    sb.Append("T_MB.Geography_Name ");
                    sb.Append("FROM ( ");
                    sb.Append("SELECT TOP 100 PERCENT ");
                    sb.Append("0 AS [Product_ID], ");
                    sb.Append("0 AS [Drug_ID], ");
                    sb.Append("T1.*, ");
                    sb.Append("NULL AS [Tier_No], ");
                    sb.Append("'' AS [Co_Pay], ");
                    sb.Append("'' AS [PA], ");
                    sb.Append("'' AS [QL], ");
                    sb.Append("'' AS [ST], ");
                    sb.Append("'' AS [Tier_Name], ");
                    sb.Append("p.Plan_Name, ");
                    //sb.Append("p.Total_Covered, ");
                    if (Convert.ToInt32(queryVals["Section_ID"]) == 9)
                    {
                        sb.Append("p.Medicaid_Enrollment-p.Medicaid_Mcare_Enrollment as Total_Covered, ");
                    }
                    else
                    {
                        if (segmentID == "3")
                        {
                            sb.Append("p.Medicaid_Enrollment as Total_Covered, ");
                        }
                        else if (segmentID == "1")
                        {
                            sb.Append("p.Total_Pharmacy-p.Total_Medicare_PartD as Total_Covered, ");
                        }
                        else if (segmentID == "2")
                        {
                            sb.Append("p.Total_Medicare_PartD as Total_Covered, ");
                        }
                        else { sb.Append("p.Total_Pharmacy as Total_Covered, "); }
                    }
                    sb.Append("'Total MB' AS [Product_Name], ");
                    sb.Append("PF.dbo.Lkp_Geographies.Geography_Name ");
                    sb.Append("from ");
                    sb.Append("PF.dbo.Plan_Drug_Formulary ");
                    sb.Append("right outer join (");

                    //Remove Product_ID, Drug_ID and MB_Trx keys to prevent duplicates (selected as zero above to preserve column names)
                    IList<string> mbTrxKeys = new List<string>();

                    foreach (string s in keys)
                        mbTrxKeys.Add(s);

                    mbTrxKeys.Remove("Product_ID");
                    mbTrxKeys.Remove("Drug_ID");
                    mbTrxKeys.Remove("MB_Trx");

                    //Add flag in queryVals to pivot query on MB_Trx in 'ConstructMAPivotQuery'
                    NameValueCollection mbQueryVals = new NameValueCollection(queryVals);
                    mbQueryVals.Add("Is_MB_Trx", "true");

                    string mbTrxQuery = GetPivotPlanDataQuery(mbTrxComm, dataYear, timeFrameVals, tableName, dataField, mbTrxKeys, isDetailed, productID, mbQueryVals).IncludeSum().ToString();

                    //Add MB Trx pivot query
                    sb.Append(mbTrxQuery);
                    sb.Append(") as T1 on ");
                    sb.Append("PF.dbo.Plan_Drug_Formulary.Is_Predominant = 1 inner join ");
                    sb.Append("pf.dbo.plans p on p.Plan_ID = T1.Plan_ID inner join ");
                    sb.Append("PF.dbo.Lkp_Geographies on PF.dbo.Lkp_Geographies.Geography_ID = p.Plan_State ");
                    sb.Append(") AS T_MB ");
                    sb.Append("UNION ");

                    //Add existing pivot query
                    sb.Append(comm.CommandText);
                    sb.Append(") as TableX ");
                    sb.Append("ORDER BY Plan_Name, Product_Name, SortCol");

                    comm.CommandText = sb.ToString();
                }
                #endregion

                //for adding one more row for Total MB Nrx for each plan and segment
                #region detailed NRx view - total mb calculation

                //Wrap query to obtain Total MB only for NRX and detailed view
                if (string.Compare(trxMst, "nrx", true) == 0 && isDetailed)
                {
                    SqlCommand mbNrxComm = new SqlCommand();

                    StringBuilder sb = new StringBuilder();

                    sb.Append("SELECT TOP 100 PERCENT * FROM (");
                    sb.Append("SELECT TOP 100 PERCENT ");
                    sb.Append("2 AS [SortCol],");

                    //Change column name if selecting Top X Plans (to match unioned query)
                    if (rollup > 1)
                        sb.Append("T_MB.Product_ID AS 'Prod_ID', ");
                    else
                        sb.Append("T_MB.Product_ID, ");

                    sb.Append("T_MB.Drug_ID, ");
                    sb.Append("T_MB.Thera_ID, ");
                    sb.Append("T_MB.Segment_ID, ");
                    sb.Append("T_MB.Segment_Name, ");
                    sb.Append("T_MB.Geography_ID, ");
                    sb.Append("T_MB.Plan_ID, ");

                    //Only add Data_Year if not Rolling Quarter selection
                    if (string.Compare(queryVals["Calendar_Rolling"], "Calendar", true) == 0)
                        sb.Append("T_MB.Data_Year, ");

                    //Change column names of dynamic fields to match column names of unioned query
                    for (int x = 0; x < timeFrameVals.Count; x++)
                        sb.AppendFormat("T_MB.MB_NRx{0} AS [Product_Nrx{0}], ", x);

                    sb.Append("T_MB.MB_NRx_Sum AS [Product_Nrx_Sum], ");
                    sb.Append("T_MB.Tier_No, ");
                    sb.Append("T_MB.Co_Pay, ");
                    sb.Append("T_MB.PA, ");
                    sb.Append("T_MB.QL, ");
                    sb.Append("T_MB.ST, ");
                    sb.Append("T_MB.Tier_Name, ");
                    sb.Append("T_MB.Plan_Name, ");
                    sb.Append("T_MB.Total_Covered, ");
                    sb.Append("T_MB.Product_Name, ");

                    //Add column name if selecting Top X Plans (to match unioned query)
                    if (rollup > 1)
                        sb.Append("0 AS [Product_ID], ");

                    sb.Append("T_MB.Geography_Name ");
                    sb.Append("FROM ( ");
                    sb.Append("SELECT TOP 100 PERCENT ");
                    sb.Append("0 AS [Product_ID], ");
                    sb.Append("0 AS [Drug_ID], ");
                    sb.Append("T1.*, ");
                    sb.Append("NULL AS [Tier_No], ");
                    sb.Append("'' AS [Co_Pay], ");
                    sb.Append("'' AS [PA], ");
                    sb.Append("'' AS [QL], ");
                    sb.Append("'' AS [ST], ");
                    sb.Append("'' AS [Tier_Name], ");
                    sb.Append("p.Plan_Name, ");
                    //sb.Append("p.Total_Covered, ");
                    if (Convert.ToInt32(queryVals["Section_ID"]) == 9)
                    {
                        sb.Append("p.Medicaid_Enrollment-p.Medicaid_Mcare_Enrollment as Total_Covered, ");
                    }
                    else
                    {
                        if (segmentID == "3")
                        {
                            sb.Append("p.Medicaid_Enrollment as Total_Covered, ");
                        }
                        else if (segmentID == "1")
                        {
                            sb.Append("p.Total_Pharmacy-p.Total_Medicare_PartD as Total_Covered, ");
                        }
                        else if (segmentID == "2")
                        {
                            sb.Append("p.Total_Medicare_PartD as Total_Covered, ");
                        }
                        else { sb.Append("p.Total_Pharmacy as Total_Covered, "); }
                    }
                    sb.Append("'Total MB' AS [Product_Name], ");
                    sb.Append("PF.dbo.Lkp_Geographies.Geography_Name ");
                    sb.Append("from ");
                    sb.Append("PF.dbo.Plan_Drug_Formulary ");
                    sb.Append("right outer join (");

                    //Remove Product_ID, Drug_ID and MB_Trx keys to prevent duplicates (selected as zero above to preserve column names)
                    IList<string> mbNrxKeys = new List<string>();

                    foreach (string s in keys)
                        mbNrxKeys.Add(s);

                    mbNrxKeys.Remove("Product_ID");
                    mbNrxKeys.Remove("Drug_ID");
                    mbNrxKeys.Remove("MB_NRx");

                    //Add flag in queryVals to pivot query on MB_Trx in 'ConstructMAPivotQuery'
                    NameValueCollection mbQueryVals = new NameValueCollection(queryVals);
                    mbQueryVals.Add("Is_MB_NRx", "true");

                    string mbNrxQuery = GetPivotPlanDataQuery(mbNrxComm, dataYear, timeFrameVals, tableName, dataField, mbNrxKeys, isDetailed, productID, mbQueryVals).IncludeSum().ToString();

                    //Add MB Trx pivot query
                    sb.Append(mbNrxQuery);
                    sb.Append(") as T1 on ");
                    sb.Append("PF.dbo.Plan_Drug_Formulary.Is_Predominant = 1 inner join ");
                    sb.Append("pf.dbo.plans p on p.Plan_ID = T1.Plan_ID inner join ");
                    sb.Append("PF.dbo.Lkp_Geographies on PF.dbo.Lkp_Geographies.Geography_ID = p.Plan_State ");
                    sb.Append(") AS T_MB ");
                    sb.Append("UNION ");

                    //Add existing pivot query
                    sb.Append(comm.CommandText);
                    sb.Append(") as TableX ");
                    sb.Append("ORDER BY Plan_Name, Product_Name, SortCol");

                    comm.CommandText = sb.ToString();
                }
                #endregion

                //Wrap query for paging
                string pagingEnabled = queryVals["PagingEnabled"];
                if (!string.IsNullOrEmpty(pagingEnabled))
                {
                    if (Convert.ToBoolean(pagingEnabled) == true)
                    {
                        int startPage = Convert.ToInt32(queryVals["StartPage"]);
                        int totalPerPage = Convert.ToInt32(queryVals["TotalPerPage"]);

                        int rowNumber = (startPage * totalPerPage) - totalPerPage;

                        StringBuilder sb = new StringBuilder();
                        sb.AppendFormat("SELECT TOP {0} * FROM ", totalPerPage);
                        sb.Append("(SELECT ");
                        sb.Append("ROW_NUMBER() OVER (Order By Geography_Name ASC, Plan_Name ASC, Product_Name ASC, Segment_Name ASC, Tier_Name ASC) ");
                        sb.Append("AS RowNumber, * ");
                        sb.AppendFormat("FROM ({0}) AS Results2) AS Results WHERE RowNumber > {1} ORDER BY RowNumber, Product_Name, Segment_Name", comm.CommandText, rowNumber);

                        comm.CommandText = sb.ToString();
                    }
                    else if (string.IsNullOrEmpty(queryVals["Export"]))//Only select count if not export
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendFormat("SELECT COUNT(0) AS 'RowCount' FROM ({0}) AS FullCount", comm.CommandText);

                        comm.CommandText = sb.ToString();
                    }
                }

                conn.Open();
                DbDataReader rdr = comm.ExecuteReader();
                IEnumerable<GenericDataRecord> g = Pinsonault.Data.GenericDataRecord.CreateCollection(rdr);
                conn.Close();

                return g;
            }
            catch (Exception x)
            {
                throw x;
            }
            finally
            {
                if (conn != null) conn.Dispose();
                if (comm != null) comm.Dispose();
            }
        }

        public void ProcessGrid(GridView grid, bool isMonth, IEnumerable<GenericDataRecord> g, IList<int> timeFrameVals, NameValueCollection queryVals)
        {
            if (queryVals["Section_ID"] == "9")
            {
                BoundField tierColumn = grid.Columns.OfType<BoundField>().FirstOrDefault(c => c.DataField == "Tier_Name");
                if (tierColumn != null)
                    tierColumn.Visible = false;
            }

            List<int> timeFrame = timeFrameVals.ToList();

            //If Rolling Quarterly, reverse time frame values (only for summary)
            if ((string.Compare(queryVals["Calendar_Rolling"], "Rolling", true) == 0) && (string.Compare(grid.ClientID, "ctl00_partialPage_detailedGrid", true) != 0))
                timeFrame.Reverse();

            BoundField boundCol;

            string trxMst = queryVals["Trx_Mst"];

            if (string.IsNullOrEmpty(trxMst))
                trxMst = "Trx";

            for (int x = 0; x < timeFrame.Count; x++)
            {
                boundCol = new BoundField();
                boundCol.ItemStyle.CssClass = "alignRight";
                boundCol.DataField = string.Format("Product_{0}{1}", trxMst, x);
                boundCol.HeaderText = GetHeaderText(isMonth, timeFrame[x], queryVals);

                //Check if client/user has decimal formatting enabled
                if (HttpContext.Current.User.IsInRole("mpa_decimal"))
                {
                    if ((string.Compare(trxMst, "trx", true) == 0) || (string.Compare(trxMst, "nrx", true) == 0))
                        boundCol.DataFormatString = "{0:N2}";
                    else
                        boundCol.DataFormatString = "{0:N3}%";
                }
                else
                {
                    if ((string.Compare(trxMst, "trx", true) == 0) || (string.Compare(trxMst, "nrx", true) == 0))
                        boundCol.DataFormatString = "{0:N0}";
                    else
                        boundCol.DataFormatString = "{0:N0}%";
                }

                grid.Columns.Add(boundCol);
            }

            //Check if Formulary History Popup
            if (string.Compare(queryVals["IsFormularyHistory"], "true", true) == 0)
            {
                string previousTimeFrame = GetHeaderText(isMonth, GetPreviousTimeFrame(timeFrame[0], Convert.ToInt32(queryVals["Section_ID"])), queryVals);
                string currentTimeFrame = GetHeaderText(isMonth, timeFrame[0], queryVals);

                //Add Formulary History specific columns
                //Only add Tier column if not State Medicaid
                if (queryVals["Section_ID"] != "9")
                {
                    boundCol = new BoundField();
                    boundCol.DataField = "Tier_Name0";
                    boundCol.HeaderText = previousTimeFrame;
                    grid.Columns.Add(boundCol);

                    boundCol = new BoundField();
                    boundCol.DataField = "Tier_Name1";
                    boundCol.HeaderText = currentTimeFrame;
                    grid.Columns.Add(boundCol);
                }

                boundCol = new BoundField();
                boundCol.DataField = "Formulary_Status_Abbr0";
                boundCol.HeaderText = previousTimeFrame;
                grid.Columns.Add(boundCol);

                boundCol = new BoundField();
                boundCol.DataField = "Formulary_Status_Abbr1";
                boundCol.HeaderText = currentTimeFrame;
                grid.Columns.Add(boundCol);

                boundCol = new BoundField();
                boundCol.DataField = "PA0";
                boundCol.HeaderText = previousTimeFrame;
                grid.Columns.Add(boundCol);

                boundCol = new BoundField();
                boundCol.DataField = "PA1";
                boundCol.HeaderText = currentTimeFrame;
                grid.Columns.Add(boundCol);

                boundCol = new BoundField();
                boundCol.DataField = "QL0";
                boundCol.HeaderText = previousTimeFrame;
                grid.Columns.Add(boundCol);

                boundCol = new BoundField();
                boundCol.DataField = "QL1";
                boundCol.HeaderText = currentTimeFrame;
                grid.Columns.Add(boundCol);

                boundCol = new BoundField();
                boundCol.DataField = "ST0";
                boundCol.HeaderText = previousTimeFrame;
                grid.Columns.Add(boundCol);

                boundCol = new BoundField();
                boundCol.DataField = "ST1";
                boundCol.HeaderText = currentTimeFrame;
                grid.Columns.Add(boundCol);

                boundCol = new BoundField();
                boundCol.DataField = "Co_Pay0";
                boundCol.HeaderText = previousTimeFrame;
                grid.Columns.Add(boundCol);

                boundCol = new BoundField();
                boundCol.DataField = "Co_Pay1";
                boundCol.HeaderText = currentTimeFrame;
                grid.Columns.Add(boundCol);
            }

            grid.DataSource = g;
            grid.DataBind();
        }

        public void ProcessGridPrescriber(GridView grid, bool isMonth, IEnumerable<GenericDataRecord> g, IList<int> timeFrameVals, NameValueCollection queryVals)
        {
            List<int> timeFrame = timeFrameVals.ToList();

            //If Rolling Quarterly, reverse time frame values (only for summary)
            if ((string.Compare(queryVals["Calendar_Rolling"], "Rolling", true) == 0) && (string.Compare(grid.ClientID, "ctl00_partialPage_detailedGrid", true) != 0))
                timeFrame.Reverse();

            BoundField boundCol;

            string trxMst = queryVals["Trx_Mst"];

            if (string.IsNullOrEmpty(trxMst))
                trxMst = "Trx";

            for (int x = 0; x < timeFrame.Count; x++)
            {
                boundCol = new BoundField();
                boundCol.ItemStyle.CssClass = "alignRight";
                boundCol.DataField = string.Format("Product_{0}{1}", trxMst, x);
                boundCol.HeaderText = GetHeaderText(isMonth, timeFrame[x], queryVals);
                
                //Check if client/user has decimal formatting enabled
                if (HttpContext.Current.User.IsInRole("mpa_decimal"))
                {
                    if ((string.Compare(trxMst, "trx", true) == 0) || (string.Compare(trxMst, "nrx", true) == 0))
                        boundCol.DataFormatString = "{0:N2}";
                    else
                        boundCol.DataFormatString = "{0:N3}%";
                }
                else
                {
                    if ((string.Compare(trxMst, "trx", true) == 0) || (string.Compare(trxMst, "nrx", true) == 0))
                        boundCol.DataFormatString = "{0:N0}";
                    else
                        boundCol.DataFormatString = "{0:N0}%";
                }

                grid.Columns.Add(boundCol);
            }

            grid.DataSource = g;
            grid.DataBind();
        }


        //Used for legacy support
        public void ProcessChart(IList<int> timeFrameVals, Chart chart, bool isMonth, IEnumerable<GenericDataRecord> g, string report, string year, NameValueCollection queryVals)
        {
            ProcessChart(timeFrameVals, chart, isMonth, g, report, year, queryVals, null);
        }

        //Used for FHR
        public void ProcessChart(IList<int> timeFrameVals, Chart chart, bool isMonth, IEnumerable<GenericDataRecord> g, string report, string year, NameValueCollection queryVals, string dataField)
        {
            List<int> timeFrame = timeFrameVals.ToList();

            //If Rolling Quarterly, reverse time frame values
            if ((string.Compare(queryVals["Calendar_Rolling"], "Rolling", true) == 0))
                timeFrame.Reverse();

            chart.ChartAreas[0].AxisY.Title = queryVals["Trx_Mst"];
            int index = 0;
            int chartNum = 0;

            switch (chart.ClientID)
            {
                case "ctl00_Tile4_chart_chartDisplay1_chart":
                    chartNum = 1;
                    break;
                case "ctl00_Tile4_chart_chartDisplay2_chart":
                    chartNum = 2;
                    break;
                case "ctl00_Tile4_chart_chartDisplay3_chart":
                    chartNum = 3;
                    break;
            }

            //Format numbers based on Trx/Mst or Nrx/Msn
            if ((string.Compare(queryVals["Trx_Mst"], "trx", true) == 0) || (string.Compare(queryVals["Trx_Mst"], "nrx", true) == 0))
                chart.ChartAreas[0].AxisY.LabelStyle.Format = "{0:N0}";
            else
                chart.ChartAreas[0].AxisY.LabelStyle.Format = "{0:P0}";

            //If Formulary History report, return a list of timeframe changes for faster processing.
            //This list is used as a search-point when each point is plotted on the chart.
            List<FHRChanges> changes = null;

            if (string.Compare(report, "formularyhistory", true) == 0)
            {
                using (PathfinderMarketplaceAnalyticsEntities context = new PathfinderMarketplaceAnalyticsEntities(Pinsonault.Web.Session.GetClientApplicationConnectionString("MarketplaceAnalytics")))
                {
                    changes = context.GetFHRChanges(queryVals, dataField);
                    //Also place is session variable for summary grid change/highlight processing
                    //HttpContext.Current.Session["FHR_Changes"] = changes;
                }
            }

            foreach (var y in g)
            {
                if (string.Compare(report, "trending", true) == 0)
                {
                    //Get Chart Titles

                    if (chartNum == 1)//National
                    {
                        chart.Titles[0].Text = "National";
                        chart.Attributes["_title"] = "National";
                    }

                    if (chartNum == 2)//Regional or Account Manager
                    {
                        int geographyType = Convert.ToInt32(queryVals["Geography_Type"]);
                        if (geographyType == 2)
                        {
                            chart.Titles[0].Text = GetRegionName(queryVals["Region_ID"]);
                            chart.Attributes["_title"] = GetRegionName(queryVals["Region_ID"]);
                        }
                        if (geographyType == 3)
                        {
                            chart.Titles[0].Text = GetAccountManagerName(queryVals["Territory_ID"]);
                            chart.Attributes["_title"] = GetAccountManagerName(queryVals["Territory_ID"]);
                        }
                    }
                    
                    if (chartNum == 3)//State
                    {
                        chart.Titles[0].Text = GetStateName(queryVals["State_ID"]);
                        chart.Attributes["_title"] = GetStateName(queryVals["State_ID"]);
                    }

                    //As per Trending requirements, if user selects more than one time period, chart type is line
                    if (timeFrame.Count > 1)
                    {
                        chart.Series[index].Type = SeriesChartType.Line;
                        chart.Series[index].BorderWidth = 3;
                    }
                }
                if (string.Compare(report, "prescribertrending", true) == 0)
                {
                    if (chartNum == 1) //Region
                    {
                        chart.Titles[0].Text = GetPrescriberGeographyName(queryVals["Region_ID"], "region");
                        chart.Attributes["_title"] = GetPrescriberGeographyName(queryVals["Region_ID"], "region");
                    }
                    if (chartNum == 2) //District
                    {
                        chart.Titles[0].Text = GetPrescriberGeographyName(queryVals["District_ID"], "district");
                        chart.Attributes["_title"] = GetPrescriberGeographyName(queryVals["District_ID"], "district");
                    }
                    if (chartNum == 3) //Territory
                    {
                        chart.Titles[0].Text = GetPrescriberGeographyName(queryVals["Territory_ID"], "territory");
                        chart.Attributes["_title"] = GetPrescriberGeographyName(queryVals["Territory_ID"], "territory");
                    }

                    if (timeFrame.Count > 1)
                    {
                        chart.Series[index].Type = SeriesChartType.Line;
                        chart.Series[index].BorderWidth = 3;
                    }
                }

                if (string.Compare(report, "comparison", true) == 0)
                {
                    bool isCalendar = false;

                    if (string.Compare(queryVals["Calendar_Rolling"], "Calendar", true) == 0)
                        isCalendar = true;
                    else
                        isCalendar = false;

                    //Get Chart Title
                    if (isCalendar)
                    {
                        if (isMonth)
                        {
                            chart.Titles[0].Text = string.Format("{0} {1}", GetTimeFrameName(Convert.ToInt32(queryVals[string.Format("MonthQuarterSelection{0}", chartNum)]), "month"), year);
                            chart.Attributes["_title"] = string.Format("{0} {1}", GetTimeFrameName(Convert.ToInt32(queryVals[string.Format("MonthQuarterSelection{0}", chartNum)]), "month"), year);
                        }
                        else
                        {
                            chart.Titles[0].Text = string.Format("{0} {1}", GetTimeFrameName(Convert.ToInt32(queryVals[string.Format("MonthQuarterSelection{0}", chartNum)]), "quarter"), year);
                            chart.Attributes["_title"] = string.Format("{0} {1}", GetTimeFrameName(Convert.ToInt32(queryVals[string.Format("MonthQuarterSelection{0}", chartNum)]), "quarter"), year);
                        }
                    }
                    else
                    {
                        chart.Titles[0].Text = string.Format("{0} Rolling", GetTimeFrameName(Convert.ToInt32(queryVals[string.Format("RollingQuarterSelection{0}", chartNum)]), "rollingquarter"), year);
                        chart.Attributes["_title"] = string.Format("{0} Rolling", GetTimeFrameName(Convert.ToInt32(queryVals[string.Format("RollingQuarterSelection{0}", chartNum)]), "rollingquarter"), year);
                    }
                }

                if (string.Compare(report, "affiliation", true) == 0)
                {
                    //Get Chart Titles
                    if (chartNum == 1)//National
                    {
                        chart.Titles[0].Text = "National";
                        chart.Attributes["_title"] = "National";
                    }

                    if (chartNum == 2)//at Plan level, populate plan name for chart title
                    {
                        int PlanID = Convert.ToInt32(queryVals["Plan_ID"]);

                        //populate the plan name 
                        string planName = GetPlanName(PlanID);

                        chart.Titles[0].Text = planName;
                        chart.Attributes["_title"] = planName;
                    }

                    //As per Trending requirements, if user selects more than one time period, chart type is line
                    if (timeFrame.Count > 1)
                    {
                        chart.Series[index].Type = SeriesChartType.Line;
                        chart.Series[index].BorderWidth = 3;
                    }
                }

                if (string.Compare(report, "formularyhistory", true) == 0)
                {
                    //Get Chart Titles
                    int PlanID = Convert.ToInt32(queryVals["Plan_ID"]);

                    //populate the plan name 
                    string planName = GetPlanName(PlanID);

                    chart.Titles[0].Text = planName;
                    chart.Attributes["_title"] = planName;

                    //As per Trending requirements, if user selects more than one time period, chart type is line
                    if (timeFrame.Count > 1)
                    {
                        chart.Series[index].Type = SeriesChartType.Line;
                        chart.Series[index].BorderWidth = 3;
                    }
                }

                chart.Series[index].CustomAttributes = "DrawingStyle=LightToDark";
                chart.Series[index].CustomAttributes = "DrawingStyle=Cylinder";

                int prodNameOrdinal = 0;
                int prodIDOrdinal = 0;

                prodNameOrdinal = y.GetOrdinal("Product_Name");
                prodIDOrdinal = y.GetOrdinal("Product_ID");

                for (int fieldIndex = 0; fieldIndex < timeFrame.Count(); fieldIndex++)
                {
                    int pointIndex = 0;
                    int productTrxMstOrdinal = 0;

                    productTrxMstOrdinal = y.GetOrdinal(string.Format("Product_{0}{1}", queryVals["Trx_Mst"], fieldIndex.ToString()));
                    pointIndex = chart.Series[index].Points.AddY(y.GetValue(productTrxMstOrdinal));

                    //Manually set interval to one due to rendering issue with small numbers in TRx chart
                    if ((!y.IsDBNull(productTrxMstOrdinal)) && (Convert.ToInt32(y.GetValue(productTrxMstOrdinal)) < 5) && intervalFlag)
                        chart.ChartAreas[0].AxisY.Interval = 1;
                    else
                    {
                        chart.ChartAreas[0].AxisY.Interval = 0;
                        intervalFlag = false;
                    }

                    chart.Series[index].CustomAttributes = "DrawingStyle=Cylinder";
                    chart.Series[index].Name = y.GetValue(prodNameOrdinal).ToString(); //dr["Product_Name"].ToString();
                    chart.Series[index].Points[pointIndex].Color = ReportColors.StandardReports.GetColor(index % 6);
                    chart.Series[index].Color = ReportColors.StandardReports.GetColor(index % 6);
                    chart.Series[index].Points[pointIndex].AxisLabel = GetHeaderText(isMonth, timeFrame[fieldIndex], queryVals);

                    //If formularyhistoryreport, check if there is a formulary data change for the point before creating a link/marker
                    if (string.Compare(report, "formularyhistory", true) == 0)
                    {
                        //Check for data change for all point except for the first one
                        if (pointIndex > 0)
                        {
                            int? changed = 0;
                            int productId = Convert.ToInt32(y.GetValue(prodIDOrdinal));
                            int? tf = timeFrame[fieldIndex];
                            
                            changed = changes.Where(t => t.ID == productId).Where(t => t.Timeframe == tf).Select(t => t.Changed).FirstOrDefault();
                            
                            //If there was a change, create a market/hyperlink
                            if (changed > 0)
                            {
                                chart.Series[index].Points[pointIndex].Href = string.Format("javascript:chartFHREvent({0},{1},'{2}',{3},'{4}','{5}','{6}','{7}','{8}')", queryVals["Section_ID"], queryVals["Plan_ID"], queryVals["Product_ID"], queryVals["Market_Basket_ID"], timeFrame[fieldIndex], isMonth, queryVals["Trx_Mst"], y.GetValue(prodIDOrdinal), fieldIndex);
                                chart.Series[index].Points[pointIndex].MarkerStyle = MarkerStyle.Diamond;
                                chart.Series[index].Points[pointIndex].MarkerSize = 10;
                                chart.Series[index].Points[pointIndex].BorderColor = System.Drawing.Color.Black;
                                chart.Series[index].Points[pointIndex].MarkerColor = ReportColors.FormularyHistoryReporting.GetColor(fieldIndex); //System.Drawing.Color.Red;
                            }
                        }
                    }
                    else
                        chart.Series[index].Points[pointIndex].Href = string.Format("javascript:chartEvent({0},{1})", chartNum, y.GetValue(prodIDOrdinal));
                }

                index++;
            }

            //Hide the rest
            while (index < chart.Series.Count)
            {
                chart.Series[index].ShowInLegend = false;
                index++;
            }
        }

        public class RowCounts : System.Collections.Generic.List<int> { }

        //Function used to group rows for detailed grid
        public void GroupRows(GridView gv, int y)
        {
            string text = "";
            string accountName = "";
            int count = 0;

            RowCounts rowCounts = new RowCounts();
            rowCounts.Clear();

            // Loop through all rows to get row counts
            foreach (GridViewRow gvr in gv.Rows)
            {
                if (gvr.RowType == DataControlRowType.DataRow)
                {
                    //Only merge Column Index 2 (Lives) per Account name
                    if (y == 2)
                    {
                        if (gvr.Cells[y].Text == text && gvr.Cells[1].Text == accountName)
                            count++;
                        else
                        {
                            if (count > 0)
                                rowCounts.Add(count);

                            text = gvr.Cells[y].Text;
                            //
                            accountName = gvr.Cells[1].Text;
                            count = 1;
                        }
                    }
                    else
                    {
                        if (gvr.Cells[y].Text == text)
                            count++;
                        else
                        {
                            if (count > 0)
                                rowCounts.Add(count);

                            text = gvr.Cells[y].Text;
                            count = 1;
                        }
                    }
                }
            }

            if (count > 0)
                rowCounts.Add(count);

            // Loop through all rows again to set rowspan
            text = "";
            accountName = "";
            int i = 0;
            foreach (GridViewRow gvr in gv.Rows)
            {
                if (gvr.RowType == DataControlRowType.DataRow)
                {
                    //Only merge Column Index 2 (Lives) per Account name
                    if (y == 2)
                    {
                        if (gvr.Cells[y].Text == text && gvr.Cells[1].Text == accountName)
                            gvr.Cells.Remove(gvr.Cells[y]);
                        else
                        {
                            text = gvr.Cells[y].Text;
                            accountName = gvr.Cells[1].Text;

                            // Related to the count update above, check for null
                            count = (rowCounts[i] != null) ? rowCounts[i++] : 1;

                            if (count > 1)
                                gvr.Cells[y].RowSpan = count;
                        }
                    }
                    else
                    {
                        if (gvr.Cells[y].Text == text)
                            gvr.Cells.Remove(gvr.Cells[y]);
                        else
                        {
                            text = gvr.Cells[y].Text;

                            // Related to the count update above, check for null
                            count = (rowCounts[i] != null) ? rowCounts[i++] : 1;

                            if (count > 1)
                                gvr.Cells[y].RowSpan = count;
                        }
                    }
                }
            }
        }

        //Function used to group rows for Prescriber grid
        public void PrescriberGroupRows(GridView gv, int y)
        {
            string text = "";
            int count = 0;

            RowCounts rowCounts = new RowCounts();
            rowCounts.Clear();

            // Loop through all rows to get row counts
            foreach (GridViewRow gvr in gv.Rows)
            {
                if (gvr.RowType == DataControlRowType.DataRow)
                {

                    if (gvr.Cells[y].Text == text)
                        count++;
                    else
                    {
                        if (count > 0)
                            rowCounts.Add(count);

                        text = gvr.Cells[y].Text;
                        count = 1;
                    }
                }
            }

            if (count > 0)
                rowCounts.Add(count);

            //Loop through all rows again to set rowspan
            text = "";
            int i = 0;
            foreach (GridViewRow gvr in gv.Rows)
            {
                if (gvr.RowType == DataControlRowType.DataRow)
                {

                    if (gvr.Cells[y].Text == text)
                        gvr.Cells.Remove(gvr.Cells[y]);
                    else
                    {
                        text = gvr.Cells[y].Text;

                        // Related to the count update above, check for null
                        count = (rowCounts[i] != null) ? rowCounts[i++] : 1;

                        if (count > 1)
                            gvr.Cells[y].RowSpan = count;
                    }
                }
            }
        }

        /// <summary>
        /// Returns header text per timeframe
        /// </summary>
        /// <param name="isMonth">bool</param>
        /// <param name="num">Int32</param>
        /// <param name="queryVals">NameValueCollection</param>
        /// <returns>int</returns>
        public string GetHeaderText(bool isMonth, int num, NameValueCollection queryVals)
        {
            string headerText;

            //Check if comparison report (only comparison report has 'MonthQuarterSelection' and 'RollingQuarterSelection' QueryVals
            if ((!string.IsNullOrEmpty(queryVals["MonthQuarterSelection1"])) || (!string.IsNullOrEmpty(queryVals["RollingQuarterSelection1"])))
            {
                //If comparison report, set timeframe headers to either 'Trx' or 'Mst'
                headerText = queryVals["Trx_Mst"];
            }
            else
            {
                using (PathfinderMarketplaceAnalyticsEntities clientContext = new PathfinderMarketplaceAnalyticsEntities(Pinsonault.Web.Session.GetClientApplicationConnectionString("MarketplaceAnalytics")))
                {
                    //Check if Formulary History report (formulary history doesn't have the following queryVals)
                    if (string.IsNullOrEmpty(queryVals["Quarter_Selection"]) && string.IsNullOrEmpty(queryVals["Month_Selection"]) && string.IsNullOrEmpty(queryVals["Rolling_Selection"]))
                    {
                        //Only Med-D is monthly based
                        if (Convert.ToInt32(queryVals["Section_ID"]) == 17)
                            headerText = GetTimeFrameName(num, "monthyear");
                        else
                            headerText = GetTimeFrameName(num, "quarteryear");
                    }
                    else
                    {
                        //Check if Calendar type query
                        if (string.Compare(queryVals["Calendar_Rolling"], "Calendar", true) == 0)
                        {
                            if (isMonth)
                                headerText = GetTimeFrameName(num, "month");
                            else
                                headerText = GetTimeFrameName(num, "quarter");
                        }
                        else
                            headerText = GetTimeFrameName(num, "rollingquarter");
                    }
                }
            }

            return headerText;
        }

        /// <summary>
        /// Returns full region name based on region id
        /// </summary>
        /// <param name="regionID">string</param>
        /// <returns>string</returns>
        public string GetRegionName(string regionID)
        {
            string region = "";

            using (PathfinderClientEntities context = new PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
            {
                region =
                   (from p in context.TerritorySet
                    where p.ID == regionID
                    select p.Name).FirstOrDefault();
            }

            return region;
        }

        /// <summary>
        /// Returns account manager name based on territory id
        /// </summary>
        /// <param name="regionID">string</param>
        /// <returns>string</returns>
        public string GetAccountManagerName(string territoryID)
        {

            int clientID = Pinsonault.Web.Session.ClientID;

            using (PathfinderEntities context = new PathfinderEntities())
            {
                var query =
                   (from u in context.UserSet
                    join t in context.UserTerritorySet on
                    u.User_ID equals t.UserId
                    where t.TerritoryId == territoryID
                    && t.ClientId == clientID
                    select u).ToList();

                var accountmanager =
                    (from u in query
                     select new { Name = string.Format("{0} {1}", u.User_F_Name, u.User_L_Name) }).FirstOrDefault();

                return accountmanager.Name;
            }
        }


        /// <summary>
        /// Returns full state name based on abbreviation
        /// </summary>
        /// <param name="stateID">string</param>
        /// <returns>string</returns>
        public string GetStateName(string stateID)
        {
            string state = "";

            using (PathfinderEntities context = new PathfinderEntities())
            {
                state =
                   (from p in context.StateSet
                    where p.ID == stateID
                    select p.Name).FirstOrDefault();
            }

            return state;
        }

        /// <summary>
        /// Returns timeframe text pased on timeframe integer and type
        /// </summary>
        /// <param name="id">Int32</param>
        /// <param name="type">string</param>
        /// <returns>string</returns>
        public string GetTimeFrameName(int id, string type)
        {
            string name = "";

            using (PathfinderMarketplaceAnalyticsEntities context = new PathfinderMarketplaceAnalyticsEntities(Pinsonault.Web.Session.GetClientApplicationConnectionString("MarketplaceAnalytics")))
            {
                if (string.Compare(type, "month", true) == 0)
                {
                    name = (from p in context.LkpMarketplaceShortLongMonthNamesSet
                            where p.Number == id
                            select p.ShortName).FirstOrDefault();
                }

                if (string.Compare(type, "quarter", true) == 0)
                {
                    name = (from p in context.LkpMarketplaceShortLongQuarterNamesSet
                            where p.Number == id
                            select p.ShortName).FirstOrDefault();
                }

                if (string.Compare(type, "rollingquarter", true) == 0)
                {
                    name = (from p in context.LkpMarketplaceShortLongRollingQuarterNamesSet
                            where p.Number == id
                            select p.ShortName).FirstOrDefault();
                }

                if (string.Compare(type, "monthyear", true) == 0)
                {
                    name = (from p in context.FHMonthYearsSet
                            where p.ID == id
                            select p.Name).FirstOrDefault();
                }

                if (string.Compare(type, "quarteryear", true) == 0)
                {
                    name = (from p in context.FHQuarterYearsSet
                            where p.ID == id
                            select p.Name).FirstOrDefault();
                }
            }

            return name;
        }

        /// <summary>
        /// Returns previous timeframe from passing in timeframe
        /// </summary>
        /// <param name="currentTimeFrame">Int32</param>
        /// <param name="sectionID">Int32</param>
        /// <returns>int</returns>
        public int GetPreviousTimeFrame(int currentTimeframe, int sectionID)
        {
            int year1;
            int year2;
            int time1;
            int time2;
            int previousTimeframe;

            //Determine previous timeframe base on passed timeframe
            year1 = Convert.ToInt32(currentTimeframe.ToString().Substring(0, 4));
            time1 = Convert.ToInt32(currentTimeframe.ToString().Substring(4, (currentTimeframe.ToString().Length - 4)));

            if (time1 == 1)
            {
                if (sectionID == 17) //Time is month based for Med-D
                    time2 = 12;
                else //Time is quarter based for all other sections
                    time2 = 4;
                year2 = year1 - 1;
            }
            else
            {
                time2 = time1 - 1;
                year2 = year1;
            }

            previousTimeframe = Convert.ToInt32(year2.ToString() + time2.ToString());

            return previousTimeframe;
        }

        /// <summary>
        /// Returns plan name as per the planID
        /// </summary>
        /// <param name="planID">Int32</param>
        /// <returns>string</returns>
        public string GetPlanName(Int32 planID)
        {
            string planName = string.Empty;
            using (PathfinderEntities context = new PathfinderEntities())
            {
                planName = (from p in context.PlanMasterSet
                            where p.Plan_ID == planID
                            select p.Plan_Name).FirstOrDefault();
            }

            return planName;
        }

        /// <summary>
        /// Returns prescriber geography name as per the geography id and type
        /// </summary>
        /// <param name="geopraphyID">string</param>
        /// <param name="type">string</param>
        /// <returns>string</returns>
        public string GetPrescriberGeographyName(string geographyID, string type)
        {
            string geographyName = string.Empty;

            using (PathfinderMarketplaceAnalyticsEntities context = new PathfinderMarketplaceAnalyticsEntities(Pinsonault.Web.Session.GetClientApplicationConnectionString("MarketplaceAnalytics")))
            {
                if (type == "region")
                {
                    geographyName = (from p in context.PrescribersRegionDistrictTerrSet
                                where p.Region_ID == geographyID
                                select p.Region_Name).FirstOrDefault();
                }
                if (type == "district")
                {
                    geographyName = (from p in context.PrescribersRegionDistrictTerrSet
                                     where p.District_ID == geographyID
                                     select p.District_Name).FirstOrDefault();
                }
                if (type == "territory")
                {
                    geographyName = (from p in context.PrescribersRegionDistrictTerrSet
                                     where p.Territory_ID == geographyID
                                     select p.Territory_Name).FirstOrDefault();
                }
            }

            return geographyName;
        }

        /// <summary>
        /// Returns top physicians per territory and timeframe
        /// </summary>
        /// <param name="tableName">string</param>
        /// <param name="queryVals">NameValueCollection</param>
        /// <param name="prescriberGeographyType">string</param>
        /// <param name="geographyID">string</param>
        /// <param name="dataField">string</param>
        /// <param name="dataYear">string</param>
        /// <param name="timeFrameVals">IList<int></param>
        /// <returns>IList<int></returns>
        public IList<string> GetTopPhysicians(string tableName, NameValueCollection queryVals, string prescriberGeographyType, string geographyID, string dataField, string dataYear, IList<int> timeFrameVals)
        {
            string take = (rollup == 5) ? "50" : "100"; //Take top 50 is rollup is 5, or else take top 100 if rollup is 6

            string tbName = string.Empty;
            if ((string.Compare(tableName, "MS_Prescriber_Monthly_Territory", true) == 0) || (string.Compare(tableName, "Physician_Monthly_Territory", true) == 0))
                tbName = "MS_Rtl_monthly_base";
            if ((string.Compare(tableName, "MS_Prescriber_Quarterly_Territory", true) == 0) || (string.Compare(tableName, "Physician_Quarterly_Territory", true) == 0))
                tbName = "MS_Rtl_quarterly_base";
            if ((string.Compare(tableName, "MS_Prescriber_Rolling_Quarterly_Territory", true) == 0) || (string.Compare(tableName, "Physician_Rolling_Quarterly_Territory", true) == 0))
                tbName = "MS_Rtl_rolling_quarterly_base";

            StringBuilder sbTop = new StringBuilder();
            sbTop.Append("SELECT Phys_IMS_Id FROM(SELECT TOP " + take + " Phys_IMS_Id, ");
            sbTop.Append("SUM(Product_" + queryVals["Trx_Mst"] + ") AS Total FROM tr." + tbName + " b ");
            sbTop.Append("LEFT OUTER JOIN tr.v_MS_Territory t ON b.Territory_Id = ");
            if (string.Compare(prescriberGeographyType, "region", true) == 0)
            {
                sbTop.Append("t.T3_Id WHERE t.T1_Id = '" + geographyID + "' AND ");
            }
            if (string.Compare(prescriberGeographyType, "district", true) == 0)
            {
                sbTop.Append("t.T3_Id WHERE t.T2_Id = '" + geographyID + "' AND ");
            }
            if (string.Compare(prescriberGeographyType, "territory", true) == 0)
            {
                sbTop.Append("t.T3_Id WHERE t.T3_Id = '" + geographyID + "' AND ");
            }

            sbTop.Append(dataField + " IN(" + String.Join(",", timeFrameVals.Select(x => x.ToString()).ToArray()) + ") ");

            if (string.Compare(queryVals["Calendar_Rolling"], "Calendar", true) == 0)
            {
                sbTop.Append("AND Data_Year = " + dataYear + " ");
            }
            sbTop.Append("GROUP BY Phys_IMS_Id ORDER BY	Total DESC) AS A");

            List<string> physIDArr = new List<string>();

            SqlConnection conn = null;
            SqlCommand com = null;
            SqlDataReader r = null;

            try
            {
                conn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString);
                com = new SqlCommand(sbTop.ToString(), conn);

                com.CommandType = CommandType.Text;
                conn.Open();
                r = com.ExecuteReader();
                while (r.Read())
                    physIDArr.Add(r["Phys_IMS_Id"].ToString());

                return physIDArr;
            }
            catch (Exception x)
            {
                throw new Exception(x.Message.ToString());
            }
            finally
            {
                if (conn != null)
                    conn.Dispose();
                if (com != null)
                    com.Dispose();
            }
        }

        /// <summary>
        /// Returns top plans per territory and timeframe
        /// </summary>
        /// <param name="tableName">string</param>
        /// <param name="queryVals">NameValueCollection</param>
        /// <param name="prescriberGeographyType">string</param>
        /// <param name="geographyID">string</param>
        /// <param name="dataField">string</param>
        /// <param name="dataYear">string</param>
        /// <param name="timeFrameVals">IList<int></param>
        /// <returns>IList<int></returns>
        public int[] GetTopPlansByPhysicians(string tableName, NameValueCollection queryVals, string prescriberGeographyType, string geographyID, string dataField, string dataYear, IList<int> timeFrameVals)
        {
            string take = (rollup == 2) ? "10" : "20"; //Take top 10 is rollup is 2, or else take top 20 if rollup is 3

            string tbName = string.Empty;
            if (string.Compare(tableName, "Physician_Monthly_Territory", true) == 0)
                tbName = "MS_Monthly_Base_Prescribers";
            if (string.Compare(tableName, "Physician_Quarterly_Territory", true) == 0)
                tbName = "MS_Quarterly_Base_Prescribers";
            if (string.Compare(tableName, "Physician_Rolling_Quarterly_Territory", true) == 0)
                tbName = "MS_Rolling_Quarterly_Base_Prescribers";

            StringBuilder sbTop = new StringBuilder();
            sbTop.Append("SELECT Plan_Id FROM(SELECT TOP " + take + " p.Plan_Id, ");
            sbTop.Append("SUM(Product_" + queryVals["Trx_Mst"] + ") AS Total FROM tr." + tbName + " b ");
            sbTop.Append("LEFT OUTER JOIN tr.v_MS_Territory t ON b.Territory_Id = t.T3_Id ");
            sbTop.Append("INNER JOIN PF.dbo.Plans p ON b.Plan_ID = p.Plan_ID ");
            if (string.Compare(prescriberGeographyType, "region", true) == 0)
            {
                sbTop.Append("WHERE t.T1_Id = '" + geographyID + "' AND ");
            }
            if (string.Compare(prescriberGeographyType, "district", true) == 0)
            {
                sbTop.Append("WHERE t.T2_Id = '" + geographyID + "' AND ");
            }
            if (string.Compare(prescriberGeographyType, "territory", true) == 0)
            {
                sbTop.Append("WHERE t.T3_Id = '" + geographyID + "' AND ");
            }

            sbTop.Append(dataField + " IN(" + String.Join(",", timeFrameVals.Select(x => x.ToString()).ToArray()) + ") ");

            if (string.Compare(queryVals["Calendar_Rolling"], "Calendar", true) == 0)
            {
                sbTop.Append("AND Data_Year = " + dataYear + " ");
            }
            if (!string.IsNullOrEmpty(queryVals["Section_ID"]))
                sbTop.Append("AND p.Section_ID =" + queryVals["Section_ID"] + " ");            
            
            sbTop.Append("GROUP BY p.Plan_Id ORDER BY	Total DESC) AS A");

            List<int> planIDArr = new List<int>();

            SqlConnection conn = null;
            SqlCommand com = null;
            SqlDataReader r = null;

            try
            {
                conn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString);
                com = new SqlCommand(sbTop.ToString(), conn);

                com.CommandType = CommandType.Text;
                conn.Open();
                r = com.ExecuteReader();
                while (r.Read())
                    planIDArr.Add(Convert.ToInt32(r["Plan_Id"].ToString()));

                return planIDArr.ToArray();
            }
            catch (Exception x)
            {
                throw new Exception(x.Message.ToString());
            }
            finally
            {
                if (conn != null)
                    conn.Dispose();
                if (com != null)
                    com.Dispose();
            }
        }

        public Dictionary<string, CriteriaItem> LoadCriteriaItems(NameValueCollection filters, Dictionary<string, CriteriaItem> items, System.Data.Objects.ObjectContext QueryContext, string report)
        {
            //Query for product names and add to collection
            QueryDefinition def = new QueryDefinition(filters);
            CriteriaItem item = new CriteriaItem("Product_ID", "Drug Selection");
            item.Text = string.Join(", ", Generic.CreateEntityQuery<DrugProducts>(QueryContext, def).Select(p => p.Product_Name).ToArray());
            items.Add(item.Key, item);

            //Add Geography
            string geography = filters["Geography"];

            if (report == "affiliation")
            {
                item = new CriteriaItem("Plan_ID", "Plan");
                
                if (string.Compare(geography, "US", true) == 0)
                    item.Text = "All National Plans";
                else if (!string.IsNullOrEmpty(filters["Plan_ID"]))
                    item.Text = GetPlanName(Convert.ToInt32(filters["Plan_ID"]));

                items.Add(item.Key, item);
            }
            if (report == "prescribers")
            {
                item = new CriteriaItem("Plan_ID", "Plan");
                
                if (!string.IsNullOrEmpty(filters["Plan_ID"]))
                    item.Text = GetPlanName(Convert.ToInt32(filters["Plan_ID"]));
                
                items.Add(item.Key, item);
            }

            if (report == "prescribertrending" || report == "prescribertrendingpopup")
            {
                if (string.Compare(filters["Prescriber_Geography_Type"], "Region", true) == 0)
                {
                    item = new CriteriaItem("Region_ID", "Region");
                    item.Text = GetPrescriberGeographyName(filters["Region_ID"], "region");
                    items.Add(item.Key, item);
                }
                if (string.Compare(filters["Prescriber_Geography_Type"], "District", true) == 0)
                {
                    item = new CriteriaItem("District_ID", "District");
                    item.Text = GetPrescriberGeographyName(filters["District_ID"], "district");
                    items.Add(item.Key, item);
                }
                if (string.Compare(filters["Prescriber_Geography_Type"], "Territory", true) == 0)
                {
                    item = new CriteriaItem("Territory_ID", "Territory");
                    item.Text = GetPrescriberGeographyName(filters["Territory_ID"], "territory");
                    items.Add(item.Key, item);
                }
            }
            else if (!string.IsNullOrEmpty(geography))
            {
                item = new CriteriaItem("Geography_ID", "Geography");
                if (string.Compare(geography, "US", true) == 0)
                    item.Text = "National";
                else
                {
                    item.Text = GetStateName(geography);

                    if (string.IsNullOrEmpty(item.Text))
                        item.Text = GetRegionName(geography);

                    //Check if Account Manager type was selected (only applicable for trending report)
                    if (report == "trending" && filters["Geography_Type"] == "3")
                        item.Text = GetAccountManagerName(geography);
                }     
                items.Add(item.Key, item);               
               
            }            

            //Check if 'Combined' Section is selected
            //If so - manually add criteria item since it is a hard-coded Section option
            if (filters["Section_ID"] == "-1")
            {
                item = new CriteriaItem("Section_ID", "Section");
                item.Text = "Combined";
                items.Add(item.Key, item);

                //Since hard-coded section breaks generic 'LoadCriteriaItems', also get the MarketBasket
                using (PathfinderModel.PathfinderEntities context = new PathfinderModel.PathfinderEntities())
                {
                    int theraID = Convert.ToInt32(filters["Market_Basket_ID"]);
                    item = new CriteriaItem("Market_Basket_ID", "Market Basket");

                    item.Text = (from p in context.TherapeuticClassMasterSet
                                 where p.ID == theraID
                                 select p.Name).FirstOrDefault();

                    items.Add(item.Key, item);
                }
            }

            //Add timeframe to output
            if (report == "trending" || report == "affiliation" || report == "prescribertrending" || report == "prescribertrendingpopup")
            {
                if (string.Compare(filters["Calendar_Rolling"], "Calendar", true) == 0) //Calendar Based
                {
                    //Add Year
                    item = new CriteriaItem("Year_Selection", "Year");
                    item.Text = filters["Year_Selection"];

                    items.Add(item.Key, item);

                    if (!string.IsNullOrEmpty(filters["Month_Selection"])) //Add Month
                    {
                        item = new CriteriaItem("Month_Selection", "Month(s)");

                        if (filters["Month_Selection"].IndexOf(',') > -1)
                        {
                            string[] timeFrameArr = filters["Month_Selection"].Split(',');
                            string[] timeFrameNames = new string[timeFrameArr.Length];

                            for (int x = 0; x < timeFrameArr.Count(); x++)
                                timeFrameNames[x] = GetTimeFrameName(Convert.ToInt32(timeFrameArr[x]), "month");

                            item.Text = string.Join(", ", timeFrameNames);
                        }
                        else
                            item.Text = GetTimeFrameName(Convert.ToInt32(filters["Month_Selection"]), "month");

                        items.Add(item.Key, item);
                    }
                    else //Add Quarter
                    {
                        item = new CriteriaItem("Quarter_Selection", "Quarter(s)");
                        item.Text = filters["Quarter_Selection"];

                        items.Add(item.Key, item);
                    }
                }
                else //Rolling Based
                {
                    item = new CriteriaItem("Rolling_Selection", "Rolling Quarter(s)");
                    item.Text = filters["Rolling_Selection"];

                    items.Add(item.Key, item);
                }
            }

            if (report == "formularyhistory")
            {
                string monthquarter;

                if (Convert.ToInt32(filters["Section_ID"]) == 17) //Med D is month based
                    monthquarter = "monthyear";
                else
                    monthquarter = "quarteryear";

                item = new CriteriaItem("Timeframe", "Timeframe(s)");

                if (filters["Timeframe"].IndexOf(',') > -1)
                {
                    string[] timeFrameArr = filters["Timeframe"].Split(',');
                    string[] timeFrameNames = new string[timeFrameArr.Length];

                    for (int x = 0; x < timeFrameArr.Count(); x++)
                        timeFrameNames[x] = GetTimeFrameName(Convert.ToInt32(timeFrameArr[x]), monthquarter);

                    item.Text = string.Join(", ", timeFrameNames);
                }
                else
                    item.Text = GetTimeFrameName(Convert.ToInt32(filters["Timeframe"]), monthquarter);

                items.Add(item.Key, item);                             
            }

            if (report == "comparison")
            {
                string selection; 
                    
                if (string.Compare(filters["IsDrilldown"], "true", true) == 0)    
                    selection = filters["Selection_Clicked"];
                else
                    selection = filters["Selection"];

                if (string.Compare(filters["Calendar_Rolling"], "Calendar", true) == 0) //Calendar Based
                {
                    //Add Year
                    item = new CriteriaItem(string.Format("Year{0}", selection), "Year Selection");
                    item.Text = filters[string.Format("Year{0}", selection)];

                    items.Add(item.Key, item);

                    if (filters[string.Format("MonthQuarter{0}", selection)] == "1") //It is a quarter
                    {
                        item = new CriteriaItem(string.Format("MonthQuarterSelection{0}", selection), "Quarter Selection");
                        item.Text = filters[string.Format("MonthQuarterSelection{0}", selection)];

                        items.Add(item.Key, item);
                    }
                    else //It is a month
                    {
                        item = new CriteriaItem(string.Format("MonthQuarterSelection{0}", selection), "Month Selection"); ;
                        item.Text = GetTimeFrameName(Convert.ToInt32(filters[string.Format("MonthQuarterSelection{0}", selection)]), "month");

                        items.Add(item.Key, item);
                    }
                }
                else //Rolling Based
                {
                    item = new CriteriaItem(string.Format("RollingQuarterSelection{0}", selection), "Rolling Quarter Selection");
                    item.Text = filters[string.Format("RollingQuarterSelection{0}", selection)];

                    items.Add(item.Key, item);
                }
            }

            //return results
            return items;
        }
    }
}