<%@ Page Title="" Language="C#" MasterPageFile="~/custom/MasterPages/SellSheetStep.master" AutoEventWireup="true" CodeFile="geographyselection.aspx.cs" Inherits="custom_pinso_sellsheets_geographyselection" %>
<%@ MasterType VirtualPath="~/custom/MasterPages/SellSheetStep.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="scriptContainer" Runat="Server">
    <script type="text/javascript">
        clientManager.add_pageInitialized(geographySelection_pageInitialized);
        clientManager.add_pageLoaded(geographySelection_pageLoaded);
        clientManager.add_pageUnloaded(geographySelection_pageUnloaded);

        function geographySelection_pageInitialized(sender, args)
        {
            if ($get("divSellSheetMap"))
            {
                var mapIsReady = loadFlashMap(clientManager.get_CurrentMapUIStateAsText(), "divSellSheetMap");
                //alert(mapIsReady);
                if (mapIsReady)
                {
                    geographySelection_reloadMap();

                }
                else //wait to make sure map object is ready
                {
                    var time = 500;

                    //Chrome fix for map not displaying correctly
                    if (chrome)
                        time = 2000;

                    new cmd(null, geographySelection_reloadMap, null, time);                    
                }
            }
            _geoNameCallBackDelegate = Function.createDelegate(this, _geoNameCallBack);
        }
        
        function geographySelection_pageLoaded(sender, args)
        {
            if (!$.browser.msie)
                ensureMapIsLoaded(clientManager.get_CurrentMapUIStateAsText(), "divSellSheetMap");

            //Set this to 1 so blank textbox doesn't trigger '..already exists..' alert
            $("#<%= txtGeoValid.ClientID %>").val("1");
        }

        function geographySelection_pageUnloaded(sender, args)
        {
            resetFlashMap("#divSellSheetMap");
            try
                {
                    //only ie still has map available
                    if ($.browser.msie)
                    {
                        //change map mode back to zoom mode
                        fmMapModeZoom();

                        //clear selected areas
                        var selectedStates = fmMapModeExportListAreas();

                        if (selectedStates.indexOf(',') > 0)
                        {
                            var splitStates = selectedStates.split(",");

                            for (var state in splitStates)
                                fmMapModeRemoveArea(1, splitStates[state], "");
                        }
                        else
                            fmMapModeRemoveArea(1, selectedStates, "");
                    }
            }
            catch (ex)
            { 
            }
            
            delete (_geoNameCallBackDelegate);

            sender.remove_pageInitialized(geographySelection_pageInitialized);
            sender.remove_pageLoaded(geographySelection_pageLoaded);
            sender.remove_pageUnloaded(geographySelection_pageUnloaded);
        }

        function geographySelection_loadStates(time)
        {
            //Load page if editing
            var states = $("#<%= txtStates.ClientID %>").val();

            //Delay filling of selected states to make sure map is loaded correctly
            if (states.length > 0)
                new cmd(null, geographySelection_fillStates, null, time);
        }

        function geographySelection_fillStates()
        {
            var statesSplit = $("#<%= txtStates.ClientID %>").val().split(",");
            var state;

            for (var x in statesSplit)
            {
                state = $.trim(statesSplit[x]);
                if (state)
                    fmMapModeAddArea(1, "us_" + state.toLowerCase(), "");
            }
        }
        
        function geographySelection_reloadMap()
        {
            //IE Browser hack to get map to load properly on empty cache
            $(".tblMap object").css("border-style", "none");
            //------------------------------------------------------
            if (clientManager.get_MapIsReady())
            {
                //change map mode to selection mode
                fmMapModeSelect();

                //make sure the map is zoomed out
                fmInitialView();

                fmThemeReloadAreas("areas/mapdata.ashx?s=" + geographySelection_mapState());
                
                //Chrome fix for map not displaying correctly
                if (chrome)
                    new cmd(null, geographySelection_fillStates, null, 1500);
                else
                    geographySelection_fillStates();

                //alert("it is ready");
            }
            else
            {
                var time = 500;

                //Chrome fix for map not displaying correctly
                if (chrome)
                    time = 1000;

                new cmd(null, geographySelection_reloadMap, null, time);
                //alert("it aint ready");
            }
        }

        function geographySelection_mapState()
        {
            var data = clientManager.get_SelectionData();

            var mapData = {};
            mapData["UserKey"] = clientManager.get_UserKey();
            mapData["Application"] = 8;
            mapData["Channel"] = [1];
            mapData["Drug"] = 0;

            return Sys.Serialization.JavaScriptSerializer.serialize(mapData);
        }

        function ssMap_click(event, areaID, label)
        {
        
            var selectedStates = fmMapModeExportListAreas();
            var splitStates = selectedStates.split(",");

            if (splitStates.length > 3)
            {
                $alert("Please select only up to 3 states");

                fmMapModeRemoveArea(1, areaID, "")
            }

            $("#<%= txtStates.ClientID %>").val(fmMapModeExportListAreas());
        }

        function txtGeoName_changed(geoName)
        {
            if (geoName.length > 0)
            {
                var ssid = $("#<%= txtSSID.ClientID %>").val();
                
                $.post(clientManager.get_ApplicationManager().get_ServiceUrl() + "/CheckGeoNameDuplicates", { geographyName: geoName, sellSheetID: ssid }, _geoNameCallBackDelegate, "json");
            }
        }

        function _geoNameCallBack(r)
        {
            if (r && r.d)
            {
                if (r.d.CheckGeoNameDuplicates)
                {
                    $alert("This geography name already exists, please choose a new name");
                    $("#<%= txtGeoValid.ClientID %>").val("0");
                }
                else
                    $("#<%= txtGeoValid.ClientID %>").val("1");
            }
        }
   </script>
        

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="StepBody" Runat="Server">  
<asp:HiddenField ID="txtSSID" runat="server" />  
<asp:HiddenField ID="txtStates" runat="server" />
<pinso:ClientValidator ID="vldStates" runat="server" Target="txtStates" Required="true" Text="Please select at least one state" />
<asp:HiddenField ID="txtGeoValid" runat="server" />
<asp:HiddenField ID="txtGeoValidCompare" runat="server" Value="1" />
<asp:Label ID="msglbl" runat="server" Visible="false" Text="Saving changes will reset the plan selection and the sell sheet will be moved to the drafted sell sheets section." style="color:Red;"></asp:Label>
<br />
<%--<pinso:ClientValidator ID="vldGeoValid" runat="server" CompareOperator="Equal" DataType="Integer" CompareTo="txtGeoValidCompare" Target="txtGeoValid" Required="true" Text="The geography name already exists, please choose a new name" />--%>
<table width="100%" class="tblMap">
    <tr>
        <td align="center">
            <div id="divSellSheetMap"></div>
        </td>
    </tr>
    <tr>
        <td align="center" valign="top" class="geoRow">            
                <span id="ssBold">Please provide geography name below</span><span class="requiredRed">*</span><br />
                <asp:TextBox runat="server" ID="txtGeoName" CssClass="geoName" MaxLength="75" />        
                <pinso:ClientValidator ID="vldGeoName" runat="server" Target="txtGeoName"  Required="true" Text="Please enter a geography name" />                 
        </td>
    </tr>
</table>




<input type="button" class="btnPrev"
        onclick="clientManager.get_ApplicationManager().back(clientManager)"  
        value="Back" />
    <asp:Button ID="btnNext" runat="server" Text="Next" CssClass="postback validate btnNext" 
        onclick="btnNext_Click" />
</asp:Content>

