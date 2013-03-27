<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterMonthQuarter.ascx.cs" Inherits="marketplaceanalytics_controls_FilterMonthQuarter" %>
<div id="filterCalendarRolling">
    <div class="filterGeo">
    <asp:Literal runat="server" ID="filterLabel" Text='Time Frame Type' />
    </div>
    <div style="margin-left: 5px">
    <pinso:RadiobuttonValueList ID="Monthly_Quarterly" runat="server" BorderStyle="None" RepeatLayout="Flow" CssClass="listItemWidth" RepeatDirection="Horizontal"  >
        <asp:ListItem Text="Monthly" Value="M" onClick="onTimeFrameChanged('M');"></asp:ListItem> 
        <asp:ListItem Text="Quarterly" Value="Q" onClick="onTimeFrameChanged('Q');"></asp:ListItem>                                    
    </pinso:RadiobuttonValueList>
    </div>
</div>