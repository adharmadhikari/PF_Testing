<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PowerPlanRxReportFilters.ascx.cs" Inherits="powerplanrx_controls_PowerPlanRxReportFilters" %>
<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            function getPPRXValues() {
                var report = $find('<%=ddlReportType.ClientID %>');
                var plan = $find('<%=ddlNationalAccount.ClientID %>');
                var manager = $find('<%=ddlAccountManager.ClientID %>');
                var brand = $find('<%=ddlBrands.ClientID %>');
                var region = $find('<%=ddlRegion.ClientID %>');
                var district = $find('<%=ddlDistrict.ClientID %>');
                var territory = $find('<%=ddlTerritory.ClientID %>');

                //if region/district/territory are not selected - make them equal 0
                if (region.get_value() == "") region.set_value(0);
                if (district.get_value() == "") district.set_value(0);
                if (territory.get_value() == "") territory.set_value(0);
                
                //construct report url
                var url = "ReportViewer.aspx?Client_Key=<%=Pinsonault.Web.Session.ClientKey %>&reportname=PPRX_Reports&report=" + report.get_value() + "&Plan_ID=" + plan.get_value() +
                    "&Brand_ID=" + brand.get_value() + "&Region_ID=" + region.get_value() + "&District_ID=" + district.get_value() +
                    "&Territory_ID=" + territory.get_value() + "&AccountManager=" + manager.get_value();
                
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

            function myUserControlClickHandler() {
                $find("<%= RadAjaxManager.GetCurrent(Page).ClientID %>").ajaxRequest("content");
            }

        </script>
    </telerik:RadCodeBlock>
<telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
    <script type="text/javascript">
        </script>
     </telerik:RadCodeBlock>
<div>
<table border="0" cellpadding="0" cellspacing="0" width="100%"  >
        <col width="10%" />
        <col width="85%" />
        
            <tr>
                <td>
                    <asp:Label ID="lblReportType" runat="server" Text="Report Type"></asp:Label>
                        </td><td >
                    <telerik:RadComboBox EnableEmbeddedSkins="false" SkinID="impactGen" ID="ddlReportType"
                        runat="server" CausesValidation="False">
                        <Items>
                         
                            <telerik:RadComboBoxItem Value="SummaryReport" Text="Summary" />
                            <telerik:RadComboBoxItem Value="DetailsReport" Text="Details" />
                        </Items>
                    </telerik:RadComboBox>
                </td>
            </tr>
            <tr>            
                <td>
                    <asp:Label ID="lblNationalAccount" runat="server" Text="National Account"></asp:Label></td>
                <td>
                    <telerik:RadComboBox EnableEmbeddedSkins="false" SkinID="impactGen" ID="ddlNationalAccount"
                        runat="server" DataSourceID="dsPlans" DataTextField="Plan_Name" DataValueField="Plan_ID" AppendDataBoundItems="true">
                        <Items>
                            <telerik:RadComboBoxItem Value="0" Text="-- Any National Account--" />
                        </Items>
                    </telerik:RadComboBox>
                </td>
            </tr>
            <tr>            
                <td>
                    <asp:Label ID="lblRegion" runat="server" Text="Region"></asp:Label></td><td>
                    <telerik:RadComboBox ID="ddlRegion" runat="server" Width="180" AutoPostBack="true" EnableLoadOnDemand="true" EnableEmbeddedSkins="false"
                        OnSelectedIndexChanged="ddlRegion_SelectedIndexChanged"
                        SkinID="impactGen" />
                </td>
            </tr>
            <tr>            
                <td>
                    <asp:Label ID="lblDistrict" runat="server" Text="District"></asp:Label></td><td>
                    <telerik:RadComboBox ID="ddlDistrict" runat="server" Width="180" AutoPostBack="true" EnableLoadOnDemand="true"
                        OnSelectedIndexChanged="ddlDistrict_SelectedIndexChanged" EnableEmbeddedSkins="false"
                        SkinID="impactGen" >
                        
                        </telerik:RadComboBox>

                </td>
            </tr>
            <tr>            
                <td>
                    <asp:Label ID="lblTerritory" runat="server" Text="Territory"></asp:Label></td><td>
                    <telerik:RadComboBox ID="ddlTerritory" runat="server" Width="180" EnableEmbeddedSkins="false" SkinID="impactGen">
                        
                        </telerik:RadComboBox>
  
                </td>
            </tr>
            <tr>            
                <td>
                    <asp:Label ID="lblBrands" runat="server" Text="Brand"></asp:Label></td><td>
                    <telerik:RadComboBox EnableEmbeddedSkins="false" SkinID="impactGen" ID="ddlBrands"
                        runat="server" DataSourceID="dsBrand" DataTextField="PP_Brand_Name" DataValueField="PP_Brand_ID"
                        AppendDataBoundItems="true">
                        <Items>
                            <telerik:RadComboBoxItem Value="0" Text="-- Any Brand --" />
                        </Items>
                    </telerik:RadComboBox>
                </td>
            </tr>
            <tr>            
                <td>
                    <asp:Label ID="lblAccountManager" runat="server" Text="Account Manager"></asp:Label></td><td>
                    <telerik:RadComboBox EnableEmbeddedSkins="false" SkinID="impactGen" ID="ddlAccountManager"
                        runat="server" DataSourceID="dsAccountManagers" DataTextField="Created_By" DataValueField="Created_By" DropDownWidth="200px"
                        AppendDataBoundItems="true" CausesValidation="False">
                        <Items>
                            <telerik:RadComboBoxItem Value="0" Text="-- Any Regional Account Manager --" />
                        </Items>
                    </telerik:RadComboBox>
                </td>
            </tr>
        </table>
    <pinso:CustomButton ID="btnSubmit" runat="server" Text="Submit" OnClientClick="getPPRXValues(); return false;"  />


        <telerik:RadAjaxManagerProxy ID="RadAjaxManagerProxy1" runat="server">
         <AjaxSettings>
             <telerik:AjaxSetting AjaxControlID="ddlRegion" >
                 <UpdatedControls>
                     <telerik:AjaxUpdatedControl ControlID="ddlDistrict" />
                 </UpdatedControls>
             </telerik:AjaxSetting>
             <telerik:AjaxSetting AjaxControlID="ddlDistrict">
                 <UpdatedControls>
                     <telerik:AjaxUpdatedControl ControlID="ddlTerritory" />
                 </UpdatedControls>
             </telerik:AjaxSetting>
         </AjaxSettings> 
        </telerik:RadAjaxManagerProxy>
        

       <asp:SqlDataSource ID="dsAccountManagers" runat="server" ConnectionString="<%$ ConnectionStrings:PathfinderClientDB_Format %>"
            SelectCommand="pprx.usp_GetAccountManagers" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
        <asp:SqlDataSource ID="dsBrand" runat="server" ConnectionString="<%$ ConnectionStrings:PathfinderClientDB_Format %>"
            SelectCommand="pprx.usp_GetBrandName" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
       <asp:SqlDataSource ID="dsPlans" runat="server" ConnectionString="<%$ ConnectionStrings:PathfinderClientDB_Format %>"
            SelectCommand="pprx.usp_GetPlans" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
</div>