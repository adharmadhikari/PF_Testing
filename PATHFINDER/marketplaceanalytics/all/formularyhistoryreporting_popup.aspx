<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Popup.master" AutoEventWireup="true" CodeFile="formularyhistoryreporting_popup.aspx.cs" Inherits="marketplaceanalytics_all_formularyhistoryreporting_popup" %>
<%@ Register src="~/marketplaceanalytics/controls/GridTemplateFHR.ascx" tagname="FHRGrid" tagprefix="pinso" %>
<%--<%@ Register src="~/marketplaceanalytics/controls/PrescriberReportScript.ascx" tagname="PrescriberReportScript" tagprefix="pinso" %>
--%>
<asp:Content ID="Content1" ContentPlaceHolderID="scriptContainer" Runat="Server">

<%-- <pinso:PrescriberReportScript ID="PrescriberReportScript" runat="server" /> --%>
<script type="text/javascript">
    function customExport(type, module)
    {
        //custom logic here..
        var data = clientManager.getContextValue("fhQuery"); 
                            
        //window.top.clientManager.exportView(type, false, module, data); //2nd param should false if not using confirmation
        $exportModule(type, true, 2, 0, module, data);
    }
    function setBGColor(color) 
    {
        $('.ui-draggable .tileContainerHeader').css({ 'background-color': color });        
    }
</script>
<style type="text/css">
    .doubleHeader
    {
        border-bottom: solid 1px white!important;
    }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="title" Runat="Server">
<asp:Panel ID="pnlColor" runat="server" Width="100%" Height="100%"><asp:Label ID="popupPlanName" runat="server" Text=""></asp:Label></asp:Panel>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="tools" Runat="Server">
    <pinso:TileOptionsMenu runat="server" ID="optionsMenu" UserRole="export" ContainerID="infoPopup" Module="formularyhistorymarketplacemodal" ExportHandler="window.top.customExport"  />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="main" runat="Server">
    <pinso:FHRGrid runat="server" ID="fhrGrid" />
</asp:Content>
