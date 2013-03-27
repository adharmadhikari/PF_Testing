<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterMarketSegment.ascx.cs" Inherits="restrictionsreport_controls_FilterMarketSegment" %>
 <div class="filterGeo">
    <asp:Literal runat="server" ID="filterLabel" Text="Market Segment" />
    </div>
    <%--var geoEnabled = (s.get_value()!=4); c = $find('ctl00_partialPage_filtersContainer_Geography_rcbGeographyType'); if(c) {if(!geoEnabled){c.get_items().getItem(0).select(); $('#filterGeography').addClass('disabled');} else {$('#filterGeography').removeClass('disabled');} c.set_enabled(geoEnabled);} --%>
    <telerik:RadComboBox ID="Section_ID" runat="server"  EnableEmbeddedSkins="false" Skin="pathfinder" AppendDataBoundItems="true" DropDownWidth="190px" OnClientSelectedIndexChanged="function (s, a){ var geoEnabled = (s.get_value()!=4); d = $find('ctl00_partialPage_filtersContainer_Region_rcbGeographyType'); if(d) {if(!geoEnabled){d.get_items().getItem(0).select(); $('#filterGeography').addClass('disabled');} else {$('#filterGeography').removeClass('disabled');} d.set_enabled(geoEnabled);}}" OnClientDropDownClosed="sectionDropDownClosed">
    </telerik:RadComboBox>
<div id="client_validator" runat="server">
    <pinso:ClientValidator runat="server" id="validator1" target="Section_ID" DataField="Section_ID" Required="true" Text='Please select at least one market segment.' />
</div>