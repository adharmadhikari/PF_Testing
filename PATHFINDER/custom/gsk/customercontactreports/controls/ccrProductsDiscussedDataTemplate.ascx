<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ccrProductsDiscussedDataTemplate.ascx.cs" Inherits="custom_controls_ccrProductsDiscussedDataTemplate" %>

<telerik:RadGrid OnPreRender="gridCcrProductsDiscussed_PreRender" SkinID="radTable" runat="server" ID="gridCcrProductsDiscussed" AllowSorting="False"  AllowFilteringByColumn="false" AutoGenerateColumns="False"
 AllowPaging="True" EnableEmbeddedSkins="False" GridLines="None">
   
<MasterTableView AutoGenerateColumns="false" AllowSorting="false" ClientDataKeyNames="Products_Discussed_ID" PageSize="10">   
         <Columns>
            <telerik:GridBoundColumn DataField="Products_Discussed_ID" 
                UniqueName="Products_Discussed_ID" HeaderStyle-Width="5%" HeaderText=' '  />
            <telerik:GridBoundColumn DataField="Drug_Name" HeaderStyle-Width="47%" 
                 HeaderText='Product Discussed' UniqueName="Drug_Name" />             
            <telerik:GridBoundColumn DataField="RecordCount" HeaderStyle-Width="22%" 
                 HeaderText='# Of Calls' UniqueName="RecordCount" DataType="System.Int32" /> 
            <telerik:GridBoundColumn DataField="User_ID_Percent" HeaderStyle-Width="26%" 
                 HeaderText='% Of Calls' DataFormatString="{0:F2}%" UniqueName="User_ID_Percent" DataType="System.Double" />              
         </Columns>        
</MasterTableView>
   <ClientSettings ClientEvents-OnRowSelecting="gridCcrProductsDiscussed_OnRowSelecting">
          <Scrolling AllowScroll="true" UseStaticHeaders="true"/>
          <Selecting AllowRowSelect="true" /> 
    </ClientSettings>
</telerik:RadGrid>
