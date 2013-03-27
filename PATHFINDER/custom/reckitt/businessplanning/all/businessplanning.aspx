<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SingleSection.master" AutoEventWireup="true" CodeFile="businessplanning.aspx.cs" Inherits="custom_reckitt_businessplanning_all_businessplanning"  %>
<%@ Register src="~/todaysaccounts/controls/PlanInfoAddress.ascx" tagname="PlanInfoAddress" tagprefix="pinso" %>
<%@ Register src="~/todaysaccounts/controls/CoveredLives.ascx"  tagname="CoveredLives" tagprefix="pinso" %>
<%@ Register src="~/todaysaccounts/controls/CoveredLivesSM.ascx" tagname="CoveredLivesSM" tagprefix="pinso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="scriptContainer" Runat="Server">
    <link runat="server" href="custom/reckitt/content/styles/reckitt.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        /*.reckitt #divTile3 
        {
         overflow-y: scroll!important;
         overflow-x: scroll!important;
        }*/
        .genBusinessPlanning 
        {
            width:100% !important;
            text-align: left;  
        }
         .genBusinessPlanning td
        {             
            padding:2px 2px 5px 2px;  
            text-align:left;
            cursor:pointer;
            vertical-align: top;
        }
          .genBP_AccountSummaryTable
        {
            width:100% !important;
            text-align: left; 
        }
        .genBP_AccountSummaryTable td
        {
            padding:5px 2px 5px 2px;
            text-align:left;
            cursor:pointer;
            vertical-align: top;
        }    
    </style>

 <script type="text/javascript">
    

    </script>     
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Tile3Title" Runat="Server">
    <asp:Literal runat="server" ID="Literal1" Text='<%$ Resources:Resource, SectionTitle_BusinessPlanning %>' />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Tile3Tools" Runat="Server">
    <div id="divButtons" runat="server" style="padding-top: 2px">
        <a id="Save" href="javascript:UpdateData()" runat="server" style='margin-right:15px'>Save</a> 
        <span> </span><a id="Edit" href ="javascript:EditLinkClick()" runat="server" style='margin-right:15px'>Edit</a>   
        <span> </span><a href ="javascript:OpenGoalTactics('ctl00_Tile3_hdnBPID')" id="btnGoals" runat="server" style='margin-right:15px'>Goal/Tactics</a>         
        <span> </span><a href="javascript:exportPDF('<%= Request.QueryString["Plan_ID"] %>','<%= Request.QueryString["Segment_ID"] %>','ctl00_Tile3_hdnBPID')" style='margin-right:15px'>PDF</a>  
    </div> 
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Tile3" Runat="Server">
    <asp:HiddenField ID="hdnBPID" runat="server" Value="" />
    <asp:HiddenField ID="hdnURL" runat="server" Value="" />
<%--    <table class="genBusinessPlanning">
    <tr>
    <td> --%>
    <div class="topSection">
        <div class="busPlanTriSectionLeft">
            <div class="busPlanTriSection1" id="tile9">
                <div id="divTile9Container" class="busPlanAddress">   
                    <div id="tile9ContainerHeader" class="tileContainerHeader">
                        <div class="title">
                            <asp:Literal runat="server" ID="Literal3" Text='<%$ Resources:Resource, SectionTitle_PlanInfoAddress %>' />
                        </div>
                        <div class="pagination">
                        </div>
                        <div class="tools">
                        </div>
                        <div class="clearAll"></div>
                    </div>
                    <div id="tile9Content">
                        <pinso:PlanInfoAddress ID="PlanInfoAddress" runat="server" ShowSectionDisclaimer="false" />
                    </div>
                </div>
            </div>
    <%--    </td>
        <td>  --%>
            <div class="busPlanTriSection2" id="tile8">
                <div id="divTile8Container" class="busPlanCoveredLives">   
                    <div id="tile8ContainerHeader" class="tileContainerHeader">
                        <div class="title">
                            <asp:Literal runat="server" ID="coveredLivesHdr" Text='<%$ Resources:Resource, SectionTitle_CoveredLives %>' />
                        </div>
                        <div class="pagination">
                        </div>
                        <div class="tools">
                        </div>
                        <div class="clearAll"></div>
                    </div>
                    <div id="tile8Content">
                        <asp:Panel ID="pnlCoveredLives" runat="server" Visible="true">
                            <pinso:CoveredLives runat="server" ID="coveredLives" ShowSectionDisclaimer="false" />
                        </asp:Panel>
                        <asp:Panel ID="pnlStateMedicaid" runat="server" Visible="false">
                            <pinso:CoveredLivesSM runat="server" ID="coveredLivesSM" ShowSectionDisclaimer="false" />
                        </asp:Panel>
                    </div>
                </div>
            </div> 
        </div>       
    <%--     </td>
        <td>--%>
        <div class="busPlanTriSectionRight">
            <div id="divTile7Container" class="busPlanKeyContacts">
                <div id="divTile7ContainerHeader" class="tileContainerHeader">
                    <div class="title"><asp:Literal runat="server" ID="Literal4" Text='<%$ Resources:Resource, SectionTitle_KeyContacts %>' /></div>
                    <div class="pagination" style="float:right"></div>
                    <div class="clearAll"></div>
                </div>
                <div id="kcView">                   
                      <asp:GridView ID="grvKeyContacts" CssClass="keyContactsTable" runat="server" AutoGenerateColumns="False" GridLines="Both" 
                            DataSourceID="dsBP_KeyContacts" DataKeyNames="Business_Plan_ID"   >
                            <Columns>
                                <asp:BoundField DataField="KC_F_Name" HeaderText="First Name" ReadOnly="True" />
                                <asp:BoundField DataField="KC_L_Name" HeaderText="Last Name" ReadOnly="True" />
                                <asp:BoundField DataField="KC_Title_Name" HeaderText="Designation" ReadOnly="True" />
                                <asp:BoundField DataField="KC_Email" HeaderText="E-Mail" ReadOnly="True" />
                                <asp:BoundField DataField="KC_Phone" HeaderText="Phone" ReadOnly="True" />
                                <asp:TemplateField HeaderText="Select">                 
                                     <ItemTemplate>
                                        <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# (Eval("Business_Plan_ID") != null) %>' Enabled="false" />                            
                                        <asp:Label ID="lblKCID" runat="server" text='<%# Eval("KC_ID")%>' visible="false" />
                                        <asp:Label ID="lblKCTypeID" runat="server" text='<%# Eval("KC_Type_ID")%>' visible="false" />
                                     </ItemTemplate>                                                             
                                </asp:TemplateField>        
                            </Columns>
                        </asp:GridView>
                        <asp:LinkButton ID="linkEditKC" runat="server" OnClick ="EditBusinessPlan" style="display: none;" CssClass="postback validate linkEditBusinessPlan"></asp:LinkButton> 
                        <asp:LinkButton ID="linkUpdateKC" runat="server" OnClick="UpdateKeyContacts" CssClass="postback validate linkUpdateAccountSummary" Text="Update" style="display: none;"></asp:LinkButton>                          
                </div>
            </div>
        </div>
    </div>
<%--    </td>
    </tr>
    <tr>
    <td colspan="3">--%>
    <div class="clearAll" />
    <div class="bottomSection">
<%--        <div class="popupArea" width="80%">--%>
            <div id="divTile6Container" class="busPlanAccountSummary">
                <div id="divTile6ContainerHeader" class="tileContainerHeader">
                    <div class="title"><asp:Literal runat="server" ID="Literal2" Text="Account Summary Statement"/></div>
                    <div class="pagination" style="float:right"></div>
                    <div class="clearAll"></div>
                </div>
                <div id="divTile6">
                    <asp:FormView  runat="server" ID="frmBPAccountSummary" DefaultMode="ReadOnly" DataKeyNames="Plan_ID,Business_Plan_ID"    
                        DataSourceID="dsBusinessPlans" CellPadding="0" CellSpacing="0" Width="100%" valign="top">
                        <ItemTemplate>
                            <table cellpadding="0"  cellspacing="0" class="genBP_AccountSummaryTable">
                                <tr>
                                    <td colspan="4" class="borderBottom">
                                        <b><asp:Label ID="Acct_Summary" runat="server" 
                                            text='<%# ConvertLineBreaks(Convert.ToString(Eval("Acct_Summary")))%>' Width="100%"></b></asp:Label>
                                        </td>
                                </tr>
                                <tr>
                                    <td width="35%" class="borderBottom">
                                        <b>Does the Organization offer OTC coverage?</b>
                                    </td>
                                    <td width="15%" class="borderBottom">                       
                                        <%# ConvertOTC_DBValues(Convert.ToString(Eval("OTC_Coverage_YN"))) %>                                     
                                    </td>
                                    <td width="35%" class="borderLeft">
                                        <b>Is P &amp; T review required?</b></td>
                                    <td width="15%">                        
                                         <%# ConvertOTC_DBValues(Convert.ToString(Eval("PT_Review_YN"))) %>                                    
                                    </td>
                                </tr>
                                <tr>
                                    <td width="35%" class="borderBottom">
                                        <b>Opportunities exist to participate in OTC patient and/or provider educational 
                                        programs? </b></td>
                                    <td width="15%" class="borderBottom">          
                                        <%# ConvertOTC_DBValues(Convert.ToString(Eval("OTC_Opportunities_YN")))%>
                                    </td>
                                    <td width="35%" class="borderBottom borderLeft"></td>
                                    <td width="15%" class="borderBottom"> 
                                        <asp:Label ID="NO_PT_Explanation" runat="server" MaxLength="50" 
                                            Text='<%# Eval("NO_PT_Explanation") %>' ></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="3" width="35%" class="borderBottom">
                                        <b>Opportunities</b></td>
                                    <td rowspan="2" width="15%">
                                         <div>
                                             <%# ShowCheckBoxValue(Convert.ToBoolean(Eval("O_Cough_Cold_Kits_YN")),"Cough/Cold Kits")%> <br /><br />
                                             <%# ShowCheckBoxValue(Convert.ToBoolean(Eval("O_Health_Fairs_YN")), "Health Fairs")%> <br /><br />
                                             <%# ShowCheckBoxValue(Convert.ToBoolean(Eval("O_Education_Brochures_YN")),"Educational Brochures")%> <br /><br />
                                             <%# ShowCheckBoxValue(Convert.ToBoolean(Eval("O_Other_YN")), "Other")%> <br /><br />
                                         </div>                                        
                                    </td>
                                    <td rowspan="2" width="35%" class="borderBottom borderLeft">
                                        <b>How quickly can a new product be scheduled?</b></td>
                                    <td width="15%">                        
                                        <%# ConvertSchedulePeriodValues(Convert.ToString(Eval("Product_Review_Period")))%>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="15%" class="borderBottom">
                                        <asp:Label ID="NA_Schedule_Period" runat="server" MaxLength="50" 
                                            Text='<%# Eval("NA_Schedule_Period") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="15%" class="borderBottom">
                                        <asp:Label ID="O_Other_Explanation" runat="server" MaxLength="50" 
                                            Text='<%# Eval("O_Other_Explanation") %>'></asp:Label>
                                    </td>
                                    <td width="35%" class="borderBottom borderLeft"> 
                                        <b>Can the time frame be reduced for review?</b></td>
                                    <td width="15%" class="borderBottom">
                                        <%# ConvertOTC_DBValues(Convert.ToString(Eval("Timeframe_Reduced_YN"))) %>
                                    </td>
                                </tr>
                            </table>
                            <asp:LinkButton ID="linkEditBusinessPlan" runat="server" OnClick ="EditBusinessPlan" style="display: none;" CssClass="postback validate linkEditBusinessPlan"></asp:LinkButton> 
                        </ItemTemplate> 
                        <EditItemTemplate>
                            <table cellpadding="0"  cellspacing="0" class="genBP_AccountSummaryTable" >
                                <tr>
                                    <td colspan="4" class="borderBottom">
                                        <asp:TextBox ID="Acct_Summary" runat="server" Columns = "150"  Rows="4"
                                            MaxLength="500" TextMode="MultiLine" text='<%# Bind("Acct_Summary")%>' Width="99%" ToolTip="Account summary statement" ></asp:TextBox>
                                         <pinso:ClientValidator ID="cvAcctSummary" Target="Acct_Summary" Text ="Maximum 500 characters allowed for Account Summary." Required="false" runat="server" DataType="String" MaxLength="500"  />  
                                        </td>
                                </tr>
                                <tr>
                                    <td width="35%" class="borderBottom">
                                        <b>Does the Organization offer OTC coverage?</b></td>
                                    <td width="15%" class="borderBottom">                       
                                        <pinso:RadiobuttonValueList ID="OTC_Coverage_YN" runat="server" BorderStyle="None" RepeatDirection="Horizontal" SelectedVal='<%# Bind("OTC_Coverage_YN") %>'>
                                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem> 
                                            <asp:ListItem Text="No" Value="2"></asp:ListItem> 
                                            <asp:ListItem Text="Not Available" Value="0" Selected="True"></asp:ListItem> 
                                        </pinso:RadiobuttonValueList>
                                    </td>
                                    <td width="35%" class="borderLeft">
                                        <b>Is P &amp; T review required?</b></td>
                                    <td width="15%">                        
                                         <pinso:RadiobuttonValueList ID="PT_Review_YN" runat="server" BorderStyle="None" RepeatDirection="Horizontal" SelectedVal='<%# Bind("PT_Review_YN") %>'>
                                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem> 
                                            <asp:ListItem Text="No" Value="2"></asp:ListItem> 
                                            <asp:ListItem Text="Not Available" Value="0" Selected="True"></asp:ListItem> 
                                        </pinso:RadiobuttonValueList>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="35%" class="borderBottom">
                                        <b>Do Opportunities exist to participate in OTC patient and/or provider educational 
                                        programs?</b> </td>
                                    <td width="15%" class="borderBottom">          
                                        <pinso:RadiobuttonValueList ID="OTC_Opportunities_YN" runat="server" BorderStyle="None" RepeatDirection="Horizontal" SelectedVal='<%# Bind("OTC_Opportunities_YN") %>'>
                                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem> 
                                            <asp:ListItem Text="No" Value="2"></asp:ListItem> 
                                            <asp:ListItem Text="Not Available" Value="0" Selected="True"></asp:ListItem> 
                                        </pinso:RadiobuttonValueList>
                                    </td>
                                    <td width="35%" class="borderBottom borderLeft"></td>
                                    <td width="15%" class="borderBottom">
                                        <asp:TextBox ID="NO_PT_Explanation" runat="server" MaxLength="50" 
                                            TextMode="MultiLine" Text='<%# Bind("NO_PT_Explanation") %>' Width="95%" ToolTip="Explanation for P&T review not required." ></asp:TextBox>
                                        <pinso:ClientValidator ID="ClientValidator1" Target="NO_PT_Explanation" Text ="Maximum 50 characters allowed." Required="false" runat="server" DataType="String" MaxLength="50"  />  
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="3" width="35%" class="borderBottom">
                                        <b>Opportunities</b></td>
                                    <td rowspan="2" width="15%">
                                       <asp:CheckBox id="O_Cough_Cold_Kits_YN" runat="server" Text="Cough/Cold Kits" Checked='<%# Bind("O_Cough_Cold_Kits_YN") %>' /><br />
                                        <asp:CheckBox id="O_Health_Fairs_YN" runat="server" Text="Health Fairs" Checked='<%# Bind("O_Health_Fairs_YN") %>' /><br />
                                        <asp:CheckBox id="O_Education_Brochures_YN" runat="server" Text="Educational Brochures" Checked='<%# Bind("O_Education_Brochures_YN") %>' /><br />
                                        <asp:CheckBox id="O_Other_YN" runat="server" Text="Other" Checked='<%# Bind("O_Other_YN") %>' /><br />
                                    </td>
                                    <td rowspan="2" width="35%" class="borderBottom borderLeft">
                                        <b>How quickly can a new product be scheduled?</b></td>
                                    <td width="15%">                        
                                         <pinso:RadiobuttonValueList ID="Product_Review_Period" runat="server" BorderStyle="None" RepeatDirection="Horizontal" SelectedVal='<%# Bind("Product_Review_Period") %>'>
                                            <asp:ListItem Value="1" Text="0-1 months"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="2-3 months"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="4-6 months"></asp:ListItem>
                                            <asp:ListItem Value="4" Text="7-12 months"></asp:ListItem>
                                            <asp:ListItem Value="5" Text="Not Available" Selected="True"></asp:ListItem>
                                        </pinso:RadiobuttonValueList>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="15%" class="borderBottom">
                                        <asp:TextBox ID="NA_Schedule_Period" runat="server" MaxLength="50" 
                                            TextMode="MultiLine" Text='<%# Bind("NA_Schedule_Period") %>' Width="95%" ToolTip="comments for NA product schedule period." ></asp:TextBox>
                                        <pinso:ClientValidator ID="ClientValidator2" Target="NA_Schedule_Period" Text ="Maximum 50 characters allowed." Required="false" runat="server" DataType="String" MaxLength="50"  />  
                                    </td>
                                </tr>
                                <tr>
                                    <td width="15%" class="borderBottom">
                                        <asp:TextBox ID="O_Other_Explanation" runat="server" MaxLength="50" 
                                            TextMode="MultiLine" Text='<%# Bind("O_Other_Explanation") %>' Width="95%" ToolTip="Explanation for other opportunities." ></asp:TextBox>
                                        <pinso:ClientValidator ID="ClientValidator3" Target="O_Other_Explanation" Text ="Maximum 50 characters allowed." Required="false" runat="server" DataType="String" MaxLength="50"  />  
                                    </td>
                                    <td width="35%" class="borderBottom borderLeft">
                                        <b>Can the time frame be reduced for review?</b></td>
                                    <td width="15%" class="borderBottom">
                                         <pinso:RadiobuttonValueList ID="Timeframe_Reduced_YN" runat="server" BorderStyle="None"  RepeatDirection="Horizontal" SelectedVal='<%# Bind("Timeframe_Reduced_YN") %>'>
                                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem> 
                                            <asp:ListItem Text="No" Value="2"></asp:ListItem> 
                                            <asp:ListItem Text="Not Available" Value="0" Selected="True"></asp:ListItem> 
                                        </pinso:RadiobuttonValueList>
                                    </td>
                                </tr>
                            </table>
                            <asp:LinkButton ID="linkUpdateAccountSummary" runat="server" CommandName="Update" CssClass="postback validate linkUpdateAccountSummary" Text="Update" style="display: none;"></asp:LinkButton> 
                        </EditItemTemplate> 
                        <EmptyDataTemplate>No Records present.</EmptyDataTemplate>
                    </asp:FormView> 
                </div>          
            </div> 
<%--        </div>--%>
    </div>
<%--    </td>
    </tr> 
    </table>--%>  
    <asp:EntityDataSource ID="dsBusinessPlans" runat="server" 
            ConnectionString="name=PathfinderReckittEntities" 
            DefaultContainerName="PathfinderReckittEntities" EnableUpdate="true" OnUpdating="UpdateAccountSummary" 
            ContextTypeName="Pinsonault.Application.Reckitt.PathfinderReckittEntities, Pinsonault.Application.Reckitt" 
            EntitySetName="BusinessPlanSet" AutoGenerateWhereClause="true">
        <WhereParameters>                
                   <asp:QueryStringParameter QueryStringField="Plan_ID" Name="Plan_ID" Type="Int32"/> 
        </WhereParameters>               
</asp:EntityDataSource>

<asp:EntityDataSource ID="dsBP_KeyContacts" runat="server" 
    ConnectionString="name=PathfinderReckittEntities" 
    DefaultContainerName="PathfinderReckittEntities" 
    EntitySetName="BusinessPlanningKeyContactsSet" OnSelecting="OnSelectingContacts" 
    AutoGenerateWhereClause="true" OrderBy="it.Business_Plan_ID desc">
         <WhereParameters>
            <asp:QueryStringParameter QueryStringField="Plan_ID" Name="Plan_ID" Type="Int32"/>
        </WhereParameters>
</asp:EntityDataSource>
</asp:Content>
