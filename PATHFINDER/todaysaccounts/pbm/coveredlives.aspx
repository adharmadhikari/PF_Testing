<%@ Page Title="" Theme="pathfinder" Language="C#" MasterPageFile="~/MasterPages/TriSection.master" AutoEventWireup="true" EnableViewState="true" CodeFile="coveredlives.aspx.cs" Inherits="todaysaccounts_pbm_coveredlives_Section2" %>
<%@ Register src="~/todaysaccounts/controls/CoveredLives.ascx" tagname="CoveredLives" tagprefix="pinso" %>
<%--<%@ Register src="~/todaysaccounts/controls/BenefitDesignComm.ascx" tagname="CommBenefitDesign" tagprefix="pinso" %>
<%@ Register src="~/todaysaccounts/controls/BenefitDesignMedD.ascx" tagname="MedDBenefitDesign" tagprefix="pinso" %>
<%@ Register src="~/todaysaccounts/controls/BenefitDesignComm.ascx" tagname="CommBenefitDesign" tagprefix="pinso" %>--%>
<%@ Register src="~/todaysaccounts/controls/BenefitDesignPBM.ascx" tagname="PBMBenefitDesign" tagprefix="pinso" %>
<%@ Register src="~/todaysaccounts/controls/CoveredLivesScript.ascx" tagname="CoveredLivesScript" tagprefix="pinso" %>
<%@ Register src="~/todaysaccounts/controls/CoveredLivesDrillDown.ascx" tagname="CoveredLivesDrillDown" tagprefix="pinso" %>
<%@ Register src="~/todaysaccounts/controls/FHRIcon.ascx" tagname="FHRIcon" tagprefix="pinso" %>
<%-- Today's Accounts - PBM -  Covered Lives / Benefit Design--%>

<asp:Content ID="Content7" ContentPlaceHolderID="scriptContainer" Runat="Server">
 <pinso:CoveredLivesScript runat="server" ID="coveredLivesScript1" />  
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="Tile3Title" Runat="Server"><asp:Literal runat="server" ID="Literal3" Text='<%$ Resources:Resource, SectionTitle_CoveredLives %>' />
</asp:Content>
<asp:Content ID="Content9" ContentPlaceHolderID="Tile3Tools" Runat="Server">
</asp:Content>
<asp:Content ID="Content10" ContentPlaceHolderID="Tile3" Runat="Server">
     <pinso:CoveredLives ID="CoveredLives1" runat="server" />
</asp:Content>
<asp:Content ID="Content11" ContentPlaceHolderID="Tile4Title" Runat="Server"><asp:Literal runat="server" ID="Literal4" Text='<%$ Resources:Resource, SectionTitle_BenefitDesign %>' />
</asp:Content>
<asp:Content ID="Content12" ContentPlaceHolderID="Tile4Tools" Runat="Server">
</asp:Content>
<asp:Content ID="Content13" ContentPlaceHolderID="Tile4" Runat="Server">
    <pinso:PBMBenefitDesign ID="PBMBenefitDesign1" runat="server" AllowSorting="true"  OnClientRowSelected="gridPBMBenefitDesg_rowclick"/>
    <%--<pinso:CommBenefitDesign ID="CommBenefitDesign1" runat="server" AllowSorting="true"  OnClientRowSelected="gridCommBenefitDesg_rowclick"/>
    <pinso:MedDBenefitDesign ID="MedDBenefitDesign1" runat="server" AllowSorting="true" OnClientRowSelected="gridMedDBenefitDesg_rowclick" />--%>
</asp:Content>
<asp:Content ID="Content14" ContentPlaceHolderID="Tile5Title" Runat="Server"><asp:Literal runat="server" ID="Literal5" Text='Benefit Design - Drug Level' />
</asp:Content>
<asp:Content ID="Content15" ContentPlaceHolderID="Tile5Tools" Runat="Server">
    <pinso:FHRIcon ID="FHRIcon" runat="server" />
    <pinso:TileOptionsMenu runat="server" ID="optionsMenu" UserRole="export"/>   
</asp:Content>
<asp:Content ID="Content16" ContentPlaceHolderID="Tile5" Runat="Server">
    <pinso:CoveredLivesDrillDown ID="CoveredLivesDrillDown1" runat="server"/>
</asp:Content>









