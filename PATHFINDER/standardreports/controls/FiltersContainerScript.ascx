<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FiltersContainerScript.ascx.cs" Inherits="standardreports_controls_FiltersContainerScript" %>
<%@ OutputCache Shared="true" VaryByParam="None" Duration="120" %>  
  
    <script type="text/javascript">

        clientManager.add_pageLoaded(filters_pageLoaded, "moduleOptionsContainer");
        clientManager.add_pageUnloaded(filters_pageUnloaded, "moduleOptionsContainer");

        var _filterPageDefaults = [];
        
        function filters_pageLoaded(sender, args)
        {
            checkSectionAccountType();
            $clearAlert(); //make sure any previous alerts are cleared.

            $addHandler($get("requestReportButton"), "click", requestReport);
            $addHandler($get("clearFiltersButton"), "click", clearReportFilters);

            $reloadContainer("moduleOptionsContainer", clientManager.get_SelectionData());

            standardreports_content_resize();
        }
        //for showing hiding appropriate account type and section filter
        function checkSectionAccountType()
        {
            var accountsubtype = $get('ctl00_partialPage_filtersContainer_AccountType_Segment_ID');
            var accounttype = $find('ctl00_partialPage_filtersContainer_AccountType_Class_Partition');
            if (accountsubtype && accountsubtype.control)
            {
                accountsubtype.control.set_visible(false);
                $("#filterAccountSubType").hide();
            }
            //    if (clientManager.get_SelectionData() == null)
            //    {
            //        clearReportFilters();
            //    }
            if ((clientManager.get_SelectionData() && clientManager.get_SelectionData()["Section_ID"] && clientManager.get_SelectionData()["Section_ID"].value == 17)
                || ($getContainerData("filtersContainer") && $getContainerData("filtersContainer")["Section_ID"] && $getContainerData("filtersContainer")["Section_ID"].value == 17))
            {
                refreshAccountType(17);
                if (accountsubtype && accountsubtype.control)
                {
                    accountsubtype.control.set_visible(true);
                    $("#filterAccountSubType").show();
                }
            }
            //code not required as pbm will be having regional plans            
//            else if ((clientManager.get_SelectionData() && clientManager.get_SelectionData()["Section_ID"] && clientManager.get_SelectionData()["Section_ID"].value == 4)
//                || ($getContainerData("filtersContainer") && $getContainerData("filtersContainer")["Section_ID"] && $getContainerData("filtersContainer")["Section_ID"].value == 4))
//            {
//                refreshAccountType(0);
//                if(accounttype)
//                {
//                    accounttype.findItemByValue(1).select();
//                    accounttype.disable();
//                }
//            }
            showHidePlanFilter()
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

            //get Section_Id from filter container selection section channel menu is invisible else get it from channel menu selection
//            if (clientManager.get_EffectiveChannel() > 0)
//                data["Section_ID"] = clientManager.get_EffectiveChannel();

            var geogID = data["Geography_ID"];

            if ((typeof (geogID) == "undefined"))
                data["Geography_ID"] = "US";

            if (data["Section_ID"] && data["Section_ID"].value != 17 && data["Segment_ID"])
                delete data["Segment_ID"];

            var regionTypeID = data["Region_Type_ID"];
            if ((typeof (regionTypeID) == "undefined") && clientManager.get_Module() == "formularydrilldown")
                data["Region_Type_ID"] = 0;

            //set rank null so that we can remove this filter
            if (data["Rank"] && (data["Rank"].get_value() == -1 || data["Rank"].get_value() == "999999"))
            {
                delete (data["Rank"]);
            }

            var classPartition = data["Class_Partition"];
            if (clientManager.get_Module() == "geographiccoverage" && (typeof (classPartition) == "undefined"))
            {
                data["Class_Partition"] = 2;
                if (clientManager.get_Region())
                    data["Geography_ID"] = clientManager.get_Region();
            }
            if (data["Section_ID"] && data["Section_ID"].value == 17 && typeof (classPartition) != "undefined" && classPartition.get_value() == 26)
            {
                data["Region_Type_ID"] = 6;
                data["Class_Partition"] = 2;

                //set ma_region ids as geography_id
                data["Geography_ID"] = data["MA_Region_ID"];
            }

            if (data["Section_ID"] && data["Section_ID"].value == 17 && typeof (classPartition) != "undefined" && classPartition.get_value() == 27)
            {
                data["Class_Partition"] = 2;
                data["Region_Type_ID"] = 7;
                //set PDP region ids as geography_id
                data["Geography_ID"] = data["Region_ID"];
            }
            if (data["Region_ID"])
                delete (data["Region_ID"]);

            if (data["MA_Region_ID"])
                delete (data["MA_Region_ID"]);

            //code to handle managed medicaid with rank filter selected
            if (data["Section_ID"] && data["Section_ID"].value == 6 && !data["Rank"])
            {
                if ((typeof (classPartition) == "undefined"))
                {
                    data["Class_Partition"] = 2;
                }
            }

            if (data["Is_Predominant"] && data["Is_Predominant"].get_value() == 1)
            {
                data["Is_Predominant"].set_value("true");
            }

            //if rank is null or zero remove rank from data

            if ($validateContainerData("filtersContainer", data, '<%= Resources.Resource.Label_Report_Filters %>'))
            {
                if (clientManager.get_Module() == "formularycoverage")
                    data["Thera_ID"] = data["Market_Basket_ID"]; //temp change- rename the property in entity model later
                if (clientManager.get_Module() == "tiercoveragecomparison" || clientManager.get_Module() == "formularystatus"
                                                || clientManager.get_Module() == "formularydrilldown"
                                                || clientManager.get_Module() == "formularycoverage" || clientManager.get_Module() == "geographiccoverage") 
                {
                    data["Is_All_Section"] = false; //all section is not selected set default

                    if (!data["Section_ID"]) //all section is selected
                    {
                        data["Is_All_Section"] = true; //set the filter for all section is selected
                        delete data["Segment_ID"];
                        data["excludeSegment"] = { isExtension: true, name: "Segment_ID", value: 8 };
                    }
                    else if (data["Section_ID"] && data["Section_ID"].value == 4) 
                    {//only PBM is selected
                        data["onlyPBM"] = { isExtension: true, name: "onlyPBM", value: true };
                        data["excludeSegment"] = { isExtension: true, name: "Segment_ID", value: 8 };
                        delete data["Segment_ID"];
                        delete data["Section_ID"];
                    }
                    else if (data["Section_ID"] && $.isArray(data["Section_ID"].value))
                        data["excludeSegment"] = { isExtension: true, name: "Segment_ID", value: 8 };
                    else if (data["Section_ID"] && !data["Segment_ID"]) 
                    {
                        if(data["Section_ID"].value == 1)
                            data["Segment_ID"] = { name: "Segment_ID", value: 1 };
                        if (data["Section_ID"].value == 17)
                            data["Segment_ID"] = { name: "Segment_ID", value: 2 };
                        if (data["Section_ID"].value == 9)
                            data["Segment_ID"] = { name: "Segment_ID", value: 3 };
                        if (data["Section_ID"].value == 6)
                            data["Segment_ID"] = { name: "Segment_ID", value: 6 };
                        if (data["Section_ID"].value == 4)
                            data["Segment_ID"] = { name: "Segment_ID", value: 4 };
                    }
                }
                //If livesdistribution report is selected with All option.
                else if (clientManager.get_Module() == "livesdistribution" && !data["Section_ID"])
                {
                    delete data["Segment_ID"];
                }
                else if (data["Section_ID"] && data["Section_ID"].value == 17 && !data["Segment_ID"]
                    && (clientManager.get_Module() == "geographiccoverage"))
                    data["Segment_ID"] = { name: "Segment_ID", value: 2 };
                else if (data["Section_ID"] && data["Section_ID"].value == 17 && (clientManager.get_Module() == "livesdistribution"
                                                                                    || clientManager.get_Module() == "affiliationsformulary"))
                    delete data["Segment_ID"];   

                if ($get('Plan_ID') && $('#Plan_ID_DATA').val() != "")
                    data["Plan_ID"] = $('#Plan_ID_DATA').val().split(',');
                else
                    delete data["Plan_ID"];

                data["__options"] = $getContainerData("optionsContainer");
                
                clientManager.set_SelectionData(data);
            }
        }

        function clearReportFilters() {
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
        function RegionLoad(s, a) {

            var l = s.get_element();
            var region_ID = $get('Region_ID');
            if (region_ID) {
                var region_ID_control = region_ID.control;
                if (region_ID_control)
                    $loadPinsoListItems(region_ID_control, pdpregions, null, -1);
            }
            else {
                $createCheckboxDropdown('ctl00_partialPage_filtersContainer_Geography_Region_ID', 'Region_ID', null, { 'defaultText': '--All--', 'multiItemText': '--Change Selection--' }, null, 'moduleOptionsContainer');
                var region_ID_control = $get('Region_ID').control;
                $loadPinsoListItems(region_ID_control, pdpregions, null, -1);
            };
            $updateCheckboxDropdownText('ctl00_partialPage_filtersContainer_Geography_Region_ID', 'Region_ID');
            var c = $get('ctl00_partialPage_filtersContainer_Geography_rcbGeographyType'); var s = $get('ctl00_partialPage_filtersContainer_Geography_Region_ID');
            if (!c || !c.control || !s || !s.control) return;
            var gt = c.control.get_value();
            switch (parseInt(gt, 10)) {
                case 6: s.control.set_visible(true);
                    break;
                default: s.control.set_visible(false);
                    break;
            }
        }
        //MA Regions
        function MARegionLoad(s, a) {

            var l = s.get_element();
            var region_ID = $get('MA_Region_ID');
            if (region_ID) {
                var region_ID_control = region_ID.control;
                if (region_ID_control)
                    $loadPinsoListItems(region_ID_control, maregions, null, -1);
            }
            else {
                $createCheckboxDropdown('ctl00_partialPage_filtersContainer_Geography_MA_Region_ID', 'MA_Region_ID', null, { 'defaultText': '--All--', 'multiItemText': '--Change Selection--' }, null, 'moduleOptionsContainer');
                var region_ID_control = $get('MA_Region_ID').control;
                $loadPinsoListItems(region_ID_control, maregions, null, -1);
            };
            $updateCheckboxDropdownText('ctl00_partialPage_filtersContainer_Geography_MA_Region_ID', 'MA_Region_ID');
            var c = $get('ctl00_partialPage_filtersContainer_Geography_rcbGeographyType');
            var s = $get('ctl00_partialPage_filtersContainer_Geography_MA_Region_ID');
            if (!c || !c.control || !s || !s.control) return;
            var gt = c.control.get_value();
            switch (parseInt(gt, 10)) {
                case 5: s.control.set_visible(true);
                    break;
                default: s.control.set_visible(false);
                    break;
            }
        }
        //end of ma and PDP region load- - used for formulary drilldown

        function geoSelectedIndexChanged(s, a) {
            var l = $get('ctl00_partialPage_filtersContainer_Geography_Geography_ID');
            var gt = a.get_item().get_value();
            var regCtrl = $get('ctl00_partialPage_filtersContainer_Geography_Geography_ID');
            var regCtrlRegion = $get('ctl00_partialPage_filtersContainer_Geography_Region_ID');
            var regCtrlMARegion = $get('ctl00_partialPage_filtersContainer_Geography_MA_Region_ID');

            if (!regCtrl || !regCtrl.control) return;
            loadranktypefilter(2); //hide rank filter and show it only for national
            if (gt && gt == '1') {
                $loadListItems(l, null, { value: '', text: '' }
         );
                l.control.set_visible(false);
                regCtrl.control.set_visible(false);
                loadranktypefilter(1);//show rankt only for national
            }
            if (gt && gt == '2') {
                $loadListItems(l, clientManager.get_RegionListOptions());
                l.control.set_visible(true);
                regCtrl.control.set_visible(true);
            }
            if (gt && gt == '3') {
                $loadListItems(l, clientManager.get_States());
                l.control.set_visible(true);
                regCtrl.control.set_visible(true);
            }
            if (gt && gt == '4') {
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
                if (region_id.control) {
                    //region_id.control.reset();
                    $updateCheckboxDropdownText('ctl00_partialPage_filtersContainer_Geography_MA_Region_ID', 'MA_Region_ID');
                }
            }
            if (gt && gt == '6') {
                regCtrl.control.set_visible(false);
                regCtrlRegion.control.set_visible(true);

                //for getting region ID
                var c = $get('ctl00_partialPage_filtersContainer_Geography_Region_ID');
                if (!c || !c.control) return;

                var region_id = $get('ctl00_partialPage_filtersContainer_Geography_Region_ID');
                if (region_id.control) {
                    //region_id.control.reset();
                    $updateCheckboxDropdownText('ctl00_partialPage_filtersContainer_Geography_Region_ID', 'Region_ID');
                }
            }

            reportFiltersResize();
        }
        //on section dropdown closed event
        function sectionDropDownClosed(s, a)
        {
            var data = $getContainerData("filtersContainer");
            var section = data["Section_ID"];
            
            var accountsubtype = $get('ctl00_partialPage_filtersContainer_AccountType_Segment_ID');
            var accounttype = $find('ctl00_partialPage_filtersContainer_AccountType_Class_Partition');

            if (accounttype) accounttype.enable();

            if (accountsubtype && accountsubtype.control)
            {
                refreshAccountType(0);
                accountsubtype.control.set_visible(false);
                $("#filterAccountSubType").hide();
            }
            var report_type = $find('ctl00_partialPage_filtersContainer_ReportType_Rank');
            if (section && section.get_value())
            {
                if ($.isArray(section.value)) {
                    if (clientManager.get_Module() == "tiercoveragecomparison" || clientManager.get_Module() == "formularystatus"
                            || clientManager.get_Module() == "formularydrilldown" || clientManager.get_Module() == "formularycoverage") 
                    report_type.disable();
                    // changed logic - 06/19/2012 - Deepthi
                    //reset section selection if PBM is selected with other sections. PBM should be selected seperately, it can't be selected with other sections
                    //$.each(section.get_value(), function(k, o) {
                    //if (o == 4) {
                    // var sectionidctrl = $find(s.get_id());
                    //$alert('PBM should be selected seperately.', "Report Filters", null, null, Pathfinder.UI.AlertType.Warning); 
                    // resetSectionSelection('MarketSegmentIDOptionList', 4);
                    // }
                //else if (data["Drug_ID"]) resetDrugSelection($find('DrugIDList'),false); });
                $.each(section.get_value(), function(k, o) { if (data["Drug_ID"]) resetDrugSelection($find('DrugIDList'), false); });
                }
                else
                {
                    //report_type.enable();
                    if (section.get_value() == 17)
                    {
                        refreshAccountType(section.get_value());
                        //hide Account subtype for lives distribution, affiliations formulary and geographic coverage report
                        if (clientManager.get_Module() != "livesdistribution" && clientManager.get_Module() != "affiliationsformulary"
                            && clientManager.get_Module() != "geographiccoverage" && accountsubtype && accountsubtype.control)
                        {
                            accountsubtype.control.set_visible(true);
                            $("#filterAccountSubType").show();
                        }
                    }
                    else if (section.get_value() == 4)
                    {
//                        refreshAccountType(0);
//                        if (accounttype)
//                        {
//                            accounttype.findItemByValue(1).select();
//                            accounttype.disable();
//                        }
                        if (clientManager.get_Module() == "tiercoveragecomparison" || clientManager.get_Module() == "formularystatus"
                            || clientManager.get_Module() == "formularydrilldown" || clientManager.get_Module() == "formularycoverage") 
                        report_type.disable();
                    }
                    else 
                    {
                        if (clientManager.get_Module() == "tiercoveragecomparison" || clientManager.get_Module() == "formularystatus"
                            || clientManager.get_Module() == "formularydrilldown" || clientManager.get_Module() == "formularycoverage")
                        report_type.enable();
                    }
                }
            }
            else {
                if (clientManager.get_Module() == "tiercoveragecomparison" || clientManager.get_Module() == "formularystatus"
                            || clientManager.get_Module() == "formularydrilldown" || clientManager.get_Module() == "formularycoverage")
                    report_type.enable();
            }
            refreshPlanList(data);
        }

        //for resetting the section selection
        function resetSectionSelection(controlid, sectionID)
        {
            var market_segment_id = $get(controlid).control;
            $loadPinsoListItems(market_segment_id, channelsList, null, sectionID);
            var accounttype = $find('ctl00_partialPage_filtersContainer_AccountType_Class_Partition');
            //code not required as pbm will be having regional plans
//            if (sectionID == 4)
//            {
//                refreshAccountType(0);
//                accounttype.findItemByValue(1).select();
//                accounttype.disable();
//            }
        }
       
        //for refreshing the Account Type menu based on sectionid
        function refreshAccountType(sectionid)
        {
            if (!$find('ctl00_partialPage_filtersContainer_AccountType_Class_Partition')) return;
            var url = "services/PathfinderService.svc/AccountTypesSet?$filter=Section_ID eq " + sectionid + " and Plan_Classification_ID ne 26 and  Plan_Classification_ID ne 27 " + "&$orderby=Plan_Classification_ID";
            //if formulary drilldown then return MA and PDP also for account type
            if(clientManager.get_Module() == "formularydrilldown")
                url = "services/PathfinderService.svc/AccountTypesSet?$filter=Section_ID eq " + sectionid + "&$orderby=Plan_Classification_ID";
            
            $.getJSON(url, null, function(result, status)
            {
                var d = result.d;
                $loadListItems($find('ctl00_partialPage_filtersContainer_AccountType_Class_Partition'), d, !d.length ? { value: "", text: "no records available"} : null, 0, 'Plan_Classification_ID', 'Account_Type_Name');

            });

        }
        //for showing/hiding rank based on geographyid filter
        function loadranktypefilter(GeographyID)
        {
            var comboRankType = $find('ctl00_partialPage_filtersContainer_ReportType_Rank');
            if (!comboRankType) return;
            //remove the ranks from dropdown if geographyid is other than national; refer to Lkp_Rank_Types table for all possible ranks
            if (GeographyID != 1)
            {
                if (comboRankType.findItemByValue(10)) comboRankType.findItemByValue(10).set_visible(false);
                if (comboRankType.findItemByValue(20)) comboRankType.findItemByValue(20).set_visible(false);
                if (comboRankType.findItemByValue(1000)) comboRankType.findItemByValue(1000).set_visible(false);
                if (comboRankType.findItemByValue(13)) comboRankType.findItemByValue(13).set_visible(false);
            }
            else
            {
                if (comboRankType.findItemByValue(10)) comboRankType.findItemByValue(10).set_visible(true);
                if (comboRankType.findItemByValue(20)) comboRankType.findItemByValue(20).set_visible(true);
                if (comboRankType.findItemByValue(1000)) comboRankType.findItemByValue(1000).set_visible(true);
                if (comboRankType.findItemByValue(13)) comboRankType.findItemByValue(13).set_visible(true);
            }
        }
        //for loading parent plans by section
        function LoadParentPlanListBySection(sender, args)
        {
            if (!$find('ctl00_partialPage_filtersContainer_ParentPlans_Parent_ID')) return;
            var val = sender.get_value();

            if (val == 0) val = null;
            refreshParentPlanList(val);

        }
        //refreshing parent plans by section
        function refreshParentPlanList(sectionID)
        {
            if (sectionID == -1)
                sectionID = $find('ctl00_partialPage_filtersContainer_FilterSection_Section_ID').get_value();

            if (!$find('ctl00_partialPage_filtersContainer_ParentPlans_Parent_ID')) return;
            var url = "services/PathfinderService.svc/ParentPlanSet?$filter=Section_ID eq " + sectionID + "&$orderby=Plan_Name";
            $.getJSON(url, null, function(result, status)
            {
                var d = result.d;
                $loadListItems($find('ctl00_partialPage_filtersContainer_ParentPlans_Parent_ID'), d, !d.length ? { value: "", text: "no records available"} : null, 0, 'Parent_ID', 'Plan_Name');

            });
        }
        //for resetting the drug selection if multiple sections and drugs are selected. Only one drug should be selected with multiple sections or vice versa
        function resetDrugSelection(DrugIDList, showmsg)
        {
            var section;
            var selDrug;
            var DrugItems;
            if ($getContainerData("filtersContainer"))
            {
                section = $getContainerData("filtersContainer")["Section_ID"];
                selDrug = $getContainerData("filtersContainer")["Drug_ID"];
                DrugItems = clientManager.get_DrugListOptions()[$getContainerData("filtersContainer")["Market_Basket_ID"].value];
            }

            if (section && section.get_value() && $.isArray(section.value) && selDrug && selDrug.get_value() && $.isArray(selDrug.value) && clientManager.get_Module() != "formularydrilldown")
            {
                var msg = 'With multiple section selection, only one drug can be selected and with multiple drugs only one section can be selected.';
                if (showmsg == true) $alert(msg, "Report Filters", null, null, Pathfinder.UI.AlertType.Warning);
                $loadPinsoListItems(DrugIDList, DrugItems, null, DrugItems.length > 1 ? -1 : DrugItems[0].ID, true);
                $updateCheckboxDropdownText('ctl00_partialPage_filtersContainer_DrugSelection_Drug_ID', DrugIDList);
            }
        }
        function refreshPlanList(data)
        {
            var PlanID = $get('Plan_ID');

            if (PlanID != null) 
            {
                if (!PlanID && !PlanID.control) return;
            }
            
            clearPlanSelectionList();
            var sectionid = 0;
            var section = data["Section_ID"];

            if (section && section.get_value())
            {
                var filtersection = '';
                if ($.isArray(section.value))
                {
                    //reset section selection if PBM is selected with other sections. PBM should be selected seperately, it can't be selected with other sections
                    $.each(section.get_value(), function(k, o)
                    {
                        if (filtersection == '') filtersection = "(Section_ID eq " + o;
                        else filtersection = filtersection + " or Section_ID eq " + o;
                    });
                }
                else
                {
                    filtersection = "(Section_ID eq " + section.get_value();
                }

                var planlist = $('.searchTextBoxFilter #Plan_ID')[0];
                if (planlist && planlist.SearchList)
                {
                    planlist = planlist.SearchList;
                    filtersection += " ) and substringof('{0}',Plan_Name)&$top=50&$orderby=Plan_Name";

                    planlist.set_queryFormat("$filter=" + filtersection);
                    planlist.set_queryValues();
                }
            }
        }
        function clearPlanSelectionList()
        {
            var planlist = $('.searchTextBoxFilter #Plan_ID')[0];
            if (planlist && planlist.SearchList)
            {
                planlist = planlist.SearchList;
                planlist.clearSearchListSelection();
            }
        }
        function rankIndexChanged(s, a)
        {           
            var c = $get('Plan_ID');
            if (!c ) return;
            var gt = a.get_item().get_value();
            switch (parseInt(gt, 10))
            {
                case -1: $("#filterPlan").show();
                    break;
                default: { $("#filterPlan").hide(); clearPlanSelectionList(); }
                    break;
            }
        }
        function showHidePlanFilter()
        {
            $("#filterPlan").hide(); clearPlanSelectionList();
            data = $getContainerData("filtersContainer");
            if (data && data["Rank"])
            {
                if (data["Rank"].value == -1)
                {
                    $("#filterPlan").show();
                    refreshPlanList(data);
                }
            }
        }
    </script>