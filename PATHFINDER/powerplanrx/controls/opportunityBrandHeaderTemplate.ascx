<%@ Control Language="C#" AutoEventWireup="true" CodeFile="opportunityBrandHeaderTemplate.ascx.cs" Inherits="controls_opportunityBrandHeaderTemplate" %>
<table style="width:100%" cellpadding="0" cellspacing="0"> 
    <tr>
        <td colspan="4" class="<%= DataField1 %>"><%= DataField1 %></td>
    </tr>   
    <tr>
        <td class="left"  style="width:25%">Tier</td>
        <td class="left"  style="width:25%">Co-Pay</td>
        <td class="left"  style="width:25%">Trx</td>
        <td class="right"  style="width:25%">Mst</td>
    </tr>
</table>