using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PathfinderClientModel;
using PathfinderModel;
using Pinsonault.Web;
using System.Text;
using System.Runtime.Serialization.Json;
using System.IO;
using Telerik.Web.UI;

public partial class todaysanalytics_controls_FilterGeography : UserControl//, IFilterControl 
{
    public todaysanalytics_controls_FilterGeography()
    {
        ContainerID = "moduleOptionsContainer";
    }

    public string ContainerID { get; set; }

    protected override void OnInit(EventArgs e)
    {
        dsAcctMgr.ConnectionString = Pinsonault.Web.Session.ClientConnectionString;

        base.OnInit(e);
    }

    protected override void OnLoad(EventArgs e)
    {

        //Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, rcbGeographyType.ClientID, null, ContainerID);
        //Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Geography_ID.ClientID, null, ContainerID);
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Geography_Type.ClientID, null, ContainerID);
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Region_ID.ClientID, null, ContainerID);
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, State_ID.ClientID, null, ContainerID);
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Territory_ID.ClientID, null, ContainerID);


        //string frag = "switch(parseInt(gt,10)) {case 1: $loadListItems(l,null,{value:'" + DefaultValue + "',text:'" + DefaultValue + "'});l.control.set_visible(false); break; case 2: $loadListItems(l, clientManager.get_RegionListOptions()); l.control.set_visible(true);break; case 3: $loadListItems(l, clientManager.get_States()); l.control.set_visible(true);break;}";

        //Geography_ID.OnClientLoad = "function(s,a){ var l=s.get_element(); var gt = $find('" + rcbGeographyType.ClientID + "').get_value(); " + frag + "}";

        //rcbGeographyType.OnClientLoad = "function(s,a) { var data = clientManager.get_SelectionData(); if(data && data[\"Geography_ID\"] && data[\"Geography_ID\"].value != \"US\"){ s.findItemByValue(clientManager.get_States()[data[\"Geography_ID\"].value] ? 3 : 2).select();}}";

        //rcbGeographyType.OnClientSelectedIndexChanged = "function(s, a){var l=$get('" + Geography_ID.ClientID + "'); if(!l || !l.control) return; var gt = a.get_item().get_value(); " + frag + " reportFiltersResize(); }";


        ////Region item only visible for some clients
        //RadComboBoxItem item = rcbGeographyType.Items.FindItemByValue("2");
        //if ( item != null )
        //{
        //    item.Visible = Context.User.IsInRole("sr_rgns");
        //}

        if (!Page.IsPostBack)
        {
            //Get all available states
            using (PathfinderEntities context = new PathfinderEntities())
            {
                var q = (from p in context.StateSet
                         orderby p.Name
                         select p).ToList().Select(p => new GenericListItem { ID = p.ID.ToString(), Name = p.Name });
                if (q != null)
                {
                    //List<GenericListItem> list = q.ToList();
                    //list.Insert(0, new GenericListItem { ID = "0", Name = "All" });
                    Pinsonault.Web.Support.RegisterGenericListVariable(this.Page, q.ToArray(), "allStates");
                }
            }

            using (PathfinderClientEntities context = new PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
            {
                //Get available Territories and States to create JSON variable to dynamically populate Region/State selection
                var territoryQuery =
                    (from d in context.TerritorySet
                     orderby d.Name
                     where d.User_Level == 2
                     select d).ToList().Select(p => new GenericListItem { ID = p.ID.ToString(), Name = p.Name });

                int territoryCount = territoryQuery.Count();
                
                //Hide Region selection if no territories
                if (territoryCount == 0)
                    foreach (RadComboBoxItem cbi in Geography_Type.Items)
                        if (cbi.Value == "2")
                            cbi.Visible = false;

                string territories = "";
                StringBuilder sbState = new StringBuilder();

                Type[] types = { typeof(GenericListItem[]), typeof(GenericListItem) };
                DataContractJsonSerializer serializer = serializer = new DataContractJsonSerializer(typeof(GenericListItem), "root", types);

                sbState.Append("var states = {");

                using (MemoryStream ms = new MemoryStream())
                {
                    serializer.WriteObject(ms, territoryQuery.ToArray());
                    string script = UTF8Encoding.UTF8.GetString(ms.ToArray());
                    territories = string.Format("var territories = {0};", script);
                }

                int x = 1;
                foreach (GenericListItem li in territoryQuery)
                {
                    sbState.AppendFormat("{0}:", li.ID);

                    var stateQuery = (from d in context.TerritoryGeographySet
                                      join n in context.StateSet on
                                      d.Geography_ID equals n.ID
                                      orderby n.Name
                                      where d.Territory_ID == li.ID
                                      select n).ToList().Select(p => new GenericListItem { ID = p.ID, Name = p.Name });

                    using (MemoryStream ms = new MemoryStream())
                    {
                        serializer.WriteObject(ms, stateQuery.ToArray());
                        string script = UTF8Encoding.UTF8.GetString(ms.ToArray());
                        sbState.Append(script);
                    }

                    if (x != territoryCount) //only add a comma if it is not the last item
                        sbState.Append(",");

                    x++;
                }

                sbState.Append("};");

                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "territories", territories, true);
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "states", sbState.ToString(), true);

                //string frag = "if (gt && gt != 'US'){$loadListItems(l, states[gt], {value:'',text:'Select State'});l.control.set_visible(true);}else l.control.set_visible(false);";
                string frag = "var terCtrl =$get('" + Territory_ID.ClientID + "'); var regCtrl =$get('" + Region_ID.ClientID + "'); var territoryValidatorID = '" + validator1.ClientID + "'; var regionValidatorID = '" + validator2.ClientID + "';if(!regCtrl || !regCtrl.control || !terCtrl || !terCtrl.control) return; if (gt && gt == '1'){$loadListItems(l, allStates, {value:'',text:'Select State'});l.control.set_visible(true); terCtrl.control.set_visible(false);$('#' + territoryValidatorID).attr('_required', 'false');regCtrl.control.set_visible(false);$('#' + regionValidatorID).attr('_required', 'false');}if (gt && gt == '2'){$loadListItems(l, states[rg], {value:'',text:'Select State'});l.control.set_visible(true);terCtrl.control.set_visible(false);$('#' + territoryValidatorID).attr('_required', 'false'); regCtrl.control.set_visible(true);$('#' + regionValidatorID).attr('_required', 'true');}if (gt && gt == '3'){l.control.set_visible(false);terCtrl.control.set_visible(true);$('#' + territoryValidatorID).attr('_required', 'true');regCtrl.control.set_visible(false);$('#' + regionValidatorID).attr('_required', 'false');} ";

                Region_ID.OnClientLoad = "function (s, a) {var t = s.get_element();$loadListItems(t, territories, {value:'',text:'Select Region'});var data = clientManager.get_SelectionData();if (data && data[\"Region_ID\"] && data[\"Region_ID\"].value != \"US\") s.findItemByValue(data[\"Region_ID\"].value).select(); var gt = $find('" + Geography_Type.ClientID + "').get_value(); if (gt && gt != '2')t.control.set_visible(false);else t.control.set_visible(true);}";

                State_ID.OnClientLoad = "function(s,a){ var l=s.get_element(); var gt = $find('" + Geography_Type.ClientID + "').get_value(); var rg = $find('" + Region_ID.ClientID + "').get_value(); " + frag + "}";

                Territory_ID.OnClientLoad = "function (s, a) {var t = s.get_element(); var data = clientManager.get_SelectionData(); if (data && data[\"Territory_ID\"]) s.findItemByValue(data[\"Territory_ID\"].value).select(); var gt = $find('" + Geography_Type.ClientID + "').get_value(); if (gt && gt != '3')t.control.set_visible(false);else t.control.set_visible(true);}";

                //Region_ID.OnClientSelectedIndexChanged = "function(s, a){var l=$get('" + State_ID.ClientID + "'); if(!l || !l.control) return; var gt = a.get_item().get_value(); " + frag + " reportFiltersResize(); }";
                Geography_Type.OnClientSelectedIndexChanged = "function(s, a){var l=$get('" + State_ID.ClientID + "'); if(!l || !l.control) return; var gt = a.get_item().get_value(); var rg = $find('" + Region_ID.ClientID + "').get_value();" + frag + " reportFiltersResize(); }";

                Region_ID.OnClientSelectedIndexChanged = "function(s,a){var l=$get('" + State_ID.ClientID + "'); if(!l || !l.control) return; var rg = a.get_item().get_value(); $loadListItems(l, states[rg], {value:'',text:'Select State'});}";


                //ClientScriptManager csm = Page.ClientScript;
                //csm.RegisterStartupScript(typeof(Page), "InitializeCheckboxes", "$('#timeFrameYearContainer input:checkbox, #timeFrameQuarterContainer input:checkbox, #timeFrameMonthContainer input:checkbox').each(function(){var id=$(this).attr('id');if($(this).attr('checked'))$('#timeFrameContainer label[for='+id+']').addClass('selectedDateOption');$(this).click(function(){checkboxCheckStatus(this,false)})});$('#timeFrameYearContainer input:checkbox').click(function(){$('#timeFrameYearContainer label').not('#timeFrameYearContainer label[for='+$(this).attr('id')+']').removeClass('selectedDateOption');$('#timeFrameYearContainer input:checkbox:checked').not(this).removeAttr('checked');if($('#timeFrameYearContainer input:checked').length==0)clearQuarterMonth();else $('#timeFrameContainer .selectAll').show()});clearQuarterMonth();$('#timeFrameMonthContainer span').each(function(idx){if(idx==5)$(this).after('<br/><br/>')});$('#timeFrameYearContainer label').each(function(){$(this).bind('click',function(){clearQuarterMonth();for(var key in yrQtr[$(this).text()]){var cbxID=$('#timeFrameQuarterContainer label:contains('+yrQtr[$(this).text()][key].Name+')').attr('for');$('#'+cbxID).removeAttr('disabled');$('#timeFrameQuarterContainer label[for='+cbxID+']').removeClass('disabledCheckbox')}for(var key in yrMth[$(this).text()]){var cbxID=$('#timeFrameMonthContainer label:contains('+yrMth[$(this).text()][key].Name+')').attr('for');$('#'+cbxID).removeAttr('disabled');$('#timeFrameMonthContainer label[for='+cbxID+']').removeClass('disabledCheckbox')}})});", true);

                //Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "CheckBoxFunctions", "function toggleChecked(status,containerID){if(containerID=='timeFrameQuarterContainer'){$resetContainer('divMonthContainer');$('#timeFrameMonthContainer label').removeClass('selectedDateOption')}else{$resetContainer('divQuarterContainer');$('#timeFrameQuarterContainer label').removeClass('selectedDateOption')}$('#'+containerID+' input:checkbox').each(function(){if(!$(this).attr('disabled')){$(this).attr('checked',status);checkboxCheckStatus(this,true)}})}function checkboxCheckStatus(ctrl,allClicked){var id=$(ctrl).attr('id');if($('#'+id).attr('checked'))$('#timeFrameContainer label[for='+id+']').addClass('selectedDateOption');else $('#timeFrameContainer label[for='+id+']').removeClass('selectedDateOption');if(!allClicked){if($('#'+id).parents('#timeFrameQuarterContainer').length>0){$('#optionAllQuarter').attr('checked','');$resetContainer('divMonthContainer');$('#timeFrameMonthContainer label').removeClass('selectedDateOption')}if($('#'+id).parents('#timeFrameMonthContainer').length>0){$('#optionAllMonth').attr('checked','');$resetContainer('divQuarterContainer');$('#timeFrameQuarterContainer label').removeClass('selectedDateOption')}}}function clearQuarterMonth(){$resetContainer('divQuarterContainer');$resetContainer('divMonthContainer');$('#timeFrameMonthContainer :input').attr('disabled',true);$('#timeFrameMonthContainer label').addClass('disabledCheckbox').removeClass('selectedDateOption');$('#timeFrameQuarterContainer :input').attr('disabled',true);$('#timeFrameQuarterContainer label').addClass('disabledCheckbox').removeClass('selectedDateOption');$('#timeFrameContainer .selectAll').hide()}", true);
            }
        }

        base.OnLoad(e);
    }

    #region IFilterControl Members

    string _defaultValue; 
    public string DefaultValue 
    { 
        get
        {
            if ( _defaultValue == null ) 
                return "US";

            return _defaultValue;
        }
        set
        {
            _defaultValue = value;
        }    
    }
    
    #endregion
}
