<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterRollup.ascx.cs" Inherits="todaysanalytics_controls_FilterRollup" %>
<div id="filterRollup">
    <div class="filterGeo">
    <asp:Literal runat="server" ID="filterLabel" Text='Rollup Type' />
    </div>
    <telerik:RadComboBox runat="server" ID="Rollup_Type" Skin="pathfinder" EnableEmbeddedSkins="false" MaxHeight="200px" 
    >
        <Items>
            <telerik:RadComboBoxItem runat="server" Value="1" Text="All" />
            <telerik:RadComboBoxItem runat="server" Value="2" Text="Top 10 Plans" />
            <telerik:RadComboBoxItem runat="server" Value="3" Text="Top 20 Plans" />
            <telerik:RadComboBoxItem runat="server" Value="4" Text="By Account" />
        </Items>        
    </telerik:RadComboBox>
</div>