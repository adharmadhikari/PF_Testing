<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SellSheetOrderList.ascx.cs" Inherits="custom_controls_SellSheetOrderList" %>
 <div id="PreviousOrders" class="previousOrders" style="display:none;"  >
 <div id="PreviousOrdersHeader" class="tileContainerHeader">
    <div ID="BDHeader1" class="title" runat="server">Previous Orders</div>
    <div class="tools">
       <a id="ReOrder" href="javascript:PopulateNewOrderSection();" runat="server">Re-Order</a>
    </div> 
     <div class="pagination">
       <%-- This div is used to display paging--%>
    </div>
    <div class="clearAll"></div>
</div> 
 <telerik:RadGrid SkinID="radTable" runat="server" ID="gridSellSheetOrders" AllowSorting="true"  AllowFilteringByColumn="false" EnableEmbeddedSkins="false">        
        <MasterTableView autogeneratecolumns="false" 
            ClientDataKeyNames="Sell_Sheet_ID,Order_ID,User_ID,Territory_ID">
           <Columns>
            <telerik:GridBoundColumn DataField="Created_DT" UniqueName="Created_DT" HeaderText="Order Date" ItemStyle-CssClass="alignRight" HeaderStyle-Width="12%" DataFormatString="{0:d}"/>
            <telerik:GridBoundColumn DataField="Sell_Sheet_Name" UniqueName="Sell_Sheet_Name" HeaderText="Sell Sheet Name" HeaderStyle-Width="50%" />
            <telerik:GridBoundColumn DataField="Sell_Sheet_Copies" UniqueName="Sell_Sheet_Copies" HeaderText="Quantity" HeaderStyle-Width="10%" ItemStyle-CssClass="alignRight"/>
            <telerik:GridBoundColumn DataField="Location_Name" HeaderText="Ship Location" UniqueName="Location_Name" HeaderStyle-Width="13%"/>
            </Columns>  
            <SortExpressions>
                <telerik:GridSortExpression FieldName="Created_DT" SortOrder="Descending"  />   
            </SortExpressions>               
        </MasterTableView>           
        <ClientSettings>
              <DataBinding Location="~/custom/pinso/sellsheets/services/pathfinderdataservice.svc" DataService-TableName="SellSheetOrdersSet" SelectCountMethod="GetSellSheetOrdersCount" />
              <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="150px"/>
              <Selecting AllowRowSelect="true" /> 
        </ClientSettings>
    </telerik:RadGrid>
<pinso:RadGridWrapper runat="server" ID="radGridWrapper" Target="gridSellSheetOrders" ShowNumberOfRecords="false" PagingSelector="#PreviousOrdersHeader .pagination"  CustomPaging="false"  MergeRows="false" RequiresFilter="true" AutoUpdate="false" AutoLoad="false" 
  UtcDateColumns="Created_DT" />
</div>