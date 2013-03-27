using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PathfinderModel;
public partial class standardreports_controls_FDrilldownDataVA : System.Web.UI.UserControl
{
    protected override void OnLoad(EventArgs e)
    {
        gridFVA.Columns.FindByUniqueName("Plan_Name").HeaderStyle.Width = new Unit("27%");
                
        base.OnLoad(e);
    }


}
