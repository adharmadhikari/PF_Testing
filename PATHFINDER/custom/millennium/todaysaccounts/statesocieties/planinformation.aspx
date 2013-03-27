<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SingleSection.master" AutoEventWireup="true" CodeFile="planinformation.aspx.cs" Inherits="custom_millennium_todaysaccounts_statesocieties_planinformation" %>
<%@ Register src="~/custom/millennium/todaysaccounts/controls/PlanInfoDetailsSS.ascx" tagname="PlanInfoDetails" tagprefix="pinso" %>
<%@ Register src="~/custom/millennium/todaysaccounts/controls/AddEditPlanInfoScript.ascx" tagname="PlanInfoScript" tagprefix="pinso" %>
<%@ OutputCache VaryByParam="None" Duration="1" NoStore="true" %>
<%-- Today's Accounts - Medicare Jurisdiction -  Plan Information & Contacts --%>
<asp:Content ContentPlaceHolderID="scriptContainer" runat="server" ID="scriptContainer1">
    <pinso:PlanInfoScript ID="PlanInfoScript1" runat="server" />
</asp:Content>



<asp:Content ID="Content1" ContentPlaceHolderID="Tile3Title" Runat="Server">
    <asp:Literal runat="server" ID="titleText3" Text='<%$ Resources:Resource, SectionTitle_PlanInfo %>' />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Tile3Tools" Runat="Server">
 <table><tr> 
    
    <td><pinso:TileOptionsMenu runat="server" ID="planDetailsTileOptions" /></td>
    <td><a id="Edit" href="javascript:OpenSSPlanInfo('EditPlan','')">Edit</a></td>
    </tr></table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Tile3" Runat="Server">
    <pinso:PlanInfoDetails ID="PlanInfoDetails" runat="server" />
</asp:Content>




