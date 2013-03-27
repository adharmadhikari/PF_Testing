<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ChildFormulary.ascx.cs" Inherits="standardreports_controls_ChildFormulary" %>

 <telerik:RadGrid runat="server" ID="ChildPlanRadGrid" SkinID="radTable" AutoGenerateColumns="false" 
    EnableEmbeddedSkins="false" Skin="pathfinder" PageSize="50" Width="100%">
    <MasterTableView PageSize="50" AutoGenerateColumns="false" AllowSorting="true" Width="100%" AllowMultiColumnSorting="true">
        <Columns>   
        <telerik:GridBoundColumn DataField="Segment_Name" HeaderText="Section Name"  HeaderStyle-Width="10%"
            SortExpression="Segment_Name" UniqueName="Segment_Name">
        </telerik:GridBoundColumn>                
        <telerik:GridBoundColumn DataField="Plan_Name" HeaderText="Child Plan Name"   HeaderStyle-Width="40%"
            SortExpression="Plan_Name" UniqueName="Plan_Name">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="PBM_Service" HeaderText="Services Offered" HeaderStyle-Width="15%"
            SortExpression="PBM_Service" UniqueName="PBM_Service">
        </telerik:GridBoundColumn>
         <telerik:GridBoundColumn DataField="Drug_Name" HeaderText="Drug Name" HeaderStyle-Width="15%"
            SortExpression="Drug_Name" UniqueName="Drug_Name">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="Tier_Name" HeaderText="Tier Name" HeaderStyle-Width="15%"
            SortExpression="Tier_Name" UniqueName="Tier_Name">
        </telerik:GridBoundColumn>        
        <telerik:GridBoundColumn DataField="Plan_Total_Lives" DataType="System.Int32"  
            ItemStyle-CssClass="alignRight totalLives" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="15%"
            HeaderText="Total Lives" SortExpression="Plan_Total_Lives" DataFormatString="{0:n0}" 
            UniqueName="Plan_Total_Lives">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="Plan_Pharmacy_Lives"   DataFormatString="{0:n0}" 
            ItemStyle-CssClass="alignRight totalLives" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="15%"
            DataType="System.Int32" HeaderText="Pharmacy Lives" SortExpression="Plan_Pharmacy_Lives" UniqueName="Plan_Pharmacy_Lives">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="Plan_Medicare_PartD_Lives" DataFormatString="{0:n0}" 
            ItemStyle-CssClass="alignRight totalLives" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="15%"
            DataType="System.Int32" HeaderText="Medicare Part D Lives" 
            SortExpression="Plan_Medicare_PartD_Lives" UniqueName="Plan_Medicare_PartD_Lives">
        </telerik:GridBoundColumn>                        
        </Columns>
    </MasterTableView>
    <ClientSettings AllowColumnHide="True" AllowRowHide="True" AllowColumnsReorder="True" ReorderColumnsOnClient="True" >
        <Resizing EnableRealTimeResize="True" ResizeGridOnColumnResize="True" AllowColumnResize="true" ClipCellContentOnResize="false"></Resizing>
        <DataBinding Location="~/standardreports/services/pathfinderdataservice.svc"  DataService-TableName="AffiliationsFormularySet" />
    </ClientSettings>
</telerik:RadGrid>
<pinso:RadGridWrapper runat="server" ID="gridWrapper" Target="ChildPlanRadGrid" MergeRows="false" PagingSelector="#tile3 .pagination" RequiresFilter ="true" AutoLoad="false" ShowNumberOfRecords="true" />
