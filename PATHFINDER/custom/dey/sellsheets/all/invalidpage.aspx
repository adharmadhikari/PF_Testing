<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SingleSectionNoHeader.master" AutoEventWireup="true" CodeFile="invalidpage.aspx.cs" Inherits="custom_unitedthera_sellsheets_invalidpage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="scriptContainer" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Tile3" Runat="Server">
    Invalid Selection<br />
    <input type="button" onclick="clientManager.get_ApplicationManager().back(clientManager)"  value="Back" />
</asp:Content>

