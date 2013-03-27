/// <reference name="MicrosoftAjax.js"/>
/// <reference path="~/content/scripts/jquery-1.4.1-vsdoc.js"/>
/// <reference path="~/content/scripts/ui.js"/>
/// <reference path="~/content/scripts/clientManager.js"/>


//Formulary Sell Sheets business rules ---------------------------------------------------------------------------------------------------------------------------------------------
Pathfinder.UI.AlconFormularySellSheetsApplication = function(id) {
    Pathfinder.UI.AlconFormularySellSheetsApplication.initializeBase(this, [id]);
};
Pathfinder.UI.AlconFormularySellSheetsApplication.prototype =
{
    createSellSheet: function() {
        //Clear the plan selection for the first time.
        clientManager.setContextValue("ssSelectedPlansList");

        var dt = new Date();
        dt = "'" + encodeURIComponent(dt.localeFormat("d") + " " + dt.localeFormat("t")) + "'";

        $.getJSON("custom/Alcon/sellsheets/services/AlconDataService.svc/CreateSellSheet?Created=" + dt, null, this._onCreateCallbackDelegate);
    }
};
Pathfinder.UI.AlconFormularySellSheetsApplication.registerClass("Pathfinder.UI.AlconFormularySellSheetsApplication", Pathfinder.UI.FormularySellSheetsApplication);

//Customer Contact Reports  ---------------------------------------------------------------------------------------------------------------------------------------------
Pathfinder.UI.AlconCustomerContactReportsApplication = function(id) {
    Pathfinder.UI.AlconCustomerContactReportsApplication.initializeBase(this, [id]);
};
Pathfinder.UI.AlconCustomerContactReportsApplication.prototype =
{

};
Pathfinder.UI.AlconCustomerContactReportsApplication.registerClass("Pathfinder.UI.AlconCustomerContactReportsApplication", Pathfinder.UI.CustomerContactReportsApplication);


//Activity Reporting  ---------------------------------------------------------------------------------------------------------------------------------------------
Pathfinder.UI.AlconActivityReportingApplication = function(id)
{
    Pathfinder.UI.AlconActivityReportingApplication.initializeBase(this, [id]);
};
Pathfinder.UI.AlconActivityReportingApplication.prototype =
{

};
Pathfinder.UI.AlconActivityReportingApplication.registerClass("Pathfinder.UI.AlconActivityReportingApplication", Pathfinder.UI.ActivityReportingApplication);

//Sell Sheet Reporting
Pathfinder.UI.AlconFormularySellSheetsReportingApplication = function(id)
{
    Pathfinder.UI.AlconFormularySellSheetsReportingApplication.initializeBase(this, [id]);
};
Pathfinder.UI.AlconFormularySellSheetsReportingApplication.prototype =
{
    get_UrlName: function() { return "custom/" + this.get_clientKey() + "/sellsheetreporting"; },

    get_Title: function() { return ""; },

    getUrl: function(channelName, module, pageName, hasData, isCustom)
    {
        channelName = "all";

        return Pathfinder.UI.AlconFormularySellSheetsReportingApplication.callBaseMethod(this, 'getUrl', [channelName, module, pageName, hasData, isCustom]);
    },
    get_ModuleMenu: function()
    {
        return $find("moduleSelector");
    },
    get_OptionsServiceUrl: function(clientManager)
    {
        return this.get_ServiceUrl() + "/GetSellSheetReportingOptions";
    },

    getDefaultModule: function(clientManager)
    {
        return "formularysellsheetreport";
    },

    get_ModuleOptionsUrl: function(clientManager)
    {
        return this.getUrl("all", null, "formularysellsheetreport_filters.aspx", false);

    },
    resize: function()
    {
        standardreports_content_resize();    

        Pathfinder.UI.AlconFormularySellSheetsReportingApplication.callBaseMethod(this, 'resize');
    },

    resizeSection: function()
    {
        standardreports_section_resize();
       
        if ($.browser.version == "7.0")
        {
            $(".rmLink").css({ width: "195px" });
        }
        Pathfinder.UI.AlconFormularySellSheetsReportingApplication.callBaseMethod(this, 'resizeSection');
    }

};
Pathfinder.UI.AlconFormularySellSheetsReportingApplication.registerClass("Pathfinder.UI.AlconFormularySellSheetsReportingApplication", Pathfinder.UI.BasicApplication);

//end of sell sheet reporting
//formulary history reporting starts
Pathfinder.UI.AlconFormularyHistoryReportingApplication = function(id)
{
    Pathfinder.UI.AlconFormularyHistoryReportingApplication.initializeBase(this, [id]);
};

Pathfinder.UI.AlconFormularyHistoryReportingApplication.registerClass("Pathfinder.UI.AlconFormularyHistoryReportingApplication", Pathfinder.UI.FormularyHistoryReportingApplication);

//formulary history reporting ends
function OnDrilldownDataBinding(sender, args) {
    //fix filter for custom query
    var filter = clientManager.get_SelectionData()["Meeting_Outcome_ID"];
    if (filter)
        $setGridFilter(sender, filter.name, filter.value, filter.filterType, filter.dataType, true);

    filter = clientManager.get_SelectionData()["Followup_Notes_ID"];
    if (filter)
        $setGridFilter(sender, filter.name, filter.value, filter.filterType, filter.dataType, true);
}
//filter document type code -starts
function CreateDocTypeList(s, a) {
   
    $createCheckboxDropdown(s.get_id(),'DocumentTypeIDOptionList',DocumentType,{ 'defaultText': '--All Document Types --','multiItemText': '--Selection Changed--' },null ,'moduleOptionsContainer');   
    var document_type_id = $get('DocumentTypeIDOptionList').control;
    $loadPinsoListItems(document_type_id, DocumentType, null, -1); 
 }

//filter document type code - ends
 function viewDocument1(docID) {
    
         window.open(clientManager.get_BasePath() + "/usercontent/OpenBusinessDocument.ashx?&Document_ID=" + docID);

 }

 //filters for sell sheet report-starts
//for section load
 function get_SectionLoad(s, a)
 {
     $createCheckboxDropdown('ctl00_partialPage_filtersContainer_MarketSegment_Section_ID', 'Section_ID', null, { 'defaultText': '', 'multiItemText': '--Change Selection--' }, { 'itemClicked': onMarketSegmentItemClicked }, 'moduleOptionsContainer');
     var section_id_control = $get('Section_ID').control;
     $loadPinsoListItems(section_id_control, channelsList, { 'ID': '0', 'Name': 'All', 'Value': '0' }, 0, false);
 }

//for updating plan
function update_PlanLoad(s, a)
{
    var val = s.get_element().checkboxList.get_values();
    //updatePlans(val);
}
function updatePlans(val)
{
    var eq = '';
    if (val != null)
    {
        if ($.isArray(val))
        {
            $.each(val, function(intIndex, objValue)
            {
                eq += "Section_ID eq " + objValue;
                if (intIndex != (val.length - 1))
                    eq += " or ";
            });
        }
        else
            eq = "Section_ID eq " + val;
        url = "services/pathfinderservice.svc" + "/PlanInfoListViewSet?$filter=" + eq + "&$orderby=Plan_Name";   
    }
    else
        url = "services/pathfinderservice.svc" + "/PlanInfoListViewSet?$orderby=Plan_Name";

   //if section is DoD then only display plans which have formulary lives. Do not display all DoD plans in filters
    if (val == 12)
        url = "services/PathfinderService.svc" + "/PlanInfoListViewSet?$filter=Section_ID eq " + val + " and Custom_Sort eq 1" + "&$orderby=Plan_Name"; // V_PlanInfoList_Base

    $.getJSON(url, null, function(result, status)
    {
        var d = result.d;
        var plan_id = 'ctl00_partialPage_filtersContainer_TimeFrame_Plan_ID';
        $loadListItems($find(plan_id), d, !d.length ? { value: "", text: "no records available"} : { value: "", text: "--Select Account--" }, -1, 'Plan_ID', 'Plan_Name');
    });
}
//for Title load
//function get_TitleLoad(s, a)
//{
//    $loadListItems($find(s.get_id()), titlesList, !titlesList.length ? { value: "", text: "no records available"} : { value: "", text: "--Select Title--" });
//}

 //for sell sheet market basket load
 function onMBLoad(s, a)
 {
     var url = "custom/Alcon/sellsheetreporting/services/AlconService.svc" + "/SellSheetTheraListSet?$orderby=Name";

     $.getJSON(url, null, function(result, status)
     {
         var d = result.d;

         $createCheckboxDropdown('ctl00_partialPage_filtersContainer_DrugSelection_Thera_ID', 'Thera_ID', null, { 'defaultText': '', 'multiItemText': '--Change Selection--' }, { 'itemClicked': onMarketSegmentItemClicked }, 'moduleOptionsContainer');
         var thera_id_control = $get('Thera_ID').control;
         $loadPinsoListItems(thera_id_control, d, { 'ID': '0', 'Name': 'All', 'Value': '0' }, 0, false);
         $updateCheckboxDropdownText('ctl00_partialPage_filtersContainer_DrugSelection_Thera_ID', 'Thera_ID');
         
         var drug_id = $find('ctl00_partialPage_filtersContainer_DrugSelection_Drug_ID');
         var val = thera_id_control.get_values();
         updateDrugs(val, drug_id);
     });

 }
 //for updating the drug as per thera selection
 function UpdateDrugList(s, a)
 {
     var val = s.get_element().checkboxList.get_values();
     var drug_id = $find('ctl00_partialPage_filtersContainer_DrugSelection_Drug_ID');
     updateDrugs(val, drug_id)
 }
 function updateDrugs(val, drugControlid)
 {
     var eq = '';
     var url;

     if (val != null)
     {
         if ($.isArray(val))
         {
             $.each(val, function(intIndex, objValue)
             {
                 eq += "Thera_ID eq " + objValue;
                 if (intIndex != (val.length - 1))
                     eq += " or ";
             }
           );
         }
         else
             eq = "Thera_ID eq " + val;
         url = "custom/Alcon/sellsheetreporting/services/AlconService.svc" + "/SellSheetDrugListSet?$filter=" + eq + "&$orderby=Name";
     }   
     else
         url = "custom/Alcon/sellsheetreporting/services/AlconService.svc" + "/SellSheetDrugListSet?$orderby=Name";
     $.getJSON(url, null, function(result, status)
     {
         var d = result.d;

         var drug_id = $get('Drug_ID');
         if (drug_id)
         {
             var drug_id_control = drug_id.control;
             if (drug_id_control) $loadPinsoListItems(drug_id_control, d, {'ID':'0','Name':'All','Value':'0'},0,false);
         }
         else
         {
             $createCheckboxDropdown('ctl00_partialPage_filtersContainer_DrugSelection_Drug_ID', 'Drug_ID', null, { 'defaultText': '', 'multiItemText': '--Change Selection--' }, null, 'moduleOptionsContainer');
             var drug_id_control = $get('Drug_ID').control;
             $loadPinsoListItems(drug_id_control, d, { 'ID': '0', 'Name': 'All', 'Value': '0' }, 0, false);
         };
         $updateCheckboxDropdownText('ctl00_partialPage_filtersContainer_DrugSelection_Drug_ID', 'Drug_ID');
     });
     
 }
 // for sales geographies
 function SalesGeoTypeLoad(s, a)
 {
     s.findItemByValue(1).select();
 }
 function salesGeoIDLoad(s, a)
 {    
     var l = s.get_element();
     var gt = $find('ctl00_partialPage_filtersContainer_TimeFrame_rcbGeographyType').get_value();
     var regCtrl = $get('ctl00_partialPage_filtersContainer_TimeFrame_Geography_ID');
     
     if (!regCtrl || !regCtrl.control) return;

     if (gt && gt == '1')
     {
         $loadListItems(l, null, { value: '', text: '' });

         l.control.set_visible(false);
         regCtrl.control.set_visible(false);
     }
     //states - code changed to get states instead sales territories, regions etc - test track - 1911
     if (gt && gt == '2') 
     {
         $loadListItems(l, sellsheetstatesList);
         l.control.set_visible(true); regCtrl.control.set_visible(true);
     }
 }
 function salesgeoType_SelectedIndexChanged(s, a)
 {
     //to get Sales Territories, regions and districts   
     var gt = a.get_item().get_value();
     var regCtrl = $get('ctl00_partialPage_filtersContainer_TimeFrame_Geography_ID');
     var planCtrl = $get('ctl00_partialPage_filtersContainer_TimeFrame_Plan_ID');

     if (!regCtrl || !regCtrl.control) return;
     //national
     if (gt && gt == '1')
     {
         $loadListItems(regCtrl, null, { value: '', text: '' });
         regCtrl.control.set_visible(false);
         //planCtrl.control.disable();
     }
     //states - code changed to get states instead sales territories, regions etc - test track - 1911
     if (gt && gt == '2') 
     {
         $loadListItems(regCtrl, sellsheetstatesList);
         regCtrl.control.set_visible(true);
         //planCtrl.control.enable();
     }
     reportFiltersResize();
 }
// function PlanIDLoad(s, a)
// {
//     var planCtrl = s.get_element();
//     planCtrl.control.disable();
// }
 
 //filter for sell sheet report- ends
 //class template selection in sell sheet validation-- starts
 function validateDrugSelection(drugTextControlID,txtRequiredDrugSelected, template_id)
 {
     var thera_id = null;
     var SeldrugIDs = null;
     txtRequiredDrugSelected.val("");

     if ($find(theraCtrlID) != null)
     {
         thera_id = $find(theraCtrlID).get_value();
         //if (thera_id > 0) 
           // txtRequiredDrugSelected.val("true");
     }

     if (drugCtrlID && drugCtrlID.control)
         SeldrugIDs = $get(drugCtrlID).control.get_element().checkboxList.get_values();
     else
         SeldrugIDs = drugTextControlID.val();
         
     if ($.isArray(SeldrugIDs))
         SeldrugIDs = SeldrugIDs.join(",");

     if (thera_id != null && SeldrugIDs != null && template_id != null)
     {         
         var url = "custom/Alcon/sellsheetreporting/services/AlconService.svc" + "/SellSheetDrugListSet?$filter=Thera_ID eq " + thera_id + " and Selected eq 1" ;
         txtRequiredDrugSelected.val("");
         $.getJSON(url, null, function(result, status)
         {
             var d = result.d;
             if (d.length > 0)
             {
                 for (var i = 0; i < d.length; i++)
                 {
                     if (d[i].Template_ID != template_id)
                     {
                         if (SeldrugIDs.indexOf(d[i].ID) > -1)
                         {
                             $get("Drug_ID").control.UnselectItem(d[i].ID.toString());
                             alert(d[i].Name + " cannot be selected with this template.");
                         }
                     }
                     else if (d[i].Template_ID == template_id)
                     {
                         if (d[i].Selected == 1 && SeldrugIDs.indexOf(d[i].ID) == -1)
                         {
                             $get("Drug_ID").control.selectItem(d[i].ID.toString());
                         }
                         //if (SeldrugIDs.indexOf(d[i].ID) > -1)
                             txtRequiredDrugSelected.val("true");
                     }
                     var drugs = $get(drugCtrlID).control;
                     $updateCheckboxDropdownText(drugs, "Drug_ID");
                     UpdateSelectedDrugList();
                 }

             }
         });
     }

 }

 //class template selection in sell sheet validation-- ends



 

 