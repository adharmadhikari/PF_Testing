<%@ Page Title="" Language="C#" MasterPageFile="~/marketplaceanalytics/MasterPages/filters.master" AutoEventWireup="true" CodeFile="ma_affiliations_filters.aspx.cs" Inherits="marketplaceanalytics_all_affiliations_filters" %>

<%@ Register src="../controls/FilterAffiliation.ascx" tagname="FilterSection" tagprefix="uc2" %>
<%@ Register src="../controls/FilterDrugSelection.ascx" tagname="FilterDrugSelection" tagprefix="uc3" %>
<%@ Register src="../controls/FilterCalendarRolling.ascx" tagname="FilterCalendarRolling" tagprefix="uc4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="filtersContainer" Runat="Server">   
    <uc2:FilterSection ID="AffiliationType" runat="server" />
    <uc3:FilterDrugSelection ID="TheraDrugSelection" runat="server" MaxDrugs="5" />
    <uc4:FilterCalendarRolling ID="CalendarRolling" runat="server" />
</asp:Content>

