using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PathfinderModel;
public partial class standardreports_controls_FDrilldownDataMedD : System.Web.UI.UserControl
{
    protected override void OnLoad(EventArgs e)
    {
        gridFMedD.Columns.FindByUniqueName("Plan_Name").HeaderStyle.Width = new Unit("16%");
        base.OnLoad(e);
    }


}
