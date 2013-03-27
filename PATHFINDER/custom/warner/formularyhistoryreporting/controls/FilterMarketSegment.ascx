<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterMarketSegment.ascx.cs" Inherits="custom_warner_formularyhistoryreporting_controls_FilterMarketSegment" %>
<div class="filterGeo">
    <asp:Literal runat="server" ID="filterLabel" Text="Select a Market Segment" />
</div>
    
<telerik:RadComboBox ID="Section_ID" runat="server" EnableEmbeddedSkins="false" Skin="pathfinder"  DropDownWidth="190px"
 DataTextField="Name" DataValueField="ID" AppendDataBoundItems="true">
 <Items>
    <%--<telerik:RadComboBoxItem Text="Combined (Commercial + Part D + Managed Medicaid)" Value="-1"/>--%> 
 </Items>
</telerik:RadComboBox>

<div class="filterGeo">
    <asp:Literal runat="server" ID="Literal1" Text='<%$ Resources:Resource, Label_Geography %>'  />
</div>
<telerik:RadComboBox runat="server" ID="rcbGeographyType" CssClass="queryExt" Skin="pathfinder" EnableEmbeddedSkins="false" MaxHeight="200px">
    <Items>  
        <telerik:RadComboBoxItem runat="server" Value="1" Text="National" />          
        <telerik:RadComboBoxItem runat="server" Value="2" Text="Regional" />
        <telerik:RadComboBoxItem runat="server" Value="3" Text="State" />           
    </Items>        
</telerik:RadComboBox>
<telerik:RadComboBox runat="server" ID="Geography_ID" Skin="pathfinder" CssClass="string"  EnableEmbeddedSkins="false" MaxHeight="200px" />              

    
<%--<div class="filterGeo">
     <asp:Literal runat="server" ID="litAccountName" Text="Account Name" />
</div>--%>

    
   