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
using Pinsonault.Application.Nipro;

public partial class custom_pinso_customercontactreports_all_meetingtype : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Charts
        ccrChart1.FindControl("chartProduct1").Visible = false;
        ccrChart1.FindControl("chartProduct1National").Visible = false;
        ccrChart1.FindControl("chartProduct2").Visible = false;
        ccrChart1.FindControl("chartProduct2National").Visible = false;
        ccrChart1.FindControl("chartProduct3").Visible = false;
        ccrChart1.FindControl("chartProduct3National").Visible = false;

        //Data Grids
        ccrMeetingTypeData1.FindControl("dataProduct1").Visible = false;
        ccrMeetingTypeData1.FindControl("dataProduct1National").Visible = false;
        ccrMeetingTypeData1.FindControl("dataProduct2").Visible = false;
        ccrMeetingTypeData1.FindControl("dataProduct2National").Visible = false;
        ccrMeetingTypeData1.FindControl("dataProduct3").Visible = false;
        ccrMeetingTypeData1.FindControl("dataProduct3National").Visible = false;


        //Instantiate helper class
        ContactReportProvider cr = new ContactReportProvider();

        NameValueCollection queryValues = new NameValueCollection(Request.QueryString);

        ////Add aggregates to NameValueCollection
        //queryValues.Add("__aggr", "Count(User_ID), PercentCount(User_ID)");
        //queryValues.Add("__select", "Meeting_Type_ID, Meeting_Type_Name, Products_Discussed_ID, Drug_Name");
        //queryValues.Add("__sort", "Meeting_Type_Name");

        CustomerContactReportsQueryDef qd;

        using ( PathfinderClientModel.PathfinderClientEntities clientContext = new PathfinderClientModel.PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString) )
        {
            IList<DbDataRecord> meetingTypeFilterProd1 = null;
            IList<DbDataRecord> meetingTypeFilterProd1National = null;

            string[] productId = cr.GetProductsDiscussedID();
            int prodID = 0;

            //Create Entity Query based on number of products selected (max 3 products)
            //If 3 products, all statements execute
            //If 2 products, first 2 statements execute
            //If 1 product, first statement executes
            int chartIndex = 1;

            for ( int i = 0; i < productId.Length; i++ )
            {
                queryValues["Products_Discussed_ID"] = productId[i];

                prodID = Convert.ToInt32(productId[i]);

                qd = new MeetingTypeCustomerContactReportQueryDef("ContactReportDataSummary", queryValues);
                meetingTypeFilterProd1 = (qd.CreateQuery(clientContext) as IEnumerable<DbDataRecord>).ToList();

                if ( meetingTypeFilterProd1.Count() > 0 )
                {
                    ccrChart1.FindControl(string.Format("chartProduct{0}",chartIndex)).Visible = true;
                    ccrMeetingTypeData1.FindControl(string.Format("dataProduct{0}", chartIndex)).Visible = true;

                    cr.ProcessChart(ccrChart1.FindControl(string.Format("chartProduct{0}", chartIndex)).FindControl("chart") as Chart, meetingTypeFilterProd1.ToList(), prodID, "meetingtype", chartIndex);
                    cr.ProcessGrid(ccrMeetingTypeData1.FindControl(string.Format("dataProduct{0}", chartIndex)).FindControl("gridCcrMeetingType") as RadGrid, meetingTypeFilterProd1);
                }

                //Get National data if only Region/State was selected for comparison chart
                if ( string.IsNullOrEmpty(Request.QueryString["Is_National"]) )
                {
                    compareContainer.Visible = true;

                    NameValueCollection queryValueNational = new NameValueCollection(queryValues);

                    queryValueNational.Remove("Geography_ID");
                    queryValueNational.Add("Is_National", "1");

                    qd = new MeetingTypeCustomerContactReportQueryDef("ContactReportDataSummary", queryValueNational);
                    meetingTypeFilterProd1National = (qd.CreateQuery(clientContext) as IEnumerable<DbDataRecord>).ToList();

                    if ( meetingTypeFilterProd1National.Count() > 0 )
                    {
                        ccrChart1.FindControl(string.Format("chartProduct{0}National", chartIndex)).Visible = true;
                        ccrMeetingTypeData1.FindControl(string.Format("dataProduct{0}National", chartIndex)).Visible = true;

                        cr.ProcessChart(ccrChart1.FindControl(string.Format("chartProduct{0}National", chartIndex)).FindControl("chart") as Chart, meetingTypeFilterProd1National.ToList(), prodID, "meetingtype", chartIndex + 3);
                        cr.ProcessGrid(ccrMeetingTypeData1.FindControl(string.Format("dataProduct{0}National", chartIndex)).FindControl("gridCcrMeetingType") as RadGrid, meetingTypeFilterProd1National);
                    }
                }

                chartIndex++;
            }
        }
    }
}
