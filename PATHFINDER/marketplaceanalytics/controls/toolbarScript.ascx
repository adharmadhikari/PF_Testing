<%@ Control Language="C#" AutoEventWireup="true" CodeFile="toolbarScript.ascx.cs" Inherits="marketplaceanalytics_controls_toolbarScript" %>
<%@ OutputCache Shared="true" VaryByParam="None" Duration="120" %>  
  
    <script type="text/javascript">

        clientManager.add_pageLoaded(toolbar_pageLoaded, "toolbarArea");
        clientManager.add_pageUnloaded(toolbar_pageUnloaded, "toolbarArea");

        function toolbar_pageLoaded(sender, args)
        {
            //$addHandler($get("requestReportButton"), "click", requestReport);
            //$addHandler($get("clearFiltersButton"), "click", clearReportFilters);

            var data = clientManager.get_SelectionData();
            var trxMst;

            if (data)
                trxMst = data["Trx_Mst"];

            //Check if TrxMst has a value
            if (trxMst)
            {
                if (typeof trxMst == "object")
                    trxMst = trxMst.value;
            }

            if ((typeof (trxMst) != "undefined")) //If there is selection data, set the value
            {
                if (trxMst == 'Trx')
                    $("#ctl00_partialPage_filterTrxMst_Trx_Mst_0").attr('checked', 'checked');
                else if (trxMst == 'Nrx')
                    $("#ctl00_partialPage_filterTrxMst_Trx_Mst_2").attr('checked', 'checked');
                else if (trxMst == 'Msn')
                    $("#ctl00_partialPage_filterTrxMst_Trx_Mst_3").attr('checked', 'checked');
                else
                    $("#ctl00_partialPage_filterTrxMst_Trx_Mst_1").attr('checked', 'checked');
            }
            else//Otherwise set default value to Trx
                $("#ctl00_partialPage_filterTrxMst_Trx_Mst_0").attr('checked', 'checked');

            //Initialize formatting for Trx/Mst selector
            $('#filterTrxMst input:radio').each(function()
            {
                var id = $(this).attr('id');
                if ($(this).attr('checked')) $('#filterTrxMst label[for=' + id + ']').addClass('selectedTrxMstOption');
                $(this).click(function()
                {
                    radioCheckStatus(this);
                    requestReportToolbar();
                })
            });

            $('#refreshSelection').click(function()
            {
                requestReportToolbar();
            });
        }       

        function radioCheckStatus(ctrl)
        {
            var id = $(ctrl).attr('id');
            $('#filterTrxMst label').not('#filterTrxMst label[for=' + id + ']').removeClass('selectedTrxMstOption');

            $('#filterTrxMst label[for=' + id + ']').addClass('selectedTrxMstOption');
        }

        function requestReportToolbar()
        {
            var data = $getContainerData("filtersContainer");
            var data2 = $getContainerData("timeFrameContainer");
            var data3 = $getContainerData("toolbarContainer");

            for (var k in data2)
                data[k] = data2[k];

            for (var k in data3)
                data[k] = data3[k];

            //Check if drilldown was selected in order to persist
            var dataDrilldown = clientManager.get_SelectionData(1);
            if (dataDrilldown && dataDrilldown["IsDrilldown"] == true)
            {
                data["IsDrilldown"] = true;
                data["Selection_Clicked"] = dataDrilldown["Selection_Clicked"];
                data["Product_ID_Drilldown"] = dataDrilldown["Product_ID"];
            }   
            
            //Validate to check if at least 1 timeframe is selected for Calendar Selection
            var isValid = true;

            if ($("#timeFrameCalendar").length > 0 && $('.navbar2').is(':visible'))
            {
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
        
        function runExport(type)
        {
            $openWindow("usercontent/confirmexport.aspx?type=" + type, null, null, 400, 200, "confirmexp");
        }

        function toolbar_pageUnloaded(sender, args)
        {
            //$removeHandler($get("requestReportButton"), "click", requestReport);
            //$removeHandler($get("clearFiltersButton"), "click", clearReportFilters);

            clientManager.remove_pageLoaded(toolbar_pageLoaded, "toolbarArea");
            clientManager.remove_pageUnloaded(toolbar_pageUnloaded, "toolbarArea");
        }                
    </script>