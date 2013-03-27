<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterSection.ascx.cs" Inherits="standardreports_controls_FilterSection" %>
<div class="filterGeo">
    <asp:Literal runat="server" ID="filterLabel" Text="Select a Market Segment" />
</div>
<telerik:RadComboBox ID="Section_ID" runat="server" EnableEmbeddedSkins="false" Skin="pathfinder"  DropDownWidth="190px"    
    DataTextField="Name" DataValueField="ID" AppendDataBoundItems="true" OnClientDropDownClosed="sectionDropDownClosed">
</telerik:RadComboBox>

 
    
   