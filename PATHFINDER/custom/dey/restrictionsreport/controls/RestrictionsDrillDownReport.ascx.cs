using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class restrictionsreport_controls_MedicalPharmacyCoverageDrillDown : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
      

        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "_rrddpagevars", string.Format("var gridRRDrillDownID = '{0}';", gridRestrictionsReportdrilldownReport.ClientID), true);




    }
}
