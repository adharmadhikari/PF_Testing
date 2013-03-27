<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CCReportScripts.ascx.cs" Inherits="custom_controls_CCReportScript" %>
<%@ OutputCache Shared="true" VaryByParam="None" Duration="100" %> 


 <script type="text/javascript">

    var _planSectionGridTimeoutHandle;
    var _lastSearchValue = {};

    var uiReady = false; //flag to make sure search is ready
    
    var planMessage = '<%= Resources.Resource.Message_CCR_Select_Plan %>';
    var docMessage = '<%= Resources.Resource.Message_CCR_Delete_Document %>';
    var title = "Customer Contact Reports";
    
    clientManager.add_pageInitialized(pageInitialized);
    clientManager.add_pageUnloaded(pageUnloaded);

    function get_PlanSectionSelectGrid() 
    {
        ///<summary>Gets the Plan Information Grid control located on the Customer Contact Report entry.</summary>
        return $find("ctl00_ctl00_Tile3_Tile8_CCPlan1_gridPlans");        
    }
    
    function get_CustomerContactGrid()
    {
        ///<summary>Gets the Customer Contact Grid control located on the Customer Contact Report entry.</summary>        
        return $find("ctl00_ctl00_Tile3_Tile6_CCRGridList1_gridCCReports");            
    }
    
//    function get_BusinessPlansGrid() 
//    {
//        ///<summary>Gets the Business Plans Grid control located on the Customer Contact Report entry.</summary>
//        return $find("ctl00_ctl00_Tile3_Tile7_BusinessDocument1_gridCCDocuments");
//    }

    function pageInitialized() 
    {
        clientManager.registerComponent('ctl00_ctl00_Tile3_Tile8_CCPlan1_gridPlans_ctl00_ctl02_ctl02_rdlSection', null);
        clientManager.registerComponent('ctl00_ctl00_Tile3_Tile8_CCPlan1_gridPlans_ctl00_ctl02_ctl02_rdlStates', null);

//        var gm = $find("ctl00_ctl00_Tile3_Tile7_BusinessDocument1_gridCCDocuments$GridWrapper");
//        if (gm)
//            gm.add_recordCountChanged(gridCCDocuments_onRecordCount);
//        $(".ccrPlanSelectView .tools .reqsel").hide();

        //Get channels assigned to CCRs
        var src = clientManager.get_ChannelMenuOptions()[clientManager.get_Application()];
        clientManager.get_ChannelMenu().set_visible(false);
        var items = {};
        //need to exclude "All" option - using blank default option instead; exclude combined option also
        $.each(src, function(k, o) { if (src[k]["ID"] != 0 && src[k]["ID"] != 99) items[k] = o; });
        $loadListItems($find('ctl00_ctl00_Tile3_Tile8_CCPlan1_gridPlans_ctl00_ctl02_ctl02_rdlSection'), items, { value: null, text: "" });

        items = clientManager.get_States();
        $loadListItems($find('ctl00_ctl00_Tile3_Tile8_CCPlan1_gridPlans_ctl00_ctl02_ctl02_rdlStates'), items, { value: "", text: "" });
        
        var gridPlanSection = get_PlanSectionSelectGrid();
        var mt = gridPlanSection.get_masterTableView();
        var name = 'AE_UserID';
       
        var value = $('.userid').text();

        var dataType = 'System.int32';
        if (value != null && value != "")
            $setGridFilter(gridPlanSection, name, value, null, dataType);
        else if (name)
            $clearGridFilter(mt, name);

        //Adding Territory_ID check---------------------------------------------
        //pull all the records with Territory_ID not equal to "".
        name = 'Territory_ID';
        filterType = 'NotEqualTo';
        value = "";
        dataType = 'System.String';

        if (value != null)
            $setGridFilter(gridPlanSection, name, value, filterType, dataType);
        else if (name)
            $clearGridFilter(mt, name);
        //-------------------------------------------------------------------------    
        
        mt.clearSelectedItems();

        mt.rebind();
    var filterRow = mt.get_tableFilterRow();

        $(filterRow).show();

        uiReady = true;
    }

    function pageUnloaded() 
    {
        clientManager.remove_pageInitialized(pageInitialized);
        clientManager.remove_pageUnloaded(pageUnloaded);
        
//        var gm = $find("ctl00_ctl00_Tile3_Tile7_BusinessDocument1_gridCCDocuments$GridWrapper");
//        if (gm)
//            gm.remove_recordCountChanged(gridCCDocuments_onRecordCount);
    }

//    function gridCCDocuments_onRecordCount(sender, args) {
//        var mt = sender.get_masterTableView();

//        //hide the delete link and view link if no records present for Business Doc grid
//        if (mt.get_virtualItemCount() == 0) {
//            //hide view and delete link.
//            $(".ccrBusinessPlans .tools .reqsel").hide();
//        }
//        else {
//            //select the first record in the grid and show the view and delete link
//            mt.selectItem(0);
//            $(".ccrBusinessPlans .tools .reqsel").show();
//        }
//    }
    
    function getSelectedPlanID()
    {
        var planid;
        var items = get_PlanSectionSelectGrid().get_masterTableView().get_selectedItems();
        if(items && items.length > 0)
            planid = items[0].get_dataItem()["Plan_ID"]; 

        return planid;
    }

    function getSelectedPlanName()
    {
        var planname;
        var items = get_PlanSectionSelectGrid().get_masterTableView().get_selectedItems();
        if (items && items.length > 0)
            planname = items[0].get_dataItem()["Plan_Name"];

        return planname;
    }

//    function getSelectedDocID()
//    {
//        var docID;
//        var items = get_BusinessPlansGrid().get_masterTableView().get_selectedItems();
//        if(items && items.length > 0)
//            docID = items[0].get_dataItem()["Document_ID"]; 

//        return docID;    
//    }
    
    function OpenCCR(LinkClicked, CCRID, PPID) 
    {
        var planID = getSelectedPlanID();
        var planName = getSelectedPlanName();
                       
        var url = null;
        if (CCRID)
        {
            url = clientManager.get_ApplicationManager().get_UrlName() + "/all/AddEditCCReports.aspx?LinkClicked=" + LinkClicked + "&CRID=" + CCRID + "&PlanID=" + PPID + "&PlanName=" + encodeURIComponent(planName);                
        }
        else 
        {
            if (planID)
                url = clientManager.get_ApplicationManager().get_UrlName() + "/all/AddEditCCReports.aspx?LinkClicked=" + "AddCCR" + "&PlanID=" + planID + "&PlanName=" + encodeURIComponent(planName);
            else
                $alert(planMessage, title, null, 50);                
        }

        if (url)             
            $openWindow(url, null, null, 750, 450, LinkClicked);
    }
    
    function onFilterPlansSectionByState(sender, args)
    {
        if(uiReady) new cmd(null, filterPlansSection, ["Plan_State", args.get_item().get_value()], 150);
    }
    
    function onFilterPlansSectionBySection(sender, args)
    {
        var val = args.get_item().get_value();
        if (val == 0) val = null;

        if(uiReady) new cmd(null, filterPlansSection, ["Section_ID", val, null, "System.Int32"], 150);
    }
    
    function onFilterPlansSectionByPlanType(sender, args) 
    {
        if(uiReady) new cmd(null, filterPlansSection, ["Plan_Type_ID", args.get_item().get_value(), null, "System.Int32"], 150);
    }
    
//    function OpenDeleteDoc() 
//    {
//         var grid = get_BusinessPlansGrid();
//         var mt = grid.get_masterTableView();
//         
//         if (grid.get_masterTableView().get_selectedItems()[0] != null) 
//         {
//             DeleteDoc("RemoveWnd", grid.get_masterTableView().get_selectedItems()[0]._dataItem.Document_ID);
//         }
//         else 
//         {
//             $alert(docMessage, title, null, 50); 
//         }             
//     }
//     
//     function DeleteDoc(LinkClicked, DOCID) 
//    {
//        if (DOCID != "") 
//            $openWindow(clientManager.get_ApplicationManager().get_UrlName() + "/all/RemoveDocument.aspx?LinkClicked=" + LinkClicked + "&DocumentID=" + DOCID, null, null, 500, 110, LinkClicked);
//    }

//    function gridCCDocuments_OnRowSelecting(sender, args)
//    {
//        $(".ccrBusinessPlans .tools .reqsel").show();
//    }

//    function viewDocument()
//    {
//        var docID = getSelectedDocID();
//        if (docID) 
//            window.open(clientManager.get_BasePath() + "/usercontent/OpenBusinessDocument.ashx?&Document_ID=" + docID);
//        else 
//            $alert(docMessage, title, null, 50);
//    }
//    
    function gridCCReports_OnRowSelecting(sender, args) 
    {
        OpenCCR("EditCCR", args._dataKeyValues.Contact_Report_ID, args._dataKeyValues.Plan_ID);
    }

//    function OpenDocUpload(LinkClicked) 
//    {
//        var planid = getSelectedPlanID();
//        if (planid) 
//            $openWindow(clientManager.get_ApplicationManager().get_UrlName() + "/all/DocumentUpload.aspx?Plan_ID=" + planid, null, null, 650, 250, LinkClicked);
//        else 
//            $alert(planMessage, title, null, 50);
//    }

    function gridPlans_OnRowSelected(sender, args)
    {
        var autoScroll = false;
        //resize page
        if(!$(sender.get_element()).hasClass("mini"))
        {
            $(sender.get_element()).addClass("mini");

            clientManager.get_ApplicationManager().resizeSection();

            autoScroll = true;
        }

        $(".ccrPlanSelectView .tools .reqsel").show();
       // $(".ccrBusinessPlans .tools .reqsel").hide();

        //load data
        var planid = args._dataKeyValues.Plan_ID;
        var dataItem = args.get_gridDataItem().get_dataItem();
        
        var gridContact = get_CustomerContactGrid();
       // var gridBusiness = get_BusinessPlansGrid();
        
        if (gridContact) 
        {
            var mt = gridContact.get_masterTableView();

            if (planid)
                $setGridFilter(gridContact, "Plan_ID", planid, "EqualTo", "System.Int32");
            else
                $clearGridFilter(mt, "Plan_ID");

            mt.clearSelectedItems();

            mt.rebind();
        }

//        if (gridBusiness) 
//        {
//            var mt = gridBusiness.get_masterTableView();

//            if (planid)
//                $setGridFilter(gridBusiness, "Plan_ID", planid, "EqualTo", "System.Int32");
//            else
//                $clearGridFilter(mt, "Plan_ID");

//            mt.clearSelectedItems();

//            mt.rebind();
//        }
        
        //make sure selected row is still visible
        if(autoScroll) sender.get_element().control.GridWrapper.scrollToSelection();     
    }


     function setPlanSectionGridTimeout(sender, args) 
    {
        ///<summary>Event handler for setting a timeout to delay searching for items in the Plan Section Grid.  The purpose of the delay is to avoid excessive requests based on rapid user input such as key presses in a text box.  By delaying the request it is possible for the user to completely type their search phrase before the search starts while still giving the appearance that results are returned as their typed text changes.  This event handler should only be attached to input controls that filter the Plan Information Grid and require delayed filtering functionality.</summary>
        if (_planSectionGridTimeoutHandle)
            _planSectionGridTimeoutHandle.cancel();

        _planSectionGridTimeoutHandle = new cmd(null, planSectionGridTimeout, [sender, args], 500);
    }

    function planSectionGridTimeout(args)
    {
        var sender = args[0];
        args = args[1];
        
        ///<summary>Internal callback for handling delayed searches (see setPlanSectionGridTimeout).  This method should not be called directly.</summary>
        _planSectionGridTimeoutHandle = null; //0;

        if (sender) {
            //don't filter for same thing twice.
            if (sender.value != _lastSearchValue[args.dataField])
                filterPlansSection([args.dataField, sender.value, args.filterType]);

            _lastSearchValue[args.dataField] = sender.value;
        }
    }

    function clearOtherGrids()
    {
        var gridContact = get_CustomerContactGrid();
        //var gridBusiness = get_BusinessPlansGrid();
        var mtContact = gridContact.get_masterTableView();
        //var mtBusiness = gridBusiness.get_masterTableView();
        
        mtContact.get_filterExpressions().clear();
        //mtBusiness.get_filterExpressions().clear();
        
        gridContact.get_element().control.GridWrapper.clearGrid();
        //gridBusiness.get_element().control.GridWrapper.clearGrid();    
    }
        
    function resetSectionPlans() 
    {
        $(".mini").removeClass("mini");

        $(".ccrPlanSelectView .tools .reqsel").hide();
       // $(".ccrBusinessPlans .tools .reqsel").hide();
        
        clientManager.get_ApplicationManager().resizeSection();

        var grid = get_PlanSectionSelectGrid();

        var mt = grid.get_masterTableView();

        var fcount = $clearGridFilterSelections(mt);

        //need to reset filters
        mt.get_filterExpressions().clear();
        
        //Added to reset the plans list with user aligned plans.
        var name = 'AE_UserID';
        var value = $('.userid').text();
        var dataType = 'System.int32';
        if (value != null && value != "")
            $setGridFilter(grid, name, value, null, dataType);
        else if (name)
            $clearGridFilter(mt, name);
            
        //Adding Territory_ID check---------------------------------------------
        //pull all the records with Territory_ID not equal to "".
        name = 'Territory_ID';
        filterType = 'NotEqualTo';
        var value1 = "";
        dataType = 'System.String';

        if (value != null)
            $setGridFilter(grid, name, value1, filterType, dataType);
        else if (name)
            $clearGridFilter(mt, name);
        //-------------------------------------------------------------------------    
        
        _lastSearchValue = {};

        if (fcount > 0)
            filterPlansSection([]);
        else
            mt.clearSelectedItems();

        mt.rebind();
        
        //resets scroll to top since nothing is selected
        grid.get_element().control.GridWrapper.scrollToSelection();
    }

    function filterPlansSection(args) {
        var name = args[0];
        var value = args[1];
        var filterType = args[2];
        var dataType = args[3];

        var grid = get_PlanSectionSelectGrid();

        if (grid)
        {
            clearOtherGrids();
            
            var mt = grid.get_masterTableView();
      
            if (value != null && value != "")
                $setGridFilter(grid, name, value, filterType, dataType);
            else if (name)
                $clearGridFilter(mt, name);

            mt.clearSelectedItems();

            mt.rebind();
        }
    }
    function MorePlans() {

        //pull all the records with AE_UserID equal to 0 (which will return all the plans irrespective of plan alignments).
        var name = 'AE_UserID';
        var value = "0";
        var dataType = 'System.int32';

        var grid = get_PlanSectionSelectGrid();

        if (grid) {
            clearOtherGrids();

            var mt = grid.get_masterTableView();

            if (value != null)
                $setGridFilter(grid, name, value, null, dataType);
            else if (name)
                $clearGridFilter(mt, name);

            //Adding Territory_ID check---------------------------------------------
            //pull all the records with Territory_ID equal to "".
            name = 'Territory_ID';
            var value1 = "";
            dataType = 'System.String';

            if (value != null)
                $setGridFilter(grid, name, value1, null, dataType);
            else if (name)
                $clearGridFilter(mt, name);
            //-------------------------------------------------------------------------    
        
            mt.clearSelectedItems();

            mt.rebind();
           
        }
    }
</script>