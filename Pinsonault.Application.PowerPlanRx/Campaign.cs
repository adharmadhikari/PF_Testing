using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Configuration;
using System.Data.SqlTypes;
using System.Data;

namespace Pinsonault.Application.PowerPlanRx
{
    public enum SegmentID
    {
        Commercial = 1,//MCO
        PartD = 2, //PTD
        Other = 3 // Other
    }
    //enum for campaign phase ID
    public enum PhaseID
    {
        Campaign_Created = 1,
        Profile = 2,
        Timeline = 3,
        Goal_Setup = 4,
        Team_Setup = 5,
        Tactics = 6,
        Messages = 7,
        Marketing_Execution = 8,
        Sales_Execution = 9,
        Feedback = 10,
        All_Steps_Completed = 11
    }
    /// <summary>
    /// This class contains functions to get the CampaignInfo and update the campaign info
    /// </summary>
    public partial class Campaign
    {
        /// <summary>
        /// for getting the campaign product and competitor product formulary data
        /// </summary>
        /// <param name="iCampaignID"></param>
        /// <returns></returns>
        public static DataTable GetCampaignProductFormulary(int iCampaignID)
        {
            SqlConnection cn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString);
            try
            {
                SqlParameter[] arParams = new SqlParameter[1];

                arParams[0] = new SqlParameter("@Campaign_ID", iCampaignID);
                return SqlHelper.ExecuteDataset(cn, "pprx.usp_GetCampaignProductFormulary", arParams).Tables[0];                
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// for updating the campaign product formulary info
        /// </summary>
        /// <param name="iCampaignID"></param>
        /// <param name="iProductID"></param>
        /// <param name="strProduct_Tier"></param>
        /// <param name="strProduct_Copay"></param>
        /// <param name="strProduct_PA_Comment"></param>
        /// <param name="strProduct_QL_Comment"></param>
        /// <param name="strProduct_ST_Comment"></param>
        public static void UpdateCampaignProductFormulary(int iCampaignID, int iProductID, string strProduct_Tier, string strProduct_Copay, string strProduct_PA_Comment, string strProduct_QL_Comment, string strProduct_ST_Comment, string strModifiedBy)
        {
            //update campaign product formulary
            SqlConnection cn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString);
            try
            {
                SqlParameter[] arParams = new SqlParameter[8];

                arParams[0] = new SqlParameter("@Campaign_ID", iCampaignID);
                arParams[1] = new SqlParameter("@Product_ID", iProductID);
                arParams[2] = new SqlParameter("@Product_Tier", strProduct_Tier);
                arParams[3] = new SqlParameter("@Product_Copay", strProduct_Copay);
                arParams[4] = new SqlParameter("@Product_PA_Comment", strProduct_PA_Comment);
                arParams[5] = new SqlParameter("@Product_QL_Comment", strProduct_QL_Comment);
                arParams[6] = new SqlParameter("@Product_ST_Comment", strProduct_ST_Comment);
                arParams[7] = new SqlParameter("@Modified_BY", strModifiedBy);

                SqlHelper.ExecuteNonQuery(cn, "pprx.usp_UpdateCampaignProductFormulary", arParams);

            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// for updating the campaign plan info
        /// </summary>
        /// <param name="iCampaignID"></param>
        /// <param name="bFormulary_Change_Status"></param>
        /// <param name="dtFormulary_Change_Eff_Date"></param>
        /// <param name="bPlan_Participation_PT"></param>
        /// <param name="strKey_Employers"></param>
        /// <param name="strAffiliated_Phys_Groups"></param>
        /// <param name="strPlan_Penetrate_Region"></param>
        /// <param name="strContract_Share_Goal"></param>
        /// <param name="strOther_Facts1"></param>
        /// <param name="strOther_Facts2"></param>
        /// <param name="strOther_Facts3"></param>
        /// <param name="strOther_Facts4"></param>
        public static void UpdateCampaignPlanInfo(int iCampaignID, bool bFormulary_Change_Status,
                                                  System.Data.SqlTypes.SqlDateTime dtFormulary_Change_Eff_Date, bool bPlan_Participation_PT, string strKey_Employers,
                                                  string strAffiliated_Phys_Groups, 
                                                  string strPlan_Penetrate_Region, string strContract_Share_Goal, string strOther_Facts1,
                                                  string strOther_Facts2,string strOther_Facts3, string strOther_Facts4, int strModifiedBy)
        {            
            //update campaign product formulary
            SqlConnection cn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString);
            try
            {
                SqlParameter[] arParams = new SqlParameter[13];

                arParams[0] = new SqlParameter("@Campaign_ID", iCampaignID);
                arParams[1] = new SqlParameter("@Formulary_Change_Status", bFormulary_Change_Status);
                arParams[2] = new SqlParameter("@Formulary_Change_Eff_Date", dtFormulary_Change_Eff_Date);
                arParams[3] = new SqlParameter("@Plan_Participation_PT", bPlan_Participation_PT);
                arParams[4] = new SqlParameter("@Key_Employers", strKey_Employers);
                arParams[5] = new SqlParameter("@Affiliated_Phys_Groups", strAffiliated_Phys_Groups);
                arParams[6] = new SqlParameter("@Plan_Penetrate_Region", strPlan_Penetrate_Region);
                arParams[7] = new SqlParameter("@Contract_Share_Goal", strContract_Share_Goal);
                arParams[8] = new SqlParameter("@Other_Facts1", strOther_Facts1);
                arParams[9] = new SqlParameter("@Other_Facts2", strOther_Facts2);
                arParams[10] = new SqlParameter("@Other_Facts3", strOther_Facts3);
                arParams[11] = new SqlParameter("@Other_Facts4", strOther_Facts4);
                arParams[12] = new SqlParameter("@Modified_BY", strModifiedBy);                

                SqlHelper.ExecuteNonQuery(cn, "pprx.usp_Campaign_UpdatePlanProfile", arParams);

            }

            catch
            {
                throw;
            }
        }

        /// <summary>
        /// This function is used for adding a Campaign Team member
        /// </summary>
        /// <param name="intCampaignID"></param>
        /// <param name="TerritoryID"></param>
        /// <param name="strUserID"></param>
        public static void UpdateCampaignTeam(int campaignID, string territoryID, int userID)
        {
            //update campaign product formulary
            SqlConnection cn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString);
            try
            {
                SqlParameter[] arParams = new SqlParameter[3];

                arParams[0] = new SqlParameter("@Campaign_ID", campaignID);
                arParams[1] = new SqlParameter("@Territory_ID", territoryID);
                arParams[2] = new SqlParameter("@User_ID", userID);

                SqlHelper.ExecuteNonQuery(cn, "pprx.usp_UpdateCampaignTeam", arParams);

            }

            catch
            {
                throw;
            }
        }

        /// <summary>
        /// This function is used for adding an Ad Hoc Campaign Team member
        /// </summary>
        /// <param name="intCampaignID"></param>
        /// <param name="strName"></param>
        /// <param name="intTitleID"></param>
        /// <param name="strEmail"></param>
        /// <param name="strPhone"></param>
        /// <param name="strUserID"></param>
        public static void UpdateAdHocCampaignTeam(int campaignID, string adHocName, int titleID, string adHocEmail, string adHocPhone, int userID)
        {
            //update campaign product formulary
            SqlConnection cn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString);
            try
            {
                SqlParameter[] arParams = new SqlParameter[6];

                arParams[0] = new SqlParameter("@Campaign_ID", campaignID);
                arParams[1] = new SqlParameter("@User_Name", adHocName);
                arParams[2] = new SqlParameter("@Title_ID", titleID);
                arParams[3] = new SqlParameter("@Email", adHocEmail);
                arParams[4] = new SqlParameter("@Phone", adHocPhone);
                arParams[5] = new SqlParameter("@User_ID", userID);

                SqlHelper.ExecuteNonQuery(cn, "pprx.usp_UpdateAdHocCampaignTeam", arParams);

            }

            catch
            {
                throw;
            }
        }
               
        /// <summary>
        /// This function is used for creating the campaign. It returns the  created Campaign ID from database
        /// </summary>
        /// <param name="strPlanID"></param>
        /// <param name="iBrandID"></param>
        /// <param name="strTerritoryID"></param>
        /// <param name="strUserID"></param>
        /// <param name="strRationals"></param>
        /// <returns></returns>
        public static int CreateCampaign(string strPlanID, int iBrandID, string strTerritoryID, string strUserID, int SegmentID)
        {
            SqlConnection cn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString);
            int iCampaign_ID = 0;
            try
            {
                SqlParameter[] arParams = new SqlParameter[5];

                arParams[0] = new SqlParameter("@Plan_ID", strPlanID);
                arParams[1] = new SqlParameter("@Brand_ID", iBrandID);
                arParams[2] = new SqlParameter("@Territory_ID", strTerritoryID);
                arParams[3] = new SqlParameter("@User_ID", strUserID);
                //arParams[4] = new SqlParameter("@Campaign_Rationale", strRationals);
                arParams[4] = new SqlParameter("@Segment_ID", SegmentID);

                iCampaign_ID = System.Convert.ToInt32(SqlHelper.ExecuteScalar(cn, "pprx.usp_InsertCampaignInfo", arParams));
                return iCampaign_ID;
            }
            catch (Exception ex)
            {
                throw ex;
            }       
        }

        public static int DeleteCampaignRationalsByCampaignID(int Campaign_ID)
        {
            SqlConnection cn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString);
           
            try
            {
                SqlParameter[] arParams = new SqlParameter[1];

                arParams[0] = new SqlParameter("@Campaign_ID", Campaign_ID);
             
               return SqlHelper.ExecuteNonQuery(cn, "pprx.usp_DeleteCampaignRationaleByCampaignID", arParams);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// For updating the Campaign rationale
        /// </summary>
        /// <param name="iCampaignID"></param>
        /// <param name="strRationals"></param>
        /// <param name="strUserID"></param>
        /// <returns></returns>
        public static int InsertCampaignRationale(int iCampaignID, int iRationale)
        {
            SqlConnection cn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString);
            
            try
            {
                SqlParameter[] arParams = new SqlParameter[2];

                arParams[0] = new SqlParameter("@Campaign_ID", iCampaignID);
                arParams[1] = new SqlParameter("@Rationale_ID", iRationale);               

                return SqlHelper.ExecuteNonQuery(cn, "pprx.usp_InsertCampaignRationale", arParams);
            }
            catch
            {
                throw;
            }           

        }
        /// <summary>
        /// to update modified by and modified date in Campaign mast table
        /// </summary>
        /// <param name="Campaign_ID"></param>
        /// <param name="ModifiedBy"></param>
        /// <returns></returns>
        public static int UpdateCampaignModifiedDtByCampaign(int Campaign_ID, string ModifiedBy)
        {
            SqlConnection cn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString);

            try
            {
                SqlParameter[] arParams = new SqlParameter[2];

                arParams[0] = new SqlParameter("@Campaign_ID", Campaign_ID);
                arParams[1] = new SqlParameter("@Modified_By", ModifiedBy);

                return SqlHelper.ExecuteNonQuery(cn, "pprx.usp_UpdateCampaignDateByCampaignID", arParams);
            }
            catch
            {
                throw;
            }
        }
        
        /// <summary>
        /// For determining wheather the Campaign is Active or not.
        /// </summary>
        /// <param name="iCampaignID"></param>
        /// <returns></returns>
        public static bool IsCampaignActive(int iCampaignID)
        {
            bool bActive = false;
            SqlConnection cn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString);

            SqlParameter[] arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@Campaign_ID", iCampaignID);
           
            bActive = System.Convert.ToBoolean(SqlHelper.ExecuteScalar(cn, "pprx.usp_Campaign_IsActive", arParams));
            return bActive; 
        }
        public static int GetExistingActiveCampaignID(string strPlan_ID, int iBrandID, string strTerritoryID)
        {
            int iCampaignID = 0;
            SqlConnection cn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString);

            SqlParameter[] arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@Plan_ID", strPlan_ID);
            arParams[1] = new SqlParameter("@Brand_ID", iBrandID);
            arParams[2] = new SqlParameter("@Territory_ID", strTerritoryID);

            iCampaignID = Convert.ToInt32(SqlHelper.ExecuteScalar(cn, "pprx.usp_GetCampaignID", arParams));
            return iCampaignID;
        }
      
        /// <summary>
        /// This function returns the DataSet having details of Physicians List in a selected District ID, Plan Id and Brand ID
        /// Campaign ID is passed in the stored proc to get the plan and brand ID
        /// </summary>
        /// <param name="strDist_ID"></param>
        /// <param name="iCampaignID"></param>
        /// <returns></returns>
        public static DataSet GetPhysList(string strDist_ID, int iCampaignID)
        {
            SqlConnection cn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString);
                        
            SqlParameter[] arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@District_ID", strDist_ID);
            arParams[1] = new SqlParameter("@Campaign_ID", iCampaignID);
          
            return SqlHelper.ExecuteDataset(cn, "pprx.usp_GetPhysicianListByDistrictID", arParams);
            
        }
        /// <summary>
        /// For getting the details from SV_Base table based on District_ID,Brand_ID and Segment_ID
        /// </summary>
        /// <param name="strDistrict_ID">District ID</param>
        /// <param name="iBrandID">Brand ID</param>
        /// <param name="iSegment_ID">Segment ID</param>
        /// <returns></returns>
        public static DataTable GetSVBaseByDistrictSegmentBrandID(string strDistrict_ID, int iBrandID, int iSegment_ID)
        {
            SqlConnection cn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString);

            SqlParameter[] arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@District_ID", strDistrict_ID);
            arParams[1] = new SqlParameter("@Brand_ID", iBrandID);
            arParams[2] = new SqlParameter("@Segment_ID", iSegment_ID);
           
            return SqlHelper.ExecuteDataset(cn, "pprx.usp_GetSV_BaseByDistrictIDBrandID", arParams).Tables[0];            
        }
        /// <summary>
        /// For getting the details from SV_Base table based on Region_ID, District_ID,Brand_ID and Segment_ID
        /// </summary>
        /// <param name="strRegion_ID">Region ID</param>
        /// <param name="strDistrict_ID">District ID</param>
        /// <param name="iBrandID">Brand ID</param>
        /// <param name="iSegment_ID">Segment ID</param>
        /// <returns></returns>
        public static DataTable GetSVBaseByRegionDistrictSegmentBrandID(string strType_ID, string strRegion_ID, string strDistrict_ID, int iBrandID, int iSegment_ID)
        {
            SqlConnection cn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString);

            SqlParameter[] arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@Type_ID", strType_ID);
            arParams[1] = new SqlParameter("@Region_ID", strRegion_ID);
            arParams[2] = new SqlParameter("@District_ID", strDistrict_ID);
            arParams[3] = new SqlParameter("@Brand_ID", iBrandID);
            arParams[4] = new SqlParameter("@Segment_ID", iSegment_ID);

            return SqlHelper.ExecuteDataset(cn, "pprx.usp_GetSV_BaseByRegionIDDistrictIDBrandID", arParams).Tables[0];
        }

        /// <summary>
        /// This function returns the DataSet for the District Region Excel Report
        /// </summary>
        /// <param name="strType_ID"></param>
        /// <param name="strRegion_ID"></param>
        /// <param name="strDistrict_ID"></param>
        /// <param name="iBrandID"></param>
        /// <param name="iSegment_ID"></param>
        /// <returns></returns>
        public static DataSet dsSVBaseByRegionDistrictSegmentBrandID(string strType_ID, string strRegion_ID, string strDistrict_ID, int iBrandID, int iSegment_ID)        
        {
            SqlConnection cn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString);

            SqlParameter[] arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@Type_ID", strType_ID);
            arParams[1] = new SqlParameter("@Region_ID", strRegion_ID);
            arParams[2] = new SqlParameter("@District_ID", strDistrict_ID);
            arParams[3] = new SqlParameter("@Brand_ID", iBrandID);
            arParams[4] = new SqlParameter("@Segment_ID", iSegment_ID);

            return SqlHelper.ExecuteDataset(cn, "pprx.usp_GetSV_BaseByRegionIDDistrictIDBrandID", arParams);
        }

        /// <summary>
        /// This function updates the campaign phase id only if required phase id > current phase id of campaign
        /// </summary>
        /// <param name="iCampaignID">CampaignID</param>
        /// <param name="iRequiredPhaseID">Required Phase ID</param>
        /// <returns>integer</returns>
        public static int UpdatePhaseIDByCampaignIDReqdPhaseID(int iCampaignID, int iRequiredPhaseID)
        {
            SqlConnection cn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString);

            SqlParameter[] arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@Campaign_ID", iCampaignID);
            arParams[1] = new SqlParameter("@RequiredPhaseID", iRequiredPhaseID);

            return SqlHelper.ExecuteNonQuery(cn, "pprx.usp_UpdateCampaignPhaseByCampaignID", arParams);
        }
        /// <summary>
        /// For getting the Details Impact report having plan and district ids associated with campaign
        /// </summary>
        /// <param name="strQuery"></param>
        /// <returns></returns>
        public static DataSet GetPlanDistrictReports(string strQuery)
        {
            SqlConnection cn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString);


            return SqlHelper.ExecuteDataset(cn, CommandType.Text, strQuery);
        }
        /// <summary>
        /// It gives the required column name and the translated name to be shown on the excel sheet
        /// as per the report type id and row number in excel report
        /// </summary>
        /// <param name="iReportType"></param>
        /// <param name="iRowID"></param>
        /// <returns></returns>
        public static DataTable GetReportTemplate(int iReportType,int iRowID)
        {            
            SqlConnection cn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString);

            SqlParameter[] arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@ReportTypeID", iReportType);
            arParams[1] = new SqlParameter("@Row", iRowID);

            return SqlHelper.ExecuteDataset(cn, "pprx.usp_Report_GetExcelRowDetails",arParams).Tables[0];
        }
        /// <summary>
        /// For archiving campaign opportunity data after campaign creation.
        /// </summary>
        /// <param name="iCampaignID"></param>
        /// <param name="iSegmentID"></param>
        /// <param name="strTerritoryID"></param>
        /// <param name="strBrandList">comma separated brand id list</param>
        /// <param name="strAEName">AE Name</param>
        public static void InsertCampaignOpportunityData(int iCampaignID, int iSegmentID,string strTerritoryID, string strBrandList,string strAEName)
        {
            SqlConnection cn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString);

            SqlParameter[] arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@Campaign_ID", iCampaignID);
            arParams[1] = new SqlParameter("@Segment_ID", iSegmentID);
            arParams[2] = new SqlParameter("@AE_Territory_ID", strTerritoryID);
            arParams[3] = new SqlParameter("@Brand_ID_List", strBrandList);
            arParams[4] = new SqlParameter("@AE_Name", strAEName);
            
            SqlHelper.ExecuteNonQuery(cn, "pprx.usp_Insert_Campaign_Opportunity_Archived_Data", arParams);            
        }


        /// <summary>
        /// This function returns Territory_ID for selected User.
        /// </summary>
        /// <param name="Full_Name"></param>
        /// <returns></returns>
        public static String GetUserTerritory(string UserFullName)
        {
            SqlConnection cn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString);

            SqlParameter[] arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@Full_Name", UserFullName);
            
            Int32 Territory_ID = System.Convert.ToInt32(SqlHelper.ExecuteScalar(cn, "pprx.usp_GetUserTerritory", arParams));
            return Territory_ID.ToString();
        }
    }
}
