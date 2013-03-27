<%@ Page Title="" Language="C#" MasterPageFile="~/prescriberreporting/MasterPages/filters.master" AutoEventWireup="true" CodeFile="prescribertrending_filters.aspx.cs" Inherits="prescriberreporting_all_prescribertrending_filters" %>
<%@ Register src="../controls/FilterRollup.ascx" tagname="FilterRollup" tagprefix="uc1" %>
<%--<%@ Register src="../controls/FilterChannel.ascx" tagname="FilterChannel" tagprefix="uc2" %>--%>
<%@ Register src="../controls/FilterGeography.ascx" tagname="FilterGeography" tagprefix="uc3" %>
<%@ Register src="../controls/FilterDrugSelection.ascx" tagname="FilterDrugSelection" tagprefix="uc4" %>
<%@ Register src="../controls/FilterCalendarRolling.ascx" tagname="FilterCalendarRolling" tagprefix="uc5" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="scriptContainer" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="filtersContainer" Runat="Server">
<uc1:FilterRollup ID="Rollup" runat="server" />
<%--<uc2:FilterChannel ID="Channel" runat="server" />--%>
<uc3:FilterGeography ID="Geography" runat="server" />
<uc4:FilterDrugSelection ID="TheraDrugSelection" runat="server" MaxDrugs="5" />
<uc5:FilterCalendarRolling ID="CalendarRolling" runat="server" />
</asp:Content>

