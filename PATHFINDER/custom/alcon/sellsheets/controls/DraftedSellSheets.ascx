<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DraftedSellSheets.ascx.cs" Inherits="custom_controls_DraftedSellSheets" %>







<telerik:RadGrid SkinID="radTable" Width="100%" runat="server" ID="gridDraftedSellSheets" AllowSorting="true"  AllowFilteringByColumn="false" EnableEmbeddedSkins="false">        
    <MasterTableView autogeneratecolumns="false" ClientDataKeyNames="Sell_Sheet_ID,User_ID,Territory_ID,Current_Step" ItemStyle-Wrap="true">
       <Columns>
        <telerik:GridBoundColumn DataField="Sell_Sheet_Name" UniqueName="Sell_Sheet_Name" HeaderText="Sell Sheet Name" HeaderStyle-Width="20%" />
        <telerik:GridBoundColumn DataField="Created_DT" UniqueName="Created_DT" HeaderText="Date Created" ItemStyle-CssClass="alignRight" HeaderStyle-Width="15%" DataFormatString="{0:d}"/>
        <telerik:GridBoundColumn DataField="Modified_DT" UniqueName="Modified_DT" HeaderText="Last Modified" ItemStyle-CssClass="alignRight" HeaderStyle-Width="15%" DataFormatString="{0:d}"/>
        <telerik:GridBoundColumn DataField="Type_Name" UniqueName="Type_Name" HeaderText="Type" HeaderStyle-Width="10%" />
        <telerik:GridBoundColumn DataField="Drug_Name" HeaderText="Drugs" UniqueName="Drug_Name" HeaderStyle-Width="25%" />
        </Columns>  
        <SortExpressions>
            <telerik:GridSortExpression FieldName="Created_DT" SortOrder="Descending"  />   
            <telerik:GridSortExpression FieldName="Modified_DT" SortOrder="Descending"  />   
        </SortExpressions>                             
    </MasterTableView>           
    <ClientSettings ClientEvents-OnRowSelecting="gridDraftedSellSheets_OnRowSelecting">
          <DataBinding Location="~/custom/pinso/sellsheets/services/PathfinderDataService.svc" DataService-TableName="DraftedSellSheetsSet" SelectCountMethod="GetDraftedSellSheetCount"/>
          <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="240px" />
          <Selecting AllowRowSelect="true" /> 
    </ClientSettings>
</telerik:RadGrid> 
<pinso:RadGridWrapper runat="server" ID="radGridWrapper" ShowNumberOfRecords="false" Target="gridDraftedSellSheets" PagingSelector="#DraftedSellSheetsHeader .pagination"  CustomPaging="false"  MergeRows="false" RequiresFilter="false" AutoUpdate="false" AutoLoad="true" 
  UtcDateColumns="Modified_DT, Created_DT" />
