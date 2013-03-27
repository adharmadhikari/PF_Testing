
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="OTCCoverageScript.ascx.cs" Inherits="custom_reckitt_otccoverage_controls_OTCCoverageScript" %>
<%@ OutputCache Shared="true" VaryByParam="None" Duration="100" %>


<script type="text/javascript">
    clientManager.add_pageInitialized(ss_pageInitialized);
    clientManager.add_pageUnloaded(ss_pageUnloaded);

    function ss_pageInitialized(sender, args)
    {
        var gd = $find("ctl00_Tile3_OTCCoverageMain1_gridOTCCoverage$GridWrapper"); 
        if (gd) gd.add_recordCountChanged(gridOTCCoverage_onRecordCount);
    }

    function ViewCoverage() 
    {
        //Opens modal window for viewing/updating OTC Coverage for selected plan.
        var oManager = GetRadWindowManager();
        var grid = getOTCGrid();

        var q = clientManager.getSelectionDataForPostback();
        str = "custom/reckitt/otccoverage/all/AddEditCoverage.aspx?LinkClicked=ViewOTC" + q;
        str = str + "&OTCID=" + grid.get_masterTableView().get_selectedItems()[0].get_dataItem().OTC_Coverage_Id;
        
        var oWnd = radopen(str, "ViewOTC");
        oWnd.setSize(805, 550);
        oWnd.Center();
    }

    function OpenAddCoverage()
    {
        //Opens modal window for adding new OTC Coverage.
        var oManager = GetRadWindowManager();

        var q = clientManager.getSelectionDataForPostback();
        str = "custom/reckitt/otccoverage/all/AddEditCoverage.aspx?LinkClicked=AddOTC&" + q;
        
        var oWnd = radopen(str, "AddOTC");
        oWnd.setSize(805, 550);
        oWnd.Center();
    }

    //Gets reference for OTC coverage grid.
    function getOTCGrid() {
        return $find("ctl00_Tile3_OTCCoverageMain1_gridOTCCoverage");
    }

    //When any record is selected from OTC coverage grid enable functionality links.
    function gridOTCCoverage_OnRowSelecting(sender, args)
    {
        $get("ctl00_Tile3Tools_ViewCoverage").style.display = "";
        $get("separator3").style.display = "";
    }

    function gridOTCCoverage_onRecordCount(sender, args)
    {
        var mt = sender.get_masterTableView();

        //If there are no records in OTC Coverage grid then disable functionality links 
        //else enable them and select first record.
        if (mt.get_virtualItemCount() == 0)
        {
            $get("ctl00_Tile3Tools_ViewCoverage").style.display = "none";
            $get("separator3").style.display = "none";
        }
        else
        {
            mt.selectItem(0);
            $get("ctl00_Tile3Tools_ViewCoverage").style.display = "";
            $get("separator3").style.display = "";
        }
    }

    function ss_pageUnloaded(sender, args)
    {
    }
</script>