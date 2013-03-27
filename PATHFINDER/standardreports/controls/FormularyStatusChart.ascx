<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FormularyStatusChart.ascx.cs" Inherits="standardreports_controls_FormularyStatusChart" %>
<%--<%@ Register src="~/standardreports/controls/FormularyUpdateDate.ascx" tagname="FormularyUpdateDate" tagprefix="pinso" %>
--%>
<%@ Register src="~/standardreports/controls/FormularyStatusChartTemplate.ascx" tagname="FormularyStatusChart" tagprefix="pinso" %>

<%--<pinso:FormularyUpdateDate runat="server" ID="formularyUpdateDate" />--%>

        <pinso:FormularyStatusChart runat="server" id="chartNational" Thumbnail="true" Visible="false" />
        <pinso:FormularyStatusChart runat="server" id="chartRegional" Thumbnail="true" Visible="false" />
   

