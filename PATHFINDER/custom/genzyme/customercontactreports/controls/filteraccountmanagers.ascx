<%@ Control Language="C#" AutoEventWireup="true" CodeFile="filteraccountmanagers.ascx.cs" Inherits="custom_genzyme_customercontactreports_controls_filteraccountmanagers" %>
<asp:PlaceHolder runat="server" id="placeholder">
  <div class="filterGeo">
        <asp:Literal runat="server" ID="filterLabel" Text='<%$ Resources:Resource, Label_Account_Manager %>' />
    </div>
    <telerik:RadComboBox runat="server" ID="User_ID" DataSourceID="dsAcctMgr" DataTextField="FullName" DataValueField="User_ID" 
      AppendDataBoundItems="true" Skin="pathfinder" EnableEmbeddedSkins="false" MaxHeight="300px" >
        
    </telerik:RadComboBox>  
  
</asp:PlaceHolder>
<asp:EntityDataSource ID="dsAcctMgr" runat="server" EntitySetName="AccountManagerSet" DefaultContainerName="PathfinderGenzymeEntities"  ConnectionString="name=PathfinderGenzymeEntities"
        ContextTypeName="Pinsonault.Application.Genzyme.PathfinderGenzymeEntities, Pinsonault.Application.Genzyme" OrderBy="it.User_F_Name, it.User_L_Name"
    AutoGenerateWhereClause="true">
</asp:EntityDataSource> 

 