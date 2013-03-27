<%@ Page Title="PowerPlanRx - My Campaign Opportunities" Language="C#"  
    EnableViewState="true"  Theme="impact" AutoEventWireup="true" CodeFile="mycampaigns_opportunitiesReal.aspx.cs" 
    Inherits="mycampaigns_opportunitiesReal" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server"> 
    <link id="lnkImpactCss" runat="server" href="~/App_Themes/impact/impact.css" rel="Stylesheet" type="text/css" />   
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
    <script type="text/javascript">
        $(document).ready(function()
        {
            var i = 0;
            var brand;
            $("#dial").dialog({ autoOpen: false, modal: true, width: 800, height: 450, title: "", resizable: false, draggable: false });
            for (i = 0; i < 100; i++)
            {

                brand = "BrandName" + i;

                var j = $("." + brand);

                if (j.length > 0)
                    j.text($("#" + brand).val());
                else
                    break;
            }

        });
        
        // for campaign rational selection pop up
        function createNewCampaign(url)
        {
            $("#dial").html("<iframe style='height:100%;width:100%'></iframe>").dialog('option', 'title', 'Create Campaign').dialog('open').find("iframe").attr("src", url);
        }
        
        //for showing brand comparision pop up
        function showBrandComparison(PlanID, BrandID, MBID, SegmentID)
        {           
            //$("#dial").html("Loading...").load("BrandComparisonPieChart.aspx?plan_id=" + PlanID + "&mb_id=" + MBID + "&brand_id=" + BrandID + " form >*").dialog('option', 'title', 'Brand Comparison').dialog('open');
            $("#dial").html("<iframe style='height:100%;width:100%'></iframe>").dialog('option', 'title', 'Brand Comparison').dialog('open').find("iframe").attr("src", "BrandComparisonPieChart.aspx?plan_id=" + PlanID + "&mb_id=" + MBID + "&brand_id=" + BrandID + "&segment_id=" + SegmentID);
            
        }

        //ui events functions
        function validate() //To notify user to select a market basket from dropdown
        {
            var combo = $find("<%= mbradcombobox.ClientID %>");
            var text = combo.get_value();
            if (text == "0")
            {
                alert("Please select a market basket.");
                return false;
            }
            return true;
        }
        
        var cancelDropDownClosing = false;

        function StopPropagation(e)
        {
            //cancel bubbling
            e.cancelBubble = true;
            if (e.stopPropagation)
            {
                e.stopPropagation();
            }
        }

        function onDropDownClosing()
        {
            cancelDropDownClosing = false;
        }

        function onCheckBoxClick(chk)
        {
            var combo = $find("<%= radBrands.ClientID %>");
            var RTb = $get("<%= tempbox1.ClientID %>");

            //prevent second combo from closing
            cancelDropDownClosing = true;
            //holds the text of all checked items
            var text = "";
            //holds the values of all checked items
            var values = "";
            //get the collection of all items
            var items = combo.get_items();
            //enumerate all items
            for (var i = 0; i < items.get_count(); i++)
            {
                var item = items.getItem(i);
                //get the checkbox element of the current item
                var chk1 = $get(combo.get_id() + "_i" + i + "_chk1");
                if (chk1.checked)
                {
                    text += item.get_text() + ",";
                    values += item.get_value() + ",";
                }
            }
            //remove the last comma from the string
            text = removeLastComma(text);
            values = removeLastComma(values);

            if (text.length > 0)
            {
                //set the text of the combobox
                combo.set_text(text);
                RTb.value = values;
            }
            else
            {
                //all checkboxes are unchecked so reset the controls                
                combo.set_text("");
            }
        }

        //this method removes the ending comma from a string
        function removeLastComma(str)
        {
            return str.replace(/,$/, "");
        }

        function OnClientDropDownClosingHandler(sender, e)
        {
            //do not close the second combo if  a checkbox from the first is clicked
            e.set_cancel(cancelDropDownClosing);
        }
    </script>
   
   <div class="MC_Opportunity">
        <table cellpadding="0" cellspacing="0" id="iframeTable" class="iframeTable">
            <tr>
                <td class="tileContainerHeader" colspan="7">My Campaign Opportunity</td>
            </tr>
            <tr>
                <td>Market Basket:&nbsp;<span
                        style="font-weight: bold; color: Red;">*</span></td>   
                <td >
                    <telerik:RadComboBox SkinID="impactGen" ID="mbradcombobox" runat="server" DataSourceID="dsMarketBasket" 
                        DataTextField="MB_Name" DataValueField="MB_ID" AppendDataBoundItems="true" EnableEmbeddedSkins="false"
                        OnSelectedIndexChanged="mbradcombobox_SelectedIndexChanged" AutoPostBack="true">       
                        <Items>
                            <telerik:RadComboBoxItem runat="server" Value="0" Text="Select a Market Basket" />  
                        </Items>       
                    </telerik:RadComboBox>               
                </td>
                <td >Segment:</td>
                <td>
                <telerik:RadComboBox EnableEmbeddedSkins="false" SkinID="impactGen" ID="rcbsegment" runat="server" DataSourceID="dsSegments" 
                    DataTextField="Segment_Name" DataValueField="Segment_ID" AppendDataBoundItems="true" >
                    <Items>

                    </Items>
                </telerik:RadComboBox>    
                </td>
                <td>Competitor Brands:</td>
                <td>       
                    <telerik:RadComboBox EnableEmbeddedSkins="false" SkinID="impactGen" ID="radBrands" runat="server" 
                        DataSourceID="dsBrands"
                        DataValueField="PP_Brand_ID" DataTextField="PP_Brand_Name" EmptyMessage="Please select a brand" HighlightTemplatedItems="true"
                        AllowCustomText="true" Width="240px"
                        OnClientDropDownClosed="onDropDownClosing" >
                        <ItemTemplate>
                            <div onclick="StopPropagation(event)">
                                <asp:CheckBox runat="server" ID="chk1" Checked="false" onclick="onCheckBoxClick(this)"/>
                                <asp:Label runat="server" ID="Label1" AssociatedControlID="chk1">
                                    <%# Eval("PP_Brand_Name")%>
                                </asp:Label>                                
                            </div>
                        </ItemTemplate>
                    </telerik:RadComboBox>                   
                    <asp:HiddenField ID="tempbox1" runat="server" Value="" />          
                </td>
                <td>         
                    <asp:HiddenField ID="brandscnt" runat="server" Value="" />
                    <asp:HiddenField ID="NNBrandsList" runat="server" Value="" /> 
                    <div class="impactBtns" id="div1" runat="server">
                        <pinso:CustomButton ID="btnSubmit" EnableEmbeddedSkins="false" SkinID="formButton" runat="server" Text="Submit" 
                        OnClick="btnSubmit_Click" OnClientClick=" return validate()"/>            
                    </div>
                </td>
            </tr>    
            <asp:SqlDataSource ID="dsMarketBasket" runat="server" ConnectionString="<%$ ConnectionStrings:PathfinderClientDB_Format %>" 
                SelectCommand="select PP_MB_ID as MB_ID,PP_MB_Name as MB_Name from pprx.Lkp_PP_Market_Basket">
            </asp:SqlDataSource>

            <asp:SqlDataSource ID="dsSegments" runat="server" ConnectionString="<%$ ConnectionStrings:PathfinderClientDB_Format %>" 
                SelectCommandType="StoredProcedure" SelectCommand="pprx.usp_GetSectionSegments" >
                 <SelectParameters>
                    <asp:Parameter Name="IncludeAll" DbType="Boolean" DefaultValue="true" /> 
                 </SelectParameters> 
            </asp:SqlDataSource>

            <asp:SqlDataSource ID="dsBrands" runat="server" ConnectionString="<%$ ConnectionStrings:PathfinderClientDB_Format %>"
            SelectCommand="pprx.usp_BrandsByMarketBasket" SelectCommandType="StoredProcedure"> 
                <SelectParameters>
                    <asp:Parameter Name="Is_Campaign_Brand" DbType="Boolean" DefaultValue="false" />      
                    <asp:controlparameter name="MB_ID" controlid="mbradcombobox" propertyname="SelectedValue" Type="Int32" DefaultValue="1" />
                </SelectParameters>
            </asp:SqlDataSource>       		    
         </table>
   </div>
   
   <div class="MC_Results" id="divResult" runat="server" visible="false"> 
        <telerik:RadGrid OnSortCommand="onSort" ID="radPlanProductBrandData" SkinID="table1" EnableEmbeddedSkins="false" runat="server" 
            AllowSorting="false" GridLines="None" 
            AutoGenerateColumns="False" BorderStyle="None"  EmptyDataText="No Data available"            
            Width="100%" >
            <MasterTableView autogeneratecolumns="False" datakeynames="Plan_ID">
          
            <Columns>
                <telerik:GridBoundColumn DataField="Plan_ID" HeaderText="Plan_ID" ReadOnly="True"  Visible="false" SortExpression="Plan_ID" />             
                <telerik:GridTemplateColumn HeaderText="" ItemStyle-CssClass="addRationale merged">     
                    <ItemTemplate>                                                 
                        <asp:HyperLink ID="HyperLink1" NavigateUrl='<%# string.Format("javascript:createNewCampaign(\"Campaign_Rationale.aspx?Plan_ID={0}&Brand_ID={1}&SegmentID={2}&BrandList={3}\")",Eval("Plan_ID"),Eval("PP_Brand_ID"),Eval("Segment_Id"),CompetitorsBrandIDs+","+Eval("PP_Brand_ID")) %>' 
                        runat="server" ImageUrl="~/App_Themes/impact/images/plus.gif"></asp:HyperLink>              
                    </ItemTemplate>                     
                </telerik:GridTemplateColumn> 
                 <telerik:GridTemplateColumn HeaderText=""  ItemStyle-CssClass="merged">     
                    <ItemTemplate>  
                        <asp:HyperLink ID="HyperLink1" NavigateUrl='<%# string.Format("javascript:showBrandComparison(\"{0}\", {1},{2},{3})", Eval("Plan_ID"), Eval("PP_Brand_ID"),mbradcombobox.SelectedValue, Eval("Segment_ID")) %>' runat="server" ImageUrl="~/App_Themes/impact/images/pie_icon.gif"></asp:HyperLink>  
                    </ItemTemplate>                      
                </telerik:GridTemplateColumn>                     
                <telerik:GridBoundColumn DataField="PlanRank" HeaderText="Rank" ReadOnly="True" SortExpression="PlanRank" ItemStyle-CssClass="merged" />                            
                <telerik:GridBoundColumn DataField="Plan_Name" HeaderText="Plan Name" ReadOnly="True" SortExpression="Plan_Name"  ItemStyle-CssClass="merged alignLeft" />           
                <telerik:GridBoundColumn DataField="Section_ID" HeaderText="Section_ID" ReadOnly="True" Visible="false"  SortExpression="Section_ID"  ItemStyle-CssClass="merged" />
                <%--<telerik:GridBoundColumn DataField="Section_Name" HeaderText="Section_Name" Visible="false" ReadOnly="True" SortExpression="Section_Name"  ItemStyle-CssClass="merged alignLeft" />--%>
                <telerik:GridBoundColumn DataField="Segment_ID" HeaderText="Segment_ID" ReadOnly="True" Visible="false" SortExpression="Segment_ID" ItemStyle-CssClass="merged" />
                <telerik:GridBoundColumn DataField="Segment_Name" HeaderText="Segment Name" ReadOnly="True" SortExpression="Segment_Name" ItemStyle-CssClass="merged alignLeft" />                
                <telerik:GridBoundColumn DataField="covered_lives" HeaderText="Covered Lives"  DataFormatString="{0:#,###}" ReadOnly="True" SortExpression="covered_lives" ItemStyle-CssClass="merged alignRight" />                      
                <telerik:GridBoundColumn DataField="MB_Trx" HeaderText="Market Volume" ReadOnly="True" DataFormatString="{0:#,###}" SortExpression="MB_Trx" ItemStyle-CssClass="rightScroll alignRight" />  
            </Columns>
            </MasterTableView>
            <ClientSettings>
                <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="0" />
            </ClientSettings>
        </telerik:RadGrid>     
   </div>
   
   <div id="dial" style="display:none;"></div>
 
 </form>
</body>
</html>