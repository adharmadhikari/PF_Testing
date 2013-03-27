<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/PartialPage.master" AutoEventWireup="true" CodeFile="SellSheetCarousel.aspx.cs" Inherits="custom_unitedthera_all_SellSheetCarousel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="scriptContainer" Runat="Server">
    <script type="text/javascript" >
        clientManager.add_pageLoaded(sellSheetCarousel_pageLoaded, "customInfoArea");
        clientManager.add_pageUnloaded(sellSheetCarousel_pageUnloaded, "customInfoArea");

        function sellSheetCarousel_pageLoaded(sender, args)
        {
        }

        function initializeSideBarCarousel(startPos)
        {
            //Initialize carousel - called from SellSheetStep.master
            $('#templateSidebar').jcarousel(
            {
                scroll: 1,
                start: startPos,
                initCallback: sideBarTemplate_initCallback
            });
        }

        function sellSheetCarousel_pageUnloaded(sender, args)
        {
            sender.remove_pageLoaded(sellSheetCarousel_pageLoaded, "customInfoArea");
            sender.remove_pageUnloaded(sellSheetCarousel_pageUnloaded, "customInfoArea");
        }

        function setSideBarTheraDrugTemplate(templateID)
        {
            //Update the templateID hidden field on Step 1
            $("#ctl00_ctl00_Tile3_StepBody_txtTemplateID").val(templateID)

            //Remove all 'selectedTemplate' CSS classes in Template Selector
            $('#templateSidebar li img').removeClass('selectedTemplate');

            //Add CSS class to selected item
            //Since the jQuery reflector creates an image below the template image, we need to apply 'selectedTemplate' only to the template image and not the reflection
            $('#templateSidebar li').filter('[rel=' + templateID + ']').find("img[id*='ctl00']").addClass('selectedTemplate');

            var queryString = clientManager.getSelectionDataForPostback();
            var ssID = queryString.replace("Sell_Sheet_ID=", "");

            //Update TemplateID in Database
            if (ssID.length > 0)
                clientManager.get_ApplicationManager().updateTemplateID(templateID, ssID);
        }

        function sideBarTemplate_initCallback(carousel, state)
        {
            $(".sellsheets #customInfoArea div").css("visibility", "visible"); 
            
            //Make the jCarousel object accessible globally
            clientManager.get_ApplicationManager().set_carousel(carousel);
        };   
        
    </script>
        
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="partialPage" Runat="Server">
    <div class="ssSidebarMainHeader">Template Options</div>
    <div class="ssSidebarHeader" style="display: none"><span class="ssBold">Templates</span></div>
    <div class='templates' style="display:none">
        <ul id="templateSidebar" class="jcarousel-skin-sidebar">
            <asp:Repeater ID="rptTemplates" runat="server" DataSourceID="dsTemplates">
                <ItemTemplate>
                <!-- Added rel attribute to image so selected template can be highlighted in css on page load -->
                <li id="Li1" runat="server" rel='<%# Eval("Template_ID") %>'><asp:Image ID="Image1" runat="server" ImageUrl='<%# String.Format("custom/{0}/sellsheets/templates/{1}", Pinsonault.Web.Session.ClientKey, Eval("Template_Name") ).Replace(".", "sm.") %>' OnClick='<%# String.Format("setSideBarTheraDrugTemplate({0})", Eval("Template_ID")) %>'/></li>
                </ItemTemplate>
            </asp:Repeater>
            <asp:EntityDataSource ID="dsTemplates" runat="server" EntitySetName="TheraDrugTemplateSet" DefaultContainerName="PathfinderClientEntities">
            </asp:EntityDataSource>
        </ul>
    </div>
    <div class="ssSidebarHeader"><span class="ssBold">Tips</span></div>
    <div class="tipContainer">
        <div class='tip'></div>
    </div>
</asp:Content>

