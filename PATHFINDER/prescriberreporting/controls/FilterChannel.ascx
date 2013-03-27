<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterChannel.ascx.cs" Inherits="prescriberreporting_controls_FilterChannel" %>
<div>    
  
    <div class="filterGeo">
    <asp:Literal runat="server" ID="filterLabel" Text="Channel" />
    </div>
    
     <telerik:RadComboBox runat="server" ID="Section_ID"  DataTextField="Name" DataValueField="ID"  AppendDataBoundItems="true" Skin="pathfinder" EnableEmbeddedSkins="false" DropDownWidth="185px">
        <Items>
            <telerik:RadComboBoxItem Text="Combined (Commercial + Part D)" Value="-1"/> 
        </Items>
    </telerik:RadComboBox>    
</div>