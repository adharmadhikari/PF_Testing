<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterTierAsOption.ascx.cs" Inherits="standardreports_controls_FilterTierAsOption" %>

<%--<pinso:CheckboxValueList  CssClass="chkBoxDiv"  runat="server" ID="Tier_ID" DataValueFormatString="t{0}" HasAllOption="true" DataSourceID="dsTiers" DataTextField="Name" DataValueField="ID" TrueFalse="true" BreakCount="4" />
<pinso:CheckboxListClientControl runat="server" ID="tierOptionList" Target="Tier_ID" ContainerID="moduleOptionsContainer" />
<asp:EntityDataSource runat="server" ID="dsTiers" DefaultContainerName="PathfinderEntities" ConnectionString="name=PathfinderEntities" EntitySetName="TierSet" EntityTypeFilter="Tier" />--%>
<div class="filterGeo">
    <asp:Literal runat="server" ID="filterLabel" Text='<%$ Resources:Resource, Label_TierOption %>'  />
    </div>
<telerik:RadComboBox ID="Tier_ID" runat="server" EnableEmbeddedSkins="false" Skin="pathfinder" OnClientLoad="function(s,a){ $createCheckboxDropdown(s.get_id(), 'TierIDOptionList', _tierSet, {'booleanOptions':true, 'optionNameFormat':'t{0}'}, null, 'moduleOptionsContainer'); }"  Height="160px" />