<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Pyramid.master" AutoEventWireup="true" CodeFile="deyrestrictionsreport.aspx.cs" Inherits="restrictionsreport_all_restrictionsreport" %>
<%@ Register src="../controls/RestrictionsReportChart.ascx" tagname="RestrictionsReportChart" tagprefix="pinso" %>
<%@ Register src="../controls/RestrictionsReportScript.ascx" tagname="RestrictionsReportScript" tagprefix="pinso" %>
<%@ Register src="../controls/RestrictionsReportData.ascx" tagname="RestrictionsReportData" tagprefix="pinso" %>
<%@ Register src="../controls/RestrictionsReportDrillDown.ascx" tagname="RestrictionsReportDrillDown" tagprefix="pinso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="scriptContainer" runat="Server">
    <pinso:RestrictionsReportScript ID="RestrictionsReportscript" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Tile3Title" runat="Server">
    <asp:Literal runat="server" ID="lbl1" Text='QL Restriction Report' />
</asp:Content>
<asp:Content ID="optionsMenuContent" ContentPlaceHolderID="Tile3Tools" runat="Server">
    <pinso:TileOptionsMenu runat="server" ID="optionsMenu" Module="deyrestrictionsreport" UserRole="export"/>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Tile3" Runat="Server">
    <div id="divtiercoverageChart" style="height:90%">
          <pinso:RestrictionsReportChart ID="RestrictionsReportChart1" runat="server" />    
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Tile4" runat="server"> 
    <pinso:RestrictionsReportData ID="RestrictionsReportData1" runat="server" />   
</asp:Content>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="tile5">
    <pinso:RestrictionsReportDrillDown ID="gridDrilldown" runat="server" />
</asp:Content>


