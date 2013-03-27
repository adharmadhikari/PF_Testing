<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TierCoveragePieChart.ascx.cs" Inherits="standardreports_controls_TierCoveragePieChart" %>
<%@ Register src="~/standardreports/controls/TierCoveragePieChartTemplate.ascx" tagname="TierCoveragePieChartTemplate" tagprefix="pinso" %>
    <table width="100%">
    <tr>
    <td>
        <pinso:TierCoveragePieChartTemplate runat="server" id="piechartTierCoverage1" Thumbnail="true" Visible="true" />
    </td>
    </tr>
    </table>
    