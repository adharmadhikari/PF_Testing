<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterTimeFrame.ascx.cs" Inherits="marketplaceanalytics_controls_FilterTimeFrame" %>
<%@ Register Namespace="Pathfinder" TagPrefix="pinso" %>

<div id="timeFrameContainer">
    <div id="timeFrameCalendar">
        <div id="timeFrameLeft">
            <div id="timeFrameLeft1">
                <div id="divYearContainer" class="tileContainer">
                    <div class="tileContainerHeader">
                        <div class="title">Year</div>
                        <div class="tools"></div>
                        <div class="pagination"></div>
                        <div class="clearAll"></div>
                    </div>
                    <div id="timeFrameYearContainer">
                        <pinso:CheckboxValueList CssClass="chkBoxDiv" runat="server" ID="Year_Selection" DataSourceID="dsYears" DataTextField="Year" GroupValues="true" HasAllOption="false" DataValueField="Year" RepeatDirection="Horizontal" RepeatColumns="2" BreakCount="2"/>
                        <pinso:CheckboxListClientControl runat="server"  ID="yearSelectionList" Target="Year_Selection" ContainerID="timeFrameContainer" BreakCount="2"/>
                    </div>
                </div>        
            </div>
            <div id="timeFrameLeft2">
                <div id="divQuarterContainer" class="tileContainer">
                    <div class="tileContainerHeader">
                        <div class="title">Quarter</div>
                        <div class="tools">
                            <div class="selectAll">
                                <input type="checkbox" id="optionAllQuarter" onclick="javascript:toggleChecked(this.checked, 'timeFrameQuarterContainer');" /><label for="optionAllQuarter" >Select All</label>
                            </div>
                        </div>
                        <div class="pagination"></div>
                        <div class="clearAll"></div>
                    </div>
                    
                    <div id="timeFrameQuarterContainer">
                        <pinso:CheckboxValueList CssClass="chkBoxDiv" runat="server" ID="Quarter_Selection" DataSourceID="dsQuarters" DataTextField="ShortName" HasAllOption="false" GroupValues="true" DataValueField="Number" RepeatDirection="Horizontal" RepeatColumns="2" BreakCount="2" />
                        <pinso:CheckboxListClientControl runat="server" ID="quarterSelectionList" Target="Quarter_Selection" ContainerID="timeFrameContainer" BreakCount="2" />
                    </div>
                </div> 
            </div>
        </div>    
        <div id="timeFrameRight" >
            <div id="divMonthContainer" class="tileContainer">
                <div class="tileContainerHeader">
                    <div class="title">Month</div>
                    <div class="tools">
                        <div class="selectAll">
                            <input type="checkbox" id="optionAllMonth" onclick="javascript:toggleChecked(this.checked, 'timeFrameMonthContainer');" /><label for="optionAllMonth" >Select All</label>
                        </div>
                    </div>
                    <div class="pagination"></div>
                    <div class="clearAll"></div>
                </div>                
                <div id="timeFrameMonthContainer">
                    <pinso:CheckboxValueList CssClass="chkBoxDiv" runat="server" ID="Month_Selection" DataSourceID="dsMonths" DataTextField="ShortName" HasAllOption="false" GroupValues="true"  DataValueField="Number" RepeatDirection="Horizontal"  BreakCount="6"/>
                    <pinso:CheckboxListClientControl runat="server" ID="monthSelectionList" Target="Month_Selection" ContainerID="timeFrameContainer" BreakCount="6"   />
                </div>
            </div> 
        </div> 
    </div> 
    <div id="timeFrameRolling">
        <div id="divRollingContainer" class="tileContainer">
            <div class="tileContainerHeader">
                <div class="title">Rolling Quarters (Qtr 1 is most recent)</div>
                <div class="tools">
                    <div class="selectAll">
                        <input type="checkbox" id="optionAllQuarterRolling" onclick="javascript:toggleChecked(this.checked, 'timeFrameRollingContainer');" /><label for="optionAllQuarterRolling" >Select All</label>
                    </div>
                </div>
                <div class="pagination"></div>
                <div class="clearAll"></div>
            </div>                
            <div id="timeFrameRollingContainer">
                <pinso:CheckboxValueList CssClass="chkBoxDiv" runat="server" ID="Rolling_Selection" DataSourceID="dsRollingQuarters" DataTextField="ShortName" HasAllOption="false" GroupValues="true"  DataValueField="Number" RepeatDirection="Horizontal"  BreakCount="6"/>
                <pinso:CheckboxListClientControl runat="server" ID="rollingSelectionList" Target="Rolling_Selection" ContainerID="timeFrameContainer" BreakCount="6"   />
            </div>
        </div> 
    </div>
</div>
<asp:EntityDataSource runat="server" ID="dsYears" DefaultContainerName="PathfinderMarketplaceAnalyticsEntities" ConnectionString="name=PathfinderMarketplaceAnalyticsEntities" EntitySetName="LkpMarketplaceYearsSet" />
<asp:EntityDataSource runat="server" ID="dsQuarters" DefaultContainerName="PathfinderMarketplaceAnalyticsEntities" ConnectionString="name=PathfinderMarketplaceAnalyticsEntities" EntitySetName="LkpMarketplaceShortLongQuarterNamesSet" />
<asp:EntityDataSource runat="server" ID="dsRollingQuarters" DefaultContainerName="PathfinderMarketplaceAnalyticsEntities" ConnectionString="name=PathfinderMarketplaceAnalyticsEntities" EntitySetName="LkpMarketplaceShortLongRollingQuarterNamesSet" />
<asp:EntityDataSource runat="server" ID="dsMonths" DefaultContainerName="PathfinderMarketplaceAnalyticsEntities" ConnectionString="name=PathfinderMarketplaceAnalyticsEntities" EntitySetName="LkpMarketplaceShortLongMonthNamesSet" />
