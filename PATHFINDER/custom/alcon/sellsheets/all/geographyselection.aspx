<%@ Page Title="" Language="C#" MasterPageFile="~/custom/Alcon/sellsheets/Alcon_SellSheetStep.master" AutoEventWireup="true" CodeFile="geographyselection.aspx.cs" Inherits="custom_alcon_sellsheets_geographyselection" %>
<%@ MasterType VirtualPath="~/custom/Alcon/sellsheets/Alcon_SellSheetStep.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="scriptContainer" Runat="Server">


<script type="text/javascript">
        clientManager.add_pageLoaded(geographySelection_pageLoaded);
        clientManager.add_pageUnloaded(geographySelection_pageUnloaded);
        
        function geographySelection_pageLoaded(sender, args)
        {
            //Set this to 1 so blank textbox doesn't trigger '..already exists..' alert
            $("#<%= txtGeoValid.ClientID %>").val("1");

            //Initialize formatting for Trx/Mst selector
            $('.states input:radio').each(function()
            {
                var id = $(this).attr('id');
                if ($(this).attr('checked')) $('.states label[for=' + id + ']').addClass('selectedStateOption');
                $(this).click(function()
                {
                    radioCheckStatus(this);
                })
            });

            var templateid = $("#<%= txtTemplate.ClientID %>").val();
            var items = $('#templateSidebar li');
            
            $('#templateSidebar').removeAttr('style');
            $('#templateSidebar').css("left", "18px");

            
            items.css({ display: "none" });
            items.filter('[rel=' + templateid + ']').css({ display: "inline"});
           
            //$('#templateSidebar').css("margin-left", "18px");

         
        }

        function radioCheckStatus(ctrl)
        {
            var id = $(ctrl).attr('id');
            $('.states label').not('.states label[for=' + id + ']').removeClass('selectedStateOption');

            $('.states label[for=' + id + ']').addClass('selectedStateOption');
        }

        function geographySelection_pageUnloaded(sender, args)
        {
            sender.remove_pageLoaded(geographySelection_pageLoaded);
            sender.remove_pageUnloaded(geographySelection_pageUnloaded);
        }

        function geographySelection_loadStates(time)
        {
            //Load page if editing
            var states = $("#<%= txtStates.ClientID %>").val();
        }

        function geographySelection_fillStates()
        {
            var statesSplit = $("#<%= txtStates.ClientID %>").val().split(",");
            var state;

//            for (var x in statesSplit)
//            {
//                state = $.trim(statesSplit[x]);
//                if (state)
//                    fmMapModeAddArea(1, "us_" + state.toLowerCase(), "");
//            }
        }

        function geographySelection_mapState()
        {
            var data = clientManager.get_SelectionData();

            var mapData = {};
            mapData["UserKey"] = clientManager.get_UserKey();
            mapData["Application"] = 8;
            mapData["Channel"] = 1;
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
        function MyRadioClickHandler_CV(value, name) {
            $("#<%= txtStates.ClientID %>").val(value);
            $("#<%= txtGeoName.ClientID %>").val(name);
        }
   </script>
        

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="StepBody" Runat="Server">  
<asp:HiddenField ID="txtSSID" runat="server" />  
<asp:HiddenField ID="txtStates" runat="server" />
<asp:HiddenField ID="txtTemplate" runat="server" />
<pinso:ClientValidator ID="vldStates" runat="server" Target="txtStates" Required="true" Text="Please select at least one state" />
<asp:HiddenField ID="txtGeoValid" runat="server" />
<asp:HiddenField ID="txtGeoValidCompare" runat="server" Value="1" />
<asp:Label ID="msglbl" runat="server" Visible="false" Text="Saving changes will reset the plan selection and the sell sheet will be moved to the drafted sell sheets section." style="color:Red;"></asp:Label>
<br />
<%--<pinso:ClientValidator ID="vldGeoValid" runat="server" CompareOperator="Equal" DataType="Integer" CompareTo="txtGeoValidCompare" Target="txtGeoValid" Required="true" Text="The geography name already exists, please choose a new name" />--%>
<table width="100%" cellspacing="10">
    <tr>
        <td align="center">
                <div class="states">
                    <asp:RadioButtonList ID="rbl_states" runat="server" DataTextField="Name" 
                        DataValueField="ID" RepeatColumns="6" CellPadding="5" RepeatDirection="Horizontal" 
                        CellSpacing="10" onprerender="rbl_states_PreRender"> 
                    </asp:RadioButtonList>
                </div>
                <%--<asp:EntityDataSource ID="ds_states" runat="server"  
                    EntitySetName="V_States" ConnectionString="name=PathfinderAlconEntities"
                    DefaultContainerName="PathfinderAlconEntities" OrderBy="it.Geography_Name" >
                </asp:EntityDataSource>--%>
        </td>
    </tr>
    <tr>
        <td align="center" valign="top" class="geoRow">            
                <span id="ssBold">Please Select a state from above</span><span class="requiredRed">*</span><br />
                <asp:TextBox runat="server" ID="txtGeoName" style="display:none" CssClass="geoName" MaxLength="75" ReadOnly="true" />        
<%--                <pinso:ClientValidator ID="vldGeoName" runat="server" Target="txtGeoName"  Required="true" Text="Please enter a geography name" />                 
--%>        </td>
    </tr>
</table>




<input type="button" class="btnPrev"
        onclick="clientManager.get_ApplicationManager().back(clientManager)"  
        value="Back" />
    <asp:Button ID="btnNext" runat="server" Text="Next" CssClass="postback validate btnNext" 
        onclick="btnNext_Click" />
</asp:Content>

