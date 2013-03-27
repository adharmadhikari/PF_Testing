<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FormularyStatusPieChart.ascx.cs" Inherits="standardreports_controls_FormularyStatusPieChart" %>
<%@ Register src="~/standardreports/controls/FormularyStatusPieChartTemplate.ascx" tagname="FormularyStatusPieChartTemplate" tagprefix="pinso" %>
    <table width="100%">
    <tr>
    <td>
        <pinso:FormularyStatusPieChartTemplate runat="server" id="piechartFStatus1" Thumbnail="true" Visible="true" />
    </td>
    </tr>
    </table>
    