<%@ Control Language="C#" AutoEventWireup="true" CodeFile="fhrScript.ascx.cs" Inherits="custom_pinso_formularyhistoryreporting_controls_fhrScript" %>

<script type="text/javascript">
    var DrillDownGridUrl = '<%= DrillDownGridUrl %>';
    $(document).ready(function()
    {
        ShowHideOptions();
    });
    clientManager.add_pageInitialized(fhrPageInitialized,'fhrGridContainer');
    clientManager.add_pageUnloaded(fhrPageUnloaded,'fhrGridContainer');

    function fhrPageInitialized() 
    {        
        clientManager.registerComponent('ctl00_Tile3_fhrGridContainer', null);      
        
    }
    function fhrPageUnloaded() 
    {
        clientManager.remove_pageInitialized(fhrPageInitialized,'fhrGridContainer');
        clientManager.remove_pageUnloaded(fhrPageUnloaded,'fhrGridContainer');
    }
    
    function RefreshPage(Url, PageAction, ContainerID) {
        var data = clientManager.get_SelectionData();
        var pageIndex;
        if ($get('formularycomparison_lblPageIndex'))
            pageIndex = parseInt($get('formularycomparison_lblPageIndex').outerText);
        else
            pageIndex = 1;
        if (PageAction == "Next") {
            data["Next"] = "true";
            data["Previous"] = "false";
            pageIndex = pageIndex + 1;
        }

        if (PageAction == "Previous") {
            data["Previous"] = "true";
            data["Next"] = "false";
            pageIndex = pageIndex - 1;
        }
        data["StartPage"] = pageIndex;
        clientManager.loadPage(Url + "?" + $getDataForPostback(data), ContainerID);

    }
    //for pagination
    function setGridPage(grid, page, totalCount) {
        var grid = $find("fhrGridContainer");
        var data = clientManager.cleanSelectionData(clientManager.get_SelectionData());

        grid.set_pageNumber(page + 1);
        grid.set_params(data);
        grid.dataBind();

        var pageContainer = grid.get_pageContainer();
        $(pageContainer).html(grid.constructPager(grid, totalCount));

        var data = clientManager.get_SelectionData();
    }
    
    //for showing or hiding options menu. if drug id is selected, show it else hide it
    function ShowHideOptions()
    {
        var data = clientManager.get_SelectionData();
        if (data["Drug_ID"])
        {
            document.getElementById('ctl00_Tile3Tools_optionsMenu_tileOptionsMenu').style.visibility = 'visible';
        }
        else
        {
            document.getElementById('ctl00_Tile3Tools_optionsMenu_tileOptionsMenu').style.visibility = 'hidden';
            $('.pagination').hide();
        }
    }


    
</script>