<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GridDrillDownTemplate.ascx.cs" Inherits="marketplaceanalytics_controls_GridDrillDownTemplate" %>
<%--<telerik:RadGrid SkinID="radTable" runat="server" 
        ID="gridDrillDownTemplate" AllowSorting="True" 
        AutoGenerateColumns="False" EnableEmbeddedSkins="False" PageSize="50" 
        GridLines="None" >        
        <MasterTableView PageSize="50" 
         AutoGenerateColumns="false" >            
             <Columns>
                <telerik:GridBoundColumn DataField="Plan_Name" HeaderStyle-Width="10%" 
                     HeaderText='Account Name' SortExpression="Plan_Name" UniqueName="Plan_Name" 
                     ReadOnly="True" /> 
                <telerik:GridBoundColumn DataField="Geography_Name" HeaderStyle-Width="10%" 
                     HeaderText='Geography' SortExpression="Geography_Name" 
                     UniqueName="Geography_Name" DataType="System.String" ReadOnly="True" /> 	
                <telerik:GridBoundColumn DataField="Total_Covered" HeaderStyle-Width="10%" 
                     HeaderText='Lives' SortExpression="Total_Covered" 
                     UniqueName="Total_Covered" ReadOnly="True" /> 
                <telerik:GridBoundColumn DataField="MB_TRx" HeaderStyle-Width="10%" 
                     HeaderText='Mkt Trx' SortExpression="MB_TRx" 
                     UniqueName="MB_TRx" ReadOnly="True" /> 	
                <telerik:GridBoundColumn DataField="Drug_Name" HeaderStyle-Width="10%" 
                     HeaderText='Products' SortExpression="Products" 
                     UniqueName="Products" DataType="System.String" ReadOnly="True" /> 	
                <telerik:GridBoundColumn DataField="Tier_Name" HeaderStyle-Width="10%" 
                     HeaderText='Tier' SortExpression="Tier_Name" 
                     UniqueName="Tier_Name" ReadOnly="True" /> 	
                <telerik:GridBoundColumn DataField="Co_Pay" HeaderStyle-Width="10%" 
                     HeaderText='Co-Pay' SortExpression="Co_Pay" 
                     UniqueName="Co_Pay" ReadOnly="True" /> 	
                <telerik:GridBoundColumn DataField="Restrictions" HeaderStyle-Width="10%" 
                     HeaderText='Restrictions' SortExpression="Restrictions" 
                     UniqueName="Restrictions" ReadOnly="True" />                      
             </Columns>                         
             <SortExpressions>
                <telerik:GridSortExpression FieldName="Plan_Name" SortOrder="Ascending"  />     
             </SortExpressions>               
        </MasterTableView>
        
        <ClientSettings>        
            <DataBinding Location="~/custom/pinso/customercontactreports/services/pathfinderdataservice.svc" DataService-TableName="ContactReportDataSet" 
                SelectCountMethod="GetCRDrillDownCount" />             
            <Scrolling AllowScroll="True" UseStaticHeaders="True"   />
        </ClientSettings>          
</telerik:RadGrid>
<pinso:RadGridWrapper runat="server" ID="radGridWrapper" Target="gridDrillDownTemplate" MergeRows="false" PagingSelector="#divTile5Container .pagination" DrillDownLevel="1" AutoUpdate="true" RequiresFilter="true" />--%>
