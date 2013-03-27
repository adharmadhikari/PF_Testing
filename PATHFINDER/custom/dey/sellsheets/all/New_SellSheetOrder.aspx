<%@ Page Language="C#" AutoEventWireup="true" CodeFile="New_SellSheetOrder.aspx.cs" Inherits="custom_dey_sellsheets_all_New_SellSheetOrder" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register src="~/custom/dey/sellsheets/controls/SellSheetScript.ascx" tagname="SellSheetScript" tagprefix="pinso" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
   <link id="link2" runat="server" href="~/content/styles/main.css" rel="Stylesheet" type="text/css" />
   <link id="link1" runat="server" href="~/App_Themes/pathfinder/pathfinder.css" rel="Stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="scriptManager">
        <Scripts>
            <asp:ScriptReference Path="https://ajax.microsoft.com/ajax/jquery/jquery-1.4.2.min.js" />
          <%--  <asp:ScriptReference Path="~/content/scripts/jquery-1.4.1-vsdoc.js" />
            <asp:ScriptReference Path="~/content/scripts/jquery-ui-1.7.2.custom.min.js" />--%>
            <asp:ScriptReference Path="https://ajax.microsoft.com/ajax/jquery.ui/1.8.5/jquery-ui.min.js" />
            <asp:ScriptReference Path="~/content/scripts/ui.js" />
            <asp:ScriptReference Path="~/content/scripts/components.js" />
<%--            <asp:ScriptReference Path="~/content/scripts/clientmanager.js" /> --%>
            <asp:ScriptReference Path="~/content/scripts/ClientManager-vsdoc.js" />            
         </Scripts>
         </asp:ScriptManager>
<asp:HiddenField  ID="Selected_SheetID" runat="server" />
<asp:HiddenField ID="Selected_RepID" runat="server" />
<asp:HiddenField ID="Order_ID" runat="server" />
<asp:HiddenField ID="Modified_OrderID" runat="server" />
<div>
    <div id="SellSheetOrderHeader" class="tileContainerHeader">
       <div style="float:right; padding-right:5px; padding-top:5px">
            <asp:LinkButton ID="lb_order" runat="server" CssClass="title" Text="Order" onclick="lb_order_Click" />
       </div> 
       <div class="pagination"><%-- This div is used to display paging--%></div>
       <div class="clearAll"></div>
    </div>

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
            <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="97px"/>
            <Selecting AllowRowSelect="true"  /> 
        </ClientSettings>
    </telerik:RadGrid>    
<asp:SqlDataSource ID="ds_Rep" runat="server" /> 
</div>
<script type="text/javascript">
//    function RefreshGrid()
//    {
//        return window.top.$find("ctl00_Tile3_SellSheetOrderList1_gridSellSheetOrders");
//    }
    function RefreshOrders()
    {
        //RefreshGrid().get_masterTableView().rebind();
        parent.OpenOrderDtls();
    }
</script>
    </form>
</body>
</html>
