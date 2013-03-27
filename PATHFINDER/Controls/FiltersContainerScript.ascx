<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FiltersContainerScript.ascx.cs" Inherits="Controls_FiltersContainerScript" %>
<%@ OutputCache Shared="true" VaryByParam="None" Duration="120" %>  
  
    <script type="text/javascript">

        clientManager.add_pageLoaded(filters_pageLoaded, "moduleOptionsContainer");
        clientManager.add_pageUnloaded(filters_pageUnloaded, "moduleOptionsContainer");

        var _filterPageDefaults = [];
        
        function filters_pageLoaded(sender, args)
        {
            $clearAlert(); //make sure any previous alerts are cleared.
            
            $addHandler($get("requestReportButton"), "click", requestReport);
            $addHandler($get("clearFiltersButton"), "click", clearReportFilters);

            $reloadContainer("moduleOptionsContainer", sender.get_SelectionData());

            sender.get_ApplicationManager().resize();
            
            //            truncateMenu();            
        }

        function filters_pageUnloaded(sender, args)
        {
            $removeHandler($get("requestReportButton"), "click", requestReport);
            $removeHandler($get("clearFiltersButton"), "click", clearReportFilters);

            sender.remove_pageLoaded(filters_pageLoaded, "moduleOptionsContainer");
            sender.remove_pageUnloaded(filters_pageUnloaded, "moduleOptionsContainer");
        }

        function requestReport()
        {
//            $hideAlert();
            
            var data = $getContainerData("filtersContainer");

            if ($validateContainerData("filtersContainer", data, '<%= Resources.Resource.Label_Report_Filters %>'))
            {
                //Always sending Channel if selection is allowed
                if(clientManager.get_ChannelMenu().get_visible())
                    data["Section_ID"] = clientManager.get_EffectiveChannel();
                    
                data["__options"] = $getContainerData("optionsContainer");

                clientManager.set_SelectionData(data);
            }
        }

        function clearReportFilters()
        {
            $resetContainer("filterControls");
        }
                
    </script>