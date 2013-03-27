using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pinsonault.Web.Services;
using System.Data.Services;
using PathfinderClientModel;
using System.Linq.Expressions;
using Pinsonault.Data;
using System.ServiceModel.Web;
using System.Web;


namespace Pinsonault.Application.Alcon
{

    public class AlconService : PathfinderDataServiceBase<PathfinderAlconEntities>
    {
        // This method is called only once to initialize service-wide policies.
        public static void InitializeService(IDataServiceConfiguration config)
        {
            Pinsonault.Web.Support.InitializeService(config);          
            config.SetEntitySetAccessRule("PlanDocumentsViewSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("PlanGridSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("ContactReportDrilldownSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("ActivityReportingDetailsSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("ContactReportDataExSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("SellSheetReportSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("SellSheetTheraListSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("SellSheetDrugListSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("SellSheetGetUserSet", EntitySetRights.AllRead);
        }

       #region Query Interceptors
        [QueryInterceptor("ContactReportDrilldownSet")]
        public Expression<Func<ContactReportDrilldown, bool>> FilterDrillDown()
        {
            string strwhere = string.Empty;
            int[] ContactReportIDs = { 0 };

            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["Meeting_Activity_ID"]))
            {
                string meetingActivityIds = System.Web.HttpContext.Current.Request.QueryString["Meeting_Activity_ID"].ToString();
                strwhere = " it.Meeting_Activity_ID in {" + meetingActivityIds + "}";
            }

            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["Products_Discussed_ID"]))
            {
                string productsdiscussedIds = System.Web.HttpContext.Current.Request.QueryString["Products_Discussed_ID"].ToString();
                if (string.IsNullOrEmpty(strwhere))
                    strwhere = " it.Products_Discussed_ID in {" + productsdiscussedIds + "}";
                else
                    strwhere = strwhere + " and it.Products_Discussed_ID in {" + productsdiscussedIds + "}";
            }

            if (!string.IsNullOrEmpty(strwhere))
            {
                var list = (from CRID in CurrentDataSource.ContactReportDrillDownFilterSet.Where(strwhere)
                            orderby CRID.Contact_Report_ID
                            select CRID.Contact_Report_ID).ToList().Distinct();
                if (list.Count() > 0)
                    ContactReportIDs = list.ToArray();

                return Pinsonault.Data.Generic.GetFilterForList<ContactReportDrilldown, int>(ContactReportIDs, "Contact_Report_ID");

            }
            else
                return e => true;

        }

        [QueryInterceptor("ContactReportDataExSet")]
        public Expression<Func<ContactReportDataEx, bool>> FilterProdAndMeetingTypeDrillDown()
        {
            string strwhere = string.Empty;
            int[] ContactReportIDs = { 0 };

            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["Products_Discussed_ID"]))
            {
                string productsdiscussedIds = System.Web.HttpContext.Current.Request.QueryString["Products_Discussed_ID"].ToString();
                if (string.IsNullOrEmpty(strwhere))
                    strwhere = " it.Products_Discussed_ID in {" + productsdiscussedIds + "}";
                else
                    strwhere = strwhere + " and it.Products_Discussed_ID in {" + productsdiscussedIds + "}";
            }
            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["Meeting_Activity_ID"]))
            {
                string MeetingActivityIDs = System.Web.HttpContext.Current.Request.QueryString["Meeting_Activity_ID"].ToString();
                if (string.IsNullOrEmpty(strwhere))
                    strwhere = " it.Meeting_Activity_ID in {" + MeetingActivityIDs + "}";
                else
                    strwhere = strwhere + " and it.Meeting_Activity_ID in {" + MeetingActivityIDs + "}";
            }

            if (!string.IsNullOrEmpty(strwhere))
            {
                var list = (from CRID in CurrentDataSource.ContactReportDrillDownFilterSet.Where(strwhere)
                            orderby CRID.Contact_Report_ID
                            select CRID.Contact_Report_ID).ToList().Distinct();
                if (list.Count() > 0)
                    ContactReportIDs = list.ToArray();

                return Pinsonault.Data.Generic.GetFilterForList<ContactReportDataEx, int>(ContactReportIDs, "Contact_Report_ID");

            }
            else
                return e => true;

        }

        [QueryInterceptor("SellSheetReportSet")]
        public Expression<Func<SellSheetReport, bool>> FilterSellSheetReport()
        {
            string strwhere = string.Empty;
            Int32[] SellSheetIDs = { 0 };

            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["Section_ID"]))
            {
                string Section_IDs = System.Web.HttpContext.Current.Request.QueryString["Section_ID"].ToString();
                if (string.IsNullOrEmpty(strwhere))
                    strwhere = " it.Section_ID in {" + Section_IDs + "}";
                else
                    strwhere = strwhere + " and it.Section_ID in {" + Section_IDs + "}";
            }
            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["Thera_ID"]))
            {
                string theraIds = System.Web.HttpContext.Current.Request.QueryString["Thera_ID"].ToString();
                if (string.IsNullOrEmpty(strwhere))
                    strwhere = " it.Thera_ID in {" + theraIds + "}";
                else
                    strwhere = strwhere + " and it.Thera_ID in {" + theraIds + "}";
            }
            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["Drug_ID"]))
            {
                string drugIds = System.Web.HttpContext.Current.Request.QueryString["Drug_ID"].ToString();
                if (string.IsNullOrEmpty(strwhere))
                    strwhere = " it.Drug_ID in {" + drugIds + "}";
                else
                    strwhere = strwhere + " and it.Drug_ID in {" + drugIds + "}";
            }
            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["Plan_ID"]))
            {
                string Plan_IDs = System.Web.HttpContext.Current.Request.QueryString["Plan_ID"].ToString();
                if (string.IsNullOrEmpty(strwhere))
                    strwhere = " it.Plan_ID in {" + Plan_IDs + "}";
                else
                    strwhere = strwhere + " and it.Plan_ID in {" + Plan_IDs + "}";
            }
            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["User_ID"]))
            {
                string User_IDs = System.Web.HttpContext.Current.Request.QueryString["User_ID"].ToString();
                if (string.IsNullOrEmpty(strwhere))
                    strwhere = " it.User_ID in {" + User_IDs + "}";
                else
                    strwhere = strwhere + " and it.User_ID in {" + User_IDs + "}";
            }
            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["Title_ID"]))
            {
                string Title_ID = System.Web.HttpContext.Current.Request.QueryString["Title_ID"].ToString();
                if (string.IsNullOrEmpty(strwhere))
                    strwhere = " it.Title  = " + " '" + Title_ID + "'";
                else
                    strwhere = strwhere + " and it.Title  = " + " '" + Title_ID + "'" ;
            }

            //filter sales territory_id 
            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["Geography_ID"]))
            {
                string Geography_ID = System.Web.HttpContext.Current.Request.QueryString["Geography_ID"].ToString();
                if (string.IsNullOrEmpty(strwhere))
                    strwhere = " (it.State_ID  = '" + Geography_ID + "')";
                else
                    strwhere = strwhere + " and (it.State_ID  = '" + Geography_ID + "')";
            }           

            if (!string.IsNullOrEmpty(strwhere))
            {
                string filter = HttpContext.Current.Request.QueryString["$filter"];
                filter = filter.Replace("and", ",");
                string[] filter_params = filter.Split(',');

                //string CreatedDt = HttpContext.Current.Request.QueryString["$filter"].Substring(HttpContext.Current.Request.QueryString["$filter"].IndexOf("Created_DT"));
                string fromDt = filter_params[0].Substring(filter_params[0].IndexOf("ge")).Replace("ge datetime", "").Replace("'", "");
                string ToDt = filter_params[1].Substring(filter_params[1].IndexOf("le")).Replace("le datetime", "").Replace("'", "").Replace(")", "");
               int status_ID = Convert.ToInt16(filter_params[2].Substring(filter_params[2].IndexOf("eq")).Replace("eq", "").Trim());
                DateTime dtFrom = Convert.ToDateTime(fromDt);
                DateTime dtTo = Convert.ToDateTime(ToDt);
                int maxRecords = 500; //temp fix to avoid Stack Overflow
                var list = (from SSID in CurrentDataSource.SellSheetReportFilterSet.Where(strwhere).Where(SSID => SSID.Created_DT >= dtFrom).Where(SSID => SSID.Created_DT <= dtTo).Where(SSID => SSID.Status_ID == status_ID)
                            orderby SSID.Sell_Sheet_ID descending
                            select SSID.Sell_Sheet_ID).ToList().Distinct().Take(maxRecords);
                if (list.Count() > 0)
                    SellSheetIDs = list.ToArray();
             
                return Pinsonault.Data.Generic.GetFilterForList<SellSheetReport,Int32>(SellSheetIDs, "Sell_Sheet_ID");
            }
            else
                return e => true;

        }

        #endregion

        [WebGet]
        public string GetCCRModuleOptions()
        {
            using (PathfinderModel.PathfinderEntities context = new PathfinderModel.PathfinderEntities())
            {
                return context.GetUserReportOptionsAsJSON(Pinsonault.Web.Session.UserID, Pinsonault.Web.Identifiers.CustomerContactReports);
            }
        }       

    }
}