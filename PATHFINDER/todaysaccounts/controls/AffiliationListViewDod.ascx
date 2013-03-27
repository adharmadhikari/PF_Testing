<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AffiliationListViewDod.ascx.cs" Inherits="todaysaccounts_controls_AffiliationListViewDod" %>
<telerik:RadGrid SkinID="radTable" runat="server" ID="gridAffiliations"  Width="100%"
AllowSorting="true" PageSize="50" AllowPaging="true" AllowFilteringByColumn="false"  EnableEmbeddedSkins="false">        
        <MasterTableView autogeneratecolumns="False"  AllowMultiColumnSorting="true" ClientDataKeyNames="Plan_ID" PageSize="50"  Width="100%">
            <Columns>
                <telerik:GridBoundColumn UniqueName="Mid_Parent_Plan_ID" DataField="Mid_Parent_Plan_ID" Visible="false" />
                <telerik:GridBoundColumn  HeaderStyle-Width="40%" ItemStyle-Width="40%" SortExpression="Mid_Parent_Plan" HeaderText="Regional Parent" UniqueName="Mid_Parent_Plan"  DataField="Mid_Parent_Plan" ItemStyle-CssClass="firstCol" />
                <telerik:GridBoundColumn  HeaderStyle-Width="40%" ItemStyle-Width="40%" SortExpression="Child_Plan" HeaderText="Affiliated Account" UniqueName="Child_Plan"  DataField="Child_Plan" ItemStyle-CssClass="firstCol" />
                <telerik:GridBoundColumn  HeaderStyle-Width="20%" ItemStyle-Width="20%" DataField="Total_Covered" DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="Right" DataType="System.Int32" HeaderText='<%$ Resources:Resource, Label_Covered_Lives %>' SortExpression="Total_Covered" UniqueName="Total_Covered" ItemStyle-CssClass="alignRight"  />                
            </Columns>
            <SortExpressions>
                 <telerik:GridSortExpression FieldName="Mid_Parent_Plan" SortOrder="Ascending" />
                <telerik:GridSortExpression FieldName="Child_Plan" SortOrder="Ascending" />
            </SortExpressions>
            <PagerStyle Visible="false" />
            <FilterItemStyle CssClass="planInfoFilterRow" />
        </MasterTableView>
       
        <ClientSettings>
            <DataBinding Location="~/todaysaccounts/services/pathfinderdataservice.svc" DataService-TableName="PlanAffiliationListView_DodSet" SelectCountMethod="GetAffiliationCount" />
            <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="100%" />
            <ClientEvents OnRowDataBound="onDOD_RowDataBound" />
        </ClientSettings>

    </telerik:RadGrid>
    <pinso:RadGridWrapper runat="server" ID="gridWrapper" AutoUpdate="true" RequiresFilter="true" Target="gridAffiliations" PagingSelector="#divTile3Container .pagination" />
     