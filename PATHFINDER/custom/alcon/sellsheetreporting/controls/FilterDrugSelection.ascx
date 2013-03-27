<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterDrugSelection.ascx.cs" Inherits="custom_controls_FilterDrugSelection" %>


<!-- READ ME: Client side events for Market Basket and Drug ID controls are set in alcon.js -->
<div class="filterGeo">
    <asp:Literal runat="server" ID="filterLabel" Text="Market Basket"  />
    </div>
<telerik:RadComboBox ID="Thera_ID" EnableEmbeddedSkins="false" Skin="pathfinder" 
    DropDownWidth="190px" MaxHeight="225px" Height="160px"  runat="server" AppendDataBoundItems="true"
    CssClass="queryExt" OnClientDropDownClosed="UpdateDrugList" OnClientLoad="onMBLoad"> 
</telerik:RadComboBox>                  

<div class="filterGeo">
    <asp:Literal runat="server" ID="Literal1" Text="Product"  />
</div>
<telerik:RadComboBox ID="Drug_ID" runat="server" EnableEmbeddedSkins="false" Skin="pathfinder" MaxHeight="225px" Height="160px" 
    CssClass="queryExt">
</telerik:RadComboBox>


<%--<pinso:ClientValidator runat="server" id="validator1" target="Drug_ID" DataField="Drug_ID" Required="true" Text='<%$ Resources:Resource, Message_Required_DrugSelection %>' />
--%>