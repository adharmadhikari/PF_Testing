<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FormularyCoveragePieChart.ascx.cs" Inherits="standardreports_controls_FormularyCoveragePieChart" %>
<%@ Register src="~/standardreports/controls/FormularyCoveragePieChartTemplate.ascx" tagname="FormularyCoveragePieChartTemplate" tagprefix="pinso" %>
    <table width="100%">
    <tr>
    <td>
        <pinso:FormularyCoveragePieChartTemplate runat="server" id="piechartFCoverage1" Thumbnail="true" Visible="true" />
    </td>
    </tr>
    </table>
    