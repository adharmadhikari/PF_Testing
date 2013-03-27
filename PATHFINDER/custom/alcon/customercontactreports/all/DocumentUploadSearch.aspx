<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/SingleSection.master" CodeFile="DocumentUploadSearch.aspx.cs" Inherits="custom_Alcon_customercontactreports_All_DocumentUploadSearch" %>
<%@ Register src="../controls/documentuploadsearch.ascx" tagname="DocumentUploadSearch" tagprefix="pinso" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="scriptContainer" Runat="Server">
<%--<pinso:CDrilldownScript ID="cdrilldownscript" runat="server" />--%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Tile3Title" Runat="Server">
    <asp:Literal runat="server" ID="Literal1" Text="Document Upload Search" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Tile3Tools" Runat="Server">
    <pinso:TileOptionsMenu runat="server" ID="optionsMenu" UserRole="export" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Tile3" Runat="Server">
    <pinso:DocumentUploadSearch ID="DocumentUploadSearch" runat="server" />
</asp:Content>
