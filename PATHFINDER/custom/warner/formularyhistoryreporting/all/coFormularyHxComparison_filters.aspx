<%@ Page Title="" Language="C#" MasterPageFile="~/custom/pinso/formularyhistoryreporting/MasterPages/filters.master" AutoEventWireup="true" CodeFile="coFormularyHxComparison_filters.aspx.cs" Inherits="formularyhistoryreporting_all_coFormularyHxComparison_filters" %>
<%@ Register src="~/custom/pinso/formularyhistoryreporting/controls/FilterMarketSegment.ascx" tagname="FilterMarketSegment" tagprefix="uc1" %>
<%@ Register src="~/custom/pinso/formularyhistoryreporting/controls/FilterGeography.ascx" tagname="FilterGeography" tagprefix="uc4" %>
<%@ Register src="~/custom/pinso/formularyhistoryreporting/controls/FilterDisplayOptions.ascx" tagname="FilterDisplayOptions" tagprefix="uc2" %>
<%@ Register src="~/custom/pinso/formularyhistoryreporting/controls/FilterDrugSelection.ascx" tagname="FilterDrugSelection" tagprefix="uc3" %>


<asp:Content ID="filtersContainer" ContentPlaceHolderID="filtersContainer" Runat="Server">
<uc1:FilterMarketSegment ID="MarketSegment" runat="server" />

<uc3:FilterDrugSelection ID="DrugSelection" runat="server" />
<uc2:FilterDisplayOptions ID="DisplayOptions" runat="server" />

</asp:Content>

