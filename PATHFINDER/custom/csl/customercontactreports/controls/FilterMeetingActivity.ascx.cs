using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PathfinderClientModel;

public partial class custom_controls_FilterMeetingActivity : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Meeting_Activity_ID.ClientID, null, "moduleOptionsContainer");
        using (PathfinderClientEntities context = new PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
        {
            Meeting_Activity_ID.DataSource = context.LkpMeetingActivitySet;
            Meeting_Activity_ID.DataBind();
            
        }
    }
}
