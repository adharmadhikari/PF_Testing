<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FiltersContainerScript.ascx.cs" Inherits="restrictionsreport_controls_FiltersContainerScript" %>
<%@ OutputCache Shared="true" VaryByParam="None" Duration="120" %>  
  
    <script type="text/javascript">

        clientManager.add_pageLoaded(filters_pageLoaded, "moduleOptionsContainer");
        clientManager.add_pageUnloaded(filters_pageUnloaded, "moduleOptionsContainer");

        var _filterPageDefaults = [];
        var pbmClicked = false;

        function filters_pageLoaded(sender, args) {
            showHidePlanFilter();
            $clearAlert(); //make sure any previous alerts are cleared.

            $addHandler($get("requestReportButton"), "click", requestReport);
            $addHandler($get("clearFiltersButton"), "click", clearReportFilters);

            $reloadContainer("moduleOptionsContainer", clientManager.get_SelectionData());

            if (clientManager.get_Module() == 'deyrestrictionsreport' || clientManager.get_Module() == 'deyrestrictionsdrilldownreport') {
                //Hide QL and ST restrictions and select PA by default and disable
                var restrictions = $find('ctl00_partialPage_filtersContainer_Restrictions_restrictions');
                if (restrictions) {
                    restrictions.selectItem('QL');
                    $('#ctl00_partialPage_filtersContainer_Restrictions_restrictions_QL').attr('disabled', true);
                    $('#ctl00_partialPage_filtersContainer_Restrictions_restrictions_PA, label[for="ctl00_partialPage_filtersContainer_Restrictions_restrictions_PA"]').hide();
                    $('#ctl00_partialPage_filtersContainer_Restrictions_restrictions_ST, label[for="ctl00_partialPage_filtersContainer_Restrictions_restrictions_ST"]').hide();
                }
            }
            
            standardreports_content_resize();
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

        function sectionDropDownClosed(s, a) {
            var data = $getContainerData("filtersContainer");
            refreshPlanList(data);
        }

        function onMarketSegmentClicked(sender, args)
        {
            var segmentIDctrl = $get("MarketSegmentIDOptionList").control;
            var segmentIds = segmentIDctrl.get_values();

            //For deyrestrictionsreport if more than one segment is selected, only one drug can be selected
            var drug_id = $get("DrugIDList");
            if (drug_id)
            {
                var drug_ctrl = drug_id.control;

                if ($.isArray(segmentIds) && clientManager.get_Module() == 'deyrestrictionsreport')
                {
                    drug_ctrl.set_maxItems(1);
                    drug_ctrl.reset();
                    $updateCheckboxDropdownText(drugCtrlID, 'DrugIDList');
                }
                else
                    drug_ctrl.set_maxItems(5);
            }

            //only make PBM single select if Regular restrictions report is selected
            if (clientManager.get_Module() == 'deyrestrictionsreport') {
                //All segments but PBMs are multi-select
                if (!pbmClicked) {
                    if (segmentIds == "4" || ($.isArray(segmentIds) && $.inArray("4", segmentIds) > -1)) {
                        //Only reset control is something other than PBM clicked. (i.e: PBM and Commercial, only select PBM)
                        if (segmentIds != "4") {
                            //Set a flag to avoid endless loop of onMarketSegmentClicked
                            pbmClicked = true;

                            //Reset all selections in control and select only PBM
                            segmentIDctrl.reset();
                            segmentIDctrl.selectItem(4);

                            //Only show the following alert if a segment is clicked AFTER PBM is first selected
                            //(i.e: PBM already selected, then clicking on Commercial)
                            if (args.item.id != "4")
                                $alert("Please de-select PBM to select other Segment Options.", "Market Segment");
                        }
                    }
                }
                else
                    pbmClicked = false;
            }

            //Bind Plan Selection Dropdown
            var values = sender.get_values();
            var query = "";
            var url = "custom/dey/restrictionsreport/services/PathfinderDataService.svc/PlanSearchSet?$filter=";
            if (values)
            {
                query = "(";

                if ($.isArray(values))
                {
                    for (var i = 0; i < values.length; i++)
                    {
                        if (query.length > 1)
                            query += " or ";
                        query += "Section_ID eq " + values[i];
                    }
                }
                else
                    query += "Section_ID eq " + values;

                query += ")";
            }

            query += "&$select=ID,Name&$orderby=Name";
            url += query;
            $.getJSON(url, null, function(result, status)
            {
                var d = result.d;

                var uniquePlansIDs = {};
                var uniquePlans = [];
                var param = "ID"

                //JSON Distinct
                $.each(d, function(idx, val)
                {
                    var curID = this.ID;
                    var proposedID = uniquePlansIDs[curID];
                    if (curID != proposedID)
                    {
                        uniquePlansIDs[this.ID] = this.ID;
                        uniquePlans.push(this);
                    }
                });

                //var plan_id = $get("Plan_ID");

                //if (plan_id)
                //{
                //    var plan_id_control = plan_id.control;
                //    if (plan_id_control)
                //        $loadPinsoListItems(plan_id_control, uniquePlans, null, -1);
                //}
                //else
                //{
                //    $createCheckboxDropdown("ctl00_partialPage_filtersContainer_ReportType_Plan_ID", "Plan_ID", null, { 'defaultText': '--No Selection--', 'multiItemText': '--Change Selection--' }, null, "moduleOptionsContainer");
                //    $loadPinsoListItems($find('Plan_ID'), uniquePlans, null, -1);
                //}

                //$updateCheckboxDropdownText("ctl00_partialPage_filtersContainer_ReportType_Plan_ID", "Plan_ID");
                //Don't reload if SectionID is different
                //if (secID == s.get_value())
                //    $reloadContainer("moduleOptionsContainer", data);
            });
        }
        
        //request report
        function requestReport() {
            var data = $getContainerData("filtersContainer");

            //data["Section_ID"] = clientManager.get_EffectiveChannel();

            var geogID = data["Geography_ID"];

            if ((typeof (geogID) == "undefined"))
                data["Geography_ID"] = "US";

            var regionTypeID = data["Region_Type_ID"];
            if ((typeof (regionTypeID) == "undefined") && clientManager.get_Module() == "formularydrilldown")
                data["Region_Type_ID"] = 0;

            //set rank null so that we can remove this filter
            if (data["Rank"] && (data["Rank"].get_value() == -1 || data["Rank"].get_value() == "999999")) {
                delete (data["Rank"]);
            }

            var classPartition = data["Class_Partition"];

            if (data["Region_ID"])
                delete (data["Region_ID"]);

            if (data["MA_Region_ID"])
                delete (data["MA_Region_ID"]);

            //code to handle managed medicaid with rank filter selected
            if (data["Section_ID"] && data["Section_ID"].value == 6 && !data["Rank"]) {
                if ((typeof (classPartition) == "undefined")) {
                    data["Class_Partition"] = 2;
                }
            }

            if (data["Is_Predominant"] && data["Is_Predominant"].get_value() == 1) {
                data["Is_Predominant"].set_value("true");
            }

            //if rank is null or zero remove rank from data
            if ($validateContainerData("filtersContainer", data, '<%= Resources.Resource.Label_Report_Filters %>')) {
        if (clientManager.get_Module() == "deyrestrictionsdrilldownreport") {
            if (data["Market_Basket_ID"])
                delete data["Market_Basket_ID"];
        }
        data["Is_All_Section"] = false

        if ($get('Plan_ID') && $('#Plan_ID_DATA').val() != "")
            data["Plan_ID"] = $('#Plan_ID_DATA').val().split(',');
        else
            delete data["Plan_ID"];

        data["__options"] = $getContainerData("optionsContainer");
        clientManager.set_SelectionData(data);
    }
}

        function clearReportFilters()
        {
            $resetContainer("filterControls");
        }

        //This function is to set the Geography filter as per selection in AccountType filter and it is called from Account Type filter control


        function SetGeography(s, a) {
            var comboGeoType = $find('ctl00_partialPage_filtersContainer_Geography_rcbGeographyType');
            if (!comboGeoType) return;
            if (comboGeoType.findItemByValue(5)) { comboGeoType.findItemByValue(5).set_visible(false); }
            if (comboGeoType.findItemByValue(6)) { comboGeoType.findItemByValue(6).set_visible(false); }

            if (s.get_value() == 1) {
                comboGeoType.enable();

                if (comboGeoType.findItemByValue(2))
                { comboGeoType.findItemByValue(2).set_visible(false); }
                comboGeoType.findItemByValue(3).set_visible(false);
                comboGeoType.findItemByValue(1).select();
                comboGeoType.disable();
            }
            else if (s.get_value() == 26) {
                comboGeoType.enable();
                comboGeoType.findItemByValue(5).set_visible(true);
                comboGeoType.findItemByValue(5).select();
                comboGeoType.disable();
            }

            else if (s.get_value() == 27) {
                comboGeoType.enable();
                comboGeoType.findItemByValue(6).set_visible(true);
                comboGeoType.findItemByValue(6).select(); comboGeoType.disable();
            }
            else {
                comboGeoType.enable();
                comboGeoType.findItemByValue(1).select();
                if (comboGeoType.findItemByValue(2))
                { comboGeoType.findItemByValue(2).set_visible(true); }
                comboGeoType.findItemByValue(3).set_visible(true);
            }
        }

        function GeoLoad(s, a) {
            var l = s.get_element();
            var gt = $find('ctl00_partialPage_filtersContainer_Geography_rcbGeographyType').get_value();
            var regCtrl = $get('ctl00_partialPage_filtersContainer_Geography_Geography_ID');

            var regCtrlRegion = $get('ctl00_partialPage_filtersContainer_Geography_Region_ID');
            var regCtrlMARegion = $get('ctl00_partialPage_filtersContainer_Geography_MA_Region_ID');

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
            if (gt && gt == '4') {
                $loadListItems(l, accountmanagerterritories);
                l.control.set_visible(true);
                regCtrl.control.set_visible(true);
            }
            if (gt && gt == '5') //MA 
            {
                regCtrl.control.set_visible(false);
                regCtrlRegion.control.set_visible(false);
                regCtrlMARegion.control.set_visible(true);
            }
            if (gt && gt == '6') //PDP
            {
                regCtrl.control.set_visible(false);
                regCtrlMARegion.control.set_visible(false);
                regCtrlRegion.control.set_visible(true);
            }
        }

        

        //PDP Regions
        function RegionLoad(s, a)
        {

            var l = s.get_element();
            var region_ID = $get('Region_ID');
            if (region_ID)
            {
                var region_ID_control = region_ID.control;
                if (region_ID_control)
                    $loadPinsoListItems(region_ID_control, pdpregions, null, -1);
            }
            else
            {
                $createCheckboxDropdown('ctl00_partialPage_filtersContainer_Geography_Region_ID', 'Region_ID', null, { 'defaultText': '--All--', 'multiItemText': '--Change Selection--' }, null, 'moduleOptionsContainer');
                var region_ID_control = $get('Region_ID').control;
                $loadPinsoListItems(region_ID_control, pdpregions, null, -1);
            };
            $updateCheckboxDropdownText('ctl00_partialPage_filtersContainer_Geography_Region_ID', 'Region_ID');
            var c = $get('ctl00_partialPage_filtersContainer_Geography_rcbGeographyType'); var s = $get('ctl00_partialPage_filtersContainer_Geography_Region_ID');
            if (!c || !c.control || !s || !s.control) return;
            var gt = c.control.get_value();
            switch (parseInt(gt, 10))
            {
                case 6: s.control.set_visible(true);
                    break;
                default: s.control.set_visible(false);
                    break;
            }
        }
        //MA Regions
        function MARegionLoad(s, a)
        {

            var l = s.get_element();
            var region_ID = $get('MA_Region_ID');
            if (region_ID)
            {
                var region_ID_control = region_ID.control;
                if (region_ID_control)
                    $loadPinsoListItems(region_ID_control, maregions, null, -1);
            }
            else
            {
                $createCheckboxDropdown('ctl00_partialPage_filtersContainer_Geography_MA_Region_ID', 'MA_Region_ID', null, { 'defaultText': '--All--', 'multiItemText': '--Change Selection--' }, null, 'moduleOptionsContainer');
                var region_ID_control = $get('MA_Region_ID').control;
                $loadPinsoListItems(region_ID_control, maregions, null, -1);
            };
            $updateCheckboxDropdownText('ctl00_partialPage_filtersContainer_Geography_MA_Region_ID', 'MA_Region_ID');
            var c = $get('ctl00_partialPage_filtersContainer_Geography_rcbGeographyType');
            var s = $get('ctl00_partialPage_filtersContainer_Geography_MA_Region_ID');
            if (!c || !c.control || !s || !s.control) return;
            var gt = c.control.get_value();
            switch (parseInt(gt, 10))
            {
                case 5: s.control.set_visible(true);
                    break;
                default: s.control.set_visible(false);
                    break;
            }
        }
        //end of ma and PDP region load

        function geoSelectedIndexChanged(s, a)
        {
            var l = $get('ctl00_partialPage_filtersContainer_Geography_Geography_ID');
            var gt = a.get_item().get_value();
            var regCtrl = $get('ctl00_partialPage_filtersContainer_Geography_Geography_ID');
            var regCtrlRegion = $get('ctl00_partialPage_filtersContainer_Geography_Region_ID');
            var regCtrlMARegion = $get('ctl00_partialPage_filtersContainer_Geography_MA_Region_ID');

            if (!regCtrl || !regCtrl.control) return;
            if (gt && gt == '1')
            {
                $loadListItems(l, null, { value: '', text: '' }
         );
                l.control.set_visible(false);
                regCtrl.control.set_visible(false);
            }
            if (gt && gt == '2')
            {
                $loadListItems(l, clientManager.get_RegionListOptions());
                l.control.set_visible(true);
                regCtrl.control.set_visible(true);
            }
            if (gt && gt == '3')
            {
                $loadListItems(l, clientManager.get_States());
                l.control.set_visible(true);
                regCtrl.control.set_visible(true);
            }
            if (gt && gt == '4')
            {
                $loadListItems(l, accountmanagerterritories);
                l.control.set_visible(true);
                regCtrl.control.set_visible(true);
            }
            regCtrlRegion.control.set_visible(false);
            regCtrlMARegion.control.set_visible(false);

            if (gt && gt == '5') //MA
            {
                //$loadListItems(l, maregions); l.control.set_visible(true); regCtrl.control.set_visible(true);
                regCtrl.control.set_visible(false);
                regCtrlMARegion.control.set_visible(true);

                //for getting region ID
                var c = $get('ctl00_partialPage_filtersContainer_Geography_MA_Region_ID');
                if (!c || !c.control) return;

                var region_id = $get('ctl00_partialPage_filtersContainer_Geography_MA_Region_ID');
                if (region_id.control)
                {
                    //region_id.control.reset();
                    $updateCheckboxDropdownText('ctl00_partialPage_filtersContainer_Geography_MA_Region_ID', 'MA_Region_ID');
                }
            }
            if (gt && gt == '6')
            {
                regCtrl.control.set_visible(false);
                regCtrlRegion.control.set_visible(true);

                //for getting region ID
                var c = $get('ctl00_partialPage_filtersContainer_Geography_Region_ID');
                if (!c || !c.control) return;

                var region_id = $get('ctl00_partialPage_filtersContainer_Geography_Region_ID');
                if (region_id.control)
                {
                    //region_id.control.reset();
                    $updateCheckboxDropdownText('ctl00_partialPage_filtersContainer_Geography_Region_ID', 'Region_ID');
                }
            }

            reportFiltersResize();
        }
        function refreshPlanList(data) {
            var PlanID = $get('Plan_ID');

            if (PlanID != null) {
                if (!PlanID && !PlanID.control) return;
            }

            clearPlanSelectionList();
            var sectionid = 0;
            var section = data["Section_ID"];

            if (section && section.get_value()) {
                var filtersection = '';
                if ($.isArray(section.value)) {
                    //reset section selection if PBM is selected with other sections. PBM should be selected seperately, it can't be selected with other sections
                    $.each(section.get_value(), function (k, o) {
                        if (filtersection == '') filtersection = "(Section_ID eq " + o;
                        else filtersection = filtersection + " or Section_ID eq " + o;
                    });
                }
                else {
                    filtersection = "(Section_ID eq " + section.get_value();
                }

                var planlist = $('.searchTextBoxFilter #Plan_ID')[0];
                if (planlist && planlist.SearchList) {
                    planlist = planlist.SearchList;
                    filtersection += " ) and substringof('{0}',Plan_Name)&$top=50&$orderby=Plan_Name";

                    planlist.set_queryFormat("$filter=" + filtersection);
                    planlist.set_queryValues();
                }
            }
        }

        function clearPlanSelectionList() {
            var planlist = $('.searchTextBoxFilter #Plan_ID')[0];
            if (planlist && planlist.SearchList) {
                planlist = planlist.SearchList;
                planlist.clearSearchListSelection();
            }
        }
        function rankIndexChanged(s, a) {
            var c = $get('Plan_ID');
            if (!c) return;
            var gt = a.get_item().get_value();
            switch (parseInt(gt, 10)) {
                case -1: $("#filterPlan").show();
                    break;
                default: { $("#filterPlan").hide(); clearPlanSelectionList(); }
                    break;
            }
        }
        function showHidePlanFilter() {
            $("#filterPlan").hide(); clearPlanSelectionList();
            data = $getContainerData("filtersContainer");
            if (data && data["Rank"]) {
                if (data["Rank"].value == -1) {
                    $("#filterPlan").show();
                    refreshPlanList(data);
                }
            }
        }
        
    </script>