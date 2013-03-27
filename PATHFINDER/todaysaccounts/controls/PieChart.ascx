<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PieChart.ascx.cs" Inherits="todaysaccounts_controls_PieChart" %>
<%@ Register src="~/todaysaccounts/controls/PieChartTemplate.ascx" tagname="PieChartTemplate" tagprefix="pinso" %>
    <table width="100%">
    <tr>
    <td>
        <pinso:PieChartTemplate runat="server" id="piechartFCoverage1" Thumbnail="true" Visible="true" />
    </td>
    </tr>
    </table>
    