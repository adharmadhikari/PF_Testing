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
        GenzymeContactReportProvider cr = new GenzymeContactReportProvider();

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
        queryValues.Add("__select", "Meeting_Type_ID, Meeting_Type_Name, Products_Discussed_ID, Drug_Name");
        queryValues.Add("__sort", "Meeting_Type_Name");

        using (PathfinderGenzymeEntities clientContext = new PathfinderGenzymeEntities())
        {
            IList<DbDataRecord> meetingTypeFilterProd1 = null;
            IList<DbDataRecord> meetingTypeFilterProd1National = null;
            IList<DbDataRecord> meetingTypeFilterProd2 = null;
            IList<DbDataRecord> meetingTypeFilterProd2National = null;
            IList<DbDataRecord> meetingTypeFilterProd3 = null;
            IList<DbDataRecord> meetingTypeFilterProd3National = null;

            string[] productId = cr.GetCRProductsDiscussedID();
            int prodID = 0;

            //Create Entity Query based on number of products selected (max 3 products)
            //If 3 products, all statements execute
            //If 2 products, first 2 statements execute
            //If 1 product, first statement executes
            if (productId.Count() >= 1)
            {
                queryValues["Products_Discussed_ID"] = productId[0];
                meetingTypeFilterProd1 = Pinsonault.Data.Generic.CreateGenericEntityQuery<Pinsonault.Application.Genzyme.ContactReportDataSummary>(clientContext, new QueryDefinition(queryValues)).ToList();
                if (meetingTypeFilterProd1.Count() > 0)
                {
                    ccrChart1.FindControl("chartProduct1").Visible = true;
                    ccrMeetingTypeData1.FindControl("dataProduct1").Visible = true;
                    prodID = Convert.ToInt32(productId[0]);
                    cr.ProcessGenzymeChart(chart1, meetingTypeFilterProd1.ToList(), prodID, "meetingtype", 1);
                    cr.ProcessGenzymeGrid(grid1, meetingTypeFilterProd1);
                }

                //Get National data if only Region/State was selected for comparison chart
                if (string.IsNullOrEmpty(Request.QueryString["Is_National"]))
                {
                    compareContainer.Visible = true;

                    NameValueCollection queryValueNational = new NameValueCollection(queryValues);

                    queryValueNational.Remove("Geography_ID");
                    queryValueNational.Add("Is_National", "1");

                    meetingTypeFilterProd1National = Pinsonault.Data.Generic.CreateGenericEntityQuery<Pinsonault.Application.Genzyme.ContactReportDataSummary>(clientContext, new QueryDefinition(queryValueNational)).ToList();

                    if (meetingTypeFilterProd1National.Count() > 0)
                    {
                        ccrChart1.FindControl("chartProduct1National").Visible = true;
                        ccrMeetingTypeData1.FindControl("dataProduct1National").Visible = true;
                        prodID = Convert.ToInt32(productId[0]);
                        cr.ProcessGenzymeChart(chart1National, meetingTypeFilterProd1National.ToList(), prodID, "meetingtype", 4);
                        cr.ProcessGenzymeGrid(grid1National, meetingTypeFilterProd1National);
                    }
                }
            }
            if (productId.Count() >= 2)
            {
                queryValues["Products_Discussed_ID"] = productId[1];
                meetingTypeFilterProd2 = Pinsonault.Data.Generic.CreateGenericEntityQuery<Pinsonault.Application.Genzyme.ContactReportDataSummary>(clientContext, new QueryDefinition(queryValues)).ToList();
                if (meetingTypeFilterProd2.Count() > 0)
                {
                    ccrChart1.FindControl("chartProduct2").Visible = true;
                    ccrMeetingTypeData1.FindControl("dataProduct2").Visible = true;
                    prodID = Convert.ToInt32(productId[1]);
                    cr.ProcessGenzymeChart(chart2, meetingTypeFilterProd2.ToList(), prodID, "meetingtype", 2);
                    cr.ProcessGenzymeGrid(grid2, meetingTypeFilterProd2);
                }

                //Get National data if only Region/State was selected for comparison chart
                if (string.IsNullOrEmpty(Request.QueryString["Is_National"]))
                {
                    NameValueCollection queryValueNational = new NameValueCollection(queryValues);

                    queryValueNational.Remove("Geography_ID");
                    queryValueNational.Add("Is_National", "1");

                    meetingTypeFilterProd2National = Pinsonault.Data.Generic.CreateGenericEntityQuery<Pinsonault.Application.Genzyme.ContactReportDataSummary>(clientContext, new QueryDefinition(queryValueNational)).ToList();

                    if (meetingTypeFilterProd2National.Count() > 0)
                    {
                        ccrChart1.FindControl("chartProduct2National").Visible = true;
                        ccrMeetingTypeData1.FindControl("dataProduct2National").Visible = true;
                        prodID = Convert.ToInt32(productId[1]);
                        cr.ProcessGenzymeChart(chart2National, meetingTypeFilterProd2National.ToList(), prodID, "meetingtype", 5);
                        cr.ProcessGenzymeGrid(grid2National, meetingTypeFilterProd2National);
                    }
                }
            }
            if (productId.Count() == 3)
            {
                queryValues["Products_Discussed_ID"] = productId[2];
                meetingTypeFilterProd3 = Pinsonault.Data.Generic.CreateGenericEntityQuery<Pinsonault.Application.Genzyme.ContactReportDataSummary>(clientContext, new QueryDefinition(queryValues)).ToList();
                if (meetingTypeFilterProd3.Count() > 0)
                {
                    ccrChart1.FindControl("chartProduct3").Visible = true;
                    ccrMeetingTypeData1.FindControl("dataProduct3").Visible = true;
                    prodID = Convert.ToInt32(productId[2]);
                    cr.ProcessGenzymeChart(chart3, meetingTypeFilterProd3.ToList(), prodID, "meetingtype", 3);
                    cr.ProcessGenzymeGrid(grid3, meetingTypeFilterProd3);
                }

                //Get National data if only Region/State was selected for comparison chart
                if (string.IsNullOrEmpty(Request.QueryString["Is_National"]))
                {
                    NameValueCollection queryValueNational = new NameValueCollection(queryValues);

                    queryValueNational.Remove("Geography_ID");
                    queryValueNational.Add("Is_National", "1");

                    meetingTypeFilterProd3National = Pinsonault.Data.Generic.CreateGenericEntityQuery<Pinsonault.Application.Genzyme.ContactReportDataSummary>(clientContext, new QueryDefinition(queryValueNational)).ToList();

                    if (meetingTypeFilterProd3National.Count() > 0)
                    {
                        ccrChart1.FindControl("chartProduct3National").Visible = true;
                        ccrMeetingTypeData1.FindControl("dataProduct3National").Visible = true;
                        prodID = Convert.ToInt32(productId[2]);
                        cr.ProcessGenzymeChart(chart3National, meetingTypeFilterProd3National.ToList(), prodID, "meetingtype", 6);
                        cr.ProcessGenzymeGrid(grid3National, meetingTypeFilterProd3National);
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
