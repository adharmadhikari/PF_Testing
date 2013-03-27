<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterChannelPlans.ascx.cs" Inherits="marketplaceanalytics_controls_FilterChannelPlans" %>
<div> 
    <div class="filterGeo">
    <asp:Literal runat="server" ID="filterLabel" Text="Channel" />
    </div>
    
     <telerik:RadComboBox runat="server" ID="Section_ID"  DataTextField="Name" DataValueField="ID"  AppendDataBoundItems="true" Skin="pathfinder" EnableEmbeddedSkins="false" DropDownWidth="185px">
        
    </telerik:RadComboBox>
    <div id="planContainer">
    <div class="filterGeo">
        <asp:Literal runat="server" ID="litPlan1" Text="First Account Name" />
    </div>
    <telerik:RadComboBox runat="server" ID="Plan_ID1"  DataTextField="Name" DataValueField="ID" AppendDataBoundItems="true" Skin="pathfinder" EnableEmbeddedSkins="false" DropDownWidth="375px" ></telerik:RadComboBox>
   
    <telerik:RadComboBox runat="server" ID="Plan_ID2"  DataTextField="Name" DataValueField="ID" AppendDataBoundItems="true" Skin="pathfinder" EnableEmbeddedSkins="false" DropDownWidth="375px" >
    </telerik:RadComboBox>
    
   </div>
</div>