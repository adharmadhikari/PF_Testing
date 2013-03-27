<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Popup.master" AutoEventWireup="true" CodeFile="formularyhistoryreporting_popup.aspx.cs" Inherits="todaysaccounts_all_formularyhistoryreporting_popup" %>
<%@ Register src="~/todaysaccounts/controls/GridTemplateFHR.ascx" tagname="FHRGrid" tagprefix="pinso" %>
<asp:Content ID="Content1" ContentPlaceHolderID="scriptContainer" Runat="Server">
<script type="text/javascript">
    function customExport(type, module)
    {
        var data = clientManager.getContextValue("ta_fhQuery");

        //window.top.clientManager.exportView(type, false, module, data); //2nd param should false if not using confirmation
        $exportModule(type, true, 1, 0, module, data);
    }
</script>
<style type="text/css">
    .doubleHeader
    {
        border-bottom: solid 1px white!important;
    }
    #infoPopup .tools
    {
        padding-bottom: 0px!important;
        padding-top: 0px!important;
    }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="title" Runat="Server">
<label style="margin-right:50px">Benefit Design - Drug Level (Historical Comparison)</label><label ID="planName" runat="server"></label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="tools" Runat="Server">
    <pinso:TileOptionsMenu runat="server" ID="optionsMenu" UserRole="export" ContainerID="infoPopup" Module="formularyhistoryreporting_ta" ExportHandler="window.top.customExport"  />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="main" runat="Server">
    <div class="tileContainerHeader" style="margin-top:1px" ><div class="title"><label ID="formularyName" runat="server" style="margin-right:50px;" ></label><label ID="marketBasketName" runat="server"></label></div></div>
    <pinso:FHRGrid runat="server" ID="fhrGrid" />
</asp:Content>
