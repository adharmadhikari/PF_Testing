<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.UI.UserControl" %>
<%@ OutputCache Shared="true" VaryByParam="None" Duration="100" %>

<%--


        WARNING:  Requires FormularyStatusScript.ascx to also be included on page.

--%>

<script type="text/javascript">


    function formularyReportPageInitialized(sender, args) 
    {
           //change scale function to use geographic coverage specific version
           formularyReportScaleChart = geographicCoverage_scaleChart;          

           var mapIsReady = false;

           if (flashSupported) {
               var j = $("#divTile1 object").remove();

               if ($.browser.msie) {
                   j.appendTo("#divReportMap");

                   if (!ensureMapIsLoaded(geographicCoverage_mapState(), "divReportMap")) {
                       if (!clientManager.get_MapIsReady())
                           new cmd(null, initMap, ["fmASEngine", geographicCoverage_mapState()], 500);
                       else
                           mapIsReady = true;
                   }
               }
           }
           else
               mapIsReady = true;

           if (mapIsReady)
               geographicCoverage_reloadMap();

           geographicCoverage_selectionChanged(sender);
           sender.add_regionChanged(geographicCoverage_regionChanged);
       }

       function formularyReportPageLoaded(sender, args) {
           
        if (!$.browser.msie)
            ensureMapIsLoaded(geographicCoverage_mapState(), "divReportMap");

        $("#divReportMap").mouseenter(fixMapTip);
    }

    function formularyReportPageUnloaded(sender, args) {
        
        //relocate map back to tile1
        if (flashSupported) {
            var j = $("#divReportMap object").remove();

            if ($.browser.msie)
                j.appendTo("#divTile1");

        }
        else
            $("#divReportMap map").remove();

        $("#divReportMap").unbind("mouseenter", fixMapTip);

        sender.remove_regionChanged(geographicCoverage_regionChanged);
        
        
    }

    function geographicCoverage_scaleChart() {
        scaleChart(4); 
    }
    function geographicCoverage_selectionChanged(sender, args) {
        
        if (!args || args.level == 0)
            sender.loadPage("standardreports/controls/FormularyStatusChartImage.aspx?" + sender.getSelectionDataForPostback(), "divformularystatusChart");
    }
    function geographicCoverage_regionChanged(sender, args) {
        var region = sender.get_Region();
        if (!region) region = "US";
        
        var data = clientManager.get_SelectionData();
        data["Geography_ID"] = region;
        //partial page update to avoid pageinitialize event and reducing page load time
        clientManager.loadPage("standardreports/controls/FormularyStatusChartImage.aspx?" + $getDataForPostback(data), "divformularystatusChart");

        $("#formularyStatusDrilldownTitle").html('<%= Resources.Resource.Message_Formulary_Status_DrillDown %>');

        var gw = $get("ctl00_Tile5_gridFSDrilldown_gridformularystatusdrilldown").GridWrapper;
        if (gw) {
            gw.clearGrid();
            $(gw.get_element()).css("visibility", "hidden");
        }

        adjustTile5HeightForDrilldown();
        if (!flashSupported)
            geographicCoverage_reloadMap();
    }
    function geographicCoverage_mapState()
    {
        var data = clientManager.get_SelectionData();
        //var sectionID = data["Section_ID"].value;
        var sectionID;
        var onlyPBM;
        if (data["Section_ID"] != sectionID)
        {
            sectionID = data["Section_ID"].value;
        }
        else if (data.onlyPBM != onlyPBM)
        {
            if (data.onlyPBM.value == true)
            {
                sectionID = 4;
            }
        }
        else
        {
            sectionID = 0;
        }

        var x;
        var SectionID_String = sectionID.toString();
        //        var SectionID_String = " ";
        //        for (x = 0; x < sectionID.length; x++) {
        //            SectionID_String = sectionID[x] + "," + SectionID_String;
        //        }
        //        var strLen = SectionID_String.length;
        //        SectionID_String = SectionID_String.slice(0, strLen - 2);
        var mapData = {};
        mapData["UserKey"] = clientManager.get_UserKey();
        mapData["Application"] = 3;
        if (clientManager.get_Region())
            mapData["Region"] = clientManager.get_Region();

        //Convert selected sections to Int Array
        var channels = [];
        if (data && SectionID_String)
        {
            var stringChannels = SectionID_String.split(",");

            for (var i = 0; i < stringChannels.length; i++) channels.push(parseInt(stringChannels[i], 10));
        }
        else
        {
            stringChannels = "1,4,6,9,11,12,17";
            stringChannels = stringChannels.split(",");

            for (var i = 0; i < stringChannels.length; i++) channels.push(parseInt(stringChannels[i], 10));
        }
        //mapData["Channel"] = data && SectionID_String ? SectionID_String : "1,6,9,11,12,17";
        mapData["Channel"] = channels;
        mapData["Drug"] = data ? data["Drug_ID"].value : 0;
        mapData["MarketBasket"] = data["Market_Basket_ID"].value;
        var restrictions = "";

        if (data["PA"])
            restrictions = "PA";
        if (data["QL"])
        {
            if (restrictions == "") restrictions = "QL";
            else restrictions = restrictions + "_QL";
        }
        if (data["ST"])
        {
            if (restrictions == "") restrictions = "ST";
            else restrictions = restrictions + "_ST";
        }
        mapData["restrictions"] = restrictions;

        return Sys.Serialization.JavaScriptSerializer.serialize(mapData);
    }

    function geographicCoverage_reloadMap() {
   
        if (flashSupported)
            fmThemeReloadAreas("areas/mapdata.ashx?s=" + geographicCoverage_mapState());
        else {
            $("#divTile1 map").remove();
            $("#divReportMap").load("map.aspx?cmd=restoreview&s=" + geographicCoverage_mapState() + " form>*");
        }
    }
    

   

</script>