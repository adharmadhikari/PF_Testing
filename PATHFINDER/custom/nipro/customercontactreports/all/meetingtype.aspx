<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Pyramid.master" AutoEventWireup="true" CodeFile="meetingtype.aspx.cs" Inherits="custom_pinso_customercontactreports_all_meetingtype" %>
<%@ Register src="../controls/ccrChart.ascx" tagname="ccrChart" tagprefix="pinso" %>
<%@ Register Src="../controls/ccrMeetingTypeData.ascx" tagname="ccrMeetingTypeData" tagprefix="pinso"%>
<%@ Register Src="../controls/ccrMeetingTypeDrillDown.ascx" tagname="ccrMeetingTypeDrillDown" tagprefix="pinso"%>
<%@ Register Src="../controls/ccrMeetingTypeScript.ascx" tagname ="ccrMeetingTypeScript" TagPrefix="pinso" %>

<%-- Customer Contact -  Meeting Type Report --%>

<asp:Content ID="Content1" ContentPlaceHolderID="scriptContainer" Runat="Server">
    <!--[if IE 7]>
        <style type="text/css">
        .customercontactreports #divTile4
        {
            overflow: hidden!important;
        }
        </style>
    <![endif]-->
    <pinso:ccrMeetingTypeScript ID ="ccrMeetingTypeScript1" runat="server" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Tile3Title" Runat="Server">
    <asp:Literal runat="server" ID="lbl1" Text='Customer Contact Meeting Type' /> 
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Tile3Tools" Runat="Server">
    <div class="compareContainer" id="compareContainer" runat="server" visible="false"><a id="compareNational" href="javascript:CompareNational(1);" runat="server" >Compare National</a><span class="comparePipe">|</span></div>
    <pinso:TileOptionsMenu runat="server" ID="optionsMenu" UserRole="export" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Tile3" runat="Server">
    <div id="divCcrMeetingTypeChart" style="height:100%">
        <pinso:ccrChart ID="ccrChart1" runat="server" />     
    </div>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="Tile4Title" Runat="Server">
    Summary View
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="Tile4Tools" Runat="Server">
</asp:Content>

<asp:Content ID="Content7" ContentPlaceHolderID="Tile4" Runat="Server">
    <pinso:ccrMeetingTypeData id="ccrMeetingTypeData1" runat ="server" />
</asp:Content>

<asp:Content ID="Content8" ContentPlaceHolderID="Tile5Title" Runat="Server">
    Detailed View
</asp:Content>

<asp:Content ID="Content9" ContentPlaceHolderID="Tile5Tools" Runat="Server">
</asp:Content>

<asp:Content ID="Content10" ContentPlaceHolderID="Tile5" Runat="Server">
    <pinso:ccrMeetingTypeDrillDown id ="ccrMeetingTypeDrillDown1" runat ="server" />
</asp:Content>

