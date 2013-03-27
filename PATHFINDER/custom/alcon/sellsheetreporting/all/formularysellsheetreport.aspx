<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SingleSection.master" AutoEventWireup="true" CodeFile="formularysellsheetreport.aspx.cs" Inherits="custom_alcon_sellsheets_all_formularysellsheetreport" %>
<%@ OutputCache VaryByParam="None" Duration="1" NoStore="true"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="scriptContainer" Runat="Server">
    <script type="text/javascript">
        function openPDFExport(SellSheetID)
        {
            var clientKey = clientManager.get_ClientKey();
            var randomnumber = Math.random() * 101;
            var str = "custom/" + clientKey + "/sellsheets/all/GenerateSellSheetPDF.aspx?Sell_Sheet_ID=" + SellSheetID + "&rnd=" + randomnumber;
            window.open(str);
        }        
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Tile3Title" Runat="Server">
Formulary Sell Sheets Reporting
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Tile3Tools" Runat="Server">
    <pinso:TileOptionsMenu runat="server" ID="optionsMenu" UserRole="export" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Tile3" Runat="Server">
    <telerik:RadGrid ID="radsellsheetreport" runat ="server" SkinID="radTable" AutoGenerateColumns="false"
        EnableEmbeddedSkins="False" PageSize="50" GridLines="None">        
        <MasterTableView ClientDataKeyNames="Sell_Sheet_ID" AllowSorting="true"  >
            <Columns>               
                <telerik:GridBoundColumn DataField="Thera_Name" SortExpression="Thera_Name" HeaderText="Market Basket" HeaderStyle-Width="16%"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Drug_Name" HeaderText="Product(s)" HeaderStyle-Width="16%"></telerik:GridBoundColumn> 
                <telerik:GridBoundColumn DataField="Created_BY" HeaderText="Person Created" HeaderStyle-Width="16%"></telerik:GridBoundColumn>               
                <telerik:GridBoundColumn DataField="State_ID" SortExpression="State_ID" HeaderText="State" HeaderStyle-Width="16%"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Created_DT" SortExpression="Created_DT" HeaderText="Date Created" DataFormatString="{0:d}" HeaderStyle-Width="16%"></telerik:GridBoundColumn>
                <telerik:GridHyperLinkColumn DataTextField="Sell_Sheet_Name" HeaderText="Sell Sheet Name"
                    DataNavigateUrlFormatString='javascript:openPDFExport({0});' DataNavigateUrlFields="Sell_Sheet_ID" HeaderStyle-Width="20%">
                </telerik:GridHyperLinkColumn>
            </Columns>
        </MasterTableView>
         <ClientSettings >
            <DataBinding Location="../services/AlconService.svc" DataService-TableName="SellSheetReportSet"   />               
            <Scrolling AllowScroll="True" UseStaticHeaders="True"  />            
            <Selecting AllowRowSelect="false" /> 
        </ClientSettings>   
    </telerik:RadGrid>
    <pinso:RadGridWrapper runat="server" ID="radGridWrapper" Target="radsellsheetreport"  MergeRows="false" PagingSelector="#divTile3Container .pagination" 
        RequiresFilter="true" AutoUpdate="true"    UtcDateColumns="Created_DT" />

</asp:Content>

