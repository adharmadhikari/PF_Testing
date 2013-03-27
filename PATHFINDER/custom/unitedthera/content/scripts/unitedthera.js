/// <reference name="MicrosoftAjax.js"/>
/// <reference path="~/content/scripts/jquery-1.4.1-vsdoc.js"/>
/// <reference path="~/content/scripts/ui-vsdoc.js"/>
/// <reference path="~/content/scripts/clientManager-vsdoc.js"/>


//Formulary Sell Sheets business rules ---------------------------------------------------------------------------------------------------------------------------------------------
Pathfinder.UI.UnitedtheraFormularySellSheetsApplication = function(id) {
Pathfinder.UI.UnitedtheraFormularySellSheetsApplication.initializeBase(this, [id]);
};
//Pathfinder.UI.UnitedtheraFormularySellSheetsApplication.prototype =
//{
//    createSellSheet: function() {
//        //Clear the plan selection for the first time.
//        clientManager.setContextValue("ssSelectedPlansList");

//        var dt = new Date();
//        dt = "'" + encodeURIComponent(dt.localeFormat("d") + " " + dt.localeFormat("t")) + "'";

//        $.getJSON("custom/unitedthera/sellsheets/services/UnitedTheraDataService.svc/CreateSellSheet?Created=" + dt, null, this._onCreateCallbackDelegate);
//    }
//};
Pathfinder.UI.UnitedtheraFormularySellSheetsApplication.registerClass("Pathfinder.UI.UnitedtheraFormularySellSheetsApplication", Pathfinder.UI.FormularySellSheetsApplication);

