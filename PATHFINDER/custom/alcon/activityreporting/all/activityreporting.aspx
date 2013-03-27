<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Pyramid.master" AutoEventWireup="true" CodeFile="activityreporting.aspx.cs" EnableViewState="true" Inherits="custom_alcon_activityreporting_all_activityreporting" %>
<%@ Register src="../controls/Chart.ascx" tagname="activityReportingChart" tagprefix="pinso" %>
<%@ Register src="../controls/ActivityReportingData.ascx" tagname="activityReportingData" tagprefix="pinso" %>
<%@ Register src="../controls/ActivityReportingDrillDown.ascx" tagname="activityReportingDrillDown" tagprefix="pinso" %>
<%@ Register src="../controls/ActivityReportingScript.ascx" tagname="activityReportingScript" tagprefix="pinso" %>

<%-- Customer Contact -  Meeting Activity Report --%>

<asp:Content ID="Content1" ContentPlaceHolderID="scriptContainer" runat="Server">
    <!--[if IE 7]>
        <style type="text/css">
        .customercontactreports #divTile4
        {
            overflow: hidden!important;
        }
        </style>
    <![endif]-->
    <pinso:activityReportingScript ID="activityReportingScript1" runat="server" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Tile3Title" runat="Server">
    <asp:Literal runat="server" ID="lbl1" Text='Activity Reporting' /> 
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Tile3Tools" Runat="Server">
    <pinso:TileOptionsMenu runat="server" ID="optionsMenu" UserRole="export" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Tile3" runat="Server">
  <div id="divCcrMeetingActivityChart" style="height:100%">
    <pinso:activityReportingChart ID="activityReportingChart1" runat="server" /> 
  </div>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="Tile4Title" Runat="Server">
    Summary View
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="Tile4Tools" Runat="Server">
</asp:Content>

<asp:Content ID="Content7" ContentPlaceHolderID="tile4" runat="server" >
     <pinso:activityReportingData ID="activityReportingData1" runat="server" />
</asp:Content>

<asp:Content ID="Content8" ContentPlaceHolderID="Tile5Title" Runat="Server">
    Detailed View
</asp:Content>

<asp:Content ID="Content9" ContentPlaceHolderID="Tile5Tools" Runat="Server">
</asp:Content>

<asp:Content ContentPlaceHolderID="tile5" runat="server" >
<pinso:activityReportingDrillDown ID="activityReportingDrillDown1" runat="server" /> 
</asp:Content>
