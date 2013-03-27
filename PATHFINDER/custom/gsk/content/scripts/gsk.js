//Customer Contact Reports  ---------------------------------------------------------------------------------------------------------------------------------------------
Pathfinder.UI.GskCustomerContactReportsApplication = function(id)
{
    Pathfinder.UI.GskCustomerContactReportsApplication.initializeBase(this, [id]);
};
Pathfinder.UI.GskCustomerContactReportsApplication.prototype =
{

};
Pathfinder.UI.GskCustomerContactReportsApplication.registerClass("Pathfinder.UI.GskCustomerContactReportsApplication", Pathfinder.UI.CustomerContactReportsApplication);



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