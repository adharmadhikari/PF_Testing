<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ActivityReportingDataTemplate.ascx.cs" Inherits="custom_controls_ActivityReportingDataTemplate" %>

<telerik:RadGrid SkinID="radTable" OnPreRender="gridActivityType_PreRender" runat="server" ID="gridActivityType" AllowSorting="False"  AllowFilteringByColumn="false" AutoGenerateColumns="False"
 AllowPaging="True" EnableEmbeddedSkins="False" GridLines="None">
    <MasterTableView PageSize="50" AutoGenerateColumns="false" AllowSorting="false" ClientDataKeyNames="Activity_Type_ID">   
         <Columns>
            <telerik:GridBoundColumn DataField="Activity_Type_ID" 
                UniqueName="Activity_Type_ID" HeaderStyle-Width="5%" HeaderText=' ' />
            <telerik:GridBoundColumn DataField="Activity_Type_Name" HeaderStyle-Width="47%" 
                HeaderText='Activity Name' UniqueName="Activity_Type_Name" />             
            <telerik:GridBoundColumn DataField="Activity_Hours" HeaderStyle-Width="22%" 
                HeaderText='# Of Hours' UniqueName="Activity_Hours" DataType="System.Int32" /> 
            <telerik:GridBoundColumn DataField="Activity_Hours_Percent" HeaderStyle-Width="26%" 
                HeaderText='% Of Calls' DataFormatString="{0:F2}%"  
                UniqueName="Activity_Hours_Percent" DataType="System.Double" /> 
         </Columns>        
    </MasterTableView>
    <ClientSettings ClientEvents-OnRowSelecting="grid_OnRowSelecting">
        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="230px" />
        <Selecting AllowRowSelect="true" /> 
    </ClientSettings>
</telerik:RadGrid>
