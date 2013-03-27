<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SplitSection.master" AutoEventWireup="true" CodeFile="planinformation.aspx.cs" Inherits="custom_reckitt_todaysaccounts_nhome_planinformation" %>
<%@ Register src="~/custom/reckitt/todaysaccounts/controls/PlanInfoDetails.ascx" tagname="PlanInfoDetails" tagprefix="pinso" %>
<%@ Register src="~/todaysaccounts/controls/MyKeyContactsList.ascx" tagname="MyKeyContactsList" tagprefix="pinso" %>
<%@ Register src="~/todaysaccounts/controls/KeyContactsScript.ascx" tagname="KeyContactsScript" tagprefix="pinso" %>
<%@ Register src="~/todaysaccounts/controls/MyKeyContactToolTipLink.ascx" tagname="MyKeyContactToolTipLink" tagprefix="pinso" %>


<%-- Today's Accounts - Nursing Home -  Plan Information & Contacts --%>

<asp:Content ContentPlaceHolderID="scriptContainer" runat="server" ID="scriptContainer1">
    <pinso:KeyContactsScript ID="KeyContactsScript1" runat="server" />
</asp:Content>


<asp:Content ID="Content1" ContentPlaceHolderID="Tile3Title" Runat="Server">
    <asp:Literal runat="server" ID="titleText3" Text='<%$ Resources:Resource, SectionTitle_PlanInfo %>' />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Tile3Tools" Runat="Server">
    <pinso:TileOptionsMenu runat="server" ID="planDetailsTileOptions" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Tile3" Runat="Server">
    <pinso:PlanInfoDetails ID="PlanInfoDetails" runat="server" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Tile4Title" Runat="Server">
    <asp:Literal runat="server" ID="Literal1" Text='<%$ Resources:Resource, SectionTitle_KeyContacts %>' />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Tile4Tools" Runat="Server">
    <pinso:MyKeyContactToolTipLink runat="server" ID="KeyContactToolTipLink1" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="Tile4" Runat="Server">
    <pinso:MyKeyContactsList runat="server" ID="MyKeyContactsList1" DrillDownLevel="1" />
</asp:Content>
