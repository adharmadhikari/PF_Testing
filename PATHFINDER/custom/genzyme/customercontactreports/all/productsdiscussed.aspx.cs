using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dundas.Charting.WebControl;
using Telerik.Web.UI;
using System.Data.Common;
using System.Collections.Specialized;
using Pathfinder;
using Pinsonault.Data;
using Pinsonault.Application.Genzyme;

public partial class custom_pinso_customercontactreports_all_productsdiscussed : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Charts
        ccrChart1.FindControl("chartProduct1").Visible = false; 
        ccrChart1.FindControl("chartProduct1National").Visible = false; //Chart 1 National never needed
        ccrChart1.FindControl("chartProduct2").Visible = false;
        ccrChart1.FindControl("chartProduct2National").Visible = false; //Chart 2 National never needed
        ccrChart1.FindControl("chartProduct3").Visible = false; //Chart 3 never needed
        ccrChart1.FindControl("chartProduct3National").Visible = false; //Chart 3 National never needed

        Chart chart1 = (Chart)ccrChart1.FindControl("chartProduct1").FindControl("chart");
        Chart chart2 = (Chart)ccrChart1.FindControl("chartProduct2").FindControl("chart");

        //Data Grids
        ccrProductsDiscussedData1.FindControl("dataProduct1").Visible = false;  //YTD always visible
        ccrProductsDiscussedData1.FindControl("dataProduct2").Visible = false;

        RadGrid grid1 = (RadGrid)ccrProductsDiscussedData1.FindControl("dataProduct1").FindControl("gridCcrProductsDiscussed");
        RadGrid grid2 = (RadGrid)ccrProductsDiscussedData1.FindControl("dataProduct2").FindControl("gridCcrProductsDiscussed");

        //Instantiate helper class
        ContactReportProvider cr = new ContactReportProvider();

        NameValueCollection queryValues = new NameValueCollection(Request.QueryString);

        //-----------------------------------------------------------------------------------------------------------------------
        //get the filtered contact report id
        int[] ContactReportIDs = null;

        //get contact report id
        string strwhere = string.Empty;

        if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["Meeting_Activity_ID"]))
        {
            string meetingActivityIds = System.Web.HttpContext.Current.Request.QueryString["Meeting_Activity_ID"].ToString();
            strwhere = " it.Meeting_Activity_ID in {" + meetingActivityIds + "}";
        }

        if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["Meeting_Outcome_ID"]))
        {
            string meetingOutcomeIds = System.Web.HttpContext.Current.Request.QueryString["Meeting_Outcome_ID"].ToString();

            if (string.IsNullOrEmpty(strwhere))
                strwhere = " it.Outcome_ID in {" + meetingOutcomeIds + "}";
            else
                strwhere = strwhere + " and it.Outcome_ID in {" + meetingOutcomeIds + "}";
        }

        if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["Followup_Notes_ID"]))
        {
            string followupIds = System.Web.HttpContext.Current.Request.QueryString["Followup_Notes_ID"].ToString();
            if (string.IsNullOrEmpty(strwhere))
                strwhere = " it.Followup_ID in {" + followupIds + "}";
            else
                strwhere = strwhere + " and it.Followup_ID in {" + followupIds + "}";

        }
        //filter the outcome and followup notes, get the contact report id and set it in the filter criteria
        if (!string.IsNullOrEmpty(strwhere))
        {
            using (Pinsonault.Application.Genzyme.PathfinderGenzymeEntities ctx = new Pinsonault.Application.Genzyme.PathfinderGenzymeEntities())
            {
                var list = (from CRID in ctx.ContactReportFilterSet.Where(strwhere)
                            orderby CRID.Contact_Report_ID
                            select CRID.Contact_Report_ID).ToList().Distinct();
                ContactReportIDs = list.ToArray();
            }
            queryValues.Add("Contact_Report_ID", ConvertArrayToString(ContactReportIDs));
        }

        //-----------------------------------------------------------------------------------------------------------------------

        //Add aggregates to NameValueCollection
        queryValues.Add("__aggr", "Count(User_ID), PercentCount(User_ID)");
        queryValues.Add("__select", "Drug_Name, Products_Discussed_ID");
        queryValues.Add("__sort", "Drug_Name");

        using (PathfinderGenzymeEntities clientContext = new PathfinderGenzymeEntities())
        {
            IList<DbDataRecord> prodDiscFilterProd1 = null;
            IList<DbDataRecord> prodDiscFilterProd2 = null;

            //Create Entity Query based on Date Filter
            DateTime yearBeginDt = Convert.ToDateTime(string.Format("01/01/{0}", DateTime.Now.Year));
            DateTime today = Convert.ToDateTime(string.Format("{0}/{1}/{2}", DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Year));
            string[] contactDate = queryValues["Contact_Date"].Split(',');

            //Show Current Month or Timeframe based on Contact_Date querystring value
            if ((DateTime.Compare(yearBeginDt, Convert.ToDateTime(contactDate[0])) != 0) || (DateTime.Compare(today, Convert.ToDateTime(contactDate[1])) != 0))
            {
                prodDiscFilterProd2 = Pinsonault.Data.Generic.CreateGenericEntityQuery<Pinsonault.Application.Genzyme.ContactReportDataSummary>(clientContext, new QueryDefinition(queryValues)).ToList();
                if (prodDiscFilterProd2.Count() > 0)
                {
                    ccrChart1.FindControl("chartProduct2").Visible = true;
                    ccrProductsDiscussedData1.FindControl("dataProduct2").Visible = true;
                    cr.ProcessChart(chart2, prodDiscFilterProd2, contactDate, 2);
                    cr.ProcessGrid(grid2, prodDiscFilterProd2);
                }
            }

            //Always show YTD Data
            queryValues["Contact_Date"] = string.Format("{0},{1}", yearBeginDt.ToShortDateString(), today.ToShortDateString());
            prodDiscFilterProd1 = Pinsonault.Data.Generic.CreateGenericEntityQuery<Pinsonault.Application.Genzyme.ContactReportDataSummary>(clientContext, new QueryDefinition(queryValues)).ToList();
            if (prodDiscFilterProd1.Count() > 0)
            {
                ccrChart1.FindControl("chartProduct1").Visible = true;
                ccrProductsDiscussedData1.FindControl("dataProduct1").Visible = true;
                contactDate[0] = yearBeginDt.ToShortDateString();
                contactDate[1] = today.ToShortDateString();
                cr.ProcessChart(chart1, prodDiscFilterProd1, contactDate, 1);
                cr.ProcessGrid(grid1, prodDiscFilterProd1);
            }
        }
    }

    private string ConvertArrayToString(int[] intArray)
    {
        string[] stringArray = Array.ConvertAll<int, string>(intArray, new Converter<int, string>(ConvertIntToString));
        string result = string.Join(",", stringArray);
        return result;
    }
    private string ConvertIntToString(int intParameter)
    { return intParameter.ToString(); }
}
