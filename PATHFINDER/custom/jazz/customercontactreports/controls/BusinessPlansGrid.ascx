<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BusinessPlansGrid.ascx.cs" Inherits="custom_controls_BusinessPlansGrid" %>
<div id="businessPlansView">
    <telerik:RadGrid SkinID="radTable" runat="server" ID="gridCCDocuments" AllowSorting="True" EnableEmbeddedSkins="False" PageSize="10" GridLines="None">        
        <MasterTableView autogeneratecolumns="false" ClientDataKeyNames="Document_ID" AllowPaging="true" Width="100%" PageSize="10">
            <Columns>
                <telerik:GridBoundColumn DataField="Document_Date" HeaderText='Document Date' HeaderStyle-Width="50%" SortExpression="Document_Date" UniqueName="Document_Date" DataType ="System.DateTime" DataFormatString="{0:M/dd/yyyy}" /> 
                <telerik:GridBoundColumn DataField="Document_Name" HeaderText='Document Name' HeaderStyle-Width="50%" SortExpression="Document_Name" UniqueName="Document_Name" DataType="System.String"></telerik:GridBoundColumn>
            </Columns>           
            <SortExpressions>
                <telerik:GridSortExpression FieldName="Document_Date" SortOrder="Descending" />
            </SortExpressions>
        </MasterTableView>
        <PagerStyle Visible="false" /> 
        <ClientSettings ClientEvents-OnRowSelecting="gridCCDocuments_OnRowSelecting">
            <DataBinding Location="~/custom/jazz/customercontactreports/services/PathfinderDataService.svc" DataService-TableName="PlanDocumentsViewSet" SelectCountMethod ="GetDocumentCount">
            </DataBinding>
            <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="145px" /> 
            <Selecting AllowRowSelect="true" />
        </ClientSettings>           
    </telerik:RadGrid>
    <pinso:RadGridWrapper runat="server" ID="gridWrapper" Target="gridCCDocuments" PagingSelector="#tile7ContainerHeader .pagination" MergeRows="false"  RequiresFilter ="true" AutoLoad="true" UtcDateColumns="Document_Date" ShowNumberOfRecords="false" />
</div> 
