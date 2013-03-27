<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterRollup.ascx.cs" Inherits="prescriberreporting_controls_FilterRollup" %>
<div id="filterRollup">
    <div class="filterGeo">
    <asp:Literal runat="server" ID="filterLabel" Text='Rollup Type' />
    </div>
    <telerik:RadComboBox runat="server" ID="Rollup_Type" Skin="pathfinder" EnableEmbeddedSkins="false" MaxHeight="200px" 
    >
        <Items>
            <telerik:RadComboBoxItem runat="server" Value="1" Text="All" />
            <telerik:RadComboBoxItem runat="server" Value="5" Text="Top 50 Prescribers" />
            <telerik:RadComboBoxItem runat="server" Value="6" Text="Top 100 Prescribers" />
        </Items>        
    </telerik:RadComboBox>
</div>