<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Chart.ascx.cs" Inherits="marketplace_controls_Chart" %>
<%@ Register src="~/prescriberreporting/controls/ChartTemplate.ascx" tagname="Chart" tagprefix="pinso" %>
<%@ Register src="~/prescriberreporting/controls/GridTemplate.ascx" tagname="Grid" tagprefix="pinso" %>
<div id="chartLeft">
    <div id="chartLeft1">
        <div id="chart1" class="tileContainer" >
            <div class="tileContainerHeader" onclick="javascript:chartClicked(1);">
                <div class="title"><asp:Label ID="lbl1" runat="server" Text="Region"></asp:Label></div>
                <div class="tools">
                    <img class="showHideBtn enlarge" alt="enlarge" title="enlarge" src="content/images/spacer.gif"
                        onclick="maxMrkt(1, event);" />
                    <img class="showHideBtn close" alt="close" title="close" src="content/images/spacer.gif"
                        onclick="minMrkt(1, event);" />
                </div>
                <div class="pagination"></div>
                <div class="clearAll"></div>
            </div>
            <div id="chartContainer1" >
                <pinso:Chart runat="server" id="chartDisplay1" Thumbnail="true" ShowLegendOnlyInExport="true"  />
            </div>
            <div id="gridContainer1">
                <pinso:Grid runat="server" id="grid1" />
            </div>
        </div>
    </div>
    <div id="chartLeft2">
        <div id="chart2" class="tileContainer" >
            <div class="tileContainerHeader" onclick="javascript:chartClicked(2);">
                <div class="title"><asp:Label ID="lbl2" runat="server" Text="District"></asp:Label></div>
                <div class="tools">
                    <img class="showHideBtn enlarge" alt="enlarge" title="enlarge" src="content/images/spacer.gif"
                        onclick="maxMrkt(2, event);" />
                    <img class="showHideBtn close" alt="close" title="close" src="content/images/spacer.gif"
                        onclick="minMrkt(2, event);" />
                </div>
                <div class="pagination"></div>
                <div class="clearAll"></div>
            </div>
            <div id="chartContainer2" >
                <pinso:Chart runat="server" id="chartDisplay2" Thumbnail="true" ShowLegendOnlyInExport="true" />
            </div>
            <div id="gridContainer2">
                <pinso:Grid runat="server" id="grid2" />
            </div>
        </div>
    </div>
    <div class="clearAll"></div>
</div>
<div id="chartRight">
    <div id="chart3" class="tileContainer" >
        <div class="tileContainerHeader" onclick="javascript:chartClicked(3);">
            <div class="title"><asp:Label ID="lbl3" runat="server" Text="Territory"></asp:Label></div>
            <div class="tools">
                <img class="showHideBtn enlarge" alt="enlarge" title="enlarge" src="content/images/spacer.gif"
                    onclick="maxMrkt(3, event);" />
                <img class="showHideBtn close" alt="close" title="close" src="content/images/spacer.gif"
                    onclick="minMrkt(3, event);" />
            </div>
            <div class="pagination"></div>
            <div class="clearAll"></div>
        </div>
        <div id="chartContainer3" >
            <pinso:Chart runat="server" id="chartDisplay3" Thumbnail="true"  ShowLegendOnlyInExport="true"/>
        </div>
        <div id="gridContainer3">
            <pinso:Grid runat="server" id="grid3" />
        </div>
    </div>
</div>
<div class="clearAll"></div>
