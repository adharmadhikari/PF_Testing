<%@ Page Title="" Language="C#" MasterPageFile="~/marketplaceanalytics/MasterPages/filters.master" AutoEventWireup="true" CodeFile="comparison_filters.aspx.cs" Inherits="marketplaceanalytics_all_comparison_filters" %>

<%@ Register src="../controls/FilterRollup.ascx" tagname="FilterRollup" tagprefix="uc1" %>
<%@ Register src="../controls/FilterChannel.ascx" tagname="FilterChannel" tagprefix="uc2" %>
<%@ Register src="../controls/FilterGeography.ascx" tagname="FilterGeography" tagprefix="uc3" %>
<%@ Register src="../controls/FilterDrugSelection.ascx" tagname="FilterDrugSelection" tagprefix="uc4" %>
<%@ Register src="../controls/FilterCalendarRolling.ascx" tagname="FilterCalendarRolling" tagprefix="uc5" %>
<%--<%@ Register src="../controls/FilterAccountManagerWithAll.ascx" tagname="FilterAccountManager" tagprefix="uc6" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="filtersContainer" Runat="Server">
    <uc1:FilterRollup ID="Rollup" runat="server" />
<uc2:FilterChannel ID="Channel" runat="server" />
<%--<uc6:FilterAccountManager ID="FilterAccountManager1" runat="server" />--%>
<uc3:FilterGeography ID="Geography" runat="server" />
<uc4:FilterDrugSelection ID="TheraDrugSelection" runat="server" MaxDrugs="5" />
<uc5:FilterCalendarRolling ID="CalendarRolling" runat="server" />
</asp:Content>

