<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AffiliationsListViewVA.ascx.cs" Inherits="todaysaccounts_controls_AffiliationsListViewVA" %>
<telerik:RadGrid SkinID="radTable" runat="server" ID="gridAffiliations" AllowSorting="true" PageSize="50" AllowPaging="true" AllowFilteringByColumn="false"  EnableEmbeddedSkins="false">        
        <MasterTableView autogeneratecolumns="False" ClientDataKeyNames="Plan_ID" PageSize="50"  Width="100%">
            <Columns>
                <telerik:GridBoundColumn SortExpression="Plan_Name" HeaderStyle-Width="40%" ItemStyle-Width="40%" HeaderText='<%$ Resources:Resource, Label_Plan_Name %>' UniqueName="Plan_Name"  DataField="Plan_Name" ItemStyle-CssClass="firstCol" />
                <telerik:GridBoundColumn DataField="Plan_State" HeaderStyle-Width="30%" ItemStyle-Width="30%" HeaderText='<%$ Resources:Resource, Label_State %>' SortExpression="Plan_State" UniqueName="Plan_State" />
                <telerik:GridBoundColumn DataField="Total_Covered" HeaderStyle-Width="30%" ItemStyle-Width="30%" DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="Right" DataType="System.Int32" HeaderText='<%$ Resources:Resource, Label_Covered_Lives %>' SortExpression="Total_Covered" UniqueName="Total_Covered" ItemStyle-CssClass="alignRight"  />                
            </Columns>
            <SortExpressions>
                <telerik:GridSortExpression FieldName="Plan_Name" SortOrder="Ascending" />
            </SortExpressions>
            <PagerStyle Visible="false" />
            <FilterItemStyle CssClass="planInfoFilterRow" />
        </MasterTableView>
       
        <ClientSettings ClientEvents-OnRowSelected="onAffiliationsRowSelected">
            <DataBinding Location="~/todaysaccounts/services/pathfinderdataservice.svc" DataService-TableName="PlanAffiliationListViewSet" SelectCountMethod="GetAffiliationCount" />
            <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="100%" />
        </ClientSettings>

    </telerik:RadGrid>
    <pinso:RadGridWrapper runat="server" ID="gridWrapper" AutoUpdate="true" RequiresFilter="true" Target="gridAffiliations" PagingSelector="#divTile3Container .pagination" />