/// <reference name="MicrosoftAjax.js"/>
/// <reference path="~/content/scripts/jquery-1.4.1-vsdoc.js"/>
/// <reference path="~/content/scripts/ui.js"/>
/// <reference path="~/content/scripts/clientManager.js"/>


//Restrictions Report starts
Pathfinder.UI.DeyRestrictionsReportApplication = function(id)
{
    Pathfinder.UI.DeyRestrictionsReportApplication.initializeBase(this, [id]);
};

Pathfinder.UI.DeyRestrictionsReportApplication.prototype =
{
    get_UrlName: function() { return "custom/" + this.get_clientKey() + "/restrictionsreport"; },

    get_Title: function() { return "Report Filters"; },

    getUrl: function(channelName, module, pageName, hasData, isCustom)
    {
        channelName = "all";
        //hasData = false;

        return Pathfinder.UI.DeyRestrictionsReportApplication.callBaseMethod(this, 'getUrl', [channelName, module, pageName, hasData, isCustom]);
    },
    get_OptionsServiceUrl: function(clientManager)
    {
        return "custom/Dey/restrictionsreport/services/PathfinderDataService.svc" + "/GetRestrictionsReportOptions";
    },

    getDefaultModule: function(clientManager)
    {
        return "deyrestrictionsreport";
    },

    get_ModuleOptionsUrl: function(clientManager)
    {
        return this.getUrl("all", null, clientManager.get_Module() + "_filters.aspx", false);
    },

    activate: function(clientManager)
    {
        if (!this._moduleChangingDelegate)
        {
            this._moduleChangingDelegate = Function.createDelegate(this, this._moduleChanging);
            clientManager.add_moduleChanging(this._moduleChangingDelegate);
        }

        Pathfinder.UI.DeyRestrictionsReportApplication.callBaseMethod(this, "activate", [clientManager]);
    },

    onModuleChanging: function(cm, newModule)
    {
        if (cm.get_UIReady())
        {
            cm.clearSelectionData(true);
        }
    },

    _moduleChanging: function(sender, args)
    {
        this.onModuleChanging(sender, args.get_Value());
    },

    resize: function()
    {
        standardreports_content_resize();

       Pathfinder.UI.DeyRestrictionsReportApplication.callBaseMethod(this, 'resize');
    },

    resizeSection: function()
    {
        standardreports_section_resize();
        if ($.browser.version == "7.0")
        {
            $(".rmLink").css({ width: "195px" });
        }
        Pathfinder.UI.DeyRestrictionsReportApplication.callBaseMethod(this, 'resizeSection');
    }

};

Pathfinder.UI.DeyRestrictionsReportApplication.registerClass("Pathfinder.UI.DeyRestrictionsReportApplication", Pathfinder.UI.BasicApplication);
//Restrictions Report Ends

//Formulary History Reporting starts
Pathfinder.UI.DeyFormularyHistoryReportingApplication = function(id)
{
    Pathfinder.UI.DeyFormularyHistoryReportingApplication.initializeBase(this, [id]);
};

Pathfinder.UI.DeyFormularyHistoryReportingApplication.registerClass("Pathfinder.UI.DeyFormularyHistoryReportingApplication", Pathfinder.UI.FormularyHistoryReportingApplication);

//Formulary History Reporting ends


//Formulary Sell Sheets business rules ---------------------------------------------------------------------------------------------------------------------------------------------
Pathfinder.UI.DeyFormularySellSheetsApplication = function(id)
{
    Pathfinder.UI.DeyFormularySellSheetsApplication.initializeBase(this, [id]);
};
//Pathfinder.UI.UnitedtheraFormularySellSheetsApplication.prototype =
//{
//    createSellSheet: function() {
//        //Clear the plan selection for the first time.
//        clientManager.setContextValue("ssSelectedPlansList");

//        var dt = new Date();
//        dt = "'" + encodeURIComponent(dt.localeFormat("d") + " " + dt.localeFormat("t")) + "'";

//        $.getJSON("custom/unitedthera/sellsheets/services/UnitedTheraDataService.svc/CreateSellSheet?Created=" + dt, null, this._onCreateCallbackDelegate);
//    }
//};
Pathfinder.UI.DeyFormularySellSheetsApplication.registerClass("Pathfinder.UI.DeyFormularySellSheetsApplication", Pathfinder.UI.FormularySellSheetsApplication);

//Formulary Sell Sheets ends

//class template selection in sell sheet validation-- starts
function validateDrugSelection(drugTextControlID, txtRequiredDrugSelected)
{
    var thera_id = null;
    var SeldrugIDs = null;
    txtRequiredDrugSelected.val("");

    if ($find(theraCtrlID) != null)
    {
        thera_id = $find(theraCtrlID).get_value();
        //if (thera_id > 0) 
        // txtRequiredDrugSelected.val("true");
    }

    if (drugCtrlID && drugCtrlID.control)
        SeldrugIDs = $get(drugCtrlID).control.get_element().checkboxList.get_values();
    else
        SeldrugIDs = drugTextControlID.val();

    if ($.isArray(SeldrugIDs))
        SeldrugIDs = SeldrugIDs.join(",");

    if (thera_id != null && SeldrugIDs != null)
    {
        var url = "services/PathfinderService.svc" + "/Client_App_Drug_ListSet?$filter=TherapeuticClassID eq " + thera_id + " and ClientID eq 59 and App_ID eq 8 and IsDrugSelected eq true";
        txtRequiredDrugSelected.val("");
        $.getJSON(url, null, function(result, status)
        {
            var d = result.d;
            var count = 0;
            if (d.length > 0)
            {
                for (var i = 0; i < d.length; i++)
                {
                    if (SeldrugIDs.indexOf(d[i].ID) != -1)
                        count++;
                }

                if (count > 0)
                    txtRequiredDrugSelected.val("true");
                else
                    txtRequiredDrugSelected.val("false");
            }
        });
    }

}

//class template selection in sell sheet validation-- ends
