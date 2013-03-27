using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.Serialization.Json;
using System.IO;
using Telerik.Web.UI;
using Pinsonault.Web.UI;
using PathfinderClientModel;
using PathfinderModel;
using Pinsonault.Web;

public partial class standardreports_controls_FilterGeographyMedD : UserControl, IFilterControl
{
    public standardreports_controls_FilterGeographyMedD()
    {
        ContainerID = "moduleOptionsContainer";
    }

    public string ContainerID { get; set; }

    protected override void OnLoad(EventArgs e)
    {

        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, rcbGeographyType.ClientID, null, ContainerID);
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Geography_ID.ClientID, null, ContainerID);
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, MA_Region_ID.ClientID, null, ContainerID);
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Region_ID.ClientID, null, ContainerID);
        bool bAccountManagerVisible = false;
        if (!Page.IsPostBack)
        {
            //Get available territories for all account managers
            using (PathfinderClientEntities context = new PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
            {
                var q = (from p in context.AccountManagersByTerritorySet
                         orderby p.User_F_Name
                         orderby p.User_L_Name
                         select p).ToList().Select(p => new GenericListItem { ID = p.Territory_ID.ToString(), Name = p.FullName });
                if (q != null)
                {
                    Pinsonault.Web.Support.RegisterGenericListVariable(this.Page, q.ToArray(), "accountmanagerterritories");
                    if (q.Count() > 0) bAccountManagerVisible = true;
                }
            }

            //Get available MA Regions
            using (PathfinderEntities context = new PathfinderEntities())
            {
                var q = (from p in context.MARegionsSet 
                         orderby p.Region_ID                         
                         select p).ToList().Select(p => new GenericListItem { ID = p.Region_ID.ToString(), Name = p.Region_Name });
                if (q != null)
                {
                    Pinsonault.Web.Support.RegisterGenericListVariable(this.Page, q.ToArray(), "maregions");
                }
            }

            //Get available PDP Regions
            using (PathfinderEntities context = new PathfinderEntities())
            {
                var q = (from p in context.PDPRegionsSet
                         orderby p.Region_ID
                         select p).ToList().Select(p => new GenericListItem { ID = p.Region_ID.ToString(), Name = p.Region_Name });
                if (q != null)
                {
                    Pinsonault.Web.Support.RegisterGenericListVariable(this.Page, q.ToArray(), "pdpregions");
                }
            }
        }
        
        Geography_ID.OnClientLoad = "function(s, a){GeoLoad(s, a)}";//"function(s,a){ var l=s.get_element(); var gt = $find('" + rcbGeographyType.ClientID + "').get_value(); " + frag + "}";

        //for MA and PDP regions

        MA_Region_ID.OnClientLoad = "function(s, a){MARegionLoad (s, a)}";
        Region_ID.OnClientLoad = "function(s, a){RegionLoad (s, a)}";

        //end of code for MA and PDP regions        
       
        //if selected channel is managed medicaid, sectionid = 6 , or if selected Class_Partition = 2 ;then enable the Geography Dropdown, else disable this dropdown and make national selected by default.        
        rcbGeographyType.OnClientLoad = "function(s,a) {var gType = s.get_element(); var data = clientManager.get_SelectionData();if(data && data[\"Geography_ID\"] && data[\"Geography_ID\"].value != \"US\"){if(clientManager.get_States()[data[\"Geography_ID\"].value]){s.findItemByValue(3).select();}else{ var isAccountManager = false; for (var i = 0; i < accountmanagerterritories.length; i++){if (data[\"Geography_ID\"].value == accountmanagerterritories[i].ID) isAccountManager = true;} if(isAccountManager){s.findItemByValue(4).select();} else{if(s.findItemByValue(2)){s.findItemByValue(2).select();}}}}else {s.findItemByValue(1).select();} if((data && data[\"Class_Partition\"] && data[\"Class_Partition\"].value == \"2\") || clientManager.get_EffectiveChannel() == 6 ) {gType.control.enable();gType.control.findItemByValue(5).set_visible(false);gType.control.findItemByValue(6).set_visible(false);if(gType.control.findItemByValue(2)){gType.control.findItemByValue(2).set_visible(true);}gType.control.findItemByValue(3).set_visible(true);}else{gType.control.disable();gType.control.findItemByValue(1).select();if(gType.control.findItemByValue(2)){gType.control.findItemByValue(2).set_visible(false);}gType.control.findItemByValue(3).set_visible(false);}}";
       
        rcbGeographyType.OnClientSelectedIndexChanged = "function(s, a){geoSelectedIndexChanged (s, a)}";

        //Region item only visible for some clients
        RadComboBoxItem item = rcbGeographyType.Items.FindItemByValue("2");
        
        if (item != null)
        {
            item.Visible = Context.User.IsInRole("sr_rgns");
        }

        //account manager visible only if data is present       
        RadComboBoxItem itemAM = rcbGeographyType.Items.FindItemByValue("4");
        if (itemAM != null)
        {
            itemAM.Visible = bAccountManagerVisible;
        }
        base.OnLoad(e);
    }

    #region IFilterControl Members

    string _defaultValue;
    public string DefaultValue
    {
        get
        {
            if (_defaultValue == null)
                return string.Empty;

            return _defaultValue;
        }
        set
        {
            _defaultValue = value;
        }
    }

    #endregion
}
