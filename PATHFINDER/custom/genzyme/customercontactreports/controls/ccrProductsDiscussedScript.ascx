<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ccrProductsDiscussedScript.ascx.cs" Inherits="custom_controls_ccrProductsDiscussedScript" %>
<script type="text/javascript">

    clientManager.add_pageInitialized(ccrProductsDiscussedPageInitialized);
    clientManager.add_pageUnloaded(ccrProductsDiscussedPageUnloaded);
    
    function ccrProductsDiscussedPageInitialized()
    {
        clientManager.registerComponent('ctl00_Tile4_ccrProductsDiscussedData1_dataProduct1_gridCcrProductsDiscussed', null);
        clientManager.registerComponent('ctl00_Tile4_ccrProductsDiscussedData1_dataProduct2_gridCcrProductsDiscussed', null);

        //Hide Grid 2 always
        $('#ctl00_Tile4_ccrProductsDiscussedData1_dataProduct2_gridCcrProductsDiscussed').addClass('hiddenGrid');

        ////Highlight Chart 1 always
        //$('#ccrGrid1').addClass('selectedChart');

        //Grid Header colors are always same as Chart 1 header on load
        $("#tile4 .tileContainerHeader").css("background-color", "#20b1aa"); //LightSeaGreen
        $("#tile5 .tileContainerHeader").css("background-color", "#20b1aa");

        //Call scaleChart in UI.js to automatically resize charts
        scaleChart();       
    }


    function ccrProductsDiscussedPageUnloaded()
    {
        clientManager.remove_pageInitialized(ccrProductsDiscussedPageInitialized);
        clientManager.remove_pageUnloaded(ccrProductsDiscussedPageUnloaded);
    }

    function ccrProductsDiscussedDrilldown(productID, noColumn, ctrlClientId)
    {
       //Co-relate selected pie, and slice, to a specific grid, and row, when this function called from chart.
        if (noColumn >= 0)
        {
            var grid = null;
            if (ctrlClientId == "ctl00_Tile3_ccrChart1_chartProduct1_chart")
                grid = $find('ctl00_Tile4_ccrProductsDiscussedData1_dataProduct1_gridCcrProductsDiscussed');
            if (ctrlClientId == "ctl00_Tile3_ccrChart1_chartProduct2_chart")
                grid = $find('ctl00_Tile4_ccrProductsDiscussedData1_dataProduct2_gridCcrProductsDiscussed');

            var mt = grid.get_masterTableView();

            mt.selectItem(noColumn);
        }
        else //Call is from grid
        {
            var data = { Products_Discussed_ID: productID };

            data["Geography_ID"] = clientManager.get_SelectionData()["Geography_ID"];
            data["Is_National"] = clientManager.get_SelectionData()["Is_National"];

            //If YTD Chart or Grid is clicked, Contact_Date is always YTD
            if ((ctrlClientId == "ctl00_Tile3_ccrChart1_chartProduct1_chart") || (ctrlClientId == "ctl00_Tile4_ccrProductsDiscussedData1_dataProduct1_gridCcrProductsDiscussed"))
            {

                //Calculate YTD
                var today = new Date();
                var thisYear = today.getFullYear();
                var thisMonth = today.getMonth() + 1;
                var thisDay = today.getDate();
                var yearBeginDate = new Date("1/1/" + thisYear);
                var endDate = new Date(thisMonth + "/" + thisDay + "/" + thisYear);

                var contactDate = new Pathfinder.UI.dataParam("Contact_Date", [yearBeginDate.format("d"), today.format("d")], "System.DateTime", "Between", [yearBeginDate.format("d"), today.format("d")]);

                data["Contact_Date"] = contactDate;
            }
            else
                data["Contact_Date"] = clientManager.get_SelectionData()["Contact_Date"];

            data["Meeting_Activity_ID"] = clientManager.get_SelectionData()["Meeting_Activity_ID"];
            if (data["Meeting_Activity_ID"]) data["Meeting_Activity_ID"].isExtension = false;
            
            data["Meeting_Type_ID"] = clientManager.get_SelectionData()["Meeting_Type_ID"];
            data["Account_Name"] = clientManager.get_SelectionData()["Account_Name"];
            data["Section_ID"] = clientManager.get_SelectionData()["Section_ID"];

            if (clientManager.get_SelectionData()["Meeting_Outcome_ID"])
                data["Meeting_Outcome_ID"] = clientManager.get_SelectionData()["Meeting_Outcome_ID"];

            if (clientManager.get_SelectionData()["Followup_Notes_ID"])
                data["Followup_Notes_ID"] = clientManager.get_SelectionData()["Followup_Notes_ID"];

            if (clientManager.get_SelectionData()["User_ID"])
                data["User_ID"] = clientManager.get_SelectionData()["User_ID"];

            clientManager.set_SelectionData(data, 1);
        }

        //$("#ctl00_Tile5_ccrProductsDiscussedDrillDown1_gridCcrProductsDiscussedDrillDown_GridData").height($("#divTile5Container").height() - ($("#ctl00_Tile5_ccrProductsDiscussedDrillDown1_gridCcrProductsDiscussedDrillDown_GridHeader").height() + 26));
    }
 
    function gridCcrProductsDiscussed_OnRowSelecting(sender, args)
    {
        var productID = args._dataKeyValues.Products_Discussed_ID;

        //No need to Co-relate selected pie, and slice, to a specific grid, and row, when this function called from grid, itself.
        //Therefore, pass -1.
        ccrProductsDiscussedDrilldown(productID, -1, sender.ClientID);
    }
    
    function showHideGrids(gridNum)
    {
        switch (gridNum)
        {
            case "1":
                $('#ctl00_Tile4_ccrProductsDiscussedData1_dataProduct2_gridCcrProductsDiscussed').addClass('hiddenGrid');
                $('#ctl00_Tile4_ccrProductsDiscussedData1_dataProduct1_gridCcrProductsDiscussed').removeClass('hiddenGrid');
                //$('#ccrGrid1').addClass('selectedChart');
                //$('#ccrGrid2').removeClass('selectedChart');
                //$('#ccrGrid3').removeClass('selectedChart');
                $("#tile4 .tileContainerHeader").css("background-color", "#20b1aa"); //LightSeaGreen
                $("#tile5 .tileContainerHeader").css("background-color", "#20b1aa");
                break;
            case "2":
                $('#ctl00_Tile4_ccrProductsDiscussedData1_dataProduct1_gridCcrProductsDiscussed').addClass('hiddenGrid');
                $('#ctl00_Tile4_ccrProductsDiscussedData1_dataProduct2_gridCcrProductsDiscussed').removeClass('hiddenGrid');
                //$('#ccrGrid2').addClass('selectedChart');
                //$('#ccrGrid1').removeClass('selectedChart');
                //$('#ccrGrid3').removeClass('selectedChart');
                $("#tile4 .tileContainerHeader").css("background-color", "#fda601"); //Orange
                $("#tile5 .tileContainerHeader").css("background-color", "#fda601");
                break;
        }

        resetGridHeadersX(500);
    } 

</script>