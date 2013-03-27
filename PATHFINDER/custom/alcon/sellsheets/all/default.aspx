<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SingleSection.master" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="custom_Alcon_sellsheets_sellsheets" %>
<%@ Register src="~/custom/Alcon/sellsheets/controls/CompletedSellSheets.ascx" tagname="CompletedSellSheets" tagprefix="pinso" %>
<%@ Register src="~/custom/Alcon/sellsheets/controls/DraftedSellSheets.ascx" tagname="DraftedSellSheets" tagprefix="pinso" %>
<%@ Register src="~/custom/Alcon/sellsheets/controls/SellSheetOrderList.ascx" tagname="SellSheetOrderList" tagprefix="pinso" %>
<%@ Register src="~/custom/Alcon/sellsheets/controls/NewSellSheetOrder.ascx" tagname="NewSellSheetOrder" tagprefix="pinso" %>
<%@ Register src="~/custom/Alcon/sellsheets/controls/SellSheetScript.ascx" tagname="SellSheetScript" tagprefix="pinso" %>

<asp:Content ID="Content2" ContentPlaceHolderID="scriptContainer" Runat="Server">
    <pinso:SellSheetScript runat="server" />
     
    <!--[if IE 7]>
        <style type="text/css">
        #tile3 #divTile3
        {
            padding-right: expression(this.scrollHeight > this.offsetHeight ? 11 : 4) !important;
        }
        .completedSellSheet
        {
            padding-right: 1px;
        }
        .completedSellSheet .tileContainerHeader
        {
            margin-right: -1px;
        }
        
        .previousOrders
        {
            padding-right: 1px;
        }
        .previousOrders .tileContainerHeader
        {
            margin-right: -1px;
        }
        </style>
    <![endif]-->    
    <!--[if IE 6]>
        <style type="text/css">
        #tile3 #divTile3
        {
            padding-right: expression(this.scrollHeight > this.offsetHeight ? 6 : 4) !important;
        }

        </style>
    <![endif]-->    
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="Tile3Title">
    <pinso:CustomButton runat="server" ID="btnCreateNewSellSheet" CssClass="createNewSellSheet" OnClientClick="clientManager.get_ApplicationManager().createSellSheet(); return false;" Text="Create New Sell Sheet" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Tile3" Runat="Server">
<table width="100%">
    <tr>
        <td width="50%" valign="top">
            <div>
                <div class="completedSellSheet" id="completedSellSheetContainer" >                
                    <div id="CompletedSellSheetsHeader" class="tileContainerHeader">
                        <table cellpadding="0" cellspacing="0" border="0" width="100%">
                            <tr>
                                <td nowrap="nowrap" style="padding-left: 5px;">
                                    My Sell Sheets
                                </td> 
                                <td nowrap="nowrap" align="right">
                                    <div class="pagination" >
                                        <%-- This div is used to display paging--%>
                                    </div>
                                </td>
                                <td nowrap="nowrap" style="width: 275px" align="right">
                                    <div class="tools" style="width: 275px">
                                        <span id="separator3" class="pipe">|</span><a id="EditSellSheet" href="javascript:EditCompletedSellSheet();" runat="server">Edit</a>
                                        <span id="separator4" class="pipe">|</span><a id="RemoveSellSheet" href="javascript:OpenRemoveSellSheets('Completed');" runat="server">Remove</a>
                                        <span id="separator5" class="pipe">|</span><a id="EmailSellSheet" href="javascript:openEmailSellSheet();" runat="server">E-mail</a>
                                        <span id="separator6" class="pipe">|</span><a id="ExportSellSheet" href="javascript:openPDFExport();" runat="server" >Preview</a>
                                        <%-- <a id="Preview" href="javascript:OpenPreview();" runat="server" >Preview</a>
                                        <span id="separator7" class="pipe">|</span><a id="OrderPrintsforSellSheet" href="javascript:OpenOrderDtls();" runat="server">Order Prints</a>
                                        <a href="javascript:void(0)" onclick="clientManager.get_ApplicationManager().createSellSheet()" runat="server" id="CreateNewSellSheet">Create New Sell Sheet</a>--%>
                                    </div>                                     
                                </td>                    
                            </tr>
                        </table>  
                    </div>                                          
                    <pinso:CompletedSellSheets ID="CompletedSellSheets1" runat="server"/>
                </div>
             </div>   
        </td>
        

        <td width="50%" valign="top">
            <div>
                <div class="draftedSellSheet">
                    <div id="DraftedSellSheetsHeader" class="tileContainerHeader">
                     <div class="title">Draft Sell Sheets</div>
                        <div class="tools">
                            <span id="separator1" class="pipe" visible="true">|</span><a id="ResumeDraft" href="javascript:OpenDraftSellSheet();" runat="server">Resume</a>
                            <span id="separator2" class="pipe" visible="true">|</span><a id="RemoveDraft" href="javascript:OpenRemoveSellSheets('Drafted');" runat="server">Remove</a>
                        </div> 
                        <div class="pagination">
                        <%-- This div is used to display paging--%>
                        </div>
                        <div class="clearAll"></div>
                    </div>
                <pinso:DraftedSellSheets ID="DraftedSellSheets1" runat="server"/>
                </div>  
            </div>          
        </td>
    </tr>
    
    <tr>
        <td width="50%" valign="top">
            <pinso:SellSheetOrderList ID="SellSheetOrderList1" runat="server"/>
        </td>
        <td width="50%" valign="top">
            <pinso:NewSellSheetOrder ID="NewSellSheetOrder1" runat="server"/>
        </td>
    </tr>
</table>
    

    <div class="clearAll"></div>
</asp:Content>

