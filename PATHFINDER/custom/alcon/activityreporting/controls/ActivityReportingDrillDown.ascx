<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ActivityReportingDrillDown.ascx.cs" Inherits="custom_controls_ActivityReportingDrillDown" %>
<telerik:RadGrid SkinID="radTable" runat="server" 
        ID="gridDrillDown" AllowSorting="True" 
        AutoGenerateColumns="False" EnableEmbeddedSkins="False" PageSize="50" 
        GridLines="None" >
        
        <MasterTableView PageSize="50" 
         AutoGenerateColumns="false" 
         ClientDataKeyNames="Activity_ID">
            
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
                <telerik:GridBoundColumn DataField="Activity_Type_Name" HeaderStyle-Width="17%" 
                     HeaderText='Activity Name' SortExpression="Activity_Type_Name" 
                     UniqueName="Activity_Type_Name" ReadOnly="True" /> 
                <telerik:GridBoundColumn DataField="Activity_Hours" HeaderStyle-Width="27%" 
                     HeaderText='Hours' SortExpression="Activity_Hours" 
                     UniqueName="Activity_Hours" ReadOnly="True" /> 	
             </Columns>                         
             <SortExpressions>
                <telerik:GridSortExpression FieldName="Territory_Name" SortOrder="Ascending"  />     
             </SortExpressions>               
        </MasterTableView>
        
        <ClientSettings>        
            <DataBinding Location="~/custom/alcon/activityreporting/services/Alcondataservice.svc" DataService-TableName="ActivityReportingDetailsSet" 
                 />               
            <Scrolling AllowScroll="True" UseStaticHeaders="True"   />
        </ClientSettings>          
</telerik:RadGrid>
<pinso:RadGridWrapper runat="server" ID="radGridWrapper" Target="gridDrillDown" MergeRows="true" PagingSelector="#divTile5Container .pagination" DrillDownLevel="1" AutoUpdate="true" RequiresFilter="true" UtcDateColumns="Activity_Date" />
