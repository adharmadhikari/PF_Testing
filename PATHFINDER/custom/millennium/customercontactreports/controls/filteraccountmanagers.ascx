<%@ Control Language="C#" AutoEventWireup="true" CodeFile="filteraccountmanagers.ascx.cs" Inherits="custom_millennium_businessplanning_controls_filteraccountmanagers" %>
<asp:PlaceHolder runat="server" id="placeholder">
  <div class="filterGeo">
        <asp:Literal runat="server" ID="filterLabel" Text='<%$ Resources:Resource, Label_Account_Manager %>' />
    </div>
    <telerik:RadComboBox runat="server" ID="User_ID"  DataTextField="FullName" DataValueField="User_ID" 
      AppendDataBoundItems="true" Skin="pathfinder" EnableEmbeddedSkins="false" MaxHeight="300px" >
        
    </telerik:RadComboBox>  
  
</asp:PlaceHolder>
<%--<asp:EntityDataSource ID="dsAcctMgr" ConnectionString="name=PathfinderMillenniumEntities" runat="server" EntitySetName="AccountManagerSet" EntityTypeFilter="AccountManager" DefaultContainerName="PathfinderMillenniumEntities" OrderBy="it.User_F_Name, it.User_L_Name"
    AutoGenerateWhereClause="true">
</asp:EntityDataSource> 
--%>

 