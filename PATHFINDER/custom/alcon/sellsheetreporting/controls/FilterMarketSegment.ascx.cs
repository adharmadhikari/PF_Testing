using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PathfinderModel;
using PathfinderClientModel;
using Pinsonault.Web;
using Telerik.Web.UI;
using Pinsonault.Web.UI;

public partial class custom_controls_FilterMarketSegment : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Section_ID.ClientID, null, "moduleOptionsContainer");
        //Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Title_ID.ClientID, null, "moduleOptionsContainer");

        using (PathfinderEntities context = new PathfinderEntities())
        {
            int clientID = Pinsonault.Web.Session.ClientID;
            int applicationID = Identifiers.SellSheetReporting;
          
            //get the list of Sections for a selected client for App_id = 17 - sell sheet reporting

            var channels = (from c in context.ClientApplicationAccessSet
                           join s in context.SectionSet on
                           c.SectionID equals s.ID
                           where c.ClientID == clientID
                           where c.ApplicationID == applicationID 
                           where s.ID != 0
                           orderby s.Sort_Order
                           select s).ToList().Select(s => new GenericListItem { ID = s.ID.ToString(), Name = s.Name.ToString() });
                          
            if (channels != null)
            {
                Pinsonault.Web.Support.RegisterGenericListVariable(this.Page, channels.ToArray(), "channelsList");
            }
            Section_ID.OnClientLoad = "function(s,a){get_SectionLoad(s,a)}";
            //Section_ID.SelectedIndex = 0;
            //Load  titles
          
            //var titles = (from us in context.UserSet
            //         where us.Client.Client_ID == clientID
            //         where us.Title != null 
            //         orderby us.Title
            //              select us.Title).Distinct().ToList().Select(us => new GenericListItem { ID = us, Name = us });
            //if (titles != null)
            //{
            //    Pinsonault.Web.Support.RegisterGenericListVariable(this.Page, titles.ToArray(), "titlesList");
            //}

            //Title_ID.OnClientLoad = "function(s,a){ get_TitleLoad(s,a);}";
        }
   }
}
