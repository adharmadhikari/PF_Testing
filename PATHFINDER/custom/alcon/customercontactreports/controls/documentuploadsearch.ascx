<%@ Control Language="C#" AutoEventWireup="true" CodeFile="documentuploadsearch.ascx.cs" Inherits="custom_Alcon_customercontactreports_Controls_documentuploadsearch" %>
<div id="documentuploadsearch">
    <telerik:RadGrid SkinID="radTable" runat="server" ID="gridCCDocuments" AllowSorting="True" EnableEmbeddedSkins="False" PageSize="50" GridLines="None">        
        <MasterTableView autogeneratecolumns="false" ClientDataKeyNames="Document_ID" AllowPaging="true" Width="100%" PageSize="50">
            
            <Columns>
             <telerik:GridBoundColumn DataField="section_name" HeaderText='Account Type'  SortExpression="section_name" UniqueName="section_name" DataType="System.String"></telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="Plan_Name" HeaderText='Account Name'  SortExpression="Plan_Name" UniqueName="Plan_Name" DataType="System.String"></telerik:GridBoundColumn>
            
            <telerik:GridBoundColumn DataField="Created_BY" HeaderText='Account Manager'  SortExpression="Created_BY" UniqueName="Created_BY" DataType="System.String"></telerik:GridBoundColumn>
            
                <telerik:GridBoundColumn DataField="Contact_Date" HeaderText='Document Date'  SortExpression="Contact_Date" UniqueName="Contact_Date" DataType ="System.DateTime" DataFormatString="{0:M/dd/yyyy}" /> 
               
                <telerik:GridHyperlinkColumn DataNavigateUrlFormatString='javascript:viewDocument1({0});' 
                DataNavigateUrlFields="Document_ID" 
                DataTextFormatString='{0}' DataTextField="Document_Name"
                
                HeaderText='Document Name'  
                SortExpression="Document_Name" 
                UniqueName="Document_Name" 
                DataType="System.String"></telerik:GridHyperlinkColumn>
                
               
               <%-- <telerik:GridTemplateColumn DataField="Document_Name" HeaderText='Document Name'  SortExpression="Document_Name" UniqueName="Document_Name" >
                <ItemTemplate>
                
                <asp:HyperLink NavigateUrl='<%# string.Format("javascript:viewDocument1({0})", Eval("Document_ID"))%>' runat="server" Text='<%#string.Format("{0}", Eval("Document_Name")) %>' ></asp:HyperLink>
                </ItemTemplate>
                
                </telerik:GridTemplateColumn>--%>
                <telerik:GridBoundColumn DataField="Document_Type_Name" HeaderText='Document Type'  SortExpression="Document_Type_Name" UniqueName="Document_Type_Name" DataType="System.String"></telerik:GridBoundColumn>
               
            </Columns>           
            <SortExpressions>
                <telerik:GridSortExpression FieldName="Contact_Date" SortOrder="Descending" />
            </SortExpressions>
        </MasterTableView>
        <PagerStyle Visible="false" /> 
        <ClientSettings>
            <DataBinding Location="~/custom/Alcon/customercontactreports/services/AlconDataService.svc" DataService-TableName="PlanDocumentsViewSet" SelectCountMethod ="GetDocumentCount">
            </DataBinding>
            <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="145px" /> 
            <Selecting AllowRowSelect="true" />
        </ClientSettings>           
    </telerik:RadGrid>
    <pinso:RadGridWrapper runat="server" ID="gridWrapper" Target="gridCCDocuments" PagingSelector="#tile7ContainerHeader .pagination" MergeRows="false"  RequiresFilter ="true" AutoLoad="true" UtcDateColumns="Contact_Date" ShowNumberOfRecords="false" />
</div> 