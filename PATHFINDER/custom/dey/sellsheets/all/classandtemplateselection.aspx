<%@ Page Title="" Language="C#" EnableViewState="true" MasterPageFile="~/custom/dey/sellsheets/Dey_SellSheetStep.master" AutoEventWireup="true" CodeFile="classandtemplateselection.aspx.cs" Inherits="custom_dey_sellsheets_classandtemplateselection" %>
<%@ MasterType VirtualPath="~/custom/dey/sellsheets/Dey_SellSheetStep.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="scriptContainer" Runat="Server">
    <!--[if lt IE 8]>
        <style type="text/css">
        #tile3 #divTile3
        {
            position: relative !important;
        }
        </style>
    <![endif]-->  
    <!--[if IE 7]>
        <style type="text/css">
        .btnSingle
        {
            position: fixed !important;
            bottom: 40px !important;
            right: 30px !important;
        }
        </style>
    <![endif]-->
    <!--[if lt IE 7]>
        <style type="text/css">
        .btnSingle
        {
            position: absolute !important;
        }       
        </style>
    <![endif]-->
    <script type="text/javascript" >
        clientManager.add_pageLoaded(classTemplateSelection_pageLoaded);
        clientManager.add_pageUnloaded(classTemplateSelection_pageUnloaded);

        var ssRequiredDrugSelected = $("#<%= txtRequiredDrugSelected.ClientID %>");
        var ssDruglistSelected = $("#<%= txtDrugID.ClientID %>"); 
        
        function classTemplateSelection_pageLoaded(sender, args)
        {
            //IE 6 Fix - fixed next button postion
            if (/MSIE (\d+\.\d+);/.test(navigator.userAgent))
                var ieversion = new Number(RegExp.$1);

            if (ieversion <= 6)
            {
                var height = getWorkspaceHeight();

                $("#divTile3").height(height - $("#divTile3Container .tileContainerHeader").height() + 5);
            }
            //END IE 6 Fix            

            //Create Drug list
            $createCheckboxDropdown(drugCtrlID, "Drug_ID", null, { 'maxItems': 3, 'defaultText': '<%= Resources.Resource.Label_No_Selection %>', 'multiItemText': '<%= Resources.Resource.Label_Multiple_Selection %>' }, { 'error': classTemplateSelection_onDrugListError }, null);

            //Update selected item in Drug list
            $updateCheckboxDropdownText(drugCtrlID, "Drug_ID");
            
           

            //Load page if editing
            var thera = $("#<%= txtTheraID.ClientID %>").val();
            var drugs = $("#<%= txtDrugID.ClientID %>").val();
            var drug_id = $get("Drug_ID").control;
            var thera_id = $find(theraCtrlID);

            if (thera.length > 0)
            {
                var itm = thera_id.findItemByValue(thera);
                itm.select();
                $loadPinsoListItems(drug_id, customdrugListOptions[thera], null, -1);
                
                if (drugs.length > 0)
                {
                    $loadPinsoListItems(drug_id, customdrugListOptions[thera], null, -1,true);
                    selectDrugList(drugs, drug_id);
                }
                $updateCheckboxDropdownText(drugCtrlID, "Drug_ID");
                UpdateSelectedDrugList();
                validateDrugSelection(ssDruglistSelected, ssRequiredDrugSelected);
            }
            
            //$("#mycarousel li").find('img').reflect(50);

            //Get the carousel list item index to load selected template
            var templateid = $("#<%= txtTemplateID.ClientID %>").val();
            var items = $('#mycarousel li');
            var selectedTemplate = $('#mycarousel li').filter('[rel=' + templateid + ']');
            var index = items.index(selectedTemplate);

            //Remove style attribute because it has a default border attribute
            //$('#mycarousel li').filter('[rel=' + templateid + ']').find('img').removeAttr("style");
            $('#mycarousel li').find('img').removeAttr("style");
            $('#mycarousel li').filter('[rel=' + templateid + ']').find('img').addClass('selectedTemplate');

            //Initialize carousel
            $('#mycarousel').jcarousel(
            {
                scroll: 1,
                start: index,
                initCallback: template_initCallback
            });

            //Bind Carousel Sidebar click event
            $('#mycarousel li').bind('click', function()
            {
                //Find the same list item in the sidebar by the rel attribute and obtain the index
                var items = $('#templateSidebar li');
                var selectedTemplate = $('#templateSidebar li').filter('[rel=' + this.getAttribute('rel') + ']');
                var index = items.index(selectedTemplate);

                //Scroll to the item based on the index
                sender.get_ApplicationManager().get_carousel().scroll(index);

                //Add the selectedTemplate CSS class to the selected template
                $('#templateSidebar li img').removeClass('selectedTemplate');
                //$('#templateSidebar li').filter('[rel=' + this.getAttribute('rel') + ']').find('img').removeAttr("style");
                //$('#templateSidebar li').find('img').removeAttr("style");
                $('#templateSidebar li').filter('[rel=' + this.getAttribute('rel') + ']').find('img').addClass('selectedTemplate');
                return false;
            });
        }

        function template_initCallback(carousel)
        {
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
            $alert("Maximum 3 drugs should be selected.", "Warning");
        }

        function rdcmbTheraClass_DropDownClosed(sender, args)
        {
            var vals = sender._getControl()._value;
            var drugs = $get(drugCtrlID).control;
            var drug_id = $get("Drug_ID").control;

            $loadPinsoListItems(drug_id, customdrugListOptions[vals], null, -1);

            //If Thera Selection is changed, set originally selected drugs as selected
            var selectedTheraDrugs = $("#<%= txtDrugID.ClientID %>").val();
            if (selectedTheraDrugs != "")
                selectDrugList(selectedTheraDrugs, drug_id);

            //Set text which appears on the top of the drug dropdown list.
            $updateCheckboxDropdownText(drugs, "Drug_ID");

            if ($.isArray(vals))
                vals = vals.join(",");

            if (vals)
                $("#<%= txtTheraID.ClientID %>").val(vals);
            else
                $("#<%= txtTheraID.ClientID %>").val("");

            //Call the Drugs_DropDownClosed function to update the selected 
            //values textbox in case a Thera Class was removed
            combo = $get(drugCtrlID).control;
            rdcmbDrugs_DropDownClosed(combo, null);
        }

        function sortDrugList(a, b)
        {
            if (a.Name.toLowerCase() > b.Name.toLowerCase())
                return 1;
            else if (a.Name.toLowerCase() < b.Name.toLowerCase())
                return -1;
            return 0;
        }

        function rdcmbDrugs_DropDownClosed(sender, args)
        {
            UpdateSelectedDrugList();

            validateDrugSelection(ssDruglistSelected, ssRequiredDrugSelected);
        }

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

           if (vals)
                ssDruglistSelected.val(vals);
            else
                ssDruglistSelected.val("");

        }
        function selectDrugList(selectedDrugs, drug_id)
        {
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
            $("#<%= txtTemplateID.ClientID %>").val(templateID);

            //Remove all CSS classes in Template Selector
            $('#mycarousel li img').removeClass('selectedTemplate');

            //Add CSS class to selected item
            //$('#mycarousel li').filter('[rel=' + templateID + ']').find('img').removeAttr("style");

            //Must call relect before selectedTemplate class is applied
            //$("#mycarousel li").find('img').reflect(50);

            $('#mycarousel li').filter('[rel=' + templateID + ']').find('img').addClass('selectedTemplate');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="StepBody" Runat="Server">
    <asp:HiddenField ID="txtTheraID" runat="server" />        
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
                DropDownWidth="300px" MaxHeight="250px" AppendDataBoundItems="true" Width="200px">
                <Items>
<%--                    <telerik:RadComboBoxItem Text="--No Selection--" Value="0" Selected="true" />
--%>                 </Items>
            </telerik:RadComboBox>
            <asp:SqlDataSource ID="ds_thera" runat="server"
            SelectCommand="usp_SellSheet_Theralist"  SelectCommandType="StoredProcedure">
              <SelectParameters>
                  <asp:QueryStringParameter  Name="Sell_Sheet_ID"  QueryStringField="Sell_Sheet_ID" DbType ="Int32"  />
              </SelectParameters>
            </asp:SqlDataSource>
            <pinso:ClientValidator ID="vldThera" runat="server" Target="rdcmbTheraClass" Required="true"  Text="Please select a Therapeutic Class" />
        </td>
        <td valign="middle">
            <span class="ssBold">Drug Selection:</span>
        </td>
        <td>
            <telerik:RadComboBox ID="rdcmbDrugs" runat="server" EnableEmbeddedSkins="false" 
                SkinID="planInfoCombo" Skin="pathfinder" Height="160px" 
                OnClientDropDownClosed="rdcmbDrugs_DropDownClosed" >
            </telerik:RadComboBox>
            <pinso:ClientValidator ID="vldDrugs" runat="server" Target="rdcmbDrugs" Required="true" Text="Please select at least one drug" />
        </td>
    </tr>
    <tr>
        <td colspan="4" align="left">
            <span class="ssBold" >Templates for <asp:Label ID="lblBrand" runat="server"></asp:Label></span>
            <div id="ssTemplateDivider"></div>
        </td>
    </tr>
    <tr>
        <td colspan="4" align="center">     
        <ul id="mycarousel" class="jcarousel-skin-step1">
            <asp:Repeater ID="rptTemplates" runat="server" DataSourceID="dsTemplates">
                <ItemTemplate>
                <!-- Added rel attribute to image so selected template can be highlighted in css on page load -->
                <li id="Li1" runat="server" rel='<%# Eval("Template_ID") %>'><asp:Image ID="Image1" runat="server" ImageUrl='<%# String.Format("custom/{0}/sellsheets/templates/{1}", Pinsonault.Web.Session.ClientKey, Eval("Template_Name") ).Replace(".", "med.") %>' OnClick='<%# String.Format("setTheraDrugTemplate({0})", Eval("Template_ID") ) %>'/></li>
                </ItemTemplate>
            </asp:Repeater>
            <asp:EntityDataSource ID="dsTemplates" runat="server" EntitySetName="TheraDrugTemplateSet" DefaultContainerName="PathfinderClientEntities">
            </asp:EntityDataSource>
        </ul>
        </td>
    </tr>
    </table>
    </div>
    <pinso:ClientValidator ID="vldTemplate" runat="server" Target="txtTemplateID" Required="true" Text="Please select a template" />
    <pinso:ClientValidator ID="vldReqDrug" runat="server" Target="txtRequiredDrugSelected" Required="true" CompareToValue="true" Text="Please select the template drug." />
    
    <br />
    <asp:Button ID="btnNext" runat="server" Text="Next" CssClass="postback validate btnSingle" 
        onclick="btnNext_Click" />
</asp:Content>

