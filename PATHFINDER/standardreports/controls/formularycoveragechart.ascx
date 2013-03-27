<%@ Control Language="C#" AutoEventWireup="true" CodeFile="formularycoveragechart.ascx.cs" Inherits="standardreports_controls_formularycoveragechart"  %>
<%@ Register src="~/standardreports/controls/FormularyCoverageChartTemplate.ascx" tagname="FormularyCoverageChart" tagprefix="pinso" %>
<%--<%@ Register src="~/standardreports/controls/FormularyUpdateDate.ascx" tagname="FormularyUpdateDate" tagprefix="pinso" %>

<pinso:FormularyUpdateDate runat="server" ID="formularyUpdateDate" />--%>
    
       
       
    <pinso:FormularyCoverageChart runat="server" id="chartNational" Thumbnail="true" Visible="false" />
    <pinso:FormularyCoverageChart runat="server" id="chartRegional" Thumbnail="true" Visible="false" />
  