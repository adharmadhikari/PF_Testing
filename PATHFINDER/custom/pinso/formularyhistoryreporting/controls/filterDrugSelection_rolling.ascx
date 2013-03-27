<%@ Control Language="C#" AutoEventWireup="true" CodeFile="filterDrugSelection_rolling.ascx.cs" Inherits="custom_pinso_formularyhistoryreporting_controls_filterDrugSelection_rolling" %>
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
<telerik:RadComboBox ID="Drug_ID" runat="server" EnableEmbeddedSkins="false" Skin="pathfinder" MaxHeight="225px" Height="160px">
</telerik:RadComboBox>

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
   
        <telerik:RadComboBox ID="TimeFrameQtr" runat="server" EnableEmbeddedSkins="false" Skin="pathfinder" MaxHeight="225px" Height="160px">
           <Items>
                <telerik:RadComboBoxItem runat="server" Value="2" Text="Rolling 2 Quarters" />
                <telerik:RadComboBoxItem runat="server" Value="3" Text="Rolling 3 Quarters" />
                <telerik:RadComboBoxItem runat="server" Value="4" Text="Rolling 4 Quarters" />
           </Items>
        </telerik:RadComboBox>   
        <telerik:RadComboBox ID="TimeFrameMonth" runat="server" EnableEmbeddedSkins="false" Skin="pathfinder" MaxHeight="225px" Height="160px">
           <Items>
                <telerik:RadComboBoxItem runat="server" Value="3" Text="Rolling 3 Months" />
                <telerik:RadComboBoxItem runat="server" Value="6" Text="Rolling 6 Months" />
                <telerik:RadComboBoxItem runat="server" Value="9" Text="Rolling 9 Months" />
                <telerik:RadComboBoxItem runat="server" Value="12" Text="Rolling 12 Months" />
           </Items>
        </telerik:RadComboBox>      
    

<pinso:ClientValidator runat="server" id="validator1" target="Drug_ID" DataField="Drug_ID" Required="true" Text='<%$ Resources:Resource, Message_Required_DrugSelection %>' />

