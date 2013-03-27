/// <reference name="MicrosoftAjax.js"/>
/// <reference path="~/content/scripts/jquery-1.4.1-vsdoc.js"/>

//flashmap required vars
var fmASMcPath;
var fmEngine;
var fmASEngine;

//global vars which act a % to compute width for the tiled areas depending of the browser window size
var topLf = 2.42;
var topRt = 1.76;
var mapEnlarge = 1.5;
var modalEnlarge = 2.3;
var topLfSR = 3;
var topSRHeight = 2.85;
var ie6 = $.browser.msie && $.browser.version == "6.0";
var ie7 = $.browser.msie && $.browser.version == "7.0";
var ie8 = $.browser.msie && $.browser.version == "8.0";
var ie9 = window.navigator.userAgent.indexOf("Trident/5") > -1;
var chrome = window.navigator.userAgent.indexOf("Chrome/") > -1;
var safari = window.navigator.userAgent.indexOf("Safari/") > -1;
var mac = window.navigator.platform.indexOf("Mac") > -1;
var animationSpeed = 300;
var gridResetCmd = null;
var flashSupported = window.navigator.userAgent.indexOf("iPad;") == -1;

$(document).ready(function()
{
    $("#fauxModal").fadeTo(0, 0.0);
    $("#fauxModal").css({
        visibility: "hidden"
    }
        );

    $("#divTile1").mouseenter(fixMapTip);
}
);

function safeSub(val1, val2)
{
    var val = val1 - val2;
    return val >= 0 ? val : 0;
}
//CLEARS TELERIK COMPUTE WIDTH IN HEADERS FOR THE DATA TABLES
function resetGridHeadersX(x)
{
    if (gridResetCmd) gridResetCmd.cancel();
    gridResetCmd = new cmd(null, resetGridHeaders, [], x);
}
function resetGridHeaders()
{
    $("#tile3Min .dashboardTable .rgDataDiv, #tile3Max .dashboardTable .rgDataDiv, .singleSection #tile3 .dashboardTable .rgDataDiv  ").css({
        overflow: "visible"
    }
      );

    if (ie6)
    {
        $("#tile2 .dashboardTable .rgHeaderDiv, #tile5 .dashboardTable .rgHeaderDiv").css({
            width: "auto", paddingRight: "0px"
        }
      );
        $(".section2SR #tile5 .dashboardTable .rgDataDiv").css({
            width: "100%", paddingRight: "0px"
        }
      );
        if ($(".livesdistribution").length == 0)
        {
            $("#tile3 #divTile3 .rgHeaderDiv").css({ width: "auto", paddingRight: "0px" });
        }
        //SPH 02/15/2010 - added clearing marginRight - may be better way but this fixes issues with ie6 going crazy on resize when data is present in drill down grid of geo cvg report
        $("#tile5FSDataDrillDown .rgHeaderDiv, #tile5TCDataDrillDown .rgHeaderDiv").css({
            width: "auto", paddingRight: "16px", marginRight: ""
        }
      );
        $("#tile5CLDataDrillDown .rgFooterDiv").css({
            paddingRight: "0px"
        }
      );


        $("#maxAff .dashboardTable .rgHeaderDiv").css({
            width: "auto", paddingRight: "0px"
        }
      );
        $(".section2SR #tile5 .dashboardTable .rgHeaderDiv, .section2SR #tile5 .dashboardTable .rgFooterDiv ").css({
            width: "auto"
        }
      );
        $(".section2SR #tile5 .dashboardTable .rgDataDiv").css({
            width: "auto"
        }
      );
        $(".section2SR #tile4 .dashboardTable .rgDataDiv").css({
            width: "100%"
        }
      );
        $("#ctl00_main_gridContacts_GridHeader").css({
            width: "100%"
        });
    }
    else
    {
        $("#tile2 .dashboardTable .rgHeaderDiv, #tile5 .dashboardTable .rgHeaderDiv, .section2SR #tile4 .dashboardTable").css({
            width: "auto"
        }
      );
        $(".section2SR #tile5 .dashboardTable .rgDataDiv, #tile5T .dashboardTable .rgHeaderDiv,  #maxTBtm .dashboardTable .rgHeaderDiv").css({
            width: "auto"
        }
      );
        $(".section2SR #tile4 .dashboardTable .rgDataDiv").css({
            width: "100%"
        }
      );
        $("#maxAff .dashboardTable .rgHeaderDiv").css({
            width: "auto", paddingRight: "0px"
        }
      );
        $("#tile3 #srTile1 .dashboardTable .rgHeaderDiv").css({
            width: "auto", marginRight: "0px"
        }
      );
        $("#tile3 .dashboardTable .rgHeaderDiv").css({
            width: "auto", marginRight: "0px"
        }
      );
        $(".section2SR #tile5 .dashboardTable .rgHeaderDiv, .section2SR #tile5 .dashboardTable .rgFooterDiv ").css({
            width: "auto"
        }
      );
        $("#tile3 #divTile3 .rgHeaderDiv").css({
            width: "auto", paddingRight: "0px"
        }
      );
        $(".section2SR #tile3 #divTile3 .rgHeaderDiv").css({
            width: "auto"
        }
      );
        $(".rgDataDiv .rgMasterTable").each(function() { padScrollbar(this); });
    }


    if (mac & safari)
    {
        //Hack to fix header alignment for rad grids
        setTimeout(function()
        {
            $('.rgHeaderDiv').css('margin-right', '0px');
            $('.rgFooterDiv').css('margin-right', '0px');
            $('.rgMasterTable').css('width', '99.9%');
        }, 1000);
    }

    if (chrome || (!(mac) && safari) || ie9)
    {
        setTimeout(function()
        {
            $('.rgMasterTable').css('width', '98%');
            setTimeout(function()
            {
                $('.rgMasterTable').css('width', '100%');
            }, 100);
        }, 100);
    }

    if (ie7)
    {

        //        $(".rgHeaderDivPlanInfo").removeClass("rgHeaderDivPlanInfo"); //<---this line of code fixes ie8 running as ie7 (compat mode) but doesn't work correctly in real ie7 sp3
        //        $("#ctl00_main_planInfo_gridPlanInfo .rgHeaderDiv").css("margin-right", 0);

        //        $("#ctl00_Tile5_gridFSDrilldown_gridformularystatusdrilldown .rgHeaderDiv").css("margin-right", 0);

        $("#ctl00_Tile3_AffiliationsListView_gridAffiliations .rgHeaderDiv").css("margin-right", 0);
        $("#ctl00_Tile3_AffiliationsListView_gridAffiliations .rgDataDiv").css("overflow-x", "hidden");
        $("#ctl00_Tile3_AffiliationsListView_gridAffiliations .rgDataDiv table").width("100%");
    }

    //Fix for header alignment issues in IE6
    if (ie6 && ($(".customercontactreports").length > 0))
        $(".rgDataDiv .rgMasterTable").each(function() { padScrollbar(this); });

}
function getTile1Height()
{
    var browserWindow = $(window);
    var divHeight = browserWindow.height();
    var divWidth = browserWindow.width();
    var divTile1Height = getDivTile1HeightForTA(divWidth, topLf);
    return divTile1Height + 27;
}

function getDivTile1HeightForTA(divHeight)
{
    //var screenRat = window.screen.height / window.screen.width;
    //depending on monitor res the top section will be maxed at 1/3 of max height unless resolution is narrow widescreen which will be 1/4 max height
    //var taMaxTopHeight = window.screen.height / (screenRat > 0.65 ? 3 : 4);
    var taMaxTopHeight = $(window).height() / ($(window).height() > 700 ? 3 : 3.5);
    var divTile1Height = divHeight / 3;

    if (divTile1Height > taMaxTopHeight) divTile1Height = taMaxTopHeight;

    return divTile1Height;
}

function fixMapTip()
{
    //SPH 3/5/2010 - Fix tooltip scaling - REQUIRES FLASH MAPS UPDATE!
    try
    {
        if (fmEngine && clientManager.get_MapIsReady())
        {
            if (mapToolTipCmd) mapToolTipCmd.cancel();
            mapToolTipCmd = new cmd(null, fmRecalcWidthHeightFactor, [], 500);
        }
    }
    catch (ex) { }
    //-----
}

//TODAY'S ACCOUNTS
function resizeSearchTextBox()
{
    var jsearchBox = $("#divTile2 .planName .searchTextBox .textBox");
    var jsearchBoxP = jsearchBox.parent();
    jsearchBox.width(jsearchBoxP.width() - 20);
}
function todaysaccounts_content_resize()
{
    var browserWindow = $(window);
    var divHeight = browserWindow.height();
    var divWidth = browserWindow.width();
    var divTile1Height = getDivTile1HeightForTA(divHeight);
    var tile1Height = divTile1Height + 27;
    if (divHeight > 700)
    {
        var divideBy = 3;
    } else
    {
        var divideBy = 3.5;
    }
    //    $(".tileContainerHeader").show(); //remove this after custome options screen shot demo is gone
    $("#tile1 .min").show();

    //IE 7 hack, only hide Benefit Design infoPopup if not IE7 or Benefet Design grid not visible
    if (!(ie7 && $('#ctl00_Tile5_CoveredLivesDrillDown1_gridcoveredlivesdrilldown').length > 0))
        $("#infoPopup").hide();

    $("#fauxModal").css({
        width: divWidth, height: divHeight
    }
   );
    $("#ctl00_main_planInfo_gridPlanInfo").css({
        height: divTile1Height - $('#planInfoLegend').height()
    }
   );

    $("#expandTile1, #tileMin1").css({
        height: tile1Height
    }
   , animationSpeed);
    grid_resize(divTile1Height);
    $("#tile1").css({
        width: (divHeight / divideBy) * 1.666, height: tile1Height
    }
      , animationSpeed);
    $("#tile1 #divTile1").css({
        height: divTile1Height
    }
      , animationSpeed);
    var marginLft = isTile1Visible() ? (divHeight / divideBy) * 1.666 : 30;

    //SPH 1/7/2010 - Chrome fix (safari reference setting marginLeft)
    $("#tile2").css({ height: tile1Height, marginLeft: (!$.browser.safari ? marginLft : "") });

    props = { height: tile1Height };
    if (chrome) props["width"] = divWidth - 50;
    $("#tileMin2").css(props);
    //end chrome width fix


    $("#maxPlanInfo").css({
        width: (divWidth / topLf) * modalEnlarge, height: (divHeight / 1.15), top: divHeight / 15, right: divWidth / 37
    }
   , animationSpeed);

    //SPH 2/10/2010 moved maxed grid resize code to grid_resize
    //      if (ie6)
    //      {
    //          $("#maxPlanInfo #divTile2Plans .dashboardTable .rgDataDiv").css({
    //              height: divHeight - 112
    //          }
    //       );
    //          $("#maxPlanInfo #divTile2Plans .dashboardTable").css({
    //              height: divHeight - 123
    //          }
    //   , animationSpeed);

    //      } else
    //      {
    //          $("#maxPlanInfo #divTile2Plans .dashboardTable .rgDataDiv").css({
    //              height: divHeight - 119
    //          }
    //       );
    //          $("#maxPlanInfo #divTile2Plans .dashboardTable").css({
    //              height: divHeight - 100
    //          }
    //   , animationSpeed);

    //      }
    $("#maxMap").css({
        width: ((divHeight / divideBy) * 1.666) * mapEnlarge, height: (tile1Height * mapEnlarge) - 15, top: divHeight / 6, left: divWidth / 5
    }
   , animationSpeed, showModal);
    $("#maxMap #divTile1").css({
        height: (tile1Height - 27) * mapEnlarge, width: (divWidth / topLf) * mapEnlarge
    }
   , animationSpeed);
    $("#maxAff").css({
        width: (divWidth / topLf) * modalEnlarge, height: (divHeight / 1.15), top: divHeight / 15, left: divWidth / 40
    }
   , animationSpeed);
    $("#maxAff #divTile3").css({
        height: divHeight - 143
    }
   );
    $("#maxAff #ctl00_Tile3_AffiliationsListView_gridAffiliations_GridData").css({
        height: divHeight - 162
    }
   );
    $("#maxKC").css({
        width: (divWidth / topLf) * modalEnlarge, height: (divHeight / 1.15), top: divHeight / 15, left: divWidth / 40
    }
   , animationSpeed);
    $("#maxKC #kcView").css({
        height: divHeight - 160
    }
   );
    $(".todaysAccounts2Max").css({
        width: (divWidth / topLf) * modalEnlarge, height: (divHeight / 1.15), top: divHeight / 15, left: divWidth / 40
    }
   , animationSpeed);
    $("#divTile3Max, #divTile4Max, #divTile5Max").css({
        height: divHeight - 143
    }
   );

    //SPH 1/11/2010 - take out if animation put back
    showModal();
    //SPH 1/11/2010 - take out if animation put back

    resizeSearchTextBox();

    //    fixMapTip();

    todaysaccounts_section_resize();
}
var mapToolTipCmd = null;

// SECTION RESIZE CONTROLS RESIZING FOR BOTTOM TILES OF APP, GETS CALLED ON BY CLIENT MANAGER, RUNS ON EVERY CLICK 
function todaysaccounts_section_resize()
{
    // var gets current window size
    var windowHeight = $(window);
    var divHeight = windowHeight.height();
    var divWidth = windowHeight.width();
    // height for map container #divTile1, map width divided by a number to keep aspect ratio of map
    var divTile1Height = getDivTile1HeightForTA(divWidth, topLf);

    // gets height of the height of the #tile1
    var tile1Height = divTile1Height + 27;

    //SPH 02/10/2010 - Moved to TA Application code
    //   $("#section2 .enlarge").show();
    //   $("#tile2 .enlarge").show();
    //   $("#tile2 .min").hide();

    // sets heights for these containers in lower section
    $("#tile3 #divTile3, #tile4 #divTile4, #tile5 #divTile5").css({
        height: safeSub((divHeight - tile1Height), 169)
    }
       );
    //SPH 02/09/2010 - Sub 251 for ie7 as well as 6 (changed from 255 to 251 also)
    var div5Ftr = $("#CoveredLivesDrilldownFooter").height();
    $("#tile5 #divTile5 .rgDataDiv").css({
        height: safeSub((divHeight - tile1Height), ((ie6 || ie7 ? 251 : 240) + div5Ftr))
    }
   );
    //resizing for Key Contacts Grid
    $("#tile4Min #myKcView, #tile3Min #kcView").css("height", (divHeight - (tile1Height)) - 190);
    $("#tile3Max #kcView, #tile4Max #myKcView").css("height", (divHeight - (tile1Height)) - 190);
    //resizing for Affiliations Data Grid
    $("#tile3 #ctl00_Tile3_AffiliationsListView_gridAffiliations_GridData").css("height", (divHeight - (tile1Height)) - 188);
    //Sets Key Contacts tile3 maximized div width
    if (ie6)
    {
        $("#tile3Max").css({
            width: divWidth - 48
        }
   , animationSpeed);
    } else
    {
        $("#tile3Max").css({
            width: divWidth - 52
        }
   );
    }


    //Sets width of My Key Contacts minimized div "expand my key contacts"
    //SPH 1/11/2010 - Added condition to divTile4Container selection so it is only resized if tile4 wasn't renamed by maximizing tile - 2/16/2010 - added condition for tile5 because BD was 1 pixel high because of calc
    $("#tile4Min, #tile4Max, #expandTile4").css({
        height: safeSub((divHeight - tile1Height), 142)
    }
   );
    $("#tile3Max").css({
        height: (divHeight - tile1Height) - 140
    }
   );
    //Sets Key Contacts tile3 minimized div width 
    $("#tile3Min").css({
        width: divWidth / 2 - 12
    }
       );
    //Sets My Key Contacts tile4 maxmimized div width 
    $("#tile4Max").css({
        width: divWidth / 2 - 12
    }
       );


    //required so sorting works on maxed Benefit Design - this section does postback so when it returns it is not configured properly so we need to call maxCL again but with no animation.
    if ($("#section2").hasClass("todaysAccounts2Max"))
    {
        maxCL(true);
    }

    fixIEScroll();

    if (ie7)
        $("#ctl00_Tile3_AffiliationsListView_gridAffiliations .rgDataDiv table").width("100%");

    //clears Telerik computed width in the headers for the data table
    //setTimeout("resetGridHeaders()", 1500);
    resetGridHeadersX(500);
}
//END TODAY'S ACCOUNTS

//TODAY'S ANALYTICS
function todaysanalytics_content_resize()
{
    var browserWindow = $(window);
    var divHeight = browserWindow.height();
    var divWidth = browserWindow.width();
    var tile2Height = safeSub(divHeight, 105);
    var collaspeLft = $(".todaysAccounts2Expand").height();

    //    $(".tileContainerHeader").show(); //remove this after custome options screen shot demo is gone

    $("#fauxModal").css({
        width: divWidth, height: divHeight
    });


    $(" #tile2 ").css({
        width: "200px", top: "5px", left: "6px", position: "absolute", marginLeft: "0px"
    }
      );
    if (collaspeLft > 0)
    {
        $("#section2").css({
            marginLeft: "34px"
        }, animationSpeed);
    } else
    {
        $("#section2").css({
            marginLeft: "208px"
        }
       , animationSpeed);
    }

    //Tile2 properties
    $("#tile2, #tileMin2SR").css({
        height: tile2Height
    }
   , animationSpeed);
    $("#expandTile2SR, #tileMin2").css({
        height: safeSub(divHeight, 112)
    }
    , animationSpeed);
    $("#expandTile2SR").css({
        height: "100%"
    }
    , animationSpeed);
    $("#tile2 .min").show();
    $("#tile2 .enlarge, #divTile2Plans").hide();

    reportFiltersResize();
    ////Right part of SR   
    todaysanalytics_section_resize();
}

function todaysanalytics_section_resize()
{
    var browserWindow = $(window);
    var divHeight = browserWindow.height();
    var divWidth = browserWindow.width();
    var tile2Height = divHeight / topSRHeight;
    var ie6 = $.browser.msie && $.browser.version == "6.0";
    var hdrElement = $("#divTile4 thead tr");
    var height = 20;
    if ($get("tile2") && $(".maximized").length == 0)
        $(".section2SR .enlarge").show();
    $("#maxTChart .enlarge, #maxSRMap .enlarge, #maxTBtm .enlarge, #maxChart .enlarge, #maxSRTile4 .enlarge, #maxSRTile5 .enlarge").hide();
    if (hdrElement.length > 0)
    {
        height = Sys.UI.DomElement.getBounds(hdrElement[0]).height;
    }
    //Tile 3 Properties
    var timeFrameHeight = 58;

    //Hide Timeframecontainer if formularyhistoryreporting
    if (clientManager.get_Module() == 'formularyhistoryreporting')
        timeFrameHeight = 0;

    //$("#timeFrameContainer").css({
    //    height: timeFrameHeight, textAlign: "center", width: "auto", overflow: "hidden"
    //});

    //$("#timeFrameContainer #divYearContainer, #timeFrameContainer #divQuarterContainer, #timeFrameContainer #divMonthContainer").css({
    //    height: (timeFrameHeight - 2)
    //});

    $("#tile4 #divTile4Container").css({
        //height: timeFrameHeight, textAlign: "center", width: "auto", overflow: "hidden"
        height: (safeSub(safeSub(divHeight, 131), timeFrameHeight) * .7), textAlign: "center", width: "auto", overflow: "hidden"
    });

    $("#tile4 #chart1, #tile4 #chart2, #tile4 #chart3").css({
        height: $("#tile4 #divTile4Container").height() - 2, width: "auto", overflow: "hidden"
    });


    $("#tile4 #chart1 .chartContainer, #tile4 #chart1 #chartContainer1, #tile4 #chart2 .chartContainer, #tile4 #chart2 #chartContainer2, #tile4 #chart3 .chartContainer, #tile4 #chart3 #chartContainer3").css({
        height: safeSub($("#tile4 #chart1").height(), $("#tile4 #chart1 .tileContainerHeader").height()) * .75
    });

    $("#tile4 #chart1 #gridContainer1, #tile4 #chart2 #gridContainer2, #tile4 #chart3 #gridContainer3").css({
        height: safeSub($("#tile4 #chart1").height(), ($("#tile4 #chart1 .tileContainerHeader").height() + $("#tile4 #chart1 .chartContainer").height()))
    });

    //Set the height of the grids
    $("#gridContainer1 .rgDataDiv").height($("#gridContainer1").height() - $("#gridContainer1 .rgHeaderDiv").height());
    $("#gridContainer2 .rgDataDiv").height($("#gridContainer2").height() - $("#gridContainer2 .rgHeaderDiv").height());
    $("#gridContainer3 .rgDataDiv").height($("#gridContainer3").height() - $("#gridContainer3 .rgHeaderDiv").height());

    //Dynamically set the widths of the chart areas based on number of charts - CSS file is set to display 3 charts by default
    var chartCount = getMrktChartCount();

    if (chartCount == 1)
    {
        $("#chartRight:not(.maximized)").css("width", "0%");
        $("#chart3").css("display", "none");
        $("#chart2").css("display", "none");
        $("#chartLeft").css("width", "100%");
        $("#chartLeft2:not(.maximized)").css("width", "0%");
        $("#chartLeft1:not(.maximized)").css("width", "100%");
    }

    if (chartCount == 2)
    {
        $("#chartRight:not(.maximized)").css("width", "0%");
        $("#chart3").css("display", "none");
        $("#chart2").css("display", "");
        $("#chartLeft").css("width", "100%");
        $("#chartLeft2:not(.maximized)").css("width", "49.8%");
        $("#chartLeft1:not(.maximized)").css("width", "49.7%");

        if ($(".prescriberreporting").length > 0)
        {
            if ($("#ctl00_Tile4_chart_grid1_gridTemplate").length < 1)
            {
                $("#chart1").css("display", "none");
                $("#chartLeft2:not(.maximized)").css("width", "100%");
                $("#chartLeft1:not(.maximized)").css("width", "0%");
            }
        }
    }

    if (chartCount == 3)
    {
        $("#chartRight:not(.maximized)").css("width", "33.5%");
        $("#chart3").css("display", "");
        $("#chart2").css("display", "");
        $("#chartLeft:not(.maximized)").css("width", "66%");
        $("#chartLeft2:not(.maximized)").css("width", "49.5%");
        $("#chartLeft1:not(.maximized)").css("width", "49.6%");

        if ($(".prescriberreporting").length > 0)
        {
            if ($("#ctl00_Tile4_chart_grid1_gridTemplate").length < 1)
            {
                $("#chart1").css("display", "none");
                $("#chartLeft2:not(.maximized)").css("width", "100%");
                $("#chartLeft:not(.maximized)").css("width", "50%");
                $("#chartRight:not(.maximized)").css("width", "49.6%");
                $("#chartLeft1:not(.maximized)").css("width", "0%");
            }
        }
    }

    if (chartCount == 4)//Chart count of 4 = hide regional
    {
        $("#chartRight:not(.maximized)").css("width", "49.8%");
        $("#chart3").css("display", "");
        $("#chart2").css("display", "none");
        $("#chartLeft").css("width", "49.7%");
        $("#chartLeft2:not(.maximized)").css("width", "0%");
        $("#chartLeft1:not(.maximized)").css("width", "100%");

        if ($(".prescriberreporting").length > 0)
        {
            if ($("#ctl00_Tile4_chart_grid1_gridTemplate").length < 1)
            {
                $("#chart1").css("display", "none");
                $("#chartLeft1:not(.maximized)").css("width", "0%");
                $("#chartRight:not(.maximized)").css("width", "100%");
            }
        }
    }


    if (ie6 || ie7)
    {
        if (ie7)
            $("#chart1, #chart2, #chart3").css("width", ""); //100%"); //auto won't cut it for ie6 (ugh!)
        else
            $("#chart1, #chart2, #chart3").css("width", "100%"); //auto won't cut it for ie6 (ugh!)

        var chartCount = $("#chart1 .grid, #chart2 .grid, #chart3 .grid").length;
        if (chartCount < 1) chartCount = 1;

        var chartWidth = ($("#divTile4").width() / chartCount);
        $(".TriStack2 .cloneHeader").width(safeSub(chartWidth, !ie6 ? 10 : 20));
        $("#chartLeft .drillDownContainer, #chartRight .drillDownContainer").width(safeSub(chartWidth, 5));
        if (ie6)
        {
            $("#divTile5 .drillDownContainer, #divTile5 .cloneHeader").width($("#divTile4").width());
            $("#divTile5 .gridClone").css("margin-right", "17px");
        }
        var hasChart3 = $("#chart3").css("display") != "none";

        $("#chartLeft1:not(.maximized), #chartLeft2:not(.maximized), #chartRight:not(.maximized)").width(chartWidth - 3);
        if (chartCount > 2 || !hasChart3)
            $("#chartLeft").width((chartWidth * 2) - (ie7 && hasChart3 ? 2 : 0));
        else
            $("#chartLeft").width(chartWidth);

        $("#gridContainer1 .drillDownContainer").height($("#chart1").height() - $("#chartContainer1").height() - $("#gridContainer1 .cloneHeaderFull").height() - $("#chart1 .tileContainerHeader").height());
        $("#gridContainer2 .drillDownContainer").height($("#chart2").height() - $("#chartContainer2").height() - $("#gridContainer2 .cloneHeaderFull").height() - $("#chart2 .tileContainerHeader").height());
        $("#gridContainer3 .drillDownContainer").height($("#chart3").height() - $("#chartContainer3").height() - $("#gridContainer3 .cloneHeaderFull").height() - $("#chart3 .tileContainerHeader").height());

    }



    $("#tile3").removeClass("leftTile");
    $(".todaysAccounts1").css({
        padding: "0px",
        position: "relative"
    });

    //    //Code to show timeframe selector
    //    $(".navbar2").show().css({ "margin-left": "208px" });

    //Resize dynamic static grid headers
    //    $("#ctl00_Tile4_chart_grid1_gridTemplate_gridClone").width($("#ctl00_Tile4_chart_grid1_gridTemplate").width());
    //    $("#ctl00_Tile4_chart_grid2_gridTemplate_gridClone").width($("#ctl00_Tile4_chart_grid2_gridTemplate").width());
    //    $("#ctl00_Tile4_chart_grid3_gridTemplate_gridClone").width($("#ctl00_Tile4_chart_grid3_gridTemplate").width());
    //    $("#ctl00_partialPage_detailedGrid_gridClone").width($("#ctl00_partialPage_detailedGrid").width());

    //    $("#ctl00_Tile4_chart_grid1_drillDownContainer").height($("#gridContainer1").height() - $("#ctl00_Tile4_chart_grid1_gridTemplate_gridCloneDiv").height());
    //    $("#ctl00_Tile4_chart_grid2_drillDownContainer").height($("#gridContainer2").height() - $("#ctl00_Tile4_chart_grid2_gridTemplate_gridCloneDiv").height());
    //    $("#ctl00_Tile4_chart_grid3_drillDownContainer").height($("#gridContainer3").height() - $("#ctl00_Tile4_chart_grid3_gridTemplate_gridCloneDiv").height());
    //    $("#drillDownContainer").height($("#divTile5").height() - $("#ctl00_partialPage_detailedGrid_gridCloneDiv").height());

    //Resize headers for Table 1, 2, 3 and detailed grid
    resizeStaticHeaders('ctl00_Tile4_chart_grid1_drillDownContainer');
    resizeStaticHeaders('ctl00_Tile4_chart_grid2_drillDownContainer');
    resizeStaticHeaders('ctl00_Tile4_chart_grid3_drillDownContainer');
    resizeStaticHeaders('drillDownContainer');

    $("#ctl00_Tile4_chart_grid1_drillDownContainer").height($("#gridContainer1").height() - $("#ctl00_Tile4_chart_grid1_drillDownContainer_gridCloneDiv").height());
    $("#ctl00_Tile4_chart_grid2_drillDownContainer").height($("#gridContainer2").height() - $("#ctl00_Tile4_chart_grid2_drillDownContainer_gridCloneDiv").height());
    $("#ctl00_Tile4_chart_grid3_drillDownContainer").height($("#gridContainer3").height() - $("#ctl00_Tile4_chart_grid3_drillDownContainer_gridCloneDiv").height());

    //Fix for summary grid width in IE6 & IE7
    if (ie6 || ie7)
    {
        //yeah, we did this already (above) but do it again anyway - makes bad browsers happy :-)
        $("#gridContainer1 .drillDownContainer").height($("#chart1").height() - $("#chartContainer1").height() - $("#gridContainer1 .cloneHeaderFull").height() - $("#chart1 .tileContainerHeader").height());
        $("#gridContainer2 .drillDownContainer").height($("#chart2").height() - $("#chartContainer2").height() - $("#gridContainer2 .cloneHeaderFull").height() - $("#chart2 .tileContainerHeader").height());
        $("#gridContainer3 .drillDownContainer").height($("#chart3").height() - $("#chartContainer3").height() - $("#gridContainer3 .cloneHeaderFull").height() - $("#chart3 .tileContainerHeader").height());
        //

        if ($("#ctl00_Tile4_chart_grid1_drillDownContainer").height() > ($("#ctl00_Tile4_chart_grid1_drillDownContainer").height() + $("#ctl00_Tile4_chart_grid1_drillDownContainer_gridCloneDiv").height()))
            $("#ctl00_Tile4_chart_grid1_drillDownContainer").width($("#ctl00_Tile4_chart_grid1_drillDownContainer").width() - 16);

        if ($("#ctl00_Tile4_chart_grid2_drillDownContainer").height() > ($("#ctl00_Tile4_chart_grid2_drillDownContainer").height() + $("#ctl00_Tile4_chart_grid2_drillDownContainer_gridCloneDiv").height()))
            $("#ctl00_Tile4_chart_grid2_drillDownContainer").width($("#ctl00_Tile4_chart_grid2_drillDownContainer").width() - 16);

        if ($("#ctl00_Tile4_chart_grid3_drillDownContainer").height() > ($("#ctl00_Tile4_chart_grid3_drillDownContainer").height() + $("#ctl00_Tile4_chart_grid3_drillDownContainer_gridCloneDiv").height()))
            $("#ctl00_Tile4_chart_grid3_drillDownContainer").width($("#ctl00_Tile4_chart_grid3_drillDownContainer").width() - 16);
    }


    $("#chartLeft1.maximized, #chartLeft2.maximized, #chartRight.maximized").each(function()
    {
        maxMrkt($(this).attr("_chartidx"), null);
    });

    if ($("#tile5.maximized").length == 0)
    {
        //        $("#tile5 #divTile5Container").css({
        //            //height: timeFrameHeight, textAlign: "center", width: "auto", overflow: "hidden"
        //            height: safeSub(safeSub(divHeight, 131), (timeFrameHeight + $("#tile4 #divTile4Container").height())) + 14, textAlign: "center", width: "auto", overflow: "hidden"
        //        });

        $("#tile5 #divTile5").css({
            height: (safeSub(safeSub(divHeight, 131), (timeFrameHeight + $("#tile4 #divTile4Container").height())) - 12), textAlign: "center"
        });

        //Make the drill down grid a bit higher for formularyhistoryreporting since the timeframe filters are not visible
        if (clientManager.get_Module() == 'formularyhistoryreporting')
            $("#tile5 #divTile5").height($("#tile5 #divTile5").height() + 5);

        $("#drillDownContainer").height($("#divTile5").height() - $("#drillDownContainer_gridCloneDiv").height());
    }
    //Remove div output by gridview
    //    var content1 = $("#ctl00_Tile4_chart_grid1_drillDownContainer div:first");
    //    if (content1.length > 0)
    //    {
    //        $("#ctl00_Tile4_chart_grid1_drillDownContainer div:first").remove();
    //        $("#ctl00_Tile4_chart_grid1_drillDownContainer").append(content1[0].innerHTML);
    //    }

    //    var content2 = $("#ctl00_Tile4_chart_grid2_drillDownContainer div:not([id])").contents();
    //    $("#ctl00_Tile4_chart_grid2_drillDownContainer").append(content1);
    //    $("#ctl00_Tile4_chart_grid2_drillDownContainer div:not([id])").remove();

    //    var content3 = $("#ctl00_Tile4_chart_grid3_drillDownContainer div:not([id])").contents();
    //    $("#ctl00_Tile4_chart_grid3_drillDownContainer").append(content1);
    //    $("#ctl00_Tile4_chart_grid3_drillDownContainer div:not([id])").remove();


    //clears Telerik computed width in the headers for the data table
    //setTimeout("resetGridHeaders()", 1500);
    //resetGridHeadersX(500);
}
//END TODAY'S ANALYTICS



//STANDARD REPORTS
function standardreports_content_resize()
{



    var browserWindow = $(window);
    var divHeight = browserWindow.height();
    var divWidth = browserWindow.width();
    var tile2Height = safeSub(divHeight, 105);
    var collaspeLft = $(".todaysAccounts2Expand").height();

    //    $(".tileContainerHeader").show(); //remove this after custome options screen shot demo is gone

    $("#fauxModal").css({
        width: divWidth, height: divHeight
    });


    $(" #tile2 ").css({
        width: "200px", top: "5px", left: "6px", position: "absolute", marginLeft: "0px"
    }
      );
    if (collaspeLft > 0)
    {
        $("#section2").css({
            marginLeft: "34px"
        }, animationSpeed);
    } else
    {
        $("#section2").css({
            marginLeft: "208px"
        }
       , animationSpeed);
    }

    //Tile2 properties
    $("#tile2, #tileMin2SR").css({
        height: tile2Height
    }
   , animationSpeed);
    $("#expandTile2SR, #tileMin2").css({
        height: safeSub(divHeight, 112)
    }
, animationSpeed);
    $("#expandTile2SR").css({
        height: "100%"
    }
, animationSpeed);
    $("#tile2 .min").show();
    $("#tile2 .enlarge, #divTile2Plans").hide();

    reportFiltersResize();
    ////Right part of SR   
    standardreports_section_resize();

}
function reportFiltersResize()
{
    //    if (!(ie6 || ie7))
    //    {
    var tile2Height = safeSub($(window).height(), 105);
    var sectionHeight = safeSub(tile2Height, 26);

    $("#divTile2ModuleSelection").height(sectionHeight);

    var filtersHeight = $("#filtersContainer").outerHeight() + $("#optionsContainer").outerHeight() + 15;
    var availableHeight = safeSub(sectionHeight, ($("#moduleSubHeader").css("display") == "none" ? 0 : $("#moduleSubHeader").height() * 2) + $("#moduleSelector").height() + $("#channelSelectorContainer").height() + 61);

    if ($get("filterControls"))
    {
        if (filtersHeight <= availableHeight)
        {
            $("#filterControls").css({
                height: filtersHeight
            });
        }
        if (filtersHeight >= availableHeight)
        {
            $("#filterControls").css({
                height: availableHeight
            });
        }
    }



    //fixes FF and Chrome
    $("#filtersContainer .rcbInputCellLeft, #optionsContainer .rcbInputCellLeft").width("90%");
    //    }
    //    else
    //        reportFiltersResizeIE6();

    //IE6 Hack
    if (ie6)
    {
        $("#divTile2ModuleSelection").height(sectionHeight - 1);

        //Fix for Report Filters hiding on resize
        setTimeout("$('.livesdistribution #moduleOptionsContainer').css({'display':'none'});$('.livesdistribution #moduleOptionsContainer').css({'display':'inline'});$('.livesdistribution #filtersContainer').css({'margin-left':'5px'});", 2000);
    }
}

//function reportFiltersResizeIE6()
//{
//    var browserWindow = $(window);
//    var divHeight = browserWindow.height();

//    var filtersHeight = $("#filtersContainer").outerHeight() + $("#optionsContainer").outerHeight() + 15;
//    var availableHeight = safeSub(divHeight, 415);

//    if ($get("filterControls"))
//    {
//        if (filtersHeight <= availableHeight)
//        {
//            $("#filterControls").css({
//                height: filtersHeight
//            });
//        }
//        if (filtersHeight >= availableHeight)
//        {
//            $("#filterControls").css({
//                height: availableHeight
//            });
//        }
//    }
//}

function adjustTile5HeightForDrilldown(maxHeight)
{
    if (!maxHeight)
    {
        var divHeight = $(window).height();
        maxHeight = safeSub(divHeight, getSRTile3Height(divHeight));
    }
    $(".section2SR #tile5:not(.maximized) #divTile5 .dashboardTable .rgDataDiv").css({
        height: safeSub((maxHeight), 163 + $("#tile5 .drillDownTitle").height() + $("#tile5 .rgHeaderDiv").height())
    }
       );
}
function getSRTile3Height(divHeight)
{
    if (!divHeight)
        divHeight = $(window).height();

    if (!$get("tile4") && !$get("tile4SR") && !$get("maxSRTile4") && !$get("tile6"))
    {
        return divHeight - 131;
    }
    else
    {
        return divHeight * .40;
    }
}
function standardreports_section_resize()
{
    var browserWindow = $(window);
    var divHeight = browserWindow.height();
    var divWidth = browserWindow.width();
    var tile2Height = divHeight / topSRHeight;
    var ie6 = $.browser.msie && $.browser.version == "6.0";
    var hdrElement = $("#divTile4 thead tr");
    var height = 20;
    if ($get("tile2"))
        $(".section2SR .enlarge").show();
    $("#maxTChart .enlarge, #maxSRMap .enlarge, #maxTBtm .enlarge, #maxChart .enlarge, #maxSRTile4 .enlarge, #maxSRTile5 .enlarge").hide();
    if (hdrElement.length > 0)
    {
        height = Sys.UI.DomElement.getBounds(hdrElement[0]).height;
    }
    //Tile 3 Properties (if Tile4 & 5 exist statement)
    var tile3Height = getSRTile3Height(divHeight);

    if (ie6)
    {
        $("#tile3 #divTile3Container ").css({
            height: tile3Height
        }
       );
    }

    var maxHeight = divHeight - tile3Height;

    $("#tile4 #divTile4, #tile5 #divTile5, #tile4SR #divTile4, #tile5SR #divTile5 ").css({
        height: safeSub((maxHeight), 164)
    });

    $("#tile3 #divTile3, #tile3SR #divTile3").css({
        height: tile3Height, textAlign: "center", width: "auto", overflow: "hidden"
    }
       );
    $(".section2SR #tile4 #divTile4 .dashboardTable .rgDataDiv, .section2SR #tile5 #divTile5 .dashboardTable .rgDataDiv").css({
        overflow: "auto"
    }
       );

    $(".section2SR #tile4 #divTile4 .dashboardTable .rgDataDiv").css({ height: safeSub((divHeight - tile3Height) - height, 164) });

    var j = $(".section2SR #tile4 #divTile4 .tiercoverage .dashboardTable .rgDataDiv");
    if (j.length == 1)
        j.css("height", safeSub((maxHeight) - height, 182 + $(".reportDataTitle").height()));
    else
        j.css("height", "");

    j = $(".section2SR #tile4 #divTile4 .formularystatus .dashboardTable .rgDataDiv");
    if (j.length == 1)
        j.css("height", safeSub((maxHeight) - height, 164 + $(".reportDataTitle").height()));
    else
        j.css("height", "");

    $("#ctl00_Tile4_tiercoveragedata1_gridtiercoverage_FrozenScroll").css({
        width: "800px"
    }
       );
    $("#maxSRTile4 #ctl00_Tile4_tiercoveragedata1_gridtiercoverage_FrozenScroll").css({
        width: "100%"
    }
       );

    adjustTile5HeightForDrilldown(maxHeight);

    $(".section2SR #tile5 #divTile5 #tile5CLDataDrillDown .dashboardTable .rgDataDiv").css({
        height: safeSub((maxHeight), 210)
    }
       );
    //SPH 3/17/2010 - appears to work same for ie6-8 so taking out conditional and also added adjustment for formulary date
    //    if (ie6)
    //    {
    //        $(".section2SR #tile3 #divTile3 .rgDataDiv").css({
    //            height: divHeight - 165
    //        }
    //       );
    //    } else
    //    {
    $(".section2SR #tile3 #divTile3 .rgDataDiv").css({
        height: divHeight - 130 - $(".section2SR #tile3 #divTile3 .rgHeaderDiv").height() - $(".formularyUpdateDate").height()
    });


    //    }

    $("#tile3T #divTile3, #tile4T #divTile4").css({
        height: tile2Height
    });

    //SPH 2/15/2010 - decreased constant that is subtracted since float is fixed on chart next to map - ie6 still not correct so putting 1px back
    $("#tile5T #divTile5").css({
        height: divHeight - tile2Height - 163 - (ie6 ? 1 : 0)
    }
       );
    $("#tile5T #divTile5 .dashboardTable .rgDataDiv").css({
        height: (divHeight - tile2Height) - 197 - (ie6 ? 1 : 0)
    });
    //-------------------------------------------------------------------------
    $("#maxTBtm #divTile5").css({
        height: divHeight - 150
    }
       , animationSpeed);
    $("#maxTBtm #divTile5 .dashboardTable .rgDataDiv").css({
        height: divHeight - 184
    }
           );
    $("#maxTBtm").css({
        width: (divWidth - 100), bottom: "60px", right: "40px"
    }
       , animationSpeed);
    $("#maxTChart #divTile4").css({
        height: divHeight - 255
    }
       , animationSpeed);

    $("#maxTChart").css({
        width: (divWidth - 200), top: (divWidth / divHeight) + 100, right: (divWidth / divHeight) + 100
    }
       , animationSpeed);
    $("#maxSRMap #divTile3").css({
        height: divHeight - 255
    }
       , animationSpeed);

    $("#maxSRMap").css({
        width: (divWidth - 350), top: (divWidth / divHeight) + 100, left: (divWidth / divHeight) + 150
    }
       , animationSpeed);
    $("#maxChart #divTile3").css({
        height: divHeight - 150
    }
       , animationSpeed);

    $("#maxChart").css({
        width: (divWidth - 100), top: (divWidth / divHeight) + 40, left: (divWidth / divHeight) + 40
    }
       , animationSpeed);

    $("#maxSRTile5 #divTile5").css({
        height: divHeight - 150
    }, animationSpeed);

    $("#maxSRTile5").css({
        width: divWidth - 100, bottom: "40px", right: "40px"
    }
       , animationSpeed);

    $("#tile3").removeClass("leftTile");
    $(".todaysAccounts1").css({
        padding: "0px",
        position: "relative"
    });



    if (ie6)
    {
        $("#ctl00_main_subheader1_channelMenu").css("visibility", "hidden").css("visibility", "");
    }

    if (chrome)
    {
        var j = $("#divTile4 div[id$=_Frozen]");
        if (j.length)
        {
            var p = j.parent();
            p.find(".rgDataDiv").css("overflow-x", "hidden");
            j.find("div").width(p.find(".rgMasterTable").width());
        }
    }


    if (ie6)
    {
        //Fix for channel selection menu
        $(".channelSelectorContainer .rmVertical .rmLink, .channelSelectorContainer .rmVertical .rmItem").css('cssText', 'width: 195px !important');

        //Formulary Status scroll issue fix
        if ($('#ctl00_Tile4_gridFDSummary_gridNational_gridformularystatus').width() > $('#divTile4').width())
            $('#ctl00_Tile4_gridFDSummary_gridNational_gridformularystatus').css('margin-right', '17px');

        if ($('#ctl00_Tile4_gridFDSummary_gridRegional_gridformularystatus').width() > $('#divTile4').width())
            $('#ctl00_Tile4_gridFDSummary_gridRegional_gridformularystatus').css('margin-right', '17px');

        //Tier Coverage scroll issue fix
        if ($('#ctl00_Tile4_tiercoveragedata1_dataUS_gridtiercoverage').width() > $('#divTile4').width())
            $('#ctl00_Tile4_tiercoveragedata1_dataUS_gridtiercoverage').css('margin-right', '17px');

        if ($('#ctl00_Tile4_tiercoveragedata1_gridStateTerr_gridtiercoverage').width() > $('#divTile4').width())
            $('#ctl00_Tile4_tiercoveragedata1_gridStateTerr_gridtiercoverage').css('margin-right', '17px');
    }

    //clears Telerik computed width in the headers for the data table
    //setTimeout("resetGridHeaders()", 1500);
    resetGridHeadersX(500);
}

//END STANDARD REPORTS

function getWorkspaceHeight()
{
    return Sys.UI.DomElement.getBounds($(".footer")[0]).y - $(".header").height() - $(".navbar").height() - 20;
    //this old version is nicer but IE6 does not cooperate 
    //return $(document.body).height() - ($(".footer").height() + $(".header").height() + $(".navbar").height()) - 20; //20 i think is for margins
}


////CUSTOM OPTIONS
//function customapplication_content_resize()
//{
//    var browserWindow = $(window);
//    var divHeight = browserWindow.height();
//    var divWidth = browserWindow.width();
//    $("#tile2").hide();
//    $(".tileContainerHeader").hide();
//    $("#tile3").removeClass("leftTile");

//    //SPH 1/4/2010 - Fixes margin being incorrect when coming from SR
//    $("#section2").css({
//        marginLeft: "0px"
//    }
//    );
//    //SPH 1/4/2010 - Fixes margin being incorrect when coming from SR 

//    customapplication_section_resize();

//}
//function customapplication_section_resize()
//{
//    var browserWindow = $(window);
//    var divHeight = browserWindow.height();
//    var divWidth = browserWindow.width();

//    $("#tile3 #divTile3, .todaysAccounts2").css({
//        height: divHeight - 108, textAlign: "center", width: "auto", overflow: "auto"
//    }
//        );

//}
////END CUSTOM OPTIONS


//FORMULARY SELL SHEETS
function formularysellsheets_content_resize()
{
    formularysellsheets_section_resize();

    //setTimeout("resetGridHeaders()", 500);
    resetGridHeadersX(500);
}
function formularysellsheets_section_resize()
{
    var height = getWorkspaceHeight();

    $("#customInfoArea, #divTile3, #divTile3Container").height(height);

    $("#divTile3").height(height - $("#divTile3Container .tileContainerHeader").height() - ($("body").hasClass("mysellsheets") ? -5 : 45));
}
//END FORMULARY SELL SHEETS


//ACCESS BASED SELLING
function accessbasedselling_content_resize()
{
    var browserWindow = $(window);
    var divHeight = browserWindow.height();
    var divWidth = browserWindow.width();
    var divTile1Height = (divWidth / topLf) / 1.666;
    var tile1Height = divTile1Height + 27;

    //    $(".tileContainerHeader").show(); //remove this after custome options screen shot demo is gone

    grid_resize(divTile1Height);

    $("#expandTile1, #tileMin1").animate({
        height: tile1Height
    }
   , animationSpeed);
    $("#tile1").animate({
        width: divWidth / topLf, height: tile1Height
    }
   , animationSpeed);
    $("#tile1 #divTile1").animate({
        height: divTile1Height
    }
   , animationSpeed);
    $("#tile2").animate({
        height: tile1Height, marginLeft: divWidth / topLf
    }
   , animationSpeed);
    accessbasedselling_section_resize();
}
function accessbasedselling_section_resize()
{
    var windowHeight = $(window);
    var divHeight = windowHeight.height();
    var divWidth = windowHeight.width();
    var divTile1Height = (divWidth / topLf) / 1.666;
    var tile1Height = divTile1Height + 27;
    $("#tile2 .enlarge").show();
    $("#tile2 .min").hide();
    // sets heights for these containers in lower section
    $("#divTile3").css({
        height: (divHeight - tile1Height) - 138
    }
   );
    $("#divTile4").css({
        height: (divHeight - tile1Height) - 139
    }
   );
    $("#tile3").css({
        width: "60%"
    }
   );
    $("#tile4").css({
        width: "40%"
    }
   );
    $("#absmodules").css({
        "padding-top": "4px", marginLeft: "0px"
    }
   );
    $("#divTile3Container .title").css({ "padding-bottom": "0px", "padding-top": "0px" });
    $(".dashboardTable div.RadComboBox_pathfinder .rcbInputCell .rcbInput").css({
        "background": "#2d58a7 url(app_themes/pathfinder/images/arwDwnGray.png) 110px no-repeat", "color": "#ffffff", width: "100%"
    }
   );
    $(".dashboardTable .rgDataDiv").css({
        height: (divHeight - tile1Height) - 138
    }
   );

    //setTimeout("resetGridHeaders()", 1500);
    resetGridHeadersX(500);
}
// END ACCESS BASED SELLING

//POWER PLAN RX
function powerplanrx_content_resize()
{
    var browserWindow = $(window);
    var divHeight = browserWindow.height();
    var divWidth = browserWindow.width();
    $("#tile2").hide();

    //    $(".tileContainerHeader").show(); //remove this after custome options screen shot demo is gone

    $("#section2").css({
        marginLeft: "0px", marginTop: "0px", width: "auto"
    }
    );
    //Right part of SR   
    $("#section2").css({
        position: "relative"
    }
    );
    powerplanrx_section_resize();
}
function powerplanrx_section_resize()
{
    var browserWindow = $(window);
    var divHeight = browserWindow.height();
    var divWidth = browserWindow.width();
    var tile3Height;
    if (!$get("tile4") && !$get("tile4SR"))
    {
        tile3Height = divHeight - 138;
    }
    else
    {
        tile3Height = divHeight * .40;
    }
    //Browser Settings
    if (ie6)
    {
        $("#tile3 #divTile3Container ").css({
            height: tile3Height, top: "-5px"
        }
        );
    }
    else
    {
        $("#tile3 #divTile3, #tile3SR #divTile3").css({
            height: tile3Height, textAlign: "center", width: "auto", overflow: "hidden"
        }
        );
    }
    $("#tile3").removeClass("leftTile");
}
//END POWER PLAN RX


//TEXT RESIZE
//function textResize() ///{ }
//{
//    var windowHeight = $(window);
//    var divHeight = windowHeight.height();
//    var divWidth = windowHeight.width();
//    //resizes text of the app when browser size is below the width of 1024
//    if (divWidth <= 1024)
//    {
//        $("body").css("font-size", "10px");
//        $("#htmlModalWindow body").css("font-size", "11px");
//        $("input").css("font-size", "10px");
//        $("div.mainMenu .rmText, div.channel .rmText").css("font-size", "11px");
//        $(".navbar a.button").css("font-size", "11px");
//        $(".navbar .textBox").css("font-size", "11px");
//        $(".navbar .favLabel").css("font-size", "11px");
//    }
//    else
//    {
//        $("body").css("font-size", "11px");
//        $("#htmlModalWindow body").css("font-size", "11px");
//        $("input").css("font-size", "11px");
//        $("div.mainMenu .rmText, div.channel .rmText").css("font-size", "12px");
//        $(".navbar a.button").css("font-size", "12px");
//        $(".navbar .textBox").css("font-size", "12px");
//        $(".navbar .favLabel").css("font-size", "12px");
//    }
//}
//END TEXT RESIZE
//CHECK SCROLLING
function padScrollbar(e)
{

    var j = $(e);
    //SPH 1/7/2010 - Chrome fix - SPH 4/27/2010 - fixes some ie7 issues too
    if ($.browser.safari || ie7)
        j.parent().parent().find(".rgHeaderDiv, .rgFooterDiv").css("padding-right", "0px");
    //SPH 1/7/2010 - Chrome fix

    if (!ie6 && (j.height() > j.parent().height() || j.parent().css("overflow-y") == "scroll"))
        j.parent().parent().find(".rgHeaderDiv, .rgFooterDiv").css("width", "auto").css("margin-right", "17px");
    else if (ie6 && (j.height() > j.parent().height() || j.parent().css("overflow-y") == "scroll"))
        j.parent().parent().find(".rgHeaderDiv, .rgFooterDiv").css({
            paddingRight: "0px",
            marginRight: "16px"
        }
           );
    else
        j.parent().parent().find(".rgHeaderDiv, .rgFooterDiv").css({
            paddingRight: "0px",
            marginRight: "0px"
        }
       );

    //Fix for header alignment issues in IE6 & 7
    if (((ie7) || (ie6)) && ($(".customercontactreports").length > 0))
    {
        j.parent().parent().find(".rgHeaderDiv, .rgFooterDiv").css("width", j.width() + "px");

        $(".ccrBusinessPlans .rgHeaderDiv").css("margin-right", "");
    }
}
//RESIZES DATA GRID WHEN CLICK ON THE SEARCH LINK IN DATA TABLE
function grid_resize(divTile1Height)
{
    if (!divTile1Height)
    {
        if (!$get("maxPlanInfo"))
            divTile1Height = getDivTile1HeightForTA($(window).width(), topLf);
        else
            divTile1Height = $(window).height() - (ie6 ? 127 : 160);
    }
    var toggleHeight = 0;
    $("#divTile2Plans .dashboardTable .rgHeaderDiv").each(function()
    {
        toggleHeight = $(this).height();
    }
       );
    $("#divTile2Plans .dashboardTable .rgDataDiv").css({
        height: divTile1Height - toggleHeight
    }
       );

    if ($get("maxPlanInfo"))
    {
        //SPH 02/10/2010 - Made height adjustments to use correct window height so it would not distort based on window size (divide by 1.15 to match how modal is sized)
        var divHeight = $(window).height() / 1.15;
        if (ie6)
        {
            $("#maxPlanInfo #divTile2Plans .dashboardTable .rgDataDiv").css({ height: divHeight - 29 - toggleHeight - $('#planInfoLegend').height() });
            $("#maxPlanInfo #divTile2Plans .dashboardTable").css({ height: divHeight - 59 - $('#planInfoLegend').height() }, animationSpeed);
        }
        else
        {
            $("#maxPlanInfo #divTile2Plans .dashboardTable .rgDataDiv").css({ height: divHeight - 26 - toggleHeight - $('#planInfoLegend').height() });
            $("#maxPlanInfo #divTile2Plans .dashboardTable").css({ height: divHeight - 26 - $('#planInfoLegend').height() }, animationSpeed);
        }
    }
    else
    {
        setTimeout(function()
        {
            $("#divTile2Plans .dashboardTable").css({
                height: divTile1Height - $('#planInfoLegend').height()
            });
        }, 500);

        $("#ctl00_main_planInfo_gridPlanInfo .rgDataDiv").css({
            height: divTile1Height - $('#planInfoLegend').height() - $('#ctl00_main_planInfo_gridPlanInfo .rgHeaderDiv').height()
        });
    }
}
function toggleArrow()
{
    $(".advToggleArw").toggle();
    if (window != window.top) window.top.toggleArrow();
}
function showMap()
{
    var windowHeight = $(window);
    var divHeight = windowHeight.height();
    var divWidth = windowHeight.width();
    var divTile1Height = getDivTile1HeightForTA(divWidth, topLf);
    var tile1Height = divTile1Height + 27;
    $("#divTile1").css({
        position: "relative", top: "0px"
    }
   );
    $("#tile1 .title").show(1000);
    $("#tile1 #divTile1").css({
        height: divTile1Height
    });
}
function minTile1()
{
    if ($("#tileMin1").length > 0)
        return;
    var windowHeight = $(window);
    var divHeight = windowHeight.height();
    var divWidth = windowHeight.width();
    var divTile1Height = getDivTile1HeightForTA(divHeight);
    var tile1Height = divTile1Height + 27;
    $("#tile1 .title").hide();
    $("#divTile1").css({
        position: "absolute", top: "-1000px"
    }
   );
    $("#tile1").animate({
        width: "25px"
    }
   , animationSpeed);

    //SPH 02/11/2010 - Fix chrome issue where plan grid does not fill screen when map minimized 
    var props = { marginLeft: "30px" };
    if (chrome)
        props["width"] = divWidth - 50;
    $("#tile2").animate(props, animationSpeed, function() { resizeSearchTextBox(); });
    //


    $("#expandTile1").animate({
        width: "25px", height: tile1Height
    }
   , animationSpeed);
    $(".min").hide();
    $("#tile1 .enlarge").hide();
    $("#tile1").attr({
        id: "tileMin1"
    }
   );
    $("#tile2").attr({
        id: "tileMin2"
    }
   );

    //    if($.browser.safari)
    //        grid_resize(divTile1Height);
}

function ensureMapIsLoaded(uiState, containerID)
{
    if (!flashSupported) return;

    //if (!fmEngineID) fmEngineID = "fmASEngine";
    var fmEngineID = "fmASEngine";
    if (!containerID) containerID = "divTile1";

    var engine = document[fmEngineID];

    var created = false;

    //--flashmaps
    if (!engine || $.browser.mozilla)
    {
        if (DetectFlashVer(7, 0, 0))
        {
            var fm = fmObjectGetParams(fmEngineID, "controls/map/fmASMap/mapContainer.swf", "100%", "100%", "true", "high", "#FFFFFF", ".", "showall");
            $get(containerID).innerHTML = fmObjectGenerate(fm.attrs, fm.params);

            fmEngine = document[fmEngineID];
            fmASMcPath = "map_mc.";
            fmASEngine = fmEngine;

            //Initialize map with current ui state - loads map data based on current Channel & Drug
            //Using cmd object to delay initialization .5 seconds to make sure map object is ready.  initMap function will keep trying up to 5 times until successful
            new cmd(null, initMap, [fmEngineID, uiState], 500);
            //
        }
        else
        {
            $("#" + containerID).html("This content requires the Adobe Flash Player. <a target='_blank' href='https://get.adobe.com/flashplayer/'>Get Flash</a>");
        }

        created = true;
    }

    //--flashmaps

    return created;
}


function chromeTile2Fix()
{
    if (chrome)
    {
        $("#tile2").css("marginLeft", "").css("width", "");
        $("#tileMin2").css("marginLeft", "30px").css("width", $(window).width() - 50);
    }
}

function maxTile1()
{
    if ($("#tile1").length > 0 || $("#expandTile1 a").attr("disabled") == "true") //SPH 1/11/2010 - check disabled attribute for FF and Chrome
        return;

    var windowHeight = $(window);
    var divHeight = windowHeight.height();
    var divWidth = windowHeight.width();
    var divTile1Height = getDivTile1HeightForTA(divHeight);
    var tile1Height = divTile1Height + 27;
    if (divHeight > 700)
    {
        var divideBy = 3;
    } else
    {
        var divideBy = 3.5;
    }
    $("#tileMin1").attr({
        id: "tile1"
    }
   );
    $("#tileMin2").attr({
        id: "tile2"
    }
   );
    if ($.browser.safari)
    {
        $("#tile2").removeAttr("style");
        $("#tile2").height(tile1Height);
        $("#tile2").width("auto");
        $("#tile2").animate({
            marginLeft: ((divHeight / divideBy) * 1.666) + 6, width: divWidth - (divWidth / topLf) - 26
        }
      , animationSpeed, chromeTile2Fix);
    }
    else
    {
        $("#tile2").width("auto");
        $("#tile2").css("z-index", "").css("position", "").css("right", "").css("top", "").animate({
            marginLeft: ((divHeight / divideBy) * 1.666) + 5
        }
      , animationSpeed);
    }
    $("#tile1").animate({
        width: (divHeight / divideBy) * 1.666
    }
   , animationSpeed, showMap);
    $("#expandTile1").animate({
        width: "0px", height: tile1Height, height: (divHeight - (tile1Height)) - 143
    }
   , animationSpeed, null, maxTile1AnimationCallback);
    $("#tile1 .min").show();
    $("#tile1 .enlarge").show();

    //ensureMapIsLoaded(clientManager.get_CurrentUIStateAsText(clientManager.mapDataRequestUIStateProperties));
    fixMapTip();

}
function maxTile1AnimationCallback()
{
    ensureMapIsLoaded(clientManager.get_CurrentMapUIStateAsText());
    resizeSearchTextBox();
}


function minTile2(callback)
{
    var windowHeight = $(window);
    var divHeight = windowHeight.height();
    var divWidth = windowHeight.width();
    var divTile1Height = getDivTile1HeightForTA(divWidth, topLf);
    var tile1Height = divTile1Height + 27;
    var tile2Height = divHeight - 112;
    var section2width = divWidth - 228;
    $(".min").hide();
    $(".enlarge").hide();
    $("#tile2 .tileContainerHeader .title").hide();
    $("#tile2 .tileContainerHeader .tools").hide();
    $("#tile3").attr({
        id: "tile3SR"
    }
   );
    $("#tile4").attr({
        id: "tile4SR"
    }
   );
    $("#tile5").attr({
        id: "tile5SR"
    }
   );
    $("#tile2").animate({
        width: "25px"
    }
   , animationSpeed);
    $(".todaysAccounts2").animate({
        marginLeft: "34px"
    }
   , animationSpeed, callback);

    $("#expandTile2SR").animate({
        width: "25px", height: "100%"
    }
   , animationSpeed);
    $("#section2").css({
        width: "auto"
    }
   );
    $("#tile2 #divTile2 .moduleSelector").hide();
    $("#tile2").attr({
        id: "tileMin2SR"
    }
   );
    $("#section2").addClass("todaysAccounts2Expand");
}
function maxTile2(callback)
{
    var windowHeight = $(window);
    var divHeight = windowHeight.height();
    var divWidth = windowHeight.width();
    var divTile1Height = getDivTile1HeightForTA(divWidth, topLf);
    var tile1Height = divTile1Height + 27;
    $("#tileMin2SR").attr({
        id: "tile2"
    }
   );
    $("#tile3SR").attr({
        id: "tile3"
    }
   );
    $("#tile4SR").attr({
        id: "tile4"
    }
   );
    $("#tile5SR").attr({
        id: "tile5"
    }
   );
    $("#tile2 #divTile2").show();
    $(".todaysAccounts2").animate({
        marginLeft: "208px"
    }
   , animationSpeed, callback);
    $("#tile2").animate({
        width: "200px"
    }
   , animationSpeed);
    $("#expandTile2SR").animate({
        width: "0px"
    }
   , animationSpeed, maxTile2post);
    $("#tile2 #divTile2Container ").show();
    $(".min").show();
    $(".enlarge").show();
    $("#tile2 .enlarge").hide();

    $("#section2").removeClass("todaysAccounts2Expand");
}
function maxTile2post()
{
    $("#tile2 .tileContainerHeader .title").show();
    $("#tile2 .tileContainerHeader .tools").show();
    $("#tile2 #divTile2 .moduleSelector").show();
}
function maxTile4()
{
    var windowHeight = $(window);
    var divHeight = windowHeight.height();
    var divWidth = windowHeight.width();
    var divTile1Height = getDivTile1HeightForTA(divWidth, topLf);
    var tile1Height = divTile1Height + 27;
    $("#tile3Max .enlarge").hide();
    if (ie6)
    {
        $("#tile3Max").animate({
            width: divWidth / 2 - 8
        }
   , animationSpeed);
        $("#tile4Min").animate({
            width: divWidth / 2 - 8
        }
   , animationSpeed);
    } else
    {
        $("#tile3Max").animate({
            width: divWidth / 2 - 12
        }
   , animationSpeed);
        $("#tile4Min").animate({
            width: divWidth / 2 - 12
        }
   , animationSpeed);
    }
    $("#expandTile4").animate({
        width: "0px", height: (divHeight - tile1Height) - 142
    }
   , animationSpeed, maxTile4post);
    $("#tile4Min #divTile4Container").show();
    $("#tile3Max").attr({
        id: "tile3Min"
    }
   );
    $("#tile4Min").attr({
        id: "tile4Max"
    }
   );
}
function maxTile4post()
{
    $("#tile4Max .tileContainerHeader .title").show();
    $("#tile4Max .tileContainerHeader .tools").show();
    $("#tile4Max #divTile4").show();
}
function minTile4()
{
    var windowHeight = $(window);
    var divHeight = windowHeight.height();
    var divWidth = windowHeight.width();
    var divTile1Height = getDivTile1HeightForTA(divWidth, topLf);
    var tile1Height = divTile1Height + 27;
    if (ie6)
    {
        $("#tile3Min").animate({
            width: divWidth - 48
        }
   , animationSpeed);
    } else
    {
        $("#tile3Min").animate({
            width: divWidth - 52
        }
   , animationSpeed);
    }
    $("#tile4Max").animate({
        width: "28px"
    }
   , animationSpeed);
    $("#expandTile4").animate({
        width: "28px", height: (divHeight - tile1Height) - 142
    }
   , animationSpeed);
    $("#tile4Max .tileContainerHeader .title").hide();
    $("#tile4Max .tileContainerHeader .tools").hide();
    $("#tile4Max #divTile4Container").hide();
    $("#tile3Min").attr({
        id: "tile3Max"
    }
   );
    $("#tile4Max").attr({
        id: "tile4Min"
    }
   );
    $("#tile3Max .enlarge").show();
}
function maxMap()
{
    var windowHeight = $(window);
    var divHeight = windowHeight.height();
    var divWidth = windowHeight.width();
    var divTile1Height = getDivTile1HeightForTA(divHeight);
    var tile1Height = divTile1Height + 27;
    if (divHeight > 700)
    {
        var divideBy = 3;
    } else
    {
        var divideBy = 3.5;
    }
    $(".enlarge").hide();
    $(".min").hide();
    $("#tile1").attr({
        id: "maxMap"
    }
   );
    $("#maxMap").css({
        position: "absolute", zIndex: "10000", top: "75px", left: "10px"
    }
   );
    $("#maxMap").animate({
        width: ((divHeight / divideBy) * 1.666) * mapEnlarge, height: (tile1Height * mapEnlarge) - 15, top: divHeight / 6, left: divWidth / 5
    }
   , animationSpeed, showModal);
    $("#divTile1").animate({
        height: (tile1Height - 27) * mapEnlarge, width: ((divHeight / divideBy) * 1.666) * mapEnlarge
    }
   , animationSpeed, fixMapTip);
    $("#maxMap .close").show();
    $("#fauxModal").css({
        zIndex: "9000", visibility: "visible"
    }
   );
}
function minMap()
{
    var windowHeight = $(window);
    var divWidth = windowHeight.width();
    var divHeight = windowHeight.height();
    var divTile1Height = getDivTile1HeightForTA(divHeight);
    var tile1Height = divTile1Height + 27;
    if (divHeight > 700)
    {
        var divideBy = 3;
    } else
    {
        var divideBy = 3.5;
    }
    $("#maxMap .close").hide();
    $("#maxMap").animate({
        width: (divHeight / divideBy) * 1.666, height: tile1Height, top: "73px", left: "5px"
    }
   , animationSpeed, hideModal);
    $("#divTile1").animate({
        height: divTile1Height, width: "100%"
    }
   , animationSpeed, fixMapTip);
    $("#maxMap").attr({
        id: "tile1"
    }
   );
    $(".enlarge").show();
    $("#tile1 .min").show();
}
function maxPlanInfo()
{
    var windowHeight = $(window);
    var divHeight = windowHeight.height();
    var divWidth = windowHeight.width();
    var divTile1Height = getDivTile1HeightForTA(divHeight);
    var tile1Height = divTile1Height + 27;
    var tile1Visible = $get("tile1") != null;

    $("#tile2, #tileMin2").attr({
        id: "maxPlanInfo"
    }
   );
    $("#maxPlanInfo .enlarge").hide();
    $("#maxPlanInfo .min").hide();
    $("#maxPlanInfo .textResize").show();
    $("#maxPlanInfo").css({
        position: "absolute", zIndex: "10000", top: "73px", right: "10px", width: (tile1Visible ? ("auto") : divWidth - 52)
    }
   );
    $("#maxPlanInfo").animate({
        width: (divWidth / topLf) * modalEnlarge, height: (divHeight / 1.15), top: divHeight / 15, right: divWidth / 37, marginLeft: "0px"
    }
   , animationSpeed, showModal);

    grid_resize(divTile1Height);
    //      if (ie6) {
    //          $("#maxPlanInfo #divTile2Plans .dashboardTable .rgDataDiv").css({
    //              height: divHeight - 112
    //          }
    //       );
    //          $("#maxPlanInfo #divTile2Plans .dashboardTable").animate({
    //              height: divHeight - 123
    //          }
    //   , animationSpeed);

    //      } else {
    //          $("#maxPlanInfo #divTile2Plans .dashboardTable .rgDataDiv").css({
    //              height: divHeight - 119
    //          }
    //       );
    //          $("#maxPlanInfo #divTile2Plans .dashboardTable").animate({
    //              height: divHeight - 100
    //          }
    //   , animationSpeed);

    //      }
    $("#maxPlanInfo .close").show();
    $("#fauxModal").css({
        zIndex: "9000", visibility: "visible"
    }
   );
}
function isTile1Visible()
{
    return $get("tile1") != null;
}
function minPlanInfo()
{
    var windowHeight = $(window);
    var divHeight = windowHeight.height();
    var divWidth = windowHeight.width();
    var divTile1Height = getDivTile1HeightForTA(divHeight);
    var tile1Height = divTile1Height + 27;
    var tile1Visible = isTile1Visible();
    var divideBy = (divHeight > 700) ? 3 : 3.5;
    var tileLeft = ((divHeight / divideBy) * 1.666) + 6;
    var tileWidth = tile1Visible ? divWidth - tileLeft : divWidth - 52;



    $(".textResize").hide();
    $("#maxPlanInfo .min").hide();
    $("#maxPlanInfo .close").hide();
    $("#maxPlanInfo").animate({ width: tileWidth, height: tile1Height, top: "73px", right: "10px" }, animationSpeed, function() { hideModal(); $("#maxPlanInfo, #tile2, #tileMin2").css("z-index", "").css("left", "").css("width", "").css("position", ""); chromeTile2Fix(); });
    $("#divTile2Plans .dashboardTable").animate({
        height: divTile1Height
    }
   , animationSpeed);
    //SPH 02/10/2010 - Replaced with call to grid_resize below (had to be called after maxPlanInfo renamed back to tile2 or tileMin2
    //   $("#divTile2Plans .dashboardTable .rgDataDiv").css( {
    //      height : divTile1Height - 20 }
    //   );   

    textSmall();

    $("#maxPlanInfo .enlarge").show();
    $("#maxPlanInfo").attr({
        id: (tile1Visible ? "tile2" : "tileMin2")
    }
   );

    //Fix for Safari Tile2 Minimize width issue
    if (safari)
    {
        setTimeout(function()
        {
            todaysaccounts_content_resize();
        }, 500)
    }

    //SPH 02/10/2010 - replaces code to size grid - uses shared resize func
    grid_resize(divTile1Height);

    //For millennium->Custom segments do not add left margin since ExpandMap div is not shown there.
    if ((clientManager.get_Channel() == 105) || (clientManager.get_Channel() == 106) || (clientManager.get_Channel() == 107) || (clientManager.get_Channel() == 108))
        $("#tileMin2").css({ marginLeft: "0px" });
    else
        $("#tileMin2").css({ marginLeft: "30px" }); //.css("position", "").css("width","");
}

function maxCL(noAnimate)
{
    var windowHeight = $(window);
    var divHeight = windowHeight.height();
    var divWidth = windowHeight.width();
    var divTile1Height = getDivTile1HeightForTA(divWidth, topLf);
    var tile1Height = divTile1Height + 27;
    var modalHeight = (divHeight / 1.15);

    $("#section2 .enlarge").hide();
    $("#section2").addClass("todaysAccounts2Max");
    $(".todaysAccounts2Max").css({
        width: divWidth - 20, position: "absolute", zIndex: "10000", top: tile1Height + 115, left: "10px"
    }
    );
    if (!noAnimate)
    {
        $(".todaysAccounts2Max").animate({
            width: (divWidth / topLf) * modalEnlarge, height: modalHeight, top: divHeight / 15, left: divWidth / 40
        }
    , animationSpeed, function() { fixIEScroll(); showModal(); });
    }
    else
    {
        $(".todaysAccounts2Max").css({
            width: (divWidth / topLf) * modalEnlarge, height: modalHeight, top: divHeight / 15, left: divWidth / 40
        }
    , animationSpeed, function() { fixIEScroll(); showModal(); });
    }
    $("#divTile3").attr({
        id: "divTile3Max"
    }
    );
    $("#divTile4").attr({
        id: "divTile4Max"
    }
    );
    $("#divTile5").attr({
        id: "divTile5Max"
    }
    );
    //SPH 2/10/2010 - using modalHeight and changed constants to work properly with various sized modal windows (previously used window height which is not accurate)
    $("#divTile3Max, #divTile4Max, #lfView").css({
        height: modalHeight - 28
    }
    );
    //SPH 2/11/2010 - IE6 not cooperating with height of 3rd box
    $("#divTile5Max").css({ height: modalHeight - (!ie6 ? 28 : 30) });


    //SPH 2/9/2010 - Grid was not resizing on max
    //SPH 2/10/2010 - using modalHeight and changed constants to work properly with various sized modal windows (previously used window height which is not accurate)
    var div5Ftr = $("#CoveredLivesDrilldownFooter").height();
    $("#divTile5Max .rgDataDiv").height(modalHeight - ((ie6 || ie7 ? 113 : 98) + div5Ftr));
    resetGridHeaders();
    //

    //SPH 2/2/2010 - this height is causing Tile 4 in CL to not expand to bottom on max
    if (!ie6)
        $("#divTile4Container").height("");

    $(".todaysAccounts2Max .close").show();
    $("#fauxModal").css({
        zIndex: "9000", visibility: "visible"
    }
    );
}
function minCL()
{
    var windowHeight = $(window);
    var divHeight = windowHeight.height();
    var divWidth = windowHeight.width();
    var divTile1Height = getDivTile1HeightForTA(divWidth, topLf);
    var tile1Height = divTile1Height + 27;
    $(".todaysAccounts2Max .close").hide();
    $(".todaysAccounts2Max").removeClass("todaysAccounts2Max").animate({
        width: divWidth - 20, height: (divHeight - (tile1Height)) - 169, top: tile1Height + 115, left: "10px"
    }
    , animationSpeed, function() { fixIEScroll(); hideModal(); });

    var div5Ftr = $("#CoveredLivesDrilldownFooter").height();
    $("#divTile5Max .rgDataDiv").height((divHeight - tile1Height) - ((ie6 || ie7 ? 251 : 240) + div5Ftr));
    resetGridHeaders();

    $("#divTile3Max").attr({
        id: "divTile3"
    }
    );
    $("#divTile4Max").attr({
        id: "divTile4"
    }
    );
    $("#divTile5Max").attr({
        id: "divTile5"
    }
    );
    $("#divTile3, #divTile4, #divTile5, #lfView").animate({
        height: (divHeight - (tile1Height)) - 169
    }
    , animationSpeed, function() { $(".todaysAccounts2 .enlarge").show(); });


}
function maxAff()
{
    var windowHeight = $(window);
    var divHeight = windowHeight.height();
    var divWidth = windowHeight.width();
    var divTile1Height = getDivTile1HeightForTA(divWidth, topLf);
    var tile1Height = divTile1Height + 27;

    $("#section2 .textResize").show();

    $("#tile3").attr({
        id: "maxAff"
    }
   );

    $("#maxAff").css({
        width: divWidth - 20, position: "absolute", zIndex: "10000", top: tile1Height + 115, left: "10px"
    }
   );

    if (ie6)
    {
        $("#maxAff .dashboardTable .rgDataDiv ").css({
            overflow: "auto"
        });
    }
    $("#maxAff").animate({
        // top and left in the next line causes crash in IE7 sp3, remove and works fine
        width: (divWidth / topLf) * modalEnlarge, height: (divHeight / 1.15), top: divHeight / 15, left: divWidth / 40
    }
   , animationSpeed, showModal);
    $("#maxAff #divTile3").css({
        height: divHeight - 143
    }
   );
    $("#maxAff .dashboardTable .rgDataDiv").css({
        height: divHeight - 162
    }
   );
    $("#maxAff .close").show();
    $("#maxAff .enlarge").hide();
    $("#fauxModal").css({
        zIndex: "9000", visibility: "visible"
    }
   );
}
function minAff()
{
    var windowHeight = $(window);
    var divHeight = windowHeight.height();
    var divWidth = windowHeight.width();
    var divTile1Height = getDivTile1HeightForTA(divWidth, topLf);
    var tile1Height = divTile1Height + 27;
    $(".textResize").hide();
    $("#maxAff .close").hide();
    $("#maxAff").animate({
        width: divWidth - 20, height: (divHeight - (tile1Height)) - 169, top: tile1Height + 115, left: "10px"
    }
   , animationSpeed, hideModal);
    $("#maxAff .dashboardTable").animate({
        height: divTile1Height
    }
   , animationSpeed, function() { $(this).css("height", ""); });
    $("#maxAff .dashboardTable .rgDataDiv").css({
        height: (divHeight - (tile1Height)) - 189
    }
   );
    $("#maxAff #divTile3").css({
        height: (divHeight - (tile1Height)) - 169
    }
   );
    if (ie6)
    {
        $("#maxAff .dashboardTable .rgDataDiv ").css({
            overflow: "visible",
            width: "100%"
        });
    }
    textSmall();
    $("#maxAff").attr({
        id: "tile3"
    }
   );
    $("#tile3 .enlarge").show();
}
function maxKC()
{
    var windowHeight = $(window);
    var divHeight = windowHeight.height();
    var divWidth = windowHeight.width();
    var divTile1Height = getDivTile1HeightForTA(divWidth, topLf);
    var tile1Height = divTile1Height + 27;
    $("#tile3Max .enlarge").hide();
    $(".textResize").show();
    $("#tile3Max").attr({
        id: "maxKC"
    }
   );

    $("#maxKC").css({
        width: divWidth - 52, position: "absolute", zIndex: "10000", top: tile1Height + 115, left: "10px"
    }
   );
    $("#maxKC").animate({
        width: (divWidth / topLf) * modalEnlarge, height: (divHeight / 1.15), top: divHeight / 15, left: divWidth / 40
    }
   , animationSpeed, showModal);
    $("#maxKC #kcView").css({
        height: divHeight - 160
    }
   );
    $("#maxKC .close").show();
    $("#fauxModal").css({
        zIndex: "9000", visibility: "visible"
    }
   );
}
function minKC()
{
    var windowHeight = $(window);
    var divHeight = windowHeight.height();
    var divWidth = windowHeight.width();
    var divTile1Height = getDivTile1HeightForTA(divWidth, topLf);
    var tile1Height = divTile1Height + 27;
    $(".textResize").hide();
    $("#maxKC .close").hide();
    $("#maxKC").animate({
        width: divWidth - 52, height: (divHeight - (tile1Height)) - 169, top: tile1Height + 111, left: "10px"
    }
   , animationSpeed, hideModal);
    textSmall();
    $("#maxKC").attr({
        id: "tile3Max"
    }
   );
    $("#tile3Max #kcView").css({
        height: (divHeight - (tile1Height)) - 190
    }
   );
    $("#tile3Max .enlarge").show();
}
function showModal()
{
    $("#fauxModal").fadeTo("slow", 0.80);
}
function hideModal()
{
    var windowHeight = $(window);
    var divWidth = windowHeight.width();
    var divHeight = windowHeight.height();
    if (divHeight > 700)
    {
        var divideBy = 3;
    } else
    {
        var divideBy = 3.5;
    }
    $("#fauxModal").fadeTo("slow", 0.00);
    $("#tile1").css({
        position: "relative", top: "0px", left: "0px", zIndex: "0"
    }
   );
    if ($.browser.safari)
    {
        $("#tile2").css({
            position: "relative", top: "0px", marginLeft: (divHeight / divideBy) * 1.666 + 5, right: "0px", zIndex: "0", width: divWidth - (divWidth / topLf) - 26
        }
      );
        chromeTile2Fix();
    }
    else
    {
        $("#tile2").css({
            position: "relative", top: "0px", marginLeft: (divHeight / divideBy) * 1.666 + 5, right: "0px", zIndex: "0", width: "auto"
        }
      );
    }
    $("#fauxModal").css({
        zIndex: "0", visibility: "hidden"
    }
   );
    $("#divTile3Container").removeAttr("style");
    $("#tile3").removeAttr("style");
    $("#tile4").removeAttr("style");
    $("#tile3Max").removeAttr("style");
    $("#tile3Max").width(divWidth - 52);
    $("#section2").removeAttr("style");

}

function hideModalSR()
{
    $("#fauxModal").fadeTo("slow", 0.00);
    $("#tile3").removeAttr("style");
    $("#tile4").removeAttr("style");
    $("#tile5").removeAttr("style");
    $("#tile6").removeAttr("style");
    $(".tile").removeAttr("style");
    $("#tile3T").removeAttr("style");
    $("#tile4T").removeAttr("style");
    $("#tile5T").removeAttr("style");
    $("#tile5T").css("marginTop", "5px");

    $("#fauxModal").css({
        zIndex: "0", visibility: "hidden"
    }
   );
    $(".section2SR .leftTile, .section2SR .rightTile").css({
        marginTop: 0
    }
   );
    $(".section2SR #tile3 .enlarge").show();
    $(".section2SR #tile4 .enlarge").show();
    $(".section2SR #tile5 .enlarge").show();
    $(".section2SR #tile6 .enlarge").show();
    $(".tile .enlarge").show();
    $("#tile3T .enlarge").show();
    $("#tile4T .enlarge").show();
    $("#tile5T .enlarge").show();

}

function hideModalMA()
{
    $("#tile4 .enlarge").show();
    $("#chartLeft1, #chartLeft2, #chartRight").removeAttr("style");
    todaysanalytics_content_resize();
}

function maxChart()
{
    var windowHeight = $(window);
    var divHeight = windowHeight.height();
    var divWidth = windowHeight.width();
    //Tile 3 Properties (if Tile4 & 5 exist statement)
    var tile3Height;
    if (!$get("tile4") && !$get("tile4SR"))
    {
        tile3Height = divHeight - 138;
    }
    else
    {
        tile3Height = divHeight * .40;
    }
    $(".enlarge").hide();
    $("#fauxModal").css({
        zIndex: "9000", visibility: "visible"
    }
   );

    var j = $(".chartContainer");
    if (j.length > 1)
    {
        j.width("100%").height("50%");
    }

    $(".section2SR #tile3").attr({
        id: "maxChart"
    }
   );
    $("#maxChart").css({
        position: "absolute", width: divWidth - 226, zIndex: "10000", top: "78px", left: "217px"
    }
   );
    $(".section2SR .leftTile, .section2SR .rightTile").css({
        marginTop: tile3Height + 28
    }
   );
    $("#maxChart #divTile3").animate({
        height: divHeight - 150
    }
   , animationSpeed);

    $("#maxChart").animate({
        width: (divWidth - 100), top: (divWidth / divHeight) + 40, left: (divWidth / divHeight) + 40
    }
   , animationSpeed, showModal);
    $(" #maxChart .close").show();

}
function minChart()
{
    var windowHeight = $(window);
    var divWidth = windowHeight.width();
    var divHeight = windowHeight.height();

    //Tile 3 Properties (if Tile4 & 5 exist statement)
    var tile3Height;
    if (!$get("tile4") && !$get("tile4SR"))
    {
        tile3Height = divHeight - 138;
    }
    else
    {
        tile3Height = divHeight * .40;
    }


    var j = $(".chartContainer");
    if (j.length > 1)
    {
        j.width("50%").height("100%");
    }

    $(".section2SR #maxChart .close").hide();
    $("#maxChart").animate({
        top: "78px", left: "217px", width: divWidth - 226
    }
   , animationSpeed, hideModalSR);
    $("#maxChart").attr({
        id: "tile3"
    }
   );

    $(".section2SR #divTile3").animate({
        height: tile3Height
    }
   , animationSpeed);
}

//function maxSRTile4()
//{
//    var windowHeight = $(window);
//    var divHeight = windowHeight.height();
//    var divWidth = windowHeight.width();
//    $(".enlarge").hide();
//    $("#fauxModal").css({
//        zIndex: "9000", visibility: "visible"
//    }
//   );

//    $(".section2SR #tile4").attr({
//        id: "maxSRTile4"
//    }
//   );
//    $("#maxSRTile4").css({
//        position: "absolute", zIndex: "10000", bottom: "32px", left: "217px"
//    }
//   );
//    $("#maxSRTile4 #divTile4").animate({
//        height: divHeight - 150
//    }
//   , animationSpeed);

//    $("#maxSRTile4").animate({
//        width: (divWidth - 100), bottom: "40px", left: "40px"
//    }
//   , animationSpeed, showModal);
//    $(".section2SR #maxSRTile4 .close").show();


//    $("#ctl00_Tile4_tiercoveragedata1_gridtiercoverage_Frozen").hide();
//    if ($("#ctl00_Tile4_tiercoveragedata1_gridtiercoverage_Frozen").length > 0)
//    {
//        //fix header issue in fixed scrolling grid - also if grid is scrolled when maximized you can't see all columns so fixing that too
//        $("#divTile4 .rgHeaderDiv colgroup col, #divTile4 .rgDataDiv colgroup col, #divTile4 .rgHeaderDiv th, #divTile4 .rgDataDiv td[_xcol!=true]").css("display", "block");
//        $("#divTile4 .rgHeaderDiv th:last").css("display", "none");
//        $("#divTile4 .rgDataDiv tr").each(function() { $(this).find("td:last").css("display", "none"); });
//    }

//    //Fix for header alignment issues CCR
//    if ($(".customercontactreports").length > 0)
//        resetGridHeadersX(500);
//}
//function minSRTile4()
//{
//    var windowHeight = $(window);
//    var divWidth = windowHeight.width();
//    var divHeight = windowHeight.height();


//    $(".section2SR #maxSRTile4 .close").hide();
//    $("#maxSRTile4").animate({
//        bottom: "32px", left: "217px", width: (divWidth - 226) / 2
//    }
//   , animationSpeed, hideModalSR);

//    //SPH 02/15/2010 - height of tile was incorrect - borrowing code from standardreports_section_resize which is correct
//    //Tile 3 Properties (if Tile4 & 5 exist statement)   
//    var tile3Height = divHeight * .40;
//    $("#maxSRTile4 #divTile4").animate({
//        height: safeSub((divHeight - tile3Height), 164) //(divHeight / 2) - 84
//    }
//   , animationSpeed, function() { $("#ctl00_Tile4_tiercoveragedata1_gridtiercoverage_Frozen").show().scrollLeft(0); });
//    $("#maxSRTile4").attr({
//        id: "tile4"
//    }
//   );

//    //Fix for header alignment issues CCR
//    if ($(".customercontactreports").length > 0)
//        resetGridHeadersX(500);
//}


function maxTile(tile, callback, animCallback)
{
    var resizeFunc = "animate";
    var maxed = $(".maximized");

    if (maxed.length > 0)
    {
        tile = maxed.attr("_tile");
        callback = maxed[0]["_cb"];
        maxed.addClass("invaliddim");
        resizeFunc = "css";
    }
    if (!tile) return;


    var windowHeight = $(window);
    var divHeight = windowHeight.height();
    var divWidth = windowHeight.width();

    $("#fauxModal").css({ zIndex: "9000", visibility: "visible" });

    var tileSrc = tile;
    var elementID = "tile" + tile;
    var selector = "#" + elementID;

    $(selector + " .enlarge").hide();

    tile = parseInt(tile, 10); //tile may have character but at this point use number only

    var containerHeight = $(selector + " #divTile" + tile).height();

    var rect = Sys.UI.DomElement.getBounds($get(elementID));

    var fromTop = rect.y < 80; //divHeight / 2 > rect.y;

    if (maxed.length == 0)
    {
        $(selector).addClass("maximized")
                    .attr("_tile", tileSrc)
                    .attr("_x", rect.x)
                    .attr("_y", rect.y)
                    .attr("_w", rect.width)
                    .attr("_h", rect.height)
                    .attr("_ch", containerHeight)
                    .attr("_ft", fromTop ? "yes" : "no")
                    .each(function() { this["_cb"] = callback; });
    }

    $(selector).css("width", rect.width); //force width so content doesn't snap 

    if (fromTop)
    {
        //position tile absolute
        $(selector).css({ position: "absolute", zIndex: "10000", top: rect.y, left: rect.x });
        //move tile to proper pos
        $(selector)[resizeFunc]({ width: (divWidth - 100), top: "75px", left: "40px" }, animationSpeed, showModal);
    }
    else
    {
        var w = (divWidth - 100);

        //position tile absolute        
        $(selector).css({ position: "absolute", zIndex: "10000", bottom: divHeight - (rect.y + rect.height), right: divWidth - (rect.x + rect.width) });

        //move tile to proper pos
        $(selector)[resizeFunc]({ width: w, bottom: "40px", right: "40px" }, animationSpeed, function() { showModal(); if (animCallback) animCallback(tile); });
    }

    //resize content
    $(selector + " #divTile" + tile)[resizeFunc]({ height: safeSub(divHeight, 150) }, animationSpeed);

    $("#section2 " + selector + " .close").show();

    //frozen scroll de-freeze
    $(selector + " .RadGrid").each(
                function()
                {
                    //fix header issue in fixed scrolling grid - also if grid is scrolled when maximized you can't see all columns so fixing that too                    
                    if (this.control && this.control.ClientSettings && this.control.ClientSettings.Scrolling.FrozenColumnsCount > 0)
                        $(this).find(".rgHeaderDiv colgroup col, .rgDataDiv colgroup col, .rgHeaderDiv th, .rgDataDiv td[_xcol!=true]").css("display", "");
                    //leave as example - this code was from time TierCoverage used client side databinding
                    //        $("#divTile" + tile + " .rgHeaderDiv th:last").css("display", "none");
                    //        $("#divTile" + tile + " .rgDataDiv tr").each(function() { $(this).find("td:last").css("display", "none"); });

                    $(this).find(" div[id$=_Frozen]").hide();
                }
        );

    //perform additional sizing of contents
    if (callback)
        callback(tile);

    if (resizeFunc != "animate" && animCallback)
        animCallback(tile);
}

function minTile()
{
    var windowHeight = $(window);
    var divWidth = windowHeight.width();
    var divHeight = windowHeight.height();

    var j = $(".maximized");

    var tile = j.attr("_tile");

    tile = parseInt(tile, 10); //tile may have character but at this point use number only        

    j.removeClass("maximized");

    if (j.hasClass("invaliddim"))
    {
        j.removeClass("invaliddim");
        hideModalSR();

        minTileCallback(tile);
    }
    else
    {
        var height = parseInt(j.attr("_h"));
        var width = parseInt(j.attr("_w"));
        var x = parseInt(j.attr("_x"));
        var y = parseInt(j.attr("_y"));
        var ch = parseInt(j.attr("_ch"));

        var fromTop = j.attr("_ft") == "yes";

        if (fromTop)
        {
            j.animate({ top: y, left: x, width: width }, animationSpeed, function() { hideModalSR(); });
        }
        else
        {
            j.animate({ bottom: divHeight - (y + height), right: divWidth - (x + width), width: width }, animationSpeed, function() { hideModalSR(); });
        }

        j.find("#divTile" + tile).animate({ height: ch }, animationSpeed, function() { minTileCallback(tile); });
    }


    //function() { $("#divTile" + tile + " div[id$=_Frozen]").show().scrollLeft(0); }

    j.find(".close").hide();

    j.find(".enlarge").show();
    //    if(callback)
    //        callback(tile);

    if ($(".marketplaceanalytics").length > 0)
        todaysanalytics_content_resize();
}

function minTileCallback(tile)
{
    var j = $("#divTile" + tile + " div[id$=_Frozen]");
    if (ie6)//make ie6 happy
    {
        var j2 = j.find("div");
        j2.width(j.parent().find(".rgMasterTable").width());
    }
    j.show().scrollLeft(0);

    clientManager.get_ApplicationManager().resizeSection();

    if (ie6) //more ie6 happiness
        $("#divTile" + tile).css("visibility", "hidden").css("visibility", "visible");
}

function maxGridTile(tile)
{
    var windowHeight = $(window);
    var divHeight = windowHeight.height();
    var divWidth = windowHeight.width();
    //    $(".enlarge").hide();
    //    $("#fauxModal").css({
    //        zIndex: "9000", visibility: "visible"
    //    }
    //   );

    //    $(".section2SR #tile5").attr({
    //        id: "maxSRTile5"
    //    }
    //   );
    //    $("#maxSRTile5").css({
    //        position: "absolute", zIndex: "10000", bottom: "32px", right: "10px"
    //    }
    //   );
    $("#tile" + tile + " .rgDataDiv").css({
        height: divHeight - 190, overflow: "auto"
    }
        );


    resetGridHeadersX(1000);
    //    $("#maxSRTile5 #divTile5").animate({
    //        height: divHeight - 150
    //    }
    //   , animationSpeed);

    //    $("#maxSRTile5").animate({
    //        width: (divWidth - 100), bottom: "40px", right: "40px"
    //    }
    //   , animationSpeed, showModal);
    //    $(".section2SR #maxSRTile5 .close").show();

}

function maxAffiliationTile(tile)
{
    var windowHeight = $(window);
    var divHeight = windowHeight.height();
    var divWidth = windowHeight.width();

    $("#tile" + tile + " .rgDataDiv").css({
        height: divHeight - 170, overflow: "auto"
    }
        );
}
//function minSRTile5()
//{
//    var windowHeight = $(window);
//    var divWidth = windowHeight.width();
//    var divHeight = windowHeight.height();
//    //Tile 3 Properties (if Tile4 & 5 exist statement)
//    var tile3Height;
//    if (!$get("tile4") && !$get("tile4SR"))
//    {
//        tile3Height = divHeight - 138;
//    }
//    else
//    {
//        tile3Height = divHeight * .40;
//    }
//    $("#maxSRTile5 .close").hide();
//    $("#maxSRTile5").animate({
//        bottom: "32px", right: "10px", width: (divWidth - 226) / 2
//    }
//   , animationSpeed, hideModalSR);
//    $("#maxSRTile5 .rgDataDiv").css({
//        height: (divHeight - tile3Height) - 203
//    }
//       );
//    $(".section2SR #divTile5").animate({
//        height: (divHeight - tile3Height) - 164
//    }
//   , animationSpeed);
//    $("#maxSRTile5").attr({
//        id: "tile5"
//    }
//   );
//}


//function maxSRMap()
//{
//    var windowHeight = $(window);
//    var divHeight = windowHeight.height();
//    var divWidth = windowHeight.width();
//    $(".enlarge").hide();
//    $("#fauxModal").css({
//        zIndex: "9000", visibility: "visible"
//    }
//   );

//    $("#tile3T").attr({
//        id: "maxSRMap"
//    }
//   );
//    $("#maxSRMap").css({
//        position: "absolute", width: (divWidth - 226) / 2, zIndex: "10000", top: "78px", left: "217px"
//    }
//   );
//    $("#maxSRMap #divTile3").animate({
//        height: divHeight - 255
//    }
//   , animationSpeed);

//    $("#maxSRMap").animate({
//        width: (divWidth - 300), top: (divWidth / divHeight) + 100, left: (divWidth / divHeight) + 150
//    }
//   , animationSpeed, showModal);
//    $("#maxSRMap .close").show();

//}
//function minSRMap()
//{
//    var windowHeight = $(window);
//    var divWidth = windowHeight.width();
//    var divHeight = windowHeight.height();
//    var tile2Height = divHeight / topSRHeight;

//    $("#maxSRMap .close").hide();
//    $("#maxSRMap").animate({
//        top: "78px", left: "217px", width: (divWidth - 226) / 2
//    }
//   , animationSpeed, hideModalSR);
//    $("#maxSRMap #divTile3").animate({
//        height: tile2Height
//    }
//   , animationSpeed);
//    $("#maxSRMap").attr({
//        id: "tile3T"
//    }
//   );
//}

//function maxTChart()
//{
//    var windowHeight = $(window);
//    var divHeight = windowHeight.height();
//    var divWidth = windowHeight.width();
//    $(".enlarge").hide();
//    $("#fauxModal").css({
//        zIndex: "9000", visibility: "visible"
//    }
//   );

//    $("#tile4T").attr({
//        id: "maxTChart"
//    }
//   );
//    $("#maxTChart").css({
//        position: "absolute", width: (divWidth - 226) / 2, zIndex: "10000", top: "78px", right: "10px"
//    }
//   );
//    $("#maxTChart #divTile4").animate({
//        height: divHeight - 255
//    }
//   , animationSpeed);

//    $("#maxTChart").animate({
//        width: (divWidth - 200), top: (divWidth / divHeight) + 100, right: (divWidth / divHeight) + 100
//    }
//   , animationSpeed, showModal);
//    $("#maxTChart .close").show();

//}
//function minTChart()
//{
//    var windowHeight = $(window);
//    var divWidth = windowHeight.width();
//    var divHeight = windowHeight.height();
//    var tile2Height = divHeight / topSRHeight;

//    $("#maxTChart .close").hide();
//    $("#maxTChart").animate({
//        top: "78px", right: "10px", width: (divWidth - 226) / 2
//    }
//   , animationSpeed, hideModalSR);
//    $("#maxTChart #divTile4").animate({
//        height: tile2Height
//    }
//   , animationSpeed);
//    $("#maxTChart").attr({
//        id: "tile4T"
//    }
//   );
//}

function maxTBtm(tile)
{
    var windowHeight = $(window);
    var divHeight = windowHeight.height();
    var divWidth = windowHeight.width();
    //    $(".enlarge").hide();
    //    $("#fauxModal").css({
    //        zIndex: "9000", visibility: "visible"
    //    }
    //   );

    //    $("#tile5T").attr({
    //        id: "maxTBtm"
    //    }
    //   );
    //    $("#maxTBtm").css({
    //        position: "absolute", zIndex: "10000", bottom: "32px", right: "10px", width: divWidth - 227
    //    }
    //   );
    //    $("#maxTBtm #divTile5").animate({
    //        height: divHeight - 150
    //    }
    //   , animationSpeed);
    $("#divTile" + tile + " .dashboardTable .rgDataDiv").css({
        height: divHeight - 184
    }
       );
    //    $("#maxTBtm").animate({
    //        width: (divWidth - 100), bottom: "40px", right: "40px"
    //    }
    //   , animationSpeed, showModal);
    //    $("#maxTBtm .close").show();
}
//function minTBtm()
//{
//    var windowHeight = $(window);
//    var divWidth = windowHeight.width();
//    var divHeight = windowHeight.height();
//    var tile2Height = divHeight / topSRHeight;

//    $("#maxTBtm .close").hide();
//    $("#maxTBtm").animate({
//        bottom: "32px", right: "10px", width: divWidth - 227
//    }
//   , animationSpeed, hideModalSR);

//    //SPH 2/15/2010 - decreased constant that is subtracted since float is fixed on chart next to map - ie6 still not correct so putting 1px back
//    $("#maxTBtm #divTile5").animate({
//        height: divHeight - tile2Height - 163 - (ie6 ? 1 : 0)
//    }
//   , animationSpeed);
//    $("#maxTBtm #divTile5 .dashboardTable .rgDataDiv").css({
//        height: (divHeight - tile2Height) - 197 - (ie6 ? 1 : 0)
//    }
//       );
//    //----------------------------------------------------------------------   

//    $("#maxTBtm").attr({
//        id: "tile5T"
//    }
//   );
//}
//function maxBtmLf()
//{
//    var windowHeight = $(window);
//    var divHeight = windowHeight.height();
//    var divWidth = windowHeight.width();
//    var divTile1Height = getDivTile1HeightForTA(divWidth, topLf);
//    var tile1Height = divTile1Height + 27;

//    $(".enlarge").hide();
//    $("#fauxModal").css({
//        zIndex: "9000", visibility: "visible"
//    }
//   );

//    $("#tile3").attr({
//        id: "maxBtmLf"
//    }
//   );
//    $("#maxBtmLf").css({
//        position: "absolute", zIndex: "10000", bottom: "40px", left: "10px", width: divWidth / 2
//    }
//   );
//    $("#maxBtmLf").animate({
//        height: divHeight - 100, width: (divWidth / 2) * 1.5, bottom: divHeight / 20, left: divWidth / 8
//    }
//   , animationSpeed, showModal);
//    $("#maxBtmLf #divTile3, #maxBtmLf #lfView").css({
//        height: divHeight - 127, width: "100%"
//    });
//    $("#maxBtmLf .close").show();
//}
//function minBtmLf()
//{
//    var windowHeight = $(window);
//    var divHeight = windowHeight.height();
//    var divWidth = windowHeight.width();
//    var divTile1Height = getDivTile1HeightForTA(divWidth, topLf);
//    var tile1Height = divTile1Height + 27;

//    $("#maxBtmLf .close").hide();
//    $("#maxBtmLf #divTile3,#maxBtmLf #lfView").css("height", (divHeight - (tile1Height)) - 169);
//    $("#maxBtmLf").animate({
//        bottom: "40px", left: "10px", width: divWidth / 2, height: divHeight / 2
//    }
//   , animationSpeed, hideModal);
//    $("#maxBtmLf").attr({
//        id: "tile3"
//    }
//   );
//    $(".enlarge").show();

//}
//function maxBtmRt()
//{
//    var windowHeight = $(window);
//    var divHeight = windowHeight.height();
//    var divWidth = windowHeight.width();
//    var divTile1Height = getDivTile1HeightForTA(divWidth, topLf);
//    var tile1Height = divTile1Height + 27;

//    $(".enlarge").hide();
//    $("#fauxModal").css({
//        zIndex: "9000", visibility: "visible"
//    }
//   );

//    $("#tile4").attr({
//        id: "maxBtmRt"
//    }
//   );
//    $("#maxBtmRt").css({
//        position: "absolute", zIndex: "10000", bottom: "40px", right: "10px", width: divWidth / 2
//    }
//   );
//    $("#maxBtmRt").animate({
//        height: divHeight - 100, width: (divWidth / 2) * 1.5, bottom: divHeight / 20, right: divWidth / 8
//    }
//   , animationSpeed, showModal);

//    //SPH 1/11/2010 - clear height so all data shows up
//    $("#divTile4Container").css({
//        height: ""
//    });

//    $("#maxBtmRt #divTile4, #maxBtmRt #rtView").css({
//        height: divHeight - 127, width: "100%"
//    });
//    $("#maxBtmRt .close").show();
//}
//function minBtmRt()
//{
//    var windowHeight = $(window);
//    var divHeight = windowHeight.height();
//    var divWidth = windowHeight.width();
//    var divTile1Height = getDivTile1HeightForTA(divWidth, topLf);
//    var tile1Height = divTile1Height + 27;

//    $("#maxBtmRt .close").hide();
//    $("#maxBtmRt #divTile4,#maxBtmRt #rtView").css("height", (divHeight - (tile1Height)) - 169);

//    $("#maxBtmRt").animate({
//        bottom: "40px", right: "10px", width: divWidth / 2, height: divHeight / 2
//    }
//   , animationSpeed, hideModal);
//    $("#maxBtmRt").attr({
//        id: "tile4"
//    }
//   );

//    //SPH 1/11/2010 - reset height so data fits to minimized tile
//    $("#divTile4Container").css({
//        height: (divHeight - tile1Height) - 142
//    });

//    $(".enlarge").show();

//}

function maxMrkt(chart, event)
{
    var windowHeight = $(window);
    var divHeight = windowHeight.height();
    var divWidth = windowHeight.width();

    var divTile1Height; // = getDivTile1HeightForTA(divHeight);

    var taMaxTopHeight = $(window).height() / ($(window).height() > 700 ? 3 : 3.5);
    var divTile1Height = divHeight / 3;

    if (divTile1Height > taMaxTopHeight) divTile1Height = taMaxTopHeight;

    var tile1Height = divTile1Height + 27;
    if (divHeight > 700)
    {
        var divideBy = 2;
    } else
    {
        var divideBy = 2.5;
    }
    $(".section2SR .enlarge").hide();
    $(".section2SR .min").hide();

    //    if (chart == 1)
    //    {
    //        //Chart 1
    //        $("#chartLeft1").attr({
    //            id: "maxchartLeft1"
    //        });
    var chartContainerID = (chart == 1 ? "chartLeft1" : chart == 2 ? "chartLeft2" : "chartRight");
    var chartID = "#chart" + chart;

    var rect = Sys.UI.DomElement.getBounds($get(chartContainerID));
    chartContainerID = "#" + chartContainerID;

    var chartCount = $("#chart1 .grid, #chart2 .grid, #chart3 .grid").length;
    if (chartCount < 1) chartCount = 1;
    width = ($("#divTile4").width() / chartCount);

    $(chartContainerID).css({
        position: "absolute", zIndex: "10000", top: rect.y + "px", left: rect.x + "px", "width": width
    }); //.remove().appendTo("#section2");

    var max = $(chartContainerID).hasClass("maximized");
    var resizeFunc = max ? "css" : "animate";

    $(chartContainerID)[resizeFunc]({
        width: divWidth - 250, height: (tile1Height * 2) - 15, top: (divHeight / 2) - ((tile1Height * 2) - 15) / 2, left: (divWidth / 2) - ((divWidth - 250) / 2)
    }, animationSpeed, function() { showModal(); /*yes! dbl call to resizeStaticHeaders is on purpose*/setTimeout("resizeStaticHeaders('ctl00_Tile4_chart_grid" + chart + "_drillDownContainer')", 1); setTimeout("resizeStaticHeaders('ctl00_Tile4_chart_grid" + chart + "_drillDownContainer')", 100); });

    $(chartID).height((tile1Height * 2) - 15);

    $(chartID + " .chartContainer, " + chartID + " #chartContainer" + chart).css({
        height: safeSub($(chartID).height(), $(chartID + " .tileContainerHeader").height()) * .75
    });

    $(chartID + " #gridContainer" + chart).css({
        height: safeSub($(chartID).height(), ($(chartID + " .tileContainerHeader").height() + $(chartID + " .chartContainer").height()))
    });

    $(chartContainerID + " .close").show();

    $(chartContainerID + " .drillDownContainer, " +
            chartContainerID + " .drillDownContainer table, " +
            chartContainerID + " .gridClone, " +
            chartContainerID + " .cloneHeader").width("100%");

    if (max) //no animation 
        resizeStaticHeaders('ctl00_Tile4_chart_grid' + chart + '_drillDownContainer');


    $(chartContainerID).addClass("maximized").attr("_chartidx", chart);

    $("#divTile4Container").css(
    {
        overflow: "visible"
    });

    $("#fauxModal").css({
        zIndex: "9000", visibility: "visible"
    }
   );

    cancelEvent(event);
}

function cancelEvent(event)
{
    if (event)
    {
        event.cancelBubble = true;
    }
}

function minMrkt(chart, event)
{
    var windowHeight = $(window);
    var divWidth = windowHeight.width();
    var divHeight = windowHeight.height();

    var tile3Height = 58;

    var chartDivWidth;
    var chartDivHeight;

    var chartCount = getMrktChartCount();

    var chartContainerID = (chart == 1 ? "chartLeft1" : chart == 2 ? "chartLeft2" : "chartRight");
    var chartID = "#chart" + chart;

    var rect = Sys.UI.DomElement.getBounds($get(chartContainerID));
    chartContainerID = "#" + chartContainerID;

    $(chartContainerID + " .drillDownContainer, " + chartContainerID + " .cloneHeader").width("");

    //    if (chart == 1)
    //    {
    //Chart 1
    $(chartContainerID + " .close").hide();

    $(chartContainerID).removeClass("maximized");

    var width;

    if (chartCount == 1 || chartCount == 4)
        width = "100%";

    if ((chartCount) == 2 || (chartCount == 3))
        width = "49.6%";

    var chartCount = $("#chart1 .grid, #chart2 .grid, #chart3 .grid").length;
    if (chartCount < 1) chartCount = 1;
    //        if (ie6 || ie7)
    //        {
    width = ($("#divTile4").width() / chartCount);
    //        }

    var rectDiv4 = Sys.UI.DomElement.getBounds($get("divTile4"));
    var leftPos = rectDiv4.x;
    var topPos = rectDiv4.y;
    leftPos += chart == 1 ? 0 : chart == 2 || chartCount == 2 ? width : width * 2;

    $(chartContainerID).animate({
        width: width, height: (safeSub(safeSub(divHeight, 131), tile3Height) * .7) - 2, left: leftPos, top: topPos
    }, animationSpeed, function() { hideModalMA(); resizeStaticHeaders('ctl00_Tile4_chart_grid' + chart + '_drillDownContainer'); });

    $(chartID).css({
        height: $("#divTile4Container").height() - 2
    });

    $(chartID + " .chartContainer, #chart1 #chartContainer" + chart).css({
        height: safeSub($(chartID).height(), $(chartID + " .tileContainerHeader").height()) * .75
    });

    $(chartID + " #gridContainer" + chart).css({
        height: safeSub($(chartID).height(), ($(chartID + " .tileContainerHeader").height() + $(chartID + " .chartContainer").height()))
    });

    //    setTimeout("resizeStaticHeaders('ctl00_Tile4_chart_grid" + chart + "_drillDownContainer')", 500);
    //    }
    //    if (chart == 2)
    //    {
    //        //Chart 2
    //        $("#maxchartLeft2 .close").hide();

    //        var width;

    //        if (chartCount == 2)
    //            width = "49.8%";
    //        if (chartCount == 3)
    //            width = "49.5%";

    //        $("#maxchartLeft2").css({
    //            width: width, height: (safeSub(safeSub(divHeight, 131), tile3Height) * .7) - 2, top: "", left: "", position: "inherit"
    //        }, animationSpeed, hideModal);

    //        $("#chart2").css({
    //            height: $("#divTile4Container").height() - 2
    //        });

    //        $("#chart2 .chartContainer, #chart2 #chartContainer2").css({
    //            height: safeSub($("#chart1").height(), $("#chart1 .tileContainerHeader").height()) * .75
    //        });

    //        $("#chart2 #gridContainer2").css({
    //            height: safeSub($("#chart1").height(), ($("#chart1 .tileContainerHeader").height() + $("#chart1 .chartContainer").height()))
    //        });

    //        $("#maxchartLeft2").attr({
    //            id: "chartLeft2"
    //        });

    //        setTimeout("resizeStaticHeaders('ctl00_Tile4_chart_grid2_drillDownContainer')", 500);
    //    }
    //    if (chart == 3)
    //    {
    //        //Chart 3
    //        $("#maxchartRight .close").hide();

    //        var width;

    //        if (chartCount == 3)
    //            width = "33.5%";
    //        if (chartCount == 4)
    //            width = "49.8%";

    //        $("#maxchartRight").css({
    //            width: width, height: (safeSub(safeSub(divHeight, 131), tile3Height) * .7) - 2, top: "", left: "", position: "inherit"
    //        }, animationSpeed, hideModal);

    //        $("#chart3").css({
    //            height: $("#divTile4Container").height() - 2
    //        });

    //        $("#chart3 .chartContainer, #chart3 #chartContainer3").css({
    //            height: safeSub($("#chart1").height(), $("#chart1 .tileContainerHeader").height()) * .75
    //        });

    //        $("#chart3 #gridContainer3").css({
    //            height: safeSub($("#chart1").height(), ($("#chart1 .tileContainerHeader").height() + $("#chart1 .chartContainer").height()))
    //        });

    //        $("#maxchartRight").attr({
    //            id: "chartRight"
    //        });

    //        setTimeout("resizeStaticHeaders('ctl00_Tile4_chart_grid3_drillDownContainer')", 500);
    //    }

    $("#divTile4Container").css(
    {
        overflow: "hidden"
    });

    $("#fauxModal").css({
        zIndex: "9000", visibility: "hidden"
    }
   );

    $(".section2SR .enlarge").show();
    //$("#tile1 .min").show();

    cancelEvent(event);
}

function maxTile5MrktAnim()
{
    setTimeout("resizeStaticHeaders('drillDownContainer')", 1);
}
function maxTile5Mrkt()
{
    var windowHeight = $(window);
    var divHeight = windowHeight.height();
    //    var divWidth = windowHeight.width();

    //    $("#fauxModal").css({
    //        zIndex: "9000", visibility: "visible"
    //    }
    //   );

    //    var max = $("#tile5").hasClass("maximized");
    //    var resizeFunc = max ? "css" : "animate";


    //    $(".section2SR #tile5 .enlarge").hide();

    //    $("#tile5").css({
    //        position: "absolute", zIndex: "10000", bottom: "32px", left: "217px"
    //    }
    //   );
    //    $("#tile5 #divTile5Container")[resizeFunc]({
    //        height: divHeight - 150
    //    }
    //   , animationSpeed);


    if (ie6)
    {
        $("#divTile5").find(".drillDownContainer, .cloneHeader, .gridClone").width("100%");
    }


    //    $("#tile5")[resizeFunc]({
    //        width: (divWidth - 100), bottom: "40px", left: "40px"
    //    }
    //   , animationSpeed, showModal);
    //    $(".section2SR #tile5 .close").show();

    //    $("#tile5").addClass("maximized");


    //    setTimeout("$('#tile5 #divTile5').height($('#tile5 #divTile5Container').height() - $('#tile5 #divTile5Container .tileContainerHeader').height())", 500);

    $('#tile5 #drillDownContainer').height(safeSub(divHeight, $("#ctl00_partialPage_detailedGrid_gridCloneDiv").height() + 150));
}

//function minTile5Mrkt()
//{
//    var windowHeight = $(window);
//    var divWidth = windowHeight.width();
//    var divHeight = windowHeight.height();
//    var tile3Height = 58;

//    $("#tile5").removeClass("maximized");

//    $(".section2SR #tile5 .close").hide();
//    $("#tile5").animate({
//        bottom: "32px", left: "217px"
//    }
//   , animationSpeed);

//    $("#tile5 #divTile5Container").animate({
//        height: safeSub(safeSub(divHeight, 131), (tile3Height + $("#tile4 #divTile4Container").height())) + 14
//    }
//   , animationSpeed);

//    $("#tile5").removeAttr('style');


//    $("#fauxModal").css({
//        zIndex: "9000", visibility: "hidden"
//    }
//   );

//    setTimeout("$('#tile5 #divTile5').height($('#tile5 #divTile5Container').height() - $('#tile5 #divTile5Container .tileContainerHeader').height())", 500);

//    setTimeout("$('#tile5 #drillDownContainer').height($('#tile5 #divTile5').height() - $('#tile5 .cloneHeader').height())", 500);

//    setTimeout("resizeStaticHeaders('drillDownContainer')", 500);

//    $(".section2SR .enlarge").show();
//}

//Used to get chart count for marketplace analytics
function getMrktChartCount()
{
    var chartCount = 1;
    var territoryVisible = false;

    if ($("#ctl00_Tile4_chart_grid2_gridTemplate").length > 0)
    {
        chartCount++;
        territoryVisible = true;
    }

    if ($("#ctl00_Tile4_chart_grid3_gridTemplate").length > 0)
    {
        chartCount++;
        if (territoryVisible == false)
            chartCount = chartCount + 2;
    }

    return chartCount;
}

//Used to resize static headers
function resizeStaticHeaders(gridContainer)
{
    var tbl = $("#" + gridContainer).find("table");

    if (tbl.length > 0)
    {
        var tblID = $(tbl).attr("id");

        $("#" + gridContainer + "_gridClone").width($(tbl).width() - (ie6 ? 5 : 0));

        var firstRow = $("#" + tblID + " tbody tr:first td");
        var firstDRow = tbl[0].rows[1];


        var firstRowClone = $("#" + gridContainer + "_gridClone tr:first th");
        if (firstRowClone.length > 0)
        {
            firstRowClone.each(function(n)
            {
                var cell = $(this);
                var cellWidth = cell.width();
                var cellIndex = $(this).index() + 1;

                //Check if key column
                if (cellIndex == 1 && (cell[0].innerText == "" || cell[0].innerHTML == "&nbsp;"))
                    cellWidth = 10;

                $("#" + tblID + " tr:first th:nth-child(" + cellIndex + ")").width(cellWidth);
                if (firstDRow)
                    firstDRow.cells(cellIndex - 1).style.width = cellWidth + "px";
            });
        }


        if (firstRow.length > 0)
        {
            firstRow.each(function(n)
            {
                var cell = $(this);
                var cellWidth = cell.width();
                var cellIndex = $(this).index() + 1;

                //Check if key column
                if (cellIndex == 1 && cell[0].innerText == "")
                    cellWidth = 10;

                $("#" + gridContainer + "_gridClone tr:first th:nth-child(" + cellIndex + ")").width(cellWidth);
                if (firstDRow)
                    firstDRow.cells(cellIndex - 1).style.width = cellWidth + "px";
            });
        }

        //

        //

        $(tbl).css("margin-top", "-" + ($("#" + tblID + " tr:first").height()) + "px");
        //        $(tbl).find("tr:first").hide();

        //Adjust heights
        var j = $("#" + gridContainer);
        if (j.length > 0)
            j.height(safeSub(j.parent().height(), (Sys.UI.DomElement.getBounds(j[0]).y - Sys.UI.DomElement.getBounds(j.parent()[0]).y)));

        //Add margin to header only if scroll bar exists
        if ($("#" + gridContainer).height() > ($("#" + tblID).height() - $("#" + gridContainer + "_gridCloneDiv").height()))
            $("#" + gridContainer + "_gridCloneDiv").css("margin-right", "0px");
        else
            $("#" + gridContainer + "_gridCloneDiv").css("margin-right", "17px");
    }
}


// TEXT RESIZING FOR DATA GRIDS 
function textSmall()
{
    var windowHeight = $(window);
    var divWidth = windowHeight.width();
    if (divWidth <= 1024)
    {
        $("#htmlModalWindow .dashboardTable td, #maxPlanInfo .dashboardTable td, #maxKC .dashboardTable td, #maxAff .dashboardTable td ").css("font-size", "10px");
    }
    else
    {
        $("#htmlModalWindow .dashboardTable td, #maxPlanInfo .dashboardTable td, #maxKC .dashboardTable td, #maxAff .dashboardTable td").css("font-size", "11px");
    }
}
function textMedium()
{
    $("#htmlModalWindow .dashboardTable td, #maxPlanInfo .dashboardTable td, #maxKC .dashboardTable td, #maxAff .dashboardTable td").css("font-size", "13px");
}
function textLarge()
{
    $("#htmlModalWindow .dashboardTable td, #maxPlanInfo .dashboardTable td, #maxKC .dashboardTable td, #maxAff .dashboardTable td").css("font-size", "15px");
}
function truncateMenu(id, len)
{
    var selector = ".planInfoTruncate .rcbInputCell .rcbInput, .standardReportsTruncate .rcbInputCell .rcbInput";
    if (id)
        selector = "#" + id + " .rcbInputCell .rcbInput";
    if (!len)
        len = 25;
    var itemEllipse = $(selector);
    if (itemEllipse)
    {
        var trunc = itemEllipse.attr("value");
        if (trunc)
        {
            if (trunc.length > len)
            {
                trunc = trunc.substring(0, len);
                trunc += '...';
                itemEllipse.attr("value", trunc);
            }
        }
    }
}
// used for resizing dundas flash charts
function scaleChart(tiles)
{
    var selector = "";
    var objectTag;

    if ($.browser.msie)
        objectTag = " object";
    else if (flashSupported)
        objectTag = " embed";
    else
        objectTag = " img.chart";

    if (!tiles)
        tiles = [3];
    else if (!$.isArray(tiles))
        tiles = [tiles];

    for (var i = 0; i < tiles.length; i++)
        selector += ((selector ? ", " : "") + "#divTile" + tiles[i] + " div:not(.hiddenGrid) > .chartContainer " + objectTag); //3 embed, #divTile4 embed" : "#divTile3 object, #divTile4 object";


    var countObjects = $(selector).length;
    var w = "100%";
    if (countObjects == 2)
        w = "50%";
    if (countObjects == 3)
        w = "33%";

    $(".chartContainer").css({ width: w });

    if (flashSupported)
        $(selector).width("100%").height("100%");

    $(".chartThumb img").tooltip({
        track: true,
        showURL: false
    });
}
// used for resizing dundas flash charts for marketplace analytics
function scaleChartMrkt(tiles)
{
    var selector = "";
    var objectTag = $.browser.msie ? " object" : " embed";

    if (!tiles)
        tiles = [3];
    else if (!$.isArray(tiles))
        tiles = [tiles];

    for (var i = 0; i < tiles.length; i++)
        selector += ((selector ? ", " : "") + "#divTile" + tiles[i] + " div:not(.hiddenGrid) > .chartContainer " + objectTag); //3 embed, #divTile4 embed" : "#divTile3 object, #divTile4 object";


    //var countObjects = $(selector).length;
    //var w = "100%";
    //if (countObjects == 2)
    //    w = "50%";
    //if (countObjects == 3)
    var w = "100%";

    $(".chartContainer").css({ width: w });

    $(selector).width("100%").height("100%");

    $(".chartThumb img").tooltip({
        track: true,
        showURL: false
    });
}

function fixIEScroll()
{
    //resizes scroll bars for IE7 and IE6 in covered lives views
    if (ie6 || ie7)
    {
        var lfviewTotal = $("#lfView .dashboardTable").height() + $("#lfView .totalCL").height() + $("#lfView .totalCL2").height();
        if (lfviewTotal >= $(".TriSectionLft #divTile3, #divTile3Max").height())
        {
            $(".TriSectionLft #lfView").css({
                marginRight: "16px"
            });
            $("#lfView .areaHeader").css({
                width: "100%"
            });
        } else
        {
            $(".TriSectionLft #lfView").css({
                marginRight: "0px"
            });
        }
    }

    if (ie6 || ie7)
    {
        if ($("#benDsnTotal").height() >= $(".TriSectionLft #divTile4, #divTile4Max").height())
        {
            $("#benDsnTotal").css({
                marginRight: "16px"
            });
            $("#benDsnTotal .areaHeader").css({
                width: "100%"
            });

        }
    }
}


function buildHeaderRow(table, rowDetails, telerik)
{
    if (!table) return;
    if (!rowDetails) return;

    if (telerik)
        table = $(table).find("table")[0];

    if (!table) return;

    var row = table.insertRow(0);

    row.className = "gridHdr";

    var cell;
    var props;
    for (var i = 0; i < rowDetails.length; i++)
    {
        props = rowDetails[i];
        if (props.span > 0)
        {
            cell = row.insertCell(-1);
            cell.colSpan = props.span;
            cell.className = props.cssClass;
            cell.innerHTML = props.text ? props.text : "&nbsp;";

        }
    }
}




// Used to load Flash Map on page other than Today's Accounts
function loadFlashMap(data, containerID)
{
    var mapIsReady = false;

    //Remove map from Today's Accounts
    var j = $("#divTile1 object").remove();

    if ($.browser.msie)
    {
        j.appendTo("#" + containerID);

        function innerLoadFlashMap()
        {
            if (!ensureMapIsLoaded(data, containerID))
            {
                if (!clientManager.get_MapIsReady())
                    new cmd(null, initMap, ["fmASEngine", data], 500);
                else
                    mapIsReady = true;
            }
        }

        if (ie6 || ie7) //delay ie6/7 because appendTo operation causes issue with init
            new cmd(null, innerLoadFlashMap, null, 1);
        else
            innerLoadFlashMap();
    }

    return mapIsReady;
}

// Used to reset Flash Map for Today's Accounts
function resetFlashMap(containerID)
{
    var j = $(containerID + " object").remove();
    if ($.browser.msie)
        j.appendTo("#divTile1");
}


function setPageUIState(clientManager)
{
    if (clientManager.get_UIReady())
    {
        //app is ApplicationUrlName unless custom options which then is set to ChannelUrlName which is something such as CustomerContactReports or FormularySellSheets
        var app = clientManager.get_ApplicationUrlName().split('/');
        app = app[app.length - 1];
        var mod = clientManager.get_Module();
        var client = clientManager.get_ClientKey();

        //clears all current classes
        $(document.body).attr("class", "").addClass(app).addClass(mod).addClass(client);
    }
}

// Account Planning Resize
function AccountPlanningApplication_content_resize()
{
    var browserWindow = $(window);
    var divHeight = browserWindow.height();
    var divWidth = browserWindow.width();
    $("#tile2").hide();

    //    $(".tileContainerHeader").show(); //remove this after custome options screen shot demo is gone

    $("#section2").css({
        marginLeft: "0px", marginTop: "0px", width: "auto"
    }
    );
    //Right part of SR   
    $("#section2").css({
        position: "relative"
    }
    );
    AccountPlanningApplication_section_resize();
}
function AccountPlanningApplication_section_resize()
{
    var browserWindow = $(window);
    var divHeight = browserWindow.height();
    var divWidth = browserWindow.width();
    var tile3Height;
    if (!$get("tile4") && !$get("tile4SR"))
    {
        tile3Height = divHeight - 138;
    }
    else
    {
        tile3Height = divHeight * .40;
    }
    //Browser Settings
    if (ie6)
    {
        $("#tile3 #divTile3Container ").css({
            height: tile3Height, top: "-5px"
        }
        );
    }
    else
    {
        $("#tile3 #divTile3, #tile3SR #divTile3").css({
            height: tile3Height, textAlign: "center", width: "auto", overflow: "hidden"
        }
        );
    }
    $("#tile3").removeClass("leftTile");
}


// End Account Planning Resize
//custom fhr report resize
function fhr_content_resize()
{



    var browserWindow = $(window);
    var divHeight = browserWindow.height();
    var divWidth = browserWindow.width();
    var tile2Height = safeSub(divHeight, 105);
    var collaspeLft = $(".todaysAccounts2Expand").height();

    //    $(".tileContainerHeader").show(); //remove this after custome options screen shot demo is gone

    $("#fauxModal").css({
        width: divWidth, height: divHeight
    });


    $(" #tile2 ").css({
        width: "200px", top: "5px", left: "6px", position: "absolute", marginLeft: "0px"
    }
      );
    if (collaspeLft > 0)
    {
        $("#section2").css({
            marginLeft: "34px"
        }, animationSpeed);
    } else
    {
        $("#section2").css({
            marginLeft: "208px"
        }
       , animationSpeed);
    }

    //Tile2 properties
    $("#tile2, #tileMin2SR").css({
        height: tile2Height
    }
   , animationSpeed);
    $("#expandTile2SR, #tileMin2").css({
        height: safeSub(divHeight, 112)
    }
, animationSpeed);
    $("#expandTile2SR").css({
        height: "100%"
    }
, animationSpeed);
    $("#tile2 .min").show();
    $("#tile2 .enlarge, #divTile2Plans").hide();

    reportFiltersResize();
    ////Right part of SR   
    fhr_section_resize();

}

function fhr_section_resize()
{
    if (clientManager.get_Module() != "cotiercoveragehxformulary" && clientManager.get_Module() != "corestrictionshxformulary")
    {
        var browserWindow = $(window);
        var divHeight = browserWindow.height();
        var divWidth = browserWindow.width();
        var tile2Height = divHeight / topSRHeight;
        var ie6 = $.browser.msie && $.browser.version == "6.0";
        var hdrElement = $("#divTile4 thead tr");
        var height = 20;
        if ($get("tile2"))
            $(".section2SR .enlarge").show();
        $("#maxTChart .enlarge, #maxSRMap .enlarge, #maxTBtm .enlarge, #maxChart .enlarge, #maxSRTile4 .enlarge, #maxSRTile5 .enlarge").hide();
        if (hdrElement.length > 0)
        {
            height = Sys.UI.DomElement.getBounds(hdrElement[0]).height;
        }
        //Tile 3 Properties (if Tile4 & 5 exist statement)
        var tile3Height = getSRTile3Height(divHeight);

        if (ie6)
        {
            $("#tile3 #divTile3Container ").css({
                height: tile3Height
            }
       );
        }

        var maxHeight = divHeight - tile3Height;

        $("#tile4 #divTile4, #tile5 #divTile5, #tile4SR #divTile4, #tile5SR #divTile5 ").css({
            height: safeSub((maxHeight), 164)
        });

        $("#tile3 #divTile3, #tile3SR #divTile3").css({
            height: tile3Height, textAlign: "center", width: "auto", overflow: "hidden"
        }
       );
        $(".section2SR #tile4 #divTile4 .dashboardTable .rgDataDiv, .section2SR #tile5 #divTile5 .dashboardTable .rgDataDiv").css({
            overflow: "auto"
        }
       );

        $(".section2SR #tile4 #divTile4 .dashboardTable .rgDataDiv").css({ height: safeSub((divHeight - tile3Height) - height, 164) });

        var j = $(".section2SR #tile4 #divTile4 .tiercoverage .dashboardTable .rgDataDiv");
        if (j.length == 1)
            j.css("height", safeSub((maxHeight) - height, 182 + $(".reportDataTitle").height()));
        else
            j.css("height", "");

        j = $(".section2SR #tile4 #divTile4 .formularystatus .dashboardTable .rgDataDiv");
        if (j.length == 1)
            j.css("height", safeSub((maxHeight) - height, 164 + $(".reportDataTitle").height()));
        else
            j.css("height", "");

        $("#ctl00_Tile4_tiercoveragedata1_gridtiercoverage_FrozenScroll").css({
            width: "800px"
        }
       );
        $("#maxSRTile4 #ctl00_Tile4_tiercoveragedata1_gridtiercoverage_FrozenScroll").css({
            width: "100%"
        }
       );

        adjustTile5HeightForDrilldown(maxHeight);

        $(".section2SR #tile5 #divTile5 #tile5CLDataDrillDown .dashboardTable .rgDataDiv").css({
            height: safeSub((maxHeight), 210)
        }
           );

        $(".section2SR #tile3 #divTile3 .dashboardTable .rgDataDiv ").css({
            overflowX: "scroll",
            overflow: "auto",
            width: "100%"
        }
          );

        if (ie6)
        {
            $(".section2SR #tile3 #divTile3 .rgDataDiv").css({
                height: divHeight - 185
            }
          );
        }
        else
        {
            $(".section2SR #tile3 #divTile3 .rgDataDiv").css({
                height: divHeight - 160 - $(".section2SR #tile3 #divTile3 .rgHeaderDiv").height() - $(".fhrNote").height() - $("tilePaginationHeader").height()
            });

        }

        $("#ctl00_Tile3_fhrComparison_fhrContainer").css({
            height: divHeight - 140 - $(".fhrNote").height(), 'border-bottom': "#2d58a7 1px solid", overflowX: "auto", overflowY: "auto"
        });

        $("#tile3T #divTile3, #tile4T #divTile4").css({
            height: tile2Height
        }
       );

        //SPH 2/15/2010 - decreased constant that is subtracted since float is fixed on chart next to map - ie6 still not correct so putting 1px back
        $("#tile5T #divTile5").css({
            height: divHeight - tile2Height - 163 - (ie6 ? 1 : 0)
        }
       );
        $("#tile5T #divTile5 .dashboardTable .rgDataDiv").css({
            height: (divHeight - tile2Height) - 197 - (ie6 ? 1 : 0)
        });
        //-------------------------------------------------------------------------
        $("#maxTBtm #divTile5").css({
            height: divHeight - 150
        }
       , animationSpeed);
        $("#maxTBtm #divTile5 .dashboardTable .rgDataDiv").css({
            height: divHeight - 184
        }
           );
        $("#maxTBtm").css({
            width: (divWidth - 100), bottom: "60px", right: "40px"
        }
       , animationSpeed);
        $("#maxTChart #divTile4").css({
            height: divHeight - 255
        }
       , animationSpeed);

        $("#maxTChart").css({
            width: (divWidth - 200), top: (divWidth / divHeight) + 100, right: (divWidth / divHeight) + 100
        }
       , animationSpeed);
        $("#maxSRMap #divTile3").css({
            height: divHeight - 255
        }
       , animationSpeed);

        $("#maxSRMap").css({
            width: (divWidth - 350), top: (divWidth / divHeight) + 100, left: (divWidth / divHeight) + 150
        }
       , animationSpeed);
        $("#maxChart #divTile3").css({
            height: divHeight - 150
        }
       , animationSpeed);

        $("#maxChart").css({
            width: (divWidth - 100), top: (divWidth / divHeight) + 40, left: (divWidth / divHeight) + 40
        }
       , animationSpeed);

        $("#maxSRTile5 #divTile5").css({
            height: divHeight - 150
        }, animationSpeed);

        $("#maxSRTile5").css({
            width: divWidth - 100, bottom: "40px", right: "40px"
        }
       , animationSpeed);

        $("#tile3").removeClass("leftTile");
        $(".todaysAccounts1").css({
            padding: "0px",
            position: "relative"
        });



        if (ie6)
        {
            $("#ctl00_main_subheader1_channelMenu").css("visibility", "hidden").css("visibility", "");
        }

        if (chrome)
        {
            var j = $("#divTile4 div[id$=_Frozen]");
            if (j.length)
            {
                var p = j.parent();
                p.find(".rgDataDiv").css("overflow-x", "hidden");
                j.find("div").width(p.find(".rgMasterTable").width());
            }
        }

        if ($.browser.version == "7.0")
        {
            $(".rmLink").css({ width: "160px" });
        }


        if (ie6)
        {
            //Fix for channel selection menu
            $(".channelSelectorContainer .rmVertical .rmLink, .channelSelectorContainer .rmVertical .rmItem").css('cssText', 'width: 195px !important');

            //Formulary Status scroll issue fix
            if ($('#ctl00_Tile4_gridFDSummary_gridNational_gridformularystatus').width() > $('#divTile4').width())
                $('#ctl00_Tile4_gridFDSummary_gridNational_gridformularystatus').css('margin-right', '17px');

            if ($('#ctl00_Tile4_gridFDSummary_gridRegional_gridformularystatus').width() > $('#divTile4').width())
                $('#ctl00_Tile4_gridFDSummary_gridRegional_gridformularystatus').css('margin-right', '17px');

            //Tier Coverage scroll issue fix
            if ($('#ctl00_Tile4_tiercoveragedata1_dataUS_gridtiercoverage').width() > $('#divTile4').width())
                $('#ctl00_Tile4_tiercoveragedata1_dataUS_gridtiercoverage').css('margin-right', '17px');

            if ($('#ctl00_Tile4_tiercoveragedata1_gridStateTerr_gridtiercoverage').width() > $('#divTile4').width())
                $('#ctl00_Tile4_tiercoveragedata1_gridStateTerr_gridtiercoverage').css('margin-right', '17px');
        }

        //clears Telerik computed width in the headers for the data table
        //setTimeout("resetGridHeaders()", 1500);
        resetGridHeadersX(500);
    }
}
//end of custom fhr resize