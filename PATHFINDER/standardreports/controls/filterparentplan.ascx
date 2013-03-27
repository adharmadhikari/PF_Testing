<%@ Control Language="C#" AutoEventWireup="true" CodeFile="filterparentplan.ascx.cs" Inherits="standardreports_controls_filtertieraffiliationtype" %>
<div>
    &nbsp;&nbsp;<asp:Literal runat="server" ID="ParentPlanfilterLabel" Text='Parent Plan'  />
</div>
<telerik:RadComboBox runat="server" ID="Parent_ID" DataTextField="Plan_Name" DataValueField="Parent_ID" AppendDataBoundItems="true" Skin="pathfinder" EnableEmbeddedSkins="false" MaxHeight="200px">
<Items>
</Items>
</telerik:RadComboBox>

