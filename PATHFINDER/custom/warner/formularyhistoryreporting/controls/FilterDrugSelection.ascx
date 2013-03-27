<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterDrugSelection.ascx.cs" Inherits="custom_warner_formularyhistoryreporting_controls_FilterDrugSelection" %>
<!-- READ ME: Client side events for Market Basket and Drug ID controls are set in code behind -->
<div class="filterGeo">
    <asp:Literal runat="server" ID="filterLabel" Text='<%$ Resources:Resource, Label_DrugSelection %>'  />
    </div>
<telerik:RadComboBox ID="Market_Basket_ID" EnableEmbeddedSkins="false" SkinID="standardReportsTruncate" Skin="pathfinder" DropDownWidth="300px" MaxHeight="225px" EnableViewState="false" runat="server" AppendDataBoundItems="true">
    
    <Items>
        <telerik:RadComboBoxItem runat="server" Value="" Text='<%$ Resources:Resource, Label_ListItem_Therapeutic_Class_All %>' />
    </Items>
</telerik:RadComboBox>                  

<div class="filterGeo">
    <asp:Literal runat="server" ID="Literal1" Text='<%$ Resources:Resource, Label_DrugID %>'  />
    </div>
<telerik:RadComboBox ID="Product_ID" runat="server" EnableEmbeddedSkins="false" Skin="pathfinder" MaxHeight="225px" Height="160px" DropDownWidth="300px" >
</telerik:RadComboBox>

<pinso:ClientValidator runat="server" id="validator1" target="Product_ID" DataField="Product_ID" Required="true" Text='<%$ Resources:Resource, Message_Required_DrugSelection %>' />


<div class="filterGeo">
        <asp:Literal runat="server" ID="Literal4" Text='Report Type' />
    </div>
<div id="filterTrxMst" style="margin-left: 5px">
    <pinso:RadiobuttonValueList ID="Trx_Mst" runat="server" BorderStyle="None" RepeatDirection="Horizontal" 
        RepeatLayout="Flow" CssClass="listItemWidth">
        <asp:ListItem Text="Volume" Value="Trx" Selected="True" ></asp:ListItem> 
        <asp:ListItem Text="Percentage" Value="Mst"></asp:ListItem>                                 
    </pinso:RadiobuttonValueList>
</div>


 <div class="filterGeo">
        <asp:Literal runat="server" ID="Literal3" Text='Time Frame Type' />
    </div>
    <div style="margin-left: 5px">
        <pinso:RadiobuttonValueList ID="Monthly_Quarterly" runat="server" BorderStyle="None" RepeatLayout="Flow" CssClass="listItemWidth" RepeatDirection="Horizontal"  >
            <asp:ListItem Text="Monthly" Value="M" onClick="onTimeFrameChanged('M');"></asp:ListItem> 
            <asp:ListItem Text="Quarterly" Value="Q" onClick="onTimeFrameChanged('Q');"></asp:ListItem>                                    
        </pinso:RadiobuttonValueList>
    </div>

    <div class="filterGeo">
        <asp:Literal runat="server" ID="Literal2" Text="Choose Time Frame"  />
    </div>
   
        <telerik:RadComboBox ID="TimeFrame1" runat="server" EnableEmbeddedSkins="false" Skin="pathfinder" MaxHeight="225px" Height="160px" Width="70px">
        </telerik:RadComboBox>- 
        
        <telerik:RadComboBox ID="TimeFrame2" runat="server" EnableEmbeddedSkins="false" Skin="pathfinder" MaxHeight="225px" Height="160px" Width="70px">
        </telerik:RadComboBox>

<%--<pinso:ClientValidator runat="server" id="validator1" target="Drug_ID" DataField="Drug_ID" Required="true" Text='<%$ Resources:Resource, Message_Required_DrugSelection %>' />
--%>