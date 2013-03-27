<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterMeetingOutcome.ascx.cs" Inherits="custom_controls_FilterMeetingOctcome" %>
<div class="filterGeo">
    <asp:Literal runat="server" ID="filterLabel" Text="Meeting Outcome" />
    
</div>
<telerik:RadComboBox ID="Outcome_ID" runat="server" EnableEmbeddedSkins="false" Skin="pathfinder" DataSourceID="dsOutcome" DataTextField="Meeting_Outcome_Name" DataValueField="Meeting_Outcome_ID" AppendDataBoundItems="true">
<Items>
  <telerik:RadComboBoxItem Text="--All Meeting Types--" Value="" />
</Items>
</telerik:RadComboBox>
<asp:EntityDataSource ID="dsOutcome" runat="server" 
    ConnectionString="name=PathfinderClientEntities" 
    DefaultContainerName="PathfinderClientEntities" 
    EntitySetName="LkpMeetingOutcomeSet" 
    Select="it.[Meeting_Outcome_ID], it.[Meeting_Outcome_Name], it.[Client_ID], it.[Status]" OrderBy="it.[Meeting_Outcome_Name]"></asp:EntityDataSource>

