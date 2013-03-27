using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class controls_Messages : System.Web.UI.UserControl
{
    public Telerik.Web.UI.RadGrid MessageGrid
    {
        get { return rgMessageReadOnly; }
    }
     protected override void OnInit(EventArgs e)
    {
        Pinsonault.Web.Session.CheckSessionState();     
        dsMessageSelectedReadOnly.ConnectionString = Pinsonault.Web.Session.ClientDBConnectionString;
              
        base.OnInit(e);
    }

}
