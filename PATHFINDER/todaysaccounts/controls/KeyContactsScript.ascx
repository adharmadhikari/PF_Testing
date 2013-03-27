<%@ Control Language="C#" ClassName="KeyContactScript" CodeFile="KeyContactsScript.ascx.cs" Inherits="controls_KeyContactsScript" %>
<%@ OutputCache Shared="true" VaryByParam="None" Duration="100" %>



<script type="text/javascript">
    clientManager.add_pageInitialized(contact_pageInitialized);
    clientManager.add_pageUnloaded(contact_pageUnloaded);

    var dodIDs = '<%= DODHeadquarters %>';

    function contact_pageInitialized(sender, args)
    {
        var g = $find("ctl00_Tile3_KeyContactsList1_keyContactsListGrid_gridKeyContacts$GridWrapper");
        if (g) g.add_dataBinding(contact_onDataBinding);

        g = $find("ctl00_Tile4_MyKeyContactsList1_gridMyContacts$GridWrapper");
        if (g)
        {
            g.add_dataBinding(contact_onMyContactsDataBinding);
            if (g.get_drillDownLevel() > 0)
            {
                var data = sender.get_SelectionData();
                delete data["Section_ID"]; //to avoid error in custom segment key contact for reckitt
                sender.set_SelectionData(data, 1);
            }

        } 
    }

    function contact_onDataBinding(sender, args) {
        $clearGridFilter(sender, "Prod_ID");
        $clearGridFilter(sender, "Plan_State");
        $clearGridFilter(sender, "Original_Section");
        $clearGridFilter(sender, "VISN");       

        //DOD shares contacts accross accounts
        if (clientManager.get_EffectiveChannel() == 12)
            $setGridFilter(sender, "Plan_ID", dodIDs, "Custom", "System.Int32");
    }

    function contact_onMyContactsDataBinding(sender, args) {
        $clearGridFilter(sender, "Prod_ID");
        $clearGridFilter(sender, "Plan_State");
        $clearGridFilter(sender, "Original_Section");
        $clearGridFilter(sender, "VISN");            

        //DOD shares contacts accross accounts
        //        if (clientManager.get_EffectiveChannel() == 12)
        //            $setGridFilter(sender, "Plan_ID", dodIDs, "Custom", "System.Int32");
    }

    function contact_pageUnloaded(sender, args) {
        clientManager.remove_pageInitialized(contact_pageInitialized);
        clientManager.remove_pageUnloaded(contact_pageUnloaded);
    }


    function ClearForm() {
        $("#AddKCMain input[type != submit]").val("");
        $("TEXTAREA").val("");
    }

    //Calls OpenKC from dashboard.aspx file to open a pop-up window for viewing key contact details.
    function onKCGridRowClick(sender, args) {
        OpenKC("ViewKC", args._dataKeyValues.KC_ID);
    }

    //Calls OpenMyKC from dashboard.aspx file to open a pop-up window for viewing my key contact details.
    function onMyKCGridRowClick(sender, args) {
        OpenMyKC("ViewKC", args._dataKeyValues.KC_ID);
    }

    //Calls OpenVAKC from dashboard.aspx file to open a pop-up window for viewing key contact details for VA.
    function onVAKCGridRowClick(sender, args) {
        OpenVAKC("ViewKC", args._dataKeyValues.KC_ID, args._dataKeyValues.Parent_Plan_ID);
    }

    //Get the pointer for key contacts grid.
    function getPlanGrid() {
        return $get("ctl00_Tile3_KeyContactsList1_keyContactsListGrid_gridKeyContacts");
    }

    //Opens modal window for Add/Edit My Key Contacts.
    function OpenMyKC(LinkClicked, KCID) {
        var oManager = GetRadWindowManager();
        var grid = clientManager.get_PlanInfoGrid();
        var str = "";

        if (LinkClicked == "DelKC") {
            str = "./todaysaccounts/all/OpenDelMyKCs.aspx?LinkClicked=" + LinkClicked + "&KCID=" + KCID;
        }
        else {
            str = "./todaysaccounts/all/OpenAddEditMyKCs.aspx?LinkClicked=" + LinkClicked + "&KCID=" + KCID;
        }

        var q;

        //        if (clientManager.get_EffectiveChannel() != 12)
        q = clientManager.getSelectionDataForPostback() + "&PlanName=" + encodeURIComponent(grid.get_masterTableView().get_selectedItems()[0]._dataItem.Plan_Name);
        //        else if (dodIDs)
        //            q = "Plan_ID=" + $.trim(dodIDs.split(",")[0]) + "&PlanName=" + clientManager.get_ChannelMenu().get_items().getItem(0).get_text();

        str = str + "&" + q;

        var oWnd = radopen(str, LinkClicked);
        oWnd.setSize(960, 370);
        oWnd.Center();
    }

    //Opens modal window for View/Add/Edit Key Contacts.
    function OpenKC(LinkClicked, KCID) {
        var oManager = GetRadWindowManager();
        var grid = clientManager.get_PlanInfoGrid();

        var str = "./todaysaccounts/all/OpenAddEditKCs.aspx?LinkClicked=" + LinkClicked + "&KCID=" + KCID;
        var q = clientManager.getSelectionDataForPostback();
        str = str + "&" + q + "&PlanName=" + encodeURIComponent(grid.get_masterTableView().get_selectedItems()[0]._dataItem.Plan_Name);

        var oWnd = radopen(str, LinkClicked);
        oWnd.setSize(960, 390);
        oWnd.Center();
    }

    //Opens modal window for View/Add/Edit Key Contacts.
    function OpenVAKC(LinkClicked, KCID, Plan_ID) {
        var oManager = GetRadWindowManager();
        var grid = clientManager.get_PlanInfoGrid();

        var str = "./todaysaccounts/all/OpenAddEditKCs.aspx?LinkClicked=" + LinkClicked + "&KCID=" + KCID;
        //var q = clientManager.getSelectionDataForPostback();
        var q = "PlanID=" + Plan_ID;
        str = str + "&" + q + "&PlanName=" + encodeURIComponent(grid.get_masterTableView().get_selectedItems()[0]._dataItem.Plan_Name);

        var oWnd = radopen(str, LinkClicked);
        oWnd.setSize(960, 370);
        oWnd.Center();
    }

    //for exporting mykeycontacts
    function onExportClicked(type) {

        // var PlanID = '<%= Request.QueryString["Plan_ID"] %>';
        var PlanID = parent.clientManager.get_SelectionData()["Plan_ID"];

        var status = "true";
        var data = { Plan_ID: PlanID, Status: status };
        $exportModule(type, true, clientManager.get_Application(), 0, 'mykeycontacts', data);

    }      
</script>