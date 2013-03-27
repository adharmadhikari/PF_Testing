<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterAffiliation.ascx.cs" Inherits="marketplaceanalytics_controls_FilterAffiliation" %>
<div id="filterAffiliation">   
    <div class="filterGeo">
        <asp:Literal runat="server" ID="filterLabel" Text="Channel" />
    </div>
    <telerik:RadComboBox runat="server" ID="Section_ID" DataSourceID="dsFormularyType"
        DataTextField="Name" DataValueField="ID" 
        OnClientLoad="function(s,a) { if(!clientManager.get_SelectionData()){ LoadPlanListByChannelType(s,a);}}"  
        OnClientSelectedIndexChanged="LoadPlanListByChannelType" 
        AppendDataBoundItems="true" Skin="pathfinder" EnableEmbeddedSkins="false" DropDownWidth="185px">
        <Items>
            <telerik:RadComboBoxItem Text="Combined (Commercial, Part D, Managed Medicaid)" Value="-1"/>             
        </Items>
    </telerik:RadComboBox>
    
    <div class="filterGeo">
        <asp:Literal runat="server" ID="Literal1" Text="Account Name" />
    </div> 
    <telerik:RadComboBox ID="Plan_ID" runat="server" MaxHeight="300px" DropDownWidth="250px" EnableEmbeddedSkins="false" SkinID="planInfoCombo" 
        Skin="pathfinder" AppendDataBoundItems="true" >
    </telerik:RadComboBox>
    
     <asp:EntityDataSource runat="server" ID="dsFormularyType" DefaultContainerName="PathfinderEntities" 
     ConnectionString="name=PathfinderEntities" EntitySetName="FormularyTypeSet" OrderBy="it.Name"      
     Where="it.ID in {1,17,4,6} and it.Client_ID = @Client_ID and it.app_id = @app_id">
        <WhereParameters>
            <asp:SessionParameter SessionField="ClientID" Name="Client_ID" Type="Int32" DefaultValue="0" />  
            <asp:Parameter Name="app_id" DefaultValue="2" Type="Int32" />          
        </WhereParameters>
     </asp:EntityDataSource>
   
    <pinso:ClientValidator runat="server" id="validator1" target="Plan_ID" Required="true" Text="Please select a plan." DataField="Plan_ID" />
</div>