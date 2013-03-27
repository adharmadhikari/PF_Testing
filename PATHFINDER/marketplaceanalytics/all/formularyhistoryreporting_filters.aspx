<%@ Page Title="" Language="C#" MasterPageFile="~/marketplaceanalytics/MasterPages/filters.master" AutoEventWireup="true" CodeFile="formularyhistoryreporting_filters.aspx.cs" Inherits="marketplaceanalytics_all_formularyhistoryreporting_filters" %>
<%@ Register src="../controls/FilterDrugSelection.ascx" tagname="FilterDrugSelection" tagprefix="dsel" %>
<%@ Register src="../controls/FilterChannelPlans.ascx" tagname="FilterChannelPlanSelection" tagprefix="cp" %>
<%@ Register src="../controls/FilterFHRTimeFrame.ascx" tagname="FilterTimeframe" tagprefix="tf" %>
<%@ Register src="../controls/FilterMonthQuarter.ascx" tagname="FilterMonthQuarter" tagprefix="mq" %>
<asp:Content ID="Content1" ContentPlaceHolderID="filtersContainer" Runat="Server">
    <cp:FilterChannelPlanSelection ID="FilterChannelPlanSelection" runat="server"/>
    <dsel:FilterDrugSelection ID="TheraDrugSelection" runat="server" MaxDrugs="5" />
    <mq:FilterMonthQuarter ID="FilterMonthQuarter" runat="server" />
    <tf:FilterTimeframe ID="FilterTimeframe" runat="server" />
</asp:Content>

