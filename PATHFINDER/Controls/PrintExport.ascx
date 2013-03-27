<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PrintExport.ascx.cs" Inherits="Controls_PrintExport" %>

<div id="printIcon" style="width: 15px; margin-left: 5px; cursor:pointer;">
    <img id="Img1" alt="Print" runat="server" src="~/App_Themes/pathfinder/images/printIconToolbar.gif" />
</div>

<h1><asp:Literal ID = "ltTitle" runat = "server" Text=""></asp:Literal></h1>
<div class="panelImage"><asp:Panel ID = "pnlImage" runat="server"></asp:Panel></div>
<div class="printContent"><asp:Panel ID = "pnlPrintContent" runat="server"></asp:Panel></div>



