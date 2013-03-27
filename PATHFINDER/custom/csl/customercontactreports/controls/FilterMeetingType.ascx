<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterMeetingType.ascx.cs" Inherits="custom_controls_FilterMeetingType" %>
<div class="filterGeo">
    <asp:Literal runat="server" ID="filterLabel" Text="Meeting Type" />
    
</div>
<telerik:RadComboBox ID="Meeting_Type_ID" runat="server" EnableEmbeddedSkins="false" Skin="pathfinder" DataTextField="Meeting_Type_Name" DataValueField="Meeting_Type_ID" AppendDataBoundItems="true">
<Items>
  <telerik:RadComboBoxItem Text="--All Meeting Types--" Value="" />
</Items>
</telerik:RadComboBox>
<%--<asp:EntityDataSource ID="dsMeetType" runat="server" 
    ConnectionString="name=PathfinderClientEntities" 
    DefaultContainerName="PathfinderClientEntities" 
    EntitySetName="LkpMeetingTypeSet" 
    Select="it.[Meeting_Type_ID], it.[Meeting_Type_Name], it.[Client_ID]" OrderBy="it.[Meeting_Type_Name]"></asp:EntityDataSource>--%>

