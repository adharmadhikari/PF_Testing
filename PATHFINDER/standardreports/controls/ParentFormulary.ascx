<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ParentFormulary.ascx.cs" Inherits="standardreports_controls_ParentFormulary" %>
<telerik:RadGrid ID="ParentPlanRadGrid" runat="server" SkinID="radTable" EnableEmbeddedSkins="false" Skin="pathfinder" 
    GridLines="None">   
<MasterTableView autogeneratecolumns="False">
    <Columns>        
        <telerik:GridBoundColumn DataField="Plan_Name" HeaderText="Parent Plan Name" HeaderStyle-Width="35%"
            SortExpression="Plan_Name" UniqueName="Plan_Name">
        </telerik:GridBoundColumn>
         <telerik:GridBoundColumn DataField="Drug_Name" HeaderText="Drug Name" HeaderStyle-Width="13%"
            SortExpression="Drug_Name" UniqueName="Drug_Name">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="Tier_Name" HeaderText="Tier Name" HeaderStyle-Width="13%"
            SortExpression="Tier_Name" UniqueName="Tier_Name">
        </telerik:GridBoundColumn>        
        <telerik:GridBoundColumn DataField="Plan_Total_Lives" DataType="System.Int32" ItemStyle-CssClass="alignRight totalLives" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="13%"
            HeaderText="Total Lives" SortExpression="Plan_Total_Lives" DataFormatString="{0:n0}" 
            UniqueName="Plan_Total_Lives">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="Plan_Pharmacy_Lives"   DataFormatString="{0:n0}" ItemStyle-CssClass="alignRight totalLives" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="13%"
            DataType="System.Int32" HeaderText="Pharmacy Lives" SortExpression="Plan_Pharmacy_Lives" UniqueName="Plan_Pharmacy_Lives" Visible="false">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="Plan_Medicare_PartD_Lives" DataFormatString="{0:n0}" ItemStyle-CssClass="alignRight totalLives" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="13%"
            DataType="System.Int32" HeaderText="Medicare Part D Lives"  SortExpression="Plan_Medicare_PartD_Lives" UniqueName="Plan_Medicare_PartD_Lives" Visible="false">
        </telerik:GridBoundColumn>
    </Columns>
</MasterTableView>
 <ClientSettings >       
        <DataBinding Location="~/standardreports/services/pathfinderdataservice.svc"  DataService-TableName="AffiliationsFormularyParentPlanSet" />
    </ClientSettings>
</telerik:RadGrid>
<pinso:RadGridWrapper runat="server" ID="RadGridWrapper1" Target="ParentPlanRadGrid"    RequiresFilter ="true" AutoLoad="false" ShowNumberOfRecords="true" />
