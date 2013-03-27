<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SingleSection.master" AutoEventWireup="true" CodeFile="custom_geographiccoverageFAM.aspx.cs" EnableViewState="true" Inherits="custom_millenium_executivereports_all_custom_geographiccoverageFAM" %>

<%--<%@ Register src="~/custom/millennium/executivereports/controls/GeographicCoverageScript.ascx" tagname="GeographicCoverageScript" tagprefix="pinso" %>
<%@ Register src="~/custom/millennium/executivereports/controls/GeographicCoverageFormularyStatusDrillDown.ascx" tagname="formularystatusdrilldown" tagprefix="pinso" %>
--%>
<%-- Executive Reports - Geographic Coverage FAM --%>
<asp:Content ID="Content2" ContentPlaceHolderID="Tile3Title" Runat="Server">    
    Geographic Coverage Report By FAM
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Tile3Tools" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Tile3" Runat="Server"> 
    <iframe id="reportviewerframe" src="custom/millennium/executivereports/all/ReportViewer.aspx?reportname=MillenniumExecutiveReports&report=GeographicCoverageFam"  frameborder="0" width="100%" height="100%"></iframe>       
</asp:Content>

<%-- This used T.master
    <asp:Content ID="Content1" ContentPlaceHolderID="scriptContainer" Runat="Server">
    <pinso:GeographicCoverageScript ID="GeographicCoverageScript1" runat="server" />  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Tile3Title" Runat="Server">    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Tile3Tools" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Tile3" Runat="Server">        
    <div id="divReportMap" style="height:100%;width:100%"></div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Tile4Title" Runat="Server">    
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="Tile4Tools" Runat="Server">
    <pinso:TileOptionsMenu runat="server" ID="tileOptions" UserRole="export"/>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="Tile4" Runat="Server">
    <div id="divformularystatusChart" style="height:100%"></div>
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="Tile5Title" Runat="Server">
</asp:Content>
<asp:Content ID="Content9" ContentPlaceHolderID="Tile5Tools" Runat="Server">
</asp:Content>
<asp:Content ID="Content10" ContentPlaceHolderID="Tile5" Runat="Server">
    <pinso:formularystatusdrilldown ID="gridFSDrilldown" runat="server" />  
</asp:Content>--%>