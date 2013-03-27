<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ccrProductsDiscussedDrillDown.ascx.cs" Inherits="custom_controls_ccrProductsDiscussedDrillDown" %>
<telerik:RadGrid SkinID="radTable" runat="server" 
        ID="gridCcrProductsDiscussedDrillDown" AllowSorting="True" 
        AutoGenerateColumns="False" EnableEmbeddedSkins="False" PageSize="50" 
        GridLines="None" >        
        <MasterTableView PageSize="50" 
         AutoGenerateColumns="false" 
         ClientDataKeyNames="Plan_ID">            
             <Columns>
                <telerik:GridBoundColumn DataField="Account_Manager" HeaderStyle-Width="15%" 
                     HeaderText='Account Manager' SortExpression="Account_Manager" UniqueName="Account_Manager" 
                     ReadOnly="True" /> 
                <telerik:GridBoundColumn DataField="Plan_Name" HeaderStyle-Width="17%" 
                     HeaderText='Account Name' SortExpression="Plan_Name" UniqueName="Plan_Name" 
                     ReadOnly="True" />    
                <telerik:GridBoundColumn DataField="Contact_Date" HeaderStyle-Width="10%" 
                     HeaderText='Date' SortExpression="Contact_Date" 
                     UniqueName="Contact_Date" DataType="System.DateTime" DataFormatString="{0:M/dd/yyyy}" ReadOnly="True" /> 	
                <telerik:GridBoundColumn DataField="Meeting_Type_Name" HeaderStyle-Width="13%" 
                     HeaderText='Meeting Type' SortExpression="Meeting_Type_Name" 
                     UniqueName="Meeting_Type_Name" ReadOnly="True" />                      
                <telerik:GridBoundColumn DataField="Meeting_Activity_Name" HeaderStyle-Width="13%" 
                     HeaderText='Meeting Activity' SortExpression="Meeting_Activity_Name" 
                     UniqueName="Meeting_Activity_Name" ReadOnly="True" /> 
                <telerik:GridBoundColumn DataField="Persons_Met" HeaderStyle-Width="22%" 
                     HeaderText='Persons Met' SortExpression="Persons_Met" 
                     UniqueName="Persons_Met" ReadOnly="True" /> 
                <telerik:GridBoundColumn DataField="Followup_Date" HeaderStyle-Width="10%" 
                     HeaderText='Follow-up' SortExpression="Followup_Date" 
                     UniqueName="Followup_Date" DataType="System.DateTime" DataFormatString="{0:M/dd/yyyy}" ReadOnly="True" /> 	
             </Columns>                         
             <SortExpressions>
                <telerik:GridSortExpression FieldName="Plan_Name" SortOrder="Ascending"  />     
             </SortExpressions>               
        </MasterTableView>
        
        <ClientSettings>        
            <DataBinding Location="~/custom/gsk/customercontactreports/services/GSKdataservice.svc" DataService-TableName="ContactReportDataSet" 
                SelectCountMethod="GetCRDrillDownCount" />    
            <Scrolling AllowScroll="True" UseStaticHeaders="True" />
        </ClientSettings>          
</telerik:RadGrid>
<pinso:RadGridWrapper runat="server" ID="radGridWrapper" Target="gridCcrProductsDiscussedDrillDown" MergeRows="false" PagingSelector="#divTile5Container .pagination" DrillDownLevel="1" AutoUpdate="true" RequiresFilter="true" UtcDateColumns="Contact_Date,Followup_Date" />
