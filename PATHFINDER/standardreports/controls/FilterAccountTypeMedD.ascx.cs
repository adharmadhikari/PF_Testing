using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class standardreports_controls_FilterAccountTypeMedD : System.Web.UI.UserControl
{
    public string ContainerID { get; set; }

    public standardreports_controls_FilterAccountTypeMedD()
    {

        ContainerID = "moduleOptionsContainer";

    }
    protected override void OnLoad(EventArgs e)
    {
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Class_Partition.ClientID, null, ContainerID);

        //string frag = "var comboGeoType = $find('ctl00_partialPage_filtersContainer_Geography_rcbGeographyType'); if (!comboGeoType) return; if(s.get_value() == 1){if(comboGeoType.findItemByValue(2)){comboGeoType.findItemByValue(2).set_visible(false);}comboGeoType.findItemByValue(3).set_visible(false);comboGeoType.findItemByValue(1).select();comboGeoType.disable();}else{comboGeoType.enable();if(comboGeoType.findItemByValue(2)){comboGeoType.findItemByValue(2).set_visible(true);}comboGeoType.findItemByValue(3).set_visible(true);}";
        
        //SetGeography(s,a) function is present in filtercontainerscript.ascx file
        string frag = "SetGeography(s,a)";

        Class_Partition.OnClientLoad = "function(s,a){" + frag + "}";
        Class_Partition.OnClientSelectedIndexChanged = "function(s,a){" + frag + "}";

        base.OnLoad(e);
    }
}
