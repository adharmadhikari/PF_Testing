<%@ Control Language="C#" AutoEventWireup="true" CodeFile="businessplanning.ascx.cs" Inherits="custom_reckitt_businessplanning_controls_businessplanning" %>
<asp:PlaceHolder runat="server" id="placeholder">
    <div class="filterGeo">
         <asp:Literal runat="server" ID="ltAccountType" Text='<%$ Resources:Resource, Label_Account_Type %>'/>
    </div>    
    <telerik:RadComboBox ID="Segment_ID" runat="server" DropDownWidth="150px"  Height="100px" EnableEmbeddedSkins="false" 
        SkinID="planInfoCombo" Skin="pathfinder" AppendDataBoundItems="true" DataTextField="Name" DataValueField="ID" OnClientLoad="function(s,a) { if(!clientManager.get_SelectionData()){ LoadPlanList(s,a);}}"  OnClientSelectedIndexChanged="LoadPlanList" 
        CssClass="rdcmbMS" CausesValidation="false">
    </telerik:RadComboBox>
    
    <div class="filterGeo">
        <asp:Literal runat="server" ID="Literal1" Text="Account Name" />
    </div>      
    <telerik:RadComboBox ID="Plan_ID" runat="server" DropDownWidth="220px" Height="100px" EnableEmbeddedSkins="false" SkinID="planInfoCombo" 
        Skin="pathfinder" AppendDataBoundItems="true" >
    </telerik:RadComboBox>
    
   
    <pinso:ClientValidator runat="server" id="validator1" target="Plan_ID" Required="true" Text="Please select a plan." DataField="Plan_ID" />

</asp:PlaceHolder>


