<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AffiliationsListView.ascx.cs" Inherits="todaysaccounts_controls_AffiliationsListView" %>
<telerik:RadGrid SkinID="radTable" runat="server" ID="gridAffiliations" AllowSorting="true" PageSize="50" AllowPaging="true" AllowFilteringByColumn="false"  EnableEmbeddedSkins="false">        
        <MasterTableView autogeneratecolumns="False" ClientDataKeyNames="Plan_ID" PageSize="50"  Width="100%">
            <Columns>
                <telerik:GridBoundColumn HeaderStyle-Width="200px" SortExpression="Plan_Name" HeaderText='<%$ Resources:Resource, Label_Plan_Name %>' UniqueName="Plan_Name"  DataField="Plan_Name" ItemStyle-CssClass="firstCol" />
                <telerik:GridBoundColumn DataField="Plan_State" HeaderStyle-Width="100px" HeaderText='<%$ Resources:Resource, Label_State %>' SortExpression="Plan_State" UniqueName="Plan_State" />
                <telerik:GridBoundColumn DataField="Plan_Type_Name" HeaderStyle-Width="100px" HeaderText='<%$ Resources:Resource, Label_Plan_Type %>' SortExpression="Plan_Type_Name" UniqueName="Plan_Type_Name" />                                    
                <telerik:GridBoundColumn DataField="Total_Covered" DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="100px" DataType="System.Int32" HeaderText='<%$ Resources:Resource, Label_Covered_Lives %>' SortExpression="Total_Covered" UniqueName="Total_Covered" ItemStyle-CssClass="alignRight"  />                
                <telerik:GridBoundColumn DataField="Total_Pharmacy" HeaderStyle-Width="100px" DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="Right" DataType="System.Int32" HeaderText='<%$ Resources:Resource, Label_Pharmacy_Lives %>' SortExpression="Total_Pharmacy" UniqueName="Total_Pharmacy" ItemStyle-CssClass="alignRight"/>
                <telerik:GridBoundColumn DataField="PBM_Service" HeaderStyle-Width="100px" HeaderText='<%$ Resources:Resource, Label_PBM_Service %>' SortExpression="PBM_Service" UniqueName="PBM_Service" />
                <telerik:GridBoundColumn DataFormatString='<%$ Resources:Resource, HTML_PlanWebsite_Link %>' DataField="Plan_WebSite" HeaderStyle-Width="100px"  HeaderText='<%$ Resources:Resource, Label_Website %>' UniqueName="Plan_WebSite" ItemStyle-CssClass="links" />                
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
            <Selecting AllowRowSelect="true" />
        </ClientSettings>

    </telerik:RadGrid>
    <pinso:RadGridWrapper runat="server" ID="gridWrapper" AutoUpdate="true" RequiresFilter="true" Target="gridAffiliations" PagingSelector="#divTile3Container .pagination" />