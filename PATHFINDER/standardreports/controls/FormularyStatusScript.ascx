<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FormularyStatusScript.ascx.cs" Inherits="standardreports_controls_FormularyStatusScript" %>
<%@ OutputCache Shared="true" VaryByParam="None" Duration="100" %>

<%--


        WARNING:  This script code is used by both the Formulary Status Report and Geographic Coverage report.  
        
        If you modify this code please test changes with both reports.


--%>

<script type="text/javascript">

    var f1Text = "<%= Resources.Resource.Label_Formulary_Status_1 %>";
    var f2Text = "<%= Resources.Resource.Label_Formulary_Status_2 %>";
    var f3Text = "<%= Resources.Resource.Label_Formulary_Status_3 %>";
    var f4Text = "Other Restrictions";
    var f5Text = "<%= Resources.Resource.Label_Formulary_Status_5%>";

    var drilldownTitle = "";
    var binding = false;
    
    var formularyReportPageInitialized = null;
    var formularyReportPageLoaded = null;
    var formularyReportPageUnloaded = null;
    var formularyStatusInit = false;
    var formularyReportScaleChart = function() { scaleChart(); };

    clientManager.add_pageInitialized(pageInitialized);
    clientManager.add_pageLoaded(pageLoaded);
    clientManager.add_pageUnloaded(pageUnloaded);

    function pageInitialized(sender, args)
    {
        if (formularyReportPageInitialized)
            formularyReportPageInitialized(sender, args);

        sender.add_pageInitialized(formularyReportScaleChart, "divformularystatusChart");

        var gridDrilldown = $get("ctl00_Tile5_gridFSDrilldown_gridformularystatusdrilldown").GridWrapper;        
        gridDrilldown.add_dataBound(gridformularystatusdrilldown_onDataBound);

        formularyReportScaleChart();

        //check if drill down already set due to restoreView
        uiStateChanged(clientManager);

        sender.add_uiStateChanged(uiStateChanged);
        
        EnableDisablePieChart();

    }

    function EnableDisablePieChart() {
        var data = clientManager.get_SelectionData();

        if (data && (typeof (data['Drug_ID']) != "undefined")) {
            if ($.isArray(data['Drug_ID'].value)) //If multiple drugs are selected then hide the pie chart link.
                $("#StdReportPieChart").hide();
            else
                $("#StdReportPieChart").show(); //else show the pie chart link.
        }
    }

    function pageLoaded(sender, args)
    {
        if (formularyReportPageLoaded)
            formularyReportPageLoaded(sender, args);
    }

    function pageUnloaded(sender, args)
    {
        if (formularyReportPageUnloaded)
            formularyReportPageUnloaded(sender, args);

        sender.remove_pageInitialized(pageInitialized);
        sender.remove_pageLoaded(pageLoaded);
        sender.remove_pageInitialized(formularyReportScaleChart, "divformularystatusChart");
        sender.remove_pageUnloaded(pageUnloaded);
        sender.remove_uiStateChanged(uiStateChanged);
    }

    //When clicked on PieChart icon from formulary status report, it opens a popup window containing pie chart.
    function OpenFSPieChartViewer(x, y, width, height) {
        var app = clientManager.get_ApplicationManager();
        var url = app.getUrl("all", clientManager.get_Module(), "OpenFormularyStatusPieChart.aspx");

        var data = clientManager.get_SelectionData();

        if (data) {
            if (typeof (data['Section_ID']) != "undefined") {
                if (typeof (data['Selected_Section_ID']) == "undefined") {
                    data["Selected_Section_ID"] = data["Section_ID"];
                }

                //if section_id has multiple values
                if ($.isArray(data['Selected_Section_ID'].value)) {
                    //delete Segment_ID filter.    
                    if (typeof (data['Segment_ID']) != "undefined")
                        delete data["Segment_ID"];
                }
                else {
                    //If Med-D is not selected then delete Segment_iD filter
                    var selSecIDVal = data['Selected_Section_ID'].value;
                    if (selSecIDVal != 17) {
                        if (typeof (data['Segment_ID']) != "undefined")
                            delete data["Segment_ID"];
                    }
                }
                delete data["Section_ID"];
            }
            else
                data["Selected_Section_ID"] = "0"; //if All section is selected



            data["Section_ID"] = repSecIDs.split(',');
        }

        var q;
        q = clientManager.getSelectionDataForPostback();
        url = url + "?" + q;

        var oManager = GetRadWindowManager();
        var oWnd = radopen(url, "PieChart");
        oWnd.setSize(750, 600);
        oWnd.Center();
    }  
    

    function uiStateChanged(sender, args)
    {
        //If data drill down otherwise clear grid.
        var data = clientManager.get_SelectionData(1);
        if (data)
        {
            data = data["__options"];
            if (data)
                drilldownTitle = String.format("<%= Resources.Resource.Label_Formulary_Status_DrillDown %>", data["chartTitle"], data["drugName"], data["coverageStatus"]);
        }
        else
        {
            $("#formularyStatusDrilldownTitle").html('<%= Resources.Resource.Message_Formulary_Status_DrillDown %>');

            var gw = $get("ctl00_Tile5_gridFSDrilldown_gridformularystatusdrilldown").GridWrapper;
            if (gw)
            {
                gw.clearGrid();
                $(gw.get_element()).css("visibility", "hidden");
            }
        }
        adjustTile5HeightForDrilldown();
    }

    function gridFSDrilldown_setfilter(drugID, drugName, coverageStatusID, coverageStatus, region, sectionID) {
        if (binding) return;
        binding = true;


        var grid = $get("ctl00_Tile5_gridFSDrilldown_gridformularystatusdrilldown").control;
        var masterTable = grid.get_masterTableView();

        var data = {};
        if (sectionID > 0)
            data["Section_ID"] = sectionID;
            
        if (sectionID == 17 && !clientManager.get_SelectionData()["Segment_ID"])
            data["Segment_ID"] = { name: "Segment_ID", value: 2 };
        else if (sectionID == 17 && clientManager.get_SelectionData()["Segment_ID"])
            data["Segment_ID"] = clientManager.get_SelectionData()["Segment_ID"];        

        data["Geography_ID"] = new Pathfinder.UI.dataParam("Geography_ID", region, "System.String", "EqualTo");

        data["Market_Basket_ID"] = clientManager.get_SelectionData()["Market_Basket_ID"];
        data["Plan_ID"] = clientManager.get_SelectionData()["Plan_ID"];

        data["Drug_ID"] = drugID;

        if (coverageStatusID != 4)
            data["Coverage_Status_ID"] = coverageStatusID;
        else
            data["Coverage_Status_ID"] = 2; //still restriction if 4

        data["Rank"] = clientManager.get_SelectionData()["Rank"];

        data["User_ID"] = clientManager.get_SelectionData()["User_ID"];

        data["Is_Predominant"] = clientManager.get_SelectionData()["Is_Predominant"];
        data["onlyPBM"] = clientManager.get_SelectionData()["onlyPBM"];
        data["Is_All_Section"] = clientManager.get_SelectionData()["Is_All_Section"];
        data["excludeSegment"] = clientManager.get_SelectionData()["excludeSegment"];    

        //Plan Classification ID
        //        if (clientManager.get_SelectionData()["Plan_Classification_ID"])
        //            data["Plan_Classification_ID"] = clientManager.get_SelectionData()["Plan_Classification_ID"];
        if (clientManager.get_SelectionData()["Class_Partition"])
            data["Class_Partition"] = clientManager.get_SelectionData()["Class_Partition"];


        if (coverageStatusID == 2) {
            var d = clientManager.get_SelectionData();

            if (d["PA"])
                data["PA"] = d["PA"];
            if (d["QL"])
                data["QL"] = d["QL"];
            if (d["ST"])
                data["ST"] = d["ST"];
            if (!d["PA"] && !d["QL"] && !d["ST"])
                data["Coverage_Status_ID"] = 4;  //for setting no records if no restrictions are selected for covered with selected restrictions lives
        }
        else if (coverageStatusID == 4) //if Coverage_Status_ID=4 then use 'Other_Lives' field
        {
            var d = clientManager.get_SelectionData();
            //FYI:  DataService requires "IsNull" filter type to work while export (which uses Generic query gen) needs value to be #NULL
            if (d["PA"])
                data["PA"] = new Pathfinder.UI.dataParam("PA", "#NULL", "System.String", "IsNull");
            if (d["QL"])
                data["QL"] = new Pathfinder.UI.dataParam("QL", "#NULL", "System.String", "IsNull");
            if (d["ST"])
                data["ST"] = new Pathfinder.UI.dataParam("ST", "#NULL", "System.String", "IsNull");

            data["_others"] = new Pathfinder.UI.dataParam("_others", "true", "System.String", "EqualTo", null, true);
        }        
        
        //    if (typeof channel != 'undefined') {
        //        grid.get_masterTableView().hideColumn(0);
        //    };


        //data["Section_ID"] = sectionID;
        data["rcbGeographyType"] = clientManager.get_SelectionData()["rcbGeographyType"];

        //set these values as "options" so they don't get applied as filters.
        data["__options"] = { "drugName": drugName, "coverageStatus": coverageStatus, "chartTitle": clientManager.getRegionNameByID(region) };

        clientManager.set_SelectionData(data, 1);
    }


    function gridformularystatusdrilldown_onDataBound(sender, args)
    {
        $(sender.get_element()).css("visibility", "visible");

        $("#formularyStatusDrilldownTitle").html(drilldownTitle);

        adjustTile5HeightForDrilldown();

        binding = false;
    }
    
</script>