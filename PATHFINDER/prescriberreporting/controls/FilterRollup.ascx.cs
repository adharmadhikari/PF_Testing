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

public partial class prescriberreporting_controls_FilterRollup : UserControl, IFilterControl 
{
    public prescriberreporting_controls_FilterRollup()
    {
        ContainerID = "moduleOptionsContainer";
    }

    public string ContainerID { get; set; }

    protected override void OnLoad(EventArgs e)
    {

        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Rollup_Type.ClientID, null, ContainerID);
        
        
        //Page.ClientScript.RegisterClientScriptBlock(typeof(standardreports_controls_FilterGeography), "_geographyDefault", string.Format("var _geographyDefault='{0}';", DefaultValue), true);
        string frag = "var comboSectionID = $find('ctl00_partialPage_filtersContainer_Channel_Section_ID'); if (comboSectionID.get_value() == -1){comboSectionID.findItemByValue(1).select();c.control.set_visible(false);}";
        string fragReset = "var plan_id = $get('Plan_ID'); if (plan_id && plan_id.control){plan_id.control.reset();}";
        //Geography_ID.OnClientLoad = "function(s,a){ var l=s.get_element(); var gt = $find('" + rcbGeographyType.ClientID + "').get_value(); " + frag + "}";

        ////////Rollup_Type.OnClientLoad = "function(s,a) { var c=$find('ctl00_partialPage_filtersContainer_Channel_Account_Name'); var data = clientManager.get_SelectionData(); if(data && data[\"Rollup_Type\"] && data[\"Rollup_Type\"].value == \"4\" ){ c.control.set_visible(true); }else{ c.control.set_visible(false); }}";
        Rollup_Type.OnClientLoad = "function(s,a) {var data = clientManager.get_SelectionData(); if (data && data['Rollup_Type'] && data['Rollup_Type'].value == '4' )s.findItemByValue(4).select();else s.findItemByValue(1).select();}";
        ////////Rollup_Type.OnClientSelectedIndexChanged = "function(s, a){var c=$find('ctl00_partialPage_filtersContainer_Channel_Account_Name'); var l=$get('" + Rollup_Type.ClientID + "'); if(!l || !l.control) return; var gt = a.get_item().get_value(); " + frag + " reportFiltersResize(); }";
        Rollup_Type.OnClientSelectedIndexChanged = "function(s, a){var c = $get('ctl00_partialPage_filtersContainer_Channel_Plan_ID');var l = $get('ctl00_partialPage_filtersContainer_Rollup_Rollup_Type');$('#ctl00_partialPage_filtersContainer_Channel_validator1').attr('_required', 'false');if(!l || !l.control || !c || !c.control)return;" + fragReset + "var gt = a.get_item().get_value();switch (parseInt(gt, 10)) {case 2: c.control.set_visible(false); $('#ctl00_partialPage_filtersContainer_Channel_validator1').attr('_required', 'false');" + frag + " break; case 3: c.control.set_visible(false); $('#ctl00_partialPage_filtersContainer_Channel_validator1').attr('_required', 'false');" + frag + " break; case 4: var plan_id = $get('Plan_ID'); if (plan_id.control){plan_id.control.reset();$updateCheckboxDropdownText('ctl00_partialPage_filtersContainer_Channel_Plan_ID', 'Plan_ID');} c.control.set_visible(true);$('#ctl00_partialPage_filtersContainer_Channel_validator1').attr('_required', 'true');break;default:c.control.set_visible(false);$('#ctl00_partialPage_filtersContainer_Channel_validator1').attr('_required', 'false');break; } reportFiltersResize();}";

        //Region item only visible for some clients
        //RadComboBoxItem item = rcbGeographyType.Items.FindItemByValue("2");
        //if ( item != null )
        //{
        //    item.Visible = Context.User.IsInRole("sr_rgns");
        //}

        base.OnLoad(e);
    }

    #region IFilterControl Members

    string _defaultValue; 
    public string DefaultValue 
    { 
        get
        {
            if ( _defaultValue == null ) 
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
