<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterFormularyType.ascx.cs" Inherits="standardreports_controls_FilterFormularyType" %>
<div>
    
    <%-- 
    <select id="Segment_ID">
        <option value="1">Commercial</option>
        <option value="2">Medicare Part D</option>
    </select>
    --%>
    <div class="filterGeo">
    <asp:Literal runat="server" ID="filterLabel" Text='<%$ Resources:Resource, Label_FormularyType %>' />
    </div>
    
     <telerik:RadComboBox runat="server" ID="Section_ID" DataSourceID="dsFormularyType" DataTextField="Name" DataValueField="ID" OnClientSelectedIndexChanged="function (s, a){ var c = $find('Plan_Name$SearchList'); if (c) {c.set_queryValues(s.get_value());c.clear();} var geoEnabled = (s.get_value()!=4); c = $find('ctl00_partialPage_filtersContainer_Geography_rcbGeographyType'); if(c) {if(!geoEnabled){c.get_items().getItem(0).select(); $('#filterGeography').addClass('disabled');} else {$('#filterGeography').removeClass('disabled');} c.set_enabled(geoEnabled);} var IsStateMedicaid = (s.get_value() == 9);if (IsStateMedicaid) { $('#ctl00_partialPage_filtersContainer_Tier_filterTier').hide();} else {$('#ctl00_partialPage_filtersContainer_Tier_filterTier').show();} }" AppendDataBoundItems="true" Skin="pathfinder" EnableEmbeddedSkins="false" MaxHeight="200px">
        <Items>
        </Items>
    </telerik:RadComboBox>
    
     <asp:EntityDataSource runat="server" ID="dsFormularyType" DefaultContainerName="PathfinderEntities" ConnectionString="name=PathfinderEntities" EntitySetName="FormularyTypeSet" OrderBy="it.Name" AutoGenerateWhereClause="true">
        <WhereParameters>
            <asp:SessionParameter SessionField="ClientID" Name="Client_ID" Type="Int32" DefaultValue="0" />
            <asp:Parameter Name="app_id" DefaultValue="3" Type="Int32" />
        </WhereParameters>
     </asp:EntityDataSource>
       
   
</div>