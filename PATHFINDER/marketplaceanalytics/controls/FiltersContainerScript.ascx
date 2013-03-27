<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FiltersContainerScript.ascx.cs" Inherits="marketplaceanalytics_controls_FiltersContainerScript" %>
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
        
        //channel is not required for affiliations
        if (clientManager.get_Module() != 'ma_affiliations' && clientManager.get_Module() != 'formularyhistoryreporting') 
        {
            //Set default value of Channel to 'Commercial Payer'
            var combo = $find("ctl00_partialPage_filtersContainer_Channel_Section_ID");
            var itm = combo.findItemByValue(1);
            itm.select();
        }

        //Enable and Inialize Time Frame Type control only for Formulary History Reporting
        if (clientManager.get_Module() == 'formularyhistoryreporting')
        {
            var data = clientManager.get_SelectionData();
            var monthQuarter;

            if (data)
                monthQuarter = data["Monthly_Quarterly"];

            //Check if TrxMst has a value
            if (monthQuarter)
            {
                if (typeof monthQuarter == "object")
                    monthQuarter = monthQuarter.value;
            }

            if ((typeof (monthQuarter) != "undefined")) //If there is selection data, set the value
            {
                //Load Timeframe list
                onTimeFrameChanged(monthQuarter);

                if (monthQuarter == 'M')
                    $("#ctl00_partialPage_filtersContainer_FilterMonthQuarter_Monthly_Quarterly_0").attr('checked', 'checked');
                else
                    $("#ctl00_partialPage_filtersContainer_FilterMonthQuarter_Monthly_Quarterly_1").attr('checked', 'checked');
            }
            else//Otherwise set default value to Monthly
            {
                $("#ctl00_partialPage_filtersContainer_FilterMonthQuarter_Monthly_Quarterly_0").attr('checked', 'checked');

                //Load Timeframe list
                onTimeFrameChanged("M");
            }
        }

        $("#timeframeArea").show();

        //Set timeframe selector based on module selected
        var pageName;
        if (clientManager.get_Module() == 'comparison')
            pageName = "filtertimeframecomparison.aspx";
        else
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

        if ($("#timeFrameCalendar").length > 0 && $('.navbar2').is(':visible')) {
            //Check if calendar or module is formularyhistoryreporting, for formularyhistoryreporting only calendar control would be visible 
            if ($("#ctl00_partialPage_filtersContainer_CalendarRolling_Calendar_Rolling_0").attr('checked') || clientManager.get_Module() == 'formularyhistoryreporting') {
                var checkedCount = 0;

                $('#timeFrameQuarterContainer input:checkbox, #timeFrameMonthContainer input:checkbox').each(function() {
                    if ($(this).attr('checked'))
                        checkedCount++;
                });

                if (checkedCount == 0) {
                    isValid = false;
                    $alert("Please select a year and quarter(s) or a year and month(s)", "Time Frame Selection");
                }
            }
            //Check if rolling
            if ($("#ctl00_partialPage_filtersContainer_CalendarRolling_Calendar_Rolling_1").attr('checked')) {
                if (!(data2["Rolling_Selection"])) {
                    isValid = false;
                    $alert("Please select at least one rolling quarter", "Time Frame Selection");
                }
            }
        }
        else //FHR Report
        {                        
            //Make sure Account 1 Selection does not equal Account 2 Selection
            if (data["Plan_ID1"] && data["Plan_ID2"] && data["Plan_ID1"].value == data["Plan_ID2"].value) 
            {
                $alert("Please select a different account - Second Account Name selection must differ from First Account Name selection.", "Account Selection");
                isValid = false;
            }            
        }

        //For FHR Report - if 'All' Timeframe is selected, add all available timeframes to query
        if ((clientManager.get_Module() == 'formularyhistoryreporting') && (!data["Timeframe"])) 
            data["Timeframe"] = clientManager.getContextValue("ma_fhTimeframes");
        
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

    //Called from FilterMonthQuarter.ascx
    function onTimeFrameChanged(monthlyQuarterly)
    {
        //Load timeframe droplist based on section selected
        if (monthlyQuarterly == "M")
            url = "marketplaceanalytics/services/MarketplaceDataService.svc/FHMonthYearsSet?$orderby=Data_Year desc,Data_Month desc";
        else
            url = "marketplaceanalytics/services/MarketplaceDataService.svc/FHQuarterYearsSet?$orderby=Data_Year desc,Data_Quarter desc";

        $.getJSON(url, null, function(result, status)
        {
            var d = result.d;
            var timeframe_id = $get("Timeframe_ID");

            var items = new Array();

            for (var i in d)
            {
                items.push(d[i].ID);
            }
            clientManager.setContextValue("ma_fhTimeframes", items);

            if (timeframe_id)
            {
                var timeframe_id_control = timeframe_id.control;
                if (timeframe_id_control)
                    $loadPinsoListItems(timeframe_id_control, d, { 'ID': 0, 'Name': 'All' }, -1); //  plan_id_control.dispose();                    
            }
            else //Create Timeframe list
            {
                $createCheckboxDropdown("ctl00_partialPage_filtersContainer_FilterTimeframe_Timeframe", "Timeframe_ID", null, { 'defaultText': 'All', 'multiItemText': '--Change Selection--' }, null, "moduleOptionsContainer");
                $loadPinsoListItems($find('Timeframe_ID'), d, { 'ID': 0, 'Name': 'All' }, -1);
            }

            $updateCheckboxDropdownText("ctl00_partialPage_filtersContainer_FilterTimeframe_Timeframe", "Timeframe_ID");

            timeframe_id = $get('Timeframe_ID');

            if (timeframe_id.control)
            {
                timeframe_id.control.reset();
            }
        });
    }   
</script>