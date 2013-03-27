<%@ Control Language="C#" AutoEventWireup="true" CodeFile="filteraccountselection.ascx.cs" Inherits="custom_merz_businessplanning_controls_filteraccountselection" %>
<asp:PlaceHolder runat="server" id="placeholder">
    <div class="filterGeo">
        <asp:Literal runat="server" ID="filterLabel" Text='<%$ Resources:Resource, Label_Therapeutic_Class %>' />
    </div>
    <telerik:RadComboBox runat="server" ID="Thera_ID" DataSourceID="dsThera" DataTextField="Thera_Name" DataValueField="Thera_ID" 
      AppendDataBoundItems="true" Skin="pathfinder" EnableEmbeddedSkins="false" MaxHeight="300px" OnClientSelectedIndexChanging="onListChanging" OnClientSelectedIndexChanged="onTheraListChanged">
        
    </telerik:RadComboBox>   
    
    <div class="filterGeo">
        <asp:Literal runat="server" ID="ltAccountType" Text='<%$ Resources:Resource, Label_Account_Type %>' />
    </div>    
    <telerik:RadComboBox runat="server" ID="Section_ID" 
        AppendDataBoundItems="true" Skin="pathfinder" EnableEmbeddedSkins="false" MaxHeight="200px" OnClientLoad="onSectionListLoad" OnClientSelectedIndexChanging="onListChanging"  OnClientSelectedIndexChanged="onSectionListChanged">
    </telerik:RadComboBox>  
    
    <div class="filterGeo">
        <asp:Literal runat="server" ID="Literal1" Text="Account Name" />
    </div>      
    <telerik:RadComboBox ID="Plan_ID" runat="server" DropDownWidth="220px" Height="100px" EnableEmbeddedSkins="false" SkinID="planInfoCombo" 
        Skin="pathfinder" AppendDataBoundItems="true" 
         Visible="true" CssClass="rdcmbPlan">
    </telerik:RadComboBox>
    <pinso:ClientValidator ID="ClientValidator1" Target="Plan_ID" Text ='Please select the Account Name' Required="true" runat="server" />    
</asp:PlaceHolder>

    <asp:EntityDataSource ID="dsThera" runat="server" EntitySetName="BPTheraListSet" DefaultContainerName="PathfinderMerzEntities" ConnectionString="name=PathfinderMerzEntities" 
        AutoGenerateWhereClause="true">
    </asp:EntityDataSource>    

  