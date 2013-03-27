<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ReportScript.ascx.cs" Inherits="custom_warner_formularyhistoryreporting_controls_ReportScript" %>
<script type="text/javascript">
     var showPhysList = <%= ShowPhysList %>;         
    clientManager.add_pageInitialized(trendingReportPageInitialized);
    clientManager.add_pageUnloaded(trendingReportPageUnloaded);
    
function trendingReportPageInitialized()
{
    clientManager.registerComponent('ctl00_Tile3_chart_grid1_gridTemplate', null);
    clientManager.registerComponent('ctl00_Tile3_chart_grid2_gridTemplate', null);

    scaleChartMrkt(3);
    customtodaysanalytics_content_resize();    

    $create(Pathfinder.UI.ThinGrid, { 
    loadSelector: ".grid",
    url: 'custom/warner/formularyhistoryreporting/all/detailgrid.aspx',
    autoLoad: false,
    staticHeader: true,
    requestPageCount: true,
    pageSelector: ".gridCount",
    pageContainer: "#divTile4Container .pagination",
    pageSize: 50,
    enablePaging: true,
    autoUpdate: true, 
    drillDownLevel: 1 }, null, null, $get("drillDownContainer"));

    //Show instructions
    $("#drillDownContainer").html("<div class='drillDownInstruct'>Click the chart header to view details for all drugs or the graph to view details for a drug.</div>");

    //restore page ui state if needed
    data = clientManager.get_SelectionData(1);

    if (data)
    {
        var gridNum = data["Selection_Clicked"];
        updateHeaderColor(gridNum);

        var grid = $find("ctl00_Tile4_chart_grid" + gridNum + "_drillDownContainer");
        if (grid)
            grid.selectRowByDatakey(data["Product_ID"]);
    }
    else //not refresh - coming from trx/mst refresh?
    {
        data = clientManager.get_SelectionData();
        if (data && data["IsDrilldown"] == true)
        {
            if (data["Product_ID_Drilldown"].toString().indexOf(",") == -1)
                chartEvent(data["Selection_Clicked"], data["Product_ID_Drilldown"]);
            else
                showHideGrids(parseInt(data["Selection_Clicked"]));
        }
    }  

}


function trendingReportPageUnloaded()
{
    clientManager.remove_pageInitialized(trendingReportPageInitialized);
    clientManager.remove_pageUnloaded(trendingReportPageUnloaded);
}
function updateHeaderColor(selection)
{
    var color;

    switch (parseInt(selection, 10))
    {
        case 1: color = "#20b1aa"; //LightSeaGreen
            break;
        case 2: color = "#fda601"; //Orange
            break;
        case 3: color = "#98cd35"; //YellowGreen
            break;
    }

    $("#tile4 .tileContainerHeader").css("background-color", color);
}

function setGridPage(grid, page, totalCount)
{
    var grid = $find("drillDownContainer");
    var data = clientManager.cleanSelectionData(clientManager.get_SelectionData(1));

    grid.set_pageNumber(page + 1);
    grid.set_params(data);
    grid.dataBind();

    var pageContainer = grid.get_pageContainer();
    $(pageContainer).html(grid.constructPager(grid, totalCount));
}

function gridEvent(sender, args)
{
    var gridID = sender.get_id();

    toggleGridRowSelection(gridID, args.ID);
}

function chartEvent(chartNum, datakey) //Handles drilldown from chart
{
    var grid;

    if ($find("ctl00_Tile3_chart_grid" + chartNum + "_drillDownContainer"))
    {
        grid = $find("ctl00_Tile3_chart_grid" + chartNum + "_drillDownContainer");

        grid.selectRowByDatakey(datakey);

        toggleGridRowSelection("ctl00_Tile3_chart_grid" + chartNum + "_drillDownContainer", datakey);
    }
}

function chartFHREvent(sectionID, planID, productID, marketBasketID, timeFrame, isMonth, trxmst, selectedProductID, fieldIndex, monthlyQuarterly)
{
    var q = "&Section_ID=" + sectionID + "&Plan_ID=" + planID + "&Product_ID=" + productID + "&Market_Basket_ID=" + marketBasketID + "&Timeframe=" + timeFrame + "&isMonth=" + isMonth + "&Trx_Mst=" + trxmst + "&Selected_Product_ID=" + selectedProductID + "&Field_Index=" + fieldIndex + "&Monthly_Quarterly=" + monthlyQuarterly;

    var col = {};
    col["Section_ID"] = sectionID;
    col["Plan_ID"] = planID;
    col["Product_ID"] = productID;
    col["Market_Basket_ID"] = marketBasketID;
    col["Timeframe"] = timeFrame;
    col["isMonth"] = isMonth;
    col["Trx_Mst"] = trxmst;
    col["Selected_Product_ID"] = selectedProductID;
    col["Field_Index"] = fieldIndex;
    col["Monthly_Quarterly"] = monthlyQuarterly;

    clientManager.setContextValue("fhQuery", col);

    var url = "marketplaceanalytics/all/formularyhistoryreporting_popup.aspx?" + q;

    clientManager.openViewer(url, null, null, 950, 300);

    $("#infoPopup").draggable({ handle: "div.tileContainerHeader" });
}

function toggleGridRowSelection(gridID, Drug_ID, Tier_ID,TierTimeIndex)
{
    if (gridID == "ctl00_Tile3_chart_grid1_gridTemplate")
    {
        //$find("ctl00_Tile3_chart_grid2_gridTemplate").clearSelections();
        showHideGrids(1, Drug_ID, Tier_ID,TierTimeIndex);
    }
    if (gridID == "ctl00_Tile3_chart_grid2_gridTemplate")
    {
        showHideGrids(2, Drug_ID, Tier_ID,TierTimeIndex);
        //$find("ctl00_Tile3_chart_grid1_gridTemplate").clearSelections();
    }
}

function showHideGrids(gridNum, ProductID, TierOrCoverageStatusID, TierTimeIndex)
{
    $("#drillDownContainer").text("");
    var data = clientManager.cleanSelectionData(clientManager.get_SelectionData());

    data["IsDrilldown"] = true;

    data["Selection_Clicked"] = gridNum;

    updateHeaderColor(gridNum);

    if (ProductID)
        data["Product_ID"] = ProductID;
    if (TierOrCoverageStatusID)
    {
        if (clientManager.get_Module() == "corestrictionshxformulary")
            data["Coverage_Status_ID"] = TierOrCoverageStatusID;
        else
            data["Tier_ID"] = TierOrCoverageStatusID;
    }
    if (TierTimeIndex)
        data["TierTimeIndex"] = TierTimeIndex;

    var grid = $find("drillDownContainer");

    if (grid)
    {
        //Reset pager
        grid.set_params(data);
        grid.set_pageNumber(1);
        grid.set_requestPageCount(true);
        //grid.get_gridCount();
    }

    clientManager.set_SelectionData(data, 1);

    //Show detailed grid
    $("#divTile4Container").show();
}

function chartClicked(chartNum)
{
    $find("ctl00_Tile4_chart_grid1_drillDownContainer").clearSelections();
    $find("ctl00_Tile4_chart_grid2_drillDownContainer").clearSelections();
    $find("ctl00_Tile4_chart_grid3_drillDownContainer").clearSelections();
    showHideGrids(chartNum);
}


</script>