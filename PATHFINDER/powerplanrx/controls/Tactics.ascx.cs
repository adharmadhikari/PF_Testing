using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;


public partial class controls_Tactics : System.Web.UI.UserControl
{
    protected override void OnInit(EventArgs e)
    {
        dsTacticsSelectedReadOnly.ConnectionString = Pinsonault.Web.Session.ClientDBConnectionString;
        base.OnInit(e);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }
    public Telerik.Web.UI.RadGrid TacticGrid
    {
        get { return rgTacticsReadOnly; }
    }
}
