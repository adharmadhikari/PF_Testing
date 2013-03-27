<%@ Page Theme="pathfinder" Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableViewState="false"
    AutoEventWireup="true" CodeFile="dashboard.aspx.cs" Inherits="dashboard" EnableTheming="true" %>
<%@ OutputCache VaryByParam="None" Duration="1"  NoStore="true" %>

<%@ Register Src="~/controls/planinfo.ascx" TagName="planinfo" TagPrefix="pinso" %>
<%@ Register Src="controls/subheader.ascx" TagName="subheader" TagPrefix="pinso" %>
<%@ Register Src="controls/subheaderMenu.ascx" TagName="subheaderMenu" TagPrefix="pinso" %>
<%@ Register Src="~/controls/ModuleSelection.ascx" TagName="ModuleSelection" TagPrefix="pinso" %>
<%--<%@ Register Src="~/controls/Map.ascx" TagName="Map" TagPrefix="pinso" %>--%>

<asp:Content runat="server" ContentPlaceHolderID="Main" ID="Main">    
    <pinso:subheader ID="subheader1" runat="server" />
    
    <asp:ScriptManagerProxy runat="server" ID="scriptManagerProxy">
        <Scripts>
            <asp:ScriptReference Path="~/controls/map/include/fmASAPI.js" />
            <asp:ScriptReference Path="~/controls/map/include/fmActivate.js" />   
        </Scripts>
    </asp:ScriptManagerProxy>    
    <div id="table1" class="todaysAccounts1" style="visibility:hidden">
        <div id="tile1" class="leftTile">
            <div id="expandTile1"><a href="javascript:void(0);" title="Expand" onclick="maxTile1();"></a></div>
            <div id="divTile1Container" class="tileContainer">
                <div class="tileContainerHeader">
                    <div class="title" runat="server" id="mapOptions">
                        <telerik:RadMenu EnableEmbeddedSkins="false" runat="server" ID="rdlMarketBasketList" SkinID="drugMarket" ClickToOpen="true" />
                        <telerik:RadMenu EnableEmbeddedSkins="false"  runat="server" ID="rdlDrugList" SkinID="drugName" ClickToOpen="true" />                     
                        <telerik:RadMenu EnableEmbeddedSkins="false"  runat="server" ID="rdlRegionList" SkinID="drugName" ClickToOpen="true" />   
                        <div class="resetBtnDiv" style="display:none;" id="btnResetGeog"><a href="javascript:clientManager.resetGeography();">Reset Region</a></div>
                    </div>
                    <div class="tools">
                        <img class="showHideBtn min" alt="collapse" title="collapse" src="content/images/spacer.gif" onclick="minTile1();" />
                        <img class="showHideBtn enlarge" alt="enlarge" title="enlarge" src="content/images/spacer.gif" onclick="maxMap();" />
                        <img class="showHideBtn close" alt="close" title="close" src="content/images/spacer.gif" onclick="minMap();" />
                        </div>
                    <div class="clearAll">
                    </div>
                </div>
                <div id="divTile1" class="mapTile">
                    <%--<pinso:Map runat="server" ID="map" ContainerFileName="mapcontainer.swf" />--%>
                </div>
            </div>
        </div>
        <div id="tile2" class="rightTile"> 
        <div id="expandTile2SR"><a href="javascript:void(0);" title="Expand" onclick="clientManager.get_ApplicationManager().expandSidePanel()"></a></div>           
            <div id="divTile2Container" class="tileContainer">
                <div class="tileContainerHeader">
                    <div class="title"></div>
                    <div class="myAccts" style="display:none;">
                        <telerik:RadMenu SkinID="accountView" runat="server" ID="menuMyAccts" EnableEmbeddedSkins="false" ClickToOpen="true" OnClientItemClicked="function (sender, args){var val =args.get_item().get_value(); if(val==0)return;clientManager.get_PlanInfoGrid().GridWrapper.set_restrictByTerritory(val == 2);$refreshMenuOptions(sender, val);}">
                            <Items>
                                <telerik:RadMenuItem Text="All Accounts" Value="0">
                                    <Items>
                                        <telerik:RadMenuItem Text="All Accounts" Value="1" />
                                        <telerik:RadMenuItem Text="My Accounts" Value="2" />
                                    </Items>                                
                                </telerik:RadMenuItem>
                            </Items>
                        </telerik:RadMenu>
                    </div>
                    <div class="tools">
                        <span class="textResize"><span class="textSm"><a href="javascript:void(0);" onclick="textSmall();">A</a></span><span class="textMd"><a href="javascript:void(0);" onclick="textMedium();">A</a></span><span class="textLg"><a href="javascript:void(0);" onclick="textLarge();">A</a></span></span>
                        <img class="showHideBtn min" alt="collapse" title="collapse" src="content/images/spacer.gif" onclick="clientManager.get_ApplicationManager().collapseSidePanel()" />
                        <img class="showHideBtn close" alt="close" title="close" src="content/images/spacer.gif" onclick="minPlanInfo();" />
                        <img class="showHideBtn enlarge" alt="enlarge" title="enlarge" src="content/images/spacer.gif" onclick="maxPlanInfo();"/>
                    </div>
                    <div class="pagination"></div>
                    <div class="clearAll">
                    </div>
                </div>
                <div id="divTile2">
                    <div class="tile2Div" id="divTile2Plans"><pinso:planinfo ID="planInfo" runat="server" /></div>
                    <div class="tile2Div" id="divTile2ModuleSelection" style="display:none;"><pinso:ModuleSelection runat="server" ID="moduleSelection" /></div>                    
                </div>
            </div>
        </div>
        <div class="clearAll">
    </div>
    </div>
    <div class="clearAll">
    </div>
    <div class="navbar2" style="visibility:hidden"><pinso:subheaderMenu ID="subheaderMenu" runat="server" /></div>
    <div id="section2" class="todaysAccounts2" style="visibility:hidden">
        
        <!-- LOAD MASTERPAGE AREA -->
        
    </div>
    <div class="clearAll">
    </div>

     <telerik:radwindowmanager EnableEmbeddedSkins="false" Skin="pathfinder" id="RadWindowManager1" runat="server" DestroyOnClose="true" Modal="true" 
           Behaviors="Close" VisibleTitlebar="false">    
    </telerik:radwindowmanager>
    
    <%--Favorites List Popup --%>
    <div id="favoritesListWindow"></div>
    
    <%--Generic Popup Window --%>
    <div id="infoPopup">        
    </div>

        <iframe id="_clientmanagerhistory" src='content/history.aspx' style="display:none"></iframe>

</asp:Content>

<asp:Content ID="contentInitializationScript" runat="server" ContentPlaceHolderID="initializationScript">
    <%-- This script must be at end of page to ensure that all other ajax controls have been initialized.  --%>
    <script type="text/javascript">

        Sys.Application.add_init(function()
        {
            clientManager = $create(Pathfinder.UI.ClientManager, 
                { "BasePath": '<%= Pinsonault.Web.Support.BasePath %>'
                    ,"ClientKey": '<%= Pinsonault.Web.Session.ClientKey %>'
                    ,"ClientOptions": '<%= Pinsonault.Web.Session.ClientOptions %>'
                    ,"UserKey": '<%= Pinsonault.Web.Session.UserKey %>'
                    ,"Application": '<%= ApplicationID %>'
                    ,"UserGeography": userGeography
                    ,"ApplicationMenuOptions": appMenuItems
                    ,"ChannelMenuOptions": channelMenuItems
                    ,"DrugListOptions": drugListOptions
                    ,"MarketBasketListOptions": marketBasketListOptions 
                    ,"RegionListOptions": regionsList
//                    , "RegionGeographyListOptions": regionsGeographyList
                    , "ModuleOptions": userModules
                    ,"CurrentUIState": <%= CurrentUIState %>
                    ,"clientHasCustomPlans": clientHasCustomPlans 
                }, null, null, $get("mainSection"));
        });
        
        //for showing the loading symbol while page is loading 
        $("#report-pane").ajaxStart(function()
        {
            var width = $(this).width();
            var height = $(this).height();
            $("#report-loading").css(
                { top: 105, left: ((width / 2) - 50) }).fadeIn(200);    // fast fade in of 200 mili-seconds
        }
        ).ajaxStop(function()
            {
                $("#report-loading", this).fadeOut(1000);    // slow fade out of 1 second 
        });
   </script>
      
</asp:Content>
