<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/custom/labopharm/sellsheets/PreviewMaster.master" CodeFile="SellSheetCreatePreview.aspx.cs" Inherits="custom_labopharm_sellsheets_SellSheetCreatePreview" %>
<%@Register Src="~/custom/labopharm/sellsheets/controls/SSPreviewControl1.ascx" TagName="SSPreview" TagPrefix="labopharm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="scriptContainer" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Tile3" Runat="Server">
    <asp:Panel runat="server" ID="SSPreviewMain">
        <labopharm:SSPreview ID="SSPreview1" runat="server"/>
    </asp:Panel>
</asp:Content>