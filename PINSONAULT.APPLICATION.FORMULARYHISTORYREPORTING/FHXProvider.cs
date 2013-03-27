using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Pinsonault.Data.Reports;
using System.Data.Common;
using Pinsonault.Application;
using System.Web;
using System.Collections.Specialized;
using PathfinderModel;
using PathfinderClientModel;
using Pinsonault.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Objects;
using System.Globalization;
//using Microsoft.ApplicationBlocks.Data;

namespace Pinsonault.Application.FormularyHistoryReporting
{
    public class FHXProvider
    {
        string dataField = null;
        string tableName = null;
        string planLevelTableName = null;
        IList<string> keys = new List<string>();
        List<int> timeFrame = null;
        string segmentID = "1";

        /// <summary>
        /// Following values are derived from fhr.Lkp_Display_Options table
        /// </summary>
        public enum LkpDisplayOptions
        {
            Tier = 1,
            F_Status = 2,
            PA = 3,
            QL = 4,
            ST = 5,
            CoPay = 6
        }
        //extension of LKpDisplayOptions. did not disturb other functions. used only for Tier and Restrictions report
        public enum LkpDisplayOptionsEx
        {
            Tier = 1,
            F_Status = 2,
            PA = 3,
            QL = 4,
            ST = 5,
            CoPay = 6,
            Restrictions = 7
        }

        public Dictionary<string, CriteriaItem> LoadCriteriaItems(NameValueCollection filters, Dictionary<string, CriteriaItem> items, System.Data.Objects.ObjectContext QueryContext, string report)
        {
            //Query for product names and add to collection
            QueryDefinition def = new QueryDefinition(filters);
            CriteriaItem item = null;

            //Add Geography
            string geography = filters["Geography_ID"];

            if (!string.IsNullOrEmpty(geography))
            {
                items.Remove("geography_id");
                item = new CriteriaItem("Geography_ID", "Geography");
                if (string.Compare(geography, "US", true) == 0)
                    item.Text = "National";
                else
                {
                    item.Text = GetStateName(geography);

                    if (string.IsNullOrEmpty(item.Text))
                        item.Text = GetRegionName(geography);
                }

                items.Add(item.Key, item);
            }
            //add Benefit Design filter criteria           
            if (!string.IsNullOrEmpty(filters["Is_Predominant"]))
            {
                items.Remove("is_predominant");
                item = new CriteriaItem("Is_Predominant", "Benefit Design");
                item.Text = "Predominant";
                items.Add(item.Key, item);
            }

            if (!string.IsNullOrEmpty(filters["TimeFrameQtr"]) || !string.IsNullOrEmpty(filters["TimeFrameMonth"]))
            {
                string TimeFrameName = "Quarters";
                string TimeFrameNumber = filters["TimeFrameQtr"]; 

                if (filters["Monthly_Quarterly"] == "M")
                {
                    TimeFrameName = "Months";
                    TimeFrameNumber = filters["TimeFrameMonth"];
                }

                item = new CriteriaItem("TimeFrameQtr", "Time Frame");
                item.Text = string.Format("{0} {1} {2}", "Rolling", TimeFrameNumber, TimeFrameName);
                items.Add(item.Key, item);
            }

            //return results
            return items;
        }
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
        public string GetTimeFrameName(int id, string type)
        {
            string name = "";
            return name;
        }
        public string GetPlanName(Int32 planID)
        {
            string planName = string.Empty;
            using (PathfinderEntities context = new PathfinderEntities())
            {
                planName = (from p in context.fhrPlanSectionListSet
                            where p.ID == planID
                            select p.Name).FirstOrDefault();
            }

            return planName;
        }
        #region Tier Hx Report
               
        public IList<string> GetMaxQuarterMonthAllPlans(NameValueCollection queryVals, int DataYearQuarter)
        {
            SqlConnection conn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString);
            SqlCommand comm = new SqlCommand();
            IList<string> timeFrameMonths = new List<string>();

            string strClientDBName = string.Format("PF_{0}", Pinsonault.Web.Session.ClientKey);

            //comm.Connection = conn;

            int SegmentID = 1;
            if (queryVals["Section_ID"] == "17")
                SegmentID = 2;
            else if (queryVals["Section_ID"] == "9")
                SegmentID = 3;
            else if (queryVals["Section_ID"] == "6")
                SegmentID = 6;
            else if (queryVals["Section_ID"] == "4")
                SegmentID = 4;

            try
            {
                //StringBuilder sb = new StringBuilder();
                //sb.AppendFormat("select MAX(Data_Year_Month) AS Data_Year_Month from {0}.{1}.{2} ", strClientDBName, "fhr", "V_GetPlanProductFormularyHistory ");
                //sb.AppendFormat(" where Product_ID in ({0}) ", queryVals["Product_ID"]);
                //sb.AppendFormat(" and Segment_ID = {0} ", SegmentID);
                //sb.AppendFormat(" and Data_Year_Quarter = {0} ", DataYearQuarter);

                //comm.CommandText = sb.ToString();
                //conn.Open();
                //string strResult = comm.ExecuteScalar().ToString();

                //timeFrameMonths.Add(strResult);

                //conn.Close();
                switch (DataYearQuarter.ToString().Substring(5))
                {
                    case "1":
                        timeFrameMonths.Add(DataYearQuarter.ToString().Substring(0, 4) + "03");
                        break;
                    case "2":
                        timeFrameMonths.Add(DataYearQuarter.ToString().Substring(0, 4) + "06");
                        break;
                    case "3":
                        timeFrameMonths.Add(DataYearQuarter.ToString().Substring(0, 4) + "09");
                        break;
                    case "4":
                        timeFrameMonths.Add(DataYearQuarter.ToString().Substring(0, 4) + "12");
                        break;
                }

                return timeFrameMonths;
            }
            catch (SqlException)
            {
                switch(DataYearQuarter.ToString().Substring(5))
                {
                    case "1":
                        timeFrameMonths.Add(DataYearQuarter.ToString().Substring(0,4) + "03");
                        break;
                    case "2":
                        timeFrameMonths.Add(DataYearQuarter.ToString().Substring(0,4) + "06");
                        break;
                    case "3":
                        timeFrameMonths.Add(DataYearQuarter.ToString().Substring(0,4) + "09");
                        break;
                    case "4":
                        timeFrameMonths.Add(DataYearQuarter.ToString().Substring(0,4) + "12");
                        break;
                }
                return timeFrameMonths;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn != null) conn.Dispose();
                if (comm != null) comm.Dispose();

            }
        }

        public IEnumerable<GenericDataRecord> GetTierDataEx(IList<int> timeFrameVals, string geographyID, string tableName, string dataField, IList<string> keys, NameValueCollection queryVals)
        {
              //Query for records
            SqlConnection conn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString);
            SqlCommand comm = new SqlCommand();
            comm.CommandTimeout = 180;
            string strClientDBName = string.Format("PF_{0}",Pinsonault.Web.Session.ClientKey);

            string strplanClassificationID = "2,3"; //consider regional plans          

            string[] ProductIDarr = queryVals["Product_ID"].Split(',');

            string trxMst;
            trxMst = queryVals["Trx_Mst"];
            if (string.IsNullOrEmpty(trxMst))
                trxMst = "Trx";

            dataField = "Data_Year_Month";
            if (queryVals["Monthly_Quarterly"] == "Q")
            {
                IList<int> timeFrameForQ = new List<int>();
                timeFrameForQ.Add(Convert.ToInt32(timeFrameVals[0]));
                timeFrameForQ.Add(Convert.ToInt32(timeFrameVals[1]));

                timeFrameVals.Clear();

                IList<string> monthForQuarter = new List<string>();

                monthForQuarter = GetMaxQuarterMonthAllPlans(queryVals, timeFrameForQ[0]);
                timeFrameVals.Add(Convert.ToInt32(monthForQuarter.FirstOrDefault().ToString()));

                monthForQuarter.Clear();

                monthForQuarter = GetMaxQuarterMonthAllPlans(queryVals, timeFrameForQ[1]);
                timeFrameVals.Add(Convert.ToInt32(monthForQuarter.FirstOrDefault().ToString()));
            }

            comm.Connection = conn;

            IList<string> keysPercent = null;
            keysPercent = keys.ToList();
            keysPercent.Remove("Tier_No");
            //List<string> drugs = new List<string>();
            try
            {                
                StringBuilder sb = new StringBuilder();
                StringBuilder sbProdQuery = new StringBuilder();              
               
                int c = 0;
                
                timeFrame = timeFrameVals.ToList();

                int SegmentID = 1;
                if (queryVals["Section_ID"] == "17")
                    SegmentID = 2;
                else if (queryVals["Section_ID"] == "9")
                    SegmentID = 3;
                else if (queryVals["Section_ID"] == "6")
                    SegmentID = 6;
                else if (queryVals["Section_ID"] == "4")
                    SegmentID = 4;
                //....

                sb.AppendFormat(" With warnerTable AS ( " );

                sb.AppendFormat(" select Drug_ID, Drug_Name, Product_ID,Product_Name, Tier_No,{0}, Formulary_Lives",dataField);
                if (geographyID.ToLower() != "us")
                    sb.AppendFormat(" ,Geography_ID ");
                sb.AppendFormat(" from {0}.{1}.{2} ", strClientDBName, "fhr", tableName);
                sb.AppendFormat(" where {0} in ({1},{2}) ", dataField, timeFrame[0], timeFrame[1]);
                sb.AppendFormat(" and Product_ID in ({0}) ", queryVals["Product_ID"]);

                if (queryVals["Section_ID"] == "-1")
                    sb.AppendFormat(" and Segment_ID in ({0}) ", "1,2,6"); //combined section
                else
                    sb.AppendFormat(" and Segment_ID = ({0}) ", SegmentID);

                if (geographyID.ToLower() != "us")
                    sb.AppendFormat(" and Geography_ID = '{0}' ", queryVals["Geography_ID"]);
                sb.AppendFormat(" and Plan_Classification_ID in ({0}) ", strplanClassificationID);

                sb.AppendFormat(" ) ");

                //sb.AppendFormat(" SELECT lkp.Tier_ID,lkp.Tier_Name, ");
                sb.AppendFormat(" SELECT Drug0.Tier_No As Tier_ID, (Select Tier_Name from PF.dbo.Lkp_Tiers where Tier_ID = Drug0.Tier_No) AS Tier_Name, ");
                
                sb.AppendFormat(" Drug{0}.Product_ID As Drug{1}, Drug{0}.Formulary_Lives0 As Drug{1}_TotalCovered1, Drug{0}.Formulary_Lives1 As Drug{1}_TotalCovered2 ", c, c + 1);

                if (string.Compare(trxMst, "mst", true) == 0) //if % is required  
                {
                    sb.AppendFormat(" ,Drug{0}.Drug_Percent1 AS Drug{1}_Percent1 ", c, c + 1);
                    sb.AppendFormat(" ,Drug{0}.Drug_Percent2 AS Drug{1}_Percent2 ", c, c + 1);
                }
                sb.AppendFormat(" ,Drug{0}.Product_Name AS Drug{1}_Name ", c, c + 1);
                sb.AppendFormat(" ,Drug{0}.Tier_No AS Drug{1}_TierID ", c, c + 1);

                

                sbProdQuery.AppendFormat(" ( Select A.* ");
                if (string.Compare(trxMst, "mst", true) == 0) //if % is required  
                {
                    sbProdQuery.AppendFormat(", Case when B.Formulary_Lives0 > 0 Then (A.Formulary_Lives0*100.00/B.Formulary_Lives0) ELSE 0.00 END As Drug_Percent1 ");
                    sbProdQuery.AppendFormat(", Case when B.Formulary_Lives1 > 0 Then (A.Formulary_Lives1*100.00/B.Formulary_Lives1) ELSE 0.00 END As Drug_Percent2 ");
                }
                sbProdQuery.AppendFormat(" FROM ");

                sbProdQuery.AppendFormat(" (Select Drug_ID,Drug_Name,Product_ID,Product_Name,Tier_No,ISNULL([{0}], 0) as Formulary_Lives0,ISNULL([{1}], 0) as Formulary_Lives1", timeFrame[0], timeFrame[1]);
                if (geographyID.ToLower() != "us")
                    sbProdQuery.AppendFormat(" ,Geography_ID ");

                sbProdQuery.AppendFormat(" from warnerTable AS SourceTable ");
                sbProdQuery.AppendFormat(" pivot(SUM(Formulary_Lives) for {0} in ([{1}], [{2}]) ) as pivotTable0 ", dataField, timeFrame[0], timeFrame[1]);
                sbProdQuery.AppendFormat(" where Product_ID in ({0}) ) A ", queryVals["Product_ID"]);
                //insert % query here
                if (string.Compare(trxMst, "mst", true) == 0) //if % is required  
                {
                    sbProdQuery.AppendFormat(" Left join ");
                    sbProdQuery.AppendFormat(" (Select Drug_ID,Product_ID,Product_Name,ISNULL([{0}], 0) as Formulary_Lives0 ,ISNULL([{1}], 0) as Formulary_Lives1", timeFrame[0], timeFrame[1]);
                    if (geographyID.ToLower() != "us")
                        sbProdQuery.AppendFormat(" ,Geography_ID ");
                    sbProdQuery.AppendFormat(" from ");
                    sbProdQuery.AppendFormat(" (Select Drug_ID,Product_ID,Product_Name, {0} ,Formulary_Lives ", dataField);
                    if (geographyID.ToLower() != "us")
                        sbProdQuery.AppendFormat(" ,Geography_ID ");
                    sbProdQuery.AppendFormat(" from warnerTable where Product_ID in ({0}) ) AS SourceTable pivot(SUM(Formulary_Lives) ", queryVals["Product_ID"]);
                    sbProdQuery.AppendFormat(" for {0} in ([{1}], [{2}]) ", dataField, timeFrame[0], timeFrame[1]);
                    sbProdQuery.AppendFormat(" ) as pivotTable0)B ON A.Product_ID = B.Product_ID and A.Drug_ID = B.Drug_ID ");
                    if (geographyID.ToLower() != "us")
                        sbProdQuery.AppendFormat(" and A.Geography_ID = B.Geography_ID ");
                }

                sbProdQuery.AppendFormat(" )Drug{0}", c);


                //sb.AppendFormat(" from {0} order by Drug1 , Drug1_TierID", sbProdQuery);
                sb.AppendFormat(" from {0} WHERE Drug0.Tier_No is not null order by Drug0.Product_Name , Tier_Name ", sbProdQuery);

                comm.CommandText = sb.ToString();

                conn.Open();
                DbDataReader rdr = comm.ExecuteReader();
                IEnumerable<GenericDataRecord> g = Pinsonault.Data.GenericDataRecord.CreateCollection(rdr);
                //DataSet g = SqlHelper.ExecuteDataset(conn, CommandType.Text, comm.CommandText);
                
                conn.Close();

                return g;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn != null) conn.Dispose();
                if (comm != null) comm.Dispose();

            }

        }
        /// <summary>
        /// for getting the Tier Coverage Historical Formulary, Restrictions Hx report for Tier drilldown or coverage status drilldown Report data
        /// </summary>
        /// <param name="timeFrameVals"></param>
        /// <param name="geographyID"></param>
        /// <param name="tableName"></param>
        /// <param name="dataField"></param>
        /// <param name="keys"></param>
        /// <param name="queryVals"></param>
        /// <returns></returns>
        public IEnumerable<GenericDataRecord> GetTierOrCoverageDrilldownData(IList<int> timeFrameVals, string geographyID, string tableName, string dataField, IList<string> keys, NameValueCollection queryVals)
        {
            //Query for records
            SqlConnection conn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString);
            SqlCommand comm = new SqlCommand();
            comm.CommandTimeout = 180;


            string strplanClassificationID = "2,3"; //consider regional plans
            string[] planClassificationIDarr = strplanClassificationID.Split(',');

            string[] productIDarr = queryVals["Product_ID"].Split(',');
            tableName = "V_GetPlanProductFormularyHistory";

            dataField = "Data_Year_Month";
            if (queryVals["Monthly_Quarterly"] == "Q")
            {
                IList<int> timeFrameForQ = new List<int>();
                timeFrameForQ.Add(Convert.ToInt32(timeFrameVals[0]));
                timeFrameForQ.Add(Convert.ToInt32(timeFrameVals[1]));

                timeFrameVals.Clear();

                IList<string> monthForQuarter = new List<string>();

                monthForQuarter = GetMaxQuarterMonthAllPlans(queryVals, timeFrameForQ[0]);
                timeFrameVals.Add(Convert.ToInt32(monthForQuarter.FirstOrDefault().ToString()));

                monthForQuarter.Clear();

                monthForQuarter = GetMaxQuarterMonthAllPlans(queryVals, timeFrameForQ[1]);
                timeFrameVals.Add(Convert.ToInt32(monthForQuarter.FirstOrDefault().ToString()));
            }

            comm.Connection = conn;
            //List<string> drugs = new List<string>();
            try
            {
                StringBuilder sb = new StringBuilder();

                Pinsonault.Data.SQLUtil.MaxFragmentLength = 35;
                timeFrame = timeFrameVals.ToList();

                int SegmentID = 1;
                if (queryVals["Section_ID"] == "17")
                    SegmentID = 2;
                else if (queryVals["Section_ID"] == "9")
                    SegmentID = 3;
                else if (queryVals["Section_ID"] == "6")
                    SegmentID = 6;
                else if (queryVals["Section_ID"] == "4")
                    SegmentID = 4;

                if (queryVals["Section_ID"] == "-1") //Channel is Combined - use segment ID of 1,2 & 6
                {
                    comm.Parameters.Add("@SegmentID0", SqlDbType.Int).Value = 2;
                    comm.Parameters.Add("@SegmentID1", SqlDbType.Int).Value = 1;
                    comm.Parameters.Add("@SegmentID2", SqlDbType.Int).Value = 6;

                    //segmentID = "1,2,6";
                }
                else
                    comm.Parameters.AddWithValue("@SegmentID", SegmentID);

                //string[] geographyIDarr = queryVals["Geography_ID"].Split(',');
                
                SQLPivotQuery<int> query = null;
                
                query = SQLPivotQuery<int>.Create(tableName, string.Format("PF_{0}", Pinsonault.Web.Session.ClientKey), "fhr", keys, dataField, timeFrame).ConvertNullToEmptyString();

                if (!string.IsNullOrEmpty(queryVals["Tier_ID"]))
                    query.Pivot(SQLFunction.MAX, "Tier_Name");

                if (!string.IsNullOrEmpty(queryVals["Coverage_Status_ID"]))
                {
                    query.Pivot(SQLFunction.MAX, "Coverage_Status_ID");
                    //query.Pivot(SQLFunction.MAX, "Coverage_Status_Name");
                }

                comm.Parameters.AddWithValue("@productID", Convert.ToInt32(productIDarr[0]));
                query.Where("Product_ID", "productID", SQLOperator.EqualTo);
                               
                if (queryVals["Section_ID"] == "-1")
                    query.Where("Segment_ID", "SegmentID", SQLOperator.In, 3);
                else
                    query.Where("Segment_ID", "SegmentID", SQLOperator.EqualTo);
                
                //if (geographyIDarr.Count() > 0)
                //{
                //    for (int x = 0; x < geographyIDarr.Count(); x++)
                //        comm.Parameters.Add(string.Format("@Geography_ID0{0}", x), SqlDbType.NVarChar).Value = geographyIDarr[x];

                //    query.Where("Geography_ID", "Geography_ID0", SQLOperator.In, geographyIDarr.Count());
                //}   
                if(geographyID.ToLower() != "us")
                    comm.Parameters.Add(string.Format("@Geography_ID0{0}", 0), SqlDbType.NVarChar).Value = geographyID;
               
                //include Plan and Formulary lives from latest data always
                //string extView = "Plan_Drug_Formulary";

                //comm.CommandText = string.Format("Select Base.*,ext.Plan_Name,ext.Product_Name as Plan_Product_Name, ext.Plan_State_ID,ext.Plan_Pharmacy_Lives,ext.Formulary_Name,ext.Formulary_Lives,ext.Drug_Name from ({0}) Base Inner Join PF.[dbo].[{1}] ext ON base.Plan_ID = ext.Plan_ID and base.Drug_ID = ext.Drug_ID and Base.Segment_ID = ext.Segment_ID and Base.Pinso_Formulary_ID = ext.Pinso_Formulary_ID and Base.Plan_Product_ID = ext.Product_ID and ext.Plan_Classification_ID in ({2})", query, extView,strplanClassificationID);
                comm.CommandText = string.Format("Select Base.* ");
                if (!string.IsNullOrEmpty(queryVals["Coverage_Status_ID"]))
                    comm.CommandText = string.Format(" {0}  ,(Select Coverage_Status_Name from PF.dbo.Lkp_Coverage_Status where Coverage_Status_ID = Coverage_Status_ID0) As Coverage_Status_Name0, (Select Coverage_Status_Name from PF.dbo.Lkp_Coverage_Status where Coverage_Status_ID = Coverage_Status_ID1) As Coverage_Status_Name1 ", comm.CommandText);

                comm.CommandText = string.Format(" {0} from ({1}) Base where Base.Plan_Classification_ID in ({2})", comm.CommandText, query, strplanClassificationID);
                
                //filter for selected time frame Tier
                string timeIndex = "0";
                if (!string.IsNullOrEmpty(queryVals["TierTimeIndex"]))
                    timeIndex = "1";

                if (!string.IsNullOrEmpty(queryVals["Tier_ID"]))
                    using (PathfinderEntities pf = new PathfinderEntities())
                    {
                        int tierid = Convert.ToInt32(queryVals["Tier_ID"]);
                        var tiername = (from tierset in pf.TierSet where tierset.ID == tierid select tierset.Name).FirstOrDefault();
                        comm.CommandText = string.Format("{0}{1}", comm.CommandText, string.Format(" and Base.Tier_Name{0} = '{1}' ",timeIndex, tiername));
                    }

                if (!string.IsNullOrEmpty(queryVals["Coverage_Status_ID"]))
                {
                    int coverageStatusID = Convert.ToInt32(queryVals["Coverage_Status_ID"]);
                    comm.CommandText = string.Format("{0}{1}", comm.CommandText, string.Format(" and Base.Coverage_Status_ID{0} = '{1}' ", timeIndex, coverageStatusID));
                }
               
                //Wrap query for paging
                string pagingEnabled = queryVals["PagingEnabled"];
                if (!string.IsNullOrEmpty(pagingEnabled))
                {
                    if (Convert.ToBoolean(pagingEnabled) == true)
                    {
                        int startPage = Convert.ToInt32(queryVals["StartPage"]);
                        int totalPerPage = Convert.ToInt32(queryVals["TotalPerPage"]);

                        int rowNumber = (startPage * totalPerPage) - totalPerPage;

                        StringBuilder sb1 = new StringBuilder();
                        sb1.AppendFormat("SELECT TOP {0} * FROM ", totalPerPage);
                        sb1.Append("(SELECT ");
                        sb1.Append("ROW_NUMBER() OVER (Order By Plan_Name asc, Formulary_Lives Desc, Product_Name asc) ");
                        sb1.Append("AS RowNumber, * ");
                        sb1.AppendFormat("FROM ({0}) AS Results2) AS Results WHERE RowNumber > {1} ORDER BY RowNumber, Drug_Name", comm.CommandText, rowNumber);

                        comm.CommandText = sb1.ToString();
                    }
                    else if (string.IsNullOrEmpty(queryVals["Export"]))//Only select count if not export
                    {
                        StringBuilder sb1 = new StringBuilder();
                        sb1.AppendFormat("SELECT COUNT(0) AS 'RowCount' FROM ({0}) AS FullCount", comm.CommandText);

                        comm.CommandText = sb1.ToString();
                    }
                }

                conn.Open();
                DbDataReader rdr = comm.ExecuteReader();
                IEnumerable<GenericDataRecord> g = Pinsonault.Data.GenericDataRecord.CreateCollection(rdr);
                conn.Close();

                return g;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn != null) conn.Dispose();
                if (comm != null) comm.Dispose();

            }
        }

        /// <summary>
        /// for getting the Tier Coverage Historical Formulary summary Report data for Trx (or Mst) for a given selection
        /// </summary>
        /// <param name="timeFrameVals"></param>
        /// <param name="geographyID"></param>
        /// <param name="tableName"></param>
        /// <param name="dataField"></param>
        /// <param name="keys"></param>
        /// <param name="queryVals"></param>
        /// <returns></returns>
        public IEnumerable<GenericDataRecord> GetTierTRXData(IList<int> timeFrameVals, string geographyID, string tableName, string dataField, IList<string> keys, NameValueCollection queryVals)
        {
            //Query for records
            SqlConnection conn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString);
            SqlCommand comm = new SqlCommand();
            comm.CommandTimeout = 180;

            string[] productIDarr = queryVals["Product_ID"].Split(',');

            string trxMst;
            trxMst = queryVals["Trx_Mst"];
            if (string.IsNullOrEmpty(trxMst))
                trxMst = "Trx";
            comm.Connection = conn;
           
            try
            {
                StringBuilder sb = new StringBuilder();              

                Pinsonault.Data.SQLUtil.MaxFragmentLength = 35;
                timeFrame = timeFrameVals.ToList();
            
                string[] geographyIDarr = queryVals["Geography_ID"].Split(',');

                SQLPivotQuery<int> query = null;
                
                query = SQLPivotQuery<int>.Create(tableName, string.Format("PF_{0}",Pinsonault.Web.Session.ClientKey) , "tr", keys, dataField, timeFrame);

                if (!string.IsNullOrEmpty(queryVals["Section_ID"]))
                    ProcessSectionSegment(ref comm, ref query, queryVals);

                if (string.Compare(trxMst, "trx", true) == 0)
                {
                    query.Pivot(SQLFunction.SUM, "Product_TRx");
                }
                if (string.Compare(queryVals["Trx_Mst"], "mst", true) == 0)
                {
                    //query.Pivot(SQLFunction.SUM, "Product_MST");
                    query.Pivot(SQLFunction.SUM, "Product_TRx");
                    query.Pivot(SQLFunction.SUM, "MB_Trx");
                }                          

                if (productIDarr.Count() > 0)
                {
                    for (int x = 0; x < productIDarr.Count(); x++)
                        comm.Parameters.Add(string.Format("@productID{0}", x), SqlDbType.Int).Value = Convert.ToInt32(productIDarr[x]);

                    query.Where("Product_ID", "productID", SQLOperator.In, productIDarr.Count());
                }

                if (geographyIDarr.Count() > 0)
                {
                    for (int x = 0; x < geographyIDarr.Count(); x++)
                        comm.Parameters.Add(string.Format("@Geography_ID0{0}", x), SqlDbType.NVarChar).Value = geographyIDarr[x];

                    query.Where("Geography_ID", "Geography_ID0", SQLOperator.In, geographyIDarr.Count());
                }
               
                sb.AppendFormat("( SELECT * FROM ({0}) AS Drug0)", query);
                comm.CommandText = string.Format("{0}{1}", comm.CommandText, " where Is_Predominant = 1 ");
                if (string.Compare(trxMst, "trx", true) == 0)
                {
                    sb.Append("Order By Product_Name");
                }
                comm.CommandText = sb.ToString();
                
                if (string.Compare(trxMst, "mst", true) == 0)
                {
                    IList<string> colsTrxToSum = new List<string>();
                    IList<string> colsMBTrxToSum = new List<string>();
                    StringBuilder sbMst = new StringBuilder();
                    sbMst.Append("SELECT TOP 100 PERCENT *, ");

                    for (int x = 0; x < timeFrame.Count; x++)
                    {
                        sbMst.AppendFormat("Product_Mst{0} = CASE ISNULL(MB_Trx{0}, 0) WHEN 0 THEN 0.000 ELSE (Product_Trx{0} / MB_Trx{0}) * 100 END,", x);

                        colsTrxToSum.Add(string.Format("Product_Trx{0}", x));
                        colsMBTrxToSum.Add(string.Format("MB_Trx{0}", x));
                    }

                    sbMst.AppendFormat("Product_Mst_Sum = CASE({0}) WHEN 0 THEN 0.000 ELSE (({1})/({2})) END ", string.Join(" + ", colsMBTrxToSum.ToArray()), string.Join(" + ", colsTrxToSum.ToArray()), string.Join(" + ", colsMBTrxToSum.ToArray()));
                    sbMst.AppendFormat("FROM ({0}) AS T2 ORDER BY Product_Name", comm.CommandText);

                    //Remove Drug_ID from WHERE clause in case Product is in results
                    comm.CommandText = sbMst.ToString().Replace("and pivotTable0.Drug_ID = pivotTable1.Drug_ID", "");
                }
               
                conn.Open();
                DbDataReader rdr = comm.ExecuteReader();
                IEnumerable<GenericDataRecord> g = Pinsonault.Data.GenericDataRecord.CreateCollection(rdr);
                conn.Close();

                return g;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn != null) conn.Dispose();
                if (comm != null) comm.Dispose();

            }
        }
       
        /// <summary>
        /// for Tier HX report , Trx Drilldown, which give comparision of Tier, copay, restrictions and Trx (or Mst) for a given selection
        /// </summary>
        /// <param name="timeFrameVals"></param>
        /// <param name="geographyID"></param>
        /// <param name="tableName"></param>
        /// <param name="dataField"></param>
        /// <param name="keys"></param>
        /// <param name="queryVals"></param>       
        /// <param name="mstableName"></param>
        /// <param name="mskeys"></param>
        /// <param name="msdataField"></param>
        /// <returns></returns>
        public IEnumerable<GenericDataRecord> GetTierTRXDrilldownData(IList<int> timeFrameVals, string geographyID, string tableName, string dataField, IList<string> keys, NameValueCollection queryVals, string mstableName, IList<string> mskeys, string msdataField)
        {
            //Query for records
            SqlConnection conn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString);
            SqlCommand comm = new SqlCommand();
            comm.CommandTimeout = 180;

            timeFrame = timeFrameVals.ToList(); //for market share cosider Quarter but for formulary data consider max data month to avoid duplicate

            dataField = "Data_Year_Month";
            if (queryVals["Monthly_Quarterly"] == "Q")
            {
                IList<int> timeFrameForQ = new List<int>();
                timeFrameForQ.Add(Convert.ToInt32(timeFrameVals[0]));
                timeFrameForQ.Add(Convert.ToInt32(timeFrameVals[1]));

                timeFrameVals.Clear();

                IList<string> monthForQuarter = new List<string>();

                monthForQuarter = GetMaxQuarterMonthAllPlans(queryVals, timeFrameForQ[0]);
                timeFrameVals.Add(Convert.ToInt32(monthForQuarter.FirstOrDefault().ToString()));

                monthForQuarter.Clear();

                monthForQuarter = GetMaxQuarterMonthAllPlans(queryVals, timeFrameForQ[1]);
                timeFrameVals.Add(Convert.ToInt32(monthForQuarter.FirstOrDefault().ToString()));
            }

            string productid = queryVals["Product_ID"];            

            string trxMst;
            trxMst = queryVals["Trx_Mst"];
            if (string.IsNullOrEmpty(trxMst))
                trxMst = "Trx";

            comm.Connection = conn;
            //change table names //todo: move this code later to calling function
            tableName = "V_GetPlanProductFormularyHistory";
            mstableName = "MS_MonthlyByState";

             if (queryVals["Monthly_Quarterly"] == "Q")
                 mstableName = "MS_QuarterlyByState";            

            try
            {               
                //create query for Tier,copay and Restrictions comparision

               StringBuilder sbQuery1 = new StringBuilder();
                sbQuery1.Append("select V_GetPlanProductFormularyHistory.Plan_ID,V_GetPlanProductFormularyHistory.Drug_ID,V_GetPlanProductFormularyHistory.Product_ID, V_GetPlanProductFormularyHistory.Product_Name, V_GetPlanProductFormularyHistory.Pinso_Formulary_ID,V_GetPlanProductFormularyHistory.Segment_ID,V_GetPlanProductFormularyHistory.Plan_Product_ID, V_GetPlanProductFormularyHistory.Geography_ID, V_GetPlanProductFormularyHistory.Geography_Name,Data_Year_Month,Tier_No,Tier_Name,Restrictions , Co_Pay, Section_ID ");
                sbQuery1.Append(",Plan_State_ID,Plan_Pharmacy_Lives,Formulary_Name,Formulary_Lives,Drug_Name,Is_Predominant,Plan_Classification_ID,Plan_Name ");
                
                sbQuery1.AppendFormat(" from PF_{0}.{1}.{2} ", Pinsonault.Web.Session.ClientKey, "fhr", tableName);

                string sb1 = string.Format("{0} where {1} = {2} ", sbQuery1.ToString(), dataField, timeFrameVals[0]);
                string sb2 = string.Format("{0} where {1} = {2} ", sbQuery1.ToString(), dataField, timeFrameVals[1]);

                StringBuilder sbQuery = new StringBuilder();
                sbQuery.AppendFormat("select A.Section_ID, A.Plan_ID, A.Drug_ID, A.Product_ID,A.Product_Name, A.Pinso_Formulary_ID,A.Segment_ID,A.Plan_Product_ID, A.Geography_ID, A.Geography_Name, A.Tier_No As Tier0,A.Tier_Name AS Tier_Name0,A.Restrictions AS Restrictions0, A.Co_Pay AS Co_Pay0,B.Tier_No As Tier1,B.Tier_Name AS Tier_Name1,B.Restrictions AS Restrictions1 , B.Co_Pay AS Co_Pay1 ");
                sbQuery.AppendFormat(",A.Plan_State_ID,A.Plan_Pharmacy_Lives,A.Formulary_Name,A.Formulary_Lives,A.Drug_Name,A.Is_Predominant,A.Plan_Classification_ID,A.Plan_Name from ({0})A ",sb1);
                sbQuery.AppendFormat(" full outer join ({0})B ON A.Geography_ID = B.Geography_ID and A.Plan_ID = B.Plan_ID and A.Plan_Product_ID = B.Plan_Product_ID and A.Segment_ID = B.Segment_ID and A.Section_ID = B.Section_ID and A.Drug_ID = B.Drug_ID and A.Product_ID = B.Product_ID and A.Pinso_Formulary_ID = B.Pinso_Formulary_ID ", sb2);
                //add product geography filters               

                //Filter the query for Section and Segment id for all the sections except for Med D and Managed Medicaid
                //For Med D, filter by only segemnt ID
                int SegmentID = 1;
                if (queryVals["Section_ID"] == "17")
                    SegmentID = 2;
                else if (queryVals["Section_ID"] == "9")
                    SegmentID = 3;
                else if (queryVals["Section_ID"] == "6")
                    SegmentID = 6;
                else if (queryVals["Section_ID"] == "4")
                    SegmentID = 4;

               //segmentid
                if (queryVals["Section_ID"] == "-1")
                    sbQuery.AppendFormat(" where A.Segment_ID in (1,6,17) ", SegmentID); //combined 1, 6,17
                else
                    sbQuery.AppendFormat(" where A.Segment_ID = {0}" , SegmentID);

                if (queryVals["Section_ID"] != "17" && queryVals["Section_ID"] != "6")
                {                   
                    sbQuery.AppendFormat(" and A.Section_ID = {0} ", Convert.ToInt32(queryVals["Section_ID"]));
                }

                //filter Geography_ID                
                if (!string.IsNullOrEmpty(queryVals["Geography_ID"]) && geographyID.ToLower() != "us")
                    sbQuery.AppendFormat(" and A.Geography_ID = '{0}' ", queryVals["Geography_ID"].ToString());

                //filter Product_ID               
                if (!string.IsNullOrEmpty(queryVals["Product_ID"]))
                    sbQuery.AppendFormat(" and A.Product_ID = {0} ", Convert.ToInt32(queryVals["Product_ID"]));               

                comm.CommandText = sbQuery.ToString();
               
                //include Plan and Formulary lives from latest data always
                string extView = "Plan_Drug_Formulary";                

                SQLPivotQuery<int> mpaQuery = SQLPivotQuery<int>.Create(mstableName, string.Format("PF_{0}", Pinsonault.Web.Session.ClientKey), "tr", mskeys, msdataField, timeFrame);

                if (!string.IsNullOrEmpty(queryVals["Section_ID"]))
                    ProcessSectionSegment(ref comm, ref mpaQuery, queryVals);

                if (string.Compare(queryVals["Trx_Mst"], "mst", true) == 0)
                //mpaQuery.Pivot(SQLFunction.SUM, "Product_Mst");
                {
                    mpaQuery.Pivot(SQLFunction.SUM, "Product_TRx");
                    mpaQuery.Pivot(SQLFunction.SUM, "MB_Trx");
                }
                else
                    mpaQuery.Pivot(SQLFunction.SUM, "Product_Trx");

                string[] productIDarr = queryVals["Product_ID"].Split(',');

                if (productIDarr.Count() > 0)
                {
                    for (int x = 0; x < productIDarr.Count(); x++)
                    {
                        if (!comm.Parameters.Contains(string.Format("@productID{0}", x)))
                            comm.Parameters.Add(string.Format("@productID{0}", x), SqlDbType.Int).Value = Convert.ToInt32(productIDarr[x]);
                    }

                    mpaQuery.Where("Product_ID", "productID", SQLOperator.In, productIDarr.Count());
                }
               
                string[] geographyIDarr = queryVals["Geography_ID"].Split(',');
                if (geographyIDarr.Count() > 0 && geographyID.ToLower() != "us")
                {
                    for (int x = 0; x < geographyIDarr.Count(); x++)
                    {
                        if (!comm.Parameters.Contains(string.Format("@Geography_ID{0}", x)))
                            comm.Parameters.Add(string.Format("@Geography_ID{0}", x), SqlDbType.NVarChar).Value = geographyIDarr[x];
                    }

                    mpaQuery.Where("Geography_ID", "Geography_ID", SQLOperator.In, geographyIDarr.Count());
                }
                StringBuilder sbMst = new StringBuilder();
                string mpaQueryString = "";
                if (string.Compare(trxMst, "mst", true) == 0)
                {
                    IList<string> colsTrxToSum = new List<string>();
                    IList<string> colsMBTrxToSum = new List<string>();

                    sbMst.Append("SELECT TOP 100 PERCENT *, ");

                    for (int x = 0; x < timeFrame.Count; x++)
                    {
                        sbMst.AppendFormat("Product_Mst{0} = CASE ISNULL(MB_Trx{0}, 0) WHEN 0 THEN 0.000 ELSE (Product_Trx{0} / MB_Trx{0}) * 100 END,", x);

                        colsTrxToSum.Add(string.Format("Product_Trx{0}", x));
                        colsMBTrxToSum.Add(string.Format("MB_Trx{0}", x));
                    }

                    sbMst.AppendFormat("Product_Mst_Sum = CASE({0}) WHEN 0 THEN 0.000 ELSE (({1})/({2})) END ", string.Join(" + ", colsMBTrxToSum.ToArray()), string.Join(" + ", colsTrxToSum.ToArray()), string.Join(" + ", colsMBTrxToSum.ToArray()));
                    sbMst.AppendFormat("FROM ({0}) AS T2 ORDER BY Product_Name", mpaQuery.ToString());

                    //Remove Drug_ID from WHERE clause in case Product is in results
                    mpaQueryString = sbMst.ToString().Replace("and pivotTable0.Drug_ID = pivotTable1.Drug_ID", "");
                }
                else
                    mpaQueryString = mpaQuery.ToString();

                //combine trx and historical formulary data
                comm.CommandText = string.Format("Select Base.*, IsNULL(Product_{3}0,0) as Product_{3}0 , IsNULL(Product_{3}1,0) as Product_{3}1 from ({0}) Base Inner Join PF.[dbo].[{1}] ext ON base.Plan_ID = ext.Plan_ID and base.Drug_ID = ext.Drug_ID and Base.Segment_ID = ext.Segment_ID and Base.Pinso_Formulary_ID = ext.Pinso_Formulary_ID and Base.Plan_Product_ID = ext.Product_ID Left Join ({2}) mpadata  ON base.Plan_ID = mpadata.Plan_ID and base.Drug_ID = mpadata.Drug_ID and Base.Segment_ID = mpadata.Segment_ID ", comm.CommandText, extView, mpaQueryString, queryVals["Trx_Mst"]);

                //filter added for Is_Predominant = true in case the checkbox is checked for Predominant benefit design
                //if (!string.IsNullOrEmpty(queryVals["Is_Predominant"]))
                comm.CommandText = string.Format("{0}{1}", comm.CommandText, " where Base.Is_Predominant = 1 and Base.Plan_Classification_ID in (2,3) "); //filter predominant and regional plans only

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
                        sb.Append("ROW_NUMBER() OVER (ORDER BY Plan_name asc, Plan_Pharmacy_Lives desc, Product_Name asc,Formulary_Name asc,Drug_Name asc) ");
                        sb.Append("AS RowNumber, * ");
                        sb.AppendFormat("FROM ({0}) AS Results2) AS Results WHERE RowNumber > {1} ORDER BY Plan_name asc, Product_Name asc,Plan_Pharmacy_Lives desc, Formulary_Name asc,Drug_Name asc", comm.CommandText, rowNumber);

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
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn != null) conn.Dispose();
                if (comm != null) comm.Dispose();

            }
        }   
        /// <summary>
        /// for restriction Hx summary report; for coverage status report
        /// </summary>
        /// <param name="timeFrameVals"></param>
        /// <param name="geographyID"></param>
        /// <param name="tableName"></param>
        /// <param name="dataField"></param>
        /// <param name="keys"></param>
        /// <param name="queryVals"></param>
        /// <returns></returns>
        public IEnumerable<GenericDataRecord> GetCoverageStatusDataEx(IList<int> timeFrameVals, string geographyID, string tableName, string dataField, IList<string> keys, NameValueCollection queryVals)             
        {
              //Query for records
            SqlConnection conn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString);
            SqlCommand comm = new SqlCommand();
            comm.CommandTimeout = 180;
            string strClientDBName = string.Format("PF_{0}",Pinsonault.Web.Session.ClientKey);

            string strplanClassificationID = "2,3"; //consider regional plans          

            string[] ProductIDarr = queryVals["Product_ID"].Split(',');

            string trxMst;
            trxMst = queryVals["Trx_Mst"];
            if (string.IsNullOrEmpty(trxMst))
                trxMst = "Trx";

            dataField = "Data_Year_Month";
            if (queryVals["Monthly_Quarterly"] == "Q")
            {
                IList<int> timeFrameForQ = new List<int>();
                timeFrameForQ.Add(Convert.ToInt32(timeFrameVals[0]));
                timeFrameForQ.Add(Convert.ToInt32(timeFrameVals[1]));

                timeFrameVals.Clear();

                IList<string> monthForQuarter = new List<string>();

                monthForQuarter = GetMaxQuarterMonthAllPlans(queryVals, timeFrameForQ[0]);
                timeFrameVals.Add(Convert.ToInt32(monthForQuarter.FirstOrDefault().ToString()));

                monthForQuarter.Clear();

                monthForQuarter = GetMaxQuarterMonthAllPlans(queryVals, timeFrameForQ[1]);
                timeFrameVals.Add(Convert.ToInt32(monthForQuarter.FirstOrDefault().ToString()));
            }

            comm.Connection = conn;

            //IList<string> keysPercent = null;
            //keysPercent = keys.ToList();
            //keysPercent.Remove("Tier_No");
            //List<string> drugs = new List<string>();
            try
            {                
                StringBuilder sb = new StringBuilder();
                StringBuilder sbProdQuery = new StringBuilder();              
               
                int c = 0;
                
                timeFrame = timeFrameVals.ToList();

                int SegmentID = 1;
                if (queryVals["Section_ID"] == "17")
                    SegmentID = 2;
                else if (queryVals["Section_ID"] == "9")
                    SegmentID = 3;
                else if (queryVals["Section_ID"] == "6")
                    SegmentID = 6;
                else if (queryVals["Section_ID"] == "4")
                    SegmentID = 4;
                //....

                sb.AppendFormat(" With warnerCovTable AS ( " );

                sb.AppendFormat(" select Drug_ID, Drug_Name, Product_ID,Product_Name, Coverage_Status_ID,{0}, Formulary_Lives",dataField);
                if (geographyID.ToLower() != "us")
                    sb.AppendFormat(" ,Geography_ID ");
                sb.AppendFormat(" from {0}.{1}.{2} ", strClientDBName, "fhr", tableName);
                sb.AppendFormat(" where {0} in ({1},{2}) ", dataField, timeFrame[0], timeFrame[1]);
                sb.AppendFormat(" and Product_ID in ({0}) ", queryVals["Product_ID"]);

                if (queryVals["Section_ID"] == "-1")
                    sb.AppendFormat(" and Segment_ID in ({0}) ", "1,2,6"); //combined section
                else
                    sb.AppendFormat(" and Segment_ID = ({0}) ", SegmentID);

                if (geographyID.ToLower() != "us")
                    sb.AppendFormat(" and Geography_ID = '{0}' ", queryVals["Geography_ID"]);
                sb.AppendFormat(" and Plan_Classification_ID in ({0}) ", strplanClassificationID);

                sb.AppendFormat(" ) ");

                //sb.AppendFormat(" SELECT lkp.Tier_ID,lkp.Tier_Name, ");
                sb.AppendFormat(" SELECT Drug0.Coverage_Status_ID As Coverage_Status_ID, (Select Coverage_Status_Name from PF.dbo.Lkp_Coverage_Status where Coverage_Status_ID = Drug0.Coverage_Status_ID) AS Coverage_Status_Name, ");

               
                sb.AppendFormat(" Drug{0}.Product_ID As Drug{1}, Drug{0}.Formulary_Lives0 As Drug{1}_TotalCovered1, Drug{0}.Formulary_Lives1 As Drug{1}_TotalCovered2 ", c, c + 1);

                if (string.Compare(trxMst, "mst", true) == 0) //if % is required  
                {
                    sb.AppendFormat(" ,Drug{0}.Drug_Percent1 AS Drug{1}_Percent1 ", c, c + 1);
                    sb.AppendFormat(" ,Drug{0}.Drug_Percent2 AS Drug{1}_Percent2 ", c, c + 1);
                }
                sb.AppendFormat(" ,Drug{0}.Product_Name AS Drug{1}_Name ", c, c + 1);
                sb.AppendFormat(" ,Drug{0}.Coverage_Status_ID AS Drug{1}_CoverageStatusID ", c, c + 1);

                sbProdQuery.AppendFormat(" ( Select A.* ");
                if (string.Compare(trxMst, "mst", true) == 0) //if % is required  
                {
                    sbProdQuery.AppendFormat(", Case when B.Formulary_Lives0 > 0 Then (A.Formulary_Lives0*100.00/B.Formulary_Lives0) ELSE 0.00 END As Drug_Percent1 ");
                    sbProdQuery.AppendFormat(", Case when B.Formulary_Lives1 > 0 Then (A.Formulary_Lives1*100.00/B.Formulary_Lives1) ELSE 0.00 END As Drug_Percent2 ");
                }
                sbProdQuery.AppendFormat(" FROM ");

                sbProdQuery.AppendFormat(" (Select Drug_ID,Drug_Name,Product_ID,Product_Name,Coverage_Status_ID,ISNULL([{0}], 0) as Formulary_Lives0,ISNULL([{1}], 0) as Formulary_Lives1", timeFrame[0], timeFrame[1]);
                if (geographyID.ToLower() != "us")
                    sbProdQuery.AppendFormat(" ,Geography_ID ");
                sbProdQuery.AppendFormat(" from warnerCovTable AS SourceTable ");
                sbProdQuery.AppendFormat(" pivot(SUM(Formulary_Lives) for {0} in ([{1}], [{2}]) ) as pivotTable0 ", dataField, timeFrame[0], timeFrame[1]);
                sbProdQuery.AppendFormat(" where Product_ID in ({0}) ) A ", queryVals["Product_ID"]);
                //insert % query here
                if (string.Compare(trxMst, "mst", true) == 0) //if % is required  
                {
                    sbProdQuery.AppendFormat(" Left join ");
                    sbProdQuery.AppendFormat(" (Select Drug_ID,Product_ID,Product_Name,ISNULL([{0}], 0) as Formulary_Lives0 ,ISNULL([{1}], 0) as Formulary_Lives1", timeFrame[0], timeFrame[1]);
                    if (geographyID.ToLower() != "us")
                        sbProdQuery.AppendFormat(" ,Geography_ID ");
                    sbProdQuery.AppendFormat(" from ");
                    sbProdQuery.AppendFormat(" (Select Drug_ID,Product_ID,Product_Name, {0} ,Formulary_Lives ", dataField);
                    if (geographyID.ToLower() != "us")
                        sbProdQuery.AppendFormat(" ,Geography_ID ");
                    sbProdQuery.AppendFormat(" from warnerCovTable where Product_ID in ({0}) ) AS SourceTable pivot(SUM(Formulary_Lives) ", queryVals["Product_ID"]);
                    sbProdQuery.AppendFormat(" for {0} in ([{1}], [{2}]) ", dataField, timeFrame[0], timeFrame[1]);
                    sbProdQuery.AppendFormat(" ) as pivotTable0)B ON A.Product_ID = B.Product_ID and A.Drug_ID = B.Drug_ID ");
                    if (geographyID.ToLower() != "us")
                        sbProdQuery.AppendFormat(" and A.Geography_ID = B.Geography_ID ");
                }

                sbProdQuery.AppendFormat(" )Drug{0}", c);

                sb.AppendFormat(" from {0} where Drug0.Coverage_Status_ID is not null order by Drug0.Product_Name , Coverage_Status_Name ", sbProdQuery);

                comm.CommandText = sb.ToString();

                conn.Open();
                DbDataReader rdr = comm.ExecuteReader();
                IEnumerable<GenericDataRecord> g = Pinsonault.Data.GenericDataRecord.CreateCollection(rdr);
                conn.Close();

                return g;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn != null) conn.Dispose();
                if (comm != null) comm.Dispose();

            }

        }
        #endregion

        /// <summary>
        /// for getting the comparison report data
        /// </summary>
        /// <param name="timeFrameVals"></param>
        /// <param name="geographyID"></param>
        /// <param name="tableName"></param>
        /// <param name="dataField"></param>
        /// <param name="keys"></param>
        /// <param name="queryVals"></param>
        /// <param name="display_optionsVals"></param>
        /// <returns></returns>
        public IEnumerable<GenericDataRecord> GetData(IList<int> timeFrameVals, string geographyID, string tableName, string dataField, IList<string> keys, NameValueCollection queryVals, IList<int> display_optionsVals)
        {
            //Query for records
            SqlConnection conn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString);
            SqlCommand comm = new SqlCommand();

            string drugid = queryVals["Drug_ID"];

            comm.Connection = conn;

            try
            {
                comm.CommandText = ConstructFHXPivotQuery(comm, geographyID, timeFrameVals, tableName, dataField, keys, drugid, queryVals, display_optionsVals).ConvertNullToEmptyString().IncludeComparison().ToString();
                string strAddColumns = string.Empty;
                List<string> strWhere = new List<string>();
                string strAddWhereClause = "";

                //now replace the "select pivotTable0.Plan_ID" with "select pivotTable0.Plan_ID,Is_Co_Pay_Changed "
                //get the appropriate columns for display_optionsVals
                List<string> strPivotCompareColumn = new List<string>();
                for (int x = 0; x < display_optionsVals.Count; x++)
                {
                    switch (display_optionsVals[x])
                    {
                        case (int)LkpDisplayOptions.Tier:
                            strWhere.Add("Is_Tier_Name_Changed = 1 ");
                            break;
                        case (int)LkpDisplayOptions.CoPay:
                            strPivotCompareColumn.Add("Is_Co_Pay_Changed");
                            strWhere.Add("Is_Co_Pay_Changed = 1 ");
                            break;
                        case (int)LkpDisplayOptions.F_Status:
                            strPivotCompareColumn.Add("Is_Formulary_Status_Name_Changed");
                            strWhere.Add("Is_Formulary_Status_Name_Changed = 1 ");
                            break;
                        case (int)LkpDisplayOptions.PA:
                            strPivotCompareColumn.Add("Is_PA_Changed");
                            strWhere.Add("Is_PA_Changed = 1 ");
                            break;
                        case (int)LkpDisplayOptions.QL:
                            strPivotCompareColumn.Add("Is_QL_Changed");
                            strWhere.Add("Is_QL_Changed = 1 ");
                            break;
                        case (int)LkpDisplayOptions.ST:
                            strPivotCompareColumn.Add("Is_ST_Changed");
                            strWhere.Add("Is_ST_Changed = 1 ");
                            break;
                        //todo: add all values for case 0 = all display options
                    }
                    if (strPivotCompareColumn.Count > 0)
                        strAddColumns = string.Join(",", strPivotCompareColumn.ToArray());


                    if (strWhere.Count > 0)
                        strAddWhereClause = string.Join(" or ", strWhere.ToArray());

                }
                if (!string.IsNullOrEmpty(strAddColumns))
                    comm.CommandText = comm.CommandText.Replace("select pivotTable0.Plan_ID", string.Format("{0},{1}", "select pivotTable0.Plan_ID", strAddColumns));
                //else
                //    comm.CommandText = comm.CommandText;

                if (!string.IsNullOrEmpty(queryVals["chk_ViewChangesOnly"]) && !string.IsNullOrEmpty(strAddWhereClause))
                    comm.CommandText = string.Format("Select * from ({0}) v  where {1} ", comm.CommandText, strAddWhereClause);

                //include Plan and Formulary lives from latest data always
                string extView = "Plan_Drug_Formulary";

                comm.CommandText = string.Format("Select Base.*,ext.Plan_Name,ext.Product_Name,ext.Plan_State_ID,ext.Plan_Pharmacy_Lives,ext.Formulary_Name,ext.Formulary_Lives,ext.Drug_Name from ({0}) Base Inner Join PF.[dbo].[{1}] ext ON base.Plan_ID = ext.Plan_ID and base.Drug_ID = ext.Drug_ID and Base.Segment_ID = ext.Segment_ID and Base.Pinso_Formulary_ID = ext.Pinso_Formulary_ID and Base.Plan_Product_ID = ext.Product_ID ", comm.CommandText, extView);

                //filter added for Is_Predominant = true in case the checkbox is checked for Predominant benefit design
                if (!string.IsNullOrEmpty(queryVals["Is_Predominant"]))
                    comm.CommandText = string.Format("{0}{1}", comm.CommandText, " where ext.Is_Predominant = 1 ");

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
                        sb.Append("ROW_NUMBER() OVER (ORDER BY Plan_name asc, Plan_Pharmacy_Lives desc, Product_Name asc,Formulary_Name asc,Drug_Name asc) ");
                        sb.Append("AS RowNumber, * ");
                        sb.AppendFormat("FROM ({0}) AS Results2) AS Results WHERE RowNumber > {1} ORDER BY Plan_name asc, Product_Name asc,Plan_Pharmacy_Lives desc, Formulary_Name asc,Drug_Name asc", comm.CommandText, rowNumber);

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
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn != null) conn.Dispose();
                if (comm != null) comm.Dispose();

            }
        }

        /// <summary>
        /// for getting the rolling report data
        /// </summary>
        /// <param name="timeFrameVals"></param>
        /// <param name="geographyID"></param>
        /// <param name="tableName"></param>
        /// <param name="dataField"></param>
        /// <param name="keys"></param>
        /// <param name="queryVals"></param>
        /// <param name="display_optionsVals"></param>
        /// <returns></returns>

        public IEnumerable<GenericDataRecord> GetRollingData(IList<int> timeFrameVals, string geographyID, string tableName, string dataField, IList<string> keys, NameValueCollection queryVals, IList<int> display_optionsVals)
        {
            //Query for records
            SqlConnection conn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString);
            SqlCommand comm = new SqlCommand();

            string drugid = queryVals["Drug_ID"];

            comm.Connection = conn;

            try
            {
                comm.CommandText = ConstructFHXRollingPivotQuery(comm, geographyID, timeFrameVals, tableName, dataField, keys, drugid, queryVals, display_optionsVals).ConvertNullToEmptyString().IncludeComparison().ToString();

                List<string> strWhere = new List<string>();
                string strAddWhereClause = "";

                //changes made to exclude the change in last timeframe as per the requirement
                // because that timeframe would describe the change with previous time frame which is not included in report
                for (int itimecount = 0; itimecount < timeFrameVals.Count - 1; itimecount++)
                {
                    strWhere.Add(string.Format("{0}{1} = 1 ", "Changed_Overall", itimecount));
                }
                if (strWhere.Count > 0)
                    strAddWhereClause = string.Join(" or ", strWhere.ToArray());

                if (!string.IsNullOrEmpty(queryVals["chk_ViewChangesOnly"]) && !string.IsNullOrEmpty(strAddWhereClause))
                    comm.CommandText = string.Format("Select * from ({0}) v  where {1} ", comm.CommandText, strAddWhereClause);


                //include Plan and Formulary lives from latest data always
                string extView = "Plan_Drug_Formulary";
               
                comm.CommandText = string.Format("Select Base.*,ext.Plan_Name,ext.Product_Name,ext.Plan_State_ID,ext.Plan_Pharmacy_Lives,ext.Formulary_Name,ext.Formulary_Lives,ext.Drug_Name from ({0}) Base Inner Join PF.[dbo].[{1}] ext ON base.Plan_ID = ext.Plan_ID and base.Drug_ID = ext.Drug_ID and Base.Segment_ID = ext.Segment_ID and Base.Pinso_Formulary_ID = ext.Pinso_Formulary_ID and Base.Plan_Product_ID = ext.Product_ID ", comm.CommandText, extView);

                //filter added for Is_Predominant = true in case the checkbox is checked for Predominant benefit design
                if (!string.IsNullOrEmpty(queryVals["Is_Predominant"]))
                    comm.CommandText = string.Format("{0}{1}", comm.CommandText, " where ext.Is_Predominant = 1 ");


                conn.Open();
                DbDataReader rdr = comm.ExecuteReader();
                IEnumerable<GenericDataRecord> g = Pinsonault.Data.GenericDataRecord.CreateCollection(rdr);
                conn.Close();

                return g;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn != null) conn.Dispose();
                if (comm != null) comm.Dispose();

            }
        }

        /// <summary>
        /// for creating comparison report pivot query
        /// </summary>
        /// <param name="comm"></param>
        /// <param name="geographyID"></param>
        /// <param name="timeFrameVals"></param>
        /// <param name="tableName"></param>
        /// <param name="dataField"></param>
        /// <param name="keys"></param>
        /// <param name="DrugID"></param>
        /// <param name="queryVals"></param>
        /// <param name="display_optionsVals"></param>
        /// <returns></returns>
        public SQLPivotQuery<int> ConstructFHXPivotQuery(SqlCommand comm, string geographyID, IList<int> timeFrameVals, string tableName, string dataField, IList<string> keys, string DrugID, NameValueCollection queryVals, IList<int> display_optionsVals)
        {
            Pinsonault.Data.SQLUtil.MaxFragmentLength = 35;

            SQLPivotQuery<int> query = null;

            timeFrame = timeFrameVals.ToList();
            //dataField = "timeFrame";
            dataField = "Data_Year_Month";
            query = SQLPivotQuery<int>.Create(tableName, "PF", "fhr", keys, dataField, timeFrame);


            //Filter the query for Section and Segment id for all the sections except for Med D and Managed Medicaid
            //For Med D, filter by only segemnt ID
            int SegmentID = 1;
            if (queryVals["Section_ID"] == "17")
                SegmentID = 2;
            else if (queryVals["Section_ID"] == "9")
                SegmentID = 3;
            else if (queryVals["Section_ID"] == "6")
                SegmentID = 6;
            else if (queryVals["Section_ID"] == "4")
                SegmentID = 4;

            query.Where("Segment_ID", "SegmentID", SQLOperator.EqualTo);
            comm.Parameters.Add("@SegmentID", SqlDbType.Int).Value = SegmentID;

            if (queryVals["Section_ID"] != "17" && queryVals["Section_ID"] != "6")
            {
                query.Where("Section_ID", "SectionID", SQLOperator.EqualTo);
                comm.Parameters.Add("@SectionID", SqlDbType.Int).Value = Convert.ToInt32(queryVals["Section_ID"]);
            }

            //filter Plan_ID if it is present in the query
            if (!string.IsNullOrEmpty(queryVals["Plan_ID"]))
            {
                string[] planIDarr = queryVals["Plan_ID"].Split(',');

                if (planIDarr.Count() > 0)
                {
                    for (int x = 0; x < planIDarr.Count(); x++)
                        comm.Parameters.Add(string.Format("@PlanID{0}", x), SqlDbType.Int).Value = Convert.ToInt32(planIDarr[x]);

                    query.Where("Plan_ID", "PlanID", SQLOperator.In, planIDarr.Count());
                }
            }

            //filter Geography_ID
            string[] geographyIDarr = queryVals["Geography_ID"].Split(',');
            if (geographyIDarr.Count() > 0)
            {
                for (int x = 0; x < geographyIDarr.Count(); x++)
                    comm.Parameters.Add(string.Format("@Geography_ID{0}", x), SqlDbType.NVarChar).Value = geographyIDarr[x];

                query.Where("Geography_ID", "Geography_ID", SQLOperator.In, geographyIDarr.Count());
            }

            //filter Drug_ID
            string[] drugIDarr = queryVals["Drug_ID"].Split(',');

            if (drugIDarr.Count() > 0)
            {
                for (int x = 0; x < drugIDarr.Count(); x++)
                    comm.Parameters.Add(string.Format("@drugID{0}", x), SqlDbType.Int).Value = Convert.ToInt32(drugIDarr[x]);

                query.Where("Drug_ID", "drugID", SQLOperator.In, drugIDarr.Count());
            }

            query.Pivot(SQLFunction.MAX, "Tier_Name");
            for (int x = 0; x < display_optionsVals.Count; x++)
            {
                switch (display_optionsVals[x])
                {
                    //case (int)LkpDisplayOptions.Tier:
                    //strPivotCompareColumn.Add("Is_Tier_Name_Changed");
                    //break;
                    case (int)LkpDisplayOptions.CoPay:
                        query.Pivot(SQLFunction.MAX, "Co_Pay");
                        break;
                    case (int)LkpDisplayOptions.F_Status:
                        query.Pivot(SQLFunction.MAX, "Formulary_Status_Name");
                        break;
                    case (int)LkpDisplayOptions.PA:
                        query.Pivot(SQLFunction.MAX, "PA");
                        break;
                    case (int)LkpDisplayOptions.QL:
                        query.Pivot(SQLFunction.MAX, "QL");
                        break;
                    case (int)LkpDisplayOptions.ST:
                        query.Pivot(SQLFunction.MAX, "ST");
                        break;
                }
            }


            return query;
        }
        
        /// <summary>
        /// for creating rolling report pivot query
        /// </summary>
        /// <param name="comm"></param>
        /// <param name="geographyID"></param>
        /// <param name="timeFrameVals"></param>
        /// <param name="tableName"></param>
        /// <param name="dataField"></param>
        /// <param name="keys"></param>
        /// <param name="DrugID"></param>
        /// <param name="queryVals"></param>
        /// <param name="display_optionsVals"></param>
        /// <returns></returns>
        public SQLPivotQuery<int> ConstructFHXRollingPivotQuery(SqlCommand comm, string geographyID, IList<int> timeFrameVals, string tableName, string dataField, IList<string> keys, string DrugID, NameValueCollection queryVals, IList<int> display_optionsVals)
        {
            Pinsonault.Data.SQLUtil.MaxFragmentLength = 35;

            SQLPivotQuery<int> query = null;

            timeFrame = timeFrameVals.ToList();
            //dataField = "timeFrame";
            dataField = "Data_Year_Month";
            query = SQLPivotQuery<int>.Create(tableName, "PF", "fhr", keys, dataField, timeFrame);


            //filter the query for Secion and Segment id for all the sections except for med D. For Med D, filter by only segemnt ID
            int SegmentID = 1;
            if (queryVals["Section_ID"] == "17")
                SegmentID = 2;
            else if (queryVals["Section_ID"] == "9")
                SegmentID = 3;
            else if (queryVals["Section_ID"] == "6")
                SegmentID = 6;
            else if (queryVals["Section_ID"] == "4")
                SegmentID = 4;

            query.Where("Segment_ID", "SegmentID", SQLOperator.EqualTo);
            comm.Parameters.Add("@SegmentID", SqlDbType.Int).Value = SegmentID;

            if (queryVals["Section_ID"] != "17" && queryVals["Section_ID"] != "6")
            {
                query.Where("Section_ID", "SectionID", SQLOperator.EqualTo);
                comm.Parameters.Add("@SectionID", SqlDbType.Int).Value = Convert.ToInt32(queryVals["Section_ID"]);
            }

            //filter Plan_ID if it is present in the query
            if (!string.IsNullOrEmpty(queryVals["Plan_ID"]))
            {
                string[] planIDarr = queryVals["Plan_ID"].Split(',');

                if (planIDarr.Count() > 0)
                {
                    for (int x = 0; x < planIDarr.Count(); x++)
                        comm.Parameters.Add(string.Format("@PlanID{0}", x), SqlDbType.Int).Value = Convert.ToInt32(planIDarr[x]);

                    query.Where("Plan_ID", "PlanID", SQLOperator.In, planIDarr.Count());
                }
            }

            //filter Geography_ID
            string[] geographyIDarr = queryVals["Geography_ID"].Split(',');
            if (geographyIDarr.Count() > 0)
            {
                for (int x = 0; x < geographyIDarr.Count(); x++)
                    comm.Parameters.Add(string.Format("@Geography_ID{0}", x), SqlDbType.NVarChar).Value = geographyIDarr[x];

                query.Where("Geography_ID", "Geography_ID", SQLOperator.In, geographyIDarr.Count());
            }

            //filter Drug_ID
            string[] drugIDarr = queryVals["Drug_ID"].Split(',');

            if (drugIDarr.Count() > 0)
            {
                for (int x = 0; x < drugIDarr.Count(); x++)
                    comm.Parameters.Add(string.Format("@drugID{0}", x), SqlDbType.Int).Value = Convert.ToInt32(drugIDarr[x]);

                query.Where("Drug_ID", "drugID", SQLOperator.In, drugIDarr.Count());
            }

            query.Pivot(SQLFunction.MAX, "Changed_Overall");
            for (int x = 0; x < display_optionsVals.Count; x++)
            {
                switch (display_optionsVals[x])
                {
                    case (int)LkpDisplayOptions.Tier:
                        query.Pivot(SQLFunction.MAX, "Tier_Name");
                        query.Pivot(SQLFunction.MAX, "Changed_Tier");
                        break;
                    case (int)LkpDisplayOptions.CoPay:
                        query.Pivot(SQLFunction.MAX, "Co_Pay");
                        query.Pivot(SQLFunction.MAX, "Changed_CoPay");
                        break;
                    case (int)LkpDisplayOptions.F_Status:
                        query.Pivot(SQLFunction.MAX, "Formulary_Status_Name");
                        query.Pivot(SQLFunction.MAX, "Changed_FormularyStatus");
                        break;
                    case (int)LkpDisplayOptions.PA:
                        query.Pivot(SQLFunction.MAX, "PA");
                        query.Pivot(SQLFunction.MAX, "Changed_PA");
                        break;
                    case (int)LkpDisplayOptions.QL:
                        query.Pivot(SQLFunction.MAX, "QL");
                        query.Pivot(SQLFunction.MAX, "Changed_QL");
                        break;
                    case (int)LkpDisplayOptions.ST:
                        query.Pivot(SQLFunction.MAX, "ST");
                        query.Pivot(SQLFunction.MAX, "Changed_ST");
                        break;
                }
            }


            return query;
        }

        /// <summary>
        /// for getting time frame vals for comparison report
        /// </summary>
        /// <param name="queryValues"></param>
        /// <param name="iSectionID"></param>
        /// <returns></returns>
        public IList<int> GetTimeFrameVals(NameValueCollection queryValues, int iSectionID, bool original)
        {
            IList<int> timeFrameVals = new List<int>();
            string timeFrame;
            string timeFrame1;
            string timeFrame2;

            timeFrame = queryValues["TimeFrame"];

            timeFrame1 = queryValues["TimeFrame1"];//change it to get Time Frame 1 
            timeFrame2 = queryValues["TimeFrame2"];

            if (timeFrame1 != timeFrame2)
            {
                //Return original unprocessed timeframe
                if (original)
                {
                    timeFrameVals.Add(Convert.ToInt32(timeFrame1));
                    timeFrameVals.Add(Convert.ToInt32(timeFrame2));
                }
                else
                {
                    //If timeframe is quarterly, get equivalent Max Month
                    if (queryValues["Monthly_Quarterly"] == "Q")
                    {
                        NameValueCollection nvc = queryValues;
                        IList<string> monthForQuarter = new List<string>();
                        nvc["Timeframe"] = timeFrame1;

                        monthForQuarter = GetMaxQuarterMonth(nvc);
                        timeFrameVals.Add(Convert.ToInt32(monthForQuarter.FirstOrDefault().ToString()));

                        nvc["Timeframe"] = timeFrame2;

                        monthForQuarter.Clear();
                        monthForQuarter = GetMaxQuarterMonth(nvc);
                        timeFrameVals.Add(Convert.ToInt32(monthForQuarter.FirstOrDefault().ToString()));
                    }
                    else
                    {
                        timeFrameVals.Add(Convert.ToInt32(timeFrame1));
                        timeFrameVals.Add(Convert.ToInt32(timeFrame2));
                    }
                }
            }
            
            return timeFrameVals;
        }

        /// <summary>
        /// for getting rolling time frame vals
        /// </summary>
        /// <param name="queryValues"></param>
        /// <param name="iSectionID"></param>
        /// <returns></returns>
        public IList<int> GetRollingTimeFrameVals(NameValueCollection queryValues, int iSectionID, bool original)
        {
            IList<int> timeFrameVals = new List<int>();

            string current = "c";
            string previous_month = "p_m";
            string previous_quarter = "p_q";
            int segmentID = 1;
            if (queryValues["Section_ID"] == "17")
                segmentID = 2;
            //else if (queryValues["Section_ID"] == "9")
            //    segmentID = 3;
            //else if (queryValues["Section_ID"] == "6")
            //    segmentID = 6;
            else if (queryValues["Section_ID"] == "4")
                segmentID = 4;

            int currentDataQtr = 0;
            int currentDataYear = 0;
            int currentDataMonth = 0;

            int previousDataQtr = 0;
            int previousDataYearMonth = 0;
            int previousDataYearQuarter = 0;
            int previousDataMonth = 0;

            using (PathfinderModel.PathfinderEntities context = new PathfinderModel.PathfinderEntities())
            {
                //current time frame
                var c = (from d in context.LkpFormularyDataPeriodSet
                         where d.Current_Previous == current &&
                         d.Segment_ID == segmentID
                         select d).FirstOrDefault();
                currentDataQtr = c.Data_Quarter;
                currentDataYear = c.Data_Year;
                currentDataMonth = c.Data_Month;

                //previous time frame month
                var pm = (from dp in context.LkpFormularyDataPeriodSet
                         where dp.Current_Previous == previous_month &&
                         dp.Segment_ID == segmentID
                         select dp).FirstOrDefault();

                //previousDataQtr = p.Data_Quarter;
                previousDataYearMonth = pm.Data_Year;
                previousDataMonth = pm.Data_Month;

                //previous time frame quarter
                var pq = (from dp in context.LkpFormularyDataPeriodSet
                         where dp.Current_Previous == previous_quarter &&
                         dp.Segment_ID == segmentID
                         select dp).FirstOrDefault();

                previousDataQtr = pq.Data_Quarter;
                previousDataYearQuarter = pq.Data_Year;

            }
            //TODO: change it later to get the month or quarter if it is less than 1 or 3
            if (queryValues["Monthly_Quarterly"] == "Q")
            {
                //Get original unprocessed Timeframe Values for column headers
                if (original)
                {
                    //timeFrameVals.Add(Convert.ToInt32(string.Format("{0}{1}", DateTime.Today.Date.Year, "01")));
                    timeFrameVals.Add(currentDataYear * 100 + currentDataQtr);
                    timeFrameVals.Add(previousDataYearQuarter * 100 + previousDataQtr);

                    switch (queryValues["TimeFrameQtr"])
                    {
                        case "2":
                            //timeFrameVals.Add(Convert.ToInt32(string.Format("{0}{1}", previousDataYear, previousDataQtr)));
                            break;

                        case "3":
                            for (int iTimeCount = 2; iTimeCount < 3; iTimeCount++)
                            {
                                if (previousDataQtr > 1)
                                {
                                    previousDataQtr = previousDataQtr - 1;
                                }
                                else
                                {
                                    previousDataQtr = 4;
                                    previousDataYearQuarter = previousDataYearQuarter - 1;
                                }
                                timeFrameVals.Add(previousDataYearQuarter * 100 + previousDataQtr);
                            }
                            break;

                        case "4":
                            for (int iTimeCount = 2; iTimeCount < 4; iTimeCount++)
                            {
                                if (previousDataQtr > 1)
                                {
                                    previousDataQtr = previousDataQtr - 1;
                                }
                                else
                                {
                                    previousDataQtr = 4;
                                    previousDataYearQuarter = previousDataYearQuarter - 1;
                                }
                                timeFrameVals.Add(previousDataYearQuarter * 100 + previousDataQtr);
                            }
                            break;
                    }
                }
                else //Get Max Month timeframe values for use in query
                {
                    //Get the equivalent Max month for the Quarter
                    int currentDataQtrMonth = RollingQuarterToMaxMonth(currentDataQtr);
                    int previousDataQtrMonth = RollingQuarterToMaxMonth(previousDataQtr);


                    //timeFrameVals.Add(Convert.ToInt32(string.Format("{0}{1}", DateTime.Today.Date.Year, "01")));
                    timeFrameVals.Add(currentDataYear * 100 + currentDataQtrMonth);
                    timeFrameVals.Add(previousDataYearQuarter * 100 + previousDataQtrMonth);

                    switch (queryValues["TimeFrameQtr"])
                    {
                        case "2":
                            //timeFrameVals.Add(Convert.ToInt32(string.Format("{0}{1}", previousDataYear, previousDataQtr)));
                            break;

                        case "3":
                            for (int iTimeCount = 2; iTimeCount < 3; iTimeCount++)
                            {
                                if (previousDataQtr > 1)
                                {
                                    previousDataQtrMonth = previousDataQtrMonth - 3;
                                }
                                else
                                {
                                    previousDataQtrMonth = 12;
                                    previousDataYearQuarter = previousDataYearQuarter - 1;
                                }
                                timeFrameVals.Add(previousDataYearQuarter * 100 + previousDataQtrMonth);
                            }
                            break;

                        case "4":
                            for (int iTimeCount = 2; iTimeCount < 4; iTimeCount++)
                            {
                                if (previousDataQtr > 1)
                                {
                                    previousDataQtrMonth = previousDataQtrMonth - 3;
                                }
                                else
                                {
                                    previousDataQtrMonth = 12;
                                    previousDataYearQuarter = previousDataYearQuarter - 1;
                                }
                                timeFrameVals.Add(previousDataYearQuarter * 100 + previousDataQtrMonth);
                            }
                            break;
                    }
                }
            }
            else if (queryValues["Monthly_Quarterly"] == "M")
            {
                //timeFrameVals.Add(201012);
                timeFrameVals.Add((currentDataYear * 100 + currentDataMonth));
                timeFrameVals.Add((previousDataYearMonth * 100 + previousDataMonth));

                switch (queryValues["TimeFrameMonth"])
                {
                    case "3":
                        for (int iTimeCount = 2; iTimeCount < 3; iTimeCount++)
                        {
                            if (previousDataMonth > 1)
                            {
                                previousDataMonth = previousDataMonth - 1;
                            }
                            else
                            {
                                previousDataMonth = 12;
                                previousDataYearMonth = previousDataYearMonth - 1;
                            }
                            timeFrameVals.Add(previousDataYearMonth * 100 + previousDataMonth);
                        }
                        break;

                    case "6":
                        for (int iTimeCount = 2; iTimeCount < 6; iTimeCount++)
                        {
                            if (previousDataMonth > 1)
                            {
                                previousDataMonth = previousDataMonth - 1;
                            }
                            else
                            {
                                previousDataMonth = 12;
                                previousDataYearMonth = previousDataYearMonth - 1;
                            }
                            timeFrameVals.Add(previousDataYearMonth * 100 + previousDataMonth);
                        }

                        break;

                    case "9":
                        for (int iTimeCount = 2; iTimeCount < 9; iTimeCount++)
                        {
                            if (previousDataMonth > 1)
                            {
                                previousDataMonth = previousDataMonth - 1;
                            }
                            else
                            {
                                previousDataMonth = 12;
                                previousDataYearMonth = previousDataYearMonth - 1;
                            }
                            timeFrameVals.Add(previousDataYearMonth * 100 + previousDataMonth);
                        }
                        break;

                    case "12":
                        for (int iTimeCount = 2; iTimeCount < 12; iTimeCount++)
                        {
                            if (previousDataMonth > 1)
                            {
                                previousDataMonth = previousDataMonth - 1;
                            }
                            else
                            {
                                previousDataMonth = 12;
                                previousDataYearMonth = previousDataYearMonth - 1;
                            }
                            timeFrameVals.Add(previousDataYearMonth * 100 + previousDataMonth);
                        }
                        break;
                }
            }
            return timeFrameVals;
        }

        public IList<int> GetDisplayOptionList(NameValueCollection queryValues, int iSectionID)
        {
            IList<int> display_optionsVals = new List<int>();
            string display_options = string.Empty;

            if (!string.IsNullOrEmpty(queryValues["Display_ID"]))
            {
                display_options = queryValues["Display_ID"];

                //Add time frame values to a list for processing
                if (display_options.IndexOf(',') > -1)
                {
                    string[] display_optionsArr = display_options.Split(',');

                    foreach (string s in display_optionsArr)
                        display_optionsVals.Add(Convert.ToInt32(s));
                }
                else
                    display_optionsVals.Add(Convert.ToInt32(display_options));
            }
            //all diplay options are selected
            else
            {
                if (iSectionID != 9) iSectionID = 0;
                using (PathfinderEntities context = new PathfinderEntities())
                {
                    var display = from c in context.fhrGetSectionDisplayOptionsSet
                                  where c.Section_ID == iSectionID
                                  select c.ID;

                    display_optionsVals = display.ToArray();

                }
            }
            return display_optionsVals;
        }
        public string GetDBCompareColumnName(string propertyname)
        {
            string strDBCompare = "";
            if (propertyname.Contains("Tier_Name"))
                strDBCompare = string.Format("Changed_Tier{0}", propertyname.Replace("Tier_Name", ""));

            if (propertyname.Contains("Formulary_Status_Name"))
                strDBCompare = string.Format("Changed_FormularyStatus{0}", propertyname.Replace("Formulary_Status_Name", ""));

            if (propertyname.Contains("PA"))
                strDBCompare = string.Format("Changed_PA{0}", propertyname.Replace("PA", ""));

            if (propertyname.Contains("QL"))
                strDBCompare = string.Format("Changed_QL{0}", propertyname.Replace("QL", ""));

            if (propertyname.Contains("ST"))
                strDBCompare = string.Format("Changed_ST{0}", propertyname.Replace("ST", ""));

            if (propertyname.Contains("Co_Pay"))
                strDBCompare = string.Format("Changed_CoPay{0}", propertyname.Replace("Co_Pay", ""));

            return strDBCompare;
        }

        public string GetDBCompareColumnName_CompareReport(string propertyname)
        {
            string strDBCompare_ComparisonReport = "";
            if (propertyname.Contains("Tier_Name"))
                strDBCompare_ComparisonReport = string.Format("Is_{0}_Changed", "Tier_Name");

            if (propertyname.Contains("Formulary_Status_Name"))
                strDBCompare_ComparisonReport = string.Format("Is_{0}_Changed", "Formulary_Status_Name");

            if (propertyname.Contains("PA"))
                strDBCompare_ComparisonReport = string.Format("Is_{0}_Changed", "PA");

            if (propertyname.Contains("QL"))
                strDBCompare_ComparisonReport = string.Format("Is_{0}_Changed", "QL");

            if (propertyname.Contains("ST"))
                strDBCompare_ComparisonReport = string.Format("Is_{0}_Changed", "ST");

            if (propertyname.Contains("Co_Pay"))
                strDBCompare_ComparisonReport = string.Format("Is_{0}_Changed", "Co_Pay");

            return strDBCompare_ComparisonReport;
        }

        /// <summary>
        /// Converts a Rolling Quarter to Max Month
        /// </summary>
        /// <param name="quarter">The Quarter to Convert to Month</param>
        public int RollingQuarterToMaxMonth(int quarter)
        {
            switch (quarter)
            {
                case 1:
                    return 3;
                case 2:
                    return 6;
                case 3:
                    return 9;
                case 4:
                    return 12;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// declaring row counts as generic list
        /// </summary>
        public class RowCounts : System.Collections.Generic.List<int> { }

       /// <summary>
        /// Function used to group rows for grid
       /// </summary>
       /// <param name="gv"></param>
       /// <param name="y"></param>
       
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

        /// <summary>
        /// Returns the corresponding Max Month(s) per Quarter(s)
        /// </summary>
        /// <param name="queryVals">Current query string.</param>
        /// <param name="dataField">Timeframe column to select by.</param>
        /// <returns></returns>
        public IList<string> GetMaxQuarterMonth(NameValueCollection queryVals)
        {
            using (PathfinderEntities context = new PathfinderEntities())
            {
                IList<Int32> timeFrameQuarters = new List<Int32>();
                IList<string> timeFrameMonths = new List<string>();
                string timeFrame = queryVals["Timeframe"];

                //Add time frame values to a list for processing
                if (timeFrame.IndexOf(',') > -1)
                {
                    string[] timeFrameArr = timeFrame.Split(',');

                    foreach (string s in timeFrameArr)
                        timeFrameQuarters.Add(Convert.ToInt32(s));
                }
                else
                    timeFrameQuarters.Add(Convert.ToInt32(timeFrame));

                foreach (int quarter in timeFrameQuarters)
                {
                    int sectionID = Convert.ToInt32(queryVals["Section_ID"]);
                    string query = "SELECT VALUE O FROM fhrGetFHXDataGeographySet AS O WHERE O.Drug_ID IN {" + queryVals["Drug_ID"] + "} AND O.Data_Year_Quarter = " + quarter.ToString() + " AND O.Plan_ID IN {" + queryVals["Plan_ID"] + "}" ;

                    ObjectQuery<fhrGetFHXDataGeography> o = new ObjectQuery<fhrGetFHXDataGeography>(query, context);

                    //Only query on SectionID if not Med-D
                    if (sectionID != 17)
                        o = (ObjectQuery<fhrGetFHXDataGeography>)o
                            .Where(d => d.Section_ID == sectionID);

                    int segmentID = 1;

                    if (Convert.ToInt32(queryVals["Section_ID"]) == 17) //Med-D
                        segmentID = 2;
                    else if (Convert.ToInt32(queryVals["Section_ID"]) == 9) //State Medicaid
                        segmentID = 3;
                    //else if (Convert.ToInt32(queryVals["Section_ID"]) == 6) //Managed Medicaid
                    //    segmentID = 6;    
                    else if (Convert.ToInt32(queryVals["Section_ID"]) == 4) //PBM
                        segmentID = 4;

                    o = (ObjectQuery<fhrGetFHXDataGeography>)o
                            .Where(d => d.Segment_ID == segmentID);

                    //Get the respective Max Month for each Quarter
                    List<int?> months = new List<int?>();
                    months = o.Select(d => d.Data_Year_Month).ToList();
                    if (months.Count > 0)
                        timeFrameMonths.Add(months.Max().Value.ToString());
                }

                return timeFrameMonths;
            }
        }

        //Used to process Section and Segment values based on Segment ID for Market Place Data
        public void ProcessSectionSegment(ref SqlCommand comm, ref SQLPivotQuery<int> query, NameValueCollection queryVals)
        {
            if (queryVals["Section_ID"] == "-1") //Combined: Section - 1,17,6; Segment - 1,2,6
            {
                //Get Commercial, Medicare & Managed Medicaid (Section ID 1,17,6; Segment ID 1,2,6)              
                comm.Parameters.Add("@SectionID0", SqlDbType.Int).Value = 1;
                comm.Parameters.Add("@SectionID1", SqlDbType.Int).Value = 17;
                comm.Parameters.Add("@SectionID2", SqlDbType.Int).Value = 6;

                query.Where("Section_ID", "SectionID", SQLOperator.In, 3);

                comm.Parameters.Add("@SegmentID0", SqlDbType.Int).Value = 2;
                comm.Parameters.Add("@SegmentID1", SqlDbType.Int).Value = 1;
                comm.Parameters.Add("@SegmentID2", SqlDbType.Int).Value = 6;

                query.Where("Segment_ID", "SegmentID", SQLOperator.In, 3);

                segmentID = "1,2,6";
            }
            else if (Convert.ToInt32(queryVals["Section_ID"]) == 8) //Other - include Other as well as DOD, FEP and Medical Groups
            {
                comm.Parameters.Add("@SectionID0", SqlDbType.Int).Value = 8;//Other
                comm.Parameters.Add("@SectionID1", SqlDbType.Int).Value = 12;//DOD
                comm.Parameters.Add("@SectionID2", SqlDbType.Int).Value = 20;//FEP
                comm.Parameters.Add("@SectionID3", SqlDbType.Int).Value = 18;//Medical Groups

                query.Where("Section_ID", "SectionID", SQLOperator.In, 4);

                comm.Parameters.Add("@SegmentID", SqlDbType.Int).Value = 1;

                query.Where("Segment_ID", "SegmentID", SQLOperator.EqualTo);
            }
            else if (Convert.ToInt32(queryVals["Section_ID"]) == 4) //PBM: Section - 4; Segment - 1,4
            {
                comm.Parameters.Add("@SectionID", SqlDbType.Int).Value = 4;
                query.Where("Section_ID", "SectionID", SQLOperator.EqualTo);

                comm.Parameters.Add("@SegmentID0", SqlDbType.Int).Value = 1;
                comm.Parameters.Add("@SegmentID1", SqlDbType.Int).Value = 4;
                query.Where("Segment_ID", "SegmentID", SQLOperator.In, 2);

                segmentID = "1,4";
            }
            else if (Convert.ToInt32(queryVals["Section_ID"]) == 1) //Commercial: Section - 1,6; Segment - 1
            {
                if(!comm.Parameters.Contains("@SectionID0"))
                    comm.Parameters.Add("@SectionID0", SqlDbType.Int).Value = 1;
                if (!comm.Parameters.Contains("@SectionID1"))
                    comm.Parameters.Add("@SectionID1", SqlDbType.Int).Value = 6;
                query.Where("Section_ID", "SectionID", SQLOperator.In, 2);

                if (!comm.Parameters.Contains("@SegmentID"))
                    comm.Parameters.Add("@SegmentID", SqlDbType.Int).Value = 1;

                query.Where("Segment_ID", "SegmentID", SQLOperator.EqualTo);
            }
            else
            {
                if (Convert.ToInt32(queryVals["Section_ID"]) == 17) //Med-D: Section - not passed; Segment - 2
                {
                    comm.Parameters.Add("@SegmentID", SqlDbType.Int).Value = 2;
                    segmentID = "2";
                }
                else if (Convert.ToInt32(queryVals["Section_ID"]) == 6) //Managed Medicaid: Section - not passed; Segment - 6
                {
                    comm.Parameters.Add("@SegmentID", SqlDbType.Int).Value = 6;
                    segmentID = "6";
                }
                else //All other Sections
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
        /// <summary>
        /// for getting the translated name for excel export columns for tier and restriction Hx reports
        /// </summary>
        /// <param name="map"></param>
        /// <param name="newFilters"></param>
        /// <returns></returns>
        public string GetMapTranslatedNameForTier_RestrictionsReport(ColumnMap map, NameValueCollection newFilters)
        {            
            IList<int> timeFrameVals = new List<int>();
            string[] productIDarr = newFilters["Product_ID"].Split(',');
          
            timeFrameVals.Add(Convert.ToInt32(newFilters["TimeFrame1"]));
            timeFrameVals.Add(Convert.ToInt32(newFilters["TimeFrame2"]));

            if (map.PropertyName.LastIndexOfAny(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' }) > -1 && map.PropertyName != "Drug1_Name")           
            {
                int itimeframeindex = Convert.ToInt32(map.PropertyName.Substring(map.PropertyName.LastIndexOfAny(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' })));
                int idrugIndex = Convert.ToInt32(map.PropertyName.Substring(map.PropertyName.IndexOfAny((new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' })), 1));

                if (map.PropertyName.ToLower().Contains("totalcovered") || map.PropertyName.ToLower().Contains("percent"))
                {
                    itimeframeindex = itimeframeindex - 1;
                    idrugIndex = idrugIndex - 1;
                }               

                string strTimeframename = "";

                if (newFilters["Monthly_Quarterly"] == "M")
                    strTimeframename = string.Format("{0} {1}", CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(Convert.ToInt32(timeFrameVals[itimeframeindex].ToString().Substring(4))), timeFrameVals[itimeframeindex].ToString().Substring(0, 4));
                else
                    strTimeframename = string.Format("Q{0} {1}", timeFrameVals[itimeframeindex].ToString().Substring(5), timeFrameVals[itimeframeindex].ToString().Substring(0, 4));

                string ProductName = "";

                //trx
                if (map.PropertyName.ToLower().Contains("trx"))
                    map.TranslatedName = string.Format("Trx {0}", strTimeframename);
                //Mst
                else if (map.PropertyName.ToLower().Contains("mst"))
                    map.TranslatedName = string.Format("Mst {0}", strTimeframename);
                //tier
                else if (map.PropertyName.ToLower().Contains("tier_name"))
                    map.TranslatedName = string.Format("Tier ,{0}", strTimeframename);
                //copay
                else if (map.PropertyName.ToLower().Contains("co_pay"))
                    map.TranslatedName = string.Format("Co Pay ,{0}", strTimeframename);
                //Restrictions
                else if (map.PropertyName.ToLower().Contains("restrictions"))
                    map.TranslatedName = string.Format("Restrictions ,{0}", strTimeframename);
                //Coverage_Status_Name
                else if (map.PropertyName.ToLower().Contains("coverage_status_name"))
                    map.TranslatedName = string.Format("Coverage Status ,{0}", strTimeframename);
                //summary portion of tier and restriction report will have total covered ot percent columns
                else if(map.PropertyName.ToLower().Contains("totalcovered") || map.PropertyName.ToLower().Contains("percent"))                
                {

                    //int ProductID = Convert.ToInt32(productIDarr[idrugIndex]);

                    //using (PathfinderClientEntities clientcontext = new PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
                    //{
                    //    var p = (from dp in clientcontext.DrugProductsSet
                    //             where dp.Product_ID == ProductID
                    //             select dp.Product_Name).FirstOrDefault();
                    //    ProductName = p.ToString();
                    //}

                    map.TranslatedName = strTimeframename;// string.Format("{0},{1}", strTimeframename, ProductName);
                }
            }
            return map.TranslatedName;
        }

    }
   
}
