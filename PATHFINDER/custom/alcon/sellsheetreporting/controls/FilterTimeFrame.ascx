<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterTimeFrame.ascx.cs" Inherits="custom_controls_FilterTimeFrame" %>
<div class="filterGeo">
    <asp:Literal runat="server" ID="Literal1" Text='<%$ Resources:Resource, Label_Geography %>'  />
</div>
<telerik:RadComboBox runat="server" ID="rcbGeographyType" CssClass="queryExt" Skin="pathfinder" EnableEmbeddedSkins="false" MaxHeight="200px">
    <Items>  
        <telerik:RadComboBoxItem runat="server" Value="1" Text="National" /> 
        <telerik:RadComboBoxItem runat="server" Value="2" Text="State" />                   
    </Items>        
</telerik:RadComboBox>
<telerik:RadComboBox runat="server" ID="Geography_ID" Skin="pathfinder" CssClass="queryExt"  EnableEmbeddedSkins="false" MaxHeight="200px" />    

<%--<telerik:RadComboBox ID="Plan_ID" runat="server" EnableEmbeddedSkins="false" Skin="pathfinder" DropDownWidth="190px" CssClass="queryExt"
 AppendDataBoundItems="true"></telerik:RadComboBox>--%>
 <div class="filterGeo">
     <asp:Literal runat="server" ID="litUserName" Text="User" />
</div>
 <div id="filterUser">
    <div class="searchTextBoxFilter">
        <input type="text" id="User_ID" class="textBox" />
    </div>
     <pinso:SearchList ID="user_searchlist" runat="server" Target="User_ID" CssClass="queryExt" ClientManagerID="mainSection" OffsetX="-6" 
                                    ContainerID="moduleOptionsContainer"
                                    QueryFormat="$filter=substringof('{0}',FullName)&$top=50&$orderby=FullName" 
                                    QueryValues=""
                                    DataField="User_ID" 
                                    TextField="FullName"
                                    MultiSelect="false"
                                    WaterMarkText="Type to search"/>
 </div>
 
 <div class="filterGeo">
     <asp:Literal runat="server" ID="litAccountName" Text="Account Name" />
</div>
 <div id="filterPlan">
        <div class="searchTextBoxFilter">
            <input type="text" id="Plan_ID" class="textBox" />
        </div>
        <pinso:SearchList ID="searchlist" runat="server" Target="Plan_ID" CssClass="queryExt" ClientManagerID="mainSection" OffsetX="-6" 
                                    ContainerID="moduleOptionsContainer"
                                    ServiceUrl="services/pathfinderservice.svc/PlanInfoListViewSet" 
                                    QueryFormat="$filter=substringof('{0}',Plan_Name)&$top=100&$orderby=Plan_Name" 
                                    QueryValues=""
                                    DataField="Plan_ID" 
                                    TextField="Plan_Name"
                                    MultiSelect="true" 
                                    MultiSelectHeaderText="Selected Accounts"
                                    WaterMarkText="Type to search"/>
</div>  
 
<div class="filterGeo">
    <asp:Literal runat="server" ID="Timeselect" Text="Choose Time Frame " />  
</div>

<div id ="timeFrame">
<div class="filterGeo">
    <input id="txtFrom" name="Created_DT" class="datePicker" style="width:70px" type="text"  /> - <input id="txtTo" name="Created_DT" class="datePicker" style="width:70px" type="text" />
</div>
 
  <pinso:ClientValidator runat="server" id="ClientValidator1" target="txtFrom" DataType="Date" Text='Please enter a valid start date.' Required="true" />
  <pinso:ClientValidator runat="server" id="ClientValidator2" target="txtTo" DataType="Date" Text='Please enter a valid end date.'  Required="true" />

</div>
