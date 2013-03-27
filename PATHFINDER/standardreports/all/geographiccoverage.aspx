<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/T.master" AutoEventWireup="true" CodeFile="geographiccoverage.aspx.cs" Inherits="standardreports_commercial_geographiccoverage" %>
<%@ Register src="~/standardreports/controls/GeographicCoverageScript_new.ascx" tagname="GeographicCoverageScript" tagprefix="pinso" %>
<%@ Register src="~/standardreports/controls/FormularyStatusScript.ascx" tagname="formularystatusscript" tagprefix="pinso" %>
<%@ Register src="~/standardreports/controls/FormularyStatusChart.ascx" tagname="formularystatuschart" tagprefix="pinso" %>
<%@ Register src="~/standardreports/controls/FormularyStatusDrillDown.ascx" tagname="formularystatusdrilldown" tagprefix="pinso" %>

<%-- Standard Reports - Commercial - Geographic Coverage Report --%>

<asp:Content ID="Content1" ContentPlaceHolderID="scriptContainer" Runat="Server">
    <pinso:FormularyStatusScript runat="server" ID="Script1" />
    <pinso:GeographicCoverageScript ID="GeographicCoverageScript1" runat="server" />    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Tile3Title" Runat="Server">    <%--
    <telerik:RadMenu EnableEmbeddedSkins="false" runat="server" ID="rdlMarketBasketList" SkinID="drugMarket" ClickToOpen="true" />
    <telerik:RadMenu EnableEmbeddedSkins="false"  runat="server" ID="rdlDrugList" SkinID="drugName" ClickToOpen="true" /> --%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Tile3Tools" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Tile3" Runat="Server">        
    <div id="divReportMap" style="height:100%;width:100%"></div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Tile4Title" Runat="Server">    
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="Tile4Tools" Runat="Server">
    <%--<div id="StdReportPieChart" onclick="javascript:OpenFSPieChartViewer(null,null,500,400);"></div>--%>
    <pinso:TileOptionsMenu runat="server" ID="tileOptions" UserRole="export"/>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="Tile4" Runat="Server">
      <div id="divformularystatusChart" style="height:100%;margin-left: 5%;"></div>
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="Tile5Title" Runat="Server">
</asp:Content>
<asp:Content ID="Content9" ContentPlaceHolderID="Tile5Tools" Runat="Server">
 
</asp:Content>
<asp:Content ID="Content10" ContentPlaceHolderID="Tile5" Runat="Server">
    <pinso:formularystatusdrilldown ID="gridFSDrilldown" runat="server" />  
</asp:Content>

