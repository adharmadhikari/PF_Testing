<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/TriStack.master" AutoEventWireup="true" CodeFile="coTierCoverageHxFormulary.aspx.cs" Inherits="custom_warner_formularyhistoryreporting_all_coTierCoverageHxFormulary" %>
<%@ Register Src="~/custom/warner/formularyhistoryreporting/controls/Chart.ascx" TagName="chart" TagPrefix="pinso" %>
<%@ Register Src="~/custom/warner/formularyhistoryreporting/controls/ReportScript.ascx" TagName="ReportScript" TagPrefix="pinso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="scriptContainer" Runat="Server">
    <pinso:ReportScript ID="ReportScript" runat="server" DrillDownGridUrl="custom/warner/formularyhistoryreporting/all/detailgrid.aspx" /> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Tile3Title" Runat="Server">
Formulary Changes Impact Analysis
</asp:Content>
<asp:Content ID="optionsMenuContent" ContentPlaceHolderID="Tile3Tools" Runat="Server" >
    <pinso:TileOptionsMenu runat="server" ID="optionsMenu" UserRole="sr_fdx" ExportConfirm="true" />   
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Tile3" Runat="Server">
    <pinso:chart ID="chart" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Tile4Title" Runat="Server">
 Detailed View
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="Tile4Tools" Runat="Server">
 
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="Tile4" Runat="Server">
    <div id="drillDownContainer" class="drillDownContainer">
        
    </div>
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="Tile5Title" Runat="Server">
   
</asp:Content>
<asp:Content ID="Content9" ContentPlaceHolderID="Tile5Tools" Runat="Server">
</asp:Content>
<asp:Content ID="Content10" ContentPlaceHolderID="Tile5" Runat="Server">

</asp:Content>