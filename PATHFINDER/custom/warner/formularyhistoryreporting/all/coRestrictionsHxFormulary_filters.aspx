<%@ Page Title="" Language="C#" MasterPageFile="~/custom/warner/formularyhistoryreporting/MasterPages/filters.master" AutoEventWireup="true" CodeFile="coRestrictionsHxFormulary_filters.aspx.cs" Inherits="custom_warner_formularyhistoryreporting_coRestrictionsHxFormulary_filters" %>

<%@ Register src="~/custom/warner/formularyhistoryreporting/controls/FilterMarketSegment.ascx" tagname="FilterMarketSegment" tagprefix="uc1" %>
<%@ Register Src="~/custom/warner/formularyhistoryreporting/controls/FilterDrugSelection.ascx" TagName="FilterDrugSelection" TagPrefix="uc3" %>

<asp:Content ID="filtersContainer" ContentPlaceHolderID="filtersContainer" Runat="Server">
    <uc1:FilterMarketSegment ID="MarketSegment" runat="server" />
    <uc3:FilterDrugSelection ID="DrugSelection" runat="server" />
</asp:Content>
