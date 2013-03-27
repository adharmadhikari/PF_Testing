<%@ Control Language="C#" AutoEventWireup="true" CodeFile="documenttype.ascx.cs" Inherits="custom_Alcon_customercontactreports_Controls_documenttype" %>
<div class="filterGeo">
    <asp:Literal runat="server" ID="filterLabel" Text="Document Type" />
</div>   
<telerik:RadComboBox ID="Document_Type_ID" runat="server" EnableEmbeddedSkins="false" Skin="pathfinder" 
    AppendDataBoundItems="true" DropDownWidth="190px" OnClientLoad="CreateDocTypeList" >
</telerik:RadComboBox>
    