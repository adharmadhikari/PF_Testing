<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ReportScript.ascx.cs" Inherits="prescriberreporting_controls_ReportScript" %>


<script type="text/javascript">
    
    
    var showPhysList = <%= ShowPhysList %>;
          
    clientManager.add_pageInitialized(trendingReportPageInitialized);
    clientManager.add_pageUnloaded(trendingReportPageUnloaded);
    
    function trendingReportPageInitialized()
    {
        clientManager.registerComponent('ctl00_Tile4_chart_grid1_gridTemplate', null);
        clientManager.registerComponent('ctl00_Tile4_chart_grid2_gridTemplate', null);
        clientManager.registerComponent('ctl00_Tile4_chart_grid3_gridTemplate', null);
        
        //Set detailed grid header always to grid #1
        //$("#tile5 .tileContainerHeader").css("background-color", "#20b1aa"); //LightSeaGreen

        scaleChartMrkt(4);

        $create(Pathfinder.UI.ThinGrid, { loadSelector: ".grid", url: '<%= DrillDownGridUrl %>', autoLoad: false, staticHeader: true, requestPageCount: true, pageSelector: ".gridCount", pageContainer: "#divTile5Container .pagination", pageSize: 50, enablePaging: true, autoUpdate: true, drillDownLevel: 1 }, showPhysList ? { click: onGridClicked } : null, null, $get("drillDownContainer"));


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

        $("#tile5 .tileContainerHeader").css("background-color", color);
    }

    function showHideGrids(gridNum, productID)
    {
        $("#drillDownContainer").text("");
        var data = clientManager.cleanSelectionData(clientManager.get_SelectionData());

        data["IsDrilldown"] = true;

        data["Selection_Clicked"] = gridNum;

        updateHeaderColor(gridNum);

        if (productID)
            data["Product_ID"] = productID;

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
        $("#divTile5Container").show();
    }

    function onGridClicked(sender, args)
    {
        //Disable prescriber-popup for Formulary History Reporting
        if ($('.formularyhistoryreporting').length < 1)
        {
            //Check if drilldown was selected in order to persist
            var data = clientManager.get_SelectionData(1);
           
            if (data)
            {
                //var data = clientManager.cleanSelectionData(clientManager.get_SelectionData(1));
                //            var q = clientManager.getSelectionDataForPostback();
                //            q = q + "&Plan_ID=" + args.ID + "&title=" + args.ID;
                
                delete (data["PagingEnabled"]);
                     
                var vals = args.ID;
                var physicianID, defaultProductID;

                physicianID = vals.toString().split("~")[0];
                defaultProductID = vals.toString().split("~")[1];

                var col = {};
                
                var q = $getDataForPostback(data, null, col);

                var prescriberGeographyType = null;
                
                if (data["Selection_Clicked"] == "1")
                    prescriberGeographyType = "region";
                if (data["Selection_Clicked"] == "2")
                    prescriberGeographyType = "district";
                if (data["Selection_Clicked"] == "3")
                    prescriberGeographyType = "territory";

                q = q + "&Physician_ID=" + physicianID + "&Default_Product_ID=" + defaultProductID + "&Prescriber_Geography_Type=" + prescriberGeographyType;
                if(physicianID)
                {
                    col["Physician_ID"] = physicianID;   //Physician_ID comes from selected value in grid
                    col["Default_Product_ID"] = defaultProductID;
                    //col["Section_ID"] = 1;      Default to nothing for All Option
                    col["Prescriber_Geography_Type"] = prescriberGeographyType;
                    col["Selection_Clicked"] = data["Selection_Clicked"];
                    clientManager.setContextValue("physTrendingQuery", col);
                    
                    var url = "prescriberreporting/all/MktPopup.aspx?" + q;
                    clientManager.openViewer(url, null, null, 800);
                }
                else
                    setTimeout(function(){$alert("There is no marketshare data for this plan.", "PathfinderRx");}, 10);

            }
        }
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
        
        if ($find("ctl00_Tile4_chart_grid" + chartNum + "_drillDownContainer"))
        {
            grid = $find("ctl00_Tile4_chart_grid" + chartNum + "_drillDownContainer");

            grid.selectRowByDatakey(datakey);

            toggleGridRowSelection("ctl00_Tile4_chart_grid" + chartNum + "_drillDownContainer", datakey);
        }
    }
    
    function chartFHREvent(sectionID, planID, productID, marketBasketID, timeFrame, isMonth, trxmst, selectedProductID, fieldIndex)
    {
        var q = "&Section_ID=" + sectionID + "&Plan_ID=" + planID + "&Product_ID=" + productID + "&Market_Basket_ID=" + marketBasketID + "&Timeframe=" + timeFrame + "&isMonth=" + isMonth + "&Trx_Mst=" + trxmst + "&Selected_Product_ID=" + selectedProductID + "&Field_Index=" + fieldIndex;
        
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
        
        clientManager.setContextValue("fhQuery", col);
            
        var url = "marketplaceanalytics/all/formularyhistoryreporting_popup.aspx?" + q;

        clientManager.openViewer(url, null, null, 950, 300);
        
        $("#infoPopup").draggable({ handle: "div.tileContainerHeader"});
    }

    function toggleGridRowSelection(gridID, key)
    {
        if  (gridID == 'ctl00_Tile4_chart_grid1_drillDownContainer')
        {
            $find("ctl00_Tile4_chart_grid2_drillDownContainer").clearSelections();
            $find("ctl00_Tile4_chart_grid3_drillDownContainer").clearSelections();
            showHideGrids(1, key);
        }
        if  (gridID == 'ctl00_Tile4_chart_grid2_drillDownContainer')
        {
            showHideGrids(2, key);
            $find("ctl00_Tile4_chart_grid1_drillDownContainer").clearSelections();
            $find("ctl00_Tile4_chart_grid3_drillDownContainer").clearSelections();
        }
        if  (gridID == 'ctl00_Tile4_chart_grid3_drillDownContainer')
        {
            $find("ctl00_Tile4_chart_grid1_drillDownContainer").clearSelections();
            $find("ctl00_Tile4_chart_grid2_drillDownContainer").clearSelections();
            showHideGrids(3, key);
        }
    }

    function chartClicked(chartNum)
    {
        $find("ctl00_Tile4_chart_grid1_drillDownContainer").clearSelections();
        $find("ctl00_Tile4_chart_grid2_drillDownContainer").clearSelections();
        $find("ctl00_Tile4_chart_grid3_drillDownContainer").clearSelections();
        showHideGrids(chartNum);
    }
</script>