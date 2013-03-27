<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterRegion.ascx.cs" Inherits="custom_controls_FilterRegion" %>
<div id="filterGeography">
    <div class="filterGeo">
    <asp:Literal runat="server" ID="filterLabel" Text="Select Region"/>
    </div>
    <telerik:RadComboBox runat="server" ID="rcbGeographyType" CssClass="notfilter" Skin="pathfinder" EnableEmbeddedSkins="false" MaxHeight="200px" >
        <Items>
            <telerik:RadComboBoxItem runat="server" Value="1" Text="National" />
            <%--<telerik:RadComboBoxItem runat="server" Value="2" Text="Regional" />--%>
            <telerik:RadComboBoxItem runat="server" Value="3" Text="State" />
        </Items>        
    </telerik:RadComboBox>
        <%--<div class="filterGeo">
    <asp:Literal runat="server" ID="ltSelectTerr" Text="Select Territory"/>
    </div>--%>
    
    <telerik:RadComboBox runat="server" ID="Geography_ID" Skin="pathfinder" CssClass="string"  EnableEmbeddedSkins="false" MaxHeight="200px" />           
</div>