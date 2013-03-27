<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TierCoverageDrillDown.ascx.cs" Inherits="standardreports_controls_TierCoverageDrillDown" %>
<div id="tierCoverageDrilldownTitle" class="drillDownTitle"></div>
<div id="tile5TCDataDrillDown">
<telerik:RadGrid SkinID="radTable" runat="server" ID="gridtiercoveragedrilldown" AllowSorting="true" AutoGenerateColumns="false" 
     AllowPaging="false" AllowFilteringByColumn="false" EnableEmbeddedSkins="false" PageSize="50" >
        <MasterTableView PageSize="50" AutoGenerateColumns="false" ClientDataKeyNames="Plan_Name" AllowMultiColumnSorting="true">
            <Columns>
            <telerik:GridBoundColumn DataField="PBM_Name" HeaderStyle-Width="25%" HeaderText="PBM" SortExpression="PBM_Name"
                    ItemStyle-CssClass="mergewithsecondnextCol pbmname"></telerik:GridBoundColumn> 
                <telerik:GridBoundColumn DataField="Section_Name" HeaderStyle-Width="15%" HeaderText="Section" SortExpression="Section_Name"
                    ItemStyle-CssClass="mergewithnextCol sectionname"></telerik:GridBoundColumn> 
                <telerik:GridBoundColumn DataField="Plan_Name" HeaderStyle-Width="30%" HeaderText='<%$ Resources:Resource, Label_Account_Name %>' 
                    SortExpression="Plan_Name" UniqueName="Plan_Name" /> 
                <telerik:GridBoundColumn DataField="Formulary_Name" HeaderStyle-Width="15%" HeaderText='Formulary' SortExpression="Formulary_Name" 
                    UniqueName="Formulary_Name" ItemStyle-CssClass="notmerged formularyname" /> 	                
                <telerik:GridBoundColumn DataField="Formulary_Lives"  DataFormatString="{0:n0}" HeaderStyle-Width="15%" 
                    HeaderText='<%$ Resources:Resource, Label_Lives %>' SortExpression="Formulary_Lives" UniqueName="Formulary_Lives" 
                    ItemStyle-CssClass="alignRight totalLives" />                 
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
            <DataBinding Location="~/standardreports/services/pathfinderdataservice.svc"  
                    DataService-TableName="ReportsTierCoverageDrilldownSet" 
                 />               
            <Scrolling AllowScroll="True" UseStaticHeaders="true"  />           
            <%--<Selecting AllowRowSelect="true" />--%>
        </ClientSettings>   
</telerik:RadGrid>
</div>
<pinso:RadGridWrapper runat="server" ID="radGridWrapper" Target="gridtiercoveragedrilldown" MergeRows="true" DrillDownLevel="1" 
    PagingSelector="#divTile5Container .pagination" AutoUpdate="true" RequiresFilter="true" LoadingText=""  />
