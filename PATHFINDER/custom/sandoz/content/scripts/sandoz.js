/// <reference name="MicrosoftAjax.js"/>
/// <reference path="~/content/scripts/jquery-1.4.1-vsdoc.js"/>
/// <reference path="~/content/scripts/ui-vsdoc.js"/>
/// <reference path="~/content/scripts/clientManager-vsdoc.js"/>


//Formulary Sell Sheets business rules ---------------------------------------------------------------------------------------------------------------------------------------------
Pathfinder.UI.SandozFormularySellSheetsApplication = function(id)
{
    Pathfinder.UI.SandozFormularySellSheetsApplication.initializeBase(this, [id]);
};
//Pathfinder.UI.SandozFormularySellSheetsApplication.prototype =
//{
//    createSellSheet: function()
//    {
//        //Clear the plan selection for the first time.
//        clientManager.setContextValue("ssSelectedPlansList");

//        var dt = new Date();
//        dt = "'" + encodeURIComponent(dt.localeFormat("d") + " " + dt.localeFormat("t")) + "'";

//        $.getJSON("custom/sandoz/sellsheets/services/SandozDataService.svc/CreateSellSheet?Created=" + dt, null, this._onCreateCallbackDelegate);
//    }
//};
Pathfinder.UI.SandozFormularySellSheetsApplication.registerClass("Pathfinder.UI.SandozFormularySellSheetsApplication", Pathfinder.UI.FormularySellSheetsApplication);

