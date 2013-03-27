
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



$(document).ready(function () {

    //------------REPORTS DASHBOARD----------------//
    //filter sidebar
    $('#districtdrilldownReportfilters').hide();
    $('#pprxReportfilters').show();

    $('.reportsBtn').click(function (event) {
        event.preventDefault();
        $(this).addClass("selected").siblings().removeClass("selected");
        var id = $(this).find("a").attr("id")
        id= id.replace("ctl00_ctl00_main_main_", "")
      $('#' + id + 'filters').siblings().hide();
      $('#' + id +'filters').show();
    });

 
    //$('#ctl00_ctl00_main_main_pprxfilters_btnSubmit').click(function () {
    //    dialogModule.open();
    //});
    //------------------------------------------//

    $(".ui-tabs-nav li:last a").css({
        border: "0px"
    });

    // fixes report tab to highlight as selected when clicking on the district report
    var districtReportsPage = $(".districtsReportsPage").height();
    if (districtReportsPage) {
        $("#mainTab li:last").addClass("selected");
    }

    // adds classes to body tag, needs to be before content_resize 
    var mainClass = $("#mainTab .selected a").text().toLowerCase().replace(/ /g, '');
    var subClass = $("#subTab .selected a").text().toLowerCase().replace(/ /g, '');
    var subClassMods = $("#impactSubmodules .selected a").text().toLowerCase().replace(/ /g, '');
    $("body").addClass(mainClass);
    $("body").addClass(subClass);
    $("body").addClass(subClassMods);

    //end

    function resetGridHeaders() {
        if (mac & safari) {
            //Hack to fix header alignment for rad grids
            setTimeout(function() {
                $('.rgHeaderDiv').css('margin-right', '0px');
                $('.rgFooterDiv').css('margin-right', '0px');
                $('.rgMasterTable').css('width', '99.9%');
            }, 1000);
        }

        if (chrome || (!(mac) && safari) || ie9) {
            setTimeout(function() {
                $('.rgMasterTable').css('width', '98%');
                setTimeout(function() {
                    $('.rgMasterTable').css('width', '100%');
                }, 100);
            }, 100);
        }


    }


    function content_resize() {
        //removes impactSubmodules div if empty
        var emptyImpactSubmodules = $("#impactSubmodules").html();
        if (emptyImpactSubmodules == "") {
            $("#impactSubmodules").remove();
        }
        var browserWindow = $(window);
        var divHeight = browserWindow.height();
        var divWidth = browserWindow.width();
        var headerHeight = $(".header").outerHeight() + $(".navbar").outerHeight() + $("#subTab").outerHeight() + $(".footer").outerHeight() + $("#impactSubmodules").outerHeight() + 20; // plus 20 is for the padding around the contentArea div
        var contentHeight = $(".header").outerHeight() + $(".navbar").outerHeight() + $("#subTab").outerHeight() + $(".footer").outerHeight() + $("#impactSubmodules").outerHeight() + $(".contentArea .tileContainerHeader").outerHeight() + 20; // plus 20 is for the padding around the contentArea div
        var MCOpportunity = $(".MC_Opportunity").outerHeight() - 12;

        var ch = divHeight - headerHeight;
        if (contentHeight < 0) contentHeight = 0;
        $(".contentArea").css({
            height: ch
        });

        $(".allcurrentcampaigns .contentArea .rgDataDiv").css({
            height: divHeight - contentHeight
        });

        $(".topGrid .rgDataDiv").css({
            height: "50px"
        });

        $(".btmGrid .rgDataDiv").css({
            height: divHeight - contentHeight - 258
        });


        $(".currentcampaigns .contentArea .rgDataDiv").css({
            height: divHeight - contentHeight
        });

        //$(".btmGrid .rgDataDiv td table td:first-child, .topGrid .rgDataDiv td table td:first-child, .rightPane .rgDataDiv td table td:first-child").css("border-right", "1px solid #ccc");
        $(".rightPane .rgDataDiv td table td:first-child").css("border-right", "1px solid #ccc");


        $(".campaignresults .btmGrid .rgDataDiv td table td:last-child, .campaignresults .topGrid .rgDataDiv td table td:last-child").css("border-right", "none");
        $(".tactics .rgDataDiv, .campaignsummary .rgDataDiv, .campaignresults .rgDataDiv, .btmGrid .rgDataDiv, .messages .rgDataDiv").removeAttr("style");
        //$(".tactics .rgDataDiv, .campaignsummary .rgDataDiv, .campaignresults .rgDataDiv, .messages .rgDataDiv").removeAttr("style");
        $(".btmGrid .rgDataDiv").css({
            overflowX: "auto",
            overflowY: "auto",
            width: "100%"
        });

        $(".tactics .rgDataDiv, .campaignsummary .rgDataDiv, .campaignresults .rgDataDiv, .messages .rgDataDiv").css({
            overflowX: "hidden",
            overflowY: "auto",
            width: "100%"
        });

        $(".campaignsummary .btmGrid .rgHeaderDiv .rgMasterTable, .campaignsummary .btmGrid .rgDataDiv .rgMasterTable").css({
            paddingRight: "70px"
        });

        $(".campaignsummary .topGrid .rgHeaderDiv .rgMasterTable, .campaignsummary .topGrid .rgDataDiv .rgMasterTable ").css({
            paddingRight: "70px"
        });
        // fixes hidden columns at the end of the scrolling tables on the goals page
        //        $(".campaigngoals .btmGrid .rgMasterTable ").css({
        //            paddingRight: "220px"
        //        });
        $(".campaignresults .btmGrid .rgDataDiv .rgMasterTable ").css({
            paddingRight: "220px"
        });
        //        $(".campaigngoals .topGrid .rgMasterTable ").css({
        //            paddingRight: "160px"
        //        });
        $(".campaignresults .topGrid .rgDataDiv .rgMasterTable ").css({
            paddingRight: "160px"
        });

        // IE6 scroll fix, when a table is inside a div, it pushes the scroll bar beyond the outer div
        $(".ie6 .contentArea:not(.ie6 .campaignopportunities .contentArea)").wrapInner("<div class='ie6WrapperDiv' />");
        if ($(".ie6WrapperDiv").outerHeight() > $(".contentArea").outerHeight()) {
            $(".ie6WrapperDiv").css("width", divWidth - 38);
        }

        resetGridHeaders()


    }

    $(window).wresize(content_resize);
    var contentAreaHeight = $(".contentArea").height();
    content_resize();

    // popup tooltips
    $(".btnInfo").mouseover(function() {
        $("#instructions1").css("display", "block");
    }).mouseout(function() {
        $("#instructions1").css("display", "none");
    });

    $(".btnEmailTeam").mouseover(function() {
        $("#instructions2").css("display", "block");
    }).mouseout(function() {
        $("#instructions2").css("display", "none");
    });


    // reports page left menu adds selected class
   // var detectReportsPage = $("#districtregioniframetable").height();
   // if (detectReportsPage) {
   //     $(".reportsBtn:last").addClass("selected");
    // } else {

    //select first button on page load
    $(".reportsBtn:first").addClass("selected");

   // }

});


/*   
=============================================================================== 
WResize is the jQuery plugin for fixing the IE window resize bug 
............................................................................... 
Copyright 2007 / Andrea Ercolino 
------------------------------------------------------------------------------- 
LICENSE: http://www.opensource.org/licenses/mit-license.php 
WEBSITE: http://noteslog.com/ 
=============================================================================== 
*/

(function($) {
    $.fn.wresize = function(f) {
        version = '1.1';
        wresize = { fired: false, width: 0 };

        function resizeOnce() {
            if ($.browser.msie) {
                if (!wresize.fired) {
                    wresize.fired = true;
                }
                else {
                    var version = parseInt($.browser.version, 10);
                    wresize.fired = false;
                    if (version < 7) {
                        return false;
                    }
                    else if (version == 7) {
                        //a vertical resize is fired once, an horizontal resize twice 
                        var width = $(window).width();
                        if (width != wresize.width) {
                            wresize.width = width;
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        function handleWResize(e) {
            if (resizeOnce()) {
                return f.apply(this, [e]);
            }
        }

        this.each(function() {
            if (this == window) {
                $(this).resize(handleWResize);
            }
            else {
                $(this).resize(f);
            }
        });

        return this;
    };

})(jQuery);
function resizeSplitter() {
    var browserWindow = $(window);
    var divHeight = browserWindow.height();
    var contentHeight = $(".header").outerHeight() + $(".navbar").outerHeight() + $("#subTab").outerHeight() + $(".footer").outerHeight() + $("#impactSubmodules").outerHeight() + $(".contentArea .tileContainerHeader").outerHeight() + 20; // plus 20 is for the padding around the contentArea div
    var contentHeight1 = $(".header").outerHeight() + $(".navbar").outerHeight() + $("#subTab").outerHeight() + $(".footer").outerHeight() + $("#impactSubmodules").outerHeight() + 20; 
    var MCOpportunity = $(".MC_Opportunity").outerHeight() - 12;
    var glidHeaderHeight = $(".rightPane .rgHeaderDiv").outerHeight();
    $(".mycampaigns .MC_Results, .mycampaigns .RadSplitter_impact, .mycampaigns .leftPane ").css({
        height: (divHeight - contentHeight) - MCOpportunity
    });
    $(".mycampaigns .MC_Results .table1 .rgDataDiv   ").css({
    height: (divHeight - contentHeight) - MCOpportunity - glidHeaderHeight
    });
    $(".mycampaigns .rightPane").css({
    height: (divHeight - contentHeight) - MCOpportunity ,
        overflow: "hidden"

    });
    $(".reports .rightPane").css({
        height: divHeight - contentHeight1

    });
    $(".reports .RadSplitter_impact, .reports .leftPane ").css({
        height: divHeight - contentHeight1
    });

}

//Create Iframe Module for inserting Iframe into page
//input div, url, height
//output iframe html into give div, open loading dialog
function insertIframe(div, url, height) {
    //var height = $(".contentArea").height();
    //$("#RAD_SPLITTER_PANE_CONTENT_ctl00_ctl00_main_main_radPaneContent").append("<iframe id='pprxiframetable' src=" + url + " frameborder='0' width='100%' height='" + height + "' scrolling='no'></iframe>");
    $(div).append("<iframe id='reportvieweriframe' src=" + url + " frameborder='0' width='100%' height='" + height + "' scrolling='no'></iframe>");
    $(".contentArea").css("overflow-y", "hidden");
    dialogModule.open();
}


//dialog Module for loading dialog
var dialogModule = (function () {

    var $loadingDialog = $("<div id='loading-dialog'></div>")
        .dialog({
            dialogClass: 'alert',
            autoOpen: false,
            resizable: false,
            draggable: false,
            width: 50,
            modal: true,
        });

    return {
        open: function () {
            $(".ui-dialog-titlebar").hide()
            $loadingDialog.dialog('open');
        },

        close: function () {
            $loadingDialog.dialog('close');
        }
    }


})();