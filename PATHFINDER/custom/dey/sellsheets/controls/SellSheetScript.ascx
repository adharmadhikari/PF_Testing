
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SellSheetScript.ascx.cs" Inherits="custom_controls_SellSheetScript" %>
<%@ OutputCache Shared="true" VaryByParam="None" Duration="100" %>


<script type="text/javascript">
    clientManager.add_pageInitialized(ss_pageInitialized);
    clientManager.add_pageLoaded(ss_pageLoaded); 
    clientManager.add_pageUnloaded(ss_pageUnloaded);

    function ss_pageInitialized(sender, args)
    {
        //Added these functions to check the number of records in the grid for enabling and disabling 
        //functionality links.
        var gd = $find("ctl00_Tile3_DraftedSellSheets1_gridDraftedSellSheets$GridWrapper");
        if (gd) gd.add_recordCountChanged(gridDraftedSellSheets_onRecordCount);

        var gc = $find("ctl00_Tile3_CompletedSellSheets1_gridCompletedSellSheets$GridWrapper");
        if (gc) gc.add_recordCountChanged(gridCompletedSellSheets_onRecordCount);

//        var gc = $find("ctl00_Tile3_SellSheetOrderList1_gridSellSheetOrders$GridWrapper");
//        if (gc) gc.add_recordCountChanged(gridSellSheetOrders_onRecordCount);
    }

    function ss_pageLoaded(sender, args)
    {
        $createCheckboxDropdown(regionCtrlID, "Region_ID", null, { 'defaultText': '<%= Resources.Resource.Label_No_Selection %>', 'multiItemText': '<%= Resources.Resource.Label_Multiple_Selection %>' }, null, null);
        $createCheckboxDropdown(DistrictCtrlID, "District_ID", null, { 'defaultText': '<%= Resources.Resource.Label_No_Selection %>', 'multiItemText': '<%= Resources.Resource.Label_Multiple_Selection %>' }, null, null);
        $createCheckboxDropdown(RepCtrlID, "Rep_ID", null, { 'defaultText': '<%= Resources.Resource.Label_No_Selection %>', 'multiItemText': '<%= Resources.Resource.Label_Multiple_Selection %>' }, null, null);
    }

    function ss_pageUnloaded(sender, args)
    {
        var gd = $find("ctl00_Tile3_DraftedSellSheets1_gridDraftedSellSheets$GridWrapper");
        if (gd) gd.remove_recordCountChanged(gridDraftedSellSheets_onDataBound);

        gd = $find("ctl00_Tile3_CompletedSellSheets1_gridCompletedSellSheets$GridWrapper");
        if (gd) gd.remove_recordCountChanged(gridCompletedSellSheets_onDataBound);

        gd = $find("ctl00_Tile3_SellSheetOrderList1_gridSellSheetOrders$GridWrapper");
        if (gd) gd.remove_recordCountChanged(gridSellSheetOrders_onRecordCount);

        clientManager.remove_pageInitialized(ss_pageInitialized);
        clientManager.remove_pageLoaded(ss_pageLoaded);
        clientManager.remove_pageUnloaded(ss_pageUnloaded);
    }     

     function RefreshOrderList()
     {
         var grid = $find("ctl00_Tile3_SellSheetOrderList1_gridSellSheetOrders");
         grid.get_masterTableView().rebind();
     }
     
    //Resumes the drafted sell sheet from the step where the user left off.
    function OpenDraftSellSheet() {
        var grid = getDraftGrid();
        var Sheet_ID = 0;
        var StepNM = "";

        //Clear the plan selection for the first time.
        clientManager.setContextValue("ssSelectedPlansList");
        
        //Gets the selected Sell_Sheet_ID and Current_Step from the draftedsellsheets grid.
        Sheet_ID = grid.get_masterTableView().get_selectedItems()[0]._dataItem.Sell_Sheet_ID;
        StepNM = grid.get_masterTableView().get_selectedItems()[0]._dataItem.Current_Step;

        if (StepNM == null)
            StepNM = "classandtemplateselection";

        clientManager.set_SelectionData({ Sell_Sheet_ID: Sheet_ID }, StepNM);
    }

    //Opens Step1 for editing the completed sell sheet.
    function EditCompletedSellSheet() {
        var grid = $find("ctl00_Tile3_CompletedSellSheets1_gridCompletedSellSheets");
        var Sheet_ID = 0;
        var StepNM = "";

        //Clear the plan selection for the first time.
        clientManager.setContextValue("ssSelectedPlansList");
        
        //Gets the selected Sell_Sheet_ID and Current_Step from the completedsellsheets grid.
        Sheet_ID = grid.get_masterTableView().get_selectedItems()[0]._dataItem.Sell_Sheet_ID;
        StepNM = "classandtemplateselection";
        clientManager.set_SelectionData({ Sell_Sheet_ID: Sheet_ID }, StepNM);
    }

    //Gets reference for draftedsellsheets grid.
    function getDraftGrid() {
        return $find("ctl00_Tile3_DraftedSellSheets1_gridDraftedSellSheets");
    }

    //Gets reference for completedsellsheets grid.
    function getCompletedGrid() {
        return $find("ctl00_Tile3_CompletedSellSheets1_gridCompletedSellSheets");
    }

    //Opens modal window for removing selected sell sheet.
    function OpenRemoveSellSheets(SellSheetDesc) {
        var oManager = GetRadWindowManager();
        var grid = null;
        
        //Getting the clientkey
        var ClientKey = clientManager.get_ClientKey();
        
        if (SellSheetDesc == "Drafted") {
            grid = getDraftGrid();
        }
        else {
            grid = getCompletedGrid();
        }

        //Opens popup window to remove the sell sheet.
        var str = "custom/" + ClientKey + "/sellsheets/all/RemoveSellSheet.aspx?SellSheetDesc=" + SellSheetDesc;
        var Sheet_ID = grid.get_masterTableView().get_selectedItems()[0]._dataItem.Sell_Sheet_ID;

        str = str + "&Sell_Sheet_ID=" + Sheet_ID;
        var oWnd = radopen(str, "RemoveWnd");
        oWnd.setSize(500, 110);
        oWnd.Center();
    }

    function OpenPreview()
    {
        var oManager = GetRadWindowManager();
        var grid = getCompletedGrid();

        //Getting the clientkey
        var ClientKey = clientManager.get_ClientKey();

        var Sheet_ID = grid.get_masterTableView().get_selectedItems()[0]._dataItem.Sell_Sheet_ID;

        //Opens popup window to remove the sell sheet.
        var str = "custom/" + ClientKey + "/sellsheets/all/CompletedSellSheetPreview.aspx?Sell_Sheet_ID=" + Sheet_ID ;
//        var oWnd = radopen(str, "PreviewWnd");
//        oWnd.setSize(960, 650);
        //        oWnd.Center();
        $openWindow(str);
    }
    
    //Opens modal window for Emailing a Sell Sheet.
    function openEmailSellSheet()
    {
        var clientKey = clientManager.get_ClientKey();

        var grid = getCompletedGrid();

        var masterTable = grid.get_masterTableView();
        var selectedRow = masterTable.get_selectedItems()[0];
        var cell = masterTable.getCellByColumnUniqueName(selectedRow, "Created_DT");

        var sheetDate = cell.innerHTML;

        var sheetID = grid.get_masterTableView().get_selectedItems()[0]._dataItem.Sell_Sheet_ID;
        var sheetName = grid.get_masterTableView().get_selectedItems()[0]._dataItem.Sell_Sheet_Name;

        var str = "custom/" + clientKey + "/sellsheets/all/EmailSellSheet.aspx?Sell_Sheet_ID=" + sheetID;

        str = str + "&Sell_Sheet_Name=" + sheetName;
        str = str + "&Sell_Sheet_Date=" + sheetDate;

        var win = radopen(str, "EmailSSWnd");
        win.setSize(400, 275);
        win.Center();
    }

    //Generates PDF
    function openPDFExport()
    {
        var clientKey = clientManager.get_ClientKey();

        var grid = getCompletedGrid();

        var randomnumber = Math.random() * 101;

        var sheetID = grid.get_masterTableView().get_selectedItems()[0]._dataItem.Sell_Sheet_ID;

        var str = "custom/" + clientKey + "/sellsheets/all/GenerateSellSheetPDF.aspx?Sell_Sheet_ID=" + sheetID + "&rnd=" + randomnumber;

        //$.post(str);

        window.open(str);
        
        //var win = radopen(str, "PDFSSWnd");
        // win.setSize(375, 250);
        //win.Center();
    }

    //Hide Remove and Resume links for the firsttime.
    function gridDraftedSellSheets_onRecordCount(sender, args)
    {
        var mt = sender.get_masterTableView();

        //If there are no records in Drafted sellsheets grid then disable functionality links 
        //else enable them and select first record.
        //        if (mt.get_virtualItemCount() == 0)
        //        {
        //            $get("ctl00_Tile3_RemoveDraft").disabled = true;
        //            $get("ctl00_Tile3_ResumeDraft").disabled = true;
        //        }
        //        else
        //        {
        //            mt.selectItem(0);
        //            $get("ctl00_Tile3_RemoveDraft").disabled = false;
        //            $get("ctl00_Tile3_ResumeDraft").disabled = false;
        //        }

        //If there are records in the drafted sell sheets grid then clear selected items.
        if (mt.get_virtualItemCount() != 0)
            mt.clearSelectedItems();
        
        $get("ctl00_Tile3_RemoveDraft").style.display = "none";
        $get("ctl00_Tile3_ResumeDraft").style.display = "none";
        $get("separator1").style.display = "none";
        $get("separator2").style.display = "none";

    }

    //On completed sellsheets row selection hide the orderslist and new orders panels.
    function gridCompletedSellSheets_OnRowSelecting(sender, args)
    {
        if ($get("NewOrder") != null)
            $get("NewOrder").style.display = "none";
        if ($get("PreviousOrders") != null)
            $get("PreviousOrders").style.display = "none";

        //Reset the new order form.
        $resetContainer("NewOrder");

        //Clear invalid CSS class from the new order form
        $("#NewOrder").find("input").removeClass("invalid");
        $("#NewOrder").find("div").removeClass("invalid");

        var Sheet_ID = args._dataKeyValues.Sell_Sheet_ID;

        var clientKey = clientManager.get_ClientKey();

        if ($("#ctl00_Tile3_previewcompleted").length > 0)
            $("#ctl00_Tile3_previewcompleted")[0].src = "custom/" + clientKey + "/sellsheets/all/CompletedSellSheetPreview.aspx?Sell_Sheet_ID=" + Sheet_ID;
    }   

    //On drafted sellsheets row selection show Remove/Resume links.
    function gridDraftedSellSheets_OnRowSelecting(sender, args)
    {
        $get("ctl00_Tile3_RemoveDraft").style.display = "";
        $get("ctl00_Tile3_ResumeDraft").style.display = "";
        $get("separator1").style.display = "";
        $get("separator2").style.display = "";
    }
    

    function gridCompletedSellSheets_onRecordCount(sender, args)
    {
        var mt = sender.get_masterTableView();

        //If there are no records in Completed sellsheets grid then disable functionality links 
        //else enable them and select first record.
        if (mt.get_virtualItemCount() == 0)
        {
            $get("ctl00_Tile3_EditSellSheet").style.display = "none";
            $get("ctl00_Tile3_RemoveSellSheet").style.display = "none";
            $get("ctl00_Tile3_EmailSellSheet").style.display = "none";
            $get("ctl00_Tile3_ExportSellSheet").style.display = "none";
            $get("separator3").style.display = "none";
            $get("separator4").style.display = "none";
            $get("separator5").style.display = "none";
            $get("separator6").style.display = "none";
        }
        else
        {
            mt.selectItem(0);
            $get("ctl00_Tile3_EditSellSheet").style.display = "";
            $get("ctl00_Tile3_RemoveSellSheet").style.display = "";
            $get("ctl00_Tile3_EmailSellSheet").style.display = "";
            $get("ctl00_Tile3_ExportSellSheet").style.display = "";
            $get("separator3").style.display = "";
            $get("separator4").style.display = "";
            $get("separator5").style.display = "";
            $get("separator6").style.display = "";
        }
    }


//    function gridSellSheetOrders_onRecordCount(sender, args)
//    {
//        var mt = sender.get_masterTableView();

//        //If there are no records in orderlist grid then disable functionality links 
//        //else enable them and select first record.
//        if (mt.get_virtualItemCount() == 0)
//        {
//            // $get("ctl00_Tile3_SellSheetOrderList1_ReOrder").disabled = true;
//            $get("ctl00_Tile3_SellSheetOrderList1_ReOrder").style.display = "none";
//        }
//        else
//        {
//            mt.selectItem(0);
//            //$get("ctl00_Tile3_SellSheetOrderList1_ReOrder").disabled = false;
//            $get("ctl00_Tile3_SellSheetOrderList1_ReOrder").style.display = "";
//        }
//    }
    
    //This function is called when "Order Prints" link is clicked from formulary sell sheets - preview panel.
    function OpenOrderDtls()
    {
        //Show Orderlist and Neworder panels.
        $get("NewOrder").style.display = "";
        $get("PreviousOrders").style.display = "";

        //reset_sellsheetordercontainer();

        var region_id = $get("Region_ID").control;
        region_id.clear();
        var url = "custom/Dey/sellsheets/services/DeyDataService.svc" + "/SellSheetTerritorySet?$filter=User_Level eq 1 & $orderby=Name";
        $.getJSON(url, null, function(result, status)
        {
            var d = result.d;
            $loadPinsoListItems(region_id, d, null, -1);
        });
        $updateCheckboxDropdownText(regionCtrlID, region_id);

        var District_id = $get("District_ID").control;
        District_id.clear();
        var url = "custom/Dey/sellsheets/services/DeyDataService.svc" + "/SellSheetTerritorySet?$filter=User_Level eq 2 & $orderby=Name";
        $.getJSON(url, null, function(result, status)
        {
            var d = result.d;
            $loadPinsoListItems(District_id, d, null, -1);
        });
        $updateCheckboxDropdownText(DistrictCtrlID, District_id);

        var Rep_id = $get("Rep_ID").control;
        Rep_id.clear();
        var url = "custom/Dey/sellsheets/services/DeyDataService.svc" + "/SellSheetTerritoryRepsSet?$filter=User_Level eq 3 & $orderby=Name";
        $.getJSON(url, null, function(result, status)
        {
            var d = result.d;
            $loadPinsoListItems(Rep_id, d, null, -1);
        });
        $updateCheckboxDropdownText(RepCtrlID, Rep_id);

        var iframe = $('#ctl00_Tile3_framerep');
        iframe.removeAttr("src");

        //Set the filter to populate all the records only for selected Sell_Sheet_ID from completed sell sheet list.       
        var grid = $find("ctl00_Tile3_CompletedSellSheets1_gridCompletedSellSheets");
        var Sheet_ID = grid.get_masterTableView().get_selectedItems()[0]._dataItem.Sell_Sheet_ID;

        var gdOrders = $find("ctl00_Tile3_SellSheetOrderList1_gridSellSheetOrders").get_masterTableView();
        $setGridFilter(gdOrders, "Sell_Sheet_ID", Sheet_ID, "EqualTo", "System.Int32");
        gdOrders.rebind();

        $('#ctl00_Tile3_SelectedSheetID').val(Sheet_ID.toString());
    }

    function rdcmbRep_DropDownClosed(sender, args)
    {
        var vals = sender.get_element().checkboxList.get_values();
        if (vals)
        {
            if ($.isArray(vals))
                vals = vals.join(",");
            $('#ctl00_Tile3_SelectedRepID').val(vals);
        }
    }

    function LoadRepData()
    {
        var sheetID = $('#ctl00_Tile3_SelectedSheetID').val();
        var repID = $('#ctl00_Tile3_SelectedRepID').val();
        if (repID.length > 0)
        {
            var url = "custom/dey/sellsheets/all/New_SellSheetOrder.aspx?SellSheetID=" + sheetID + "&RepID=" + repID;
            var iframe = $('#ctl00_Tile3_framerep');
            iframe.attr("src", url);
            iframe.attr("height", "100%");
        }
        else
            alert("Please select a Sales Rep from Dropdownlist");
    }

    function rdcmbDistrict_DropDownClosed(sender, args)
    {
        var vals = sender.get_element().checkboxList.get_values();
        var Rep_id = $get("Rep_ID").control;
        Rep_id.clear();
        var eq = '';
        if (vals != null)
        {
            if ($.isArray(vals))
            {
                $.each(vals, function(intIndex, objValue)
                {
                    eq += "Parent_Territory eq '" + objValue + "'";
                    if (intIndex != (vals.length - 1))
                        eq += " or ";
                });
            }
            else
                eq = "Parent_Territory eq '" + vals + "'";
            url = "custom/dey/sellsheets/services/DeyDataService.svc" + "/SellSheetTerritoryRepsSet?$filter=" + eq + " and User_Level eq 3 & $orderby=Name";
        }
        else
            url = "custom/dey/sellsheets/services/DeyDataService.svc" + "/SellSheetTerritoryRepsSet?$filter=User_Level eq 3 & $orderby=Name";

        
        $.getJSON(url, null, function(result, status)
        {
            var d = result.d;
            $loadPinsoListItems(Rep_id, d, null, -1);
        });
        $updateCheckboxDropdownText(RepCtrlID, "Rep_ID");

    }
    function rdcmbRegion_DropDownClosed(sender, args)
    {
        var vals = sender.get_element().checkboxList.get_values();
        var District_id = $get("District_ID").control;
        District_id.clear();
        var eq = '';
        if (vals != null)
        {
            if ($.isArray(vals))
            {
                $.each(vals, function(intIndex, objValue)
                {
                    eq += "Parent_Territory_ID eq '" + objValue + "'";
                    if (intIndex != (vals.length - 1))
                        eq += " or ";
                });
            }
            else
                eq = "Parent_Territory_ID eq '" + vals + "'";
            url = "custom/dey/sellsheets/services/DeyDataService.svc" + "/SellSheetTerritorySet?$filter=" + eq + " and User_Level eq 2 & $orderby=Name";
        }
        else
            var url = "custom/Dey/sellsheets/services/DeyDataService.svc" + "/SellSheetTerritorySet?$filter=User_Level eq 2 & $orderby=Name";
        $.getJSON(url, null, function(result, status)
        {
            var d = result.d;
            $loadPinsoListItems(District_id, d, null, -1);
        });
        $updateCheckboxDropdownText(DistrictCtrlID, "District_ID");

    }
    function reset_sellsheetordercontainer()
    {
        $resetContainer('NewOrder');
        var rep_grid = $get(GridCtrlID).control;
        rep_grid.GridWrapper.clearGrid();
        var mt = rep_grid.get_masterTableView();
        $clearGridFilter(mt, "ID");
    }

    function OpenEditAddress()
    {
        var sheet_ID = $('#Selected_SheetID').val();
        var Rep_ID = $('#Selected_RepID').val();

        var width = 300;
        var height = 400;
        var left = (screen.width - width) / 2;
        var top = (screen.height - height) / 2;

        //    var params = 'width=' + width + ', height=' + height;
        //    params += ', top=' + top + ', left=' + left;
        //    params += ', directories=no';
        //    params += ', location=no';
        //    params += ', menubar=no';
        //    params += ', resizable=no';
        //    params += ', scrollbars=no';
        //    params += ', status=no';
        //    params += ', toolbar=no';

        var url = "EditAddress.aspx?sheet_ID=" + sheet_ID + "&Rep_ID=" + Rep_ID;


        window.open(url, "EditAddress", "width= 300,height=400,left=" + left + ", top=" + top);

        // window.open(url, 'EditAddress', params);
        //if (window.focus) { newwin.focus() } 
        return false;

    }

    function gridRep_OnRowSelecting(sender, args)
    {
        var Rep_ID = args._dataKeyValues.ID;
        $('#Selected_RepID').val(Rep_ID);
    }

   
</script>