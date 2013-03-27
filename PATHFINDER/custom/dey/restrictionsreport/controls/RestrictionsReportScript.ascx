<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RestrictionsReportScript.ascx.cs" Inherits="restrictionsreport_controls_RestrictionsReportScript" %>
<%@ OutputCache Shared="true" VaryByParam="None" Duration="100" %>

<script type="text/javascript">
    
    var tierCoverageInit = false;

    var drilldownTitle = "";
    clientManager.add_pageInitialized(pageInitialized);
    clientManager.add_pageInitialized(tierCoverageScaleChart, "divtiercoverageChart");
    clientManager.add_pageUnloaded(pageUnloaded);

    function pageInitialized() {
        if (clientManager.get_Module() == 'deyrestrictionsreport') {
            var gridDrilldown = $get("ctl00_Tile5_gridDrilldown_gridRestrictionsReportdrilldown").GridWrapper;
            gridDrilldown.add_dataBound(gridmedicalpharmacycoveragedrilldown_onDataBound);
            tierCoverageScaleChart();
            uiStateChanged(clientManager);
            //check if drill down already set due to restoreView
            clientManager.add_uiStateChanged(uiStateChanged);
        } else {
            var gridDrilldown = $get("ctl00_Tile3_gridDrilldown_gridRestrictionsReportdrilldownReport").GridWrapper;
            gridDrilldown.add_dataBound(gridrestrictionsdrilldownreport_onDataBound);
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

    //for QL Restriction Drilldown Report
    function gridrestrictionsdrilldownreport_onDataBound(sender, args) {
    }

    //for QL Restrictions Report Drilldown section
    function gridmedicalpharmacycoveragedrilldown_onDataBound(sender, args)
    {
        $(sender.get_element()).css("visibility", "visible");

        $("#restrictionsReportDrilldownTitle").html(drilldownTitle);

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
                drilldownTitle = String.format("All <span style='font-weight:bold'>{0}</span> Accounts having <span style='font-weight:bold'>{1}</span> on <span style='font-weight:bold'>{2}</span>", data["sectionName"], data["drugName"], data["criteriaName"]);
        }
        else
        {
            $("#restrictionsReportDrilldownTitle").html('<%= Resources.Resource.Message_Tier_Coverage_DrillDown %>');

            var gw = $get("ctl00_Tile5_gridDrilldown_gridRestrictionsReportdrilldown").GridWrapper;
            if (gw)
            {
                gw.clearGrid();
                $(gw.get_element()).css("visibility", "hidden");
            }
        }

        adjustTile5HeightForDrilldown();
    }

    function restrictionsReportDrilldown(region, drugID, criteriaID, drugName, criteriaName, sectionID, sectionName, multipleSections)
    {
        if (binding) return;
        binding = true;
        var data = { Geography_ID: new Pathfinder.UI.dataParam("Geography_ID", region, "System.String", "EqualTo"), Drug_ID: drugID, Criteria_ID: criteriaID, Section_ID: sectionID };

        //var channel = clientManager.get_SelectionData()["Section_ID"];
        //data["Section_ID"] = channel;

        //data["Market_Basket_ID"] = clientManager.get_SelectionData()["Market_Basket_ID"];
        
        data["Plan_ID"] = clientManager.get_SelectionData()["Plan_ID"];
        //Plan Classification ID
        //if (clientManager.get_SelectionData()["Plan_Classification_ID"])
        //    data["Plan_Classification_ID"] = clientManager.get_SelectionData()["Plan_Classification_ID"];
        if (clientManager.get_SelectionData()["Class_Partition"])
            data["Class_Partition"] = clientManager.get_SelectionData()["Class_Partition"];
            
        data["Rank"] = clientManager.get_SelectionData()["Rank"];
        data["Is_Predominant"] = clientManager.get_SelectionData()["Is_Predominant"];
        data["User_ID"] = clientManager.get_SelectionData()["User_ID"];

        //data["Pharmacy_Medical"] = pharmacyMedical;

        data["PA"] = clientManager.get_SelectionData()["PA"];
        data["QL"] = clientManager.get_SelectionData()["QL"];
        data["ST"] = clientManager.get_SelectionData()["ST"];
        //for getting the geography type id
        data["rcbGeographyType"] = clientManager.get_SelectionData()["rcbGeographyType"];
        //set these values as "options" so they don't get applied as filters.
        data["__options"] = { "criteriaName": criteriaName, "sectionName": sectionName, "drugName": drugName, "chartTitle": clientManager.getRegionNameByID(region) };

        clientManager.set_SelectionData(data, 1);
    }

    function OpenQLFormCriteria(planID, drugID, formularyID, segmentID, productID, restrictionID)
    {

        var app = clientManager.get_ApplicationManager();
        var url = app.getUrl("all", clientManager.get_Module(), "OpenQL.aspx");

        url = url + "?PlanID=" + planID + "&DrugID=" + drugID + "&FormularyID=" + formularyID + "&SegmentID=" + segmentID + "&ProductID=" + productID + "&RestrictionID=" + restrictionID;

        var mt = $get(gridRRDrillDownID).control.get_masterTableView();
        var cell;

        //Get the list of dataitems which matches the selected Drug_ID.
        var list = $.grep(mt.get_dataItems(), function(i) { if (i.get_dataItem()) return i.get_dataItem().Plan_ID == planID && i.get_dataItem().Formulary_ID == formularyID && i.get_dataItem().Product_ID == productID; else return false; }, false);

        //Get the cellIndex for selected cell.
        if (list && list.length > 0)
        {
            var col;
            var rect;

            col = mt.getColumnByUniqueName("QL");

            if (col)
            {
                cell = list[0].get_element().cells[col.get_element().cellIndex];
            }
            else
                throw new Error("Cannot find column 'QL'");
        }

        //Getting cell bounds.
        rect = Sys.UI.DomElement.getBounds(cell);

        //Open pop-up window with calculated co-ordinates.
        var width = 300;
        var height = 100;
        clientManager.openViewer(url, rect.x - 275, rect.y + 21, width, height);

        $("#infoPopup").draggable({ handle: "div.tileContainerHeader" });
    }


</script>