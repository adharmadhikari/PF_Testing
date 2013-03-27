<%@ Page  EnableViewStateMac="false"  Title="" Theme="pathfinder" Language="C#" MasterPageFile="~/MasterPages/SplitSectionHybrid.master" AutoEventWireup="true" CodeFile="contacts.aspx.cs" Inherits="todaysaccounts_fep_contacts_Section2" %>
<%@ Register src="~/todaysaccounts/controls/KeyContactsList.ascx" tagname="KeyContactsList" tagprefix="pinso" %>
<%@ Register src="~/todaysaccounts/controls/MyKeyContactsList.ascx" tagname="MyKeyContactsList" tagprefix="pinso" %>
<%@ Register src="~/todaysaccounts/controls/KeyContactsScript.ascx" tagname="KeyContactsScript" tagprefix="pinso" %>
<%@ Register src="~/todaysaccounts/controls/KeyContactToolTipLink.ascx" tagname="KeyContactToolTipLink" tagprefix="pinso" %>
<%@ Register src="~/todaysaccounts/controls/MyKeyContactToolTipLink.ascx" tagname="MyKeyContactToolTipLink" tagprefix="pinso" %>

<%-- Today's Accounts fep Contacts --%>
<asp:Content ContentPlaceHolderID="scriptContainer" runat="server" ID="scriptContainer1">
    <pinso:KeyContactsScript runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="Tile3Title" Runat="Server">
    <asp:Literal runat="server" ID="titleText" Text='<%$ Resources:Resource, SectionTitle_KeyContacts %>' />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Tile3Tools" Runat="Server">
    <pinso:KeyContactToolTipLink runat="server" ID="tooltipLink" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Tile3" Runat="Server">

    <pinso:KeyContactsList ID="KeyContactsList1" runat="server" />
    
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Tile4Title" Runat="Server">
    <asp:Literal runat="server" ID="Literal1" Text='<%$ Resources:Resource, SectionTitle_MyKeyContacts %>' />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Tile4Tools" Runat="Server">
    <pinso:TileOptionsMenu runat="server" ID="optionsMenu" UserRole="export" Module="mykeycontacts"  ExportConfirm="false" Channel="0" />     
    <pinso:MyKeyContactToolTipLink runat="server" ID="KeyContactToolTipLink1" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="Tile4" Runat="Server">
    <pinso:MyKeyContactsList runat="server" ID="MyKeyContactsList1" />
</asp:Content>


