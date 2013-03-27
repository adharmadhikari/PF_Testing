<%@ Page  EnableViewStateMac="false"  Title="" Theme="pathfinder" Language="C#" MasterPageFile="~/MasterPages/SingleSection.master" AutoEventWireup="true" CodeFile="contacts.aspx.cs" Inherits="custom_millennium_todaysaccounts_rtarecords_contacts" %>
<%@ Register src="~/custom/millennium/todaysaccounts/controls/KDMDetailsRTA.ascx" tagname="KDMDetailsRTA" tagprefix="pinso" %>
<%@ Register src="~/custom/millennium/todaysaccounts/controls/KDMAddressRTA.ascx" tagname="KDMAddressRTA" tagprefix="pinso" %>
<%--<%@ Register src="~/custom/millennium/todaysaccounts/controls/KDMScriptRTA.ascx" tagname="KDMScriptRTA" tagprefix="pinso" %>--%>
<%@ Register src="~/custom/millennium/todaysaccounts/controls/AddEditKDMScript.ascx" tagname="AddEditKDMScript" tagprefix="pinso" %>
<%-- Today's Accounts - VARecords -  Plan Information & Contacts --%>

<asp:Content ContentPlaceHolderID="scriptContainer" runat="server" ID="scriptContainer1">
     <%--<pinso:KDMScriptRTA ID="KDMScriptRTA1" runat="server" /> --%>
     
     <pinso:AddEditKDMScript ID="AddEditKDMScrip1" runat="server" /> 
</asp:Content>



<asp:Content ID="Content1" ContentPlaceHolderID="Tile3Title" Runat="Server">
    <asp:Literal runat="server" ID="titleText3" Text='Key Decision Makers' />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Tile3Tools" Runat="Server">
    <pinso:TileOptionsMenu runat="server" ID="planDetailsTileOptions" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Tile3" Runat="Server">
    <pinso:KDMDetailsRTA ID="KDMDetailsRTA" runat="server" />
    <pinso:KDMAddressRTA ID="KDMAddressRTA" runat="server" />
</asp:Content>



