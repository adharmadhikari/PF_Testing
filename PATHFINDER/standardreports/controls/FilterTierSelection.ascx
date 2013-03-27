<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterTierSelection.ascx.cs" Inherits="standardreports_controls_FilterTierSelection" %>
<div id="filterTier" runat="server">
<div class="filterGeo">
    <asp:Literal runat="server" ID="filterLabel" Text='<%$ Resources:Resource, Label_TierCoverage %>'  />
    </div>
<telerik:RadComboBox ID="Tier_ID" runat="server" EnableEmbeddedSkins="false" Skin="pathfinder" OnClientLoad="function(s,a){ $createCheckboxDropdown(s.get_id(), 'TierIDList', _tierSet, { 'allOptionID': '-1' }, null, 'moduleOptionsContainer'); }"  Height="160px" />
</div>