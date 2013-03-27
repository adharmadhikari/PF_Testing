using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PathfinderModel;

public partial class prescriberreporting_controls_FilterChannel : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        using (PathfinderEntities context = new PathfinderEntities())
        {
            int clientID = Pinsonault.Web.Session.ClientID;

            var channels = from c in context.ClientApplicationAccessSet
                           join s in context.SectionSet on
                           c.SectionID equals s.ID
                           where c.ClientID == clientID
                           where c.ApplicationID == 15
                           where c.SectionID != 0
                           select new
                           {
                               s.ID,
                               s.Name
                           };

            Section_ID.DataSource = channels.OrderBy(s => s.Name);
            Section_ID.DataBind();
        }


        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Section_ID.ClientID, null, "moduleOptionsContainer"); 
    }
}
