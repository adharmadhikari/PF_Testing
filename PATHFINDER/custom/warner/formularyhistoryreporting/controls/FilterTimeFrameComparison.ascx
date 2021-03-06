﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterTimeFrameComparison.ascx.cs" Inherits="custom_warner_formularyhistoryreporting_controls_FilterTimeFrameComparison" %>
<%@ Register src="~/custom/warner/formularyhistoryreporting/controls/FilterTimeFrameComparisonSelector.ascx" tagname="filterTimeFrameComparisonSelector" tagprefix="pinso" %>
<%@ Register Namespace="Pathfinder" TagPrefix="pinso" %>

<div id="timeFrameContainer">
    <div id="timeFrameComparisonLeft">
        <div id="timeFrameComparisonLeft1">
            <div id="divYearContainer" class="tileContainer">
                <div class="tileContainerHeader">
                    <div class="title">Selection 1</div>
                    <div class="tools"></div>
                    <div class="pagination"></div>
                    <div class="clearAll"></div>
                </div>
                <div id="timeFrameSelection1Container">
                    <pinso:filterTimeFrameComparisonSelector ID="filterTimeFrameComparisonSelector1" runat="server" />
                </div>
            </div>        
        </div>
        <div id="timeFrameComparisonLeft2">
            <div id="divQuarterContainer" class="tileContainer">
                <div class="tileContainerHeader">
                    <div class="title">Selection 2</div>
                    <div class="tools"></div>
                    <div class="pagination"></div>
                    <div class="clearAll"></div>
                </div>                
                <div id="timeFrameSelection2Container">
                    <pinso:filterTimeFrameComparisonSelector ID="filterTimeFrameComparisonSelector2" runat="server" />
                </div>
            </div> 
        </div>
    </div>    

</div>