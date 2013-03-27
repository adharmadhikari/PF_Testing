<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FormularyCoverageDrillDown.ascx.cs" Inherits="standardreports_controls_FormularyCoverageDrillDown" %>
<div id="formularyCoverageDrilldownTitle" class="drillDownTitle"></div>
<div id="tile5formularyCoverageDrilldownData">
<telerik:RadGrid SkinID="radTable" runat="server" ID="gridformularycoveragedrilldown" AllowSorting="true" AutoGenerateColumns="false" 
     AllowPaging="false" AllowFilteringByColumn="false" EnableEmbeddedSkins="false" PageSize="50" >
        <MasterTableView PageSize="50" AutoGenerateColumns="false" ClientDataKeyNames="Plan_Name" Width="100%" AllowMultiColumnSorting="true">
            <Columns>
                <telerik:GridBoundColumn DataField="PBM_Name" HeaderStyle-Width="20%" HeaderText="PBM" SortExpression="PBM_Name"
                    ItemStyle-CssClass="mergewithsecondnextCol pbmname"></telerik:GridBoundColumn> 
                <telerik:GridBoundColumn DataField="Section_Name" HeaderStyle-Width="15%" HeaderText="Section" SortExpression="Section_Name"
                    ItemStyle-CssClass="mergewithnextCol sectionname"></telerik:GridBoundColumn> 
                <telerik:GridBoundColumn DataField="Plan_Name" HeaderStyle-Width="37%" HeaderText='<%$ Resources:Resource, Label_Account_Name %>' SortExpression="Plan_Name" UniqueName="Plan_Name" /> 
                <telerik:GridBoundColumn DataField="Formulary_Name" HeaderStyle-Width="15%" HeaderText='Formulary' 
                    ItemStyle-CssClass="notmerged formularyname" SortExpression="Formulary_Name" UniqueName="Formulary_Name" /> 	
                <telerik:GridBoundColumn DataField="Formulary_Lives"  DataFormatString="{0:n0}" HeaderStyle-Width="15%" HeaderText='<%$ Resources:Resource, Label_Lives %>' SortExpression="Formulary_Lives" UniqueName="Formulary_Lives" ItemStyle-CssClass="alignRight totalLives" /> 
                <telerik:GridBoundColumn DataField="Tier_Name" HeaderStyle-Width="10%" HeaderText='Tier' SortExpression="Tier_Name" UniqueName="Tier_Name" /> 	
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
            <DataBinding Location="~/standardreports/services/pathfinderdataservice.svc"  DataService-TableName="FormularyCoverageDrilldownSet" 
                 />               
            <Scrolling AllowScroll="True" UseStaticHeaders="true"  />            
        </ClientSettings>   
</telerik:RadGrid>
</div>
<pinso:RadGridWrapper runat="server" ID="radGridWrapper" Target="gridformularycoveragedrilldown" MergeRows="true" DrillDownLevel="1" 
    PagingSelector="#divTile5Container .pagination" AutoUpdate="true" RequiresFilter="true" />
