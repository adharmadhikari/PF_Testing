<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterTimeFrameScript.ascx.cs" Inherits="marketplaceanalytics_controls_FilterTimeFrameScript" %>
<%@ OutputCache Shared="true" VaryByParam="None" Duration="120" %>  
  
    <script type="text/javascript">

        clientManager.add_pageLoaded(filterTimeFrame_pageLoaded, "timeframeArea");
        clientManager.add_pageUnloaded(filterTimeFrame_pageUnloaded, "timeframeArea");

        function filterTimeFrame_pageLoaded(sender, args)
        {
            //Completely reset month and quarter container
            clearQuarterMonth();
        
            //Relead container from selection data
            $reloadContainer("timeframeArea", clientManager.get_SelectionData());
            
            //Update text of Drug Selection (above line of code resets selection, below line is needed to restore selection)
            $updateCheckboxDropdownText('ctl00_partialPage_filtersContainer_TheraDrugSelection_Product_ID','DrugIDList');
        
            //Add CSS class to selected options
            $('#timeFrameYearContainer input:checkbox, #timeFrameQuarterContainer input:checkbox, #timeFrameMonthContainer input:checkbox, #timeFrameRollingContainer input:checkbox').each(function()
            {
                var id = $(this).attr('id');
                if ($(this).attr('checked')) $('#timeFrameContainer label[for=' + id + ']').addClass('selectedDateOption');
                $(this).click(function()
                {
                    checkboxCheckStatus(this, false)
                })
            });

            //Get available rolling quarters
            $('#timeFrameRollingContainer label').each(function()
            {
                getAvailableRollingQuarters($(this).text());
            });
            
            //Loads timeframe options if there is selection data
            $('#timeFrameYearContainer input:checkbox').each(function()
            {
                var id = $(this).attr('id');

                if ($(this).attr('checked'))
                {
                    $('#timeFrameContainer label[for=' + id + ']').addClass('selectedDateOption');

                    getAvailableTimeframePerYear($('#timeFrameContainer label[for=' + id + ']').text());

                    $('#timeFrameCalendar .selectAll').show();
                }
            });
            
            //Allows only one year to be checked
            $('#timeFrameYearContainer input:checkbox').click(function()
            {
                $('#timeFrameYearContainer label').not('#timeFrameYearContainer label[for=' + $(this).attr('id') + ']').removeClass('selectedDateOption');
                $('#timeFrameYearContainer input:checkbox:checked').not(this).removeAttr('checked');
                if ($('#timeFrameYearContainer input:checked').length == 0) clearQuarterMonth();
                else $('#timeFrameCalendar .selectAll').show()
            });

//            //Formats the months to break after 6
//            if ($('#timeFrameMonthContainer br').length == 0)
//            {
//                $('#timeFrameMonthContainer span').each(function(idx)
//                {
//                    if (idx == 5) $(this).after('<br/><br/>')
//                });
//            }
            
            //Clear Quarter and Month selection and load available timeframe per year clicked            
            $('#timeFrameYearContainer label').each(function()
            {
                $(this).bind('click', function()
                {
                    clearQuarterMonth();

                    getAvailableTimeframePerYear($(this).text())
                })
            });

            //Check to see if data type is calendar or rolling
            var data = clientManager.get_SelectionData();
            var calendarRolling;

            if (data)
                calendarRolling = data["Calendar_Rolling"];

            //Check if Calendar_Rolling has a value
            if (calendarRolling)
            {
                if (typeof calendarRolling == "object")
                    calendarRolling = calendarRolling.value;
            }

            if ((typeof (calendarRolling) != "undefined")) //If there is selection data
            {
                if (calendarRolling == 'Calendar')
                {
                    $("#timeFrameCalendar").show();
                    $("#timeFrameRolling").hide();
                }
                else
                {
                    $("#timeFrameCalendar").hide();
                    $("#timeFrameRolling").show();
                }
            }
            else //If no value, hide Rolling by default
                $("#timeFrameRolling").hide();

            if (ie7)
            {
                $("#timeFrameYearContainer, #timeFrameQuarterContainer, #timeFrameMonthContainer, #timeFrameRollingContainer").width("100%");
            }     
        }
        
        //Loads available quarters and months based on year selected
        function getAvailableTimeframePerYear(year) 
        {
            for (var key in yrQtr[year])
            {
                var cbxID = $('#timeFrameQuarterContainer label:contains(' + yrQtr[year][key].Name + ')').attr('for');
                $('#' + cbxID).removeAttr('disabled');
                $('#timeFrameQuarterContainer label[for=' + cbxID + ']').removeClass('disabledCheckbox')
            }
            for (var key in yrMth[year])
            {
                var cbxID = $('#timeFrameMonthContainer label:contains(' + yrMth[year][key].Name + ')').attr('for');
                $('#' + cbxID).removeAttr('disabled');
                $('#timeFrameMonthContainer label[for=' + cbxID + ']').removeClass('disabledCheckbox')
            }
        }



        //Loads available rolling quarters 
        function getAvailableRollingQuarters(qtr)
        {
            if (!qtr) return;
            var qtrNum = qtr.replace("Qtr ", "");

            var flag = false;

            for (var a in rollingQtr)
            {
                var id = rollingQtr[a].ID;

                if (id == qtrNum)
                    flag = true;
            }

            if (!flag)
            {
                var cbxID = $("#timeFrameRollingContainer label:contains(" + qtr + ")").attr('for');
                $('#' + cbxID).attr('disabled', 'disabled');                
                $('#timeFrameRollingContainer label[for=' + cbxID + ']').addClass('disabledCheckbox');
            }
        }

        function filterTimeFrame_pageUnloaded(sender, args)
        {
            clientManager.remove_pageLoaded(filterTimeFrame_pageLoaded);
            clientManager.remove_pageUnloaded(filterTimeFrame_pageUnloaded);
        }

        //Check all timeframe options per 'Select All' click for relevant container
        function toggleChecked(status, containerID)
        {
            if (containerID == 'timeFrameQuarterContainer')
            {
                $resetContainer('divMonthContainer');
                $('#timeFrameMonthContainer label').removeClass('selectedDateOption')
            } 
            if (containerID == 'timeFrameMonthContainer')
            {
                $resetContainer('divQuarterContainer');
                $('#timeFrameQuarterContainer label').removeClass('selectedDateOption')
            }
            $('#' + containerID + ' input:checkbox').each(function()
            {
                if (!$(this).attr('disabled'))
                {
                    $(this).attr('checked', status);
                    checkboxCheckStatus(this, true)
                }
            })
        }
        function checkboxCheckStatus(ctrl, allClicked)
        {
            var id = $(ctrl).attr('id');
            if ($('#' + id).attr('checked')) $('#timeFrameContainer label[for=' + id + ']').addClass('selectedDateOption');
            else $('#timeFrameContainer label[for=' + id + ']').removeClass('selectedDateOption');
            if (!allClicked)
            {
                if ($('#' + id).parents('#timeFrameQuarterContainer').length > 0)
                {
                    $('#optionAllQuarter').attr('checked', '');
                    $resetContainer('divMonthContainer');
                    $('#timeFrameMonthContainer label').removeClass('selectedDateOption')
                }
                if ($('#' + id).parents('#timeFrameMonthContainer').length > 0)
                {
                    $('#optionAllMonth').attr('checked', '');
                    $resetContainer('divQuarterContainer');
                    $('#timeFrameQuarterContainer label').removeClass('selectedDateOption')
                }
                if ($('#' + id).parents('#timeFrameRollingContainer').length > 0)
                    $('#optionAllQuarterRolling').attr('checked', '');
            }
        }
        function clearQuarterMonth()
        {
            $resetContainer('divQuarterContainer');
            $resetContainer('divMonthContainer');
            $('#timeFrameMonthContainer :input').attr('disabled', true);
            $('#timeFrameMonthContainer label').addClass('disabledCheckbox').removeClass('selectedDateOption');
            $('#timeFrameQuarterContainer :input').attr('disabled', true);
            $('#timeFrameQuarterContainer label').addClass('disabledCheckbox').removeClass('selectedDateOption');
            $('#timeFrameCalendar .selectAll').hide()
        }             
    </script>