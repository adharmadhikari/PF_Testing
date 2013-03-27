using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PathfinderModel;
using PathfinderClientModel;
using Pinsonault.Web;
using Telerik.Web.UI;

public partial class standardreports_controls_FilterSearchTypeStateMedicaid : System.Web.UI.UserControl
{
    public string ContainerID { get; set; }

    public standardreports_controls_FilterSearchTypeStateMedicaid()
    {

        ContainerID = "moduleOptionsContainer";

    }

    protected override void OnLoad(EventArgs e)
    {
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Search_Type.ClientID, null, ContainerID);
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Plan_ID.ClientID, null, ContainerID);
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

                int channel = Convert.ToInt32(Request.QueryString["channel"]);

                var t = (from p in context.PlanSearchSet
                         where p.Section_ID == channel
                         orderby p.Name
                         select p).ToList().Select(p => new GenericListItem { ID = p.ID.ToString(), Name = p.Name });
                if (t != null)
                {
                    //List<GenericListItem> list = q.ToList();
                    //list.Insert(0, new GenericListItem { ID = "0", Name = "All" });
                    Pinsonault.Web.Support.RegisterGenericListVariable(this.Page, t.ToArray(), "allPlans");
                }
            }


        }

        string frag = "if ((gt && gt == '1') || (gt && gt == '2')){$loadListItems(l,null,{value:'" + DefaultValue + "',text:'" + DefaultValue + "'});l.control.set_visible(false); }if (gt && gt == '3'){$loadListItems(l, accountmanagerterritories, {value:'" + DefaultValue + "',text:'All Account Managers'});l.control.set_visible(true);}if (gt && gt == '4'){$loadListItems(l, clientManager.get_RegionListOptions());l.control.set_visible(true);} var plan_id = $get('Plan_ID'); if (plan_id.control){plan_id.control.reset();$updateCheckboxDropdownText('ctl00_partialPage_filtersContainer_SearchType_Plan_ID', 'Plan_ID');}";

        string frag2 = "var c = $get('ctl00_partialPage_filtersContainer_SearchType_Search_Type');var s = $get('ctl00_partialPage_filtersContainer_SearchType_Plan_ID');if(!c || !c.control || !s || !s.control) return;var gt = c.control.get_value();switch (parseInt(gt, 10)) {case 1: s.control.set_visible(true); break;default:s.control.set_visible(false);break;}";

        string frag3 = "var regCtrl = $('#ctl00_partialPage_filtersContainer_SearchType_validator1'); if(!regCtrl){ return;} var searchType = $find('ctl00_partialPage_filtersContainer_SearchType_Search_Type').get_value();if (searchType == 1){regCtrl.attr('_required', 'true');}else{regCtrl.attr('_required', 'false');}";

        Geography_ID.OnClientLoad = "function(s,a){ var l=s.get_element(); var gt = $find('" + Search_Type.ClientID + "').get_value(); " + frag + " " + frag3 + "}";

        Search_Type.OnClientLoad = "function(s,a) { }";

        Search_Type.OnClientSelectedIndexChanged = "function(s, a){var l=$get('" + Geography_ID.ClientID + "');  if(!l || !l.control) return; var gt = a.get_item().get_value(); " + frag + " " + frag2 + " " + frag3 + " reportFiltersResize(); }";

        Plan_ID.OnClientLoad = "function(s,a){var plan_id = $get('Plan_ID');if (plan_id) {var plan_id_control = plan_id.control;if (plan_id_control)$loadPinsoListItems(plan_id_control, allPlans, null, -1);}else {$createCheckboxDropdown('ctl00_partialPage_filtersContainer_SearchType_Plan_ID', 'Plan_ID', null, { 'defaultText': 'Select Account(s)', 'multiItemText': '--Change Selection--' }, null, 'moduleOptionsContainer'); var plan_id_control = $get('Plan_ID').control; $loadPinsoListItems(plan_id_control, allPlans, null, -1);}; $updateCheckboxDropdownText('ctl00_partialPage_filtersContainer_SearchType_Plan_ID', 'Plan_ID');" + frag2 + " }";

        ////Region item only visible for some clients
        RadComboBoxItem item = Search_Type.Items.FindItemByValue("4");
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
