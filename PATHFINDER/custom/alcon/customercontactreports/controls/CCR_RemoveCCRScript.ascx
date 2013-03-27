<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CCR_RemoveCCRScript.ascx.cs" Inherits="custom_Alcon_customercontactreports_controls_CCR_RemoveCCRScript" %>
<%@ OutputCache Shared="true" VaryByParam="None" Duration="1" %>  
  
<script type="text/javascript">

    //To close the popup window.
    function CloseWin() 
    {
        var manager = window.top.GetRadWindowManager();
        var window1 = manager.getWindowByName("RemoveWnd");
        window1.close();
    }

     function RefreshCCR() 
    {
        window.top.$find("ctl00_ctl00_Tile3_Tile6_CCRGridList1_gridCCReports").get_masterTableView().rebind();

        window.setTimeout(CloseWin, 4000);
    }
</script>