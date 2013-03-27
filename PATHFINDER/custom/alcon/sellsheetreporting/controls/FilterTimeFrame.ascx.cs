using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PathfinderModel;
using Pinsonault.Web;

public partial class custom_controls_FilterTimeFrame : System.Web.UI.UserControl
{
  protected void Page_Load(object sender, EventArgs e)
    {
        user_searchlist.ServiceUrl = string.Format("custom/{0}/sellsheetreporting/services/AlconService.svc/SellSheetGetUserSet", Pinsonault.Web.Session.ClientKey);

        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, rcbGeographyType.ClientID, null, "moduleOptionsContainer");
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Geography_ID.ClientID, null, "moduleOptionsContainer");
        //Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Plan_ID.ClientID, null, "moduleOptionsContainer"); 
    }
    protected override void OnLoad(EventArgs e)
    {
        using (PathfinderEntities pfentity = new PathfinderEntities())
        {
            int clientID = Pinsonault.Web.Session.ClientID;
            //register values of states in a script variable and use it in java script
            //states
            var sellsheetstates = (from c in pfentity.StateSet                                 
                                orderby c.Name
                                select c).ToList().Select(s => new GenericListItem { ID = s.ID.ToString(), Name = s.Name.ToString() });

            if (sellsheetstates != null)
            {
                Pinsonault.Web.Support.RegisterGenericListVariable(this.Page, sellsheetstates.ToArray(), "sellsheetstatesList");
            }                     
        }
        //java script code is in alcon.js and filter scripts
        Geography_ID.OnClientLoad = "function(s, a){salesGeoIDLoad(s, a)}";
        rcbGeographyType.OnClientLoad = "function(s,a) {SalesGeoTypeLoad(s,a)}";
        rcbGeographyType.OnClientSelectedIndexChanged = "function(s, a){salesgeoType_SelectedIndexChanged (s, a)}";
        //Plan_ID.OnClientLoad = "function(s,a){PlanIDLoad(s, a)}";
        base.OnLoad(e);
    }
}
