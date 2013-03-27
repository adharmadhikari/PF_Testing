<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CCRGridView.ascx.cs" Inherits="custom_controls_CCRGridView" %>
<div id="ccrView">
    <telerik:RadGrid SkinID="radTable" runat="server" ID="gridCCReports" AllowSorting="True" EnableEmbeddedSkins="False" PageSize="50">        
        <MasterTableView autogeneratecolumns="false" ClientDataKeyNames="Contact_Report_ID, Plan_ID" AllowPaging="true" Width="100%" PageSize="50">
            <Columns>
                <telerik:GridBoundColumn DataField="Contact_Date" HeaderText='<%$ Resources:Resource, Label_CR_Date %>' HeaderStyle-Width="15%" SortExpression="Contact_Date" UniqueName="Contact_Date" DataType ="System.DateTime" DataFormatString="{0:M/dd/yyyy}" /> 
                <telerik:GridBoundColumn DataField="Meeting_Activity_Name" HeaderText='<%$ Resources:Resource, Label_Meeting_Activity %>' HeaderStyle-Width="25%" SortExpression="Meeting_Activity_Name" UniqueName="Meeting_Activity_Name"  DataType ="System.String"/>          
                <telerik:GridBoundColumn DataField="Meeting_Type_Name" HeaderText='<%$ Resources:Resource, Label_Meeting_Type %>' HeaderStyle-Width="25%" SortExpression="Meeting_Type_Name" UniqueName="Meeting_Type_Name" DataType = "System.String" />
                <telerik:GridBoundColumn DataField="Drug_Name"  HeaderText='<%$ Resources:Resource, Label_Products_Discussed %>' HeaderStyle-Width="35%" SortExpression="Drug_Name"  UniqueName="Drug_Name" DataType ="System.String" />          
            </Columns>    
            <SortExpressions>
                <telerik:GridSortExpression FieldName="Contact_Date" SortOrder="Descending" />
            </SortExpressions>                   
        </MasterTableView>
        <PagerStyle Visible="false" /> 
        <ClientSettings ClientEvents-OnRowSelecting="gridCCReports_OnRowSelecting">
            <DataBinding Location="~/custom/jazz/customercontactreports/services/PathfinderDataService.svc" DataService-TableName="ContactReportProductsDiscussedViewSet" SelectCountMethod ="GetCCReportCount">
            </DataBinding>
            <Scrolling AllowScroll="true" UseStaticHeaders="true" /> 
            <Selecting AllowRowSelect="true" />
        </ClientSettings>   
    </telerik:RadGrid>    
    <pinso:RadGridWrapper runat="server" ID="gridWrapper" Target="gridCCReports" PagingSelector="#tile6ContainerHeader .pagination" MergeRows="false"  RequiresFilter ="true" AutoLoad="true" UtcDateColumns="Contact_Date" />
</div> 
