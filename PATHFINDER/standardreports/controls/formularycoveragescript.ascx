<%@ Control Language="C#" AutoEventWireup="true" CodeFile="formularycoveragescript.ascx.cs" Inherits="standardreports_controls_formularycoveragescript" %>
<%@ OutputCache Shared="true" VaryByParam="None" Duration="100" %>

<script type="text/javascript">
    
    var formularyCoverageInit = false;

    var drilldownTitle = "";
    clientManager.add_pageInitialized(pageInitialized);
    clientManager.add_pageInitialized(formularyCoverageScaleChart, "divformularystatusChart");
    clientManager.add_pageUnloaded(pageUnloaded);

    
    function pageInitialized() 
    {
        var gridDrilldown = $get("ctl00_Tile5_gridDrilldown_gridformularycoveragedrilldown").GridWrapper;

        gridDrilldown.add_dataBound(gridformularycoveragedrilldown_onDataBound);

        formularyCoverageScaleChart();
        
        //check if drill down already set due to restoreView
        uiStateChanged(clientManager);

        clientManager.add_uiStateChanged(uiStateChanged);

        EnableDisablePieChart();

    }

    function EnableDisablePieChart() 
    {
        var data = clientManager.get_SelectionData();
        
        if (data && (typeof (data['Drug_ID']) != "undefined")) 
        {
            if ($.isArray(data['Drug_ID'].value)) //If multiple drugs are selected then hide the pie chart link.
                $("#StdReportPieChart").hide();
            else
                $("#StdReportPieChart").show(); //else show the pie chart link.
        }
    }

    //When clicked on PieChart icon from formulary status report, it opens a popup window containing pie chart.
    // function OpenPieChartViewer(secids, x, y, width, height) {
    function OpenPieChartViewer(x, y, width, height) {
        var app = clientManager.get_ApplicationManager();
        var url = app.getUrl("all", clientManager.get_Module(), "OpenFormularyCoveragePieChart.aspx");

        var data = clientManager.get_SelectionData();
        if (data) 
        {
            if (typeof (data['Section_ID']) != "undefined") 
            {
                if (typeof (data['Selected_Section_ID']) == "undefined") 
                {
                    data["Selected_Section_ID"] = data["Section_ID"];
                }

                //if section_id has multiple values
                if ($.isArray(data['Selected_Section_ID'].value)) 
                {
                    //delete Segment_ID filter.    
                    if (typeof (data['Segment_ID']) != "undefined")
                        delete data["Segment_ID"];
                }
                else 
                {
                    //If Med-D is not selected then delete Segment_iD filter
                    var selSecIDVal = data['Selected_Section_ID'].value;
                    if (selSecIDVal != 17) 
                    {
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
 
    
    function pageUnloaded()
    {
        clientManager.remove_pageInitialized(pageInitialized);
        clientManager.remove_pageInitialized(scaleChart, "divformularystatusChart");
        clientManager.remove_pageUnloaded(pageUnloaded);
        clientManager.remove_uiStateChanged(uiStateChanged);
    }

    function formularyCoverageScaleChart()
    {
        scaleChart();
    }

    var binding = false;

    function gridformularycoveragedrilldown_onDataBound(sender, args)
    {
        $(sender.get_element()).css("visibility", "visible");

        $("#formularyCoverageDrilldownTitle").html(drilldownTitle);

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
                //drilldownTitle = String.format("All {0} accounts having {1} with formulary status {2}", data["chartTitle"], data["drugName"], data["formularystatusName"]);
                drilldownTitle = String.format("<%= Resources.Resource.Label_Formulary_Coverage_DrillDown %>", data["chartTitle"], data["drugName"], data["formularystatusName"]); 
        }
        else
        {
            $("#formularyCoverageDrilldownTitle").html('<%= Resources.Resource.Message_Formulary_Status_DrillDown %>');

            var gw = $get("ctl00_Tile5_gridDrilldown_gridformularycoveragedrilldown").GridWrapper;
            if (gw)
            {
                gw.clearGrid();
                $(gw.get_element()).css("visibility", "hidden");
            }
        }

        adjustTile5HeightForDrilldown();
    }

    function formularyCoverageDrilldown(region, drugID, formularystatusID, drugName, formularystatusName,sectionID)
    {
        if (binding) return;
        binding = true;

        var data = { Geography_ID: new Pathfinder.UI.dataParam("Geography_ID", region, "System.String", "EqualTo"), Drug_ID: drugID, Formulary_Status_ID: formularystatusID };

       // var channel = clientManager.get_SelectionData()["Section_ID"];
       // data["Section_ID"] = channel;
        if (sectionID > 0)
        {           
            data["Section_ID"] = sectionID;
        }
        if (sectionID == 17 && !clientManager.get_SelectionData()["Segment_ID"])
            data["Segment_ID"] = { name: "Segment_ID", value: 2 };
        else if (sectionID == 17 && clientManager.get_SelectionData()["Segment_ID"])
            data["Segment_ID"] = clientManager.get_SelectionData()["Segment_ID"];

        data["Thera_ID"] = clientManager.get_SelectionData()["Market_Basket_ID"];

        data["Plan_ID"] = clientManager.get_SelectionData()["Plan_ID"];
     
        if (clientManager.get_SelectionData()["Class_Partition"])
            data["Class_Partition"] = clientManager.get_SelectionData()["Class_Partition"];
            
        data["Rank"] = clientManager.get_SelectionData()["Rank"];
        data["Is_Predominant"] = clientManager.get_SelectionData()["Is_Predominant"];
        data["User_ID"] = clientManager.get_SelectionData()["User_ID"];
        data["onlyPBM"] = clientManager.get_SelectionData()["onlyPBM"];
        data["excludeSegment"] = clientManager.get_SelectionData()["excludeSegment"];
        data["Is_All_Section"] = clientManager.get_SelectionData()["Is_All_Section"];

        data["PA"] = clientManager.get_SelectionData()["PA"];
        data["QL"] = clientManager.get_SelectionData()["QL"];
        data["ST"] = clientManager.get_SelectionData()["ST"];
        //for getting the geography type id
        data["rcbGeographyType"] = clientManager.get_SelectionData()["rcbGeographyType"];
        //set these values as "options" so they don't get applied as filters.
        data["__options"] = { "formularystatusName": formularystatusName, "drugName": drugName, "chartTitle": clientManager.getRegionNameByID(region) };
        
        clientManager.set_SelectionData(data, 1);
    }

</script>
