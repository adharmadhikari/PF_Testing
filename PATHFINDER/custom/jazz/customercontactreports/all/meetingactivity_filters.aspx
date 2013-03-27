<%@ Page Title="" Language="C#" MasterPageFile="~/custom/jazz/customercontactreports/MasterPages/filters.master" AutoEventWireup="true" CodeFile="meetingactivity_filters.aspx.cs" Inherits="custom_jazz_customercontactreports_all_meetingactivity_filters" %>

<%@ Register src="~/custom/jazz/customercontactreports/controls/FilterRegion.ascx"tagname="FilterRegion" tagprefix="pinso" %>
<%@ Register src="~/custom/jazz/customercontactreports/controls/filteraccountmanagers.ascx" tagname="filteraccountmanagers" tagprefix="pinso" %>
<%@ Register src="~/custom/jazz/customercontactreports/controls/FilterMarketSegment.ascx" tagname="FilterMarketSegment" tagprefix="pinso" %>
<%@ Register src="~/custom/jazz/customercontactreports/controls/FilterAccount.ascx" tagname="FilterAccount" tagprefix="pinso" %>
<%@ Register src="~/custom/jazz/customercontactreports/controls/FilterMeetingType.ascx" tagname="FilterMeetingType" tagprefix="pinso" %>
<%@ Register src="~/custom/jazz/customercontactreports/controls/FilterTimeFrame.ascx" tagname="FilterTimeFrame" tagprefix="pinso" %>
<%@ Register src="~/custom/jazz/customercontactreports/controls/FilterProducts.ascx" tagname="FilterProducts" tagprefix="pinso" %>


<asp:Content ID="filtersContainer" ContentPlaceHolderID="filtersContainer" Runat="Server">
    <pinso:filteraccountmanagers ID="filteraccountmanagers" runat="server" />
    <pinso:FilterMarketSegment ID="FilterMarketSegment" runat="server" />
    <pinso:FilterAccount ID="FilterAccount" runat="server" />
    <pinso:FilterMeetingType ID="FilterMeetingType" runat="server" />
    <pinso:FilterTimeFrame ID="FilterTimeFrame" runat="server" />
    <pinso:FilterProducts ID="FilterProducts" runat="server" />    
</asp:Content>
