<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SingleSection.master" AutoEventWireup="true" CodeFile="MillenniumCustomerContactDrillDown.aspx.cs" Inherits="custom_millennium_customercontactreports_all_customcontactdrilldown" %>
<%--<%@ Register src="~/custom/controls/CCRDrilldownScript.ascx" tagname="CDrilldownScript" tagprefix="pinso" %>--%>
<%@ Register src="~/custom/millennium/customercontactreports/controls/CCRDrilldownData.ascx" tagname="CDrilldownData" tagprefix="pinso" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="scriptContainer" Runat="Server">
<%--<pinso:CDrilldownScript ID="cdrilldownscript" runat="server" />--%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Tile3Title" Runat="Server">
    <asp:Literal runat="server" ID="Literal1" Text="Customer Contact Drill Down" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Tile3Tools" Runat="Server">
     <div class="compareContainer" id="compareContainer" runat="server" visible="false"><a id="compareNational" href="javascript:CompareNational(1);" runat="server" >Compare National</a><span class="comparePipe">|</span></div>
    <pinso:TileOptionsMenu runat="server" ID="optionsMenu" ExportConfirm="false" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Tile3" Runat="Server">
    <pinso:CDrilldownData ID="cdrilldowndata" runat="server" />
</asp:Content>

