<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterMeetingOutcome.ascx.cs" Inherits="custom_controls_FilterMeetingOctcome" %>
<div class="filterGeo">
    <asp:Literal runat="server" ID="filterLabel" Visible="false" Text="Meeting Outcome" />
    
</div>
<telerik:RadComboBox ID="Meeting_Outcome_ID" runat="server" Visible="false" EnableEmbeddedSkins="false" Skin="pathfinder" AppendDataBoundItems="true"  CssClass="queryExt">

</telerik:RadComboBox>

