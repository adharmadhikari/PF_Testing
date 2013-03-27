//Customer Contact Reports  ---------------------------------------------------------------------------------------------------------------------------------------------
Pathfinder.UI.JazzCustomerContactReportsApplication = function(id)
{
    Pathfinder.UI.JazzCustomerContactReportsApplication.initializeBase(this, [id]);
};
Pathfinder.UI.JazzCustomerContactReportsApplication.prototype =
{

};
Pathfinder.UI.JazzCustomerContactReportsApplication.registerClass("Pathfinder.UI.JazzCustomerContactReportsApplication", Pathfinder.UI.CustomerContactReportsApplication);



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
