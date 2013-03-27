<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Pyramid.master" AutoEventWireup="true" CodeFile="meetingactivity.aspx.cs" EnableViewState="true" Inherits="custom_gsk_customercontactreports_all_meetingactivity" %>
<%@ Register src="~/custom/gsk/customercontactreports/controls/ccrChart.ascx" tagname="ccrChart" tagprefix="pinso" %>
<%@ Register src="~/custom/gsk/customercontactreports/controls/ccrMeetingActivityData.ascx" tagname="ccrMeetingActivityData" tagprefix="pinso" %>
<%@ Register src="~/custom/gsk/customercontactreports/controls/ccrMeetingActivityDrillDown.ascx" tagname="ccrMeetingActivityDrillDown" tagprefix="pinso" %>
<%@ Register src="~/custom/gsk/customercontactreports/controls/ccrMeetingActivityScript.ascx" tagname="ccrMeetingActivityScript" tagprefix="pinso" %>

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
    <pinso:ccrMeetingActivityScript ID="ccrMeetingActivityScript1" runat="server" /> 
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Tile3Title" runat="Server">
    <asp:Literal runat="server" ID="lbl1" Text='Customer Contact Meeting Activity' /> 
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Tile3Tools" Runat="Server">
    <div class="compareContainer" id="compareContainer" runat="server" visible="false"><a id="compareNational" href="javascript:CompareNational(1);" runat="server" >Compare National</a><span class="comparePipe">|</span></div>
    <pinso:TileOptionsMenu runat="server" ID="optionsMenu" UserRole="export" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Tile3" runat="Server">
  <div id="divCcrMeetingActivityChart" style="height:100%">
    <pinso:ccrChart ID="ccrChart1" runat="server" />     
  </div>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="Tile4Title" Runat="Server">
    Summary View
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="Tile4Tools" Runat="Server">
</asp:Content>

<asp:Content ID="Content7" ContentPlaceHolderID="tile4" runat="server" >
     <pinso:ccrMeetingActivityData ID="ccrMeetingActivityData1" runat="server" />  
</asp:Content>

<asp:Content ID="Content8" ContentPlaceHolderID="Tile5Title" Runat="Server">
    Detailed View
</asp:Content>

<asp:Content ID="Content9" ContentPlaceHolderID="Tile5Tools" Runat="Server">
</asp:Content>

<asp:Content ContentPlaceHolderID="tile5" runat="server" >
   <pinso:ccrMeetingActivityDrillDown ID="ccrMeetingActivityDrillDown1" runat="server" />  
</asp:Content>
