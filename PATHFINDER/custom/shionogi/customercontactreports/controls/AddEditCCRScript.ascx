<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AddEditCCRScript.ascx.cs" Inherits="custom_controls_AddEditCCRScript" %>
<script type="text/javascript">
    $(document).ready(function()
    {
        //$(".datePicker").datepicker();
        $("#ctl00_main_formViewCCR_rdCCRDate").datepicker();
        $("#ctl00_main_formViewCCR_rdFollowUpDate").datepicker();

        $(".RadComboBox .rcbArrowCell A").height(21);

        //Fix IE drop down arrow issue with RadDropList/ItemTemplate
        if ($.browser.msie)
            new cmd(null, fixDropdowns, null, 500);

        //Fix multiple submit issue by disabling enter key
        $("#AddCCRMain").keypress(function(e)
        {
            if (e.which == 13)
                return false;
        });
        
        //Disable use of up/down arrows to select items in checkbox dropdowns
        $("#ctl00_main_formViewCCR_rdlProductsDiscussed_Input, #ctl00_main_formViewCCR_rdlKeyContacts_Input, #ctl00_main_formViewCCR_rdlMeetingOutCome_Input, #ctl00_main_formViewCCR_rdlFollowUp_Input").keydown(function(e)
        {
            if (e.which == 13 || e.which == 38 || e.which == 40)
                return false;
        });
        
        //Fix possible editing of radComboBox
        $("#ctl00_main_formViewCCR_rdlProductsDiscussed_Input").attr("readOnly", "readonly");
        $("#ctl00_main_formViewCCR_rdlKeyContacts_Input").attr("readOnly", "readonly");
        $("#ctl00_main_formViewCCR_rdlMeetingOutCome_Input").attr("readOnly", "readonly");
        $("#ctl00_main_formViewCCR_rdlFollowUp_Input").attr("readOnly", "readonly");

    });

    function fixDropdowns()
    {
        $("#ctl00_main_formViewCCR_rdlProductsDiscussed table").width($("#ctl00_main_formViewCCR_rdlProductsDiscussed table").width() - 2);
        $("#ctl00_main_formViewCCR_rdlKeyContacts table").width($("#ctl00_main_formViewCCR_rdlKeyContacts table").width() - 2);
        $("#ctl00_main_formViewCCR_rdlMeetingOutCome table").width($("#ctl00_main_formViewCCR_rdlMeetingOutCome table").width() - 2);
        $("#ctl00_main_formViewCCR_rdlFollowUp table").width($("#ctl00_main_formViewCCR_rdlFollowUp table").width() - 2);
    }
    
    function ClearForm()
    {
        //Reset the form values.
        document.forms[0].reset();

        $("#ctl00_main_hdnPrdsDisccused").val('');
        $("#ctl00_main_hdnFollowupNotes").val('');
        $("#ctl00_main_hdnMeetOutcome").val('');
        $("#ctl00_main_hdnKeyContacts").val('');

        $("#spanProducts").html('');
        $("#spanFollowup").html('');
        $("#spanMeetOut").html('');
        $("#spanKeyContacts").html('');

        if ($.browser.msie && $.browser.version < 7)
        {
            $("#aspnetForm input[type=checkbox]").removeAttr("defaultChecked");
            $("#aspnetForm input[type=checkbox]").removeAttr("checked");
            $("#aspnetForm input[type=checkbox]").attr("defaultChecked", false);
            $("#aspnetForm input[type=checkbox]").attr("checked", false);
        }

        $("#AddCCRMain input[type != submit]").val("");
    }

    function UpdChkSelection() 
    {
        var hdnPrdsDisc = $("#ctl00_main_hdnPrdsDisccused");

        $("#ctl00_main_formViewCCR_rdlProductsDiscussed input[type=checkbox]").removeAttr("checked");
        var str = hdnPrdsDisc.val();
        var t ="<ul>";
        if (str) 
        {
            var a = str.split(",");
            
            for (var i = 0; i < a.length; i++)
            {
                $("#ctl00_main_formViewCCR_rdlProductsDiscussed #p" + a[i] + " input").attr("checked", true);

                if ($.browser.msie && $.browser.version < 7)
                    $("#ctl00_main_formViewCCR_rdlProductsDiscussed #p" + a[i] + " input").attr("defaultChecked", "defaultChecked");

                //var checkbox = $("#ctl00_main_formViewCCR_rdlProductsDiscussed #p" + a[i] + " input");
                //var domCheckbox = checkbox[0];
                //var isChecked = domCheckbox.checked;

                //alert(isChecked);

                t = t + "<li id=pp" + a[i] + ">" + $("#p" + a[i] + " label").text() + "</li>";
            }
            
            t = t + "</ul>"
            $("#spanProducts").append(t);
        }
    }
    function updFollowChkSelection() 
    {
        var hdnFollowup = $("#ctl00_main_hdnFollowupNotes");
       
        $("#ctl00_main_formViewCCR_rdlFollowUp input[type=checkbox]").removeAttr("checked");
        var str = hdnFollowup.val();
        var t = "<ul>";

        $("#spanFollowup").html("");
        if (str) 
        {
            var a = str.split(",");
            var hasOther = false;
            for (var i = 0; i < a.length; i++)
            {
                $("#ctl00_main_formViewCCR_rdlFollowUp #f" + a[i] + " input").attr("checked", true);

                if ($.browser.msie && $.browser.version < 7)
                    $("#ctl00_main_formViewCCR_rdlFollowUp #f" + a[i] + " input").attr("defaultChecked", "defaultChecked");
                //$("#f" + a[i] + " input").attr("checked", true);
                t = t + "<li id=ff" + a[i] + ">" + $("#f" + a[i] + " label").text() + "</li>";
                
                if (a[i] == 100)
                {
                    hasOther = true;
                }
            }

            if (hasOther)            
                $("#ctl00_main_formViewCCR_txtFollowupNotesOther").attr("readonly",false);            
            else
                $("#ctl00_main_formViewCCR_txtFollowupNotesOther").attr("readonly", true).val("");
                
            t = t + "</ul>"
            $("#spanFollowup").append(t)
        }
    }
    function updMeetOutcomeChkSelection() 
    {
        var hdnMeetout = $("#ctl00_main_hdnMeetOutcome");

        $("#ctl00_main_formViewCCR_rdlMeetingOutCome input[type=checkbox]").removeAttr("checked");
        var str = hdnMeetout.val();
        var t = "<ul>";

        $("#spanMeetOut").html("");
        
        if (str) 
        {
            var a = str.split(",");
            var hasOther = false;
            for (var i = 0; i < a.length; i++)
            {
                $("#ctl00_main_formViewCCR_rdlMeetingOutCome #m" + a[i] + " input").attr("checked", true);

                if ($.browser.msie && $.browser.version < 7)
                    $("#ctl00_main_formViewCCR_rdlMeetingOutCome #m" + a[i] + " input").attr("defaultChecked", "defaultChecked");
                //$("#m" + a[i] + " input").attr("checked", true);
                t = t + "<li id=mm" + a[i] + ">" + $("#m" + a[i] + " label").text() + "</li>";

                if (a[i] == 100)
                {
                    hasOther = true;
                }
            }

            if (hasOther)            
                $("#ctl00_main_formViewCCR_txtMeetingOutcomeOther").attr("readonly",false);            
            else
                $("#ctl00_main_formViewCCR_txtMeetingOutcomeOther").attr("readonly", true).val("");
            
            t = t + "</ul>"
            $("#spanMeetOut").append(t)
        }
    }
    function updKeyContactChkSelection() 
    {
        var hdnMeetout = $("#ctl00_main_hdnKeyContacts");

        $("#ctl00_main_formViewCCR_rdlKeyContacts input[type=checkbox]").removeAttr("checked");
        var str = hdnMeetout.val();
        var tree_ul = $('#spanKeyContacts ul').children();
        var t = "<ul>";
        
        if (str) 
        {
            var a = str.split("|");
            for (var i = 0; i < a.length; i++) 
            {
                var strl = a[i].split(",");
                $("#ctl00_main_formViewCCR_rdlKeyContacts #k" + strl[0] + " input").attr("checked", true);

                if ($.browser.msie && $.browser.version < 7)
                    $("#ctl00_main_formViewCCR_rdlKeyContacts #k" + strl[0] + " input").attr("defaultChecked", "defaultChecked");
                //$("#k" + strl[0] + " input").attr("checked", true);
                t = t + "<li id=kk" + strl[0] + ">" + $("#k" + strl[0] + " label").text() + "</li>";
            }
            
            t = t + "</ul>"
            $("#spanKeyContacts").append(t)
        }

    }
    function KeyContactChanged(sender, FullID, Iskey) 
    {
        var tree_ul = $('#spanKeyContacts ul').children();
        var t = "";

        if (sender.checked == false) 
        {
            if (tree_ul) 
            {
                var b = "k" + FullID;
                $("#k" + b).remove();
            }
        }

        if (sender.checked == true) 
        {
            if ($('#spanKeyContacts > ul').size() > 0) 
            {
                t = t + "<li id=kk" + FullID + ">" + $("#k" + FullID + " label").text() + "</li>";
                $("#spanKeyContacts ul").append(t);
            }
            else 
            {
                t = "<ul>" + "<li id=kk" + FullID + ">" + $("#k" + FullID + " label").text() + "</li></ul>";
                $("#spanKeyContacts").append(t);
            }
        }
        
        var strPlans = "";
        var a = [];
        var hdnKeyContact = $("#ctl00_main_hdnKeyContacts");
        var IDs = FullID + "," + Iskey;
        var ind = sender.id;
        if (window.event)
            window.event.cancelBubble = true;

        //Get currently selected list of KeyContactsID.
        if (hdnKeyContact.length > 0)
            strPlans = hdnKeyContact.val();

        //Get list of selected KeyContactsID in an array.
        if (strPlans != "")
            a = strPlans.split("|");

        //Remove the current IDs(KeyContactsID) from the array.
        a = $.grep(a, function(i) { return i != IDs; }, false);

        //If checkbox is checked and the array length is less than 10 then add the selected key contactID to the list
        //Please note that the current contact selection allows maximum of 10 selected key contacts
        if (sender.checked) 
        {
            if (a.length < 10) 
                a[a.length] = IDs;
            else 
            {
                sender.checked = false;
                $alert("A maximum of 10 contacts can be selected.", "Key Contacts");
            }
        }
//        else 
//        {
//            if (a.length == 0) 
//            {
//            }
//        }

        strPlans = a.join("|");

        //Update hidden variable.
        hdnKeyContact.val(strPlans);
    }
    
    
    function MeetOutcomeChanged(sender, MeetOutID)
    {       
        var tree_ul = $('#spanMeetOut ul').children();
        var t = "";

//        if (sender.checked == false) 
//        {
//            if (tree_ul) 
//            {
//                var b = "m" + MeetOutID;
//                $("#m" + b).remove();
//            }
//        }

//        if (sender.checked == true) 
//        {
//            if ($('#spanMeetOut > ul').size() > 0) 
//            {
//                t = t + "<li id=mm" + MeetOutID + ">" + $("#m" + MeetOutID + " label").text() + "</li>";
//                $("#spanMeetOut ul").append(t);
//            }
//            else 
//            {
//                t = "<ul>" + "<li id=mm" + MeetOutID + ">" + $("#m" + MeetOutID + " label").text() + "</li></ul>";
//                $("#spanMeetOut").append(t);
//            }
//        }
        
        var strPlans = "";
        var a = [];
        var hdnMeetout = $("#ctl00_main_hdnMeetOutcome");
        var IDs = MeetOutID;

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

        //If checkbox is checked and the array length is less than 10 then add the selected MeetOutID to the list
        //Please note that the current MeetOut selection allows maximum of 10 selected plans.
        if (sender.checked) 
        {
            if (a.length < 10) 
                a[a.length] = IDs;
            else 
            {
                sender.checked = false;
                $alert("A maximum of 10 meeting Outcome can be selected.", "Meeting Outcome");
            }
        }
//        else 
//        {
//            if (a.length == 0) 
//            {
//            }
//        }

        strPlans = a.join(",");

        //Update hidden variable.
        hdnMeetout.val(strPlans);

        updMeetOutcomeChkSelection();

    }
    function FollowupNotesChanged(sender, FollowupID)
    {
        var tree_ul = $('#spanFollowup ul').children();
        var t = "";
        
//        if (sender.checked == false) 
//        {
//            if (tree_ul) 
//            {
//                var b = "f" + FollowupID;
//                $("#f" + b).remove();
//            }
//        }

//        if (sender.checked == true) 
//        {
//            if ($('#spanFollowup > ul').size() > 0) 
//            {
//                t = t + "<li id=ff" + FollowupID + ">" + $("#f" + FollowupID + " label").text() + "</li>";
//                $("#spanFollowup ul").append(t);
//            }
//            else 
//            {
//                t = "<ul>" + "<li id=ff" + FollowupID + ">" + $("#f" + FollowupID + " label").text() + "</li></ul>";
//                $("#spanFollowup").append(t);
//            }
//        }
        
        var strPlans = "";
        var a = [];
        var hdnFollowup = $("#ctl00_main_hdnFollowupNotes");
        var IDs = FollowupID;

        if (window.event)
            window.event.cancelBubble = true;

        //Get currently selected list of PlanIDs.
        if (hdnFollowup.length > 0)
            strPlans = hdnFollowup.val();

        //Get list of selected PlanIDs in an array.
        if (strPlans != "")
            a = strPlans.split(",");

        //Remove the current IDs(FollowupNotesID) from the array.
        a = $.grep(a, function(i) { return i != IDs; }, false);

        //If checkbox is checked and the array length is less than 10 then add the selected PlanID to the list
        //Please note that the current plan selection allows maximum of 10 selected plans.
        if (sender.checked) 
        {
            if (a.length < 10) 
                a[a.length] = IDs;
            else 
            {
                sender.checked = false;
                $alert("A maximum of 10 Notes can be selected.", "Followup Notes");
            }
        }
//        else 
//        {
//            if (a.length == 0) 
//            {
//                //sender.checked = true;
//                //$alert("At least one Notes should be selected.", "Followup Notes");
//            }
//        }

        strPlans = a.join(",");

        //Update hidden variable.
        hdnFollowup.val(strPlans);

        updFollowChkSelection();
    }
    function ProdsDiscussChanged(sender, ProdDisID) 
    {
        var tree_ul = $('#spanProducts ul').children();
            
        var t = "";

        if (sender.checked == false) 
        {
            if (tree_ul) 
            {
                var b = "p" + ProdDisID;
                $("#p" + b).remove();
            }
        }
        if (sender.checked == true) 
        {
            if ($('#spanProducts > ul').size() > 0) 
            {
                t = t + "<li id=pp" + ProdDisID + ">" + $("#p" + ProdDisID + " label").text() + "</li>";
                $("#spanProducts ul").append(t);
            }
            else 
            {
                t = "<ul>" + "<li id=pp" + ProdDisID + ">" + $("#p" + ProdDisID + " label").text() + "</li></ul>";
                $("#spanProducts").append(t);
            }
        }

        var strPlans = "";
        var a = [];
        var hdnPrdsDisc = $("#ctl00_main_hdnPrdsDisccused");
        var IDs = ProdDisID;

        if (window.event)
            window.event.cancelBubble = true;

        //Get currently selected list of PoductsDiscussedIDs.
        if (hdnPrdsDisc.length > 0)
            strPlans = hdnPrdsDisc.val();

        //Get list of selected ProductsDiscussedIDs in an array.
        if (strPlans != "")
            a = strPlans.split(",");

        //Remove the current IDs(ProductDiscussedID) from the array.
        a = $.grep(a, function(i) { return i != IDs; }, false);

        //If checkbox is checked and the array length is less than 10 then add the selected ProductsDiscussedIDs to the list
        //Please note that the current plan selection allows maximum of 10 selected plans.
        if (sender.checked) 
        {
            if (a.length < 10) 
                a[a.length] = IDs;
            else 
            {
                sender.checked = false;
                $alert("A maximum of 10 Drugs can be selected.", "Products Discussed");
            }
        }
//        else 
//        {
//            if (a.length == 0) {
//            }
//        }

        strPlans = a.join(",");
        hdnPrdsDisc.val(strPlans);
    }
    
    function setComboText(sender, args) 
    {
        //sender.set_text(" ");
    }
</script>
