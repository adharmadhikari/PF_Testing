/// <reference name="MicrosoftAjax.js"/>
/// <reference path="~/content/scripts/jquery-1.4.1-vsdoc.js"/>
/// <reference path="~/content/scripts/ui.js"/>
/// <reference path="~/content/scripts/clientManager.js"/>
/// <reference path="~/controls/map/include/fmasapi.js"/>

//Custom Segments
function OpenPlanInfo(LinkClicked, PlanID) {
    var oManager = GetRadWindowManager();
    var grid = clientManager.get_PlanInfoGrid();
    var str = "";
    var channel = clientManager.get_Channel();

          if (LinkClicked == "DelPlan") {
             str = "./custom/millennium/todaysaccounts/all/DeletePlan.aspx?LinkClicked=" + LinkClicked;
              var q;
 q = clientManager.getSelectionDataForPostback() + "&PlanName=" + encodeURIComponent(grid.get_masterTableView().get_selectedItems()[0]._dataItem.Plan_Name) + "&PlanID=" + (grid.get_masterTableView().get_selectedItems()[0]._dataItem.Plan_ID);
     }
         else {
    if (channel == 105)
        str = "./custom/millennium/todaysaccounts/all/AddEditPlanInfoMJ.aspx?LinkClicked=" + LinkClicked;
    if (channel == 106)
        str = "./custom/millennium/todaysaccounts/all/AddEditPlanInfoSS.aspx?LinkClicked=" + LinkClicked;
    if (channel == 107)
        str = "./custom/millennium/todaysaccounts/all/AddEditPlanInfoVAR.aspx?LinkClicked=" + LinkClicked;
    if(channel==108)
        str = "./custom/millennium/todaysaccounts/all/AddEditPlanInfoRTA.aspx?LinkClicked=" + LinkClicked;
    var q;
    if (LinkClicked != "AddPlan") {
       
        q = clientManager.getSelectionDataForPostback() + "&PlanName=" + encodeURIComponent(grid.get_masterTableView().get_selectedItems()[0]._dataItem.Plan_Name);
    }
    else { 
     if (channel == 105)
         q = clientManager.getSelectionDataForPostback() + "&PlanName=" + encodeURIComponent("Medicare Jurisdiction");
 if (channel == 106)
     q = clientManager.getSelectionDataForPostback() + "&PlanName=" + encodeURIComponent("State Societies");
     if (channel == 107)
         q = clientManager.getSelectionDataForPostback() + "&PlanName=" + encodeURIComponent("VA Records");
     if (channel == 108)
     q = clientManager.getSelectionDataForPostback() + "&PlanName=" + encodeURIComponent("RTA Records");
    }
}
    str = str + "&" + q;

    var oWnd = radopen(str, LinkClicked);
    oWnd.setSize(960, 370);
    oWnd.Center();
}
//Customer Contact Reports
Pathfinder.UI.MillenniumCustomerContactReportsApplication = function(id) {
    Pathfinder.UI.MillenniumCustomerContactReportsApplication.initializeBase(this, [id]);
};
Pathfinder.UI.MillenniumCustomerContactReportsApplication.prototype =
{
get_ModuleOptionsUrl: function(clientManager) {
        if (clientManager.get_Module() != "customercontactreport")
            return this.getUrl(clientManager.get_Module(),null, clientManager.get_Module() + "_filters.aspx", false);
        else
            return null;
    }
};
Pathfinder.UI.MillenniumCustomerContactReportsApplication.registerClass("Pathfinder.UI.MillenniumCustomerContactReportsApplication", Pathfinder.UI.CustomerContactReportsApplication);



//Business Planning business rules ---------------------------------------------------------------------------------------------------------------------------------------------
Pathfinder.UI.MillenniumExecutiveReportsApplication = function(id)
{
    Pathfinder.UI.MillenniumExecutiveReportsApplication.initializeBase(this, [id]);
};
Pathfinder.UI.MillenniumExecutiveReportsApplication.prototype =
{
    get_UrlName: function() { return "custom/" + this.get_clientKey() + "/executivereports"; },

    get_Title: function() { return "Reports"; },

    getUrl: function(channelName, module, pageName, hasData, isCustom)
    {

        channelName = "all";
        //All reports should be shown on click since the Report Viewer control is now used
        //for showing the report directly for these modules i.e. custom_coveredlivesreportNAM etc
        //if (module == "custom_coveredlivesreport" || module == "custom_formularycoveragereportnam" || module == "custom_formularycoveragereportfam")
        //{
        hasData = true;
        //}
        return Pathfinder.UI.MillenniumExecutiveReportsApplication.callBaseMethod(this, 'getUrl', [channelName, module, pageName, hasData, isCustom]);
    },
    get_OptionsServiceUrl: function(clientManager)
    {
        return "custom/millennium/executivereports/services/PathfinderDataService.svc" + "/GetExecutiveReportsOptions";
    },

    getDefaultModule: function(clientManager)
    {
        return "custom_coveredlivesreport";
    },

    get_ModuleOptionsUrl: function(clientManager)
    {
        //Filters are no longer needed since the Report Viewer control is now used    
        //if (clientManager.get_Module() == "custom_coveredlivesreport" || clientManager.get_Module() == "custom_coveredlivesreportnam" || clientManager.get_Module() == "custom_coveredlivesreportfam" || clientManager.get_Module() == "custom_formularycoveragereportnam" || clientManager.get_Module() == "custom_formularycoveragereportfam")
        return null;
        //else
        //    return this.getUrl("all", null, "filters.aspx", false);
    },

    activate: function(clientManager)
    {
        if (!this._moduleChangingDelegate)
        {
            this._moduleChangingDelegate = Function.createDelegate(this, this._moduleChanging);
            clientManager.add_moduleChanging(this._moduleChangingDelegate);
        }

        Pathfinder.UI.MillenniumExecutiveReportsApplication.callBaseMethod(this, "activate", [clientManager]);      
    },

    onModuleChanging: function(cm, newModule)
    {
        if (cm.get_UIReady())
        {
            cm.clearSelectionData(true);

            //Clear report pager
            //if (ReportPager.get_reportCombinations())
            //    ReportPager.resetReportPager();
        }
    },

    _moduleChanging: function(sender, args)
    {
        this.onModuleChanging(sender, args.get_Value());
    },

    resize: function()
    {
        standardreports_content_resize();

        //Resize report viewer
        //setTimeout("$('#reportviewerframe').contents().find('#ReportViewer1_ctl10').height($('#divTile3').height() - $('#reportviewerframe').contents().find('#ReportViewer1_ctl06').height() - 2);", 2500);
       Pathfinder.UI.MillenniumExecutiveReportsApplication.callBaseMethod(this, 'resize');
    },

    resizeSection: function()
    {
        standardreports_section_resize();
        if ($.browser.version == "7.0")
        {
            $(".rmLink").css({ width: "195px" });
        }
        Pathfinder.UI.MillenniumExecutiveReportsApplication.callBaseMethod(this, 'resizeSection');
    }

};


Pathfinder.UI.MillenniumExecutiveReportsApplication.registerClass("Pathfinder.UI.MillenniumExecutiveReportsApplication", Pathfinder.UI.BasicApplication);

//Todays Accounts business rules ---------------------------------------------------------------------------------------------------------------------------------------------
Pathfinder.UI.MillenniumCustomSegmentsApplication = function(id)
{
    Pathfinder.UI.MillenniumCustomSegmentsApplication.initializeBase(this, [id]);

    this._channelChangedDelegate = null;
    this._previousChannel = null;
    this._resetChannel = false;
    this._rebindPlanInfoGrid = true;

    this._previousRegion = null;
    this._initialActivation = false;

    this._defaultModule = "planinformation";
};
Pathfinder.UI.MillenniumCustomSegmentsApplication.prototype =
{
    get_UrlName: function()
    {
        return "todaysaccounts";
    },

    getDefaultChannel: function() {
        // sl 1/4/2013 as default, 108 (RTA Records)
        return this._previousChannel ? this._previousChannel : 108;
       // return this._previousChannel ? this._previousChannel : 105;
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

        Pathfinder.UI.MillenniumCustomSegmentsApplication.callBaseMethod(this, 'initialize', [clientManager]);
    },

    dispose: function()
    {
        delete (this._channelChangedDelegate);

        Pathfinder.UI.MillenniumCustomSegmentsApplication.callBaseMethod(this, 'dispose');
    },

    activate: function(clientManager)
    {

        this._resetChannel = (clientManager.get_EffectiveChannel() == 0); //global flag gets reset later so keep local value

        var resetChannel = (this._resetChannel || clientManager.get_EffectiveChannel() > 100); //if effective channel is still zero when returning to TA then we need to reset to something

        Pathfinder.UI.MillenniumCustomSegmentsApplication.callBaseMethod(this, 'activate', [clientManager]);

        if (this._channelChangedDelegate == null)
        {
            this._channelChangedDelegate = Function.createDelegate(this, this._channelChanged);
            clientManager.add_channelChanged(this._channelChangedDelegate);
        }
        // sl 1/4/2013 as default, 108 (RTA Records)
        clientManager.set_Channel("108");
        //clientManager.set_Channel("105");
        
        
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
        Pathfinder.UI.MillenniumCustomSegmentsApplication.callBaseMethod(this, 'deactivate', [clientManager]);
    },

    configureSection: function()
    {
        if (!$get("divTile5Max"))
            $("#section2 .enlarge").show();
    },

    resize: function()
    {
        todaysaccounts_content_resize();

        Pathfinder.UI.MillenniumCustomSegmentsApplication.callBaseMethod(this, 'resize');
    },

    resizeSection: todaysaccounts_section_resize,

    resizeModal: function()
    {
        Pathfinder.UI.MillenniumCustomSegmentsApplication.callBaseMethod(this, 'resizeModal');

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
        var channel = clientManager.get_Channel()[0]; //clientManager.get_EffectiveChannel();
        var application = clientManager.get_Application();
        var region = clientManager.get_Region();

        //var channelChanged = (this._previousChannel != channel);
        var channelChanged = this._rebindPlanInfoGrid;
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

            this._previousChannel = channel;
            this._previousRegion = region;

            //if channel is changed or application is changed
            if (!this._resetChannel || applicationChanged)
            {
                var application = clientManager.get_Application();
                var channel = clientManager.get_Channel();
                
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
        Pathfinder.UI.MillenniumCustomSegmentsApplication.callBaseMethod(this, 'configureDashboardTiles', [clientManager]);

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

        Pathfinder.UI.MillenniumCustomSegmentsApplication.callBaseMethod(this, 'configureModuleMenu', [clientManager, channel, currentModule]);

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
Pathfinder.UI.MillenniumCustomSegmentsApplication.registerClass("Pathfinder.UI.MillenniumCustomSegmentsApplication", Pathfinder.UI.Application);

