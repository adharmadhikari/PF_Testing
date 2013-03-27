using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class standardreports_controls_FilterDrugSelection : System.Web.UI.UserControl
{
    public standardreports_controls_FilterDrugSelection()
    {
        ContainerID = "moduleOptionsContainer";
        MaxDrugs = 5;
    }

    public string ContainerID { get; set; }
    public int MaxDrugs { get; set; }

    protected override void OnLoad(EventArgs e)
    {
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Market_Basket_ID.ClientID, null, ContainerID);
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Drug_ID.ClientID, null, ContainerID);

        //not ideal but event handlers are added in code behind this way so element ids can be dynamically set (can vary depending on naming containers).
        Market_Basket_ID.OnClientLoad = "function(s,a){$loadListItems(s, clientManager.get_MarketBasketListOptions(), null, clientManager.get_MarketBasket());}";
        //Market_Basket_ID.OnClientSelectedIndexChanged = "function(s,a){var d = $find(\"DrugIDList\");if(!d) return;" + (MaxDrugs > 1 ? "$loadPinsoListItems" : "$loadListItems") + "(d, clientManager.get_DrugListOptions()[s.get_value()],null,(clientManager.get_Drug() ? clientManager.get_Drug() : -1)); var dl = $find(\"" + Drug_ID.ClientID + "\"); dl.set_visible(d.get_count()>0);$updateCheckboxDropdownText(dl,'DrugIDList');}";
        if ( MaxDrugs > 1 )
        {
            Market_Basket_ID.OnClientSelectedIndexChanged = "function(s,a){var d = $find(\"DrugIDList\");if(!d) return;$loadPinsoListItems(d, clientManager.get_DrugListOptions()[s.get_value()],null,-1, true); var dl = $find(\"" + Drug_ID.ClientID + "\"); dl.set_visible(d.get_count()>0);$updateCheckboxDropdownText(dl,'DrugIDList');}";
            Drug_ID.OnClientLoad = "function(s,a){ var data = clientManager.get_DrugListOptions()[$find('" + Market_Basket_ID.ClientID + "').get_value()]; $createCheckboxDropdown(s.get_id(), 'DrugIDList', null, {'maxItems':" + MaxDrugs.ToString() + ",'defaultText':'" + Resources.Resource.Label_No_Selection + "'}, {'error':function(){$alert('Maximum " + MaxDrugs.ToString() + " drugs should be selected.', 'Report Filters');}}, 'moduleOptionsContainer'); $loadPinsoListItems($find('DrugIDList'), data,null, data.length>1 ? -1 : data[0].ID, true);$updateCheckboxDropdownText(s,'DrugIDList');}";
            Drug_ID.OnClientDropDownClosed = "function(s,a){resetDrugSelection($find('DrugIDList'),true);}";
        }
        else
        {
            Drug_ID.OnClientLoad = "function(s,a){var data = clientManager.get_DrugListOptions()[$find('" + Market_Basket_ID.ClientID + "').get_value()]; $loadListItems($find('" + Drug_ID.ClientID + "'), data,null, data.length>1 ? -1 : data[0].ID);}";
            Market_Basket_ID.OnClientSelectedIndexChanged = "function(s,a){var d = $find('" + Drug_ID.ClientID + "');if(!d) return;$loadListItems(d, clientManager.get_DrugListOptions()[s.get_value()],null,-1); var dl = $find(\"" + Drug_ID.ClientID + "\");}";
        }
        base.OnLoad(e);
    }
}
