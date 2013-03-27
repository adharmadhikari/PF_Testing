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

        //Add aggregates to NameValueCollection
        queryValues.Add("__aggr", "Count(User_ID), PercentCount(User_ID)");
        queryValues.Add("__select", "Drug_Name, Products_Discussed_ID");
        queryValues.Add("__sort", "Drug_Name");

        using ( PathfinderClientModel.PathfinderClientEntities clientContext = new PathfinderClientModel.PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString) )
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
                prodDiscFilterProd2 = Generic.CreateGenericEntityQuery<PathfinderClientModel.ContactReportDataSummary>(clientContext, new QueryDefinition(queryValues)).ToList();
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
            prodDiscFilterProd1 = Generic.CreateGenericEntityQuery<PathfinderClientModel.ContactReportDataSummary>(clientContext, new QueryDefinition(queryValues)).ToList();
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
}
