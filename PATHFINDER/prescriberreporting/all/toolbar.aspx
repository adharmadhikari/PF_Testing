<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/PartialPage.master" AutoEventWireup="true" CodeFile="toolbar.aspx.cs" Inherits="prescriberreporting_all_toolbar" %>
<%@ Register src="~/prescriberreporting/controls/FilterTrxMst.ascx" tagname="FilterTrxMst" tagprefix="pinso" %>
<%@ Register src="~/prescriberreporting/controls/toolbarScript.ascx" tagname="ToolbarScript" tagprefix="pinso" %>
<%@ OutputCache NoStore="true" Duration="1" VaryByParam="None" %>

<asp:Content ID="Content2" ContentPlaceHolderID="scriptContainer" Runat="Server">
    <pinso:ToolbarScript ID="toolbarScript" runat="server" />  
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="partialPage" Runat="Server">
    <div class="dataDate"><asp:Literal runat="server" ID="litDataDate" /></div>
    <div id="toolbarContainer">    
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <div id="refreshSelection" title="Refresh Selection"/>      
            </td>
            <td>
                <pinso:FilterTrxMst ID="filterTrxMst" runat="server" />
            </td>            
            <td>
                <div id="exportExcel" title="Excel Export" onclick="runExport('excel');"/>      
            </td>
            <td>
                <div id="print" title="Print" onclick="runExport('print');"/>      
            </td>
        </tr>
    </table>
    </div>
    <div class="clearAll"></div>
</asp:Content>

