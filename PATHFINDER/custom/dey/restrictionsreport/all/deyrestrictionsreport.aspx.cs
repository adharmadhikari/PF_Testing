using System;
using PathfinderClientModel;
using Pinsonault.Application.StandardReports;
using Pinsonault.Application.Dey;

public partial class restrictionsreport_all_restrictionsreport : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PathfinderDeyEntities ctx = new PathfinderDeyEntities();
        ReportPageLoader.LoadReport<RestrictionsReportSummary, RestrictionsReportChartQueryDefinition>(Request.QueryString, RestrictionsReportChart1, null, true, true, ctx);
        ReportPageLoader.LoadReport<RestrictionsReportSummary, RestrictionsReportQueryDefinition>(Request.QueryString, null, RestrictionsReportData1, true, true, ctx);
    }
}

