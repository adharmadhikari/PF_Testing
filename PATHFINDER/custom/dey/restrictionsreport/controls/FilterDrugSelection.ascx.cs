using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PathfinderModel;
using Pinsonault.Web;
using System.Text;

public partial class restrictionsreport_controls_FilterDrugSelection : System.Web.UI.UserControl
{
    public restrictionsreport_controls_FilterDrugSelection()
    {
        ContainerID = "moduleOptionsContainer";
        MaxDrugs = 5;
    }

    protected override void OnPreRender(EventArgs e)
    {
        //creates the drop down list for Market Segment
        using (PathfinderEntities context = new PathfinderEntities())
        {
            //Get all available drugs for Restrictions Report
            StringBuilder sb = new StringBuilder("var availableDrugs = {");

            int currentTheraID = 0;


            var q = (from p in context.Client_App_Drug_ListSet
                     where p.App_ID == 21 && p.ClientID == Pinsonault.Web.Session.ClientID
                     orderby p.DrugSortOrder
                     select new { p.TherapeuticClassID, p.TherapeuticClassName, p.ID, p.Drug_Name,p.IsDrugSelected }).Distinct().AsEnumerable();

            bool hasDrugs = false;
            
            foreach (var r in q)
            {
                if (r.TherapeuticClassID != currentTheraID)
                {
                    if (currentTheraID > 0)
                    {
                        sb.Remove(sb.Length - 1, 1);
                        sb.Append("]");
                    }
                    sb.AppendFormat("{0}{1}:[", (currentTheraID > 0 ? "," : ""), r.TherapeuticClassID);

                    currentTheraID = r.TherapeuticClassID;
                }

                sb.Append("{ID:");
                sb.AppendFormat("{0},Name:\"{1}\",Selected:{2}", r.ID, r.Drug_Name, r.IsDrugSelected.ToString().ToLower());
                sb.Append("},");

                hasDrugs = true;
            }

            if (hasDrugs)
            {
                sb.Remove(sb.Length - 1, 1);
                sb.Append("]};");
            }
            else
                sb.Append("};");

            //Get all available Thera Classes for Restrictions Report
            var t = (from p in context.Client_App_Drug_ListSet
                     where p.App_ID == 21 && p.ClientID == Pinsonault.Web.Session.ClientID
                     orderby p.TherapeuticClassName
                     select new { p.TherapeuticClassID, p.TherapeuticClassName }).Distinct().ToList().Select(p => new GenericListItem { ID = p.TherapeuticClassID.ToString(), Name = p.TherapeuticClassName });

            Pinsonault.Web.Support.RegisterGenericListVariable(this.Page, t.OrderBy(p => p.Name).ToArray(), "availableThera");
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "_rrpagevars", sb.ToString(), true);
        }
        base.OnPreRender(e);
    }

    public string ContainerID { get; set; }
    public int MaxDrugs { get; set; }

    protected override void OnLoad(EventArgs e)
    {
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Market_Basket_ID.ClientID, null, ContainerID);
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Drug_ID.ClientID, null, ContainerID);
        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "_sspagevars", string.Format("var drugCtrlID = '{0}'; ", Drug_ID.ClientID), true);

        //not ideal but event handlers are added in code behind this way so element ids can be dynamically set (can vary depending on naming containers).
        Market_Basket_ID.OnClientLoad = "function(s,a){$loadListItems(s, availableThera, null, clientManager.get_MarketBasket());}";
        //Market_Basket_ID.OnClientSelectedIndexChanged = "function(s,a){var d = $find(\"DrugIDList\");if(!d) return;" + (MaxDrugs > 1 ? "$loadPinsoListItems" : "$loadListItems") + "(d, clientManager.get_DrugListOptions()[s.get_value()],null,(clientManager.get_Drug() ? clientManager.get_Drug() : -1)); var dl = $find(\"" + Drug_ID.ClientID + "\"); dl.set_visible(d.get_count()>0);$updateCheckboxDropdownText(dl,'DrugIDList');}";
        if ( MaxDrugs > 1 )
        {
            Market_Basket_ID.OnClientSelectedIndexChanged = "function(s,a){var d = $find(\"DrugIDList\");if(!d) return;$loadPinsoListItems(d, availableDrugs[s.get_value()],null,-1, true); var dl = $find(\"" + Drug_ID.ClientID + "\"); dl.set_visible(d.get_count()>0);$updateCheckboxDropdownText(dl,'DrugIDList');}";
            Drug_ID.OnClientLoad = "function(s,a){ var data = availableDrugs[$find('" + Market_Basket_ID.ClientID + "').get_value()]; $createCheckboxDropdown(s.get_id(), 'DrugIDList', null, {'maxItems':" + MaxDrugs.ToString() + ",'defaultText':'" + Resources.Resource.Label_No_Selection + "'}, {'error':function(){$alert('Maximum " + MaxDrugs.ToString() + " drugs should be selected.', 'Report Filters');}}, 'moduleOptionsContainer'); $loadPinsoListItems($find('DrugIDList'), data,null, data.length>1 ? -1 : data[0].ID, true);$updateCheckboxDropdownText(s,'DrugIDList');}";
        }
        else
        {
            Drug_ID.OnClientLoad = "function(s,a){var data = availableDrugs[$find('" + Market_Basket_ID.ClientID + "').get_value()]; $loadListItems($find('" + Drug_ID.ClientID + "'), data,null, data.length>1 ? -1 : data[0].ID);}";
            Market_Basket_ID.OnClientSelectedIndexChanged = "function(s,a){var d = $find('" + Drug_ID.ClientID + "');if(!d) return;$loadListItems(d, availableDrugs[s.get_value()],null,-1); var dl = $find(\"" + Drug_ID.ClientID + "\");}";
        }
        base.OnLoad(e);
    }
}
