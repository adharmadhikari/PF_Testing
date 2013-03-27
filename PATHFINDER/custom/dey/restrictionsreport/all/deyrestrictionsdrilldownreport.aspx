<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SingleSection.master" AutoEventWireup="true" CodeFile="deyrestrictionsdrilldownreport.aspx.cs" Inherits="restrictionsreport_all_restrictionsreport" %>
<%@ Register src="../controls/RestrictionsReportScript.ascx" tagname="RestrictionsReportScript" tagprefix="pinso" %>
<%@ Register src="../controls/RestrictionsDrillDownReport.ascx" tagname="RestrictionsDrillDownReport" tagprefix="pinso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="scriptContainer" runat="Server">
    <pinso:RestrictionsReportScript ID="RestrictionsReportscript" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Tile3Title" runat="Server">
    <asp:Literal runat="server" ID="lbl1" Text='QL Restrictions Drilldown Report' />
</asp:Content>
<asp:Content ID="optionsMenuContent" ContentPlaceHolderID="Tile3Tools" runat="Server">
    <pinso:TileOptionsMenu runat="server" ID="optionsMenu" Module="deyrestrictionsdrilldownreport" UserRole="export"/>
</asp:Content>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="Tile3">
    <pinso:RestrictionsDrillDownReport ID="gridDrilldown" runat="server" />
</asp:Content>


