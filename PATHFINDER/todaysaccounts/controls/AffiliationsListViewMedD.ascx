<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AffiliationsListViewMedD.ascx.cs" Inherits="todaysaccounts_controls_AffiliationsListViewMedD" %>
<telerik:RadGrid SkinID="radTable" runat="server" ID="gridAffiliations" AllowSorting="true" PageSize="50" AllowPaging="true" AllowFilteringByColumn="false"  EnableEmbeddedSkins="false" Width="100%">        
        <MasterTableView autogeneratecolumns="False" ClientDataKeyNames="Plan_ID" PageSize="50"  Width="100%" AllowMultiColumnSorting="true">
            <Columns>
                <telerik:GridBoundColumn HeaderStyle-Width="200px" SortExpression="Plan_Name" HeaderText='<%$ Resources:Resource, Label_Plan_Name %>' UniqueName="Plan_Name"  DataField="Plan_Name" ItemStyle-CssClass="firstCol" />
                <telerik:GridBoundColumn DataField="Prod_Name" HeaderStyle-Width="200px" HeaderText='<%$ Resources:Resource, Label_Plan_Product %>' SortExpression="Prod_Name" UniqueName="Prod_Name" ItemStyle-CssClass="notmerged"/>
                <telerik:GridBoundColumn DataField="Prod_Type_Name" HeaderStyle-Width="90px" HeaderText='<%$ Resources:Resource, Label_Product_Type %>' SortExpression="Prod_Type_Name" UniqueName="Prod_Type_Name" ItemStyle-CssClass="notmerged" />
                <telerik:GridBoundColumn DataField="Plan_State" HeaderStyle-Width="60px" HeaderText='<%$ Resources:Resource, Label_State %>' SortExpression="Plan_State" UniqueName="Plan_State" />
                <%--<telerik:GridBoundColumn DataField="Plan_Type_Name" HeaderStyle-Width="90px" HeaderText='<%$ Resources:Resource, Label_Plan_Type %>' SortExpression="Plan_Type_Name" UniqueName="Plan_Type_Name" />--%>                                                               
                <telerik:GridBoundColumn DataField="Total_Pharmacy" HeaderStyle-Width="100px" DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="Right" DataType="System.Int32" HeaderText='<%$ Resources:Resource, Label_Pharmacy_Lives %>' SortExpression="Total_Pharmacy" UniqueName="Total_Pharmacy" ItemStyle-CssClass="alignRight"/>
                <telerik:GridBoundColumn DataField="PBM_Service" HeaderStyle-Width="100px" HeaderText='<%$ Resources:Resource, Label_PBM_Service %>' SortExpression="PBM_Service" UniqueName="PBM_Service" ItemStyle-CssClass="notmerged"/>
                <telerik:GridBoundColumn DataFormatString='<%$ Resources:Resource, HTML_PlanWebsite_Link %>' DataField="Plan_WebSite" HeaderStyle-Width="160px"  HeaderText='<%$ Resources:Resource, Label_Website %>' UniqueName="Plan_WebSite" ItemStyle-CssClass="notmerged links" />                
            </Columns>
            <SortExpressions>
                <telerik:GridSortExpression FieldName="Plan_Name" SortOrder="Ascending" />
                <telerik:GridSortExpression FieldName="Prod_Name" SortOrder="Ascending" />
            </SortExpressions>
            <PagerStyle Visible="false" />
            <FilterItemStyle CssClass="planInfoFilterRow" />
        </MasterTableView>
       
        <ClientSettings ClientEvents-OnRowSelected="onMedDAffiliationsRowSelected" ClientEvents-OnDataBound="clearSelection">
            <DataBinding Location="~/todaysaccounts/services/pathfinderdataservice.svc" DataService-TableName="PlanAffiliationListView_MedDProductsSet" SelectCountMethod="GetMedDAffiliationCount" />
            <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="100%" />
            <Selecting AllowRowSelect="true" />
        </ClientSettings>

    </telerik:RadGrid>
    <pinso:RadGridWrapper runat="server" ID="gridWrapper" AutoUpdate="true" RequiresFilter="true" Target="gridAffiliations" PagingSelector="#divTile3Container .pagination" />