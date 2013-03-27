<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FiltersContainerScript.ascx.cs" Inherits="formularyhistoryreporting_controls_FiltersContainerScript" %>
<%@ OutputCache Shared="true" VaryByParam="None" Duration="120" %>  
  
    <script type="text/javascript">

        clientManager.add_pageLoaded(filters_pageLoaded, "moduleOptionsContainer");
        clientManager.add_pageUnloaded(filters_pageUnloaded, "moduleOptionsContainer");

        var _filterPageDefaults = [];
        
        function filters_pageLoaded(sender, args) {
            $clearAlert(); //make sure any previous alerts are cleared.
            
            $addHandler($get("requestReportButton"), "click", requestReport);
            $addHandler($get("clearFiltersButton"), "click", clearReportFilters);

            $reloadContainer("moduleOptionsContainer", clientManager.get_SelectionData());

            //clientManager.registerComponent('ctl00_partialPage_filtersContainer_MarketSegment_Plan_ID', null);

            //Refresh Account Name Search
            var geoID = 'US';
            var sectionID = $find('ctl00_partialPage_filtersContainer_MarketSegment_Section_ID').get_value();

            var geoIDCtrl = $find('ctl00_partialPage_filtersContainer_MarketSegment_Geography_ID');

            if (geoIDCtrl && geoIDCtrl != null && geoIDCtrl.get_value() != "")
                geoID = geoIDCtrl.get_value();

            refreshPlanSelectionList(sectionID, geoID);            

            loadTimeFrame();
            
            standardreports_content_resize();         
        }

        function filters_pageUnloaded(sender, args)
        {
            $clearHandlers($get("requestReportButton"));
            $clearHandlers($get("clearFiltersButton"));

            clientManager.remove_pageLoaded(filters_pageLoaded, "moduleOptionsContainer");
            clientManager.remove_pageUnloaded(filters_pageUnloaded, "moduleOptionsContainer");
        }
        //request report
        function requestReport()
        {
            var data = $getContainerData("filtersContainer");

            var isValid = true;
            data["Plan_ID"] = $('#Plan_ID_DATA').val();

            var geogID = data["Geography_ID"];

            if ((typeof (geogID) == "undefined"))
                data["Geography_ID"] = "US";

            if (data["Plan_ID"] == "")
            {
                $alert("Please select at least one Account.", "Account Selection");
                isValid = false;
            }           
            if (clientManager.get_Module() == "coformularyhxcomparison" && data["TimeFrame1"] && data["TimeFrame2"] && data["TimeFrame1"].value == data["TimeFrame2"].value)
            {
                $alert("Please select two different time frames.", "Time Frame Selection");
                isValid = false;
            }

            if (isValid)
            {
                if ($validateContainerData("filtersContainer", data, '<%= Resources.Resource.Label_Report_Filters %>'))
                {
                    data["__options"] = $getContainerData("optionsContainer");
                    clientManager.set_SelectionData(data);
                    if (clientManager.get_Module() == "coformularyhxrolling")
                        $exportModule("excel", false, clientManager.get_Application(), 0, clientManager.get_Module(), data)
                }
            }
        }   
         
        function clearReportFilters() {
            $resetContainer("filterControls");
        }

        function get_fhr_SectionLoad(s, a)
        {
            var val = s.get_value();

            //If PBM, disable Geography selection and select National by default
            var geographySelection = $find('ctl00_partialPage_filtersContainer_MarketSegment_rcbGeographyType');
            if (!geographySelection) return;
            if (val == 4)
            {
                geographySelection.findItemByValue(1).select();
                geographySelection.disable();
            }
            else
                geographySelection.enable();

            //    if (val == 0) val = null;
            clientManager.set_SelectionData({ Section_ID: val });

            var geoID = 'US';
            var geoIDCtrl = $find('ctl00_partialPage_filtersContainer_MarketSegment_Geography_ID');
            if (!geoIDCtrl) return;
            if (geoIDCtrl != null && geoIDCtrl.get_value() != "")
                geoID = geoIDCtrl.get_value();

            //Load plan selection list
            refreshPlanSelectionList(val, geoID);

            //Clear previously selected plans
            clearPlanSelectionList();

            //update Time Frame
            var segmentid = 1;
            if (val == 17) segmentid = 2;
            var theraclientid = $find('ctl00_partialPage_filtersContainer_DrugSelection_Market_Basket_ID');
            var monthly_quarterly = $('#ctl00_partialPage_filtersContainer_DrugSelection_Monthly_Quarterly input:checked').val();

            if (!theraclientid) return;
            var thera_id = theraclientid.get_value();

            if (clientManager.get_Module() == "coformularyhxrolling")
            {
//                if (val == 17)
//                {
//                    $find('ctl00_partialPage_filtersContainer_DrugSelection_TimeFrameQtr').set_visible(false);
//                    $find('ctl00_partialPage_filtersContainer_DrugSelection_TimeFrameMonth').set_visible(true);
//                }
//                else
//                {
//                    $find('ctl00_partialPage_filtersContainer_DrugSelection_TimeFrameQtr').set_visible(true);
//                    $find('ctl00_partialPage_filtersContainer_DrugSelection_TimeFrameMonth').set_visible(false);
//                }
            }
            else
            {

                var url = "services/pathfinderservice.svc" + "/fhrGetFormularyDataPeriodSet?$filter=Segment_ID eq " + segmentid + " and Thera_ID eq " + thera_id + " and Monthly_Quarterly eq '" + monthly_quarterly + "'";

                $.getJSON(url, null, function(result, status)
                {
                    var d = result.d;
                    $loadListItems($find('ctl00_partialPage_filtersContainer_DrugSelection_TimeFrame1'), d, !d.length ? { value: "", text: "no records available"} : null, 0, 'TimeFrame', 'TimeFrameName');
                    $loadListItems($find('ctl00_partialPage_filtersContainer_DrugSelection_TimeFrame2'), d, !d.length ? { value: "", text: "no records available"} : null, 0, 'TimeFrame', 'TimeFrameName');
                });
            }

            //load Display options

            //var sectionid = val;
            if (val != 9) val = 0;

            var display_id = $get('Display_ID');
            if (!display_id) return;

            var url = "services/pathfinderservice.svc" + "/fhrGetSectionDisplayOptionsSet?$filter=Section_ID eq " + val + "&$orderby=ID";

            $.getJSON(url, null, function(result, status)
            {
                var d = result.d;
                if (display_id)
                {
                    var display_id_control = display_id.control;
                    if (display_id_control) $loadPinsoListItems(display_id_control, d, null, -1, true);
                }
                else
                {
                    $createCheckboxDropdown('ctl00_partialPage_filtersContainer_DisplayOptions_Display_ID', 'Display_ID', null, { 'defaultText': '--All--', 'multiItemText': '--Change Selection--' }, null, 'moduleOptionsContainer');
                    var display_id_control = $get('Display_ID').control; $loadPinsoListItems(display_id_control, d, null, -1, true);
                };
                $updateCheckboxDropdownText('ctl00_partialPage_filtersContainer_DisplayOptions_Display_ID', 'Display_ID');
            });
        }

        function onTimeFrameQtrLoad(s, a)
        {
            if (clientManager.get_Module() == "coformularyhxrolling")
            {
                if ($('#ctl00_partialPage_filtersContainer_DrugSelection_Monthly_Quarterly input:checked').val() == "M")
                    $find('ctl00_partialPage_filtersContainer_DrugSelection_TimeFrameQtr').set_visible(false);
                else
                    $find('ctl00_partialPage_filtersContainer_DrugSelection_TimeFrameQtr').set_visible(true);
            }
        }
        function onTimeFrameMonthLoad(s, a)
        {
            if (clientManager.get_Module() == "coformularyhxrolling")
            {
                if ($('#ctl00_partialPage_filtersContainer_DrugSelection_Monthly_Quarterly input:checked').val() == "M")
                    $find('ctl00_partialPage_filtersContainer_DrugSelection_TimeFrameMonth').set_visible(true);
                else
                    $find('ctl00_partialPage_filtersContainer_DrugSelection_TimeFrameMonth').set_visible(false);
            }
        }

        function onMBLoad(s, a)
        {
            //Load Time Frame Control
            loadTimeFrame();
            
            $loadListItems(s, clientManager.get_MarketBasketListOptions(), null, clientManager.get_MarketBasket());

            var timeframe = $get('ctl00_partialPage_filtersContainer_DrugSelection_TimeFrame');
            var sectionid = $find('ctl00_partialPage_filtersContainer_MarketSegment_Section_ID');
            var monthly_quarterly = $('#ctl00_partialPage_filtersContainer_DrugSelection_Monthly_Quarterly input:checked').val();

            if (!sectionid) return;

            var section_id = sectionid.get_value();
            var segmentid = 1;

            if (section_id == 17) segmentid = 2;

            var thera_id = s.get_value();

            var url = "services/pathfinderservice.svc" + "/fhrGetFormularyDataPeriodSet?$filter=Segment_ID eq " + segmentid + " and Thera_ID eq " + thera_id + " and Monthly_Quarterly eq '" + monthly_quarterly + "'";

            $.getJSON(url, null, function(result, status)
            {
                var d = result.d;
                $loadListItems($find('ctl00_partialPage_filtersContainer_DrugSelection_TimeFrame1'), d, !d.length ? { value: "", text: "no records available"} : null, 0, 'TimeFrame', 'TimeFrameName');
                $loadListItems($find('ctl00_partialPage_filtersContainer_DrugSelection_TimeFrame2'), d, !d.length ? { value: "", text: "no records available"} : null, 0, 'TimeFrame', 'TimeFrameName');
            });
        }
        
        function LoadDisplayOptions(s, a) {
            var sectionid = $find('ctl00_partialPage_filtersContainer_MarketSegment_Section_ID');
            if (!sectionid) sectionid = 0;

            var val = sectionid.get_value();
            if (val != 9) val = 0;

            var display_id = $get('Display_ID');

            var url = "services/pathfinderservice.svc" + "/fhrGetSectionDisplayOptionsSet?$filter=Section_ID eq " + val + "&$orderby=ID";

            $.getJSON(url, null, function(result, status) {
                var d = result.d;
                if (display_id) {
                    var display_id_control = display_id.control;
                    if (display_id_control) $loadPinsoListItems(display_id_control, d, null, -1, true);
                }
                else {
                    $createCheckboxDropdown('ctl00_partialPage_filtersContainer_DisplayOptions_Display_ID', 'Display_ID', null, { 'defaultText': '--All--', 'multiItemText': '--Change Selection--' }, null, 'moduleOptionsContainer');
                    var display_id_control = $get('Display_ID').control; $loadPinsoListItems(display_id_control, d, null, -1, true);
                };
                $updateCheckboxDropdownText('ctl00_partialPage_filtersContainer_DisplayOptions_Display_ID', 'Display_ID');
            });
        }
        function onMBIndexChanged(s, a) {
            //TODO:
        }
        function GeoTypeLoad(s, a) {
            var gType = s.get_element();
            var data = clientManager.get_SelectionData();
            if (data && data["Geography_ID"]) {
                if (clientManager.get_States()[data["Geography_ID"].value]) {
                    s.findItemByValue(3).select();
                }
                else {
                    if (s.findItemByValue(2)) {
                        s.findItemByValue(2).select();
                    }
                }
            }
            else {
                s.findItemByValue(1).select();
            }
        }

        function GeoLoad(s, a) {
            var l = s.get_element();
            var gt = $find('ctl00_partialPage_filtersContainer_MarketSegment_rcbGeographyType').get_value();
            var regCtrl = $get('ctl00_partialPage_filtersContainer_MarketSegment_Geography_ID');

            if (!regCtrl || !regCtrl.control) return;
            if (gt && gt == '1') {
                $loadListItems(l, null, { value: '', text: '' }
     );
                l.control.set_visible(false);
                regCtrl.control.set_visible(false);
            }
            if (gt && gt == '2') {
                $loadListItems(l, clientManager.get_RegionListOptions());
                l.control.set_visible(true);
                regCtrl.control.set_visible(true);
            }
            if (gt && gt == '3') {
                $loadListItems(l, clientManager.get_States());
                l.control.set_visible(true); regCtrl.control.set_visible(true);
            }
        }

        function geoSelectedIndexChanged(s, a) {
            var l = $get('ctl00_partialPage_filtersContainer_MarketSegment_Geography_ID');
            var gt = a.get_item().get_value();
            var regCtrl = $get('ctl00_partialPage_filtersContainer_MarketSegment_Geography_ID');

            if (!regCtrl || !regCtrl.control) return;
            if (gt && gt == '1') {
                $loadListItems(l, null, { value: '', text: '' });
                // l.control.set_visible(false);
                regCtrl.control.set_visible(false);
            }
            if (gt && gt == '2') {
                $loadListItems(l, clientManager.get_RegionListOptions());
                //l.control.set_visible(true);
                regCtrl.control.set_visible(true);
            }
            if (gt && gt == '3') {
                $loadListItems(l, clientManager.get_States());
                //l.control.set_visible(true);
                regCtrl.control.set_visible(true);
            }
            var geoID = 'US';
            var sectionID = $find('ctl00_partialPage_filtersContainer_MarketSegment_Section_ID').get_value();

            var geoIDCtrl = $find('ctl00_partialPage_filtersContainer_MarketSegment_Geography_ID');

            if (geoIDCtrl && geoIDCtrl != null && geoIDCtrl.get_value() != "")
                geoID = geoIDCtrl.get_value();

            refreshPlanSelectionList(sectionID, geoID);
            //Clear previously selected plans
            clearPlanSelectionList();

            reportFiltersResize();
        }
        function geoIDSelectedIndexChanged(s, a) {
            var geoID = 'US';
            var sectionID = $find('ctl00_partialPage_filtersContainer_MarketSegment_Section_ID').get_value();

            var geoIDCtrl = $find('ctl00_partialPage_filtersContainer_MarketSegment_Geography_ID');

            if (geoIDCtrl && geoIDCtrl != null && geoIDCtrl.get_value() != "")
                geoID = geoIDCtrl.get_value();

            refreshPlanSelectionList(sectionID, geoID);
            //Clear previously selected plans
            clearPlanSelectionList();
        }

        function refreshPlanSelectionList(sectionID, geoID)
        {
            var x = $('.searchTextBoxFilter #Plan_ID')[0];
            if (x && x.SearchList)
            {
                x = x.SearchList;

                query = "Section_ID eq " + sectionID;
                query += " and Geography_ID eq '" + geoID + "' and substringof('{0}',Name)&$top=50&$orderby=Name";

                x.set_queryFormat("$filter=" + query);
                x.set_queryValues();
            }    
        }

        function clearPlanSelectionList()
        {
            var x = $('.searchTextBoxFilter #Plan_ID')[0];
            if (x && x.SearchList)
            {
                x = x.SearchList;

                x.clearSearchListSelection();
            }
        }

        //Called from FilterDrugSelection.ascx
        function onTimeFrameChanged(monthlyQuarterly)
        {
            if (clientManager.get_Module() == "coformularyhxrolling")
            {
                if (monthlyQuarterly == "M")
                {
                    if ($find('ctl00_partialPage_filtersContainer_DrugSelection_TimeFrameQtr') && $find('ctl00_partialPage_filtersContainer_DrugSelection_TimeFrameMonth'))
                    {
                        $find('ctl00_partialPage_filtersContainer_DrugSelection_TimeFrameQtr').set_visible(false);
                        $find('ctl00_partialPage_filtersContainer_DrugSelection_TimeFrameMonth').set_visible(true);
                    }
                }
                else
                {
                    if ($find('ctl00_partialPage_filtersContainer_DrugSelection_TimeFrameQtr') && $find('ctl00_partialPage_filtersContainer_DrugSelection_TimeFrameMonth'))
                    {
                        $find('ctl00_partialPage_filtersContainer_DrugSelection_TimeFrameQtr').set_visible(true);
                        $find('ctl00_partialPage_filtersContainer_DrugSelection_TimeFrameMonth').set_visible(false);
                    }
                }
            }
            else
            {
                var sectionid = $find('ctl00_partialPage_filtersContainer_MarketSegment_Section_ID');
                if (!sectionid) return;
                var section_id = sectionid.get_value();
                var segmentid = 1;
                if (section_id == 17) segmentid = 2;

                var theraclientid = $find('ctl00_partialPage_filtersContainer_DrugSelection_Market_Basket_ID');
                if (!theraclientid) return;
                var thera_id = theraclientid.get_value();

                //Load timeframe droplist
                var url = "services/pathfinderservice.svc" + "/fhrGetFormularyDataPeriodSet?$filter=Segment_ID eq " + segmentid + " and Thera_ID eq " + thera_id + " and Monthly_Quarterly eq '" + monthlyQuarterly + "'";

                $.getJSON(url, null, function(result, status)
                {
                    var d = result.d;
                    $loadListItems($find('ctl00_partialPage_filtersContainer_DrugSelection_TimeFrame1'), d, !d.length ? { value: "", text: "no records available"} : null, 0, 'TimeFrame', 'TimeFrameName');
                    $loadListItems($find('ctl00_partialPage_filtersContainer_DrugSelection_TimeFrame2'), d, !d.length ? { value: "", text: "no records available"} : null, 0, 'TimeFrame', 'TimeFrameName');
                });
            }
        }

        function loadTimeFrame()
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
                //onTimeFrameChanged(monthQuarter);
                if (monthQuarter == 'M')
                    $("#ctl00_partialPage_filtersContainer_DrugSelection_Monthly_Quarterly_0").attr('checked', 'checked');
                else
                    $("#ctl00_partialPage_filtersContainer_DrugSelection_Monthly_Quarterly_1").attr('checked', 'checked');
            }
            else//Otherwise set default value to Monthly
                $("#ctl00_partialPage_filtersContainer_DrugSelection_Monthly_Quarterly_0").attr('checked', 'checked');
        }
    </script>