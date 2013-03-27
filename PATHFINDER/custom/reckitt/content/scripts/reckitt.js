/// <reference name="MicrosoftAjax.js"/>
/// <reference path="~/content/scripts/jquery-1.4.1-vsdoc.js"/>
/// <reference path="~/content/scripts/ui.js"/>
/// <reference path="~/content/scripts/clientManager.js"/>
/// <reference path="~/controls/map/include/fmasapi.js"/>


//Business Planning business rules ---------------------------------------------------------------------------------------------------------------------------------------------
Pathfinder.UI.ReckittBusinessPlanningApplication = function(id) {
    Pathfinder.UI.ReckittBusinessPlanningApplication.initializeBase(this, [id]);
};
Pathfinder.UI.ReckittBusinessPlanningApplication.prototype =
{
    get_UrlName: function() { return "custom/" + this.get_clientKey() + "/businessplanning"; },

    get_Title: function() { return "Business Planning"; },

    getUrl: function(channelName, module, pageName, hasData, isCustom) {

        channelName = "all";

        return Pathfinder.UI.ReckittBusinessPlanningApplication.callBaseMethod(this, 'getUrl', [channelName, module, pageName, hasData, isCustom]);
    },
    get_OptionsServiceUrl: function(clientManager) {
        return this.get_ServiceUrl() + "/GetBusinessPlanningOptions";
    },

    getDefaultModule: function(clientManager) {
        return "businessplanning";
    },

    get_ModuleOptionsUrl: function(clientManager) {
        //if (clientManager.get_Module() == "businessplanning")
        return this.getUrl("all", null, "filters.aspx", false);
        //        else
        //            return null;
    },

    resize: function() {
        standardreports_content_resize();


        $(".topSection").height(($("#tile2").height() * .5) - 30);

        $("#divTile9Container").height($(".topSection").height());

        $("#divTile8Container").height($(".topSection").height());
        $("#ctl00_Tile3_pnlCoveredLives").height($("#divTile8Container").height() - $("#tile8ContainerHeader").height());
        $("#ctl00_Tile3_pnlStateMedicaid").height($("#divTile8Container").height() - $("#tile8ContainerHeader").height());

        $("#divTile7Container").height($(".topSection").height());
        $("#kcView").height($("#divTile7Container").height() - $("#divTile7ContainerHeader").height());

        $("#divTile6Container").height(($("#tile2").height() * .5) - 17);

        $("#divTile6").height($("#divTile6Container").height() - $("#divTile6ContainerHeader").height());

        Pathfinder.UI.ReckittBusinessPlanningApplication.callBaseMethod(this, 'resize');
    },

    resizeSection: function() {
        standardreports_section_resize();
        var browserWindow = $(window);
        var divHeight = browserWindow.height();
        var divWidth = browserWindow.width();

        var fullWidth = safeSub(divWidth, ($get("divTile3") ? Sys.UI.DomElement.getBounds($get("divTile3")).x + 16 : 0));
        var pwidth = Math.round(fullWidth * .99);

        $(".topSection").height(($("#tile2").height() * .5) - 30);

        $("#divTile9Container").height($(".topSection").height());

        $("#divTile8Container").height($(".topSection").height());

        $("#ctl00_Tile3_pnlCoveredLives").height($("#divTile8Container").height() - $("#tile8ContainerHeader").height());
        $("#ctl00_Tile3_pnlStateMedicaid").height($("#divTile8Container").height() - $("#tile8ContainerHeader").height());

        $("#divTile7Container").height($(".topSection").height());

        $("#kcView").height($("#divTile7Container").height() - $("#divTile7ContainerHeader").height());

        $("#divTile6Container").height(($("#tile2").height() * .5) - 17);

        $("#divTile6").height($("#divTile6Container").height() - $("#divTile6ContainerHeader").height());

        if (ie7) {
            $("#ctl00_Tile3_frmBPAccountSummary").width(pwidth - 7);
        }
        var tileHeight = (divHeight * .5) - 120;
        if (ie6) {
            $(".topSection").height(tileHeight);
            $("#divTile9Container").height(tileHeight);

            $("#divTile8Container").height(tileHeight);

            $("#ctl00_Tile3_pnlCoveredLives").height(tileHeight - $("#tile8ContainerHeader").height());
            $("#ctl00_Tile3_pnlStateMedicaid").height(tileHeight - $("#tile8ContainerHeader").height());

            $("#divTile7Container").height(tileHeight);

            $("#kcView").height(tileHeight - $("#divTile7ContainerHeader").height());

            $("#divTile6Container").height((divHeight * .5) - 30).width(pwidth + 6);
            $("#ctl00_Tile3_frmBPAccountSummary").width(pwidth - 9);

            $("#divTile6").height((divHeight * .5) - 30 - $("#divTile6ContainerHeader").height()).width(pwidth);
        }


        Pathfinder.UI.ReckittBusinessPlanningApplication.callBaseMethod(this, 'resizeSection');
    }

};
Pathfinder.UI.ReckittBusinessPlanningApplication.registerClass("Pathfinder.UI.ReckittBusinessPlanningApplication", Pathfinder.UI.BasicApplication);

Pathfinder.UI.ReckittBaseApplication = function(id)
{
    Pathfinder.UI.ReckittBaseApplication.initializeBase(this, [id]);
};

Pathfinder.UI.ReckittBaseApplication.prototype =
{
 get_ModuleMenu: function() { return null; }, 

    configureDashboardTiles: function(clientManager)
    {
        $(".todaysAccounts1").hide();

        Pathfinder.UI.ReckittBaseApplication.callBaseMethod(this, 'configureDashboardTiles', [clientManager]);
    },
    
    resetDashboardTiles: function(clientManager)
    {
        $(".todaysAccounts1").show();
        
        Pathfinder.UI.ReckittBaseApplication.callBaseMethod(this, 'resetDashboardTiles', [clientManager]);
    },

    resize: function()
    {
        var browserWindow = $(window);
        var divHeight = browserWindow.height();
        var divWidth = browserWindow.width();
    
        $("#tile2").hide();
        
        $(".RadWindow table").height("100%");
        $("#RadWindowWrapper_modal").css({
            height: divHeight - 150, width: divWidth / 1.05
        }
        , animationSpeed);

        this.resizeSection();
        
        Pathfinder.UI.ReckittBaseApplication.callBaseMethod(this, 'resize');
    },

    resizeSection: function()
    {
        var height = getWorkspaceHeight();

        $("#divTile3, #divTile3Container").height(height);

        $("#divTile3").height(height - $("#divTile3Container .tileContainerHeader").height() - 45);    
    }
};
Pathfinder.UI.ReckittBaseApplication.registerClass("Pathfinder.UI.ReckittBaseApplication", Pathfinder.UI.BasicApplication);



//OTC Coverage Business Rules --------------------------------------------------------------------------------------------------------------------------------------------
Pathfinder.UI.ReckittOTCCoverageApplication = function(id)
{
    Pathfinder.UI.ReckittOTCCoverageApplication.initializeBase(this, [id]);
};

Pathfinder.UI.ReckittOTCCoverageApplication.prototype =
{
    get_UrlName: function() { return "custom/" + this.get_clientKey() + "/otccoverage"; },

    get_Title: function() { return "OTC Coverage"; }   
};
Pathfinder.UI.ReckittOTCCoverageApplication.registerClass("Pathfinder.UI.ReckittOTCCoverageApplication", Pathfinder.UI.ReckittBaseApplication);

//functions used for Business planning
function OpenGoalTactics(hiddenfield)
{
    var BusinessPlanID = "0";
    var oManager = GetRadWindowManager();
    if (document.getElementById(hiddenfield) != null) {
        BusinessPlanID = document.getElementById(hiddenfield).value;
    }

    var str = "GoalTactics.aspx?Business_Plan_ID=" + BusinessPlanID;
    var q = clientManager.getSelectionDataForPostback();

    str = str + "&" + q;

    $openWindow(clientManager.getUrl(str), null, null, 800, 495,"goalsTactics");
}
function EditLinkClick() 
{    
    $("#ctl00_Tile3_frmBPAccountSummary_linkEditBusinessPlan").click();
    $("#ctl00_Tile3_linkEditKC").click();
}
function UpdateData() {
    //remove special characters
//    $("#ctl00_Tile3_frmBPAccountSummary_Acct_Summary").val(specialCharCleanup($("#ctl00_Tile3_frmBPAccountSummary_Acct_Summary").val(), false));
//    $("#ctl00_Tile3_frmBPAccountSummary_NO_PT_Explanation").val(specialCharCleanup($("#ctl00_Tile3_frmBPAccountSummary_NO_PT_Explanation").val(), false));
//    $("#ctl00_Tile3_frmBPAccountSummary_NA_Schedule_Period").val(specialCharCleanup($("#ctl00_Tile3_frmBPAccountSummary_NA_Schedule_Period").val(), false));
//    $("#ctl00_Tile3_frmBPAccountSummary_O_Other_Explanation").val(specialCharCleanup($("#ctl00_Tile3_frmBPAccountSummary_O_Other_Explanation").val(), false));
   //update
    $("#ctl00_Tile3_frmBPAccountSummary_linkUpdateAccountSummary").click();
    $("#ctl00_Tile3_linkUpdateKC").click();
}
function exportPDF(planID, segID, hdnBPID) {
    var bpID = document.getElementById(hdnBPID).value;  
     var urlvalueNew = "";
     if (bpID != null) 
    {
        urlvalueNew = "custom/reckitt/businessplanning/all/export.aspx?Plan_ID=" + planID + "&Segment_ID=" + segID + "&Business_Plan_ID=" + bpID ;
        var url = "custom/reckitt/businessplanning/services/ReckittDataService.svc/PlanListSet?$filter=ID eq " + planID;
        $.getJSON(url, null, function(result, status) {
            var d = result.d;
            window.location = urlvalueNew + "&Plan_Name=" + d[0].Name; ;
        });        
    }   
}

function LoadPlanList(sender, args)
{

    var val = sender.get_value();
    
    if (val == 0) val = null;
    var url = "custom/reckitt/businessplanning/services/ReckittDataService.svc/PlanListSet?$filter=Section_ID eq " + val + "&$orderby=Name";
    $.getJSON(url, null, function(result, status) {
        var d = result.d;
        $loadListItems($find('ctl00_partialPage_filtersContainer_BusinessPlanCreation_Plan_ID'), d, !d.length?{ value: "", text: "no records available" }:null);


        //after list is loaded check if current Plan ID can be selected
        var data = clientManager.get_SelectionData();
        var planID;
        if (data && data["Plan_ID"]) planID = data["Plan_ID"].value;
        if (planID) {
            var li = $find('ctl00_partialPage_filtersContainer_BusinessPlanCreation_Plan_ID').findItemByValue(planID);
            if (li) li.select();
        }
        
    });
}
//function specialCharCleanup(specialchartext,varPreservechar) {
//    specialchartext = escape(specialchartext);
//          
//    if (varPreservechar == true) {
//        specialchartext = specialchartext.replace(/%u201C/g, "&ldquo;");
//        specialchartext = specialchartext.replace(/%u201D/g, "&rdquo;");
//        specialchartext = specialchartext.replace(/%u2018/g, "&lsquo;");
//        specialchartext = specialchartext.replace(/%u2019/g, "&rsquo;");
//        specialchartext = specialchartext.replace(/%u2026/g, "&hellip;");
//    }
//    else {
//        specialchartext = specialchartext.replace(/%u201C/g, "\"");
//        specialchartext = specialchartext.replace(/%u201D/g, "\"");
//        specialchartext = specialchartext.replace(/%u2018/g, "'");
//        specialchartext = specialchartext.replace(/%u2019/g, "'");
//        specialchartext = specialchartext.replace(/%u2026/g, "...");
//    }
//    specialchartext = specialchartext.replace(/%u2013/g, "&ndash;");
//    specialchartext = specialchartext.replace(/%u2014/g, "&mdash;");
//    specialchartext = specialchartext.replace(/%A9/g, "&copy;");
//    specialchartext = specialchartext.replace(/%AE/g, "&reg;");
//    specialchartext = specialchartext.replace(/%u2122/g, "&trade;");
//    specialchartext = specialchartext.replace(/%0A/g, "\r\n");
//    specialchartext = unescape(specialchartext);
//    return specialchartext;
//}
//Customer Contact report Business Rules --------------------------------------------------------------------------------------------------------------------------------------------
Pathfinder.UI.ReckittCustomerContactReportsApplication = function(id) {
    Pathfinder.UI.ReckittCustomerContactReportsApplication.initializeBase(this, [id]);
};

Pathfinder.UI.ReckittCustomerContactReportsApplication.registerClass("Pathfinder.UI.ReckittCustomerContactReportsApplication", Pathfinder.UI.CustomerContactReportsApplication);

//--------------------------------------------------------------------------------------------------------------------------------------------

//custom segment ---------------------------------------------------------------------------------------------------------------------------------------------
Pathfinder.UI.ReckittCustomSegmentsApplication = function(id)
{
    Pathfinder.UI.ReckittCustomSegmentsApplication.initializeBase(this, [id]);

    this._channelChangedDelegate = null;
    this._previousChannel = null;
    this._resetChannel = false;
    this._rebindPlanInfoGrid = true;

    this._previousRegion = null;
    this._initialActivation = false;

    this._defaultModule = "planinformation";
};
Pathfinder.UI.ReckittCustomSegmentsApplication.prototype =
{
    get_UrlName: function()
    {
        return "todaysaccounts";
    },

    getDefaultChannel: function()
    {
        return this._previousChannel ? this._previousChannel : 101;
    },

    getDefaultModule: function(clientManager)
    {
        if (clientManager.get_Channel() != 0)
            return this._defaultModule;

        return null;
    },

    initialize: function(clientManager)
    {
        //if drug controls not enabled then don't send drug id when requesting map data for TA
        //        if (!clientManager.get_MarketBasketList())
        this.mapDataRequestUIStateProperties = ["Application", "Channel", "Geography", "Region"];

        Pathfinder.UI.ReckittCustomSegmentsApplication.callBaseMethod(this, 'initialize', [clientManager]);
    },

    dispose: function()
    {
        delete (this._channelChangedDelegate);

        Pathfinder.UI.ReckittCustomSegmentsApplication.callBaseMethod(this, 'dispose');
    },

    activate: function(clientManager)
    {

        this._resetChannel = (clientManager.get_EffectiveChannel() == 0); //global flag gets reset later so keep local value

        var resetChannel = (this._resetChannel); //if effective channel is still zero when returning to TA then we need to reset to something

        Pathfinder.UI.ReckittCustomSegmentsApplication.callBaseMethod(this, 'activate', [clientManager]);

        if (this._channelChangedDelegate == null)
        {
            this._channelChangedDelegate = Function.createDelegate(this, this._channelChanged);
            clientManager.add_channelChanged(this._channelChangedDelegate);
        }

        clientManager.set_Channel("101");
        //Only try to refresh grid if TA was activated once already or map is initialized (dashboard may have loaded to another app first)
        //        if (this._initialActivation || clientManager.get_MapIsReady())
        //        {
        this._refreshPlanInfoGrid(clientManager);
        //        }

        this._initialActivation = true;

        if (!ensureMapIsLoaded(clientManager.get_CurrentMapUIStateAsText()))
        {
            if (!clientManager.get_MapIsReady())
                new cmd(null, initMap, ["fmASEngine", clientManager.get_CurrentMapUIStateAsText()], 500);
            else if (!resetChannel) //if resetting channel then don't need to reload map data manually
                clientManager.mapReloadData();
        }
        //$('#ctl00_main_subheader1_channelMenu').show().css('display', 'inline-block');
        //Show the channel menu
        if (clientManager) clientManager.get_ChannelMenu().set_visible(true);

        $("#tile2 .enlarge").show();
        $("#tile2 .min").hide();
    },

    deactivate: function(clientManager)
    {
        if (flashSupported)
        {
            fmPOIsHideCategory("visn");
            fmPOIsHideCategory("msa");
        }

        this._previousChannel = clientManager.get_EffectiveChannel();
        this._previousRegion = clientManager.get_Region();

        clientManager.remove_channelChanged(this._channelChangedDelegate);
        delete (this._channelChangedDelegate);
        $("#planInfoLegend").show();
        $('#ctl00_main_subheader1_channelMenu').hide();
        $("#addplanOpt").hide();
        Pathfinder.UI.ReckittCustomSegmentsApplication.callBaseMethod(this, 'deactivate', [clientManager]);
    },

    configureSection: function()
    {
        if (!$get("divTile5Max"))
            $("#section2 .enlarge").show();
    },

    resize: function()
    {
        todaysaccounts_content_resize();

        Pathfinder.UI.ReckittCustomSegmentsApplication.callBaseMethod(this, 'resize');
    },

    resizeSection: todaysaccounts_section_resize,

    resizeModal: function()
    {
        Pathfinder.UI.ReckittCustomSegmentsApplication.callBaseMethod(this, 'resizeModal');

        $(".RadWindow iframe").each(function()
        {
            this.contentWindow.$("#ctl00_main_gridContacts_GridData").css({ height: $(this.contentWindow).height() - 73, overflow: "auto" });
        });
    },

    getDefaultData: function(clientManager)
    {
        ///<summary>Returns default data for Today's account which is the last selected row in the plan info grid.  If no rows are selected or available null is returned.</summary>

        var mt = clientManager.get_PlanInfoGrid().get_masterTableView();
        if (mt.get_virtualItemCount() > 0)
        {
            var items = mt.get_selectedItems();
            var data = {};
            if (items != null && items.length > 0)
            {
                var names = mt.get_clientDataKeyNames();

                for (var i = 0; i < names.length; i++)
                {
                    data[names[i]] = items[0].get_dataItem()[names[i]];
                }

                return data;
            }
        }

        return null;
    },

    _refreshPlanInfoGrid: function(clientManager)
    {
        //var channel = clientManager.get_Channel()[0]; //clientManager.get_EffectiveChannel();
        var channel = clientManager.get_Channel();
        var application = clientManager.get_Application();
        var region = clientManager.get_Region();

        //var channelChanged = (this._previousChannel != channel);
        var channelChanged = this._rebindPlanInfoGrid;
        if (channelChanged == null) channelChanged = (this._previousChannel != channel);
        //var applicationChanged = (Sys.Serialization.JavaScriptSerializer.deserialize(clientManager._history[clientManager._currentHistoryIndex], true)["Application"] != application)
        var state = clientManager._history[clientManager._currentHistoryIndex];
        if (typeof (state) == "undefined") state = clientManager.get_CurrentUIStateAsText();
        var applicationChanged = (Sys.Serialization.JavaScriptSerializer.deserialize(state, true)["Application"] != application)

        //if channel and region is same as when app deactivated then no need to refresh - just return
        if (!channelChanged && this._previousRegion == region && !applicationChanged)
        {
            this.configureModuleMenu(clientManager, channel, clientManager.get_Module());
        }
        else
        {
            //sl 4/5/2012 to avoid filter error: previous selected filter is not properly cleared
            if (channelChanged)
                clientManager.resetPlans();


            var channelToReset = this.getDefaultChannel();

            this._previousChannel = clientManager.get_EffectiveChannel(); //channel;
            this._previousRegion = region;

            //if channel is changed or application is changed
            if (!this._resetChannel || applicationChanged)
            {
                var application = clientManager.get_Application();


                $("#addplanOpt").show();


                var grid = clientManager.get_PlanInfoGrid();
                if (grid)
                {
                    var mt = grid.get_masterTableView();
                    if (channel == 0) channel = null;
                    $setGridFilter(grid, "Section_ID", channel, "EqualTo", "System.Int32");
                    $setGridFilter(grid, "Plan_State", region);

                    if (channel != 11) //clear VISN filter if channel is not VISN
                        $clearGridFilter(mt, "VISN");

                    if (clientManager.get_TrackingEnabled())
                        mt.clearSelectedItems();

                    mt.rebind();
                }
            }
            else //if switching from another app that does not support channels such as Standard Reports then set to 1 for Commercial Payers - this function will be recalled because channel is updated but next time it should actually do refresh
                clientManager.set_Channel(channelToReset);
        }

        this._resetChannel = false;

        $("#tileMin1").css("display", "none");
        $("#tileMin2").css("margin-left", "00px");
        $("#planInfoLegend").css("display", "none");

    },

    _channelChanged: function(sender, args)
    {
        this._refreshPlanInfoGrid(sender);
    },


    _getPlanInfoGridTitle: function(basePath)
    {
        ///<summary>Returns the HTML that is displayed in the title section of the Plan Information Grid's tile.</summary>

        return "<a class='advToggle' onClick='toggleArrow();' href='javascript:clientManager.togglePlanInfoGridFilters()'><img class='advToggleArw arwRt' src='" + basePath + "/content/images/spacer.gif' /><img class='advToggleArw arwDw' src='" + basePath + "/content/images/spacer.gif' />Search</a>" +
                "&nbsp;<span id='resetPlans' class='ml10' style='display:none;cursor:pointer;vertical-align:middle;' onclick='clientManager.resetPlans()'>Reset</span>";

    },

    configureDashboardTiles: function(clientManager)
    {
        $("#divTile2Container .title").html(this._getPlanInfoGridTitle(clientManager.get_BasePath()));
        $("#divTile2Container .pagination").show();


        $("#tile1 .enlarge").show();

        $(".tile2Div").hide();

        $("#divTile2Plans").show();

        var opts = $.grep(clientManager.getModuleOptionsByApp(this.get_ApplicationID()), function(i, x) { return i.ID == "contacts"; });
        if (opts && opts.length > 0)
            $("#contactSearchOpt").show();

        $(".myAccts").show();

        //        if(clientManager.get_Channel() > 0)
        //            maxTile1();
        Pathfinder.UI.ReckittCustomSegmentsApplication.callBaseMethod(this, 'configureDashboardTiles', [clientManager]);

        $(".todaysAccounts2, .todaysAccounts1, #tile2").removeAttr("style");
        $("#tile2").height(getTile1Height());
    },

    resetDashboardTiles: function(clientManager)
    {
        $("#contactSearchOpt").hide();
        $(".myAccts").hide();

        if (!$.browser.msie)
            $("#divTile1 object").remove();

        //can't have a #tileMin2 in other apps
        $("#tileMin2").attr({ id: "tile2" });
    },

    configureModuleMenu: function(clientManager, channel, currentModule)
    {
        if (clientManager.get_Channel() == 0)
        {
            if (!this._resetChannel)
            {
                $("#expandTile1 a").attr("disabled", false);
                maxTile1();
                //               $("#expandTile1 a").attr("disabled", true);
                //               minTile1();
            }

            if (channel == 0)
                return; //don't do anything else if All is selected - when Plan is automatically selected the menu will update
        }

        var menu = this.get_ModuleMenu();

        Pathfinder.UI.ReckittCustomSegmentsApplication.callBaseMethod(this, 'configureModuleMenu', [clientManager, channel, currentModule]);

        $(".navbar2").show();

        //Load Module menu for today's accounts
        var map;
        var modules = clientManager.getModuleOptionsByApp(this.get_ApplicationID());
        var module;
        for (var i = 0; i < modules.length; i++)
        {
            module = modules[i];
            if (module.ID != "map") //map is a configurable module but not part of menu
            {
                if (module.Channels[channel] == 1)
                    menu.addItem("link_" + module.ID, module.Name, module.ID);
            }
            else
                map = module.Channels[channel] == 1;
        }
        //

        var firstItem = menu.getItemAt(0);
        if (firstItem)
            this._defaultModule = firstItem.value;

        //cm must be ready and channel should not be 0 otherwise this was called to update menu based on a selection (map should stay hidden)
        //SPH 9/25/09 - Don't think we need condition "clientManager.get_UIReady() && " anymore
        if (clientManager.get_Channel() >= 0)
        {
            if (map)
            {
                $("#expandTile1 a").attr("disabled", true);
                minTile1();
            }
            else
            {
                $("#expandTile1 a").attr("disabled", true);
                minTile1();
            }
        }


        //Check if current module is available for selected Section - If not reset module to first selected option
        if (menu.getItem("link_" + currentModule) != null)
        {
            menu.highlightItem("link_" + currentModule);
            return currentModule;
        }
        else
        {
            var item = menu.getItemAt(0);
            if (item)
            {
                menu.highlightItem(item.id);
                return item.value;
            }
        }
    }
};
Pathfinder.UI.ReckittCustomSegmentsApplication.registerClass("Pathfinder.UI.ReckittCustomSegmentsApplication", Pathfinder.UI.Application);
//end of custom segment
