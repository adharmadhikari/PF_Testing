<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterMarketSegment.ascx.cs" Inherits="custom_controls_FilterMarketSegment" %>
<div class="filterGeo">
    <asp:Literal runat="server" ID="filterLabel" Text="Market Segment" />
</div>
    
<telerik:RadComboBox ID="Section_ID" runat="server" EnableEmbeddedSkins="false" Skin="pathfinder"  DropDownWidth="190px"
 DataTextField="Name" DataValueField="ID" AppendDataBoundItems="true"  CssClass="queryExt">

</telerik:RadComboBox>
 
 <%--<div class="filterGeo">
        <asp:Literal runat="server" ID="Literal2" Text="Title"  />
</div>
<telerik:RadComboBox runat="server" ID="Title_ID" Skin="pathfinder" CssClass="queryExt"  EnableEmbeddedSkins="false" MaxHeight="200px" /> 
--%>
 
<%--<pinso:ClientValidator runat="server" id="validator1" target="Plan_ID" DataField="Plan_ID" Required="true" Text="Please select at least one Account." />
--%>
 
    
   