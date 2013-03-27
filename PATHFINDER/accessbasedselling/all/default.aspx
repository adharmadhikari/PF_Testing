<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SplitSection.master" Theme="pathfinder" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="accessbasedselling_all_default" %>


<asp:Content ContentPlaceHolderID="scriptContainer" runat="server" ID="scriptContainer">


    <script type="text/javascript">

        clientManager.add_pageLoaded(abs_pageLoaded);
        clientManager.add_pageUnloaded(abs_pageUnloaded);
        var absUIReady = false;
        
        function abs_pageLoaded(sender, args)
        {
            
            $("#ctl00_Tile3_gridCompAnalysis").hide();

            if ($.browser.msie && $.browser.version < 8)
                $("#imgAbsChart").css("height", "220");
                        
            c = $find("ctl00_Tile3Title_rcbTopPayers");
//            alert(c);
            if (c)
            {
                c.get_items().getItem(1).select(); //set_selectedIndex(1);
            }

            c = $find("absmodules");
            if (c)
            {
                c.addItem("companalysis", "Comparative Analysis", "companalysis");
                c.add_itemClicked(onModuleClicked);
            }
            
            c = $find("abschartmodules");
            if(c)
            {
                resetChartModules();
                c.add_itemClicked(onChartModuleClicked);
            }

            if(clientManager.get_UserGeography().RegionID == "CA")
            {
                $("#divAssignment").hide();

                $(".chkb").remove();
                
                c = $get("ctl00_Tile3_gridPrescribers").GridWrapper;
                if (c)
                {
                    c.add_dataBound(dataBound);                    
                }
            }

            c = $find("ctl00_Tile3_gridCompAnalysis");
            if (c)
            {
                if (clientManager.get_UserGeography().RegionID == "CA")
                    c.get_element().GridWrapper.add_dataBound(dataBound);                    
                    
                c = c.get_masterTableView().get_element();
                c = $(c).find("thead")[0];
                if (c)
                {
                    var r = c.insertRow(0);
                    c = r.insertCell(-1);
                    c.colSpan = clientManager.get_UserGeography().RegionID == "CA" ? 2 : 3;
                    c.innerHTML = "&nbsp;";
                    c = r.insertCell(-1);
                    c.colSpan = 5;
                    c.innerHTML = "Product 1";
                    c.style.color = "#ffffff";
                    c.style.fontWeight = "bold";
                    c.style.backgroundColor = "#008800";
                    c = r.insertCell(-1);
                    c.colSpan = 5;
                    c.style.backgroundColor = "#FF8800";
                    c.style.color = "#ffffff";
                    c.style.fontWeight = "bold";
                    c.innerHTML = "Product 2";
                }    
            }

            absUIReady = true;
            
            resetChart();
        }

        function dataBound(sender, args)
        {
            var c = sender.get_masterTableView();
            var items = $.grep(c.get_dataItems(), function(i) { return i.get_dataItem() != null && (i.get_dataItem().Selected_Row == 2 || i.get_dataItem().Selected_Row == 4 || i.get_dataItem().Comparative_Analysis_ID == 2 || i.get_dataItem().Comparative_Analysis_ID == 4); });
            var item;
            for (var i = 0; i < items.length; i++)
            {
                item = items[i];
                var text = $(item.get_element().cells[0]).text();
                $(item.get_element().cells[0]).html(text + "<img onclick='showPopup(this, event)' style='margin-left:3px' src='content/imagesabs/addFav.png' />");
            }
        }
        
        function resetChart()
        {
            if (absUIReady)
            {
                var url = "content/imagesabs/charts/";
                var geog = clientManager.get_UserGeography();
                geog = (geog && geog.RegionID ? geog.RegionID : "208");

                if (!$find("absmodules").get_selectedItem())
                {
                    url += geog + "_" + $find("ctl00_Tile3Title_rcbTopPayers").get_value() + "_" + $find("abschartmodules").get_selectedItem().value + ".jpg";
                }
                else
                {
                    url += geog + "_3.jpg";
                }
                
//                alert(url);
                $("#imgAbsChart").attr("src", url);
            }
        }

        function abs_pageUnloaded(sender, args)
        {            
            $disposeControl($find("absmodules"));
            $disposeControl($find("abschartmodules"));

            $(".dashboardTable div.RadComboBox_pathfinder .rcbInputCell .rcbInput").css({ "background": "url(app_themes/pathfinder/images/arwDwnGray.png) 3px center no-repeat", "color": "#2d58a8" });
            
            clientManager.remove_pageLoaded(abs_pageLoaded);
            clientManager.remove_pageUnloaded(abs_pageUnloaded);
        }

        function resetChartModules(compAnalysis)
        {
            var c = $find("abschartmodules");
            c.clear();

            if (!compAnalysis)
            {
                c.addItem("drugperf", "Drug Performance", "drugperf");
                c.addItem("rxtrend", "Rx Trend", "rxtrend");
                c.selectItem("drugperf");   
            }
            else
            {
                c.addItem("marketshare", "Market Share", "marketshare");
                c.addItem("accessbytier", "Access by Tier", "accessbytier");
                c.addItem("accessbycopay", "Access by Co-Pay", "accessbycopay");
                c.selectItem("marketshare");            
            }        
        }
        
        function onTopPayersChanged(sender, args)
        {
            var grid = $get("ctl00_Tile3_gridPrescribers").GridWrapper;
            $setGridFilter(grid.get_grid(), "ABS_Detail_Type_ID", args.get_item().get_value(), null, "System.Int32");

            if (grid.get_clientManager().get_UserGeography().RegionID != "CA")
            {
                var sortExp = new Telerik.Web.UI.GridSortExpression();
                sortExp.set_fieldName("Selected_Row");
                sortExp.set_sortOrder(1);

                grid.get_masterTableView().get_sortExpressions()._array = $.grep(grid.get_masterTableView().get_sortExpressions()._array, function(i) { return i != null && i.get_fieldName() != "Selected_Row"; }, false);

                if (args.get_item().get_value() == 2)
                {
                    grid.get_masterTableView().get_sortExpressions().add(sortExp);
                }
            }
            grid.dataBind();

            resetChart();
        }

        function onTopPrescribersChanged(sender, args)
        {
            var grid = $get("ctl00_Tile3_gridPrescribers").GridWrapper;
            $setGridFilter(grid.get_grid(), "ABS_Detail_Type_ID", args.get_item().get_value(), null, "System.Int32");
            grid.dataBind();

            resetChart();
        }

        var selected = false;
        function onModuleClicked(sender, args)
        {
            selected = !selected;
            if (!selected)
            {
                sender.selectItem("XXXXX");
                sender.highlightItem("XXXXX");
                $("#ctl00_Tile3_gridCompAnalysis").hide();
                $("#ctl00_Tile3_gridPrescribers").show();

                $("#ctl00_Tile3Title_rcbTopPayers input").css("color", "#ffffff");
            }
            else
            {
                $("#ctl00_Tile3_gridPrescribers").hide();
                $("#ctl00_Tile3_gridCompAnalysis").show();

                $("#ctl00_Tile3Title_rcbTopPayers input").css("color", "#888888");
            }

            $find("ctl00_Tile3Title_rcbTopPayers").set_enabled(!selected);
            
            
            resetChartModules(selected);
            
            resetChart();
        }

        function onChartModuleClicked(sender, args)
        {
            resetChart();
        }

        function showAssignTacticsMessages(review)
        {
            var p = "";

            $("input[type=checkbox]").each(function() { if (this.checked) p += parseInt(this.value) + "|"; });

            if (p != "") 
                $openWindow(clientManager.getUrl("tacticsandmessages.aspx?p=" + p + "&review=" + (review==true)));
        }

        function showPopup(sender, e)
        {
            if (!e) e = event;
            
            var rect = Sys.UI.DomElement.getBounds(sender);

            clientManager.openViewer("accessbasedselling/all/popup.aspx?beak=true", rect.x + 25, rect.y - 439, 700, 480, "divTile3");
        }

        function openTrainingMenu()
        {
            $openWindow("accessbasedselling/all/trainingandeducation.aspx", null, null, 730, 310);
        }

        function openStepEdit(id) {
            $openWindow("accessbasedselling/all/stepedit.aspx", null, null, 1100, 600);
        }
    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Tile3Title" Runat="Server">
    <div class="dashboardTable" style="float:left">
        <telerik:RadComboBox runat="server" ID="rcbTopPayers" EnableEmbeddedSkins="false" Skin="pathfinder" OnClientSelectedIndexChanged="onTopPayersChanged">
            <Items>
                <telerik:RadComboBoxItem Text="" Value="0" />
                <telerik:RadComboBoxItem Text="Top 5 Payers" Value="1" />
                <telerik:RadComboBoxItem Text="Top 10 Payers" Value="1" />
                <telerik:RadComboBoxItem Text="Top 25 Payers" Value="1" />
                <telerik:RadComboBoxItem Text="Top 50 Payers" Value="1" />
                <telerik:RadComboBoxItem Text="" Value="0" DisabledImageUrl="content/imagesabs/div.png" Enabled="false" Height="3px" />
                <telerik:RadComboBoxItem Text="Top 5 Prescribers" Value="2" />
                <telerik:RadComboBoxItem Text="Top 10 Prescribers" Value="2" />
                <telerik:RadComboBoxItem Text="Top 25 Prescribers" Value="2" />
                <telerik:RadComboBoxItem Text="Top 50 Prescribers" Value="2" />                
            </Items>
        </telerik:RadComboBox>
        <%--<telerik:RadComboBox runat="server" ID="rcbTopPrescribers" EnableEmbeddedSkins="false" SkinID="planInfoCombo" Skin="pathfinder" OnClientSelectedIndexChanged="onTopPrescribersChanged">
            <Items>
                <telerik:RadComboBoxItem Text="" />
                <telerik:RadComboBoxItem Text="Top 5 Prescribers" />
                <telerik:RadComboBoxItem Text="Top 10 Prescribers" />
                <telerik:RadComboBoxItem Text="Top 25 Prescribers" />
                <telerik:RadComboBoxItem Text="Top 50 Prescribers" />
            </Items>
        </telerik:RadComboBox>--%>    
    </div>            
    <div id="absmodules"  style="float:left"></div>
    <pinso:Menu runat="server" ID="absModulesMenu" Target="absmodules"  />    
    
</asp:Content>
<asp:Content ContentPlaceHolderID="Tile3Tools" ID="tile3Tools" runat="server">
    <div id="divAssignment">
        <a style="color:White;" href="javascript:showAssignTacticsMessages()">Assign Tactics/Message</a>
        &nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;
        <a style="color:White;" href="javascript:showAssignTacticsMessages(true)">Review Tactics/Message</a>
    </div>
    <%--<a style="color:White;" href="javascript:clientManager.refreshSection2()">Refresh</a>--%>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Tile3" Runat="Server">    
    <telerik:RadGrid runat="server" ID="gridPrescribers" SkinID="radTable" AutoGenerateColumns="false" PageSize="5" EnableEmbeddedSkins="false" Skin="pathfinder">
        <MasterTableView PageSize="5" AllowSorting="true" AllowMultiColumnSorting="true">
            <Columns>
                <telerik:GridBoundColumn HeaderStyle-Width="30px" DataField="Selected_Row" DataFormatString='&lt;input type="checkbox" value="{0}" /&gt;'  />
                <telerik:GridBoundColumn HeaderText="Account Name" DataField="Payor_Plan_Name" />
                <telerik:GridBoundColumn HeaderText="Prescriber Name" DataField="Prescriber_Name" />
                <telerik:GridHyperLinkColumn ItemStyle-CssClass="helpLink" DataNavigateUrlFormatString='javascript:openStepEdit({0});' DataNavigateUrlFields="Selected_Row" UniqueName="Formulary_Status" DataTextFormatString='{0}' DataTextField ="Formulary_Status" HeaderText="Formulary Status"></telerik:GridHyperLinkColumn>
                <%-- <telerik:GridBoundColumn HeaderText="Formulary Status" DataField="Formulary_Status" />                --%>
                <telerik:GridBoundColumn HeaderText="Rank" DataField="Rank" />
                <telerik:GridBoundColumn HeaderText="Product 1 MKT TRX" DataField="SRI_MKT_TRX" DataFormatString="{0:n0}" ItemStyle-CssClass="alignRight" />
                <telerik:GridBoundColumn HeaderText="Product 1 TRX" DataField="LEXAPRO_TRX" DataFormatString="{0:n0}" ItemStyle-CssClass="alignRight" />
                <telerik:GridBoundColumn HeaderText="Product 1 Share" DataField="LX_Share" DataFormatString="{0:p}" ItemStyle-CssClass="alignRight" />
                <telerik:GridBoundColumn HeaderText="Product 1 Vs National" DataField="LEXAPRO_Vs_National" DataFormatString="{0:p}" ItemStyle-CssClass="alignRight" />
                <telerik:GridBoundColumn HeaderText="Product 1 NPA Variance" DataField="LX_NPA_Variance" DataFormatString="{0:p}" ItemStyle-CssClass="alignRight" />
                <telerik:GridBoundColumn HeaderText="MKT TRX" DataField="SNRI_MKT_TRX" DataFormatString="{0:n0}" ItemStyle-CssClass="alignRight" />
                <telerik:GridBoundColumn HeaderText="Share" DataField="SNRI_Share" DataFormatString="{0:p}" ItemStyle-CssClass="alignRight" />
                <telerik:GridBoundColumn HeaderText="Payer Share" DataField="SNRI_Vs_Payer_Share" DataFormatString="{0:p}" ItemStyle-CssClass="alignRight" />
                <telerik:GridBoundColumn HeaderText="NPA Variance" DataField="SNRI_NPA_Variance" DataFormatString="{0:p}" ItemStyle-CssClass="alignRight" />
            </Columns>
            <SortExpressions>
            <telerik:GridSortExpression FieldName="Rank" SortOrder="Ascending" />
                <telerik:GridSortExpression FieldName="Payor_Plan_Name" SortOrder="Ascending" />
                <telerik:GridSortExpression FieldName="Prescriber_Name" SortOrder="Ascending" />
                <telerik:GridSortExpression FieldName="Formulary_Status" SortOrder="Ascending" />
            </SortExpressions>
        </MasterTableView>
        <ClientSettings>
            <Selecting AllowRowSelect="true" />
            <Scrolling AllowScroll="true" UseStaticHeaders="false" />
            <DataBinding Location="~/accessbasedselling/services/pathfinderdataservice.svc" DataService-TableName="ABSDetailViewSet" />
        </ClientSettings>     
    </telerik:RadGrid>
    <pinso:RadGridWrapper runat="server" ID="gridWrapper" Target="gridPrescribers" />

    <telerik:RadGrid runat="server" ID="gridCompAnalysis" SkinID="radTable" AutoGenerateColumns="false" PageSize="5" EnableEmbeddedSkins="false" Skin="pathfinder">
        <MasterTableView PageSize="5" AllowSorting="true" AllowMultiColumnSorting="true">
            <Columns>            
                <telerik:GridBoundColumn HeaderStyle-Width="30px" DataField="Comparative_Analysis_ID" DataFormatString='&lt;input type="checkbox" value="{0}" /&gt;' />
                <telerik:GridBoundColumn HeaderText="Account Name" DataField="Plan_Name" HeaderStyle-Width="140px" />
                <telerik:GridBoundColumn HeaderText="Pharmacy<br />Lives" DataField="Pharmacy_Lives" DataFormatString="{0:n0}" ItemStyle-CssClass="alignRight" HeaderStyle-Width="75px" />
                <telerik:GridBoundColumn HeaderText="Formulary<br />Status" DataField="Lexapro_Formulary_Status" HeaderStyle-BackColor="#008800" HeaderStyle-ForeColor="#ffffff" HeaderStyle-Width="80px" />                
                <telerik:GridHyperLinkColumn HeaderText="Restrictions" DataTextField="Lexapro_Restrictions" DataTextFormatString="{0}" DataNavigateUrlFields="Lexapro_Restrictions" DataNavigateUrlFormatString='javascript:$openWindow("accessbasedselling/all/pa.aspx")' HeaderStyle-BackColor="#008800" HeaderStyle-ForeColor="#ffffff" HeaderStyle-Width="50px" />              
                <telerik:GridBoundColumn HeaderText="Co-pay" DataField="Lexapro_COPAY" HeaderStyle-BackColor="#008800" HeaderStyle-ForeColor="#ffffff"  HeaderStyle-Width="50px" />              
                <telerik:GridBoundColumn HeaderText="Tier" DataField="Lexapro_TIER" DataFormatString="{0:n0}" ItemStyle-CssClass="alignRight" HeaderStyle-BackColor="#008800" HeaderStyle-ForeColor="#ffffff" HeaderStyle-Width="50px" />
                <telerik:GridBoundColumn HeaderText="<div style='text-align:center;width:45px'>MST</div>" DataField="Lexapro_Mst" DataFormatString="{0:p}" ItemStyle-CssClass="alignRight" HeaderStyle-BackColor="#008800" HeaderStyle-ForeColor="#ffffff" HeaderStyle-Width="50px" />
                <telerik:GridBoundColumn HeaderText="Formulary<br />Status" DataField="Cymbalta_Formulary_Status" HeaderStyle-BackColor="#FF8800" HeaderStyle-ForeColor="#ffffff" HeaderStyle-Width="80px" />                
                <telerik:GridHyperLinkColumn HeaderText="Restrictions" HeaderStyle-BackColor="#FF8800" HeaderStyle-ForeColor="#ffffff" DataTextField="Cymbalta_Restrictions" DataTextFormatString="{0}" DataNavigateUrlFields="Lexapro_Restrictions" DataNavigateUrlFormatString='javascript:$openWindow("accessbasedselling/all/pa.aspx")' HeaderStyle-Width="50px" />              
                <telerik:GridBoundColumn HeaderText="Co-pay" DataField="Cymbalta_COPAY" HeaderStyle-BackColor="#FF8800" HeaderStyle-ForeColor="#ffffff" HeaderStyle-Width="50px" />              
                <telerik:GridBoundColumn HeaderText="Tier" DataField="Cymbalta_TIER" DataFormatString="{0:n0}" ItemStyle-CssClass="alignRight" HeaderStyle-BackColor="#FF8800" HeaderStyle-ForeColor="#ffffff" HeaderStyle-Width="50px" />
                <telerik:GridBoundColumn HeaderText="<div style='text-align:center;width:45px'>MST</div>" DataField="Cymbalta_Mst" DataFormatString="{0:p}" ItemStyle-CssClass="alignRight" HeaderStyle-BackColor="#FF8800" HeaderStyle-ForeColor="#ffffff" HeaderStyle-Width="50px" />
            </Columns>
            <SortExpressions>
                <telerik:GridSortExpression FieldName="Pharmacy_Lives" SortOrder="Descending" />
            </SortExpressions>
        </MasterTableView>
        <ClientSettings>
            <Selecting AllowRowSelect="true" />
            <Scrolling AllowScroll="true" UseStaticHeaders="false" />
            <DataBinding Location="~/accessbasedselling/services/pathfinderdataservice.svc" DataService-TableName="ABSComparativeAnalysysSet" />
        </ClientSettings>     
    </telerik:RadGrid>
    <pinso:RadGridWrapper runat="server" ID="RadGridWrapper1" Target="gridCompAnalysis" AutoLoad="true" RequiresFilter="false" />    
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="Tile4Title" Runat="Server">        
    <div id="abschartmodules"></div>
    <pinso:Menu runat="server" ID="absChartModules" Target="abschartmodules" />
</asp:Content>

<asp:Content ID="Content8" ContentPlaceHolderID="Tile4" Runat="Server">    
    <div style="text-align:center;">
        <img id="imgAbsChart" style="height:224px;" src="content/imagesabs/charts/screen2.jpg" />
    </div>
</asp:Content>
