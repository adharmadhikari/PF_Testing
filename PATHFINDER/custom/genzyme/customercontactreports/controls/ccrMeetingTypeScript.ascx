<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ccrMeetingTypeScript.ascx.cs" Inherits="custom_controls_MeetingTypeScript" %>
<%@ OutputCache Shared="true" VaryByParam="None" Duration="100" %>

<script type="text/javascript">
    
    clientManager.add_pageInitialized(ccrMeetingTypePageInitialized);
    clientManager.add_pageUnloaded(ccrMeetingTypePageUnloaded);
    function ccrMeetingTypePageInitialized()
    {
        clientManager.registerComponent('ctl00_Tile4_ccrMeetingTypeData1_dataProduct1_gridCcrMeetingType', null);
        clientManager.registerComponent('ctl00_Tile4_ccrMeetingTypeData1_dataProduct1National_gridCcrMeetingType', null);
        clientManager.registerComponent('ctl00_Tile4_ccrMeetingTypeData1_dataProduct2_gridCcrMeetingType', null);
        clientManager.registerComponent('ctl00_Tile4_ccrMeetingTypeData1_dataProduct2National_gridCcrMeetingType', null);
        clientManager.registerComponent('ctl00_Tile4_ccrMeetingTypeData1_dataProduct3_gridCcrMeetingType', null);
        clientManager.registerComponent('ctl00_Tile4_ccrMeetingTypeData1_dataProduct3National_gridCcrMeetingType', null);

        //Hide grids depending on data
        if ($find("ctl00_Tile4_ccrMeetingTypeData1_dataProduct1_gridCcrMeetingType"))
        {
            $('#ctl00_Tile4_ccrMeetingTypeData1_dataProduct2_gridCcrMeetingType').addClass('hiddenGrid');
            $('#ctl00_Tile4_ccrMeetingTypeData1_dataProduct3_gridCcrMeetingType').addClass('hiddenGrid');

            //Grid Header colors are Chart 1
            $("#tile4 .tileContainerHeader").css("background-color", "#20b1aa"); //LightSeaGreen
            $("#tile5 .tileContainerHeader").css("background-color", "#20b1aa");
        }
        else
        {
            if ($find("ctl00_Tile4_ccrMeetingTypeData1_dataProduct2_gridCcrMeetingType"))
            {
                $('#ctl00_Tile4_ccrMeetingTypeData1_dataProduct1_gridCcrMeetingType').addClass('hiddenGrid');
                $('#ctl00_Tile4_ccrMeetingTypeData1_dataProduct3_gridCcrMeetingType').addClass('hiddenGrid');

                //Grid Header colors are Chart 2
                $("#tile4 .tileContainerHeader").css("background-color", "#fda601"); //Orange
                $("#tile5 .tileContainerHeader").css("background-color", "#fda601");
            }
            else
            {
                if ($find("ctl00_Tile4_ccrMeetingTypeData1_dataProduct3_gridCcrMeetingType"))
                {
                    $('#ctl00_Tile4_ccrMeetingTypeData1_dataProduct1_gridCcrMeetingType').addClass('hiddenGrid');
                    $('#ctl00_Tile4_ccrMeetingTypeData1_dataProduct2_gridCcrMeetingType').addClass('hiddenGrid');

                    //Grid Header colors are Chart 3
                    $("#tile4 .tileContainerHeader").css("background-color", "#98cd35"); //YellowGreen
                    $("#tile5 .tileContainerHeader").css("background-color", "#98cd35");
                }
                else
                {
                    //There is no data so hide everything and show default colors
                    $('#ctl00_Tile4_ccrMeetingTypeData1_dataProduct1_gridCcrMeetingType').addClass('hiddenGrid');
                    $('#ctl00_Tile4_ccrMeetingTypeData1_dataProduct2_gridCcrMeetingType').addClass('hiddenGrid');
                    $('#ctl00_Tile4_ccrMeetingTypeData1_dataProduct3_gridCcrMeetingType').addClass('hiddenGrid');

                    $("#tile4 .tileContainerHeader").css("background-color", "#20b1aa"); //LightSeaGreen
                    $("#tile5 .tileContainerHeader").css("background-color", "#20b1aa");
                }
            }
        }

        //Hide National grids always
        $('#ctl00_Tile4_ccrMeetingTypeData1_dataProduct1National_gridCcrMeetingType').addClass('hiddenGrid');
        $('#ctl00_Tile4_ccrMeetingTypeData1_dataProduct2National_gridCcrMeetingType').addClass('hiddenGrid');
        $('#ctl00_Tile4_ccrMeetingTypeData1_dataProduct3National_gridCcrMeetingType').addClass('hiddenGrid');

        //Hide all National charts always
        $('#chart1National').addClass('hiddenGrid');
        $('#chart2National').addClass('hiddenGrid');
        $('#chart3National').addClass('hiddenGrid');
        
        //Highlight Chart 1 always
        //$('#ccrGrid1').addClass('selectedChart');

        //Call scaleChart in UI.js to automatically resize charts
        scaleChart();
    }

    function ccrMeetingTypePageUnloaded()
    {
        clientManager.remove_pageInitialized(ccrMeetingTypePageInitialized);
        clientManager.remove_pageUnloaded(ccrMeetingTypePageUnloaded);
    }

    function ccrmeetingtypeDrilldown(productID, meetingTypeID, noColumn, ctrlClientId)
    {
        //Co-relate selected pie, and slice, to a specific grid, and row, when this function called from chart.
        if (noColumn >= 0) //Call is from chart
        {
            var grid = null;
            if (ctrlClientId == "ctl00_Tile3_ccrChart1_chartProduct1_chart")
                grid = $find('ctl00_Tile4_ccrMeetingTypeData1_dataProduct1_gridCcrMeetingType');
            if (ctrlClientId == "ctl00_Tile3_ccrChart1_chartProduct2_chart")
                grid = $find('ctl00_Tile4_ccrMeetingTypeData1_dataProduct2_gridCcrMeetingType');
            if (ctrlClientId == "ctl00_Tile3_ccrChart1_chartProduct3_chart")
                grid = $find('ctl00_Tile4_ccrMeetingTypeData1_dataProduct3_gridCcrMeetingType');

            if (ctrlClientId == "ctl00_Tile3_ccrChart1_chartProduct1National_chart")
                grid = $find('ctl00_Tile4_ccrMeetingTypeData1_dataProduct1National_gridCcrMeetingType');
            if (ctrlClientId == "ctl00_Tile3_ccrChart1_chartProduct2National_chart")
                grid = $find('ctl00_Tile4_ccrMeetingTypeData1_dataProduct2National_gridCcrMeetingType');
            if (ctrlClientId == "ctl00_Tile3_ccrChart1_chartProduct3National_chart")
                grid = $find('ctl00_Tile4_ccrMeetingTypeData1_dataProduct3National_gridCcrMeetingType');

            var mt = grid.get_masterTableView();

            mt.selectItem(noColumn);
        }
        else //Call is from grid
        {
            var data = { Products_Discussed_ID: productID, Meeting_Type_ID: meetingTypeID };

            data["Contact_Date"] = clientManager.get_SelectionData()["Contact_Date"];
            
            data["Meeting_Activity_ID"] = clientManager.get_SelectionData()["Meeting_Activity_ID"];
            if (data["Meeting_Activity_ID"]) data["Meeting_Activity_ID"].isExtension = false;
            
            data["Plan_Name"] = clientManager.get_SelectionData()["Plan_Name"];
            data["Section_ID"] = clientManager.get_SelectionData()["Section_ID"];

            if (clientManager.get_SelectionData()["Meeting_Outcome_ID"])
                data["Meeting_Outcome_ID"] = clientManager.get_SelectionData()["Meeting_Outcome_ID"];

            if (clientManager.get_SelectionData()["Followup_Notes_ID"])
                data["Followup_Notes_ID"] = clientManager.get_SelectionData()["Followup_Notes_ID"];

            if (clientManager.get_SelectionData()["User_ID"])
                data["User_ID"] = clientManager.get_SelectionData()["User_ID"];

            //Non National Chart or Grid is clicked, get regular selection data
            if ((ctrlClientId == "ctl00_Tile3_ccrChart1_chartProduct1_chart") ||
                (ctrlClientId == "ctl00_Tile4_ccrMeetingTypeData1_dataProduct1_gridCcrMeetingType") ||
                (ctrlClientId == "ctl00_Tile3_ccrChart1_chartProduct2_chart") ||
                (ctrlClientId == "ctl00_Tile4_ccrMeetingTypeData1_dataProduct2_gridCcrMeetingType") ||
                (ctrlClientId == "ctl00_Tile3_ccrChart1_chartProduct3_chart") ||
                (ctrlClientId == "ctl00_Tile4_ccrMeetingTypeData1_dataProduct3_gridCcrMeetingType"))
            {
                data["Geography_ID"] = clientManager.get_SelectionData()["Geography_ID"];
                data["Is_National"] = clientManager.get_SelectionData()["Is_National"];
            }
            else //National Chart or Grid is clicked, modify selection data to obtain National
                data["Is_National"] = "1";

            clientManager.set_SelectionData(data, 1);
        }
    }
    
    function gridMeetingType_OnRowSelecting(sender, args) {
        var productID = args._dataKeyValues.Products_Discussed_ID;
        var meetingTypeID = args._dataKeyValues.Meeting_Type_ID;

        //No need to Co-relate selected pie, and slice, to a specific grid, and row, when this function called from grid, itself.
        //Therefore, pass, -1, -1.
        ccrmeetingtypeDrilldown(productID, meetingTypeID, -1, sender.ClientID);
    }
    
    function showHideGrids(gridNum)
    {
        switch (String(gridNum))
        {
            case "1":
                $('#ctl00_Tile4_ccrMeetingTypeData1_dataProduct2_gridCcrMeetingType').addClass('hiddenGrid');
                $('#ctl00_Tile4_ccrMeetingTypeData1_dataProduct2National_gridCcrMeetingType').addClass('hiddenGrid');
                $('#ctl00_Tile4_ccrMeetingTypeData1_dataProduct3_gridCcrMeetingType').addClass('hiddenGrid');
                $('#ctl00_Tile4_ccrMeetingTypeData1_dataProduct3National_gridCcrMeetingType').addClass('hiddenGrid');
                $('#ctl00_Tile4_ccrMeetingTypeData1_dataProduct1_gridCcrMeetingType').removeClass('hiddenGrid');
                $('#ctl00_Tile4_ccrMeetingTypeData1_dataProduct1National_gridCcrMeetingType').addClass('hiddenGrid');

                $("#tile4 .tileContainerHeader").css("background-color", "#20b1aa"); //LightSeaGreen
                $("#tile5 .tileContainerHeader").css("background-color", "#20b1aa");

                $("#ctl00_Tile3Tools_compareNational").attr("href", "javascript:CompareNational(1);");
                break;
            case "1National":
                $('#ctl00_Tile4_ccrMeetingTypeData1_dataProduct2_gridCcrMeetingType').addClass('hiddenGrid');
                $('#ctl00_Tile4_ccrMeetingTypeData1_dataProduct2National_gridCcrMeetingType').addClass('hiddenGrid');
                $('#ctl00_Tile4_ccrMeetingTypeData1_dataProduct3_gridCcrMeetingType').addClass('hiddenGrid');
                $('#ctl00_Tile4_ccrMeetingTypeData1_dataProduct3National_gridCcrMeetingType').addClass('hiddenGrid');
                $('#ctl00_Tile4_ccrMeetingTypeData1_dataProduct1_gridCcrMeetingType').addClass('hiddenGrid');
                $('#ctl00_Tile4_ccrMeetingTypeData1_dataProduct1National_gridCcrMeetingType').removeClass('hiddenGrid');

                $("#tile4 .tileContainerHeader").css("background-color", "#8dbd8b"); //DarkSeaGreen
                $("#tile5 .tileContainerHeader").css("background-color", "#8dbd8b");
                break;
            case "2":
                $('#ctl00_Tile4_ccrMeetingTypeData1_dataProduct1_gridCcrMeetingType').addClass('hiddenGrid');
                $('#ctl00_Tile4_ccrMeetingTypeData1_dataProduct1National_gridCcrMeetingType').addClass('hiddenGrid');
                $('#ctl00_Tile4_ccrMeetingTypeData1_dataProduct3_gridCcrMeetingType').addClass('hiddenGrid');
                $('#ctl00_Tile4_ccrMeetingTypeData1_dataProduct3National_gridCcrMeetingType').addClass('hiddenGrid');
                $('#ctl00_Tile4_ccrMeetingTypeData1_dataProduct2_gridCcrMeetingType').removeClass('hiddenGrid');
                $('#ctl00_Tile4_ccrMeetingTypeData1_dataProduct2National_gridCcrMeetingType').addClass('hiddenGrid');

                $("#tile4 .tileContainerHeader").css("background-color", "#fda601"); //Orange
                $("#tile5 .tileContainerHeader").css("background-color", "#fda601");

                $("#ctl00_Tile3Tools_compareNational").attr("href", "javascript:CompareNational(2);");
                break;
            case "2National":
                $('#ctl00_Tile4_ccrMeetingTypeData1_dataProduct1_gridCcrMeetingType').addClass('hiddenGrid');
                $('#ctl00_Tile4_ccrMeetingTypeData1_dataProduct1National_gridCcrMeetingType').addClass('hiddenGrid');
                $('#ctl00_Tile4_ccrMeetingTypeData1_dataProduct3_gridCcrMeetingType').addClass('hiddenGrid');
                $('#ctl00_Tile4_ccrMeetingTypeData1_dataProduct3National_gridCcrMeetingType').addClass('hiddenGrid');
                $('#ctl00_Tile4_ccrMeetingTypeData1_dataProduct2_gridCcrMeetingType').addClass('hiddenGrid');
                $('#ctl00_Tile4_ccrMeetingTypeData1_dataProduct2National_gridCcrMeetingType').removeClass('hiddenGrid');

                $("#tile4 .tileContainerHeader").css("background-color", "#f4a361"); //SandyBrown
                $("#tile5 .tileContainerHeader").css("background-color", "#f4a361");
                break;
            case "3":
                $('#ctl00_Tile4_ccrMeetingTypeData1_dataProduct1_gridCcrMeetingType').addClass('hiddenGrid');
                $('#ctl00_Tile4_ccrMeetingTypeData1_dataProduct1National_gridCcrMeetingType').addClass('hiddenGrid');
                $('#ctl00_Tile4_ccrMeetingTypeData1_dataProduct2_gridCcrMeetingType').addClass('hiddenGrid');
                $('#ctl00_Tile4_ccrMeetingTypeData1_dataProduct2National_gridCcrMeetingType').addClass('hiddenGrid');
                $('#ctl00_Tile4_ccrMeetingTypeData1_dataProduct3_gridCcrMeetingType').removeClass('hiddenGrid');
                $('#ctl00_Tile4_ccrMeetingTypeData1_dataProduct3National_gridCcrMeetingType').addClass('hiddenGrid');

                $("#tile4 .tileContainerHeader").css("background-color", "#98cd35"); //YellowGreen
                $("#tile5 .tileContainerHeader").css("background-color", "#98cd35");

                $("#ctl00_Tile3Tools_compareNational").attr("href", "javascript:CompareNational(3);");
                break;
            case "3National":
                $('#ctl00_Tile4_ccrMeetingTypeData1_dataProduct1_gridCcrMeetingType').addClass('hiddenGrid');
                $('#ctl00_Tile4_ccrMeetingTypeData1_dataProduct1National_gridCcrMeetingType').addClass('hiddenGrid');
                $('#ctl00_Tile4_ccrMeetingTypeData1_dataProduct2_gridCcrMeetingType').addClass('hiddenGrid');
                $('#ctl00_Tile4_ccrMeetingTypeData1_dataProduct2National_gridCcrMeetingType').addClass('hiddenGrid');
                $('#ctl00_Tile4_ccrMeetingTypeData1_dataProduct3_gridCcrMeetingType').addClass('hiddenGrid');
                $('#ctl00_Tile4_ccrMeetingTypeData1_dataProduct3National_gridCcrMeetingType').removeClass('hiddenGrid');

                $("#tile4 .tileContainerHeader").css("background-color", "#2b8c57"); //SeaGreen
                $("#tile5 .tileContainerHeader").css("background-color", "#2b8c57");
                break;
        }

        //Clear all grids
        var gw = $find("ctl00_Tile5_ccrMeetingTypeDrillDown1_gridMeetingTypeDrillDown$GridWrapper");
        gw.clearGrid();

        var grid = null;

        grid = $find('ctl00_Tile4_ccrMeetingTypeData1_dataProduct1_gridCcrMeetingType');        
        if (grid)
            grid.clearSelectedItems();
            
        grid = $find('ctl00_Tile4_ccrMeetingTypeData1_dataProduct2_gridCcrMeetingType');
        if (grid)
            grid.clearSelectedItems();
            
        grid = $find('ctl00_Tile4_ccrMeetingTypeData1_dataProduct3_gridCcrMeetingType');
        if (grid)
            grid.clearSelectedItems();
            
        grid = $find('ctl00_Tile4_ccrMeetingTypeData1_dataProduct1National_gridCcrMeetingType');
        if (grid)
            grid.clearSelectedItems();

        grid = $find('ctl00_Tile4_ccrMeetingTypeData1_dataProduct2National_gridCcrMeetingType');
        if (grid)
            grid.clearSelectedItems();
            
        grid = $find('ctl00_Tile4_ccrMeetingTypeData1_dataProduct3National_gridCcrMeetingType');
        if (grid)
            grid.clearSelectedItems();

        resetGridHeadersX(500);
    }

    function CompareNational(chartID)
    {
        $('#divCcrMeetingTypeChart > div').addClass('hiddenGrid');
        $('#chart' + chartID).removeClass('hiddenGrid');
        $('#chart' + chartID + 'National').removeClass('hiddenGrid');

        $("#ctl00_Tile3Tools_compareNational").attr("href", "javascript:CompareNationalHide(" + chartID + ");");
        $("#ctl00_Tile3Tools_compareNational").text("Cancel Comparison");

        //Call scaleChart in UI.js to automatically resize charts
        scaleChart();
    }

    function CompareNationalHide(chartID)
    {
        $('#divCcrMeetingTypeChart > div').removeClass('hiddenGrid');
        $('#chart1National').addClass('hiddenGrid');
        $('#chart2National').addClass('hiddenGrid');
        $('#chart3National').addClass('hiddenGrid');

        $("#ctl00_Tile3Tools_compareNational").attr("href", "javascript:CompareNational(" + chartID + ");");
        $("#ctl00_Tile3Tools_compareNational").text("National Comparison");

        //Change header colors back
        showHideGrids(chartID);

        //Call scaleChart in UI.js to automatically resize charts
        scaleChart();
    }
</script>