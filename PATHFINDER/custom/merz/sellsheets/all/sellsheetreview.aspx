<%@ Page Title="" Language="C#" MasterPageFile="~/custom/MasterPages/SellSheetStep.master" AutoEventWireup="true" CodeFile="sellsheetreview.aspx.cs" Inherits="custom_pinso_sellsheets_sellsheetreview" %>
<%@ Register src="~/custom/merz/sellsheets/controls/SellSheetPreviewScript.ascx" tagname="SellSheetPreviewScript" tagprefix="merz" %>

<asp:Content ID="Content1" ContentPlaceHolderID="scriptContainer" Runat="Server">
    <merz:SellSheetPreviewScript ID="SellSheetPreviewScript1" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="StepBody" Runat="Server">
    <center>
        <table border="0" cellpadding="0" cellspacing="0" id="iframeTable" class="iframeTable">
            <tr >
                <td class="pdfLoader">
                    <asp:Literal runat="server" ID="ssPreview"></asp:Literal>
                </td>
                <td align="left" valign="top">
                    <asp:Literal runat="server" ID="litExport"></asp:Literal>
                </td>
            </tr>
        </table>        
    </center>
    
    <table width="100%" >
        <tr>
            <td align="right" >
                <input type="button" class="btnPrev" onclick='clientManager.setContextValue("ssSelectedPlansList"); clientManager.get_ApplicationManager().back(clientManager);'  value="Back" />
                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btnNext"  OnClientClick="javascript:OpenSaveWindow();return false;" />
            </td>
        </tr>
    </table>     
</asp:Content>

