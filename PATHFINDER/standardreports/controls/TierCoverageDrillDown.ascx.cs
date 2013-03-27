using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class standardreports_controls_TierCoverageDrillDown : System.Web.UI.UserControl
{
    protected override void OnLoad(EventArgs e)
    {
        gridtiercoveragedrilldown.Columns.FindByUniqueName("Plan_Name").HeaderStyle.Width = new Unit("28%");
        gridtiercoveragedrilldown.Columns.FindByUniqueName("Formulary_Name").HeaderStyle.Width = new Unit("18%");

        base.OnLoad(e);
    }
}
