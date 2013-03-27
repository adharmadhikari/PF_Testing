<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CCRDrilldownData.ascx.cs" Inherits="custom_controls_CCRDrilldownData" %>

  <telerik:RadGrid SkinID="radTable" runat="server" ID="gridCR" AllowSorting="true"   
        PageSize="50" AllowPaging="true" AllowFilteringByColumn="false" EnableEmbeddedSkins="false" Width="100%" >
        <MasterTableView AutoGenerateColumns="False" ClientDataKeyNames="Plan_Name, Geography_Name, Contact_Date, Meeting_Activity_Name, Meeting_Type_Name,Meeting_Outcome_Name,Followup_Notes_Name" PageSize="50"
         AllowMultiColumnSorting="true" Width="100%" >
            <Columns>
            <telerik:GridBoundColumn DataField="Account_Manager" HeaderStyle-Width="10%" ItemStyle-CssClass="firstCol planName"
                     HeaderText='Account Manager'  UniqueName="Account_Manager" 
                     DataType="System.String" /> 
             <telerik:GridBoundColumn DataField="Plan_Name" HeaderText="Account Name" ItemStyle-CssClass="planName"
                          UniqueName="Plan_Name" DataType="System.String" HeaderStyle-Width="12%" />                    
             
             <telerik:GridBoundColumn DataField="Geography_Name" HeaderText="Geography" ItemStyle-CssClass="geogName"
                          UniqueName="Geography_Name" DataType="System.String" HeaderStyle-Width="10%"></telerik:GridBoundColumn>

             <telerik:GridBoundColumn DataField="Contact_Date" HeaderText='<%$ Resources:Resource, Label_CR_Date %>' ItemStyle-CssClass="alignRight contactDT"
                         UniqueName="Contact_Date" DataType ="System.DateTime" DataFormatString="{0:M/dd/yyyy}" HeaderStyle-Width="8%"> </telerik:GridBoundColumn>         
               
             <telerik:GridBoundColumn DataField="Section_Name" HeaderText="Market Segment" ItemStyle-CssClass="sectName"
                        UniqueName="Section_Name" DataType="System.String" HeaderStyle-Width="10%" />
                 
             <telerik:GridBoundColumn DataField="Meeting_Activity_Name" HeaderText='<%$ Resources:Resource, Label_Meeting_Activity %>' ItemStyle-CssClass="meetActivity"
                        UniqueName="Meeting_Activity_Name"  DataType ="System.String" HeaderStyle-Width="10%"/>  
                               
             <telerik:GridBoundColumn DataField="Meeting_Type_Name" HeaderText='<%$ Resources:Resource, Label_Meeting_Type %>' ItemStyle-CssClass="meetType"
                        UniqueName="Meeting_Type_Name" DataType = "System.String" HeaderStyle-Width="10%"/>       
                       
             <telerik:GridBoundColumn DataField="Followup_Notes_Name" HeaderText="Followup Notes" ItemStyle-CssClass="alignRight"
                        UniqueName="Followup_Notes_Name" DataType = "System.String" HeaderStyle-Width="10%"/>    
             
             <telerik:GridBoundColumn DataField="Meeting_Outcome_Name" HeaderText="Meeting Outcome" ItemStyle-CssClass="alignRight"
                        UniqueName="Meeting_Outcome_Name" DataType = "System.String" HeaderStyle-Width="10%"/>                           
                                       
             <telerik:GridBoundColumn DataField="Drug_Name" HeaderText="Drug" HeaderStyle-Width="10%" ItemStyle-CssClass="notmerged drugName" 
                       UniqueName="Drug_Name" DataType="System.String"></telerik:GridBoundColumn>                
           
            </Columns>
            <PagerStyle Visible="false" />
            
            <SortExpressions>
          <telerik:GridSortExpression FieldName="Account_Manager" />
                <telerik:GridSortExpression FieldName="Plan_Name" SortOrder="Ascending" />
                <telerik:GridSortExpression FieldName="Geography_Name" />   
                <telerik:GridSortExpression FieldName="Contact_Date" />
                <telerik:GridSortExpression FieldName="Section_Name" />
                <telerik:GridSortExpression FieldName="Meeting_Activity_Name" />
                <telerik:GridSortExpression FieldName="Meeting_Type_Name" />
                <telerik:GridSortExpression FieldName="Followup_Notes_Name" />
                <telerik:GridSortExpression FieldName="Meeting_Outcome_Name" />
                <telerik:GridSortExpression FieldName="Drug_Name" />            
            </SortExpressions>
        </MasterTableView>
   

       <ClientSettings >
            <DataBinding Location="~/custom/auxilium/customercontactreports/services/auxiliumDataService.svc" DataService-TableName="ContactReportDataSet" 
                />  
             
            <Scrolling AllowScroll="True" UseStaticHeaders="True" />
           
        </ClientSettings> 
    </telerik:RadGrid>

   <pinso:RadGridWrapper runat="server" ID="radGridWrapper" Target="gridCR"  MergeRows="false" PagingSelector="#divTile3Container .pagination" RequiresFilter="true" AutoUpdate="true" UtcDateColumns="Contact_Date" />
