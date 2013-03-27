<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PlanSelectionScript.ascx.cs" Inherits="custom_controls_PlanSelectionScript" %>
<%@ OutputCache Shared="true" VaryByParam="None" Duration="100" %>


<script type="text/javascript">
    clientManager.add_pageInitialized(ps_pageInitialized);
    clientManager.add_pageUnloaded(ps_pageUnloaded);
    clientManager.add_pageLoaded(ps_pageLoaded);
    var medicaidSelected = false;
    
    function ps_pageInitialized(sender, args)
    {
        UpdChkSelection();

        var grid = $get("ctl00_ctl00_Tile3_StepBody_gridPlanSelectionList");
        buildHeaderRow(grid, _HeaderDetails, true);
    }

    function ps_pageLoaded(sender, args)
    {
        //var container = $("#divTile3");

        //if (container.clientHeight < container.scrollHeight)
        //    alert("there is a scroll bar");
        //else
        //    alert("there is no scroll");
        var tableData = $("#planSelectContainer .rgDataDiv").width();
        var header = $("#planSelectContainer .rgHeaderDiv").width();
        
        //Fix for header width
        if (tableData == header)
            $("#planSelectContainer .rgHeaderDiv").width($("#planSelectContainer .rgHeaderDiv").width() - 19);

        $("#planSelectContainer .dashboardTable").addClass('planSelectTable');  
    }

    function PlanSelectionChanged(sender, PlanID, FormularyID, ProductID, SegmentID)
    {
        if (SegmentID == 3 && sender.checked)
            medicaidSelected = true;
        if (SegmentID == 3 && sender.checked == false)
            medicaidSelected = false;
        
        var strPlans = "";
        var a = [];
        var hdnPlanLst = $("#ctl00_ctl00_Tile3_StepBody_hdnPlansSelected");
        var IDs = PlanID + "|" + FormularyID + "|" + ProductID;

        if (window.event)
            window.event.cancelBubble = true;

        //Get currently selected list of PlanIDs.
        if (hdnPlanLst.length > 0)
            strPlans = hdnPlanLst.val();

        //Get list of selected PlanIDs in an array.
        if (strPlans != "")
            a = strPlans.split(",");

        //Remove the current IDs(PlanID|FormularyID|ProductID) from the array.
        a = $.grep(a, function(i) { return i != IDs; }, false);

        //If checkbox is checked and the array length is less than 10 then add the selected PlanID to the list
        //Please note that the current plan selection allows maximum of 10 selected plans.
        if (SegmentID == 1)
        {
            if (sender.checked)
            {
                if (a.length < 10)
                {
                    a[a.length] = IDs;
                }
                else
                {
                    sender.checked = false;
                    $alert("A maximum of 10 Commercial plans can be selected.", "Plan Selection");
                }
            }
            else
            {
                if (a.length == 0)
                {
                    //                sender.checked = true;
                    //                $alert("At least one plan should be selected.", "Plan Selection");
                    //                a[a.length] = IDs; //Keep the current IDs as is in array since only one plan is selected.
                }
            }
        }
        else
        {
            if (sender.checked)
            {
                var len = 5;
                if (medicaidSelected)
                    len = 6;

                if (a.length < len)
                {
                    a[a.length] = IDs;
                }
                else
                {
                    sender.checked = false;
                    $alert("A maximum of 5 Medicare Part-D plans and 1 State Medicaid plan can be selected.", "Plan Selection");
                }
            }
            else
            {
                if (a.length == 0)
                {
                    //                sender.checked = true;
                    //                $alert("At least one plan should be selected.", "Plan Selection");
                    //                a[a.length] = IDs; //Keep the current IDs as is in array since only one plan is selected.
                }
            }
        }

        strPlans = a.join(",");

        //Update hidden variable.
        hdnPlanLst.val(strPlans);
        //Storing the selected checkbox values.
        clientManager.setContextValue("ssSelectedPlansList", hdnPlanLst.val());

    }

    function UpdChkSelection()
    {
        var hdnPlanLst = $("#ctl00_ctl00_Tile3_StepBody_hdnPlansSelected");
        //Retrieve the selected checkbox values stored earlier.
        var strList = clientManager.getContextValue("ssSelectedPlansList");

        //Update hidden variable.
        if (strList)
        {
            //Remove all the checkmarks first then check the selected records one by one.
            $("#ctl00_ctl00_Tile3_StepBody_gridPlanSelectionList input[type=checkbox]").removeAttr("checked");

            // hidden variable contains comma separated list of selected rows containing
            // Plan_ID|FormularyID|Product_ID combination.
            hdnPlanLst.val(strList);

            //Select checkboxes based on the stored ids.
            //Split on comma first.
            var a = strList.split(",");
            for (var i = 0; i < a.length; i++)
            {
                //Replace pipe(|) with unduscore(_) character.
                //eg. "501|1|50501" will be replaced with "501_1_50501"
                var str = a[i].replace(/\|/g, "_");
                //If corresponding id(Plan_ID_FormularyID_Product_ID) combination is found 
                //then set checked = true.
                $("#" + str + " input").attr("checked", true);
            }
        }
    }

    function ps_pageUnloaded(sender, args)
    {
        //clientManager.setContextValue("ssSelectedPlansList");
        clientManager.remove_pageInitialized(ps_pageInitialized);
        clientManager.remove_pageLoaded(ps_pageLoaded);
        clientManager.remove_pageUnloaded(ps_pageUnloaded);
    }

    
    //Opens modal window for Adding an account in formulary sell sheet.
    function OpenAddAccount()
    {
        var clientKey = clientManager.get_ClientKey();

        var oManager = GetRadWindowManager();
        var hdnPlanLst = $("#ctl00_ctl00_Tile3_StepBody_hdnPlansSelected");
        //Storing the selected checkbox values.
        clientManager.setContextValue("ssSelectedPlansList", hdnPlanLst.val());

        var q = clientManager.getSelectionDataForPostback();
        str = "custom/" + clientKey + "/sellsheets/all/AddAccount.aspx?" + q;

        var oWnd = radopen(str, "AddAcc");
        oWnd.setSize(400, 170);
        oWnd.Center();
    }
</script>
