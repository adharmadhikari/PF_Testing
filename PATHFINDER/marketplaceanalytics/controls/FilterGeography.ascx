<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterGeography.ascx.cs" Inherits="todaysanalytics_controls_FilterGeography" %>
<div id="filterGeography">
    <div class="filterGeo">
    <asp:Literal runat="server" ID="filterLabel" Text='<%$ Resources:Resource, Label_Geography %>'  />
    </div>
    <%--<telerik:RadComboBox runat="server" ID="rcbGeographyType" CssClass="notfilter" Skin="pathfinder" EnableEmbeddedSkins="false" MaxHeight="200px" >
        <Items>
            <telerik:RadComboBoxItem runat="server" Value="1" Text="National" />
            <telerik:RadComboBoxItem runat="server" Value="2" Text="Regional" />
            <telerik:RadComboBoxItem runat="server" Value="3" Text="State" />
        </Items>        
    </telerik:RadComboBox>--%> 
    <telerik:RadComboBox runat="server" ID="Geography_Type" Skin="pathfinder"  EnableEmbeddedSkins="false" MaxHeight="200px" >          
        <Items>
            <telerik:RadComboBoxItem runat="server" Value="1" Text="National" />
            <telerik:RadComboBoxItem runat="server" Value="2" Text="Regional" />
            <telerik:RadComboBoxItem runat="server" Value="3" Text="Account Manager" />
        </Items>   
    </telerik:RadComboBox>
    <telerik:RadComboBox runat="server" ID="Territory_ID" DataSourceID="dsAcctMgr" DataTextField="FullName" DataValueField="Territory_ID" 
      AppendDataBoundItems="true" Skin="pathfinder" EnableEmbeddedSkins="false" MaxHeight="300px"  >      
      <Items>
        <telerik:RadComboBoxItem runat="server" Value="" Text="Select Account Manager" />
      </Items>  
    </telerik:RadComboBox>
    <telerik:RadComboBox runat="server" ID="Region_ID" Skin="pathfinder" CssClass="string"  EnableEmbeddedSkins="false" MaxHeight="200px" />
    <telerik:RadComboBox runat="server" ID="State_ID" Skin="pathfinder" CssClass="string"  EnableEmbeddedSkins="false" MaxHeight="200px" />
    <pinso:ClientValidator runat="server" id="validator1" target="Territory_ID" DataField="Territory_ID" Required="true" Text='Please select an Account Manager' />
    <pinso:ClientValidator runat="server" id="validator2" target="Region_ID" DataField="Region_ID" Required="true" Text='Please select a Region' />
    <asp:EntityDataSource ID="dsAcctMgr" runat="server" EntitySetName="AccountManagersByTerritorySet" DefaultContainerName="PathfinderClientEntities" OrderBy="it.User_F_Name, it.User_L_Name"
    AutoGenerateWhereClause="true">
</asp:EntityDataSource> 
</div>