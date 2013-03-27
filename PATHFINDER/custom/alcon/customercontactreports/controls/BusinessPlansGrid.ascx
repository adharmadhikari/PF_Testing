<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BusinessPlansGrid.ascx.cs" Inherits="custom_controls_BusinessPlansGrid" %>
<div id="businessPlansView">
    <telerik:RadGrid SkinID="radTable" runat="server" ID="gridCCDocuments" AllowSorting="True" EnableEmbeddedSkins="False" PageSize="10" GridLines="None">        
        <MasterTableView autogeneratecolumns="false" ClientDataKeyNames="Document_ID" AllowPaging="true" Width="100%" PageSize="10">
            <Columns>
                <telerik:GridBoundColumn DataField="Contact_Date" HeaderText='Document Date' HeaderStyle-Width="30%" SortExpression="Contact_Date" UniqueName="Contact_Date" DataType ="System.DateTime" DataFormatString="{0:M/dd/yyyy}" /> 
                <telerik:GridBoundColumn DataField="Document_Name" HeaderText='Document Name' HeaderStyle-Width="35%" SortExpression="Document_Name" UniqueName="Document_Name" DataType="System.String"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Document_Type_Name" HeaderText='Document Type' HeaderStyle-Width="35%" SortExpression="Document_Type_Name" UniqueName="Document_Type_Name" DataType="System.String"></telerik:GridBoundColumn>
            </Columns>           
            <SortExpressions>
                <telerik:GridSortExpression FieldName="Contact_Date" SortOrder="Descending" />
            </SortExpressions>
        </MasterTableView>
        <PagerStyle Visible="false" /> 
        <ClientSettings ClientEvents-OnRowSelecting="gridCCDocuments_OnRowSelecting">
            <DataBinding Location="~/custom/Alcon/customercontactreports/services/AlconDataService.svc" DataService-TableName="PlanDocumentsViewSet" SelectCountMethod ="GetDocumentCount">
            </DataBinding>
            <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="145px" /> 
            <Selecting AllowRowSelect="true" />
        </ClientSettings>           
    </telerik:RadGrid>
    <pinso:RadGridWrapper runat="server" ID="gridWrapper" Target="gridCCDocuments" PagingSelector="#tile7ContainerHeader .pagination" MergeRows="false"  RequiresFilter ="true" AutoLoad="true" UtcDateColumns="Document_Date" ShowNumberOfRecords="false" />
</div> 
