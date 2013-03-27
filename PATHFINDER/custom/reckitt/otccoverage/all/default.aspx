<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SingleSection.master" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="custom_reckitt_otccoverage_otccoverage" %>
<%@ Register src="~/custom/reckitt/otccoverage/controls/OTCCoverageMain.ascx" tagname="OTCCoverageMain" tagprefix="pinso" %>
<%@ Register src="~/custom/reckitt/otccoverage/controls/OTCCoverageScript.ascx" tagname="OTCCoverageScript" tagprefix="pinso" %>

<asp:Content ID="Content2" ContentPlaceHolderID="scriptContainer" Runat="Server">
    <pinso:OTCCoverageScript ID="OTCCoverageScript1" runat="server"/>
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="Tile3Title">
OTC Coverage
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Tile3Tools">
        <a id="AddCoverage" href="javascript:OpenAddCoverage();" runat="server">Add OTC Coverage</a>
        <span id="separator3">|</span> <a id="ViewCoverage" href="javascript:ViewCoverage();" runat="server">View</a>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Tile3" Runat="Server">
    <pinso:OTCCoverageMain ID="OTCCoverageMain1" runat="server"/>
</asp:Content>

