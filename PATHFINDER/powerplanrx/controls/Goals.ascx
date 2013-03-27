<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Goals.ascx.cs" Inherits="controls_GoalsByDistrict" %>

<script type="text/javascript">

    $(document).ready(function() {
        var browserWindow = $(window);
        var divHeight = browserWindow.height();
        var divWidth = browserWindow.width();

        $("#dial").dialog({ resizable: false, draggable: false, autoOpen: false, modal: true, width: divWidth - 100, height: 600, title: "" });

        //        $("input[type=text]").change(function(event) {
        //            //$(" .UserInputBox").css({ "background-color": "#FBB36B" });
        //            //alert("here");
        //        });


        $("input[type=text]").keydown(function(event) {
            // Allow: backspace, delete, tab, escape, enter and decimal        
            if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27
            || event.keyCode == 13 || event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 190 ||
            // Allow: Ctrl+A            
             (event.keyCode == 65 && event.ctrlKey === true) ||
            // Allow: home, end, left, right             
             (event.keyCode >= 35 && event.keyCode <= 39)) {
                // let it happen, don't do anything                  
                return;
            }
            else {
                // Ensure that it is a number and s     top the keypress
                if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
                    event.preventDefault();
                }



            }
        });
    });


    function allocateGoals(objTextbox, monthI) {
        //var val = document.getElementById('<%= percentGrowth_month1.ClientID %>').value;
        //alert('value: ' + val + ', objTextbox: ' + objTextbox.value);  // same value
        var percentI = objTextbox.value

        if (percentI == null || percentI == "" || percentI < 0) {
            alert("Please enter a numeric value in '% Volume Growth'.");
            return false;
        }

        // to avoid error: .3, ..3,  1.3.5,
        var repeatString = 0;
        if (percentI.match(/\./g) != null) {
            repeatString = percentI.match(/\./g).length;
        }

        if (percentI.substring(0, 1) == '.' || repeatString > 1) {
            percentI = percentI.replace(/\./g, "");
            percentI = "0." + percentI;
            var cell_percent = ($(objTextbox).attr('id'));
            document.getElementById(cell_percent).value = percentI;
        }


        //reset validator
        //alert("test:  " + document.getElementById('<%= hValidator.ClientID %>').value);
        //document.getElementById('<%= hValidator.ClientID %>').value = 0;
        //document.getElementById('<%= txtDifference.ClientID %>').value = 0;

        //        document.getElementById('<%= hMonthI.ClientID %>').value = 0;
        //        document.getElementById('<%= hValidateOK.ClientID %>').value = "";
        //        document.getElementById('<%= hChangedList.ClientID %>').value = "";
        //        document.getElementById('<%= hRegion.ClientID %>').value = "";
        //document.getElementById('<%= hValidateOK.ClientID %>').value = "N";

        document.getElementById('<%= hGoalPercentOK.ClientID %>').value = "Y";
        ////////////// Plan: Begin ///// single row, get control's id using CSS /////////////////////
        var plan_Base_TRx_ID = $(".cssTRx_Plan").attr('id');
        var plan_Base_TRx = $("#" + plan_Base_TRx_ID).text();
        plan_Base_TRx = plan_Base_TRx.replace(",", "");

        var plan_Base_MST_ID = $(".cssMST_Plan").attr('id');
        var plan_Base_MST = $("#" + plan_Base_MST_ID).text();

        // to get Baseline MB_TRx
        var plan_Base_MB_TRx;
        if (plan_Base_MST > 0)
            plan_Base_MB_TRx = parseFloat(parseFloat(plan_Base_TRx) * 100 / parseFloat(plan_Base_MST)).toFixed(2);
        else
            plan_Base_MB_TRx = 0;

        // Goal: TRx
        var plan_Goal_TRx_ID = $(".cssTRx" + monthI).attr('id');
        plan_Goal_TRx_ID = plan_Goal_TRx_ID.replace("_text", "");

        var cell_plan_TRx = $find(plan_Goal_TRx_ID);

        // increased TRx
        var plan_increased_TRx;
        plan_increased_TRx = parseFloat(parseFloat(plan_Base_TRx) * parseInt(percentI) / 100).toFixed(2);

        var plan_Goal_TRx = parseFloat(parseFloat(plan_Base_TRx) + parseFloat(plan_increased_TRx)).toFixed(2);

        var plan_Goal_MB_TRx = parseFloat(parseFloat(plan_Base_MB_TRx) + parseFloat(plan_increased_TRx)).toFixed(2);
        // Goal: MST
        var plan_Goal_MST_ID = $(".cssMST" + monthI).attr('id');
        plan_Goal_MST_ID = plan_Goal_MST_ID.replace("_text", "");

        var cell_plan_MST = $find(plan_Goal_MST_ID);

        var plan_Goal_MST;
        if (plan_Goal_MB_TRx > 0)
            plan_Goal_MST = parseFloat(parseFloat(plan_Goal_TRx) * 100 / parseFloat(plan_Goal_MB_TRx)).toFixed(2);
        else
            plan_Goal_MST = 0;

        if (cell_plan_TRx != null && cell_plan_MST != null) {
            cell_plan_TRx.set_value(plan_Goal_TRx);   //Plan TRx
            cell_plan_MST.set_value(plan_Goal_MST);   //Plan MST
        }

        /////// Plan: end //////

        var grid = $find('<%= gridViewDistricts.ClientID %>');

        master = grid.get_masterTableView();
        var dataItems = master.get_dataItems();
        var masterRowCount = master.get_dataItems().length;

        //alert('Region Count: ' + masterRowCount);  // sl testing

        for (var i = 0; i < masterRowCount; i++) {
            // // allocate Region TRx, MST
            var R_row = master.get_dataItems()[i];

            var nonTargetedFlag = false;
            var _regionID = master.getCellByColumnUniqueName(R_row, "Region_ID");

            if (_regionID.innerHTML == "Non-Targeted") {
                nonTargetedFlag = true;
            }


            var R_Base_TRx = R_row.findElement("R_TRx").innerHTML;
            R_Base_TRx = R_Base_TRx.replace(",", "");
            var R_Base_MST = R_row.findElement("R_MST").innerHTML;

            // to get Baseline MB_TRx
            var R_Base_MB_TRx;
            if (R_Base_MST > 0)
                R_Base_MB_TRx = parseFloat(parseFloat(R_Base_TRx) * 100 / parseFloat(R_Base_MST)).toFixed(2);
            else
                R_Base_MB_TRx = 0;

            // Goal: TRx
            var R_Goal_TRx_ID = R_row.findControl("R_TRx" + monthI).get_element().id;
            var cell_R_TRx = $find(R_Goal_TRx_ID);

            // increased TRx
            var R_increased_TRx;
            R_increased_TRx = parseFloat(parseFloat(R_Base_TRx) * parseInt(percentI) / 100).toFixed(2);

            var R_Goal_TRx = parseFloat(parseFloat(R_Base_TRx) + parseFloat(R_increased_TRx)).toFixed(2);

            var R_Goal_MB_TRx = parseFloat(parseFloat(R_Base_MB_TRx) + parseFloat(R_increased_TRx)).toFixed(2);
            // Goal: MST
            var R_Goal_MST_ID = R_row.findControl("R_MST" + monthI).get_element().id;
            var cell_R_MST = $find(R_Goal_MST_ID);

            var R_Goal_MST;
            if (R_Goal_MB_TRx > 0)
                R_Goal_MST = parseFloat(parseFloat(R_Goal_TRx) * 100 / parseFloat(R_Goal_MB_TRx)).toFixed(2);
            else
                R_Goal_MST = 0;

            if (cell_R_TRx != null && cell_R_MST != null && !nonTargetedFlag) {
                cell_R_TRx.set_value(R_Goal_TRx);   //Region TRx
                cell_R_MST.set_value(R_Goal_MST);   //Region MST
            }



            // District
            if (dataItems[i].get_nestedViews().length > 0) {
                var nestedView = dataItems[i].get_nestedViews()[0];
                //alert(nestedView.get_name());
                var childItems = nestedView.get_dataItems();
                var childItemsCount = nestedView.get_dataItems().length;
                //alert('Child - District Count:  ' + childItemsCount);


                for (var j = 0; j < childItemsCount; j++) {
                    // allocate District TRx, MST
                    var D_row = nestedView.get_dataItems()[j];
                    var D_Base_TRx = D_row.findElement("D_TRx").innerHTML;
                    D_Base_TRx = D_Base_TRx.replace(",", "");
                    var D_Base_MST = D_row.findElement("D_MST").innerHTML;

                    // to get Baseline MB_TRx
                    var D_Base_MB_TRx;
                    if (D_Base_MST > 0)
                        D_Base_MB_TRx = parseFloat(parseFloat(D_Base_TRx) * 100 / parseFloat(D_Base_MST)).toFixed(2);
                    else
                        D_Base_MB_TRx = 0;

                    // Goal: TRx
                    var D_Goal_TRx_ID = D_row.findControl("D_TRx" + monthI).get_element().id;
                    var cell_D_TRx = $find(D_Goal_TRx_ID);

                    // increased TRx
                    var D_increased_TRx;
                    D_increased_TRx = parseFloat(parseFloat(D_Base_TRx) * parseInt(percentI) / 100).toFixed(2);

                    var D_Goal_TRx = parseFloat(parseFloat(D_Base_TRx) + parseFloat(D_increased_TRx)).toFixed(2);
                    var D_Goal_MB_TRx = parseFloat(parseFloat(D_Base_MB_TRx) + parseFloat(D_increased_TRx)).toFixed(2);


                    // Goal: MST
                    var D_Goal_MST_ID = D_row.findControl("D_MST" + monthI).get_element().id;
                    var cell_D_MST = $find(D_Goal_MST_ID);

                    var D_Goal_MST;
                    if (D_Goal_MB_TRx > 0)
                        D_Goal_MST = parseFloat(parseFloat(D_Goal_TRx) * 100 / parseFloat(D_Goal_MB_TRx)).toFixed(2);

                    else
                        D_Goal_MST = 0;


                    if (cell_D_TRx != null && cell_D_MST != null) {
                        cell_D_TRx.set_value(D_Goal_TRx);   //District TRx
                        cell_D_MST.set_value(D_Goal_MST);   //District MST
                        //to restore the orig District TRx( not saved yet) from setting % goal, District TRx id & value is saved in hidden: hChangedList
                        //document.getElementById('<%= hChangedList.ClientID %>').value = document.getElementById('<%= hChangedList.ClientID %>').value + "," + D_Goal_TRx_ID + ":" + D_Goal_TRx + ",";
                    }
                    // Territory
                    if (childItems[j].get_nestedViews().length > 0) {

                        var nestedView2 = childItems[j].get_nestedViews()[0];
                        var childItems2 = nestedView2.get_dataItems();
                        var childItemsCount2 = nestedView2.get_dataItems().length;
                        //alert('Child - Territory Count:  ' + childItemsCount2);


                        //allocate Territory TRx, MST
                        for (var k = 0; k < childItemsCount2; k++) {

                            var T_row = nestedView2.get_dataItems()[k];
                            var T_Base_TRx = T_row.findElement("T_TRx").innerHTML;
                            T_Base_TRx = T_Base_TRx.replace(",", "");
                            var T_Base_MST = T_row.findElement("T_MST").innerHTML;

                            // to get Baseline MB_TRx
                            var T_Base_MB_TRx;
                            if (T_Base_MST > 0)
                                T_Base_MB_TRx = parseFloat(parseFloat(T_Base_TRx) * 100 / parseFloat(T_Base_MST)).toFixed(2);
                            else
                                T_Base_MB_TRx = 0;

                            // Goal: TRx
                            var T_Goal_TRx_ID = T_row.findControl("T_TRx" + monthI).get_element().id;
                            var cell_T_TRx = $find(T_Goal_TRx_ID);

                            // increased TRx
                            var T_increased_TRx;
                            T_increased_TRx = parseFloat(parseFloat(T_Base_TRx) * parseInt(percentI) / 100).toFixed(2);

                            var T_Goal_TRx = parseFloat(parseFloat(T_Base_TRx) + parseFloat(T_increased_TRx)).toFixed(2);
                            var T_Goal_MB_TRx = parseFloat(parseFloat(T_Base_MB_TRx) + parseFloat(T_increased_TRx)).toFixed(2);

                            // Goal: MST
                            var T_Goal_MST_ID = T_row.findControl("T_MST" + monthI).get_element().id;
                            var cell_T_MST = $find(T_Goal_MST_ID);

                            var T_Goal_MST;
                            if (T_Goal_MB_TRx > 0)
                                T_Goal_MST = parseFloat(parseFloat(T_Goal_TRx) * 100 / parseFloat(T_Goal_MB_TRx)).toFixed(2);

                            else
                                T_Goal_MST = 0;


                            if (cell_T_TRx != null && cell_T_MST != null) {
                                cell_T_TRx.set_value(T_Goal_TRx);   //Territory TRx
                                cell_T_MST.set_value(T_Goal_MST);   //Territory MST
                            }
                        }


                    } // territory

                }  // district: for 


            } // district
        }
    }

    function donotChange(sender, eventArgs) {

        if (document.getElementById('<%= hValidateOK.ClientID %>').value == "N") {
            var oldValue = eventArgs.get_oldValue();
            eventArgs.set_newValue(oldValue);

            //reset
            document.getElementById('<%= hValidateOK.ClientID %>').value = "Y"
        }
    }

    function ReAllocation(parentRegion, percentSaved, oldTRx, oldMST, base_D_TRx, base_D_MST, goal_D_TRx, goal_D_MST, duration, monthI, hierarchyIndex, itemIndex) {
        //        var btnReset = document.getElementById("ctl00_goalButton_btnSubmit");
        //        btnReset.disabled = false;


        //validataion:  to reallocate, percent increment & its calcuulation should be saved first
        if (document.getElementById('<%= hGoalPercentOK.ClientID %>').value == "Y") {
            alert("You can't change the District level data unless you set the goal % Volume Growth and save it first. Please click 'Reset'.");
            var btnReset = document.getElementById("ctl00_goalButton_btnSubmit");
            btnReset.disabled = true;
            
            // sl 3/5/2013 not necessary but requested:  to disable editable textbox
            $(document).ready(function() {
                $("input[type=text]").attr("disabled", true);
            });
           
            return false;
        }

        if (percentSaved == "N") {
            alert("Please set the goal % Volume Growth and save the data first.");
            var cell_D_TRx_obj_1 = $find(goal_D_TRx);
            cell_D_TRx_obj_1.set_value(0);
            return false;
        }


        //alert("hierarchy index: " + hierarchyIndex + ",  item index:  " + itemIndex);

        //////// District: recalculate Goal MST  ///////////////////////////////

        var percentI;
        // to avoid error, check 1st month's object (if object, assuming the other months are objects)
        if (document.getElementById('<%= percentGrowth_month1.ClientID %>') != null) {
            if (monthI == 1)
                percentI = document.getElementById('<%= percentGrowth_month1.ClientID %>').value;
            else if (monthI == 2)
                percentI = document.getElementById('<%= percentGrowth_month2.ClientID %>').value;

            else if (monthI == 3)
                percentI = document.getElementById('<%= percentGrowth_month3.ClientID %>').value;

            else if (monthI == 4)
                percentI = document.getElementById('<%= percentGrowth_month4.ClientID %>').value;
            else if (monthI == 5)
                percentI = document.getElementById('<%= percentGrowth_month5.ClientID %>').value;
            else if (monthI == 6)
                percentI = document.getElementById('<%= percentGrowth_month6.ClientID %>').value;

            else if (monthI == 7)
                percentI = document.getElementById('<%= percentGrowth_month7.ClientID %>').value;

            else if (monthI == 8)
                percentI = document.getElementById('<%= percentGrowth_month8.ClientID %>').value;
            else if (monthI == 9)
                percentI = document.getElementById('<%= percentGrowth_month9.ClientID %>').value;
            else if (monthI == 10)
                percentI = document.getElementById('<%= percentGrowth_month10.ClientID %>').value;

            else if (monthI == 11)
                percentI = document.getElementById('<%= percentGrowth_month11.ClientID %>').value;

            else if (monthI == 12)
                percentI = document.getElementById('<%= percentGrowth_month12.ClientID %>').value;
        }


        // validate and restore the orig value: if change the same District after changing (reallocation for the same district is only once allowed, otherwise can't remember the changed value.)
        var changedList = document.getElementById('<%= hChangedList.ClientID %>').value;
        var a = changedList.split(",");
        var val;
        for (var v in a) {
            val = $.trim(a[v]);
            var id = val.split(":")[0];
            var value = val.split(":")[1];


            if (id == goal_D_TRx) {
                alert("You already allocated TRx for the district. Please allocate TRx in other district in the same region.");
                var cell_D_TRx_obj = $find(goal_D_TRx);
                cell_D_TRx_obj.set_value(value);
                return false;
            }
        }



        // validate:  if validator != 0 and try to reallocate the TRx difference in other month
        if (document.getElementById('<%= txtDifference.ClientID %>') != null) {
            var tempIndicator = document.getElementById('<%= txtDifference.ClientID %>').value;
            var tempHiddenMonthID = document.getElementById('<%= hMonthI.ClientID %>').value;
            //alert(tempIndicator + "~ " + tempHiddenMonthID + ", monthI: " + monthI);
            if ((tempIndicator != 0) && (tempHiddenMonthID != monthI)) {

                alert("Please check the 'District TRx Indicator' and re-allocate the value to other district in the same month.");
                document.getElementById('<%= hValidateOK.ClientID %>').value = "N";
                return false;
            }
        }

        //validate:  if validator != 0 and try to reallocate the TRx difference in other region
        if (document.getElementById('<%= hValidator.ClientID %>').value != 0 && document.getElementById('<%= hRegion.ClientID %>').value != parentRegion) {

            alert("Please check the 'District TRx Indicator' and re-allocate the value to other district in the same Region.");
            document.getElementById('<%= hValidateOK.ClientID %>').value = "N";
            return false;
        }

        var D_Goal_TRx_New = $("#" + goal_D_TRx).val();

        if (D_Goal_TRx_New <= 0 || D_Goal_TRx_New == "" || D_Goal_TRx_New == null || percentI <= 0 || percentI == "" || percentI == null) {
            alert("Please enter a numeric value in District TRx.");
            var cell_D_TRx_obj = $find(goal_D_TRx);
            cell_D_TRx_obj.set_value('Not Valid');

            //            var btnReset = document.getElementById("ctl00_goalButton_btnSubmit");
            //            btnReset.disabled = true;

            return false;
        }

        // to avoid error: .3, ..3,  1.3.5,
        var repeatString = 0;
        if (D_Goal_TRx_New.match(/\./g) != null) {
            repeatString = D_Goal_TRx_New.match(/\./g).length;
        }

        if (D_Goal_TRx_New.substring(0, 1) == '.' || repeatString > 1) {
            D_Goal_TRx_New = D_Goal_TRx_New.replace(/\./g, "");
            D_Goal_TRx_New = "0." + D_Goal_TRx_New;
            var cell_D_TRx_obj_2 = $find(goal_D_TRx);
            cell_D_TRx_obj_2.set_value(D_Goal_TRx_New);

        }

        /////////// District: recalculation
        var D_Goal_TRx_Old = oldTRx;
        //alert('D_Goal_TRx_Old: ' + D_Goal_TRx_Old + 'D_Goal_TRx_New' + D_Goal_TRx_New);
        // calculate TRx difference and store in txtDifference indicator
        var D_TRx_difference = parseFloat(parseFloat(D_Goal_TRx_New) - parseFloat(D_Goal_TRx_Old)).toFixed(2);
        var diffIndicator = document.getElementById('<%= txtDifference.ClientID %>').value;
        var newDiff = parseFloat(parseFloat(D_TRx_difference) + parseFloat(diffIndicator)).toFixed(2);

        //alert('newDiff:  ' + newDiff);
        document.getElementById('<%= txtDifference.ClientID %>').value = newDiff;
        document.getElementById('<%= hValidator.ClientID %>').value = newDiff;
        document.getElementById('<%= hMonthI.ClientID %>').value = monthI;
        document.getElementById('<%= hChangedList.ClientID %>').value = document.getElementById('<%= hChangedList.ClientID %>').value + "," + goal_D_TRx + ":" + $("#" + goal_D_TRx).val() + ",";
        document.getElementById('<%= hRegion.ClientID %>').value = parentRegion;
        document.getElementById('<%= hValidateOK.ClientID %>').value = "Y";

        //highlight textbox
        if (document.getElementById('<%= hValidateOK.ClientID %>').value == "Y") {
            HighlightChanged(goal_D_TRx);
        }


        //calculate old MB TRx using old TRx and MST
        var D_Goal_MB_TRx_Old_Calc = parseFloat((D_Goal_TRx_Old) * 100 / parseFloat(oldMST)).toFixed(2);
        var D_TRx_difference = parseFloat(parseFloat(D_Goal_TRx_New) - parseFloat(D_Goal_TRx_Old)).toFixed(2);
        var D_Goal_MB_TRx_New = parseFloat(parseFloat(D_Goal_MB_TRx_Old_Calc) + parseFloat(D_TRx_difference)).toFixed(2);

        // calculate new Goal MST
        var D_Goal_MST_New = parseFloat(parseFloat(D_Goal_TRx_New) * 100 / parseFloat(D_Goal_MB_TRx_New)).toFixed(2);
        var cell_D_MST = $find(goal_D_MST);
        cell_D_MST.set_value(D_Goal_MST_New);

        /////   District recalculation: end /////





        //////// Territory: recalculate Goal TRx, MST  ///////////////////////////////
        var grid = $find('<%= gridViewDistricts.ClientID  %>');

        master = grid.get_masterTableView();
        var dataItems = master.get_dataItems();
        var masterRowCount = master.get_dataItems().length;
        var regionIndex = hierarchyIndex.substr(0, 1);

        if (dataItems[regionIndex].get_nestedViews().length > 0) {

            var nestedView_D = dataItems[regionIndex].get_nestedViews()[0];
            //alert("D get_name: " + nestedView_D.get_name());
            var childItems_D = nestedView_D.get_dataItems();   //District items
            var childItems_D_Count = nestedView_D.get_dataItems().length;
            //alert('Child - District Count:  ' + childItems_D_Count);

            // Territory
            if (childItems_D[itemIndex].get_nestedViews().length > 0) {
                var nestedView_T = childItems_D[itemIndex].get_nestedViews()[0];
                var childItems_T = nestedView_T.get_dataItems();   //Territory items
                var childItems_T_Count = nestedView_T.get_dataItems().length;
                //alert("T name: " + nestedView_T.get_name() + ', Child - Territory Count:  ' + childItems_T_Count);

                //re-allocate Territory TRx, MST
                for (var t = 0; t < childItems_T_Count; t++) {

                    var T_row = nestedView_T.get_dataItems()[t];
                    var T_Goal_TRx_ID = T_row.findControl("T_TRx" + monthI).get_element().id;
                    var T_Goal_TRx_Old = $("#" + T_Goal_TRx_ID).val();

                    var T_Goal_MST_ID = T_row.findControl("T_MST" + monthI).get_element().id;
                    var T_Goal_MST_Old = $("#" + T_Goal_MST_ID).val();


                    // to calculate new Territory TRx(=x):  old District TRx: new District TRx = old Territory TRx: x
                    var T_Goal_TRx_New = parseFloat(parseFloat(D_Goal_TRx_New) * parseFloat(T_Goal_TRx_Old) / parseFloat(D_Goal_TRx_Old)).toFixed(2);
                    //alert("t trx old: " + T_Goal_TRx_Old + ", d trx new: " + D_Goal_TRx_New + ", d trx old: " + D_Goal_TRx_Old);
                    var cell_T_TRx = $find(T_Goal_TRx_ID);
                    //cell_T_TRx.set_value(T_Goal_TRx_New);    //important: hold until MST is calculated. 


                    //Territory: calculate old MB TRx using old TRx and MST
                    var T_Goal_MB_TRx_Old_Calc = parseFloat((T_Goal_TRx_Old) * 100 / parseFloat(T_Goal_MST_Old)).toFixed(2);
                    var T_TRx_difference = parseFloat(parseFloat(T_Goal_TRx_New) - parseFloat(T_Goal_TRx_Old)).toFixed(2);
                    var T_Goal_MB_TRx_New = parseFloat(parseFloat(T_Goal_MB_TRx_Old_Calc) + parseFloat(T_TRx_difference)).toFixed(2);



                    // calculate new Goal MST
                    var T_Goal_MST_New = parseFloat(parseFloat(T_Goal_TRx_New) * 100 / parseFloat(T_Goal_MB_TRx_New)).toFixed(2);
                    var goal_T_MST_ID = T_row.findControl("T_MST" + monthI).get_element().id;

                    //alert('goal_T_MST_ID: ' + goal_T_MST_ID);

                    var cell_T_MST = $find(goal_T_MST_ID);
                    cell_T_MST.set_value(T_Goal_MST_New); //MST
                    cell_T_TRx.set_value(T_Goal_TRx_New); //TRx

                }


            }
        }
    }
    //    function DisableButton(b) {
    //        b.disabled = true;
    //        b.value = 'Submitting';
    //        b.form.submit();
    //    }

    function Blur(sender, eventArgs) {
        alert(sender.get_id());
    }


    function HighlightChanged(objD) {
        var RadNumericTextBox1 = $find(objD);
        RadNumericTextBox1.get_styles().EnabledStyle[0] += "background-color: #FBB36B !important;color: Red  !important; ";
        RadNumericTextBox1.updateCssClass();

    }

    function submitValidate(it) {
        var _validator = document.getElementById('<%= hValidator.ClientID %>').value;
        if (_validator != 0) {
            alert("Please check District TRx Indicator. The value should be 0 to submit.");
            return false;
        }

        else {

            var duration = document.getElementById('<%= hDuration.ClientID %>').value;
            //alert ('duration: ' + duration);

            for (var i = 1; i <= duration; i++) {
                var percentID = "ctl00_main_goals_percentGrowth_month" + i;
                var percentValue = document.getElementById(percentID).value;
                //alert('percentValue: ' + percentValue);

                if (percentValue == "" || percentValue == null) {
                    alert("Please enter a numeric value in '% Volume Growth'.");
                    return false;

                }
            }
            var bFailed = false;

            $("input[type=text]").each(function() {
                if (isNaN($(this).val())) {

                    bFailed = true;
                }
            });

            if (bFailed) {
                alert("Please enter a numeric value in District TRx.");
                return false;
            }


            var btnReset = document.getElementById("reset");
            btnReset.disabled = true;

            //		    it.disabled = true;
            //		    it.value = 'Submitting';

            return true;


        }



    }



    function showDistrictPhysicians(campaignId, dist, start, end) {
        abortResize = true;
        $("#dial").html("Loading...").load("physicians.aspx?id=" + campaignId + "&dist=" + dist + "&start=" + start + "&end=" + end + " form >*").dialog('option', 'title', 'Physician List').dialog('open');
        abortResize = false;
    }

    function showDistrictChart(campaignId, dist, start, end, brandid) {
        abortResize = true;
        $("#dial").html("Loading...").load("districtprofileTRxchart.aspx?id=" + campaignId + "&dist=" + dist + "&start=" + start + "&end=" + end + "&brandid=" + brandid + " form >*").dialog('option', 'title', 'District Profile').dialog('open');
        abortResize = false;
    }
    
    
    
    
        
</script>

<style type="text/css">

 HTML BODY .RadInput_Default .riTextBox
{
	border: none;
	background: transparent ;
	text-align: right; 
}

HTML BODY .RadInput .riTextBox
{
	border: none;
	padding: 0;
	text-align: right;
	background: transparent;
} 


.rgGroupCol
{
    padding-left: 0 !important;
    padding-right: 0 !important;
    font-size:1px !important;
}
 
.rgExpand,
.rgCollapse
{
    display:none !important;
}


</style>

  
<asp:Panel runat="server" ID="panelCampaignInformation">
    <div class="tileContainerHeader">
        <div class="CampaignInfo">
            Campaign Name:
            <asp:Label runat="server" ID="lblPlanName" /></div>
        <%-- <div style="float:right" runat="server" id="targetAdditional">
                        <a style="display:block" href="javascript:showAddDistricts(<%=Request.QueryString["id"]%>)">Target Additional Districts</a>                        
                    </div>--%>
        <%--<div style="float: right">
            <asp:DropDownList runat="server" ID="topNDistricts" AutoPostBack="true" CausesValidation="false"
                Visible="false">
                <asp:ListItem Text="Top 10 Districts" Value="20" />
                <asp:ListItem Text="Top 25 Districts" Value="50" />
            </asp:DropDownList>
        </div>--%>
        <div class="clearAll">
        </div>
    </div>

    <div id="divVolumeGrowth" runat="server" >
    <table style="width: 100%;" >
    <tr><td align="center">
        <table id="tblGoalPercent" runat="server" style="width: 90%;border:solid 1px #990000;" align="center "  >
        <tr align="left"><td>
        <span class="instruction">Step 1: Please set goals by entering a number in '% Volume Growth' Textbox and click 'Submit' to save the allocation first.</span>
        </td></tr>
            <tr>
                <td style="width: 90%; " >
                    <table style="width: 100%" cellpadding="3" cellspacing="3">
                    
                        <tr class="tblBG"  >
                            <td class="tblBG"  >
                                % Volume Growth
                            </td>
                            <td class="tblBG" runat="server"   id="tdGoalHeader1">
                               <asp:Label ID="GoalIncrement1" runat="server" Text="month1"></asp:Label>
                            </td>
                            <td class="tblBG" runat="server"   id="tdGoalHeader2">
                                <asp:Label ID="GoalIncrement2" runat="server" Text="month2"></asp:Label>
                            </td>
                            <td class="tblBG" runat="server"   id="tdGoalHeader3">
                                <asp:Label ID="GoalIncrement3" runat="server" Text="month3"></asp:Label>
                            </td>
                             <td class="tblBG" runat="server"   id="tdGoalHeader4">
                                <asp:Label ID="GoalIncrement4" runat="server" Text="month4"></asp:Label>
                            </td>
                            <td class="tblBG" runat="server"   id="tdGoalHeader5">
                                <asp:Label ID="GoalIncrement5" runat="server" Text="month5"></asp:Label>
                            </td>
                            <td class="tblBG" runat="server"   id="tdGoalHeader6">
                                <asp:Label ID="GoalIncrement6" runat="server" Text="month6"></asp:Label>
                            </td>
                             <td class="tblBG" runat="server"   id="tdGoalHeader7">
                                <asp:Label ID="GoalIncrement7" runat="server" Text="month7"></asp:Label>
                            </td>
                            <td class="tblBG" runat="server"   id="tdGoalHeader8">
                                <asp:Label ID="GoalIncrement8" runat="server" Text="month8"></asp:Label>
                            </td>
                            <td class="tblBG" runat="server"   id="tdGoalHeader9">
                                <asp:Label ID="GoalIncrement9" runat="server" Text="month9"></asp:Label>
                            </td>
                             <td class="tblBG" runat="server"   id="tdGoalHeader10">
                                <asp:Label ID="GoalIncrement10" runat="server" Text="month10"></asp:Label>
                            </td>
                            <td class="tblBG" runat="server"   id="tdGoalHeader11">
                                <asp:Label ID="GoalIncrement11" runat="server" Text="month11"></asp:Label>
                            </td>
                            <td class="tblBG" runat="server"   id="tdGoalHeader12">
                                <asp:Label ID="GoalIncrement12" runat="server" Text="month12"></asp:Label>
                            </td>
                        </tr>
                        <tr class="tblBG_2"   >
                            <td class="tblBG_2"    >
                                &nbsp;
                            </td>
                            <td class="tblBG_2"  runat="server" id = "tdGoalPercent1" >
                                <asp:TextBox Width="38px" onchange="javascript:allocateGoals(this, 1);" ID="percentGrowth_month1"
                                    runat="server" ></asp:TextBox>%
                            </td>
                            <td class="tblBG_2"  runat="server" id = "tdGoalPercent2" >
                                <asp:TextBox Width="38px" onchange="javascript:allocateGoals(this, 2);" ID="percentGrowth_month2"
                                    runat="server"></asp:TextBox>%
                            </td>
                            <td class="tblBG_2"  runat="server" id = "tdGoalPercent3" >
                                <asp:TextBox Width="38px" onchange="javascript:allocateGoals(this, 3);" ID="percentGrowth_month3"
                                    runat="server"></asp:TextBox>%
                            </td>
                            <td class="tblBG_2"  runat="server" id = "tdGoalPercent4" >
                                <asp:TextBox Width="38px" onchange="javascript:allocateGoals(this, 4);" ID="percentGrowth_month4"
                                    runat="server" ></asp:TextBox>%
                            </td>
                            <td class="tblBG_2"  runat="server" id = "tdGoalPercent5" >
                                <asp:TextBox Width="38px" onchange="javascript:allocateGoals(this, 5);" ID="percentGrowth_month5"
                                    runat="server"></asp:TextBox>%
                            </td>
                            <td class="tblBG_2"  runat="server" id = "tdGoalPercent6" >
                                <asp:TextBox Width="38px" onchange="javascript:allocateGoals(this, 6);" ID="percentGrowth_month6"
                                    runat="server"></asp:TextBox>%
                            </td>
                            <td class="tblBG_2"  runat="server"  id = "tdGoalPercent7">
                                <asp:TextBox Width="38px" onchange="javascript:allocateGoals(this, 7);" ID="percentGrowth_month7"
                                    runat="server" ></asp:TextBox>%
                            </td>
                            <td class="tblBG_2"  runat="server" id = "tdGoalPercent8" >
                                <asp:TextBox Width="38px" onchange="javascript:allocateGoals(this, 8);" ID="percentGrowth_month8"
                                    runat="server"></asp:TextBox>%
                            </td>
                            <td class="tblBG_2"  runat="server" id = "tdGoalPercent9" >
                                <asp:TextBox Width="38px" onchange="javascript:allocateGoals(this, 9);" ID="percentGrowth_month9"
                                    runat="server"></asp:TextBox>%
                            </td>
                            <td class="tblBG_2"  runat="server" id = "tdGoalPercent10" >
                                <asp:TextBox Width="38px" onchange="javascript:allocateGoals(this, 10);" ID="percentGrowth_month10"
                                    runat="server" ></asp:TextBox>%
                            </td>
                            <td class="tblBG_2"  runat="server" id = "tdGoalPercent11" >
                                <asp:TextBox Width="38px" onchange="javascript:allocateGoals(this, 11);" ID="percentGrowth_month11"
                                    runat="server"></asp:TextBox>%
                            </td>
                            <td class="tblBG_2"  runat="server"  id = "tdGoalPercent12">
                                <asp:TextBox Width="38px" onchange="javascript:allocateGoals(this, 12);" ID="percentGrowth_month12"
                                    runat="server"></asp:TextBox>%
                            </td>
                        </tr>
                    </table>
                </td>
                <%--<td valign="bottom" align="center" style="width: 20%">
                    <asp:Button ID="btnSaveAllocation" runat="server" Text="Save Allocation"
                         OnClientClick="this.disabled='true';this.value='Saving....';"
                        UseSubmitBehavior="false" />
                </td>--%>
            </tr>
           
        </table>
        </td></tr>
    </table>
        <br />
    </div>

</asp:Panel>

<%--<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">  
            <AjaxSettings>  
                <telerik:AjaxSetting  AjaxControlID="gridViewDistricts">  
                    <UpdatedControls>  
                        <telerik:AjaxUpdatedControl  ControlID="gridViewDistricts"  />  
                    </UpdatedControls>  
                </telerik:AjaxSetting>  
            </AjaxSettings>  
        </telerik:RadAjaxManager> --%>

<%--  Plan Goals: begin --%>   
<div class="tileSubHeader">Plan Goals</div>
<telerik:RadGrid CssClass="topGrid" EnableEmbeddedSkins="false" 
    EnableEmbeddedBaseStylesheet="false" runat="server" ID="gridView"
    AutoGenerateColumns="false"  OnItemCreated="gridView_ItemCreated"
      OnPreRender="gridView_PreRender"  
    AllowSorting="false" AllowPaging="false" 
    onitemdatabound="gridView_ItemDataBound" Width="100%" >
    <PagerStyle Position="Top" />
    <MasterTableView AllowSorting="false" DataKeyNames="Campaign_ID,Plan_ID,Brand_ID">
        <Columns>
            <telerik:GridBoundColumn HeaderText="Plan" DataField="Plan_Name" HeaderStyle-CssClass="headerTh_plan"
                UniqueName="Plan_ID" ItemStyle-CssClass="headerTh_item_plan" />
            <%-- 1/15/2013 added 'Brand_Name' --%>
            <telerik:GridTemplateColumn UniqueName="BaseCol_Plan_Comp" HeaderStyle-CssClass="headerTh_Comp"
                 ItemStyle-CssClass="headerTh_item_Comp" >
                <HeaderTemplate>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="top">
                                Brand
                            </td>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="alignLeft">
                                <asp:Label ID="Plan_Brand_Name" runat="server" CssClass="cssTRx_D" Text='<%# Eval("Brand_Name") %>'>
                                </asp:Label>
                            </td>
                        </tr>
                        <%-- testing: Competitor --%>
                        <tr>
                            <td class="alignLeft" style="border-top: solid 1px #ccc;">
                                Competitors
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            <telerik:GridTemplateColumn UniqueName="BaseCol" HeaderStyle-CssClass="baseline_header_plan"
                ItemStyle-CssClass="baseline_item_plan">
                <HeaderTemplate>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td colspan="2" class="top">
                                Baseline
                            </td>
                        </tr>
                        <tr>
                            <td class="left" style="width: 50%; border-right: solid 1px #ccc;">
                                TRx
                            </td>
                            <td class="right" style="width: 50%">
                                MS
                            </td>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="alignRight" style="width: 50%; border-right: solid 1px #ccc;">
                                <asp:Label ID="TRx" runat="server" CssClass="cssTRx_Plan" Text='<%# String.Format("{0:n0}",Eval("TRx")) %>'>
                                </asp:Label>
                            </td>
                            <td class="alignRight" style="width: 50%">
                                <asp:Label ID="MST" runat="server" CssClass="cssMST_Plan" Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("MST"), "{0:n2}")%>'>
                                </asp:Label>
                            </td>
                        </tr>
                        
                        
					  <%-- testing: Competitor --%>
                        <tr>
                            <td  class="alignRight"   style="width: 50%; border-right: solid 1px #ccc;border-top: solid 1px #ccc;">
                                <asp:Label ID="Comp_Plan_TRx" runat="server" CssClass="cssTRx_Plan" Text='<%# String.Format("{0:n0}",Eval("Comp_Plan_TRx")) %>'></asp:Label>
                            </td>
                            <td class="alignRight"  style="width: 50%; border-top: solid 1px #ccc;">
                                <asp:Label ID="Comp_Plan_MST" runat="server" CssClass="cssMST_Plan" Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("Comp_Plan_MST"), "{0:n2}")%>'></asp:Label>
                            </td>
                        </tr>
                        
                        
                    </table>
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            <%-- : 12 month GridTemplateColumn --%>
            <%--month1--%>
            <telerik:GridTemplateColumn UniqueName="month1" HeaderStyle-CssClass="rightScroll"
                ItemStyle-CssClass="rightScroll">
                <HeaderTemplate>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td id="Td1"  colspan="2" class="top" runat="server">
                                <asp:Label ID="header_month1" runat="server" Text="month1"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="left" style="width: 50%; border-right: solid 1px #ccc;">
                                TRx
                            </td>
                            <td class="right" style="width: 50%">
                                MS
                            </td>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="alignRight" style="width: 50%; border-right: solid 1px #ccc;">
                                <telerik:RadTextBox Width="27px" ID="TRx1" runat="server" CssClass="cssTRx1" Text='<%# String.Format("{0:n0}",Eval("TRx1")) %>'>
                                </telerik:RadTextBox>
                            </td>
                            <td class="alignRight" style="width: 50%">
                                <telerik:RadTextBox Width="27px" ID="MST1" runat="server" CssClass="cssMST1" Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("MST1"), "{0:n2}")%>'>
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            <%--month2--%>
            <telerik:GridTemplateColumn UniqueName="month2" HeaderStyle-CssClass="rightScroll"
                ItemStyle-CssClass="rightScroll">
                <HeaderTemplate>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td id="Td2"  colspan="2" class="top" runat="server">
                                <asp:Label ID="header_month2" runat="server" Text="month2"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="left" style="width: 50%; border-right: solid 1px #ccc;">
                                TRx
                            </td>
                            <td class="right" style="width: 50%">
                                MS
                            </td>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="alignRight" style="width: 50%; border-right: solid 1px #ccc;">
                                <telerik:RadTextBox Width="27px" ID="TRx2" runat="server" CssClass="cssTRx2" Text='<%# String.Format("{0:n0}",Eval("TRx2")) %>'>
                                </telerik:RadTextBox>
                            </td>
                            <td class="alignRight" style="width: 50%">
                                <telerik:RadTextBox Width="27px" ID="MST2" runat="server" CssClass="cssMST2" Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("MST2"), "{0:n2}")%>'>
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            <%--month3--%>
            <telerik:GridTemplateColumn UniqueName="month3" HeaderStyle-CssClass="rightScroll"
                ItemStyle-CssClass="rightScroll">
                <HeaderTemplate>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td id="Td3"  colspan="2" class="top" runat="server">
                                <asp:Label ID="header_month3" runat="server" Text="month3"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="left" style="width: 50%; border-right: solid 1px #ccc;">
                                TRx
                            </td>
                            <td class="right" style="width: 50%">
                                MS
                            </td>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="alignRight" style="width: 50%; border-right: solid 1px #ccc;">
                                <telerik:RadTextBox Width="27px" ID="TRx3" runat="server" CssClass="cssTRx3" Text='<%# String.Format("{0:n0}",Eval("TRx3")) %>'>
                                </telerik:RadTextBox>
                            </td>
                            <td class="alignRight" style="width: 50%">
                                <telerik:RadTextBox Width="27px" ID="MST3" runat="server" CssClass="cssMST3" Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("MST3"), "{0:n2}")%>'>
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </telerik:GridTemplateColumn>
             <%--month4--%>
            <telerik:GridTemplateColumn UniqueName="month4" HeaderStyle-CssClass="rightScroll"
                ItemStyle-CssClass="rightScroll">
                <HeaderTemplate>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td id="Td4"  colspan="2" class="top" runat="server">
                                <asp:Label ID="header_month4" runat="server" Text="month4"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="left" style="width: 50%; border-right: solid 1px #ccc;">
                                TRx
                            </td>
                            <td class="right" style="width: 50%">
                                MS
                            </td>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="alignRight" style="width: 50%; border-right: solid 1px #ccc;">
                                <telerik:RadTextBox Width="27px" ID="TRx4" runat="server" CssClass="cssTRx4" Text='<%# String.Format("{0:n0}",Eval("TRx4")) %>'>
                                </telerik:RadTextBox>
                            </td>
                            <td class="alignRight" style="width: 50%">
                                <telerik:RadTextBox Width="27px" ID="MST4" runat="server" CssClass="cssMST4" Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("MST4"), "{0:n2}")%>'>
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            
             <%--month5--%>
            <telerik:GridTemplateColumn UniqueName="month5" HeaderStyle-CssClass="rightScroll"
                ItemStyle-CssClass="rightScroll">
                <HeaderTemplate>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td id="Td5" colspan="2" class="top" runat="server">
                                <asp:Label ID="header_month5" runat="server" Text="month5"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="left" style="width: 50%; border-right: solid 1px #ccc;">
                                TRx
                            </td>
                            <td class="right" style="width: 50%">
                                MS
                            </td>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="alignRight" style="width: 50%; border-right: solid 1px #ccc;">
                                <telerik:RadTextBox Width="27px" ID="TRx5" runat="server" CssClass="cssTRx5" Text='<%# String.Format("{0:n0}",Eval("TRx5")) %>'>
                                </telerik:RadTextBox>
                            </td>
                            <td class="alignRight" style="width: 50%">
                                <telerik:RadTextBox Width="27px" ID="MST5" runat="server" CssClass="cssMST5" Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("MST5"), "{0:n2}")%>'>
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            
             <%--month6--%>
            <telerik:GridTemplateColumn UniqueName="month6" HeaderStyle-CssClass="rightScroll"
                ItemStyle-CssClass="rightScroll">
                <HeaderTemplate>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td id="Td6" colspan="2" class="top" runat="server">
                                <asp:Label ID="header_month6" runat="server" Text="month6"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="left" style="width: 50%; border-right: solid 1px #ccc;">
                                TRx
                            </td>
                            <td class="right" style="width: 50%">
                                MS
                            </td>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="alignRight" style="width: 50%; border-right: solid 1px #ccc;">
                                <telerik:RadTextBox Width="27px" ID="TRx6" runat="server" CssClass="cssTRx6" Text='<%# String.Format("{0:n0}",Eval("TRx6")) %>'>
                                </telerik:RadTextBox>
                            </td>
                            <td class="alignRight" style="width: 50%">
                                <telerik:RadTextBox Width="27px" ID="MST6" runat="server" CssClass="cssMST6" Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("MST6"), "{0:n2}")%>'>
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            
             <%--month7--%>
            <telerik:GridTemplateColumn UniqueName="month7" HeaderStyle-CssClass="rightScroll"
                ItemStyle-CssClass="rightScroll">
                <HeaderTemplate>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td id="Td7" colspan="2" class="top" runat="server">
                                <asp:Label ID="header_month7" runat="server" Text="month7"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="left" style="width: 50%; border-right: solid 1px #ccc;">
                                TRx
                            </td>
                            <td class="right" style="width: 50%">
                                MS
                            </td>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="alignRight" style="width: 50%; border-right: solid 1px #ccc;">
                                <telerik:RadTextBox Width="27px" ID="TRx7" runat="server" CssClass="cssTRx7" Text='<%# String.Format("{0:n0}",Eval("TRx7")) %>'>
                                </telerik:RadTextBox>
                            </td>
                            <td class="alignRight" style="width: 50%">
                                <telerik:RadTextBox Width="27px" ID="MST7" runat="server" CssClass="cssMST7" Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("MST7"), "{0:n2}")%>'>
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </telerik:GridTemplateColumn>
             <%--month8--%>
            <telerik:GridTemplateColumn UniqueName="month8" HeaderStyle-CssClass="rightScroll"
                ItemStyle-CssClass="rightScroll">
                <HeaderTemplate>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td id="Td8" colspan="2" class="top" runat="server">
                                <asp:Label ID="header_month8" runat="server" Text="month8"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="left" style="width: 50%; border-right: solid 1px #ccc;">
                                TRx
                            </td>
                            <td class="right" style="width: 50%">
                                MS
                            </td>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="alignRight" style="width: 50%; border-right: solid 1px #ccc;">
                                <telerik:RadTextBox Width="27px" ID="TRx8" runat="server" CssClass="cssTRx8" Text='<%# String.Format("{0:n0}",Eval("TRx8")) %>'>
                                </telerik:RadTextBox>
                            </td>
                            <td class="alignRight" style="width: 50%">
                                <telerik:RadTextBox Width="27px" ID="MST8" runat="server" CssClass="cssMST8" Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("MST8"), "{0:n2}")%>'>
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            
             <%--month9--%>
            <telerik:GridTemplateColumn UniqueName="month9" HeaderStyle-CssClass="rightScroll"
                ItemStyle-CssClass="rightScroll">
                <HeaderTemplate>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td id="Td9" colspan="2" class="top" runat="server">
                                <asp:Label ID="header_month9" runat="server" Text="month9"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="left" style="width: 50%; border-right: solid 1px #ccc;">
                                TRx
                            </td>
                            <td class="right" style="width: 50%">
                                MS
                            </td>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="alignRight" style="width: 50%; border-right: solid 1px #ccc;">
                                <telerik:RadTextBox Width="27px" ID="TRx9" runat="server" CssClass="cssTRx9" Text='<%# String.Format("{0:n0}",Eval("TRx9")) %>'>
                                </telerik:RadTextBox>
                            </td>
                            <td class="alignRight" style="width: 50%">
                                <telerik:RadTextBox Width="27px" ID="MST9" runat="server" CssClass="cssMST9" Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("MST9"), "{0:n2}")%>'>
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            
             <%--month10--%>
            <telerik:GridTemplateColumn UniqueName="month10" HeaderStyle-CssClass="rightScroll"
                ItemStyle-CssClass="rightScroll">
                <HeaderTemplate>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td id="Td10" colspan="2" class="top" runat="server">
                                <asp:Label ID="header_month10" runat="server" Text="month10"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="left" style="width: 50%; border-right: solid 1px #ccc;">
                                TRx
                            </td>
                            <td class="right" style="width: 50%">
                                MS
                            </td>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="alignRight" style="width: 50%; border-right: solid 1px #ccc;">
                                <telerik:RadTextBox Width="27px" ID="TRx10" runat="server" CssClass="cssTRx10" Text='<%# String.Format("{0:n0}",Eval("TRx10")) %>'>
                                </telerik:RadTextBox>
                            </td>
                            <td class="alignRight" style="width: 50%">
                                <telerik:RadTextBox Width="27px" ID="MST10" runat="server" CssClass="cssMST10" Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("MST10"), "{0:n2}")%>'>
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            
             <%--month11--%>
            <telerik:GridTemplateColumn UniqueName="month11" HeaderStyle-CssClass="rightScroll"
                ItemStyle-CssClass="rightScroll">
                <HeaderTemplate>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td id="Td11" colspan="2" class="top" runat="server">
                                <asp:Label ID="header_month11" runat="server" Text="month11"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="left" style="width: 50%; border-right: solid 1px #ccc;">
                                TRx
                            </td>
                            <td class="right" style="width: 50%">
                                MS
                            </td>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="alignRight" style="width: 50%; border-right: solid 1px #ccc;">
                                <telerik:RadTextBox Width="27px" ID="TRx11" runat="server" CssClass="cssTRx11" Text='<%# String.Format("{0:n0}",Eval("TRx11")) %>'>
                                </telerik:RadTextBox>
                            </td>
                            <td class="alignRight" style="width: 50%">
                                <telerik:RadTextBox Width="27px" ID="MST11" runat="server" CssClass="cssMST11" Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("MST11"), "{0:n2}")%>'>
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            
             <%--month12--%>
            <telerik:GridTemplateColumn UniqueName="month12" HeaderStyle-CssClass="rightScroll"
                ItemStyle-CssClass="rightScroll">
                <HeaderTemplate>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td id="Td12" colspan="2" class="top" runat="server">
                                <asp:Label ID="header_month12" runat="server" Text="month12"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="left" style="width: 50%; border-right: solid 1px #ccc;">
                                TRx
                            </td>
                            <td class="right" style="width: 50%">
                                MS
                            </td>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="alignRight" style="width: 50%; border-right: solid 1px #ccc;">
                                <telerik:RadTextBox Width="27px" ID="TRx12" runat="server" CssClass="cssTRx12" Text='<%# String.Format("{0:n0}",Eval("TRx12")) %>'>
                                </telerik:RadTextBox>
                            </td>
                            <td class="alignRight" style="width: 50%">
                                <telerik:RadTextBox Width="27px" ID="MST12" runat="server" CssClass="cssMST12" Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("MST12"), "{0:n2}")%>'>
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            
            
            
        </Columns>
    </MasterTableView>
    <ClientSettings Scrolling-AllowScroll="true" Scrolling-UseStaticHeaders="true" Scrolling-FrozenColumnsCount="3" />

</telerik:RadGrid>
          
<%--  Plan Goals: end --%> 

  
<%--  Region/District/Territory Goals: begin --%> 
<div class="tileSubHeader">Region/District/Territory Goals

<span id="spanIndicator" runat="server" style="float:right;">
<asp:HiddenField ID ="hMonthI" runat="server" Value="0" />
<asp:HiddenField ID = "hDuration" runat="server" Value="0" />
<asp:HiddenField ID="hValidator" runat="server" Value="0" />
<asp:HiddenField ID="hGoalPercentOK" runat="server" Value="" />
<asp:HiddenField ID="hValidateOK" runat="server" Value="" />
<asp:HiddenField ID="hChangedList" runat="server" Value="" />
<asp:Label ID="lblRegion" runat="server" Text="Region Indicator: "></asp:Label>
<asp:TextBox ID="hRegion" Width="80px" runat="server" Value="" ReadOnly="true" BackColor="#FBB36B"></asp:TextBox> 
<asp:Label ID="lblDifference" runat="server" Text="District TRx Indicator: " ></asp:Label>
<asp:TextBox ID="txtDifference" Width="27px" runat="server" Text="0" ReadOnly="true" BackColor="#FBB36B" ></asp:TextBox>
</span>
</div>

<telerik:RadGrid CssClass="btmGrid" EnableEmbeddedSkins="false"
 EnableEmbeddedBaseStylesheet="false" 
    runat="server" ID="gridViewDistricts" AutoGenerateColumns="false" 
    AllowSorting="false" OnItemDataBound="gridViewDistricts_OnItemDataBound" 
    OnPreRender="gridViewDistricts_PreRender" 
      OnItemCommand="gridViewDistricts_ItemCommand"
       OnColumnCreated="gridViewDistricts_ColumnCreated"
       OnDetailTableDataBind="gridViewDistricts_DetailTableDataBind" OnNeedDataSource="gridViewDistricts_NeedDataSource"   
      AllowPaging="false" ClientSettings-Scrolling-AllowScroll="true" >
   
  <%--      --%>
   
    <MasterTableView DataKeyNames="Campaign_ID,Plan_ID,Brand_ID, Region_ID" HierarchyDefaultExpanded="true"
         HierarchyLoadMode="Client"    >
        <ItemStyle CssClass="R_ItemStyle" />
       <AlternatingItemStyle CssClass="R_ItemStyle_Alt" />
        <%-- District --%>
        <DetailTables>
            <telerik:GridTableView DataKeyNames="Campaign_ID,Plan_ID,Brand_ID,District_ID" Name="DistrictDetails"
                 runat="server" CssClass="DTable" HierarchyLoadMode="Client">
                <%--<ParentTableRelation>
                    <telerik:GridRelationFields DetailKeyField="District_ID" MasterKeyField="Region_ID" />
                </ParentTableRelation> --%>
                <%--    <ExpandCollapseColumn CollapseImageUrl="../App_Themes/impact/images/arwRt.png" ExpandImageUrl="../App_Themes/impact/images/arwDwn.png" >
                        </ExpandCollapseColumn>--%>
                <%-- Territory: Begin --%>
                <DetailTables>
                    <telerik:GridTableView DataKeyNames="Campaign_ID,Plan_ID,Brand_ID, Territory_ID"
                        CssClass="TTable" Name="TerritoryDetails" HierarchyLoadMode="Client" runat="server">
                        <%--<ParentTableRelation>
                            <telerik:GridRelationFields DetailKeyField="Territory_ID" MasterKeyField="District_ID" />
                        </ParentTableRelation>--%>
                        <%--         <ExpandCollapseColumn CollapseImageUrl="../App_Themes/impact/images/arwRt.png" ExpandImageUrl="../App_Themes/impact/images/arwDwn.png" >
                        </ExpandCollapseColumn>--%>
                        <Columns>
                            <telerik:GridBoundColumn HeaderText="Territory" DataField="Territory_ID" HeaderStyle-CssClass="headerTh_T"
                                ItemStyle-CssClass="merged_T" UniqueName="Territory_ID" />
                            <telerik:GridTemplateColumn UniqueName="BaseCol_T" HeaderStyle-CssClass="baseline_header_T"
                                ItemStyle-CssClass="baseline_item_T">
                                <HeaderTemplate>
                                    <table width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td colspan="2" class="top">
                                                Baseline
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="left" style="width: 50%; border-right: solid 1px #ccc;">
                                                TRx
                                            </td>
                                            <td class="right" style="width: 50%">
                                                MS
                                            </td>
                                        </tr>
                                    </table>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <table width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td class="alignRight" style="width: 50%; border-right: solid 1px #ccc;">
                                                <asp:Label ID="T_TRx" runat="server" CssClass="cssTRx_T" Text='<%# String.Format("{0:n0}", Eval("T_TRx")) %>'></asp:Label>
                                            </td>
                                            <td class="alignRight" style="width: 50%">
                                                <asp:Label ID="T_MST" runat="server" CssClass="cssMST_T" Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("T_MST"), "{0:n2}")%>'></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <%-- T_: 12 month GridTemplateColumn --%>
                            <%--T_month1--%>
                            <telerik:GridTemplateColumn UniqueName="T_month1" HeaderStyle-CssClass="rightScroll_T"
                                ItemStyle-CssClass="rightScroll">
                                <HeaderTemplate>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td id="Td13"  colspan="2" class="top" runat="server">
                                                <asp:Label ID="T_header_month1" runat="server" Text="month1"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="left" style="width: 50%; border-right: solid 1px #ccc;">
                                                TRx
                                            </td>
                                            <td class="right" style="width: 50%">
                                                MS
                                            </td>
                                        </tr>
                                    </table>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td class="alignRight" style="width: 50%; border-right: solid 1px #ccc;">
                                                <telerik:RadTextBox Width="27px" ID="T_TRx1" runat="server" CssClass="T_cssTRx"
                                                    Text='<%# String.Format("{0:n0}", Eval("T_TRx1")) %>'>
                                                </telerik:RadTextBox>
                                            </td>
                                            <td class="alignRight" style="width: 50%">
                                                <telerik:RadTextBox Width="27px" ID="T_MST1" runat="server" CssClass="T_cssMST"
                                                    Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("T_MST1"), "{0:n2}")%>'>
                                                </telerik:RadTextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <%--T_month2--%>
                            <telerik:GridTemplateColumn UniqueName="T_month2" HeaderStyle-CssClass="rightScroll_T"
                                ItemStyle-CssClass="rightScroll">
                                <HeaderTemplate>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td id="Td5" colspan="2" class="top" runat="server">
                                                <asp:Label ID="T_header_month2" runat="server" Text="month2"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="left" style="width: 50%; border-right: solid 1px #ccc;">
                                                TRx
                                            </td>
                                            <td class="right" style="width: 50%">
                                                MS
                                            </td>
                                        </tr>
                                    </table>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td class="alignRight" style="width: 50%; border-right: solid 1px #ccc;">
                                                <telerik:RadTextBox Width="27px" ID="T_TRx2" runat="server" CssClass="T_cssTRx"
                                                    Text='<%# String.Format("{0:n0}", Eval("T_TRx2")) %>'>
                                                </telerik:RadTextBox>
                                            </td>
                                            <td class="alignRight" style="width: 50%">
                                                <telerik:RadTextBox Width="27px" ID="T_MST2" runat="server" CssClass="T_cssMST"
                                                    Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("T_MST2"), "{0:n2}")%>'>
                                                </telerik:RadTextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <%--T_month3--%>
                            <telerik:GridTemplateColumn UniqueName="T_month3" HeaderStyle-CssClass="rightScroll_T"
                                ItemStyle-CssClass="rightScroll">
                                <HeaderTemplate>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td id="Td6" colspan="2" class="top" runat="server">
                                                <asp:Label ID="T_header_month3" runat="server" Text="month3"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="left" style="width: 50%; border-right: solid 1px #ccc;">
                                                TRx
                                            </td>
                                            <td class="right" style="width: 50%">
                                                MS
                                            </td>
                                        </tr>
                                    </table>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td class="alignRight" style="width: 50%; border-right: solid 1px #ccc;">
                                                <telerik:RadTextBox Width="27px" ID="T_TRx3" runat="server" CssClass="T_cssTRx"
                                                    Text='<%# String.Format("{0:n0}", Eval("T_TRx3")) %>'>
                                                </telerik:RadTextBox>
                                            </td>
                                            <td class="alignRight" style="width: 50%">
                                                <telerik:RadTextBox Width="27px" ID="T_MST3" runat="server" CssClass="T_cssMST"
                                                    Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("T_MST3"), "{0:n2}")%>'>
                                                </telerik:RadTextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <%--T_month4--%>
                            <telerik:GridTemplateColumn UniqueName="T_month4" HeaderStyle-CssClass="rightScroll_T"
                                ItemStyle-CssClass="rightScroll">
                                <HeaderTemplate>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td id="Td7" colspan="2" class="top" runat="server">
                                                <asp:Label ID="T_header_month4" runat="server" Text="month4"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="left" style="width: 50%; border-right: solid 1px #ccc;">
                                                TRx
                                            </td>
                                            <td class="right" style="width: 50%">
                                                MS
                                            </td>
                                        </tr>
                                    </table>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td class="alignRight" style="width: 50%; border-right: solid 1px #ccc;">
                                                <telerik:RadTextBox Width="27px" ID="T_TRx4" runat="server" CssClass="T_cssTRx"
                                                    Text='<%# String.Format("{0:n0}", Eval("T_TRx4")) %>'>
                                                </telerik:RadTextBox>
                                            </td>
                                            <td class="alignRight" style="width: 50%">
                                                <telerik:RadTextBox Width="27px" ID="T_MST4" runat="server" CssClass="T_cssMST"
                                                    Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("T_MST4"), "{0:n2}")%>'>
                                                </telerik:RadTextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                              <%--T_month5--%>
                            <telerik:GridTemplateColumn UniqueName="T_month5" HeaderStyle-CssClass="rightScroll_T"
                                ItemStyle-CssClass="rightScroll">
                                <HeaderTemplate>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td id="Td14"  colspan="2" class="top" runat="server">
                                                <asp:Label ID="T_header_month5" runat="server" Text="month5"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="left" style="width: 50%; border-right: solid 1px #ccc;">
                                                TRx
                                            </td>
                                            <td class="right" style="width: 50%">
                                                MS
                                            </td>
                                        </tr>
                                    </table>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td class="alignRight" style="width: 50%; border-right: solid 1px #ccc;">
                                                <telerik:RadTextBox Width="27px" ID="T_TRx5" runat="server" CssClass="T_cssTRx"
                                                    Text='<%# String.Format("{0:n0}", Eval("T_TRx5")) %>'>
                                                </telerik:RadTextBox>
                                            </td>
                                            <td class="alignRight" style="width: 50%">
                                                <telerik:RadTextBox Width="27px" ID="T_MST5" runat="server" CssClass="T_cssMST"
                                                    Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("T_MST5"), "{0:n2}")%>'>
                                                </telerik:RadTextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                              <%--T_month6--%>
                            <telerik:GridTemplateColumn UniqueName="T_month6" HeaderStyle-CssClass="rightScroll_T"
                                ItemStyle-CssClass="rightScroll">
                                <HeaderTemplate>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td id="Td15"  colspan="2" class="top" runat="server">
                                                <asp:Label ID="T_header_month6" runat="server" Text="month6"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="left" style="width: 50%; border-right: solid 1px #ccc;">
                                                TRx
                                            </td>
                                            <td class="right" style="width: 50%">
                                                MS
                                            </td>
                                        </tr>
                                    </table>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td class="alignRight" style="width: 50%; border-right: solid 1px #ccc;">
                                                <telerik:RadTextBox Width="27px" ID="T_TRx6" runat="server" CssClass="T_cssTRx"
                                                    Text='<%# String.Format("{0:n0}", Eval("T_TRx6")) %>'>
                                                </telerik:RadTextBox>
                                            </td>
                                            <td class="alignRight" style="width: 50%">
                                                <telerik:RadTextBox Width="27px" ID="T_MST6" runat="server" CssClass="T_cssMST"
                                                    Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("T_MST6"), "{0:n2}")%>'>
                                                </telerik:RadTextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                              <%--T_month7--%>
                            <telerik:GridTemplateColumn UniqueName="T_month7" HeaderStyle-CssClass="rightScroll_T"
                                ItemStyle-CssClass="rightScroll">
                                <HeaderTemplate>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td id="Td16"  colspan="2" class="top" runat="server">
                                                <asp:Label ID="T_header_month7" runat="server" Text="month7"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="left" style="width: 50%; border-right: solid 1px #ccc;">
                                                TRx
                                            </td>
                                            <td class="right" style="width: 50%">
                                                MS
                                            </td>
                                        </tr>
                                    </table>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td class="alignRight" style="width: 50%; border-right: solid 1px #ccc;">
                                                <telerik:RadTextBox Width="27px" ID="T_TRx7" runat="server" CssClass="T_cssTRx"
                                                    Text='<%# String.Format("{0:n0}", Eval("T_TRx7")) %>'>
                                                </telerik:RadTextBox>
                                            </td>
                                            <td class="alignRight" style="width: 50%">
                                                <telerik:RadTextBox Width="27px" ID="T_MST7" runat="server" CssClass="T_cssMST"
                                                    Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("T_MST7"), "{0:n2}")%>'>
                                                </telerik:RadTextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                              <%--T_month8--%>
                            <telerik:GridTemplateColumn UniqueName="T_month8" HeaderStyle-CssClass="rightScroll_T"
                                ItemStyle-CssClass="rightScroll">
                                <HeaderTemplate>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td id="Td17"  colspan="2" class="top" runat="server">
                                                <asp:Label ID="T_header_month8" runat="server" Text="month8"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="left" style="width: 50%; border-right: solid 1px #ccc;">
                                                TRx
                                            </td>
                                            <td class="right" style="width: 50%">
                                                MS
                                            </td>
                                        </tr>
                                    </table>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td class="alignRight" style="width: 50%; border-right: solid 1px #ccc;">
                                                <telerik:RadTextBox Width="27px" ID="T_TRx8" runat="server" CssClass="T_cssTRx"
                                                    Text='<%# String.Format("{0:n0}", Eval("T_TRx8")) %>'>
                                                </telerik:RadTextBox>
                                            </td>
                                            <td class="alignRight" style="width: 50%">
                                                <telerik:RadTextBox Width="27px" ID="T_MST8" runat="server" CssClass="T_cssMST"
                                                    Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("T_MST8"), "{0:n2}")%>'>
                                                </telerik:RadTextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                              <%--T_month9--%>
                            <telerik:GridTemplateColumn UniqueName="T_month9" HeaderStyle-CssClass="rightScroll_T"
                                ItemStyle-CssClass="rightScroll">
                                <HeaderTemplate>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td id="Td18"  colspan="2" class="top" runat="server">
                                                <asp:Label ID="T_header_month9" runat="server" Text="month9"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="left" style="width: 50%; border-right: solid 1px #ccc;">
                                                TRx
                                            </td>
                                            <td class="right" style="width: 50%">
                                                MS
                                            </td>
                                        </tr>
                                    </table>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td class="alignRight" style="width: 50%; border-right: solid 1px #ccc;">
                                                <telerik:RadTextBox Width="27px" ID="T_TRx9" runat="server" CssClass="T_cssTRx"
                                                    Text='<%# String.Format("{0:n0}", Eval("T_TRx9")) %>'>
                                                </telerik:RadTextBox>
                                            </td>
                                            <td class="alignRight" style="width: 50%">
                                                <telerik:RadTextBox Width="27px" ID="T_MST9" runat="server" CssClass="T_cssMST"
                                                    Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("T_MST9"), "{0:n2}")%>'>
                                                </telerik:RadTextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                              <%--T_month10--%>
                            <telerik:GridTemplateColumn UniqueName="T_month10" HeaderStyle-CssClass="rightScroll_T"
                                ItemStyle-CssClass="rightScroll">
                                <HeaderTemplate>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td id="Td19"  colspan="2" class="top" runat="server">
                                                <asp:Label ID="T_header_month10" runat="server" Text="month10"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="left" style="width: 50%; border-right: solid 1px #ccc;">
                                                TRx
                                            </td>
                                            <td class="right" style="width: 50%">
                                                MS
                                            </td>
                                        </tr>
                                    </table>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td class="alignRight" style="width: 50%; border-right: solid 1px #ccc;">
                                                <telerik:RadTextBox Width="27px" ID="T_TRx10" runat="server" CssClass="T_cssTRx"
                                                    Text='<%# String.Format("{0:n0}", Eval("T_TRx10")) %>'>
                                                </telerik:RadTextBox>
                                            </td>
                                            <td class="alignRight" style="width: 50%">
                                                <telerik:RadTextBox Width="27px" ID="T_MST10" runat="server" CssClass="T_cssMST"
                                                    Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("T_MST10"), "{0:n2}")%>'>
                                                </telerik:RadTextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            
                              <%--T_month11--%>
                            <telerik:GridTemplateColumn UniqueName="T_month11" HeaderStyle-CssClass="rightScroll_T"
                                ItemStyle-CssClass="rightScroll">
                                <HeaderTemplate>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td id="Td20"  colspan="2" class="top" runat="server">
                                                <asp:Label ID="T_header_month11" runat="server" Text="month11"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="left" style="width: 50%; border-right: solid 1px #ccc;">
                                                TRx
                                            </td>
                                            <td class="right" style="width: 50%">
                                                MS
                                            </td>
                                        </tr>
                                    </table>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td class="alignRight" style="width: 50%; border-right: solid 1px #ccc;">
                                                <telerik:RadTextBox Width="27px" ID="T_TRx11" runat="server" CssClass="T_cssTRx"
                                                    Text='<%# String.Format("{0:n0}", Eval("T_TRx11")) %>'>
                                                </telerik:RadTextBox>
                                            </td>
                                            <td class="alignRight" style="width: 50%">
                                                <telerik:RadTextBox Width="27px" ID="T_MST11" runat="server" CssClass="T_cssMST"
                                                    Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("T_MST11"), "{0:n2}")%>'>
                                                </telerik:RadTextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                              <%--T_month12--%>
                            <telerik:GridTemplateColumn UniqueName="T_month12" HeaderStyle-CssClass="rightScroll_T"
                                ItemStyle-CssClass="rightScroll">
                                <HeaderTemplate>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td id="Td21"  colspan="2" class="top" runat="server">
                                                <asp:Label ID="T_header_month12" runat="server" Text="month12"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="left" style="width: 50%; border-right: solid 1px #ccc;">
                                                TRx
                                            </td>
                                            <td class="right" style="width: 50%">
                                                MS
                                            </td>
                                        </tr>
                                    </table>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td class="alignRight" style="width: 50%; border-right: solid 1px #ccc;">
                                                <telerik:RadTextBox Width="27px" ID="T_TRx12" runat="server" CssClass="T_cssTRx"
                                                    Text='<%# String.Format("{0:n0}", Eval("T_TRx12")) %>'>
                                                </telerik:RadTextBox>
                                            </td>
                                            <td class="alignRight" style="width: 50%">
                                                <telerik:RadTextBox Width="27px" ID="T_MST12" runat="server" CssClass="T_cssMST"
                                                    Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("T_MST12"), "{0:n2}")%>'>
                                                </telerik:RadTextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            
                            
                        </Columns>
                    </telerik:GridTableView>
                </DetailTables>
                <%--  Territory: End --%>
                
                <%-- District: Begin --%>
                <Columns>
                    <telerik:GridTemplateColumn HeaderStyle-CssClass="headerTh_imageLink" ItemStyle-CssClass="merged">
                        <ItemTemplate>
                            <asp:HyperLink runat="server" ID="physLink" Visible="true" NavigateUrl='<%# string.Format("javascript:showDistrictPhysicians({0}, \"{1}\", {2}, {3})", Eval("Campaign_ID"), Eval("District_ID"), Eval("Start_Data_Key"), Eval("End_Data_Key")) %>'
                                ToolTip="Top Physicians"><img src="../content/images/list.png" alt="Top Physicians" /></asp:HyperLink>
                            <asp:HyperLink runat="server" ID="distLink" Visible="true" NavigateUrl='<%# string.Format("javascript:showDistrictChart({0}, \"{1}\", {2}, {3},{4})", Eval("Campaign_ID"), Eval("District_ID"), Eval("Start_Data_Key"), Eval("End_Data_Key"), Eval("Brand_ID")) %>'
                                ToolTip="District Profile Brand Trx"><img src="../content/images/chart.gif" alt="District Profile Brand Trx" /></asp:HyperLink>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn HeaderText="District" DataField="District_ID" HeaderStyle-CssClass="headerTh_D"
                        ItemStyle-CssClass="merged_D" UniqueName="District_ID" />
                        
                        <%-- 1/15/2013 added 'Brand_Name' --%>
                    <telerik:GridTemplateColumn UniqueName="BaseCol_D_Comp" HeaderStyle-CssClass="headerTh_D_Brand"
                       ItemStyle-CssClass="merged_D">
                        <HeaderTemplate>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="top">
                                        Brand
                                    </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="alignLeft" >
                                        <asp:Label ID="D_Brand_Name" runat="server" CssClass="cssTRx_D" Text='<%# Eval("Brand_Name") %>'>
                                        </asp:Label>
                                    </td>
                                </tr>
                                <%-- testing: Competitor --%>
                                <tr>
                                    <td class="alignLeft" style="border-top: solid 1px #ccc;">
                                        Competitors
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>    
                        
                        
                    <telerik:GridTemplateColumn UniqueName="BaseCol_D" HeaderStyle-CssClass="baseline_header_D"
                        ItemStyle-CssClass="baseline_item_D">
                        <HeaderTemplate>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td colspan="2" class="top" >
                                        Baseline
                                    </td>
                                </tr>
                                <tr>
                                    <td class="left" style="width: 50%; border-right: solid 1px #ccc;">
                                        TRx
                                    </td>
                                    <td class="right" style="width: 50%">
                                        MS
                                    </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="alignRight" style="width: 50%; border-right: solid 1px #ccc; ">
                                        <asp:Label ID="D_TRx" runat="server" CssClass="cssTRx_D" Text='<%# String.Format("{0:n0}", Eval("D_TRx")) %>'>
                                        </asp:Label>
                                    </td>
                                    <td class="alignRight" style="width: 50%;" >
                                        <asp:Label ID="D_MST" runat="server" CssClass="cssMST_D" Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("D_MST"), "{0:n2}")%>'>
                                        </asp:Label>
                                    </td>
                                </tr>
                                
                                 <%-- testing: Competitor --%>
                        <tr>
                            <td  class="alignRight"   style="width: 50%; border-right: solid 1px #ccc; border-top: solid 1px #ccc;">
                                <asp:Label ID="Comp_D_TRx" runat="server" CssClass="cssTRx_D" Text='<%# String.Format("{0:n0}", Eval("Comp_D_TRx")) %>'></asp:Label>
                            </td>
                            <td class="alignRight"  style="width: 50; border-top: solid 1px #ccc;" >
                                <asp:Label ID="Comp_D_MST" runat="server" CssClass="cssMST_D" Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("Comp_D_MST"), "{0:n2}")%>'></asp:Label>
                            </td>
                        </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                
                    
                    <%-- D_: 12 month GridTemplateColumn --%>
                    <%--D_month1--%>
                    <telerik:GridTemplateColumn UniqueName="D_month1" HeaderStyle-CssClass="rightScroll"
                        ItemStyle-CssClass="rightScroll">
                        <HeaderTemplate>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td id="Td22"  colspan="2" class="top" runat="server">
                                        <asp:Label ID="D_header_month1" runat="server" Text="month1"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="left" style="width: 50%; border-right: solid 1px #ccc;">
                                        TRx
                                    </td>
                                    <td class="right" style="width: 50%">
                                        MS
                                    </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="alignRight" style="width: 50%; border-right: solid 1px #ccc;">
                                       
                                        <telerik:RadTextBox Width="27px" ID="D_TRx1" runat="server" CssClass="UserInputBox"    
                                            Text='<%# String.Format("{0:n0}", Eval("D_TRx1")) %>'>
                                         <ClientEvents OnValueChanging="donotChange" />  
                                        </telerik:RadTextBox>
                                    </td>
                                    <td class="alignRight" style="width: 50%">
                                        <telerik:RadTextBox Width="27px" ID="D_MST1" runat="server" CssClass="D_cssMST"
                                            Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("D_MST1"), "{0:n2}")%>'>
                                        </telerik:RadTextBox>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <%--D_month2--%>
                    <telerik:GridTemplateColumn UniqueName="D_month2" HeaderStyle-CssClass="rightScroll"
                        ItemStyle-CssClass="rightScroll">
                        <HeaderTemplate>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td id="Td23"  colspan="2" class="top" runat="server">
                                        <asp:Label ID="D_header_month2" runat="server" Text="month2"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="left" style="width: 50%; border-right: solid 1px #ccc;">
                                        TRx
                                    </td>
                                    <td class="right" style="width: 50%">
                                        MS
                                    </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="alignRight" style="width: 50%; border-right: solid 1px #ccc;">
                                        <telerik:RadTextBox Width="27px" ID="D_TRx2" runat="server"  CssClass="UserInputBox"
                                            Text='<%# String.Format("{0:n0}", Eval("D_TRx2")) %>'>
                                        <ClientEvents OnValueChanging="donotChange" />  
                                        </telerik:RadTextBox>
                                    </td>
                                    <td class="alignRight" style="width: 50%">
                                        <telerik:RadTextBox Width="27px" ID="D_MST2" runat="server" CssClass="D_cssMST"
                                            Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("D_MST2"), "{0:n2}")%>'>
                                        </telerik:RadTextBox>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <%--D_month3--%>
                    <telerik:GridTemplateColumn UniqueName="D_month3" HeaderStyle-CssClass="rightScroll"
                        ItemStyle-CssClass="rightScroll">
                        <HeaderTemplate>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td id="Td24" colspan="2" class="top" runat="server">
                                        <asp:Label ID="D_header_month3" runat="server" Text="month3"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="left" style="width: 50%; border-right: solid 1px #ccc;">
                                        TRx
                                    </td>
                                    <td class="right" style="width: 50%">
                                        MS
                                    </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="alignRight" style="width: 50%; border-right: solid 1px #ccc;">
                                        <telerik:RadTextBox Width="27px" ID="D_TRx3" runat="server" CssClass="UserInputBox" 
                                            Text='<%# String.Format("{0:n0}", Eval("D_TRx3")) %>'>
                                        <ClientEvents OnValueChanging="donotChange" />  

                                          </telerik:RadTextBox>
                                    </td>
                                    <td class="alignRight" style="width: 50%">
                                        <telerik:RadTextBox Width="27px" ID="D_MST3" runat="server" CssClass="D_cssMST"
                                            Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("D_MST3"), "{0:n2}")%>'>
                                        </telerik:RadTextBox>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <%--D_month4--%>
                    <telerik:GridTemplateColumn UniqueName="D_month4" HeaderStyle-CssClass="rightScroll"
                        ItemStyle-CssClass="rightScroll">
                        <HeaderTemplate>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td id="Td25" colspan="2" class="top" runat="server">
                                        <asp:Label ID="D_header_month4" runat="server" Text="month4"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="left" style="width: 50%; border-right: solid 1px #ccc;">
                                        TRx
                                    </td>
                                    <td class="right" style="width: 50%">
                                        MS
                                    </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <tr>
                                
                                    <td class="alignRight" style="width: 50%; border-right: solid 1px #ccc;">
                                       <telerik:RadTextBox Width="27px" ID="D_TRx4" runat="server" CssClass="UserInputBox"
                                            Text='<%# String.Format("{0:n0}", Eval("D_TRx4")) %>'>
                                        <ClientEvents OnValueChanging="donotChange" />  
                                        </telerik:RadTextBox>
                                    </td>
                                    <td class="alignRight" style="width: 50%">
                                        <telerik:RadTextBox Width="27px" ID="D_MST4" runat="server" CssClass="D_cssMST"
                                            Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("D_MST4"), "{0:n2}")%>'>
                                        </telerik:RadTextBox>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                      <%--D_month5--%>
                    <telerik:GridTemplateColumn UniqueName="D_month5" HeaderStyle-CssClass="rightScroll"
                        ItemStyle-CssClass="rightScroll">
                        <HeaderTemplate>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td id="Td26" colspan="2" class="top" runat="server">
                                        <asp:Label ID="D_header_month5" runat="server" Text="month5"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="left" style="width: 50%; border-right: solid 1px #ccc;">
                                        TRx
                                    </td>
                                    <td class="right" style="width: 50%">
                                        MS
                                    </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="alignRight" style="width: 50%; border-right: solid 1px #ccc;">
                                        <telerik:RadTextBox Width="27px" ID="D_TRx5" runat="server" CssClass="UserInputBox"
                                            Text='<%# String.Format("{0:n0}", Eval("D_TRx5")) %>'>
                                        <ClientEvents OnValueChanging="donotChange" />  
                                        </telerik:RadTextBox>
                                    </td>
                                    <td class="alignRight" style="width: 50%">
                                        <telerik:RadTextBox Width="27px" ID="D_MST5" runat="server" CssClass="D_cssMST"
                                            Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("D_MST5"), "{0:n2}")%>'>
                                        </telerik:RadTextBox>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    
                      <%--D_month6--%>
                    <telerik:GridTemplateColumn UniqueName="D_month6" HeaderStyle-CssClass="rightScroll"
                        ItemStyle-CssClass="rightScroll">
                        <HeaderTemplate>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td id="Td27" colspan="2" class="top" runat="server">
                                        <asp:Label ID="D_header_month6" runat="server" Text="month6"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="left" style="width: 50%; border-right: solid 1px #ccc;">
                                        TRx
                                    </td>
                                    <td class="right" style="width: 50%">
                                        MS
                                    </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="alignRight" style="width: 50%; border-right: solid 1px #ccc;">
                                        <telerik:RadTextBox Width="27px" ID="D_TRx6" runat="server" CssClass="UserInputBox"
                                            Text='<%# String.Format("{0:n0}", Eval("D_TRx6")) %>'>
                                        <ClientEvents OnValueChanging="donotChange" />  
                                        </telerik:RadTextBox>
                                    </td>
                                    <td class="alignRight" style="width: 50%">
                                        <telerik:RadTextBox Width="27px" ID="D_MST6" runat="server" CssClass="D_cssMST"
                                            Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("D_MST6"), "{0:n2}")%>'>
                                        </telerik:RadTextBox>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    
                      <%--D_month7--%>
                    <telerik:GridTemplateColumn UniqueName="D_month7" HeaderStyle-CssClass="rightScroll"
                        ItemStyle-CssClass="rightScroll">
                        <HeaderTemplate>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td id="Td28"  colspan="2" class="top" runat="server">
                                        <asp:Label ID="D_header_month7" runat="server" Text="month7"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="left" style="width: 50%; border-right: solid 1px #ccc;">
                                        TRx
                                    </td>
                                    <td class="right" style="width: 50%">
                                        MS
                                    </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="alignRight" style="width: 50%; border-right: solid 1px #ccc;">
                                        <telerik:RadTextBox Width="27px" ID="D_TRx7" runat="server" CssClass="UserInputBox"
                                            Text='<%# String.Format("{0:n0}", Eval("D_TRx7")) %>'>
                                        <ClientEvents OnValueChanging="donotChange" />  
                                        </telerik:RadTextBox>
                                    </td>
                                    <td class="alignRight" style="width: 50%">
                                        <telerik:RadTextBox Width="27px" ID="D_MST7" runat="server" CssClass="D_cssMST"
                                            Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("D_MST7"), "{0:n2}")%>'>
                                        </telerik:RadTextBox>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    
                      <%--D_month8--%>
                    <telerik:GridTemplateColumn UniqueName="D_month8" HeaderStyle-CssClass="rightScroll"
                        ItemStyle-CssClass="rightScroll">
                        <HeaderTemplate>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td id="Td29" colspan="2" class="top" runat="server">
                                        <asp:Label ID="D_header_month8" runat="server" Text="month8"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="left" style="width: 50%; border-right: solid 1px #ccc;">
                                        TRx
                                    </td>
                                    <td class="right" style="width: 50%">
                                        MS
                                    </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="alignRight" style="width: 50%; border-right: solid 1px #ccc;">
                                        <telerik:RadTextBox Width="27px" ID="D_TRx8" runat="server" CssClass="UserInputBox"
                                            Text='<%# String.Format("{0:n0}", Eval("D_TRx8")) %>'>
                                        <ClientEvents OnValueChanging="donotChange" />  
                                        </telerik:RadTextBox>
                                    </td>
                                    <td class="alignRight" style="width: 50%">
                                        <telerik:RadTextBox Width="27px" ID="D_MST8" runat="server" CssClass="D_cssMST"
                                            Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("D_MST8"), "{0:n2}")%>'>
                                        </telerik:RadTextBox>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    
                      <%--D_month9--%>
                    <telerik:GridTemplateColumn UniqueName="D_month9" HeaderStyle-CssClass="rightScroll"
                        ItemStyle-CssClass="rightScroll">
                        <HeaderTemplate>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td id="Td30"  colspan="2" class="top" runat="server">
                                        <asp:Label ID="D_header_month9" runat="server" Text="month9"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="left" style="width: 50%; border-right: solid 1px #ccc;">
                                        TRx
                                    </td>
                                    <td class="right" style="width: 50%">
                                        MS
                                    </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="alignRight" style="width: 50%; border-right: solid 1px #ccc;">
                                        <telerik:RadTextBox Width="27px" ID="D_TRx9" runat="server" CssClass="UserInputBox"
                                            Text='<%# String.Format("{0:n0}", Eval("D_TRx9")) %>'>
                                        <ClientEvents OnValueChanging="donotChange" />  
                                        </telerik:RadTextBox>
                                    </td>
                                    <td class="alignRight" style="width: 50%">
                                        <telerik:RadTextBox Width="27px" ID="D_MST9" runat="server" CssClass="D_cssMST"
                                            Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("D_MST9"), "{0:n2}")%>'>
                                        </telerik:RadTextBox>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    
                      <%--D_month10--%>
                    <telerik:GridTemplateColumn UniqueName="D_month10" HeaderStyle-CssClass="rightScroll"
                        ItemStyle-CssClass="rightScroll">
                        <HeaderTemplate>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td id="Td31"  colspan="2" class="top" runat="server">
                                        <asp:Label ID="D_header_month10" runat="server" Text="month10"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="left" style="width: 50%; border-right: solid 1px #ccc;">
                                        TRx
                                    </td>
                                    <td class="right" style="width: 50%">
                                        MS
                                    </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="alignRight" style="width: 50%; border-right: solid 1px #ccc;">
                                        <telerik:RadTextBox Width="27px" ID="D_TRx10" runat="server" CssClass="UserInputBox"
                                            Text='<%# String.Format("{0:n0}", Eval("D_TRx10")) %>'>
                                        <ClientEvents OnValueChanging="donotChange" />  
                                        </telerik:RadTextBox>
                                    </td>
                                    <td class="alignRight" style="width: 50%">
                                        <telerik:RadTextBox Width="27px" ID="D_MST10" runat="server" CssClass="D_cssMST"
                                            Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("D_MST10"), "{0:n2}")%>'>
                                        </telerik:RadTextBox>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    
                      <%--D_month11--%>
                    <telerik:GridTemplateColumn UniqueName="D_month11" HeaderStyle-CssClass="rightScroll"
                        ItemStyle-CssClass="rightScroll">
                        <HeaderTemplate>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td id="Td32"  colspan="2" class="top" runat="server">
                                        <asp:Label ID="D_header_month11" runat="server" Text="month11"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="left" style="width: 50%; border-right: solid 1px #ccc;">
                                        TRx
                                    </td>
                                    <td class="right" style="width: 50%">
                                        MS
                                    </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="alignRight" style="width: 50%; border-right: solid 1px #ccc;">
                                        <telerik:RadTextBox Width="27px" ID="D_TRx11" runat="server" CssClass="UserInputBox"
                                            Text='<%# String.Format("{0:n0}", Eval("D_TRx11")) %>'>
                                        <ClientEvents OnValueChanging="donotChange" />  
                                        </telerik:RadTextBox>
                                    </td>
                                    <td class="alignRight" style="width: 50%">
                                        <telerik:RadTextBox Width="27px" ID="D_MST11" runat="server" CssClass="D_cssMST"
                                            Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("D_MST11"), "{0:n2}")%>'>
                                        </telerik:RadTextBox>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    
                      <%--D_month12--%>
                    <telerik:GridTemplateColumn UniqueName="D_month12" HeaderStyle-CssClass="rightScroll"
                        ItemStyle-CssClass="rightScroll">
                        <HeaderTemplate>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td id="Td33" colspan="2" class="top" runat="server">
                                        <asp:Label ID="D_header_month12" runat="server" Text="month12"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="left" style="width: 50%; border-right: solid 1px #ccc;">
                                        TRx
                                    </td>
                                    <td class="right" style="width: 50%">
                                        MS
                                    </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="alignRight" style="width: 50%; border-right: solid 1px #ccc;">
                                        <telerik:RadTextBox Width="27px" ID="D_TRx12" runat="server" CssClass="UserInputBox"
                                            Text='<%# String.Format("{0:n0}", Eval("D_TRx12")) %>'>
                                        <ClientEvents OnValueChanging="donotChange" />  
                                        </telerik:RadTextBox>
                                    </td>
                                    <td class="alignRight" style="width: 50%">
                                        <telerik:RadTextBox Width="27px" ID="D_MST12" runat="server" CssClass="D_cssMST"
                                            Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("D_MST12"), "{0:n2}")%>'>
                                        </telerik:RadTextBox>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    
                    
                    
                    
                </Columns>
            </telerik:GridTableView>
        </DetailTables>
        <%-- District: End  --%>
        
        <%-- Region: Begin --%>
        <Columns>
            <telerik:GridBoundColumn HeaderText="Region" DataField="Region_ID" HeaderStyle-CssClass="headerTh_R"
             ItemStyle-CssClass="headerTh_item_R" UniqueName="Region_ID" />
             
                <%-- 1/15/2013 added 'Brand_Name' --%>
                    <telerik:GridTemplateColumn UniqueName="BaseCol_R_Comp" HeaderStyle-CssClass="headerTh_R_Brand"
                       ItemStyle-CssClass="merged_R">
                        <HeaderTemplate>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="top">
                                        Brand
                                    </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="alignLeft" >
                                        <asp:Label ID="R_Brand_Name" runat="server" CssClass="cssTRx_R" Text='<%# Eval("Brand_Name") %>'>
                                        </asp:Label>
                                    </td>
                                </tr>
                                <%-- testing: Competitor --%>
                                <tr>
                                    <td class="alignLeft" style="border-top: solid 1px #ccc;">
                                        Competitors
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>  
             
             
             
             
            <telerik:GridTemplateColumn UniqueName="BaseCol_R" HeaderStyle-CssClass="baseline_header_R"
                        ItemStyle-CssClass="baseline_item_R">  
               
                <HeaderTemplate>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td colspan="2" class="top">
                                Baseline
                            </td>
                        </tr>
                        <tr>
                            <td class="left" style="width: 50%; border-right: solid 1px #ccc;">
                                TRx
                            </td>
                            <td class="right" style="width: 50%">
                                MS
                            </td>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td  class="alignRight"   style="width: 50%; border-right: solid 1px #ccc;">
                                <asp:Label ID="R_TRx" runat="server" CssClass="cssTRx_R" Text='<%# String.Format("{0:n0}",Eval("R_TRx")) %>'></asp:Label>
                            </td>
                            <td class="alignRight"  style="width: 50%">
                                <asp:Label ID="R_MST" runat="server" CssClass="cssMST_R" Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("R_MST"), "{0:n2}")%>'></asp:Label>
                            </td>
                        </tr>
                        
                        <%-- testing: Competitor --%>
                        <tr>
                            <td  class="alignRight"   style="width: 50%; border-right: solid 1px #ccc;border-top: solid 1px #ccc;">
                                <asp:Label ID="Comp_R_TRx" runat="server" CssClass="cssTRx_R" Text='<%# String.Format("{0:n0}",Eval("Comp_R_TRx")) %>'></asp:Label>
                            </td>
                            <td class="alignRight"  style="width: 50%; border-top: solid 1px #ccc;">
                                <asp:Label ID="Comp_R_MST" runat="server" CssClass="cssMST_R" Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("Comp_R_MST"), "{0:n2}")%>'></asp:Label>
                            </td>
                        </tr>
                        
                        
                    </table>
                </ItemTemplate>

            </telerik:GridTemplateColumn>
            
            
            <%-- R_: 12 month GridTemplateColumn --%>
             <%--R_month1--%>
            <telerik:GridTemplateColumn UniqueName="R_month1" HeaderStyle-CssClass="rightScroll"
                ItemStyle-CssClass="rightScroll">
                <HeaderTemplate>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td id="Td20" colspan="2" class="top" runat="server">
                                <asp:Label ID="R_header_month1" runat="server" Text="month1"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="left" style="width: 50%; border-right: solid 1px #ccc;">
                                TRx
                            </td>
                            <td class="right" style="width: 50%">
                                MS
                            </td>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td  class="alignRight"   style="width: 50%; border-right: solid 1px #ccc;">
                                <telerik:RadTextBox Width="27px" ID="R_TRx1"  runat="server" CssClass="R_cssTRx" Text='<%# String.Format("{0:n0}",Eval("R_TRx1")) %>'></telerik:RadTextBox>
                            </td>
                            <td class="alignRight"  style="width: 50%">
                                <telerik:RadTextBox Width="27px" ID="R_MST1"  runat="server" CssClass="R_cssMST" Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("R_MST1"), "{0:n2}")%>'></telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            <%--R_month2--%>
            <telerik:GridTemplateColumn UniqueName="R_month2" HeaderStyle-CssClass="rightScroll"
                ItemStyle-CssClass="rightScroll">
                <HeaderTemplate>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td id="Td21"  colspan="2" class="top" runat="server">
                                <asp:Label ID="R_header_month2" runat="server" Text="month2"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="left" style="width: 50%; border-right: solid 1px #ccc;">
                                TRx
                            </td>
                            <td class="right" style="width: 50%">
                                MS
                            </td>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td  class="alignRight"   style="width: 50%; border-right: solid 1px #ccc;">
                                <telerik:RadTextBox Width="27px" ID="R_TRx2"   runat="server" CssClass="R_cssTRx" Text='<%# String.Format("{0:n0}",Eval("R_TRx2")) %>'></telerik:RadTextBox>
                            </td>
                            <td class="alignRight"  style="width: 50%">
                                <telerik:RadTextBox Width="27px" ID="R_MST2"   runat="server" CssClass="R_cssMST" Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("R_MST2"), "{0:n2}")%>'></telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            <%--R_month3--%>
            <telerik:GridTemplateColumn UniqueName="R_month3" HeaderStyle-CssClass="rightScroll"
                ItemStyle-CssClass="rightScroll">
                <HeaderTemplate>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td id="Td22"  colspan="2" class="top" runat="server">
                                <asp:Label ID="R_header_month3" runat="server" Text="month3"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="left" style="width: 50%; border-right: solid 1px #ccc;">
                                TRx
                            </td>
                            <td class="right" style="width: 50%">
                                MS
                            </td>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td  class="alignRight"   style="width: 50%; border-right: solid 1px #ccc;">
                                <telerik:RadTextBox Width="27px" ID="R_TRx3"   runat="server" CssClass="R_cssTRx" Text='<%# String.Format("{0:n0}",Eval("R_TRx3")) %>'></telerik:RadTextBox>
                            </td>
                            <td class="alignRight"  style="width: 50%">
                                <telerik:RadTextBox Width="27px" ID="R_MST3"   runat="server" CssClass="R_cssMST" Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("R_MST3"), "{0:n2}")%>'></telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            <%--R_month4--%>
            <telerik:GridTemplateColumn UniqueName="R_month4" HeaderStyle-CssClass="rightScroll"
                ItemStyle-CssClass="rightScroll">
                <HeaderTemplate>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td id="Td23"   colspan="2" class="top" runat="server">
                                <asp:Label ID="R_header_month4" runat="server" Text="month4"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="left" style="width: 50%; border-right: solid 1px #ccc;">
                                TRx
                            </td>
                            <td class="right" style="width: 50%">
                                MS
                            </td>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td  class="alignRight"   style="width: 50%; border-right: solid 1px #ccc;">
                                <telerik:RadTextBox Width="27px" ID="R_TRx4"   runat="server" CssClass="R_cssTRx" Text='<%# String.Format("{0:n0}",Eval("R_TRx4")) %>'></telerik:RadTextBox>
                            </td>
                            <td class="alignRight"  style="width: 50%">
                                <telerik:RadTextBox Width="27px" ID="R_MST4"   runat="server" CssClass="R_cssMST" Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("R_MST4"), "{0:n2}")%>'></telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            
             <%--R_month5--%>
            <telerik:GridTemplateColumn UniqueName="R_month5" HeaderStyle-CssClass="rightScroll"
                ItemStyle-CssClass="rightScroll">
                <HeaderTemplate>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td id="Td24"   colspan="2" class="top" runat="server">
                                <asp:Label ID="R_header_month5" runat="server" Text="month5"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="left" style="width: 50%; border-right: solid 1px #ccc;">
                                TRx
                            </td>
                            <td class="right" style="width: 50%">
                                MS
                            </td>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td  class="alignRight"   style="width: 50%; border-right: solid 1px #ccc;">
                                <telerik:RadTextBox Width="27px" ID="R_TRx5"   runat="server" CssClass="R_cssTRx" Text='<%# String.Format("{0:n0}",Eval("R_TRx5")) %>'></telerik:RadTextBox>
                            </td>
                            <td class="alignRight"  style="width: 50%">
                                <telerik:RadTextBox Width="27px" ID="R_MST5"   runat="server" CssClass="R_cssMST" Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("R_MST5"), "{0:n2}")%>'></telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            
             <%--R_month6--%>
            <telerik:GridTemplateColumn UniqueName="R_month6" HeaderStyle-CssClass="rightScroll"
                ItemStyle-CssClass="rightScroll">
                <HeaderTemplate>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td id="Td25"   colspan="2" class="top" runat="server">
                                <asp:Label ID="R_header_month6" runat="server" Text="month6"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="left" style="width: 50%; border-right: solid 1px #ccc;">
                                TRx
                            </td>
                            <td class="right" style="width: 50%">
                                MS
                            </td>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td  class="alignRight"   style="width: 50%; border-right: solid 1px #ccc;">
                                <telerik:RadTextBox Width="27px" ID="R_TRx6"   runat="server" CssClass="R_cssTRx" Text='<%# String.Format("{0:n0}",Eval("R_TRx6")) %>'></telerik:RadTextBox>
                            </td>
                            <td class="alignRight"  style="width: 50%">
                                <telerik:RadTextBox Width="27px" ID="R_MST6"   runat="server" CssClass="R_cssMST" Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("R_MST6"), "{0:n2}")%>'></telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </telerik:GridTemplateColumn>
             <%--R_month7--%>
            <telerik:GridTemplateColumn UniqueName="R_month7" HeaderStyle-CssClass="rightScroll"
                ItemStyle-CssClass="rightScroll">
                <HeaderTemplate>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td id="Td26"   colspan="2" class="top" runat="server">
                                <asp:Label ID="R_header_month7" runat="server" Text="month7"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="left" style="width: 50%; border-right: solid 1px #ccc;">
                                TRx
                            </td>
                            <td class="right" style="width: 50%">
                                MS
                            </td>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td  class="alignRight"   style="width: 50%; border-right: solid 1px #ccc;">
                                <telerik:RadTextBox Width="27px" ID="R_TRx7"   runat="server" CssClass="R_cssTRx" Text='<%# String.Format("{0:n0}",Eval("R_TRx7")) %>'></telerik:RadTextBox>
                            </td>
                            <td class="alignRight"  style="width: 50%">
                                <telerik:RadTextBox Width="27px" ID="R_MST7"   runat="server" CssClass="R_cssMST" Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("R_MST7"), "{0:n2}")%>'></telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            
             <%--R_month8--%>
            <telerik:GridTemplateColumn UniqueName="R_month8" HeaderStyle-CssClass="rightScroll"
                ItemStyle-CssClass="rightScroll">
                <HeaderTemplate>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td id="Td27"   colspan="2" class="top" runat="server">
                                <asp:Label ID="R_header_month8" runat="server" Text="month8"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="left" style="width: 50%; border-right: solid 1px #ccc;">
                                TRx
                            </td>
                            <td class="right" style="width: 50%">
                                MS
                            </td>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td  class="alignRight"   style="width: 50%; border-right: solid 1px #ccc;">
                                <telerik:RadTextBox Width="27px" ID="R_TRx8"   runat="server" CssClass="R_cssTRx" Text='<%# String.Format("{0:n0}",Eval("R_TRx8")) %>'></telerik:RadTextBox>
                            </td>
                            <td class="alignRight"  style="width: 50%">
                                <telerik:RadTextBox Width="27px" ID="R_MST8"   runat="server" CssClass="R_cssMST" Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("R_MST8"), "{0:n2}")%>'></telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            
             <%--R_month9--%>
            <telerik:GridTemplateColumn UniqueName="R_month9" HeaderStyle-CssClass="rightScroll"
                ItemStyle-CssClass="rightScroll">
                <HeaderTemplate>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td id="Td28"   colspan="2" class="top" runat="server">
                                <asp:Label ID="R_header_month9" runat="server" Text="month9"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="left" style="width: 50%; border-right: solid 1px #ccc;">
                                TRx
                            </td>
                            <td class="right" style="width: 50%">
                                MS
                            </td>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td  class="alignRight"   style="width: 50%; border-right: solid 1px #ccc;">
                                <telerik:RadTextBox Width="27px" ID="R_TRx9"   runat="server" CssClass="R_cssTRx" Text='<%# String.Format("{0:n0}",Eval("R_TRx9")) %>'></telerik:RadTextBox>
                            </td>
                            <td class="alignRight"  style="width: 50%">
                                <telerik:RadTextBox Width="27px" ID="R_MST9"   runat="server" CssClass="R_cssMST" Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("R_MST9"), "{0:n2}")%>'></telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            
             <%--R_month10--%>
            <telerik:GridTemplateColumn UniqueName="R_month10" HeaderStyle-CssClass="rightScroll"
                ItemStyle-CssClass="rightScroll">
                <HeaderTemplate>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td id="Td29"   colspan="2" class="top" runat="server">
                                <asp:Label ID="R_header_month10" runat="server" Text="month10"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="left" style="width: 50%; border-right: solid 1px #ccc;">
                                TRx
                            </td>
                            <td class="right" style="width: 50%">
                                MS
                            </td>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td  class="alignRight"   style="width: 50%; border-right: solid 1px #ccc;">
                                <telerik:RadTextBox Width="27px" ID="R_TRx10"   runat="server" CssClass="R_cssTRx" Text='<%# String.Format("{0:n0}",Eval("R_TRx10")) %>'></telerik:RadTextBox>
                            </td>
                            <td class="alignRight"  style="width: 50%">
                                <telerik:RadTextBox Width="27px" ID="R_MST10"   runat="server" CssClass="R_cssMST" Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("R_MST10"), "{0:n2}")%>'></telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            
             <%--R_month11--%>
            <telerik:GridTemplateColumn UniqueName="R_month11" HeaderStyle-CssClass="rightScroll"
                ItemStyle-CssClass="rightScroll">
                <HeaderTemplate>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td id="Td30"   colspan="2" class="top" runat="server">
                                <asp:Label ID="R_header_month11" runat="server" Text="month11"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="left" style="width: 50%; border-right: solid 1px #ccc;">
                                TRx
                            </td>
                            <td class="right" style="width: 50%">
                                MS
                            </td>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td  class="alignRight"   style="width: 50%; border-right: solid 1px #ccc;">
                                <telerik:RadTextBox Width="27px" ID="R_TRx11"   runat="server" CssClass="R_cssTRx" Text='<%# String.Format("{0:n0}",Eval("R_TRx11")) %>'></telerik:RadTextBox>
                            </td>
                            <td class="alignRight"  style="width: 50%">
                                <telerik:RadTextBox Width="27px" ID="R_MST11"   runat="server" CssClass="R_cssMST" Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("R_MST11"), "{0:n2}")%>'></telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            
             <%--R_month12--%>
            <telerik:GridTemplateColumn UniqueName="R_month12" HeaderStyle-CssClass="rightScroll"
                ItemStyle-CssClass="rightScroll">
                <HeaderTemplate>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td id="Td31"   colspan="2" class="top" runat="server">
                                <asp:Label ID="R_header_month12" runat="server" Text="month12"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="left" style="width: 50%; border-right: solid 1px #ccc;">
                                TRx
                            </td>
                            <td class="right" style="width: 50%">
                                MS
                            </td>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td  class="alignRight"   style="width: 50%; border-right: solid 1px #ccc;">
                                <telerik:RadTextBox Width="27px" ID="R_TRx12"   runat="server" CssClass="R_cssTRx" Text='<%# String.Format("{0:n0}",Eval("R_TRx12")) %>'></telerik:RadTextBox>
                            </td>
                            <td class="alignRight"  style="width: 50%">
                                <telerik:RadTextBox Width="27px" ID="R_MST12"   runat="server" CssClass="R_cssMST" Text='<%# Pinsonault.Application.PowerPlanRx.Goals.GetFormattedGoalValue(Eval("R_MST12"), "{0:n2}")%>'></telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            
            
        
           <%-- Region: End --%>
            
        </Columns>
    </MasterTableView>
    <%-- FROZEN COLUMN count is set in code since it depends on columns that are sometimes hidden. --%>
    

    <ClientSettings AllowExpandCollapse="true" Scrolling-AllowScroll="true"
      Scrolling-FrozenColumnsCount="4"  >
      <%--Scrolling-UseStaticHeaders="true"--%>
      </ClientSettings>
         
</telerik:RadGrid>


<%--<asp:Label runat="server" CssClass="rowCountDebug" ID="rowCount" Visible="true" Text="Row Count:" />--%>
 <asp:SqlDataSource runat="server" ID="dsPlanGoals" ConnectionString='<%$ConnectionStrings:PathfinderClientDB_Format %>'> 
        <%-- UpdateCommand="usp_Campaign_UpdateGoals" UpdateCommandType="StoredProcedure" --%>
        
        <SelectParameters>
            <asp:QueryStringParameter Name="Campaign_ID" QueryStringField="id" DefaultValue="0" />
        </SelectParameters>
    </asp:SqlDataSource>
   
<asp:SqlDataSource runat="server" ID="dsDGoals" ConnectionString='<%$ConnectionStrings:PathfinderClientDB_Format %>'>
    <SelectParameters>
        <asp:QueryStringParameter Name="Campaign_ID" QueryStringField="id" DefaultValue="0" />
        <%--  <asp:Parameter Name="MBTRx" DefaultValue="0" />--%>
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource runat="server" ID="dsRGoals" ConnectionString='<%$ConnectionStrings:PathfinderClientDB_Format %>'>
    <SelectParameters>
        <asp:QueryStringParameter Name="Campaign_ID" QueryStringField="id" DefaultValue="0" />
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource runat="server" ID="dsTGoals" ConnectionString='<%$ConnectionStrings:PathfinderClientDB_Format %>'>
    <SelectParameters>
        <asp:QueryStringParameter Name="Campaign_ID" QueryStringField="id" DefaultValue="0" />
    </SelectParameters>
</asp:SqlDataSource>
<div id="dial" style="display: none;">
</div>
<div id="dialTargeting" style="display: none;">
</div>
