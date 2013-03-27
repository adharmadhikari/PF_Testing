/// <reference name="MicrosoftAjax.js"/>
/// <reference path="~/content/scripts/jquery-1.4.1-vsdoc.js"/>
/// <reference path="~/content/scripts/ui.js"/>
/// <reference path="~/content/scripts/clientManager.js"/>

//Formulary History Reporting starts
Pathfinder.UI.WarnerFormularyHistoryReportingApplication = function(id)
{
    Pathfinder.UI.WarnerFormularyHistoryReportingApplication.initializeBase(this, [id]);
};

Pathfinder.UI.WarnerFormularyHistoryReportingApplication.registerClass("Pathfinder.UI.WarnerFormularyHistoryReportingApplication", Pathfinder.UI.FormularyHistoryReportingApplication);

//Formulary History Reporting ends
function maxcustomMrkt(chart, event)
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

    var chartCount = 2; //$("#chart1 .grid, #chart2 .grid, #chart3 .grid").length;
    //if (chartCount < 1) chartCount = 1;

    width = ($("#divTile3").width() / chartCount);

    $(chartContainerID).css({
        position: "absolute", zIndex: "10000", top: rect.y + "px", left: rect.x + "px", "width": width
    }); //.remove().appendTo("#section2");

    var max = $(chartContainerID).hasClass("maximized");
    var resizeFunc = max ? "css" : "animate";

    $(chartContainerID)[resizeFunc]({
        width: divWidth - 250, height: (tile1Height * 2) - 15, top: (divHeight / 2) - ((tile1Height * 2) - 15) / 2, left: (divWidth / 2) - ((divWidth - 250) / 2)
    }, animationSpeed, function()
    {
        showModal(); /*yes! dbl call to resizeStaticHeaders is on purpose*/
//        setTimeout("resizeStaticHeaders('ctl00_Tile3_chart_grid" + chart + "_drillDownContainer')", 1);
//        setTimeout("resizeStaticHeaders('ctl00_Tile3_chart_grid" + chart + "_drillDownContainer')", 100);
     });

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

    //if (max) //no animation 
        //resizeStaticHeaders('ctl00_Tile3_chart_grid' + chart + '_drillDownContainer');


    $(chartContainerID).addClass("maximized").attr("_chartidx", chart);

    $("#tile3 #divTile3 .rgHeaderDiv").css({ width: "auto", paddingRight: "0px" });
    $("#divTile3Container").css(
    {
        overflow: "visible"
    });

    $("#fauxModal").css({
        zIndex: "9000", visibility: "visible"
    }
   );

    cancelEvent(event);
}
function mincustomMrkt(chart, event)
{
    var windowHeight = $(window);
    var divWidth = windowHeight.width();
    var divHeight = windowHeight.height();

    var tile3Height = 0;

    var chartDivWidth;
    var chartDivHeight;

    var chartCount = getCustomMrktChartCount();

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

    var chartCount = 2; //$("#chart1 .grid, #chart2 .grid, #chart3 .grid").length;
    //if (chartCount < 1) chartCount = 1;
    //        if (ie6 || ie7)
    //        {
    width = ($("#divTile3").width() / chartCount);
    //        }

    var rectDiv4 = Sys.UI.DomElement.getBounds($get("divTile3"));
    var leftPos = rectDiv4.x;
    var topPos = rectDiv4.y;
    leftPos += chart == 1 ? 0 : chart == 2 || chartCount == 2 ? width : width * 2;

    $(chartContainerID).animate({
        width: width, height: (safeSub(safeSub(divHeight, 131), tile3Height) * .7) - 2, left: leftPos, top: topPos
    }, animationSpeed, function()
    {
        hideModalCustomMA();
    //    resizeStaticHeaders('ctl00_Tile3_chart_grid' + chart + '_drillDownContainer'); 
    });

    $(chartID).css({
        height: $("#divTile3Container").height() - 2
    });

    $(chartID + " .chartContainer, #chart1 #chartContainer" + chart).css({
        height: safeSub($(chartID).height(), $(chartID + " .tileContainerHeader").height()) * .75
    });

    $(chartID + " #gridContainer" + chart).css({
        height: safeSub($(chartID).height(), ($(chartID + " .tileContainerHeader").height() + $(chartID + " .chartContainer").height()))
    });

    $("#divTile3Container").css(
    {
        overflow: "hidden"
    });
    $("#divTile3Container #divTile3 .tileContainer").css({overflow: "hidden"});

    $("#fauxModal").css({
        zIndex: "9000", visibility: "hidden"
    }
   );

    $(".section2SR .enlarge").show();
    //$("#tile1 .min").show();

    cancelEvent(event);
}
function hideModalCustomMA()
{
    $("#tile3 .enlarge").show();
    $("#chartLeft1, #chartLeft2, #chartRight").removeAttr("style");
    customtodaysanalytics_content_resize();
}
function customtodaysanalytics_content_resize()
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
    customtodaysanalytics_section_resize();
}

function customtodaysanalytics_section_resize()
{
    var browserWindow = $(window);
    var divHeight = browserWindow.height();
    var divWidth = browserWindow.width();
    var tile2Height = divHeight / topSRHeight;
    var ie6 = $.browser.msie && $.browser.version == "6.0";
    var hdrElement = $("#divTile3 thead tr");
    var height = 20;
    if ($get("tile2") && $(".maximized").length == 0)
        $(".section2SR .enlarge").show();
    $("#maxTChart .enlarge, #maxSRMap .enlarge, #maxTBtm .enlarge, #maxChart .enlarge, #maxSRtile3 .enlarge, #maxSRtile4 .enlarge").hide();
    if (hdrElement.length > 0)
    {
        height = Sys.UI.DomElement.getBounds(hdrElement[0]).height;
    }
    //Tile 3 Properties
    var timeFrameHeight = 0;  

    $("#tile3 #divTile3Container").css({
        //height: timeFrameHeight, textAlign: "center", width: "auto", overflow: "hidden"
    height: (safeSub(safeSub(divHeight, 131), timeFrameHeight) * .6) + $("#tile3 #chart1 .tileContainerHeader").height(), textAlign: "center", width: "auto", overflow: "hidden"
    });

   
    $("#tile3 .dashboardTable TH").css("padding-right", "20px");
    $("#tile3 .dashboardTable td").css("padding-right", "20px");

    $("#tile3 #divTile3Container #divTile3").css({ height: ($("#tile3 #divTile3Container").height() - $("#tile3 #divTile3Container .tileContainerHeader").height())
    });

    $("#tile3 #chart1, #tile3 #chart2, #tile3 #chart3").css({
        height: $("#tile3 #divTile3").height() - 2, width: "auto", overflow: "auto"
    });


    $("#tile3 #chart1 .chartContainer, #tile3 #chart1 #chartContainer1, #tile3 #chart2 .chartContainer, #tile3 #chart2 #chartContainer2, #tile3 #chart3 .chartContainer, #tile3 #chart3 #chartContainer3").css({
    height: safeSub($("#tile3 #chart1").height(), $("#tile3 #chart1 .tileContainerHeader").height()) * .75 - $("#tile3 #chart1 .tileContainerHeader").height()
    });

    $("#tile3 #chart1 #gridContainer1, #tile3 #chart2 #gridContainer2, #tile3 #chart3 #gridContainer3").css({
    height: ($("#tile3 #chart1").height()- $("#tile3 #chart1 .tileContainerHeader").height() - $("#tile3 #chart1 .chartContainer").height() )
    });

    //Set the height of the grids
    $("#gridContainer1 .rgDataDiv").height($("#gridContainer1").height() - $("#gridContainer1 .rgHeaderDiv").height());
    $("#gridContainer2 .rgDataDiv").height($("#gridContainer2").height() - $("#gridContainer2 .rgHeaderDiv").height());
    $("#gridContainer3 .rgDataDiv").height($("#gridContainer3").height() - $("#gridContainer3 .rgHeaderDiv").height());

    //Dynamically set the widths of the chart areas based on number of charts - CSS file is set to display 3 charts by default
    var chartCount = getCustomMrktChartCount();

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
        $("#chartLeft2:not(.maximized)").css("float", "right");
        $("#chartLeft1:not(.maximized)").css("width", "49.7%");
        $("#chartLeft1:not(.maximized)").css("float", "left");

        if ($(".prescriberreporting").length > 0)
        {
            if ($("#ctl00_tile3_chart_grid1_gridTemplate").length < 1)
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
            if ($("#ctl00_tile3_chart_grid1_gridTemplate").length < 1)
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
            if ($("#ctl00_tile3_chart_grid1_gridTemplate").length < 1)
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

        var chartWidth = ($("#divTile3").width() / chartCount);
        $(".TriStack2 .cloneHeader").width(safeSub(chartWidth, !ie6 ? 10 : 20));
        $("#chartLeft .drillDownContainer, #chartRight .drillDownContainer").width(safeSub(chartWidth, 5));
        if (ie6)
        {
            $("#divTile4 .drillDownContainer, #divTile4 .cloneHeader").width($("#divTile3").width());
            $("#divTile4 .gridClone").css("margin-right", "17px");
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
  
    //Resize headers for Table 1, 2, 3 and detailed grid
//    resizeStaticHeaders('ctl00_tile3_chart_grid1_drillDownContainer');
//    resizeStaticHeaders('ctl00_tile3_chart_grid2_drillDownContainer');
//    resizeStaticHeaders('ctl00_tile3_chart_grid3_drillDownContainer');
//    resizeStaticHeaders('drillDownContainer');

    $("#ctl00_tile3_chart_grid1_drillDownContainer").height($("#gridContainer1").height() - $("#ctl00_tile3_chart_grid1_drillDownContainer_gridCloneDiv").height());
    $("#ctl00_tile3_chart_grid2_drillDownContainer").height($("#gridContainer2").height() - $("#ctl00_tile3_chart_grid2_drillDownContainer_gridCloneDiv").height());
    $("#ctl00_tile3_chart_grid3_drillDownContainer").height($("#gridContainer3").height() - $("#ctl00_tile3_chart_grid3_drillDownContainer_gridCloneDiv").height());

    //Fix for summary grid width in IE6 & IE7
    if (ie6 || ie7)
    {
        //yeah, we did this already (above) but do it again anyway - makes bad browsers happy :-)
        $("#gridContainer1 .drillDownContainer").height($("#chart1").height() - $("#chartContainer1").height() - $("#gridContainer1 .cloneHeaderFull").height() - $("#chart1 .tileContainerHeader").height());
        $("#gridContainer2 .drillDownContainer").height($("#chart2").height() - $("#chartContainer2").height() - $("#gridContainer2 .cloneHeaderFull").height() - $("#chart2 .tileContainerHeader").height());
        $("#gridContainer3 .drillDownContainer").height($("#chart3").height() - $("#chartContainer3").height() - $("#gridContainer3 .cloneHeaderFull").height() - $("#chart3 .tileContainerHeader").height());
        //

        if ($("#ctl00_tile3_chart_grid1_drillDownContainer").height() > ($("#ctl00_tile3_chart_grid1_drillDownContainer").height() + $("#ctl00_tile3_chart_grid1_drillDownContainer_gridCloneDiv").height()))
            $("#ctl00_tile3_chart_grid1_drillDownContainer").width($("#ctl00_tile3_chart_grid1_drillDownContainer").width() - 16);

        if ($("#ctl00_tile3_chart_grid2_drillDownContainer").height() > ($("#ctl00_tile3_chart_grid2_drillDownContainer").height() + $("#ctl00_tile3_chart_grid2_drillDownContainer_gridCloneDiv").height()))
            $("#ctl00_tile3_chart_grid2_drillDownContainer").width($("#ctl00_tile3_chart_grid2_drillDownContainer").width() - 16);

        if ($("#ctl00_tile3_chart_grid3_drillDownContainer").height() > ($("#ctl00_tile3_chart_grid3_drillDownContainer").height() + $("#ctl00_tile3_chart_grid3_drillDownContainer_gridCloneDiv").height()))
            $("#ctl00_tile3_chart_grid3_drillDownContainer").width($("#ctl00_tile3_chart_grid3_drillDownContainer").width() - 16);
    }


    $("#chartLeft1.maximized, #chartLeft2.maximized, #chartRight.maximized").each(function()
    {
        maxMrkt($(this).attr("_chartidx"), null);
    });

    if ($("#tile4.maximized").length == 0)
    {
        //        $("#tile4 #divTile4Container").css({
        //            //height: timeFrameHeight, textAlign: "center", width: "auto", overflow: "hidden"
        //            height: safeSub(safeSub(divHeight, 131), (timeFrameHeight + $("#tile3 #divTile3Container").height())) + 14, textAlign: "center", width: "auto", overflow: "hidden"
        //        });

        $("#tile4 #divTile4").css({
            height: (safeSub(safeSub(divHeight, 131), (timeFrameHeight + $("#tile3 #divTile3Container").height())) - 12), textAlign: "center"
        });

        //Make the drill down grid a bit higher for formularyhistoryreporting since the timeframe filters are not visible
        //if (clientManager.get_Module() == 'formularyhistoryreporting')
            $("#tile4 #divTile4").height($("#tile4 #divTile4").height() + 5);

        $("#drillDownContainer").height($("#divTile4").height() - $("#drillDownContainer_gridCloneDiv").height());
    }

}
function getCustomMrktChartCount()
{
    var chartCount = 1;
    var territoryVisible = false;

    if ($("#ctl00_Tile3_chart_grid2_gridTemplate").length > 0)
    {
        chartCount++;
        territoryVisible = true;
    }

    if ($("#ctl00_Tile3_chart_grid3_gridTemplate").length > 0)
    {
        chartCount++;
        if (territoryVisible == false)
            chartCount = chartCount + 2;
    }

    return chartCount;
}

