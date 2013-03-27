<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Pyramid.master" AutoEventWireup="true" CodeFile="productsdiscussed.aspx.cs" Inherits="custom_reckitt_customercontactreports_all_productsdiscussed" %>
<%@ Register src="~/custom/reckitt/customercontactreports/controls/ccrChart.ascx" tagname="ccrChart" tagprefix="pinso" %>
<%@ Register src="~/custom/reckitt/customercontactreports/controls/ccrProductsDiscussedData.ascx" tagname="ccrProductsDiscussedData" tagprefix="pinso" %>
<%@ Register src="~/custom/reckitt/customercontactreports/controls/ccrProductsDiscussedDrillDown.ascx" tagname="ccrProductsDiscussedDrillDown" tagprefix="pinso" %>
<%@ Register src="~/custom/reckitt/customercontactreports/controls/ccrProductsDiscussedScript.ascx" tagname="ccrProductsDiscussedScript" tagprefix="pinso" %>

<%-- Customer Contact -  Products Discussed Report --%>

<asp:Content ID="Content1" ContentPlaceHolderID="scriptContainer" Runat="Server">
    <pinso:ccrProductsDiscussedScript ID="ccrProductsDiscussedScript1" runat="server" /> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Tile3Title" Runat="Server">
    <asp:Literal runat="server" ID="lbl1" Text='Customer Contact Products Discussed' /> 
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Tile3Tools" Runat="Server">
    <pinso:TileOptionsMenu runat="server" ID="optionsMenu" ExportConfirm="false" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Tile3" Runat="Server">
    <div id="divCcrProductsDiscussedChart" style="height:100%">
        <pinso:ccrChart ID="ccrChart1" runat="server" />     
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Tile4Title" Runat="Server">
    Summary View
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="Tile4Tools" Runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="Tile4" Runat="Server">
    <pinso:ccrProductsDiscussedData ID="ccrProductsDiscussedData1" runat="server" />  
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="Tile5Title" Runat="Server">
    Detailed View
</asp:Content>
<asp:Content ID="Content9" ContentPlaceHolderID="Tile5Tools" Runat="Server">
</asp:Content>
<asp:Content ID="Content10" ContentPlaceHolderID="Tile5" Runat="Server">
   <pinso:ccrProductsDiscussedDrillDown ID="ccrProductsDiscussedDrillDown1" runat="server" />  
</asp:Content>

