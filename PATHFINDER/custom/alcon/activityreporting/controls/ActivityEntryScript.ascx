<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ActivityEntryScript.ascx.cs" Inherits="custom_Alcon_ActivityReporting_controls_ActivityEntryScript" %>
<%@ OutputCache Shared="true" VaryByParam="None" Duration="100" %>


<script type="text/javascript">

    clientManager.add_pageUnloaded(activityentry_pageUnloaded);
    clientManager.add_pageLoaded(activityentry_pageLoaded);
 
    function activityentry_pageLoaded(sender, args)
    {
        $("#ctl00_Tile3_txtActivityDate").datepicker();

        $(".hasDatepicker").keydown(function(event)
        {
            //Prevent backspace and delete          
            //if (event.keyCode == 46 || event.keyCode == 8)
            //{
                event.preventDefault();
            //}
        });

        $(".activityhourstext").keydown(function(event)
        {
            // Allow only backspace and delete         
            if (event.keyCode == 46 || event.keyCode == 8)
            {
                // let it happen, don't do anything
            }
            else if (event.keyCode >= 96 && event.keyCode <= 105) //Keypad
            {
                // let it happen, don't do anything
            }
            else
            {
                // Ensure that it is a number and stop the keypress             
                if (event.keyCode < 48 || event.keyCode > 57)
                {
                    event.preventDefault();
                }
            }
        }); 
    }

    function showGrid(sender)
    {
        var a = setTimeout("if($('#ui-datepicker-div').is(':visible') == false){var data = {}; data['ActivityDate'] = $('#ctl00_Tile3_txtActivityDate')[0].value; clientManager.set_SelectionData(data);}", 500);
    }
    

    function ActivityEntryChanged_NumericOnly(sender,ActivityTypeID,Hours)
    {  
    
    }

    function ActivityEntryChanged(sender, ActivityTypeID, Hours)
    {
        var hr1 = $("#ctl00_Tile3_gvActivityEntry_ctl00_ctl04_txtHours").val();
        var hr2 = $("#ctl00_Tile3_gvActivityEntry_ctl00_ctl06_txtHours").val();
        var hr3 = $("#ctl00_Tile3_gvActivityEntry_ctl00_ctl08_txtHours").val();
        var hr4 = $("#ctl00_Tile3_gvActivityEntry_ctl00_ctl10_txtHours").val();
        var hr5 = $("#ctl00_Tile3_gvActivityEntry_ctl00_ctl12_txtHours").val();
        var hr6 = $("#ctl00_Tile3_gvActivityEntry_ctl00_ctl14_txtHours").val();
        var hr7 = $("#ctl00_Tile3_gvActivityEntry_ctl00_ctl16_txtHours").val();
        var hr8 = $("#ctl00_Tile3_gvActivityEntry_ctl00_ctl18_txtHours").val();
        var hr9 = $("#ctl00_Tile3_gvActivityEntry_ctl00_ctl20_txtHours").val();
        var hr10 = $("#ctl00_Tile3_gvActivityEntry_ctl00_ctl22_txtHours").val();
        var hr11 = $("#ctl00_Tile3_gvActivityEntry_ctl00_ctl24_txtHours").val();
        var hr12 = $("#ctl00_Tile3_gvActivityEntry_ctl00_ctl26_txtHours").val();
        var hr13 = $("#ctl00_Tile3_gvActivityEntry_ctl00_ctl28_txtHours").val();
        var hr14 = $("#ctl00_Tile3_gvActivityEntry_ctl00_ctl30_txtHours").val();

        var hrTotal = parseInt(hr1) + parseInt(hr2) + parseInt(hr3) + parseInt(hr4) + parseInt(hr5) + parseInt(hr6) + parseInt(hr7) + parseInt(hr8) + parseInt(hr9) + parseInt(hr10) + parseInt(hr11) + parseInt(hr12) + parseInt(hr13) + parseInt(hr14);

        if (hrTotal > 24)
        {
            var a = $(sender)[0];
            $(a).val('0');
            $alert("The amount of hours can not exceed 24 hours for a day.", "Activity Entry")
        }
    }

    function activityentry_pageUnloaded(sender, args)
    {
        clientManager.remove_pageLoaded(activityentry_pageLoaded);
        clientManager.remove_pageUnloaded(activityentry_pageUnloaded);
    }

</script>
