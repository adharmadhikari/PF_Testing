<%@ Page Title="PowerPlanRx Report" Language="C#" 
    Theme="impact" AutoEventWireup="true" EnableViewState="true"
    CodeFile="PowerPlanRxReportIFrame.aspx.cs" Inherits="PowerPlanRxReport" %>
    
    
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link id="Link1" runat="server" href="~/powerplanrx/content/styles/main.css" rel="stylesheet" type="text/css" />
    <link id="Link2" runat="server" href="~/powerplanrx/content/styles/ui.all.css" rel="stylesheet" type="text/css" />    
 
</head>
<body>
    <form id="form1" runat="server"> 
    <asp:ScriptManager runat="server" ID="scriptManager">
        <Scripts>
            <asp:ScriptReference Path="https://ajax.microsoft.com/ajax/jquery/jquery-1.3.2.min.js" />
            <asp:ScriptReference Path="~/powerplanrx/content/scripts/jquery-ui-1.7.2.custom.min.js" />
            <asp:ScriptReference Path="~/powerplanrx/content/scripts/ui.js" />
            <asp:ScriptReference Path="~/powerplanrx/content/scripts/css_browser_selector.js" />
            <asp:ScriptReference Path="~/powerplanrx/content/scripts/print.js" />

        </Scripts>
    </asp:ScriptManager>

    <div class="tileContainerHeader">
        <asp:Label ID="lblHeader" runat="server" Text="PowerPlanRx Report"></asp:Label>
    </div>
    <div class="powerplanrxReportsPage">
        <table border="0" cellpadding="0" cellspacing="0" width="100%"  >
        <col width="10%" />
        <col width="25%" />
        <col width="5%" />
        <col width="25%" />
        <col width="5%" />
        <col width="25%" />

            <tr>
                <td>
                    <asp:Label ID="lblReportType" runat="server" Text="Report Type"></asp:Label>&nbsp;<span
                        style="font-weight: bold; color: Red;">*</span>
                        </td><td >
                    <telerik:RadComboBox EnableEmbeddedSkins="false" SkinID="impactGen" ID="ddlReportType"
                        runat="server" CausesValidation="False">
                        <Items>
                            <%--<telerik:RadComboBoxItem   Value="0" Text="-- Select a Type --" />--%>
                            <telerik:RadComboBoxItem Value="1" Text="Summary" />
                            <telerik:RadComboBoxItem Value="2" Text="Details" />
                        </Items>
                    </telerik:RadComboBox>
                </td>
                <td >
                    <asp:Label ID="lblBrands" runat="server" Text="Brand"></asp:Label>&nbsp;<span style="font-weight: bold;
                        color: Red;">*</span></td><td >
                    <telerik:RadComboBox EnableEmbeddedSkins="false" SkinID="impactGen" ID="ddlBrands"
                        runat="server" DataSourceID="dsBrand" DataTextField="PP_Brand_Name" DataValueField="PP_Brand_ID"
                        AppendDataBoundItems="true">
                        <Items>
                            <telerik:RadComboBoxItem Value="0" Text="-- Any Brand --" />
                        </Items>
                    </telerik:RadComboBox>
                </td>
                <td >
                    <asp:Label ID="lblSegment" runat="server" Text="National Account"></asp:Label>&nbsp;</td><td >
                    <telerik:RadComboBox EnableEmbeddedSkins="false" SkinID="impactGen" ID="ddlNationalAccount"
                        runat="server">
                        <Items>
                            <telerik:RadComboBoxItem Value="0" Text="-- Any National Account--" />
                        </Items>
                    </telerik:RadComboBox>
                    <br />
                    
                </td>
            </tr>
            <tr>            
                <td>
                    <asp:Label ID="lblRegion" runat="server" Text="Region"></asp:Label>&nbsp;<span style="font-weight: bold;
                        color: Red;">*</span></td><td>
                    <telerik:RadComboBox EnableEmbeddedSkins="false" SkinID="impactGen" ID="ddlRegion"
                        runat="server" DataSourceID="dsRegion" DataTextField="Region_Name" DataValueField="Region_ID"
                        AutoPostBack="true" AppendDataBoundItems="true" Width="180" 
                        OnSelectedIndexChanged="OnRegionChanged" CausesValidation="False">
                        <Items>
                            <telerik:RadComboBoxItem Value="0" Text="-- Select a Region --" />
                        </Items>
                    </telerik:RadComboBox>
                </td>
                <td>
                    <asp:Label ID="lblDistrict" runat="server" Text="District"></asp:Label></td><td>
                    <telerik:RadComboBox EnableEmbeddedSkins="false" SkinID="impactGen" ID="ddlDistrict"
                        runat="server" DataSourceID="dsDistrict" DataTextField="District_Name" DataValueField="District_ID" DropDownWidth="200px"
                        AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="OnDistrictChanged" CausesValidation="False">
                        <Items>
                            <telerik:RadComboBoxItem Value="0" Text="-- Any District --" />
                        </Items>
                    </telerik:RadComboBox>
                    <br />                    
                    
                </td>
                <td>
                    <asp:Label ID="lblTerritory" runat="server" Text="Territory"></asp:Label></td>
                <td>
                    <telerik:RadComboBox EnableEmbeddedSkins="false" SkinID="impactGen" ID="ddlTerritory"
                        runat="server" DataSourceID="dsTerritory" DataTextField="Territory_Name" DataValueField="Territory_ID" DropDownWidth="200px"
                        AppendDataBoundItems="true" CausesValidation="False">
                        <Items>
                            <telerik:RadComboBoxItem Value="0" Text="-- Any Territory --" />
                        </Items>
                    </telerik:RadComboBox>
                    </td>
            </tr>
            <tr>            
                <td>
                    <asp:Label ID="lblAccountManager" runat="server" Text="Account Manager"></asp:Label></td><td>
                    <telerik:RadComboBox EnableEmbeddedSkins="false" SkinID="impactGen" ID="ddlAccountManager"
                        runat="server" DataSourceID="dsDistrict" DataTextField="District_Name" DataValueField="District_ID" DropDownWidth="200px"
                        AppendDataBoundItems="true" CausesValidation="False">
                        <Items>
                            <telerik:RadComboBoxItem Value="0" Text="-- Any Regional Account Manager --" />
                        </Items>
                    </telerik:RadComboBox>
                </td>
                <td>
                    &nbsp;</td><td>
                    &nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
        </table>
        <pinso:CustomButton ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
        
       
        <asp:CompareValidator Display="None" ID="cvBrand" runat="server" ErrorMessage="Please select a Brand"
            ControlToValidate="ddlBrands" ValueToCompare="-- Select a Brand --" Operator="NotEqual">         
        </asp:CompareValidator>
        <br />
        <asp:CompareValidator Display="None" ID="cvRegion" runat="server" ErrorMessage="Please select a Region"
            ControlToValidate="ddlRegion" ValueToCompare="-- Select a Region --" Operator="NotEqual">         
        </asp:CompareValidator>
                    
        <asp:ValidationSummary ID="ValidationSummary1" DisplayMode="BulletList" EnableClientScript="true" runat="server"/>

        <asp:Label ID="lblMessage" runat="server" Text="No Records present for the selected search criteria."
            Visible="false"></asp:Label>
        
       
        <br />
        <div id="divPageContent" runat="server">
        <iframe id="reportviewerframe" runat="server" visible="false" frameborder="0" height="1" scrolling="no" width="95%">
        </iframe> 
        </div>

        <asp:SqlDataSource ID="dsBrand" runat="server" ConnectionString="<%$ ConnectionStrings:PathfinderClientDB_Format %>"
            SelectCommand="usp_GetBrandName" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
       
        <asp:SqlDataSource ID="dsRegion" runat="server" ConnectionString="<%$ ConnectionStrings:PathfinderClientDB_Format %>"
            SelectCommand="usp_GetRegionName" SelectCommandType="StoredProcedure">            
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="dsDistrict" runat="server" ConnectionString="<%$ ConnectionStrings:PathfinderClientDB_Format %>"
            SelectCommand="usp_GetDistrictName" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:ControlParameter ControlID="ddlRegion" Name="Region_ID" PropertyName="SelectedValue"
                    DefaultValue="0" />
            </SelectParameters>
        </asp:SqlDataSource>
          <asp:SqlDataSource ID="dsTerritory" runat="server" ConnectionString="<%$ ConnectionStrings:PathfinderClientDB_Format %>"
            SelectCommand="usp_GetTerritoryName" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:ControlParameter ControlID="ddlDistrict" Name="District_ID" PropertyName="SelectedValue"
                    DefaultValue="0" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
 </form>
</body>
</html>