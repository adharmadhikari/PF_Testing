/// <reference name="MicrosoftAjax.js"/>
/// <reference path="~/content/scripts/jquery-1.4.1-vsdoc.js"/>
/// <reference path="~/content/scripts/ui-vsdoc.js"/>
/// <reference path="~/content/scripts/clientManager-vsdoc.js"/>


//Formulary Sell Sheets business rules ---------------------------------------------------------------------------------------------------------------------------------------------
Pathfinder.UI.CslFormularySellSheetsApplication = function(id) {
    Pathfinder.UI.CslFormularySellSheetsApplication.initializeBase(this, [id]);
};
//Pathfinder.UI.CslFormularySellSheetsApplication.prototype =
//{
//    createSellSheet: function() {
//        //Clear the plan selection for the first time.
//        clientManager.setContextValue("ssSelectedPlansList");

//        var dt = new Date();
//        dt = "'" + encodeURIComponent(dt.localeFormat("d") + " " + dt.localeFormat("t")) + "'";

//        $.getJSON("custom/csl/sellsheets/services/CSLDataService.svc/CreateSellSheet?Created=" + dt, null, this._onCreateCallbackDelegate);
//    }
//};
Pathfinder.UI.CslFormularySellSheetsApplication.registerClass("Pathfinder.UI.CslFormularySellSheetsApplication", Pathfinder.UI.FormularySellSheetsApplication);

//Customer Contact Reports  ---------------------------------------------------------------------------------------------------------------------------------------------
Pathfinder.UI.CslCustomerContactReportsApplication = function(id) {
    Pathfinder.UI.CslCustomerContactReportsApplication.initializeBase(this, [id]);
};
Pathfinder.UI.CslCustomerContactReportsApplication.prototype =
{

};
Pathfinder.UI.CslCustomerContactReportsApplication.registerClass("Pathfinder.UI.CslCustomerContactReportsApplication", Pathfinder.UI.CustomerContactReportsApplication);



function OnDrilldownDataBinding(sender, args) {
    //fix filter for custom query
    var filter = clientManager.get_SelectionData()["Meeting_Outcome_ID"];
    if (filter)
        $setGridFilter(sender, filter.name, filter.value, filter.filterType, filter.dataType, true);

    filter = clientManager.get_SelectionData()["Followup_Notes_ID"];
    if (filter)
        $setGridFilter(sender, filter.name, filter.value, filter.filterType, filter.dataType, true);
}


