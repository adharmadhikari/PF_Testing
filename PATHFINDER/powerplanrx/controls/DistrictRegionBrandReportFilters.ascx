<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DistrictRegionBrandReportFilters.ascx.cs" Inherits="powerplanrx_controls_DistrictRegionBrandReportFilters" %>

<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            function getValues() {
                var type = $find('<%=ddlReportType.ClientID %>');
                var brand = $find('<%=ddlBrands.ClientID %>');
                var segment = $find('<%=ddlSegment.ClientID %>');
                var region = $find('<%=ddlRegion.ClientID %>');
                var district = $find('<%=ddlDistrict.ClientID %>');
                var valid = validateControls(region, brand, segment);

                //only do this if results is true
                if (valid) {
                    if (district.get_value() == "") district.set_value(0);
                    var url = "ReportViewer.aspx?Client_Key=<%=Pinsonault.Web.Session.ClientKey %>&reportname=PPRX_Reports&report=DistrictRegionBrandReport&Type_ID=" + type.get_value() +
                        "&Brand_ID=" + brand.get_value() + "&Segment_ID=" + segment.get_value() + "&Region_ID=" + region.get_value() + "&District_ID=" + district.get_value();

                    //insert iframe
                    $("#RAD_SPLITTER_PANE_CONTENT_ctl00_ctl00_main_main_radPaneContent").html("");
                    var div = "#RAD_SPLITTER_PANE_CONTENT_ctl00_ctl00_main_main_radPaneContent"
                    var height = $(".contentArea").height();
                    insertIframe(div, url, height);

                    //formatting
                    $(".reportsPage").width("100%");
                    $(".contentArea").css("overflow-x", "hidden").css("overflow-y", "hidden");
                    $("#ctl00_ctl00_main_main_radSplit").width("100%");
                    $("#RAD_SPLITTER_ctl00_ctl00_main_main_radSplit").width("100%");
                    $("#ctl00_ctl00_main_main_radPaneContent").width("100%");
                    $("#RAD_SPLITTER_PANE_CONTENT_ctl00_ctl00_main_main_radPaneContent").width($("#ctl00_ctl00_main_main_radPaneContent").width());
                }
            }


            function myUserControlClickHandler() {
                $find("<%= RadAjaxManager.GetCurrent(Page).ClientID %>").ajaxRequest("content");
            }


            function validateControls(region, brand, segment) {
                var results = true;
                if (region.get_text() == "-- Select a Region --") {
                    $("#regionValidate").show();
                    results = false;
                } else {
                    $("#regionValidate").hide();
                }
                if (brand.get_text() == "-- Select a Brand --") {
                    $("#brandsValidate").show();
                    results = false;
                } else {
                    $("#brandsValidate").hide();
                    }
                if (segment.get_text() == "-- Select a Segment --") {
                    $("#segmentValidate").show();
                    results = false;
                } else {
                    $("#segmentValidate").hide();
                }
                return results;
            }
        </script>
    </telerik:RadCodeBlock>
<div>
<table border="0" cellpadding="0" cellspacing="0" width="100%"  >
        <col width="10%" />
        <col width="85%" />
        
            <tr>
                <td>
                    <asp:Label ID="lblReportType" runat="server" Text="Report Type"></asp:Label>&nbsp;<span
                        style="font-weight: bold; color: Red;">*</span></td><td >
                    <telerik:RadComboBox EnableEmbeddedSkins="false" SkinID="impactGen" ID="ddlReportType" Width="180"
                        runat="server" CausesValidation="False">
                        <Items>
                         
                            <telerik:RadComboBoxItem Value="1" Text="Single Brand TRx" />
                            <telerik:RadComboBoxItem Value="2" Text="Market Basket Group TRx" />
                        </Items>
                    </telerik:RadComboBox>
                </td>
            </tr>

            <tr>
                <td class="auto-style1">
                    <asp:Label ID="lblBrands" runat="server" Text="Brand"></asp:Label><span style="font-weight: bold; color: Red;">&nbsp;*</span></td><td class="auto-style1">
                    <telerik:RadComboBox EnableEmbeddedSkins="false" SkinID="impactGen" ID="ddlBrands"
                        runat="server" DataSourceID="dsBrand" DataTextField="PP_Brand_Name" DataValueField="PP_Brand_ID" Width="180"
                        AppendDataBoundItems="true">
                        <Items>
                            <telerik:RadComboBoxItem Value="0" Text="-- Select a Brand --" />
                        </Items>
                    </telerik:RadComboBox>
                </td>
            </tr>

            <tr>            
                <td>
                    <asp:Label ID="lblSegment" runat="server" Text="Segment"></asp:Label><span style="font-weight: bold; color: Red;">&nbsp;*</span></td><td>
                    <telerik:RadComboBox EnableEmbeddedSkins="false" SkinID="impactGen" ID="ddlSegment"
                        runat="server" DataSourceID="dsSegment" DataTextField="Segment_Name" DataValueField="Segment_ID" Width="180"
                        AppendDataBoundItems="true">
                        <Items>
                            <telerik:RadComboBoxItem Value="0" Text="-- Select a Segment --" />
                        </Items>
                    </telerik:RadComboBox>
                </td>
            </tr>

            <tr>            
                <td>
                    <asp:Label ID="lblRegion" runat="server" Text="Region"></asp:Label><span style="font-weight: bold; color: Red;">&nbsp;*</span></td><td>
                    <telerik:RadComboBox ID="ddlRegion" runat="server" Width="180" AutoPostBack="true" EnableEmbeddedSkins="false" EnableLoadOnDemand="true"
                        OnSelectedIndexChanged="ddlRegion_SelectedIndexChanged"
                        SkinID="impactGen" />
                </td>
            </tr>
            <tr>            
                <td>
                    <asp:Label ID="lblDistrict" runat="server" Text="District"></asp:Label></td><td>
                    <telerik:RadComboBox ID="ddlDistrict" runat="server" Width="180"
                        EnableEmbeddedSkins="false"
                        SkinID="impactGen" >
                        
                        </telerik:RadComboBox>

                </td>
            </tr>
            
        </table>
    <pinso:CustomButton ID="btnSubmit" runat="server" Text="Submit" OnClientClick="getValues(); return false;"  />
    <div id="regionbrandsegmentValidation">
        <div id="brandsValidate" style="font-weight: bold; color: Red; display:none;">* Please Select a Brand</div>
        <div id="segmentValidate" style="font-weight: bold; color: Red; display:none;">* Please Select a Segment</div>
        <div id="regionValidate" style="font-weight: bold; color: Red; display:none;">* Please Select a Region</div>
    </div>
        <telerik:RadAjaxManagerProxy ID="RadAjaxManagerProxy2" runat="server">
         <AjaxSettings>
             <telerik:AjaxSetting AjaxControlID="ddlRegion" >
                 <UpdatedControls>
                     <telerik:AjaxUpdatedControl ControlID="ddlDistrict" />
                 </UpdatedControls>
             </telerik:AjaxSetting>
         </AjaxSettings> 
        </telerik:RadAjaxManagerProxy>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />

        <asp:SqlDataSource ID="dsBrand" runat="server" ConnectionString="<%$ ConnectionStrings:PathfinderClientDB_Format %>"
            SelectCommand="pprx.usp_GetBrandName" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
       
</div>
        <asp:SqlDataSource ID="dsSegment" runat="server" ConnectionString="<%$ ConnectionStrings:PathfinderClientDB_Format %>"
            SelectCommand="pprx.usp_GetSectionSegments" SelectCommandType="StoredProcedure">
             <SelectParameters>
                    <asp:Parameter Name="IncludeAll" DbType="Boolean" DefaultValue="false" /> 
             </SelectParameters>     
        </asp:SqlDataSource>
       
        