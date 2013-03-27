<%@ Page Title="" Language="C#" MasterPageFile="~/standardreports/MasterPages/filters.master" AutoEventWireup="true" CodeFile="geographiccoverage_filters.aspx.cs" Inherits="standardreports_all_geographiccoverage_filters" %>
<%@ Register src="../controls/FilterDrugSelection.ascx" tagname="FilterDrugSelection" tagprefix="uc4" %>
<%@ Register src="../controls/FilterRestrictions.ascx" tagname="FilterRestrictions" tagprefix="uc5" %>
<%@ Register Src="../controls/FilterSection.ascx" tagname="FilterChannel" tagprefix="ucSection" %>
<asp:Content ID="Content1" ContentPlaceHolderID="filtersContainer" Runat="Server">   
    <ucSection:FilterChannel ID="Section_ID" runat="server" />
    <uc4:FilterDrugSelection ID="DrugSelection" runat="server" MaxDrugs="1"/>
    <uc5:FilterRestrictions ID="Restrictions" runat="server"  />    
</asp:Content>

