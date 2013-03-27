<%@ Page Title="" Language="C#" MasterPageFile="~/standardreports/MasterPages/filters.master" AutoEventWireup="true" CodeFile="livesdistribution_filters.aspx.cs" Inherits="standardreports_all_livesdistribution_filters" %>
<%@ Register src="../controls/FilterAccountType.ascx" tagname="FilterAccountType" tagprefix="uc1" %>
<%@ Register src="../controls/FilterGeography.ascx" tagname="FilterGeography" tagprefix="uc2" %>
<%@ Register Src="../controls/FilterSection.ascx" tagname="FilterChannel" tagprefix="ucSection" %>

<asp:Content ID="Content1" ContentPlaceHolderID="filtersContainer" Runat="Server">
    <ucSection:FilterChannel ID="Section_ID" runat="server"/>
    <uc1:FilterAccountType ID="AccountType" runat="server" />
    <uc2:FilterGeography ID="Geography" runat="server" />
</asp:Content>

