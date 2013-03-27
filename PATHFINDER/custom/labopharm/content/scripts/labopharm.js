/// <reference name="MicrosoftAjax.js"/>
/// <reference path="~/content/scripts/jquery-1.4.1-vsdoc.js"/>
/// <reference path="~/content/scripts/ui-vsdoc.js"/>
/// <reference path="~/content/scripts/clientManager-vsdoc.js"/>


//Formulary Sell Sheets business rules ---------------------------------------------------------------------------------------------------------------------------------------------
Pathfinder.UI.LabopharmFormularySellSheetsApplication = function(id)
{
    Pathfinder.UI.LabopharmFormularySellSheetsApplication.initializeBase(this, [id]);
};
//Pathfinder.UI.LabopharmFormularySellSheetsApplication.prototype =
//{
//    createSellSheet: function()
//    {
//        //Clear the plan selection for the first time.
//        clientManager.setContextValue("ssSelectedPlansList");

//        var dt = new Date();
//        dt = "'" + encodeURIComponent(dt.localeFormat("d") + " " + dt.localeFormat("t")) + "'";

//        $.getJSON("custom/Labopharm/sellsheets/services/LabopharmDataService.svc/CreateSellSheet?Created=" + dt, null, this._onCreateCallbackDelegate);
//    }
//};
Pathfinder.UI.LabopharmFormularySellSheetsApplication.registerClass("Pathfinder.UI.LabopharmFormularySellSheetsApplication", Pathfinder.UI.FormularySellSheetsApplication);


//Business Planning business rules ---------------------------------------------------------------------------------------------------------------------------------------------
Pathfinder.UI.LabopharmBusinessPlanningApplication = function(id)
{
    Pathfinder.UI.LabopharmBusinessPlanningApplication.initializeBase(this, [id]);
};
Pathfinder.UI.LabopharmBusinessPlanningApplication.prototype =
{
    get_UrlName: function() { return "custom/" + this.get_clientKey() + "/businessplanning"; },

    get_Title: function() { return ""; },

    getUrl: function(channelName, module, pageName, hasData, isCustom)
    {

        channelName = "all";

        return Pathfinder.UI.LabopharmBusinessPlanningApplication.callBaseMethod(this, 'getUrl', [channelName, module, pageName, hasData, isCustom]);
    },
    get_OptionsServiceUrl: function(clientManager)
    {
        return this.get_ServiceUrl() + "/GetBusinessPlanningOptions";
    },

    getDefaultModule: function(clientManager)
    {
        return "businessplanning";
    },

    get_ModuleOptionsUrl: function(clientManager)
    {
        //        if (clientManager.get_Module() == "businessplanning")
        return this.getUrl("all", null, "filters.aspx", false);
        //        else
        //            return null;
    },

    _prepForAnimation: function()
    {
        $(".rightBPTile, .leftBPTile").width("24.5%");
        $(".pbmTile").width("70%");
        $(".sppTile").width("29.5%");
    },

    collapseSidePanel: function()
    {
        this._prepForAnimation();

        minTile2(this.resizeSection);
    },
    expandSidePanel: function()
    {
        this._prepForAnimation();

        maxTile2(this.resizeSection);
    },

    resize: function()
    {
        var browserWindow = $(window);
        var divHeight = browserWindow.height();
        var divWidth = browserWindow.width();
        var tile2Height = safeSub(divHeight, 105);
        var collaspeLft = $(".todaysAccounts2Expand").height();

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


        Pathfinder.UI.LabopharmBusinessPlanningApplication.callBaseMethod(this, 'resize');
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
            $(".section2SR .enlarge").show();
        $("#maxTChart .enlarge, #maxSRMap .enlarge, #maxTBtm .enlarge, #maxChart .enlarge, #maxSRTile4 .enlarge, #maxSRTile5 .enlarge").hide();
        if (hdrElement.length > 0)
        {
            height = Sys.UI.DomElement.getBounds(hdrElement[0]).height;
        }
        //Tile 3 Properties (if Tile4 & 5 exist statement)
        var tile3Height = divHeight - 131;

        if (ie6)
        {
            $("#tile3 #divTile3Container ").css({ height: tile3Height });
        }

        $("#tile3 #divTile3, #tile3SR #divTile3").css({ height: tile3Height, textAlign: "center", width: "auto" });

        $(".section2SR #tile3 #divTile3 .rgDataDiv").css({
            height: divHeight - 166 - $(".formularyUpdateDate").height()
        });

        $("#tile3T #divTile3").css({ height: tile2Height });

        var bpPlanInfo = $get("bpPlanInfo");
        if (bpPlanInfo)
        {
            var rect = Sys.UI.DomElement.getBounds(bpPlanInfo);
            $(bpPlanInfo).find("#divTile3").height(tile3Height - rect.y + 70);
        }

        $("#tile3").removeClass("leftTile");
        $(".todaysAccounts1").css({
            padding: "0px",
            position: "relative"
        });

        var tile3Width = $("#MainBPSection").width();
        var qtr = safeSub((tile3Width / 4), 4.5);
        $(".rightBPTile, .leftBPTile").width(qtr);

        var w1 = tile3Width * .7;
        $(".pbmTile").width(w1);
        $(".sppTile").width(safeSub(tile3Width - w1, 6));

        $(".rightMedTile .rgMasterTable").css("width", "");

        if (ie6 && this.resizeSection)
        {
            $("#ie6hackdiv").remove();
            $(document.body).append("<div id='ie6hackdiv' style='position:absolute;display:none'></div>");
            
            $("#moduleOptionsContainer").hide().show(); //ie6 hacking because report filters disappear even though all styles indicate they should be visible.            
        }
        //        //clears Telerik computed width in the headers for the data table
        //        //setTimeout("resetGridHeaders()", 1500);
        //        resetGridHeadersX(500);
    }
};
Pathfinder.UI.LabopharmBusinessPlanningApplication.registerClass("Pathfinder.UI.LabopharmBusinessPlanningApplication", Pathfinder.UI.BasicApplication);


//END Business Planning


function openBPUploadwindow()
{
    var bpid = $("#ctl00_Tile3_BP_ID").val();
    $openWindow("custom/Labopharm/businessplanning/all/upload.aspx?BP_ID=" + bpid, null, null, 400, 200, "Popupwindow");
}

//opens a pop-up window for removing selected medical policy document
function openBPDeletewindow()
{
    var bpid = $("#ctl00_Tile3_BP_ID").val();
    var mpid = $find("ctl00_Tile3_MedicalPolicy1_RadGridbppOLICY").get_masterTableView().get_selectedItems()[0]._dataItem.Medical_Policy_ID;
    var mpname = $find("ctl00_Tile3_MedicalPolicy1_RadGridbppOLICY").get_masterTableView().get_selectedItems()[0]._dataItem.Medical_Policy_Name;
    $openWindow("custom/Labopharm/businessplanning/all/RemoveMedPolicyDoc.aspx?BP_ID=" + bpid + "&MP_ID=" + mpid + "&MP_Name=" + mpname, null, null, 500, 110, "RemoveMPPopupwindow");
}


function BP_pageInitialized(sender, args)
{
    $("#section2 .RadComboBox").each(ConfigComboBoxes);

    var mode;
    //check formview's current mode and show and hide Edit and Save links based on that.
    mode = $("#ctl00_Tile3_AddEditBP1_frmvmMode").val();

    var hdnSection_ID = $("#ctl00_Tile3_hdnSection_ID").val();
    var hdnreport = $("#ctl00_Tile3_hdnreport").val();
    var hdnExport = $("#ctl00_Tile3_hdnExport").val();

    //If "Medicare Carrier (A & B)" section is selected 
    //then synchronise Hidden varible("MedCEnrollment") from AddEditBusinessPlanning.ascx AND
    //Label/Textbox control from CoveredLives_MC.ascx
    var hdnval = $("#ctl00_Tile3_AddEditBP1_formVWBP_MedCEnrollment").val();

    
    if (mode == "readonly")
    {
        //If "Medicare Carrier (A & B)" section is selected 
        if (hdnSection_ID == "2")
        {
            //synchronise Hidden varible("MedCEnrollment") from AddEditBusinessPlanning.ascx AND
            //Label control from CoveredLives_MC.ascx
            $("#ctl00_Tile3_InfoLives1_CLlivesMC_MedCarrierABlbl").text(hdnval);
        }
    }
    else if (mode == "edit")
    {
        //If "Medicare Carrier (A & B)" section is selected
        if (hdnSection_ID == "2")
        {
            //synchronise Hidden varible("MedCEnrollment") from AddEditBusinessPlanning.ascx AND
            //Textbox control from CoveredLives_MC.ascx
            $("#ctl00_Tile3_InfoLives1_CLlivesMC_MedCarrierABtxt").val(hdnval);
        }
    }

    clientManager.add_formSubmitted(_onSaveCallback);

    //Show medical policy documents with status = 1(Active documents) 
    //deleted documents will have status = 2.
    SetMedPolicyFilter();

    var gm = $find("ctl00_Tile3_MedicalPolicy1_RadGridbppOLICY$GridWrapper");
    if (gm)
       gm.add_recordCountChanged(RadGridbppOLICY_onRecordCount);
}


function SetMedPolicyFilter()
{
    var grid = $find("ctl00_Tile3_MedicalPolicy1_RadGridbppOLICY$GridWrapper");
    if (grid)
    {
        //Show medical policy documents with status = 1(Active documents) 
        //deleted documents will have status = 2.
        $setGridFilter(grid, "Med_Policy_Status_ID", 1, "EqualTo", "System.Int32");
        grid.dataBind();
    }
}

//When clicked on Medical Policy grid - Comments section it opens a popup window containing Document Details.
function OpenMedPolicyDetails(Medical_Policy_ID, Type, x, y, width, height)
{
    var app = clientManager.get_ApplicationManager();
    var url = app.getUrl("all", clientManager.get_Module(), "OpenDocDetails.aspx");
    url = url + "?Medical_Policy_ID=" + Medical_Policy_ID + "&Type=" + Type;

    var mt = $get("ctl00_Tile3_MedicalPolicy1_RadGridbppOLICY").control.get_masterTableView();
    var cell;

    //Get the list of dataitems which matches the selected Medical_Policy_ID.
    var list = $.grep(mt.get_dataItems(), function(i) { if (i.get_dataItem()) return i.get_dataItem().Medical_Policy_ID == Medical_Policy_ID; else return false; }, false);

    //Get the cellIndex for selected cell.
    if (list && list.length > 0)
    {
        var col;
        var rect;

        if (Type == "comments")
        {
            col = mt.getColumnByUniqueName("Comments");
        }

        if (col)
        {
            cell = list[0].get_element().cells[col.get_element().cellIndex];
        }
        else
            throw new Error("Cannot find column 'Comments'");
    }

    //Getting cell bounds.
    rect = Sys.UI.DomElement.getBounds(cell);

    //Open pop-up window with calculated co-ordinates.
    clientManager.openViewer(url, rect.x - width, rect.y, width, height);
}  

function BP_pageUnloaded(sender, args)
{
    clientManager.unloadPage("bpPlanInfo");
    clientManager.remove_pageLoaded(BP_pageInitialized);
    clientManager.remove_pageUnloaded(BP_pageUnloaded);
    clientManager.remove_formSubmitted(_onSaveCallback);

    gm = $find("ctl00_Tile3_MedicalPolicy1_RadGridbppOLICY$GridWrapper");
    if (gm)
        gm.remove_recordCountChanged(RadGridbppOLICY_onRecordCount);
}

//To Initialize Business Plans Report page.
function BP_Report_pageInitialized(sender, args)
{
    //Show medical policy documents with status = 1(Active documents) 
    //deleted documents will have status = 2.
    SetMedPolicyFilter();
}

function RadGridbppOLICY_onDataBound(sender, args)
{
    AddLink();
}

//Code for adding comments link which will be displayed for each record in the Medical Policy grid.
//It adds an information icon in the last grid column. Hovering over the icon will display document details. 
function AddLink()
{
    var grid = $get(gridBPMedPolicyID).control;
         var masterTable = grid.get_masterTableView();

         for (var i = 0; i <= (masterTable.get_dataItems().length - 2); i++) {
             var cell;
             var href; //href for link to reassign to img tag of comments
             var j; //temp jQuery object
             
             var item = masterTable.get_dataItems()[i];
             var dataItem = item.get_dataItem();
             
             if (dataItem != null)
             {
                //Comments
                 cell = item.get_element().cells[2];
                 j = $(cell).find("a");
                 if (j.length)
                 {
                        href = j.attr("href").replace("javascript:", "");
                        j.html("<img src='content/images/information.png' onmouseout=\"$('#infoPopup').hide()\" onmouseover='" + href + "' />");
                 }
             }
         }
}

//To unload Business Plans Report page
function BP_Report_pageUnloaded(sender, args)
{
    clientManager.remove_pageLoaded(BP_Report_pageInitialized);
    clientManager.remove_pageUnloaded(BP_Report_pageUnloaded);
}

function RadGridbppOLICY_onRecordCount(sender, args)
{
    var mt = sender.get_masterTableView();

    //If there are no records in medical policy grid then disable delete link
    //else enable it and select first record.
    if (mt.get_virtualItemCount() == 0)
    {
        //disable Delete link.
        if($get("ctl00_Tile3_MedicalPolicy1_A2"))
            $get("ctl00_Tile3_MedicalPolicy1_A2").style.display = "none";
        
        if($get("ctl00_Tile3_MedicalPolicy1_separator31"))    
            $get("ctl00_Tile3_MedicalPolicy1_separator31").style.display = "none";
    }
    else
    {
        //If user has alignment then 'Delete' link will be visible. Only in the case select first row else not.
        if ($get("ctl00_Tile3_MedicalPolicy1_A2"))
            mt.selectItem(0);

        if ($get("ctl00_Tile3_MedicalPolicy1_A2"))
            //Enable delete link.
            $get("ctl00_Tile3_MedicalPolicy1_A2").style.display = "";
            
        if($get("ctl00_Tile3_MedicalPolicy1_separator31"))    
            $get("ctl00_Tile3_MedicalPolicy1_separator31").style.display = "";
    }
}

function ExportBP()
{
    var clientKey = clientManager.get_ClientKey();

    var randomnumber = Math.random() * 101;

    var Plan_ID = $("#ctl00_Tile3_hdnPlan_ID").val();
    var Section_ID = $("#ctl00_Tile3_hdnSection_ID").val(); 
    var Thera_ID = $("#ctl00_Tile3_hdnThera_ID").val(); 

    var str = "custom/" + clientKey + "/businessplanning/all/GenerateBPPDF.aspx?Plan_ID=" + Plan_ID + "&Section_ID=" + Section_ID + "&Thera_ID=" + Thera_ID + "&report=2&Export=1&rnd=" + randomnumber;

    window.open(str);
}

function EditBP()
{
    //Call forview edit button click event.
    $("#ctl00_Tile3_AddEditBP1_formVWBP_Editbtn").click();

    //This is done is codebehind
    //Hide Edit link and show Save link.
    //     $get("ctl00_Tile3Tools_EditBP").style.display = "none";
    //     $get("ctl00_Tile3Tools_separator1").style.display = "none";
    //     $get("ctl00_Tile3Tools_SaveBP").style.display = "";
    //     $get("ctl00_Tile3Tools_separator2").style.display = "";
}


//To configure Other_Restrictions(PA/QL/ST) combobox.
function ConfigComboBoxes()
{
    clientManager.registerComponent(this.id);

    if ($(this).hasClass("restrictions"))
    {
        var items = [{ ID: "PA", Value: "PA", Name: "PA" }, { ID: "QL", Value: "QL", Name: "QL" }, { ID: "ST", Value: "ST", Name: "ST"}];

        //Create an id, which will be used for checkboxlistcombobox, which will be unique per combobox
        var chkboxlistid = this.id + "_Restrictions";

        //Create and load restrictions list, with default caption as 'No Selection'.
        $createCheckboxDropdown(this.id, chkboxlistid, null, { 'defaultText': '--No Selection--' }, null, null);

        //Load items in the combobox with nothing selected for the first time. So -1 is passed as last parameter.
        $loadPinsoListItems(chkboxlistid, items, null, -1)

        //Find the hidden variable which is ending with "_SelectedRestrictions" string.
        //Which is created to store the selected values for restrictions combobox
        //For ex: x = "PA, ST" or "PA, QL, ST" etc
        var x = $(this).parent().find("input[id$='_SelectedRestrictions']").val();

        if (x)
        {
            x = x.split(",");
            var restrictions = $find(chkboxlistid);

            //Select specific item in the restrictions combobbox for each item in x.
            for (var s in x)
            {
                restrictions.selectItem($.trim(x[s]));
            }
        }

        //Update the caption which appears up top in the combobox based on the checkbox selections.
        $updateCheckboxDropdownText(this.id, chkboxlistid);
    }

}

//Gets selected values for each restrictions combobox and sets a hidden variable for each.
function GetComboBoxValues()
{
    //This code is executed only for Restrictions combobox.
    if ($(this).hasClass("restrictions"))
    {
        //Get the handle for checkbox control which is inside restrictions combobox.
        var restrictions = $find(this.id + "_Restrictions");

        //get_values() function from components.js returns selected checkbox values as follows:
        //returns null if nothing is selected.
        //or returns a string if only one value is selected.
        //or returns an array if multiple values are selected.
        var x = restrictions.get_values();

        //This field will be set based on checkbox selection.
        var hdnfld = $(this).parent().find("input[id$='_SelectedRestrictions']");

        if (x)
        {
            if ($.isArray(x))
            //if x is na array then join it with comma and set the hidden variable accordingly.
                hdnfld.val(x.join(","));
            else
            //if x is not an array that means a string is returned so set hidden variable accordingly.
                hdnfld.val(x);
        }
        else
        {
            //if x is null that means nothing is selected so set hidden variable to blank.
            hdnfld.val("");
        }
    }
}

//This is called after the form is submitted.
function _onSaveCallback(sender, args)
{
    //if form submit is successful then toggle the links.
    if (args.result.Success)
    {
        //This is done in code behind.
        //Hide Save link and show Edit link.
        //         $get("ctl00_Tile3Tools_EditBP").style.display = "";
        //         $get("ctl00_Tile3Tools_separator1").style.display = "";
        //         $get("ctl00_Tile3Tools_SaveBP").style.display = "none";
        //         $get("ctl00_Tile3Tools_separator2").style.display = "none";
    }
    else
    {
        //alert("Error: To be completed.");
    }
}

function SaveBP()
{
    $("#section2 .RadComboBox").each(GetComboBoxValues);

    //If "Medicare Carrier (A & B)" section is selected 
    //then synchronise Hidden varible("MedCEnrollment") from AddEditBusinessPlanning.ascx AND
    //Textbox control from CoveredLives_MC.ascx
    //Copy data from textbox onto hidden variable for saving it to DB.
    var txtval = $("#ctl00_Tile3_InfoLives1_CLlivesMC_MedCarrierABtxt").val();
    $("#ctl00_Tile3_AddEditBP1_formVWBP_MedCEnrollment").val(txtval);

    //Clean up textbox data before saving it to database.
    var Thera_ID = $("#ctl00_Tile3_hdnThera_ID").val();
    if (Thera_ID == "1")
    {
        $("#ctl00_Tile3_AddEditBP1_formVWBP_AccountOverview1").val(cleanTextData($("#ctl00_Tile3_AddEditBP1_formVWBP_AccountOverview1").val()));
        $("#ctl00_Tile3_AddEditBP1_formVWBP_CurrentStatus1").val(cleanTextData($("#ctl00_Tile3_AddEditBP1_formVWBP_CurrentStatus1").val()));
        $("#ctl00_Tile3_AddEditBP1_formVWBP_Issues1").val(cleanTextData($("#ctl00_Tile3_AddEditBP1_formVWBP_Issues1").val()));
        $("#ctl00_Tile3_AddEditBP1_formVWBP_Strategies1").val(cleanTextData($("#ctl00_Tile3_AddEditBP1_formVWBP_Strategies1").val()));
        $("#ctl00_Tile3_AddEditBP1_formVWBP_Tactics1").val(cleanTextData($("#ctl00_Tile3_AddEditBP1_formVWBP_Tactics1").val()));
        $("#ctl00_Tile3_AddEditBP1_formVWBP_MedPolicyDev1").val(cleanTextData($("#ctl00_Tile3_AddEditBP1_formVWBP_MedPolicyDev1").val()));
        $("#ctl00_Tile3_AddEditBP1_formVWBP_PandT1").val(cleanTextData($("#ctl00_Tile3_AddEditBP1_formVWBP_PandT1").val()));
    }
    else if (Thera_ID == "2")
    {
        $("#ctl00_Tile3_AddEditBP1_formVWBP_Issues2").val(cleanTextData($("#ctl00_Tile3_AddEditBP1_formVWBP_Issues2").val()));
        $("#ctl00_Tile3_AddEditBP1_formVWBP_Strategies2").val(cleanTextData($("#ctl00_Tile3_AddEditBP1_formVWBP_Strategies2").val()));
        $("#ctl00_Tile3_AddEditBP1_formVWBP_Tactics2").val(cleanTextData($("#ctl00_Tile3_AddEditBP1_formVWBP_Tactics2").val()));
        $("#ctl00_Tile3_AddEditBP1_formVWBP_Notes2").val(cleanTextData($("#ctl00_Tile3_AddEditBP1_formVWBP_Notes2").val()));
    }
    
    $("#ctl00_Tile3_AddEditBP1_formVWBP_Savebtn").click();
}

//Clean up word data into plain text
var swapCodes = new Array(8211, 8212, 8216, 8217, 8220, 8221, 8226, 8230); // dec codes from char at
var swapStrings = new Array("--", "--", "'", "'", "\"", "\"", "*", "...");
function cleanTextData(input)
{
    // debug for new codes
    // for (i = 0; i < input.length; i++)  alert("'" + input.charAt(i) + "': " + input.charCodeAt(i));    
    var output = input;
    for (i = 0; i < swapCodes.length; i++)
    {
        var swapper = new RegExp("\\u" + swapCodes[i].toString(16), "g"); // hex codes
        output = output.replace(swapper, swapStrings[i]);
    }
    return output;
    
}

// Used to get the Business Plan data for an account
function gridReport_OnRowSelected(sender, args)
{

    var data = args.get_gridDataItem().get_dataItem();
    var planID = data["Plan_ID"];
    var theraID = data["Thera_ID"];
    var sectionID = data["Section_ID"];

    clientManager.add_pageLoaded(onPlanInfoLoaded, "bpPlanInfo");
    clientManager.loadPage("custom/Labopharm/businessplanning/all/businessplanning.aspx?report=2&Plan_ID=" + planID + "&Thera_ID=" + theraID + "&Section_ID=" + sectionID, "bpPlanInfo");
}

function onPlanInfoLoaded(sender, args)
{
    clientManager.remove_pageLoaded(onPlanInfoLoaded, "bpPlanInfo");
    sender.get_ApplicationManager().resizeSection();
}

//Used to remove the previous Business Plan data when new filter items are selected
function gridReport_OnDataBound(sender, args) 
{    
    //alert("Grid: " + sender.get_id());
    clientManager.unloadPage("bpPlanInfo");
}

function onListChanging(sender, args)
{
    if (sender.get_value() !== "" && sender.get_value() == args.get_item().get_value())
        args.set_cancel(true);
}
function onTheraListChanged(sender, args)
{
    loadSectionList(sender);
}


function onSectionListLoad(sender, args)
{
    //theraCtlID, sectionCtlID, planCtlID: will be set in Labopharm/businessplanning/controls/filteraccountselection.ascx.cs
    loadSectionList($find(theraCtlID));
}

function loadSectionList(sender)
{
    if (!sender) return;

    var theraID = sender.get_value();
    if (theraID == "") theraID = 0;
    var list = bpAcctTypes[theraID];

    var data = clientManager.get_SelectionData();
    var val;
    if(data) val = (data["Section_ID"] ? data["Section_ID"].value : null);

    var all;
    //For Business Planning Report: include ALL option.
    if (includeAll)
          all = {text: "All Account Types", value: ""};

      //theraCtlID, sectionCtlID, planCtlID: will be set in Labopharm/businessplanning/controls/filteraccountselection.ascx.cs
      $loadListItems($find(sectionCtlID), list, all, val);
}

function onSectionListChanged(sender, args)
{
    var all;
    //For Business Planning Report: include ALL option.
    if (includeAll)
            all = {text: "All Account Names", value: ""};
            
    var val = sender.get_value();
    if (val == 0) val = null;
    var url = "custom/Labopharm/businessplanning/services/LabopharmDataService.svc/PlanSearch1Set?$filter=Section_ID eq " + val + "&$orderby=Name";
    $getJSON(url, null, function(result, status)
    {
        if (!status)
            result = result[0];
            
        var d = result.d;

        //theraCtlID, sectionCtlID, planCtlID: will be set in Labopharm/businessplanning/controls/filteraccountselection.ascx.cs
        $loadListItems($find(planCtlID), d, all);

        //after list is loaded check if current Plan ID can be selected
        var data = clientManager.get_SelectionData();
        var planID;
        if (data && data["Plan_ID"]) planID = data["Plan_ID"].value;
        if (planID)
        {
            var li = $find(planCtlID).findItemByValue(planID);
            if (li) li.select();
        }
    });
}

