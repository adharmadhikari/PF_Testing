using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using Telerik.Web.UI;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Pinsonault.Application.PowerPlanRx
{

    /// <summary>
    /// For common functions used in the application i.e. Email etc
    /// </summary>
    public class Utility
    {
        public static void InsertBaselineProcess(int CampaignID, int DataMonth, int DataYear, string geoID, Int32 geoLevelID, int x, Decimal geoTRx, Decimal geoMST)
        {

            using (SqlConnection cn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("pprx.usp_CampaignGoals_Baseline_Insert", cn))
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Campaign_ID", CampaignID);
                    cmd.Parameters.AddWithValue("@Geography_ID", geoID);
                    cmd.Parameters.AddWithValue("@Geography_Level_ID", geoLevelID);
                    cmd.Parameters.AddWithValue("@Campaign_Month_Indicator", x);
                   // cmd.Parameters.AddWithValue("@Is_Targeted", 1);
                    cmd.Parameters.AddWithValue("@Data_Month", DataMonth);
                    cmd.Parameters.AddWithValue("@Data_Year", DataYear);

                    cmd.Parameters.AddWithValue("@Baseline_Brand_TRx", geoTRx);
                    //cmd.Parameters.AddWithValue("@Goal_MB_TRx", 
                    cmd.Parameters.AddWithValue("@Baseline_Brand_MST", geoMST);
                    // cmd.Parameters.AddWithValue("@UserName", Impact.User.FullName);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();

                }
            }
        }
        public static void InsertBaselineProcess_Plan(int CampaignID, int DataMonth, int DataYear, int x, Decimal geoTRx, Decimal geoMST)
        {

            using (SqlConnection cn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("pprx.usp_CampaignGoals_Plan_Baseline_Insert", cn))
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Campaign_ID", CampaignID);
                    cmd.Parameters.AddWithValue("@Campaign_Month_Indicator", x);
                    //cmd.Parameters.AddWithValue("@Is_Targeted", 1);
                    cmd.Parameters.AddWithValue("@Data_Month", DataMonth);
                    cmd.Parameters.AddWithValue("@Data_Year", DataYear);

                    cmd.Parameters.AddWithValue("@Baseline_Brand_TRx", geoTRx);
                    //cmd.Parameters.AddWithValue("@Goal_MB_TRx", 
                    cmd.Parameters.AddWithValue("@Baseline_Brand_MST", geoMST);
                    // cmd.Parameters.AddWithValue("@UserName", Impact.User.FullName);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();

                }
            }
        }

        public static void InsertUpdateProcess(int CampaignID, int DataMonth, int DataYear, string geoID, Int32 geoLevelID, int x, Decimal geoTRx, Decimal geoMST)
        {

            using (SqlConnection cn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("pprx.usp_Campaign_InsertUpdateGoals", cn))
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Campaign_ID", CampaignID);
                    cmd.Parameters.AddWithValue("@Geography_ID", geoID);
                    cmd.Parameters.AddWithValue("@Geography_Level_ID", geoLevelID);
                    cmd.Parameters.AddWithValue("@Campaign_Month_Indicator", x);
                    //cmd.Parameters.AddWithValue("@Is_Targeted", 1);
                    cmd.Parameters.AddWithValue("@Data_Month", DataMonth);
                    cmd.Parameters.AddWithValue("@Data_Year", DataYear);

                    cmd.Parameters.AddWithValue("@Goal_Brand_TRx", geoTRx);
                    //cmd.Parameters.AddWithValue("@Goal_MB_TRx", 
                    cmd.Parameters.AddWithValue("@Goal_Brand_MST", geoMST);
                    // cmd.Parameters.AddWithValue("@UserName", Impact.User.FullName);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();

                }
            }
        }

        public static void InsertUpdateProcess_Plan(int CampaignID, int DataMonth, int DataYear, int x, Decimal geoTRx, Decimal geoMST)
        {

            using (SqlConnection cn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("pprx.usp_Campaign_InsertUpdateGoals_Plan", cn))
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Campaign_ID", CampaignID);
                    cmd.Parameters.AddWithValue("@Campaign_Month_Indicator", x);
                    cmd.Parameters.AddWithValue("@Data_Month", DataMonth);
                    cmd.Parameters.AddWithValue("@Data_Year", DataYear);

                    cmd.Parameters.AddWithValue("@Goal_Brand_TRx", geoTRx);
                    //cmd.Parameters.AddWithValue("@Goal_MB_TRx", 
                    cmd.Parameters.AddWithValue("@Goal_Brand_MST", geoMST);
                    // cmd.Parameters.AddWithValue("@UserName", Impact.User.FullName);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();

                }
            }
        }

        public static void get_cellValue_fromLabel(GridDataItem dataItem, string controlID, string controlID2, ref Decimal TRx, ref Decimal MST)
        {
            Label _text = dataItem.FindControl(controlID) as Label;
            Label _text2 = dataItem.FindControl(controlID2) as Label;
            if (_text != null && _text2 != null)
            {
                string val1, val2;
                val1 = _text.Text;
                val2 = _text2.Text;

                if (val1 != null && val1 != "")
                    TRx = Convert.ToDecimal(val1);
                //TRx = Convert.ToInt32(val1.Replace(",", ""));

                if (val2 != null && val2 != "")
                    MST = Convert.ToDecimal(val2);
            }
        }
        public static void get_cellValue_fromTextbox(GridDataItem dataItem, string controlID, string controlID2, ref Decimal TRx, ref Decimal MST)
        {
            RadTextBox _text = dataItem.FindControl(controlID) as RadTextBox;
            RadTextBox _text2 = dataItem.FindControl(controlID2) as RadTextBox;
            if (_text != null && _text2 != null)
            {
                string val1, val2;
                val1 = _text.Text.ToString();
                val2 = _text2.Text.ToString();

                if (val1 != null && val1 != "")
                    TRx = Convert.ToDecimal(val1);
                //TRx = Convert.ToInt32(val1.Replace(",", ""));

                if (val2 != null && val2 != "")
                    MST = Convert.ToDecimal(val2);
            }
        }

        /// <summary>
        /// For sending the email
        /// </summary>
        /// <param name="strTo"></param>
        /// <param name="strFrom"></param>
        /// <param name="strSubject"></param>
        /// <param name="strBody"></param>
        /// <param name="Priority"></param>
        public static void SendEMail(string strTo, string strFrom, string strSubject, string strBody, MailPriority Priority)
        {
            SendEMail(strTo, null, strFrom, strSubject, strBody, Priority);
        }
        /// <summary>
        /// For sending the email with CC
        /// </summary>
        /// <param name="strTo"></param>
        /// <param name="strCC"></param>
        /// <param name="strFrom"></param>
        /// <param name="strSubject"></param>
        /// <param name="strBody"></param>
        /// <param name="Priority"></param>
        public static void SendEMail(string strTo, string strCC, string strFrom, string strSubject, string strBody, MailPriority Priority)
        {
            MailMessage msg = new MailMessage(strFrom, strTo, strSubject, strBody);
            if (!string.IsNullOrEmpty(strCC))
                msg.CC.Add(strCC);
            msg.Priority = Priority;

            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(ConfigurationManager.AppSettings["SmtpHost"]);
            smtp.Send(msg);
        }
        /// <summary>
        /// For getting the Admin Email address
        /// </summary>
        /// <returns></returns>
        public static string GetAdminEmail()
        {
            string strAdminEmailList = "";
            SqlConnection cn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString);

            DataTable dtAdminEmail = SqlHelper.ExecuteDataset(cn, "pprx.usp_GetAdminEMail").Tables[0];
            int iCount;
            for (iCount = 0; iCount < dtAdminEmail.Rows.Count; iCount++)
            {
                strAdminEmailList = strAdminEmailList + dtAdminEmail.Rows[iCount]["EMail"].ToString() + ",";
            }
            return strAdminEmailList;
        }
        /// <summary>
        /// For getting the user's email address
        /// </summary>
        /// <param name="iUserID"></param>
        /// <returns></returns>
        public static string GetUserEmail(int iUserID)
        {
            string strUserEmail = "";

            SqlConnection cn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString);

            SqlParameter[] arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@User_ID", iUserID);

            strUserEmail = SqlHelper.ExecuteDataset(cn, "pprx.usp_GetUserEMail", arParams).Tables[0].Rows[0]["EMail"].ToString();

            return strUserEmail;
        }


        /// <summary>
        /// helper function to properly add a column dynamically to a Telerik RadGrid
        /// </summary>
        /// <param name="grid">Telerik RadGrid that a column is added to.</param>
        /// <param name="headerTemplate">URL of a control to use as the theader template.</param>
        /// <param name="dataTemplate">URL of a control to use for a data item's template</param>
        public static GridTemplateColumn AddColumn(RadGrid grid, string name, string headerTemplate, string dataTemplate)
        {
            GridTemplateColumn column = null;

            column = grid.Columns.FindByUniqueNameSafe(name) as GridTemplateColumn;
            if (column == null)
            {
                //must add column to collection before setting properties
                column = new GridTemplateColumn();
                grid.Columns.Add(column);

                column.UniqueName = name;
            }
            column.HeaderTemplate = grid.Page.LoadTemplate(headerTemplate);
            column.ItemTemplate = grid.Page.LoadTemplate(dataTemplate);
            //column.HeaderStyle.Width = 100;
            //column.ItemStyle.Width = 100;

            return column;
        }

    }
}
