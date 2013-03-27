<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Pyramid.master" AutoEventWireup="true" CodeFile="formularystatus.aspx.cs" Inherits="standardreports_all_formularystatus" %>
<%@ Register src="../controls/FormularyStatusChart.ascx" tagname="formularystatuschart" tagprefix="pinso" %>
<%@ Register src="../controls/FormularyStatusScript.ascx" tagname="formularystatusscript" tagprefix="pinso" %>
<%@ Register src="../controls/FormularyStatusData.ascx" tagname="formularystatusdata" tagprefix="pinso" %>
<%@ Register src="../controls/FormularyStatusDrillDown.ascx" tagname="formularystatusdrilldown" tagprefix="pinso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="scriptContainer" Runat="Server">    
    <pinso:formularystatusscript ID="formularystatusscript" runat="server" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Tile3Title" Runat="Server">
    <asp:Literal runat="server" ID="lbl1" Text='<%$ Resources:Resource, SectionTitle_FormularyStatus %>' />
</asp:Content>
<asp:Content ID="optionsMenuContent" ContentPlaceHolderID="Tile3Tools" Runat="Server">
    <pinso:TileOptionsMenu runat="server" ID="optionsMenu" UserRole="export"/>
</asp:Content>



<asp:Content ID="Content4" ContentPlaceHolderID="Tile3" runat="Server">
    <div id="divformularystatusChart" style="height:100%;margin-left: 5%;">
        <pinso:formularystatuschart ID="formularystatuschart1" runat="server" Thumbnail="true" />
    </div>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="Tile4Title" Runat="Server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="Tile4Tools" Runat="Server">
 <%--<div id="StdReportPieChart" onclick="javascript:OpenFSPieChartViewer(null,null,500,400);" ></div>--%>
</asp:Content>

<asp:Content ContentPlaceHolderID="Tile4" runat="server">
    <pinso:formularystatusdata ID="gridFDSummary" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="tile5" runat="server">
    <pinso:formularystatusdrilldown ID="gridFSDrilldown" runat="server" />    
</asp:Content>

