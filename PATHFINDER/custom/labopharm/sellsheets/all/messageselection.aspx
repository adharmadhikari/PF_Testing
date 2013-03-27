<%@ Page Title="" Language="C#" EnableViewState="true" MasterPageFile="~/custom/MasterPages/SellSheetStep.master" AutoEventWireup="true" CodeFile="messageselection.aspx.cs" Inherits="custom_pinso_sellsheets_messageselection" %>
<%@ MasterType VirtualPath="~/custom/MasterPages/SellSheetStep.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="scriptContainer" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="StepBody" Runat="Server">
<div id="divIndent">
    <asp:Label runat="server" ID="HeaderMsg" Text="No message has been included in this sell sheet." style="font-weight:bold;"></asp:Label>
    <br /><br />
    <asp:CheckBox runat="server" Checked="true" ID="chkGeo" CssClass="listItemWidth" text="Include Geography Name"/>   
    <br /><br />
    <asp:CheckBox runat="server" Checked="false" ID="chkHighlightProd" CssClass="listItemWidth" text="Highlight Client's Product" visible="false" />   
</div>
        <input type="button" class="btnPrev"
        onclick="clientManager.get_ApplicationManager().back(clientManager)"  
        value="Back" />
    <asp:Button ID="btnNext" runat="server" Text="Next" CssClass="postback validate btnNext" 
        onclick="btnNext_Click" />
</asp:Content>

