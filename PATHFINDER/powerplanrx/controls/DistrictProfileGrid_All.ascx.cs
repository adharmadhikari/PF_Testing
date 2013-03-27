using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class controls_DistrictProfileGrid_All : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        dsCommercial.ConnectionString = Pinsonault.Web.Session.ClientDBConnectionString;
        dsPartD.ConnectionString = Pinsonault.Web.Session.ClientDBConnectionString;
        dsOther.ConnectionString = Pinsonault.Web.Session.ClientDBConnectionString;
    }

}
