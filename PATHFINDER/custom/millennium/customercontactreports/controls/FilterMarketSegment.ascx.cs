using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PathfinderModel;
using Pinsonault.Web;
using Pinsonault.Application.Millennium;

public partial class custom_controls_FilterMarketSegment : System.Web.UI.UserControl
{
    protected override void OnPreRender(EventArgs e)
    {  
        //creates the drop down list for Market Segment
        using (PathfinderEntities context = new PathfinderEntities())
        {
            //am - changed sort order 
            //client has single select section
            int clientID = Pinsonault.Web.Session.ClientID;
            //int applicationID = Identifiers.TodaysAccounts;
            int applicationID = Identifiers.CustomerContactReports;

            var q = (from c in context.ClientApplicationAccessSet
                     join s in context.SectionSet on
                     c.SectionID equals s.ID
                     where c.ClientID == clientID
                     where c.ApplicationID == applicationID
                     where s.ID != 99
                     orderby s.Sort_Order
                     select s).ToList().Select(p => new GenericListItem { ID = p.ID.ToString(), Name = p.Name });

            if (q != null)
            {
                //List<GenericListItem> list = q.ToList();
                //list.Insert(0, new GenericListItem { ID = "0", Name = "All" });
                Pinsonault.Web.Support.RegisterGenericListVariable(this.Page, q.ToArray(), "marketSegment");
            }
        }
        base.OnPreRender(e);
    }


    protected override void OnLoad(EventArgs e)
    {
        Pinsonault.Web.Support.RegisterTierScriptVariable(this.Page);
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Section_ID.ClientID, null, "moduleOptionsContainer");
        Section_ID.OnClientLoad = "function(s,a){$loadListItems($find('" + Section_ID.ClientID + "'), marketSegment,null, marketSegment.length>1 ? -1 : marketSegment[0].ID);}";
    }

}
