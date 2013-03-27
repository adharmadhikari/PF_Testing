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
using Pinsonault.Application.Alcon;
using Pinsonault.Data.Reports;
using PathfinderClientModel;

public partial class custom_alcon_activityreporting_all_activityreporting : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        NameValueCollection queryValues = new NameValueCollection(Request.QueryString);

        using (PathfinderAlconEntities clientContext = new PathfinderAlconEntities())
        {
            IList<DbDataRecord> activityTypeFilter = null;

            ActivityReportingQueryDef qd;

            qd = new ActivityReportingQueryDef("ActivityReportingDetails", queryValues);
            activityTypeFilter = (qd.CreateQuery(clientContext) as IEnumerable<DbDataRecord>).ToList();

            if (activityTypeFilter.Count() > 0)
            {
                ProcessChart(activityReportingChart1.FindControl("chartType1").FindControl("chart") as Chart, activityTypeFilter.ToList());
                ProcessGrid(activityReportingData1.FindControl("dataType1").FindControl("gridActivityType") as RadGrid, activityTypeFilter);
            }
            else
            {
                Chart c = (Chart)activityReportingChart1.FindControl("chartType1").FindControl("chart");
                c.Visible = false;
            }

        }
    }

    public void ProcessChart(Chart chart, List<DbDataRecord> activityTypeFilter)
    {
        NameValueCollection queryValues = new NameValueCollection(Request.QueryString);

        //string geography = string.Empty;
        //string accountManager = string.Empty;

        //using (PathfinderClientEntities clientContext = new PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
        //{
        //    if (!string.IsNullOrEmpty(queryValues["Territory_ID"]))
        //    {
        //        var geo = clientContext.TerritorySet.Where(t => t.ID == queryValues["Territory_ID"]).Select(t => t.Name).FirstOrDefault();
        //        geography = geo.ToString();                
        //    }
        //    if (!string.IsNullOrEmpty(queryValues["User_ID"]))
        //    {
        //        Int32 uid = Convert.ToInt32(queryValues["User_ID"]);
        //        var mgr = clientContext.AccountManagerSet.Where(t => t.User_ID == uid).Select(t => t.FullName).FirstOrDefault();
        //        accountManager = mgr.ToString();
        //    }
        //}
        //Commented out for now because multiple selections would make the title too big
        //chart.Titles[0].Text = string.Format("Activity Summary - {0} ({1})", string.IsNullOrEmpty(geography) ? "All Geographies" : geography, string.IsNullOrEmpty(accountManager) ? "All Acct. Mgrs." : accountManager);
        //chart.Attributes["_title"] = string.Format("Activity Summary - {0} ({1})", string.IsNullOrEmpty(geography) ? "All Geographies" : geography, string.IsNullOrEmpty(accountManager) ? "All Acct. Mgrs." : accountManager);

        chart.Titles[0].Text = "Activity Summary";
        chart.Attributes["_title"] = "Activity Summary";

        int noColumn = 0;
        int index = 0;

        foreach (var y in activityTypeFilter)
        {
            //Locate index of all fields by ordinals
            int mtgIDOrdinal = 0;
            int mtgNameOrdinal = 0;
            int activityOrdinal = y.GetOrdinal("Activity_Type_Name");
            string activityTypeText = y.GetValue(activityOrdinal).ToString();

            mtgIDOrdinal = y.GetOrdinal("Activity_Type_ID");
            mtgNameOrdinal = y.GetOrdinal("Activity_Type_Name");

            int numOfHoursOrdinal = y.GetOrdinal("Activity_Hours");
            int percentOfCallsOrdinal = y.GetOrdinal("Activity_Hours_Percent");

            chart.PaletteCustomColors = new System.Drawing.Color[activityTypeFilter.Count()];

            //chart.Series[index].Type = SeriesChartType.Bar;
            chart.Series[index].ShowInLegend = true;
            chart.Series[index].Points.AddY(y.GetValue(numOfHoursOrdinal));
            chart.Series[index].Points[noColumn].AxisLabel = " ";// activityTypeText;
            chart.Series[index].Points[noColumn].Color = Pinsonault.Data.Reports.ReportColors.CustomerContactReports.GetColor(Convert.ToInt32(y.GetValue(mtgIDOrdinal)) - 1);

            chart.Series[index].Points[noColumn].Href = string.Format("javascript:gridDrilldown({0},{1})", y.GetValue(mtgIDOrdinal), noColumn);

            noColumn++;
        }
    }

    public void ProcessGrid(Telerik.Web.UI.RadGrid grid, IList<DbDataRecord> activityTypeFilter)
    {
        grid.MasterTableView.DataSource = activityTypeFilter;
        grid.MasterTableView.DataBind();

    }
}