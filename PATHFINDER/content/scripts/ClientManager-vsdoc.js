/// <reference name="MicrosoftAjax.js"/>
/// <reference path="~/content/scripts/jquery-1.4.1-vsdoc.js"/>
/// <reference path="~/content/scripts/ui.js"/>
/// <reference path="~/controls/map/include/fmasapi.js"/>
/// <reference path="~/services/securityservice.svc" />

var clientManager = null; //singleton

var Cursor = { None: "", Default: "default", Pointer: "pointer" };

$addHandler(document.body, "selectstart", function(e, a)
{
    try
    {
        if (e.target.tagName == "BODY" || (e.target.tagName != "INPUT" && e.target.tagName != "TEXTAREA"))
        {
            document.selection.empty();
            e.preventDefault();
            e.stopPropagation();
        }
    }
    catch (ex) { }
});
$addHandler(document, "contextmenu", function(e, a)
{
    try
    {

        if (e.target.tagName != "INPUT" && e.target.tagName != "TEXTAREA" && !$(e.target).parent().hasClass("chartThumb"))
        {
            e.preventDefault();
            e.stopPropagation();

            return false;
        }
    }
    catch (ex) { }
});

var processing = 0;
$(window).ajaxSend(function() { processing++; $(".viewSelect span:first").css({ "color": "#888" }, 500); });
$(window).ajaxComplete(function() { processing--; if (processing < 0) processing = 0; if (processing == 0) $(".viewSelect span:first").css({ "color": "#2d58a8" }, 500); });

$(window).ajaxError(function(a, b, c, msg)
{
    var showAlert = true;

    if (!msg)
    {
        if (c.dataType != "json" || b.responseText.indexOf("{") > 0)
            msg = $("<DIV />").html(b.responseText.replace(/\<STYLE/ig, "<!-- ").replace(/\<\/STYLE\>/ig, "-->").replace(/\<SCRIPT/ig, "<!-- ").replace(/\<\/SCRIPT\>/ig, "-->")).html();
        else//
        {
            try
            {
                msg = Sys.Serialization.JavaScriptSerializer.deserialize(b.responseText, true);

                if (typeof msg == "object" && msg.error && msg.error.message) //deserialize successful
                    msg = msg.error.message.value;
            } catch (ex) { }
        }
    }
    else if (typeof msg == "object" && msg.name == "SyntaxError" && c.dataType == "json") //syntax error would occur if .net framework returned login page due to redirect from timeout
    {                                                                                                                                  //sometimes if invalid json is returned from data service the login page could be incorrectly displayed.
        clientManager.validateCurrentUser();
        showAlert = false;
    }

    if (!msg || typeof msg != "string") //if still no error to handle
        msg = "Unspecified error";

    if (showAlert)
        $alert(msg, "PathfinderRx Error", null, null, Pathfinder.UI.AlertType.Error);
});

Type.registerNamespace("Pathfinder.UI");

Pathfinder.UI.ClientAccount = function(clientKey, options)
{
    this._clientKey = clientKey;
    this._options = {};
    if (options)
    {
        var opts = options.split(",");
        for (var opt in opts)
        {
            this._options[$.trim(opts[opt])] = true;
        }
    }
};
Pathfinder.UI.ClientAccount.prototype =
{
    get_ClientKey: function() { return this._clientKey; },
    set_ClientKey: function(value) { this._clientKey = value; },

    HasOption: function(name)
    {
        return this._options[name] === true;
    }
};
Pathfinder.UI.ClientAccount.registerClass("Pathfinder.UI.ClientAccount");



Pathfinder.UI.Applications = function()
{
    /// <field name="TodaysAccounts" type="Number" integer="true" static="true"></field>
    /// <field name="TodaysAnalytics" type="Number" integer="true" static="true"></field>
    /// <field name="StandardReports" type="Number" integer="true" static="true"></field>
    /// <field name="CustomOptions" type="Number" integer="true" static="true"></field>
    /// <field name="AccessBasedSelling" type="Number" integer="true" static="true"></field>
    if (arguments.length !== 0) throw Error.parameterCount();
    throw Error.notImplemented();
};

Pathfinder.UI.Applications.prototype =
{
    TodaysAccounts: 1,
    TodaysAnalytics: 2,
    StandardReports: 3,
    CustomOptions: 4,
    AccessBasedSelling: 5,
    PowerPlanRx: 6,
    FormularySellSheets: 8,
    CustomerContactReports: 9,
    FormularyHistoryReporting: 14,
    PrescriberReporting: 15,
    ActivityReporting: 16,
    CustomSegments: 20
};
Pathfinder.UI.Applications.registerEnum("Pathfinder.UI.Applications");

Pathfinder.UI.AlertType = function() { };
Pathfinder.UI.AlertType.prototype =
{
    Warning: 1,
    Information: 2,
    Error: 3
};
Pathfinder.UI.AlertType.registerEnum("Pathfinder.UI.AlertType");



Pathfinder.UI.Application = function(id)
{
    ///<summary>Pathfinder.UI.Application is the base class for application management objects used to define user interface business logic implemented by ClientManager.  Each Pathfinder application is required to have a definition to implement its logic.  At minimum get_UrlName must be overriden so clientManager can map to the application's files.</summary>
    this._clientKey = null;
    this._id = id;

    this.mapDataRequestUIStateProperties = ["Application", "Channel", "Drug", "Geography"];
};

Pathfinder.UI.Application.instances = {};

Pathfinder.UI.Application.getInstance = function(clientManager, id, autoActivate)
{
    ///<summary>Creates a Pathfinder.UI.Application based object determined by the specified application id.</summary>
    ///<param name="clientManager" type="Object">A ClientManager instance is required to support initialization and activation of the requested application.</param>
    ///<param name="id" type="Numeric">Application ID</param>
    ///<param name="autoActivate" type="Boolean" optional="true">Determines if the application's "activate" method should be called prior to returning the instance to the caller.</param>
    ///<returns type="Pathfinder.UI.Application"></returns>

    var applicationInstance = Pathfinder.UI.Application.instances[id];

    if (applicationInstance == null)
    {
        id = parseInt(id, 10);

        var msg = "Invalid application id: " + id;
        var appOpt = $.grep(clientManager.get_ApplicationMenuOptions(), function(x, i) { return x.ID == id; })[0];
        if (appOpt)
        {
            if (appOpt.Name)
            {
                //for now construct the class name from app name by stripping out special chars - this should be sufficient but can cause errors if app name changes - possibly use a newly defined app value in future (for now leave as is)

                var clientKey = clientManager.get_ClientKey();
                var clientInstance = (appOpt.Custom && clientKey != "pinso" ? clientKey.substr(0, 1).toUpperCase() + clientKey.substr(1) : "");
                var app = clientInstance + appOpt.App_Folder.replace(/\W/ig, "") + "Application";
                app = Pathfinder.UI[app];
                if (app)
                    applicationInstance = new app(id);
                else
                    msg = "Missing application class definition for " + appOpt.Name;
            }
            else
                msg = "Missing application name: " + id;
        }

        if (!applicationInstance)
            throw new Error(msg);

        Pathfinder.UI.Application.instances[id] = applicationInstance;

        applicationInstance.initialize(clientManager);
    }

    clientManager._applicationManager = applicationInstance;

    if (autoActivate == true)
        applicationInstance.activate(clientManager);

    return applicationInstance;
};

Pathfinder.UI.Application.destroyInstances = function()
{
    ///<summary>Calls dispose on all Pathfinder.UI.Application instances that have been created and deletes them from memory.</summary>

    for (var id in Pathfinder.UI.Application.instances)
    {
        try
        {
            Pathfinder.UI.Application.instances[id].dispose();
            delete (Pathfinder.UI.Application.instances[id]);
        }
        catch (ex)
        {
        }
    }
};


Pathfinder.UI.Application.prototype =
{
    get_ApplicationID: function()
    {
        ///<summary>Returns the unique identifier for the application.  This should correspond to the database ID of the application.</summary>
        return this._id;
    },

    get_clientKey: function()
    {
        ///<summary>Returns the unique client identifier for the current user.  This corresponds to a text value assigned to each client and is used primarily for mapping to custom application folders.</summary>
        return this._clientKey;
    },

    get_ModuleMenu: function()
    {
        ///<summary>Returns the AJAX.Net component used for the application's module menu.  By default the component hosted in "divModuleOptions" will be returned."</summary>
        return $find("divModuleOptions");
    },

    getDefaultChannel: function()
    {
        return null;
    },

    getDefaultModule: function(clientManager)
    {
        ///<summary>Returns the default module for the application.  By default no default module is provided.</summary>
        return null;
    },

    get_UrlName: function()
    {
        ///<summary>Returns the application's base folder name.  This should be a subfolder of the main web application's folder.</summary>
        throw new Error("UrlName is not implemented.  Use a subclass of Pathfinder.UI.Application that overrides this property.");
    },

    get_ServiceUrl: function() { return this.get_UrlName() + "/services/pathfinderdataservice.svc"; },
    get_ClientServiceUrl: function() { return this.get_UrlName() + "/services/pathfinderclientdataservice.svc"; },

    get_MapUIStatePropertyNames: function()
    {
        return this.mapDataRequestUIStateProperties;
    },

    getUrl: function(channelName, module, pageName, moduleReady, isCustom)
    {
        ///<summary>Constructs a url for the data section of the dashboard in the format ~/&lt;ApplicationName&gt;/&lt;SectionName&gt;/&lt;Module&gt;.aspx.  If no data item is currently selected the url is based on the Application name only.  Additional options exist to customize a non-module based application page.</summary>
        ///<param name="channelName" type="String">Folder name for the Channel that the page is being requested from.</param>
        ///<param name="module" type="String">Name of the application module.  This value will be used as the page's name.</param>
        ///<param name="pageName" type="String" optional="true">Value to substitute for module if the page being requested is not the same as the module.</param>
        ///<param name="moduleReady" type="Boolean" optional="true">Indicates that the page being requested will have a query string or is ready to be displayed otherwise a default is displayed while a selection is pending.  This value is used to determine if the default page for the application should be returned instead of a data page represented by "module" or "pageName" parameters.</param>
        ///<param name="isCustom" type="Boolean" optional="true">Forces page to be mapped to the client's Custom application folder if parameter is True.</param>
        ///<returns type="String" />

        isCustom = isCustom == true;

        var url = (isCustom ? "custom/" + this.get_clientKey() + "/" + this.get_UrlName() : this.get_UrlName());

        if (pageName == null)
            pageName = module + ".aspx";

        //If module selected and data selected or not section2 (such as report criteria or other page fragment) then construct down to module level folder
        if (module != null && module != "" && (pageName != module + ".aspx" || moduleReady))
        {
            url += "/" + channelName + "/" + pageName;
        }
        else //use default app folder
        {
            if (pageName == module + ".aspx")
                pageName = "default.aspx";

            url += "/all/" + pageName;
        }

        return url;
    },

    getDefaultData: function(clientManager)
    {
        ///<summary>Returns the default data required for an application to initialize the dashboard after an application has been selected.  
        /// It is up to the application manager user to determine how this value is applied.
        /// For example clientManager will set the SelectionData property to this value after the Application or Channel property has changed.
        ///</summary>
        ///<param name="clientManager" type="Object"></param>
        ///<returns type="Object" />

        return null;
    },

    hasChannelMenuOptions: function(clientManager)
    {
        var opts = clientManager.get_ChannelMenuOptions()[this.get_ApplicationID()];
        if (opts)
        {
            for (var o in opts)
            {
                if (opts[o]["ID"] > 0)
                    return true; //return true on first hit on non-zero value (zero is All)
            }
        }

        return false;
    },

    canFilterByRegion: function(clientManager)
    {
        ///<summary>Internal method for determining if the current UI state supports filtering plan information by region.</summary>
        ///<returns type="Boolean" />
        var channel = clientManager.get_Channel();

        //return channel == 1 || channel == 6 || channel == 9 || channel == 11 || channel == 17;

        //Hack to get Executive Reporting map to drill down
        var applicationID = this.get_ApplicationID();

        if (applicationID == 18 || applicationID == 1)
            return true;

        var channels = clientManager.getModuleProp(applicationID, "map", "Channels");
        return (channels && channels[channel] == 1);
    },

    configureDashboardTiles: function(clientManager)
    {
        ///<summary>Provides an opportunity for subclasses to customzie visible tiles when application is activated.</summary>

        this.resize();
    },

    resetDashboardTiles: function(clientManager)
    {
        ///<summary>Provides an opportunity for subclasses to reset visible tiles and controls when application is deactivated.</summary>
    },

    configureChannelMenu: function(clientManager)
    {
        var items = clientManager.get_ChannelMenuOptions()[this.get_ApplicationID()]; //get the respective channels for each application, else it will fail in export modules where sectionid = 0
        //var items = clientManager.get_ChannelMenuOptions()[Pathfinder.UI.Applications.TodaysAccounts];       
        //        if (this.get_ApplicationID() == Pathfinder.UI.Applications.CustomSegments) -- check not required , since each application will have its own channel
        //            var items = clientManager.get_ChannelMenuOptions()[this.get_ApplicationID()];
        if (items)
        {
            var foundChannel = false;
            var foundEffectiveChannel = false;

            for (var o in items)
            {
                if (items[o]["ID"] == clientManager.get_Channel())
                    foundChannel = true;

                if (items[o]["ID"] == clientManager.get_EffectiveChannel())
                    foundEffectiveChannel = true;
            }

            if (!foundChannel)
            {
                var def = this.getDefaultChannel();
                if (def && items[def])
                    clientManager.set_Channel(def);
                else
                {
                    for (var i in items)
                    {
                        clientManager.set_Channel(items[i]["ID"]);
                        break;
                    }
                }
            }
            else if (!foundEffectiveChannel)
            {
                for (var i in items)
                {
                    clientManager.set_EffectiveChannel(items[i]["ID"]);
                    break;
                }
            }

            //$loadMenuItems(clientManager.get_ChannelMenu(), items, null, clientManager.get_Channel());

            //Register dropdown
            clientManager.registerComponent("ctl00_main_subheader1_channelMenu", null, null, "mainSection");

            //check if application is changed
            var application = clientManager.get_Application();
            var state = clientManager._history[clientManager._currentHistoryIndex];

            var applicationChanged = false;
            var state = clientManager._history[clientManager._currentHistoryIndex];
            if (typeof (state) != "undefined")
                applicationChanged = (Sys.Serialization.JavaScriptSerializer.deserialize(state, true)["Application"] != application)

            //Create Channel Dropdown Menu
            if (!clientManager.get_ChannelMenuCheckBoxList())
            {
                $createCheckboxDropdown("ctl00_main_subheader1_channelMenu", "Channel_Menu", null, { 'defaultText': 'No Selection', 'multiItemText': 'Change Selection' }, { itemClicked: this.channelMenuOptionClicked }, "mainSection");
            }
            if (applicationChanged || typeof (state) == "undefined")
            {
                //Remove Combined section (99) so it does not display in the Channel Menu
                var cleanItems = jQuery.extend(true, {}, items);
                for (var i in cleanItems)
                {
                    if (cleanItems[i].ID == 99)
                        delete (cleanItems[i]);
                }

                //Bind Channel Class list - bound outside of $createCheckboxDropdown to set selected value
                var channel_menu = clientManager.get_ChannelMenuCheckBoxList();
                $loadPinsoListItems(channel_menu, cleanItems, null, -1);

                //Set a context value to avoid looping on Custom Segment click in Channel Menu in channelMenuOptionClicked event
                clientManager.setContextValue('customSegmentClicked', 'false');
            }
            //}

            //Update selected item in Thera Class list
            $updateCheckboxDropdownText("ctl00_main_subheader1_channelMenu", "Channel_Menu");
        }
    },

    channelMenuOptionClicked: function(sender, args)
    {
        ///<summary>Handles the click event for each Channel on the Channel Menu.</summary>

        //If a Custom Channel is selected ( > 100), deselect all other channels
        var customSegmentClicked = (clientManager.getContextValue('customSegmentClicked') === 'true');
        var channelControl = clientManager.get_ChannelMenuCheckBoxList();
        if (parseInt(args.item.value) > 100 && (!customSegmentClicked))
        {
            clientManager.setContextValue('customSegmentClicked', 'true');

            //Clear all items in the checkbox list
            $(channelControl.get_element()).find("input[type=checkbox]").each(function() { this.checked = false; });

            //Select the clicked custom segment
            channelControl.selectItem(args.item.value);
        }
        //Property is set to avoid endless loop
        else
        {
            //Clear any selected custom segments in the checkbox list
            if (args.item.value < 100)
            {
                $(channelControl.get_element()).find("input[type=checkbox]").each(function()
                {
                    if (parseInt(this.value) > 100)
                        this.checked = false;
                });
            }

            clientManager.setContextValue('customSegmentClicked', 'false');
        }
    },

    configureModuleMenu: function(clientManager, channel, currentModule)
    {
        ///<summary>Allows for custom configuration of a Module menu.  Override in application classes to provide custom menu options.</summary>
        ///<param name="clientManager" type="Object"></param>
        ///<param name="channel" type="Number">The id of the current Channel.</param>
        ///<param name="currentModule" type="String">Module that was last selected by the user.  It should be used to reset the menu to the current selection if still valid for new menu options.</param>
        ///<returns type="String">Returns the selected Module.  By default it will be the value set in the currentModule parameter unless it is no longer valid for the new set of menu options.  If no longer valid the menu's default should be returned.</returns>

        if (this.get_ModuleMenu())
            this.get_ModuleMenu().clear();
        $(".navbar2").hide();
        return currentModule;
    },

    initialize: function(clientManager)
    {
        ///<summary>Initializes application instance.  This is called only once when the application object is intsantiated by a call to Pathfinder.UI.Application.getInstance.</summary>
        ///<param name="clientManager" type="Object"></param>

        this._clientKey = clientManager.get_ClientKey();
    },

    activate: function(clientManager)
    {
        ///<summary>Readies an application for interaction.  This method can be called automatically by a call to Pathfinder.UI.Application.getInstance otherwise it should be called manually when the application is selected.</summary>
        ///<param name="clientManager" type="Object"></param>


        //update module menu
        var module = this.configureModuleMenu(clientManager, clientManager.get_Channel(), null);
        if (clientManager.get_UIReady()) //sync module if clientManager is ready state
            clientManager._module = module;

        //configure dashboard tiles
        this.configureDashboardTiles(clientManager);

        //update section menus
        this.configureChannelMenu(clientManager);

        //to fix the effective channel error for applications other than TA, where default channel = 0
        if (clientManager && this.get_ApplicationID() != Pathfinder.UI.Applications.TodaysAccounts)
        {
            var items = clientManager.get_ChannelMenuOptions()[this.get_ApplicationID()]
            var x = 0;
            for (var i in items)
            {
                x++;
                if (items[i]["ID"] == 0 && x == 1 && (this.getDefaultChannel() < 0 || this.get_ApplicationID() != Pathfinder.UI.Applications.TodaysAccounts)) //to prevent setting channel if it is todays accounts or default channel < 0
                    clientManager.set_EffectiveChannel(0);
                break;
            }
        }

        $("#table1, .navbar2, #section2, .dashboardTable:not(.notbound)").css("visibility", "visible");

        if ((this.getDefaultChannel() == 105) || (this.getDefaultChannel() == 106) || (this.getDefaultChannel() == 107) || (this.getDefaultChannel() == 108))
        {
            $("tileMin1").css({ display: "none" });
            $("tileMin1").css({ width: "0px" });
            $("expandTile1").css({ display: "none" });
            $("expandTile1").css({ width: "0px" });
            $("tileMin2").css({ marginLeft: "0px" });
        }
    },

    deactivate: function(clientManager)
    {
        ///<summary>Provides method for applications to cleanup events and other objects when the application is not active.</summary>
        ///<param name="clientManager" type="Object"></param>
        //        $("#table1, .navbar2, #section2, #divTile2Plans .dashboardTable").css("visibility", "hidden");
        $("#table1, .navbar2, #section2, .dashboardTable").css("visibility", "hidden");

        this.resetDashboardTiles(clientManager);
    },

    dispose: function()
    {
        ///<summary>Provides method for applications to cleanup all resources.  It is automatically called by Pathfinder.UI.Application.destroyInstances.</summary>
    },

    collapseSidePanel: function() { minTile2(); },
    expandSidePanel: function() { maxTile2(); },

    resize: function(e)
    {
        ///<summary>Provides applications with the ability to resize window contents.  Typically an override is required due to ui changes made in configureDashboardTiles method.</summary>

        //        textResize();

        this.resizeModal();
        fixMapTip();

        maxTile();
    },

    resizeSection: function(e)
    {
        ///<summary>Provides applications with the ability to resize Section 2 contents.  Typically an override is required due to ui changes made in configureDashboardTiles method.</summary>
    },

    resizeModal: function()
    {
        var browserWindow = $(window);
        var height = browserWindow.height();
        var width = browserWindow.width();

        $(".RadWindow table").height("100%");
        $("#RadWindowWrapper_modal").css({ height: height - 150, width: width / 1.05 }, animationSpeed);
    },

    configureSection: function()
    {
        ///<summary>Provides applications with the ability to configure Section 2 contents.</summary>
    }
};
Pathfinder.UI.Application.registerClass("Pathfinder.UI.Application");




//Basic Navigation - NO-Map Base Application
Pathfinder.UI.BasicApplication = function(id)
{
    Pathfinder.UI.BasicApplication.initializeBase(this, [id]);
};

Pathfinder.UI.BasicApplication.configureDashboardTiles = function(clientManager, title, subtitle)
{
    $("#divTile2Container .title").html(title);
    $(".moduleSubHeader .title").text(subtitle);

    $("#divTile2Container .pagination").hide();

    $(".tile2Div").hide();

    $("#divTile2ModuleSelection").show();

    $("#tile1, #tileMin1").hide();

    $("#tile2 .min").show();

    $("#section2").addClass("section2SR");

    //        $("#tile2").attr("_h", $("#tile2").height());
};

Pathfinder.UI.BasicApplication.resetDashboardTiles = function(clientManager)
{
    $("#section2SR").attr("id", "section2");
    $("#section2").removeClass("section2SR");
    //Undo minTile2() function
    $("#divTile2Container .title").html("...");
    $(".moduleSubHeader .title").text("");
    $(".enlarge").hide();
    $("#expandTile2SR").width(0).hide();
    $("#tileMin2SR").attr({ id: "tile2" });
    $("#divTile2").css({ position: "" });
    $("#tile2").css("position", "").css("left", "");
    $("#tile2 .tileContainerHeader .title").show();
    $("#tile2 .tileContainerHeader .tools").show();
    $("#tile2 #divTile2 #moduleSelector").show();
    $(".enlarge").show();
    $("#tile3").addClass("leftTile");
    $("#section2").removeClass("todaysAccounts2Expand");

    $("#tile2").height(getTile1Height()).width("auto");

    $("#tile2 #divTile2Container ").show();
    $("#tile1, #tileMin1").height(getTile1Height());
    $("#tile1, #tileMin1").show();
    ///
    $("#divTile2ModuleSelection").hide();

    clientManager.unloadPage("section2");

    $("#section2").removeAttr("style");

    $("#tile2").show();

    $("#infoPopup").hide();
};

Pathfinder.UI.BasicApplication.prototype =
{
    get_Title: function() { return ""; },
    get_SubHeaderTitle: function() { return ""; },

    get_ModuleMenu: function()
    {
        return $find("moduleSelector");
    },

    get_OptionsServiceUrl: function(clientManager)
    {
        return this.get_ServiceUrl() + "/GetModuleOptions";
    },

    get_OptionsServiceArgs: function() { return null; },

    dispose: function()
    {
        delete (this._channelChangedDelegate);
        delete (this._moduleChangingDelegate);
        delete (this._moduleChangedDelegate);
        delete (this._viewRestoredDelegate);

        Pathfinder.UI.BasicApplication.callBaseMethod(this, 'dispose');
    },

    activate: function(clientManager)
    {
        Pathfinder.UI.BasicApplication.callBaseMethod(this, 'activate', [clientManager]);

        if (this._channelChangedDelegate == null)
        {
            this._channelChangedDelegate = Function.createDelegate(this, this._channelChanged);
            clientManager.add_channelChanged(this._channelChangedDelegate);
        }
        if (this._moduleChangingDelegate == null)
        {
            this._moduleChangingDelegate = Function.createDelegate(this, this._moduleChanging);
            clientManager.add_moduleChanging(this._moduleChangingDelegate);
        }
        if (this._moduleChangedDelegate == null)
        {
            this._moduleChangedDelegate = Function.createDelegate(this, this._moduleChanged);
            clientManager.add_moduleChanged(this._moduleChangedDelegate);
        }
        if (this._viewRestoredDelegate == null)
        {
            this._viewRestoredDelegate = Function.createDelegate(this, this._viewRestored);
            clientManager.add_restoredView(this._viewRestoredDelegate);
        }
    },

    deactivate: function(clientManager)
    {
        clientManager.remove_moduleChanging(this._moduleChangingDelegate);
        clientManager.remove_moduleChanged(this._moduleChangedDelegate);
        clientManager.remove_channelChanged(this._channelChangedDelegate);
        clientManager.remove_restoredView(this._viewRestoredDelegate);

        delete (this._channelChangedDelegate);
        delete (this._moduleChangingDelegate);
        delete (this._moduleChangedDelegate);
        delete (this._viewRestoredDelegate);

        clientManager.unloadPage("moduleOptionsContainer");

        Pathfinder.UI.BasicApplication.callBaseMethod(this, 'deactivate', [clientManager]);
    },

    configureDashboardTiles: function(clientManager)
    {
        Pathfinder.UI.BasicApplication.configureDashboardTiles(clientManager, this.get_Title(), this.get_SubHeaderTitle());

        Pathfinder.UI.BasicApplication.callBaseMethod(this, 'configureDashboardTiles', [clientManager]);

        this._reloadModuleOptionsContainer(clientManager, true);
    },

    resetDashboardTiles: function(clientManager)
    {
        Pathfinder.UI.BasicApplication.resetDashboardTiles(clientManager);

        Pathfinder.UI.BasicApplication.callBaseMethod(this, 'resetDashboardTiles', [clientManager]);
    },

    _updateOptions: function(menu, channel, currentModule, reportOptions)
    {
        menu.clear();
        var opt;
        for (var i in reportOptions)
        {
            opt = reportOptions[i];
            if (opt.Channels[0] || opt.Channels[channel])
            {
                menu.addItem(opt.ID, opt.Name, opt.ID);
            }
        }

        if (currentModule == "coTierCoverageHxFormulary".toLowerCase())
            currentModule = "coTierCoverageHxFormulary";
        else if (currentModule == "coRestrictionsHxFormulary".toLowerCase())
            currentModule = "coRestrictionsHxFormulary";

        //Check if current module is available for selected Section - If not reset module to first selected option
        if (menu.getItem(currentModule) != null)
        {
            menu.highlightItem(currentModule);
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

        return currentModule;
    },

    configureModuleMenu: function(clientManager, channel, currentModule)
    {
        currentModule = Pathfinder.UI.BasicApplication.callBaseMethod(this, 'configureModuleMenu', [clientManager, channel, currentModule]);

        //Hide current filters because they may no longer be valid
        clientManager.unloadPage("moduleOptionsContainer");

        var menu = this.get_ModuleMenu();

        if (currentModule == null)
            currentModule = this.getDefaultModule(clientManager);

        if (menu)
        {
            var me = this;
            if (this._reportOptions == null) //try cm's module store first
            {
                this._reportOptions = clientManager.getModuleOptionsByApp(this.get_ApplicationID());
                if (!this._reportOptions.length) this._reportOptions = null;
            }
            if (this._reportOptions == null)
            {
                //            this._reportOptions = [];
                $.getJSON(this.get_OptionsServiceUrl()
            , this.get_OptionsServiceArgs()
            , function(r, s)
            {
                //just get value in first property of "d" - name is unknown so we are using "for-loop"
                for (var s in r.d)
                {
                    me._reportOptions = Sys.Serialization.JavaScriptSerializer.deserialize(r.d[s]);
                    break;
                }
                currentModule = me._updateOptions(menu, channel, clientManager.get_Module(), me._reportOptions);

            }
            );
            }
            else
            {
                //            if (this._reportOptions.length > 0)
                //            {
                currentModule = this._updateOptions(menu, channel, clientManager.get_Module(), this._reportOptions);
                //            }
            }
        }

        return currentModule;
    },

    get_ModuleOptionsUrl: function(clientManager)
    {
        return null;
    },

    _channelChanged: function(sender, args)
    {
        //sender.unloadPage("moduleOptionsContainer");

        //need to reload module options if channel changes because when menu loads it just highlights correct item only (no selection) - function will check to make sure there is a current module to apply
        this._reloadModuleOptionsContainer(sender);
    },

    _moduleChanging: function(sender, args)
    {
        //silently clear selection data
        sender.clearSelectionData(true);
    },

    _moduleChanged: function(sender, args)
    {
        this._reloadModuleOptionsContainer(sender);
    },

    _viewRestored: function(sender, args)
    {
        this._reloadModuleOptionsContainer(sender, true);
    },

    _reloadModuleOptionsContainer: function(clientManager, uiTriggered)
    {
        //uiTriggered means it came from automation such as configureDashboardTiles and not user triggered event such as menu selection
        if (uiTriggered || clientManager.get_UIReady())
        {
            var url = this.get_ModuleOptionsUrl(clientManager);

            //load options        
            if (url && clientManager.get_Module())
                clientManager.loadPageUsingUIState(url, "moduleOptionsContainer");
            else
                clientManager.unloadPage("moduleOptionsContainer");
        }
    }
};
Pathfinder.UI.BasicApplication.registerClass("Pathfinder.UI.BasicApplication", Pathfinder.UI.Application);





//Todays Accounts business rules ---------------------------------------------------------------------------------------------------------------------------------------------
Pathfinder.UI.TodaysAccountsApplication = function(id)
{
    Pathfinder.UI.TodaysAccountsApplication.initializeBase(this, [id]);

    this._channelChangedDelegate = null;
    this._previousChannel = null;
    this._rebindPlanInfoGrid = true;
    this._resetChannel = false;

    this._previousRegion = null;
    this._initialActivation = false;

    this._defaultModule = "planinformation";
};
Pathfinder.UI.TodaysAccountsApplication.prototype =
{
    get_UrlName: function()
    {
        return "todaysaccounts";
    },

    getDefaultChannel: function()
    {
        //Return previous channel if not 99 (Combined), otherwise return 1 (Commercial)
        return this._previousChannel ? (this._previousChannel != 99 ? this._previousChannel : [1]) : [1];

    },

    getDefaultModule: function(clientManager)
    {
        var channel = clientManager.get_Channel();

        //if (clientManager.get_Channel() != 0)
        if ($.inArray(0, channel) == -1)
            return this._defaultModule;

        return null;
    },

    initialize: function(clientManager)
    {
        //if drug controls not enabled then don't send drug id when requesting map data for TA
        //        if (!clientManager.get_MarketBasketList())
        this.mapDataRequestUIStateProperties = ["Application", "Channel", "Geography", "Region"];

        Pathfinder.UI.TodaysAccountsApplication.callBaseMethod(this, 'initialize', [clientManager]);
    },

    dispose: function()
    {
        delete (this._channelChangedDelegate);

        Pathfinder.UI.TodaysAccountsApplication.callBaseMethod(this, 'dispose');
    },

    activate: function(clientManager)
    {
        this._resetChannel = (clientManager.get_EffectiveChannel() == 0); //global flag gets reset later so keep local value

        var resetChannel = (this._resetChannel || clientManager.get_EffectiveChannel() > 100); //if effective channel is still zero when returning to TA then we need to reset to something

        Pathfinder.UI.TodaysAccountsApplication.callBaseMethod(this, 'activate', [clientManager]);

        if (this._channelChangedDelegate == null)
        {
            this._channelChangedDelegate = Function.createDelegate(this, this._channelChanged);
            clientManager.add_channelChanged(this._channelChangedDelegate);
        }




        //Load Channel Menu
        clientManager.set_Channel(this.getDefaultChannel());
        clientManager.get_ChannelMenuCheckBoxList().selectItem(clientManager.get_Channel());

        //Update the selected item text
        $updateCheckboxDropdownText("ctl00_main_subheader1_channelMenu", "Channel_Menu");

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
            //Code commented to properly load the map when switching from Standard Reports Geo Coverage back to TA
            // 7/24/2012 else if (!resetChannel) needed to avoid 'Invalid area XML' error   
            else if (!resetChannel) //if resetting channel then don't need to reload map data manually
                clientManager.mapReloadData();
        }

        //Show the channel menu
        if (clientManager) clientManager.get_ChannelMenu().set_visible(true);
        //$('#ctl00_main_subheader1_channelMenu').show().css('display', 'inline-block');

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
        this._rebindPlanInfoGrid = null;

        clientManager.remove_channelChanged(this._channelChangedDelegate);
        delete (this._channelChangedDelegate);

        //Hide the channel menu
        if (clientManager) clientManager.get_ChannelMenu().set_visible(false);

        Pathfinder.UI.TodaysAccountsApplication.callBaseMethod(this, 'deactivate', [clientManager]);
    },

    configureSection: function()
    {
        if (!$get("divTile5Max"))
            $("#section2 .enlarge").show();
    },

    resize: function()
    {
        todaysaccounts_content_resize();

        Pathfinder.UI.TodaysAccountsApplication.callBaseMethod(this, 'resize');
    },

    resizeSection: todaysaccounts_section_resize,

    resizeModal: function()
    {
        Pathfinder.UI.TodaysAccountsApplication.callBaseMethod(this, 'resizeModal');

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
        //var channel = clientManager.get_EffectiveChannel();
        var channel = clientManager.get_Channel();
        var application = clientManager.get_Application();
        var region = clientManager.get_Region();

        var channelChanged = this._rebindPlanInfoGrid; //(this._previousChannel != channel);

        //to fix the error if coming back from another application and selecting multi channel
        if (channelChanged == null) channelChanged = (this._previousChannel != channel);

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

            //this._previousChannel = channel;
            this._previousChannel = clientManager.get_EffectiveChannel();
            this._previousRegion = region;

            //if channel is changed or application is changed
            if (!this._resetChannel || applicationChanged)
            {
                var grid = clientManager.get_PlanInfoGrid();
                if (grid)
                {
                    var mt = grid.get_masterTableView();

                    //Make sure search filters are showing properly
                    var filterRow = mt.get_tableFilterRow();
                    var visible = filterRow.style.display != "" && filterRow.style.display != "none";
                    if (visible)
                    {
                        //Hide all filters except for Account Name when multiple Channels are selected
                        var channel = clientManager.get_Channel();

                        // sl 5/31/2012 to avoid error: if 'All' selected,  filter(except ‘Account’ filter) should be hidden after showing 
                        //the filter in case of single Channel selection
                        if (channel.length > 1 || channel == 0 || channel == null || channel == "")
                            $('#ctl00_main_planInfo_gridPlanInfo_GridHeader .RadComboBox').hide();
                        else
                            $('#ctl00_main_planInfo_gridPlanInfo_GridHeader .RadComboBox').show();
                    }

                    //if (channel == 0) channel = null;
                    if (channel.length == 1 && $.inArray(0, channel) > -1)
                        channel = null;

                    //$setGridFilter(grid, "Section_ID", channel, "EqualTo", "System.Int32");
                    if (channel && channel.length > 1)
                        $setGridFilter(grid, "Section_ID", channel.join(","), "Custom", "System.Int32");
                    else
                        $setGridFilter(grid, "Section_ID", channel, "EqualTo", "System.Int32");

                    $setGridFilter(grid, "Plan_State", region);

                    //if (channel != 11) //clear VISN filter if channel is not VISN
                    if (channel && channel.length == 1 && $.inArray(11, channel) == -1)
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
        Pathfinder.UI.TodaysAccountsApplication.callBaseMethod(this, 'configureDashboardTiles', [clientManager]);

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
        //        if (clientManager.get_Channel() == 0)
        //        {
        //            if (!this._resetChannel)
        //            {
        //                $("#expandTile1 a").attr("disabled", false);
        //                maxTile1();
        //                //               $("#expandTile1 a").attr("disabled", true);
        //                //               minTile1();
        //            }

        //            if (channel == 0)
        //                return; //don't do anything else if All is selected - when Plan is automatically selected the menu will update
        //        }

        var menu = this.get_ModuleMenu();

        Pathfinder.UI.TodaysAccountsApplication.callBaseMethod(this, 'configureModuleMenu', [clientManager, channel, currentModule]);

        $(".navbar2").show();

        //Load Module menu for today's accounts
        var map;
        var modules = clientManager.getModuleOptionsByApp(this.get_ApplicationID());
        var module;
        for (var i = 0; i < modules.length; i++)
        {
            if ($.isArray(channel))
            {
                //Loop through selected channels
                for (var c = 0; c < channel.length; c++)
                {
                    module = modules[i];
                    var curChannel = channel[c];
                    if (module.ID != "map") //map is a configurable module but not part of menu
                    {
                        if (module.Channels[curChannel] == 1)
                            menu.addItem("link_" + module.ID, module.Name, module.ID);
                    }
                    else
                        map = module.Channels[curChannel] == 1;
                }
            }
            else
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
        }
        //

        var firstItem = menu.getItemAt(0);
        if (firstItem)
            this._defaultModule = firstItem.value;

        //cm must be ready and channel should not be 0 otherwise this was called to update menu based on a selection (map should stay hidden)
        //SPH 9/25/09 - Don't think we need condition "clientManager.get_UIReady() && " anymore
        //if (clientManager.get_Channel() >= 0)
        //{
        if (map)
        {

            //if (clientManager.get_Channel() == 0 || channel == 0 || channel == 1 || channel == 6 || channel == 9 || channel == 11 || channel == 17 || channel == 20 || channel == 99)
            channel = clientManager.get_Channel();
            //Do the map logic if channel selection is only one segment
            if (channel.length == 1)
            {
                channel = channel.toString();

                if (channel == 0 || channel == 1 || channel == 4 || channel == 6 || channel == 9 || channel == 11 || channel == 17 || channel == 20 || channel == 99)
                {
                    $("#expandTile1 a").attr("disabled", false);
                    maxTile1();
                }
                else
                {
                    $("#expandTile1 a").attr("disabled", true);
                    minTile1();
                    //                resetGridHeaders();
                }
            }
            else //Check if channel selection contains a channel where the menu should show
            {
                var showMap = 0;
                if ($.inArray(0, channel) > -1)
                    showMap++;
                if ($.inArray(1, channel) > -1)
                    showMap++;
                if ($.inArray(6, channel) > -1)
                    showMap++;
                if ($.inArray(9, channel) > -1)
                    showMap++;
                if ($.inArray(11, channel) > -1)
                    showMap++;
                if ($.inArray(17, channel) > -1)
                    showMap++;
                if ($.inArray(20, channel) > -1)
                    showMap++;
                if ($.inArray(99, channel) > -1)
                    showMap++;

                if (showMap > 0)
                {
                    $("#expandTile1 a").attr("disabled", false);
                    maxTile1();
                }
                else
                {
                    $("#expandTile1 a").attr("disabled", true);
                    minTile1();
                    //                resetGridHeaders();
                }
            }
        }
        else
        {
            $("#expandTile1 a").attr("disabled", true);
            minTile1();
            //                resetGridHeaders();
        }
        //}

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
Pathfinder.UI.TodaysAccountsApplication.registerClass("Pathfinder.UI.TodaysAccountsApplication", Pathfinder.UI.Application);







////Custom Options Business Rules --------------------------------------------------------------------------------------------------------------------------------------------
//Pathfinder.UI.CustomApplication = function(id)
//{
//    Pathfinder.UI.CustomApplication.initializeBase(this, [id]);
//};

//Pathfinder.UI.CustomApplication.prototype =
//{
//    initialize: function(clientManager)
//    {
//        clientManager.get_ChannelMenuOptions()[this.get_ApplicationID()] = [{ Name: "Customer Contact Reports", ID: 401 }, { Name: "Formulary Sell Sheets", ID: 402}];

//        Pathfinder.UI.CustomApplication.callBaseMethod(this, "initialize", [clientManager]);
//    },

//    get_UrlName: function() { return "custom/" + this.get_clientKey(); },

//    get_Title: function() { return "Custom Options"; },

//    getChannelUrlName: function(channel)
//    {
//        ///<summary>Returns the folder name for a channel id.</summary>
//        switch (channel)
//        {
//            case 401: return "ccr";
//            case 402: return "sellsheets";
//            default: return "ccr";
//        }
//    },

//    resize: function()
//    {
//        customapplication_content_resize();
//        
//        Pathfinder.UI.CustomApplication.callBaseMethod(this, 'resize');
//    },
//    resizeSection: customapplication_section_resize,

//    configureChannelMenu: function(clientManager)
//    {
//        //This is a hack for demo purposes - shouldn't be using channel menu for what is really modules
//        $loadMenuItems(clientManager.get_ChannelMenu(), [], null, 0);
//    }
//};
//Pathfinder.UI.CustomApplication.registerClass("Pathfinder.UI.CustomApplication", Pathfinder.UI.BasicApplication);




//Today's Analytics Business Rules --------------------------------------------------------------------------------------------------------------------------------------------
Pathfinder.UI.MarketplaceAnalyticsApplication = function(id)
{
    Pathfinder.UI.MarketplaceAnalyticsApplication.initializeBase(this, [id]);

    this._timeFrameLoadedDelegate = null;
    this._toolbarLoadedDelegate = null;
};

Pathfinder.UI.MarketplaceAnalyticsApplication.prototype =
{
    get_Title: function() { return "Report Type"; },
    get_UrlName: function() { return "marketplaceanalytics"; },

    getUrl: function(channelName, module, pageName, hasData, isCustom)
    {
        //not using channel menu anymore for standard reports
        channelName = "all";
        //

        ////Does not require user selection to run so hasData must be true
        //if (module == "trending")
        //    hasData = true;

        return Pathfinder.UI.MarketplaceAnalyticsApplication.callBaseMethod(this, 'getUrl', [channelName, module, pageName, hasData, isCustom]);
    },

    get_OptionsServiceUrl: function(clientManager)
    {
        return this.get_ServiceUrl() + "/GetModuleOptions";
    },

    getDefaultModule: function(clientManager)
    {
        return "trending";
    },

    get_ModuleOptionsUrl: function(clientManager)
    {
        //use module specific filter page     
        if (clientManager.get_Module())
            return this.getUrl("all", null, clientManager.get_Module() + "_filters.aspx", false);
        else
            return null;
        //        //SPH 09/13/2010 - put old code back for release that does not have complete changes
        //        if (clientManager.get_Module() != "coveredlives" && clientManager.get_Module() != "geographiccoverage")
        //            return this.getUrl("all", null, "filters.aspx", false);
        //        else
        //            return null;
    },

    activate: function(clientManager)
    {
        //Hide the channel menu
        if (clientManager) clientManager.get_ChannelMenu().set_visible(false);

        if (!this._moduleChangingDelegate)
        {
            this._moduleChangingDelegate = Function.createDelegate(this, this._moduleChanging);
            clientManager.add_moduleChanging(this._moduleChangingDelegate);
        }

        this._timeFrameLoadedDelegate = Function.createDelegate(this, this._onTimeFrameLoaded);
        this._toolbarLoadedDelegate = Function.createDelegate(this, this._onToolbarLoaded);

        clientManager.add_pageLoaded(this._timeFrameLoadedDelegate, "timeframeArea");
        clientManager.add_pageLoaded(this._toolbarLoadedDelegate, "toolbarArea");

        Pathfinder.UI.MarketplaceAnalyticsApplication.callBaseMethod(this, "activate", [clientManager]);
    },

    deactivate: function(clientManager)
    {
        Pathfinder.UI.MarketplaceAnalyticsApplication.callBaseMethod(this, "deactivate", [clientManager]);

        //clientManager.remove_moduleChanging(this._moduleChangingDelegate);

        //clientManager.remove_mapUpdating(this._mapCommandDelegate);

        //delete (this._mapCommandDelegate);

        clientManager.remove_pageLoaded(this._timeFrameLoadedDelegate, "timeframeArea");
        clientManager.remove_pageLoaded(this._toolbarLoadedDelegate, "toolbarArea");
        delete (this._timeFrameLoadedDelegate);
        delete (this._toolbarLoadedDelegate);
    },

    getDefaultData: function(clientManager)
    {
        ///<summary>Returns the default data required for an application to initialize the dashboard after an application has been selected.  It is up to the application manager user to determine how this value is applied.  For example clientManager will set the SelectionData property to this value after the Application property has changed.</summary>
        ///<param name="clientManager" type="Object"></param>
        ///<returns type="Object" />
        if (clientManager.get_Module() != "geographiccoverage")
            return Pathfinder.UI.MarketplaceAnalyticsApplication.callBaseMethod(this, "getDefaultData", [clientManager]);

        //for geographic coverage still showing same selection criteria but for new channel
        var data = clientManager.get_SelectionData();
        if (!data) data = {};

        data["Section_ID"] = new Pathfinder.UI.dataParam("Section_ID", clientManager.get_Channel(), "System.Int32", "EqualTo");
        return data;
    },

    onModuleChanging: function(cm, newModule)
    {
        if (cm.get_UIReady())
            cm.clearSelectionData(true);

        //        $("<div />").appendTo(".navbar2").attr("id", "timeframeArea");
        //        $(".navbar .clearAll").before("<div id='toolbarArea' />");
        //        //$(".navbar2").show().css({"margin-left": "207px"});
        //        //$(".navbar2 .navbar").hide();
        //        $("#timeframeArea").show();

        //        clientManager.loadPage(this.getUrl("all", "all", "toolbar.aspx"), "toolbarArea");

        //        //Set timeframe selector based on module selected
        //        if (newModule == 'comparison')
        //            clientManager.loadPage(this.getUrl("all", "all", "filtertimeframecomparison.aspx"), "timeframeArea");
        //        else
        //            clientManager.loadPage(this.getUrl("all", "all", "filtertimeframe.aspx"), "timeframeArea");
    },

    _moduleChanging: function(sender, args)
    {
        this.onModuleChanging(sender, args.get_Value());
    },

    _onToolbarLoaded: function(sender, args)
    {
        $("#toolbarArea").css("visibility", "hidden").css("visibility", ""); //ie6 hacking;
    },

    _onTimeFrameLoaded: function(sender, args)
    {
        //Hide the timeframe if module is formularyhistoryreporting
        if (clientManager.get_Module() == 'formularyhistoryreporting')
            $(".navbar2").hide();
        else
        {
            $(".navbar2").show();

            var data = clientManager.get_SelectionData();
            var calendarRolling;

            if (data)
                calendarRolling = data["Calendar_Rolling"];

            //Check if Calendar_Rolling has a value
            if (calendarRolling)
            {
                if (typeof calendarRolling == "object")
                    calendarRolling = calendarRolling.value;
            }

            if ((typeof (calendarRolling) != "undefined")) //If there is selection data, set the value
            {
                if (calendarRolling == 'Calendar')
                {
                    $("#ctl00_partialPage_filtersContainer_CalendarRolling_Calendar_Rolling_0").attr('checked', 'checked');
                    showCalendar();
                }
                else
                {
                    $("#ctl00_partialPage_filtersContainer_CalendarRolling_Calendar_Rolling_1").attr('checked', 'checked');
                    showRolling();
                }
            }
            else //Otherwise set default value to Calendar
                $("#ctl00_partialPage_filtersContainer_CalendarRolling_Calendar_Rolling_0").attr('checked', 'checked');
        }
        this.resizeSection();
    },

    configureDashboardTiles: function(clientManager)
    {
        //alert(clientManager._module);
        this.onModuleChanging(clientManager, clientManager._module);

        $("<div />").appendTo(".navbar2").attr("id", "timeframeArea");
        $(".navbar .clearAll").before("<div id='toolbarArea' />");


        clientManager.loadPage(this.getUrl("all", "all", "toolbar.aspx"), "toolbarArea");

        //        $("<div />").appendTo("#mainSection .contentArea").hide().attr("id", "customInfoArea");

        //        clientManager.loadPage(this.getUrl("all", "all", "SellSheetCarousel.aspx"), "customInfoArea");

        //        $("#section2").addClass("defaultPage");

        //        $(".todaysAccounts1").hide();

        Pathfinder.UI.MarketplaceAnalyticsApplication.callBaseMethod(this, 'configureDashboardTiles', [clientManager]);


        $(".navbar2").show().css({ "margin-left": "208px" });

    },

    resetDashboardTiles: function(clientManager)
    {
        //        var menu = this.get_ModuleMenu();
        //        if (menu) menu.dispose();

        //clientManager.unloadPage("timeframeArea");

        $("#timeframeArea").remove();
        $("#toolbarArea").remove();
        //        //        $("#customModuleHeader").remove();

        $(".navbar2").hide().css({ "margin-left": "" });

        $(".todaysAccounts1").show();

        Pathfinder.UI.MarketplaceAnalyticsApplication.callBaseMethod(this, 'resetDashboardTiles', [clientManager]);
    },

    collapseSidePanel: function()
    {
        $(".navbar2").animate({ "margin-left": "34px" }, animationSpeed);
        minTile2(function()
        {
            //stick with standard names for MA so setting back to originals after min op
            $("#tile3SR").attr({ id: "tile3" });
            $("#tile4SR").attr({ id: "tile4" });
            $("#tile5SR").attr({ id: "tile5" });

            todaysanalytics_section_resize();

            if (ie6)
                $("#divYearContainer, #divQuarterContainer, #divMonthContainer, #divRollingContainer").css("visibility", "hidden").css("visibility", ""); //another ie6 hack job
        });
    },
    expandSidePanel: function()
    {
        $(".navbar2").animate({ "margin-left": "208px" }, animationSpeed);
        maxTile2(function()
        {
            todaysanalytics_section_resize();

            if (ie6)
                $("#divYearContainer, #divQuarterContainer, #divMonthContainer, #divRollingContainer").css("visibility", "hidden").css("visibility", ""); //another ie6 hack job
        });
    },

    resize: function()
    {
        todaysanalytics_content_resize();

        Pathfinder.UI.MarketplaceAnalyticsApplication.callBaseMethod(this, 'resize');
    },

    resizeSection: todaysanalytics_section_resize

};
Pathfinder.UI.MarketplaceAnalyticsApplication.registerClass("Pathfinder.UI.MarketplaceAnalyticsApplication", Pathfinder.UI.BasicApplication);





//Standard Reports business rules ---------------------------------------------------------------------------------------------------------------------------------------------
Pathfinder.UI.StandardReportsApplication = function(id)
{
    Pathfinder.UI.StandardReportsApplication.initializeBase(this, [id]);

    this._menuVisible = true; //used by collapse/expand to restore menu properly
    this._mapCommandDelegate = null;
    this._channelOrModuleChangingDelegate = null;
    this._filtersLoadedDelegate = null;
    this._restoredData = null;
};
Pathfinder.UI.StandardReportsApplication.prototype =
{
    get_Title: function()
    {
        //return "Report Criteria";
        return "";
    },
    get_SubHeaderTitle: function()
    {
        //return "Reports"; 
        return "";
    },


    get_UrlName: function()
    {
        return "standardreports";
    },
    //changes for 1674-- starts
    getUrl: function(channelName, module, pageName, hasData, isCustom)
    {
        //not using channel menu anymore for standard reports
        //        channelName = "all";
        //

        //these 2 reports don't require user selection to run so hasData must be true        
        if (module == "coveredlives") //changes for 1674
        {
            channelName = "all";
            hasData = true;
        }

        return Pathfinder.UI.StandardReportsApplication.callBaseMethod(this, 'getUrl', [channelName, module, pageName, hasData, isCustom]);
    },

    getDefaultModule: function(clientManager)
    {
        return "formularycoverage";
    },

    get_ModuleOptionsUrl: function(clientManager)
    {
        //for these modules filter would not be database driven
        //changes for 1674
        if (clientManager.get_Module() == "formularydrilldown" || clientManager.get_Module() == "livesdistribution"
            || clientManager.get_Module() == "formularystatus" || clientManager.get_Module() == "tiercoveragecomparison"
            || clientManager.get_Module() == "geographiccoverage" || clientManager.get_Module() == "formularycoverage")
            return this.getUrl(clientManager.get_ChannelUrlName(), clientManager.get_Module(), clientManager.get_Module() + "_filters.aspx", false);
        else if (clientManager.get_Module() != "coveredlives") //change for 1674
            return this.getUrl("all", null, "filters.aspx", false);
        else
            return null;
    },
    //changes for 1674- ends
    activate: function(clientManager)
    {

        // sl 6/21/2012 avoid error: should not show Channel Menu dropdown
        $('#ctl00_main_subheader1_channelMenu').hide();


        if (!this._mapCommandDelegate)
        {
            this._mapCommandDelegate = Function.createDelegate(this, this._mapCommand);
            clientManager.add_mapUpdating(this._mapCommandDelegate);
        }
        if (!this._channelChangingDelegate)
        {
            this._channelOrModuleChangingDelegate = Function.createDelegate(this, this._channelOrModuleChanging);
            clientManager.add_channelChanging(this._channelOrModuleChangingDelegate);
            clientManager.add_moduleChanging(this._channelOrModuleChangingDelegate);
        }
        if (!this._filtersLoadedDelegate)
        {
            this._filtersLoadedDelegate = Function.createDelegate(this, this._filtersLoaded);
            clientManager.add_pageLoaded(this._filtersLoadedDelegate, "moduleOptionsContainer");
        }

        $(clientManager.get_ChannelMenu().get_element()).remove().appendTo("#channelSelectorContainer");

        //Hide the channel menu
        if (clientManager) clientManager.get_ChannelMenu().set_visible(false);

        //Set effective channel to zero for proper loading of filters
        clientManager.set_EffectiveChannel(0);

        Pathfinder.UI.StandardReportsApplication.callBaseMethod(this, "activate", [clientManager]);
    },

    deactivate: function(clientManager)
    {
        Pathfinder.UI.StandardReportsApplication.callBaseMethod(this, "deactivate", [clientManager]);

        $(clientManager.get_ChannelMenu().get_element()).remove().appendTo(".viewSelect");

        clientManager.remove_mapUpdating(this._mapCommandDelegate);

        clientManager.remove_channelChanging(this._channelOrModuleChangingDelegate);
        clientManager.remove_moduleChanging(this._channelOrModuleChangingDelegate);
        clientManager.remove_pageLoaded(this._filtersLoadedDelegate, "moduleOptionsContainer");

        delete (this._mapCommandDelegate);
        delete (this._channelOrModuleChangingDelegate);
        delete (this._filtersLoadedDelegate);
    },

    canFilterByRegion: function(clientManager)
    {
        ///<summary>Internal method for determining if the current UI state supports filtering plan information by region.</summary>
        ///<returns type="Boolean" />
        return (clientManager.get_Module() == "geographiccoverage");
    },

    getDefaultData: function(clientManager)
    {
        ///<summary>Returns the default data required for an application to initialize the dashboard after an application has been selected.  It is up to the application manager user to determine how this value is applied.  For example clientManager will set the SelectionData property to this value after the Application property has changed.</summary>
        ///<param name="clientManager" type="Object"></param>
        ///<returns type="Object" />
        //        if (clientManager.get_Module() != "geographiccoverage")
        return Pathfinder.UI.StandardReportsApplication.callBaseMethod(this, "getDefaultData", [clientManager]);

        //for geographic coverage still showing same selection criteria but for new channel
        var data = clientManager.get_SelectionData();
        if (!data) data = {};

        data["Section_ID"] = new Pathfinder.UI.dataParam("Section_ID", clientManager.get_Channel(), "System.Int32", "EqualTo");
        return data;
    },

    _channelOrModuleChanging: function(sender, args)
    {
        this._restoredData = $getContainerData("moduleOptionsContainer", false, false, true, true);
    },

    _filtersLoaded: function(sender, args)
    {
        if (this._restoredData)
        {
            $reloadContainer("moduleOptionsContainer", this._restoredData);
            delete (this._restoredData);
        }
    },

    _mapCommand: function(sender, args)
    {
        //Currently only way to prevent map from updating while viewing Geography Coverage report - causes error since map is reloaded twice if not cancelled.
        //args.set_cancel(sender.get_Module() != "geographiccoverage");
        args.set_cancel(true);
    },

    collapseSidePanel: function()
    {
        //this._menuVisible = clientManager.get_ChannelMenu().get_visible();
        //if (clientManager) clientManager.get_ChannelMenu().set_visible(false);
        minTile2();
    },
    expandSidePanel: function()
    {
        maxTile2();
        //if (clientManager) clientManager.get_ChannelMenu().set_visible(this._menuVisible === true);

        if (clientManager) clientManager.get_ChannelMenu().set_visible(false);
    },

    resize: function()
    {
        standardreports_content_resize();

        Pathfinder.UI.StandardReportsApplication.callBaseMethod(this, 'resize');
    },

    resizeSection: standardreports_section_resize//,

};
Pathfinder.UI.StandardReportsApplication.registerClass("Pathfinder.UI.StandardReportsApplication", Pathfinder.UI.BasicApplication);




//FormularySell Sheets Business Rules --------------------------------------------------------------------------------------------------------------------------------------------
Pathfinder.UI.FormularySellSheetsApplication = function(id)
{
    Pathfinder.UI.FormularySellSheetsApplication.initializeBase(this, [id]);

    this._onSaveCallbackDelegate = null;
    this._onCreateCallbackDelegate = null;
    this._onTemplateUpdateCallbackDelegate = null;
};

Pathfinder.UI.FormularySellSheetsApplication.prototype =
{
    dispose: function()
    {
        this._deleteObjects();

        Pathfinder.UI.FormularySellSheetsApplication.callBaseMethod(this, 'dispose');
    },

    _deleteObjects: function()
    {
        delete (this._onCreateCallbackDelegate);
        delete (this._onNewOrderCallbackDelegate);
        delete (this._onSaveCallbackDelegate);
        delete (this._channelChangedDelegate);
        delete (this._moduleChangingDelegate);
        delete (this._onTemplateUpdateCallbackDelegate);
    },

    activate: function(clientManager)
    {
        this._onCreateCallbackDelegate = Function.createDelegate(this, this._onCreateCallback);
        this._onNewOrderCallbackDelegate = Function.createDelegate(this, this._onNewOrderCallback);
        this._onTemplateUpdateCallbackDelegate = Function.createDelegate(this, this._onTemplateUpdateCallback);

        if (!this._onSaveCallbackDelegate)
        {
            this._onSaveCallbackDelegate = Function.createDelegate(this, this._onSaveCallback);
            clientManager.add_formSubmitted(this._onSaveCallbackDelegate);
        }

        if (!this._channelChangedDelegate)
        {
            this._channelChangedDelegate = Function.createDelegate(this, this._channelChanged);
            clientManager.add_channelChanged(this._channelChangedDelegate);
        }

        if (!this._moduleChangingDelegate)
        {
            this._moduleChangingDelegate = Function.createDelegate(this, this._moduleChanging);
            clientManager.add_moduleChanging(this._moduleChangingDelegate);
        }

        //Hide the channel menu
        if (clientManager) clientManager.get_ChannelMenu().set_visible(false);

        Pathfinder.UI.FormularySellSheetsApplication.callBaseMethod(this, "activate", [clientManager]);
    },

    deactivate: function(clientManager)
    {
        Pathfinder.UI.FormularySellSheetsApplication.callBaseMethod(this, "deactivate", [clientManager]);

        clientManager.remove_channelChanged(this._channelChangedDelegate);
        clientManager.remove_moduleChanging(this._moduleChangingDelegate);
        clientManager.remove_formSubmitted(this._onSaveCallbackDelegate);

        this._deleteObjects();
    },

    get_UrlName: function() { return "custom/" + this.get_clientKey() + "/sellsheets"; },

    get_Title: function() { return "Formulary Sell Sheets"; },

    get_ModuleMenu: function() { return null; },

    resize: function()
    {
        formularysellsheets_content_resize();

        Pathfinder.UI.FormularySellSheetsApplication.callBaseMethod(this, 'resize');
    },
    resizeSection: formularysellsheets_section_resize,

    getDefaultModule: function(clientManager)
    {
        ///<summary>Returns the default module for the application.  By default no default module is provided.</summary>

        return "mysellsheets";
    },

    configureDashboardTiles: function(clientManager)
    {
        $("<div />").appendTo("#mainSection .contentArea").hide().attr("id", "customInfoArea");

        clientManager.loadPage(this.getUrl("all", "all", "SellSheetCarousel.aspx"), "customInfoArea");

        $("#section2").addClass("defaultPage");

        $(".todaysAccounts1").hide();

        Pathfinder.UI.FormularySellSheetsApplication.callBaseMethod(this, 'configureDashboardTiles', [clientManager]);
    },

    resetDashboardTiles: function(clientManager)
    {
        var menu = this.get_ModuleMenu();
        if (menu) menu.dispose();

        clientManager.unloadPage("customInfoArea");

        $("#customInfoArea").remove();
        //        $("#customModuleHeader").remove();

        $(".todaysAccounts1").show();

        $('#mycarousel li').unbind();

        var carousel = this.get_carousel();
        if (carousel) this.set_carousel(null);

        Pathfinder.UI.FormularySellSheetsApplication.callBaseMethod(this, 'resetDashboardTiles', [clientManager]);
    },

    get_carousel: function() { return this._carousel; },

    set_carousel: function(value) { this._carousel = value; },

    _channelChanged: function(sender, args)
    {
        //        sender.set_Module(this.getDefaultModule(sender));
        this.onModuleChanging(sender, sender.get_Module());
    },

    onModuleChanging: function(cm, newModule)
    {
        if (newModule == this.getDefaultModule(cm))
        {
            cm.clearSelectionData(true);
            $("#section2").addClass("defaultPage");

            //for effect not function
            $("#ssStepBodyContainer").hide();
            //for effect not function

            if ($("#customInfoArea").css("display") != "none")
                $("#customInfoArea").animate({ width: 0 }, animationSpeed, function() { $("#customInfoArea").hide() });
            $(".sellsheets #section2").animate({ marginLeft: 0 }, animationSpeed, function() { $(".sellsheets #section2").css("margin-left", ""); });
        }
        else
        {
            $("#section2").removeClass("defaultPage");
            if ($("#customInfoArea").css("display") != "block")
            {
                //for effect not function
                $(".createNewSellSheet").hide();
                $(".sellsheetLeft").hide();
                $(".sellsheetRight").hide();
                //for effect not function

                var funcName = cm.get_UIReady() ? "animate" : "css";
                $("#section2")[funcName]({ marginLeft: 185 }, animationSpeed);
                $("#customInfoArea").css("width", 0).show()[funcName]({ width: 180 }, animationSpeed);
            }
        }
    },

    _moduleChanging: function(sender, args)
    {
        this.onModuleChanging(sender, args.get_Value());
    },

    _onSaveCallback: function(sender, args)
    {
        if (args.result.Success)
        {
            //Clear the plan selection context when clicked on next button.
            clientManager.setContextValue("ssSelectedPlansList");

            this.next(sender);
        }
        else
        {
            //alert("Error: To be completed.");
        }
    },

    _setNext: function(clientManager, direction)
    {
        var module = clientManager.get_Module();
        var newModule;

        var j = $(".sellSheetSteps .selectedStep");
        if (j.length > 0)
        {
            var step = parseInt(j.attr("stepOrder"), 10) + direction;
            newModule = $(".sellSheetSteps a[stepOrder=" + step + "]").attr("stepKey");
        }

        if (!newModule) //no next step so set back to default view - we are done with process
            newModule = this.getDefaultModule(clientManager);

        clientManager.set_Module(newModule);
    },

    next: function(clientManager)
    {
        this._setNext(clientManager, 1);
    },

    back: function(clientManager)
    {
        this._setNext(clientManager, -1);
    },

    createSellSheet: function()
    {
        //Clear the plan selection for the first time.
        clientManager.setContextValue("ssSelectedPlansList");

        var dt = new Date();
        dt = "'" + encodeURIComponent(dt.localeFormat("d") + " " + dt.localeFormat("t")) + "'";

        $.getJSON(this.get_ServiceUrl() + "/CreateSellSheet?Created=" + dt, null, this._onCreateCallbackDelegate);
    },

    _onCreateCallback: function(r)
    {
        if (r && r.d && r.d.CreateSellSheet)
        {
            clientManager.set_SelectionData({ Sell_Sheet_ID: r.d.CreateSellSheet }, "classandtemplateselection");
        }
    },

    //1/22/2010 ASH D, This function is used to save new sell sheet order.
    newSellSheetOrder: function()
    {

        //Gets all the data entered in the New Order Form.
        var data = $getContainerData("NewOrder", null, true);

        if ($validateFormData("NewOrder", data, "New Sell Sheet Order"))
        {
            //flatten data - necessary for arrays and dataParam objects
            data = $getDataForPostback(data);
            //Calls NewSellSheetOrder() from CustomDataService.cs to add new order to db.
            $.post(this.get_ServiceUrl() + "/NewSellSheetOrder", data, this._onNewOrderCallbackDelegate, "json");
        }
    },

    _onNewOrderCallback: function(r)
    {
        if (r && r.d && r.d.NewSellSheetOrder)
        {
            //After adding the order to db, rebind orderlist.
            var gdOrders = $find("ctl00_Tile3_SellSheetOrderList1_gridSellSheetOrders").get_masterTableView();
            gdOrders.rebind();

            //Reset the new order form.
            $resetContainer("NewOrder");

            //Reset the state drop list
            var combostate = $get("ctl00_Tile3_NewSellSheetOrder1_rdcmbState").control;
            combostate.findItemByText("[Select a State]").select();

            //Get the Sell_Sheet_ID from completed sell sheets list, which will be used to save data to db 
            //for newly created order.
            var grid = $find("ctl00_Tile3_CompletedSellSheets1_gridCompletedSellSheets");
            var Sheet_ID = grid.get_masterTableView().get_selectedItems()[0]._dataItem.Sell_Sheet_ID;
            $("#SelectedSheetID").val(Sheet_ID);

            $alert("New Order generated successfully.", "New Order");

            new cmd(null, $hideAlert, null, 4000);
        }
        else
        {
            $alert("Error while generating New Order.", "New Order");
        }
    },

    //Used to update Template ID from Template Selector sidebar
    updateTemplateID: function(templateID, ssID)
    {
        $.post(this.get_ServiceUrl() + "/UpdateSellSheetTemplate", { tempID: templateID, sellSheetID: ssID }, this._onTemplateUpdateCallbackDelegate, "json");
    },

    _onTemplateUpdateCallback: function(r)
    {
        if (r && r.d && r.d.UpdateSellSheetTemplate)
        {
            var stepNum = $("#sidebarStepOrder").text();

            if (stepNum == 7)
                clientManager.set_SelectionData(clientManager.get_SelectionData());
        }
    }
    //
};
Pathfinder.UI.FormularySellSheetsApplication.registerClass("Pathfinder.UI.FormularySellSheetsApplication", Pathfinder.UI.Application);



//Customer Contact Reports Application business rules ---------------------------------------------------------------------------------------------------------------------------------------------
Pathfinder.UI.CustomerContactReportsApplication = function(id)
{
    Pathfinder.UI.CustomerContactReportsApplication.initializeBase(this, [id]);
};
Pathfinder.UI.CustomerContactReportsApplication.prototype =
{
    get_UrlName: function() { return "custom/" + this.get_clientKey() + "/customercontactreports"; },

    get_Title: function() { return ""; },

    activate: function(clientManager)
    {
        //Hide the channel menu
        if (clientManager) clientManager.get_ChannelMenu().set_visible(false);

        //Set effective channel to zero for proper loading of filters
        clientManager.set_EffectiveChannel(0);

        Pathfinder.UI.CustomerContactReportsApplication.callBaseMethod(this, "activate", [clientManager]);
    },

    getUrl: function(channelName, module, pageName, hasData, isCustom)
    {
        //not using channel menu anymore for standard reports
        channelName = "all";
        //

        //Does not require user selection to run so hasData must be true
        if (module == "customercontactreport")
            hasData = true;

        return Pathfinder.UI.CustomerContactReportsApplication.callBaseMethod(this, 'getUrl', [channelName, module, pageName, hasData, isCustom]);
    },

    get_OptionsServiceUrl: function(clientManager)
    {
        return this.get_ServiceUrl() + "/GetCCRModuleOptions";
    },

    getDefaultModule: function(clientManager)
    {
        return "customercontactreport";
    },

    get_ModuleOptionsUrl: function(clientManager)
    {
        if (clientManager.get_Module() != "customercontactreport")
            return this.getUrl("all", null, "filters.aspx", false);
        else
            return null;
    },

    //    configureDashboardTiles: function(clientManager)
    //    {
    //        Pathfinder.UI.CustomerContactReportsApplication.callBaseMethod(this, "configureDashboardTiles", [clientManager]);

    ////        $("#section2").removeClass("section2SR");
    //    },

    //    resetDashboardTiles: function(clientManager)
    //    {       
    //        Pathfinder.UI.CustomerContactReportsApplication.callBaseMethod(this, "resetDashboardTiles", [clientManager]);
    //    },

    _prepForAnimation: function()
    {
        if ($(".ccrPlanSelectView .mini").length > 0)
        {
            $(".ccrPlanSelectView").width("60%"); //% while sliding
            $(".ccrBusinessPlans").width("39%");
        }
        else
            $(".ccrPlanSelectView").width("99.6%"); //% while sliding

    },

    collapseSidePanel: function()
    {
        this._prepForAnimation();

        //*IMPORTANT - For simplicity I'm not passing a delegate - however when "resizeSection" is called "this" will not be correct.  If "this" is required for any reason the callback parameter should be changed to a delegate.
        minTile2(this.resizeSection);
    },
    expandSidePanel: function()
    {
        this._prepForAnimation();

        //*IMPORTANT - For simplicity I'm not passing a delegate - however when "resizeSection" is called "this" will not be correct.  If "this" is required for any reason the callback parameter should be changed to a delegate.
        maxTile2(this.resizeSection);
    },

    resize: function()
    {
        var browserWindow = $(window);
        var divHeight = browserWindow.height();
        var divWidth = browserWindow.width();
        var tile2Height = safeSub(divHeight, 105);
        var collaspeLft = $(".todaysAccounts2Expand").height();

        ////        $(".tileContainerHeader").show(); //remove this after custome options screen shot demo is gone

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
        this.resizeSection();


        Pathfinder.UI.CustomerContactReportsApplication.callBaseMethod(this, 'resize');
    },

    resizeSection: function()
    {
        var browserWindow = $(window);
        var divHeight = browserWindow.height();
        var divWidth = browserWindow.width();
        var tile2Height = divHeight / topSRHeight;
        var hdrElement = $("#divTile4 thead tr");
        var height = 20;

        if ($get("tile2"))
        {
            if (!$get("tile4"))
                $(".section2SR .enlarge").show();
            else
            {
                $(".srBottom .enlarge").show();
                $("#tile3 .enlarge, #tile3SR .enlarge, #divTile3Container .enlarge").hide(); //only show maximize for grids, not chart
            }
        } //sph 8/9/2010 - not sure why this line was commented out - required so "Max" button doesn't appear if browser is resized after max operation
        $("#maxTChart .enlarge, #maxSRMap .enlarge, #maxTBtm .enlarge, #maxChart .enlarge, #maxSRTile4 .enlarge, #maxSRTile5 .enlarge").hide();

        if (hdrElement.length > 0)
        {
            height = Sys.UI.DomElement.getBounds(hdrElement[0]).height;
        }
        //Tile 3 Properties (if Tile4 & 5 exist statement)
        var tile3Height;
        if (!$get("tile4") && !$get("tile4SR") && !$get("maxSRTile4"))
        {
            tile3Height = divHeight - 131;
        }
        else
        {
            tile3Height = divHeight * .40;
        }
        if (ie6)
        {
            $("#tile3 #divTile3Container ").css({
                height: tile3Height
            }
           );
        }

        $("#tile4 #divTile4, #tile5 #divTile5, #tile4SR #divTile4, #tile5SR #divTile5 ").css({
            height: safeSub((divHeight - tile3Height), 164)
        });
        $("#tile3 #divTile3, #tile3SR #divTile3").css({
            height: tile3Height, textAlign: "center", width: "auto", overflow: "hidden"
        }
           );

        //Fix for Customer Contact Drill Down Report
        if ($get("ctl00_Tile3_cdrilldowndata_gridCR"))
        {
            $("#ctl00_Tile3_cdrilldowndata_gridCR_GridData").height(tile3Height - $("#ctl00_Tile3_cdrilldowndata_gridCR_GridHeader").height());
            $("#tile3 #divTile3 #ctl00_Tile3_cdrilldowndata_gridCR").height(tile3Height);
        }


        var fullWidth = safeSub(divWidth, ($get("divTile3") ? Sys.UI.DomElement.getBounds($get("divTile3")).x + 16 : 0));
        //Meetings grid scroll height
        $(".ccrMeetings .dashboardTable .rgDataDiv").css({ height: safeSub((tile3Height - height), 220) });

        //dynamic sized page depending on selection
        if ($(".ccrPlanSelectView .mini").length > 0)
        {
            $(".ccrPlanSelectView .dashboardTable .rgDataDiv").height(115);

            if (fullWidth > 0)
            {
                var pwidth = Math.round(fullWidth * .6);

                if (ie6)
                    $(".ccrPlanSelectView").height(190).width(pwidth + 6);
                if (chrome || !flashSupported)
                    $(".ccrPlanSelectView").height(190).width(pwidth - 5);
                else
                    $(".ccrPlanSelectView").height(190).width(pwidth);

                $(".ccrBusinessPlans").width(fullWidth - pwidth - 5);
            }

            $(".ccrBusinessPlans, .ccrMeetings").show();
        }
        else
        {
            $(".ccrBusinessPlans, .ccrMeetings").hide();

            if (fullWidth > 0)
                $(".ccrPlanSelectView").height(safeSub(tile3Height, 4)).width(fullWidth);
            else
                $(".ccrPlanSelectView").height(safeSub(tile3Height, 4)).width($("#divTile3").width());

            $(".ccrPlanSelectView .dashboardTable .rgDataDiv").height(safeSub((tile3Height - height), 57));
        }

        //Fix height of radGrids for report/chart screens
        $("#divTile4 .rgDataDiv").height(safeSub($("#divTile4").height(), $("#divTile4 .rgHeaderDiv").height()));
        $("#divTile5 .rgDataDiv").height(safeSub($("#divTile5").height(), $("#divTile5 .rgHeaderDiv").height()));


        $("#tile3").removeClass("leftTile");
        $(".todaysAccounts1").css({
            padding: "0px",
            position: "relative"
        });

        //clears Telerik computed width in the headers for the data table
        resetGridHeadersX(500);
    }

};
Pathfinder.UI.CustomerContactReportsApplication.registerClass("Pathfinder.UI.CustomerContactReportsApplication", Pathfinder.UI.BasicApplication);





//Power Plan Rx Business Rules --------------------------------------------------------------------------------------------------------------------------------------------
Pathfinder.UI.PowerPlanRxApplication = function(id)
{
    Pathfinder.UI.PowerPlanRxApplication.initializeBase(this, [id]);
};

Pathfinder.UI.PowerPlanRxApplication.prototype =
{
    get_UrlName: function() { return "powerplanrx"; },

    get_Title: function() { return "Power Plan Rx"; },
    
    activate: function(clientManager)
    {
        //Hack to set Application to Today's Accounts before redirecting so that way when a user
        //comes back to Pathfinder, the app will load Today's Accounts
        clientManager._application = 1;
        window.location.href = "powerplanrx/all/home.aspx";
    },

    resize: function()
    {
        powerplanrx_content_resize();

        Pathfinder.UI.PowerPlanRxApplication.callBaseMethod(this, 'resize');
    },
    resizeSection: powerplanrx_section_resize,

    configureChannelMenu: function(clientManager)
    {
        //Hide the channel menu
        if (clientManager) clientManager.get_ChannelMenu().set_visible(false);

        //just need to clear since app is demo and not correctly set up - this override should be removed for real application when built
        //$loadMenuItems(clientManager.get_ChannelMenu(), clientManager.get_ChannelMenuOptions()[this.get_ApplicationID()], null, 0);
    },

    configureModuleMenu: function(clientManager, channel, currentModule)
    {
        currentModule = Pathfinder.UI.PowerPlanRxApplication.callBaseMethod(this, 'configureModuleMenu', [clientManager, channel, currentModule]);

        var menu = this.get_ModuleMenu();

        menu.addItem("opportunities", "Opportunities", "opportunities");
    }
};
Pathfinder.UI.PowerPlanRxApplication.registerClass("Pathfinder.UI.PowerPlanRxApplication", Pathfinder.UI.BasicApplication);





//Access Based Selling business rules ---------------------------------------------------------------------------------------------------------------------------------------------
Pathfinder.UI.AccessBasedSellingApplication = function(id)
{
    Pathfinder.UI.AccessBasedSellingApplication.initializeBase(this, [id]);
};
Pathfinder.UI.AccessBasedSellingApplication.prototype =
{
    get_UrlName: function()
    {
        return "accessbasedselling";
    },

    resize: function()
    {
        accessbasedselling_content_resize();

        Pathfinder.UI.AccessBasedSellingApplication.callBaseMethod(this, 'resize');
    },

    resizeSection: accessbasedselling_section_resize,

    configureDashboardTiles: function(clientManager)
    {
        //Hide the channel menu
        if (clientManager) clientManager.get_ChannelMenu().set_visible(false);

        //        if (clientManager.get_RegionList())
        //            clientManager.get_RegionList().set_visible(false);

        $("#divTile2Container .title").html("");
        $("#divTile2Container .pagination").hide();

        $(".tile2Div").hide();

        $("#trainingOpt").show();

        //        $("#divTile2Plans").show();

        var geog = clientManager.get_UserGeography();
        var o;
        //        if (geog && (geog.Area > 0 || geog.RegionID != null))
        //        {
        $("#fmASEngine").hide();

        o = $get("absMap");
        if (o)
            $(o).show();
        else
        {
            o = document.createElement("DIV");
            o.id = "absMap";
            o.style.overflow = "hidden";
            o.innerHTML = "<img src='" + clientManager.get_BasePath() + "/content/imagesabs/map/" + (geog && geog.RegionID ? geog.RegionID : "208") + ".png' />";
            $get("divTile1").appendChild(o);
        }
        //        }

        o = $get("absTerrs");
        if (o)
            $(o).show();
        else
        {
            o = document.createElement("DIV");
            o.id = "absTerrs";
            o.className = "tile2Div";
            $get("divTile2").appendChild(o);
            clientManager.loadPage(this.getUrl(null, null, "terrgrid.aspx?k=" + clientManager.get_UserKey()), "absTerrs");
        }

        maxTile1();
        Pathfinder.UI.AccessBasedSellingApplication.callBaseMethod(this, 'configureDashboardTiles', [clientManager]);
    },

    resetDashboardTiles: function(clientManager)
    {
        $("#absMap").hide();
        $("#absTerrs").hide();
        $("#fmASEngine").show();
        $("#trainingOpt").hide();

        //        if (clientManager.get_RegionList())
        //            clientManager.get_RegionList().set_visible(true);
    }
};
Pathfinder.UI.AccessBasedSellingApplication.registerClass("Pathfinder.UI.AccessBasedSellingApplication", Pathfinder.UI.Application);

//Formulary History Reporting business rules ---------------------------------------------------------------------------------------------------------------------------------------------
Pathfinder.UI.FormularyHistoryReportingApplication = function(id)
{
    Pathfinder.UI.FormularyHistoryReportingApplication.initializeBase(this, [id]);

    this._menuVisible = true; //used by collapse/expand to restore menu properly
};
Pathfinder.UI.FormularyHistoryReportingApplication.prototype =
{
    get_UrlName: function() { return "custom/" + this.get_clientKey() + "/formularyhistoryreporting"; },

    get_Title: function() { return "Report Selection"; },

    getUrl: function(channelName, module, pageName, hasData, isCustom)
    {
        channelName = "all";

        return Pathfinder.UI.FormularyHistoryReportingApplication.callBaseMethod(this, 'getUrl', [channelName, module, pageName, hasData, isCustom]);
    },

    get_OptionsServiceUrl: function(clientManager)
    {
        return this.get_ServiceUrl() + "/GetFormularyHistoryReportingModuleOptions";
    },

    getDefaultModule: function(clientManager)
    {
        return "coFormularyHxComparison";
    },

    get_ModuleOptionsUrl: function(clientManager)
    {
        //Hide the channel menu
        if (clientManager) clientManager.get_ChannelMenu().set_visible(false);

        //for these modules filter would not be database driven
        //if (clientManager.get_Module() == "coFormularyHxComparison")
        return this.getUrl("all", null, clientManager.get_Module() + "_filters.aspx", false);
        //else
        //  return null;
    },

    collapseSidePanel: function()
    {
        //this._menuVisible = clientManager.get_ChannelMenu().get_visible();
        //if (clientManager) clientManager.get_ChannelMenu().set_visible(false);
        minTile2();
    },

    expandSidePanel: function()
    {
        maxTile2();
        //if (clientManager) clientManager.get_ChannelMenu().set_visible(this._menuVisible === true);
    },

    resize: function()
    {
        fhr_content_resize();

        Pathfinder.UI.FormularyHistoryReportingApplication.callBaseMethod(this, 'resize');
    },

    resizeSection: fhr_section_resize

};
Pathfinder.UI.FormularyHistoryReportingApplication.registerClass("Pathfinder.UI.FormularyHistoryReportingApplication", Pathfinder.UI.BasicApplication);


// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//Prescriber reporting -- starts

Pathfinder.UI.PrescriberReportingApplication = function(id)
{
    Pathfinder.UI.PrescriberReportingApplication.initializeBase(this, [id]);

    this._timeFrameLoadedDelegate = null;
    this._toolbarLoadedDelegate = null;
};

Pathfinder.UI.PrescriberReportingApplication.prototype =
{
    get_Title: function() { return "Report Type"; },
    get_UrlName: function() { return "prescriberreporting"; },

    getUrl: function(channelName, module, pageName, hasData, isCustom)
    {
        //not using channel menu  
        channelName = "all";

        return Pathfinder.UI.PrescriberReportingApplication.callBaseMethod(this, 'getUrl', [channelName, module, pageName, hasData, isCustom]);
    },

    get_OptionsServiceUrl: function(clientManager)
    {
        return this.get_UrlName() + "/services/prescriberreportingdataservice.svc" + "/GetPrescriberModuleOptions";
    },

    getDefaultModule: function(clientManager)
    {
        return "prescribertrending";
    },

    get_ModuleOptionsUrl: function(clientManager)
    {
        //use module specific filter page     
        if (clientManager.get_Module())
            return this.getUrl("all", null, clientManager.get_Module() + "_filters.aspx", false);
        else
            return null;
    },

    activate: function(clientManager)
    {
        if (!this._moduleChangingDelegate)
        {
            this._moduleChangingDelegate = Function.createDelegate(this, this._moduleChanging);
            clientManager.add_moduleChanging(this._moduleChangingDelegate);
        }
        $(clientManager.get_ChannelMenu().get_element()).remove().appendTo("#channelSelectorContainer");

        this._timeFrameLoadedDelegate = Function.createDelegate(this, this._onTimeFrameLoaded);
        this._toolbarLoadedDelegate = Function.createDelegate(this, this._onToolbarLoaded);

        clientManager.add_pageLoaded(this._timeFrameLoadedDelegate, "timeframeArea");
        clientManager.add_pageLoaded(this._toolbarLoadedDelegate, "toolbarArea");

        //Hide the channel menu
        if (clientManager) clientManager.get_ChannelMenu().set_visible(false);

        Pathfinder.UI.PrescriberReportingApplication.callBaseMethod(this, "activate", [clientManager]);
    },

    deactivate: function(clientManager)
    {
        Pathfinder.UI.PrescriberReportingApplication.callBaseMethod(this, "deactivate", [clientManager]);

        $(clientManager.get_ChannelMenu().get_element()).remove().appendTo(".viewSelect");

        clientManager.remove_pageLoaded(this._timeFrameLoadedDelegate, "timeframeArea");
        clientManager.remove_pageLoaded(this._toolbarLoadedDelegate, "toolbarArea");
        delete (this._timeFrameLoadedDelegate);
        delete (this._toolbarLoadedDelegate);
    },

    getDefaultData: function(clientManager)
    {
        ///<summary>Returns the default data required for an application to initialize the dashboard after an application has been selected.  It is up to the application manager user to determine how this value is applied.  For example clientManager will set the SelectionData property to this value after the Application property has changed.</summary>
        ///<param name="clientManager" type="Object"></param>
        ///<returns type="Object" />
        return Pathfinder.UI.PrescriberReportingApplication.callBaseMethod(this, "getDefaultData", [clientManager]);

    },

    onModuleChanging: function(cm, newModule)
    {
        if (cm.get_UIReady())
            cm.clearSelectionData(true);
    },

    _moduleChanging: function(sender, args)
    {
        this.onModuleChanging(sender, args.get_Value());
    },

    _onToolbarLoaded: function(sender, args)
    {
        $("#toolbarArea").css("visibility", "hidden").css("visibility", ""); //ie6 hacking;
    },

    _onTimeFrameLoaded: function(sender, args)
    {
        $(".navbar2").show();

        var data = clientManager.get_SelectionData();
        var calendarRolling;

        if (data)
            calendarRolling = data["Calendar_Rolling"];

        //Check if Calendar_Rolling has a value
        if (calendarRolling)
        {
            if (typeof calendarRolling == "object")
                calendarRolling = calendarRolling.value;
        }

        if ((typeof (calendarRolling) != "undefined")) //If there is selection data, set the value
        {
            if (calendarRolling == 'Calendar')
            {
                $("#ctl00_partialPage_filtersContainer_CalendarRolling_Calendar_Rolling_0").attr('checked', 'checked');
                showCalendar();
            }
            else
            {
                $("#ctl00_partialPage_filtersContainer_CalendarRolling_Calendar_Rolling_1").attr('checked', 'checked');
                showRolling();
            }
        }
        else //Otherwise set default value to Calendar
            $("#ctl00_partialPage_filtersContainer_CalendarRolling_Calendar_Rolling_0").attr('checked', 'checked');

        this.resizeSection();
    },

    configureDashboardTiles: function(clientManager)
    {

        this.onModuleChanging(clientManager, clientManager._module);

        $("<div />").appendTo(".navbar2").attr("id", "timeframeArea");
        $(".navbar .clearAll").before("<div id='toolbarArea' />");

        clientManager.loadPage(this.getUrl("all", "all", "toolbar.aspx"), "toolbarArea");

        Pathfinder.UI.PrescriberReportingApplication.callBaseMethod(this, 'configureDashboardTiles', [clientManager]);


        $(".navbar2").show().css({ "margin-left": "208px" });

    },

    resetDashboardTiles: function(clientManager)
    {

        $("#timeframeArea").remove();
        $("#toolbarArea").remove();

        $(".navbar2").hide().css({ "margin-left": "" });

        $(".todaysAccounts1").show();

        Pathfinder.UI.PrescriberReportingApplication.callBaseMethod(this, 'resetDashboardTiles', [clientManager]);
    },

    collapseSidePanel: function()
    {
        $(".navbar2").animate({ "margin-left": "34px" }, animationSpeed);
        minTile2(function()
        {
            //stick with standard names for MA so setting back to originals after min op
            $("#tile3SR").attr({ id: "tile3" });
            $("#tile4SR").attr({ id: "tile4" });
            $("#tile5SR").attr({ id: "tile5" });

            todaysanalytics_section_resize();

            if (ie6)
                $("#divYearContainer, #divQuarterContainer, #divMonthContainer, #divRollingContainer").css("visibility", "hidden").css("visibility", ""); //another ie6 hack job
        });
    },
    expandSidePanel: function()
    {
        $(".navbar2").animate({ "margin-left": "208px" }, animationSpeed);
        maxTile2(function()
        {
            todaysanalytics_section_resize();

            if (ie6)
                $("#divYearContainer, #divQuarterContainer, #divMonthContainer, #divRollingContainer").css("visibility", "hidden").css("visibility", ""); //another ie6 hack job
        });
    },

    resize: function()
    {
        todaysanalytics_content_resize();

        Pathfinder.UI.PrescriberReportingApplication.callBaseMethod(this, 'resize');
    },

    resizeSection: todaysanalytics_section_resize

};
Pathfinder.UI.PrescriberReportingApplication.registerClass("Pathfinder.UI.PrescriberReportingApplication", Pathfinder.UI.BasicApplication);

//Prescriber reporting -- ends

//Activity Reporting Application business rules ---------------------------------------------------------------------------------------------------------------------------------------------
Pathfinder.UI.ActivityReportingApplication = function(id)
{
    Pathfinder.UI.ActivityReportingApplication.initializeBase(this, [id]);
};
Pathfinder.UI.ActivityReportingApplication.prototype =
{
    dispose: function()
    {
        this._deleteObjects();

        Pathfinder.UI.ActivityReportingApplication.callBaseMethod(this, 'dispose');
    },

    _deleteObjects: function()
    {
        delete (this._onSaveCallbackDelegate);
    },

    activate: function(clientManager)
    {
        if (!this._onSaveCallbackDelegate)
        {
            this._onSaveCallbackDelegate = Function.createDelegate(this, this._onSaveCallback);
            clientManager.add_formSubmitted(this._onSaveCallbackDelegate);
        }

        //Hide the channel menu
        if (clientManager) clientManager.get_ChannelMenu().set_visible(false);

        Pathfinder.UI.ActivityReportingApplication.callBaseMethod(this, "activate", [clientManager]);
    },

    deactivate: function(clientManager)
    {
        Pathfinder.UI.ActivityReportingApplication.callBaseMethod(this, "deactivate", [clientManager]);

        clientManager.remove_formSubmitted(this._onSaveCallbackDelegate);

        this._deleteObjects();
    },

    get_UrlName: function() { return "custom/" + this.get_clientKey() + "/activityreporting"; },

    get_Title: function() { return ""; },

    getUrl: function(channelName, module, pageName, hasData, isCustom)
    {
        //not using channel menu anymore for standard reports
        channelName = "all";
        //

        //Does not require user selection to run so hasData must be true
        if (module == "activityentry")
            hasData = true;

        return Pathfinder.UI.ActivityReportingApplication.callBaseMethod(this, 'getUrl', [channelName, module, pageName, hasData, isCustom]);
    },

    get_OptionsServiceUrl: function(clientManager)
    {
        return this.get_ServiceUrl() + "/GetActivityReportingModuleOptions";
    },

    getDefaultModule: function(clientManager)
    {
        return "activityentry";
    },

    get_ModuleOptionsUrl: function(clientManager)
    {
        if (clientManager.get_Module() != "activityentry")
            return this.getUrl("all", null, clientManager.get_Module() + "_filters.aspx", false);
        else
            return null;
    },

    //    configureDashboardTiles: function(clientManager)
    //    {
    //        Pathfinder.UI.CustomerContactReportsApplication.callBaseMethod(this, "configureDashboardTiles", [clientManager]);

    ////        $("#section2").removeClass("section2SR");
    //    },

    //    resetDashboardTiles: function(clientManager)
    //    {       
    //        Pathfinder.UI.CustomerContactReportsApplication.callBaseMethod(this, "resetDashboardTiles", [clientManager]);
    //    },

    _onSaveCallback: function(sender, args)
    {
        if (args.result.Success)
        {
            //Reload the page (function located in ActivityEntryScript.ascx)

            showGrid(null);
            $alert("Activity Details have been successfully submitted.", "Activity Entry")
        }
        else
        {
            //alert("Error: To be completed.");
        }
    },

    _prepForAnimation: function()
    {
        if ($(".ccrPlanSelectView .mini").length > 0)
        {
            $(".ccrPlanSelectView").width("60%"); //% while sliding
            $(".ccrBusinessPlans").width("39%");
        }
        else
            $(".ccrPlanSelectView").width("99.6%"); //% while sliding

    },

    collapseSidePanel: function()
    {
        this._prepForAnimation();

        //*IMPORTANT - For simplicity I'm not passing a delegate - however when "resizeSection" is called "this" will not be correct.  If "this" is required for any reason the callback parameter should be changed to a delegate.
        minTile2(this.resizeSection);
    },
    expandSidePanel: function()
    {
        this._prepForAnimation();

        //*IMPORTANT - For simplicity I'm not passing a delegate - however when "resizeSection" is called "this" will not be correct.  If "this" is required for any reason the callback parameter should be changed to a delegate.
        maxTile2(this.resizeSection);
    },

    resize: function()
    {
        var browserWindow = $(window);
        var divHeight = browserWindow.height();
        var divWidth = browserWindow.width();
        var tile2Height = safeSub(divHeight, 105);
        var collaspeLft = $(".todaysAccounts2Expand").height();

        ////        $(".tileContainerHeader").show(); //remove this after custome options screen shot demo is gone

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
        this.resizeSection();


        Pathfinder.UI.ActivityReportingApplication.callBaseMethod(this, 'resize');
    },

    resizeSection: function()
    {
        var browserWindow = $(window);
        var divHeight = browserWindow.height();
        var divWidth = browserWindow.width();
        var tile2Height = divHeight / topSRHeight;
        var hdrElement = $("#divTile4 thead tr");
        var height = 20;

        if ($get("tile2"))
        {
            if (!$get("tile4"))
                $(".section2SR .enlarge").show();
            else
            {
                $(".srBottom .enlarge").show();
                $("#tile3 .enlarge, #tile3SR .enlarge, #divTile3Container .enlarge").hide(); //only show maximize for grids, not chart
            }
        } //sph 8/9/2010 - not sure why this line was commented out - required so "Max" button doesn't appear if browser is resized after max operation
        $("#maxTChart .enlarge, #maxSRMap .enlarge, #maxTBtm .enlarge, #maxChart .enlarge, #maxSRTile4 .enlarge, #maxSRTile5 .enlarge").hide();

        if (hdrElement.length > 0)
        {
            height = Sys.UI.DomElement.getBounds(hdrElement[0]).height;
        }
        //Tile 3 Properties (if Tile4 & 5 exist statement)
        var tile3Height;
        if (!$get("tile4") && !$get("tile4SR") && !$get("maxSRTile4"))
        {
            tile3Height = divHeight - 131;
        }
        else
        {
            tile3Height = divHeight * .40;
        }
        if (ie6)
        {
            $("#tile3 #divTile3Container ").css({
                height: tile3Height
            }
           );
        }

        $("#tile4 #divTile4, #tile5 #divTile5, #tile4SR #divTile4, #tile5SR #divTile5 ").css({
            height: safeSub((divHeight - tile3Height), 164)
        });
        $("#tile3 #divTile3, #tile3SR #divTile3").css({
            height: tile3Height, textAlign: "center", width: "auto", overflow: "hidden"
        }
           );

        //Fix for Customer Contact Drill Down Report
        if ($get("ctl00_Tile3_adrilldowndata_gridDrillDown"))
        {
            $("#ctl00_Tile3_adrilldowndata_gridDrillDown_GridData").height(tile3Height - $("#ctl00_Tile3_adrilldowndata_gridDrillDown_GridHeader").height());
            $("#tile3 #divTile3 #ctl00_Tile3_adrilldowndata_gridDrillDown").height(tile3Height);
        }


        var fullWidth = safeSub(divWidth, ($get("divTile3") ? Sys.UI.DomElement.getBounds($get("divTile3")).x + 16 : 0));
        //Meetings grid scroll height
        $(".ccrMeetings .dashboardTable .rgDataDiv").css({ height: safeSub((tile3Height - height), 220) });

        //dynamic sized page depending on selection
        if ($(".ccrPlanSelectView .mini").length > 0)
        {
            $(".ccrPlanSelectView .dashboardTable .rgDataDiv").height(115);

            if (fullWidth > 0)
            {
                var pwidth = Math.round(fullWidth * .6);

                if (ie6)
                    $(".ccrPlanSelectView").height(190).width(pwidth + 6);
                if (chrome || !flashSupported)
                    $(".ccrPlanSelectView").height(190).width(pwidth - 5);
                else
                    $(".ccrPlanSelectView").height(190).width(pwidth);

                $(".ccrBusinessPlans").width(fullWidth - pwidth - 5);
            }

            $(".ccrBusinessPlans, .ccrMeetings").show();
        }
        else
        {
            $(".ccrBusinessPlans, .ccrMeetings").hide();

            if (fullWidth > 0)
                $(".ccrPlanSelectView").height(safeSub(tile3Height, 4)).width(fullWidth);
            else
                $(".ccrPlanSelectView").height(safeSub(tile3Height, 4)).width($("#divTile3").width());

            $(".ccrPlanSelectView .dashboardTable .rgDataDiv").height(safeSub((tile3Height - height), 57));
        }

        //Fix height of radGrids for report/chart screens
        $("#divTile4 .rgDataDiv").height(safeSub($("#divTile4").height(), $("#divTile4 .rgHeaderDiv").height()));
        $("#divTile5 .rgDataDiv").height(safeSub($("#divTile5").height(), $("#divTile5 .rgHeaderDiv").height()));


        $("#tile3").removeClass("leftTile");
        $(".todaysAccounts1").css({
            padding: "0px",
            position: "relative"
        });

        //clears Telerik computed width in the headers for the data table
        resetGridHeadersX(500);
    }

};
Pathfinder.UI.ActivityReportingApplication.registerClass("Pathfinder.UI.ActivityReportingApplication", Pathfinder.UI.BasicApplication);
//Activity Reporting Ends

Pathfinder.UI.CommandEventArgs = function(commandName, successful)
{
    Pathfinder.UI.CommandEventArgs.initializeBase(this);

    this._successful = successful;
    this._commandName = commandName;
};
Pathfinder.UI.CommandEventArgs.prototype =
{
    get_Successful: function() { return this._successful; },
    get_CommandName: function() { return this._commandName; }

};
Pathfinder.UI.CommandEventArgs.registerClass('Pathfinder.UI.CommandEventArgs', Sys.CancelEventArgs);

Pathfinder.UI.PropertyChangingEventArgs = function(value)
{
    Pathfinder.UI.PropertyChangingEventArgs.initializeBase(this);

    this._value = value;
};
Pathfinder.UI.PropertyChangingEventArgs.prototype =
{
    get_Value: function() { return this._value; }
};
Pathfinder.UI.PropertyChangingEventArgs.registerClass('Pathfinder.UI.PropertyChangingEventArgs', Sys.CancelEventArgs);


Pathfinder.UI.FormSubmittingEventArgs = function(containerID, target, url, data)
{
    Pathfinder.UI.FormSubmittingEventArgs.initializeBase(this);

    this._target = target;
    this._containerID = containerID;
    this._url = url;
    this._data = data;
};
Pathfinder.UI.FormSubmittingEventArgs.prototype =
{
    get_containerID: function() { return this._containerID; },
    get_target: function() { return this._target; },
    get_url: function() { return this._url; },
    get_data: function() { return this._data; }

};
Pathfinder.UI.FormSubmittingEventArgs.registerClass('Pathfinder.UI.FormSubmittingEventArgs', Sys.CancelEventArgs);


// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
Pathfinder.UI.ClientManager = function(element)
{
    //If no element is defined use as regular object (some functions may not work)
    if (element)
        Pathfinder.UI.ClientManager.initializeBase(this, [element]);

    this._basePath = "";

    this._initialized = false;
    this._mapReady = !flashSupported;

    this._contextData = {};
    this._controls = {};
    this._history = [];

    //NOT USED - jQ 1.4.2 - SPH 4/5/2010
    //    this._ajaxRequestTimeout = 10000;

    this._planInfoGridFilterTimeout = 500;

    this._applicationMenuOptions = [];
    this._channelMenuOptions = { 1: [] };
    this._drugListOptions = { 0: [] };
    this._marketBasketListOptions = [];
    this._regionListOptions = [];
    //    this._regionGeographyListOptions = {};

    this._trackingEnabled = false;
    this._currentHistoryIndex = -1;

    this._applicationManager = null;
    this._clientAccount = new Pathfinder.UI.ClientAccount();

    //client state
    this._userKey = "";
    this._clientKey = "pinso";
    this._application = Pathfinder.UI.Applications.TodaysAccounts;
    this._channel = [1];               //section and effective section should be the same unless section = 0 (All sections) then effective section can be any value
    this._effectiveChannel = 1;

    this._module = null;
    this._region = null;
    this._drug = null;
    this._marketBasket = null;
    this._selectionData = null;
    //
    this._userGeography = { "CenterX": 0, "CenterY": 0, "Area": 0 };

    this._lastSearchValue = {};


    //delegates
    this._onLoadPageDelegate = null;
    this._onPlanGridRowSelectedDelegate = null;
    this._onPlanGridDataBoundDelegate = null;
    //    this._onDrugListChangedDelegate = null;
    //    this._onMarketBasketListChangedDelegate = null;
    this._onContactSearchDelegate = null;
    this._onFavoriteSavedDelegate = null;
    this._onHistoryChangeDelegate = null;
    this._onApplicationSelectionChangedDelegate = null;
    this._onChannelSelectionChangedDelegate = null;
    this._onShowFavoritesDelegate = null;
    this._onModuleMenuItemClickedDelegate = null;
    //    this._onRegionListChangedDelegate = null;

    //timer handles
    //    this._pageRequestTimeoutHandle = 0;

    //components
    this._moduleOptions = null;
    this._clientHasCustomPlans = false;
};

$createFilter = Pathfinder.UI.ClientManager.createFilter = function(name, value, filterType, dataType, queryExtension)
{
    ///<summary>Returns a new Telerik.Web.UI.GridFilterExpression instance based on the specified parameters.</summary>
    ///<param name="name" type="String">Name of the filter.</param>
    ///<param name="value" type="Variant">Value of the filter.</param>
    ///<param name="filterType" type="String">Type of operation such as 'EqualTo', 'Between', or 'StartsWith' (see Telerik documentation for complete list).  If no value is specified then EqualTo is assumed.</param>
    ///<param name="dataType" type="String">Data type that describes the value of the filter.  Parameter values include 'System.String', 'System.Int32', and 'System.DataTime' (see Telerik documentation for complete list).</param>
    ///<returns type="Telerik.Web.UI.GridFilterExpression" />

    var filter = new Pathfinder.UI.GridFilterExpression() //Telerik.Web.UI.GridFilterExpression();

    if (queryExtension)
        filter.set_isExtension(queryExtension);

    var valIsObject = typeof (value) == "object";

    //set defaults
    if (!dataType)
    {
        //If not a number default to string otherwise leave blank and what happens happens
        var checkVal = value;
        if (valIsObject) //if object assume array and do check on first value
            checkVal = checkVal[0];
        if (isNaN(checkVal))
            dataType = "System.String";
    }

    if (!filterType)
    {
        if (!valIsObject)
            filterType = "EqualTo";
        else
        {
            if (value == null) return; //don't want to create a custom null value filter - has no purpose

            filterType = "Custom";
            //flatten array
            value = value.join(",");
        }
    }
    else if (filterType == "Between")
    {
        if (valIsObject)
            value = value.join(" ");
        else
            filterType = "EqualTo"; //only 1 value so can't be "Between"
    }

    if (typeof (value) == "string") value = value.replace(new RegExp("'", "ig"), "%27");

    filter.set_fieldValue(value);
    filter.set_filterFunction(filterType);
    filter.set_columnUniqueName(name);
    filter.set_fieldName(name);
    filter.set_dataTypeName(dataType);

    return filter;
};

//NO LONGER USED - SPH 4/5/2010
//$copyGridFilters = Pathfinder.UI.ClientManager.copyGridFilters = function(sourceGrid, destinationGrid)
//{
//    ///<summary>Copies filters from one grid to another.</summary>
//    ///<param name="sourceGrid" type="Object">Grid control that contains the filters to copy.</param>
//    ///<param name="destinationGrid" type="Object">Grid control that will be updated with the source grid's filters.</param>
//    ///<returns type="void" />

//    var mt = destinationGrid.get_masterTableView();
//    var filters = mt.get_filterExpressions();

//    var filtersMaster = sourceGrid.get_masterTableView().get_filterExpressions();

//    for (var i = 0; i < filtersMaster.get_count(); i++)
//    {
//        filters.add(filtersMaster.getItem(i));
//    }
//};

$setGridFilter = Pathfinder.UI.ClientManager.setGridFilter = function(gridOrMasterTableView, name, value, filterType, dataType, queryExtension)
{
    ///<summary>Adds a filter to the specified Grid or MasterTableView control.  If the filter already exists it will be removed before the new filter is added.  Additionally if the value parameter is null the filter will simply be removed.</summary>
    ///<param name="gridOrMasterTableView">Grid or MasterTableView control that the filter will be added to.</param>
    ///<param name="name" type="String">Name of the filter.</param>
    ///<param name="value" type="Variant">Value of the filter.  If null an existing filter with the same name is removed.</param>
    ///<param name="filterType" type="String">Type of operation such as 'EqualTo', 'Between', or 'StartsWith' (see Telerik documentation for complete list).  If no value is specified then EqualTo is assumed.</param>
    ///<param name="dataType" type="String">Data type that describes the value of the filter.  Parameter values include 'System.String', 'System.Int32', and 'System.DataTime' (see Telerik documentation for complete list).  If no value is specified System.String is assumed.</param>
    ///<returns type="void" />

    if (!gridOrMasterTableView) throw new Error("ClientManager.setGridFilter: gridOrMasterTableView parameter must be specified.");
    if (!name) throw new Error("ClientManager.setGridFilter: name parameter must be specified.");

    var masterTableView = gridOrMasterTableView;
    if (masterTableView.get_masterTableView)
        masterTableView = masterTableView.get_masterTableView();

    $clearGridFilter(masterTableView, name);

    if (value != null)
    {
        //set defaults
        if (!filterType && typeof (value) != "object") filterType = "EqualTo";
        if (!dataType) dataType = "System.String";

        //add filter
        masterTableView.get_filterExpressions().add($createFilter(name, value, filterType, dataType, queryExtension));
    }
};

$clearGridFilter = Pathfinder.UI.ClientManager.clearGridFilter = function(gridOrMasterTableView, name)
{
    ///<summary>Removes a filter to the specified Grid or MasterTableView control.</summary>
    ///<param name="gridOrMasterTableView">Grid or MasterTableView control that the filter will be removed from.</param>
    ///<param name="name" type="String">Name of the filter.</param>    
    var masterTableView = gridOrMasterTableView;
    if (masterTableView.get_masterTableView)
        masterTableView = masterTableView.get_masterTableView();

    if (!masterTableView) return;
    //hack to remove item
    masterTableView.get_filterExpressions()._array = $.grep(masterTableView.get_filterExpressions()._array, function(i) { return i != null && i.get_fieldName() != name; }, false);
    //hack to remove item        
};

$clearGridFilterSelections = Pathfinder.UI.ClientManager.clearGridFilterSelections = function(gridOrMasterTableView)
{
    ///<summary>Clears input controls in a grid's filter row.  This method is intended as a helper to reset controls and does not remove filters that have already been added to a grid.</summary>
    ///<param name="gridOrMasterTableView" type="Object">Grid or MasterTableView control that should be cleared.</param>  
    if (gridOrMasterTableView.get_masterTableView)
        gridOrMasterTableView = gridOrMasterTableView.get_masterTableView();

    var base = $(gridOrMasterTableView.get_tableFilterRow()).children("TD");
    //Get all input controls in filter row excluding RadCombo clientState input controls
    var count = 0;
    base.find("INPUT[value != ''][type=text]").each(function() { count++; this.value = ""; });

    base.find("div table").each(function()
    {
        //Don't increment "count" here since textboxes for combos already accounted for in previous filter.
        //if current index is > 0 then select first item (have to use internal variables to avoid event handlers from firing and causing grid to rebind
        var c = this.parentNode.control;
        if (c && c.get_selectedIndex() > 0)
        {
            c._selectedIndex = 0;
            c._selectedItem = c.get_items().getItem(0);
        }
    });

    return count; // return how many cleared
};

//$setServiceLocationByGeography = Pathfinder.UI.ClientManager.setServiceLocationByGeography = function(cm, grid, geogID)
//{
//    ///<summary>Sets the Telerik RadGrid's ClientSettings.DataBinding.Location property to the appropriate Pathfinder service based on the specified Geography ID.  If the geogID is US or a state the default service, PathfinderService.svc, is used otherwise the client specific service, PathfinderClientService.svc, is used.</summary>
//    ///<param name="grid" type="Telerik RadGrid">Grid to update</summary>
//    ///<param name=geogID" type="String">Geography ID which can be US (National), a state, or territory id</summary>

//    if (geogID)
//    {
//        if (typeof geogID == "object")
//            geogID = geogID.value;
//    }

//    //if geogID not set or geogID is US or geogID is a state
//    if (!geogID || geogID.toUpperCase() == "US" || clientManager.get_States()[geogID])
//    {
//        grid.ClientSettings.DataBinding.Location = cm.get_ApplicationManager().get_ServiceUrl();
//    }
//    else //geogid is not state or US so assume region
//    {
//        grid.ClientSettings.DataBinding.Location = cm.get_ApplicationManager().get_ClientServiceUrl(); //"services/pathfinderclientservice.svc";
//    }
//};

$loadPinsoListItems = Pathfinder.UI.ClientManager.loadPinsoListItems = function(control, items, defaultItem, selectedValue, ignoreDefaultSelect)
{
    ///<summary>Loads a custom control with a specified list of items.</summary>
    ///<param name="control" type="Object">Custom AJAX.Net control.</summary>
    ///<param name="items" type="Object">Collection or array of items to add to the list.  Individual items must have an ID, Name, and optionally a Value property.</param>
    ///<param name="defaultItem"type="Object">A single item to add as the first entry in the list if the "items" collection does not already contain a default.</param>

    if (typeof (control) == "string")
        control = $find(control);

    if (control)
    {
        if (!control.clear) throw new Error("control does not contain a method 'clear'.");
        if (!control.addItem) throw new Error("control does not contain a method 'addItem'.");
        if (!control.selectItemAt) throw new Error("control does not contain a method 'selectItemAt'.");

        var item;
        var mi;
        control.clear();

        var selected = false;
        if (items)
        {
            if (defaultItem)
                control.addItem(defaultItem.ID, defaultItem.Name, defaultItem.Value ? defaultItem.Value : defaultItem.ID);

            var value;
            var id;
            var text;
            for (var i in items)
            {
                item = items[i];

                id = item.ID != null ? item.ID : item.id;

                if (id != null)
                {
                    value = item.Value != null ? item.Value : item.value;
                    value = value != null ? value : id;
                    text = item.Name != null ? item.Name : item.text;

                    control.addItem(id, text, value);
                    if (item.Selected && !ignoreDefaultSelect)
                        control.selectItem(value);
                }
            }

            if (!selectedValue)
            {
                control.selectItemAt(0);
            }
            else
                control.selectItem(selectedValue);
        }
    }
};

$refreshMenuOptions = Pathfinder.UI.ClientManager.refreshMenuOptions = function(menu, selectedValue)
{
    ///<summary>Reconfigures menus based on current selections.  The purpose is to change the parent menu item's text to be the selected item's text and change the selected item's highlighting.</summary>

    if (!menu) return;

    var item = menu.get_items().getItem(0);
    if (!item) return;

    var menuItems = item.get_items();
    var menuItem;
    count = menuItems.get_count();

    $(menu.get_element()).find(".menuClicked").removeClass("menuClicked");

    var found = false;

    menuItem = menu.findItemByValue(selectedValue);
    if (menuItem)
    {
        item.set_text(menuItem.get_text());
        $(menuItem.get_element()).addClass("menuClicked");
        found = true;
    }

    if (!found) //selected value no longer valid 
    {
        menuItem = menuItems.getItem(0);
        if (menuItem)
        {
            item.set_text(menuItem.get_text());
            $(menuItem.get_element()).addClass("menuClicked");
            selectedValue = menuItem.get_value();
        }
    }

    //hack - corrects width of main menu item since we are manually adjusting
    item._clearWidth();
    item._setWidth(item._getWidth() + "px");
    //hack - corrects width of main menu item since we are manually adjusting    

    menu._clicked = false; //have to set _clicked to false otherwise menu reopens automatically or at least opens on mouse over instead of click
    menu.close();

    return selectedValue;
};

$refreshChannelMenu = Pathfinder.UI.ClientManager.refreshChannelMenu = function(selectedValue)
{
    ///<summary>Reconfigures channel menu based on current selections.</summary>

    var menu = clientManager.get_ChannelMenuCheckBoxList();

    if (!menu) return;

    var channel;

    for (channel in selectedValue)
        menu.selectItem(selectedValue[channel]);

    //Update menu selected item text
    $updateCheckboxDropdownText("ctl00_main_subheader1_channelMenu", "Channel_Menu");
};

$loadMenuItems = Pathfinder.UI.ClientManager.loadMenuItems = function(menu, items, defaultItem, selectedValue)
{
    ///<summary>Loads a Telerik menu control with a specified list of items.</summary>
    ///<param name="menu" type="Object">Telerik Menu control.</param>
    ///<param name="items" type="Object">Collection or array of items to add to the list.  Individual items must have an ID or Key property to define the item's id and a Name or Value property to define the item's text.</param>
    ///<param name="defaultItem" type="Object">A single item to add as the first entry in the list if the "items" collection does not already contain a default.  Item must have a value and text property.</param>
    ///<param name="defaultItem">Default selected value.  If not specified the first item is selected.</param>

    var value;

    if (menu && items)
    {
        var item;
        var mi;
        var id;
        var text;
        var mnu = menu; //save param because if it is a submenu then we need to set "menu" to master menu control
        var isSub = false;

        if (!mnu.get_defaultGroupSettings)
        {
            menu = mnu.get_menu();
            isSub = true;
        }

        var width = parseInt(menu.get_defaultGroupSettings().get_width());

        var col = mnu.get_items().getItem(0);
        if (!col && !isSub)
        {
            mi = new Telerik.Web.UI.RadMenuItem();
            //            mi.set_text("");
            mnu.get_items().add(mi);
            col = mi;

            col.get_groupSettings().set_width(menu.get_defaultGroupSettings().get_width());
            //            col.get_groupSettings().set_height(menu.get_defaultGroupSettings().get_height());
        }
        else if (isSub)
            col = mnu;


        col = col.get_items();

        for (var x = 0; x < col.get_count(); x++)
        {
            mi = col.getItem(x);
            $(mi.get_element()).unbind('mouseenter mouseleave');
        }
        col.clear();

        var tempO = $("#__tempSpan");
        if (tempO.length == 0)
        {
            tempO = $("<div />").attr("id", "__tempSpan").css({ "position": "absolute", "left": -111111 });
            document.body.appendChild(tempO[0]);
            //$(document).append(tempO[0]);
        }

        if (defaultItem)
        {
            mi = new Telerik.Web.UI.RadMenuItem();
            mi.set_value(defaultItem.value);
            mi.set_text(defaultItem.text);
            col.add(mi);
            $(mi.get_element()).hover(function() { $(this).addClass('menuHoverItem') }, function() { $(this).removeClass('menuHoverItem'); }).children("a").attr("title", tempO.text(defaultItem.text).width() > width ? defaultItem.text : "");
        }

        for (var i in items)
        {
            item = items[i];

            //If ID available use ID else Key - if neither of those are present use i.
            id = (item.ID ? item.ID : item.Key ? item.Key : i);
            //If Name available use Name else Value - if neither of those are present use item.
            text = (item.Name ? item.Name : item.Value ? item.Value : item);

            if (id != null)
            {
                mi = new Telerik.Web.UI.RadMenuItem();
                mi.set_value(id);
                mi.set_text(text);
                col.add(mi);
                $(mi.get_element()).hover(function() { $(this).addClass('menuHoverItem') }, function() { $(this).removeClass('menuHoverItem'); }).children("a").attr("title", tempO.text(text).width() > width ? text : "");
            }
        }

        if (col.get_count() > 0)
            value = col.getItem(0).get_value();

        //hide menu if only option is default item
        menu.set_visible(true);
        if (col.get_count() == 0 || (col.get_count() == 1 && col.getItem(0).get_value() == 0))
            menu.set_visible(false);
        else if (col.get_count() > 0)
            selectedValue = $refreshMenuOptions(menu, selectedValue ? selectedValue : value);
    }

    return selectedValue ? selectedValue : value;
};

$loadListItems = Pathfinder.UI.ClientManager.loadListItems = function(list, items, defaultItem, selectedValue, varID, varName)
{
    ///<summary>Loads a Telerik RadCombobox with a specified list of items.</summary>
    ///<param name="control" type="Object">Telerik RadCombobox control.</param>
    ///<param name="items" type="Object">Collection or array of items to add to the list.  Individual items must have an ID or Key property to define the item's id and a Name or Value property to define the item's text.</param>
    ///<param name="defaultItem" type="Object">A single item to add as the first entry in the list if the "items" collection does not already contain a default.  Item must have a value and text property.</param>
    ///<param name="defaultItem">Default selected value.  If not specified the first item is selected.</param>

    if (list && (items || defaultItem))
    {
        if (list.control) list = list.control;

        var item;
        var li;
        var col = list.get_items();
        col.clear();

        if (defaultItem)
        {
            li = new Telerik.Web.UI.RadComboBoxItem();
            li.set_value(defaultItem.value);
            li.set_text(defaultItem.text);
            col.add(li);
        }

        var id;
        var text;

        for (var i in items)
        {
            item = items[i];

            //If ID available use ID else Key - if neither of those are present use i.
            id = (item.ID ? item.ID : item.Key ? item.Key : item[varID] ? item[varID] : i);
            //If Name available use Name else Value - if neither of those are present use item.
            text = (item.Name ? item.Name : item.Value ? item.Value : item[varName] ? item[varName] : item);

            if (id != null)
            {
                li = new Telerik.Web.UI.RadComboBoxItem();
                li.set_value(id);
                li.set_text(text);
                col.add(li);
            }
        }

        //select first item by default or selectedValue (param)
        if (col.get_count() > 0)
        {
            if (selectedValue == null)
                col.getItem(0).select();
            else
            {
                li = list.findItemByValue(selectedValue);
                if (li) li.select();
                else col.getItem(0).select();
            }
        }
    }
};

$getWindow = Pathfinder.UI.ClientManager.getWindow = function()
{
    ///<summary>Returns a Telerik RadWindow control for the current window or frame if available.</summary>
    ///<returns type="Telerik.Web.UI.RadWindow" />

    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement && window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
    return oWindow;
};

$closeWindow = Pathfinder.UI.ClientManager.closeWindow = function()
{
    ///<summary>Closes the current Telerik RadWindow control.  If the current window or frame is not a RadWindow no action is taken.</summary>
    ///<returns type="void"></returns>

    var w = $getWindow();
    if (w)
        w.close();
    else //not rad window so check if custom window created by DIV and IFRAME
    {
        if (window.frameElement.tagName == "IFRAME")
            $(window.frameElement.parentNode).hide();
    }
};

$openWindow = Pathfinder.UI.ClientManager.openWindow = function(url, x, y, width, height, windowName, returnWindow)
{
    ///<summary>Opens a Telerik RadWindow using the specified url and optional location and size.</summary>
    ///<param name="x" type="Numeric">Left position of the window.  If x and y are not specified then the window will be centered.  If y is specified then x defaults to 0.</param>
    ///<param name="y" type="Numeric">Top position of the window.  If x and y are not specified then the window will be centered.  If x is specified then y defaults to 0.</param>
    ///<param name="width" type="Numeric">Width of the window.  If not specified the default value will be 95% of the browser window's width.</param>
    ///<param name="height" type="Numeric">Height of the window.  If not specified the default value will be 85% of the browser window's height.</param>
    ///<param name="windowName" type="String">Optional name to assign to the window.  The name is used to construct the wrapper element's id.  If no value is specified the name "modal" will be assigned.</param>
    ///<param name="returnWindow" type="Boolean">Optional parameter to indicate if the newly created window object should be returned to the caller.  Default is False.</param>
    ///<returns type="Telerik.Web.UI.RadWindow" />

    var windowHeight = $(window);

    if (width == null)
        width = windowHeight.width() / 1.05;
    if (height == null)
        height = windowHeight.height() / 1.15;

    if (windowName == null || windowName == "")
        windowName = "modal";

    var win = radopen(url, windowName);

    win.setSize(width, height);

    if (x != null || y != null)
    {
        if (x == null) x = 0;
        if (y == null) y = 0;
        win.moveTo(x, y);
    }
    else
        win.Center();

    if (returnWindow)
        return win;
};

$setGridPage = Pathfinder.UI.ClientManager.setGridPage = function(gridOrMasterTableID, page, customPaging)
{
    ///<summary>Sets the current page of the grid based on the specified element ID.</summary>
    ///<param name="gridOrMasterTableID">Element ID of either a Telerik RadGrid or a MasterTableView of a Telerik RadGrid</summary>
    ///<param name="page" type="Numeric">New page index (zero based).</param>
    ///<returns type="void"></returns>

    var g = $get(gridOrMasterTableID);
    if (g && g.control)
    {
        g = g.control;

        if (g.get_masterTableView)
            g = g.get_masterTableView();

        if (!customPaging)
            g.set_currentPageIndex(page);
        else
        {
            $setGridFilter(g, "Page_Index", page, "EqualTo", "System.Int32");
            g.rebind();
        }

        //update pager text with new page before data is even returned for better user feel - if counts actually change the pager will be updated again once data is loaded
        var w = g.get_parent().get_element();
        w = w.control.GridWrapper;
        if (w)
        {
            $(w.get_pagerSelector()).html($constructCustomPager(g, g.get_virtualItemCount(), false, w.get_clientManager().get_BasePath(), customPaging));
            if (w.get_showLoading())
                $(w.get_pagerSelector()).find(" .pagerText").html("<span class='loading'>" + w.get_loadingText() + "</span>");
        }
    }
};

$constructCustomPager = Pathfinder.UI.ClientManager.constructCustomPager = function(gridOrMasterTableView, count, forceToTopWindow, basePath, customPaging)
{
    ///<summary>Creates HTML markup for a custom pager for a specified grid.</summary>
    ///<param name="gridOrMasterTableView">Grid or MasterTableView control that the pager is being created for.</param>
    ///<param name="count" type="Numeric">Total potential number of items that are displayed in the grid.  This is the Telerik Rad Grid's Virtual Count.</param>
    ///<param name="forceToTopWindow" type="Boolean">Optional parameter for indicating whether or not the paging buttons execute in the top window.  The default is False and is only True in special cases such as with the Plan Info grid in a modal window that mirrors the dashboard's grid.  In that case the grid in the modal window actually calls the paging function on the dashboard's grid and not the grid in the modal window.</param>
    ///<returns type="String"></returns>

    function getPageIndexFromFilter(mt)
    {
        var f = mt.get_filterExpressions().find("Page_Index");
        if (f)
        {
            return f.get_fieldValue();
        }
        return 0;
    }

    var mt = gridOrMasterTableView;
    if (mt.get_masterTableView) mt = mt.get_masterTableView();
    var gridID = mt.get_id();
    var pageHTML = "";
    var size = mt.get_pageSize();
    var page = (!customPaging ? mt.get_currentPageIndex() : getPageIndexFromFilter(mt));
    var buttonCount = 5;
    var pageCount = Math.ceil(count / size);
    var first = (page * size) + 1;
    var last = (first - 1 + size);

    if (basePath == null) basePath = "";

    var top = (forceToTopWindow === true ? "window.top." : "");

    if (last > count) last = count;

    //Pager Text
    var pagerText = (mt.get_owner().get_element().control.GridWrapper && mt.get_owner().get_element().control.GridWrapper.get_showNumberOfRecords() ? "<span class='pagerText'>" + String.format("Records {0}-{1} of {2}", first, last, count) + "</span>" : "");

    //Pager Page Buttons
    var pagerButtons = "";
    var buttonStart = parseInt(page / buttonCount) * buttonCount;
    var buttonEnd = buttonStart + buttonCount;
    if (buttonEnd > pageCount) buttonEnd = pageCount;

    for (var i = buttonStart; i < buttonEnd; i++)
    {
        if (page != i)
            pagerButtons += "<a class='pg" + i + "' href='javascript:void(0)' onclick='" + top + "$setGridPage(\"" + gridID + "\"," + i + "," + customPaging + ")'>" + (i + 1) + "</a>";
        else
            pagerButtons += "<span>" + (i + 1) + "</span>";
    }

    //Pager Next/Previous Buttons
    var pagerPrevious;
    if (buttonStart > 0)
        pagerPrevious = "<img class='pagerPrev' src='" + basePath + "/content/images/spacer.gif' onclick='" + top + "$setGridPage(\"" + gridID + "\"," + (buttonStart - 1) + "," + customPaging + ")' />";
    else
        pagerPrevious = "<img class='pagerPrev grey' src='" + basePath + "/content/images/spacer.gif' />";

    var pagerNext;
    if (buttonEnd < pageCount)
        pagerNext = "<img class='pagerNext' src='" + basePath + "/content/images/spacer.gif' onclick='" + top + "$setGridPage(\"" + gridID + "\"," + (buttonStart + buttonCount) + "," + customPaging + ")' />";
    else
        pagerNext = "<img class='pagerNext grey' src='" + basePath + "/content/images/spacer.gif' />";

    //Output pager HTML
    pagerHTML = "<div>" + pagerPrevious + "<div class='pagerButtons' style='display:inline'>" + pagerButtons + "</div>" + pagerNext + pagerText + "</div>";

    return pagerHTML;
};


$getDataForPostback = Pathfinder.UI.ClientManager.getDataForPostback = function(data, isForm, newData)
{
    ///<summary>Returns a string of key value pairs that can be used for GET or POST web requests based on the information in the data param.</summary>
    ///<param name="data" type="Object">Collection of data properties that are used to create the query string.</param>

    var q = "";

    if (data != null)
    {
        var val;
        for (var s in data)
        {
            val = data[s];
            if (val != null)
            {
                //check for nested value ...
                if (val.value == undefined)
                {
                    if (s == "__options")
                    {
                        var opts = {};

                        for (var x in data[s])
                            opts[x] = data[s][x].value;

                        val = Sys.Serialization.JavaScriptSerializer.serialize(opts);
                        if (val == "{}") continue; //empty object so continue
                    }

                    if (typeof val != "object") //if not flattened at this point exclude
                    {
                        if (newData)
                            newData[s] = val;

                        q += ((q != "" ? "&" : "") + s + "=" + val);
                    }
                    if (typeof val == "object")
                    {
                        if ($.isArray(val))
                            val = val.join(",");
                        q += ((q != "" ? "&" : "") + s + "=" + val);

                    }
                }
                else //nested value such as --> {value:xxxx,_filterType:Contains,_dataType:System.String}
                {
                    val = (val.filterType != "Contains" || isForm ? val.value : "%" + val.value + "%");
                    if (newData)
                        newData[s] = val;

                    q += ((q != "" ? "&" : "") + s + "=" + encodeURIComponent(val));
                }
            }
        }
    }

    return q;
};

$getFilterTextForServiceRequest = Pathfinder.UI.ClientManager.getFilterTextForServiceRequest = function(gridOrMasterTableView, excludePageIndex)
{
    ///<summary>Returns an Entity Framework Where clause based on the specified grid's filter collection that can be applied to a custom method of an ADO.Net Data Service.  Typically this is for the "Record Count" methods for an entity set.</summary>
    ///<param name="gridOrMasterTableView">Grid or MasterTableView control used for constructing the filter text.</param>
    ///<returns type="String"></returns>

    var mt = gridOrMasterTableView;
    if (mt.get_masterTableView) mt = mt.get_masterTableView();

    var filters = mt.get_filterExpressions();
    var filter;
    var count = filters.get_count();
    var filterText = "";
    var text;

    function getCustomFilterValueText(filter)
    {
        if (filter.get_dataTypeName() == "System.String")
            return "\"" + filter.get_fieldValue().split(",").join("\",\"") + "\"";
        else
            return filter.get_fieldValue();
    }

    for (var i = 0; i < count; i++)
    {
        filter = filters.getItem(i);
        if (!excludePageIndex || filter.get_columnUniqueName() != "Page_Index")
        {
            if (filter.get_filterFunction() == "Custom")
            {
                text = "it." + filter.get_fieldName() + " in {" + getCustomFilterValueText(filter) + "}";
            }
            else if (filter.get_dataTypeName() == "System.DateTime" && filter.get_filterFunction() == "Between")
            {
                text = filter.get_fieldValue();
                text = text.split(" ");
                text = "it." + filter.get_columnUniqueName() + " >= DateTime%27" + new Date(text[0]).format("yyyy-MM-dd 00:00") + "%27 AND it." + filter.get_columnUniqueName() + "< DateTime%27" + new Date(new Date(text[1]).setDate(new Date(text[1]).getDate() + 1)).format("yyyy-MM-dd 00:00") + "%27";

            }
            else
            {
                text = "it." + filter.toOql();
                //fix telerik putting it. in wrong place
                text = text.replace(/it\.\(/g, "(it.").replace(/\s\(/g, " (it.");

                text = text.replace(/\*/g, "%").replace(/\"/ig, '""').replace(new RegExp("'", "ig"), "\"").replace(/%27/g, "''");
            }

            filterText += (i > 0 ? " AND " : "") + text;
        }
    }

    return encodeURIComponent(filterText);
};

$unMergeGridCells = Pathfinder.UI.ClientManager.unMergeGridCells = function(gridOrMasterTableView)
{
    ///<summary>Undoes any merging performed by mergeGridCells function.</summary>
    ///<param name="gridOrMasterTableView">Grid or MasterTableView control that will be modified.</param>

    var mt = gridOrMasterTableView;
    if (mt.get_masterTableView) mt = mt.get_masterTableView();

    //reset grid cells so row spans are gone and hidden cells are visible
    $("#" + mt.get_id() + " td[rowSpan!=1]").attr("rowSpan", "1");
    $("#" + mt.get_id() + " td[_xcol=true]").css("display", "").attr("_xcol", false);
};

$mergeGridCells = Pathfinder.UI.ClientManager.mergeGridCells = function(gridOrMasterTableView)
{
    ///<summary>Merges cells with identical values on all sorted columns.  If no columns are sorted this function will simply undo any previous merging.</summary>
    ///<param name="gridOrMasterTableView">Grid or MasterTableView control that will be modified.</param>
    var mt = gridOrMasterTableView;
    if (mt.get_masterTableView) mt = mt.get_masterTableView();

    var rows = mt.get_dataItems();
    var count = rows.length;
    var row;
    var rowElement;
    var text;
    var c; //cell index retreived from sorted column
    var firstRow = {};
    var prev = {};
    var prevNew = {};
    var prevsecondcolnew = {};
    var span = {};
    var spanNew = {};
    var spansecondcolNew = {};
    var jCell;
    var cNextCol;
    var cSecondNextCol;
    var jCellNextCol;
    var jCellSecondNextCol;
    var textNextCol;
    var textSecondNextCol;

    var sortCount = mt.get_sortExpressions().get_count();

    //IE can't handle merging efficiently in versions earlier than 8 so limit
    if ($.browser.msie && $.browser.version < 8 && sortCount > 2)
        sortCount = 2;
    //

    //reset grid cells so row spans are gone and hidden cells are visible
    $unMergeGridCells(mt);

    if (sortCount > 0)
    {
        //loop 1 more than the number of physical rows so the last row is completed (meaning text var will not equal prev so code will trigger final rowspan to be set)
        for (var i = 0; i < count + 1; i++)
        {
            row = rows[i];
            if (row)
            {
                rowElement = row.get_element();
                if (rowElement.style.display == "none") rowElement = null; //if RadGrid is hiding row then ignore it as well
            }
            else  //Typically rowElement is set to null on the final row which actually doesn't exist and is the extra iteration through loop to close things out (determined by count + 1 in for loop above)
                rowElement = null;

            //For each sorted column we will merge cells
            for (var x = 0; x < sortCount; x++)
            {
                //get the cell index of the sorted column
                c = mt.getColumnByUniqueName(mt.get_sortExpressions().getItem(x).get_fieldName());
                if (c)
                    c = c.get_element().cellIndex;
                else
                    return;

                jCell = rowElement ? $(rowElement.cells[c]) : null;

                if (x + 1 < sortCount)
                {
                    cNextCol = mt.getColumnByUniqueName(mt.get_sortExpressions().getItem(x + 1).get_fieldName()).get_element().cellIndex;
                    jCellNextCol = rowElement ? $(rowElement.cells[cNextCol]) : null;
                }
                if (x + 2 < sortCount)
                {
                    cSecondNextCol = mt.getColumnByUniqueName(mt.get_sortExpressions().getItem(x + 2).get_fieldName()).get_element().cellIndex;
                    jCellSecondNextCol = rowElement ? $(rowElement.cells[cSecondNextCol]) : null;
                }
                //jCell will be null if rowElement is also null - this is ok since it means we need to wrap things up on final row.  also we are filtering out columns which have merging disabled.
                if (!jCell || (!jCell.hasClass("notmerged") && !jCell.hasClass("mergewithnextCol") && !jCell.hasClass("mergewithsecondnextCol")))
                {
                    //If row is valid get its displayed value
                    if (rowElement)
                        text = jCell.text();
                    else
                        text = null;

                    if (text != prev[c])
                    {
                        if (firstRow[c] != null && span[c] > 1)
                            $(firstRow[c].cells[c]).attr("rowSpan", span[c]);

                        if (rowElement)
                        {
                            jCell.css("display", "").attr("_xcol", false);

                            firstRow[c] = rowElement;
                            span[c] = 1;
                        }
                    }
                    else
                    {
                        //rowElement.deleteCell(1);
                        if (rowElement)
                        {
                            jCell.css("display", "none").attr("_xcol", true);
                        }
                        span[c]++;
                    }
                    prev[c] = text;
                }
                if (!jCell || (!jCell.hasClass("notmerged") && jCell.hasClass("mergewithnextCol")))
                {
                    //If row is valid get its displayed value
                    if (rowElement)
                    {
                        text = jCell.text();
                        textNextCol = jCellNextCol.text();
                    }
                    else
                        text = null;

                    if (text != prevNew[c] || (textNextCol != prevNew[cNextCol]))
                    {
                        if (firstRow[c] != null && spanNew[c] > 1)
                            $(firstRow[c].cells[c]).attr("rowSpan", spanNew[c]);

                        if (rowElement)
                        {
                            jCell.css("display", "").attr("_xcol", false);

                            firstRow[c] = rowElement;
                            spanNew[c] = 1;
                        }
                    }
                    else if (text == prevNew[c] && (textNextCol == prevNew[cNextCol]))
                    {
                        //rowElement.deleteCell(1);
                        if (rowElement)
                        {
                            jCell.css("display", "none").attr("_xcol", true);
                        }
                        spanNew[c]++;
                    }
                    prevNew[c] = text;
                    prevNew[cNextCol] = textNextCol;
                }
                //for second next col
                if (!jCell || (!jCell.hasClass("notmerged") && jCell.hasClass("mergewithsecondnextCol")))
                {
                    //If row is valid get its displayed value
                    if (rowElement)
                    {
                        text = jCell.text();
                        textNextCol = jCellNextCol.text();
                        textSecondNextCol = jCellSecondNextCol.text();
                    }
                    else
                        text = null;

                    if (text != prevsecondcolnew[c] || (textSecondNextCol != prevsecondcolnew[cSecondNextCol]))
                    {
                        if (firstRow[c] != null && spansecondcolNew[c] > 1)
                            $(firstRow[c].cells[c]).attr("rowSpan", spansecondcolNew[c]);

                        if (rowElement)
                        {
                            jCell.css("display", "").attr("_xcol", false);

                            firstRow[c] = rowElement;
                            spansecondcolNew[c] = 1;
                        }
                    }
                    else if (text == prevsecondcolnew[c] && (textSecondNextCol == prevsecondcolnew[cSecondNextCol]))
                    {
                        //rowElement.deleteCell(1);
                        if (rowElement)
                        {
                            jCell.css("display", "none").attr("_xcol", true);
                        }
                        spansecondcolNew[c]++;
                    }
                    //prevNew[c] = text;
                    //prevNew[cNextCol] = textNextCol;
                    prevsecondcolnew[c] = text;
                    prevsecondcolnew[cSecondNextCol] = textSecondNextCol;
                }
            }

            if (rowElement == null)
            {
                break; //no need to go further - no more rows left or displayed
            }
        }

    }

    //hack to fix distorted grid - after merging is complete row data is sometimes outside boundries of cells and looks really ugly
    new cmd(null, function() { if (mt.get_parent()) $(mt.get_parent().get_element()).css("visibility", "hidden").css("visibility", "visible"); }, mt, 1);
};

$refreshGridData = Pathfinder.UI.ClientManager.refreshGridData = function(gridOrMasterTableView, serviceUrl, data, preProcess)
{
    ///<summary>Manually loads a datagrid with the results of a web service request.</summary>
    ///<param name="gridOrMasterTableView" type="Object">Telerik RadGrid or MasterTableView control that is to be refreshed.</param>
    ///<param name="serviceUrl" type="String">Web address of the service</param>
    ///<param name="data" type="Map">Key/Value pairs of data to send with request.</param>

    var grid = gridOrMasterTableView;
    if (grid.get_masterTableView) grid = grid.get_masterTableView();

    $.getJSON(serviceUrl, data, function(result, status)
    {
        if (status == "success")
        {
            try
            {
                var data = result.d;
                if (data.results)
                {
                    data = data.results;
                    grid.get_owner().get_element().control.GridWrapper.updateRecordCount(result.d.__count);
                }
                if (preProcess) preProcess(data, grid);

                grid.set_dataSource(data);
                grid.dataBind();
            }
            catch (ex) { }
        }
    }
    );
};

$getGridSortString = Pathfinder.UI.ClientManager.getGridSortString = function(gridOrMasterTableView)
{
    var grid = gridOrMasterTableView;
    if (grid.get_masterTableView)
        grid = grid.get_masterTableView();

    return (grid.get_owner().get_element().control.GridWrapper ? grid.get_owner().get_element().control.GridWrapper.get_SortString() : grid.get_sortExpressions().toString()).replace(/ ASC/g, " asc").replace(/ DESC/g, " desc");
}

$getWebServiceQuery = Pathfinder.UI.ClientManager.getWebServiceQuery = function(gridOrMasterTableView)
{
    ///<summary>Returns a query string based on the specified grid's filter collection that can be applied to a web service method.</summary>
    ///<param name="gridOrMasterTableView">Grid or MasterTableView control used for constructing the filter text.</param>
    ///<returns type="String"></returns> 

    var grid = gridOrMasterTableView;
    if (grid.get_masterTableView)
        grid = grid.get_masterTableView();

    var filters = grid.get_filterExpressions();
    var filter;
    var filterText = "";
    var subFilter;
    var a;
    var val;
    for (var i = 0; i < filters.get_count(); i++)
    {
        filter = filters.getItem(i);
        if (filter.get_filterFunction() == "Custom")
        {
            subFilter = "";
            a = filter.get_fieldValue().split(",")
            for (var v in a)
            {
                val = $.trim(a[v]);
                subFilter += (subFilter != "" ? "," : "") + val;
            }
        }
        else
        {
            subFilter = filter.get_fieldValue();
            if (typeof (subFilter) == "string") subFilter = subFilter.replace(/%27/g, "'");
        }
        filterText += (filterText != "" ? "&" : "") + filter.get_fieldName() + "=" + subFilter;
    }

    return encodeURI(filterText + "&__sort=" + $getGridSortString(grid) + "&__pageindex=" + grid.get_currentPageIndex() + "&__pagesize=" + grid.get_pageSize());
}

$getJSON = function(url, data, callback)
{
    ///<summary>Alternative to calling $.getJSON which caches result based on url and query string.  Use this for items such as dependent dropdown lists.  DO NOT use for lookups where the query string changes often such as Plan List in TA.
    ///<param name="url">Url of the web service that returns JSON formatted data.</param>
    ///<param name="data">Query string to append to url (if url does not already contain a query string).  Unlike $.getJSON this version does not work with a collection.  Only string data is allowed.</param>
    ///<param name="callback">Function to call when web service returns a result.</param>

    if (data && typeof data != "string")
        throw new Error("data parameter can only be of type string.");

    var key = url + (data ? data : "");
    var result = clientManager.getContextValue(key);

    if (!result)
    {
        $.getJSON(url, data, function(r, s)
        {
            if (r && s == "success")
                clientManager.setContextValue(key, r);

            callback(r, s);
        });
    }
    else
        new cmd(null, callback, [result, "success"], 350);
}

$getDataServiceQuery = Pathfinder.UI.ClientManager.getDataServiceQuery = function(gridOrMasterTableView)
{
    ///<summary>Returns a query string based on the specified grid's filter collection that can be used for an ADO.Net Data Service request.  Unlike the default Telerik RadGrid implementation this will treat "Custom" as an OR'd list of values.</summary>
    ///<param name="gridOrMasterTableView">Grid or MasterTableView control used for constructing the filter text.</param>
    ///<returns type="String"></returns>

    var grid = gridOrMasterTableView;
    if (grid.get_masterTableView)
        grid = grid.get_masterTableView();

    var filters = grid.get_filterExpressions();
    var filter;
    var filterText = "";
    var queryExtText = "";
    var subFilter;
    var a;
    var val;
    var gw = grid.get_parent().get_element().control.GridWrapper;

    for (var i = 0; i < filters.get_count(); i++)
    {
        filter = filters.getItem(i);
        if (filter.get_fieldName() != "__options")
        {
            if (!filter.isExtension)
            {
                if (filter.get_filterFunction() == "Custom")
                {
                    subFilter = "";
                    a = filter.get_fieldValue().split(",")
                    for (var v in a)
                    {
                        val = $.trim(a[v]);
                        if (isNaN(val) || filter.get_dataTypeName() == "System.String") val = "'" + val + "'";
                        subFilter += (subFilter != "" ? " or " : "") + filter.get_fieldName() + " eq " + val;
                    }
                    filterText += (filterText != "" ? " and " : "") + "(" + subFilter + ")";
                }
                else
                {
                    //changed by Aditi to replace <= time as 23:59:59 to fix the filter condition
                    if (filter.get_filterFunction() == "Between")
                        filterText += (filterText != "" ? " and " : "") + filter.toDataService().replace(/%27/g, "''").replace("T12:00:00", "T00:00:00").replace("T12:00:00", "T23:59:59");
                    else
                        filterText += (filterText != "" ? " and " : "") + filter.toDataService().replace(/%27/g, "''");
                }
            }
            else
            {
                queryExtText += filter.get_extensionFilterText();
            }
        }
    }

    return (gw && gw.get_inlineCountEnabled() ? "$inlinecount=allpages&" : "") + (gw && gw.get_expand() ? "$expand=" + gw.get_expand() + "&" : "") + "$filter=" + encodeURIComponent(filterText) + "&$orderby=" + $getGridSortString(grid) + "&$skip=" + (grid.get_pageSize() * grid.get_currentPageIndex()) + "&$top=" + grid.get_pageSize() + queryExtText;
};

$disposeControl = Pathfinder.UI.ClientManager.disposeControl = function(c)
{
    if (c)
    {
        //        if (c.get_element().checkboxList)
        //            delete (c.get_element().checkboxList);

        c.dispose();
    }
};

$disposeTelerikGrid = Pathfinder.UI.ClientManager.disposeTelerikGrid = function(grid)
{
    try
    {
        var cols = grid.get_masterTableView() ? grid.get_masterTableView().get_columns() : null;
        if (cols)
        {
            for (var i = cols.length - 1; i >= 0; i--)
            {
                $disposeControl(cols[i]);
                delete (cols[i]);
            }
        }

        $disposeControl(Sys.Application.findComponent(grid.get_id() + "_rfltMenu"));

        $disposeControl(grid.get_masterTableViewHeader());
        $disposeControl(grid.get_masterTableViewFooter());

        $disposeControl(grid.get_masterTableView());

        $disposeControl(grid);
    }
    catch (ex)
    {
        $disposeControl(grid);
    }

};


$simplifyName = Pathfinder.UI.ClientManager.simplifyID = function(aspName, splitChar, fullName)
{
    ///<summary>Returns the tail end of an element's name so the portion of the name added for each naming container by ASP.Net is removed.  This function is used primarily when gathering values from input controls to assign a property name to the data value read from the page (see $getContainerData).<summary>
    ///<param name="aspID" type="String">Name assigned to the control by ASP.Net framework.  The Name is expected and not the ID because splitting on underscores is problematic if the base ID assigned to the control had an underscore.  Since .Net uses $ to separate segments of the Name property this is much easier to split on.</param>
    if (fullName) return aspName;

    if (!splitChar) splitChar = "$";

    var name = aspName.split(splitChar);

    return name[name.length - 1];
};

$getContainerData = Pathfinder.UI.ClientManager.getContainerData = function(containerID, includeEmptyValues, fullName, includeHiddenFields, includeNonFilters)
{
    ///<summary>Returns a collection of data values for all controls within a specified container.  The simplified id (ASP.Net name mangling removed) of the element will be used as the key for ComboBoxes, TextBoxes and TextAreas.  Checkboxes will first use the name if available otherwise the simplified id.  Additionally checkboxes with the same key will create a comma separated list of values while other controls simply keep the last value saved.</summary>
    ///<param name="containerID" type="String">ID of the element that contains the controls that data is retreived from.</param>
    ///<param name="includeEmptyValues" type="Boolean" optional="true">Determines if empty values should be included in the result set.  By default empty values are excluded.</param>

    var data = {};

    includeEmptyValues = includeEmptyValues == true;

    function getDataType(element)
    {
        var dataType = null;
        var j = $(element);

        if (j.hasClass("string"))
            return "System.String";
        else if (j.hasClass("int"))
            return "System.Int32";
        else if (j.hasClass("datePicker"))
            return "System.DateTime";
        else if (j.hasClass("double"))
            return "System.Double";

        return null;
    }

    var notFilter = includeNonFilters ? ".XXXXXXX" : ".notfilter";

    var hiddenFieldSelector = (includeHiddenFields ? ", #" + containerID + " input[type = hidden]" : ", #" + containerID + " input.staticFilter");
    //INPUT - Text - standard text input controls excluding RadComboBox input
    $("#" + containerID + " input[type = text]:not(.rcbInput):not(" + notFilter + ")" + hiddenFieldSelector).each(
                function()
                {
                    if (!this.control)
                    {
                        var value = this.value;
                        if ((value != null && value != "") || includeEmptyValues)
                        {
                            var id = this.name;
                            if (id == null || id == "")
                                id = this.id;
                            else
                                id = id = $simplifyName(this.name, fullName);

                            var type = getDataType(this);
                            if (!type) type = "System.String";
                            var filterType = (type == "System.String" ? "Contains" : "EqualTo");

                            if (data[id] != null)
                                data[id].appendValue(this.value, this.id);
                            else
                                data[id] = new Pathfinder.UI.dataParam(id, value, type, filterType, this.id, $(this).hasClass("queryExt")); //{ "value": value, "_filterType": "Contains", "_dataType": "System.String" };
                        }
                    }
                }
    );

    //INPUT - Checkbox - standard checkbox input controls
    $("#" + containerID + " input[type = checkbox]:not(" + notFilter + "), .checkboxDropDown input[type = checkbox]:not(" + notFilter + "), input[type = radio]:not(" + notFilter + ")").each(
            function()
            {
                if (!this.control && this.checked)
                {
                    //try to use name otherwise use id - using id may cause issues with checkbox lists since values will not be appended to same key                    
                    var id = this.name;
                    if (id == null || id == "")
                        id = this.id;
                    else
                        id = $simplifyName(id, fullName);

                    var ctrlId = this.id;
                    if ($(this).hasClass("chkItem"))
                    {
                        var p = this.parentElement;
                        while (p && !p.control)
                            p = p.parentElement;

                        if (p) ctrlId = p.id;
                    }

                    //add comma to create list if data is not null (meaning a value was already set by another checkbox)
                    if (data[id] != null)
                    {
                        data[id].appendValue(this.value);
                    }
                    else
                        data[id] = new Pathfinder.UI.dataParam(id, this.value, getDataType(this), null, ctrlId, $(this).hasClass("queryExt"));
                }
            }
    );

    //TEXTAREA
    $("#" + containerID + " textarea").each(
        function()
        {
            var id = $simplifyName(this.name, fullName);
            var value = this.value;
            if ((value != null && value != "") || includeEmptyValues)
                data[id] = new Pathfinder.UI.dataParam(id, value, "System.String", null, this.id, $(this).hasClass("queryExt"));
        }
    );

    //RadComboBox values - exclude items with CssClass property set to "notfilter"
    $("#" + containerID + " div:not(" + notFilter + ")" + ($.browser.msie ? "[control]" : "")).each(
                function()
                {
                    if (this.control && Telerik.Web.UI.RadComboBox.isInstanceOfType(this.control))
                    {
                        //keep only tail end of id to simplify - this means however that all criteria must have unique control ids
                        var id = $simplifyName(this.control._uniqueId, fullName);

                        var value = this.checkboxList ? this.checkboxList.get_values() : this.control.get_value();

                        if ((value != null && value != "") || includeEmptyValues)
                        {
                            //regular radcombobox or checkbox list that sets all values to one field
                            if (!this.checkboxList || !this.checkboxList.get_booleanOptions())
                                data[id] = new Pathfinder.UI.dataParam(id, value, getDataType(this), null, this.id, $(this).hasClass("queryExt"));
                            else //radcombobox hosts checkbox list that is used for true/false (on/off) values which are not accumulated into one datafield but rather separated by name.
                            {
                                var items = this.checkboxList.get_selectedItems();
                                if ($.isArray(value))
                                {
                                    for (var i = 0; i < value.length; i++)
                                    {
                                        data[value[i]] = new Pathfinder.UI.dataParam(value[i], this.checkboxList.get_trueValue(), null, null, items[i].fullID, $(this).hasClass("queryExt")); //get value that checkbox contains when true
                                    }
                                }
                                else
                                    data[value] = new Pathfinder.UI.dataParam(value, this.checkboxList.get_trueValue(), null, null, items.fullID, $(this).hasClass("queryExt")); //get value that checkbox contains when true
                            }
                        }
                        else
                            delete (data[id]);
                    }
                }
    );


    return data;
};

$validateFormData = Pathfinder.UI.ClientManager.validateFormData = function(containerID, data, title, silent)
{
    return $validateContainerData(containerID, data, title, true, silent);
};

$validateContainerData = Pathfinder.UI.ClientManager.validateContainerData = function(containerID, data, title, isForm, silent)
{
    function parseVal(val, type)
    {
        switch (type)
        {
            case "Integer":
            case "Double":
                return Number.parseLocale(val);
            case "Date":
                return Date.parseLocale(val);
        }

        return val;
    }

    var valid = true;
    var msg = "";

    $resetErrors(containerID);

    $("#" + containerID + " .validator").each(
    function()
    {
        var validator = $(this);

        var dataField1, dataField2;
        dataField1 = validator.attr("dataField");
        if (isForm)
            dataField2 = validator.attr("formField");
        var value;

        if (dataField2)
            value = data[dataField2];
        if (!value && dataField1)
            value = data[dataField1];
        else if (!dataField2 && !dataField1)
            value = $("#" + validator.attr("target")).val();

        //var value = (dataField ? data[dataField] : $("#" + validator.attr("target")).val()); //get datafield value otherwise direct from target ctrl
        if (value && value.get_value) value = value.get_value();

        var itemIsValid = true;

        //Required?
        if (validator.attr("_required") == "true")
        {
            var v = value;
            if (v && !$.isArray(v)) v = $.trim(v.toString());

            itemIsValid = (v != null && v != "");
        }

        if (value && value != "")
        {
            var dataType = validator.attr("_dataType");
            //Check Data Type
            if (dataType)
            {
                var testValue = parseVal(value, dataType);
                itemIsValid = testValue != null && !isNaN(testValue);
                if (itemIsValid && dataType == "Integer")
                    itemIsValid = value.indexOf(".") == -1;
            }

            //Check Compare To
            if (itemIsValid && (validator.attr("_compareTo") || validator.attr("_compareToValue")))
            {
                var cmpVal = validator.attr("_compareTo") ? parseVal($("#" + validator.attr("_compareTo")).val(), dataType) : parseVal(validator.attr("_compareToValue"), dataType);
                var op = validator.attr("_compareOp");
                switch (op)
                {
                    case "Equal":
                        itemIsValid = (value == cmpVal);
                        break;
                    case "NotEqual":
                        itemIsValid = (value != cmpVal);
                        break;
                    case "LessThan":
                        itemIsValid = (value < cmpVal);
                        break;
                    case "LessThanEqual":
                        itemIsValid = (value <= cmpVal);
                        break;
                    case "GreaterThan":
                        itemIsValid = (value > cmpVal);
                        break;
                    case "GreaterThanEqual":
                        itemIsValid = (value >= cmpVal);
                        break;
                }
            }

            //Check RegExp
            if (itemIsValid && validator.attr("_regExp"))
            {
                var testval = value.toString();
                var m = testval.match(new RegExp(validator.attr("_regExp")));
                if (m != null)
                    itemIsValid = (m.length > 0 && m[0] == testval);
                else
                    itemIsValid = false;
            }

            //Check MaxLength
            if (itemIsValid && validator.attr("_maxLength"))
            {
                var length = parseInt(validator.attr("_maxLength"));
                itemIsValid = value.toString().length <= length;
            }

        }

        if (!itemIsValid)
        {
            msg += $(this).text() + "<br />";

            var j = $("#" + validator.attr("target")).addClass("invalid");
            if (j.length > 0 && j[0].control)
                j = j.find("input");


            j.blur(function() { $validateContainerData(containerID, $getContainerData(containerID, null, isForm), title, isForm, true); });
        }

        valid = valid && itemIsValid;
    }
    );

    if (!valid)
    {
        if (!silent)
            $alert(msg, title);
    }
    else
        $clearAlert();

    return valid;
};

$reloadContainer = Pathfinder.UI.ClientManager.reloadControls = function(containerID, data)
{
    $("#" + containerID + " #filterControls").scrollTop(0);

    function updateCtrls(o)
    {
        if (o && o != "{}")
        {
            var item;
            for (var s in o)
            {
                item = o[s];
                if (item)
                {
                    // sl 6/21/2012 avoid error: SR filter "Section" - 'PBM' is not displayed in filter 'Section' dropdown when you refresh the page after 'PBM' is only selected 
                    if (item.name == "onlyPBM" && item.value == true)
                        item = new Pathfinder.UI.dataParam("Section_ID", 4, "System.Int32", "EqualTo", "ctl00_partialPage_filtersContainer_Section_ID_Section_ID");
                    else
                    {
                        if (item.value && !item.isExtension) item = new Pathfinder.UI.dataParam(s, item.value, item.dataType, item.filterType, item.src);
                    }
                    if (item.resetSrc)
                        item.resetSrc();
                }
            }
        }
    }

    if (!$.isArray(data))
        data = [data];

    for (var i = 0; i < data.length; i++)
    {
        if (data[i])
        {
            updateCtrls(data[i]);
            updateCtrls(data[i]["__options"]);
        }
    }


    $("#" + containerID + " div:not(.notfilter).RadComboBox").each(function() { if (this.checkboxList) $updateCheckboxDropdownText(this.control); });

    //patch checkbox list generated from server to match default value if nothing selected
    //CheckboxList (server generated) - do after regular checkboxes
    $("#" + containerID + " .chkBoxDiv").each(function()
    {
        if (this.control && this.control.reset && !this.control.get_values()) //reset if nothing selected so default value will be forced
            this.control.reset();
    });
};

$resetErrors = function(containerID)
{
    if (containerID) containerID = "#" + containerID + " ";
    $(containerID + ".invalid").removeClass("invalid").unbind("blur").find("input").unbind("blur");
};

$resetContainer = Pathfinder.UI.ClientManager.resetContainer = function(containerID)
{
    ///<summary>Clears all controls in the specified container.  *RadComboBox controls are reset by setting the selected item to the first entry in the list.  This may cause event handlers to fire which could have unintended consiquences.  Prior to calling this function a flag should be set that the event handler can check to determine if the event should truly execute or not.  After the function returns the flag should be cleared to enable regular event handling again.</summary>
    ///<param name="containerID" type="String">ID of the element that contains the controls to reset.</param>

    $resetErrors(containerID);

    //Input - Text - the :not([id$=_Input]) is to filter out RadComboBox inputs
    $("#" + containerID + " input[type=text]:not([id$=_Input]):not(.staticFilter)").val("");
    //TextArea
    $("#" + containerID + " textarea").val("");
    //Input - Checkbox
    $("#" + containerID + " input[type = checkbox]").each(function() { this.checked = this.value === 0; });

    //CheckboxList (server generated) - do after regular checkboxes
    $("#" + containerID + " .chkBoxDiv").each(function()
    {
        if (this.control && this.control.reset)
            this.control.reset();
    });

    //RadCombos
    $("#" + containerID + " div table").each(
    function()
    {
        if (this.parentNode.control && ((this.parentNode.control.get_selectedIndex && this.parentNode.control.get_selectedIndex() > 0) || this.parentNode.checkboxList))
        {
            if (this.parentNode.checkboxList)
            {
                this.parentNode.checkboxList.reset(); //("0");
                $updateCheckboxDropdownText(this.parentNode.control);
            }
            else
                this.parentNode.control.get_items().getItem(0).select();
        }
    }
    );
};

$openPlanWebsite = Pathfinder.UI.ClientManager.openPlanWebsite = function(sender, event)
{
    var grid = clientManager.get_PlanInfoGrid();
    var mt = grid.get_masterTableView();

    if (!event)
        event = window.event;

    if (!event.ctrlKey) return;

    if (document.selection) document.selection.empty();

    var cell = sender;
    while (cell != null && cell.tagName != "TD")
        cell = cell.parentNode;

    var rect = Sys.UI.DomElement.getBounds(cell);

    $openWebsite($(cell).text(), rect.x, rect.y, grid.get_element().id);
    event.cancelBubble = true;
    event.returnValue = false;
    return false;
};

$alert = function(message, header, width, height, type)
{
    if (!$get("dashboardAlert"))
    {
        alert(message);
        return;
    }

    var windowHeight = $(window);

    if (width == null) width = 350;
    if (height == null) height = 150;

    var x = (windowHeight.width() / 2) - (width / 2);
    var y = (windowHeight.height() / 2) - (height / 2);

    $("#dashboardAlert .close").show();

    var iconClass = $("#dashboardAlert #warningIcon").attr("class");

    if (message)
    {
        $("#dashboardAlert .message").html(message).height(height);

        if (!type) type = Pathfinder.UI.AlertType.Warning;

        iconClass = (type == 1 || !iconClass ? "warningIcon" : type == 2 ? "infoIcon" : type == 3 ? "errorIcon" : iconClass);
    }

    $("#dashboardAlert #warningIcon").attr("class", "").addClass(iconClass);

    if (header)
        $("#dashboardAlert .header .title .text").text(header);

    if ($("#dashboardAlert").css("visibility") != "visible")
    {
        $("#dashboardAlert").width(width).css({ "left": x + "px", "top": y + "px" }).show();

        if ($get("warnings"))
            $("#warnings").hide("transfer", { to: "#dashboardAlert", className: 'ui-effects-transfer' }, 500, function() { $("#dashboardAlert").css("visibility", "visible"); });
        else
            $("#dashboardAlert").css("visibility", "visible").css("z-index", "10001");

        $(document).click($hideAlert);
    }

    $("#warnings").attr("class", "").css("visibility", "visible").addClass(iconClass);

};

$clearAlert = function()
{
    //force hiding the alert and warning icon
    $("#dashboardAlert").css("visibility", "hidden").hide();
    $("#warnings").css("visibility", "hidden");
    $(document).unbind("click", $hideAlert);
};

$hideAlert = function(e)
{
    //don't hide if request button or warning button caused click otherwise we would never see alert    
    if (e && (e.target.id == "requestReportButton" || e.target.id == "warnings" || $(e.target).hasClass("submitButton"))) return;

    var r = Sys.UI.DomElement.getBounds($get("dashboardAlert"));

    //hide alert if click outside div
    if (e == null || e.clientX < r.x || e.clientX > (r.x + r.width) || e.clientY < r.y || e.clientY > (r.y + r.height))
    {
        if ($get("warnings"))
            $("#dashboardAlert").css("visibility", "hidden").hide("transfer", { to: "#warnings", className: 'ui-effects-transfer' }, 500);
        else
            $("#dashboardAlert").css("visibility", "hidden").hide();

        $(document).unbind("click", $hideAlert);
    }
};

$openWebsite = Pathfinder.UI.ClientManager.openWebsite = function(url, x, y, activeRegion)
{
    ///<summary>Opens the specified url in a new window.  If however the url is a semi-colon separated list of url's a small popup is opened with the list of sites to select from.</summary>

    var a = url.split(";");
    var list = [];
    var u;
    for (var s in a)
    {
        u = $.trim(a[s]).replace(new RegExp(String.fromCharCode(160), "ig"), "");
        if (u != "")
            list[list.length] = u;
    }

    if (list.length == 1)
    {
        //hide in case tooltip open from another link
        $("#infoPopup").hide();

        window.open("http://" + $.trim(list[0]));
    }
    else if (list.length > 1)
    {
        clientManager.openViewer("content/linkselection.aspx?links=" + encodeURIComponent(list.join(";")), x - 325, y - 20, 325, 70, activeRegion);
    }
};

$createCheckboxDropdown = Pathfinder.UI.ClientManager.createCheckboxDropdown = function(id, innerListID, items, properties, events, containerID)
{
    ///<summary>Creates a CheckboxList control and injects its HTML into a RadComboBox</summary>
    ///<param name="id" type="String">Element ID of the RadComboBox</param>
    ///<param name="innerListID" type="String">Element ID to assign to the element that will host the checkbox list.</param>
    ///<param name="items" type="Object" optional="true">Collection of items that are added to the checkbox list after it is created.</param>
    ///<param name="properties" type="Object" optional="true">Collection of name/value pairs to set properties of the checkbox list.</param>
    ///<param name="events" type="Function" optional="true">Collection of name/value pairs of event handlers to apply to the checkbox list.</param>
    ///<param name="containerID" type="String" optional="true">Container ID that is hosting the controls.  This is the id of the element that is loaded and unloaded for partial page updates.</param>

    //inject div that into combobox's rcbScroll div.  The inner div will hold checkboxlist component.  If a containerID is specified it is added as a css class to the inner div for extracting values - see $getContainerData - since outer div is floated outside the combobox there is no other way to select the checkboxes for a particular area of the page.
    $("#" + id + " .rcbScroll").html("<div id='" + innerListID + "' name='" + innerListID + "'></div>");

    if (!properties) properties = {};

    properties["breakCount"] = 1;
    properties["itemCssClass"] = properties["itemCssClass"] ? properties["itemCssClass"] + " notfilter" : "notfilter";

    $get(id).checkboxList = $create(Pathfinder.UI.CheckboxList, properties, events, null, $get(innerListID));

    if (items)
    {
        $loadPinsoListItems(innerListID, items);

        $updateCheckboxDropdownText(id, innerListID);
    }

    var c = $find(id);
    if (c)
    {
        c.add_dropDownOpening(Pathfinder.UI.ClientManager.updateCheckboxDropDownHeight);
        c.add_dropDownClosed($updateCheckboxDropdownText);
        c.add_disposing(Pathfinder.UI.ClientManager.removeCheckboxDropDownOnDispose);
    }

    if (clientManager)
        clientManager.registerComponent(innerListID, null, null, containerID);
};

Pathfinder.UI.ClientManager.removeCheckboxDropDownOnDispose = function(sender, args)
{
    ///<summary>Event handler for the RadComboBox's disposing event for the purpose of removing event handlers.  This function is attached when $createCheckboxDropdown is called for a combo box.  *This function should not be called directly.</summary>

    sender.remove_dropDownOpening(Pathfinder.UI.ClientManager.updateCheckboxDropDownHeight);
    sender.remove_dropDownClosed($updateCheckboxDropdownText);
    sender.remove_disposing(Pathfinder.UI.ClientManager.removeCheckboxDropDownOnDispose);
};

Pathfinder.UI.ClientManager.updateCheckboxDropDownHeight = function(sender, args)
{
    ///<summary>Event handler for the RadComboBox's dropDownOpening event that automatically adjusts the height of the list based on the number of available items.  This function is attached when $createCheckboxDropdown is called for a combo box.  *This function should not be called directly.</summary>    
    var height = 0;

    //    if (!maxItems) 
    var maxItems = 6;

    if (sender.get_element().checkboxList)
    {
        var count = sender.get_element().checkboxList.get_count();
        if (count > maxItems)
            count = maxItems;

        height = count * 23;
    }
    if (height > 0)
        $("#" + sender.get_id() + "_DropDown .rcbScroll").height(height);
};

$updateCheckboxDropdownText = Pathfinder.UI.ClientManager.updateCheckboxDropdownText = function(c, innerListID, truncateLength)
{
    if (typeof (c) == "string")
        c = $find(c);

    if (c)
    {
        if (typeof (innerListID) == "string")
            c.set_text($find(innerListID).get_text());
        else
            c.set_text(c.get_element().checkboxList.get_text());

        //        $(c.get_element()).find("input:first").focus();

        truncateMenu(c.get_id(), truncateLength ? truncateLength : 28);
    }
};

$exportModule = function(type, fromModal, application, channel, module, filters)
{
    var isPrintPage = type == "print";

    // Excel export
    var report = module;

    //Check if IE9
    //$.browser.version is incorrect since we are using IE9 Compatibility Mode, so check User Agent instead
    //    var ie9 = false;
    //    if (navigator.userAgent.indexOf("Trident/5") > -1)
    //        ie9 = true;

    var url = (fromModal && ((!$.browser.msie) || (ie9)) ? "../" : "") + "usercontent/" + (isPrintPage ? "print.aspx?" : "export.ashx?");

    var data = "";
    var dataItem;

    data += ("report=" + report);

    data += "&application=" + application + "&channel=" + channel;

    var items = 0;
    $("." + report + " .chartThumb IMG").each(function()
    {
        dataItem = "chartid=" + this.getAttribute("_chartid") + "&height=" + this.getAttribute("_height") + "&width=" + this.getAttribute("_width") + "&title=" + (this.getAttribute("_title") ? this.getAttribute("_title") : "");

        data += ("&_img" + items + "=" + encodeURIComponent(dataItem));

        items++;
    });

    if (!$.isArray(filters))
        filters = [filters];

    if (filters == null) { filters = []; }
    for (var i = 0; i < filters.length; i++)
    {
        dataItem = $getDataForPostback(filters[i]);

        data += ("&_data" + i + "=" + encodeURIComponent(dataItem));
    }

    if (isPrintPage)
    {
        var fr = window.top.open(url + data, 'printWindow', 'top=0,left=0,resizable=1,menubar=1,width=800,height=480,status=0,toolbar=0,location=0,scrollbars=1');

        if (!chrome)
            fr.print();

        fr.focus();
    }
    else //if (type == 'excel')
    {
        window.top.open(url + data);
    }
};


Pathfinder.UI.ClientManager.prototype =
{
    initialize: function()
    {
        var start = new Date();

        _startUp = new Date(start - _startUp);
        var startUpTime = _startUp.getSeconds() + ":" + _startUp.getMilliseconds();


        Pathfinder.UI.ClientManager.callBaseMethod(this, 'initialize');

        window.top.clientManager = this;

        //history tracking
        var iframe = $get("_clientmanagerhistory");  //document.createElement("IFRAME");
        //        iframe.id = "_clientmanagerhistory";
        //        iframe.style.display = "none";
        //        document.body.appendChild(iframe);
        if (iframe)
            $addHandler(iframe, "load", (this._onHistoryChangeDelegate = Function.createDelegate(this, this._onHistoryChange)));
        //


        var c; //generic ctrl var

        //initialize Menu items and event handlers - track app id also to make sure it is valid ($loadMenuItems will return a valid id if default provided doesn't match but we have to do twice because of submenu)
        c = this.get_ApplicationMenu();
        var uapp = $loadMenuItems(c, $.grep(this.get_ApplicationMenuOptions(), function(i, x) { return !i.Custom; }), null, this._application ? this._application : Pathfinder.UI.Applications.TodaysAccounts);
        if (uapp != Pathfinder.UI.Applications.TodaysAccounts) this._application = uapp;
        var custom = $.grep(this.get_ApplicationMenuOptions(), function(i, x) { return i.Custom; });
        if (custom && custom.length)
        {
            //code for sorting custom apps based on application name
            custom = custom.sort(function(a, b)
            {
                a = a.Name.toUpperCase();
                b = b.Name.toUpperCase();
                if (a > b)
                    return 1;
                else if (a < b)
                    return -1;
                else return 0;

            });
            var mi = c.get_items().getItem(0);
            if (mi)
            {
                cmi = new Telerik.Web.UI.RadMenuItem();
                cmi.set_text("Custom Options");
                cmi.set_value(0);
                mi.get_items().add(cmi);

                $(cmi.get_element()).hover(function() { $(this).addClass('menuHoverItem') }, function() { $(this).removeClass('menuHoverItem'); });

                uapp = $loadMenuItems(cmi, custom, null, this._application ? this._application : Pathfinder.UI.Applications.TodaysAccounts);
                if (uapp != Pathfinder.UI.Applications.TodaysAccounts) this._application = uapp;
            }
        }
        var appID = this._application;
        var app = $.grep(this.get_ApplicationMenuOptions(), function(i, x) { return i.ID == appID; });
        if (!app || app.length == 0)
            this._application = this.get_ApplicationMenuOptions()[0].ID; //Pathfinder.UI.Applications.TodaysAccounts;

        if (c)
        {
            c.add_itemOpening(function(s, a) { a.set_cancel(processing > 0); });
            c.add_itemClicked((this._onApplicationSelectionChangedDelegate = Function.createDelegate(this, this._onApplicationSelectionChanged)));
        }
        c = this.get_ChannelMenu();
        //MENUCHANGE
        //if (c) c.add_itemClicked((this._onChannelSelectionChangedDelegate = Function.createDelegate(this, this._onChannelSelectionChanged)));
        if (c) c.add_dropDownClosed((this._onChannelSelectionChangedDelegate = Function.createDelegate(this, this._onChannelSelectionChanged)));

        //initialize Market Basket & Drug List event handlers
        //        var mb = this.get_MarketBasketList();
        //        if (mb)
        //        {
        //            var value = $loadMenuItems(mb, this.get_MarketBasketListOptions(), null, this.get_MarketBasket());

        //            mb.add_itemClicked((this._onMarketBasketListChangedDelegate = Function.createDelegate(this, this._onMarketBasketListChanged)));
        //            if (!this._marketBasket || this._marketBasket != value) this._marketBasket = value;
        //            //
        //            c = this.get_DrugList();
        //            if (c)
        //            {
        //                value = $loadMenuItems(c, this.get_DrugListOptions()[this._marketBasket], null, this.get_Drug());

        //                c.add_itemClicked((this._onDrugListChangedDelegate = Function.createDelegate(this, this._onDrugListChanged)));
        //                if (!this._drug || this._drug != value) this._drug = value;
        //            }
        //        }

        //        c = this.get_RegionList();
        //        if (c)
        //        {
        //            if (this.get_UserGeography().RegionID == null)
        //            {
        //                $loadMenuItems(c, this.get_RegionListOptions(), { text: 'All Regions', value: 0 });
        //                c.add_itemClicked((this._onRegionListChangedDelegate = Function.createDelegate(this, this._onRegionListChanged)));
        //            }
        //            else
        //                c.set_visible(false);
        //        }

        //delegate for handling page loads - checks for errors plus additional maintenance tasks as needed
        this._onLoadPageDelegate = Function.createDelegate(this, this._onLoadPage);

        c = $get("contacts");
        if (c) $addHandler(c, "click", (this._onContactSearchDelegate = Function.createDelegate(this, this._onContactSearch)));

        $(document).click(this._onShowFavoritesDelegate = Function.createDelegate(this, function(e)
        {
            if (e.target.id != "ctl00_main_subheader1_linkViewFavorites")
            {
                $("#favoritesListWindow").slideUp("fast");
            }
        }));

        //Module Menu
        $create(Pathfinder.UI.Menu, null, { "itemClicked": (this._onModuleMenuItemClickedDelegate = Function.createDelegate(this, this._onModuleMenuItemClicked)) }, null, $get("divModuleOptions"));
        var m = $find("moduleSelector");
        m.add_itemClicked(this._onModuleMenuItemClickedDelegate);

        //restore settings - makes sure view shows what CM properties were initialized with
        this.restoreView();


        //        //Initialize map with current ui state - loads map data based on current Channel & Drug
        //        //Using cmd object to delay initialization .5 seconds to make sure map object is ready.  initMap function will keep trying up to 5 times until successful
        //        new cmd(null, initMap, this.get_CurrentUIStateAsText(this.mapDataRequestUIStateProperties), 1500);
        //        //


        //init tracking and update history
        this._trackingEnabled = true;

        this._updateHistory();

        this._initialized = true;

        setPageUIState(clientManager);

        $(window).wresize((this._windowResizedDelegate = Function.createDelegate(this, this._windowResized)));

        this._windowResized();

        $(document).keydown(function() { $(".planLink").css("cursor", Cursor.Pointer) });
        $(document).keyup(function() { $(".planLink").css("cursor", Cursor.None) });

        //        $("#table1, .navbar2, #section2").css("visibility", "visible");

        var end = new Date();

        end = new Date(end - start);
        //        alert("Page Init: " + startUpTime + "\n\nCM Init: " + end.getSeconds() + ":" + end.getMilliseconds());
    },

    dispose: function()
    {
        var c = $get("contacts");
        if (c) try { $removeHandler(c, "keypress", this._onContactSearchDelegate); } catch (ex) { };

        c = $get("_clientmanagerhistory");
        if (c) try { $removeHandler(iframe, "load", this._onHistoryChangeDelegate); } catch (ex) { };

        $(document).unbind("keydown keyup");
        $(window).unbind("wresize");

        delete (this._controls);
        delete (this._moduleOptions);

        delete (this._onContactSearchDelegate);
        delete (this._onLoadPageDelegate);
        delete (this._onPlanGridRowSelectedDelegate);
        delete (this._onPlanGridDataBoundDelegate);
        //        delete (this._onDrugListChangedDelegate);
        //        delete (this._onMarketBasketListChangedDelegate);
        delete (this._onFavoriteSavedDelegate);
        delete (this._onHistoryChangeDelegate);
        delete (this._onApplicationSelectionChangedDelegate);
        delete (this._onChannelSelectionChangedDelegate);
        delete (this._onShowFavoritesDelegate);
        delete (this._onModuleMenuItemClickedDelegate);
        delete (this._windowResizedDelegate);
        //        delete (this._onRegionListChangedDelegate);

        Pathfinder.UI.Application.destroyInstances();

        Pathfinder.UI.ClientManager.callBaseMethod(this, 'dispose');
    },

    add_uiStateChanged: function(handler) { this.get_events().addHandler("uistatechanged", handler); },
    remove_uiStateChanged: function(handler) { this.get_events().removeHandler("uistatechanged", handler); },

    add_applicationChanging: function(handler) { this.get_events().addHandler("applicationchanging", handler); },
    remove_applicationChanging: function(handler) { this.get_events().removeHandler("applicationchanging", handler); },

    add_applicationChanged: function(handler) { this.get_events().addHandler("applicationchanged", handler); },
    remove_applicationChanged: function(handler) { this.get_events().removeHandler("applicationchanged", handler); },

    add_channelChanging: function(handler) { this.get_events().addHandler("channelchanging", handler); },
    remove_channelChanging: function(handler) { this.get_events().removeHandler("channelchanging", handler); },

    add_channelChanged: function(handler) { this.get_events().addHandler("channelchanged", handler); },
    remove_channelChanged: function(handler) { this.get_events().removeHandler("channelchanged", handler); },

    add_effectiveChannelChanged: function(handler) { this.get_events().addHandler("effectivechannelchanged", handler); },
    remove_effectiveChannelChanged: function(handler) { this.get_events().removeHandler("effectivechannelchanged", handler); },

    add_moduleChanging: function(handler) { this.get_events().addHandler("modulechanging", handler); },
    remove_moduleChanging: function(handler) { this.get_events().removeHandler("modulechanging", handler); },

    add_moduleChanged: function(handler) { this.get_events().addHandler("modulechanged", handler); },
    remove_moduleChanged: function(handler) { this.get_events().removeHandler("modulechanged", handler); },

    add_drugChanging: function(handler) { this.get_events().addHandler("drugchanging", handler); },
    remove_drugChanging: function(handler) { this.get_events().removeHandler("drugchanging", handler); },

    add_drugChanged: function(handler) { this.get_events().addHandler("drugchanged", handler); },
    remove_drugChanged: function(handler) { this.get_events().removeHandler("drugchanged", handler); },

    add_marketBasketChanging: function(handler) { this.get_events().addHandler("marketbasketchanging", handler); },
    remove_marketBasketChanging: function(handler) { this.get_events().removeHandler("marketbasketchanging", handler); },

    add_marketBasketChanged: function(handler) { this.get_events().addHandler("marketbasketchanged", handler); },
    remove_marketBasketChanged: function(handler) { this.get_events().removeHandler("marketbasketchanged", handler); },

    add_regionChanging: function(handler) { this.get_events().addHandler("regionchanging", handler); },
    remove_regionChanging: function(handler) { this.get_events().removeHandler("regionchanging", handler); },

    add_regionChanged: function(handler) { this.get_events().addHandler("regionchanged", handler); },
    remove_regionChanged: function(handler) { this.get_events().removeHandler("regionchanged", handler); },

    add_geographyChanging: function(handler) { this.get_events().addHandler("geographychanging", handler); },
    remove_geographyChanging: function(handler) { this.get_events().removeHandler("geographychanging", handler); },

    add_geographyChanged: function(handler) { this.get_events().addHandler("geographychanged", handler); },
    remove_geographyChanged: function(handler) { this.get_events().removeHandler("geographychanged", handler); },

    add_selectionChanging: function(handler) { this.get_events().addHandler("selectionchanging", handler); },
    remove_selectionChanging: function(handler) { this.get_events().removeHandler("selectionchanging", handler); },

    add_selectionChanged: function(handler) { this.get_events().addHandler("selectionchanged", handler); },
    remove_selectionChanged: function(handler) { this.get_events().removeHandler("selectionchanged", handler); },

    add_mapIsReady: function(handler) { this.get_events().addHandler("mapisready", handler); },
    remove_mapIsReady: function(handler) { this.get_events().removeHandler("mapisready", handler); },

    add_mapUpdating: function(handler) { this.get_events().addHandler("mapupdating", handler); },
    remove_mapUpdating: function(handler) { this.get_events().removeHandler("mapupdating", handler); },

    add_restoringView: function(handler) { this.get_events().addHandler("restoringview", handler); },
    remove_restoringView: function(handler) { this.get_events().removeHandler("restoringview", handler); },

    add_restoredView: function(handler) { this.get_events().addHandler("restoredview", handler); },
    remove_restoredView: function(handler) { this.get_events().removeHandler("restoredview", handler); },

    add_pageInitialized: function(handler, containerID)
    {
        ///<summary>Add pageInitialized event.  Event is fired after scripts have been initialized and prior to refreshing any grids that have auto update enabled.</summary>
        ///<param name="handler">Function that handles event.  It should expect two parameters sender and args.</param>
        ///<param name="containerID" type="String" optional="true">Optional ID of a page element that the page is loaded into.  If not specified the default value is "section2".</param>

        if (!containerID) containerID = "section2";
        this.get_events().addHandler(containerID + "_pageinitialized", handler);
    },
    remove_pageInitialized: function(handler, containerID)
    {
        if (!containerID) containerID = "section2";
        this.get_events().removeHandler(containerID + "_pageinitialized", handler);
    },

    add_pageLoaded: function(handler, containerID)
    {
        ///<summary>Add pageLoaded event.  Event is fired after scripts have been initialized and any grids with auto update enabled have been refreshed.</summary>
        ///<param name="handler">Function that handles event.  It should expect two parameters sender and args.</param>
        ///<param name="containerID" type="String" optional="true">Optional ID of a page element that the page is loaded into.  If not specified the default value is "section2".</param>

        if (!containerID) containerID = "section2";
        this.get_events().addHandler(containerID + "_pageloaded", handler);
    },
    remove_pageLoaded: function(handler, containerID)
    {
        if (!containerID) containerID = "section2";
        this.get_events().removeHandler(containerID + "_pageloaded", handler);
    },

    add_pageUnloaded: function(handler, containerID)
    {
        ///<summary>Add pageUnload event.  Event is fired prior to the "container" being reloaded or when the container is explicitly unloaded.</summary>
        ///<param name="handler">Function that handles event.  It should expect two parameters sender and args.</param>
        ///<param name="containerID" type="String" optional="true">Optional ID of a page element that the page is loaded into.  If not specified the default value is "section2".</param>

        if (!containerID) containerID = "section2";
        this.get_events().addHandler(containerID + "_pageunloaded", handler);
    },
    remove_pageUnloaded: function(handler, containerID)
    {
        if (!containerID) containerID = "section2";
        this.get_events().removeHandler(containerID + "_pageunloaded", handler);
    },

    add_formSubmitting: function(handler, containerID)
    {
        if (!containerID) containerID = "section2";
        this.get_events().addHandler(containerID + "_formsubmitting", handler);
    },
    remove_formSubmitting: function(handler, containerID)
    {
        if (!containerID) containerID = "section2";
        this.get_events().removeHandler(containerID + "_formsubmitting", handler);
    },

    add_formSubmitted: function(handler, containerID)
    {
        if (!containerID) containerID = "section2";
        this.get_events().addHandler(containerID + "_formsubmitted", handler);
    },
    remove_formSubmitted: function(handler, containerID)
    {
        if (!containerID) containerID = "section2";
        this.get_events().removeHandler(containerID + "_formsubmitted", handler);
    },

    add_favoriteSaved: function(handler) { this.get_events().addHandler("favoritesaved", handler); },
    remove_favoriteSaved: function(handler) { this.get_events().removeHandler("favoritesaved", handler); },

    get_BasePath: function()
    {
        ///<summary>Returns the web application's base path.</summary>

        return this._basePath;
    },
    set_BasePath: function(value)
    {
        if (value == "/") value = "";
        this._basePath = value;
    },

    get_UIReady: function()
    {
        ///<summary>Returns true when ClientManager's initialized method has been called and tracking is currently enabled.</summary>
        return (this._initialized && this._trackingEnabled);
    },

    get_TrackingEnabled: function()
    {
        ///<summary>Indicates user history is being tracked.  Changes to options such as Application, Channel, and Module will be tracked to allow user back/forward navigation.</summary>
        return this._trackingEnabled;
    },

    get_MapIsReady: function()
    {
        return this._mapReady;
    },

    //NOT NEEDED WITH jQ 1.4.2 - SPH 4/5/2010
    //    get_AjaxRequestTimeout: function()
    //    {
    //        ///<summary>Gets the amount of time an AJAX request is allowed to wait until the user is assumed to have timed out.  This setting is used as a workaround for not getting receiving proper error responses to a user's server session timing out.  If no valid response is received in the specified time period the application redirects to the signout.aspx page.</summary>
    //        return this._ajaxRequestTimeout;
    //    },
    //    set_AjaxRequestTimeout: function(value)
    //    {
    //        ///<summary>Sets the amount of time an AJAX request is allowed to wait until the user is assumed to have timed out.  This setting is used as a workaround for not getting receiving proper error responses to a user's server session timing out.  If no valid response is received in the specified time period the application redirects to the signout.aspx page.</summary>
    //        this._ajaxRequestTimeout = value;
    //    },

    get_ApplicationMenuOptions: function()
    {
        ///<summary>Gets the list of items that are used to load the application menu.  This property should be set prior to initialization.</summary>
        return this._applicationMenuOptions;
    },
    set_ApplicationMenuOptions: function(value)
    {
        ///<summary>Sets the list of items that are used to load the application menu.  This property should be set prior to initialization.</summary>
        this._applicationMenuOptions = value;
    },

    get_ChannelMenuOptions: function()
    {
        ///<summary>Gets the list of items that are used to load the channels menu.  This property should be set prior to initialization.</summary>
        return this._channelMenuOptions;
    },
    set_ChannelMenuOptions: function(value)
    {
        ///<summary>Sets the list of items that are used to load the channels menu.  This property should be set prior to initialization.</summary>
        this._channelMenuOptions = value;
    },

    get_ModuleOptions: function()
    {
        return this._moduleOptions;
    },
    set_ModuleOptions: function(value)
    {
        this._moduleOptions = value;
    },

    getModuleOptionsByApp: function(appID)
    {
        var opts = this.get_ModuleOptions();
        if (opts)
        {
            opts = opts[appID];
        }

        if (opts)
            return opts;
        else
            return [];
    },

    get_States: function()
    {
        ///<summary>Gets the list of states.</summary>

        if (!this._states)
            this.set_States(statesList);
        return this._states;
    },
    set_States: function(value)
    {
        ///<summary>Sets the list of states.</summary>

        this._states = {};
        for (var i = 0; i < value.length; i++)
        {
            this._states[value[i].Key] = value[i].Value;
        }
    },

    get_RegionListOptions: function()
    {
        ///<summary>Gets the list of items that are used to load the regions menu.  This property should be set prior to initialization.</summary>
        return this._regionListOptions;
    },
    set_RegionListOptions: function(value)
    {
        ///<summary>Sets the list of items that are used to load the regions menu.  This property should be set prior to initialization.</summary>
        this._regionListOptions = value;
    },

    getRegionNameByID: function(ID)
    {
        var region = this.get_States()[ID];
        if (!region)
        {
            region = $.grep(this.get_RegionListOptions(), function(i, x) { return i.Key == ID; });
            if ($.isArray(region) && region.length > 0)
                region = region[0].Value + " [" + region[0].Key + "]";
            else
                region = null;
        }

        return region ? region : "";
    },

    //    get_RegionGeographyListOptions: function()
    //    {
    //        ///<summary>Gets the list of geography details for each territory if applicable.  Details include area and center point.  The data is used to configure the map when a territory is selected.</summary>
    //        return this._regionGeographyListOptions;
    //    },
    //    set_RegionGeographyListOptions: function(value)
    //    {
    //        ///<summary>Sets the list of geography details for each territory if applicable.  Details include area and center point.  The data is used to configure the map when a territory is selected.</summary>
    //        this._regionGeographyListOptions = value;
    //    },

    get_MarketBasketListOptions: function()
    {
        ///<summary>Gets the list of items that are used to load the market basket menu.  This property should be set prior to initialization.</summary>
        return this._marketBasketListOptions;
    },
    set_MarketBasketListOptions: function(value)
    {
        ///<summary>Sets the list of items that are used to load the market basket menu.  This property should be set prior to initialization.</summary>
        this._marketBasketListOptions = value;
    },
    get_DrugListOptions: function()
    {
        ///<summary>Gets the list of items that are used to load the drug menu.  This property should be set prior to initialization.</summary>
        return this._drugListOptions;
    },
    set_DrugListOptions: function(value)
    {
        ///<summary>Sets the list of items that are used to load the drug menu.  This property should be set prior to initialization.</summary>
        this._drugListOptions = value;
    },

    get_ApplicationUrlName: function()
    {
        ///<summary>Gets the current application's name for use in a url.</summary>
        var app = this.get_ApplicationManager();
        if (app)
            return app.get_UrlName();

        return "";
    },

    get_UserKey: function()
    {
        ///<summary>Gets a user specific value that can be used to uniquely identify web requests.  This value is added to a url's query string so the requested page is cached for the current user only.  This value should not be used to query for user data.  This property should be set prior to initialization.</summary>

        return this._userKey;
    },
    set_UserKey: function(value)
    {
        ///<summary>Sets a user specific value that can be used to uniquely identify web requests.  This value is added to a url's query string so the requested page is cached for the current user only.  This value should not be used to query for user data.  This property should be set prior to initialization.</summary>    

        if (this._userKey == null || this._userKey == "")
            this._userKey = value;
        else
            throw new Error("UserKey property cannot be set once initialized.");
    },

    get_ClientAccount: function()
    {
        return this._clientAccount;
    },

    get_ClientKey: function()
    {
        ///<summary>Gets the current user's client key.  This key is a unique identifier for each client and is used in constructing client specific URLs.  Default value is "pinso".</summary>
        return this.get_ClientAccount().get_ClientKey();
    },
    set_ClientKey: function(value)
    {
        ///<summary>Sets the current user's client key.  This key is a unique identifier for each client and is used in constructing client specific URLs.</summary>
        return this.get_ClientAccount().set_ClientKey(value);
    },

    get_ClientOptions: function() { },
    set_ClientOptions: function(value)
    {
        var clientKey = this.get_ClientKey();
        delete (this._clientAccount);
        this._clientAccount = new Pathfinder.UI.ClientAccount(clientKey, value);
    },

    get_Application: function()
    {
        ///<summary>Gets the current application identifier.</summary>        
        return this._application;
    },
    set_Application: function(value)
    {
        ///<summary>Sets the current application identifier and updates the dashboard to display appropriate menus and sections.</summary>

        if (value != null && value != "" && !isNaN(value))
            value = parseInt(value, 10);
        else
            value = Pathfinder.UI.Applications.TodaysAccounts;


        if (this._application != value)
        {
            var e = new Sys.CancelEventArgs();

            var h = this.get_events().getHandler("applicationchanging");
            if (h) h(this, e);

            if (!e.get_cancel())
            {
                if (this._applicationManager) this._applicationManager.deactivate(this);

                this._application = value;

                if (this.get_UIReady())
                    new cmd(this, "_onApplicationChanged", [], 300);
            }
        }
    },

    get_ApplicationManager: function()
    {
        ///<summary>Returns to application object for the last application selected.</summary>
        return this._applicationManager;
    },

    getChannelUrlName: function(application, channel)
    {
        return this.getChannelProp(application, channel, "Folder", "all");
    },

    get_ChannelUrlName: function()
    {
        ///<summary>Gets the current section's folder name.  This property is used in constructing urls for retrieving content for a section.  The return value corresponds to a subfolder of an application.</summary>
        return this.getChannelUrlName(this.get_Application(), this.get_EffectiveChannel());
    },

    get_Channel: function()
    {
        ///<summary>Gets the current section identifier.  If All sections are relavent then 0 is returned.  See EffectiveSection property to determine the section that is actually active when All is selected.</summary>        
        return this._channel;
    },
    set_Channel: function(value)
    {
        ///<summary>Sets the current section identifier and updates the dashboard to display the appropriate menus and sections.</summary>
        var channels = [];

        if ($.isArray(value))
            for (var i = 0; i < value.length; i++) channels.push(parseInt(value[i], 10));
        else
            channels.push(parseInt(value, 10));

        //if (value != null && !isNaN(value))
        //    value = parseInt(value, 10);

        //if (this._channel != value)
        //Fire onChannelChanged if the channels selected are different
        var same = $(this._channel).not(channels).get().length == 0 && $(channels).not(this._channel).get().length == 0;
        if (!same)
        {
            this._rebindPlanInfoGrid = true;
            var e = new Sys.CancelEventArgs();

            var h = this.get_events().getHandler("channelchanging");
            if (h) h(this, e);

            if (!e.get_cancel())
            {
                //this._channel = value;
                //this._effectiveChannel = value;
                this._channel = channels;
                //this._effectiveChannel = channels;
                if (this.get_UIReady())
                    new cmd(this, "_onChannelChanged", [], 300);
            }
        }
        else
            this._rebindPlanInfoGrid = false;
    },

    get_EffectiveChannel: function()
    {
        ///<summary>Gets the current section identifier.  The value returned by this property is the same as Section unless Section is 0 for 'All'.  If Section is 0 then this value will return the active section as determined by some other event, other than the Section menu, such as a row click in a grid.</summary>
        return this._effectiveChannel;
    },
    set_EffectiveChannel: function(value)
    {
        ///<summary>Sets the current effective section identifier and updates the dashboard to display appropriate menus and sections.  The value can only be set if the Section property returns 0 for 'All'.</summary>
        //if (this.get_Channel() != 0)
        //    throw new Error("ClientManager.set_EffectiveChannel: EffectiveChannel property cannot be set directly if current section is not 'All' (value of 0)");

        if (this._effectiveChannel != value)
        {
            this._effectiveChannel = value;
            if (this.get_UIReady())
                this._onEffectiveChannelChanged();
        }
    },

    get_Module: function()
    {
        ///<summary>Gets the current module identifier.  The return value corresponds to a section's subfolder where content for the module is found.</summary>
        if (this._module == null && this.get_ApplicationManager())
            return this.get_ApplicationManager().getDefaultModule(this);


        return this._module;
    },
    set_Module: function(value)
    {
        ///<summary>Sets the current module identifier and updates the dashboard to display appropriate menus and sections.  The value can be any text that represents a unique part of the application and should correspond to a subfolder on the server where the module's content can be found.  Changing the value will update the dash</summary>
        if (value != null) value = value.toLowerCase();

        if (this._module != value)
        {
            var e = new Pathfinder.UI.PropertyChangingEventArgs(value);

            var h = this.get_events().getHandler("modulechanging");
            if (h) h(this, e);

            if (!e.get_cancel())
            {
                this._module = value;
                if (this.get_UIReady())
                    this._onModuleChanged();
            }
        }
    },

    get_SelectionData: function(level)
    {
        ///<summary>Gets the data associated with the last selection.  Typically this will be data associated with a selected row of a grid.</summary>

        if (level == null) level = 0;

        if (this._selectionData)
            return this._selectionData[level];

        return null;
    },
    set_SelectionData: function(value, level, switchToModule)
    {
        ///<summary>Sets the data associated with the last selection and updates the dashboard to display appropriate menus and sections.  Typically this value will be data associated with a selected row of a grid.</summary>
        if (level == null) level = 0;

        if (typeof level == "string")
        {
            switchToModule = level;
            level = 0;
        }

        if (switchToModule && this._module != switchToModule)
        {
            //fire events for consistant behavior 
            var e = new Pathfinder.UI.PropertyChangingEventArgs(value);
            var h = this.get_events().getHandler("modulechanging");
            if (h) h(this, e);

            if (!e.get_cancel())
            {
                this._module = switchToModule;
                h = this.get_events().getHandler("modulechanged");
                if (h) h(this, new Sys.EventArgs());
            }
        }

        this._setSelectionData(value, level, true);
    },

    get_SelectionCount: function()
    {
        if (this._selectionData)
            return this._selectionData.length;

        return 0;
    },

    getAllSelectionData: function()
    {
        ///<summary>Returns all selection data at all levels.</summary>
        return this._selectionData;
    },

    clearSelectionData: function(silent)
    {
        ///<summary>Resets selection data value to null with option to call silently meaning no interface updates will occur and selectionChanged event is not fired.</summary>
        if (silent)
            this._selectionData = null;
        else
            this.set_SelectionData(null);
    },

    _setSelectionData: function(value, level, cancellable, unloadCurrent)
    {
        ///<summary>For internal use only.  Use set_SelectionData to change the data associated with the user's current selection.</summary>
        var keepCurrent = unloadCurrent != true && !this._section2Invalid;
        this._section2Invalid = false; //invalid section 2 occurs if effective channel is modified

        var e = new Sys.CancelEventArgs();

        if (cancellable)
        {
            var h = this.get_events().getHandler("selectionchanging");
            if (h) h(this, e);
        }

        if (!e.get_cancel())
        {
            if (this._selectionData == null)
                this._selectionData = [];

            //            for (var i = 0; i < level; i++)
            //            {
            //                if (!this._selectionData[i])
            //                    this._selectionData[i] = {};
            //            }
            this._selectionData[level] = value;

            if (this.get_UIReady())
                this._onSelectionChanged(keepCurrent && value != null && this.isFilteredModule(null, level) && this._controls["section2"] != null && this._controls["section2"].length != 0, cancellable, level);
        }
    },

    get_Region: function()
    {
        ///<summary>Gets the geographic region that has been selected.  This can be a state, VISN, or other defined geographic boundary.  If no region is selected the value is null.</summary>
        return this._region;
    },
    set_Region: function(value)
    {
        ///<summary>Sets the current region which can be a state, VISN, or other defined geographic boundary and updates the dashboard to display the appropriate data.</summary>

        value = (value == "" ? null : value);

        //If region cannot filter plans then we should not allow its selection.        
        if (this._region != value && this._regionCanFilterPlans())
        {
            var e = new Sys.CancelEventArgs();

            var h = this.get_events().getHandler("regionchanging");
            if (h) h(this, e);

            if (!e.get_cancel())
            {
                this._region = value;
                if (this.get_UIReady())
                    this._onRegionChanged();
            }
        }
    },

    get_Drug: function()
    {
        ///<summary>Gets the drug that has been selected.  If no drug has been selected the value is null.</summary>
        return this._drug;
    },
    set_Drug: function(value)
    {
        ///<summary>Sets the current drug id and updates the dashboard to display the appropriate data.</summary>

        if (this._drug != value)
        {
            var e = new Sys.CancelEventArgs();

            var h = this.get_events().getHandler("drugchanging");
            if (h) h(this, e);

            if (!e.get_cancel())
            {
                this._drug = value;
                if (this.get_UIReady())
                    this._onDrugChanged();
            }
        }
    },

    get_MarketBasket: function()
    {
        ///<summary>Gets the market basket that has been selected.  If no market basket has been selected the return value is null.</summary>
        return this._marketBasket;
    },
    set_MarketBasket: function(value)
    {
        ///<summary>Sets the current market basket id and updates the dashboard to display the appropriate data.</summary>

        if (this._marketBasket != value)
        {
            var e = new Sys.CancelEventArgs();

            var h = this.get_events().getHandler("marketbasketchanging");
            if (h) h(this, e);

            if (!e.get_cancel())
            {
                this._marketBasket = value;
                if (this.get_UIReady())
                    this._onMarketBasketChanged();
            }
        }
    },

    get_UserGeography: function()
    {
        ///<summary>Gets the current user's geography data which is the default area and center point for the map.</summary>
        return this._userGeography;
    },
    set_UserGeography: function(value)
    {
        ///<summary>Sets the current user's geography data which is the default area and center point for the map.</summary>
        if (value == null || value == "")
            value = { "CenterX": 0, "CenterY": 0, "Area": 0 };

        this._userGeography = value;
    },

    get_Geography: function()
    {
        ///<summary>Gets the current geography data which is the area and center point for the map.</summary>
        return this._geography;
    },
    set_Geography: function(value)
    {
        ///<summary>Sets the current geography data which is the default area and center point for the map.  Geography property will not be set if UserGeography is already set and has a list of regions associated with it.</summary>
        if (!this.get_UserGeography() || !this.get_UserGeography().Regions)
        {
            if ((this._geography ? this._geography.Regions : "") != (value ? value.Regions : ""))
            {
                var e = new Sys.CancelEventArgs();

                var h = this.get_events().getHandler("geographychanging");
                if (h) h(this, e);

                if (!e.get_cancel())
                {
                    this._geography = value;

                    if (this.get_UIReady())
                        this._onGeographyChanged();
                }
            }
        }
    },
    get_clientHasCustomPlans: function()
    {
        return this._clientHasCustomPlans;
    },
    set_clientHasCustomPlans: function(value)
    {
        this._clientHasCustomPlans = value;
    },
    get_EffectiveGeography: function()
    {
        ///<summary>Gets the active geography object.  If UserGeography has been set to a territory (RegionID is not null) then the UserGeography value is returned otherwise the value last set by a call to set_Geography is returned.</summary>
        var g = this.get_UserGeography();
        if ((!g || !g.RegionID) && this._geography) g = this._geography;

        return g;
    },

    //NOT NEEDED - SPH 4/5/2010 - 
    //    get_PlanInfoGridID: function()
    //    {
    //        ///<summary>Gets the element ID of the Plan Information Grid.</summary>
    //        return "ctl00_main_planInfo_gridPlanInfo";
    //    },

    get_PlanInfoGrid: function()
    {
        ///<summary>Gets the Plan Information Grid control located on the dashboard.</summary>
        if (!this._planInfoGrid)
        {
            var e = $get("ctl00_main_planInfo_gridPlanInfo");
            if (e && e.control)
            {
                this._planInfoGrid = e.control;
            }
        }

        return this._planInfoGrid;
    },

    get_CurrentUIStateAsText: function(props)
    {
        ///<summary>Gets the current user interface state as JSON serialized text for transmission to the server.</summary>

        var text = Sys.Serialization.JavaScriptSerializer.serialize(this.get_CurrentUIState(props));

        return text;
    },
    get_CurrentMapUIStateAsText: function()
    {
        return this.get_CurrentUIStateAsText(this.get_ApplicationManager().get_MapUIStatePropertyNames());
    },
    get_CurrentUIState: function(props)
    {
        ///<summary>Gets the current user interface state in JSON.</summary>
        var o = {};

        if (!props)
        {
            o =
            {
                UserKey: encodeURIComponent(this.get_UserKey()),
                Application: this.get_Application(),
                Channel: this.get_EffectiveChannel(),
                Module: this.get_Module(),
                Region: this.get_Region(),
                MarketBasket: this.get_MarketBasket(),
                Drug: this.get_Drug(),
                Data: this.getAllSelectionData()
            };

            //optional value
            if (this.get_Geography())
                o["Geography"] = this.get_Geography();
        }
        else //custom request
        {
            if (!$.isArray(props)) throw new Error("props parameter must be an array of property names.");

            var method;

            o["UserKey"] = this.get_UserKey(); //always set in case ui state is used in query string - prevent using another user's cached data
            for (var i = 0; i < props.length; i++)
            {
                method = this["get_" + props[i]];
                if (method)
                    o[props[i]] = method.apply(this);
            }
        }

        return o;
    },
    set_CurrentUIState: function(value, noUserValidation)
    {
        ///<summary>Sets the current user interface state values.  These values include Application, Section, Module, Region, Market Basket, and Drug.  This property can accept an Object or JSON formatted text.</summary>

        if (typeof (value) == "string" && value != "")
        {
            try
            {
                value = Sys.Serialization.JavaScriptSerializer.deserialize(value, true);
            }
            catch (ex)
            {
                return;
            }
        }

        if (value != null && value != "") //throw new Error("ClientManager.CurrentUIState: A valid value was not specified.");
        {
            //not setting UserKey - should be the same always for the current user
            if (!this.get_ApplicationMenuOptions() || this.get_ApplicationMenuOptions().length == 0)
                throw new Error("ApplicationMenuOptions property must be set prior to setting CurrentUIState.");

            //if no validation or user keys match then continue - when loading from cookie we would want to validate that user is same that generated cookie - shared favorites we might not care
            if (noUserValidation || this.get_UserKey() == decodeURIComponent(value.UserKey))
            {
                if ($.grep(this.get_ApplicationMenuOptions(), function(i, x) { return i.ID == value.Application; }).length > 0)
                {
                    this.set_Application(value.Application);
                    this.set_Channel(value.Channel);
                    this.set_Module(value.Module);
                    //not using set function because it prevents setting region if filtering by state is disabled for current channel
                    this._region = value.Region;
                    this.set_MarketBasket(value.MarketBasket);
                    this.set_Drug(value.Drug);
                    if ($.isArray(value.Data))
                    {
                        var cm = this;
                        $.each(value.Data, function(idx, item) { cm.set_SelectionData(item, idx); });
                    }
                    else
                        this.set_SelectionData(value.Data);

                    this._onUIStateChanged();
                }
            }
        }
    },

    _onUIStateChanged: function(dataLevel)
    {
        var date = new Date();
        date.setFullYear(date.getFullYear() + 50);
        document.cookie = "s=" + escape(this.get_CurrentUIStateAsText()) + "; expires=" + date.toUTCString();

        if (!dataLevel) //if dataLevel then no need to set page ui state which tracks app, client, and module.
            setPageUIState(this);

        var h = this.get_events().getHandler("uistatechanged");
        if (h) h(this, new Sys.EventArgs());
    },

    get_ApplicationMenu: function()
    {
        ///<summary>Gets the dashboard's Application menu control.</summary>
        var c = $get("ctl00_main_subheader1_applicationMenu");

        if (c) return c.control;

        return null;
    },

    get_ChannelMenu: function()
    {
        ///<summary>Gets the dashboard's Section menu control.</summary>
        var c = $get("ctl00_main_subheader1_channelMenu");

        if (c) return c.control;

        return null;
    },

    get_ChannelMenuCheckBoxList: function()
    {
        ///<summary>Gets the dashboard's Section menu control.</summary>
        var c = $get("Channel_Menu");

        if (c) return c.control;

        return null;
    },

    //    get_MarketBasketList: function()
    //    {
    //        ///<summary>Gets the dashboard's Market Basket menu.</summary>
    //        return $find("ctl00_main_rdlMarketBasketList");
    //    },

    //    get_DrugList: function()
    //    {
    //        ///<summary>Gets the dashboard's Drug menu.</summary>
    //        return $find("ctl00_main_rdlDrugList");
    //    },

    //    get_RegionList: function()
    //    {
    //        ///<summary>Gets the dashboard's Region menu.</summary>
    //        return $find("ctl00_main_rdlRegionList");
    //    },

    //    get_PlanInfoGridFilterTimeout: function()
    //    {
    //        ///<summary>Gets the number of milliseconds that the client manager waits before filtering the grid with the updated value in the search textbox.</summary>
    //        return this._planInfoGridFilterTimeout;
    //    },
    //    set_PlanInfoGridFilterTimeout: function(value)
    //    {
    //        ///<summary>Sets the number of milliseconds that the client manager waits before filtering the grid with the updated value in the search textbox.</summary>
    //        this._planInfoGridFilterTimeout = value;
    //    },

    _windowResized: function(e)
    {
        ///<summary>Event handler for window resize events.</summary>

        var app = this.get_ApplicationManager();
        if (app)
        {
            app.resize(e);
        }
    },

    //    getPlanInfoGridPagerHtml: function()
    //    {
    //        ///<summary>Gets the custom pager HTML for the Plan Information Grid.</summary>
    //        return $("#divTile2Container .pagination").html();
    //    },

    //    copyPlanInfoGridFilters: function(grid)
    //    {
    //        ///<summary>Copies filters from the PlanInfoGrid control to the specified grid control.</summary>
    //        ///<param name="grid" type="Object">Grid control that will be updated with the PlanInfoGrid's filters.</param>
    //        ///<returns type="void" />    
    //        $copyGridFilters(this.get_PlanInfoGrid(), grid);
    //    },

    restoreView: function()
    {
        ///<summary>Restores the dashboard view based on the current property values for Application, Section, Module, Drug, Market Basket, State, and SelectedData.</summary>
        ///<returns type="void" />

        //set page state
        var track = this._trackingEnabled;

        if (track)//if tracking was enabled call event otherwise not restore from current state
        {
            var h = this.get_events().getHandler("restoringview");
            if (h) h(this, new Sys.EventArgs());
        }

        this._trackingEnabled = false;

        this._onApplicationChanged();
        this._onChannelChanged();
        this._onModuleChanged();

        //sync drug menu options to correct settings
        //        $refreshMenuOptions(this.get_MarketBasketList(), this.get_MarketBasket());
        //        $refreshMenuOptions(this.get_DrugList(), this.get_Drug());

        //_doMapCommand may not execute if map not ready so must set this flag now just in case 
        this._pendingRegionZoom = this.get_Region() != null;

        //handles selected data as well as region
        this._doMapCommand("restoreview");

        this._trackingEnabled = track;

        if (track)//if tracking was enabled call event otherwise not restore from current state
        {
            var h = this.get_events().getHandler("restoredview");
            if (h) h(this, new Sys.EventArgs());
        }
        //
    },

    submitForm: function(target, args)
    {
        try
        {
            var containerID;
            var targetCtrl;
            var validate = false;

            if (!target)
                containerID = "section2";
            else if (typeof target == "string")
                containerID = target;
            else
            {
                targetCtrl = target;
                containerID = $(target).attr("_containerID");
                if (!containerID)
                    containerID = "section2";

                validate = $(target).hasClass("validate");
            }

            var url = this.getUrl();
            url += "?" + this.getSelectionDataForPostback();

            //responseType is assigned in _onLoadPage when the submit buttons and links are configured - there is currently no way to configure how the postback is handled since the button and link configuration is automated
            //functionality was based on current needs.
            //--Buttons postback to save form data and expect JSON response of status/errors
            //--Links postback to change page layout and expect HTML response of updated page (for example links in grid header trigger that sort a grid on the server)
            var responseType = args["responseType"] ? args["responseType"] : "json";

            //get all data elements
            data = $getContainerData(containerID, true, true, true);

            var args = new Pathfinder.UI.FormSubmittingEventArgs(containerID, targetCtrl, url, data);
            var h = this.get_events().getHandler(containerID + "_formsubmitting");
            if (h) h(this, args);

            if (!args.get_cancel())
            {
                var title = $("#" + containerID).attr("__title");
                if (!title) title = "Form Submit";

                if (!validate || $validateFormData(containerID, data, title))
                {
                    //send event target so page knows how to handle - such as a submit button inside FormView with CommandName = "Insert" or "Update"
                    if (targetCtrl)
                    {
                        if (targetCtrl.name)
                            data["__EVENTTARGET"] = targetCtrl.name;
                        else
                        {
                            while (targetCtrl && !targetCtrl._name)
                                targetCtrl = targetCtrl.parentNode;

                            data["__EVENTTARGET"] = targetCtrl._name;
                        }
                    }
                    //                    else
                    //                        $("#" + containerID + " input[type=submit]").each(function() { data["__EVENTTARGET"] = this.name; });
                    data["__EVENTARGUMENT"] = ""; //may need to set in future - for now send empty

                    //it is required to send viewstate
                    var s2 = $("#" + containerID);
                    s2.find("#__VIEWSTATE").each(function() { data[$simplifyName(this.name, true)] = encodeURIComponent(this.value); });
                    s2.find("#__EVENTVALIDATION").each(function() { data[$simplifyName(this.name, true)] = encodeURIComponent(this.value); });
                    s2.find("#__VIEWSTATEENCRYPTED").each(function() { data[$simplifyName(this.name, true)] = encodeURIComponent(this.value); });

                    data["__RESPONSETYPE"] = responseType;

                    //flatten data - necessary for arrays and dataParam objects
                    data = $getDataForPostback(data, true);

                    //disable at last minute to avoid losing click event handler and default submit happening if user clicks wildly at button
                    if (targetCtrl && targetCtrl.type == "submit")
                    {
                        var rect = Sys.UI.DomElement.getBounds(targetCtrl);
                        var ovr = document.createElement("IMG");
                        $(ovr).attr("src", "content/images/spacer.gif").css({ "cursor": "wait", "position": "absolute", "left": rect.x, "width": rect.width, "top": rect.y, "height": rect.height, "zIndex": 10000 });
                        $("#" + containerID).append(ovr);
                        //$(targetCtrl).attr("disabled", true);
                    }

                    var cm = this;
                    //send request to server
                    if (responseType == "json")
                    {
                        $.post(
                            url
                            , data
                            , function(result, status)
                            {
                                var h = cm.get_events().getHandler(containerID + "_formsubmitted");
                                if (h) h(cm, { "containerID": containerID, "result": result, "target": targetCtrl, get_target: function() { return this.target; }, get_result: function() { return this.result; }, get_containerID: function() { return this.containerID; } });
                            }
                            , "json"
                            );
                    }
                    else //resonse will be html - forcing postback for this request  
                    {
                        this.unloadPage(containerID, true);
                        $("#" + containerID).loadEx(url + " form>*", data, this._onLoadPageDelegate, null, null, true);
                    }

                }
            }
        }
        catch (ex)
        {
            $alert(ex.description);
        }

        return false; //always returning false to cancel out default submit behavior
    },

    exportView: function(type, fromModal, module, channel)
    {
        if (module == "")
            module = this.get_Module();
        if (channel == "")
            channel = this.get_EffectiveChannel();
        $exportModule(type, fromModal, this.get_Application(), channel, module, this.getAllSelectionData());
    },

    resetPlans: function()
    {
        ///<summary>Resets the Plan Info grid so plans for the currently selected region are shown.</summary>

        var grid = this.get_PlanInfoGrid();
        if (!grid) throw new Error("ClientManager.filterPlans: PlanInfoGrid has not been set.  Use set_PlanInfoGrid to link the ClientManager to the control.");

        this._lastSearchValue = {};

        var mt = grid.get_masterTableView();

        //tracks how many user filters were set (this excludes Section_ID and Plan_State (from map))
        var fcount = $clearGridFilterSelections(mt);

        //need to reset filters
        var filters = mt.get_filterExpressions();
        var filter = filters.find("Plan_State");
        var regionFilter = null;

        if (filter)
        {
            regionFilter = filter.get_fieldValue();
        }

        filters.clear();

        if (this.get_Region() != null && (fcount > 0 || this.get_Region() != regionFilter))
        {
            this.filterPlans("Plan_State", this.get_Region());
        }
        else if (fcount > 0)
        {
            this.filterPlans();
        }
    },

    filterPlans: function(name, value, filterType, dataType)
    {
        ///<summary>Filters the Plan Information Grid based on the specified filter values.  Rebind is called during this method call and therefore is intended to only be used when a single filter has to be applied at a time.  This is intended to support real-time filter as new selections are made on the dashboard.  If multiple filters are required simultaneously use setGridFilter method and manually call rebind on the grid control.</summary>
        ///<param name="name" type="String">Name of the filter.</param>
        ///<param name="value" type="Variant">Value of the filter.  If null an existing filter with the same name is removed.</param>
        ///<param name="filterType" type="String">Type of operation such as 'EqualTo', 'Between', or 'StartsWith' (see Telerik documentation for complete list).  If no value is specified then EqualTo is assumed.</param>
        ///<param name="dataType" type="String">Data type that describes the value of the filter.  Parameter values include 'System.String', 'System.Int32', and 'System.DataTime' (see Telerik documentation for complete list).  If no value is specified System.String is assumed.</param>
        ///<returns type="void" />

        //        if (!name && value != null) throw new Error("ClientManager.filterPlans: name parameter must be specified if passing a filter value.");

        var grid = this.get_PlanInfoGrid();
        if (!grid) throw new Error("ClientManager.filterPlans: PlanInfoGrid has not been set.  Use set_PlanInfoGrid to link the ClientManager to the control.");

        //        if (!filterType) filterType = "EqualTo";
        //        if (!dataType) dataType = "System.String";

        if (grid)
        {
            var mt = grid.get_masterTableView();


            //$setGridFilter(grid, "Section_ID", (this.get_Channel() != 0 ? this.get_Channel() : null), "EqualTo", "System.Int32");

            //sl 4/5/2012:  to avoid filter error if 'All' selected (Plan Type: Section_Name)
            var _channel = this.get_Channel();
            if ((_channel.length == 1 && $.inArray(0, _channel) > -1) || _channel == null)
            {
                if (name && name == "Section_ID")
                {
                    $clearGridFilter(mt, "Plan_Type_ID");
                    if (value != null && value != "")
                        $setGridFilter(grid, name, value, filterType, dataType);
                    //This filter is not needed because Channel is always 0 in this If Block
                    //else
                    //    $setGridFilter(grid, "Section_ID", (this.get_Channel() != 0 ? this.get_Channel() : null), "EqualTo", "System.Int32");
                }
                else if (name && name == "Plan_Type_ID")
                {
                    $clearGridFilter(mt, "Section_ID");
                    if (value != null && value != "")
                        $setGridFilter(grid, name, value, filterType, dataType);
                    //This filter is not needed because Channel is always 0 in this If Block
                    //else
                    //    $setGridFilter(grid, "Section_ID", (this.get_Channel() != 0 ? this.get_Channel() : null), "EqualTo", "System.Int32");
                }

                else
                {
                    if (value != null && value != "")
                        $setGridFilter(grid, name, value, filterType, dataType);
                    else if (name)
                        $clearGridFilter(mt, name);
                }
            }
            else
            {
                $setGridFilter(grid, "Section_ID", _channel.join(","), "Custom", "System.Int32");

                if (value != null && value != "")
                    $setGridFilter(grid, name, value, filterType, dataType);
                else if (name)
                    $clearGridFilter(mt, name);
            }
            mt.clearSelectedItems();
            mt.rebind();
        }
    },

    getChannelProp: function(application, channel, propName, defaultValue)
    {
        var prop = defaultValue;
        var items = this.get_ChannelMenuOptions()[application];
        if (items)
        {
            //            var item = items[channel];
            //            if (item)
            //            {
            //                prop = item[propName];
            //                if (!prop) prop = defaultValue;
            //            }

            for (var o in items)
            {
                if (items[o]["ID"] == channel)
                {
                    var item = items[o];
                    if (item)
                    {
                        prop = item[propName];
                        if (!prop) prop = defaultValue;
                    }
                }
            }
        }

        return prop;
    },

    getModuleProp: function(application, module, propName, defaultValue)
    {
        var prop = defaultValue;
        var items = this.getModuleOptionsByApp(application);

        if (items)
        {
            var item = $.grep(items, function(i, x) { return i.ID == module; });
            if (item && item.length)
            {
                prop = item[0][propName];
                if (!prop) prop = defaultValue;
            }
        }

        return prop;
    },

    isCustomView: function()
    {
        return this.getChannelProp(this.get_Application(), this.get_EffectiveChannel(), "Custom", false)
                || this.getModuleProp(this.get_Application(), this.get_Module(), "Custom", false);
    },

    getUrl: function(pageName)
    {
        ///<summary>Constructs a url for the data section of the dashboard in the format ~/&lt;ApplicationName&gt;/&lt;SectionName&gt;/&lt;Module&gt;/section2.aspx.  If no data item is currently selected the url is based on the Application name only.</summary>
        ///<returns type="String" />

        var custom = this.isCustomView();

        return this.get_ApplicationManager().getUrl(this.get_ChannelUrlName(), this.get_Module(), pageName, this.get_SelectionData() != null, custom);
    },

    //    getDataForPostback: function(data)
    //    {
    //        ///<summary>Returns a query string for GET/POST requests based on the data collection.</summary>
    //        ///<returns type="String" />

    //        return $getDataForPostback(data);
    //    },

    getUIStateForPostback: function()
    {
        ///<summary>Returns a query string for GET/POST requests based on the CurrentUIState data collection.</summary>
        ///<returns type="String" />

        return $getDataForPostback(this.get_CurrentUIState());
    },

    getSelectionDataForPostback: function()
    {
        ///<summary>Constructs a query string based on the current selected data item for use in a query string or form data of a postback.  If no data item is currently selected an empty string is returned.</summary>    
        ///<returns type="String" />

        return $getDataForPostback(this.get_SelectionData());
    },

    _setDataFilters: function(grid, additionData, level)
    {
        var data = this.get_SelectionData(level);

        if (data || additionData)
        {
            var filter;
            var mt = grid.get_masterTableView();
            if (data)
            {
                for (var s in data)
                {
                    if (s != "__options")
                    {
                        $clearGridFilter(mt, s);
                        if (data[s] != null)
                        {
                            //if necessary rebuild dataParam object (may have been deserialized without methods
                            if (data[s].value) data[s] = new Pathfinder.UI.dataParam(s, data[s].value, data[s].dataType, data[s].filterType, data[s].src, data[s].isExtension);

                            if (data[s].get_value == null)
                                filter = $createFilter(s, data[s]);
                            else
                                filter = $createFilter(s, data[s].get_value(), data[s].get_filterType(), data[s].get_dataType(), data[s].get_isExtension());

                            mt.get_filterExpressions().add(filter);
                        }
                    }
                }
            }
            if (additionData)
            {
                for (var s2 in additionData)
                {
                    filter = $createFilter(s2, additionData[s2]);
                    $clearGridFilter(mt, filter.get_columnUniqueName());
                    mt.get_filterExpressions().add(filter);
                }
            }

            mt.rebind();
        }
    },

    //NOT NEEDED jQ 1.4.2 - SPH 4/5/2010 - if we want it back in the future make sure to put back in proper places - unless code is removed the calls are commented out throughout this file.
    //    ajaxRequestStarting: function()
    //    {
    //        ///<summary>Helper function that is called prior to initiating any action that makes an AJAX request that does not provide an error condition when the user's session has timed out.  If ajaxRequestComplete is not called within a specified interval the user's session is assumed to have expired and they are automatically signed out of the application.</summary>

    ////        if (this._pageRequestTimeoutHandle == 0)
    ////            this._pageRequestTimeoutHandle = window.setInterval(sessionCheck, this.get_AjaxRequestTimeout());
    //    },

    //    ajaxRequestComplete: function()
    //    {
    //        ///<summary>Helper function that is called when an AJAX request completes.  This will notify the application that the user's session on the server has not timed out.</summary>

    ////        if (this._pageRequestTimeoutHandle != 0)
    ////            window.clearInterval(this._pageRequestTimeoutHandle);

    ////        this._pageRequestTimeoutHandle = 0;
    //    },

    validateCurrentUser: function()
    {
        //        this.ajaxRequestComplete();

        $(".RadWindow iframe").each(function() { if (this.radWindow) this.radWindow.close(); });

        $openWindow("content/reauthenticate.aspx", null, null, 760, 330, "login");
    },

    addToFavorites: function(description)
    {
        //        var data = "Name=" + description; // +"&Data=" + encodeURIComponent(this.get_CurrentUIState());
        if (this._onFavoriteSavedDelegate == null)
            this._onFavoriteSavedDelegate = Function.createDelegate(this, this._onFavoriteSaved);

        $.post("services/pathfinderservice.svc/AddFavorite", { Name: description, Data: this.get_CurrentUIStateAsText() }, this._onFavoriteSavedDelegate);
    },

    _onFavoriteSaved: function(o1, o2, o3)
    {
        var h = this.get_events().getHandler("favoritesaved");
        if (h) h(this, new Pathfinder.UI.CommandEventArgs("savefavorite", true));
    },

    mapReloadData: function()
    {
        if (this._regionCanFilterPlans())
        {
            //            this._pendingRegionZoom = this.get_Region() != null;
            if (flashSupported)
                fmThemeReloadAreas("areas/mapdata.ashx?s=" + this.get_CurrentMapUIStateAsText());
            else
                $("#divTile1").load("map.aspx?cmd=restoreview&s=" + clientManager.get_CurrentMapUIStateAsText() + " form>*");
        }
        //        else
        //        {
        // SPH 8/28/2009 - Probably no longer need to load default areas file since the map will always be hidden for channels that don't support drill down.
        //            fmThemeReloadAreas("areas/areas.xml");
        //        }
    },

    _doMapCommand: function(commandName, requiresMapReset)
    {
        ///<summary>Helper function that is called to update the map based on the current ui state.  The value returned by the CurrentUIState property is what is passed to the map control as the CommandArgument.</summary>
        ///<param name="commandName" type="String">Value that is passed to the map control as the CommandName.</param>
        ///<returns type="void" />

        if (!this.get_MapIsReady()) return;

        var reloadMap = false;
        var cmdName = commandName.toLowerCase();

        var args = new Pathfinder.UI.CommandEventArgs(cmdName);
        var h = this.get_events().getHandler("mapupdating");
        if (h) h(this, args);

        if (!args.get_cancel())
        {
            switch (cmdName)
            {
                case "channelchanged":
                case "drugchanged":
                case "marketbasketchanged":

                    this._pendingMapReload = true;

                    if (requiresMapReset)
                        this.centerMapToUserGeography();
                    else
                        this._internalMapIsReady();

                    break;

                case "restoreview":

                    if (flashSupported)
                    {
                        this._pendingRegionZoom = this.get_Region() != null;

                        if (this._mapReady)
                        {
                            //11/09/2009 - SPH - Commenting out setting _pendingMapReload flag because it is causing map xml to load twice when restoreView is called in TA (ex: hitting back button)
                            //                              Code added to TA's application "activate" method also loads map data - this code was added as a result of showing map in Geographic Coverage report that was added late to Pathfinder
                            //                              Change was tested with Favs as well and appears to be working since it also calls restoreView which calls TA's "activate" to load correct data.
                            //                              Other applications that rely on map may also have issues if they don't load map - Standard Reports Geographic Coverage handles map loading on report page.
                            //this._pendingMapReload = true;
                            //

                            if (this._zoomedIn)
                            {
                                this.centerMapToUserGeography();
                            }
                            else
                            {
                                this._internalMapIsReady();
                            }
                        }
                        else
                            return; //don't do anything until map loads if user has region
                    }
                case "regionchanged": //filter grid, and reset selections

                    if (this.get_Application() == Pathfinder.UI.Applications.TodaysAccounts)
                    {
                        var grid = this.get_PlanInfoGrid();
                        var mt = grid.get_masterTableView();

                        $setGridFilter(grid, "Plan_State", this.get_Region());
                        $clearGridFilter(grid, "VISN");

                        mt.clearSelectedItems();

                        mt.rebind();

                        //Fall through allowed so checking commandName again - reset Section 2 to blank or appropriate data section - Region changes require history logging while Restore View does not
                        if (commandName.toLowerCase() == "regionchanged")
                        {
                            this.clearSelectionData(true);

                            this._updateHistory();
                        }
                    }
                    //                else if (this.get_Application() == Pathfinder.UI.Applications.StandardReports)
                    //                {
                    //                    alert("Implement for Standard Reports");
                    //                }
                    break;
            }

            if (!flashSupported && this.get_UIReady()) //no flash
            {
                this.mapReloadData();
            }
        }
    },

    externalMapIsReady: function()
    {
        this._mapIsReady(true);


        var h = this.get_events().getHandler("mapisready");
        if (h) h(this, new Sys.EventArgs());
    },

    _internalMapIsReady: function()
    {
        this._skipMapEvent = false;
        this._mapIsReady();
    },

    _mapIsReady: function(external)
    {
        if (!flashSupported) return;
        //need to skip some events such as when navigating to Region (multi-states) which can cause zoom out before zoom in
        if (this._skipMapEvent && this._pendingEventName == "ZoomOut")
        {
            this._skipMapEvent = false;
            return;
        }

        //run map updates when ready and either internal update request or from a pending event (pending event flag is used to for non-msie browsers because they call this function too many times)        
        if (this._mapReady && (!external || this._pendingEvent))
        {
            fmHideCrossHair();

            if (this._pendingMapReload)
            {
                this._pendingMapReload = false;
                this.mapReloadData();
            }
            else if (this._pendingRegionZoom)
            {
                this._pendingRegionZoom = false;
                fmAreaCenter(["us_" + this.get_Region()]);
            }
            else
            {
                this.set_Region(this._pendingRegion);
                this._pendingRegion = null;

                if (this._pendingGeoUpdate)
                {
                    this._pendingGeoUpdate = false;
                    this.mapReloadData();
                }
            }

            this._pendingEvent = false;
        }

        if (this._mapReady == false)
        {
            this.centerMapToUserGeography();
        }
        this._mapReady = true;

        fmPOIsHideCategory("msa");

        if (this.get_Channel() == 11)
        {
            fmPOIsShowCategory("visn");
            //            fmMapModeSelect();
        }
        else
        {
            fmPOIsHideCategory("visn");
            //            fmMapModeZoom();
        }

        //reset territory/region list menu - if zoomed out or current region is null or single state
        //        if (!this._zoomedIn)
        //            $refreshMenuOptions(this.get_RegionList(), 0);

        if (this._zoomedIn && this.get_Geography() && !$.isArray(this.get_Region()))
            $("#btnResetGeog").show();
        else
            $("#btnResetGeog").hide();
    },

    externalMapAreaChanged: function(eventName, id, url, label)
    {
        this._pendingEventName = eventName;

        if (this._skipMapEvent && eventName == "ZoomOut") return; //sometimes map zooms out and then in so we need to skip the zoom out

        this._pendingEvent = true; //non-msie browsers have extra call to mapready

        this._pendingRegion = id && id != "null" ? $.trim(id.substr(3)).toUpperCase() : this._pendingRegion;
        this._zoomedIn = eventName == "ZoomIn" || eventName == "Move"; //assuming "Move" event is only fired when already zoomed in.

        //selected geography no longer valid - user hit back
        if (!this._zoomedIn && this.get_Geography())
        {
            this._geography = null;
            this._pendingGeoUpdate = true;
        }

        //        //have to manually call on select event
        //        if (eventName == "select")
        //        {          
        //            this._mapIsReady();
        //        }
    },

    resetGeography: function()
    {
        var value = this.get_Geography();
        //            this.set_Region(this._geography.regions.split(","));
        if (value)
        {
            this._pendingRegion = value.Regions.split(",");
            this._skipMapEvent = this.get_Region() != null; //need to skip if already zoomed otherwise map zooms out and then in which causes sync issues

            this._pendingGeoUpdate = false;

            this.centerMapToUserGeography();
        }
    },

    centerMapToUserGeography: function()
    {
        //Test if user geography is not default (only need to check one property for 0)
        var geog = this.get_EffectiveGeography();

        if (geog.CenterX != 0)
        {
            var a = geog.Area;

            var s = 100;
            if (a < 3)
                s = 1200;
            else if (a < 6)
                s = 1000;
            else if (a < 10)
                s = 800;
            else if (a < 18)
                s = 700;
            else if (a < 24)
                s = 500;
            else if (a < 35)
                s = 450;
            else if (a < 60)
                s = 400;
            else if (a < 75)
                s = 350;
            else if (a < 90)
                s = 300;
            else if (a < 180)
                s = 250;
            else if (a < 360)
                s = 200;
            else if (a < 600)
                s = 150;
            else if (a < 700)
                s = 125;

            fmAreaCenterLatLon("", geog.CenterY, geog.CenterX, s);
        }
        else if (geog.RegionID) //single region
        {
            fmAreaCenter(["us_" + geog.RegionID]);
        }
        else if (this._pendingRegionZoom)  //put here because during init the zoom is not handled because map isn't ready - this function is first that can actually handle zoom on restoreView call from cm initialize
        {
            fmAreaCenter(["us_" + this.get_Region()]);
        }
        else
        {
            fmInitialView();
        }
        this._pendingRegionZoom = false;
    },

    openViewer: function(url, x, y, width, height, activeRegion)
    {
        var windowHeight = $(window);

        if (width == null) width = (windowHeight.width() / 1.05) - (x ? x : 0);
        if (height == null) height = (windowHeight.height() / 1.15) - (y ? y : 0);

        if (!x) x = (windowHeight.width() / 2) - (width / 2);
        if (!y) y = (windowHeight.height() / 2) - (height / 2);

        if (windowHeight.height() < y + height)
            y -= height;

        $("#infoPopup").show().css("z-index", 10000).css("left", x).css("top", y).css("width", width).css("height", height).attr("activeRegion", activeRegion);
        $("#infoPopup .tileContainerHeader .title").text("...");
        this.loadPage(url, "infoPopup");
        $("#infoPopup .ajaxLoader").show();
    },

    openToolTipViewer: function(url, queryFieldName, id, x, y, width, height, title, activeRegion)
    {
        if (queryFieldName == null) queryFieldName = "id";

        //check if only page name was sent - if so then construct full url
        if (url.indexOf("/") == -1)
            url = this.getUrl(url);

        url = url + "?" + queryFieldName + "=" + id + (title ? "&title=" + encodeURIComponent(title) : "");
        this.openViewer(url, x, y, width, height, activeRegion);
    },

    openToolTipViewerWithQueryString: function(url, x, y, width, height, title, activeRegion)
    {
        //check if only page name was sent - if so then construct full url
        if (url.indexOf("/") == -1)
            url = this.getUrl(url);

        url = url + (title ? "&title=" + encodeURIComponent(title) : "");
        this.openViewer(url, x, y, width, height, activeRegion);
    },

    openItemViewer: function(pageName, queryFieldName, id, x, y, width, height)
    {
        if (queryFieldName == null) queryFieldName = "id";
        var url = this.getUrl(pageName) + "?" + queryFieldName + "=" + id;
        $openWindow(url, x, y, width, height);
    },

    openPieChartViewer: function(width, height, x, y)
    {
        var app = clientManager.get_ApplicationManager();
        var url = app.getUrl("all", clientManager.get_Module(), "OpenPieChart.aspx");
        var q;
        var geography = this.get_Region();

        q = "Section_ID=" + this.get_Channel().toString();

        if (geography != null)
            q += "&Plan_State=" + geography;

        if (clientManager.getContextValue("restrictByTerritory") && clientManager.getContextValue("restrictByTerritory") != null)
            q = q + "&ByTerritory=true";

        url = url + "?" + q;

        var oManager = GetRadWindowManager();
        var oWnd = radopen(url, "PieChart");
        oWnd.setSize(width, height);
        oWnd.Center();
    },

    showFavoritesList: function()
    {
        $("#favoritesListWindow").loadEx("usercontent/favoriteslist.aspx form>*"
                , null
                , function()
                {
                    $(this).css("visibility", "hidden").css("display", "block");
                    var rect = Sys.UI.DomElement.getBounds($get("ctl00_main_subheader1_linkViewFavorites"));
                    var rect2 = Sys.UI.DomElement.getBounds($get("favoritesListWindow"));
                    $(this).css("top", rect.y + rect.height).css("left", ((rect.x + rect.width) - rect2.width)).css("display", "none").css("visibility", "visible").slideDown("fast");
                }

        );
    },

    selectFavorite: function(id, data)
    {
        var cm = this;
        $("#favoritesListWindow").slideUp("fast", function()
        {
            cm._trackingEnabled = false;
            cm.set_CurrentUIState(data, true);
            cm._trackingEnabled = true;
            cm.restoreView();
            cm._updateHistory();
            setPageUIState(cm);
        }
        );
    },

    getContextValue: function(key)
    {
        return this._contextData[key];
    },

    setContextValue: function(key, data)
    {
        if (key)
        {
            if (data) // && typeof data != "object")
                this._contextData[key] = data;
            else //if (!data)
                this._contextData[key] = null;
            //            else
            //                throw new Error("Objects are not allowed in context cache.");
        }
    },

    clearContext: function()
    {
        this._contextData = {};
    },

    registerComponent: function(controlid, data, isUpdateable, containerID, level)
    {
        ///<summary>Tracks components loaded when jQuery load method is used to dynamically update page content.  The purpose of tracking components is so they can be properly disposed of if the dynamically loaded content is updated multiple times.  Upon request to reload a section any associated components are first disposed of as if the entire page was being reloaded.</summary>
        ///<param name="controlid" type="String">Id of the element that host's the AJAX.Net component.</param>
        ///<param name="data" type="Object">Optional preset data associated with the component that is used to set default filters.</param>
        ///<returns type="void" />

        //        if (data == null) data = {};
        if (level == null) level = 0;

        if (!containerID) containerID = "section2";

        if (this._controls[containerID] == null)
            this._controls[containerID] = [];

        var controls = this._controls[containerID];
        controls[controls.length] = { "id": controlid, "data": data, "level": level, "updateable": isUpdateable ? true : false };
    },

    isFilteredModule: function(containerID, level)
    {
        ///<summary>Checks if the current ui state contains filtered data, meaning there is an "updateable" control registered to the specified container and level.</summary>
        ///<param name="containerID" type="String">ID of the element that is the host for a partial page and is being queried to determine if it should be updated or reloaded.</param>
        ///<param name="level" type="Int32">Level of the control in a "drill-down" sequence.  By default all controls are level zero unless otherwise specified.  If a control depends on a selection of another control for loading it will typically have a higher level assigned to it.</param>

        var module = this.get_Module();

        if (!containerID) containerID = "section2";

        if (level == null) level = 0;

        var controls = this._controls[containerID];

        if (controls)
        {
            //if not module check controls
            for (var i = 0; i < controls.length; i++)
            {
                if (controls[i].updateable && controls[i].level == level)
                    return true;
            }
        }
        return false;
    },

    setPlanInfoGridTimeout: function(sender, args)
    {
        ///<summary>Event handler for setting a timeout to delay searching for items in the Plan Information Grid.  The purpose of the delay is to avoid excessive requests based on rapid user input such as key presses in a text box.  By delaying the request it is possible for the user to completely type their search phrase before the search starts while still giving the appearance that results are returned as their typed text changes.  This event handler should only be attached to input controls that filter the Plan Information Grid and require delayed filtering functionality.</summary>
        if (this._planInfoGridTimeoutHandle)
            this._planInfoGridTimeoutHandle.cancel();

        this._planInfoGridTimeoutHandle = new cmd(this, "_planInfoGridTimeout", [sender, args], this._planInfoGridFilterTimeout);
        this._planInfoGridTimeoutSource = sender;
    },

    _planInfoGridTimeout: function(sender, args)
    {
        ///<summary>Internal callback for handling delayed searches (see setPlanInfoGridTimeout).  This method should not be called directly.</summary>
        this._planInfoGridTimeoutHandle = null; //0;

        if (sender)
        {
            //don't filter for same thing twice.
            if (sender.value != this._lastSearchValue[args.dataField])
                this.filterPlans(args.dataField, sender.value, args.filterType);

            this._lastSearchValue[args.dataField] = sender.value;
        }
    },

    //Menu event handlers for controls
    _onApplicationSelectionChanged: function(sender, args)
    {
        ///<summary>Event handler for Application menu.  Do not call directly.  Use set_Application to update the current application.</summary>
        if (processing > 0) return;
        var value = args.get_item().get_value();
        if (value)
        {
            ////////////            this._application = value;
            ////////////            var date = new Date();
            ////////////            date.setFullYear(date.getFullYear() + 50);
            ////////////            document.cookie = "s=" + escape(this.get_CurrentUIStateAsText()) + "; expires=" + date.toUTCString();
            ////////////            window.location = window.location;
            this.set_Application(value);
            $refreshMenuOptions(sender, value);
        }
    },

    _onChannelSelectionChanged: function(sender, args)
    {
        ///<summary>Event handler for Section menu.  Do not call directly.  Use set_Channel to update the current section.</summary>

        //var value = args.get_item().get_value();
        var channel_ctrl = clientManager.get_ChannelMenuCheckBoxList();
        var value = channel_ctrl.get_values();

        if (value !== null && value !== "")
        {
            this.set_Channel(value);
            //$refreshMenuOptions(sender, value);
        }
        else
            this.set_Channel(0);
    },

    //    //Drug/Market event handlers for controls
    //    _onDrugListChanged: function(sender, args)
    //    {
    //        ///<summary>Event handler for Drug menu.  Do not call directly.  Use set_Drug to update the current drug.</summary>
    //        var value = args.get_item().get_value();
    //        if (value)
    //        {
    //            this.set_Drug(value);
    //            $refreshMenuOptions(sender, value);
    //        }
    //    },

    //    _onMarketBasketListChanged: function(sender, args)
    //    {
    //        ///<summary>Event handler for Market Basket menu.  Do not call directly.  Use set_MarketBasket to update the current market basket.</summary>

    //        var value = args.get_item().get_value();
    //        if (value)
    //        {
    //            this.set_MarketBasket(value);
    //            $refreshMenuOptions(sender, value);
    //        }
    //    },

    //    _onRegionListChanged: function(sender, args)
    //    {
    //        ///<summary>Event handler for Region menu.  Do not call directly.  Use set_Region to update the current region.</summary>

    //        var value = args.get_item().get_value();

    //        if (value == null) return;

    //        var geo = this.get_RegionGeographyListOptions()[value];
    //        if (geo)
    //            this.set_Geography(geo);
    //        else
    //            this.set_Geography(null);

    //        $refreshMenuOptions(sender, value);
    //    },

    _onModuleMenuItemClicked: function(sender, args)
    {
        ///<summary>Event handler for default module menu.  Do not call directly.  Use set_Module to update the current module.</summary>

        new cmd(this, "set_Module", [args.item.value], 10);
    },

    //internal event triggers
    _onApplicationChanged: function()
    {
        ///<summary>Internal method for triggering UI updates when the current application changes.</summary>

        //make sure menu shows current value
        $refreshMenuOptions(this.get_ApplicationMenu(), this.get_Application());

        if (this.get_UIReady())
            this._module = null;

        this.unloadPage("section2");

        if (!this._applicationManager || this._applicationManager.get_ApplicationID() != this.get_Application())
            this._applicationManager = Pathfinder.UI.Application.getInstance(this, this.get_Application(), true);

        //reset Section 2 to blank
        if (this._trackingEnabled)
            this._setSelectionData(this._applicationManager.getDefaultData(this), 0, false);

        this._updateHistory();

        var h = this.get_events().getHandler("applicationchanged");
        if (h) h(this, new Sys.EventArgs());

        this._onUIStateChanged();

        //fix menu sizing
        $refreshMenuOptions(this.get_ChannelMenu(), this.get_Channel()); //<--fixes menu width after style changes from app switching
    },

    _onChannelChanged: function()
    {
        ///<summary>Internal method for triggering UI updates when the current section changes.</summary>

        //make sure menu shows current value
        //$refreshMenuOptions(this.get_ChannelMenu(), this.get_Channel());
        //Load Channel Menu
        $refreshChannelMenu(this.get_Channel());

        var module = this.get_ApplicationManager().configureModuleMenu(this, this.get_EffectiveChannel(), this.get_Module());
        var modChange = this._module != module;

        if (this.get_UIReady()) //sync module if Client Manager in ready state
            this._module = module;

        //SPH 1/7/2010 - if current application does not rely on channels then there is no point running this code - if channel changed it is probably to init application to 0 (all) if previous value was something else
        //Fixes issue with Covered Lives report footer component of grid causing an error because it adds an event handler to the window element's resize event by using a "timeout" call.  Because _onChannelChanged and _onSelectionChanged are both called the grid is quickly loaded and unloaded but the use of timeout causes the event handler to be attached after the grid is destroyed and therefore it can't be cleened up properly and continues to fire after the grid is gone.
        if (this.get_ApplicationManager().hasChannelMenuOptions(this))
        {
            //Based on current UI state if plans cannot be filtered by region then reset plan filter and set region to NULL.
            var requiresMapReset = false;
            if (!this._regionCanFilterPlans())
            {
                $setGridFilter(this.get_PlanInfoGrid(), "Plan_State", null);

                requiresMapReset = this._region != null;

                //                if (this.get_RegionList())
                //                    this.get_RegionList().set_visible(false);

                $("#btnResetGeog").hide();

                this._region = null;
                this._geography = null;
            }
            //            else if (this.get_RegionList())
            //                this.get_RegionList().set_visible(true);


            var data = this.get_ApplicationManager().getDefaultData(this);
            if (this.get_Channel()[0] != 107)
            {
                if (data)
                    delete (data["VISN"]);
            }

            //Reset Section 2 to blank - defer to grid databound event if Todays Accounts and Module was not reset automatically (module is not reset if it is still relevant)
            if (this._trackingEnabled)
            {
                if (this.get_Application() != Pathfinder.UI.Applications.TodaysAccounts || modChange)
                    this._setSelectionData(data, 0, false, modChange);
                else
                {                              //SPH 09/02/2009 - Fix issue caused by auto updates not allowing newly selected channel's layout to load                                            
                    this.unloadPage(); //calling unloadPage forces subsection to be destroyed which also clears the control collection that is checked for updateable controls.  Since none will be found for section2 the entire page will be reloaded.
                    //this will fix issues such as CP Affils having different columns as PBM affils - when channel is switched the grid will not simply be refreshed but instead entire section is reloaded
                    this.clearSelectionData(true); //assuming TA will reload section by auto selecting first row
                }

                this._doMapCommand("channelchanged", requiresMapReset);
            }
            //

            this._updateHistory();
        }

        var h = this.get_events().getHandler("channelchanged");
        if (h) h(this, new Sys.EventArgs());

        this._onUIStateChanged();
    },

    _onEffectiveChannelChanged: function()
    {
        ///<summary>Internal method for triggering UI updates when the current effective section changes.</summary>

        this._module = this.get_ApplicationManager().configureModuleMenu(this, this.get_EffectiveChannel(), this.get_Module());

        //SPH 4/07/2010 - Should be setting new selection when channel is changes to set flag instead of clearing section by setting null data
        //old --- reset Section 2 to blank -
        //old        this._setSelectionData(null, 0, false);
        this.clearSelectionData(true);
        this._section2Invalid = true;

        this._updateHistory();

        var h = this.get_events().getHandler("effectivechannelchanged");
        if (h) h(this, new Sys.EventArgs());

        this._onUIStateChanged();
    },

    _onModuleChanged: function()
    {
        ///<summary>Internal method for triggering UI updates when the current module changes.</summary>

        //reset Section 2 with new module using current data selection
        this._onSelectionChanged(false, false, 0);

        this._updateHistory();

        var h = this.get_events().getHandler("modulechanged");
        if (h) h(this, new Sys.EventArgs());

        this._onUIStateChanged();
    },

    _onDrugChanged: function()
    {
        ///<summary>Internal method for triggering UI updates when the current drug changes.</summary>

        //make sure menu/combobox to current value
        //        this.get_DrugList().findItemByValue(this.get_Drug()).select();
        //        $refreshMenuOptions(this.get_DrugList(), this.get_Drug());

        this._doMapCommand("drugchanged");

        this._updateHistory();

        var h = this.get_events().getHandler("drugchanged");
        if (h) h(this, new Sys.EventArgs());

        this._onUIStateChanged();

    },

    _onMarketBasketChanged: function()
    {
        ///<summary>Internal method for triggering UI updates when the current market basket changes.</summary>

        //make sure menu/combobox to current value
        //        this.get_MarketBasketList().findItemByValue(this.get_MarketBasket()).select();
        //        $refreshMenuOptions(this.get_MarketBasketList(), this.get_MarketBasket());

        //set drug id to first in list since it is default entry in dropdown - this will prevent drugchanged event from firing.
        this._drug = this._drugListOptions[this.get_MarketBasket()][0].ID;

        //update drug list
        //        $loadMenuItems(this.get_DrugList(), this._drugListOptions[this.get_MarketBasket()]);

        //update map
        this._doMapCommand("marketbasketchanged");

        this._updateHistory();

        var h = this.get_events().getHandler("marketbasketchanged");
        if (h) h(this, new Sys.EventArgs());

        this._onUIStateChanged();
    },

    _onGeographyChanged: function()
    {
        ///<summary>Internal method for triggering UI updates when the current geography changes.</summary>

        if (this._geography)
        {
            this._pendingRegion = this._geography.Regions.split(",");
            this._skipMapEvent = this.get_Region() != null; //need to skip if already zoomed otherwise map zooms out and then in which causes sync issues
        }

        this._pendingGeoUpdate = true;

        this.centerMapToUserGeography();

        var h = this.get_events().getHandler("geographychanged");
        if (h) h(this, new Sys.EventArgs());

        this._onUIStateChanged();
    },

    _regionCanFilterPlans: function()
    {
        return this.get_ApplicationManager().canFilterByRegion(this);
    },

    restoreMap: function()
    {
        ///<summary>Restores map so all regions are visible.  The restored map will be configured based on the value of CurrentUIState.</summary>

        this.set_Region(null);
    },

    _onRegionChanged: function()
    {
        ///<summary>Internal method for triggering UI updates when the current region changes.</summary>

        if (this._regionCanFilterPlans()) //sph 5/7 && this._trackingEnabled)
        {
            this._doMapCommand("regionchanged");
        }

        var h = this.get_events().getHandler("regionchanged");
        if (h) h(this, new Sys.EventArgs());

        this._onUIStateChanged();
    },

    _onSelectionChanged: function(keepCurrent, cancellable, level)
    {
        ///<summary>Called internally by SelectedData property when the user makes a new data selection which triggers a partial page update or filter update (see keepCurrent param).  If a page load is required the current page is first unloaded to cleanup components that are no longer used.</summary>
        ///<param name="keepCurrent" type="Boolean">Indicates whether the current page element needs to be refreshed or simply re-filtered as is the case with a page hosting Telerik grids using client side databinding.</param>
        ///<returns type="void" />

        if (level == null) level = 0;

        if (this.get_UIReady() && this._selectionData)
        {
            var count = this._selectionData.length - 1;
            for (var i = level; i < count; i++)
            {
                Array.removeAt(this._selectionData, level + 1);
            }
        }

        if (!keepCurrent)
        {
            //currently only support auto updates to interface when level is zero (default top level)
            if (level == 0)
            {
                //If Requires Refresh
                //unload previous page before loading new
                this._unloadPage();

                //load new page
                this._loadPage();
            }
        }
        else //Else if requires filter update
        {
            this._resetFilteredData(null, level);
        }

        //SPH 07/17/2009 - Decided to not track history when selection data changes - gets too complicated to restore. For example the plan grid's page index sort order would also have to be tracked and restored.
        //Only track history if cancellable - that means it was based on a row selection or other user interaction and not state restores from within ClientManager.
        //        if (cancellable)
        //            this._updateHistory();

        //only fire events if cancellable
        if (cancellable)
        {
            this._updateHistory();

            //only fire events if cancellable
            var h = this.get_events().getHandler("selectionchanged");
            if (h) h(this, { "level": level });

            this._onUIStateChanged(true);
        }

    },

    refreshSection2: function()
    {
        //unload previous page before loading new
        this._unloadPage();

        //load new page
        this._loadPage();
    },

    loadPage: function(url, containerID, hideWhileLoading)
    {
        ///<summary>Loads an html element with the specified page.</summary>
        ///<param name="url" type="String">URL of the page that will be loaded into the element.</param>
        ///<param name="containerID" type="String">ID of element that will host the page.</param>
        ///<param name="hideWhileLoading" type="Boolean">Optional parameter that indicates if element's content should be hidden while request is pending.</param>

        this._unloadPage(containerID);

        $("#" + containerID).loadEx(url + " form>*", null, this._onLoadPageDelegate, null, hideWhileLoading);
    },

    loadPageUsingUIState: function(url, containerID)
    {
        ///<summary>Loads an html element with the specified page using the CurrentUIState to construct the query string.</summary>
        ///<param name="url" type="String">URL of the page that will be loaded into the element.</param>
        ///<param name="containerID" type="String">ID of element that will host the page.</param>

        var controls = this._controls[containerID];
        if (controls && controls.length > 0)
            this._unloadPage(containerID);

        var q = this.getUIStateForPostback();

        $("#" + containerID).loadEx(url + " form>*", q, this._onLoadPageDelegate);
    },

    loadPageUsingSelectionData: function(url, containerID)
    {
        ///<summary>Loads an html element with the specified page using the current SelectionData to construct the query string.</summary>
        ///<param name="url" type="String">URL of the page that will be loaded into the element.</param>
        ///<param name="containerID" type="String">ID of element that will host the page.</param>

        var controls = this._controls[containerID];
        if (controls && controls.length > 0)
            this._unloadPage(containerID);

        var q = this.getSelectionDataForPostback();

        q += (q.length > 0 ? "&" : "") + "_userkey=" + this.get_UserKey();

        $("#" + containerID).loadEx(url + " form>*", q, this._onLoadPageDelegate);
    },

    _loadPage: function()
    {
        ///<summary>Called internally to load the data section of the dashboard based on the current selection state which includes Application, Section, Module, and Selected Data.</summary>
        ///<returns type="void" />

        var url = this.getUrl();
        this.loadPageUsingSelectionData(url, "section2");
    },

    unloadPage: function(containerID, retainHTML)
    {
        ///<summary>Unloads a partial page by disposing all registered controls and clearing the html of the container.</summary>
        ///<param name="containerID" type="String">ID of the element that is hosting the partial page to unload.</summary>

        this._unloadPage(containerID);
        if (!retainHTML)
            $("#" + containerID).html("");
    },

    _unloadPage: function(containerID)
    {
        ///<summary>Called internally to cleanup components that are no longer used by the page as a result of requesting an update to the data section of the dashboard.</summary>
        ///<returns type="void" />

        if (!containerID) containerID = "section2";

        if (containerID == "section2")
            JT_uninit();

        $("#" + containerID + " input[type=submit]").unbind("click");

        //unload controls
        var controls = this._controls[containerID];
        if (controls)
        {
            var c;
            for (var i = controls.length - 1; i >= 0; i--)
            {
                c = Sys.Application.findComponent(controls[i].id);
                if (c)
                {
                    //if component is grid then dispose its master table - if this is not done grids will not function properly when used with client side databinding.
                    if (c.get_masterTableViewHeader)
                        $disposeTelerikGrid(c);
                    else
                    {
                        //dispose component
                        $disposeControl(c);
                    }
                }
                else
                {
                    c = $get(controls[i].id);
                    if (c && c.tagName == "OBJECT")
                        window[controls[i].id] = null;
                }

                Array.removeAt(controls, i);
            }
        }

        var h = this.get_events().getHandler(containerID + "_pageunloaded");
        if (h) h(this, new Sys.EventArgs());

        //unload scripts after event otherwise it might night be handled
        //        $("script[_container=" + containerID + "]").remove();
    },

    _onLoadPage: function(html, responseStatus, response, url, containerID, state)
    {
        ///<summary>Callback function used when calling jQuery.load method.  This method should not be called directly.</summary>
        ///<param name="html" type="String">HTML that is returned based on the load request.</param>
        ///<param name="responseStatus" type="String">Simple response message that indicates either "success" or "error".</param>
        ///<param name="response" type="Object">Complete response object representing the last request's results.</param>
        ///<returns type="void" />

        if (!containerID)
            containerID = "section2";

        if (responseStatus == "success")
        {
            //setup any submit forms
            var submitForm = false;
            submitForm = $("#" + containerID + " input.postback[type=submit]").attr("_containerID", containerID).click(function(e) { return clientManager.submitForm(e.target, { responseType: "json" }); }).length > 0;
            submitForm = ($("#" + containerID + " .postback a[href^=javascript:__doPostBack], #" + containerID + " a.postback").each(function() { try { var n = this.href.substr(this.href.indexOf("'") + 1); n = n.substr(0, n.indexOf("'")); this._name = n; } catch (ex) { } }).attr("href", "javascript:void(0)").attr("_containerID", containerID).click(function(e) { return clientManager.submitForm(e.target, { responseType: "html" }); }).length > 0) || submitForm;

            $("#" + containerID).removeAttr("__title");
            if (submitForm)
            {
                try
                {
                    var r = /\<title\>/.exec(html);
                    if (isNaN(r)) r = r.index + 7;
                    var r2 = /\<\/title\>/.exec(html);
                    if (isNaN(r2)) r2 = r2.index;

                    $("#" + containerID).attr("__title", $.trim(html.substr(r, r2 - r)));
                }
                catch (ex)
                {
                }
            }
            //

            //SPH - 06/15/2010 - don't know if this was removed on purpose or not so adding back in for now
            //unload prev scripts if present - not done in unload because of speed - makes update look slightly slower
            $("script[_container=" + containerID + "]").remove();
            //

            var d = document.createElement("DIV");
            d.innerHTML = html;

            //responseStatus is supposed to equal "timeout" if response header __pinsologin is true (set in loadEx function) but custom hdrs are only supported in IIS 7.0 integrated pipeline mode which requires different dev environment
            if ($(d).find("#loginPage").length == 0)
            {
                var c = $(d).children("form").children("script").filter(":not([src])");
                var st;
                for (var i = 0; i < c.length; i++)
                {
                    try
                    {
                        st = document.createElement("SCRIPT");
                        st.setAttribute("_container", containerID);
                        st.text = c[i].text;
                        document.body.appendChild(st);
                        //                        eval(c[i].text);
                    } catch (ex) { }
                }
                d.innerHTML = "";
                if (d.parentNode) d.parentNode.removeChild(d);
                d = null;
                st = null;

                var h = this.get_events().getHandler(containerID + "_pageinitialized");
                safeEvent(this, h, new Sys.EventArgs(), "Unhandled error in " + containerID + "_pageinitialized event handler.");

                var l = this.get_SelectionCount();
                for (var i = 0; i < l; i++)
                {
                    if (this.isFilteredModule(containerID, i))
                    {
                        this._resetFilteredData(containerID, i);
                        break; //once a filtered section found we can stop looking
                    }
                }

                if (containerID == "section2")
                {
                    var am = this.get_ApplicationManager();
                    am.resizeSection();
                    am.configureSection();
                }

                h = this.get_events().getHandler(containerID + "_pageloaded");
                safeEvent(this, h, new Sys.EventArgs(), "Unhandled error in " + containerID + "_pageloaded event handler.");

                if (containerID == "section2")
                    JT_init();
            }
            else //timeout occurred but hdr not set (see above comment)
            {
                $("#" + containerID).html("");

                //window.top.location = "login.aspx";
                this.validateCurrentUser();
            }
        }
        else if (responseStatus == "timeout")
        {
            //window.top.location = "login.aspx";

            this.validateCurrentUser();
        }
        else  //if error then output appropriate text - if Page not found then just show 404 otherwise show more detailed message
        {
            if (response.status == 404)
                $("#" + containerID).html("<span style='color:#f00;'>Error: 404 [" + url + "]</span>");
            else
            {
                var div = document.createElement("DIV");

                div.innerHTML = response.responseText;

                $("#" + containerID).html("<span style='color:#f00;'>Error: " + $(div).children("span").children("H2").text() + "</span>");
            }
        }
    },

    _resetFilteredData: function(containerID, level)
    {
        ///<summary>Called internally to reset filters on Telerik Grid components registered using registerComponent.</summary>
        ///<returns type="void" />

        if (!containerID) containerID = "section2";
        if (level == null) level = 0;

        var controls = this._controls[containerID];

        if (controls)
        {
            for (var i = 0; i < controls.length; i++)
            {
                var c = Sys.Application.findComponent(controls[i].id);

                //Reset control filter IF it is a rad grid or control is a RadGridWrapper and autoupdate = true and drillDownLevel is >= specified level being updated
                if (c)
                {
                    if (c.get_masterTableView &&
                    ((!c.get_autoUpdate && c.ClientSettings.DataBinding && c.ClientSettings.DataBinding.Location)  //regular radgrid with client side databinding
                    || (c.get_autoUpdate && c.get_autoUpdate() && c.get_drillDownLevel() >= level))                   //radgridwrapper with autoupdate and level >= specified
                    )
                    {
                        c.get_masterTableView().get_filterExpressions().clear();
                        this._setDataFilters(c, controls[i].data, c.get_drillDownLevel ? c.get_drillDownLevel() : 0);
                    }
                    else if (c.get_autoUpdate && c.get_autoUpdate() && c.get_drillDownLevel() >= level) //all other auto update controls
                    {
                        if (Pathfinder.UI.ThinGrid.isInstanceOfType(c))
                        {
                            c.set_params(this.get_SelectionData(level));
                            c.dataBind();
                        }
                        else
                            throw new Error("Auto update behavior not defined for registered component.");
                    }
                }
            }
        }
    },

    togglePlanInfoGridFilters: function()
    {
        ///<summary>Hides and Shows the Plan Information Grid's filter row.  If the filter row is collapsed all filters are cleared and the grid is rebound using default filtering.</summary>

        if (this.get_PlanInfoGrid())
        {
            var mt = this.get_PlanInfoGrid().get_masterTableView();
            var filterRow = mt.get_tableFilterRow();

            var visible = filterRow.style.display != "" && filterRow.style.display != "none";

            //toggle filter row and then clear filter control selections
            if (!visible)
            {
                $(filterRow).show();

                //Hide all filters except for Account Name when multiple Channels are selected or when All is selected
                var channel = this.get_Channel();

                if (channel.length > 1 || $.inArray(0, channel) > -1)
                    $('#ctl00_main_planInfo_gridPlanInfo_GridHeader .RadComboBox').hide();
                else
                    $('#ctl00_main_planInfo_gridPlanInfo_GridHeader .RadComboBox').show();
            }
            else
                $(filterRow).hide();

            this.get_ApplicationManager().resize();
        }
    },
    togglePlanSectionSelectGridFilters: function()
    {
        ///<summary>Hides and Shows the Plan Information Grid's filter row.  If the filter row is collapsed all filters are cleared and the grid is rebound using default filtering.</summary>

        if (this.get_PlanSectionSelectGrid())
        {
            var mt = this.get_PlanSectionSelectGrid().get_masterTableView();
            var filterRow = mt.get_tableFilterRow();

            var visible = filterRow.style.display != "" && filterRow.style.display != "none";

            //toggle filter row and then clear filter control selections
            if (!visible)
                $(filterRow).show();
            else
                $(filterRow).hide();

            this.get_ApplicationManager().resize();
        }
    },
    _onContactSearch: function(e)
    {
        $openWindow("todaysaccounts/all/contactsearch.aspx") //?q=" + e.target.value);
    },

    _updateHistory: function()
    {
        if (this._trackingEnabled)
        {
            var state = this.get_CurrentUIStateAsText();

            //state is not the same as previous in history add it.
            if (state != this._history[this._currentHistoryIndex + 1])
            {
                this._currentHistoryIndex++;

                $get("_clientmanagerhistory").contentWindow.location = this.get_BasePath() + "/content/history.aspx#" + this._currentHistoryIndex; //.src = "content/history.aspx?h=" + (this._currentHistoryIndex);
                this._history[this._currentHistoryIndex] = state;
            }
        }
    },

    _onHistoryChange: function(e)
    {
        if (e.target.readyState == "complete" || e.type == "load")
        {
            var o = e.target.contentWindow.location.hash;
            if (o != null && o != "")
            {
                //o = o.substr(1).split("=");
                o = o.substr(1);
                o = parseInt(o, 10);
                if (this._currentHistoryIndex > -1 && o != this._currentHistoryIndex)
                {
                    var state = this._history[o];
                    this._currentHistoryIndex = o;
                    this._trackingEnabled = false;
                    this.set_CurrentUIState(state);
                    this.restoreView();
                    this._trackingEnabled = true;
                    //must now set ui state once enabled
                    setPageUIState(this);
                }
            }
        }
    },

    about: function()
    {
        about();
    },

    customerSupport: function()
    {
    },

    terms: function()
    {
        terms();
    },

    disclaimer: function()
    {
        disclaimer();
    },

    changePassword: function()
    {
        changePassword();
    },

    cleanSelectionData: function(data)
    {
        var newdata = {};

        for (var k in data)
        {
            if (data[k] && data[k].value)
            {
                if ($.isArray(data[k].value))
                    newdata[k] = data[k].value.join(',');
                else
                    newdata[k] = data[k].value;
            }
            else
                newdata[k] = data[k];
        }

        return newdata;
    }
};
Pathfinder.UI.ClientManager.registerClass('Pathfinder.UI.ClientManager', Sys.UI.Control);

function safeEvent(theThis, h, e, msg)
{
    if (h)
    {
        try
        {
            h(theThis, e);
        }
        catch (ex)
        {
            if (msg)
                msg = msg + "<br /><br />" + ex.description;
            else
                msg = ex.description;
            $alert(msg, "Error", null, null, Pathfinder.UI.AlertType.Error);
            processing = 0; //unlock;
            $(".viewSelect span:first").css({ "color": "#2d58a8" }, 500);
        }
    }
}

//Grid Wrapper ---------------------------------------------------------------------------------------------------------------------------------------------------------------------
Pathfinder.UI.GridWrapper = function(element)
{
    Pathfinder.UI.GridWrapper.initializeBase(this, [element]);

    this._staticData = null;
    this._pagerSelector = null;
    this._autoUpdate = true;
    this._requiresFilter = true;
    this._autoLoad = false;

    this._loadingText = "";
    this._noRowsText = null;
    this._showLoading = true;
    this._mergeRows = true;

    this._sortOrder = null;

    this._customPaging = false;

    this._drillDownLevel = 0;

    this._suspendUI = false;

    this._clientManager = null;

    this._refreshCount = true; //flag to determine if record count needs to be retreived - only getting on "Rebind".

    this._showNumberOfRecords = true;

    this._expand = null;

    this._inlineCountEnabled = false;

    this._onDataBindingDelegate = null;
    this._onDataBoundDelegate = null;
    this._onCommandDelegate = null;
    this._onRecordCountCallbackDelegate = null;

    this._utcDateColumns = null;

    this._listFormat = "csv"; //expecting csv, bull, or null/"" (break);

    this._containerID = null;
};
Pathfinder.UI.GridWrapper.prototype =
{
    initialize: function()
    {
        Pathfinder.UI.GridWrapper.callBaseMethod(this, 'initialize');

        this.get_element().control.GridWrapper = this;

        //register component so RadGrid doesn't have to be - wrapper will dispose radgrid
        if (this.get_clientManager()) this.get_clientManager().registerComponent(this.get_id(), this.get_staticData(), this.get_autoUpdate(), this.get_containerID(), this.get_drillDownLevel());

        this._onRecordCountCallbackDelegate = Function.createDelegate(this, this._onRecordCountCallback);

        var grid = this.get_grid();

        grid.add_command((this._onCommandDelegate = Function.createDelegate(this, this._onCommand)));
        grid.add_dataBinding((this._onDataBindingDelegate = Function.createDelegate(this, this._onDataBinding)));
        grid.add_dataBound((this._onDataBoundDelegate = Function.createDelegate(this, this._onDataBound)));


        $("#" + grid.get_element().id + " th:first").addClass("firstCol");
        $(this.get_masterTableView().get_tableFilterRow()).children("TD:first").addClass("firstCol");

        this._configureSorting();

        this._prepDynamicColumns();

        if (this.get_autoLoad())
            this._dataBind();
        //        else if (this.get_noRecordsText())
        //        {
        //            this._hdrFixUp();
        //            $(this.get_masterTableView().get_element().rows[mt.get_element().rows.length - 1].cells[0]).text(this.get_noRecordsText());
        //        }
    },

    dispose: function()
    {
        var control = this.get_element().control;
        if (control) control.GridWrapper = null;

        var grid = this.get_grid();
        if (grid)
        {
            try
            {
                grid.remove_command(this._onCommandDelegate);
            }
            catch (ex) { }

            try
            {
                grid.remove_dataBinding(this._onDataBindingDelegate);
            }
            catch (ex) { }

            try
            {
                grid.remove_dataBound(this._onDataBoundDelegate);
            }
            catch (ex) { }

            delete (this._onCommandDelegate);
            delete (this._onDataBindingDelegate);
            delete (this._onDataBoundDelegate);
            delete (this._onRecordCountCallbackDelegate);


            $disposeTelerikGrid(grid);
        }

        Pathfinder.UI.GridWrapper.callBaseMethod(this, 'dispose');
    },

    add_dataBinding: function(handler) { this.get_events().addHandler("databinding", handler); },
    remove_dataBinding: function(handler) { this.get_events().removeHandler("databinding", handler); },

    add_dataBound: function(handler) { this.get_events().addHandler("databound", handler); },
    remove_dataBound: function(handler) { this.get_events().removeHandler("databound", handler); },

    add_recordCountChanged: function(handler) { this.get_events().addHandler("recordcountchanged", handler); },
    remove_recordCountChanged: function(handler) { this.get_events().removeHandler("recordcountchanged", handler); },

    get_clientManager: function()
    {
        if (!this._clientManager)
            this._clientManager = window.top.clientManager;

        return this._clientManager;
    },

    get_inlineCountEnabled: function() { return this._inlineCountEnabled; },
    set_inlineCountEnabled: function(value) { this._inlineCountEnabled = value; },

    get_expand: function() { return this._expand; },
    set_expand: function(value) { this._expand = value; },

    get_listFormat: function() { return this._listFormat; },
    set_listFormat: function(value) { this._listFormat = value; },

    get_containerID: function() { return this._containerID; },
    set_containerID: function(value) { this._containerID = value; },

    get_grid: function()
    {
        var e = this.get_element();
        if (e) return e.control;

        return null;
    },

    get_masterTableView: function()
    {
        var grid = this.get_grid();
        if (grid) return grid.get_masterTableView();

        return null;
    },

    get_supportsDynamicColumns: function() { return false; },

    get_drillDownLevel: function() { return this._drillDownLevel; },
    set_drillDownLevel: function(value)
    {
        if (value == null) value = 0;
        this._drillDownLevel = value;
    },

    get_autoLoad: function() { return this._autoLoad; },
    set_autoLoad: function(value) { this._autoLoad = value; },

    get_autoUpdate: function()
    {
        return this._autoUpdate;
    },
    set_autoUpdate: function(value)
    {
        this._autoUpdate = value;
    },

    get_staticData: function()
    {
        return this._staticData;
    },
    set_staticData: function(value)
    {
        if (value)
            this._staticData = eval("[" + value + "]")[0];
        else
            this._staticData = null;
    },

    get_showNumberOfRecords: function() { return this._showNumberOfRecords; },
    set_showNumberOfRecords: function(value) { this._showNumberOfRecords = value; },

    get_pagerSelector: function()
    {
        return this._pagerSelector;
    },
    set_pagerSelector: function(value)
    {
        this._pagerSelector = value;
    },

    get_mergeRows: function()
    {
        return this._mergeRows;
    },
    set_mergeRows: function(value)
    {
        this._mergeRows = value;
    },

    get_sortOrder: function()
    {
        return this._sortOrder;
    },
    set_sortOrder: function(value)
    {
        this._sortOrder = value;
    },

    get_customPaging: function()
    {
        return this._customPaging === true;
    },
    set_customPaging: function(value)
    {
        this._customPaging = value;
    },

    get_requiresFilter: function()
    {
        return this._requiresFilter;
    },
    set_requiresFilter: function(value)
    {
        this._requiresFilter = value;
    },

    get_loadingText: function() { return this._loadingText; },
    set_loadingText: function(value) { this._loadingText = value; },

    get_noRecordsText: function()
    {
        //If text specified return it otherwise if "loading" text was specified and text for no records was not we need to at least return something to clear loading text.
        if (this._noRecordsText)
            return this._noRecordsText;
        else if (this.get_loadingText())
            return " ";

        return null;
    },
    set_noRecordsText: function(value) { this._noRecordsText = value; },

    get_utcDateColumns: function() { return this._utcDateColumns; },
    set_utcDateColumns: function(value) { this._utcDateColumns = value; },

    get_showLoading: function() { return this._showLoading; },
    set_showLoading: function(value) { this._showLoading = value; },

    get_preProcessFunction: function()
    {
        if (this._utcDateColumns || this.get_expand())
        {
            var isBull = this.get_listFormat() == "bull";
            var isCsv = this.get_listFormat() == "csv";
            var lstart = (this.get_listFormat() == "bull" ? "<li>" : "");
            var lend = (this.get_listFormat() == "bull" ? "</li>" : (!this.get_listFormat() ? "<br />" : ""));

            return function(data, grid)
            {
                //adjustDateTime function fixes issue caused by ADO.Net Data Service serializing DateTime data as UTC even though we are not storing that way
                function adjustDateTime(dt)
                {
                    if (dt)
                    {
                        dt = eval("new " + dt.replace(new RegExp("\/", "ig"), ""));
                        if (!isNaN(dt))
                            dt = "/Date(" + dt.setMinutes(dt.getMinutes() + dt.getTimezoneOffset()) + ")/";
                        else
                            return "";
                    }
                    return dt;
                }

                var gw = grid.get_masterTableView ? grid.GridWrapper : grid.get_parent().GridWrapper;
                if (gw)
                {
                    var col;
                    var props;
                    var prop;
                    var csv;
                    var cols = gw.get_utcDateColumns() ? gw.get_utcDateColumns().replace(/ /ig, "").split(",") : [];
                    var rel = $.grep(grid._data._columnsData, function(i, x) { return i.DataField && i.DataField.indexOf(".") > -1; });
                    for (var i = 0; i < data.length; i++)
                    {
                        for (var c = 0; c < cols.length; c++)
                        {
                            col = cols[c];
                            data[i][col] = adjustDateTime(data[i][col]);
                        }
                        for (var c = 0; c < rel.length; c++)
                        {
                            col = rel[c].DataField;
                            props = col.split(".");
                            prop = data[i][props[0]];
                            if (prop)
                            {
                                if (!prop.results)
                                    data[i][col] = prop[props[1]]; //add expanded property
                                else
                                {      //collection so loop through to create list
                                    csv = "";
                                    if (isBull) csv = "<ul>";

                                    prop = prop.results.sort(function(a, b)
                                    {
                                        a = a[props[1]].toUpperCase();
                                        b = b[props[1]].toUpperCase();
                                        if (a > b)
                                            return 1;
                                        else if (a < b)
                                            return -1;
                                        else return 0;

                                    });
                                    for (var x = 0; x < prop.length; x++)
                                    {
                                        if (isCsv) csv += (csv.length > 0 ? ", " : "");
                                        csv += lstart + prop[x][props[1]] + lend;
                                    }
                                    if (isBull) csv += "</ul>";

                                    data[i][col] = csv; //add expanded collection property
                                }
                            }
                        }
                    }

                }
            };
        }
    },

    get_SortString: function()
    {
        return this.get_masterTableView().get_sortExpressions().toString();
    },

    scrollToSelection: function()
    {
        var mt = this.get_masterTableView();
        var items = mt.get_selectedItems();
        var top = 0;
        if (items && items.length > 0)
        {
            var row = items[0].get_element();
            var height = Sys.UI.DomElement.getBounds(row).height;

            var r = row.parentNode.rows;
            for (var i = 0; i < row.rowIndex; i++)
            {
                top += Sys.UI.DomElement.getBounds(r[i]).height;
            }

            if ((top + height) < $(mt.get_element()).parent().height()) //don't scroll if at top of grid 
                top = 0;
        }

        $(mt.get_element()).parent().scrollTop(top);
    },

    canDataBind: function()
    {
        return !this.get_requiresFilter() || this.get_masterTableView().get_filterExpressions().get_count() > 0;
    },

    dataBind: function()
    {
        this._refreshCount = true;
        this._dataBind();
    },

    _dataBind: function()
    {
        if (this._suspendUI) return;

        if (this.canDataBind())
        {
            var h = this.get_events().getHandler("databinding");
            if (h)
            {
                var args = { cancel: false, refreshing: this._refreshCount, get_cancel: function() { return this.cancel; }, set_cancel: function(value) { this.cancel = value; } };
                h(this, args);
                if (args.get_cancel()) return;
            }

            var mt = this.get_masterTableView();
            var cs = mt.get_parent().ClientSettings.DataBinding;
            var url = cs.Location + "/";
            var q = "";

            if (this.get_loadingText())
                $(mt.get_element().rows[mt.get_element().rows.length - 1].cells[0]).text(this.get_loadingText());

            if (this.get_pagerSelector() && this.get_showLoading())
                $(this.get_pagerSelector() + (this._refreshCount ? "" : " .pagerText")).html("<span class='loading'>" + this.get_loadingText() + "</span>");

            //if custom paging initialize filter for page index if not set
            if (this.get_customPaging())
            {
                if (!mt.get_filterExpressions().find("Page_Index"))
                    $setGridFilter(mt, "Page_Index", 0, "EqualTo", "System.Int32");
            }

            //if refreshing count set page back to 0
            if (this._refreshCount)
                mt.CurrentPageIndex = 0;

            if (cs.DataService && cs.DataService.TableName)
            {
                url += cs.DataService.TableName;
                q = $getDataServiceQuery(mt);
            }
            else
            {
                url += cs.SelectMethod;
                q = $getWebServiceQuery(mt);
            }

            //            if (this.get_clientManager()) this.get_clientManager().ajaxRequestStarting();

            $refreshGridData(mt, url, q, this.get_preProcessFunction());

            //Get Virtual Count to construct custom pager - update to data services does not require this (using inlineCountEnabled flag to check)
            if (!this.get_inlineCountEnabled() && this._refreshCount)
            {
                if (cs.SelectCountMethod)
                {
                    var url2 = cs.Location + "/" + cs.SelectCountMethod;

                    var filterText = "";
                    if (cs.DataService && cs.DataService.TableName)
                        filterText = "where='" + $getFilterTextForServiceRequest(mt, this.get_customPaging()) + "'";
                    else
                        filterText = $getWebServiceQuery(mt);

                    $.getJSON(url2 + "?" + filterText, null, this._onRecordCountCallbackDelegate);
                }
            }

        }
        else
            this.clearGrid();
    },

    clearGrid: function()
    {
        this._suspendUI = true;
        var mt = this.get_masterTableView();
        mt.set_dataSource([]);
        mt.dataBind();

        if (this.get_pagerSelector())
            $(this.get_pagerSelector()).html("");

        this._hdrFixUp();

        this._suspendUI = false;
    },

    _onCommand: function(sender, args)
    {
        this._refreshCount = args.get_commandName() == "RebindGrid";

        // Numeric Sorting Fix
        if (args.get_commandName() == "Sort")
        {

            var col = args.get_commandArgument();
            var mt = sender.get_masterTableView();
            var columnDef = mt.getColumnByUniqueName(col);
            if (columnDef == null) return;

            var dtype = columnDef.get_dataType();

            if ((dtype == "System.Double" || dtype == "System.Decimal" || dtype == "System.Int16" || dtype == "System.Int32" || dtype == "System.Int64"))
            {
                var sortExpressions = mt.get_sortExpressions();

                var id1 = String.format("#{0}__{1}__SortAsc", mt.get_id(), col);
                var id2 = String.format("#{0}__{1}__SortDesc", mt.get_id(), col);

                $(id1 + "," + id2).hide();

                //if sort is ASC make DESC if DESC make no sort by removing
                var exps = $.grep(sortExpressions._array, function(i, x) { return i.FieldName == col; });
                var ord;
                if (exps.length > 0)
                {
                    for (var i = 0; i < exps.length; i++)
                    {
                        ord = exps[i].get_sortOrder();
                        if (ord == 1)
                        {
                            exps[i].set_sortOrder(2);
                            $(id2).show();
                        }
                        else
                            sortExpressions.remove(exps[i]);
                    }
                }
                else //no sort should be translated to ASC
                {
                    var sortExp = new Telerik.Web.UI.GridSortExpression();
                    sortExp.set_fieldName(col);
                    sortExp.set_sortOrder(1);
                    sortExpressions.add(sortExp);
                    $(id1).show();
                }
            }
        }

        this._dataBind();
    },

    _onDataBinding: function(sender, args)
    {
        args.set_cancel(true);
    },

    _onDataBound: function(sender, args)
    {
        //        if (this.get_clientManager()) this.get_clientManager().ajaxRequestComplete();

        if (this._suspendUI) return;

        if (this.canDataBind())
        {
            var mt = this.get_masterTableView();

            if (this.get_noRecordsText())
                $(mt.get_element().rows[mt.get_element().rows.length - 1].cells[0]).text(this.get_noRecordsText());


            //Merge Rows of sorted columns
            //Deepthi - 12/04/12 - added condition to check if the browser is ie6 or ie7. To fix the drilldown issue in standard reports
            if (this.get_mergeRows() && !(ie6 || ie7))
                $mergeGridCells(this.get_grid());

            this._hdrFixUp();

            var h = this.get_events().getHandler("databound");
            if (h) h(this, new Sys.EventArgs());


            //Once grid is loaded disable multi-column sorting because it is too confusing to users
            this.get_masterTableView().set_allowMultiColumnSorting(false);

        }
        else
            this.clearGrid();
    },

    _hdrFixUp: function()
    {
        //Hack to align grid headers
        if (mac & safari)
            resetGridHeadersX(500);
        //hdr fixup
        padScrollbar(this.get_masterTableView().get_element());
    },

    _configureSorting: function()
    {
        //if sort specified initialize sort expressions
        var sortString = this.get_sortOrder();
        if (sortString != null)
        {
            var mt = this.get_masterTableView();
            var col = mt.get_sortExpressions();
            var sortExp;
            var sortVals;
            var asc;
            var btns;
            sortString = sortString.split(",");
            for (var i = 0; i < sortString.length; i++)
            {
                sortExp = new Telerik.Web.UI.GridSortExpression();
                sortVals = $.trim(sortString[i]).split(" ");
                sortExp.set_fieldName(sortVals[0]);
                asc = sortVals[1] != "DESC";
                sortExp.set_sortOrder(asc ? 1 : 2);
                col.add(sortExp);

                //* telerik hack - fix for extra image being output by telerik when setting sortexpressions in markup on server
                if (mt.getColumnByUniqueName(sortVals[0]))
                {
                    btns = $(mt.getColumnByUniqueName(sortVals[0]).get_element()).children("input");
                    if (btns.length > 2)
                    {
                        $(btns[0]).hide();
                        $(btns[asc ? 1 : 2]).show();
                    }
                }
                //
            }
        }
    },

    _prepDynamicColumns: function()
    {
        if (this.get_supportsDynamicColumns())
        {
            //configure column css classes based on first row of data portion - css classes are used to configure dynamic columns
            var g = this.get_grid();
            if ($.browser.msie && $.browser.version < 8)
                $(g.get_masterTableViewHeader().get_element().parentNode).addClass("rgHeaderDivPlanInfo");


            var rows = g.get_masterTableViewHeader().get_element().rows;
            var rowsMain = g.get_masterTableView().get_element().rows;
            //applying data row styles to header and filter row
            $(g.get_masterTableViewHeader().get_element()).find("col").each(
                    function(idx)
                    {
                        var css = rowsMain[0].cells[idx].className.split(" ");
                        //get last item because we don't want to copy data specific class like "alignRight".                    
                        css = css[css.length - 1];
                        if (css)
                        {
                            $(rows[0].cells[idx]).addClass(css);
                            if (rows[1].cells.length > idx)
                                rows[1].cells[idx].className = css;
                        }
                    }
        );
            //removing colgroup because it affects column widths
            $(g.get_element()).find("colgroup").remove();
            //    
        }
    },

    updateRecordCount: function(count)
    {
        //        if (this._refreshCount)
        this._onRecordCountCallback(count);
    },
    _onRecordCountCallback: function(data)
    {
        this._refreshCount = false;

        var pagerHTML = "";

        var mt = this.get_masterTableView();

        //make sure mt is still availabe - could be null if page unloaded before request completes
        if (mt)
        {
            var count = 0;
            if (isNaN(data)) //old
            {
                for (var k in data.d)
                    count = data.d[k];
            }
            else  //new
                count = parseInt(data, 10);


            mt.set_virtualItemCount(count);

            var h = this.get_events().getHandler("recordcountchanged");
            if (h) h(this, new Sys.EventArgs());

            if (this.get_pagerSelector() != null && this.get_pagerSelector() != "")
            {
                if (count > 0)
                {
                    pagerHTML = $constructCustomPager(mt, count, false, this.get_clientManager() ? this.get_clientManager().get_BasePath() : "/", this.get_customPaging());
                }

                $(this.get_pagerSelector()).html(pagerHTML);
            }
            //            else //using Alert and not throw new Error() because on some occasions exceptions cause strange issues from this callback
            //                $alert("pagerSelector property is not set.  If you do not need paging do not set SelectCountMethod on the RadGrid.");
        }
    }
};
Pathfinder.UI.GridWrapper.registerClass('Pathfinder.UI.GridWrapper', Sys.UI.Behavior);

Pathfinder.UI.GridFilterExpression = function()
{
    Pathfinder.UI.GridFilterExpression.initializeBase(this);

    this.isExtension = false;
};
Pathfinder.UI.GridFilterExpression.prototype =
{
    get_isExtension: function() { return this.isExtension; },
    set_isExtension: function(value) { this.isExtension = value; },

    get_extensionFilterText: function()
    {
        return String.format("&{0}={1}", encodeURIComponent(this.get_fieldName()), encodeURIComponent(this.get_fieldValue()));
    }
};
Sys.Application.add_init(function()
{
    try
    {
        if (Telerik.Web.UI.GridFilterExpression)
            Pathfinder.UI.GridFilterExpression.registerClass('Pathfinder.UI.GridFilterExpression', Telerik.Web.UI.GridFilterExpression);
    } catch (ex) { }
});

Pathfinder.UI.PlanInfoGridWrapper = function(element)
{
    Pathfinder.UI.PlanInfoGridWrapper.initializeBase(this, [element]);

    this._totalLivesHdrText = "";
    this._planTypeHdrText = "";

    this._restrictByTerritory = false;

    this._onRowSelectedDelegate = null;
};

Pathfinder.UI.PlanInfoGridWrapper.prototype =
{
    initialize: function()
    {
        Pathfinder.UI.PlanInfoGridWrapper.callBaseMethod(this, 'initialize');

        $(this.get_element()).css("visibility", "hidden").addClass("notbound");

        var g = this.get_grid();

        var c = g.get_masterTableView().getColumnByUniqueName("Total_Covered");
        if (c) this._totalLivesHdrText = $(c.get_element()).find("a").text();
        var c = g.get_masterTableView().getColumnByUniqueName("Plan_Type_Name");
        if (c) this._planTypeHdrText = $(c.get_element()).find("a").text();
        var c = g.get_masterTableView().getColumnByUniqueName("Total_Pharmacy");
        if (c) this._totalPharmHdrText = $(c.get_element()).find("a").text();
        var c = g.get_masterTableView().getColumnByUniqueName("Rx_Lives");
        if (c) this._rxLivesHdrText = $(c.get_element()).find("a").text();

        g.add_rowSelected((this._onRowSelectedDelegate = Function.createDelegate(this, this._onRowSelected)));

    },

    dispose: function()
    {
        delete (this._onPlanGridRowSelectedDelegate);

        Pathfinder.UI.PlanInfoGridWrapper.callBaseMethod(this, 'dispose');
    },

    get_containerID: function() { return "divTile2Plans"; },

    get_showLoading: function() { return false; },
    set_showLoading: function(value) { },

    get_restrictByTerritory: function() { return this._restrictByTerritory; },
    set_restrictByTerritory: function(value)
    {
        clientManager.setContextValue("restrictByTerritory", value);
        this._restrictByTerritory = value;
        this.dataBind();
    },

    _dataBind: function()
    {
        var cm = this.get_clientManager();
        var grid = this.get_grid();

        if (cm)
        {
            var channel = cm.get_Channel();
            var region = cm.get_Region();
            var clientHasCustomPlan = cm.get_clientHasCustomPlans();

            //switch service if lookup is standard or territory specific
            //            if (!this.get_restrictByTerritory())
            //            {
            //                grid.ClientSettings.DataBinding.Location = cm.get_ApplicationManager().get_ServiceUrl();
            //            }
            //            else
            //            {

            //            }

            //            //If a state is selected, select regional affiliates and independents
            //            if (cm.get_Region())
            //                $setGridFilter(grid, "Plan_Classification_ID", "2,3", "Custom", "System.Int32");
            //            //If national, select parent and independent plans
            //            else
            //                $setGridFilter(grid, "Plan_Classification_ID", "1,3", "Custom", "System.Int32");

            //if (channel == 12 || channel == 11)
            if ((channel.length == 1) && ($.inArray(12, channel) > -1 || $.inArray(11, channel) > -1))
            {
                grid.ClientSettings.DataBinding.Location = cm.get_ApplicationManager().get_ServiceUrl();
                var sortnew = grid.get_masterTableView().get_sortExpressions();

                if (sortnew.get_count() > 0)
                {
                    var sortExpression = new Telerik.Web.UI.GridSortExpression();
                    sortExpression.set_fieldName(sortnew._array[0]._fieldName);
                    sortExpression.set_sortOrder(sortnew._array[0]._sortOrder);
                }

                grid.get_masterTableView().sort("Custom_Sort DESC");
                if (sortExpression)
                    grid.get_masterTableView()._sortExpressions.add(sortExpression);

            }
            else
            {
                //if (clientHasCustomPlan == true)
                if (parseInt(channel.toString()) > 100)
                    grid.ClientSettings.DataBinding.Location = "custom/" + cm.get_ApplicationManager().get_clientKey() + "/todaysaccounts/services/CustomSegmentsDataService.svc"; //cm.get_ApplicationManager().get_ClientServiceUrl();
                else
                    grid.ClientSettings.DataBinding.Location = cm.get_ApplicationManager().get_ServiceUrl();

                if (grid.get_masterTableView().get_sortExpressions().get_count() > 0 && grid.get_masterTableView().get_sortExpressions()._array[0]._fieldName == 'Custom_Sort')
                    grid.get_masterTableView().sort("Plan_Name ASC");
                //                //Millennium custom segments: for 107(VA Records) by default sort on Plan_ID ASC and Plan_Name ASC.
                //                else if (channel && channel.length == 1 && $.inArray(107, channel) == 0)
                //                {
                //                    //sort on selected field.
                //                    if (grid.get_masterTableView().get_sortExpressions().get_count() > 0 && grid.get_masterTableView().get_sortExpressions()._array[0]._fieldName != "Plan_ID")
                //                       var grdSortExpr = grid.get_masterTableView().get_sortExpressions()._array[0];
                //                    
                //                    //clear sort expressions
                //                    grid.get_masterTableView()._sortExpressions.clear();
                //                   //Sort on Plan_ID 
                //                   var sortExpression = new Telerik.Web.UI.GridSortExpression();
                //                   sortExpression.set_fieldName("Plan_ID");
                //                   sortExpression.set_sortOrder(Telerik.Web.UI.GridSortOrder.Ascending);
                //                   grid.get_masterTableView()._sortExpressions.add(sortExpression);

                //                    //sort on selected field.
                //                    if (grdSortExpr)
                //                    {
                //                        var fldName = grdSortExpr._fieldName;
                //                        var fldSortOrd = (grdSortExpr._sortOrder == 1 ? Telerik.Web.UI.GridSortOrder.Ascending : Telerik.Web.UI.GridSortOrder.Descending);
                //                        var sortExpression = new Telerik.Web.UI.GridSortExpression();
                //                        sortExpression.set_fieldName(fldName);
                //                        sortExpression.set_sortOrder(fldSortOrd);
                //                        grid.get_masterTableView()._sortExpressions.add(sortExpression);
                //                        grid.get_masterTableView()._showSortIconForField(fldName, fldSortOrd);

                //                    }
                //                 }
                //Disable sorting if more than one channel is selected or 'All' is selected
                else if (channel.length > 1 || $.inArray(0, channel) > -1)
                {
                    //clear sort expressions
                    grid.get_masterTableView()._sortExpressions.clear();

                    //Sort on Plan Name and State for multi channel selection
                    var sortExpression = new Telerik.Web.UI.GridSortExpression();
                    sortExpression.set_fieldName("Plan_Name");
                    sortExpression.set_sortOrder(Telerik.Web.UI.GridSortOrder.Ascending);
                    grid.get_masterTableView()._sortExpressions.add(sortExpression);
                    grid.get_masterTableView()._showSortIconForField("Plan_Name", Telerik.Web.UI.GridSortOrder.None);

                    sortExpression = new Telerik.Web.UI.GridSortExpression();
                    sortExpression.set_fieldName("Plan_State");
                    sortExpression.set_sortOrder(Telerik.Web.UI.GridSortOrder.Ascending);
                    grid.get_masterTableView()._sortExpressions.add(sortExpression);
                    grid.get_masterTableView()._showSortIconForField("Plan_State", Telerik.Web.UI.GridSortOrder.None);
                    sortExpression = new Telerik.Web.UI.GridSortExpression();
                    sortExpression.set_fieldName("Section_Name");
                    sortExpression.set_sortOrder(Telerik.Web.UI.GridSortOrder.Ascending);
                    grid.get_masterTableView()._sortExpressions.add(sortExpression);
                    grid.get_masterTableView()._showSortIconForField("Section_Name", Telerik.Web.UI.GridSortOrder.None);
                }
                //If switching back to single segment, reset the sorting back to Plan_Name ASC
                else if (grid.get_masterTableView().get_sortExpressions().get_count() > 1)
                {
                    grid.get_masterTableView()._sortExpressions.clear();
                    grid.get_masterTableView().sort("Plan_Name ASC");
                }
            }

            var byTerr = this.get_restrictByTerritory() ? "ByTerritory" : "";
            if (byTerr)
            {
                //Millennium custom segments: for 107(VA Records) My Accounts will be showing top plans based on ranking.
                if (channel && channel.length == 1 && $.inArray(107, channel) == 0)
                {
                    //clear sort expressions
                    grid.get_masterTableView()._sortExpressions.clear();

                    sortExpression = new Telerik.Web.UI.GridSortExpression();
                    sortExpression.set_fieldName("Custom_Plan_Rank");
                    sortExpression.set_sortOrder(Telerik.Web.UI.GridSortOrder.Ascending);
                    grid.get_masterTableView()._sortExpressions.add(sortExpression);
                    grid.get_masterTableView()._showSortIconForField("Plan_Name", Telerik.Web.UI.GridSortOrder.None);
                }
                else
                {
                    if (grid.get_masterTableView()._sortExpressions.toList()[0].get_fieldName() == 'Custom_Plan_Rank')
                    {
                        //clear sort expressions
                        grid.get_masterTableView()._sortExpressions.clear();

                        //Sort by Plan_Name by default
                        var sortExpression1 = new Telerik.Web.UI.GridSortExpression();
                        sortExpression1.set_fieldName("Plan_Name");
                        sortExpression1.set_sortOrder(Telerik.Web.UI.GridSortOrder.Ascending);
                        grid.get_masterTableView()._sortExpressions.add(sortExpression1);
                        grid.get_masterTableView()._showSortIconForField("Plan_Name", Telerik.Web.UI.GridSortOrder.Ascending);
                    }
                }
            }
            else
            {
                var sortExpression = new Telerik.Web.UI.GridSortExpression();
                sortExpression.set_fieldName("Custom_Plan_Rank");
                sortExpression.set_sortOrder(Telerik.Web.UI.GridSortOrder.Ascending);

                //If 'Custom_Plan_Rank' field is included in sortExpressions then remove it
                //and add Plan_Name by default
                if (grid.get_masterTableView()._sortExpressions.toList()[0].get_fieldName() == 'Custom_Plan_Rank')
                {
                    //clear sort expressions
                    grid.get_masterTableView()._sortExpressions.clear();

                    //Sort by Plan_Name by default
                    var sortExpression1 = new Telerik.Web.UI.GridSortExpression();
                    sortExpression1.set_fieldName("Plan_Name");
                    sortExpression1.set_sortOrder(Telerik.Web.UI.GridSortOrder.Ascending);
                    grid.get_masterTableView()._sortExpressions.add(sortExpression1);
                    grid.get_masterTableView()._showSortIconForField("Plan_Name", Telerik.Web.UI.GridSortOrder.Ascending);
                }
            }

            grid.ClientSettings.DataBinding.DataService.TableName = String.format("PlanInfoListView{0}Set", byTerr);


            Pathfinder.UI.PlanInfoGridWrapper.callBaseMethod(this, '_dataBind');
        }
    },

    _onCommand: function(sender, args)
    {
        // Disable sorting if multiple segments are selected
        if (args.get_commandName() == "Sort")
        {
            var cm = this.get_clientManager();
            var grid = this.get_grid();

            if (cm)
            {
                var channel = cm.get_Channel();

                if (channel.length > 1)
                {
                    args.set_cancel(true);

                    //Hide all sorting arrows
                    $('#ctl00_main_planInfo_gridPlanInfo .rgSortAsc, #ctl00_main_planInfo_gridPlanInfo .rgSortDesc').hide();

                    return;
                }
            }
        }

        //this._dataBind();

        Pathfinder.UI.PlanInfoGridWrapper.callBaseMethod(this, '_onCommand', [sender, args]);
    },

    _onRowSelected: function(sender, args)
    {
        var cm = this.get_clientManager();

        //Hide popup if open
        $('#infoPopup').hide();

        ///<summary>Event handler for the plan information grid's RowSelected event.  This event is handled to support setting the effective section based on the current row which is used when the current section is 'All'.  This handler should not be called directly.</summary>
        var names = sender.get_masterTableView().get_clientDataKeyNames();
        var data = {};
        for (var i = 0; i < names.length; i++)
        {
            data[names[i]] = args.getDataKeyValue(names[i]);
        }

        if (cm.get_Channel()[0] != 107)
        {
            if (data)
                delete (data["VISN"]);
        }

        var dataItem = args.get_gridDataItem().get_dataItem();

        //If Commercial, Med-D or Managed Medicaid is selected in the row, set effective Channel to 99 if
        //two or more of those segments are selected in the Channel Dropdown
        //if (dataItem.Section_ID == 1 || dataItem.Section_ID == 6 || dataItem.Section_ID == 17)
        //{
        //Check to see if at least 2 segments are selected of Commercial, Managed Med and Med D
        var combinedSelected = 0;

        if ($.inArray(1, cm.get_Channel()) > -1)
            combinedSelected++;
        if ($.inArray(6, cm.get_Channel()) > -1)
            combinedSelected++;
        if ($.inArray(17, cm.get_Channel()) > -1)
            combinedSelected++;

        //Check to see if the plan actually has combined business
        var hasCombinedBusiness = 0;

        //sl 5/31/2012 to avoid error: if 'All' selected & Combine business - data["Original_Section"] should not be 0
        var tempSections = [];
        if (dataItem.Has_Commercial_Business)
        //hasCombinedBusiness++;
            tempSections.push(1);
        if (dataItem.Has_Managed_Medicaid_Business)
        //hasCombinedBusiness++;
            tempSections.push(6);
        if (dataItem.Has_Medicare_PartD_Business)
        //hasCombinedBusiness++;
            tempSections.push(17);


        //If multiple segments are selected and plan has combined business, then set Channel to combined
        //if (combinedSelected > 1 && hasCombinedBusiness > 1)
        //{
        //Remove any leftover row highlighting
        $(sender.get_masterTableView().get_element()).find(".rgSelectedRow").removeClass("rgSelectedRow");

        //Change the row css to highlight all the selected plan rows
        var planid = data.Plan_ID;

        var mergedRowCount = 0;
        for (var i = 0; i <= (sender.get_masterTableView().get_dataItems().length - 1); i++)
        {

            var itemM = sender.get_masterTableView().get_dataItems()[i];
            var dataItemM = itemM.get_dataItem();
            if (dataItemM != null)
            {
                //highlight the row if the selected plan_id = plan_id of the record 
                if (dataItemM.Plan_ID == planid)
                {
                    $(itemM.get_element()).addClass("rgSelectedRow");
                    mergedRowCount++;
                }
            }
        }

        //Only set Effective Channel to 99 if there is more than one row selected (merged row) and more than one channel is selected
        if (mergedRowCount > 1 && (cm.get_Channel().length > 1 || $.inArray(0, cm.get_Channel()) > -1))
        {
            //sl 5/31/2012 to avoid error: if 'All' selected & Combine business - data["Original_Section"] should not be 0
            if ($.inArray(0, cm.get_Channel()) > -1)
                data["Original_Section"] = tempSections;
            else //Send original channels in case of Combined                     
                data["Original_Section"] = cm.get_Channel();
            if (cm.get_Channel()[0] > 100)
                cm.set_EffectiveChannel(dataItem.Section_ID);
            else
            {
                cm.set_EffectiveChannel(99);
                data["Section_ID"] = 99;
            }
        }
        else
            cm.set_EffectiveChannel(dataItem.Section_ID);


        //}
        //else
        //    cm.set_EffectiveChannel(dataItem.Section_ID);
        //}
        //else //Otherwise set the effective channel to whatever row is selected
        //    cm.set_EffectiveChannel(dataItem.Section_ID);



        //Always set effective channel to the selected row
        //if (cm.get_Channel() == 0)
        //{
        //cm.set_EffectiveChannel(dataItem.Section_ID);
        //}
        //else if (cm.get_Channel() == 17)
        //if (channel.length == 1 && $.inArray(17, cm.get_Channel()) > -1)
        if (cm.get_EffectiveChannel() == 17)
        {
            if (cm.get_Region())
                data["Plan_State"] = dataItem.Plan_State;

            data["Prod_ID"] = dataItem.Prod_ID;
        }



        //Special case for CP - National vs Region - find way to make more generic
        var region = cm.get_Region();
        if (cm.get_EffectiveChannel() == 1 || cm.get_EffectiveChannel() == 99 || cm.get_EffectiveChannel() == 4 || cm.get_EffectiveChannel() == 6 || cm.get_EffectiveChannel() == 11 || cm.get_EffectiveChannel() == 12)
        {
            //Parent or PBM - show Affiliations link
            if (args.get_item().get_dataItem().Plan_Classification_ID == 1 || cm.get_EffectiveChannel() == 4) //Parent
            {
                $("#link_affiliations").show();
            }
            else //regional or other
            {
                $("#link_affiliations").hide();

                if (cm.get_Module() == "affiliations")
                {
                    //special case - must manually set selection data to new data and then reset module to planinformation which refreshes section 2 with current selection data that is manually set
                    if (!cm._selectionData) cm._selectionData = [];
                    cm._selectionData[0] = data;
                    cm.set_Module("planinformation");
                    cm.get_ApplicationManager().get_ModuleMenu().highlightItem("link_planinformation");
                    return; //return so selection data is not set again which causes issues
                }
            }
            if (cm.get_EffectiveChannel() == 11 || cm.get_EffectiveChannel() == 12)
            {
                if (args.get_item().get_dataItem().Plan_Classification_ID == 1)
                    $("#link_coveredlives").show();
                else
                    $("#link_coveredlives").hide();
            }
        }
        else if (cm.get_EffectiveChannel() == 17)
        {
            if (!region)
            {
                $("#link_affiliations").show();
            }
            else
            {
                $("#link_affiliations").hide();
                if (cm.get_Module() == "affiliations")
                {
                    //special case - must manually set selection data to new data and then reset module to planinformation which refreshes section 2 with current selection data that is manually set
                    if (!cm._selectionData) cm._selectionData = []; // mapTIle
                    cm._selectionData[0] = data;
                    cm.set_Module("planinformation");
                    cm.get_ApplicationManager().get_ModuleMenu().highlightItem("link_planinformation");
                    return; //return so selection data is not set again which causes issues
                }
            }
        }
        else if ((cm.get_Channel()[0] == 105) || (cm.get_Channel()[0] == 106) || (cm.get_Channel()[0] == 107) || (cm.get_Channel()[0] == 108))
        {
            if ((args.get_item().get_dataItem().Plan_Classification_ID == 1) && (cm.get_Channel()[0] == 107)) //Parent
            {
                $("#link_affiliations").show();

                //for section_ID=107, pass only VISN in the filter to pull affilications.
                if (cm.get_Module() == "affiliations")
                {
                    data["VISN"] = dataItem.VISN;
                }
                else
                {
                    if (data)
                        delete (data["VISN"]);
                }
            }
            else //other
            {
                $("#link_affiliations").hide();

                if (data)
                    delete (data["VISN"]);

                if (cm.get_Module() == "affiliations")
                {
                    //special case - must manually set selection data to new data and then reset module to planinformation which refreshes section 2 with current selection data that is manually set
                    if (!cm._selectionData) cm._selectionData = [];

                    cm._selectionData[0] = data;
                    cm.set_Module("planinformation");
                    cm.get_ApplicationManager().get_ModuleMenu().highlightItem("link_planinformation");
                    return; //return so selection data is not set again which causes issues
                }
            }

            if (cm.get_Module() == "contacts")
            {
                //special case - must manually set selection data to new data and then reset module to planinformation which refreshes section 2 with current selection data that is manually set
                if (!cm._selectionData) cm._selectionData = []; // mapTIle
                cm._selectionData[0] = data;
                cm.set_Module("planinformation");
                cm.get_ApplicationManager().get_ModuleMenu().highlightItem("link_planinformation");
                return; //return so selection data is not set again which causes issues
            }
        }

        //code commented to avoid showing irrelevant modules in case all is selected
        //If 'All' Channel selected, show and hide module menu options depending on channel (row) selected
        //        if ($.inArray(0, cm.get_Channel()) > -1)
        //        {
        //            var module = cm.get_ApplicationManager().configureModuleMenu(cm, cm.get_EffectiveChannel(), cm.get_Module());
        //        }

        //
        if (cm.get_SelectionData() == null || data["Plan_ID"] != cm.get_SelectionData()["Plan_ID"] || data["Prod_ID"] != cm.get_SelectionData()["Prod_ID"])
        {
            delete (data["Section_ID"]);
            new cmd(cm, "set_SelectionData", [data], 10);
            //cm.set_SelectionData(data);
        }
    },

    _onDataBound: function(sender, args)
    {
        Pathfinder.UI.PlanInfoGridWrapper.callBaseMethod(this, '_onDataBound', [sender, args]);

        if (!this.get_clientManager()) return;

        $(this.get_element()).css("visibility", "visible").removeClass("notbound");

        //
        var mt = sender.get_masterTableView();

        mt.clearSelectedItems();

        if (mt.get_dataSource().length > 0)
        {
            if (this.get_clientManager().get_SelectionData() != null)
            {
                var id = this.get_clientManager().get_SelectionData()["Plan_ID"];
                var a = $.grep(mt.get_dataItems(), function(i) { return i.get_dataItem() && i.get_dataItem().Plan_ID == id; });
                if (a && a.length > 0)
                    a[0].set_selected(true);
                else
                    mt.selectItem(0);
            }
            else
            {
                mt.selectItem(0);
            }
        }
        else //force null selection to reload Section 2
        {
            this.get_clientManager()._setSelectionData(null, 0, false);
        }

        this.adjustPlanInfoGridColumns(mt);


        this.scrollToSelection();

        //show reset button if custom filters have been added (only applies to filters above grid and not Channel or State selection in map)
        if ($(mt.get_tableFilterRow()).children("TD").find("INPUT[value != ''][type=text]").length > 0)
        {
            $("#resetPlans").show();
        }
        else
            $("#resetPlans").hide();


        resizeSearchTextBox();
    },

    _hdrFixUp: function()
    {
        resetGridHeaders();
        //        Pathfinder.UI.PlanInfoGridWrapper.callBaseMethod(this, '_hdrFixUp');
    },

    get_supportsDynamicColumns: function() { return true; },

    adjustPlanInfoGridColumns: function(mt)
    {
        var hide = "";
        var show = "";
        var hdrText = this._totalLivesHdrText;
        var hdrTextPlanType = this._planTypeHdrText;
        var hdrTextPharm = this._totalPharmHdrText;
        var hdrTextRxLives = this._rxLivesHdrText;

        var channel = this.get_clientManager().get_Channel();
        var region = this.get_clientManager().get_Region();

        if (channel.length == 1)
        {
            channel = parseInt(channel.toString(), 10);
            switch (channel)
            {
                case 0: //All
                    show = ".secName, .totalLives, .rxLives, .employerLives, .formularyManagedBy";
                    hide = ".planType, .VISN, .macJur";
                    break;
                case 1: //Commercial
                    show = ".planType, .totalLives, .rxLives, .formularyManagedBy";
                    hide = ".VISN, .macJur, .secName, .employerLives";
                    hdrText = "Medical Lives";
                    break;
                case 99: //Combined
                    show = ".planType, .totalLives, .rxLives, .formularyManagedBy";
                    hide = ".VISN, .macJur, .secName, .employerLives";
                    //hdrTextPharm = "Commercial Lives";
                    break;
                case 6: //Managed Medicaid
                    show = ".planType, .totalLives, .rxLives, .formularyManagedBy";
                    hide = ".VISN, .macJur, .secName, .employerLives";
                    //hdrTextPharm = "Managed Medicaid";
                    break;
                case 4: //PBM
                    show = ".totalLives, .rxLives, .employerLives, .formularyManagedBy";
                    hide = ".planType, .VISN, .macJur, .secName";
                    //hdrTextPharm = "PBM Pharmacy Lives";
                    break;
                case 9: //State Medicaid
                    show = ".totalLives, .rxLives, .formularyManagedBy";
                    hide = ".planType, .VISN, .macJur, .secName, .employerLives";
                    hdrTextRxLives = "FFS Lives";
                    break;
                case 14: //Employer
                    show = ".totalLives, .rxLives, .formularyManagedBy";
                    hide = ".planType, .VISN, .macJur, .secName, .employerLives";
                    //hdrText = "Number of Employees";
                    break;
                case 5: //Wholesale/Trade
                    show = ".planType, .totalLives, .rxLives, .formularyManagedBy";
                    hide = ".VISN, .macJur, .secName, .employerLives";
                    break;
                case 11: //VA
                    show = ".VISN, .totalLives, .rxLives, .formularyManagedBy";
                    hide = ".planType, .macJur, .secName, .employerLives";
                    break;
                case 3: //SPP
                case 12: //DOD
                    show = ".totalLives, .rxLives, .formularyManagedBy";
                    hide = ".VISN, .planType, .macJur, .secName, .employerLives";
                    break;
                case 15: //Coalition
                    show = "";
                    hide = ".planType, .VISN, .macJur, .secName, .employerLives, .totalLives, .rxLives, .formularyManagedBy";
                    break;
                case 2: //Med A-B
                    show = ".totalLives, .rxLives";
                    hide = ".formularyManagedBy, .planType, .totalPharm, .VISN, .macJur, .secName, .employerLives";
                    break;
                case 13: //MBHO
                    show = ".totalLives, .rxLives, .formularyManagedBy";
                    hide = ".planType, .totalPharm, .VISN, .macJur, .secName, .employerLives";
                    break;
                case 16: //MAC
                    show = ".rxLives,  .macJur";
                    hide = ".formularyManagedBy, .totalLives, .planType, .VISN, .secName, .employerLives";
                    break;
                case 17: //Med-D
                    show = ".planType, .totalLives, .rxLives, .formularyManagedBy";
                    hide = ".VISN, .macJur, .secName, .employerLives";
                    //hdrTextPharm = "Part D Pharmacy Lives";

                    //same view (V_plan_Info_List_View) will be used for all sections and Product info will be at Lives/Formulary tab , changed by Aditi : 01/11/2011
                    //                if (this.get_clientManager().get_Region()) {
                    //                    show += ",.prodName";                    
                    //                    hdrTextPlanType = "Product Type";
                    //                }
                    //                else
                    //                    hide += ",.prodName";

                    //hide += ",.prodName";

                    break;
                case 20: //FEP
                    show = ".planType, .totalLives, .rxLives, .formularyManagedBy";
                    hide = ".VISN, .macJur, .secName, .employerLives";
                    break;
                case 105:
                case 106:
                    show = "";
                    hide = ".planCity, .planType, .VISN, .macJur, .secName, .employerLives, .totalLives, .rxLives, .formularyManagedBy";
                    break;
                case 107:
                    show = ".planCity, .VISN";
                    hide = ".planType, .macJur, .secName, .employerLives, .totalLives, .rxLives, .formularyManagedBy";
                    break;
                case 108:
                    show = "";
                    hide = ".planCity, .planType, .VISN, .macJur, .secName, .employerLives, .totalLives, .rxLives, .formularyManagedBy";
                    break;
                default: //Custom - hopefully this is custom at this point - either way we will check if first record returned has Covered Lives and/or Pharmacy Lives and hide/show accordingly.  Data Set must return zero instead of Null if lives are supposed to be shown otherwise this won't work
                    show = ".planType";
                    hide = ".planCity, .VISN, .macJur, .secName, .employerLives, .totalLives, .rxLives, .formularyManagedBy";

                    var items = mt.get_dataSource();
                    if (items && items.length > 0)
                        items = items[0];

                    break;

            }
        }
        else
        {
            show = ".totalLives, .rxLives, .formularyManagedBy, .secName";
            hide = ".planType";

            //If VA is in selection, shown VISN
            if ($.inArray(11, channel) > -1)
                show += ", .VISN";
            else
                hide += ", .VISN";

            //If PBM is in selection, show Rx Lives (Employer)
            if ($.inArray(4, channel) > -1)
                show += ", .employerLives";
            else
                hide += ", .employerLives";

            //IF MAC is in selection, show MAC Jurisdiction
            if ($.inArray(16, channel) > -1)
                show += ", .macJur";
            else
                hide += ", .macJur";
        }

        //Hide the state column if a state is selected
        //Also, adjust the width accordingly if the state column is shown or hidden
        var stateWidth;
        if (region)
        {
            $(".planState").addClass("hiddenGrid");
            stateWidth = 0;
        }
        else
        {
            //Only show the state column if previously hidden (fix for row merging)
            $(".planState").removeClass("hiddenGrid");
            stateWidth = 8;
        }

        var c = this.get_masterTableView().getColumnByUniqueName("Total_Covered");
        if (c) $(c.get_element()).find("a").text(hdrText);
        var c = this.get_masterTableView().getColumnByUniqueName("Plan_Type_Name");
        if (c) $(c.get_element()).find("a").text(hdrTextPlanType);
        var c = this.get_masterTableView().getColumnByUniqueName("Total_Pharmacy");
        if (c) $(c.get_element()).find("a").text(hdrTextPharm);
        var c = this.get_masterTableView().getColumnByUniqueName("Rx_Lives");
        if (c) $(c.get_element()).find("a").text(hdrTextRxLives);

        var visible = 0; //Plan Name + State + Website (all others are dynamic)
        if (show == "")
        {
            $(".planState").width("10%");
            if (channel == 105 || channel == 106 || channel == 107 || channel == 108)
            {
                $(".planState").addClass("notmerged");
                $(hide).css("display", "none");
            }

            $(".planName").width("90%");

            grid_resize();

        }
        else
        {
            if (show != "")
                visible = show.split(",").length;
            //This was commented out because we are no longer showing website column
            //visible = visible - 1

            //p is the percentage width used for Name and website columns
            var p = "45%"; //maxed out - all columns hidden other than name, state, and website

            if (visible > 1)
            {

                p = ($(window).width() > 1024 ? 22 : 30);

                //48 is the percentage left after adding Name (22%) + Website (22%) + State (8%)
                //This was commented out because we are no longer showing the website column
                //var dynamicPercent = 100 - ((p * 2) + stateWidth);
                var dynamicPercent = 100 - ((p) + stateWidth);
                $(show).css("display", "").width(Math.ceil(dynamicPercent / visible) + "%");

                p += "%";
            }
            else if (visible == 1)
            {
                p = "39%";
                $(show).css("display", "").width("14%");
            }
            $(hide).css("display", "none");
            if (channel == 105 || channel == 106 || channel == 107)
                $(".planState").width("10%");
            else if (channel > 100 && channel < 105)//reckitt channels
            {
                $(".planState").width("47%");
            }
            else
                $(".planState").width("8%");


            if (channel == 105 || channel == 106 || channel == 108)
                $(".planName").width("80%");
            else if (channel == 107)
            {
                $(".planName").width("60%");
                $(".planCity").width("20%");
                $(".VISN").width("20%");
            }
            else
                $(".planName").width(p);

            //$(".planWebsite").width(p);

            grid_resize();
        }
    }
};
Pathfinder.UI.PlanInfoGridWrapper.registerClass('Pathfinder.UI.PlanInfoGridWrapper', Pathfinder.UI.GridWrapper);




Pathfinder.UI.FormularyDrillDownGridWrapper = function(element)
{
    Pathfinder.UI.FormularyDrillDownGridWrapper.initializeBase(this, [element]);

};

Pathfinder.UI.FormularyDrillDownGridWrapper.prototype =
{
    get_supportsDynamicColumns: function() { return true; },

    get_SortString: function()
    {
        //if Product_Name not visible then don't include in sort
        var sort = this.get_masterTableView().get_sortExpressions().toString();

        if (this._getSelectedChannel() != 17)
        {
            sort = sort.replace(/(,)?Product_Name (ASC|DESC)/ig, "");
            if (sort.indexOf(",") == 0)
                sort = sort.substr(1);
        }

        return sort;
    },

    _onDataBound: function(sender, args)
    {
        Pathfinder.UI.FormularyDrillDownGridWrapper.callBaseMethod(this, '_onDataBound', [sender, args]);

        this.adjustGridColumns();
    },

    _prepDynamicColumns: function()
    {
        Pathfinder.UI.FormularyDrillDownGridWrapper.callBaseMethod(this, '_prepDynamicColumns');

        $(".planName").width("12%");
        $(".geogName").width("11%");
        $(".totalLives").width("8%");
        $(".pharmacyLives").width("8%");
        $(".drugName").width("12%");
        $(".prodName").width("11%");
        $(".formularyName").width("11%");
        $(".formularyLives").width("7%");
        $(".tierName").width("4%");
        $(".copayRange").width("7%");
        $(".paCol").width("3%");
        $(".stCol").width("3%");
        $(".qlCol").width("3%");
    },

    _getSelectedChannel: function()
    {
        return this.get_clientManager().get_Channel();
    },

    adjustGridColumns: function()
    {
        var hide = "";
        var show = "";

        var channel = this._getSelectedChannel();

        switch (channel)
        {
            default:
                $(".prodName").css("display", "none").width(0);
                $(".planName").width("23%");
                $(".tierName").css("display", "").width("4%");

                break;
            case "9": //State Medicaid
                $(".prodName").css("display", "none").width(0);
                $(".tierName").css("display", "none").width(0);
                $(".planName").width("27%");

                break;
            case "17": //Med-D
                $("th.prodName").css("display", "").width("11%");
                $(".prodName[_xcol=false]").css("display", "").width("11%"); //display prod name if not hidden by row merging (_xcol attribute is true if hidden)
                $(".planName").width("12%");
                $(".tierName").css("display", "").width("4%");

                break;
        }

        grid_resize();
    }
};
Pathfinder.UI.FormularyDrillDownGridWrapper.registerClass('Pathfinder.UI.FormularyDrillDownGridWrapper', Pathfinder.UI.GridWrapper);


if (typeof (Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();



//generic helper functions ------------------------------------------------------------------------------------------------------------------------------------------

//Today's Accounts Plan Grid should load the first time page loads.  ClientManager takes care of setting filters and requesting data for initial load.
function onPlanInfoDataBinding(sender, args)
{
    //don't call again
    sender.remove_dataBinding(onPlanInfoDataBinding);

    args.set_cancel(true);
}

function onGridDataBinding(sender, args)
{
    //cancel if no filters set
    args.set_cancel(sender.get_masterTableView().get_filterExpressions().get_count() == 0);
}

//comes from map so it uses a different param list (not sender, args)
function onFilterPlansByVISN(eventName, id)
{
    if (typeof id == "object")
        id = id.get_item().get_value();

    softSet("filterPlans", ["VISN", id, null, "System.Int32"], 150);
}

function onFilterPlansByState(sender, args)
{
    softSet("filterPlans", ["Plan_State", args.get_item().get_value()], 150);
}

function onFilterPlansByCoveredLives(sender, args)
{
    softSet("filterPlans", ["Total_Covered", $.trim(args.get_item().get_value()).split(","), "Between", "System.Int32"], 150);
}

function onFilterPlansByMedicalLives(sender, args)
{
    softSet("filterPlans", ["Medical_Lives", $.trim(args.get_item().get_value()).split(","), "Between", "System.Int32"], 150);
}

function onFilterPlansByRxLives(sender, args)
{
    softSet("filterPlans", ["Rx_Lives", $.trim(args.get_item().get_value()).split(","), "Between", "System.Int32"], 150);
}

//function onFilterPlansByPlanManagedLives(sender, args)
//{
//    softSet("filterPlans", ["Plan_Managed_Lives", $.trim(args.get_item().get_value()).split(","), "Between", "System.Int32"], 150);
//}

//function onFilterPlansByPBMManagedLives(sender, args)
//{
//    softSet("filterPlans", ["PBM_Managed_Lives", $.trim(args.get_item().get_value()).split(","), "Between", "System.Int32"], 150);
//}

function onFilterPlansByEmployerLives(sender, args)
{
    softSet("filterPlans", ["Employer_Lives", $.trim(args.get_item().get_value()).split(","), "Between", "System.Int32"], 150);
}

function onFilterPlansByPharmacyLives(sender, args)
{
    softSet("filterPlans", ["Total_Pharmacy", $.trim(args.get_item().get_value()).split(","), "Between", "System.Int32"], 150);
}

function onFilterPlansByMedicaidLives(sender, args)
{
    softSet("filterPlans", ["Medicaid_Mcare_Enrollment", args.get_item().get_value(), "Between", "System.Int32"], 150);
}

function onFilterPlansByPlanType(sender, args)
{
    softSet("filterPlans", ["Plan_Type_ID", args.get_item().get_value(), null, "System.Int32"], 150);
}

function onFilterPlansBySection(sender, args)
{
    softSet("filterPlans", ["Section_ID", args.get_item().get_value(), null, "System.Int32"], 150);
}



//NOT NEEDED with jQ 1.4.2 - SPH 4/5/2010
//function sessionCheck()
//{
//    try
//    {
//        var service = new Pathfinder.SecurityService();
//        service.IsUserAuthenticated(sessionCheckCallback, sessionCheckError);
//    }
//    catch (ex)
//    {
//        window.location = "content/signout.aspx";
//    }
//}

//function sessionCheckCallback(result, context)
//{
//    if (result === false)
//    {
//        clientManager.validateCurrentUser();
//    }
//}

//function sessionCheckError(result, context)
//{
//    window.location = "content/signout.aspx";
//}


//Soft Set code is used for situations such as handling radio button click events - if code in event handler takes a second or two to execute the browser UI is not updated immediately so 
//radio button appears to be slow or heavy

function softSet(methodName, args, delay)
{
    new cmd(clientManager, methodName, args, delay);
}

var _cmdQueue = {};

function cmd(target, method, args, delay)
{
    ///<summary>cmd object wraps a window's timer call.  It is typically used for menus and options that don't refresh UI quickly enough if a lot of cpu is used to handle page updates.</summary>
    ///<param name="target" type="object">If the function to call is on an object the target parameter must be set to the object instance.  If the function is just a stand alone function set this parameter to null.</param>
    ///<param name="method">If target is specified this parameter must be the name of the function to call.  If target is not specified this parameter must be the function to call.</param>
    ///<param name="args">Arguments to pass to the function call.  If target is a specified an Array of arguments is expected.  If target is not specified the method should expect only a single argument that can be of any type.</param>
    ///<param name="delay">The amound of time (in milliseconds) to wait until the function is executed.</param>

    //    if (target)
    //    {
    //        if (typeof (method) != "string") throw new Error("If 'target' parameter is specified then 'method' parameter must be of type String for the name of the method to call.");
    //    }
    //    else
    //    {
    //        if (typeof (method) != "object") throw new Error("If 'target' parameter is not specified then 'method' parameter must be a function.");
    //    }

    var _target = target;
    var _method = method;
    var _args = args ? args : (_target ? [] : null);
    var _id = window.setTimeout(callCmd, delay);

    _cmdQueue[_id] = this;

    this.cancel = function()
    {
        window.clearTimeout(_id);
        delete (_cmdQueue[_id]);
    }

    function callCmd()
    {
        if (_target)
        {
            _target[_method].apply(_target, _args);
        }
        else //no target so assume function passed in.
        {
            _method(_args);
        }

        if (_id) delete (_cmdQueue[_id]);
    }
}

var __attempts = 0;
function initMap(args)
{
    var m = document[args[0]];

    if (m && m.initializeMap)
    {
        m.initializeMap("areas/maptheme.ashx?s=" + args[1]);
    }
    else
    {
        if (__attempts < 30)
        {
            __attempts++;
            new cmd(null, initMap, args, 500);
        } else
        {
            __attempts = 0;
        }
    }
}


Pathfinder.UI.dataParam = function(name, value, dataType, filterType, src, isExtension)
{
    this.name = name;
    this.value = value;

    if (filterType) this.filterType = filterType;
    if (dataType) this.dataType = dataType;
    if (src) this.src = src;
    if (isExtension) this.isExtension = isExtension;
};
Pathfinder.UI.dataParam.prototype =
{
    get_value: function() { return this.value; },
    set_value: function(value) { this.value = value; },

    get_filterType: function() { return this.filterType; },
    set_filterType: function(value) { this.filterType = value; },

    get_dataType: function() { return this.dataType; },
    set_dataType: function(value) { this.dataType = value; },

    get_name: function() { return this.name; },
    set_name: function(value) { this.name = value; },

    get_src: function() { return this.src; },
    set_src: function(value) { this.src = value; },

    get_isExtension: function() { return this.isExtension; },
    set_isExtension: function(value) { this.isExtension = value; },

    appendValue: function(value, src)
    {
        if ($.isArray(this.value))
            this.value[this.value.length] = value; //insert item to existing
        else
            this.value = [this.value, value]; //convert to array 

        if ($.isArray(this.value) && this.dataType == "System.DateTime")
            this.filterType = "Between"; //force between

        if (src)
        {
            if ($.isArray(this.src))
                this.src[this.src.length] = src;
            else
                this.src = [this.src, src];
        }
    },

    getDefaultSplitChart: function()
    {
        if (this.filterType && this.filterType.toLowerCase() == "between")
            return " ";

        return ","; //default for Custom (used as "In" filter)
    },

    getValuesAsArray: function(splitChar)
    {
        if (!splitChar) splitChar = this.getDefaultSplitChart();

        if (typeof this.value == "string" && this.dataType != "System.String")
        {
            return this.value.split(splitChar);
        }

        return !$.isArray(this.value) ? [this.value] : this.value;
    },

    resetSrc: function()
    {
        var values = this.getValuesAsArray();

        var src = this.get_src();
        if (!$.isArray(src))
            src = [src];

        var o;
        var ctrl;
        var isBool;
        for (var i = 0; i < src.length; i++)
        {
            o = $find(src[i]);
            if (o)
            {
                if (o.get_element().checkboxList || Pathfinder.UI.CheckboxList.isInstanceOfType(o))
                {
                    if (Pathfinder.UI.CheckboxList.isInstanceOfType(o)) ctrl = o;
                    else ctrl = o.get_element().checkboxList;

                    isBool = ctrl.get_booleanOptions();
                    for (var j = 0; j < values.length; j++)
                    {
                        if (!isBool)
                            ctrl.selectItem(values[j]);
                        else
                            ctrl.selectItem(this.name);
                    }
                    //                    $updateCheckboxDropdownText(o);
                }
                else if (o.findItemByValue)
                {
                    o = o.findItemByValue(values[i]);
                    if (o) o.select();
                }
            }
            else
            {
                $("#" + src[i] + "[type=text]").val(values[i]);
                $("#" + src[i] + "[type=checkbox]:not(.chkItem)").each(function() { this.checked = true; });
                $("#" + src[i] + ".chkItem[type=checkbox]").each(function()
                {
                    var p = this.parentNode;
                    while (p && !p.control)
                        p = p.parentNode;

                    if (p)
                        p.control.selectItem(this.id);
                });
            }
        }
    }
};

var _requestIndexes = {};

//TEMP WORKAROUND!!! - REPLACING JQUERY'S parseJSON METHOD - as of version 1.4.1 parseJSON includes a json format check that is breaking ado.net dataservices.  Hopefully with .net 4.0 it will be fixed.  It appears to be a problem in the "__metadata" field that the dataservice is forcing into result set.
jQuery["parseJSON"] = function(data)
{
    if (typeof data !== "string" || !data)
    {
        return null;
    }

    // Make sure leading/trailing whitespace is removed (IE can't handle it)
    data = jQuery.trim(data);

    // Try to use the native JSON parser first
    return window.JSON && window.JSON.parse && ($.browser.msie) ?
				window.JSON.parse(data) :
				(new Function("return " + data))();
};

jQuery.fn.extend(
{
    loadEx: function(url, params, callback, state, hideWhileLoading, forcePOST)
    {
        ///	<summary>
        ///		Loads HTML from a remote file and injects it into the DOM.  By default performs a GET request, but if parameters are included
        ///		then a POST will be performed.
        ///	</summary>
        ///	<param name="url" type="String">The URL of the HTML page to load.</param>
        ///	<param name="data" optional="true" type="Map">Key/value pairs that will be sent to the server.</param>
        ///	<param name="callback" optional="true" type="Function">The function called when the AJAX request is complete.  It should map function(responseText, textStatus, XMLHttpRequest, url, state) such that this maps the injected DOM element.</param>
        ///<param name="state" optional="true" type="Variant">Values that are passed to the callback function.  Typically data that identifies the caller or purpose of the request.</param>
        ///	<returns type="jQuery" />

        if (this.length > 1) throw new Error("loadEx is not intended to be used when loading web pages into multiple elements.  Use 'load' for those requests.");

        var containerID = this.attr("id");

        if (!_requestIndexes[containerID])
            _requestIndexes[containerID] = 1;
        else
            _requestIndexes[containerID]++;

        var rid = _requestIndexes[containerID];

        if (typeof url !== "string")
            return this._load(url);

        var off = url.indexOf(" ");
        if (off >= 0)
        {
            var selector = url.slice(off, url.length);
            url = url.slice(0, off);
        }

        // Default to a GET request
        var type = "GET";

        // If the second parameter was provided
        if (params)
        // If it's a function
            if (jQuery.isFunction(params))
        {
            // We assume that it's the callback
            callback = params;
            params = null;

            // Otherwise, build a param string
        } else if (typeof params === "object")
        {
            params = jQuery.param(params);
            type = "POST";
        }
        else if (forcePOST === true)
        {
            type = "POST";
        }

        var self = this;

        if (hideWhileLoading)
            this.css("visibility", "hidden");

        // Request the remote document
        jQuery.ajax({
            url: url,
            type: type,
            dataType: "html",
            data: params,
            complete: function(res, status)
            {
                if (rid == _requestIndexes[containerID])
                {
                    //if response has __pinsologin hdr need to notify callback so it can be handled
                    var loginhdr = res.getResponseHeader("__pinsologin") == "true";

                    // If successful, inject the HTML into all the matched elements
                    if (status == "success" || status == "notmodified")
                    {
                        var html = res.responseText;

                        //                        //HACK to fix Dundas not allowing us to set wmode param in control - this should be removed if they fix or better work around is found
                        //                        if ($.browser.msie)
                        //                            html = html.replace(/<\/OBJECT>/ig, "<PARAM name='wmode' value='transparent'></OBJECT>");
                        //                        else
                        //                            html = html.replace(/<EMBED/ig, "<EMBED wmode='transparent'");
                        //                        //HACK to fix Dundas not allowing us to set wmode param in control - this should be removed if they fix or better work around is found

                        if (selector)
                        {
                            // inject the contents of the document in, removing the scripts
                            // to avoid any 'Permission Denied' errors in IE
                            html = jQuery("<div/>").append(html.replace(/<script(.|\s)*?\/script>/g, ""))

                            // Locate the specified elements
							.find(selector);
                        }
                        //                        else
                        //                        {
                        //                            // If not, just inject the full result
                        //                            html = res.responseText;
                        //                        }


                        self.html(html);
                    }

                    if (hideWhileLoading)
                        self.css("visibility", "visible");

                    if (callback)
                        self.each(callback, [res.responseText, !loginhdr ? status : "timeout", res, url + "?" + params, containerID, state]);
                }
            }
        });
        return this;
    }
});

/// <summary>Export menu item has been clicked. Redirect to the export handler with the current report and filter set as parameters.</summary>
function onExportMenuItemClicked(sender, eventArgs)
{
    _exportHandler(sender, eventArgs, false);
}

function onExportMenuItemClicked2(sender, eventArgs)
{
    _exportHandler(sender, eventArgs, true);
}

function _exportHandler(sender, eventArgs, confirm)
{
    var item = eventArgs.get_item();
    if (item == null) return;

    var args = item.get_value();
    if (args == null) return;

    args = $.parseJSON(args);

    if (confirm)
        $openWindow("usercontent/confirmexport.aspx?type=" + args.type + (args.module ? "&module=" + args.module : "") + (args.channel ? "&channel=" + args.channel : "") + (args.exportHandler ? "&exportHandler=" + args.exportHandler : ""), null, null, 400, 200, "confirmexp");
    else if (!args.exportHandler)
        clientManager.exportView(args.type, null, args.module, args.channel);
    else
        eval(args.exportHandler + "(" + args.type + ", " + args.module + ")");
}

function about()
{
    try { $openWindow('usercontent/about.aspx', null, null, 600, 300, "about"); }
    catch (ex) { }
}

function terms()
{
    try { $openWindow('content/terms.aspx', null, null, 800, 400, "terms"); }
    catch (ex) { }
}

function disclaimer()
{
    try { $openWindow('content/disclaimer.aspx', null, null, 800, 400, "disclaimer"); }
    catch (ex) { }
}

function changePassword()
{
    try { $openWindow('usercontent/changepassword.aspx', null, null, 350, 225, "changepassword") }
    catch (ex) { }
}

function openPasswordChangeGuide()
{
    var url = 'PasswordGuidelines.aspx';
    window.top.open(url, 'passwordguide', 'top=0,left=0,resizable=1,menubar=0,width=400,height=300,status=0,toolbar=0,location=0,scrollbars=1');
}

function onHelpMenuItemClicked(sender, args)
{
    var val = args.get_item().get_value();
    if (val)
    {
        sender._clicked = false; //have to set _clicked to false otherwise menu reopens automatically or at least opens on mouse over instead of click
        sender.close();
        clientManager[val]();
    }
}

function CheckMaxLength(sender, args)
{
    var maxLength = $(sender).attr("MaxLength");
    maxLength = maxLength ? parseInt(maxLength, 10) : 9999999;
    args.IsValid = !args.Value || args.Value.length <= maxLength;
}

// Account Planning 

Pathfinder.UI.AccountPlanningApplication = function(id)
{
    Pathfinder.UI.AccountPlanningApplication.initializeBase(this, [id]);
};

Pathfinder.UI.AccountPlanningApplication.prototype =
{
    initialize: function(clientManager)
    {
        clientManager.get_ChannelMenuOptions()[this.get_ApplicationID()] = [{ Name: "Account Planning", ID: 401}];

        //The the Channel Menu
        $('#ctl00_main_subheader1_channelMenu').hide();

        Pathfinder.UI.AccountPlanningApplication.callBaseMethod(this, "initialize", [clientManager]);
    },

    get_UrlName: function() { return "custom/" + this.get_clientKey(); },

    get_Title: function() { return "Account Planning"; },

    getChannelUrlName: function(channel)
    {
        return "apl";
    },

    resize: function()
    {
        AccountPlanningApplication_content_resize();

        Pathfinder.UI.AccountPlanningApplication.callBaseMethod(this, 'resize');
    },

    resizeSection: AccountPlanningApplication_section_resize,

    configureChannelMenu: function(clientManager)
    {
        //This is a hack for demo purposes - shouldn't be using channel menu for what is really modules
        //$loadMenuItems(clientManager.get_ChannelMenu(), [], null, 0);
    }
};
Pathfinder.UI.AccountPlanningApplication.registerClass("Pathfinder.UI.AccountPlanningApplication", Pathfinder.UI.BasicApplication);

//function used for populating plan list based on selected channel type
function LoadPlanListByChannelType(sender, args)
{

    var val = sender.get_value();

    if (val == 0) val = null;

    var url = "marketplaceanalytics/services/PathfinderDataService.svc/ParentPlanSet?$filter=Section_ID eq " + val + "&$orderby=Plan_Name";

    $.getJSON(url, null, function(result, status)
    {
        var d = result.d;
        $loadListItems($find('ctl00_partialPage_filtersContainer_AffiliationType_Plan_ID'), d, !d.length ? { value: "", text: "no records available"} : null, 0, 'Parent_ID', 'Plan_Name');


        //after list is loaded check if current Plan ID can be selected
        var data = clientManager.get_SelectionData();
        var planID;
        if (data && data["Parent_ID"]) planID = data["Parent_ID"].value;
        if (planID)
        {
            var li = $find('ctl00_partialPage_filtersContainer_AffiliationType_Plan_ID').findItemByValue(planID);
            if (li) li.select();
        }

    });
}


function onMarketSegmentItemClicked(sender, args)
{
    var values = sender.get_values();
    var query = "";
    var x = $('.searchTextBoxFilter #Plan_Name')[0];
    if (x && x.SearchList)
    {
        x = x.SearchList;

        if (values)
        {
            query = "(";

            if ($.isArray(values))
            {
                for (var i = 0; i < values.length; i++)
                {
                    if (query.length > 1)
                        query += " or ";
                    query += "Section_ID eq " + values[i];
                }
            }
            else
                query += "Section_ID eq " + values;

            query += ")";
        }

        if (query)
            query += " and ";

        query += "substringof('{0}',Name)&$top=50&$orderby=Name";
        x.set_queryFormat("$filter=" + query);
        x.set_queryValues();
    }
}
function SectionLoad(s, a)
{
    var val = s.get_value();

    if (val == 0) val = null;

    var url = "marketplaceanalytics/services/PathfinderClientDataService.svc/MSPlanSearchSet?$filter=Section_ID eq " + val + "&$orderby=Name";

    //Hack to reload selection data for Account Name and Timeframe
    //Copy selection data
    var data = jQuery.extend(true, {}, clientManager.get_SelectionData());
    var secID;
    //Section_ID must be removed or else it will trigger 'SectionLoad'
    if (data && data["Section_ID"])
    {
        secID = data["Section_ID"].value;
        delete (data["Section_ID"]);
    }

    $.getJSON(url, null, function(result, status)
    {
        var d = result.d;
        $loadListItems($find('ctl00_partialPage_filtersContainer_FilterChannelPlanSelection_Plan_ID1'), d, !d.length ? { value: "", text: "no records available"} : null, 0, 'ID', 'Name');
        $loadListItems($find('ctl00_partialPage_filtersContainer_FilterChannelPlanSelection_Plan_ID2'), d, !d.length ? { value: "", text: "no records available"} : null, 0, 'ID', 'Name');

        //Don't reload if SectionID is different
        if (secID == s.get_value())
            $reloadContainer("moduleOptionsContainer", data);
    });

    //    //Load timeframe droplist based on section selected
    //    if (val == 17)
    //        url = "marketplaceanalytics/services/MarketplaceDataService.svc/FHMonthYearsSet?$orderby=Data_Year desc,Data_Month desc";
    //    else
    //        url = "marketplaceanalytics/services/MarketplaceDataService.svc/FHQuarterYearsSet?$orderby=Data_Year desc,Data_Quarter desc";

    //    $.getJSON(url, null, function(result, status)
    //    {
    //        var d = result.d;
    //        var timeframe_id = $get("Timeframe_ID");

    //        var items = new Array();

    //        for (var i in d)
    //        {
    //            items.push(d[i].ID);
    //        }
    //        clientManager.setContextValue("ma_fhTimeframes", items);

    //        if (timeframe_id)
    //        {
    //            var timeframe_id_control = timeframe_id.control;
    //            if (timeframe_id_control)
    //                $loadPinsoListItems(timeframe_id_control, d, { 'ID': 0, 'Name': 'All' }, -1); //  plan_id_control.dispose();                    
    //        }
    //        else //Create Timeframe list
    //        {
    //            $createCheckboxDropdown("ctl00_partialPage_filtersContainer_FilterTimeframe_Timeframe", "Timeframe_ID", null, { 'defaultText': 'All', 'multiItemText': '--Change Selection--' }, null, "moduleOptionsContainer");
    //            $loadPinsoListItems($find('Timeframe_ID'), d, { 'ID': 0, 'Name': 'All' }, -1);
    //        }

    //        $updateCheckboxDropdownText("ctl00_partialPage_filtersContainer_FilterTimeframe_Timeframe", "Timeframe_ID");

    //        timeframe_id = $get('Timeframe_ID');

    //        if (timeframe_id.control)
    //        {
    //            timeframe_id.control.reset();
    //        }

    //        //Don't reload if SectionID is different
    //        if (secID == s.get_value())
    //            $reloadContainer("moduleOptionsContainer", data);
    //    });
}

function refreshFhrPlanSelectionList(sectionID)
{
    var x = $('.searchTextBoxFilter #Plan_ID1')[0];
    if (x && x.SearchList)
    {
        x = x.SearchList;

        query = "Section_ID eq " + sectionID;
        query += " and substringof('{0}',Name)&$top=50&$orderby=Name";

        x.set_queryFormat("$filter=" + query);
        x.set_queryValues();
    }


    x = $('.searchTextBoxFilter #Plan_ID2')[0];
    if (x && x.SearchList)
    {
        x = x.SearchList;

        query = "Section_ID eq " + sectionID;
        query += " and substringof('{0}',Name)&$top=50&$orderby=Name";

        x.set_queryFormat("$filter=" + query);
        x.set_queryValues();
    }
}

function SectionReset(s, a)
{
    var data = clientManager.get_SelectionData();
    if (data && data['Section_ID'])
    {
        if (data['Section_ID'].value != s.get_value())
        {
            var timeframe_id = $get('Timeframe_ID');

            if (timeframe_id.control)
            {
                timeframe_id.control.reset();
                $updateCheckboxDropdownText("ctl00_partialPage_filtersContainer_FilterTimeframe_Timeframe", "Timeframe_ID");
            }

            var plan1 = $find('ctl00_partialPage_filtersContainer_FilterChannelPlanSelection_Plan_ID1');
            var plan2 = $find('ctl00_partialPage_filtersContainer_FilterChannelPlanSelection_Plan_ID2');

            if (plan1)
            {
                plan1.clearSelection();
                plan1.trackChanges();
                plan1.get_items().getItem(0).select();
                plan1.commitChanges();
            }

            if (plan2)
            {
                plan2.clearSelection();
                plan2.trackChanges();
                plan2.get_items().getItem(0).select();
                plan2.commitChanges();
            }
        }
    }
}

