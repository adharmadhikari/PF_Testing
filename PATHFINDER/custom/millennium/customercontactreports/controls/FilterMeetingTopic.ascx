<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterMeetingTopic.ascx.cs" Inherits="custom_millennium_customercontactreports_controls_FilterMeetingTopic" %>

<div class="filterGeo">
    <asp:Literal runat="server" ID="filterLabel" Text="Meeting Topic" />
    
</div>
<telerik:RadComboBox ID="Meeting_Activity_ID" runat="server" EnableEmbeddedSkins="false" Skin="pathfinder" DataTextField="Meeting_Activity_Name" DataValueField="Meeting_Activity_ID" AppendDataBoundItems="true">
<Items>
  <telerik:RadComboBoxItem Text="--All Meeting Topics--" Value="" />
</Items>
</telerik:RadComboBox>
<%--<asp:EntityDataSource ID="dsMeetType" runat="server" 
    ConnectionString="name=PathfinderClientEntities" 
    DefaultContainerName="PathfinderClientEntities" 
    EntitySetName="LkpMeetingTypeSet" 
    Select="it.[Meeting_Type_ID], it.[Meeting_Type_Name], it.[Client_ID]" OrderBy="it.[Meeting_Type_Name]"></asp:EntityDataSource>--%>


