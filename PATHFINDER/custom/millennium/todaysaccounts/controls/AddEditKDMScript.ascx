<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AddEditKDMScript.ascx.cs" Inherits="custom_millennium_todaysaccounts_controls_AddEditKDMScript" %>
<script type="text/javascript">
    var Public_KDM_ID;
    var Public_KDM_ADD_ID;
    $(document).ready(function() {
        //Chrome fix
        if (chrome) {
            $('.ccrModalContainer').css('padding', '10px');
            $('.ccrModalContainer').css('padding-bottom', '100px');
        }

        hideButtons();
        Public_KDM_ID = '';
        //clientManager.set_SelectionData({ KDM_ID: '' }, 1);


        //Fix possible editing of radComboBox
        $("#ctl00_main_formKDMView_rdlCAC_CMD_Input").attr("readOnly", "readonly");
        $("#ctl00_main_formKDMView_rdlCustomTitle_Input").attr("readOnly", "readonly");
        $("#ctl00_main_formKDMView_rdlCredentials_Input").attr("readOnly", "readonly");
        $("#ctl00_main_formKDMView_rdlCustomCAC_Input").attr("readOnly", "readonly");
        $("#ctl00_main_formKDMView_rdlCustomSpecialty_Input").attr("readOnly", "readonly");
        $("#ctl00_main_formKDMView_rdlCustomJobFunction_Input").attr("readOnly", "readonly");

        //disabling enter key on page, so form will not submit when enter key is hit.
        $(document).keypress(function(e)
        {
            if (e.which == 13)
            {
                return false;
            }
        });

    });
    function hideButtons() {
        Public_KDM_ID = '';
        $('#EditDeleteButtons').hide();
        $('#EditDeleteAddressButtons').hide();
        $('#AddAddressButton').hide();
        RefreshKDMAddress();
        var gridKDM = get_KDMGrid();
        if (gridKDM) {
            var masterTable = gridKDM.get_masterTableView();
            var row = masterTable.get_dataItems();
            for (var i = 0; i < row.length; i++) {
                masterTable.get_dataItems()[i].set_selected(false);
            }
        }
        // document.getElementById('ctl00_Tile3_KDMAddressRTA_gridKDMAddressRTA').style.display = 'none';
    }
    function onKDMGridRowClick(sender, args) {
        Public_KDM_ID = args._dataKeyValues.KDM_ID;
        var KDM_ID = args._dataKeyValues.KDM_ID;
        $('#EditDeleteButtons').show();
        $('#AddAddressButton').show();
        clientManager.set_SelectionData({ KDM_ID: Public_KDM_ID }, 1);
       // document.getElementById('ctl00_Tile3_KDMAddressRTA_gridKDMAddressRTA').style.display = 'block';
        KDMAddressGrid(KDM_ID, sender.ClientID);
        $('#EditDeleteAddressButtons').hide();
        
    }

    function onKDMAddressGridRowClick(sender, args) {
        Public_KDM_ID = args._dataKeyValues.KDM_ID;
        Public_KDM_ADD_ID = args._dataKeyValues.ID;        
        var KDM_ID = args._dataKeyValues.KDM_ID;
        var KDM_ADD_ID = args._dataKeyValues.ID;
        if (args._gridDataItem._dataItem.Is_Primary_Add)
            $('#EditDeleteAddressButtons').hide();
        else
            $('#EditDeleteAddressButtons').show();
    }
    function get_KDMAddressGrid() {
        if ($find("ctl00_Tile3_KDMAddressRTA_gridKDMAddress"))
            return $find("ctl00_Tile3_KDMAddressRTA_gridKDMAddress");
         else   
        if ($find("ctl00_Tile3_KDMAddressMJ_gridKDMAddress"))
            return $find("ctl00_Tile3_KDMAddressMJ_gridKDMAddress");
         else   
        if ($find("ctl00_Tile3_KDMAddressSS1_gridKDMAddress"))
            return $find("ctl00_Tile3_KDMAddressSS1_gridKDMAddress");
         else   
        if ($find("ctl00_Tile3_KDMAddressVAR1_gridKDMAddress")) {
            return $find("ctl00_Tile3_KDMAddressVAR1_gridKDMAddress");
        }
    }
    function get_KDMGrid() {
        //return $find("ctl00_Tile3_KDMDetailsRTA_gridKDMDetailsRTA");
        if ($find("ctl00_Tile3_KDMDetailsRTA_gridKDMDetailsRTA"))
            return $find("ctl00_Tile3_KDMDetailsRTA_gridKDMDetailsRTA");
        else
            if ($find("ctl00_Tile3_KDMDetailsMJ_gridKDMDetailsMJ"))
            return $find("ctl00_Tile3_KDMDetailsMJ_gridKDMDetailsMJ");
        else
            if ($find("ctl00_Tile3_KDMDetailsSS1_gridKDMDetailsSS"))
            return $find("ctl00_Tile3_KDMDetailsSS1_gridKDMDetailsSS");
        else
            if ($find("ctl00_Tile3_KDMDetailsVAR_gridKDMDetailsVAR")) {
            return $find("ctl00_Tile3_KDMDetailsVAR_gridKDMDetailsVAR");
        }
    }
    function RefreshKDMAddress() {
       var gridAddress = get_KDMAddressGrid(); 
            
        if (Public_KDM_ID == '') {
            if (gridAddress) {
                var mt = gridAddress.get_masterTableView();
                $setGridFilter(gridAddress, "KDM_ID", 0, "EqualTo", "System.Int32");
            }
        }
    }
    function KDMAddressGrid(KDMID, ctrlClientId) {
        var KDM_ID = KDMID;

        var gridAddress = get_KDMAddressGrid(); 
    
        if (gridAddress) {
            var mt = gridAddress.get_masterTableView();
          
            if (KDM_ID)
                $setGridFilter(gridAddress, "KDM_ID", KDM_ID, "EqualTo", "System.Int32");
            else
                $clearGridFilter(mt, "KDM_ID");

            mt.clearSelectedItems();

            mt.rebind();
        }
    }
    function OpenKDM(LinkClicked, KDMID, FILE) {
               
        var oManager = GetRadWindowManager();
        var grid = clientManager.get_PlanInfoGrid();
        var str = "";

        if (LinkClicked == "DelKDM")
        {
            str = "custom/millennium/todaysaccounts/all/AddEditKDM"+FILE+".aspx?LinkClicked=" + LinkClicked + "&KDM_ID=" + Public_KDM_ID;
        }else
        if (LinkClicked == "EditKDM") 
        {
            str = "custom/millennium/todaysaccounts/all/AddEditKDM"+FILE+".aspx?LinkClicked=" + LinkClicked + "&KDM_ID=" + Public_KDM_ID;
        }
        else
        {
            str = "custom/millennium/todaysaccounts/all/AddEditKDM" + FILE + ".aspx?LinkClicked=" + LinkClicked;
        }

        var q;
        
//        if (clientManager.get_EffectiveChannel() != 12)
        // q = clientManager.getSelectionDataForPostback() + "&PlanName=" + encodeURIComponent(grid.get_masterTableView().get_selectedItems()[0]._dataItem.Plan_Name);
            q = clientManager.getSelectionDataForPostback() + "&PlanName=" + encodeURIComponent(grid.get_masterTableView().get_selectedItems()[0]._dataItem.Plan_Name) + "&PlanID=" + (grid.get_masterTableView().get_selectedItems()[0]._dataItem.Plan_ID);           
//        else if (dodIDs)
//            q = "Plan_ID=" + $.trim(dodIDs.split(",")[0]) + "&PlanName=" + clientManager.get_ChannelMenu().get_items().getItem(0).get_text();

        str = str + "&" + q;

        var oWnd = radopen(str, LinkClicked);
        oWnd.setSize(960, 370);
        oWnd.Center();
    }


    function OpenKDMAddress(LinkClicked, KDMID, FILE) {

        var oManager = GetRadWindowManager();
        var grid = clientManager.get_PlanInfoGrid();
        var str = "";

        if (LinkClicked == "DelKDM") {
            str = "custom/millennium/todaysaccounts/all/AddEditKDMAddress" + FILE + ".aspx?LinkClicked=" + LinkClicked + "&KDM_ADD_ID=" + Public_KDM_ADD_ID;
        } else
            if (LinkClicked == "EditKDM") {
                str = "custom/millennium/todaysaccounts/all/AddEditKDMAddress" + FILE + ".aspx?LinkClicked=" + LinkClicked + "&KDM_ADD_ID=" + Public_KDM_ADD_ID;
        }
        else {
            str = "custom/millennium/todaysaccounts/all/AddEditKDMAddress" + FILE + ".aspx?LinkClicked=" + LinkClicked;
        }

        var q;
        q = clientManager.getSelectionDataForPostback() + "&PlanName=" + encodeURIComponent(grid.get_masterTableView().get_selectedItems()[0]._dataItem.Plan_Name) + "&KDM_ID=" + Public_KDM_ID;
        str = str + "&" + q;
        
        var oWnd = radopen(str, LinkClicked);
        oWnd.setSize(960, 370);
        oWnd.Center();
    }

    function OpenKDMSS(LinkClicked, KDMID) {

        var oManager = GetRadWindowManager();
        var grid = clientManager.get_PlanInfoGrid();
        var str = "";

        if (LinkClicked == "DelKDM") {
            str = "custom/millennium/todaysaccounts/all/AddEditKDMSS.aspx?LinkClicked=" + LinkClicked;
        } else
            if (LinkClicked == "EditKDM") {
            str = "custom/millennium/todaysaccounts/all/AddEditKDMSS.aspx?LinkClicked=" + LinkClicked;
        }
        else {
            str = "custom/millennium/todaysaccounts/all/AddEditKDMSS.aspx?LinkClicked=" + LinkClicked;
        }

        var q;

        //        if (clientManager.get_EffectiveChannel() != 12)
        // q = clientManager.getSelectionDataForPostback() + "&PlanName=" + encodeURIComponent(grid.get_masterTableView().get_selectedItems()[0]._dataItem.Plan_Name);
        q = clientManager.getSelectionDataForPostback() + "&PlanName=" + encodeURIComponent(grid.get_masterTableView().get_selectedItems()[0]._dataItem.Plan_Name) + "&PlanID=" + (grid.get_masterTableView().get_selectedItems()[0]._dataItem.Plan_ID) + "&KDM_ID=" + (grid.get_masterTableView().get_selectedItems()[0]._dataItem.KDM_ID);
        //        else if (dodIDs)
        //            q = "Plan_ID=" + $.trim(dodIDs.split(",")[0]) + "&PlanName=" + clientManager.get_ChannelMenu().get_items().getItem(0).get_text();

        str = str + "&" + q;

        var oWnd = radopen(str, LinkClicked);
        oWnd.setSize(960, 370);
        oWnd.Center();
    }










    function setCustomTitle(sender, args) {
    
        var hdnMeetOutcome = $("#ctl00_main_hdnMeetOutcome");
        var strPlans = "";

        //Get currently selected list of MeetingOutcome.
        if (hdnMeetOutcome.length > 0)
            strPlans = hdnMeetOutcome.val();

        //If list is not empty then update MeetingOutcome dropdown message to 'Change Selection' else '-Select Persons Met-'.
        if (strPlans != "")
            sender.set_text("-Change Selection-");
        else
            sender.set_text("-Select Custom Title-");
    }
    function CustomTitleChanged(sender, TitleID) {
        var tree_ul = $('#spanCustomTitle ul').children();
        var t = "";

        if (sender.checked == false) {
            if (tree_ul) {
                var b = "m" + TitleID;
                $("#m" + b).remove();
            }
        }
        if (sender.checked == true) {
            if ($('#spanCustomTitle > ul').size() > 0) {
                t = t + "<li id=mm" + TitleID + ">" + $("#ctl00_main_formKDMView_rdlCustomTitle #m" + TitleID + " label").text() + "</li>";
               
                $("#spanCustomTitle ul").append(t);
            }
            else {
                t = "<ul>" + "<li id=mm" + TitleID + ">" + $("#ctl00_main_formKDMView_rdlCustomTitle #m" + TitleID + " label").text() + "</li></ul>";
                
                $("#spanCustomTitle").append(t);
            }
        }
        var strPlans = "";
        var a = [];
        var hdnMeetout = $("#ctl00_main_hdnMeetOutcome");
        var IDs = TitleID;

        if (window.event)
            window.event.cancelBubble = true;

        //Get currently selected list of MeetOutID.
        if (hdnMeetout.length > 0)
            strPlans = hdnMeetout.val();

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
        hdnMeetout.val(strPlans);

         updCustomTitleChkSelection();

    }
    function updCustomTitleChkSelection() {
        var hdnMeetout = $("#ctl00_main_hdnMeetOutcome");

        $("#ctl00_main_formViewKDM_rdlCustomTitle input[type=checkbox]").removeAttr("checked");
        var str = hdnMeetout.val();
        var t = "<ul>";

        $("#spanCustomTitle").html("");

        if (str) {
            var a = str.split(",");
            var hasOther = false;
            for (var i = 0; i < a.length; i++) {
                $("#ctl00_main_formKDMView_rdlCustomTitle #m" + a[i] + " input").attr("checked", true);

                if ($.browser.msie && $.browser.version < 7)
                    $("#ctl00_main_formKDMView_rdlCustomTitle #m" + a[i] + " input").attr("defaultChecked", "defaultChecked");

                t = t + "<li id=mm" + a[i] + ">" + $("#m" + a[i] + " label").text() + "</li>";
                
                if (a[i] == 100) {
                    hasOther = true;
                }
            }

            if (hasOther)
                $("#ctl00_main_formViewKDM_txtCustomTitle").attr("disabled", false);
            else
                $("#ctl00_main_formViewKDM_txtCustomTitle").attr("disabled", true).val("");

            t = t + "</ul>"
            $("#spanCustomTitle").append(t)
        }
    }



    //Credentials script

    function setCustomCredentials(sender, args) {
        var hdnCredentialsOutcome = $("#ctl00_main_hdnCredentialsOutcome");
        var strPlans = "";

        //Get currently selected list of MeetingOutcome.
        if (hdnCredentialsOutcome.length > 0)
            strPlans = hdnCredentialsOutcome.val();

        //If list is not empty then update MeetingOutcome dropdown message to 'Change Selection' else '-Select Persons Met-'.
        if (strPlans != "")
            sender.set_text("-Change Selection-");
        else
            sender.set_text("-Select Custom Credentials-");
    }
    function CustomCredentialsChanged(sender, CredentialsID) {
        var tree_ul = $('#spanCredentials ul').children();
        var t = "";

        var strPlans = "";
        var a = [];
        var hdnCredentialsout = $("#ctl00_main_hdnCredentialsOutcome");
        var IDs = CredentialsID;

        if (window.event)
            window.event.cancelBubble = true;

        //Get currently selected list of MeetOutID.
        if (hdnCredentialsout.length > 0)
            strPlans = hdnCredentialsout.val();

        //Get list of selected MeetOutID in an array.
        if (strPlans != "")
            a = strPlans.split(",");

        //Remove the current IDs(MeetOutID) from the array.
        a = $.grep(a, function(i) { return i != IDs; }, false);

        //If checkbox is checked and the array length is less than 10 then add the selected MeetOutID to the list
        //Please note that the current MeetOut selection allows maximum of 10 selected plans.
        if (sender.checked) {
            if (a.length < 10)
                a[a.length] = IDs;
            else {
                sender.checked = false;
                $alert("A maximum of 10 meeting Outcome can be selected.", "Credentials Outcome");
            }
        }
        strPlans = a.join(",");

        //Update hidden variable.
        hdnCredentialsout.val(strPlans);

        updCredentialsChkSelection();

    }

    function updCredentialsChkSelection() {
        var hdnCredentialsout = $("#ctl00_main_hdnCredentialsOutcome");

        $("#ctl00_main_formViewKDM_rdlCustomTitle input[type=checkbox]").removeAttr("checked");
        var str = hdnCredentialsout.val();
        var t = "<ul>";

        $("#spanCredentials").html("");

        if (str) {
            var a = str.split(",");
            var hasOther = false;
            for (var i = 0; i < a.length; i++) {

              
                $("#ctl00_main_formKDMView_rdlCredentials #c" + a[i] + " input").attr("checked", true);

                if ($.browser.msie && $.browser.version < 7)
                    $("#ctl00_main_formKDMView_rdlCredentials #c" + a[i] + " input").attr("defaultChecked", "defaultChecked");

                t = t + "<li id=mm" + a[i] + ">" + $("#c" + a[i] + " label").text() + "</li>";                    
            
                if (a[i] == 100) {
                    hasOther = true;
                }
            }

            if (hasOther)
                $("#ctl00_main_formViewKDM_txtCustomTitle").attr("disabled", false);
            else
                $("#ctl00_main_formViewKDM_txtCustomTitle").attr("disabled", true).val("");

            t = t + "</ul>"
            $("#spanCredentials").append(t)
        }
    }

    function RefreshPlanInfo() {
        window.top.clientManager.set_SelectionData(window.top.clientManager.get_SelectionData());
        window.setTimeout(CloseWin, 2000);
        return window.top.$find("ctl00_main_planInfo_gridPlanInfo").get_masterTableView().rebind();
    }





    //CAC script

    function setCustomCAC(sender, args) {
        var hdnCACOutcome = $("#ctl00_main_hdnCACOutcome");
        var strPlans = "";
        
        //Get currently selected list of MeetingOutcome.
        if (hdnCACOutcome.length > 0)
            strPlans = hdnCACOutcome.val();

        //If list is not empty then update MeetingOutcome dropdown message to 'Change Selection' else '-Select Persons Met-'.
        if (strPlans != "")
            sender.set_text("-Change Selection-");
        else
            sender.set_text("-Select Custom CAC-");
    }
    function CustomCACChanged(sender, CACID) {
        var tree_ul = $('#spanCustomCAC ul').children();
        var t = "";

        var strPlans = "";
        var a = [];
        var hdnCACout = $("#ctl00_main_hdnCACOutcome");
        var IDs = CACID;

        if (window.event)
            window.event.cancelBubble = true;

        //Get currently selected list of MeetOutID.
        if (hdnCACout.length > 0)
            strPlans = hdnCACout.val();

        //Get list of selected MeetOutID in an array.
        if (strPlans != "")
            a = strPlans.split(",");

        //Remove the current IDs(MeetOutID) from the array.
        a = $.grep(a, function(i) { return i != IDs; }, false);

        //If checkbox is checked and the array length is less than 10 then add the selected MeetOutID to the list
        //Please note that the current MeetOut selection allows maximum of 10 selected plans.
        if (sender.checked) {
            if (a.length < 10)
                a[a.length] = IDs;
            else {
                sender.checked = false;
                $alert("A maximum of 10 meeting Outcome can be selected.", "CAC Outcome");
            }
        }
        strPlans = a.join(",");

        //Update hidden variable.
        hdnCACout.val(strPlans);

        updCACChkSelection();

    }

    function updCACChkSelection() {
        var hdnCACout = $("#ctl00_main_hdnCACOutcome");

        $("#ctl00_main_formViewKDM_rdlCustomTitle input[type=checkbox]").removeAttr("checked");
        var str = hdnCACout.val();
        var t = "<ul>";

        $("#spanCustomCAC").html("");

        if (str) {
            var a = str.split(",");
            var hasOther = false;
            for (var i = 0; i < a.length; i++) {

            
                $("#ctl00_main_formKDMView_rdlCustomCAC #b" + a[i] + " input").attr("checked", true);

                if ($.browser.msie && $.browser.version < 7)
                    $("#ctl00_main_formKDMView_rdlCustomCAC #b" + a[i] + " input").attr("defaultChecked", "defaultChecked");

                t = t + "<li id=mm" + a[i] + ">" + $("#b" + a[i] + " label").text() + "</li>";

                if (a[i] == 100) {
                    hasOther = true;
                }
            }

            if (hasOther)
                $("#ctl00_main_formViewKDM_txtCustomCAC").attr("disabled", false);
            else
                $("#ctl00_main_formViewKDM_txtCustomCAC").attr("disabled", true).val("");

            t = t + "</ul>"
            $("#spanCustomCAC").append(t)
        }
    }



    //Job Function and Specialty Function's

    function setCustomSpecialty(sender, args) {
        var hdnSpecialty = $("#ctl00_main_hdnSpecialty");
        var strPlans = "";

        //Get currently selected list of MeetingOutcome.
        if (hdnSpecialty.length > 0)
            strPlans = hdnSpecialty.val();

        if (strPlans != "")
            sender.set_text("-Change Selection-");
        else
            sender.set_text("-Select Custom Specialty-");
    }

    function setCustomJobFunction(sender, args) {
        var hdnJobFunction = $("#ctl00_main_hdnJobFunction");
        var strPlans = "";

        //Get currently selected list of MeetingOutcome.
        if (hdnJobFunction.length > 0)
            strPlans = hdnJobFunction.val();

        if (strPlans != "")
            sender.set_text("-Change Selection-");
        else
            sender.set_text("-Select Custom Job Function-");
    }


    function CustomSpecialtyChanged(sender, SpecialtyID) {
        var tree_ul = $('#spanCustomSpecialty ul').children();
        var t = "";

        if (sender.checked == false) {
            if (tree_ul) {
                var b = "s" + SpecialtyID;
                $("#s" + b).remove();
            }
        }
        if (sender.checked == true) {
            if ($('#spanCustomSpecialty > ul').size() > 0) {
                t = t + "<li id=ss" + SpecialtyID + ">" + $("#ctl00_main_formKDMView_rdlCustomSpecialty #s" + SpecialtyID + " label").text() + "</li>";

                $("#spanCustomSpecialty ul").append(t);
            }
            else {
                t = "<ul>" + "<li id=ss" + SpecialtyID + ">" + $("#ctl00_main_formKDMView_rdlCustomSpecialty #s" + SpecialtyID + " label").text() + "</li></ul>";

                $("#spanCustomSpecialty").append(t);
            }
        }

        var strPlans = "";
        var a = [];
        var hdnSpecialty = $("#ctl00_main_hdnSpecialty");
        var IDs = SpecialtyID;

        if (window.event)
            window.event.cancelBubble = true;

        //Get currently selected list of MeetOutID.
        if (hdnSpecialty.length > 0)
            strPlans = hdnSpecialty.val();

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
        hdnSpecialty.val(strPlans);

        updCustomSpecialtyChkSelection();
    }

    function updCustomSpecialtyChkSelection() {
        var hdnSpecialty = $("#ctl00_main_hdnSpecialty");

        $("#ctl00_main_formViewKDM_rdlCustomSpecialty input[type=checkbox]").removeAttr("checked");
        var str = hdnSpecialty.val();
        var t = "<ul>";

        $("#spanCustomSpecialty").html("");

        if (str) {
            var a = str.split(",");
            var hasOther = false;
            for (var i = 0; i < a.length; i++) {
                $("#ctl00_main_formKDMView_rdlCustomSpecialty #s" + a[i] + " input").attr("checked", true);

                if ($.browser.msie && $.browser.version < 7)
                    $("#ctl00_main_formKDMView_rdlCustomSpecialty #s" + a[i] + " input").attr("defaultChecked", "defaultChecked");

                t = t + "<li id=ss" + a[i] + ">" + $("#s" + a[i] + " label").text() + "</li>";

                if (a[i] == 100) {
                    hasOther = true;
                }
            }

            if (hasOther)
                $("#ctl00_main_formViewKDM_txtCustomSpecialty").attr("disabled", false);
            else
                $("#ctl00_main_formViewKDM_txtCustomSpecialty").attr("disabled", true).val("");

            t = t + "</ul>"
            $("#spanCustomSpecialty").append(t)
        }
    }



    function CustomJobFunctionChanged(sender, JobFunctionID) {
        var tree_ul = $('#spanCustomJobFunction ul').children();
        var t = "";

        if (sender.checked == false) {
            if (tree_ul) {
                var b = "j" + JobFunctionID;
                $("#j" + b).remove();
            }
        }
        if (sender.checked == true) {
            if ($('#spanCustomJobFunction > ul').size() > 0) {
                t = t + "<li id=jj" + JobFunctionID + ">" + $("#ctl00_main_formKDMView_rdlCustomJobFunction #j" + JobFunctionID + " label").text() + "</li>";

                $("#spanCustomJobFunction ul").append(t);
            }
            else {
                t = "<ul>" + "<li id=jj" + JobFunctionID + ">" + $("#ctl00_main_formKDMView_rdlCustomJobFunction #j" + JobFunctionID + " label").text() + "</li></ul>";

                $("#spanCustomJobFunction").append(t);
            }
        }

        var strPlans = "";
        var a = [];
        var hdnJobFunction = $("#ctl00_main_hdnJobFunction");
        var IDs = JobFunctionID;

        if (window.event)
            window.event.cancelBubble = true;

        //Get currently selected list of MeetOutID.
        if (hdnJobFunction.length > 0)
            strPlans = hdnJobFunction.val();

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
        hdnJobFunction.val(strPlans);

        updCustomJobFunctionChkSelection();
    }

    function updCustomJobFunctionChkSelection() {
        var hdnJobFunction = $("#ctl00_main_hdnJobFunction");

        $("#ctl00_main_formViewKDM_rdlCustomJobFunction input[type=checkbox]").removeAttr("checked");
        var str = hdnJobFunction.val();
        var t = "<ul>";

        $("#spanCustomJobFunction").html("");

        if (str) {
            var a = str.split(",");
            var hasOther = false;
            for (var i = 0; i < a.length; i++) {
                $("#ctl00_main_formKDMView_rdlCustomJobFunction #j" + a[i] + " input").attr("checked", true);

                if ($.browser.msie && $.browser.version < 7)
                    $("#ctl00_main_formKDMView_rdlCustomJobFunction #j" + a[i] + " input").attr("defaultChecked", "defaultChecked");

                t = t + "<li id=jj" + a[i] + ">" + $("#j" + a[i] + " label").text() + "</li>";

                if (a[i] == 100) {
                    hasOther = true;
                }
            }

            if (hasOther)
                $("#ctl00_main_formViewKDM_txtCustomJobFunction").attr("disabled", false);
            else
                $("#ctl00_main_formViewKDM_txtCustomJobFunction").attr("disabled", true).val("");

            t = t + "</ul>"
            $("#spanCustomJobFunction").append(t)
        }
    }

    //END ----->>>Job Function and Specialty Function's

    function RefreshPlanInfo() {
        window.top.clientManager.set_SelectionData(window.top.clientManager.get_SelectionData());
        window.setTimeout(CloseWin, 2000);
        return window.top.$find("ctl00_main_planInfo_gridPlanInfo").get_masterTableView().rebind();
    }
    function CloseWin() {
        var manager = window.top.GetRadWindowManager();

        var window1 = manager.getWindowByName("AddKDM");
        if (window1 != null)
            window1.close();

        var window2 = manager.getWindowByName("EditKDM");
        if (window2 != null)
            window2.close();

        var window3 = manager.getWindowByName("DelKDM");
        if (window3 != null)
            window3.close();    
    }

    function ClearForm() {
        //Reset the form values.
        document.forms[0].reset();
        $("#ctl00_main_hdnStatesCovered").val('');
        $("#ctl00_main_hdnMJ").val('');
        $("#ctl00_main_hdnMeetOutcome").val('');
        $("#ctl00_main_hdnCredentialsOutcome").val('');
        $("#ctl00_main_hdnCACOutcome").val('');

        $("#spanJurisditionsCovered").html('');
        $("#spanState").html('');

        $("#spanCustomTitle").html('');
        $("#spanCredentials").html('');
        $("#spanCustomCAC").html('');
        
        if ($.browser.msie && $.browser.version < 7) {
            $("#aspnetForm input[type=checkbox]").removeAttr("defaultChecked");
            $("#aspnetForm input[type=checkbox]").removeAttr("checked");
            $("#aspnetForm input[type=checkbox]").attr("defaultChecked", false);
            $("#aspnetForm input[type=checkbox]").attr("checked", false);
        }

        $("#formKDMView input[type != submit]").val("");
    }

//    function doNothing()
//    {
//        var eve = window.event;
//        var keycode = eve.keyCode || eve.which || eve.charCode;

//        if (keycode == 13)
//        {
//            eve.cancelBubble = true;
//            eve.returnValue = false;

//            if (eve.stopPropagation)
//            {
//                eve.stopPropagation();
//                eve.preventDefault();
//            }

//            return false;
//        } 
//    } 
</script>
