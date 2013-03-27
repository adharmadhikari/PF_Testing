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
using Pinsonault.Web;

public partial class custom_pinso_formularyhistoryreporting_controls_FilterGeography : UserControl, IFilterControl
{
    public custom_pinso_formularyhistoryreporting_controls_FilterGeography()
    {
        ContainerID = "moduleOptionsContainer";
    }

    public string ContainerID { get; set; }

    protected override void OnLoad(EventArgs e)
    {

        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, rcbGeographyType.ClientID, null, ContainerID);
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Geography_ID.ClientID, null, ContainerID);

        //test starts

        Geography_ID.OnClientLoad = "function(s, a){GeoLoad(s, a)}";
        rcbGeographyType.OnClientLoad = "function(s,a) {GeoTypeLoad(s,a)}";//var gType = s.get_element(); var data = clientManager.get_SelectionData();if(data && data[\"Geography_ID\"] && data[\"Geography_ID\"].value != \"US\"){if(clientManager.get_States()[data[\"Geography_ID\"].value]){s.findItemByValue(3).select();}else{if(s.findItemByValue(2)){s.findItemByValue(2).select();}}}else {s.findItemByValue(1).select();}}";
        rcbGeographyType.OnClientSelectedIndexChanged = "function(s, a){geoSelectedIndexChanged (s, a)}";

        //test ends       
        //string frag = "var regCtrl =$get('" + Geography_ID.ClientID + "');if(!regCtrl || !regCtrl.control) return;if (gt && gt == '2'){$loadListItems(l, clientManager.get_RegionListOptions());l.control.set_visible(true);regCtrl.control.set_visible(true);}if (gt && gt == '3'){$loadListItems(l, clientManager.get_States());l.control.set_visible(true);regCtrl.control.set_visible(true);}";

        //Geography_ID.OnClientLoad = "function(s,a){ var l=s.get_element(); var gt = $find('" + rcbGeographyType.ClientID + "').get_value(); " + frag + "}";

        ////if selected channel is managed medicaid, sectionid = 6 , or if selected Class_Partition = 2 ;then enable the Geography Dropdown, else disable this dropdown and make national selected by default.
        //rcbGeographyType.OnClientLoad = "function(s,a) { var gType = s.get_element(); var data = clientManager.get_SelectionData();if(data && data[\"Geography_ID\"] && data[\"Geography_ID\"].value != \"US\"){if(clientManager.get_States()[data[\"Geography_ID\"].value]){s.findItemByValue(3).select();}else{if(s.findItemByValue(2)){s.findItemByValue(2).select();}}}else {s.findItemByValue(1).select();}}";


        //rcbGeographyType.OnClientSelectedIndexChanged = "function(s, a){var l=$get('" + Geography_ID.ClientID + "'); var gt = a.get_item().get_value(); " + frag + " reportFiltersResize(); }";

        //Region item only visible for some clients
        RadComboBoxItem item = rcbGeographyType.Items.FindItemByValue("2");
        if (item != null)
        {
            item.Visible = Context.User.IsInRole("sr_rgns");
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
