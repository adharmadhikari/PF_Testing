<%@ Page Title="" Language="C#" EnableViewState="true" MasterPageFile="~/custom/Alcon/sellsheets/Alcon_SellSheetStep.master"
  AutoEventWireup="true" CodeFile="classandtemplateselection.aspx.cs" Inherits="custom_alcon_sellsheets_classandtemplateselection" %>
<%@ MasterType VirtualPath="~/custom/Alcon/sellsheets/Alcon_SellSheetStep.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="scriptContainer" Runat="Server">
  
    <script type="text/javascript" >
        clientManager.add_pageLoaded(classTemplateSelection_pageLoaded);
        clientManager.add_pageUnloaded(classTemplateSelection_pageUnloaded);
        var ssTheraID = $("#<%= rdcmbTheraClass.ClientID %>");
        var ssDrugID = $("#<%= rdcmbDrugs.ClientID %>");
        var ssdrugText = $("#<%= txtDrugID.ClientID %>");
        var ssRequiredDrugSelected = $("#<%= txtRequiredDrugSelected.ClientID %>");
        var ssDruglistSelected = $("#<%= txtDrugList.ClientID %>");
        
        function classTemplateSelection_pageLoaded(sender, args) {
            //IE 6 Fix - fixed next button postion
            if (/MSIE (\d+\.\d+);/.test(navigator.userAgent))
                var ieversion = new Number(RegExp.$1);

            if (ieversion <= 6) {
                var height = getWorkspaceHeight();

                $("#divTile3").height(height - $("#divTile3Container .tileContainerHeader").height() + 5);
            }
            //END IE 6 Fix

            //Create Drug list
            $createCheckboxDropdown(drugCtrlID, "Drug_ID", null, { 'maxItems': 4, 'defaultText': '<%= Resources.Resource.Label_No_Selection %>', 'multiItemText': 'change selection' }, { 'error': classTemplateSelection_onDrugListError }, 'moduleOptionsContainer');

            //Update selected item in Drug list
            $updateCheckboxDropdownText(drugCtrlID, "Drug_ID");


            //Load page if editing
            var thera = $("#<%= txtTheraID.ClientID %>").val();
            var drugs = $("#<%= txtDrugList.ClientID %>").val();
            var drug_id = $get("Drug_ID").control;
            var thera_id = $find(theraCtrlID);
           


            if (thera.length > 0) {
                var itm = thera_id.findItemByValue(thera);
                itm.select();
            }
            if (drugs.length > 0) {
                var url = "custom/Alcon/sellsheetreporting/services/AlconService.svc" + "/SellSheetDrugListSet?$filter=Thera_ID eq " + thera + "&$orderby=Name";
                $.getJSON(url, null, function(result, status) {
                    
                    var d = result.d;
                    $loadPinsoListItems(drug_id, d, null, -1);
                    
                    selectDrugList(drugs, drug_id);
                    $updateCheckboxDropdownText(drugCtrlID, "Drug_ID");
                });
            }

            //Get the carousel list item index to load selected template
            var templateid = $("#<%= txtTemplateID.ClientID %>").val();
            //Initialize carousel
            $('#mycarousel').jcarousel(
            {
                scroll: 1,
                start: 1,
                initCallback: template_initCallback,
                buttonNextHTML: null,
                buttonPrevHTML: null
            });
            $('#mycarousel li').css({ display: "none" });
            $('#templateSidebar li').css({ display: "none" });

            if (thera.length > 0 && templateid.length > 0) {
                $('#mycarousel li').filter('[thera_id=' + thera + ']').removeAttr('style');
                $('#mycarousel li').filter('[rel=' + templateid + ']').find('img').removeAttr('style');
                $('#mycarousel li').filter('[rel=' + templateid + ']').find('img').addClass('selectedTemplate');

                $('#templateSidebar').removeAttr('style'); 
                $('#templateSidebar li').filter('[thera_id=' + thera + ']').removeAttr('style');
                $('#templateSidebar li').filter('[rel=' + templateid + ']').find('img').removeAttr('style');
                $('#templateSidebar li').filter('[rel=' + templateid + ']').find('img').addClass('selectedTemplate');
            }

            //Bind Carousel Sidebar click event
            $('#mycarousel li').bind('click', function() {
                //Find the same list item in the sidebar by the rel attribute and obtain the index
                var items = $('#templateSidebar li');
                var selectedTemplate = $('#templateSidebar li').filter('[rel=' + this.getAttribute('rel') + ']');
                var index = items.index(selectedTemplate);

                //Scroll to the item based on the index
                sender.get_ApplicationManager().get_carousel().scroll(index);

                //Add the selectedTemplate CSS class to the selected template
                $('#templateSidebar li img').removeClass('selectedTemplate');
                $('#templateSidebar li').filter('[rel=' + this.getAttribute('rel') + ']').find('img').addClass('selectedTemplate');
                return false;
            });
            
        }  
        function template_initCallback(carousel) {
            //This finds the template selector in the sidebar and syncs the selector with Step 1
           
            $('#templateSidebar li').bind('click', function()
            {
                //Find the same list item in Step 1 by the rel attribute and obtain the index
                var items = $('#mycarousel li');
                var selectedTemplate = $('#mycarousel li').filter('[rel=' + this.getAttribute('rel') + ']');
                var index = items.index(selectedTemplate);
                
                //Scroll to the item based on the index
                carousel.scroll(index);
                
                //Add the selectedTemplate CSS class to the selected template
                $('#mycarousel li img').removeClass('selectedTemplate');
                //$('#mycarousel li').filter('[rel=' + this.getAttribute('rel') + ']').find('img').removeAttr("style");
                //Must call reflect before 'selectedTemplate' class is applied
                //$("#mycarousel li").find('img').reflect(50);
                $('#mycarousel li').filter('[rel=' + this.getAttribute('rel') + ']').find('img').removeAttr('style');
                $('#mycarousel li').filter('[rel=' + this.getAttribute('rel') + ']').find('img').addClass('selectedTemplate');                
                return false;
            });
        };
        function classTemplateSelection_pageUnloaded(sender, args) 
        {
        
            sender.remove_pageLoaded(classTemplateSelection_pageLoaded);
            sender.remove_pageUnloaded(classTemplateSelection_pageUnloaded);
        }

        function classTemplateSelection_onDrugListError(sender, args) 
        {
            $alert("Maximum 4 drugs should be selected.", "Warning");
        }

        // Function is called on Selected Index change event of Therapeutic Class dropdownlist.
        //This function gets the default drug that has to be selected when therapeutic class is changed.
        function UpdateDrugSelection(s, a)
        {
            var val = s.get_value();
            var template_id = $('#mycarousel li').filter('[thera_id =' + val + '][default=true]').attr('rel');
            if (typeof(template_id) != "undefined")
            {
                var url = "custom/Alcon/sellsheetreporting/services/AlconService.svc" + "/SellSheetDrugListSet?$filter=Thera_ID eq " + val + " and Selected eq 1 and Template_ID eq " + template_id;
                $.getJSON(url, null, function(result, status)
                {
                    var d = result.d;
                    if (d.length > 0)
                    {
                        var Default_DrugID = $("#<%= txtDrugID.ClientID %>");
                        if (Default_DrugID.val().indexOf(d[0].ID) == -1)
                        {
                            Default_DrugID.val(d[0].ID);
                            ssRequiredDrugSelected.val("true");
                        }
                    }
                });
            }
        }
        
        function rdcmbTheraClass_DropDownClosed(sender, args)
        {
            var vals = sender._getControl()._value;
            //var druglist = $("#<%= txtDrugList.ClientID %>").val();

            var drugs = $get(drugCtrlID).control;
            var drug_id = $get("Drug_ID").control;

            var url = "custom/Alcon/sellsheetreporting/services/AlconService.svc" + "/SellSheetDrugListSet?$filter=Thera_ID eq " + vals + "&$orderby=Name";
            $.getJSON(url, null, function(result, status)
            {
                var d = result.d;
                $loadPinsoListItems(drug_id, d, null, -1);

                var selectedTheraDrugs = $("#<%= txtDrugID.ClientID %>").val();

                if (selectedTheraDrugs != "")
                    selectDrugList(selectedTheraDrugs, drug_id);

                //Set text which appears on the top of the drug dropdown list.
                $updateCheckboxDropdownText(drugs, "Drug_ID");
            });

            //If Thera Selection is changed, set originally selected drugs as selected


            $('#mycarousel li').css({ display: "none" });
            $('#templateSidebar li').css({ display: "none" });
            $('#mycarousel li').filter('[thera_id =' + vals + ']').removeAttr('style');
            $('#templateSidebar li').filter('[thera_id =' + vals + ']').removeAttr('style');

            var template_id = $('#mycarousel li').filter('[thera_id =' + vals + '][default=true]').attr('rel');
            
            setTheraDrugTemplate(template_id);

            //     $('#mycarousel li').filter('[thera_id =' + vals + ']').find('img').addClass('selectedTemplate');
            //     $('#templateSidebar li').filter('[thera_id =' + vals + ']').find('img').addClass('selectedTemplate');

            if (vals) {
                $("#<%= txtTheraID.ClientID %>").val(vals);
            }
            else
                $("#<%= txtTheraID.ClientID %>").val("");
            validateTemplate();

            //Call the Drugs_DropDownClosed function to update the selected 
            //values textbox in case a Thera Class was removed
            combo = $get(drugCtrlID).control;

            rdcmbDrugs_DropDownClosed(combo, null);

        }

        //This Function is used to update the hidden Field "txtDrugList" with the selected drugs from the drop down list.
        // This Function is called from rdcmbDrugs_DropDownClosed function and validateDrugSelection function (Alcon.js)
        function UpdateSelectedDrugList()
        {
            var vals = $get(drugCtrlID).control.get_element().checkboxList.get_values();
            var thera_id = $("#<%= txtTheraID.ClientID %>").val();
            var y;
            var z;
            for (y in drugListOptions[thera_id])
            {
                if ($.isArray(vals))
                {
                    for (z in vals)
                    {
                        if (drugListOptions[thera_id][y].ID == vals[z])
                            vals[z] += "|" + thera_id;
                    }
                }
                else
                {
                    if (drugListOptions[thera_id][y].ID == vals)
                        vals += "|" + thera_id;
                }
            }
            //}

            if ($.isArray(vals))
                vals = vals.join(",");

            var Selected_DrugIds = $("#<%= txtDrugList.ClientID %>");
            //alert(vals);
            if (vals)
                Selected_DrugIds.val(vals);
            else
                Selected_DrugIds.val("");
            
        }
        function rdcmbDrugs_DropDownClosed(sender, args)
        {
            //Get selected values on custom DropList and set ASPX Hidden Label value for submit
            //var vals = sender.get_element().checkboxList.get_values();

            UpdateSelectedDrugList();
            var template_id = $("#<%= txtTemplateID.ClientID %>").val();
            var Selected_DrugIds = $("#<%= txtDrugList.ClientID %>");
            validateDrugSelection(Selected_DrugIds, ssRequiredDrugSelected, template_id);

        }

        function selectDrugList(selectedDrugs, drug_id)
        {
            drug_id.reset();
            
            var selectedSplitTheraDrugs;
            //Check to see if more than more option was selected
            if (selectedDrugs.indexOf(',') > 0)
            {
                selectedSplitTheraDrugs = selectedDrugs.split(',');
                var x;

                for (x in selectedSplitTheraDrugs)
                {
                    //Split each selection (7012|27 - DrugID|TheraID)
                    var splitDrug = selectedSplitTheraDrugs[x].split('|');
                    drug_id.selectItem(splitDrug[0]);
                }
            }
            else
            {
                selectedSplitTheraDrugs = selectedDrugs.split('|');
                drug_id.selectItem(selectedSplitTheraDrugs[0]);
            }
        }

        function setTheraDrugTemplate(templateID)
        {
            var selected_template_ID = $("#<%= txtTemplateID.ClientID %>").val();
            $("#<%= txtTemplateID.ClientID %>").val(templateID);
            //Remove all CSS classes in Template Selector
            $('#mycarousel li img').removeClass('selectedTemplate');
            //Add CSS class to selected item
            $('#mycarousel li').filter('[rel=' + templateID + ']').find('img').removeAttr("style");
            //Must call relect before selectedTemplate class is applied
            //$("#mycarousel li").find('img').reflect(50);
            $('#mycarousel li').filter('[rel=' + templateID + ']').find('img').addClass('selectedTemplate');
            
            $('#templateSidebar').css("left", ""); 
            $('#templateSidebar li').filter('[rel=' + templateID + ']').find('img').addClass('selectedTemplate');

            if (typeof(templateID) != "undefined")
            {
                if (selected_template_ID && selected_template_ID != templateID)
                {
                    var selected_thera_id = $("#<%= txtTheraID.ClientID %>").val();
                    var url = "custom/Alcon/sellsheetreporting/services/AlconService.svc" + "/SellSheetDrugListSet?$filter=Thera_ID eq " + selected_thera_id + " and Template_ID eq " + templateID;
                    $.getJSON(url, null, function(result, status)
                    {
                        var d = result.d;
                        if (d.length > 0)
                        {
                            $("#<%= txtDrugID.ClientID %>").val(d[0].ID);
                            var selectedTheraDrugs = $("#<%= txtDrugID.ClientID %>").val();

                            var drug_id = $get("Drug_ID").control;
                            var drugs = $get(drugCtrlID).control;

                            if (selectedTheraDrugs != "")
                                selectDrugList(selectedTheraDrugs, drug_id);
                            //Set text which appears on the top of the drug dropdown list.
                            $updateCheckboxDropdownText(drugs, "Drug_ID");
                        }
                    });

                }
            }

            //var drugs = $get(drugCtrlID).control;
            //$updateCheckboxDropdownText(drugs, "Drug_ID");
        }

        function sortDrugList(a, b)
        {
            if (a.Name.toLowerCase() > b.Name.toLowerCase())
                return 1;
            else if (a.Name.toLowerCase() < b.Name.toLowerCase())
                return -1;
            return 0;
        }
        function validateTemplate() {
            var thera_id = $("#<%= txtTheraID.ClientID %>").val();
            var template_id = $("#<%= txtTemplateID.ClientID %>").val();
            if (thera_id.length > 0 && template_id.length > 0)
            {
                var url = "custom/Alcon/sellsheets/services/AlconDataService.svc/check_template?theraid=" + thera_id + "&templateid=" + template_id;
                $.getJSON(url, null, function(result, status) {
                    var d = result.d;
                    if (d.check_template == false) {
                        $("#<%= txtTemplateID.ClientID %>").val("");
                    }
                });
            }
        }

       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="StepBody" Runat="Server">
    <asp:HiddenField ID="txtTheraID" runat="server" />
    <asp:HiddenField ID="txtDrugList" runat="server" />        
    <asp:HiddenField ID="txtDrugID" runat="server" />
    <asp:HiddenField ID="txtSellSheetID" runat="server" />
    <asp:HiddenField ID="txtTemplateID" runat="server" />
    <asp:HiddenField ID="txtRequiredDrugSelected" runat="server" />
    <asp:Label ID="msglbl" runat="server" Visible="false" Text="Saving changes will reset the plan selection and the sell sheet will be moved to the drafted sell sheets section." style="color:Red;"></asp:Label>
    
    <div align="center" id="classAndTemplateContainer">
    <table>
    <tr>
        <td>&nbsp;</td>
    </tr>
    <tr>
        
        <td valign="middle" align="left"> 
            
            <span class="ssBold">Therapeutic Class:</span>
        </td>
        <td>
            <telerik:RadComboBox ID="rdcmbTheraClass" runat="server" EnableEmbeddedSkins="false"
                Skin="pathfinder" OnClientDropDownClosed="rdcmbTheraClass_DropDownClosed"
                 DataSourceID="ds_thera" DataTextField="Thera_Name" DataValueField="Thera_ID" 
                 oDropDownWidth="300px" MaxHeight="250px" AppendDataBoundItems="true" OnClientSelectedIndexChanged="UpdateDrugSelection">
                 <Items>
                    <telerik:RadComboBoxItem Text="--No Selection--" Value="0" Selected="true" />
                 </Items>
            </telerik:RadComboBox>
            <asp:SqlDataSource ID="ds_thera" runat="server" ConnectionString="<%$ ConnectionStrings:PathfinderClientDB_Format %>"
                SelectCommand="usp_SellSheet_Theralist"
                 SelectCommandType="StoredProcedure">
                 <SelectParameters>
                      <asp:QueryStringParameter  Name="Sell_Sheet_ID"  QueryStringField="Sell_Sheet_ID" DbType ="Int32"  />
                 </SelectParameters>
                 </asp:SqlDataSource>
            <pinso:ClientValidator ID="vldThera" runat="server" Target="rdcmbTheraClass" Required="true" Text="Please select a Therapeutic Class" />
        </td>
        <td valign="middle">
            <span class="ssBold">Drug Selection:</span>
        </td>
        <td>
            <telerik:RadComboBox ID="rdcmbDrugs" runat="server" EnableEmbeddedSkins="false"
              SkinID="planInfoCombo" Skin="pathfinder" Height="160px" OnClientDropDownClosed="rdcmbDrugs_DropDownClosed" >
            </telerik:RadComboBox>
            <pinso:ClientValidator ID="vldDrugs" runat="server" Target="rdcmbDrugs" Required="true"  Text="Please select at least one drug" />
        </td>
    </tr>
    <tr>
        <td colspan="4" align="left">
            <span class="ssBold" >Templates for <asp:Label ID="lblBrand" runat="server"></asp:Label></span>
            <div id="ssTemplateDivider"></div>
        </td>
    </tr>
    <tr>
        <td colspan="4">
            <ul id="mycarousel" class="jcarousel-skin-step1">
                        <!-- Added rel attribute to image so selected template can be highlighted in css on page load -->
                         <li id="Li6" runat="server" rel="16" thera_id="149"  default="true" >
                            <asp:Image ID="Image6" runat="server" ImageUrl="custom/alcon/sellsheets/templates/PTD_Pathfinder_Rx_Formulary_portraitmed.jpg" OnClick="setTheraDrugTemplate(16)"/>
                        </li>
                        <li id="Li1" runat="server" rel="3" thera_id="150"  default="true">
                            <asp:Image ID="Image1" runat="server" ImageUrl="custom/Alcon/sellsheets/templates/AZT_Pathfinder_Rx_Formulary_portraitmed.jpg" OnClick="setTheraDrugTemplate(3)"/>
                        </li>
                        <li id="Li8" runat="server" rel="18" thera_id="1501"  default="true">
                            <asp:Image ID="Image8" runat="server" ImageUrl="custom/alcon/sellsheets/templates/TRZ_Pathfinder_Rx_Formulary_portraitmed.jpg" OnClick="setTheraDrugTemplate(18)"/>
                        </li>
                        <li id="Li10" runat="server" rel="20" thera_id="151">
                            <asp:Image ID="Image10" runat="server" ImageUrl="custom/alcon/sellsheets/templates/VIG_Pathfinder_Rx_Formulary_portraitmed.JPG" OnClick="setTheraDrugTemplate(20)"/>
                        </li>
                        <li id="Li4" runat="server" rel="14" thera_id="151" default="true">
                            <asp:Image ID="Image4" runat="server" ImageUrl="custom/alcon/sellsheets/templates/MZA_Pathfinder_Rx_Formulary_portraitmed.jpg" OnClick="setTheraDrugTemplate(14)"/>
                        </li>
                        <li id="Li3" runat="server" rel="13" thera_id="159"  default="true">
                            <asp:Image ID="Image3" runat="server" ImageUrl="custom/alcon/sellsheets/templates/DZL_Pathfinder_Rx_Formulary_portraitmed.jpg" OnClick="setTheraDrugTemplate(13)"/>
                        </li>
                         <li id="Li5" runat="server" rel="15" thera_id="157"  default="true">
                            <asp:Image ID="Image5" runat="server" ImageUrl="custom/alcon/sellsheets/templates/NEV_Pathfinder_Rx_Formulary_portraitmed.jpg" OnClick="setTheraDrugTemplate(15)"/>
                        </li>
                        <li id="Li7" runat="server" rel="17" thera_id="158"  default="true">
                            <asp:Image ID="Image7" runat="server" ImageUrl="custom/alcon/sellsheets/templates/TDXST_Pathfinder_Rx_Formulary_portraitmed.jpg" OnClick="setTheraDrugTemplate(17)"/>
                        </li>
                        <li id="Li2" runat="server" rel="4" thera_id="153"  default="true">
                            <asp:Image ID="Image2" runat="server" ImageUrl="custom/alcon/sellsheets/templates/CDX_Pathfinder_Rx_Formulary_portraitmed.jpg" OnClick="setTheraDrugTemplate(4)"/>
                        </li>
                         <li id="Li9" runat="server" rel="19" thera_id="187"  default="true">
                            <asp:Image ID="Image9" runat="server" ImageUrl="custom/alcon/sellsheets/templates/Patanase_portraitmed.jpg" OnClick="setTheraDrugTemplate(19)"/>
                        </li>
                        </ul>
        </td>
    </tr>
    </table>
    </div>
    <pinso:ClientValidator ID="vldTemplate" runat="server" Target="txtTemplateID" Required="true" Text="Please select a template." />    
    <pinso:ClientValidator ID="vldReqDrug" runat="server" Target="txtRequiredDrugSelected" Required="true" Text="Please select the template drug." />
    <br />
    <asp:Button ID="btnNext" runat="server" Text="Next" CssClass="postback validate btnSingle" 
        onclick="btnNext_Click" />
</asp:Content>

