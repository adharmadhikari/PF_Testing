<%@ Page Title="" Language="C#" MasterPageFile="../MasterPages/filters.master" AutoEventWireup="true" CodeFile="activityreporting_filters.aspx.cs" Inherits="activityreporting_all_activityreporting_filters" %>
<%@ Register src="../controls/FilterRegion.ascx" tagname="FilterRegion" tagprefix="uc1" %>
<%@ Register src="../controls/filteraccountmanagers.ascx" tagname="FilterAccountManagers" tagprefix="uc2" %>
<%@ Register src="../controls/FilterActivityType.ascx" tagname="FilterActivityType" tagprefix="uc3" %>
<%@ Register src="../controls/FilterTimeFrame.ascx" tagname="FilterTimeFrame" tagprefix="uc4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="scriptContainer" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="filtersContainer" Runat="Server">
<uc1:FilterRegion ID="Region" runat="server" />
<uc2:FilterAccountManagers ID="AccountManager" runat="server" />
<uc3:FilterActivityType ID="ActivityType" runat="server" />
<uc4:FilterTimeFrame ID="TimeFrame" runat="server" />
</asp:Content>

