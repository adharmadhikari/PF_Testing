using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PathfinderModel;
public partial class standardreports_controls_FDrilldownDataComm : System.Web.UI.UserControl
{
    //public bool ShowProductName { get; set; } 
    protected override void OnLoad(EventArgs e)
    {
        string val = Request.QueryString["Section_ID"];
        
        gridFComm.Columns.FindByUniqueName("Plan_Name").HeaderStyle.Width = new Unit("23%");
        base.OnLoad(e);
    }

    

  
}
