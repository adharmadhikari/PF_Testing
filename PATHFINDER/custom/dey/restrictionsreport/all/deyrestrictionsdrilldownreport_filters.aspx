<%@ Page Title="" Language="C#" MasterPageFile="../MasterPages/filters.master" AutoEventWireup="true" CodeFile="deyrestrictionsdrilldownreport_filters.aspx.cs" Inherits="restrictionsreport_all_restrictionsreport_filters" %>
<%@ Register src="../controls/FilterMarketSegment.ascx" tagname="FilterMarketSegment" tagprefix="uc0" %>
<%@ Register src="../controls/FilterAccountType.ascx" tagname="FilterAccountType" tagprefix="uc1" %>
<%@ Register src="../controls/FilterGeography.ascx" tagname="FilterGeography" tagprefix="uc2" %>
<%@ Register src="../controls/FilterReportType.ascx" tagname="FilterReportType" tagprefix="uc3" %>
<%@ Register src="../controls/FilterDrugSelection.ascx" tagname="FilterDrugSelection" tagprefix="uc4" %>
<%@ Register src="../controls/FilterRestrictions.ascx" tagname="FilterRestrictions" tagprefix="uc6" %>
<asp:Content ID="Content1" ContentPlaceHolderID="filtersContainer" Runat="Server">
    <uc0:FilterMarketSegment ID="FilterMarketSegment" runat="server" />
    <uc1:FilterAccountType ID="AccountType" runat="server" />
    <uc2:FilterGeography ID="Geography" runat="server" />
    <uc3:FilterReportType ID="ReportType" runat="server" />
    <uc4:FilterDrugSelection ID="DrugSelection" runat="server" />
    <uc6:FilterRestrictions ID="Restrictions" runat="server" />

</asp:Content>

