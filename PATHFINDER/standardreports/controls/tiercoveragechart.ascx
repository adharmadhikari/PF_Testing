<%@ Control Language="C#" AutoEventWireup="true" CodeFile="tiercoveragechart.ascx.cs" Inherits="standardreports_controls_tiercoveragechart"  %>
<%@ Register src="~/standardreports/controls/TierCoverageChartTemplate.ascx" tagname="TierCoverageChart" tagprefix="pinso" %>
<%--<%@ Register src="~/standardreports/controls/FormularyUpdateDate.ascx" tagname="FormularyUpdateDate" tagprefix="pinso" %>
--%>
<%--<pinso:FormularyUpdateDate runat="server" ID="formularyUpdateDate" />--%>

    <pinso:TierCoverageChart runat="server" id="chartNational" Thumbnail="true" Visible="false" />
    <pinso:TierCoverageChart runat="server" id="chartRegional" Thumbnail="true" Visible="false" />
    

    