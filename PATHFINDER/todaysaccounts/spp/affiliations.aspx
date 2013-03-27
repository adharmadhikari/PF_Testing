<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SingleSection.master" AutoEventWireup="true" CodeFile="affiliations.aspx.cs" Inherits="todaysaccounts_spp_affiliations" %>
<%@ Register src="~/todaysaccounts/controls/AffiliationsListView.ascx" tagname="AffiliationsListView" tagprefix="pinso" %>
<%@ Register src="~/todaysaccounts/controls/AffiliationsListViewScript.ascx" tagname="AffiliationsListViewScript" tagprefix="pinso" %>
<%@ Register src="~/todaysaccounts/controls/AffiliationsToolbar.ascx" tagname="AffiliationsToolbar" tagprefix="pinso" %>

<%--<%@ OutputCache Duration="1" NoStore="true" VaryByParam="none" %>--%>

<%-- Today's Accounts - SPP -  Affiliations --%>


<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="scriptContainer">
    <pinso:AffiliationsListViewScript ID="AffiliationsListViewScript1" runat="server" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Tile3Title" Runat="Server">
    <asp:Literal runat="server" ID="titleText" Text='<%$ Resources:Resource, SectionTitle_PlanAffiliations %>' />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Tile3Tools" Runat="Server">
    <pinso:AffiliationsToolbar runat="server" ID="affilToolbar" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Tile3" Runat="Server">
    <pinso:AffiliationsListView  ID="AffiliationsListView" runat="server" AffiliationType="2" />    
</asp:Content>
