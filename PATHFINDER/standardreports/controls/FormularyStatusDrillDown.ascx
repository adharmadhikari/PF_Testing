<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FormularyStatusDrillDown.ascx.cs" Inherits="standardreports_controls_FormularyStatusDrillDown" %>
<div id="formularyStatusDrilldownTitle" class="drillDownTitle"></div>
<div id="tile5FSDataDrillDown">
    <telerik:RadGrid SkinID="radTable" runat="server" ID="gridformularystatusdrilldown"
        AllowSorting="true" AutoGenerateColumns="false" AllowPaging="false" AllowFilteringByColumn="false"
        EnableEmbeddedSkins="false" PageSize="50">
        <MasterTableView PageSize="50" AutoGenerateColumns="false" ClientDataKeyNames="Plan_Name"
            AllowMultiColumnSorting="true">
            <Columns>
                <telerik:GridBoundColumn DataField="PBM_Name" HeaderStyle-Width="14%" HeaderText='PBM'
                    ItemStyle-CssClass="mergewithsecondnextCol pbmname" UniqueName="PBM_Name" SortExpression="PBM_Name" />
                <telerik:GridBoundColumn DataField="Section_Name" HeaderStyle-Width="14%" HeaderText='Section'
                    SortExpression="Section_Name" ItemStyle-CssClass="mergewithnextCol sectionname"
                    UniqueName="Section_Name" />
                <telerik:GridBoundColumn DataField="Plan_Name" HeaderStyle-Width="27%" HeaderText='Account Name'
                    SortExpression="Plan_Name" UniqueName="Plan_Name" />
                <telerik:GridBoundColumn DataField="Formulary_Lives" DataFormatString="{0:n0}" HeaderStyle-Width="15%"
                    HeaderText='Lives' SortExpression="Formulary_Lives" UniqueName="Formulary_Lives"
                    ItemStyle-CssClass="alignRight totalLives" />
                <telerik:GridBoundColumn DataField="Formulary_Name" HeaderStyle-Width="15%" HeaderText='Formulary'
                    ItemStyle-CssClass="notmerged formularyname" SortExpression="Formulary_Name"
                    UniqueName="Formulary_Name"  />
                <telerik:GridBoundColumn DataField="Restrictions" HeaderStyle-Width="15%" HeaderText='Restrictions'
                    SortExpression="Restrictions" UniqueName="Restrictions" />
            </Columns>
            <PagerStyle Visible="false" />
            <SortExpressions>
                <telerik:GridSortExpression FieldName="PBM_Name" SortOrder="Ascending" />
                <telerik:GridSortExpression FieldName="Section_Name" SortOrder="Ascending" />
                <telerik:GridSortExpression FieldName="Plan_Name" SortOrder="Ascending" />
                <telerik:GridSortExpression FieldName="Formulary_Name" SortOrder="Ascending" />
                <telerik:GridSortExpression FieldName="Formulary_Lives" SortOrder="None" />
            </SortExpressions>
        </MasterTableView>
        <ClientSettings>
            <DataBinding Location="~/standardreports/services/pathfinderdataservice.svc" DataService-TableName="ReportsFormularyStatusDrilldownSet" />
            <Scrolling AllowScroll="True" UseStaticHeaders="True" />
            <%--  <Selecting AllowRowSelect="true" />   --%>
        </ClientSettings>
    </telerik:RadGrid>
</div>
<pinso:RadGridWrapper runat="server" ID="radGridWrapper" Target="gridformularystatusdrilldown" MergeRows="true" PagingSelector="#divTile5Container .pagination" DrillDownLevel="1" AutoUpdate="true" RequiresFilter="true" />
