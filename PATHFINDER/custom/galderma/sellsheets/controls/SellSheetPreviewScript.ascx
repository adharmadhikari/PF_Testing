<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SellSheetPreviewScript.ascx.cs" Inherits="custom_controls_SellSheetPreviewScript" %>
<%@ OutputCache Shared="true" VaryByParam="None" Duration="100" %>

<script type="text/javascript">
//    clientManager.add_pageInitialized(ps_pageInitialized);
//    clientManager.add_pageUnloaded(ps_pageUnloaded);

//    function ps_pageInitialized(sender, args)
//    {
//        
//    }

    //Opens modal window for Adding an account in formulary sell sheet.
    function OpenSaveWindow()
    {
        $("#iframeTable").hide();

        var oManager = GetRadWindowManager();

        var clientKey = clientManager.get_ClientKey();

        var q = clientManager.getSelectionDataForPostback();
        str = "custom/" + clientKey + "/sellsheets/all/SaveSellSheet.aspx?" + q;

        var oWnd = radopen(str, "SaveSellSheet");
        oWnd.setSize(400, 200);
        oWnd.Center();
    }
    
//    function ps_pageUnloaded(sender, args)
//    {
//        clientManager.remove_pageInitialized(ps_pageInitialized);
//        clientManager.remove_pageUnloaded(ps_pageUnloaded);
//    }

    function openPDFPreview(sheetID)
    {
        var clientKey = clientManager.get_ClientKey();

        var randomnumber = Math.random() * 101;

        var str = "custom/" + clientKey + "/sellsheets/all/GenerateSellSheetPDF.aspx?Sell_Sheet_ID=" + sheetID + "&rnd=" + randomnumber;

        window.open(str);
    }

</script>

