<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SellSheetOrderList.ascx.cs" Inherits="custom_controls_SellSheetOrderList" %>

 <telerik:RadGrid SkinID="radTable" runat="server" ID="gridSellSheetOrders" 
        AllowSorting="true"  AllowFilteringByColumn="false" EnableEmbeddedSkins="false">        
        <MasterTableView autogeneratecolumns="false" Width="100%">
           <Columns>
            <telerik:GridBoundColumn DataField="OrderDate" UniqueName="OrderDate" HeaderText="Order Date" HeaderStyle-Width="15%" ItemStyle-Width="15%" DataFormatString="{0:d}"/>
            <telerik:GridBoundColumn DataField="Sell_Sheet_Name" UniqueName="Sell_Sheet_Name" HeaderText="Sell Sheet Name" HeaderStyle-Width="20%" ItemStyle-Width="20%" />
            <telerik:GridBoundColumn DataField="Quantity" UniqueName="Quantity" HeaderText="Qty" HeaderStyle-Width="5%" ItemStyle-Width="5%"/>
            <telerik:GridBoundColumn DataField="RepName" UniqueName="RepName" HeaderText="Rep Name" HeaderStyle-Width="30%" ItemStyle-Width="30%"/>
            <telerik:GridBoundColumn DataField="ShipLocation" HeaderText="Ship Location" UniqueName="ShipLocation" />
            </Columns>  
            <SortExpressions>
                <telerik:GridSortExpression FieldName="OrderDate" SortOrder="Descending"  />   
            </SortExpressions>               
        </MasterTableView>           
        <ClientSettings>
              <DataBinding Location="~/custom/dey/sellsheets/services/DeyDataService.svc" DataService-TableName="SellSheetOrdersListSet" />
              <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="250px"/>
              <Selecting AllowRowSelect="true" /> 
        </ClientSettings>
    </telerik:RadGrid>
<pinso:RadGridWrapper runat="server" ID="radGridWrapper" Target="gridSellSheetOrders" ShowNumberOfRecords="false" PagingSelector="#PreviousOrdersHeader .pagination"  CustomPaging="false"  MergeRows="false" RequiresFilter="true" AutoUpdate="false" AutoLoad="false" 
  UtcDateColumns="Created_DT" />
