<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ccrMeetingTypeDrillDown.ascx.cs" Inherits="custom_controls_MeetingTypeDrillDown" %>
 <telerik:RadGrid SkinID="radTable" runat="server" ID="gridMeetingTypeDrillDown" AllowSorting="true"   
        PageSize="50" AllowPaging="true" AllowFilteringByColumn="false" EnableEmbeddedSkins="false">
        <MasterTableView AutoGenerateColumns="False" ClientDataKeyNames="Account_Name" PageSize="50"
         AllowMultiColumnSorting="true"  >
            <Columns>
                <telerik:GridBoundColumn DataField="Account_Manager" HeaderStyle-Width="15%" 
                     HeaderText='Account Manager' SortExpression="Account_Manager" UniqueName="Account_Manager" 
                     ReadOnly="True" /> 
                <telerik:GridBoundColumn DataField="Plan_Name" HeaderStyle-Width="19%" 
                     HeaderText='Account Name' SortExpression="Plan_Name" UniqueName="Plan_Name" 
                     ReadOnly="True" /> 
                <telerik:GridBoundColumn DataField="Contact_Date" HeaderStyle-Width="11%" 
                     HeaderText='Date' SortExpression="Contact_Date" 
                     UniqueName="Contact_Date" DataType="System.DateTime" DataFormatString="{0:M/dd/yyyy}" ReadOnly="True" /> 	
                <telerik:GridBoundColumn DataField="Meeting_Activity_Name" HeaderStyle-Width="17%" 
                     HeaderText='Meeting Activity' SortExpression="Meeting_Activity_Name" 
                     UniqueName="Meeting_Activity_Name" ReadOnly="True" /> 
                <telerik:GridBoundColumn DataField="Persons_Met" HeaderStyle-Width="27%" 
                     HeaderText='Persons Met' SortExpression="Persons_Met" 
                     UniqueName="Persons_Met" ReadOnly="True" /> 
                <telerik:GridBoundColumn DataField="Followup_Date" HeaderStyle-Width="11%" 
                     HeaderText='Follow-up' SortExpression="Followup_Date" 
                     UniqueName="Followup_Date" DataType="System.DateTime" DataFormatString="{0:M/dd/yyyy}" ReadOnly="True" /> 	
             </Columns>        
            <PagerStyle Visible="false" />
            
            <SortExpressions>
                <telerik:GridSortExpression FieldName="Plan_Name" SortOrder="Ascending"  />     
             </SortExpressions>   
        </MasterTableView>
   

        <ClientSettings>        
            <DataBinding Location="~/custom/csl/customercontactreports/services/CSLDataService.svc" DataService-TableName="ContactReportDataSet" 
                SelectCountMethod="GetCRDrillDownCount" />    
            <Scrolling AllowScroll="True" UseStaticHeaders="True"  />
        </ClientSettings>    
    </telerik:RadGrid>
   <pinso:RadGridWrapper runat="server" ID="radGridWrapper" Target="gridMeetingTypeDrillDown"  RequiresFilter="true" AutoUpdate="true" PagingSelector="#divTile5Container .pagination" DrillDownLevel="1" UtcDateColumns="Contact_Date,Followup_Date" />
