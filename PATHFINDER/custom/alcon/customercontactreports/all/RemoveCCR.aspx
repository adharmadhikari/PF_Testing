<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Modal.master" AutoEventWireup="true" CodeFile="RemoveCCR.aspx.cs" Inherits="custom_Alcon_customercontactreports_all_RemoveCCR" %>
<%@ Register src="../controls/CCR_RemoveCCRScript.ascx" tagname="RemoveCCRScript" tagprefix="pinso" %>
<%@ Register src="../controls/CCR_RemoveCCR.ascx" tagname="RemoveCCR" tagprefix="pinso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <pinso:RemoveCCRScript ID="RemoveCCRScript" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="title" Runat="Server">
  <asp:Label ID="titleText" runat="server" Text="Remove Selected CCR"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="tools" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="main" Runat="Server">
    <pinso:RemoveCCR ID="RemoveCCR" runat="server" />
</asp:Content>