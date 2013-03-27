<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ActivityReportingDrilldownData.ascx.cs" Inherits="custom_controls_CCRDrilldownData" %>

<telerik:RadGrid SkinID="radTable" runat="server" 
        ID="gridDrillDown" AllowSorting="True" 
        PageSize="50" AllowPaging="true" AllowFilteringByColumn="false" EnableEmbeddedSkins="false" Width="100%" >      
        <MasterTableView PageSize="50" AutoGenerateColumns="false" ClientDataKeyNames="Activity_ID" AllowMultiColumnSorting="true" >
            
             <Columns>
                <telerik:GridBoundColumn DataField="Territory_Name" HeaderStyle-Width="15%" 
                     HeaderText='Geography' SortExpression="Territory_Name" UniqueName="Territory_Name" 
                     ReadOnly="True" /> 
                <telerik:GridBoundColumn DataField="FullName" HeaderStyle-Width="19%" 
                     HeaderText='Account Manager' SortExpression="FullName" UniqueName="FullName" 
                     ReadOnly="True" /> 
                <telerik:GridBoundColumn DataField="Activity_Date" HeaderStyle-Width="11%" 
                     HeaderText='Activity Date' SortExpression="Activity_Date" 
                     UniqueName="Activity_Date" DataType="System.DateTime" DataFormatString="{0:M/dd/yyyy}" ReadOnly="True" /> 	
                <telerik:GridBoundColumn DataField="Activity_Type_Name" HeaderStyle-Width="27%" 
                     HeaderText='Activity Name' SortExpression="Activity_Type_Name" 
                     UniqueName="Activity_Type_Name" ReadOnly="True" /> 
                <telerik:GridBoundColumn DataField="Activity_Hours" HeaderStyle-Width="17%" 
                     HeaderText='Hours' SortExpression="Activity_Hours" 
                     UniqueName="Activity_Hours" ReadOnly="True" /> 	
             </Columns>                         
             <PagerStyle Visible="false" />
             <SortExpressions>
                <telerik:GridSortExpression FieldName="Territory_Name" SortOrder="Ascending"  />    
                <telerik:GridSortExpression FieldName="FullName" SortOrder="Ascending"  />   
                <telerik:GridSortExpression FieldName="Activity_Date" SortOrder="Ascending"  />   
             </SortExpressions>               
        </MasterTableView>
        
        <ClientSettings>        
            <DataBinding Location="~/custom/alcon/activityreporting/services/Alcondataservice.svc" DataService-TableName="ActivityReportingDetailsSet" 
                 />               
            <Scrolling AllowScroll="True" UseStaticHeaders="True"   />
            <Resizing EnableRealTimeResize="true" />
        </ClientSettings>          
</telerik:RadGrid>
<pinso:RadGridWrapper runat="server" ID="radGridWrapper"  Target="gridDrillDown" MergeRows="true" PagingSelector="#divTile3Container .pagination" AutoUpdate="true" RequiresFilter="true" UtcDateColumns="Activity_Date" />