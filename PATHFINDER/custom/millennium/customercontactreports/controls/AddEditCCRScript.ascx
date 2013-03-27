<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AddEditCCRScript.ascx.cs" Inherits="custom_controls_AddEditCCRScript" %>
<script type="text/javascript">
    $(document).ready(function()
    {
        //Chrome fix
        if (chrome)
            $('.ccrModalContainer').css('padding', '0px');    
    
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
        $("#ctl00_main_formViewCCR_rdlProductsDiscussed_Input, #ctl00_main_formViewCCR_rdlKeyContacts_Input, #ctl00_main_formViewCCR_rdlMeetingOutCome_Input,#ctl00_main_formViewCCR_rdlcontacttitle_Input, #ctl00_main_formViewCCR_rdlFollowUp_Input").keydown(function(e)
        {
            if (e.which == 13 || e.which == 38 || e.which == 40)
                return false;
        });
        
        //Fix possible editing of radComboBox
        $("#ctl00_main_formViewCCR_rdlProductsDiscussed_Input").attr("readOnly", "readonly");
        $("#ctl00_main_formViewCCR_rdlKeyContacts_Input").attr("readOnly", "readonly");
        $("#ctl00_main_formViewCCR_rdlMeetingOutCome_Input").attr("readOnly", "readonly");
        $("#ctl00_main_formViewCCR_rdlcontacttitle_Input").attr("readOnly", "readonly");
        $("#ctl00_main_formViewCCR_rdlFollowUp_Input").attr("readOnly", "readonly");

    });

    function fixDropdowns()
    {
        $("#ctl00_main_formViewCCR_rdlProductsDiscussed table").width($("#ctl00_main_formViewCCR_rdlProductsDiscussed table").width() - 2);
        $("#ctl00_main_formViewCCR_rdlKeyContacts table").width($("#ctl00_main_formViewCCR_rdlKeyContacts table").width() - 2);
        $("#ctl00_main_formViewCCR_rdlMeetingOutCome table").width($("#ctl00_main_formViewCCR_rdlMeetingOutCome table").width() - 2);
        $("#ctl00_main_formViewCCR_rdlcontacttitle table").width($("#ctl00_main_formViewCCR_rdlcontacttitle table").width() - 2);
        $("#ctl00_main_formViewCCR_rdlFollowUp table").width($("#ctl00_main_formViewCCR_rdlFollowUp table").width() - 2);
    }
    
    function ClearForm()
    {
        //Reset the form values.
        document.forms[0].reset();

        $("#ctl00_main_hdnPrdsDisccused").val('');
        $("#ctl00_main_hdnFollowupNotes").val('');
        $("#ctl00_main_hdnMeetOutcome").val('');
        $("#ctl00_main_hdncontacttitle").val('');
        $("#ctl00_main_hdnKeyContacts").val('');

        $("#spanProducts").html('');
        $("#spanFollowup").html('');
        $("#spanMeetOut").html('');
        $("#spancontacttitle").html('');
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
                $("#ctl00_main_formViewCCR_txtFollowupNotesOther").attr("disabled",false);            
            else
                $("#ctl00_main_formViewCCR_txtFollowupNotesOther").attr("disabled", true).val("");
                
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
                $("#ctl00_main_formViewCCR_txtMeetingOutcomeOther").attr("disabled",false);            
            else
                $("#ctl00_main_formViewCCR_txtMeetingOutcomeOther").attr("disabled", true).val("");
            
            t = t + "</ul>"
            $("#spanMeetOut").append(t)
        }
    }
    function updcontacttitleChkSelection() {
        var hdncontacttitle = $("#ctl00_main_hdncontacttitle");

        $("#ctl00_main_formViewCCR_rdlcontacttitle input[type=checkbox]").removeAttr("checked");
        var str = hdncontacttitle.val();
        var t = "<ul>";

        $("#spancontacttitle").html("");

        if (str) {
            var a = str.split(",");
            var hasOther = false;
            for (var i = 0; i < a.length; i++) {
                $("#ctl00_main_formViewCCR_rdlcontacttitle #t" + a[i] + " input").attr("checked", true);

                if ($.browser.msie && $.browser.version < 7)
                    $("#ctl00_main_formViewCCR_rdlcontacttitle #t" + a[i] + " input").attr("defaultChecked", "defaultChecked");
                //$("#m" + a[i] + " input").attr("checked", true);
                t = t + "<li id=tt" + a[i] + ">" + $("#t" + a[i] + " label").text() + "</li>";

                if (a[i] == 100) {
                    hasOther = true;
                }
            }

            if (hasOther)
                $("#ctl00_main_formViewCCR_txtcontacttitleOther").attr("disabled", false);
            else
                $("#ctl00_main_formViewCCR_txtcontacttitleOther").attr("disabled", true).val("");

            t = t + "</ul>"
            $("#spancontacttitle").append(t)
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
           
                a[a.length] = IDs;
           
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
           
                a[a.length] = IDs;
            
        }
        strPlans = a.join(",");

        //Update hidden variable.
        hdnMeetout.val(strPlans);

        updMeetOutcomeChkSelection();

    }
    function ContactTitleChanged(sender, ContactTitleID) {
        var tree_ul = $('#spancontacttitle ul').children();
        var t = "";

        var strPlans = "";
        var a = [];
        var hdncontacttitle = $("#ctl00_main_hdncontacttitle");
        var IDs = ContactTitleID;

        if (window.event)
            window.event.cancelBubble = true;

        //Get currently selected list of ContactTitleID.
        if (hdncontacttitle.length > 0)
            strPlans = hdncontacttitle.val();

        //Get list of selected ContactTitleID in an array.
        if (strPlans != "")
            a = strPlans.split(",");

        //Remove the current IDs(ContactTitleID) from the array.
        a = $.grep(a, function(i) { return i != IDs; }, false);

        //If checkbox is checked and the array length is less than 10 then add the selected ContactTitleID to the list
        //Please note that the current MeetOut selection allows maximum of 10 selected plans.
        if (sender.checked) {
           
                a[a.length] = IDs;
            
        }
        strPlans = a.join(",");

        //Update hidden variable.
        hdncontacttitle.val(strPlans);

        updcontacttitleChkSelection();

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
           
                a[a.length] = IDs;
           
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
    function setProdDiscussedText(sender, args) {
        setTimeout(function()
        {
            var hdnPrdsDisc = $("#ctl00_main_hdnPrdsDisccused");
            var strPlans = "";

            //Get currently selected list of ProductsDiscussedIDs.
            if (hdnPrdsDisc.length > 0)
                strPlans = hdnPrdsDisc.val();

            //If list is not empty then update product dropdown message to 'Change Selection' else '-Select Products Discussed-'.
            if (strPlans != "")
                sender.set_text("-Change Selection-");
            else
                sender.set_text("-Select Products Discussed-");
        }, 500);
        
        
    }

    function setPersonsMetText(sender, args)
    {
        setTimeout(function()
        {
            var hdnKeyContacts = $("#ctl00_main_hdnKeyContacts");
            var strPlans = "";

            //Get currently selected list of personsmetIDs.
            if (hdnKeyContacts.length > 0)
                strPlans = hdnKeyContacts.val();

            //If list is not empty then update personsmet dropdown message to 'Change Selection' else '-Select Persons Met-'.
            if (strPlans != "")
                sender.set_text("-Change Selection-");
            else
                sender.set_text("-Select Attendees-");
        }, 500);
    }
    function setMeetOutcome(sender, args) {
        setTimeout(function()
        {
            var hdnMeetOutcome = $("#ctl00_main_hdnMeetOutcome");
            var strPlans = "";

            //Get currently selected list of MeetingOutcome.
            if (hdnMeetOutcome.length > 0)
                strPlans = hdnMeetOutcome.val();

            //If list is not empty then update MeetingOutcome dropdown message to 'Change Selection' else '-Select Persons Met-'.
            if (strPlans != "")
                sender.set_text("-Change Selection-");
            else
                sender.set_text("-Select Follow-up Topic-");
        }, 500);
                
    }
    function setcontacttitle(sender, args) {
        setTimeout(function()
        {
            var hdncontacttitle = $("#ctl00_main_hdncontacttitle");
            var strPlans = "";

            //Get currently selected list of contacttitle.
            if (hdncontacttitle.length > 0)
                strPlans = hdncontacttitle.val();

            //If list is not empty then update contacttitle dropdown message to 'Change Selection' else '-Select Persons Met-'.
            if (strPlans != "")
                sender.set_text("-Change Selection-");
            else
                sender.set_text("-Select Follow-up Contact Title-");
        }, 500);
    }
    function enablevalidation(sender, args) {
        if (sender.get_text() == "Yes") {

            ValidatorEnable(document.getElementById('ctl00_main_formViewCCR_Requiredfieldvalidator7'), true);
            ValidatorEnable(document.getElementById('ctl00_main_formViewCCR_Requiredfieldvalidator8'), true);

        }
        else {
            ValidatorEnable(document.getElementById('ctl00_main_formViewCCR_Requiredfieldvalidator7'), false);
            ValidatorEnable(document.getElementById('ctl00_main_formViewCCR_Requiredfieldvalidator8'), false);
        }

    }
</script>
