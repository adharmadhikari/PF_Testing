<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterTrxMst.ascx.cs" Inherits="todaysanalytics_controls_FilterTrxMst" %>
<div id="filterTrxMst">
    <pinso:RadiobuttonValueList ID="Trx_Mst" runat="server" BorderStyle="None" RepeatDirection="Horizontal" >
        <asp:ListItem Text="Trx" Value="Trx" ></asp:ListItem> 
        <asp:ListItem Text="Mst" Value="Mst"></asp:ListItem>                                 
    </pinso:RadiobuttonValueList>
</div>