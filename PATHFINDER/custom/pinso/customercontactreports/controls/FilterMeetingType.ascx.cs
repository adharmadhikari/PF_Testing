using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PathfinderClientModel;

public partial class custom_controls_FilterMeetingType : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Meeting_Type_ID.ClientID, null, "moduleOptionsContainer");
        using (PathfinderClientEntities context = new PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
        {
           Meeting_Type_ID.DataSource = context.LkpMeetingTypeSet;          
           Meeting_Type_ID.DataBind();           
        }
    }    
}
