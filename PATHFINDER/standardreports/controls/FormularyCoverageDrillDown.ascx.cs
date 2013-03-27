using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class standardreports_controls_FormularyCoverageDrillDown : System.Web.UI.UserControl
{
    protected override void OnLoad(EventArgs e)
    {
        gridformularycoveragedrilldown.Columns.FindByUniqueName("Plan_Name").HeaderStyle.Width = new Unit("23%");
        gridformularycoveragedrilldown.Columns.FindByUniqueName("Formulary_Name").HeaderStyle.Width = new Unit("15%");

        base.OnLoad(e);
    }

}
