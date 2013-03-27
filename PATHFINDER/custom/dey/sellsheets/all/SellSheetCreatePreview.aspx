<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/custom/dey/sellsheets/PreviewMaster.master" CodeFile="SellSheetCreatePreview.aspx.cs" Inherits="custom_dey_sellsheets_SellSheetCreatePreview" %>
<%@Register Src="~/custom/dey/sellsheets/controls/SSPreviewControl1.ascx" TagName="SSPreview" TagPrefix="pinso" %>
<%@Register Src="~/custom/dey/sellsheets/controls/SSPreviewControl1MedD.ascx" TagName="SSPreviewMedD" TagPrefix="pinso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="scriptContainer" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Tile3" Runat="Server">
    <asp:Panel runat="server" ID="SSPreviewMain">
        <pinso:SSPreview ID="SSPreview1" runat="server"/>
        <pinso:SSPreviewMedD ID="SSPreview1MedD" runat="server" />
    </asp:Panel>
</asp:Content>