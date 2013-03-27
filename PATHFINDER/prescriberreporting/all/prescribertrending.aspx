<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/TriStack.master" AutoEventWireup="true" CodeFile="prescribertrending.aspx.cs" Inherits="prescriberreporting_all_prescribertrending" %>
<%@ Register src="~/prescriberreporting/controls/Chart.ascx" tagname="chart" tagprefix="pinso" %>
<%@ Register src="~/prescriberreporting/controls/ReportScript.ascx" tagname="ReportScript" tagprefix="pinso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="scriptContainer" Runat="Server">
    <pinso:ReportScript ID="trendingReportScript" runat="server" DrillDownGridUrl="prescriberreporting/all/detailedgrid.aspx" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Tile3Title" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Tile3Tools" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Tile3" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Tile4Title" Runat="Server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="Tile4Tools" Runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="Tile4" Runat="Server">
    <pinso:chart ID="chart" runat="server" />
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="Tile5Title" Runat="Server">
    Detailed View
</asp:Content>
<asp:Content ID="Content9" ContentPlaceHolderID="Tile5Tools" Runat="Server">
</asp:Content>
<asp:Content ID="Content10" ContentPlaceHolderID="Tile5" Runat="Server">
<div id="drillDownContainer" class="drillDownContainer">
    <%--<pinso:drilldown ID="drilldown" runat="server" />--%>
</div>
</asp:Content>

