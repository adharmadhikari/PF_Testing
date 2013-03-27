<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ccrMeetingTypeDataTemplate.ascx.cs" Inherits="custom_controls_MeetingTypeDataTemplate" %>

<telerik:RadGrid OnPreRender="gridCcrMeetingType_PreRender" SkinID="radTable" runat="server" ID="gridCcrMeetingType" AllowSorting="False"  AllowFilteringByColumn="false" AutoGenerateColumns="False"
 AllowPaging="True" EnableEmbeddedSkins="False" GridLines="None">
    <MasterTableView AutoGenerateColumns="false" AllowSorting="false" ClientDataKeyNames="Meeting_Type_ID, Products_Discussed_ID" >
         <Columns>
            <telerik:GridBoundColumn DataField="Meeting_Type_ID" 
                UniqueName="Meeting_Type_ID" HeaderStyle-Width="5%" HeaderText=' ' />
            <telerik:GridBoundColumn DataField="Meeting_Type_Name" HeaderStyle-Width="47%" 
                HeaderText="Meeting Type Name" UniqueName="Meeting_Type_Name" /> 
            <telerik:GridBoundColumn DataField="RecordCount" HeaderStyle-Width="22%" 
                HeaderText='# Of Calls' UniqueName="RecordCount" DataType="System.Int32" /> 
            <telerik:GridBoundColumn DataField="User_ID_Percent" HeaderStyle-Width="26%" 
                HeaderText='% Of Calls' DataFormatString="{0:F2}%" 
                UniqueName="User_ID_Percent" DataType="System.Double" /> 
         </Columns>
    </MasterTableView>                  
    <ClientSettings ClientEvents-OnRowSelecting="gridMeetingType_OnRowSelecting">
        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="230px" />
        <Selecting AllowRowSelect="true" /> 
    </ClientSettings>    
</telerik:RadGrid>


