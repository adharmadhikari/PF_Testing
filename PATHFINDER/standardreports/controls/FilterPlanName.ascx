<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterPlanName.ascx.cs" Inherits="standardreports_controls_FilterPlanName" %>
<div class="filterGeo">
<asp:Literal runat="server" ID="filterLabel" Text='<%$ Resources:Resource, Label_Account_Name %>' />
</div>
<div class="searchTextBoxFilter">
 <input type="text" id="Plan_Name" class="textBox" />
 </div>
 <pinso:SearchList runat="server" Target="Plan_Name" ClientManagerID="mainSection" OffsetX="-6" ContainerID="moduleOptionsContainer" ServiceUrl="standardreports/services/pathfinderclientdataservice.svc/PlanSearchSet" 
                                QueryFormat="$filter=Section_ID eq {0} and substringof('{1}',Name)&$top=50&$orderby=Name" 
                                QueryValues="clientManager.get_Channel()"
                                DataField="Name" 
                                TextField="Name" />
 