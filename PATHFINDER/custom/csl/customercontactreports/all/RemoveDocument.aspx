<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Modal.master" AutoEventWireup="true" CodeFile="RemoveDocument.aspx.cs" Inherits="custom_pinso_customercontactreports_all_RemoveDocument" %>
<%@ Register src="../controls/CCR_RemoveBusinessDocumentScript.ascx" tagname="RemoveBusinessDocumentScript" tagprefix="pinso" %>
<%@ Register src="../controls/CCR_RemoveBusinessDocument.ascx" tagname="RemoveBusinessDocument" tagprefix="pinso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <pinso:RemoveBusinessDocumentScript ID="CCR_BusinessDocScript" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="title" Runat="Server">
  <asp:Label ID="titleText" runat="server" Text="Remove Selected Business Document"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="tools" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="main" Runat="Server">
    <pinso:RemoveBusinessDocument ID="RemoveBusinessDocument" runat="server" />
</asp:Content>