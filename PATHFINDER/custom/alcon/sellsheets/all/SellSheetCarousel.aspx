<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/PartialPage.master" AutoEventWireup="true" CodeFile="SellSheetCarousel.aspx.cs" Inherits="custom_pinso_all_SellSheetCarousel" %>

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
                start: 1,
                initCallback: sideBarTemplate_initCallback,
                buttonNextHTML: null,
                buttonPrevHTML: null
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


            validateDrugSelection(ssDruglistSelected, ssRequiredDrugSelected, templateID);

            //var queryString = clientManager.getSelectionDataForPostback();
            //var ssID = queryString.replace("Sell_Sheet_ID=", "");

            //Update TemplateID in Database
            //if (ssID.length > 0)
                //clientManager.get_ApplicationManager().updateTemplateID(templateID, ssID);
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
    <div class="ssSidebarHeader">
        <span class="ssBold">Templates</span>
    </div>
    <div id="div_templateSidebar">
      <ul id="templateSidebar" class="jcarousel-skin-sidebar">
            <li id="Li2" runat="server" rel="3" thera_id="150" >
                <asp:Image ID="Image2" runat="server" ImageUrl="custom/Alcon/sellsheets/templates/AZT_Pathfinder_Rx_Formulary_portraitsm.jpg" OnClick="setSideBarTheraDrugTemplate(3)"/>
            </li>
            <li id="Li3" runat="server" rel="4" thera_id="153" >
                <asp:Image ID="Image3" runat="server" ImageUrl="custom/alcon/sellsheets/templates/CDX_Pathfinder_Rx_Formulary_portraitsm.jpg" OnClick="setSideBarTheraDrugTemplate(4)"/>
            </li>
            <li id="Li4" runat="server" rel="13" thera_id="159" >
                <asp:Image ID="Image4" runat="server" ImageUrl="custom/alcon/sellsheets/templates/DZL_Pathfinder_Rx_Formulary_portraitsm.jpg" OnClick="setSideBarTheraDrugTemplate(13)"/>
            </li>
            <li id="Li1" runat="server" rel="20" thera_id="151">
                <asp:Image ID="Image1" runat="server" ImageUrl="custom/alcon/sellsheets/templates/VIG_Pathfinder_Rx_Formulary_portraitsm.JPG" OnClick="setSideBarTheraDrugTemplate(20)"/>
            </li>
            <li id="Li5" runat="server" rel="14" thera_id="151" >
                <asp:Image ID="Image5" runat="server" ImageUrl="custom/alcon/sellsheets/templates/MZA_Pathfinder_Rx_Formulary_portraitsm.jpg" OnClick="setSideBarTheraDrugTemplate(14)"/>
            </li>
            <li id="Li6" runat="server" rel="15" thera_id="157" >
                <asp:Image ID="Image6" runat="server" ImageUrl="custom/alcon/sellsheets/templates/NEV_Pathfinder_Rx_Formulary_portraitsm.jpg" OnClick="setSideBarTheraDrugTemplate(15)"/>
            </li>
            <li id="Li7" runat="server" rel="16" thera_id="149" >
                <asp:Image ID="Image7" runat="server" ImageUrl="custom/alcon/sellsheets/templates/PTD_Pathfinder_Rx_Formulary_portraitsm.jpg" OnClick="setSideBarTheraDrugTemplate(16)"/>
            </li>
            <li id="Li8" runat="server" rel="17" thera_id="158" >
                <asp:Image ID="Image8" runat="server" ImageUrl="custom/alcon/sellsheets/templates/TDXST_Pathfinder_Rx_Formulary_portraitsm.jpg" OnClick="setSideBarTheraDrugTemplate(17)"/>
            </li>
            <li id="Li9" runat="server" rel="18" thera_id="1501" >
                <asp:Image ID="Image9" runat="server" ImageUrl="custom/alcon/sellsheets/templates/TRZ_Pathfinder_Rx_Formulary_portraitsm.jpg" OnClick="setSideBarTheraDrugTemplate(18)"/>
            </li>
             <li id="Li10" runat="server" rel="19" thera_id="187" >
                <asp:Image ID="Image10" runat="server" ImageUrl="custom/alcon/sellsheets/templates/Patanase_portraitsm.jpg" OnClick="setSideBarTheraDrugTemplate(19)"/>
            </li>
          <%--  <asp:Repeater ID="rptTemplates" runat="server" DataSourceID="dsTemplates">
                <ItemTemplate>
                <!-- Added rel attribute to image so selected template can be highlighted in css on page load -->
                <li id="Li1" runat="server" rel='<%# Eval("Template_ID") %>'><asp:Image ID="Image1" runat="server" ImageUrl='<%# String.Format("custom/{0}/sellsheets/templates/{1}", Pinsonault.Web.Session.ClientKey, Eval("Template_Name") ).Replace(".", "sm.") %>' OnClick='<%# String.Format("setSideBarTheraDrugTemplate({0})", Eval("Template_ID")) %>'/></li>
                </ItemTemplate>
            </asp:Repeater>
            <asp:EntityDataSource ID="dsTemplates" runat="server" EntitySetName="TheraDrugTemplateSet" DefaultContainerName="PathfinderClientEntities">
            </asp:EntityDataSource>  --%>
     </ul>
    </div>
    <div class="ssSidebarHeader"><span class="ssBold">Tips</span></div>
    <div class="tipContainer">
        <div class='tip'></div>
    </div>
</asp:Content>

