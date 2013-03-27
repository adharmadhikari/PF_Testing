<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterGeographyMedD.ascx.cs" Inherits="standardreports_controls_FilterGeographyMedD" %>
<div id="filterGeography">
    <div class="filterGeo">
    <asp:Literal runat="server" ID="filterLabel" Text='<%$ Resources:Resource, Label_Geography %>'  />
    </div>
    <telerik:RadComboBox runat="server" ID="rcbGeographyType" CssClass="queryExt" Skin="pathfinder" EnableEmbeddedSkins="false" MaxHeight="200px">
        <Items>
            <telerik:RadComboBoxItem runat="server" Value="1" Text="National" />
            <telerik:RadComboBoxItem runat="server" Value="2" Text="Regional" />
            <telerik:RadComboBoxItem runat="server" Value="3" Text="State" />
            <telerik:RadComboBoxItem runat="server" Value="4" Text="Account Manager" />
            <telerik:RadComboBoxItem runat="server" Value="5" Text="MA Regions" />
            <telerik:RadComboBoxItem runat="server" Value="6" Text="PDP Regions" />
        </Items>        
    </telerik:RadComboBox>
    <telerik:RadComboBox runat="server" ID="Geography_ID" Skin="pathfinder" CssClass="string"  EnableEmbeddedSkins="false" MaxHeight="200px" /> 
    <telerik:RadComboBox runat="server" ID="MA_Region_ID" Skin="pathfinder" CssClass="string"  EnableEmbeddedSkins="false" MaxHeight="200px" />  
    <telerik:RadComboBox runat="server" ID="Region_ID" Skin="pathfinder" CssClass="string"  EnableEmbeddedSkins="false" MaxHeight="200px" />                                                    
</div>