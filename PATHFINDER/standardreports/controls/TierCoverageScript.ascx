<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TierCoverageScript.ascx.cs" Inherits="standardreports_controls_TierCoverageScript" %>
<%@ OutputCache Shared="true" VaryByParam="None" Duration="100" %>

<script type="text/javascript">
    
    var tierCoverageInit = false;

    var drilldownTitle = "";
    clientManager.add_pageInitialized(pageInitialized);
    clientManager.add_pageInitialized(tierCoverageScaleChart, "divtiercoverageChart");
    clientManager.add_pageUnloaded(pageUnloaded);

    
    function pageInitialized() 
    {
        var gridDrilldown = $get("ctl00_Tile5_gridDrilldown_gridtiercoveragedrilldown").GridWrapper;

        gridDrilldown.add_dataBound(gridtiercoveragedrilldown_onDataBound);

        tierCoverageScaleChart();
        
        //check if drill down already set due to restoreView
        uiStateChanged(clientManager);

        clientManager.add_uiStateChanged(uiStateChanged);
        
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
    
    function pageUnloaded()
    {
        clientManager.remove_pageInitialized(pageInitialized);
        clientManager.remove_pageInitialized(scaleChart, "divtiercoverageChart");
        clientManager.remove_pageUnloaded(pageUnloaded);
        clientManager.remove_uiStateChanged(uiStateChanged);
    }

    function tierCoverageScaleChart()
    {
        scaleChart();
    }

    var binding = false;
    
    function gridtiercoveragedrilldown_onDataBound(sender, args)
    {
        $(sender.get_element()).css("visibility", "visible");

        $("#tierCoverageDrilldownTitle").html(drilldownTitle);

        adjustTile5HeightForDrilldown();

        binding = false;
    }

    function uiStateChanged(sender, args)
    {
        //If data drill down otherwise clear grid.
        var data = clientManager.get_SelectionData(1);
        if (data)
        {
            data = data["__options"];
            if (data)
                drilldownTitle = String.format("<%= Resources.Resource.Label_Tier_Coverage_DrillDown %>", data["chartTitle"], data["drugName"], data["tierName"]);
        }
        else
        {
            $("#tierCoverageDrilldownTitle").html('<%= Resources.Resource.Message_Tier_Coverage_DrillDown %>');

            var gw = $get("ctl00_Tile5_gridDrilldown_gridtiercoveragedrilldown").GridWrapper;
            if (gw)
            {
                gw.clearGrid();
                $(gw.get_element()).css("visibility", "hidden");
            }
        }

        adjustTile5HeightForDrilldown();
    }

    //When clicked on PieChart icon from formulary status report, it opens a popup window containing pie chart.
    function OpenTCPieChartViewer(x, y, width, height) {
        var app = clientManager.get_ApplicationManager();
        var url = app.getUrl("all", clientManager.get_Module(), "OpenTierCoveragePieChart.aspx");

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
                data["Selected_Section_ID"] = "0";

            data["Section_ID"] = repSecIDs.split(',');
        }

        var q;
        q = clientManager.getSelectionDataForPostback();
        url = url + "?" + q;

        var oManager = GetRadWindowManager();
        var oWnd = radopen(url, "PieChart");
        oWnd.setSize(700, 600);
        oWnd.Center();
    }  

    function tierCoverageDrilldown(region, drugID, tierID, drugName, tierName, sectionID)
    {
        if (binding) return;
        binding = true;
        var data = { Geography_ID: new Pathfinder.UI.dataParam("Geography_ID", region, "System.String", "EqualTo"), Drug_ID: drugID, Tier_ID: tierID };
        if (sectionID > 0)
        {
            data["Section_ID"] = sectionID;
        }
        if (sectionID == 17 && !clientManager.get_SelectionData()["Segment_ID"])
            data["Segment_ID"] = { name: "Segment_ID", value: 2 };
        else if (sectionID == 17 && clientManager.get_SelectionData()["Segment_ID"])
            data["Segment_ID"] = clientManager.get_SelectionData()["Segment_ID"];
        //        else //exlude pbm
        //            data = { Geography_ID: new Pathfinder.UI.dataParam("Geography_ID", region, "System.String", "EqualTo"), Drug_ID: drugID, Tier_ID: tierID, Section_ID: new Pathfinder.UI.dataParam("Section_ID", 4, "System.Int32", "NotEqualTo") };

        data["Market_Basket_ID"] = clientManager.get_SelectionData()["Market_Basket_ID"];

        data["Plan_ID"] = clientManager.get_SelectionData()["Plan_ID"];
        //Plan Classification ID
        //if (clientManager.get_SelectionData()["Plan_Classification_ID"])
        //    data["Plan_Classification_ID"] = clientManager.get_SelectionData()["Plan_Classification_ID"];
        if (clientManager.get_SelectionData()["Class_Partition"])
            data["Class_Partition"] = clientManager.get_SelectionData()["Class_Partition"];

        data["Rank"] = clientManager.get_SelectionData()["Rank"];
        data["Is_Predominant"] = clientManager.get_SelectionData()["Is_Predominant"];
        data["User_ID"] = clientManager.get_SelectionData()["User_ID"];
        data["onlyPBM"] = clientManager.get_SelectionData()["onlyPBM"];
        data["Is_All_Section"] = clientManager.get_SelectionData()["Is_All_Section"];
        data["excludeSegment"] = clientManager.get_SelectionData()["excludeSegment"];

        data["PA"] = clientManager.get_SelectionData()["PA"];
        data["QL"] = clientManager.get_SelectionData()["QL"];
        data["ST"] = clientManager.get_SelectionData()["ST"];
        //for getting the geography type id
        data["rcbGeographyType"] = clientManager.get_SelectionData()["rcbGeographyType"];
        //set these values as "options" so they don't get applied as filters.
        data["__options"] = { "tierName": tierName, "drugName": drugName, "chartTitle": clientManager.getRegionNameByID(region) };

        clientManager.set_SelectionData(data, 1);
    }   
</script>