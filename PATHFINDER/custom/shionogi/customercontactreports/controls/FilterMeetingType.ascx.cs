using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Pinsonault.Web;

public partial class custom_controls_FilterMeetingType : System.Web.UI.UserControl
{
    protected override void OnInit(EventArgs e)
    {
        dsMeetType.ConnectionString = Pinsonault.Web.Session.ClientConnectionString;

        base.OnInit(e);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        Support.RegisterComponentWithClientManager(Page, Meeting_Type_ID.ClientID, null, "moduleOptionsContainer");
    }
}
