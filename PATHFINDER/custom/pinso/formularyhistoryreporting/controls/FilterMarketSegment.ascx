<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterMarketSegment.ascx.cs" Inherits="custom_controls_FilterMarketSegment" %>
<div class="filterGeo">
    <asp:Literal runat="server" ID="filterLabel" Text="Select a Market Segment" />
</div>
    
<telerik:RadComboBox ID="Section_ID" runat="server" EnableEmbeddedSkins="false" Skin="pathfinder"  DropDownWidth="190px"
 DataTextField="Name" DataValueField="ID" AppendDataBoundItems="true">

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

    
<div class="filterGeo">
     <asp:Literal runat="server" ID="litAccountName" Text="Account Name" />
</div>

<%--<telerik:RadComboBox ID="Plan_ID" runat="server" EnableEmbeddedSkins="false" Skin="pathfinder" DropDownWidth="190px" 
 AppendDataBoundItems="true"></telerik:RadComboBox>
 
<pinso:ClientValidator runat="server" id="validator1" target="Plan_ID" DataField="Plan_ID" Required="true" Text="Please select at least one Account." />
--%>
<div id="filterPlan">
        <div class="searchTextBoxFilter">
            <input type="text" id="Plan_ID" class="textBox" />
        </div>
        <pinso:SearchList ID="searchlist" runat="server" Target="Plan_ID" ClientManagerID="mainSection" OffsetX="-6" 
                                    ContainerID="moduleOptionsContainer" 
                                    ServiceUrl="services/pathfinderservice.svc/fhrPlanSectionListSet" 
                                    QueryFormat="$filter=substringof('{0}',Name)&$top=50&$orderby=Name" 
                                    QueryValues=""
                                    DataField="ID" 
                                    TextField="Name"
                                    MultiSelect="true" 
                                    MultiSelectHeaderText="Selected Accounts"
                                    WaterMarkText="Type to search"/>
</div>    
 
    
   