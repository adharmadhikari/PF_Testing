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

public partial class restrictionsreport_controls_FilterGeography : UserControl, IFilterControl
{
    public restrictionsreport_controls_FilterGeography()
    {
        ContainerID = "moduleOptionsContainer";
    }

    public string ContainerID { get; set; }

    protected override void OnLoad(EventArgs e)
    {

        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, rcbGeographyType.ClientID, null, ContainerID);
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Geography_ID.ClientID, null, ContainerID);

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
                }
            }
        }


        //Page.ClientScript.RegisterClientScriptBlock(typeof(standardreports_controls_FilterGeography), "_geographyDefault", string.Format("var _geographyDefault='{0}';", DefaultValue), true);
        //string frag = "switch(parseInt(gt,10)) 
        //{case 1: $loadListItems(l,null,{value:'" + DefaultValue + "',text:'" + DefaultValue + "'});l.control.set_visible(false); break; 
        //case 2: $loadListItems(l, clientManager.get_RegionListOptions()); l.control.set_visible(true);break; 
        //case 3: $loadListItems(l, clientManager.get_States()); l.control.set_visible(true);break;}";

        string frag = "var regCtrl =$get('" + Geography_ID.ClientID + "'); var reportTypeCtrl = $get('ctl00_partialPage_filtersContainer_ReportType_Rank'); if(!regCtrl || !regCtrl.control) return;if (gt && gt == '1'){$loadListItems(l,null,{value:'" + DefaultValue + "',text:'" + DefaultValue + "'});l.control.set_visible(false);regCtrl.control.set_visible(false);if(reportTypeCtrl && reportTypeCtrl.control){}}if (gt && gt == '2'){$loadListItems(l, clientManager.get_RegionListOptions());l.control.set_visible(true);regCtrl.control.set_visible(true);if(reportTypeCtrl && reportTypeCtrl.control){reportTypeCtrl.control.get_items().getItem(0).select();}}if (gt && gt == '3'){$loadListItems(l, clientManager.get_States());l.control.set_visible(true);regCtrl.control.set_visible(true);if(reportTypeCtrl && reportTypeCtrl.control){reportTypeCtrl.control.get_items().getItem(0).select();}}if (gt && gt == '4'){$loadListItems(l, accountmanagerterritories);l.control.set_visible(true);regCtrl.control.set_visible(true);if(reportTypeCtrl && reportTypeCtrl.control){reportTypeCtrl.control.get_items().getItem(0).select();}}";

        Geography_ID.OnClientLoad = "function(s,a){ var l=s.get_element(); var gt = $find('" + rcbGeographyType.ClientID + "').get_value(); " + frag + "}";

        //if selected channel is managed medicaid, sectionid = 6 , or if selected Class_Partition = 2 ;then enable the Geography Dropdown, else disable this dropdown and make national selected by default.
        //rcbGeographyType.OnClientLoad = "function(s,a) { var gType = s.get_element(); var data = clientManager.get_SelectionData();if(data && data[\"Geography_ID\"] && data[\"Geography_ID\"].value != \"US\"){if(clientManager.get_States()[data[\"Geography_ID\"].value]){s.findItemByValue(3).select();}else{ var isAccountManager = false; for (var i = 0; i < accountmanagerterritories.length; i++){if (data[\"Geography_ID\"].value == accountmanagerterritories[i].ID) isAccountManager = true;} if(isAccountManager){s.findItemByValue(4).select();}; else{if(s.findItemByValue(2)){s.findItemByValue(2).select();}}}}else {s.findItemByValue(1).select();} if((data && data[\"Class_Partition\"] && data[\"Class_Partition\"].value == \"2\")) {gType.control.enable();if(gType.control.findItemByValue(2)){gType.control.findItemByValue(2).set_visible(true);}gType.control.findItemByValue(3).set_visible(true);}else{gType.control.disable();gType.control.findItemByValue(1).select();if(gType.control.findItemByValue(2)){gType.control.findItemByValue(2).set_visible(false);}gType.control.findItemByValue(3).set_visible(false);}}";
        rcbGeographyType.OnClientLoad = "function(s,a) { var gType = s.get_element(); var data = clientManager.get_SelectionData();if(data && data[\"Geography_ID\"] && data[\"Geography_ID\"].value != \"US\"){if(clientManager.get_States()[data[\"Geography_ID\"].value]){s.findItemByValue(3).select();}else{ var isAccountManager = false; for (var i = 0; i < accountmanagerterritories.length; i++){if (data[\"Geography_ID\"].value == accountmanagerterritories[i].ID) isAccountManager = true;} if(isAccountManager){s.findItemByValue(4).select();}else{if(s.findItemByValue(2)){s.findItemByValue(2).select();}}}}else {s.findItemByValue(1).select();} if((data && data[\"Class_Partition\"] && data[\"Class_Partition\"].value == \"2\")) {gType.control.enable();if(gType.control.findItemByValue(2)){gType.control.findItemByValue(2).set_visible(true);}gType.control.findItemByValue(3).set_visible(true);}else{gType.control.disable();gType.control.findItemByValue(1).select();if(gType.control.findItemByValue(2)){gType.control.findItemByValue(2).set_visible(false);}gType.control.findItemByValue(3).set_visible(false);}}";

        rcbGeographyType.OnClientSelectedIndexChanged = "function(s, a){var l=$get('" + Geography_ID.ClientID + "'); var gt = a.get_item().get_value(); " + frag + " reportFiltersResize(); }";

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
