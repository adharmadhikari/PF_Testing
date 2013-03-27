<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RestrictionsDrillDownReport.ascx.cs" Inherits="restrictionsreport_controls_MedicalPharmacyCoverageDrillDown" %>
<div id="restrictionsReportDrilldownTitle" class="drillDownTitle"></div>
<div id="tile3RRDataDrillDown">
<telerik:RadGrid SkinID="radTable" runat="server" ID="gridRestrictionsReportdrilldownReport" AllowSorting="true" AutoGenerateColumns="false" 
     AllowPaging="false" AllowFilteringByColumn="false" EnableEmbeddedSkins="false" PageSize="50" >
        <MasterTableView PageSize="50" AutoGenerateColumns="false" ClientDataKeyNames="Plan_Name">
            <Columns>
                <telerik:GridBoundColumn DataField="Section_Name" HeaderStyle-Width="10%" HeaderText="Market Segment" SortExpression="Section_Name" UniqueName="Section_Name" /> 
                <telerik:GridBoundColumn DataField="Geography_Name" HeaderStyle-Width="8%" HeaderText="Geography" SortExpression="Geography_Name" UniqueName="Geography_Name" /> 
                <telerik:GridBoundColumn DataField="Plan_Name" HeaderStyle-Width="12%" HeaderText='<%$ Resources:Resource, Label_Account_Name %>' SortExpression="Plan_Name" UniqueName="Plan_Name" /> 
                <telerik:GridBoundColumn DataField="Covered_Lives" DataFormatString="{0:n0}" HeaderStyle-Width="7%" HeaderText="Total Lives" SortExpression="Covered_Lives" UniqueName="Covered_Lives" ItemStyle-CssClass="alignRight totalLives"/> 
                <telerik:GridBoundColumn DataField="Formulary_Name" HeaderStyle-Width="8%" HeaderText='Formulary' SortExpression="Formulary_Name" UniqueName="Formulary_Name" /> 	
                <telerik:GridBoundColumn DataField="Formulary_Lives"  DataFormatString="{0:n0}" HeaderStyle-Width="7%" HeaderText="Formulary Lives" SortExpression="Formulary_Lives" UniqueName="Formulary_Lives" ItemStyle-CssClass="alignRight totalLives" /> 
                <telerik:GridBoundColumn DataField="Drug_Name" HeaderStyle-Width="6%" HeaderText='Product Name' SortExpression="Drug_Name" UniqueName="Drug_Name" />	                                
                <telerik:GridBoundColumn DataField="Tier_Name" HeaderStyle-Width="3%" HeaderText='Tier' SortExpression="Tier_Name" UniqueName="Tier_Name" />	                                
                <telerik:GridBoundColumn DataField="Copay_Range" HeaderText="Copay Range" HeaderStyle-Width="5%" UniqueName="Copay_Range" DataType="System.String" ItemStyle-CssClass="alignRight notmerged copayRange"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Formulary_Status_Name" HeaderText="Status" HeaderStyle-Width="7%" ItemStyle-CssClass="statusName" UniqueName="Formulary_Status_Name" ItemStyle-Wrap="true" DataType="System.String"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="PA" HeaderText="PA"  ItemStyle-CssClass="notmerged paCol" HeaderStyle-Width="3%" UniqueName="PA" DataType="System.String"></telerik:GridBoundColumn>                
                <telerik:GridBoundColumn DataField="QL" HeaderText="QL"  ItemStyle-CssClass="notmerged qlCol" HeaderStyle-Width="3%" UniqueName="QL" DataType="System.String"></telerik:GridBoundColumn>         
                <telerik:GridBoundColumn DataField="ST" HeaderText="ST"  ItemStyle-CssClass="notmerged stCol" HeaderStyle-Width="3%" UniqueName="ST" DataType="System.String"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Criteria_Name" HeaderStyle-Width="8%" HeaderText='QL Restriction Criteria' SortExpression="Criteria_Name" UniqueName="Criteria_Name" />
                <telerik:GridBoundColumn DataField="Criteria_Description" HeaderStyle-Width="10%" HeaderText='Criteria Details' SortExpression="Criteria_Description" UniqueName="Criteria_Description" />
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
<pinso:RadGridWrapper runat="server" ID="radGridWrapper" Target="gridRestrictionsReportdrilldownReport" MergeRows="true" PagingSelector="#divTile3Container .pagination" AutoUpdate="true" RequiresFilter="true" AutoLoad="false" ShowNumberOfRecords="true" />
