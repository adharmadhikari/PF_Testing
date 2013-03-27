<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/custom/merz/sellsheets/PreviewMaster.master" CodeFile="SellSheetCreatePreview.aspx.cs" Inherits="custom_merz_sellsheets_SellSheetCreatePreview" %>
<%@Register Src="~/custom/merz/sellsheets/controls/SSPreviewControl1.ascx" TagName="SSPreview" TagPrefix="merz" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="scriptContainer" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Tile3" Runat="Server">
    <asp:Panel runat="server" ID="SSPreviewMain">
        <merz:SSPreview ID="SSPreview1" runat="server"/>
    </asp:Panel>
</asp:Content>