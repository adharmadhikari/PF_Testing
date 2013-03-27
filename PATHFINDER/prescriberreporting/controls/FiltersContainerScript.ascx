<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FiltersContainerScript.ascx.cs" Inherits="prescriberreporting_controls_FiltersContainerScript" %>
<%@ OutputCache Shared="true" VaryByParam="None" Duration="1" %>  
  
<script type="text/javascript">

    clientManager.add_pageLoaded(filters_pageLoaded, "moduleOptionsContainer");
    clientManager.add_pageUnloaded(filters_pageUnloaded, "moduleOptionsContainer");

    var _filterPageDefaults = [];
    
    function filters_pageLoaded(sender, args)
    {
        $(".navbar2").show();
        
        $clearAlert(); //make sure any previous alerts are cleared.
        
        $addHandler($get("requestReportButton"), "click", requestReport);
        $addHandler($get("clearFiltersButton"), "click", clearReportFilters);

        $("#timeframeArea").show();

        //Set timeframe selector based on module selected
        var pageName;
        pageName = "filtertimeframe.aspx";
        
        clientManager.loadPage(clientManager.get_ApplicationManager().getUrl("all", "all", pageName) + "?_userkey=" + clientManager.get_UserKey(), "timeframeArea");
        
        //Initialize click handler for Calendar/Rolling selector
        $('#filterCalendarRolling input:radio').click(function()
        {
            //Check if calendar
            if ($("#ctl00_partialPage_filtersContainer_CalendarRolling_Calendar_Rolling_0").attr('checked'))
                showCalendar();

            //Check if rolling
            if ($("#ctl00_partialPage_filtersContainer_CalendarRolling_Calendar_Rolling_1").attr('checked'))
                showRolling();
        });
        
        $reloadContainer("moduleOptionsContainer", clientManager.get_SelectionData());

        $clearAlert();
                             
        todaysanalytics_content_resize ();          
    }

    function showCalendar()
    {
        $("#timeFrameCalendar").show();
        $("#ctl00_partialPage_filterTimeComparisonFrame_filterTimeFrameComparisonSelector1_timeFrameCalendar").show();
        $("#ctl00_partialPage_filterTimeComparisonFrame_filterTimeFrameComparisonSelector2_timeFrameCalendar").show();
        $("#ctl00_partialPage_filterTimeComparisonFrame_filterTimeFrameComparisonSelector3_timeFrameCalendar").show();

        $("#timeFrameRolling").hide();
        $("#ctl00_partialPage_filterTimeComparisonFrame_filterTimeFrameComparisonSelector1_timeFrameRolling").hide();
        $("#ctl00_partialPage_filterTimeComparisonFrame_filterTimeFrameComparisonSelector2_timeFrameRolling").hide();
        $("#ctl00_partialPage_filterTimeComparisonFrame_filterTimeFrameComparisonSelector3_timeFrameRolling").hide();
        
    }

    function showRolling()
    {
        $("#timeFrameCalendar").hide();
        $("#ctl00_partialPage_filterTimeComparisonFrame_filterTimeFrameComparisonSelector1_timeFrameCalendar").hide();
        $("#ctl00_partialPage_filterTimeComparisonFrame_filterTimeFrameComparisonSelector2_timeFrameCalendar").hide();
        $("#ctl00_partialPage_filterTimeComparisonFrame_filterTimeFrameComparisonSelector3_timeFrameCalendar").hide();

        $("#timeFrameRolling").show();
        $("#ctl00_partialPage_filterTimeComparisonFrame_filterTimeFrameComparisonSelector1_timeFrameRolling").show();
        $("#ctl00_partialPage_filterTimeComparisonFrame_filterTimeFrameComparisonSelector2_timeFrameRolling").show();
        $("#ctl00_partialPage_filterTimeComparisonFrame_filterTimeFrameComparisonSelector3_timeFrameRolling").show();
        
    }

    function filters_pageUnloaded(sender, args)
    {
        //Below causes error, using clearHandlers instead
        //$removeHandler($get("requestReportButton"), "click", requestReport);
        //$removeHandler($get("clearFiltersButton"), "click", clearReportFilters);
        
        $clearHandlers($get("requestReportButton"));
        $clearHandlers($get("clearFiltersButton"));

        clientManager.remove_pageLoaded(filters_pageLoaded, "moduleOptionsContainer");
        clientManager.remove_pageUnloaded(filters_pageUnloaded, "moduleOptionsContainer");
    }

    function requestReport()
    {
        var data = $getContainerData("filtersContainer");
        var data2 = $getContainerData("timeFrameContainer");
        var data3 = $getContainerData("toolbarContainer");

        for (var k in data2)
            data[k] = data2[k];

        for (var k in data3)
            data[k] = data3[k];

        //Validate to check if at least 1 timeframe is selected for Calendar Selection
        var isValid = true;


        //Check if calendar or module is formularyhistoryreporting, for formularyhistoryreporting only calendar control would be visible 
        if ($("#ctl00_partialPage_filtersContainer_CalendarRolling_Calendar_Rolling_0").attr('checked') || clientManager.get_Module() == 'formularyhistoryreporting')
        {
            var checkedCount = 0;

            $('#timeFrameQuarterContainer input:checkbox, #timeFrameMonthContainer input:checkbox').each(function()
            {
                if ($(this).attr('checked'))
                    checkedCount++;
            });

            if (checkedCount == 0)
            {
                isValid = false;
                $alert("Please select a year and quarter(s) or a year and month(s)", "Time Frame Selection");
            }
        }
        //Check if rolling
        if ($("#ctl00_partialPage_filtersContainer_CalendarRolling_Calendar_Rolling_1").attr('checked'))
        {
            if (!(data2["Rolling_Selection"]))
            {
                isValid = false;
                $alert("Please select at least one rolling quarter", "Time Frame Selection");
            }
        }

        if ((data["Region_ID"].value == 'all') && (data["District_ID"].value == 'all') && (data["Territory_ID"].value == 'all'))
        {
            isValid = false;
            $alert("Please select at least one Area, Region or Territory", "Geography Selection");
        }

        if (isValid)
        {
            if ($validateContainerData("filtersContainer", data, '<%= Resources.Resource.Label_Report_Filters %>'))
            {
                data["__options"] = $getContainerData("optionsContainer");
                clientManager.set_SelectionData(data);
            }
        }
    }

    function clearReportFilters()
    {
        $resetContainer("filterControls");
    }

    function onSectionListChanged(sender, args) {
        var val = sender.get_value();
        if (val == 0) val = null;

        var url;
        
        //Query 'Combined Plans Set' if Channel selected is Combined (-1)
        if (val != -1)
            url = "todaysaccounts/services/PathfinderClientDataService.svc/PlanSearchSet?$filter=Section_ID eq " + val + "&$orderby=Name";
        else
            url = "todaysaccounts/services/PathfinderClientDataService.svc/PlanSearchCombinedSet?$orderby=Name";

        $getJSON(url, null, function(result, status)
        {
            if (!status)
                result = result[0];

            var d = result.d;

            var plan_id = $get("Plan_ID");

            if (plan_id)
            {
                var plan_id_control = plan_id.control;
                if (plan_id_control)
                    $loadPinsoListItems(plan_id_control, d, null, -1); //  plan_id_control.dispose();                    
            }
            else //Create Channel list
                $createCheckboxDropdown("ctl00_partialPage_filtersContainer_Channel_Plan_ID", "Plan_ID", d, { 'defaultText': '<%= Resources.Resource.Label_No_Selection %>', 'multiItemText': '<%= Resources.Resource.Label_Multiple_Selection %>' }, null, "moduleOptionsContainer");

            //After list is loaded check if current Plan ID can be selected
            //                var data = clientManager.get_SelectionData();
            //                var planID;
            //                if (data && data["Plan_ID"]) planID = data["Plan_ID"].value;
            //                if (planID) {
            //                    var plan_id_ctrl = $get("Plan_ID").control;
            //                    var x;
            //                    for (x in planID)
            //                        plan_id_ctrl.selectItem(planID[x]);

            $updateCheckboxDropdownText("ctl00_partialPage_filtersContainer_Channel_Plan_ID", "Plan_ID");

            var plan_id = $get('Plan_ID'); if (plan_id.control) { plan_id.control.reset(); }
            //                }
        });
    }

    function TopN_Product_Changed(sender, args)
    {
//        var data = clientManager.getContextValue("physQuery");

//        var cR = $find("ctl00_partialPage_filtersContainer_Geography_Region_ID");
//        var cD = $find("ctl00_partialPage_filtersContainer_Geography_District_ID");
//        var cT = $find("ctl00_partialPage_filtersContainer_Geography_Territory_ID");

//        //Region
//        if (data["Phy_Region_ID"] && cR.get_value() == 'all')
//            delete (data["Phy_Region_ID"]);

//        if (cR.get_value() != 'all')
//            data["Phy_Region_ID"] = cR.get_value();

//        //District
//        if (data["Phy_District_ID"] && cD.get_value() == 'all')
//            delete (data["Phy_District_ID"]);

//        if (cD.get_value() != 'all')
//            data["Phy_District_ID"] = cD.get_value();

//        //Territory
//        if (data["Phy_Territory_ID"] && cT.get_value() == 'all')
//            delete (data["Phy_Territory_ID"]);

//        if (cT.get_value() != 'all')
//            data["Phy_Territory_ID"] = cT.get_value();       
    }
</script>