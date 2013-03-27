using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PathfinderModel;

public partial class marketplaceanalytics_controls_FilterChannelPlans : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        using (PathfinderEntities context = new PathfinderEntities())
        {
            int clientID = Pinsonault.Web.Session.ClientID;

            //exclude Section_ID 8 = Other and 14 = Employer and 0

            var channels = from c in context.ClientApplicationAccessSet
                           join s in context.SectionSet on
                           c.SectionID equals s.ID
                           where c.ClientID == clientID
                           where c.ApplicationID == 2
                           where c.SectionID != 0
                           where c.SectionID != 8
                           where c.SectionID != 14
                           select new
                           {
                               s.ID,
                               s.Name
                           };

            Section_ID.DataSource = channels.OrderBy(s => s.Name);
            Section_ID.DataBind();
        }


        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Section_ID.ClientID, null, "moduleOptionsContainer");
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Plan_ID1.ClientID, null, "moduleOptionsContainer");
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Plan_ID2.ClientID, null, "moduleOptionsContainer");
                
        Section_ID.OnClientLoad = "function(s,a) { if(!clientManager.get_SelectionData()){ SectionLoad(s,a);}}";
        Section_ID.OnClientSelectedIndexChanged = "function(s,a){SectionLoad(s,a);}";
        Section_ID.OnClientDropDownClosing = "function(s,a){SectionReset(s,a)}";
    }
}
