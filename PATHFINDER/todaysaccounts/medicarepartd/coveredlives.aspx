<%@ Page Title="" Theme="pathfinder" Language="C#" MasterPageFile="~/MasterPages/TriSection.master" AutoEventWireup="true" EnableViewState="true" CodeFile="coveredlives.aspx.cs" Inherits="todaysaccounts_medicarepartd_coveredlives_Section2" %>
<%@ Register src="~/todaysaccounts/controls/CoveredLives.ascx" tagname="CoveredLives" tagprefix="pinso" %>
<%@ Register src="~/todaysaccounts/controls/BenefitDesignMedD.ascx" tagname="MedDBenefitDesign" tagprefix="pinso" %>
<%@ Register src="~/todaysaccounts/controls/CoveredLivesScript.ascx" tagname="CoveredLivesScript" tagprefix="pinso" %>
<%@ Register src="~/todaysaccounts/controls/CoveredLivesDrillDown.ascx" tagname="CoveredLivesDrillDown" tagprefix="pinso" %>
<%@ Register src="~/todaysaccounts/controls/FHRIcon.ascx" tagname="FHRIcon" tagprefix="pinso" %>
<%-- Today's Accounts - Med-D -  Covered Lives / Benefit Design--%>

<asp:Content ID="Content1" ContentPlaceHolderID="scriptContainer" Runat="Server">
    <pinso:CoveredLivesScript runat="server" ID="coveredLivesMedDScript1" />  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Tile3Title" Runat="Server"><asp:Literal runat="server" ID="Literal3" Text='<%$ Resources:Resource, SectionTitle_CoveredLives %>' /></asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Tile3" Runat="Server">
     <pinso:CoveredLives ID="CoveredLives1" runat="server" ShowTotalCoveredLives="false" ShowPharmLives="false" CoveredLivesEntitySet="CoveredLivesMedDSet" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Tile4Title" Runat="Server">
    <asp:Literal runat="server" ID="Literal4" Text='<%$ Resources:Resource, SectionTitle_BenefitDesign %>' />
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="Tile4" Runat="Server">
    <pinso:MedDBenefitDesign ID="MedDBenefitDesign1" runat="server" AllowSorting="true" OnClientRowSelected="gridMedDBenefitDesg_rowclick" />
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="Tile5Title" Runat="Server">
    <asp:Literal runat="server" ID="Literal5" Text='Benefit Design - Drug Level' />
</asp:Content>
<asp:Content ID="Content9" ContentPlaceHolderID="Tile5Tools" Runat="Server">
    <pinso:FHRIcon ID="FHRIcon" runat="server" />
    <pinso:TileOptionsMenu runat="server" ID="optionsMenu" UserRole="export"/>   
</asp:Content>
<asp:Content ID="Content10" ContentPlaceHolderID="Tile5" Runat="Server">
    <pinso:CoveredLivesDrillDown ID="CoveredLivesDrillDown1" runat="server"/>
</asp:Content>





