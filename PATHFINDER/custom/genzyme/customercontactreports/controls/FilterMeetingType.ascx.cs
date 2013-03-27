using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Pinsonault.Application.Genzyme ;

public partial class custom_controls_FilterMeetingType : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Meeting_Type_ID.ClientID, null, "moduleOptionsContainer");
        using (PathfinderGenzymeEntities context = new PathfinderGenzymeEntities())
        {
           Meeting_Type_ID.DataSource = context.LkpMeetingTypeSet;          
           Meeting_Type_ID.DataBind();           
        }
    }    
}
