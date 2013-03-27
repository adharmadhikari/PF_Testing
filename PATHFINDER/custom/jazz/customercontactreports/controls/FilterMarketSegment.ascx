<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterMarketSegment.ascx.cs" Inherits="custom_controls_FilterMarketSegment" %>
 <div class="filterGeo">
    <asp:Literal runat="server" ID="filterLabel" Text="Select a Market Segment" />
    </div>
    <%--var geoEnabled = (s.get_value()!=4); c = $find('ctl00_partialPage_filtersContainer_Geography_rcbGeographyType'); if(c) {if(!geoEnabled){c.get_items().getItem(0).select(); $('#filterGeography').addClass('disabled');} else {$('#filterGeography').removeClass('disabled');} c.set_enabled(geoEnabled);} --%>
    <telerik:RadComboBox ID="Section_ID" runat="server" EnableEmbeddedSkins="false" Skin="pathfinder" AppendDataBoundItems="true" DropDownWidth="190px" OnClientSelectedIndexChanged="function (s, a){ var c = $find('Plan_Name$SearchList'); if (c) {c.set_queryValues(s.get_value() ? s.get_value() : 0);c.clear();} var geoEnabled = (s.get_value()!=4); d = $find('ctl00_partialPage_filtersContainer_Region_rcbGeographyType'); if(d) {if(!geoEnabled){d.get_items().getItem(0).select(); $('#filterGeography').addClass('disabled');} else {$('#filterGeography').removeClass('disabled');} d.set_enabled(geoEnabled);}}">
    </telerik:RadComboBox>
    
    <%--<telerik:RadComboBox runat="server" ID="Section_ID" DataSourceID="dsSection" DataTextField="Name" DataValueField="ID" OnClientSelectedIndexChanged="function (s, a){ var c = $find('Plan_Name$SearchList'); if (c) {c.set_queryValues(s.get_value() ? s.get_value() : 0);c.clear();} var geoEnabled = (s.get_value()!=4); d = $find('ctl00_partialPage_filtersContainer_Region_rcbGeographyType'); if(d) {if(!geoEnabled){d.get_items().getItem(0).select(); $('#filterGeography').addClass('disabled');} else {$('#filterGeography').removeClass('disabled');} d.set_enabled(geoEnabled);}}" AppendDataBoundItems="true" Skin="pathfinder" EnableEmbeddedSkins="false" MaxHeight="200px">
        <Items>
            <telerik:RadComboBoxItem Text="--All Market Segments--" Value="" />
        </Items>
    </telerik:RadComboBox>
    <asp:EntityDataSource runat="server" ID="dsSection" 
    DefaultContainerName="PathfinderEntities" 
    ConnectionString="name=PathfinderEntities" EntitySetName="SectionSet" 
    OrderBy="it.Name" AutoGenerateWhereClause="True">
       
    </asp:EntityDataSource>--%>