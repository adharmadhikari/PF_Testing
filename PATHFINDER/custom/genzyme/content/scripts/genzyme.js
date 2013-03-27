/// <reference name="MicrosoftAjax.js"/>
/// <reference path="~/content/scripts/jquery-1.4.1-vsdoc.js"/>
/// <reference path="~/content/scripts/ui-vsdoc.js"/>
/// <reference path="~/content/scripts/clientManager-vsdoc.js"/>

//Customer Contact Reports  ---------------------------------------------------------------------------------------------------------------------------------------------
Pathfinder.UI.GenzymeCustomerContactReportsApplication = function(id) {
    Pathfinder.UI.GenzymeCustomerContactReportsApplication.initializeBase(this, [id]);
};
Pathfinder.UI.GenzymeCustomerContactReportsApplication.prototype =
{

};
Pathfinder.UI.GenzymeCustomerContactReportsApplication.registerClass("Pathfinder.UI.GenzymeCustomerContactReportsApplication", Pathfinder.UI.CustomerContactReportsApplication);



function OnDrilldownDataBinding(sender, args) {
    //fix filter for custom query
    var filter = clientManager.get_SelectionData()["Meeting_Outcome_ID"];
    if (filter)
        $setGridFilter(sender, filter.name, filter.value, filter.filterType, filter.dataType, true);

    filter = clientManager.get_SelectionData()["Followup_Notes_ID"];
    if (filter)
        $setGridFilter(sender, filter.name, filter.value, filter.filterType, filter.dataType, true);
}


