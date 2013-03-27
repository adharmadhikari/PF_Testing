<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ccrMeetingActivityDataTemplate.ascx.cs" Inherits="custom_controls_ccrMeetingActivityDataTemplate" %>

<telerik:RadGrid OnPreRender="gridCcrMeetingActivity_PreRender" SkinID="radTable" runat="server" ID="gridCcrMeetingActivity" AllowSorting="False"  AllowFilteringByColumn="false" AutoGenerateColumns="False"
 AllowPaging="True" EnableEmbeddedSkins="False" GridLines="None">
    <MasterTableView AutoGenerateColumns="false" AllowSorting="false" ClientDataKeyNames="Meeting_Activity_ID, Products_Discussed_ID, Meeting_Activity_Name">   
         <Columns>
            <telerik:GridBoundColumn DataField="Meeting_Activity_ID" 
                UniqueName="Meeting_Activity_ID" HeaderStyle-Width="5%" HeaderText=' ' />
            <telerik:GridBoundColumn DataField="Meeting_Activity_Name" HeaderStyle-Width="47%" 
                HeaderText='Activity Name' UniqueName="Meeting_Activity_Name" />             
            <telerik:GridBoundColumn DataField="RecordCount" HeaderStyle-Width="22%" 
                HeaderText='# Of Calls' UniqueName="RecordCount" DataType="System.Int32" /> 
            <telerik:GridBoundColumn DataField="User_ID_Percent" HeaderStyle-Width="26%" 
                HeaderText='% Of Calls' DataFormatString="{0:F2}%"  
                UniqueName="User_ID_Percent" DataType="System.Double" /> 
         </Columns>        
    </MasterTableView>
    <ClientSettings ClientEvents-OnRowSelecting="gridCcrMeetingActivity_OnRowSelecting">
        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="230px" />
        <Selecting AllowRowSelect="true" /> 
    </ClientSettings>
</telerik:RadGrid>
