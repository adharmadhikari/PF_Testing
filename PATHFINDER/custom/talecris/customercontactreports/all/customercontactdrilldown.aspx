<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SingleSection.master" AutoEventWireup="true" CodeFile="customercontactdrilldown.aspx.cs" Inherits="custom_pinso_customercontactreports_all_customcontactdrilldown" %>
<%--<%@ Register src="~/custom/controls/CCRDrilldownScript.ascx" tagname="CDrilldownScript" tagprefix="pinso" %>--%>
<%@ Register src="../controls/CCRDrilldownData.ascx" tagname="CDrilldownData" tagprefix="pinso" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="scriptContainer" Runat="Server">
<%--<pinso:CDrilldownScript ID="cdrilldownscript" runat="server" />--%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Tile3Title" Runat="Server">
    <asp:Literal runat="server" ID="Literal1" Text="Customer Contact Report Drill Down" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Tile3Tools" Runat="Server">
    <pinso:TileOptionsMenu runat="server" ID="TileOptionsMenu1"  />  
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Tile3" Runat="Server">
    <pinso:CDrilldownData ID="cdrilldowndata" runat="server" />
</asp:Content>

