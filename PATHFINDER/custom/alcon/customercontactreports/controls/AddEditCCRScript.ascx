<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AddEditCCRScript.ascx.cs" Inherits="custom_controls_AddEditCCRScript" %>
<script type="text/javascript">
    $(document).ready(function() {
        //$(".datePicker").datepicker();
        $("#ctl00_main_formViewCCR_rdCCRDate").datepicker();
        $("#ctl00_main_formViewCCR_rdFollowUpDate").datepicker();
       
        $(".RadComboBox .rcbArrowCell A").height(21);

        //Fix IE drop down arrow issue with RadDropList/ItemTemplate
        if ($.browser.msie)
            new cmd(null, fixDropdowns, null, 500);

        //Fix multiple submit issue by disabling enter key
        $("#AddCCRMain").keypress(function(e) {
            if (e.which == 13)
                return false;
        });

        //Disable use of up/down arrows to select items in checkbox dropdowns
        $("#ctl00_main_formViewCCR_rdlProductsDiscussed_Input, #ctl00_main_formViewCCR_rdlKeyContacts_Input, #ctl00_main_formViewCCR_rdlMeetingOutCome_Input, #ctl00_main_formViewCCR_rdlFollowUp_Input").keydown(function(e) {
            if (e.which == 13 || e.which == 38 || e.which == 40)
                return false;
        });

        //Fix possible editing of radComboBox
        $("#ctl00_main_formViewCCR_rdlProductsDiscussed_Input").attr("readOnly", "readonly");
        $("#ctl00_main_formViewCCR_rdlKeyContacts_Input").attr("readOnly", "readonly");
        $("#ctl00_main_formViewCCR_rdlMeetingOutCome_Input").attr("readOnly", "readonly");
        $("#ctl00_main_formViewCCR_rdlFollowUp_Input").attr("readOnly", "readonly");
        $("#ctl00_main_formViewCCR_rdlFollowUp_Input").attr("readOnly", "readonly");
        $("#ctl00_main_formViewCCR_rdlMeetingActivity_Input").attr("readOnly", "readonly");
        

    });

    function fixDropdowns()
    {
        $("#ctl00_main_formViewCCR_rdlProductsDiscussed table").width($("#ctl00_main_formViewCCR_rdlProductsDiscussed table").width() - 2);
        $("#ctl00_main_formViewCCR_rdlKeyContacts table").width($("#ctl00_main_formViewCCR_rdlKeyContacts table").width() - 2);
        $("#ctl00_main_formViewCCR_rdlMeetingOutCome table").width($("#ctl00_main_formViewCCR_rdlMeetingOutCome table").width() - 2);
        $("#ctl00_main_formViewCCR_rdlFollowUp table").width($("#ctl00_main_formViewCCR_rdlFollowUp table").width() - 2);
        $("#ctl00_main_formViewCCR_rdlMeetingActivity table").width($("#ctl00_main_formViewCCR_rdlMeetingActivity table").width() - 2);
    }
    
    function ClearForm()
    {
        //Reset the form values.
        document.forms[0].reset();

        $("#ctl00_main_hdnPrdsDisccused").val('');
        $("#ctl00_main_hdnFollowupNotes").val('');
        $("#ctl00_main_hdnMeetOutcome").val('');
        $("#ctl00_main_hdnKeyContacts").val('');
        $("#ctl00_main_hdnMeetActivity").val('');

        $("#spanProducts").html('');
        $("#spanFollowup").html('');
        $("#spanMeetOut").html('');
        $("#spanKeyContacts").html('');
        $("#spanMeetActivity").html('');

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
                     
                t = t + "<li id=pp" + a[i] + ">" + $("#p" + a[i] + " label").text() + "</li>";
            }
            
            t = t + "</ul>"
            $("#spanProducts").append(t);            
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

        strPlans = a.join("|");

        //Update hidden variable.
        hdnKeyContact.val(strPlans);
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

        strPlans = a.join(",");
        hdnPrdsDisc.val(strPlans);
    }
    
    function setComboText(sender, args) 
    {
        //sender.set_text(" ");
    }


    function setProdDiscussedText(sender, args)
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
    }

    function setPersonsMetText(sender, args)
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
            sender.set_text("-Select Persons Met-");
    }

    function updMeetActivityChkSelection()
    {
        var hdnMeetActivity = $("#ctl00_main_hdnMeetActivity");

        $("#ctl00_main_formViewCCR_rdlMeetingActivity input[type=checkbox]").removeAttr("checked");
        var str = hdnMeetActivity.val();
        var tree_ul = $('#spanMeetActivity ul').children();
        var t = "<ul>";

        if (str)
        {
            var a = str.split(",");
            for (var i = 0; i < a.length; i++)
            {
                var strl = a[i].split(",");
                $("#ctl00_main_formViewCCR_rdlMeetingActivity #y" + strl[0] + " input").attr("checked", true);

                if ($.browser.msie && $.browser.version < 7)
                    $("#ctl00_main_formViewCCR_rdlMeetingActivity #y" + strl[0] + " input").attr("defaultChecked", "defaultChecked");
                
                t = t + "<li id=yy" + strl[0] + ">" + $("#y" + strl[0] + " label").text() + "</li>";
            }

            t = t + "</ul>"
            $("#spanMeetActivity").append(t)
        }

    }

    function setMeetActivityText(sender, args)
    {
        var hdnMeetActivity = $("#ctl00_main_hdnMeetActivity");
        var strids = "";

        //Get currently selected list of Meeting Activity.
        if (hdnMeetActivity.length > 0)
        {
            strids = hdnMeetActivity.val();            
        }

        //If list is not empty then update dropdown message to 'Change Selection' else '-Select Meeting Activity-'.
        if (strids != "")
            sender.set_text("-Change Selection-");
        else
            sender.set_text("-Select Meeting Activity-");
    }
   
    function MeetActivityChanged(sender, MeetActID) {

        disableOtherText("ctl00_main_formViewCCR_txtMeetingActivityOther",MeetActID != 11);
   
        var tree_ul = $('#spanMeetActivity ul').children();

        var t = "";

        if (sender.checked == false)
        {
            if (tree_ul)
            {
                var b = "y" + MeetActID;
                $("#y" + b).remove();
            }
        }
        if (sender.checked == true)
        {
            if ($('#spanMeetActivity > ul').size() > 0)
            {
                t = t + "<li id=yy" + MeetActID + ">" + $("#y" + MeetActID + " label").text() + "</li>";
                $("#spanMeetActivity ul").append(t);
            }
            else
            {
                t = "<ul>" + "<li id=yy" + MeetActID + ">" + $("#y" + MeetActID + " label").text() + "</li></ul>";
                $("#spanMeetActivity").append(t);
            }
        }
        
        var strPlans = "";
        var a = [];
        var hdnMeetActivity = $("#ctl00_main_hdnMeetActivity");
        var IDs = MeetActID;

        if (window.event)
            window.event.cancelBubble = true;

        //Get currently selected list of Meeting Activity IDs.
        if (hdnMeetActivity.length > 0)
            strPlans = hdnMeetActivity.val();

        //Get list of selected Meeting Activity IDs in an array.
        if (strPlans != "")
            a = strPlans.split(",");

        //Remove the current IDs(Meeting Activity IDs) from the array.
        a = $.grep(a, function(i) { return i != IDs; }, false);

        if (sender.checked)
        {
            a[a.length] = IDs;
        }
        strPlans = a.join(",");
        hdnMeetActivity.val(strPlans);
         }
   
//for enabling or disaabling meeting type other field
    function onMeetingTypeChanged(sender, args)
    {
        disableOtherText("ctl00_main_formViewCCR_txtMeetingTypeOther", sender.get_value() != 2);
    }

//for disabling other text
    function disableOtherText(id, disabled)
    {
        if (disabled)
            $("#" + id).attr("readonly", true).val("");
        else
            $("#" + id).attr("readonly", false);
    }    
   
</script>
