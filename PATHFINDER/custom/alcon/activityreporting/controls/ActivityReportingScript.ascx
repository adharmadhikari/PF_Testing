<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ActivityReportingScript.ascx.cs" Inherits="custom_controls_ActivityReportingScript" %>
<%@ OutputCache Shared="true" VaryByParam="None" Duration="100" %>

<script type="text/javascript">

    clientManager.add_pageInitialized(activityReportingPageInitialized);
    clientManager.add_pageUnloaded(activityReportingPageUnloaded);
    function activityReportingPageInitialized()
    {
        clientManager.registerComponent('ctl00_Tile4_activityReportingData1_dataType1_gridActivityType', null);

        //Call scaleChart in UI.js to automatically resize charts
        scaleChart();
    }

    function activityReportingPageUnloaded()
    {
        clientManager.remove_pageInitialized(activityReportingPageInitialized);
        clientManager.remove_pageUnloaded(activityReportingPageUnloaded);
    }

    function gridDrilldown(activityID, noColumn)
    {
        //Co-relate selected pie, and slice, to a specific grid, and row, when this function called from chart. 
        if (noColumn >= 0) //Call is from chart
        {
            var grid = $find('ctl00_Tile4_activityReportingData1_dataType1_gridActivityType');

            var mt = grid.get_masterTableView();

            mt.selectItem(noColumn);
        }
        else //Call is from grid
        {
            var data = { Activity_Type_ID: activityID };

            data["Activity_Date"] = clientManager.get_SelectionData()["Activity_Date"];

            if (clientManager.get_SelectionData()["User_ID"])
                data["User_ID"] = clientManager.get_SelectionData()["User_ID"];

            if (clientManager.get_SelectionData()["Territory_ID"])
                data["Territory_ID"] = clientManager.get_SelectionData()["Territory_ID"];
            
            clientManager.set_SelectionData(data, 1);
        }
    }

    function grid_OnRowSelecting(sender, args) {
        //var gridId = args._tableView._data.ClientID;
        //var grid = $find(gridId);
        var activityID = args._dataKeyValues.Activity_Type_ID;

        gridDrilldown(activityID, -1);
    }

   
</script>