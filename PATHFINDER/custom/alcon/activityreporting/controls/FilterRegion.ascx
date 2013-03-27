<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterRegion.ascx.cs" Inherits="custom_controls_FilterRegion" %>
<div id="filterGeography">
    <div class="filterGeo">
    <asp:Literal runat="server" ID="filterLabel" Text="Select Geography"/>
    </div>
     <telerik:RadComboBox ID="Territory_ID" CssClass="string" runat="server" EnableEmbeddedSkins="false" Skin="pathfinder" Height="160px" />
</div>