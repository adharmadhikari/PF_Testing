using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;

namespace Pinsonault.Application.PowerPlanRx
{
    /// <summary>
    /// Summary description for Goals
    /// </summary>
    public static class Goals
    {
        public static string GetFormattedGoalValue(object value, string format)
        {
            if (value == null || typeof(DBNull).Equals(value.GetType()))
                return string.Format(format, 0);
            else
                return string.Format(format, value);
        }

        //returns timeline information about a campaign
        public static void getCampaignTimeline(int id, out DateTime start, out int duration, out string planName)
        {
            start = DateTime.MinValue;
            duration = 0;
            planName = string.Empty;


            using (SqlConnection cn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("pprx.usp_Get_Campaign_Timeline", cn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", id);

                    cn.Open();

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            start = rdr.GetDateTime(rdr.GetOrdinal("Implementation_Start_Date"));
                            duration = rdr.GetInt32(rdr.GetOrdinal("Campaign_Duration"));
                            planName = rdr.GetString(rdr.GetOrdinal("Campaign_Name"));
                        }
                    }
                }
            }
        }

        public static void getCampaignBaselineYearMonth(int id, out int baseMonth, out int baseYear, out string baseYearMonth, out int duration)
        {
            baseMonth = 0;
            baseYear = 0;
            baseYearMonth = string.Empty;
            duration = 0;
            using (SqlConnection cn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString))
            {
                cn.Open();

                // to get Baseline Month & Year 
                using (SqlCommand cmd = new SqlCommand("select Data_Month, Data_Year, Data_Key, Campaign_Duration from pprx.v_Campaign_R_D_T_Baseline where Campaign_ID=@id", cn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            baseMonth = rdr.GetInt32(0);
                            baseYear = rdr.GetInt32(1);
                            baseYearMonth = Convert.ToString(rdr.GetInt32(2));
                            duration = rdr.GetInt32(3);
                        }
                    }
                }

            }

        }


    }
}