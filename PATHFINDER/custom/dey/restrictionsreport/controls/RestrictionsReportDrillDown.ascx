<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RestrictionsReportDrillDown.ascx.cs" Inherits="restrictionsreport_controls_MedicalPharmacyCoverageDrillDown" %>
<div id="restrictionsReportDrilldownTitle" class="drillDownTitle"></div>
<div id="tile5RRDataDrillDown">
<telerik:RadGrid SkinID="radTable" runat="server" ID="gridRestrictionsReportdrilldown" AllowSorting="true" AutoGenerateColumns="false" 
     AllowPaging="false" AllowFilteringByColumn="false" EnableEmbeddedSkins="false" PageSize="50" >
        <MasterTableView PageSize="50" AutoGenerateColumns="false" ClientDataKeyNames="Plan_Name">
            <Columns>
                <telerik:GridBoundColumn DataField="Geography_Name" HeaderStyle-Width="13%" HeaderText="Geography" SortExpression="Geography_Name" UniqueName="Geography_Name" /> 
                <telerik:GridBoundColumn DataField="Plan_Name" HeaderStyle-Width="18%" HeaderText='<%$ Resources:Resource, Label_Account_Name %>' SortExpression="Plan_Name" UniqueName="Plan_Name" /> 
                <telerik:GridBoundColumn DataField="Covered_Lives" DataFormatString="{0:n0}" HeaderStyle-Width="9%" HeaderText="Total Lives" SortExpression="Covered_Lives" UniqueName="Covered_Lives" ItemStyle-CssClass="alignRight totalLives"/> 
                <telerik:GridBoundColumn DataField="Formulary_Name" HeaderStyle-Width="17%" HeaderText='Formulary' SortExpression="Formulary_Name" UniqueName="Formulary_Name" /> 	
                <telerik:GridBoundColumn DataField="Formulary_Lives"  DataFormatString="{0:n0}" HeaderStyle-Width="9%" HeaderText="Formulary Lives" SortExpression="Formulary_Lives" UniqueName="Formulary_Lives" ItemStyle-CssClass="alignRight totalLives" /> 
                <telerik:GridBoundColumn DataField="Tier_Name" HeaderStyle-Width="6%" HeaderText='Tier Status' SortExpression="Tier_Name" UniqueName="Tier_Name" />	                                
                <telerik:GridBoundColumn DataField="Copay_Range" HeaderText="Copay Range" HeaderStyle-Width="7%" UniqueName="Copay_Range" DataType="System.String" ItemStyle-CssClass="alignRight notmerged copayRange"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Formulary_Status_Name" HeaderText="Status" HeaderStyle-Width="8%" ItemStyle-CssClass="statusName" UniqueName="Formulary_Status_Name" ItemStyle-Wrap="true" DataType="System.String"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="PA" HeaderText="PA"  ItemStyle-CssClass="notmerged paCol" HeaderStyle-Width="4%" UniqueName="PA" DataType="System.String"></telerik:GridBoundColumn>                
                
                <telerik:GridHyperLinkColumn DataNavigateUrlFormatString='javascript:OpenQLFormCriteria({0},{1},{2},{3},{4},"QL");'
                    DataNavigateUrlFields="Plan_ID,Drug_ID,Formulary_ID,Segment_ID,Product_ID"
                    UniqueName="QL" DataTextFormatString='{0}' DataTextField="QL"
                    HeaderStyle-Width="4%" HeaderText="QL" >
                </telerik:GridHyperLinkColumn>             
                <telerik:GridBoundColumn DataField="ST" HeaderText="ST"  ItemStyle-CssClass="notmerged stCol" HeaderStyle-Width="4%" UniqueName="ST" DataType="System.String"></telerik:GridBoundColumn>
                <%--<telerik:GridBoundColumn DataField="Criteria_Description" HeaderStyle-Width="15%" HeaderText='Criteria Details' SortExpression="Criteria_Description" UniqueName="Criteria_Description" />--%>
            </Columns>         
            <PagerStyle Visible="false" />     
            <SortExpressions>
                <telerik:GridSortExpression FieldName="Formulary_Lives" SortOrder="Descending" />
            </SortExpressions>             
        </MasterTableView>
        
        <ClientSettings>
            <DataBinding Location="../services/DeyDataService.svc"  DataService-TableName="RestrictionsReportDrilldownSet" 
                 />               
            <Scrolling AllowScroll="True" UseStaticHeaders="true"  />
            <%--<Selecting AllowRowSelect="true" />--%>
        </ClientSettings>   
</telerik:RadGrid>
</div>
<pinso:RadGridWrapper runat="server" ID="radGridWrapper" Target="gridRestrictionsReportdrilldown" MergeRows="false" DrillDownLevel="1" PagingSelector="#divTile5Container .pagination" AutoUpdate="true" RequiresFilter="true" />
