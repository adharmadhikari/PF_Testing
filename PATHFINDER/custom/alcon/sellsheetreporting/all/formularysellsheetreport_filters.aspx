<%@ Page Title="" Language="C#" MasterPageFile="../MasterPages/filters.master" AutoEventWireup="true" CodeFile="formularysellsheetreport_filters.aspx.cs" Inherits="custom_alcon_sellsheets_all_formularysellsheetreport_filters" %>
<%@ Register src="../controls/FilterMarketSegment.ascx" tagname="FilterMarketSegment" tagprefix="uc1" %>
<%@ Register src="../controls/FilterDrugSelection.ascx" tagname="FilterDrugSelection" tagprefix="uc3" %>
<%@ Register src="../controls/FilterTimeFrame.ascx" tagname="FilterTimeFrame" tagprefix="uc5" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="scriptContainer" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="filtersContainer" Runat="Server">
    <uc1:FilterMarketSegment ID="MarketSegment" runat="server" DefaultValue="1" />   
    <uc3:FilterDrugSelection ID="DrugSelection" runat="server" />
    <uc5:FilterTimeFrame ID="TimeFrame" runat="server" />
</asp:Content>
