using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.Serialization.Json;
using System.IO;

public partial class custom_controls_FilterRegion : System.Web.UI.UserControl
{
    public custom_controls_FilterRegion()
    {
        ContainerID = "moduleOptionsContainer";
    }
    public string ContainerID { get; set; }
    protected override void OnLoad(EventArgs e)
    {
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, rcbGeographyType.ClientID, null, ContainerID);
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Geography_ID.ClientID, null, ContainerID);

        string frag = "switch(parseInt(gt,10)) {case 1: $loadListItems(l,null,{value:'" + DefaultValue + "',text:'" + DefaultValue + "'});l.control.set_visible(false); break; case 2: $loadListItems(l, clientManager.get_RegionListOptions()); l.control.set_visible(true);break; case 3: $loadListItems(l, clientManager.get_States()); l.control.set_visible(true);break;}";

        Geography_ID.OnClientLoad = "function(s,a){ var l=s.get_element();   var gt = $find('" + rcbGeographyType.ClientID + "').get_value(); " + frag + "}";

        rcbGeographyType.OnClientLoad = "function(s,a) { var data = clientManager.get_SelectionData(); if(data && data[\"Geography_ID\"] && data[\"Geography_ID\"].value != \"US\"){ s.findItemByValue(clientManager.get_States()[data[\"Geography_ID\"].value] ? 3 : 2).select();}}";

        rcbGeographyType.OnClientSelectedIndexChanged = "function(s, a){var l=$get('" + Geography_ID.ClientID + "');  if(!l || !l.control) return; var gt = a.get_item().get_value(); " + frag + " reportFiltersResize(); }";

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
