<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterAccount.ascx.cs" Inherits="custom_controls_FilterAccount" %>
<div class="filterGeo">
<asp:Literal runat="server" ID="filterLabel" Text="Account Name" />
</div>

<div class="searchTextBoxFilter">
    <input type="text" id="Plan_Name" class="textBox" />
</div>


<%-- IMPORTANT - QueryFormat and QueryValues are handled in javascript due to dependency on FilterMarketSegment --%>
 <pinso:SearchList runat="server" Target="Plan_Name" ClientManagerID="mainSection" ContainerID="moduleOptionsContainer" OffsetX="-6"
                                QueryFormat="$filter=substringof('{0}',Name)&$top=50&$orderby=Name" 
                                QueryValues=""
                                DataField="Name" 
                                TextField="Name"  ID="searchlist"/>
 