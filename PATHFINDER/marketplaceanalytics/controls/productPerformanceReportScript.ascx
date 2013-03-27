<%@ Control Language="C#" AutoEventWireup="true" CodeFile="productPerformanceReportScript.ascx.cs" Inherits="marketplaceanalytics_controls_trendingReportScript" %>
<%@ OutputCache Shared="true" VaryByParam="None" Duration="100" %>

<script type="text/javascript">
    
    clientManager.add_pageInitialized(productPerformanceReportPageInitialized);
    clientManager.add_pageUnloaded(productPerformanceReportPageUnloaded);

    function productPerformanceReportPageInitialized()
    {
        clientManager.registerComponent('ctl00_Tile4_chart_grid1_gridTemplate', null);
        clientManager.registerComponent('ctl00_Tile4_chart_grid2_gridTemplate', null);
        clientManager.registerComponent('ctl00_Tile4_chart_grid3_gridTemplate', null);
        
        //Set detailed grid header always to grid #1
        $("#tile5 .tileContainerHeader").css("background-color", "#20b1aa"); //LightSeaGreen

        scaleChartMrkt(4);
        
        //Hide detailed grid on page load
        $("#divTile5Container").hide()
    }

    function productPerformanceReportPageUnloaded()
    {
        clientManager.remove_pageInitialized(productPerformanceReportPageInitialized);
        clientManager.remove_pageUnloaded(productPerformanceReportPageUnloaded);
    }

    function showHideGrids(gridNum)
    {
        var data = clientManager.cleanSelectionData(clientManager.get_SelectionData());

        switch (gridNum)
        {
            case 1:
                $("#tile5 .tileContainerHeader").css("background-color", "#20b1aa"); //LightSeaGreen

                data["Geography_ID"] = "US";
                data["Territory_Clicked"] = "";

                break;
            case 2:
                $("#tile5 .tileContainerHeader").css("background-color", "#fda601"); //Orange
                data["Territory_Clicked"] = "1";

                break;
            case 3:
                $("#tile5 .tileContainerHeader").css("background-color", "#98cd35"); //YellowGreen
                data["Territory_Clicked"] = "";

                break;
        }

        var grid;

        if ($find("drillDownContainer"))
        {
            grid = $find("drillDownContainer");
            grid.set_params(data);
        }
        else
            grid = $create(Pathfinder.UI.ThinGrid, { loadSelector: ".grid", url: "marketplaceanalytics/all/detailedgrid.aspx", params: data, staticHeader: true }, null, null, $get("drillDownContainer"));

        grid.dataBind();

        //Show detailed grid
        $("#divTile5Container").show();
    }
</script>