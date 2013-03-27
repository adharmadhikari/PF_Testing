<%@ Control Language="C#" AutoEventWireup="true" CodeFile="OTCCoverageMain.ascx.cs" Inherits="custom_reckitt_otccoverage_controls_OTCCoverageMain" %>


<telerik:RadGrid SkinID="radTable" runat="server" ID="gridOTCCoverage" AllowSorting="true"  AllowFilteringByColumn="false" EnableEmbeddedSkins="false" PageSize="30">        
    <MasterTableView autogeneratecolumns="false" ClientDataKeyNames="Plan_ID,OTC_Coverage_Id,Plan_Name,AE_UserID">
       <Columns>
        <telerik:GridBoundColumn UniqueName="Plan_ID" ReadOnly="True" Display="False" DataField="Plan_ID" />
        <telerik:GridBoundColumn UniqueName="OTC_Coverage_Id" ReadOnly="True" Display="False" DataField="OTC_Coverage_Id" />
        <telerik:GridBoundColumn UniqueName="AE_UserID" ReadOnly="True" Display="False" DataField="AE_UserID" />
        <telerik:GridBoundColumn DataField="Section_Name" UniqueName="Section_Name" HeaderText="Market Segment" HeaderStyle-Width="20%"  />
        <telerik:GridBoundColumn DataField="Plan_Name" UniqueName="Plan_Name" HeaderText="Account Name" HeaderStyle-Width="35%"  />
        <telerik:GridBoundColumn DataField="AE_Name" UniqueName="AE_Name=" HeaderText="Account Manager" HeaderStyle-Width="15%" />
        <telerik:GridBoundColumn DataField="Modified_DT" UniqueName="Modified_DT" HeaderText="Last Modified" ItemStyle-CssClass="alignRight" HeaderStyle-Width="15%" DataFormatString="{0:d}"/>
        </Columns>  
        <SortExpressions>
            <telerik:GridSortExpression FieldName="Plan_Name" SortOrder="Ascending"  />   
        </SortExpressions>               
    </MasterTableView>           
    <ClientSettings ClientEvents-OnRowSelecting="gridOTCCoverage_OnRowSelecting">
          <DataBinding Location="~/custom/reckitt/otccoverage/services/reckittdataservice.svc" DataService-TableName="OTCCoverageSet" SelectCountMethod="GetOTCCoverageCount" />
          <Scrolling AllowScroll="false" UseStaticHeaders="false" ScrollHeight="400px" />
          <Selecting AllowRowSelect="true" /> 
    </ClientSettings>
</telerik:RadGrid>
<pinso:RadGridWrapper runat="server" ID="radGridWrapper" Target="gridOTCCoverage" PagingSelector="#divTile3Container .pagination"  CustomPaging="false"  MergeRows="false" RequiresFilter="false" AutoUpdate="false" AutoLoad="true"/>
