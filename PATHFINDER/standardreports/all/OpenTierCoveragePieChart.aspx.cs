using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dundas.Charting.WebControl;
using Pinsonault.Application.StandardReports;


public partial class standardreports_all_OpenTierCoveragePieChart : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ReportPageLoader.LoadReport<PathfinderModel.ReportsTierCoverage, TierCoverageQueryDefinition>(Request.QueryString, TierCoveragePieChart1);
    }
}
