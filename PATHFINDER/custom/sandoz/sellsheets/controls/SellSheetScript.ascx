
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SellSheetScript.ascx.cs" Inherits="custom_controls_SellSheetScript" %>
<%@ OutputCache Shared="true" VaryByParam="None" Duration="100" %>


<script type="text/javascript">
    clientManager.add_pageInitialized(ss_pageInitialized);
    clientManager.add_pageUnloaded(ss_pageUnloaded);

    function ss_pageInitialized(sender, args) {
        //Added these functions to check the number of records in the grid for enabling and disabling 
        //functionality links.
        var gd = $find("ctl00_Tile3_DraftedSellSheets1_gridDraftedSellSheets$GridWrapper");
        if (gd) gd.add_recordCountChanged(gridDraftedSellSheets_onRecordCount);

        var gc = $find("ctl00_Tile3_CompletedSellSheets1_gridCompletedSellSheets$GridWrapper");
        if (gc) gc.add_recordCountChanged(gridCompletedSellSheets_onRecordCount);

        var gc = $find("ctl00_Tile3_SellSheetOrderList1_gridSellSheetOrders$GridWrapper");
        if (gc) gc.add_recordCountChanged(gridSellSheetOrders_onRecordCount);
    }

    function ss_pageUnloaded(sender, args) {
        var gd = $find("ctl00_Tile3_DraftedSellSheets1_gridDraftedSellSheets$GridWrapper");
        if (gd) gd.remove_recordCountChanged(gridDraftedSellSheets_onDataBound);

        gd = $find("ctl00_Tile3_CompletedSellSheets1_gridCompletedSellSheets$GridWrapper");
        if (gd) gd.remove_recordCountChanged(gridCompletedSellSheets_onDataBound);

        gd = $find("ctl00_Tile3_SellSheetOrderList1_gridSellSheetOrders$GridWrapper");
        if (gd) gd.remove_recordCountChanged(gridSellSheetOrders_onRecordCount);

        clientManager.remove_pageInitialized(ss_pageInitialized);
        clientManager.remove_pageUnloaded(ss_pageUnloaded);
    }

    function RefreshOrderList() {
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

    function OpenPreview() {
        var oManager = GetRadWindowManager();
        var grid = getCompletedGrid();

        //Getting the clientkey
        var ClientKey = clientManager.get_ClientKey();

        var Sheet_ID = grid.get_masterTableView().get_selectedItems()[0]._dataItem.Sell_Sheet_ID;

        //Opens popup window to remove the sell sheet.
        var str = "custom/" + ClientKey + "/sellsheets/all/CompletedSellSheetPreview.aspx?Sell_Sheet_ID=" + Sheet_ID;
        //        var oWnd = radopen(str, "PreviewWnd");
        //        oWnd.setSize(960, 650);
        //        oWnd.Center();
        $openWindow(str);
    }

    //Opens modal window for Emailing a Sell Sheet.
    function openEmailSellSheet() {
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
    function openPDFExport() {
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
    function gridDraftedSellSheets_onRecordCount(sender, args) {
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
    function gridCompletedSellSheets_OnRowSelecting(sender, args) {
        $get("NewOrder").style.display = "none";
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
    function gridDraftedSellSheets_OnRowSelecting(sender, args) {
        $get("ctl00_Tile3_RemoveDraft").style.display = "";
        $get("ctl00_Tile3_ResumeDraft").style.display = "";
        $get("separator1").style.display = "";
        $get("separator2").style.display = "";
    }


    function gridCompletedSellSheets_onRecordCount(sender, args) {
        var mt = sender.get_masterTableView();

        //If there are no records in Completed sellsheets grid then disable functionality links 
        //else enable them and select first record.
        if (mt.get_virtualItemCount() == 0) {
            $get("ctl00_Tile3_EditSellSheet").style.display = "none";
            $get("ctl00_Tile3_RemoveSellSheet").style.display = "none";
            $get("ctl00_Tile3_EmailSellSheet").style.display = "none";
            $get("ctl00_Tile3_ExportSellSheet").style.display = "none";
            $get("ctl00_Tile3_OrderPrintsforSellSheet").style.display = "none";
            $get("separator3").style.display = "none";
            $get("separator4").style.display = "none";
            $get("separator5").style.display = "none";
            $get("separator6").style.display = "none";
            $get("separator7").style.display = "none";
            // $get("ctl00_Tile3_Preview").disabled = true;
            //            $get("ctl00_Tile3_CompletedSellSheets1_CreateNewSellSheet").disabled = true;
        }
        else {
            mt.selectItem(0);
            $get("ctl00_Tile3_EditSellSheet").style.display = "";
            $get("ctl00_Tile3_RemoveSellSheet").style.display = "";
            $get("ctl00_Tile3_EmailSellSheet").style.display = "none"; //Email link is disabled.
            $get("ctl00_Tile3_ExportSellSheet").style.display = "";
            $get("ctl00_Tile3_OrderPrintsforSellSheet").style.display = "none"; //OrderPrint link is disabled.
            $get("separator3").style.display = "";
            $get("separator4").style.display = "";
            $get("separator5").style.display = "none"; //Email separator is disabled.
            $get("separator6").style.display = "";
            $get("separator7").style.display = "none"; //OrderPrint separator is disabled.
            //  $get("ctl00_Tile3_Preview").disabled = false;
            //            $get("ctl00_Tile3_CreateNewSellSheet").disabled = false;
        }
    }


    function gridSellSheetOrders_onRecordCount(sender, args) {
        var mt = sender.get_masterTableView();

        //If there are no records in orderlist grid then disable functionality links 
        //else enable them and select first record.
        if (mt.get_virtualItemCount() == 0) {
            // $get("ctl00_Tile3_SellSheetOrderList1_ReOrder").disabled = true;
            $get("ctl00_Tile3_SellSheetOrderList1_ReOrder").style.display = "none";
        }
        else {
            mt.selectItem(0);
            //$get("ctl00_Tile3_SellSheetOrderList1_ReOrder").disabled = false;
            $get("ctl00_Tile3_SellSheetOrderList1_ReOrder").style.display = "";
        }
    }

    //This function is called when "Order Prints" link is clicked from formulary sell sheets - preview panel.
    function OpenOrderDtls() {
        //Show Orderlist and Neworder panels.
        $get("NewOrder").style.display = "";
        $get("PreviousOrders").style.display = "";

        //Set the filter to populate all the records only for selected Sell_Sheet_ID from completed sell sheet list.
        var grid = $find("ctl00_Tile3_CompletedSellSheets1_gridCompletedSellSheets");
        var Sheet_ID = grid.get_masterTableView().get_selectedItems()[0]._dataItem.Sell_Sheet_ID;

        var gdOrders = $find("ctl00_Tile3_SellSheetOrderList1_gridSellSheetOrders").get_masterTableView();
        $setGridFilter(gdOrders, "Sell_Sheet_ID", Sheet_ID, "EqualTo", "System.Int32");
        gdOrders.rebind();

        $("#SelectedSheetID").val(Sheet_ID);
    }


    function PopulateNewOrderSection() {
        var grid = $find("ctl00_Tile3_SellSheetOrderList1_gridSellSheetOrders");
        var Sheet_ID = grid.get_masterTableView().get_selectedItems()[0]._dataItem.Sell_Sheet_ID;
        var Order_ID = grid.get_masterTableView().get_selectedItems()[0]._dataItem.Order_ID;
        var qty = grid.get_masterTableView().get_selectedItems()[0]._dataItem.Sell_Sheet_Copies;
        var locid = grid.get_masterTableView().get_selectedItems()[0]._dataItem.Ship_Location_ID;
        var addr1 = grid.get_masterTableView().get_selectedItems()[0]._dataItem.Ship_Address1;
        var addr2 = grid.get_masterTableView().get_selectedItems()[0]._dataItem.Ship_Address2;
        var city = grid.get_masterTableView().get_selectedItems()[0]._dataItem.Ship_City;
        var state = grid.get_masterTableView().get_selectedItems()[0]._dataItem.Ship_State;
        var zip = grid.get_masterTableView().get_selectedItems()[0]._dataItem.Ship_Zip;
        var phone = grid.get_masterTableView().get_selectedItems()[0]._dataItem.Ship_Phone;
        var email = grid.get_masterTableView().get_selectedItems()[0]._dataItem.Ship_Email;
        var printeremail = grid.get_masterTableView().get_selectedItems()[0]._dataItem.Printer_Email;
        var printerfax = grid.get_masterTableView().get_selectedItems()[0]._dataItem.Printer_Fax;
        var comboloc = $get("ctl00_Tile3_NewSellSheetOrder1_rdcmbShipLocation").control;
        var combostate = $get("ctl00_Tile3_NewSellSheetOrder1_rdcmbState").control;


        $("#SelectedSheetID").val(Sheet_ID);
        $("#SelectedOrderID").val(Order_ID);
        $("#ctl00_Tile3_NewSellSheetOrder1_NoSellSheets").val(qty);

        if (locid != null)
            comboloc.findItemByValue(locid).select();

        if (printeremail != null)
            $("#ctl00_Tile3_NewSellSheetOrder1_PrinterEmail").val(printeremail);

        if (printerfax != null)
            $("#ctl00_Tile3_NewSellSheetOrder1_PrinterFax").val(printerfax);

        if (addr1 != null)
            $("#ctl00_Tile3_NewSellSheetOrder1_Addr1").val(addr1);

        if (addr2 != null)
            $("#ctl00_Tile3_NewSellSheetOrder1_Addr2").val(addr2);

        if (city != null)
            $("#ctl00_Tile3_NewSellSheetOrder1_City").val(city);

        if (state != null)
            combostate.findItemByValue(state).select();

        if (zip != null)
            $("#ctl00_Tile3_NewSellSheetOrder1_Zip").val(zip);

        if (phone != null)
            $("#ctl00_Tile3_NewSellSheetOrder1_Phone").val(phone);

        if (email != null)
            $("#ctl00_Tile3_NewSellSheetOrder1_Email").val(email);
    }

</script>