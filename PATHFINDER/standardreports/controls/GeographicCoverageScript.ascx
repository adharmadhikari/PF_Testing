<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.UI.UserControl" %>
<%@ OutputCache Shared="true" VaryByParam="None" Duration="100" %>

<%--


        WARNING:  Requires FormularyStatusScript.ascx to also be included on page.

--%>

<script type="text/javascript">

    var mbList;
    var drugList;

    function formularyReportPageInitialized(sender, args)
    {
        //change scale function to use geographic coverage specific version
        formularyReportScaleChart = geographicCoverage_scaleChart;

        //        sender.registerComponent("ctl00_Tile3Title_rdlChannel");
        sender.registerComponent("ctl00_Tile3Title_rdlMarketBasketList");
        //setting rdlDrugList as updateable even though it technically isn't - it just tricks the page into thinking auto updates will occur - they are actually handled by event handlers below
        sender.registerComponent("ctl00_Tile3Title_rdlDrugList", null, true);

        //        var channelList = $find("ctl00_Tile3Title_rdlChannel");
        //load Market Basket and Drug Lists
        mbList = $find("ctl00_Tile3Title_rdlMarketBasketList");
        drugList = $find("ctl00_Tile3Title_rdlDrugList");

        var id = $loadMenuItems(mbList, sender.get_MarketBasketListOptions(), null, sender.get_MarketBasket());
        sender.set_MarketBasket(id);

        id = $loadMenuItems(drugList, sender.get_DrugListOptions()[id], null, sender.get_Drug());
        sender.set_Drug(id);

        //hook up event handlers for MB and Drug list so we can update CM with new values (which then triggers map updates and chart updates)
        //        channelList.add_itemClicked(geographicCoverage_onChannelChanged);
        sender.add_channelChanged(geographicCoverage_onChannelChanged);
        mbList.add_itemClicked(geographicCoverage_onMarketBasketChanged);
        drugList.add_itemClicked(geographicCoverage_onDrugChanged);
        
        //when report is first viewed (not during restore) the selection data will be empty so fill it in
        if (!sender.get_SelectionData())
        {
            var region = sender.get_Region();
            if (!region) region = "US";
            //            var channel = channelList.get_items().getItem(0).get_items().getItem(0).get_value();
            
            //Set Class Partition to 2 if Commercial or Med D Channels or Managed Medicaid is selected
            if (sender.get_Channel() == 1 || sender.get_Channel() == 17 || sender.get_Channel() == 6)
                sender.set_SelectionData({ "Class_Partition": new Pathfinder.UI.dataParam("Class_Partition", 2, "System.Int32", "EqualTo"), "PA": "PA", Geography_ID: region, Market_Basket_ID: sender.get_MarketBasket(), Drug_ID: sender.get_Drug(), Section_ID: new Pathfinder.UI.dataParam("Section_ID", sender.get_Channel(), "System.Int32", "EqualTo") });                   
            else
                sender.set_SelectionData({ "Class_Partition": new Pathfinder.UI.dataParam("Class_Partition", "#NULL", "System.Int32", "IsNull"), "PA": "PA", Geography_ID: region, Market_Basket_ID: sender.get_MarketBasket(), Drug_ID: sender.get_Drug(), Section_ID: new Pathfinder.UI.dataParam("Section_ID", sender.get_Channel(), "System.Int32", "EqualTo") });
        }

        var mapIsReady = false;

        if (flashSupported)
        {
            var j = $("#divTile1 object").remove();
            //    else
            //        j = $("#divTile1 div:first").remove();
            if ($.browser.msie)
            {
                j.appendTo("#divReportMap");

                if (!ensureMapIsLoaded(geographicCoverage_mapState(), "divReportMap"))
                {
                    if (!clientManager.get_MapIsReady())
                        new cmd(null, initMap, ["fmASEngine", geographicCoverage_mapState()], 500);
                    else
                        mapIsReady = true;
                }
            }
        }
        else
            mapIsReady = true;
        //        $refreshMenuOptions(channelList, sender.get_SelectionData()["Section_ID"].value);

        if (mapIsReady)
            geographicCoverage_reloadMap();

        sender.add_regionChanged(geographicCoverage_regionChanged);
        sender.add_selectionChanged(geographicCoverage_selectionChanged);

        geographicCoverage_selectionChanged(sender);
    }
    
    function formularyReportPageLoaded(sender, args)
    {
        if (!$.browser.msie)
            ensureMapIsLoaded(geographicCoverage_mapState(), "divReportMap");
            
        $("#divReportMap").mouseenter(fixMapTip);
    }
    
    function formularyReportPageUnloaded(sender, args)
    {
        //relocate map back to tile1
        if (flashSupported)
        {
            var j = $("#divReportMap object").remove();

            if ($.browser.msie)
                j.appendTo("#divTile1");

        }
        else
            $("#divReportMap map").remove();
            
        $("#divReportMap").unbind("mouseenter", fixMapTip);
        
        
        sender.remove_channelChanged(geographicCoverage_onChannelChanged);

        sender.remove_regionChanged(geographicCoverage_regionChanged);
        sender.remove_selectionChanged(geographicCoverage_selectionChanged);
    }

    function geographicCoverage_scaleChart()
    {
        scaleChart(4);
    }

    function geographicCoverage_onMarketBasketChanged(sender, args)
    {
        var mid = args.get_item().get_value();

        if (mid)
        {
            var id = $loadMenuItems(drugList, clientManager.get_DrugListOptions()[mid], null, clientManager.get_Drug());
            clientManager.set_MarketBasket(mid);

            var data = clientManager.get_SelectionData();
            data["Market_Basket_ID"] = mid;
            data["Drug_ID"] = id;
            clientManager.set_SelectionData(data);

            geographicCoverage_reloadMap();
            $refreshMenuOptions(sender, mid);
        }
    }

    function geographicCoverage_onDrugChanged(sender, args)
    {
        var id = args.get_item().get_value();

        if (id)
        {
            clientManager.set_Drug(id);

            var data = clientManager.get_SelectionData();
            data["Drug_ID"] = id;
            clientManager.set_SelectionData(data);

            geographicCoverage_reloadMap();
            $refreshMenuOptions(sender, id);
        }
    }

    function geographicCoverage_onChannelChanged(sender, args)
    {
        //var id = args.get_item().get_value();
        var id = sender.get_Channel();
        
        if (id)
        {
            var data = clientManager.get_SelectionData();
            data["Section_ID"] = new Pathfinder.UI.dataParam("Section_ID", id, "System.Int32", "EqualTo");

            //Set Class Partition to 2 if Commercial or Med D Channels selected
            if (id == 1 || id == 17)
                data["Class_Partition"] = new Pathfinder.UI.dataParam("Class_Partition", 2, "System.Int32", "EqualTo");
            else
                data["Class_Partition"] = new Pathfinder.UI.dataParam("Class_Partition", "#NULL", "System.Int32", "IsNull");
                
            clientManager.set_SelectionData(data);

            geographicCoverage_reloadMap();

//            $refreshMenuOptions(sender, id);
        }
    }

    function geographicCoverage_selectionChanged(sender, args) 
    {
        if(!args || args.level == 0)
            sender.loadPage("standardreports/controls/FormularyStatusChartImage.aspx?" + sender.getSelectionDataForPostback(), "divformularystatusChart");
    }
    
    function geographicCoverage_regionChanged(sender, args)
    {
        var region = sender.get_Region();
        if (!region) region = "US";
        
        var data = clientManager.get_SelectionData();
        data["Geography_ID"] = region;
        clientManager.set_SelectionData(data);

        if(!flashSupported)
            geographicCoverage_reloadMap();        
    }

    function geographicCoverage_mapState()
    {
//        var data = clientManager.get_SelectionData();
//        var mapData = {};
//        mapData["UserKey"] = clientManager.get_UserKey();
//        mapData["Application"] = 3;
//        if (clientManager.get_Region())
//            mapData["Region"] = clientManager.get_Region();
//        mapData["Channel"] = data && data["Section_ID"] ? data["Section_ID"].value : 1;
//        mapData["Drug"] = data ? data["Drug_ID"] : 0;
        //        return Sys.Serialization.JavaScriptSerializer.serialize(mapData);

        var data = clientManager.get_SelectionData();
        //var sectionID = data["Section_ID"].value;
        var sectionID;
        if (data["Section_ID"] != sectionID) {
            sectionID = data["Section_ID"].value;
        }
        else {
            sectionID = 0;
        }

        var x;
        var SectionID_String = " ";
        for (x = 0; x < sectionID.length; x++) {
            SectionID_String = sectionID[x] + "," + SectionID_String;
        }
        var strLen = SectionID_String.length;
        SectionID_String = SectionID_String.slice(0, strLen - 2);
        var mapData = {};
        mapData["UserKey"] = clientManager.get_UserKey();
        mapData["Application"] = 3;
        if (clientManager.get_Region())
            mapData["Region"] = clientManager.get_Region();
        mapData["Channel"] = data && SectionID_String ? SectionID_String : "1,4,6,9,17";
        mapData["Drug"] = data ? data["Drug_ID"].value : 0;
        mapData["MarketBasket"] = data["Market_Basket_ID"].value;
        var restrictions = "";

        if (data["PA"])
            restrictions = "PA";
        if (data["QL"]) {
            if (restrictions == "") restrictions = "QL";
            else restrictions = restrictions + "_QL";
        }
        if (data["ST"]) {
            if (restrictions == "") restrictions = "ST";
            else restrictions = restrictions + "_ST";
        }
        mapData["restrictions"] = restrictions;

        return Sys.Serialization.JavaScriptSerializer.serialize(mapData);


    }

    function geographicCoverage_reloadMap()
    {
        if (flashSupported)
            fmThemeReloadAreas("areas/mapdata.ashx?s=" + geographicCoverage_mapState());
        else
        {
            $("#divTile1 map").remove();
            $("#divReportMap").load("map.aspx?cmd=restoreview&s=" + geographicCoverage_mapState() + " form>*");
        }
    }

</script>