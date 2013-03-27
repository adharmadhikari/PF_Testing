<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SingleSection.master" AutoEventWireup="true" CodeFile="NewSellSheetOrder.aspx.cs" Inherits="custom_dey_sellsheets_all_NewSellSheetOrder" %>
<%@ Register src="~/custom/dey/sellsheets/controls/SellSheetScript.ascx" tagname="SellSheetScript" tagprefix="pinso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

<script src="https://ajax.microsoft.com/ajax/jquery/jquery-1.4.2.min.js" type="text/javascript" />
<script src="../../../../content/scripts/jquery-ui-1.7.2.custom.min.js" type="text/javascript" />
<script src="../../../../content/scripts/ClientManager-vsdoc.js"  type="text/javascript" />

<pinso:SellSheetScript runat="server"  />
<script type="text/javascript">

    function RefreshGrid()
    {
        return window.top.$find("ctl00_Tile3_SellSheetOrderList1_gridSellSheetOrders");
    }                            
    function RefreshOrders()
    {
        RefreshGrid().get_masterTableView().rebind();
        window.top.$find("ctl00_Tile3_framerep").removeAttr("src");
        
        //OpenOrderDtls();
//       var ddl_rep = window.top.$find("ctl00_Tile3_rdcmbRep");

//        $updateCheckboxDropdownText(ddl_rep, "Region_ID");
//        var url = "custom/Dey/sellsheets/services/DeyDataService.svc" + "/SellSheetTerritorySet?$filter=User_Level eq 1";
//        $.getJSON(url, null, function(result, status)
//        {
//            var d = result.d;
//            $loadPinsoListItems(ddl_rep, d, null, -1);
//        });
//        
//        var ddl_district = window.top.$find("ctl00_Tile3_rdcmbDistrict");
//        var ddl_region = window.top.$find("ctl00_Tile3_rdcmbRegion");
    }
    
</script>
<style type="text/css">
    .rightHeader 
        {
	        float:right;
        }
</style>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="Tile3" Runat="Server">
<asp:HiddenField  ID="Selected_SheetID" runat="server" />
<asp:HiddenField ID="Selected_RepID" runat="server" />
<asp:HiddenField ID="Order_ID" runat="server" />
<asp:HiddenField ID="Modified_OrderID" runat="server" />
<div style="height:60%;">
    <div id="SellSheetOrderHeader">
       <div style="float:right; padding-right:5px; padding-top:5px">
            <asp:LinkButton ID="lb_order" runat="server" Text="Order" onclick="lb_order_Click" />
       </div> 
       <div class="pagination"><%-- This div is used to display paging--%></div>
       <div class="clearAll"></div>
    </div>
    <table width="100%" cellpadding="5" cellspacing="5">
    <tr><td>
    <telerik:RadGrid CssClass="dashboardTable" runat="server"  Width="100%" 
        ID="gridRep" AllowSorting="true" AllowFilteringByColumn="false" EnableEmbeddedSkins="false">
        <MasterTableView AutoGenerateColumns="False" ClientDataKeyNames="ID" ItemStyle-Wrap="true">
        <Columns>
            <telerik:GridBoundColumn DataField="ID" UniqueName="ID" Display="false" />
            <telerik:GridBoundColumn DataField="Name" UniqueName="Name"  
                HeaderText="Sales Rep" DataType="System.String" 
                 HeaderStyle-Width="30%" ItemStyle-Width="30%" ReadOnly="true"/>
            <telerik:GridTemplateColumn UniqueName="Quantity" HeaderText="Quantity"
                HeaderStyle-Width="20%" ItemStyle-Width="20%"  >
               <ItemTemplate>
                    <asp:DropDownList ID="ddl_quantity" runat="server">
                        <asp:ListItem Text="25" Value="25"/>
                        <asp:ListItem Text="50" Value="50" />
                        <asp:ListItem Text="75" Value="75" />
                        <asp:ListItem Text="100" Value="100" />
                     </asp:DropDownList>
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            <telerik:GridBoundColumn DataField="RepFullAddress"  UniqueName="RepFullAddress"
                HeaderText="Address" DataType="System.String" 
                HeaderStyle-Width="40%" ItemStyle-Width="40%" ReadOnly="true"/>
        </Columns>
    </MasterTableView>
        <ClientSettings> 
            <Scrolling AllowScroll="true" UseStaticHeaders="true"/>
            <Selecting AllowRowSelect="true"  /> 
        </ClientSettings>
    </telerik:RadGrid>    
    </td></tr>
    <%--<tr><td align="center">
        <pinso:CustomButton ID="lb_order" runat="server" Text="Order"  onclick="lb_order_Click" />
    </td></tr>--%>
</table>
<asp:SqlDataSource ID="ds_Rep" runat="server" /> 
</div>
</asp:Content>

