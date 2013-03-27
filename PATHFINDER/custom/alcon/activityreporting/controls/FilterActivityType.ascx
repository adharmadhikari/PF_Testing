<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterActivityType.ascx.cs" Inherits="custom_controls_FilterMeetingActivity" %>

<div class="filterGeo">
    <asp:Literal runat="server" ID="filterLabel" Text="Activity Type" />    
</div>
<telerik:RadComboBox ID="Activity_Type_ID" runat="server" EnableEmbeddedSkins="false" Skin="pathfinder">
</telerik:RadComboBox>


