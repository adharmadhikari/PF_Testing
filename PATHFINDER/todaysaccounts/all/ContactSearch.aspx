<%@ Page Language="C#" Theme="pathfinder" AutoEventWireup="true" CodeFile="ContactSearch.aspx.cs" Inherits="general_ContactSearch" MasterPageFile="~/MasterPages/Modal.master" %>
<%@ OutputCache Duration="10" VaryByParam="None" %>

<asp:Content runat="server" ContentPlaceHolderID="title">
    <asp:Literal runat="server" Text='<%$ Resources:Resource, DialogTitle_ContactSearch %>' />
    <span class="mr10 ml10" style="cursor:pointer" onclick="clearAll()">Reset</span>
    <span id="contactTypeOptions">
        <input type="radio" id="ctc1" name="ctc" checked="checked" onclick="contactTypeClick(this, {value:null})" /><label for="ctc1">All</label>
        <input type="radio" id="ctc2" name="ctc" onclick="contactTypeClick(this, {value:'Regular'})" /><label for="ctc2"><%= Resources.Resource.SectionTitle_KeyContacts %></label>
        <input type="radio" id="ctc3" name="ctc" onclick="contactTypeClick(this, {value:'Customized'})" /><label for="ctc3"><%= Resources.Resource.SectionTitle_MyKeyContacts %></label>
    </span> 
</asp:Content>

<asp:Content ContentPlaceHolderID="tools" runat="server" ID="toolsContent">
<span class="textResize"><span class="textSm"><a href="javascript:void(0);" onclick="textSmall();">A</a></span><span class="textMd"><a href="javascript:void(0);" onclick="textMedium();">A</a></span><span class="textLg"><a href="javascript:void(0);" onclick="textLarge();">A</a></span></span>
    
</asp:Content>

<asp:Content runat="server" ID="content1" ContentPlaceHolderID="main">


    <script type="text/javascript">
        
        var clientManager = window.top.clientManager;
        var uiready = false; //flag to prevent updates to the grid while filter controls are being reset.
        uiready = true;
        
        function loadSections()
        {
            uiready = false;
            var src = clientManager.get_ChannelMenuOptions()[1];
            var items = {};
            //need to exclude "All" option - using blank default option instead, exclude combined option also
            $.each(src, function(k, o)
            {
                if (src[k]["ID"] != 0 && src[k]["ID"] != 99) items[k] = o;
            });
            $loadListItems($get("ctl00_main_gridContacts_ctl00_ctl02_ctl02_sectionId").control, items, { value: null, text: "" });
            uiready = true;
        }
                        
        function getParentGrid()
        {
            return $get("ctl00_main_gridContacts").control;
        }

        function getGridWrapper()
        {
            return $get("ctl00_main_gridContacts").GridWrapper;
        }

        function getContactGrid()
        {
            ///<summary>Helper function to return the grid's MasterTableView object.</summary>
            return getParentGrid().get_masterTableView();
        }

        function contactTypeClick(sender, args)
        {
            if(sender.checked)
                onComboChanged("Contact_Type", args.value);
        }
        
        function onSectionComboChanged(sender, args)
        {
            ///<summary>Event handler for Section ID combo box.  When a selection changes a new filter is applied and the grid results are updated.</summary>
            
            onComboChanged("Section_ID", args.get_item().get_value(), "System.Int32");
        }

        function onTitleComboChanged(sender, args)
        {
            ///<summary>Event handler for Contact Title (Designation) combo box.  When a selection changes a new filter is applied and the grid results are updated.</summary>

            onComboChanged("KC_Title_ID", args.get_item().get_value(), "System.Int32");            
        }

        function onContactTypeComboChanged(sender, args)
        {
            ///<summary>Event handler for Contact Type combo box.  When a selection changes a new filter is applied and the grid results are updated.</summary>
            
            onComboChanged("Contact_Type", args.get_item().get_value());
        }

        function onComboChanged(fieldName, val, dataType)
        {
            ///<summary>Helper function for apply a filter for a combobox</summary>

            if (uiready)
            {
                var grid = getContactGrid();

                if (val == "") val = null;

                $setGridFilter(grid, fieldName, val, null, dataType);
                new cmd(grid, "rebind", [], 300);
            }
        }
        
        function addFilter(grid, controlID, fieldName, filterType, dataType)
        {
            ///<summary>helper function for adding a filter for textboxes.</summary>
            var val = $.trim($("#" + controlID).val());
            if (val != "")
                $setGridFilter(grid, fieldName, val, filterType, dataType);
            else
                $clearGridFilter(grid, fieldName);
        }

        var searchCmd;
        
        function setSearchTimeout(sender, args)
        {
            ///<summary>Initiates a search after a key is pressed in a filter textbox - if a previous action was started it is cancelled to allow user to continue typing prior to search executing.</summary>
            if (args.event.keyCode != 0 && 
                (args.event.keyCode < 48
                || (args.event.keyCode > 90 && args.event.keyCode < 96)
                || (args.event.keyCode > 111 && args.event.keyCode < 186))) return;
            
            if (searchCmd)
                searchCmd.cancel();

            searchCmd = new cmd(null, searchContacts, [sender, args], 500);            
        }
        
        function searchContacts(args)
        {
            ///<summary>Executes a search adding a new filter for the last filter textbox typed in.</summary>            
            var grid = getContactGrid();

            addFilter(grid, args[0].id, args[1].fieldName, args[1].filterType);     

            grid.rebind();
        }
        
        function clearFilters()
        {
            //Clear filters - set uiready to false so selectedIndexChanged event can skip event when control is reset with call to "select()".
            uiready = false;
            $resetContainer(getParentGrid().get_masterTableViewHeader().get_element().id);
            $get("ctc1").checked = true;
            uiready = true;
            //
            
            getContactGrid().get_filterExpressions().clear();            
        }

        function clearAll()
        {
            clearFilters();
            getGridWrapper().dataBind();
        }
    </script>

    <telerik:radwindowmanager EnableEmbeddedSkins="false" Skin="pathfinder" id="RadWindowManager1" runat="server" DestroyOnClose="true" Modal="true" Behaviors="Close" VisibleTitlebar="false" />    
    
     <telerik:RadGrid SkinID="radTable" runat="server" ID="gridContacts" AllowSorting="true" PageSize="50" AllowPaging="true" AllowFilteringByColumn="true" EnableEmbeddedSkins="false">        
        <MasterTableView autogeneratecolumns="False" AllowSorting="true" ClientDataKeyNames="KC_ID" PageSize="50" AllowMultiColumnSorting="true">
            <SortExpressions>
                <telerik:GridSortExpression FieldName="Plan_Name" SortOrder="Ascending" />
                <telerik:GridSortExpression FieldName="Section_Name" SortOrder="Ascending" />
                <telerik:GridSortExpression FieldName="KC_F_Name" SortOrder="Ascending" />
                <telerik:GridSortExpression FieldName="KC_L_Name" SortOrder="Ascending" />
            </SortExpressions>           
            <Columns>            
                <telerik:GridBoundColumn DataField="Plan_Name" DataFormatString="{0} " HeaderStyle-Width="200px" HeaderText='<%$ Resources:Resource, label_plan_name %>' SortExpression="Plan_Name" UniqueName="Plan_Name">
                    <FilterTemplate>
                        <div class="searchTextBox"><pinso:SearchTextBox runat="server" id="planName" CssClass="textBox" FieldName="Plan_Name" /></div>
                    </FilterTemplate>                 
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Section_Name" HeaderStyle-Width="150px" HeaderText='<%$ Resources:Resource, label_section_name %>' SortExpression="Section_Name" UniqueName="Section_Name">
                    <FilterTemplate>
                        <telerik:RadComboBox runat="server" ID="sectionId" OnClientLoad="loadSections" Width="90%" DropDownWidth="170px" EnableEmbeddedSkins="false" Skin="pathfinder" SkinID="planInfoCombo" AppendDataBoundItems="true" OnClientSelectedIndexChanged="onSectionComboChanged" />
                    </FilterTemplate>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="KC_F_Name" HeaderStyle-Width="150px" HeaderText='<%$ Resources:Resource, label_first_name %>' SortExpression="KC_F_Name" UniqueName="KC_F_Name">
                    <FilterTemplate>
                        <pinso:SearchTextBox runat="server" id="firstName" CssClass="textBox" FieldName="KC_F_Name" />
                    </FilterTemplate>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="KC_L_Name" HeaderStyle-Width="150px" HeaderText='<%$ Resources:Resource, label_last_name %>' SortExpression="KC_L_Name" UniqueName="KC_L_Name">
                    <FilterTemplate>
                        <pinso:SearchTextBox runat="server" id="lastName" class="textBox" FieldName="KC_L_Name"  />
                    </FilterTemplate>
                </telerik:GridBoundColumn>                
                <telerik:GridBoundColumn DataField="KC_Title_Name" HeaderStyle-Width="200px" HeaderText='<%$ Resources:Resource, label_designation %>' SortExpression="KC_Title_Name" UniqueName="KC_Title_Name">
                    <FilterTemplate>
                        <telerik:RadComboBox runat="server" ID="titleId" Width="90%" DropDownWidth="220px" Height="250px" SkinID="planInfoCombo" Skin="pathfinder" EnableEmbeddedSkins="false" DataSourceID="dsTitles" DataTextField="Name" DataValueField="ID" AppendDataBoundItems="true" OnClientSelectedIndexChanged="onTitleComboChanged">
                            <Items>
                                <telerik:RadComboBoxItem Text="" />
                            </Items>
                        </telerik:RadComboBox>
                    </FilterTemplate>                    
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="KC_Role" HeaderStyle-Width="150px" HeaderText='<%$ Resources:Resource, label_title %>' SortExpression="KC_Role" UniqueName="KC_Role">
                    <FilterTemplate>
                        <pinso:SearchTextBox runat="server" id="role" CssClass="textBox" FieldName="KC_Role" />
                    </FilterTemplate>                
                </telerik:GridBoundColumn>
                <telerik:GridHyperLinkColumn DataNavigateUrlFields="KC_Email_RadGrid_EvalHack" DataNavigateUrlFormatString="mailto:{0}" DataTextField="KC_Email" HeaderStyle-Width="200px" HeaderText='<%$ Resources:Resource, label_email %>' SortExpression="KC_Email" UniqueName="KC_Email">
                    <FilterTemplate>
                        <pinso:SearchTextBox runat="server" id="email" CssClass="textBox" FieldName="KC_Email" />
                    </FilterTemplate>
                </telerik:GridHyperLinkColumn>
                <telerik:GridBoundColumn DataField="KC_Phone" HeaderStyle-Width="150px" HeaderText='<%$ Resources:Resource, label_phone %>' SortExpression="KC_Phone" UniqueName="KC_Phone">
                    <FilterTemplate>
                        <pinso:SearchTextBox runat="server" id="phone" CssClass="textBox" FieldName="KC_Phone" />
                    </FilterTemplate>   
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="KC_Admin_Name" HeaderStyle-Width="150px" HeaderText='<%$ Resources:Resource, label_assistant_name %>' SortExpression="KC_Admin_Name" UniqueName="KC_Admin_Name">
                    <FilterTemplate>
                        <pinso:SearchTextBox runat="server" id="assistant" CssClass="textBox" FieldName="KC_Admin_Name" />
                    </FilterTemplate>                   
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="KC_Admin_PH" HeaderStyle-Width="150px" HeaderText='<%$ Resources:Resource, label_assistant_phone %>' SortExpression="KC_Admin_PH" UniqueName="KC_Admin_PH">
                    <FilterTemplate>
                        <pinso:SearchTextBox runat="server" id="officePhone" CssClass="textBox" FieldName="KC_Admin_PH" />
                    </FilterTemplate>                   
                </telerik:GridBoundColumn>
            </Columns> 
            
            <PagerStyle Visible="false" />
        </MasterTableView>    
        <ClientSettings>
            <DataBinding Location="~/todaysaccounts/services/pathfinderclientdataservice.svc" DataService-TableName="KeyContactSearchSet" SelectCountMethod="GetKeyContactSearchCount" />
            <Scrolling UseStaticHeaders="true" AllowScroll="true" />
            <Selecting AllowRowSelect="true" />
        </ClientSettings>
    </telerik:RadGrid>
    <pinso:RadGridWrapper runat="server" ID="radGridWrapper" Target="gridContacts" PagingSelector=".pagination" AutoLoad="true" AutoUpdate="false" RequiresFilter="false" />
    
    
    <%--<asp:EntityDataSource runat="server" ID="dsSections" EntitySetName="SectionSet" ConnectionString="name=PathfinderEntities" OrderBy="it.Name" DefaultContainerName="PathfinderEntities" />--%>
    <asp:EntityDataSource runat="server" ID="dsTitles" ConnectionString="name=PathfinderEntities" DefaultContainerName="PathfinderEntities" EntitySetName="TitleSet" OrderBy="it.Name" />
</asp:Content>