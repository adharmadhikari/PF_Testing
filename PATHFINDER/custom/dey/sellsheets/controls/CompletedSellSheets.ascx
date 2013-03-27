<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CompletedSellSheets.ascx.cs" Inherits="custom_controls_SellSheetList" %>


<telerik:RadGrid SkinID="radTable" Width="100%" runat="server" ID="gridCompletedSellSheets" AllowSorting="true"  AllowFilteringByColumn="false" EnableEmbeddedSkins="false">        
    <MasterTableView autogeneratecolumns="false" ClientDataKeyNames="Sell_Sheet_ID,User_ID,Territory_ID,Current_Step">
       <Columns>
        <telerik:GridBoundColumn DataField="Sell_Sheet_Name" UniqueName="Sell_Sheet_Name" HeaderText="Sell Sheet Name" HeaderStyle-Width="25%"  />
        <telerik:GridBoundColumn DataField="Created_DT" UniqueName="Created_DT" HeaderText="Date Created" ItemStyle-CssClass="alignRight" HeaderStyle-Width="18%" DataFormatString="{0:d}"/>
        <telerik:GridBoundColumn DataField="Modified_DT" UniqueName="Modified_DT" HeaderText="Last Modified" ItemStyle-CssClass="alignRight" HeaderStyle-Width="17%" DataFormatString="{0:d}"/>
        <telerik:GridBoundColumn DataField="Type_Name" UniqueName="Type_Name" HeaderText="Type" HeaderStyle-Width="10%" />
        <telerik:GridBoundColumn DataField="Drug_Name" HeaderText="Drugs" UniqueName="Drug_Name" HeaderStyle-Width="25%"/>
        </Columns>  
        <SortExpressions>
            <telerik:GridSortExpression FieldName="Created_DT" SortOrder="Descending"  />   
            <telerik:GridSortExpression FieldName="Modified_DT" SortOrder="Descending"  />   
        </SortExpressions>               
    </MasterTableView>           
    <ClientSettings ClientEvents-OnRowSelecting="gridCompletedSellSheets_OnRowSelecting">    
          <DataBinding Location="~/custom/dey/sellsheets/services/pathfinderdataservice.svc" DataService-TableName="CompletedSellSheetsSet" />
          <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="240px" />
          <Selecting AllowRowSelect="true" /> 
    </ClientSettings>
</telerik:RadGrid>
<pinso:RadGridWrapper runat="server" ID="radGridWrapper" ShowNumberOfRecords="false" 
    Target="gridCompletedSellSheets" PagingSelector="#CompletedSellSheetsHeader .pagination"  
    CustomPaging="false"  MergeRows="false" RequiresFilter="false" AutoUpdate="false" AutoLoad="true"
    UtcDateColumns="Modified_DT, Created_DT"  />
