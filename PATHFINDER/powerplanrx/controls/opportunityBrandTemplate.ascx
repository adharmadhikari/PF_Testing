<%@ Control Language="C#" AutoEventWireup="true" CodeFile="opportunityBrandTemplate.ascx.cs" Inherits="controls_opportunityBrandTemplate" %>
<table style="width:100%" cellpadding="0" cellspacing="0"> 
    <tr>
        <td style="width:25%"><%# Eval(DataField1) %><input id="<%# DataField5 %>" type="hidden" value="<%# Eval(DataField5) %>" /></td>
        <td style="width:25%"><%# Eval(DataField2)%></td>
        <td style="width:25%" class="alignRight"><%# Eval(DataField3, "{0:n0}") %></td>
        <td style="width:25%" class="alignRight"><%# Eval(DataField4, "{0:n2}")%></td>
    </tr>
</table>