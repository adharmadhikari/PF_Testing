<%@ Page Theme="pathfinder" Title="" Language="C#" MasterPageFile="~/MasterPages/SplitSection.master" AutoEventWireup="true" CodeFile="planinformation.aspx.cs" Inherits="todaysaccounts_fep_planinformation_Section2" %>
<%@ Register src="~/todaysaccounts/controls/PlanInfoDetailsCP.ascx" tagname="PlanInfoDetails" tagprefix="pinso" %>
<%@ Register src="~/todaysaccounts/controls/PlanInfoAddress.ascx" tagname="PlanInfoAddress" tagprefix="pinso" %>


<%-- Today's Accounts - fep -  Plan Information --%>


<asp:Content ID="Content1" ContentPlaceHolderID="Tile3Title" Runat="Server">
    <asp:Literal runat="server" ID="titleText3" Text='<%$ Resources:Resource, SectionTitle_PlanInfo %>' />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Tile3Tools" Runat="Server">
    <pinso:TileOptionsMenu runat="server" ID="planDetailsTileOptions" UserRole="export"/>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Tile3" Runat="Server">
    <pinso:PlanInfoDetails ID="PlanInfoDetails" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Tile4Title" Runat="Server">
    <asp:Literal runat="server" ID="titleText4" Text='<%$ Resources:Resource, SectionTitle_PlanInfoAddress %>' />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Tile4Tools" Runat="Server">
    <pinso:TileOptionsMenu runat="server" ID="planAddressTileOptions" UserRole="export"/>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="Tile4" Runat="Server">
    <pinso:PlanInfoAddress ID="PlanInfoAddress" runat="server" /> 
</asp:Content>


