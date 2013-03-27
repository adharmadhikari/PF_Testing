<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterCalendarRolling.ascx.cs" Inherits="prescriberreporting_controls_FilterCalendarRolling" %>
<div id="filterCalendarRolling">
    <div class="filterGeo">
    <asp:Literal runat="server" ID="filterLabel" Text='Data Type' />
    </div>
    <div style="margin-left: 5px">
    <pinso:RadiobuttonValueList ID="Calendar_Rolling" runat="server" BorderStyle="None" RepeatLayout="Flow" CssClass="listItemWidth" RepeatDirection="Horizontal"  >
        <asp:ListItem Text="Calendar" Value="Calendar"></asp:ListItem> 
        <asp:ListItem Text="Rolling" Value="Rolling"></asp:ListItem>                                    
    </pinso:RadiobuttonValueList>
    </div>
</div>