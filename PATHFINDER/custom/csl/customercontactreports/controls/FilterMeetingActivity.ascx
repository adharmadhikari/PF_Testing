<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterMeetingActivity.ascx.cs" Inherits="custom_controls_FilterMeetingActivity" %>

<div class="filterGeo">
    <asp:Literal runat="server" ID="filterLabel" Text="Meeting Activity" />
    
</div>
<telerik:RadComboBox ID="Meeting_Activity_ID" runat="server" EnableEmbeddedSkins="false" Skin="pathfinder" DataTextField="Meeting_Activity_Name" DataValueField="Meeting_Activity_ID" AppendDataBoundItems="true">
<Items>
  <telerik:RadComboBoxItem Text="--All Meeting Activities--" Value="" />
</Items>
</telerik:RadComboBox>


