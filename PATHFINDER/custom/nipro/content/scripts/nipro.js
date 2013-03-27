//Customer Contact Reports  ---------------------------------------------------------------------------------------------------------------------------------------------
Pathfinder.UI.NiproCustomerContactReportsApplication = function(id)
{
    Pathfinder.UI.NiproCustomerContactReportsApplication.initializeBase(this, [id]);
};
Pathfinder.UI.NiproCustomerContactReportsApplication.prototype =
{

};
Pathfinder.UI.NiproCustomerContactReportsApplication.registerClass("Pathfinder.UI.NiproCustomerContactReportsApplication", Pathfinder.UI.CustomerContactReportsApplication);

//Formulary History Reporting starts
Pathfinder.UI.NiproFormularyHistoryReportingApplication = function(id)
{
    Pathfinder.UI.NiproFormularyHistoryReportingApplication.initializeBase(this, [id]);
};

Pathfinder.UI.NiproFormularyHistoryReportingApplication.registerClass("Pathfinder.UI.NiproFormularyHistoryReportingApplication", Pathfinder.UI.FormularyHistoryReportingApplication);

//Formulary History Reporting ends


function OnDrilldownDataBinding(sender, args)
{
    //fix filter for custom query
    var filter = clientManager.get_SelectionData()["Meeting_Outcome_ID"];
    if (filter)
        $setGridFilter(sender, filter.name, filter.value, filter.filterType, filter.dataType, true);
    
    filter = clientManager.get_SelectionData()["Followup_Notes_ID"];
    if (filter)
        $setGridFilter(sender, filter.name, filter.value, filter.filterType, filter.dataType, true);
}

