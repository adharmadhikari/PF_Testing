<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/custom/alcon/sellsheets/PreviewMaster.master" CodeFile="SellSheetCreatePreview.aspx.cs" Inherits="custom_alcon_sellsheets_SellSheetCreatePreview" %>
<%@Register Src="~/custom/alcon/sellsheets/controls/SSPreviewControl1.ascx" TagName="SSPreview" TagPrefix="pinso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="scriptContainer" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Tile3" Runat="Server">
   <%-- <div style="height:1px">
    </div>--%>
    <asp:Panel runat="server" ID="SSPreviewMain">
        <pinso:SSPreview ID="SSPreview1" runat="server"/>
    </asp:Panel>
</asp:Content>