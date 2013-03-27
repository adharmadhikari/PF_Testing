<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CoveredLivesScript.ascx.cs" Inherits="controls_CoveredLivesScript" %>
<%--<%@ OutputCache Shared="true" VaryByParam="None" Duration="100" %>--%> 


 <script type="text/javascript">
 
  $(document).ready(function(){
 $('#benDsnTotal .areaHeader').click(function(){     
    $(this).next().toggle('slow');
    
    // image change ( right-arrow changed to down-arrow/ down-arrow changed to right-arrow)
    var right = "content/images/arwRtSmfff.gif";
    var down = "content/images/arwDwnW.gif";
    var id = $(this).attr("id");
    
    if ($('#'+ id + ' #arrowR').attr("src") == right)
        $('#'+ id + ' #arrowR').attr("src", down);
    else
        $('#'+ id + ' #arrowR').attr("src", right);
 
    return false;
    }).next().show();

 });

  
 //////////////////////////////////////////////////////////
  
     clientManager.add_pageLoaded(pageInitialized, '<%= this.ContainerID %>');
     clientManager.add_pageUnloaded(pageUnloaded, '<%= this.ContainerID %>');

    var commercialDataDate = '<%= Pinsonault.Web.Support.GetDataUpdateDateByKey("Commercial Formulary", Resources.Resource.Label_Section_Last_Updated) %>';
    var medDDataDate = '<%= Pinsonault.Web.Support.GetDataUpdateDateByKey("Part-D Formulary", Resources.Resource.Label_Section_Last_Updated) %>';
    var statemedicaidDataDate = '<%= Pinsonault.Web.Support.GetDataUpdateDateByKey("State Medicaid Formulary", Resources.Resource.Label_Section_Last_Updated) %>';
    var dodDataDate = '<%= Pinsonault.Web.Support.GetDataUpdateDateByKey("DoD Formulary", Resources.Resource.Label_Section_Last_Updated) %>';
    
    var allowPALink = <%= System.Web.HttpContext.Current.User.IsInRole("paforms").ToString().ToLower() %>;
    var allowMedLink = <%= System.Web.HttpContext.Current.User.IsInRole("medforms").ToString().ToLower() %>;

    var showQLLink = <%= ShowQLLink %>;
   

     var UI_Ready = false;
     var lastSelection = null;
     var preSelectCount = 10;
     var drug_name = 0; var tier = 1; var status = 2; var Pa = 3;var Ql = 4;var St= 5; var MedPolicy = 6;var copay = 7;var comment = 8;
     
     var commercialchannel = 1;
     var pbmchannel = 4;
     var managedmedicaidchannel = 6
     var statemedicaidchannel= 9;
     var vachannel = 11;
     var dodchannel = 12;
     var meddchannel = 17;
          
     function pageInitialized() {
     
        var gridDD = $get(gridCLDrillDownID)

        //If User doesn't have 'EnableFrmly'(Enable Formulary) role then disable the Benefit Design drill down section.
        if (gridDD == null)
        {
            if ('<%= this.ContainerID %>' != "infoPopup") //don't adjust for popup
            {
                //Code for hiding drilldown section and expanding the other 2 sections(tile3 and tile4) to fit on the page.                  
                $("#tile3").attr("class", "leftTile");
                $("#tile4").attr("class", "rightTile");
                $("#tile3").css("width", "50%");
                $("#tile4").css("width", "50%");
                $("#tile5").css("width", "0%");
                $("#tile5").hide();
                $(".TriSectionLft").removeClass("TriSectionLft");
            }
        }
        else
        {
            //Sets Drugs combo box with checkboxes.
            $createCheckboxDropdown(drugCtrlID, "Drug_ID", null, { 'maxItems': 10, 'defaultText': '<%= Resources.Resource.Label_No_Selection %>', 'multiItemText': '<%= Resources.Resource.Label_Multiple_Selection %>' }, { 'error': checkboxlsterr }, '<%= this.ContainerID %>');

            //initialize Market Basket & Drug List event handlers
            var mbID = clientManager.getContextValue("clSelectedTheraClass");
            var mb = $get(drugCLTheraClassCtrlID).control;
            if (mb)
            {
                //1/6/10 - Gianni DeRosa 
                //Changed line below because Market Basket was not displaying correctly on an 
                //invalid MarketBasket cookie attribute (Incident 675)
                //$loadListItems(mb, clientManager.get_MarketBasketListOptions(), null, (mbID ? mbID : clientManager.get_MarketBasket()));
                if (!clientManager.get_DrugListOptions()[clientManager.get_MarketBasket()]) $loadListItems(mb, clientManager.get_MarketBasketListOptions(), null, (mbID ? mbID : null));
                else $loadListItems(mb, clientManager.get_MarketBasketListOptions(), null, clientManager.get_MarketBasket());
                //
            }

            var gridDrilldown = $get(gridCLDrillDownID).GridWrapper;
            gridDrilldown.add_dataBound(gridcoveredlivesdrilldown_onDataBound);

            var drugschklist = $find("Drug_ID");
            var selectedDrugs = clientManager.getContextValue("clSelectedDrugs");
            if (!selectedDrugs) 
            {
                if(!drugschklist.get_values()) //only select top n if nothing pre-selected
                {
                    for (var i = 0; i < preSelectCount; i++)
                    {
                        drugschklist.selectItemAt(i);
                    }
                }
            }
            else
            {
                var drugIDs = selectedDrugs.split(",");
                drugschklist.reset();
                for (var i = 0; i < drugIDs.length; i++)
                {
                    drugschklist.selectItem(drugIDs[i]);
                }
            }

            lastSelection = drugschklist.get_values();
            if ($.isArray(lastSelection))
                lastSelection = lastSelection.join(",");

            //Selecting the first row from commercial grid. It calls gridCommBenefitDesg_rowclick event and drilldown 
            //grid is updated accordingly.
            
            var mt = $find("ctl00_Tile4_CommBenefitDesign1_gridCommBenefitDesg");
           
            if ((typeof (window['gridMedDBenefitDesignID']) != 'undefined') && (!mt))
            {           
                mt = $find(gridMedDBenefitDesignID);
                $("#CoveredLivesDrilldownTitle").html("<span>Medicare Part D</span>");       
            }
            else if ((typeof (window['gridMMBenefitDesignID']) != 'undefined') && (!mt) && (clientManager.get_EffectiveChannel() == 6))
            {                
                mt = $find(gridMMBenefitDesignID);
                $("#CoveredLivesDrilldownTitle").html("<span>Managed Medicaid</span>");
            }           
            else if ((typeof (window['gridSMBenefitDesgID']) != 'undefined') && (!mt) && (clientManager.get_EffectiveChannel() == 9))
            {
                //Change the drilldown header to 'State Medicaid'  
                $("#CoveredLivesDrilldownTitle").text("State Medicaid");
                mt = $find(gridSMBenefitDesgID);
                status = 2; Pa = 3; Ql = 4; St = 5; copay = 6; comment = 7; MedPolicy = null;              
            }
            else if ((typeof (window['gridDoDBenefitDesgID']) != 'undefined') && (!mt) && (clientManager.get_EffectiveChannel() == 12))
            {
                //Change the drilldown header to 'DoD'  
                $("#CoveredLivesDrilldownTitle").text("DoD");
                mt = $find(gridDoDBenefitDesgID);
            }
            else if ((typeof (window['gridVABenefitDesgID']) != 'undefined') && (!mt) && (clientManager.get_EffectiveChannel() == 11))
            {
                //Change the drilldown header to 'DoD'  
                $("#CoveredLivesDrilldownTitle").text("VA");
                mt = $find(gridVABenefitDesgID);
            }      
            if (mt)   // avoid error: if mt is object
            {            
                mt = mt.get_masterTableView();
                mt.selectItem(0);  
            }
            
            //If there are no records in Commercial, select the first row in MM (only for Combined section)
            //Business Rule: Benefit Design order - Comm, MM, Med D
            //if (clientManager.get_EffectiveChannel() == 99 || clientManager.get_EffectiveChannel() == 4)
            if (clientManager.get_EffectiveChannel() == 99)
            {
                var mt = $find("ctl00_Tile4_CommBenefitDesign1_gridCommBenefitDesg");

                if (mt && mt.get_masterTableView())
                {
                    mt = mt.get_masterTableView();
                    var rows = mt.get_dataItems();
                    
                    //If Commercial Grid has no records, select the first item in the MM Grid
                    if (rows.length == 0)
                    {                        
                        //mt = $find("ctl00_Tile4_MedDBenefitDesign1_gridMedDBenefitDesg")
                        if (clientManager.get_EffectiveChannel() == 99)
                            mt = $find("ctl00_Tile4_MMBenefitDesign1_gridMMBenefitDesg")
                        else
                            mt = $find("ctl00_Tile4_MedDBenefitDesign1_gridMedDBenefitDesg")
                        
                        if (mt && mt.get_masterTableView())
                        {
                            mt = mt.get_masterTableView();
                            mt.selectItem(0); 
                        }
                    }
                }                
                else  // if Comm Grid is not object
                {
                    if (clientManager.get_EffectiveChannel() == 99)
                        mt = $find("ctl00_Tile4_MMBenefitDesign1_gridMMBenefitDesg")
                    else
                        mt = $find("ctl00_Tile4_MedDBenefitDesign1_gridMedDBenefitDesg")
                        
                    if (mt && mt.get_masterTableView())
                    {
                        mt = mt.get_masterTableView();
                        mt.selectItem(0); 
                    }
                }                
            }         
            
            //Case for PBM selection - select first for in PBM grid
            if (clientManager.get_EffectiveChannel() == 4)
            {
                var mt = $find("ctl00_Tile4_PBMBenefitDesign1_gridPBMBenefitDesg");

                if (mt && mt.get_masterTableView())
                {
                    mt = mt.get_masterTableView();
                    mt.selectItem(0);                    
                }              
            }           

            var drugs = $get(drugCtrlID).control;

            //Selected text which appears on the top of the drug dropdown list.
            $updateCheckboxDropdownText(drugs, "Drug_ID");

            //If there are no records in commercial grid then hides the drilldown section and shows appropriate message.
            if (mt)
            {
                if (mt.get_dataItems().length == 0)
                {
                    // hide T. class, drugs combo and drill down grid and show msg.
                    var gw = $get(gridCLDrillDownID).GridWrapper;
                    $(gw.get_element()).css("visibility", "hidden");

                    mb.set_visible(false);
                    drugs.set_visible(false);
                    $("#CoveredLivesNoRecMsg").text("There are no records to display for the selected plan.").css("display", "block");
                }
                else
                {
                    // else unhide T. class, drugs combo and drill down grid and don't show msg.
                    var gw = $get(gridCLDrillDownID).GridWrapper;
                    $(gw.get_element()).css("visibility", "visible");

                    mb.set_visible(true);
                    drugs.set_visible(true);
                    $("#CoveredLivesNoRecMsg").html("").css("display", "none");
                }
            }
            else  // if mt is not object
            {
                  // hide T. class, drugs combo and drill down grid and show msg.
                    var gw = $get(gridCLDrillDownID).GridWrapper;
                    $(gw.get_element()).css("visibility", "hidden");

                    mb.set_visible(false);
                    drugs.set_visible(false);
                    // $("#CoveredLivesDrilldownTitle").html("<span></span>");
                    $("#CoveredLivesDrilldownTitle").css("visibility", "hidden");
                    $("#CoveredLivesNoRecMsg").text("There are no records to display.").css("display", "block");            
            }
        }
        
        //Logic for showing and hiding grids for combined section based on what channel is selected.
        var data = clientManager.get_SelectionData();
        if (data["Original_Section"])
        {
            var sections = data["Original_Section"];

            var hasCommercial = $.inArray(1, sections) > -1;
            var hasManagedMedicaid = $.inArray(6, sections) > -1;
            var hasPartD = $.inArray(17, sections) > -1;
        
            if (!hasCommercial)
            {   
                //If Commercial is not selected, hide Commercial grid
                $('#ctl00_Tile4_CommBenefitDesign1_BDHeader1, #ctl00_Tile4_CommBenefitDesign1_gridCommBenefitDesg').hide();
                $('#ctl00_Tile4_MMBenefitDesign1_BDHeader3, #ctl00_Tile4_MMBenefitDesign1_gridMMBenefitDesg').show();
                $('#ctl00_Tile4_MedDBenefitDesign1_BDHeader2, #ctl00_Tile4_MedDBenefitDesign1_gridMedDBenefitDesg').show();

                toggleGrids("ctl00_Tile4_MMBenefitDesign1_gridMMBenefitDesg", "ctl00_Tile4_MedDBenefitDesign1_gridMedDBenefitDesg", "ctl00_Tile4_CommBenefitDesign1_gridCommBenefitDesg");
            }
            if (!hasManagedMedicaid)
            {
                //If Managed Medicaid is not selected, hide Managed Medicaid grid
                $('#ctl00_Tile4_MMBenefitDesign1_BDHeader3, #ctl00_Tile4_MMBenefitDesign1_gridMMBenefitDesg').hide();
                $('#ctl00_Tile4_CommBenefitDesign1_BDHeader1, #ctl00_Tile4_CommBenefitDesign1_gridCommBenefitDesg').show();            
                $('#ctl00_Tile4_MedDBenefitDesign1_BDHeader2, #ctl00_Tile4_MedDBenefitDesign1_gridMedDBenefitDesg').show();

                toggleGrids("ctl00_Tile4_CommBenefitDesign1_gridCommBenefitDesg", "ctl00_Tile4_MedDBenefitDesign1_gridMedDBenefitDesg", "ctl00_Tile4_MMBenefitDesign1_gridMMBenefitDesg");
            }
            if (!hasPartD)
            {
                //If Med D is not selected, hide Med D grid
                $('#ctl00_Tile4_MedDBenefitDesign1_BDHeader2, #ctl00_Tile4_MedDBenefitDesign1_gridMedDBenefitDesg').hide();
                $('#ctl00_Tile4_CommBenefitDesign1_BDHeader1, #ctl00_Tile4_CommBenefitDesign1_gridCommBenefitDesg').show();
                $('#ctl00_Tile4_MMBenefitDesign1_BDHeader3, #ctl00_Tile4_MMBenefitDesign1_gridMMBenefitDesg').show();

                toggleGrids("ctl00_Tile4_CommBenefitDesign1_gridCommBenefitDesg", "ctl00_Tile4_MMBenefitDesign1_gridMMBenefitDesg", "ctl00_Tile4_MedDBenefitDesign1_gridMedDBenefitDesg");
            }
        }     
        
        UI_Ready = true;

        truncateMenu();
    }
    
    function toggleGrids(grida, gridb, gridc)
    {    
        //Clear selection on hidden grid
        if ($get(gridc))
        {
            var mt = $get(gridc).control.get_masterTableView();
            mt.clearSelectedItems();
        }
        
        var mt = $find(grida);

        if (mt && mt && mt.get_masterTableView())
        {
            mt = mt.get_masterTableView();
            var rows = mt.get_dataItems();
            
            //If there is data in grida, select the first row
            if (rows.length > 0)
                mt.selectItem(0);
            //If there is no data in grid a, select the first row in gridb
            else
            {
                var mt = $find(gridb);            
                
                if (mt && mt && mt.get_masterTableView())
                {
                    mt = mt.get_masterTableView();
                    var rows = mt.get_dataItems();
                    
                    if (rows.length > 0)
                        mt.selectItem(0);
                }
            }
        }
    }

    function checkboxlsterr(sender, args) 
    {
        $alert("Maximum 10 drugs should be selected.", "Warning");
    }

    function pageUnloaded() 
    {
        clientManager.remove_pageLoaded(pageInitialized, '<%= this.ContainerID %>');
        clientManager.remove_pageUnloaded(pageUnloaded, '<%= this.ContainerID %>');
    }

     //Loads data in druglist per therapeutic class selection.
     function rdcmbCLTheraClass_SelectedIndexChanged(sender, args) {
         var drugs = $get(drugCtrlID).control;

         //checkboxes are displayed instead of listbox for drugs.
         var drug_id = $get("Drug_ID").control;
         $loadPinsoListItems(drug_id, clientManager.get_DrugListOptions()[sender.get_value()], null, -1);

         clientManager.setContextValue("clSelectedTheraClass", sender.get_value());

         if (UI_Ready) {
             var drugschklist = $find("Drug_ID");
             if(!drugschklist.get_values()) //if no preselected items then take auto select top n items
            {
                for (var i = 0; i < preSelectCount; i++)             
                    drugschklist.selectItemAt(i);
            }
              clientManager.setContextValue("clSelectedDrugs", null);
         }
         //Selected text which appears on the top of the drug dropdown list.
         $updateCheckboxDropdownText(drugs, "Drug_ID");

         if (UI_Ready == false) {
             return;
         }

         //Refreshes drilldown grid with newly selected drug(s).
         RefreshGrid();
         truncateMenu();
     }

     //Refreshes drilldown grid with newly selected drug(s).
     function rdcmbCLDrugs_DropDownClosed(sender, args) {
         var gw = $get(gridCLDrillDownID).GridWrapper;
         $(gw.get_element()).css("visibility", "visible");

         //Filter grid with new Drug_ID's
         var vals = sender.get_element().checkboxList.get_values();
         if ($.isArray(vals))
             vals = vals.join(",");

         if (vals != lastSelection)
             RefreshGrid();

         lastSelection = vals;
         clientManager.setContextValue("clSelectedDrugs", lastSelection);
         
            
     }

     //Refreshes drilldown grid with newly selected drug(s).
     function RefreshGrid() {
         if (!UI_Ready) return;

         var grid = $get(gridCLDrillDownID).control;
         var masterTable = grid.get_masterTableView();

         var drugs = $get("Drug_ID").control.get_values();
         //If drugs are selected from the dropdown list then refresh the drilldown grid else clear it.
         if (drugs) {
             $clearGridFilter(masterTable, "Drug_ID");
             masterTable.get_filterExpressions().add($createFilter("Drug_ID", drugs, null, "System.Int32"));

             masterTable.rebind();
//             var data = clientManager.get_SelectionData();
//             data["Drug_ID"] = drugs;
//             data["Formulary_ID"] = clientManager.get_SelectionData(1)["Formulary_ID"];
//             data["Segment_ID"] = clientManager.get_SelectionData(1)["Segment_ID"];
//              data["Section_ID"] = clientManager.get_EffectiveChannel();
            var data = {"Plan_ID" : clientManager.get_SelectionData()["Plan_ID"] ,
                        "Section_ID" : clientManager.get_EffectiveChannel(), 
                        "Drug_ID": drugs, 
                        "Formulary_ID": clientManager.get_SelectionData(1)["Formulary_ID"], 
                        "Segment_ID": clientManager.get_SelectionData(1)["Segment_ID"],
                        "Product_ID": clientManager.get_SelectionData(1)["Product_ID"]
                        };
                 data["__options"] = clientManager.get_SelectionData(1)["__options"];
            clientManager.set_SelectionData(data, 1);
         }
         else {
             $find(gridCLDrillDownID + "$GridWrapper").clearGrid();
         }
         
         
     }

    function DisableLinks()
    {
        var grid = $get(gridCLDrillDownID).control;
        var masterTable = grid.get_masterTableView();

        for (var i = 0; i <= (masterTable.get_dataItems().length - 2); i++)
        {
            var cell;
            var href; //href for link to reassign to img tag of comments
            var j; //temp jQuery object

            var item = masterTable.get_dataItems()[i];
            var dataItem = item.get_dataItem();

            if (dataItem != null)
            {

                //Formulary Status Name
                if (dataItem.Formulary_Status_Name != "")
                {
                    cell = item.get_element().cells[status];
                    $(cell).attr("title", dataItem.Formulary_Status_Name);
                }

                //PA                 
                cell = item.get_element().cells[Pa];
                if (!allowPALink || (dataItem.PA_Restrictions != "" && dataItem.DPA_Form == null && dataItem.DPA_Path == null))
                {
                    $(cell).addClass("disablelink").find("a").attr("href", "javascript:void(0)");
                }
                else
                    $(cell).removeClass("disablelink");

    //            //QL
    //            cell = item.get_element().cells[Ql];
    //            if (dataItem.QL_Restrictions != "" && dataItem.DQL_Notes == null) {
    //                $(cell).addClass("disablelink").find("a").attr("href", "javascript:void(0)");
    //            }
    //            else
    //                $(cell).removeClass("disablelink");


                //QL Restriction Criteria
                cell = item.get_element().cells[Ql];
                
                //sl 7/20/2012 QL link fix
                if (showQLLink)
                {
                    if ( dataItem.QL_Restrictions != "" && dataItem.Criteria_Description == null)
                    {
                        $(cell).addClass("disablelink").find("a").attr("href", "javascript:void(0)");
                    }
                    else
                        $(cell).removeClass("disablelink");
                 }
                    
                 else
                     $(cell).addClass("disablelink").find("a").attr("href", "javascript:void(0)");

                //ST
                cell = item.get_element().cells[St];
                if (dataItem.ST_Restrictions != "" && dataItem.DST_Notes == null)
                {
                    $(cell).addClass("disablelink").find("a").attr("href", "javascript:void(0)");
                }
                else
                    $(cell).removeClass("disablelink");

                //Med Policy
                cell = item.get_element().cells[MedPolicy];
                if (cell != null)
                {
                    if (!allowMedLink || (dataItem.Med_Policy != "" && dataItem.DMed_Policy_Form == null && dataItem.DMed_Policy_Path == null))
                    {
                        $(cell).addClass("disablelink").find("a").attr("href", "javascript:void(0)");
                    }
                    else
                        $(cell).removeClass("disablelink");
                }

                //Comments
                cell = item.get_element().cells[comment];
                j = $(cell).find("a");
                if (j.length)
                {
                    if (dataItem.Comments == "*")
                    {
                        href = j.attr("href").replace("javascript:", "");
                        j.html("<img src='content/images/information.png' onmouseout=\"$('#infoPopup').hide()\" onmouseover='" + href + "' />");
                    }
                    else
                    {
                        j.html("&nbsp;"); //clean up if new data 
                        $(cell).addClass("disablelink").find("a").attr("href", "javascript:void(0)");
                    }
                }
            }
        }
    }

     //On gridCommBenefitDesg grid data binding, hide the drilldown grid and display help message to view drilldown
    function gridCommBenefitDesg_onDataBinding() {
        $("#CoveredLivesDrilldownTitle").html('<%= Resources.Resource.Message_Formulary_Status_DrillDown %>');

        var gw = $get(gridCLDrillDownID).GridWrapper;
        if (gw) 
        {
            gw.clearGrid();
            $(gw.get_element()).css("visibility", "hidden");
        }
    }

     //On drilldown grid databound, make the grid visible.
    function gridcoveredlivesdrilldown_onDataBound(sender, args) {
        DisableLinks();

        $(sender.get_element()).css("visibility", "visible");
    }
     
     // sl 3/21/2012:  Managed Medicaid Business for CP 
     //selected PlanID, Formulary Name, Drug(s)ID'(s), Segment_ID(6 for ManagedMedicaid and 8 for LIS)
    function gridMMBenefitDesg_rowclick(sender, args)
    {
        var gw = $get(gridCLDrillDownID).GridWrapper;
        $(gw.get_element()).css("visibility", "visible");

        var grid = $get(gridCLDrillDownID).control;
        var masterTable = grid.get_masterTableView();

        $setGridFilter(grid, "Plan_ID", args._dataKeyValues.Plan_ID, "EqualTo", "System.Int32");

        //Creates drugs array with 0 as one array element: [0, selected Drug_ID(s)]
        var drugs = $get("Drug_ID").control.get_values();

        //Sets the filter based selected drug(s).
        $clearGridFilter(masterTable, "Drug_ID");
        masterTable.get_filterExpressions().add($createFilter("Drug_ID", drugs, null, "System.Int32"));

        $setGridFilter(grid, "Formulary_ID", args._dataKeyValues.Formulary_ID, "EqualTo", "System.Int32");

        //sl: 3/26/2012 using Segment_ID (6 for ManagedMedicaid and 8 for LIS)
        //$setGridFilter(grid, "Segment_ID", 6, "EqualTo", "System.Int32");
        $setGridFilter(grid, "Segment_ID", args._dataKeyValues.Segment_ID, "EqualTo", "System.Int32");

        //set filter for Section_ID
        $setGridFilter(grid, "Section_ID",clientManager.get_EffectiveChannel(), "EqualTo", "System.Int32");

        //set filter for Product_ID
        $setGridFilter(grid, "Product_ID", args._dataKeyValues.Prod_ID, "EqualTo", "System.Int32");

        masterTable.rebind();

        //Change the drilldown header to 'Commercial'.
        //$("#CoveredLivesDrilldownTitle").text("Commercial - " + args._dataKeyValues.Formulary_Name);

        // sl: formulary PDL link - use Formulary_ID (Prod_ID always 0)
        $("#CoveredLivesDrilldownTitle").html("<span>Managed Medicaid - <a class='formularyLinkBD' href='javascript:formularyPDL_Link(" + args._dataKeyValues.Plan_ID + ",1," + args._dataKeyValues.Formulary_ID + ",0)'>" + args._dataKeyValues.Formulary_Name + "</a></span>");


        //$("#CoveredLivesDrilldownFooter").text(commercialDataDate);

        //Clear selection on Med-D grid & Comm grid.
        if ($get("ctl00_Tile4_MedDBenefitDesign1_gridMedDBenefitDesg")) {
            var mt = $get("ctl00_Tile4_MedDBenefitDesign1_gridMedDBenefitDesg").control.get_masterTableView();
            mt.clearSelectedItems();
        }
        if ($get("ctl00_Tile4_CommBenefitDesign1_gridCommBenefitDesg")) {
            var mt = $get("ctl00_Tile4_CommBenefitDesign1_gridCommBenefitDesg").control.get_masterTableView();
            mt.clearSelectedItems();
        }
        
        var data = {"Plan_ID" : clientManager.get_SelectionData()["Plan_ID"],"Formulary_ID": args._dataKeyValues.Formulary_ID, "Segment_ID": args._dataKeyValues.Segment_ID, "Section_ID" : clientManager.get_EffectiveChannel(), "Drug_ID": drugs , "Product_ID": args._dataKeyValues.Prod_ID};
       
        data["__options"] = { Formulary_Name: new Pathfinder.UI.dataParam("Formulary_Name", args._dataKeyValues.Formulary_Name, "System.String", "EqualTo"),"FHR_Section_ID": managedmedicaidchannel };
        clientManager.set_SelectionData(data, 1);
              
        //because of header possibly breaking grid scrolling if text wraps
        todaysaccounts_section_resize(); 
    }

     //On commercial grid row selection, sets filter on drilldown grid to view the data for selected PlanID,
     //Formulary Name, Drug(s)ID'(s), Segment_ID(1 for Commercial and 2 for Medicare Part D)
    function gridCommBenefitDesg_rowclick(sender, args) {
        var gw = $get(gridCLDrillDownID).GridWrapper;
        $(gw.get_element()).css("visibility", "visible");

        var grid = $get(gridCLDrillDownID).control;
        var masterTable = grid.get_masterTableView();

        $setGridFilter(grid, "Plan_ID", args._dataKeyValues.Plan_ID, "EqualTo", "System.Int32");

        //Creates drugs array with 0 as one array element: [0, selected Drug_ID(s)]
        var drugs = $get("Drug_ID").control.get_values();

        //Sets the filter based selected drug(s).
        $clearGridFilter(masterTable, "Drug_ID");
        masterTable.get_filterExpressions().add($createFilter("Drug_ID", drugs, null, "System.Int32"));

        $setGridFilter(grid, "Formulary_ID", args._dataKeyValues.Formulary_ID, "EqualTo", "System.Int32");

        //Segment_ID is 1 for Commercial and 2 for Medicare Part D
        //$setGridFilter(grid, "Segment_ID", 1, "EqualTo", "System.Int32");

        // sl 3/26/2012  using Segment_ID
        $setGridFilter(grid, "Segment_ID", args._dataKeyValues.Segment_ID, "EqualTo", "System.Int32");

        //set filter for Product_ID
        $setGridFilter(grid, "Product_ID", args._dataKeyValues.Prod_ID, "EqualTo", "System.Int32");
        
        //set filter for Section_ID
        $setGridFilter(grid, "Section_ID",clientManager.get_EffectiveChannel(), "EqualTo", "System.Int32");
        
        masterTable.rebind();
        
        //Change the drilldown header to 'Commercial'.
        //$("#CoveredLivesDrilldownTitle").text("Commercial - " + args._dataKeyValues.Formulary_Name);

        // sl: formulary PDL link - use Formulary_ID (Prod_ID always 0)
        $("#CoveredLivesDrilldownTitle").html("<span>Commercial - <a class='formularyLinkBD' href='javascript:formularyPDL_Link(" + args._dataKeyValues.Plan_ID + ",1," + args._dataKeyValues.Formulary_ID + ",0)'>" + args._dataKeyValues.Formulary_Name + "</a></span>");

        $("#CoveredLivesDrilldownFooter").text(commercialDataDate);

        //Clear selection on Med-D grid.
        if ($get("ctl00_Tile4_MedDBenefitDesign1_gridMedDBenefitDesg")) {
            var mt = $get("ctl00_Tile4_MedDBenefitDesign1_gridMedDBenefitDesg").control.get_masterTableView();
            mt.clearSelectedItems();
        }

        //sl 3/22/2012 Clear selection on MM grid
        if ($get("ctl00_Tile4_MMBenefitDesign1_gridMMBenefitDesg")) {
            var mt = $get("ctl00_Tile4_MMBenefitDesign1_gridMMBenefitDesg").control.get_masterTableView();
            mt.clearSelectedItems();
        }

        var data = {"Plan_ID" : clientManager.get_SelectionData()["Plan_ID"],"Formulary_ID": args._dataKeyValues.Formulary_ID,"Segment_ID": args._dataKeyValues.Segment_ID,"Section_ID" : clientManager.get_EffectiveChannel(), "Drug_ID": drugs , "Product_ID": args._dataKeyValues.Prod_ID};
        
        data["__options"] = { Formulary_Name: new Pathfinder.UI.dataParam("Formulary_Name", args._dataKeyValues.Formulary_Name, "System.String", "EqualTo"), "FHR_Section_ID": commercialchannel };
        clientManager.set_SelectionData(data, 1);
             //data["Drug_ID"] = drugs;
             //data["Formulary_ID"] = args._dataKeyValues.Formulary_ID;
             //data["Segment_ID"] = 1;
             //var data = {"Plan_ID" : clientManager.get_SelectionData()["Plan_ID"] , "Drug_ID": drugs, "Formulary_ID": args._dataKeyValues.Formulary_ID, "Segment_ID": 1 };
              
            //because of header possibly breaking grid scrolling if text wraps
        todaysaccounts_section_resize(); 
    }
    
    function gridPBMBenefitDesg_rowclick(sender, args) {
        var gw = $get(gridCLDrillDownID).GridWrapper;
        $(gw.get_element()).css("visibility", "visible");

        var grid = $get(gridCLDrillDownID).control;
        var masterTable = grid.get_masterTableView();

        $setGridFilter(grid, "Plan_ID", args._dataKeyValues.Plan_ID, "EqualTo", "System.Int32");

        //Creates drugs array with 0 as one array element: [0, selected Drug_ID(s)]
        var drugs = $get("Drug_ID").control.get_values();

        //Sets the filter based selected drug(s).
        $clearGridFilter(masterTable, "Drug_ID");
        masterTable.get_filterExpressions().add($createFilter("Drug_ID", drugs, null, "System.Int32"));

        $setGridFilter(grid, "Formulary_ID", args._dataKeyValues.Formulary_ID, "EqualTo", "System.Int32");

        //set filter Segment_ID - should always be 4 in case of PBM
        $setGridFilter(grid, "Segment_ID", 4, "EqualTo", "System.Int32");
        
        //set filter Segment_ID - should always be 4 in case of PBM
        $setGridFilter(grid, "Section_ID", 4, "EqualTo", "System.Int32");

        //set filter for Product_ID
        $setGridFilter(grid, "Product_ID", args._dataKeyValues.Prod_ID, "EqualTo", "System.Int32");
        
        masterTable.rebind();
        
        // sl: formulary PDL link - use Formulary_ID (Prod_ID always 0)
        $("#CoveredLivesDrilldownTitle").html("<span>PBM - <a class='formularyLinkBD' href='javascript:formularyPDL_Link(" + args._dataKeyValues.Plan_ID + ",1," + args._dataKeyValues.Formulary_ID + ",0)'>" + args._dataKeyValues.Formulary_Name + "</a></span>");

        var data = {"Plan_ID" : clientManager.get_SelectionData()["Plan_ID"],"Formulary_ID": args._dataKeyValues.Formulary_ID,"Segment_ID": 4,"Section_ID": 4, "Drug_ID": drugs , "Product_ID": args._dataKeyValues.Prod_ID};
       
        data["__options"] = { Formulary_Name: new Pathfinder.UI.dataParam("Formulary_Name", args._dataKeyValues.Formulary_Name, "System.String", "EqualTo"), "FHR_Section_ID": pbmchannel };
        clientManager.set_SelectionData(data, 1);
              
        //because of header possibly breaking grid scrolling if text wraps
         todaysaccounts_section_resize(); 
     }
     
     // sl: formulary PDL link
    function formularyPDL_Link(planID, segmentID, formularyID, productID)
    { 
        $.getJSON("todaysaccounts/services/PathfinderDataService.svc/GetFormularyPDLLink?param='" + planID + ',' + segmentID + ',' + formularyID + ',' + productID + "'", null, _onFormularyPDLLinkCallback);
        //////////window.open(String.format("usercontent/downloadformularypdlform.ashx?planID={0}&segmentID={1}&formularyID={2}&productID={3}", planID, segmentID, formularyID, productID));
    }
     
     
    function _onFormularyPDLLinkCallback(r)
    {
        if (r.d.GetFormularyPDLLink == "NotExist")
             $alert("Formulary PDL Link does not exist.", "Formulary PDL Link")
             
        else if (r && r.d && r.d.GetFormularyPDLLink)
        {
           //window.open(String.format("http://{0}",r.d.GetFormularyPDLLink),"FormularyPDLLink",null,null);
            //sl 9/19/2011  pdl from db: http: or https:  
             //alert (r.d.GetFormularyPDLLink);     
           window.open(String.format("{0}",r.d.GetFormularyPDLLink),"FormularyPDLLink",null,null);
        
        }
    }

     //On Med-D grid row selection, sets filter on drilldown grid to view the data for selected PlanID,
     //Formulary Name, Drug(s)ID'(s), Segment_ID(1 for Commercial and 2 for Medicare Part D)
    function gridMedDBenefitDesg_rowclick(sender, args) {
        var gw = $get(gridCLDrillDownID).GridWrapper;
        $(gw.get_element()).css("visibility", "visible");

        var grid = $get(gridCLDrillDownID).control;
        var masterTable = grid.get_masterTableView();

        $setGridFilter(grid, "Plan_ID", args._dataKeyValues.Plan_ID, "EqualTo", "System.Int32");

        //Creates drugs array with 0 as one array element: [0, selected Drug_ID(s)]
        var drugs = $get("Drug_ID").control.get_values();

        //Sets the filter based selected drug(s).
        $clearGridFilter(masterTable, "Drug_ID");
        masterTable.get_filterExpressions().add($createFilter("Drug_ID", drugs, null, "System.Int32"));

        $setGridFilter(grid, "Formulary_ID", args._dataKeyValues.Formulary_ID, "EqualTo", "System.Int32");

        //Segment_ID is 1 for Commercial and 2 for Medicare Part D
        // $setGridFilter(grid, "Segment_ID", 2, "EqualTo", "System.Int32");
                    
        // sl 3/26/2012  using Segment_ID:  Medicare Part D(Segment ID: 2),  9:  LIS (Segment ID: 8)
         $setGridFilter(grid, "Segment_ID", args._dataKeyValues.Segment_ID, "EqualTo", "System.Int32");
         
        
        //set filter for Product_ID
        $setGridFilter(grid, "Product_ID", args._dataKeyValues.Prod_ID, "EqualTo", "System.Int32");
        
        //set filter for Section_ID
        $setGridFilter(grid, "Section_ID",clientManager.get_EffectiveChannel(), "EqualTo", "System.Int32");

        masterTable.rebind();

        //Change the drilldown header to 'Medicare Part D'.
        //$("#CoveredLivesDrilldownTitle").text("Medicare Part D - " + args._dataKeyValues.Prod_Name);
        
        
        // sl: formulary PDL link  - use Prod_ID ( + Formulary_ID)
        $("#CoveredLivesDrilldownTitle").html("<span>Medicare Part D - <a class='formularyLinkBD' href='javascript:formularyPDL_Link(" + args._dataKeyValues.Plan_ID + ",2," + args._dataKeyValues.Formulary_ID + "," + args._dataKeyValues.Prod_ID + ")'>" + args._dataKeyValues.Prod_Name + " - 000"  + args._dataKeyValues.Formulary_ID + "</a></span>");

        $("#CoveredLivesDrilldownFooter").text(medDDataDate);

        //Clear selection on commercial grid.
        if ($get("ctl00_Tile4_CommBenefitDesign1_gridCommBenefitDesg")) {
            var mt = $get("ctl00_Tile4_CommBenefitDesign1_gridCommBenefitDesg").control.get_masterTableView();
            mt.clearSelectedItems();
        }
       
        //sl 3/22/2012 Clear selection on MM grid
        if ($get("ctl00_Tile4_MMBenefitDesign1_gridMMBenefitDesg")) {
            var mt = $get("ctl00_Tile4_MMBenefitDesign1_gridMMBenefitDesg").control.get_masterTableView();
            mt.clearSelectedItems();
        }
         
        var data = {"Plan_ID" : clientManager.get_SelectionData()["Plan_ID"],"Formulary_ID": args._dataKeyValues.Formulary_ID,"Segment_ID": args._dataKeyValues.Segment_ID,"Section_ID" : clientManager.get_EffectiveChannel(), "Drug_ID": drugs , "Product_ID": args._dataKeyValues.Prod_ID};
       
        data["__options"] = { Prod_Name: new Pathfinder.UI.dataParam("Prod_Name", args._dataKeyValues.Prod_Name, "System.String", "EqualTo"), "FHR_Section_ID": meddchannel };
        clientManager.set_SelectionData(data, 1);

        // Unhide T. class, drugs combo and drill down grid.
        var mb = $get(drugCLTheraClassCtrlID).control;
        mb.set_visible(true);
        var drugs = $get(drugCtrlID).control;
        drugs.set_visible(true);
        $("#CoveredLivesNoRecMsg").html("").css("display", "none");

        //because of header possibly breaking grid scrolling if text wraps
        todaysaccounts_section_resize();
    }
    //for SM
    
    //Formulary Name, Drug(s)ID'(s), Segment_ID(9 for SM)
    function gridSMBenefitDesg_rowclick(sender, args) {
        var gw = $get(gridCLDrillDownID).GridWrapper;
        $(gw.get_element()).css("visibility", "visible");

        var grid = $get(gridCLDrillDownID).control;
        var masterTable = grid.get_masterTableView();

        $setGridFilter(grid, "Plan_ID", args._dataKeyValues.Plan_ID, "EqualTo", "System.Int32");

        //Creates drugs array with 0 as one array element: [0, selected Drug_ID(s)]
        var drugs = $get("Drug_ID").control.get_values();

        //Sets the filter based selected drug(s).
        $clearGridFilter(masterTable, "Drug_ID");
        masterTable.get_filterExpressions().add($createFilter("Drug_ID", drugs, null, "System.Int32"));

        $setGridFilter(grid, "Formulary_ID", args._dataKeyValues.Formulary_ID, "EqualTo", "System.Int32");

        // sl 3/26/2012  using Segment_ID
        $setGridFilter(grid, "Segment_ID", args._dataKeyValues.Segment_ID, "EqualTo", "System.Int32");

        //set filter for Product_ID
        $setGridFilter(grid, "Product_ID", args._dataKeyValues.Prod_ID, "EqualTo", "System.Int32");

        masterTable.rebind();

        var data = {"Plan_ID" : clientManager.get_SelectionData()["Plan_ID"], "Formulary_ID": args._dataKeyValues.Formulary_ID,"Segment_ID": args._dataKeyValues.Segment_ID,"Section_ID" : clientManager.get_EffectiveChannel(),"Drug_ID": drugs, "Product_ID": args._dataKeyValues.Prod_ID};
        data["__options"] = { Formulary_Name: new Pathfinder.UI.dataParam("Formulary_Name", args._dataKeyValues.Formulary_Name, "System.String", "EqualTo") , "FHR_Section_ID": statemedicaidchannel};
        clientManager.set_SelectionData(data, 1);

        // sl: SM (segment ID: 3)  - formulary PDL link - use Formulary_ID (Prod_ID always 0)
        $("#CoveredLivesDrilldownTitle").html("<span>State Medicaid - <a class='formularyLinkBD' href='javascript:formularyPDL_Link(" + args._dataKeyValues.Plan_ID + ",3," + args._dataKeyValues.Formulary_ID + ",0)'>" + args._dataKeyValues.Formulary_Name + "</a></span>");

        $("#CoveredLivesDrilldownFooter").text(statemedicaidDataDate);                   

        //because of header possibly breaking grid scrolling if text wraps
        todaysaccounts_section_resize();
    }

    //Formulary Name, Drug(s)ID'(s), Section_ID(12 for DoD)
    function gridDoDBenefitDesg_rowclick(sender, args) {
        var gw = $get(gridCLDrillDownID).GridWrapper;
        $(gw.get_element()).css("visibility", "visible");

        var grid = $get(gridCLDrillDownID).control;
        var masterTable = grid.get_masterTableView();

        $setGridFilter(grid, "Plan_ID", args._dataKeyValues.Plan_ID, "EqualTo", "System.Int32");

        //Creates drugs array with 0 as one array element: [0, selected Drug_ID(s)]
        var drugs = $get("Drug_ID").control.get_values();

        //Sets the filter based selected drug(s).
        $clearGridFilter(masterTable, "Drug_ID");
        masterTable.get_filterExpressions().add($createFilter("Drug_ID", drugs, null, "System.Int32"));

        $setGridFilter(grid, "Formulary_ID", args._dataKeyValues.Formulary_ID, "EqualTo", "System.Int32");

        // sl 3/26/2012  using Segment_ID
        $setGridFilter(grid, "Segment_ID", args._dataKeyValues.Segment_ID, "EqualTo", "System.Int32");

        //set filter for Product_ID
        $setGridFilter(grid, "Product_ID", args._dataKeyValues.Prod_ID, "EqualTo", "System.Int32");

        masterTable.rebind();

        var data = {"Plan_ID" : clientManager.get_SelectionData()["Plan_ID"], "Formulary_ID": args._dataKeyValues.Formulary_ID,"Segment_ID": args._dataKeyValues.Segment_ID,"Section_ID" : clientManager.get_EffectiveChannel(), "Drug_ID": drugs,"Product_ID": args._dataKeyValues.Prod_ID};
        data["__options"] = { Formulary_Name: new Pathfinder.UI.dataParam("Formulary_Name", args._dataKeyValues.Formulary_Name, "System.String", "EqualTo") , "FHR_Section_ID": dodchannel};
        clientManager.set_SelectionData(data, 1);
         
        // sl: DoD (segment ID:3) - formulary PDL link - use Formulary_ID (Prod_ID always 0)
        $("#CoveredLivesDrilldownTitle").html("<span>DoD - <a class='formularyLinkBD' href='javascript:formularyPDL_Link(" + args._dataKeyValues.Plan_ID + ",3," + args._dataKeyValues.Formulary_ID + ",0)'>" + args._dataKeyValues.Formulary_Name + "</a></span>");

        $("#CoveredLivesDrilldownFooter").text(dodDataDate);                   

        //because of header possibly breaking grid scrolling if text wraps
        todaysaccounts_section_resize();
    }
    
     //Formulary Name, Drug(s)ID'(s), Section_ID(11 for VA)
    function gridVABenefitDesg_rowclick(sender, args) 
    {
        var gw = $get(gridCLDrillDownID).GridWrapper;
        $(gw.get_element()).css("visibility", "visible");

        var grid = $get(gridCLDrillDownID).control;
        var masterTable = grid.get_masterTableView();

        $setGridFilter(grid, "Plan_ID", args._dataKeyValues.Plan_ID, "EqualTo", "System.Int32");

        //Creates drugs array with 0 as one array element: [0, selected Drug_ID(s)]
        var drugs = $get("Drug_ID").control.get_values();

        //Sets the filter based selected drug(s).
        $clearGridFilter(masterTable, "Drug_ID");
        masterTable.get_filterExpressions().add($createFilter("Drug_ID", drugs, null, "System.Int32"));

        $setGridFilter(grid, "Formulary_ID", args._dataKeyValues.Formulary_ID, "EqualTo", "System.Int32");

        // sl 3/26/2012  using Segment_ID
        $setGridFilter(grid, "Segment_ID", args._dataKeyValues.Segment_ID, "EqualTo", "System.Int32");
             
        //set filter for Product_ID
        $setGridFilter(grid, "Product_ID", args._dataKeyValues.Prod_ID, "EqualTo", "System.Int32");

        masterTable.rebind();

        var data = { "Plan_ID": clientManager.get_SelectionData()["Plan_ID"], "Formulary_ID": args._dataKeyValues.Formulary_ID, "Segment_ID": args._dataKeyValues.Segment_ID,"Section_ID" : clientManager.get_EffectiveChannel(), "Drug_ID": drugs, "Product_ID": args._dataKeyValues.Prod_ID };
        data["__options"] = { Formulary_Name: new Pathfinder.UI.dataParam("Formulary_Name", args._dataKeyValues.Formulary_Name, "System.String", "EqualTo"),"FHR_Section_ID": vachannel };
        clientManager.set_SelectionData(data, 1);

        // sl: VA (segment ID:1) - formulary PDL link - use Formulary_ID (Prod_ID always 0)
        $("#CoveredLivesDrilldownTitle").html("<span>VA - <a class='formularyLinkBD' href='javascript:formularyPDL_Link(" + args._dataKeyValues.Plan_ID + ",1," + args._dataKeyValues.Formulary_ID + ",0)'>" + args._dataKeyValues.Formulary_Name + "</a></span>");

        $("#CoveredLivesDrilldownFooter").text(dodDataDate);

        //because of header possibly breaking grid scrolling if text wraps
        todaysaccounts_section_resize();
    }

    function OpenMedPolicyForm(planID, segmentID, drugID)
    {
        var theraID = $find(drugCLTheraClassCtrlID).get_value();

        window.open(String.format("usercontent/downloadmedpolicyform.ashx?planID={0}&segmentID={1}&drugID={2}&theraID={3}", planID, segmentID, drugID, theraID));
    }
         
    function OpenPAForm(planID, segmentID, drugID)
    {
        var theraID = $find(drugCLTheraClassCtrlID).get_value();

        window.open(String.format("usercontent/downloadpaform.ashx?planID={0}&segmentID={1}&drugID={2}&theraID={3}", planID, segmentID, drugID, theraID));
    }
         

    //When clicked on QL/ST link from drilldown grid, it opens a popup window containing QL/ST Notes.
    function OpenNotesViewer(Plan_ID, Drug_ID, FormularyID, SegmentID, Type, x, y, width, height) {
        var app = clientManager.get_ApplicationManager();
        var url = app.getUrl("all", clientManager.get_Module(), "OpenNotes.aspx");
        url = url + "?Plan_ID=" + Plan_ID + "&Drug_ID=" + Drug_ID + "&FormularyID=" + FormularyID + "&SegmentID=" + SegmentID + "&Type=" + Type;

        var mt = $get(gridCLDrillDownID).control.get_masterTableView();
        var cell;

        //Get the list of dataitems which matches the selected Drug_ID.
        var list = $.grep(mt.get_dataItems(), function(i) { if (i.get_dataItem()) return i.get_dataItem().Drug_ID == Drug_ID; else return false; }, false);

        //Get the cellIndex for selected cell.
        if (list && list.length > 0) {
        var col;
        var rect;

        if (Type == "ST") {
            col = mt.getColumnByUniqueName("ST_Restrictions");
        }
        else if (Type == "QL") {
            col = mt.getColumnByUniqueName("QL_Restrictions");
        }
        else if (Type == "comments") {
            col = mt.getColumnByUniqueName("Comments");
        }

        if (col) {
            cell = list[0].get_element().cells[col.get_element().cellIndex];
        }
        else
            throw new Error("Cannot find column 'Restrictions' OR 'Comments'");
        }

        //Getting cell bounds.
        rect = Sys.UI.DomElement.getBounds(cell);

        //Open pop-up window with calculated co-ordinates.
        clientManager.openViewer(url, rect.x - width, rect.y, width, height);
    }  
 
    function moreInfo(tile)
    {        
        var j = $("#tile4 .tools .moreInfo");
        if(j.length)
        {
            var e = j[0];
            var rect = Sys.UI.DomElement.getBounds(e);
            var url = "usercontent/benefitdesigndefinitions.aspx?section_id="+ clientManager.get_EffectiveChannel();
            clientManager.openViewer(url, 200, rect.y - 125, 670, 373, 'tile4');
        }
    }
    
    function OpenFHRComparison() {
        var data = clientManager.get_SelectionData(1);

        if (data) {
            //if(data["Section_ID"] == 99)
            data["Section_ID"] = data.__options.FHR_Section_ID;
            var col = {};
            var theraCtrl = $find("ctl00_Tile5_CoveredLivesDrillDown1_rdcmbCLTheraClass");
            var theraText = theraCtrl._text;

            data["Thera_Name"] = theraText;

            var q = $getDataForPostback(data, null, col);

            clientManager.setContextValue("ta_fhQuery", data);

            var url = "todaysaccounts/all/formularyhistoryreporting_popup.aspx?" + q;

            //Replace spaces in the url with pipes or else it will cause an issue with openViewer
            url = url.replace(/ /g, "|");
            clientManager.openViewer(url, null, null, 900, 400, 'tile5');
            
//            $("#infoPopup").draggable({ handle: "div.tileContainerHeader"});
//            $("#fauxModal").css({
//                zIndex: "9000", visibility: "visible"
//            });
        }
    }
    
    function OpenRestrictionCriteria(planID, drugID, formularyID, segmentID, productID, restrictionID)
    {
        var app = clientManager.get_ApplicationManager();
        var url = app.getUrl("all", clientManager.get_Module(), "OpenRestrictionCriteria.aspx");

        url = url + "?PlanID=" + planID + "&DrugID=" + drugID + "&FormularyID=" + formularyID + "&SegmentID=" + segmentID + "&ProductID=" + productID + "&RestrictionID=" + restrictionID;

        var mt = $get(gridCLDrillDownID).control.get_masterTableView();
        var cell;

        //Get the list of dataitems which matches the selected Drug_ID.
        var list = $.grep(mt.get_dataItems(), function(i) { if (i.get_dataItem()) return i.get_dataItem().Plan_ID == planID && i.get_dataItem().Formulary_ID == formularyID && i.get_dataItem().Product_ID == productID; else return false; }, false);

        //Get the cellIndex for selected cell.
        if (list && list.length > 0)
        {
            var col;
            var rect;

            col = mt.getColumnByUniqueName(restrictionID + "_RestrictionCriteria");

            if (col)
            {
                cell = list[0].get_element().cells[col.get_element().cellIndex];
            }
            else
                throw new Error("Cannot find column '" + restrictionID + "'");
        }

        //Getting cell bounds.
        rect = Sys.UI.DomElement.getBounds(cell);

        //Open pop-up window with calculated co-ordinates.
        var width = 300;
        var height = 100;
        clientManager.openViewer(url, rect.x - 275, rect.y + 21, width, height);

        $("#infoPopup").draggable({ handle: "div.tileContainerHeader" });
    }
 </script>