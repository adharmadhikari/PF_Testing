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

public partial class custom_jazz_customercontactreports_all_meetingtype : PageBase
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
        ccrMeetingTypeData1.FindControl("dataProduct1").Visible = false;
        ccrMeetingTypeData1.FindControl("dataProduct1National").Visible = false;
        ccrMeetingTypeData1.FindControl("dataProduct2").Visible = false;
        ccrMeetingTypeData1.FindControl("dataProduct2National").Visible = false;
        ccrMeetingTypeData1.FindControl("dataProduct3").Visible = false;
        ccrMeetingTypeData1.FindControl("dataProduct3National").Visible = false;

        RadGrid grid1 = (RadGrid)ccrMeetingTypeData1.FindControl("dataProduct1").FindControl("gridCcrMeetingType");
        RadGrid grid1National = (RadGrid)ccrMeetingTypeData1.FindControl("dataProduct1National").FindControl("gridCcrMeetingType");
        RadGrid grid2 = (RadGrid)ccrMeetingTypeData1.FindControl("dataProduct2").FindControl("gridCcrMeetingType");
        RadGrid grid2National = (RadGrid)ccrMeetingTypeData1.FindControl("dataProduct2National").FindControl("gridCcrMeetingType");
        RadGrid grid3 = (RadGrid)ccrMeetingTypeData1.FindControl("dataProduct3").FindControl("gridCcrMeetingType");
        RadGrid grid3National = (RadGrid)ccrMeetingTypeData1.FindControl("dataProduct3National").FindControl("gridCcrMeetingType");

        //Instantiate helper class
        ContactReportProvider cr = new ContactReportProvider();

        NameValueCollection queryValues = new NameValueCollection(Request.QueryString);

        //Add aggregates to NameValueCollection
        queryValues.Add("__aggr", "Count(User_ID), PercentCount(User_ID)");
        queryValues.Add("__select", "Meeting_Type_ID, Meeting_Type_Name, Products_Discussed_ID, Drug_Name");
        queryValues.Add("__sort", "Meeting_Type_Name");

        using ( PathfinderClientModel.PathfinderClientEntities clientContext = new PathfinderClientModel.PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString) )
        {
            IList<DbDataRecord> meetingTypeFilterProd1 = null;
            IList<DbDataRecord> meetingTypeFilterProd1National = null;
            IList<DbDataRecord> meetingTypeFilterProd2 = null;
            IList<DbDataRecord> meetingTypeFilterProd2National = null;
            IList<DbDataRecord> meetingTypeFilterProd3 = null;
            IList<DbDataRecord> meetingTypeFilterProd3National = null;

            string[] productId = cr.GetProductsDiscussedID();
            int prodID = 0;

            //Create Entity Query based on number of products selected (max 3 products)
            //If 3 products, all statements execute
            //If 2 products, first 2 statements execute
            //If 1 product, first statement executes
            if (productId.Count() >= 1)
            {
                queryValues["Products_Discussed_ID"] = productId[0];
                meetingTypeFilterProd1 = Pinsonault.Data.Generic.CreateGenericEntityQuery<PathfinderClientModel.ContactReportDataSummary>(clientContext, new QueryDefinition(queryValues)).ToList();
                if (meetingTypeFilterProd1.Count() > 0)
                {
                    ccrChart1.FindControl("chartProduct1").Visible = true;
                    ccrMeetingTypeData1.FindControl("dataProduct1").Visible = true;
                    prodID = Convert.ToInt32(productId[0]);
                    cr.ProcessChart(chart1, meetingTypeFilterProd1.ToList(), prodID, "meetingtype", 1);
                    cr.ProcessGrid(grid1, meetingTypeFilterProd1);
                }

                //Get National data if only Region/State was selected for comparison chart
                if (string.IsNullOrEmpty(Request.QueryString["Is_National"]))
                {
                    compareContainer.Visible = true;

                    NameValueCollection queryValueNational = new NameValueCollection(queryValues);

                    queryValueNational.Remove("Geography_ID");
                    queryValueNational.Add("Is_National", "1");

                    meetingTypeFilterProd1National = Pinsonault.Data.Generic.CreateGenericEntityQuery<PathfinderClientModel.ContactReportDataSummary>(clientContext, new QueryDefinition(queryValueNational)).ToList();

                    if (meetingTypeFilterProd1National.Count() > 0)
                    {
                        ccrChart1.FindControl("chartProduct1National").Visible = true;
                        ccrMeetingTypeData1.FindControl("dataProduct1National").Visible = true;
                        prodID = Convert.ToInt32(productId[0]);
                        cr.ProcessChart(chart1National, meetingTypeFilterProd1National.ToList(), prodID, "meetingtype", 4);
                        cr.ProcessGrid(grid1National, meetingTypeFilterProd1National);
                    }
                }
            }
            if (productId.Count() >= 2)
            {
                queryValues["Products_Discussed_ID"] = productId[1];
                meetingTypeFilterProd2 = Pinsonault.Data.Generic.CreateGenericEntityQuery<PathfinderClientModel.ContactReportDataSummary>(clientContext, new QueryDefinition(queryValues)).ToList();
                if (meetingTypeFilterProd2.Count() > 0)
                {
                    ccrChart1.FindControl("chartProduct2").Visible = true;
                    ccrMeetingTypeData1.FindControl("dataProduct2").Visible = true;
                    prodID = Convert.ToInt32(productId[1]);
                    cr.ProcessChart(chart2, meetingTypeFilterProd2.ToList(), prodID, "meetingtype", 2);
                    cr.ProcessGrid(grid2, meetingTypeFilterProd2);
                }

                //Get National data if only Region/State was selected for comparison chart
                if (string.IsNullOrEmpty(Request.QueryString["Is_National"]))
                {
                    NameValueCollection queryValueNational = new NameValueCollection(queryValues);

                    queryValueNational.Remove("Geography_ID");
                    queryValueNational.Add("Is_National", "1");

                    meetingTypeFilterProd2National = Pinsonault.Data.Generic.CreateGenericEntityQuery<PathfinderClientModel.ContactReportDataSummary>(clientContext, new QueryDefinition(queryValueNational)).ToList();

                    if (meetingTypeFilterProd2National.Count() > 0)
                    {
                        ccrChart1.FindControl("chartProduct2National").Visible = true;
                        ccrMeetingTypeData1.FindControl("dataProduct2National").Visible = true;
                        prodID = Convert.ToInt32(productId[1]);
                        cr.ProcessChart(chart2National, meetingTypeFilterProd2National.ToList(), prodID, "meetingtype", 5);
                        cr.ProcessGrid(grid2National, meetingTypeFilterProd2National);
                    }
                }
            }
            if (productId.Count() == 3)
            {
                queryValues["Products_Discussed_ID"] = productId[2];
                meetingTypeFilterProd3 = Pinsonault.Data.Generic.CreateGenericEntityQuery<PathfinderClientModel.ContactReportDataSummary>(clientContext, new QueryDefinition(queryValues)).ToList();
                if (meetingTypeFilterProd3.Count() > 0)
                {
                    ccrChart1.FindControl("chartProduct3").Visible = true;
                    ccrMeetingTypeData1.FindControl("dataProduct3").Visible = true;
                    prodID = Convert.ToInt32(productId[2]);
                    cr.ProcessChart(chart3, meetingTypeFilterProd3.ToList(), prodID, "meetingtype", 3);
                    cr.ProcessGrid(grid3, meetingTypeFilterProd3);
                }

                //Get National data if only Region/State was selected for comparison chart
                if (string.IsNullOrEmpty(Request.QueryString["Is_National"]))
                {
                    NameValueCollection queryValueNational = new NameValueCollection(queryValues);

                    queryValueNational.Remove("Geography_ID");
                    queryValueNational.Add("Is_National", "1");

                    meetingTypeFilterProd3National = Pinsonault.Data.Generic.CreateGenericEntityQuery<PathfinderClientModel.ContactReportDataSummary>(clientContext, new QueryDefinition(queryValueNational)).ToList();

                    if (meetingTypeFilterProd3National.Count() > 0)
                    {
                        ccrChart1.FindControl("chartProduct3National").Visible = true;
                        ccrMeetingTypeData1.FindControl("dataProduct3National").Visible = true;
                        prodID = Convert.ToInt32(productId[2]);
                        cr.ProcessChart(chart3National, meetingTypeFilterProd3National.ToList(), prodID, "meetingtype", 6);
                        cr.ProcessGrid(grid3National, meetingTypeFilterProd3National);
                    }
                }
            }
        }
    }
}
