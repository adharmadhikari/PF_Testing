<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterMeetingFollowup.ascx.cs" Inherits="custom_controls_FilterMeetingFollowup" %>
<div class="filterGeo">
    <asp:Literal runat="server" ID="filterLabel" Text="Meeting Followup" />
    
</div>
<telerik:RadComboBox ID="Followup_Notes_ID" runat="server" EnableEmbeddedSkins="false" Skin="pathfinder" AppendDataBoundItems="true" DropDownWidth="290px">

</telerik:RadComboBox>
<%--<asp:EntityDataSource ID="dsFollowup" runat="server" 
    ConnectionString="name=PathfinderClientEntities" 
    DefaultContainerName="PathfinderClientEntities" 
    EntitySetName="LkpFollowupNotesSet" 
    Select="it.[Followup_Notes_ID], it.[Followup_Notes_Name], it.[Client_ID], it.[Status]" OrderBy="it.[Followup_Notes_Name]"></asp:EntityDataSource>
--%>