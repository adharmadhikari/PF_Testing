<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CCR_RemoveBusinessDocumentScript.ascx.cs" Inherits="custom_controls_CCR_RemoveBusinessDocumentScript" %>
<%@ OutputCache Shared="true" VaryByParam="None" Duration="1" %>  
  
<script type="text/javascript">

    //To close the popup window.
    function CloseWin() 
    {
        var manager = window.top.GetRadWindowManager();
        var window1 = manager.getWindowByName("RemoveWnd");
        window1.close();
    }

    function RefreshBusinessDocs() 
    {
        window.top.$find("ctl00_ctl00_Tile3_Tile7_BusinessDocument1_gridCCDocuments").get_masterTableView().rebind();

        window.setTimeout(CloseWin, 4000);
    }
</script>