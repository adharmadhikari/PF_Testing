<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterTimeFrameComparisonScript.ascx.cs" Inherits="marketplaceanalytics_controls_FilterTimeFrameComparisonScript" %>
<%@ OutputCache Shared="true" VaryByParam="None" Duration="120" %>  
  
    <script type="text/javascript">

        clientManager.add_pageLoaded(filterTimeFrameComparison_pageLoaded, "timeframeArea");
        clientManager.add_pageUnloaded(filterTimeFrameComparison_pageUnloaded, "timeframeArea");

        function filterTimeFrameComparison_pageLoaded(sender, args)
        {
            //Get values for month/quarter
            //for (var i = 1; i <= 3; i++)
            for (var i = 1; i <= 2; i++)
            {
                var l = $get('ctl00_partialPage_filterTimeComparisonFrame_filterTimeFrameComparisonSelector' + i + '_MonthQuarterSelection' + i);
                if (l != null)
                {
                    var year = $find('ctl00_partialPage_filterTimeComparisonFrame_filterTimeFrameComparisonSelector' + i + '_Year' + i).get_value();
                    var moYr = $find('ctl00_partialPage_filterTimeComparisonFrame_filterTimeFrameComparisonSelector' + i + '_MonthQuarter' + i);
                    if (moYr)
                    {
                        var moYrVal = moYr.get_value();

                        switch (parseInt(moYrVal, 10))
                        {
                            case 1:
                                $loadListItems(l, yrQtr[year]);
                                break;
                            case 2:
                                $loadListItems(l, yrMth[year]);
                                break
                        }
                    }
                }

                var r = $get('ctl00_partialPage_filterTimeComparisonFrame_filterTimeFrameComparisonSelector' + i + '_RollingQuarterSelection' + i);
                if (r != null)
                {
                    $loadListItems(r, rollingQtrIdName);
                }
            }

            $reloadContainer("timeframeArea", clientManager.get_SelectionData());

            //Update text of Drug Selection (above line of code resets selection, below line is needed to restore selection)
            $updateCheckboxDropdownText('ctl00_partialPage_filtersContainer_TheraDrugSelection_Product_ID', 'DrugIDList');

            if ($("#ctl00_partialPage_filtersContainer_CalendarRolling_Calendar_Rolling_1").attr('checked'))
                showRolling();            
            else
                showCalendar();

        }

        function filterTimeFrameComparison_pageUnloaded(sender, args)
        {
            clientManager.remove_pageLoaded(filterTimeFrameComparison_pageLoaded);
            clientManager.remove_pageUnloaded(filterTimeFrameComparison_pageUnloaded);
        }           
    </script>