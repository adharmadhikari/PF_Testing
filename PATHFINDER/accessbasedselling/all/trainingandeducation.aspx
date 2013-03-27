<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Modal.master" AutoEventWireup="true" CodeFile="trainingandeducation.aspx.cs" Inherits="accessbasedselling_all_trainingandeducation" %>
<%@ Register Namespace="Pathfinder" TagPrefix="pinso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="title" Runat="Server">
 Training & Education Menu
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="tools" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="main" Runat="Server">
    <asp:ScriptManagerProxy runat="server" ID="scriptManagerProxy">
        <Scripts>
            <asp:ScriptReference Path="~/content/scripts/components.js" />
        </Scripts>
    </asp:ScriptManagerProxy>
    <script type="text/javascript">
        var MenuReady = false;
    
        function initMenuItems() {
            var c = $find("menuSelector");
            if (c) {
                c.clear();

                c.addItem("salesrep", "Sales Representatives", "salesrep");
                c.add_itemClicked(onMenuClicked);
                c.addItem("accmgmt", "Account Management", "accmgrs");
                c.addItem("orgmktseries", "Organized Market Series", "orgmktseries");
                c.addItem("hcareterms", "Healthcare Terms", "hcareterms");
            }

            $("#menu2").css("visibility", "hidden");
            $("#menu2header").css("visibility", "hidden");

            $("#menu3").css("visibility", "hidden");
            $("#menu3header").css("visibility", "hidden");
        }

        function onMenuClicked(sender, args) {
            if (args.item.id == "salesrep") {
                sender.highlightItem("salesrep");
                $("#menu2").css("visibility", "visible");
                $("#menu2header").css("visibility", "visible");

                $get("menu2header").innerHTML = "SALES REPRESENTATIVES";
                //Copy Sales Rep menu contents from "menuSalesRep" div tag to "menu2" div tag.
                if (MenuReady == true) {
                    $get("menu2").innerHTML = $get("menuSalesRep").innerHTML;  
                }
                
                $("#menu3").css("visibility", "hidden");
                $("#menu3header").css("visibility", "hidden");
            }
            else if (args.item.id == "hcareterms") {
                sender.highlightItem("hcareterms");
                $("#menu2").css("visibility", "visible");
                $("#menu2header").css("visibility", "visible");
    
                //storing Sales Representatives menu contents in a div tag.
                $get("menuSalesRep").innerHTML = $get("menu2").innerHTML;
                MenuReady = true;
                
                $get("menu2header").innerHTML = "HEALTHCARE TERMS";
                $get("menu2").innerHTML = "<table><tr><td align='left'>Commercial Payer</td></tr><tr><td align='left'>Prior Authorization and Precertification</td></tr><tr><td align='left'>Medicare Part D - MA-PD PDP</td></tr><tr><td>&nbsp;</td></tr><tr><td>&nbsp;</td></tr><tr><td>&nbsp;</td></tr><tr><td>&nbsp;</td></tr><tr><td>&nbsp;</td></tr><tr><td>&nbsp;</td></tr><tr><td>&nbsp;</td></tr><tr><td>&nbsp;</td></tr></table>";
                
                $("#menu3").css("visibility", "hidden");
                $("#menu3header").css("visibility", "hidden");
                
            }
            else {
                $("#menu2").css("visibility", "hidden");
                $("#menu2header").css("visibility", "hidden");

                $("#menu3").css("visibility", "hidden");
                $("#menu3header").css("visibility", "hidden");
            }


            $(".selected").parent("span").parent("span").parent("span").removeClass("selected");
            $("#salesSim").removeClass("selected");
        }

        function ChangeBKColor(link, param) {
            link = "#" + link;
            if (param == "in") {
                $(link).css("background-color", "#dfdfdf"); 
              }
            else { $(link).css("background-color", "white"); }
        }

        function OnSalesSimClick() {
            $("#menu3").css("visibility", "visible");
            $("#menu3header").css("visibility", "visible");
            $("#salesSim").addClass("selected");
            $(".selected").parent("span").parent("span").parent("span").addClass("selected");
        }
    </script>
        
    <table width="100%" class="dashboardTable">
        <tr>
            <td valign="top" align="left" style="border-right: solid 1px #2d58a7 !important;" width="30%"> 
                 <table>
                        <tr>
                            <td valign="top" align="left" style="color:#2d58a7; font-weight:bold; border-bottom: solid 1px #2d58a7 !important;">
                                <asp:Literal id="header1" runat="server" Text="MAIN CURRICULUM LIST"></asp:Literal> 
                            </td>
                        </tr>                                    
                        <tr>
                            <td align="left">
                                <div id="menuSelector" class="reportSelector"></div>
                                <pinso:Menu runat="server" ID="menuControl" Target="menuSelector" CssClass="coreBtn" SelectedCssClass="selected" OnClientInitialized="initMenuItems"/>
                            </td>
                        </tr>
                    </table> 
            </td>
            
            <td valign="top" align="left" width="35%" style="border-right: solid 1px #2d58a7 !important;" >  
                    <table>
                        <tr>
                            <td align="left" style="color:#2d58a7; font-weight:bold; border-bottom: solid 1px #2d58a7 !important;"> 
                                <div id="menu2header" visible="false" style="visibility:hidden ">SALES REPRESENTATIVES</div>
                            </td>
                        </tr>
                        
                        <tr><td>
                            <div id="menu2" visible="false" style="visibility:hidden ">
                                <table>
                                <tr><td align="left"><a href="potsproxy.aspx" id="link1" target="_blank" class="button" style='padding-left:0px' >Understanding Manged Care</a></td></tr>
                                <tr><td align="left"><a href="potsproxy.aspx" id="link2" target="_blank" class="button" style='padding-left:0px' >Pull-Through and Push-Through Strategies</a></td></tr>
                                <tr><td align="left"><a href="potsproxy.aspx" id="link3" target="_blank" class="button" style='padding-left:0px' >Medicare & Medicaid: Changing the face of healthcare</a></td></tr>
                                <tr><td align="left"><a href="potsproxy.aspx" id="link4" target="_blank"  class="button" style='padding-left:0px' >Speciality Pharmaceuticals</a></td></tr>
                                <tr><td align="left"><a href="potsproxy.aspx" id="link5" target="_blank"  class="button" style='padding-left:0px' >Selling to the Retail Pharmacist</a></td></tr>
                                <tr><td align="left"><a href="potsproxy.aspx" id="link6" target="_blank"  class="button" style='padding-left:0px' >Hospital Selling</a></td></tr>
                                <tr><td align="left"><a href="potsproxy.aspx" id="link7" target="_blank"  class="button" style='padding-left:0px' >The Senior Care Maketplace</a></td></tr>
                                <tr><td align="left"><a href="potsproxy.aspx" id="link8" target="_blank"  class="button" style='padding-left:0px' >Coaching Managed Care</a></td></tr>
                                <tr><td align="left"><div class="reportSelector"><span id="salesSim" class="coreBtn"><span class="bg"><span class="bg2"><a class="button" style='padding-left:0px' href="javascript:OnSalesSimClick();" id="link9" >Sales Simulations</a></span></span></span></div></td></tr>
                                </table>
                            </div>
                        </td></tr>
                    </table>
                
            </td>
            
             <td valign="top" align="left" width="35%">  
                    <table width="100%">
                        <tr>
                            <td align="left" style="color:#2d58a7; font-weight:bold; border-bottom: solid 1px #2d58a7 !important;"> 
                               <div id="menu3header" visible="false" style="visibility:hidden ">SALES SIMULATIONS</div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div id="menu3" visible="false" style="visibility:hidden ">
                                    <table>
                                    <tr><td align="left">New Sales Representatives</td></tr>
                                    <tr><td align="left">Seasoned Sales Representatives</td></tr>
                                    <tr><td align="left">Medicare Part D</td></tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
            </td>
        </tr>
    </table>
    
    
    <div id="menuSalesRep" visible="false" style="visibility:hidden ">
    </div>
    
</asp:Content>

