<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SingleSection.master" AutoEventWireup="true" CodeFile="activityreportingdrilldown.aspx.cs" Inherits="custom_alcon_activityreporting_all_activityreportingdrilldown" %>
<%@ Register src="~/custom/alcon/activityreporting/controls/ActivityReportingDrilldownData.ascx" tagname="ADrilldownData" tagprefix="pinso" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="scriptContainer" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Tile3Title" Runat="Server">
    <asp:Literal runat="server" ID="Literal1" Text="Activity Reporting Drilldown" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Tile3Tools" Runat="Server">
    <pinso:TileOptionsMenu runat="server" ID="optionsMenu" UserRole="export" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Tile3" Runat="Server">
    <pinso:ADrilldownData ID="adrilldowndata" runat="server" />
</asp:Content>

