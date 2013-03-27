<%@ Control Language="C#" AutoEventWireup="true" CodeFile="filtermedicalpolicy.ascx.cs" Inherits="custom_merz_businessplanning_controls_filtermedicalpolicy" %>
<asp:PlaceHolder runat="server" id="placeholder">
  
    <div class="filterGeo">
        <asp:Literal runat="server" ID="Literal1" Text='Document Type' />
    </div> 
     <telerik:RadComboBox runat="server" ID="DocumentType" DataSourceID="dsDocType" DataTextField="Document_Type_Name" DataValueField="Document_Type_ID" 
                AppendDataBoundItems="true" Skin="pathfinder" EnableEmbeddedSkins="false" MaxHeight="300px" CssClass="queryExt">
        <Items>
                <telerik:RadComboBoxItem Text="All Document Types" Selected="true"/>
        </Items>
     </telerik:RadComboBox>
  
</asp:PlaceHolder>

<asp:EntityDataSource ID="dsDocType" runat="server" EntitySetName="BusinessPlanDocumentTypesSet" DefaultContainerName="PathfinderMerzEntities" ConnectionString="name=PathfinderMerzEntities" 
        AutoGenerateWhereClause="true">
</asp:EntityDataSource>    


 