<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Popup.master" AutoEventWireup="true" CodeFile="OpenNotes.aspx.cs" Inherits="todaysaccounts_all_OpenNotes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="scriptContainer" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="title" Runat="Server">
    <asp:Literal runat="server" id="titleText" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="tools" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="main" Runat="Server">
  <%-- This page displays Ql Notes for selected Plan and Drug.--%>  
    <div id="main">     
    <table class="genTable" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td><label><asp:Literal runat="server" id="headerText" /></label></td>
        </tr>          
    </table>     

    <table class="genTable" cellpadding="0" cellspacing="0" border="0">
    <tr>
        <td><asp:Literal runat="server" id="notesText" />&nbsp;</td>
    </tr>
    </table>
    </div>
</asp:Content>