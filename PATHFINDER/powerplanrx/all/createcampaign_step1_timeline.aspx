<%@ Page Language="C#" MasterPageFile="~/powerplanrx/MasterPages/MasterPage.master" Theme="impact" Title="PowerPlanRx - Step 1 Timeline" AutoEventWireup="true" CodeFile="createcampaign_step1_timeline.aspx.cs" Inherits="createcampaign_step1_timeline" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="main" Runat="Server">
    <asp:UpdatePanel runat="server" ID="updatePanel">
        <ContentTemplate>
            <script type="text/javascript">
                function onDurationChanged()
                {
                    var duration = parseInt($get("ctl00_main_formView_dlDuration").value);
                    var dt = new Date($get("ctl00_main_formView_txtStartDate").value);

                    if(!isNaN(duration) && !isNaN(dt))
                    {
                        dt.setMonth(dt.getMonth() + duration);
                        $get("ctl00_main_formView_txtEndDate").value = dt.format("M/d/yyyy");
                    }
                }

                function validateStartDate(o, a)
                {
                    var startDate = new Date($get("ctl00_main_formView_txtStartDate").value);
                    var date = new Date();
                    a.IsValid = true;
                    
                    if (!isNaN(startDate))
                    {
                        date = date.setHours(0, 0, 0, 0);
                        a.IsValid = startDate >= date;
                    }
                }
            </script>
            
            <asp:FormView runat="server" ID="formView" class="AP_timeline" DataSourceID="dsCampaignInfo" DataKeyNames="Campaign_ID" DefaultMode="ReadOnly">
                <ItemTemplate>
                <div class="tileContainerHeader">
                    <div class="CampaignInfo">Campaign Name: <asp:label ID="Label1" runat="server"  Text ='<%# Eval("Campaign_Name")%>'></asp:label></div>
                </div>
                <div class="tileSubHeader">Campaign Timelines</div>
                <table class="TimelineContent" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="cell"><b>Campaign Start Date</b>
                    <asp:Label runat="server" ID="lblStartDate" Text='<%# Eval("Start_Date", "{0:d}") %>' /></td>
                    
                    <td class="cell"><b>Duration (months)</b>
                    <asp:Label runat="server" ID="lblDuration" Text='<%# Eval("Campaign_Duration") %>'/></td>
                                        
                    <td class="cell last"><b>Campaign End Date</b>
                    <asp:Label runat="server" ID="lblEndDate" Text='<%# Eval("End_Date", "{0:d}") %>' /></td>
                  </tr> 
                 </table> 
                </ItemTemplate>                     
                <EditItemTemplate>
                 <div class="tileContainerHeader">
                    <div class="CampaignInfo"><asp:label ID="Label1" runat="server"  Text ='<%# Eval("Campaign_Name")%>'></asp:label></div>
                </div>
                <div class="tileSubHeader">Campaign Timelines</div>
                    <table class="TimelineContent" cellpadding="0" cellspacing="0">
                        <col width="10%" />
                        <col width="23%" />
                        <col width="10%" />
                        <col width="23%" />
                        <col width="10%" />
                        <col width="23%" />
                        <tr>
                            <td class="TLcol1 cell">
                                <b>Campaign Start Date</b>
                            </td>
                            <td class="cell">
                                <asp:TextBox runat="server" ID="txtStartDate" Text='<%# Bind("Start_Date", "{0:d}") %>'
                                    onchange="onDurationChanged()" Width="80px" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Start Date is required."
                                    ControlToValidate="txtStartDate" Display="Dynamic" />
                                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txtStartDate" Display="Dynamic"
                                    Operator="DataTypeCheck" Type="Date" ErrorMessage="Please enter a valid Start Date." />
                                <ajax:CalendarExtender runat="server" ID="calendar1" TargetControlID="txtStartDate" />
                            </td>
                            <td class="cell">
                                <b>Duration (months)</b>
                            </td>
                            <td class="cell">
                                <telerik:RadComboBox runat="server" ID="dlDuration" EnableEmbeddedSkins="false" SkinID="impactGen" Width="80px"
                                    SelectedValue='<%# Bind("Campaign_Duration") %>' OnClientSelectedIndexChanged="function(){onDurationChanged();}">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="3" Value="3" />
                                        <telerik:RadComboBoxItem Text="4" Value="4" />
                                        <telerik:RadComboBoxItem Text="5" Value="5" />
                                        <telerik:RadComboBoxItem Text="6" Value="6" Selected="True" />
                                        <telerik:RadComboBoxItem Text="7" Value="7" />
                                        <telerik:RadComboBoxItem Text="8" Value="8" />
                                        <telerik:RadComboBoxItem Text="9" Value="9" />
                                        <telerik:RadComboBoxItem Text="10" Value="10" />
                                        <telerik:RadComboBoxItem Text="11" Value="11" />
                                        <telerik:RadComboBoxItem Text="12" Value="12" />
                                    </Items>
                                </telerik:RadComboBox>
                            </td>
                            <td class="cell">
                                <b>Campaign End Date</b>
                            </td>
                            <td class="cell last">
                                <asp:TextBox runat="server" ID="txtEndDate" ReadOnly="true"
                                    Text='<%# Eval("End_Date", "{0:d}") %>' />
                            </td>
                        </tr>
                    </table>
                
                                                
                </EditItemTemplate>
            </asp:FormView>   
        </ContentTemplate> 
    </asp:UpdatePanel>
    
    
    <asp:SqlDataSource runat="server" ID="dsCampaignInfo" ConnectionString='<%$ ConnectionStrings:PathfinderClientDB_Format %>'
            SelectCommand="pprx.usp_GetCampaign_Timeline" SelectCommandType="StoredProcedure"
            UpdateCommand="pprx.usp_Campaign_UpdateTimeline" UpdateCommandType="StoredProcedure"
            >
        <SelectParameters>
            <asp:QueryStringParameter Name="id" QueryStringField="id" />
        </SelectParameters>
        <UpdateParameters>
            <asp:SessionParameter SessionField="UserID" Name="UserID" />
        </UpdateParameters>
    </asp:SqlDataSource>
</asp:Content>

 