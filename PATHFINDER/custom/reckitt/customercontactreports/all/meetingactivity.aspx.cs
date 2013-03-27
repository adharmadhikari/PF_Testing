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

public partial class custom_reckitt_customercontactreports_all_meetingactivity : PageBase
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

        Chart chart1 = (Chart)ccrChart1.FindControl("chartProduct1").FindControl("chart");
        Chart chart1National = (Chart)ccrChart1.FindControl("chartProduct1National").FindControl("chart");
        Chart chart2 = (Chart)ccrChart1.FindControl("chartProduct2").FindControl("chart");
        Chart chart2National = (Chart)ccrChart1.FindControl("chartProduct2National").FindControl("chart");
        Chart chart3 = (Chart)ccrChart1.FindControl("chartProduct3").FindControl("chart");
        Chart chart3National = (Chart)ccrChart1.FindControl("chartProduct3National").FindControl("chart");

        //Data Grids
        ccrMeetingActivityData1.FindControl("dataProduct1").Visible = false;
        ccrMeetingActivityData1.FindControl("dataProduct1National").Visible = false;
        ccrMeetingActivityData1.FindControl("dataProduct2").Visible = false;
        ccrMeetingActivityData1.FindControl("dataProduct2National").Visible = false;
        ccrMeetingActivityData1.FindControl("dataProduct3").Visible = false;
        ccrMeetingActivityData1.FindControl("dataProduct3National").Visible = false;

        RadGrid grid1 = (RadGrid)ccrMeetingActivityData1.FindControl("dataProduct1").FindControl("gridCcrMeetingActivity");
        RadGrid grid1National = (RadGrid)ccrMeetingActivityData1.FindControl("dataProduct1National").FindControl("gridCcrMeetingActivity");
        RadGrid grid2 = (RadGrid)ccrMeetingActivityData1.FindControl("dataProduct2").FindControl("gridCcrMeetingActivity");
        RadGrid grid2National = (RadGrid)ccrMeetingActivityData1.FindControl("dataProduct2National").FindControl("gridCcrMeetingActivity");
        RadGrid grid3 = (RadGrid)ccrMeetingActivityData1.FindControl("dataProduct3").FindControl("gridCcrMeetingActivity");
        RadGrid grid3National = (RadGrid)ccrMeetingActivityData1.FindControl("dataProduct3National").FindControl("gridCcrMeetingActivity");

        //Instantiate helper class
        ContactReportProvider cr = new ContactReportProvider();

        NameValueCollection queryValues = new NameValueCollection(Request.QueryString);

        //get the filtered contact report id
        int[] ContactReportIDs = null;

        //get contact report id
        string strwhere = string.Empty;

        if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["Meeting_Outcome_ID"]))
        {
            string meetingOutcomeIds = System.Web.HttpContext.Current.Request.QueryString["Meeting_Outcome_ID"].ToString();
            strwhere = " it.Outcome_ID in {" + meetingOutcomeIds + "}";
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
            using (Pinsonault.Application.Reckitt.PathfinderReckittEntities ctx = new Pinsonault.Application.Reckitt.PathfinderReckittEntities())
            {
                var list = (from CRID in ctx.ContactReportFilterSet.Where(strwhere)
                            orderby CRID.Contact_Report_ID
                            select CRID.Contact_Report_ID).ToList().Distinct();
                ContactReportIDs = list.ToArray();
            }
            queryValues.Add("Contact_Report_ID", ConvertArrayToString(ContactReportIDs));
            //string strList = string.Format("'{0}'", ConvertArrayToString(ContactReportIDs));

            //string strFunction = string.Format("setFilterForReportsDrillDown({0});", strList);
            //Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "refresh", strFunction, true);
        }


        //Add aggregates to NameValueCollection
        queryValues.Add("__aggr", "Count(User_ID), PercentCount(User_ID)");
        queryValues.Add("__select", "Meeting_Activity_ID, Meeting_Activity_Name, Products_Discussed_ID, Drug_Name");
        queryValues.Add("__sort", "Meeting_Activity_Name");

        using (PathfinderClientModel.PathfinderClientEntities clientContext = new PathfinderClientModel.PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
        {
            IList<DbDataRecord> meetingActivityFilterProd1 = null;
            IList<DbDataRecord> meetingActivityFilterProd1National = null;
            IList<DbDataRecord> meetingActivityFilterProd2 = null;
            IList<DbDataRecord> meetingActivityFilterProd2National = null;
            IList<DbDataRecord> meetingActivityFilterProd3 = null;
            IList<DbDataRecord> meetingActivityFilterProd3National = null;

            ////Check if National
            //if (Request.QueryString["Geography_ID"] == null)
            //{
            //    //If national, strip 'US' from Geography_ID
            //    queryValues.Remove("Geography_ID");
            //    //Add a query value to filter by States (national) only (1: State; 0: Territory)
            //    queryValues.Add("Is_National", "1");
            //}

            string[] productId = cr.GetProductsDiscussedID();
            int prodID = 0;

            //Create Entity Query based on number of products selected (max 3 products)
            if (productId.Count() >= 1)
            {
                queryValues["Products_Discussed_ID"] = productId[0];
                meetingActivityFilterProd1 = Pinsonault.Data.Generic.CreateGenericEntityQuery<PathfinderClientModel.ContactReportDataSummary>(clientContext, new QueryDefinition(queryValues)).ToList();
                if (meetingActivityFilterProd1.Count() > 0)
                {
                    ccrChart1.FindControl("chartProduct1").Visible = true;
                    ccrMeetingActivityData1.FindControl("dataProduct1").Visible = true;
                    prodID = Convert.ToInt32(productId[0]);
                    cr.ProcessChart(chart1, meetingActivityFilterProd1.ToList(), prodID, "meetingactivity", 1);
                    cr.ProcessGrid(grid1, meetingActivityFilterProd1);
                }

                //Get National data if only Region/State was selected for comparison chart
                if (string.IsNullOrEmpty(Request.QueryString["Is_National"]))
                {
                    compareContainer.Visible = true;

                    NameValueCollection queryValueNational = new NameValueCollection(queryValues);

                    queryValueNational.Remove("Geography_ID");
                    queryValueNational.Add("Is_National", "1");

                    meetingActivityFilterProd1National = Pinsonault.Data.Generic.CreateGenericEntityQuery<PathfinderClientModel.ContactReportDataSummary>(clientContext, new QueryDefinition(queryValueNational)).ToList();

                    if (meetingActivityFilterProd1National.Count() > 0)
                    {
                        ccrChart1.FindControl("chartProduct1National").Visible = true;
                        ccrMeetingActivityData1.FindControl("dataProduct1National").Visible = true;
                        prodID = Convert.ToInt32(productId[0]);
                        cr.ProcessChart(chart1National, meetingActivityFilterProd1National.ToList(), prodID, "meetingactivity", 4);
                        cr.ProcessGrid(grid1National, meetingActivityFilterProd1National);
                    }
                }
            }
            if (productId.Count() >= 2)
            {
                queryValues["Products_Discussed_ID"] = productId[1];
                meetingActivityFilterProd2 = Pinsonault.Data.Generic.CreateGenericEntityQuery<PathfinderClientModel.ContactReportDataSummary>(clientContext, new QueryDefinition(queryValues)).ToList();
                if (meetingActivityFilterProd2.Count() > 0)
                {
                    ccrChart1.FindControl("chartProduct2").Visible = true;
                    ccrMeetingActivityData1.FindControl("dataProduct2").Visible = true;
                    prodID = Convert.ToInt32(productId[1]);
                    cr.ProcessChart(chart2, meetingActivityFilterProd2.ToList(), prodID, "meetingactivity", 2);
                    cr.ProcessGrid(grid2, meetingActivityFilterProd2);
                }

                //Get National data if only Region/State was selected for comparison chart
                if (string.IsNullOrEmpty(Request.QueryString["Is_National"]))
                {
                    NameValueCollection queryValueNational = new NameValueCollection(queryValues);

                    queryValueNational.Remove("Geography_ID");
                    queryValueNational.Add("Is_National", "1");

                    meetingActivityFilterProd2National = Pinsonault.Data.Generic.CreateGenericEntityQuery<PathfinderClientModel.ContactReportDataSummary>(clientContext, new QueryDefinition(queryValueNational)).ToList();

                    if (meetingActivityFilterProd2National.Count() > 0)
                    {
                        ccrChart1.FindControl("chartProduct2National").Visible = true;
                        ccrMeetingActivityData1.FindControl("dataProduct2National").Visible = true;
                        prodID = Convert.ToInt32(productId[1]);
                        cr.ProcessChart(chart2National, meetingActivityFilterProd2National.ToList(), prodID, "meetingactivity", 5);
                        cr.ProcessGrid(grid2National, meetingActivityFilterProd2National);
                    }
                }
            }
            if (productId.Count() == 3)
            {
                queryValues["Products_Discussed_ID"] = productId[2];
                meetingActivityFilterProd3 = Pinsonault.Data.Generic.CreateGenericEntityQuery<PathfinderClientModel.ContactReportDataSummary>(clientContext, new QueryDefinition(queryValues)).ToList();
                if (meetingActivityFilterProd3.Count() > 0)
                {
                    ccrChart1.FindControl("chartProduct3").Visible = true;
                    ccrMeetingActivityData1.FindControl("dataProduct3").Visible = true;
                    prodID = Convert.ToInt32(productId[2]);
                    cr.ProcessChart(chart3, meetingActivityFilterProd3.ToList(), prodID, "meetingactivity", 3);
                    cr.ProcessGrid(grid3, meetingActivityFilterProd3);
                }

                //Get National data if only Region/State was selected for comparison chart
                if (string.IsNullOrEmpty(Request.QueryString["Is_National"]))
                {
                    NameValueCollection queryValueNational = new NameValueCollection(queryValues);

                    queryValueNational.Remove("Geography_ID");
                    queryValueNational.Add("Is_National", "1");

                    meetingActivityFilterProd3National = Pinsonault.Data.Generic.CreateGenericEntityQuery<PathfinderClientModel.ContactReportDataSummary>(clientContext, new QueryDefinition(queryValueNational)).ToList();

                    if (meetingActivityFilterProd3National.Count() > 0)
                    {
                        ccrChart1.FindControl("chartProduct3National").Visible = true;
                        ccrMeetingActivityData1.FindControl("dataProduct3National").Visible = true;
                        prodID = Convert.ToInt32(productId[2]);
                        cr.ProcessChart(chart3National, meetingActivityFilterProd3National.ToList(), prodID, "meetingactivity", 6);
                        cr.ProcessGrid(grid3National, meetingActivityFilterProd3National);
                    }
                }
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
