<%@ Control Language="C#" AutoEventWireup="true" CodeFile="filterbenefitdesign.ascx.cs" Inherits="standardreports_controls_filterbenefitdesign" %>
<asp:PlaceHolder runat="server" ID="placeholder">
    <div class="filterGeo">
        <asp:Literal runat="server" ID="filterLabel" Text='Benefit Design' />
    </div>
    <telerik:RadComboBox runat="server" ID="Is_Predominant" CssClass="queryExt" EnableEmbeddedSkins="false" Skin="pathfinder"
        MaxHeight="300px">
        <Items>
            <telerik:RadComboBoxItem Text="--All--" Value="" />
            <telerik:RadComboBoxItem Text="Predominant" Value="1" />           
        </Items>
    </telerik:RadComboBox>
    
</asp:PlaceHolder>