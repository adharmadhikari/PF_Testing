<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterProducts.ascx.cs" Inherits="custom_controls_FilterProducts" %>
<div class="filterGeo">
    <asp:Literal runat="server" ID="filterLabel" Text="Products" />
</div>
<telerik:RadComboBox ID="Products_Discussed_ID" runat="server" EnableEmbeddedSkins="false" Skin="pathfinder" AppendDataBoundItems="true">
</telerik:RadComboBox>
<pinso:ClientValidator runat="server" id="validator1" target="Products_Discussed_ID" DataField="Products_Discussed_ID" Required="true" Text='<%$ Resources:Resource, Message_Required_ProductSelection %>' />
