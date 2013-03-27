<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Chart.ascx.cs" Inherits="custom_warner_formularyhistoryreporting_controls_Chart" %>
<%@ Register Src="~/custom/warner/formularyhistoryreporting/controls/GridTemplate.ascx" TagName="Grid" TagPrefix="pinso" %>
<%@ Register Src="~/custom/warner/formularyhistoryreporting/controls/ChartTemplate.ascx" TagName="Chart" TagPrefix="pinso" %>
<%@ Register Src="~/custom/warner/formularyhistoryreporting/controls/ChartTemplateTrx.ascx" TagName="ChartTrx" TagPrefix="pinso" %>

<div id="chartLeft">
<div id="chartLeft1" style="width:49.7%;float:left;">
    <div id="chart1" class="tileContainer" >
            <div class="tileContainerHeader" style="background:#20b1aa;">
                <div class="title"><asp:Label>Lives</asp:Label></div>
                <div class="tools">
                    <img class="showHideBtn enlarge" alt="enlarge" title="enlarge" src="content/images/spacer.gif"
                        onclick="maxcustomMrkt(1, event);" />
                    <img class="showHideBtn close" alt="close" title="close" src="content/images/spacer.gif"
                        onclick="mincustomMrkt(1, event);" />
                </div>
                <div class="pagination"></div>
                <div class="clearAll"></div>
            </div>
            <div id="chartContainer1" >
              <pinso:Chart runat="server" id="chartDisplay1" Thumbnail="true" />
           </div> 
            <div id="gridContainer1" >
                <pinso:Grid runat="server" id="grid1" />
            </div>
   </div>
</div>
<div id="chartLeft2" style="width:49.8%;float:right;">
    <div id="chart2" class="tileContainer" >
            <div class="tileContainerHeader" style="background:#fda601;">
                <div class="title"><asp:Label>Rx Volume</asp:Label></div>
                <div class="tools">
                    <img class="showHideBtn enlarge" alt="enlarge" title="enlarge" src="content/images/spacer.gif"
                        onclick="maxcustomMrkt(2, event);" />
                    <img class="showHideBtn close" alt="close" title="close" src="content/images/spacer.gif"
                        onclick="mincustomMrkt(2, event);" />
                </div>
                <div class="pagination"></div>
                <div class="clearAll"></div>
            </div>
            <div id="chartContainer2" >
              <pinso:ChartTrx runat="server" id="chartDisplay2" Thumbnail="true"/>
           </div> 
            <div id="gridContainer2" >
                <pinso:Grid runat="server" id="grid2" />
            </div>
        </div>
</div>
</div> 
