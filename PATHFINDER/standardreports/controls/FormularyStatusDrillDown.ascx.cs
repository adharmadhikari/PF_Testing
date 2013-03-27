using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class standardreports_controls_FormularyStatusDrillDown : System.Web.UI.UserControl
{
    protected override void OnLoad(EventArgs e)
    {
        gridformularystatusdrilldown.Columns.FindByUniqueName("Plan_Name").HeaderStyle.Width = new Unit("25%");
        gridformularystatusdrilldown.Columns.FindByUniqueName("Formulary_Name").HeaderStyle.Width = new Unit("17%");

        base.OnLoad(e);
    }
}
