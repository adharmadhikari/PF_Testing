<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FiltersContainerScript.ascx.cs" Inherits="formularyhistoryreporting_controls_FiltersContainerScript" %>
<%@ OutputCache Shared="true" VaryByParam="None" Duration="120" %>  
  
    <script type="text/javascript">

        clientManager.add_pageLoaded(filters_pageLoaded, "moduleOptionsContainer");
        clientManager.add_pageUnloaded(filters_pageUnloaded, "moduleOptionsContainer");

        var _filterPageDefaults = [];
        
        function filters_pageLoaded(sender, args) {
            $clearAlert(); //make sure any previous alerts are cleared.

            $addHandler($get("requestReportButton"), "click", requestReport);
            $addHandler($get("clearFiltersButton"), "click", clearReportFilters);

            $reloadContainer("moduleOptionsContainer", clientManager.get_SelectionData());

            standardreports_content_resize();

            $(".datePicker").datepicker();
              
        }

        function filters_pageUnloaded(sender, args)
        {
            $clearHandlers($get("requestReportButton"));
            $clearHandlers($get("clearFiltersButton"));

            clientManager.remove_pageLoaded(filters_pageLoaded, "moduleOptionsContainer");
            clientManager.remove_pageUnloaded(filters_pageUnloaded, "moduleOptionsContainer");
        }
        //request report
       function requestReport()
{

    var data = $getContainerData("filtersContainer");

    if ($get('Plan_ID') && $('#Plan_ID_DATA').val() != "")
    {
        data["Plan_ID"].name = "Plan_ID";
        data["Plan_ID"].value = $('#Plan_ID_DATA').val().split(',');
        data["Plan_ID"].isExtension = true;
    }
    else
        delete data["Plan_ID"];

    if ($get('User_ID') && $('#User_ID_DATA').val() != "")
    {
        data["User_ID"].name = "User_ID";
        data["User_ID"].value = $('#User_ID_DATA').val().split(',');
        data["User_ID"].isExtension = true;

    }
    else
    {
        var start = $('#txtFrom').datepicker('getDate');
        var end = $('#txtTo').datepicker('getDate');

        var diff = new Date(end - start);
        var days = Math.floor(diff / 1000 / 60 / 60 / 24);
        if (days > 30)
        {
            alert("Please enter a time frame limited to a month");
            return;
        }
        delete data["User_ID"];

    }
    data["Status_ID"] = 2;
    if ($validateContainerData("filtersContainer", data, '<%= Resources.Resource.Label_Report_Filters %>'))
    {
        data["__options"] = $getContainerData("optionsContainer");
        clientManager.set_SelectionData(data);
    }
}

        function clearReportFilters() 
        {
            $resetContainer("filterControls");         
        }

        function formatDate(value)
        {
            return value.getMonth() + 1 + "/" + value.getDate() + "/" + value.getYear();
        }
       
    </script>