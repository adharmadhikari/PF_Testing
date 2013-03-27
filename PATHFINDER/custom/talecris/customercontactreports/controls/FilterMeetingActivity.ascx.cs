using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Pinsonault.Web;

public partial class custom_controls_FilterMeetingActivity : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
    
    Support.RegisterComponentWithClientManager(Page, Meeting_Activity_ID.ClientID, null, "moduleOptionsContainer");
    }
}
