using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Data.Common;
using Dundas.Charting.WebControl;
using Telerik.Web.UI;
using Pathfinder;
using Pinsonault.Data;
using Pinsonault.Application.CSL;

public partial class custom_pinso_customercontactreports_all_meetingactivity : PageBase
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
        ccrMeetingActivityData1.FindControl("dataProduct1").Visible = false;
        ccrMeetingActivityData1.FindControl("dataProduct1National").Visible = false;
        ccrMeetingActivityData1.FindControl("dataProduct2").Visible = false;
        ccrMeetingActivityData1.FindControl("dataProduct2National").Visible = false;
        ccrMeetingActivityData1.FindControl("dataProduct3").Visible = false;
        ccrMeetingActivityData1.FindControl("dataProduct3National").Visible = false;

        //Instantiate helper class
        ContactReportProvider cr = new ContactReportProvider();

        NameValueCollection queryValues = new NameValueCollection(Request.QueryString);

        ////Add aggregates to NameValueCollection
        //queryValues.Add("__aggr", "Count(User_ID), PercentCount(User_ID)");
        //queryValues.Add("__select", "Meeting_Activity_ID, Meeting_Activity_Name, Products_Discussed_ID, Drug_Name");
        //queryValues.Add("__sort", "Meeting_Activity_Name");

        using (PathfinderClientModel.PathfinderClientEntities clientContext = new PathfinderClientModel.PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
        {
            IList<DbDataRecord> meetingActivityFilterProd1 = null;
            IList<DbDataRecord> meetingActivityFilterProd1National = null;


            string[] productId = cr.GetProductsDiscussedID();
            int prodID = 0;
            int chartIndex = 1;
            CustomerContactReportsQueryDef qd;

            for(int i=0;i<productId.Length; i++)
            {
                queryValues["Products_Discussed_ID"] = productId[i];

                prodID = Convert.ToInt32(productId[i]);

                qd = new MeetingActivityCustomerContactReportQueryDef("ContactReportDataSummary", queryValues);
                meetingActivityFilterProd1 = (qd.CreateQuery(clientContext) as IEnumerable<DbDataRecord>).ToList();

                if (meetingActivityFilterProd1.Count() > 0)
                {
                    ccrChart1.FindControl(string.Format("chartProduct{0}", chartIndex)).Visible = true;
                    ccrMeetingActivityData1.FindControl(string.Format("dataProduct{0}", chartIndex)).Visible = true;


                    cr.ProcessChart(ccrChart1.FindControl(string.Format("chartProduct{0}", chartIndex)).FindControl("chart") as Chart, meetingActivityFilterProd1.ToList(), prodID, "meetingactivity", chartIndex);
                    cr.ProcessGrid(ccrMeetingActivityData1.FindControl(string.Format("dataProduct{0}", chartIndex)).FindControl("gridCcrMeetingActivity") as RadGrid, meetingActivityFilterProd1);
                }

                //Get National data if only Region/State was selected for comparison chart
                if (string.IsNullOrEmpty(Request.QueryString["Is_National"]))
                {
                    compareContainer.Visible = true;

                    NameValueCollection queryValueNational = new NameValueCollection(queryValues);

                    queryValueNational.Remove("Geography_ID");
                    queryValueNational.Add("Is_National", "1");

                    qd = new MeetingActivityCustomerContactReportQueryDef("ContactReportDataSummary", queryValueNational);
                    meetingActivityFilterProd1National = (qd.CreateQuery(clientContext) as IEnumerable<DbDataRecord>).ToList();
                    if (meetingActivityFilterProd1National.Count() > 0)
                    {
                        ccrChart1.FindControl(string.Format("chartProduct{0}National", chartIndex)).Visible = true;
                        ccrMeetingActivityData1.FindControl(string.Format("dataProduct{0}National", chartIndex)).Visible = true;
                        
                        cr.ProcessChart(ccrChart1.FindControl(string.Format("chartProduct{0}National", chartIndex)).FindControl("chart") as Chart, meetingActivityFilterProd1National.ToList(), prodID, "meetingactivity", chartIndex+3);
                        cr.ProcessGrid(ccrMeetingActivityData1.FindControl(string.Format("dataProduct{0}National", chartIndex)).FindControl("gridCcrMeetingActivity") as RadGrid, meetingActivityFilterProd1National);
                    }
                }
                chartIndex++;
            }
        }
    }     
}
