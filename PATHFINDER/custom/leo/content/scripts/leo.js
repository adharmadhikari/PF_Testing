
Pathfinder.UI.LeoReportsApplication = function(id)
{
    Pathfinder.UI.LeoReportsApplication.initializeBase(this, [id]);
};

Pathfinder.UI.LeoReportsApplication.prototype =
{
    get_UrlName: function() { return "custom/" + this.get_clientKey() + "/reports"; },

    getDefaultModule: function() { return "toppayersbystate"; },
    
    getUrl: function(channelName, module, pageName, hasData, isCustom)
    {
        hasData = true;

        return Pathfinder.UI.LeoReportsApplication.callBaseMethod(this, 'getUrl', [channelName, module, pageName, hasData, isCustom]);
    },
    
    configureDashboardTiles: function(clientManager)
    {
        $(".todaysAccounts1").hide();
        
        Pathfinder.UI.LeoReportsApplication.callBaseMethod(this, 'configureDashboardTiles', [clientManager]);
    },

    resetDashboardTiles: function(clientManager)
    {
        $(".todaysAccounts1").show();
        
        Pathfinder.UI.LeoReportsApplication.callBaseMethod(this, 'resetDashboardTiles', [clientManager]);
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

        Pathfinder.UI.LeoReportsApplication.callBaseMethod(this, 'resize');
    },

    resizeSection: function()
    {
        var height = getWorkspaceHeight();

        $("#divTile3, #divTile3Container").height(height);

        $("#divTile3").height(height - $("#divTile3Container .tileContainerHeader").height() - 45);
    }
};
Pathfinder.UI.LeoReportsApplication.registerClass("Pathfinder.UI.LeoReportsApplication", Pathfinder.UI.BasicApplication);

