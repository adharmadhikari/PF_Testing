<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterProductsForDrilldown.ascx.cs" Inherits="custom_controls_FilterProductsForDrilldown" %>
<div class="filterGeo">
    <asp:Literal runat="server" ID="filterLabel" Text="Products" />
</div>
<telerik:RadComboBox ID="Products_Discussed_ID" runat="server" EnableEmbeddedSkins="false" Skin="pathfinder" AppendDataBoundItems="true" CssClass="queryExt">
</telerik:RadComboBox>
