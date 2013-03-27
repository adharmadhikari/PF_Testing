<%@ Page Title="" Language="C#" MasterPageFile="~/custom/millennium/customercontactreports/MasterPages/filters.master" AutoEventWireup="true" CodeFile="meetingtopic_filters.aspx.cs" Inherits="custom_millennium_customercontactreports_all_meetingactivity_filters" %>

<%@ Register src="~/custom/millennium/customercontactreports/controls/FilterRegion.ascx"tagname="FilterRegion" tagprefix="pinso" %>
<%@ Register src="~/custom/millennium/customercontactreports/controls/filteraccountmanagers.ascx" tagname="filteraccountmanagers" tagprefix="pinso" %>
<%@ Register src="~/custom/millennium/customercontactreports/controls/FilterMarketSegment.ascx" tagname="FilterMarketSegment" tagprefix="pinso" %>
<%@ Register src="~/custom/millennium/customercontactreports/controls/FilterAccount.ascx" tagname="FilterAccount" tagprefix="pinso" %>
<%@ Register src="~/custom/millennium/customercontactreports/controls/FilterMeetingType.ascx" tagname="FilterMeetingType" tagprefix="pinso" %>
<%@ Register src="~/custom/millennium/customercontactreports/controls/FilterTimeFrame.ascx" tagname="FilterTimeFrame" tagprefix="pinso" %>
<%@ Register src="~/custom/millennium/customercontactreports/controls/FilterProducts.ascx" tagname="FilterProducts" tagprefix="pinso" %>


<asp:Content ID="filtersContainer" ContentPlaceHolderID="filtersContainer" Runat="Server">
    <pinso:FilterRegion ID="FilterRegion" runat="server" />
    <pinso:filteraccountmanagers ID="filteraccountmanagers" runat="server" />
    <pinso:FilterMarketSegment ID="FilterMarketSegment" runat="server" />
    <pinso:FilterAccount ID="FilterAccount" runat="server" />
    <pinso:FilterMeetingType ID="FilterMeetingType" runat="server" />
    <pinso:FilterTimeFrame ID="FilterTimeFrame" runat="server" />
    <pinso:FilterProducts ID="FilterProducts" runat="server" />    
</asp:Content>
