<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ReviewPlanSelectionScript.ascx.cs" Inherits="custom_controls_ReviewPlanSelectionScript" %>
<%@ OutputCache Shared="true" VaryByParam="None" Duration="100" %>

<script type="text/javascript">
    clientManager.add_pageInitialized(ps_pageInitialized);
    clientManager.add_pageUnloaded(ps_pageUnloaded);

    function ps_pageInitialized(sender, args)
    {
        ReviewPlansListViewInit();
    }

    function ReviewPlansListViewInit()
    {
//        var hdnPlanCopay = $("#ctl00_ctl00_Tile3_StepBody_formSSPlans_hdnCopay");

//        if (hdnPlanCopay.length > 0)
//        {
//            if (hdnPlanCopay.val() == "True")
//            {
//                $("#ctl00_ctl00_Tile3_StepBody_ReviewPlansListView_ctrl0_Header2")[0].colSpan = "2";
//                $("#ctl00_ctl00_Tile3_StepBody_ReviewPlansListView_ctrl0_Header3")[0].colSpan = "2";
//                $("#ctl00_ctl00_Tile3_StepBody_ReviewPlansListView_ctrl0_Header4")[0].colSpan = "2";
//            }
//            else
//            {
//                $("#ctl00_ctl00_Tile3_StepBody_ReviewPlansListView_ctrl0_Header2")[0].colSpan = "1";
//                $("#ctl00_ctl00_Tile3_StepBody_ReviewPlansListView_ctrl0_Header3")[0].colSpan = "1";
//                $("#ctl00_ctl00_Tile3_StepBody_ReviewPlansListView_ctrl0_Header4")[0].colSpan = "1";
//            }
//        }
//        else
//        {
//            $("#ctl00_ctl00_Tile3_StepBody_ReviewPlansListView_ctrl0_Header2")[0].colSpan = "2";
//            $("#ctl00_ctl00_Tile3_StepBody_ReviewPlansListView_ctrl0_Header3")[0].colSpan = "2";
//            $("#ctl00_ctl00_Tile3_StepBody_ReviewPlansListView_ctrl0_Header4")[0].colSpan = "2";
//        }
    }   
    
    
    function ps_pageUnloaded(sender, args) {
        clientManager.remove_pageInitialized(ps_pageInitialized);
        clientManager.remove_pageUnloaded(ps_pageUnloaded);
    }

    function PlansHighlightChanged(sender, PlanID, FormularyID, ProductID)
    {
        var strPlans = "";
        var a = [];
        var hdnPlanHighlighted = $("#ctl00_ctl00_Tile3_StepBody_hdnPlansHighlighted");
        var IDs = PlanID + "|" + FormularyID + "|" + ProductID;

        //Get currently highlighted list of PlanIDs.
        if (hdnPlanHighlighted.length > 0)
            strPlans = hdnPlanHighlighted.val();

        //Get list of highlighted PlanIDs in an array.
        if (strPlans != "")
            a = strPlans.split(",");

        //Remove current IDs(PlanID|FormularyID|ProductID) from the array.
        a = $.grep(a, function(i) { return i != IDs; }, false);

        //Get the parentnode of selected checkbox.
        var p = sender.parentNode;
        while (p && p.tagName != "TR")
        {
            p = p.parentNode;
        }

        //If checkbox is checked and then add the selected IDs(PlanID|FormularyID|ProductID) to the list
        if (sender.checked)
        {
            a[a.length] = IDs;

            //Highlight the row on selection.
            if (p)
            {
                $(p).removeClass("UnHighlighter");
                $(p).addClass("Highlighter"); 
            }
        }
        else
        {
            //Remove the highlighting when the row is not selected.
            if (p)
            {
                $(p).removeClass("Highlighter");
                $(p).addClass("UnHighlighter"); 
            }
        }

        strPlans = a.join(",");

        //Update hidden variable.
        hdnPlanHighlighted.val(strPlans);
    }

//    function PlanRank(sender, PlanID, FormularyID, ProductID) {
//        var test = sender.val();
//        alert(test);
//    }

    //Opens modal window for Adding an account in formulary sell sheet.
    function OpenSaveWindow()
    {
        var oManager = GetRadWindowManager();

        var clientKey = clientManager.get_ClientKey();

        var q = clientManager.getSelectionDataForPostback();
        str = "custom/" + clientKey + "/sellsheets/all/SaveSellSheet.aspx?" + q;

        var oWnd = radopen(str, "SaveSellSheet");
        oWnd.setSize(400, 200);
        oWnd.Center();
    }

    function validatePlanRank() {
        var realvalues = [];
        var i = 0;
        $('#reviewPlansContainer input:text').each(function() {
            if ($(this)[0].value != "") {
                realvalues[i] = $(this)[0].value;
                i++;
            }
        });
        var count = $('#reviewPlansContainer input:text').size();

        if (realvalues.length > 0) {
            realvalues.sort();
            var min = realvalues[0];
            realvalues.reverse();
            var max = realvalues[0];
            if (min != 1)
                $("#ctl00_ctl00_Tile3_StepBody_hdnRankValidate").val("");
            else
                $("#ctl00_ctl00_Tile3_StepBody_hdnRankValidate").val("validate");

            if (!(max <= realvalues.length))
                $("#ctl00_ctl00_Tile3_StepBody_hdnRankValidate").val("");
            else
                $("#ctl00_ctl00_Tile3_StepBody_hdnRankValidate").val("validate");

        }
    }


</script>

