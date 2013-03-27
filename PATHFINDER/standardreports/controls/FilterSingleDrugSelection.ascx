<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterSingleDrugSelection.ascx.cs" Inherits="standardreports_controls_FilterSingleDrugSelection" %>

<!-- READ ME: Client side events for Market Basket and Drug ID controls are set in code behind -->
<div class="filterGeo">
    <asp:Literal runat="server" ID="filterLabel" Text='<%$ Resources:Resource, Label_DrugSelection %>'  />
    </div>
<telerik:RadComboBox ID="Market_Basket_ID" EnableEmbeddedSkins="false" SkinID="standardReportsTruncate" Skin="pathfinder" DropDownWidth="300px" MaxHeight="225px" EnableViewState="false" runat="server" AppendDataBoundItems="true">
    
    <Items>
        <telerik:RadComboBoxItem runat="server" Value="" Text='<%$ Resources:Resource, Label_ListItem_Therapeutic_Class_All %>' />
    </Items>
</telerik:RadComboBox>                  

<div class="filterGeo">
    <asp:Literal runat="server" ID="Literal1" Text='<%$ Resources:Resource, Label_DrugID %>'  />
    </div>
<telerik:RadComboBox ID="Drug_ID" runat="server" EnableEmbeddedSkins="false" Skin="pathfinder" MaxHeight="225px" Height="160px">
</telerik:RadComboBox>

<pinso:ClientValidator runat="server" id="validator1" target="Drug_ID" DataField="Drug_ID" Required="true" Text='<%$ Resources:Resource, Message_Required_DrugSelection %>' />
