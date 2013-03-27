<%@ Control Language="C#" AutoEventWireup="true" CodeFile="filteraccountmanagers.ascx.cs" Inherits="custom_Alcon_activityreporting_controls_filteraccountmanagers" %>
<asp:PlaceHolder runat="server" id="placeholder">
    <div class="filterGeo">
        <asp:Literal runat="server" ID="filterLabel" Text='<%$ Resources:Resource, Label_Account_Manager %>' />
    </div>
    <telerik:RadComboBox runat="server" ID="User_ID" Skin="pathfinder" EnableEmbeddedSkins="false" MaxHeight="300px" >
    </telerik:RadComboBox> 
</asp:PlaceHolder>


 