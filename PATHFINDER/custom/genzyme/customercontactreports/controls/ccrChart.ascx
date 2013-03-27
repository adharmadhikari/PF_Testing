<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ccrChart.ascx.cs" Inherits="custom_controls_ccrChart" %>
<%@ Register src="~/custom/genzyme/customercontactreports/controls/ccrChartTemplate.ascx" tagname="ccrChart" tagprefix="pinso" %>

<!-- Product 1 -->

<div id="chart1National"  onmousedown="javascript:showHideGrids('1National');">
    <pinso:ccrChart runat="server" id="chartProduct1National" Thumbnail="true" />
    </div>

<div id="chart1" onmousedown="javascript:showHideGrids('1');">
    <pinso:ccrChart runat="server" id="chartProduct1" Thumbnail="true" />
    </div>

<!-- Product 2 -->

<div id="chart2National" onmousedown="javascript:showHideGrids('2National');">
    <pinso:ccrChart runat="server" id="chartProduct2National" Thumbnail="true" />
    </div>

<div id="chart2" onmousedown="javascript:showHideGrids('2');">
    <pinso:ccrChart runat="server" id="chartProduct2" Thumbnail="true" />
    </div>

<!-- Product 3 -->

<div id="chart3National" onmousedown="javascript:showHideGrids('3National');">
    <pinso:ccrChart runat="server" id="chartProduct3National" Thumbnail="true" />
    </div>

<div id="chart3" onmousedown="javascript:showHideGrids('3');">
    <pinso:ccrChart runat="server" id="chartProduct3" Thumbnail="true" />
    </div>
