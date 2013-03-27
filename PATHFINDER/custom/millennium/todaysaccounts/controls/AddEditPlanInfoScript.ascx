<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AddEditPlanInfoScript.ascx.cs" Inherits="custom_controls_AddEditCCRScript" %>
<script type="text/javascript">
    $(document).ready(function()
    {
        //Chrome fix
        if (chrome)
            $('.ccrModalContainer').css('padding', '0px');    
    
        

        //Fix IE drop down arrow issue with RadDropList/ItemTemplate
        if ($.browser.msie)
            new cmd(null, fixDropdowns, null, 500);

        //Fix multiple submit issue by disabling enter key
        $("#AddPlanInfoMain").keypress(function(e)
        {
            if (e.which == 13)
                return false;
        });
        
        //Disable use of up/down arrows to select items in checkbox dropdowns
        $("#ctl00_main_formViewPlanInfo_rdlState_Input, #ctl00_main_formViewPlanInfo_rdlJC_Input, #ctl00_main_formViewPlanInfo_rdlStateCovered_Input").keydown(function(e)
        {
            if (e.which == 13 || e.which == 38 || e.which == 40)
                return false;
        });
        
        //Fix possible editing of radComboBox
        $("#ctl00_main_formViewPlanInfo_rdlState_Input").attr("readOnly", "readonly");
        $("#ctl00_main_formViewPlanInfo_rdlJC_Input").attr("readOnly", "readonly");
        $("#ctl00_main_formViewPlanInfo_rdlStateCovered_Input").attr("readOnly", "readonly");
       
    });

    function fixDropdowns()
    {
        $("#ctl00_main_formViewPlanInfo_rdlState table").width($("#ctl00_main_formViewPlanInfo_rdlState table").width() - 2);
        $("#ctl00_main_formViewPlanInfo_rdlJC table").width($("#ctl00_main_formViewPlanInfo_rdlJC table").width() - 2);
        $("#ctl00_main_formViewPlanInfo_rdlStateCovered table").width($("#ctl00_main_formViewPlanInfo_rdlStateCovered table").width() - 2);
        }
    
    function ClearForm()
    {
        //Reset the form values.
        document.forms[0].reset();

        $("#ctl00_main_hdnStatesCovered").val('');
        $("#ctl00_main_hdnMJ").val('');


        $("#spanJurisditionsCovered").html('');

        $("#spanState").html('');
       

        if ($.browser.msie && $.browser.version < 7)
        {
            $("#aspnetForm input[type=checkbox]").removeAttr("defaultChecked");
            $("#aspnetForm input[type=checkbox]").removeAttr("checked");
            $("#aspnetForm input[type=checkbox]").attr("defaultChecked", false);
            $("#aspnetForm input[type=checkbox]").attr("checked", false);
        }

        $("#AddPlanInfoMain input[type != submit]").val("");
    }

    function UpdStatesCovered() {
        var hdnStatesCovered = $("#ctl00_main_hdnStatesCovered");

        $("#ctl00_main_formViewPlanInfo_rdlStatesCovered input[type=checkbox]").removeAttr("checked");
        var str = hdnStatesCovered.val();
        var t = "<ul>";
        $("#spanState").html("");
        if (str) {
            var a = str.split(",");

            for (var i = 0; i < a.length; i++) {
                if (a[i].length < 2) {
                    $("#ctl00_main_formViewPlanInfo_rdlStateCovered #p" + 0 + a[i] + " input").attr("checked", true);

                    if ($.browser.msie && $.browser.version < 7)
                        $("#ctl00_main_formViewPlanInfo_rdlStateCovered #p" + 0 + a[i] + " input").attr("defaultChecked", "defaultChecked");

                    t = t + "<li id=pp" + 0 + a[i] + ">" + $("#p" + 0 + a[i] + " label").text() + "</li>";
                }
                else {
                    $("#ctl00_main_formViewPlanInfo_rdlStateCovered #p" + a[i] + " input").attr("checked", true);

                    if ($.browser.msie && $.browser.version < 7)
                        $("#ctl00_main_formViewPlanInfo_rdlStateCovered #p" + a[i] + " input").attr("defaultChecked", "defaultChecked");

                    t = t + "<li id=pp" + a[i] + ">" + $("#p" + a[i] + " label").text() + "</li>";
                }
            }

            t = t + "</ul>"
            $("#spanState").append(t);
        }
    }

    function updJurisditionsCovered() {
        var hdnMJ = $("#ctl00_main_hdnMJ");

        $("#ctl00_main_formViewPlanInfo_rdlJC input[type=checkbox]").removeAttr("checked");
        var str = hdnMJ.val();
        var t = "<ul>";

        $("#spanJurisditionsCovered").html("");

        if (str) {
            var a = str.split(",");
            var hasOther = false;
            for (var i = 0; i < a.length; i++) {
                $("#ctl00_main_formViewPlanInfo_rdlJC #m" + a[i] + " input").attr("checked", true);

                if ($.browser.msie && $.browser.version < 7)
                    $("#ctl00_main_formViewPlanInfo_rdlJC #m" + a[i] + " input").attr("defaultChecked", "defaultChecked");

                t = t + "<li id=mm" + a[i] + ">" + $("#m" + a[i] + " label").text() + "</li>";

                if (a[i] == 100) {
                    hasOther = true;
                }
            }

            if (hasOther)
                $("#ctl00_main_formViewPlanInfo_txtMeetingOutcomeOther").attr("disabled", false);
            else
                $("#ctl00_main_formViewPlanInfo_txtMeetingOutcomeOther").attr("disabled", true).val("");

            t = t + "</ul>"
            $("#spanJurisditionsCovered").append(t)
        }
    }


    function JurisditionsCoveredChanged(sender, JCID) {
        var tree_ul = $('#spanJurisditionsCovered ul').children();
        var t = "";
        if (sender.checked == false) {
            if (tree_ul) {
                var b = "m" + JCID;
                $("#m" + b).remove();
            }
        }
        if (sender.checked == true) {
            if ($('#spanJurisditionsCovered > ul').size() > 0) {
                t = t + "<li id=mm" + JCID + ">" + $("#m" + JCID + " label").text() + "</li>";
                $("#spanJurisditionsCovered ul").append(t);
            }
            else {
                t = "<ul>" + "<li id=mm" + JCID + ">" + $("#m" + JCID + " label").text() + "</li></ul>";
                $("#spanJurisditionsCovered").append(t);
            }
        }
        var strPlans = "";
        var a = [];
        var hdnMJ = $("#ctl00_main_hdnMJ");
        var IDs = JCID;

        if (window.event)
            window.event.cancelBubble = true;

        //Get currently selected list of MeetOutID.
        if (hdnMJ.length > 0)
            strPlans = hdnMJ.val();

        //Get list of selected MeetOutID in an array.
        if (strPlans != "")
            a = strPlans.split(",");

        //Remove the current IDs(MeetOutID) from the array.
        a = $.grep(a, function(i) { return i != IDs; }, false);
        if (sender.checked) {
            a[a.length] = IDs;
        }
        strPlans = a.join(",");

        //Update hidden variable.
        hdnMJ.val(strPlans);
    }


    function StatesCoveredChanged(sender, StateID) {
        var tree_ul = $('#spanState ul').children();
        var t = "";

        if (sender.checked == false) {
            if (tree_ul) {
                if (StateID < 10) {
                    var b = "p" + 0 + StateID;
                    $("#p" + b).remove();
                }
                else {
                    var b = "p" + StateID;
                    $("#p" + b).remove();
                }
            }
        }

        if (sender.checked == true) {
            if ($('#spanState > ul').size() > 0) {

                if (StateID < 10)
                    t = t + "<li id=pp" + 0 + StateID + ">" + $("#p" + 0 + StateID + " label").text() + "</li>";
                else

                    t = t + "<li id=pp" + StateID + ">" + $("#p" + StateID + " label").text() + "</li>";
                $("#spanState ul").append(t);
            }
            else {
                if (StateID < 10)
                    t = "<ul>" + "<li id=pp" + 0 + StateID + ">" + $("#p" + 0 + StateID + " label").text() + "</li></ul>";
                else

                    t = "<ul>" + "<li id=pp" + StateID + ">" + $("#p" + StateID + " label").text() + "</li></ul>";
                $("#spanState").append(t);
            }
        }
        var strPlans = "";
        var a = [];
        var hdnStatesCovered = $("#ctl00_main_hdnStatesCovered");
        var IDs = StateID;

        if (window.event)
            window.event.cancelBubble = true;

        //Get currently selected list of MeetOutID.
        if (hdnStatesCovered.length > 0)
            strPlans = hdnStatesCovered.val();

        //Get list of selected MeetOutID in an array.
        if (strPlans != "")
            a = strPlans.split(",");

        //Remove the current IDs(MeetOutID) from the array.
        a = $.grep(a, function(i) { return i != IDs; }, false);
        if (sender.checked) {
            a[a.length] = IDs;
        }
        strPlans = a.join(",");

        //Update hidden variable.
        hdnStatesCovered.val(strPlans);

    }
    function setComboText(sender, args) {
        //sender.set_text(" ");
    }
    function setStatesCoveredText(sender, args) {
        var hdnStatesCovered = $("#ctl00_main_hdnStatesCovered");
        var strPlans = "";

        //Get currently selected list of ProductsDiscussedIDs.
        if (hdnStatesCovered.length > 0)
            strPlans = hdnStatesCovered.val();

        //If list is not empty then update product dropdown message to 'Change Selection' else '-Select Products Discussed-'.
        if (strPlans != "")
            sender.set_text("-Change Selection-");
        else
            sender.set_text("-Select State Covered-");
    }

    function setJCText(sender, args) {
        var hdnMJ = $("#ctl00_main_hdnMJ");
        var strPlans = "";

        //Get currently selected list of personsmetIDs.
        if (hdnMJ.length > 0)
            strPlans = hdnMJ.val();

        //If list is not empty then update personsmet dropdown message to 'Change Selection' else '-Select Persons Met-'.
        if (strPlans != "")
            sender.set_text("-Change Selection-");
        else
            sender.set_text("-Select Jurisditions Covered-");
    }

    function OpenMJPlanInfo(LinkClicked, PlanID) {
        var oManager = GetRadWindowManager();
        var grid = clientManager.get_PlanInfoGrid();
        var str = "";

//        if (LinkClicked == "DelKC") {
//            str = "./custom/millennium/todaysaccounts/all/OpenDelMyKCs.aspx?LinkClicked=" + LinkClicked + "&KCID=" + KCID;
//        }
//        else {
        str = "./custom/millennium/todaysaccounts/all/AddEditPlanInfoMJ.aspx?LinkClicked=" + LinkClicked ;
      //  }

        var q;


        q = clientManager.getSelectionDataForPostback() + "&PlanName=" + encodeURIComponent(grid.get_masterTableView().get_selectedItems()[0]._dataItem.Plan_Name) + "&PlanID=" + (grid.get_masterTableView().get_selectedItems()[0]._dataItem.Plan_ID);
       
        str = str + "&" + q;

        var oWnd = radopen(str, LinkClicked);
        oWnd.setSize(960, 370);
        oWnd.Center();
    }
    function OpenSSPlanInfo(LinkClicked, PlanID) {
        var oManager = GetRadWindowManager();
        var grid = clientManager.get_PlanInfoGrid();
        var str = "";

        //        if (LinkClicked == "DelKC") {
        //            str = "./custom/millennium/todaysaccounts/all/OpenDelMyKCs.aspx?LinkClicked=" + LinkClicked + "&KCID=" + KCID;
        //        }
        //        else {
        str = "./custom/millennium/todaysaccounts/all/AddEditPlanInfoSS.aspx?LinkClicked=" + LinkClicked;
        //  }

        var q;


        q = clientManager.getSelectionDataForPostback() + "&PlanName=" + encodeURIComponent(grid.get_masterTableView().get_selectedItems()[0]._dataItem.Plan_Name) + "&PlanID=" + (grid.get_masterTableView().get_selectedItems()[0]._dataItem.Plan_ID);

        str = str + "&" + q;

        var oWnd = radopen(str, LinkClicked);
        oWnd.setSize(960, 370);
        oWnd.Center();
    }
    function OpenVARPlanInfo(LinkClicked, PlanID) {
        var oManager = GetRadWindowManager();
        var grid = clientManager.get_PlanInfoGrid();
        var str = "";

        //        if (LinkClicked == "DelKC") {
        //            str = "./custom/millennium/todaysaccounts/all/OpenDelMyKCs.aspx?LinkClicked=" + LinkClicked + "&KCID=" + KCID;
        //        }
        //        else {
        str = "./custom/millennium/todaysaccounts/all/AddEditPlanInfoVAR.aspx?LinkClicked=" + LinkClicked;
        //  }

        var q;


        q = clientManager.getSelectionDataForPostback() + "&PlanName=" + encodeURIComponent(grid.get_masterTableView().get_selectedItems()[0]._dataItem.Plan_Name) + "&PlanID=" + (grid.get_masterTableView().get_selectedItems()[0]._dataItem.Plan_ID);

        str = str + "&" + q;

        var oWnd = radopen(str, LinkClicked);
        oWnd.setSize(960, 370);
        oWnd.Center();
    }
    function OpenRTAPlanInfo(LinkClicked, PlanID) {
        var oManager = GetRadWindowManager();
        var grid = clientManager.get_PlanInfoGrid();
        var str = "";

        //        if (LinkClicked == "DelKC") {
        //            str = "./custom/millennium/todaysaccounts/all/OpenDelMyKCs.aspx?LinkClicked=" + LinkClicked + "&KCID=" + KCID;
        //        }
        //        else {
        str = "./custom/millennium/todaysaccounts/all/AddEditPlanInfoRTA.aspx?LinkClicked=" + LinkClicked;
        //  }

        var q;


        q = clientManager.getSelectionDataForPostback() + "&PlanName=" + encodeURIComponent(grid.get_masterTableView().get_selectedItems()[0]._dataItem.Plan_Name) + "&PlanID=" + (grid.get_masterTableView().get_selectedItems()[0]._dataItem.Plan_ID);

        str = str + "&" + q;

        var oWnd = radopen(str, LinkClicked);
        oWnd.setSize(960, 370);
        oWnd.Center();
    }
    function RefreshPlanInfo() {
        window.top.clientManager.set_SelectionData(window.top.clientManager.get_SelectionData());
        window.setTimeout(CloseWin, 2000);
        return window.top.$find("ctl00_main_planInfo_gridPlanInfo").get_masterTableView().rebind();
    }
    function ConfirmMsg() {
        window.setTimeout(CloseWin, 2000);
    }

    function CloseWin() {
        var manager = window.top.GetRadWindowManager();

        var window1 = manager.getWindowByName("AddPlan");
        if (window1 != null)
            window1.close();

        var window2 = manager.getWindowByName("EditPlan");
        if (window2 != null)
            window2.close();
    }
</script>
