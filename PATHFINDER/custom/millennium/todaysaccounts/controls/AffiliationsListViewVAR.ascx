<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AffiliationsListViewVAR.ascx.cs" Inherits="custom_millennium_todaysaccounts_controls_AffiliationsListViewVAR" %>
<telerik:RadGrid SkinID="radTable" runat="server" ID="gridAffiliations" AllowSorting="true" PageSize="50" AllowPaging="true" AllowFilteringByColumn="false"  EnableEmbeddedSkins="false">        
        <MasterTableView autogeneratecolumns="False" ClientDataKeyNames="VISN" PageSize="50"  Width="100%">
            <Columns>
                <telerik:GridBoundColumn HeaderStyle-Width="200px" SortExpression="Plan_Name" HeaderText='<%$ Resources:Resource, Label_Plan_Name %>' UniqueName="Plan_Name"  DataField="Plan_Name" ItemStyle-CssClass="firstCol" />
                <telerik:GridBoundColumn DataField="Plan_State" HeaderStyle-Width="100px" HeaderText='<%$ Resources:Resource, Label_State %>' SortExpression="Plan_State" UniqueName="Plan_State" />
                <telerik:GridBoundColumn DataField="VISN" HeaderStyle-Width="100px" HeaderText='VISN' SortExpression="VISN" UniqueName="VISN" />                                    
                <telerik:GridBoundColumn DataFormatString='<%$ Resources:Resource, HTML_PlanWebsite_Link %>' DataField="Plan_WebSite" HeaderStyle-Width="100px"  HeaderText='<%$ Resources:Resource, Label_Website %>' UniqueName="Plan_WebSite" ItemStyle-CssClass="links" />                
            </Columns>
            <SortExpressions>
                <telerik:GridSortExpression FieldName="Plan_Name" SortOrder="Ascending" />
            </SortExpressions>
            <PagerStyle Visible="false" />
            <FilterItemStyle CssClass="planInfoFilterRow" />
        </MasterTableView>
       
        <ClientSettings>
            <DataBinding Location="~/custom/millennium/todaysaccounts/services/millenniumdataservice.svc" DataService-TableName="PlanAffiliationListView_VARSet" SelectCountMethod="GetVARAffiliationCount" />
            <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="100%" />
            <Selecting AllowRowSelect="false" />
        </ClientSettings>

    </telerik:RadGrid>
    <pinso:RadGridWrapper runat="server" ID="gridWrapper" AutoUpdate="true" RequiresFilter="true" Target="gridAffiliations" PagingSelector="#divTile3Container .pagination" />