﻿using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Pinsonault.Application.StandardReports;

public partial class standardreports_controls_FormularyStatusChartImage : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //ReportPageLoader.LoadReport<PathfinderClientModel.ReportsFormularyStatus, FormularyStatusQueryDefinition>(Request.QueryString, formularystatuschart1);
        ReportPageLoader.LoadReport<PathfinderModel.ReportsFormularyStatusSummary, FormularyStatusQueryDefinition>(Request.QueryString, formularystatuschart1);
    }
}
