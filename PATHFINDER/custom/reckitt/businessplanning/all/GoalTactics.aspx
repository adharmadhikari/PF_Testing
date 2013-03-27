<%@ Page Title="" Language="C#" Theme="pathfinder" MasterPageFile="~/MasterPages/Modal.master" EnableViewState="true" CodeFile="GoalTactics.aspx.cs" Inherits="custom_reckitt_businessplanning_all_GoalTactics" %>
<%@ OutputCache Duration="1" VaryByParam="None" NoStore="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">
    
        
        function gridGoals_rowclick(sender, args) 
         {           
              var hdnGoalID = $get('ctl00_main_hdnGoal_ID');
              hdnGoalID.value = args._dataKeyValues.Goal_ID;       
          }
          function gridTactics_rowclick(sender, args) {
              var hdnTacticID = $get('ctl00_main_hdnTactic_ID');
              hdnTacticID.value = args._dataKeyValues.Tactic_ID;
          }
          function deleteselectedGoal() 
          {
              var bpID = '<%= Request.QueryString["Business_Plan_ID"] %>';
              deleteGoal(bpID);
          }
          function deleteGoal(bpID) {             
              var hdnGoalID = $get('ctl00_main_hdnGoal_ID');

              //Opens popup window to remove the goal
              var str = "removegoal.aspx?Business_Plan_ID=" + bpID + "&Goal_ID=" + hdnGoalID.value + "&linkremove=" + "goal";
              $openWindow(str, null, null, 500, 110,"removegoal");

          }
          function deleteTactic() {
              var bpID = '<%= Request.QueryString["Business_Plan_ID"] %>';
              var hdnTacticID = $get('ctl00_main_hdnTactic_ID');

              //Opens popup window to remove the goal
              var str = "removegoal.aspx?Business_Plan_ID=" + bpID + "&Tactic_ID=" + hdnTacticID.value + "&linkremove=" + "tactic";
              $openWindow(str, null, null, 500, 110, "removegoal");
          }
    </script>
    <style type="text/css">
      .reckitt #divTile2 
      {
         overflow-y: scroll!important;
         overflow-x: scroll!important;
      }
      .genBusinessPlanning 
      {
        width:100% !important;
        text-align: left;  
      }
        .genBusinessPlanning td
        {
            padding:1px 2px;  
            width:33%;     
            height:50%;
            text-align:left;
            cursor:pointer;
            vertical-align: top;
        }
        div.busGoalTactics 
        {                              
            overflow:auto;
            height:150px;
            width:370px;            
        }
        .containerBorder
        {
            border: solid 1px #2d58a7; 
        }
        
        #goalsContainer
        {
            padding-left: 5px;
            padding-top: 5px;
            padding-right: 5px;
        }
        
        #tacticsContainer
        {
            padding-top: 5px;
            padding-right: 5px;
        }
        
        .padContainerTactics
        {
            padding-top: 5px;
            padding-right: 5px;
        }
        
        .padContainerGoals
        {
            padding-left: 5px;
            padding-top: 5px;           
        }
        
        #goalsTacticsContainer .textBox
        {
            border: solid 1px #cccccc;
            padding:2px 4px;
        }
        .leftTile 
        {
	        float:left;
	        width:382px;	       
        }
        .rightTile 
        {
	        float:right;
	        width:380px;	        
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="title" Runat="Server">
    <asp:Literal ID="ltGoalsTactics" runat="server" Text="Goals/Tactics"></asp:Literal>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="tools" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="main" Runat="Server">        
            <asp:HiddenField ID="hdnGoal_ID" runat="server" Value="0" />
            <asp:HiddenField ID="hdnTactic_ID" runat="server" Value="0" />
            
    <div id="goalsTacticsContainer" style="overflow: auto">   
    <div class="leftTile">   
        <%--  GOALS--%>    
        <div id="goalsContainer">
            <div id="Div2" class="areaHeader tileContainerHeader" runat="server">
                <div class="title"><asp:Literal runat="server" ID="Literal2" Text="Goals"/></div>
                <div class="clearAll"></div> 
            </div>
            <div class="containerBorder">
                <div class="modalFormButtons" id="divGoalButtons" runat="server">                   
                    <pinso:CustomButton ID="btnAddGoal" runat="server" Text="Add Goal" onclick="btnAddGoal_Click"/>
                    <pinso:CustomButton ID="btnEditGoal" runat="server" Text="Edit Goal" onclick="btnEditGoal_Click" Visible="false"/>  
                    <pinso:CustomButtonNonServer ID="btnDeleteGoal" runat="server" Text="Delete Goal" onclick="deleteselectedGoal()" Visible="false"/>
                </div> 
                <div class="busGoalTactics">     
                    <telerik:RadGrid SkinID="radTable" runat="server" ID="rgGoals" AutoGenerateColumns="false" DataSourceID="dsGoals" 
                        AllowPaging="false" EnableEmbeddedSkins="false" OnSelectedIndexChanged="rgGoals_SelectedIndexChanged" 
                         AllowAutomaticUpdates="false" AllowAutomaticDeletes="false">
                        <MasterTableView ClientDataKeyNames="Goal_ID" DataKeyNames="Goal_ID">
                            <Columns>                             
                               <%-- <telerik:GridBoundColumn DataField="Goal_ID" HeaderText="Goal #" ReadOnly="True" />--%>
                                <telerik:GridBoundColumn DataField="Goal_Description" HeaderText="Goal Description" ReadOnly="True" />
                                <telerik:GridBoundColumn DataField="Goal_Status" HeaderText="Status" ReadOnly="True" />                          
                                <telerik:GridTemplateColumn HeaderText="Completion Date" ItemStyle-CssClass="alignRight"><ItemTemplate><asp:Label ID="test" runat="server" Text='<%#Eval("Goal_Completion_DT","{0:d}") %>'></asp:Label></ItemTemplate></telerik:GridTemplateColumn>
                            </Columns>                    
                        </MasterTableView>                
                        <ClientSettings Scrolling-AllowScroll="true" Scrolling-UseStaticHeaders="false" Selecting-AllowRowSelect="true" ClientEvents-OnRowSelecting="gridGoals_rowclick" 
                            EnablePostBackOnRowClick="true"/>
                    </telerik:RadGrid>
                </div>  
            </div>
        </div>
    
    
    <%--formview for goal--%>
    <div id="formGoals" runat="server" Visible="false" class="padContainerGoals">
        <div id="Div3" class="areaHeader tileContainerHeader" runat="server">
            <div class="title"><asp:Literal runat="server" ID="litGoal" Text=""/></div>
            <div class="clearAll"></div> 
        </div>
        <div class="containerBorder" style="height: 195px;">
            <asp:FormView ID="frmSectedGoal" runat="server"  DataSourceID="dsGoal" CellPadding="0" CellSpacing="0" Width="100%" 
                  DataKeyNames="Goal_ID" DefaultMode="Insert">
                <EditItemTemplate>
                    <table class="genBusinessPlanning">
                       <%-- <tr>
                            <td><label>Goal #</label></td>
                            <td><asp:label ID="GoalID" runat="server" Text='<%# Eval("Goal_ID")%>'></asp:label></td>                        
                        </tr>--%>
                       <tr>
                            <td width="35%"><label>Goal Description</label></td> 
                            <td width="65%"><asp:TextBox CssClass="textBox" ID="txtGoalDesc" runat="server" Text='<%# Bind("Goal_Description")%>'></asp:TextBox></td>
                             <asp:RequiredFieldValidator ID="reqGoalDesc" runat="server"  ErrorMessage="Invalid Description!" ControlToValidate="txtGoalDesc"></asp:RequiredFieldValidator>
                        </tr>
                        <tr>
                            <td width="35%"><label>Status</label></td> 
                            <td width="65%">                            
                                <pinso:RadiobuttonValueList ID="Goal_Status" runat="server" BorderStyle="None" RepeatDirection="Horizontal" SelectedVal='<%# Eval("Status.Status_ID") %>'>
                                        <asp:ListItem Text="Completed" Value="1"></asp:ListItem> 
                                        <asp:ListItem Text="In-Progress" Value="2"></asp:ListItem> 
                                        <asp:ListItem Text="Not Yet Started" Value="3"></asp:ListItem> 
                                        <asp:ListItem Text="Other" Value="4"></asp:ListItem>                                     
                                </pinso:RadiobuttonValueList>
                            </td>                       
                        </tr>
                        <tr>
                            <td width="35%"><label>If other please specify-</label></td>
                            <td width="65%"><asp:TextBox CssClass="textBox" ID="txtGoalStatusOther" runat="server" Text='<%# Bind("Goal_Status_Other")%>'></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td width="35%"><label>Completion Date</label></td>
                            <td width="65%">
                                <asp:TextBox CssClass="textBox" ID="txtComplDate" runat="server"  Text='<%# Bind("Goal_Completion_DT", "{0:d}")%>' MaxLength="10"></asp:TextBox>                  
                                <ajax:CalendarExtender runat="server" ID="calComplDate" TargetControlID="txtComplDate" />
                                <asp:CompareValidator ID="cvalDate" runat="server"  ErrorMessage="Invalid Date!" ControlToValidate="txtComplDate" 
                                Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                             </td>                        
                        </tr>
                    </table> 
                    <br />
   
                    <div class="modalFormButtons">                        
                         <pinso:CustomButton ID="btnUpdateGoal" runat="server" Text="Save" CommandName="Update" />                              
                    </div>               
                </EditItemTemplate>
                
                <InsertItemTemplate>             
                     
                    <table class="genBusinessPlanning" >
                       
                        <tr>
                            <td width="35%"><label>Goal Description</label></td> 
                            <td width="65%"><asp:TextBox CssClass="textBox" ID="txtGoalDesc" runat="server" Text='<%# Bind("Goal_Description")%>'></asp:TextBox></td>
                            <asp:RequiredFieldValidator ID="reqGoalDesc" runat="server"  ErrorMessage="Invalid Description!" ControlToValidate="txtGoalDesc"></asp:RequiredFieldValidator>
                        </tr>
                        <tr>
                            <td width="35%"><label>Status</label></td> 
                            <td width="65%">
                                <pinso:RadiobuttonValueList ID="Goal_Status" runat="server" BorderStyle="None" RepeatDirection="Horizontal" SelectedVal="3">
                                        <asp:ListItem Text="Completed" Value="1"></asp:ListItem> 
                                        <asp:ListItem Text="In-Progress" Value="2"></asp:ListItem> 
                                        <asp:ListItem Text="Not Yet Started" Value="3"></asp:ListItem> 
                                        <asp:ListItem Text="Other" Value="4"></asp:ListItem>                                     
                                </pinso:RadiobuttonValueList>
                            </td>                       
                        </tr>
                        <tr >
                            <td width="35%"><label>If other please specify-</label></td>
                            <td width="65%"><asp:TextBox CssClass="textBox" ID="txtGoalStatusOther" runat="server" Text='<%# Bind("Goal_Status_Other")%>'></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td width="35%"><label>Completion Date</label></td>
                            <td width="65%">
                                <asp:TextBox CssClass="textBox" ID="txtComplDate" runat="server"  Text='<%# Bind("Goal_Completion_DT", "{0:d}")%>' MaxLength="10"></asp:TextBox>                  
                                <ajax:CalendarExtender runat="server" ID="calComplDate" TargetControlID="txtComplDate" />
                                <asp:CompareValidator ID="cvalDate" runat="server"  ErrorMessage="Invalid Date!" ControlToValidate="txtComplDate" 
                                Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                             </td>                        
                        </tr>
                    </table>
                    <br />
 
                     <div class="modalFormButtons">
                           <pinso:CustomButton ID="btnNewGoal" runat="server" Text="Save" CommandName="Insert"/>                              
                    </div>            
                </InsertItemTemplate>
            </asp:FormView>  
        </div>    
    </div> 
        
    
    
    
    
    </div>
    <div class="rightTile">
    



<%-- TACTICS  --%>
        <div id="tacticsContainer">
            <div id="Div1" class="areaHeader tileContainerHeader" runat="server">
                <div class="title"><asp:Literal runat="server" ID="Literal1" Text="Tactics"/></div>
                <div class="clearAll"></div> 
            </div>
            <div class="containerBorder">
                <div class="modalFormButtons" id="divTacticButton" runat="server">
                    <pinso:CustomButton ID="btnAddTactics" runat="server" Text="Add Tactic" onclick="btnAddTactics_Click"/>
                    <pinso:CustomButton ID="btnEditTactics" runat="server" Text="Edit Tactic" onclick="btnEditTactics_Click" Visible="false"/>   
                    <pinso:CustomButtonNonServer ID="btnDeleteTactic" runat="server" Text="Delete Tactic" onclick="deleteTactic()" Visible="false"/>
                </div> 
                <div class="busGoalTactics">     
                    <telerik:RadGrid runat="server" ID="rgTactics" AutoGenerateColumns="false" SKINid="radTable" EnableEmbeddedSkins="false" 
                            AllowSorting="false" AllowPaging="false" DataSourceID="dsTactics" OnSelectedIndexChanged="rgTactics_SelectedIndexChanged">
                            <MasterTableView ClientDataKeyNames="Tactic_ID,Goal_ID" DataKeyNames="Tactic_ID,Goal_ID">
                                <Columns>                             
                                   <%-- <telerik:GridBoundColumn DataField="Tactic_ID" HeaderText="Tactic #" ReadOnly="True" />--%>
                                    <telerik:GridBoundColumn DataField="Tactic_Description" HeaderText="Tactic Description" ReadOnly="True" />
                                    <telerik:GridBoundColumn DataField="Tactic_Status" HeaderText="Status" ReadOnly="True" />                                
                                    <telerik:GridBoundColumn dataField="Tactic_Completion_Date" HeaderText="Completion Date" ReadOnly="True" DataFormatString="{0:d}" DataType="System.DateTime" ItemStyle-CssClass="alignRight" /> 
                                </Columns>
                            </MasterTableView>
                             <ClientSettings Scrolling-AllowScroll="true" Scrolling-UseStaticHeaders="false" Selecting-AllowRowSelect="true" ClientEvents-OnRowSelecting="gridTactics_rowclick" 
                            EnablePostBackOnRowClick="true"/>                       
                    </telerik:RadGrid>
                </div>   
            </div>
        </div>


<div id="formTactics" runat="server" Visible="false" class="padContainerTactics">
    <%--formview for tactic--%>   
        <div id="Div4" class="areaHeader tileContainerHeader" runat="server">
            <div class="title"><asp:Literal runat="server" ID="litTactic" Text=""/></div>
            <div class="clearAll"></div> 
        </div>
        <div class="containerBorder" style="height: 195px;">
            <asp:FormView ID="frmTactics" runat="server"  DataSourceID="dsTactic" CellPadding="0" CellSpacing="0" Width="100%" 
                DefaultMode="Insert" DataKeyNames="Tactic_ID">
                <EditItemTemplate>
                    <table class="genBusinessPlanning">
                       <%-- <tr>
                            <td><label>Tactics #</label></td>
                            <td><asp:label ID="TacticID" runat="server" Text='<%# Eval("Tactic_ID")%>'></asp:label></td>                        
                        </tr>--%>
                       <tr>
                            <td><label>Tactic Description</label></td> 
                            <td><asp:TextBox CssClass="textBox" ID="txtTacticDesc" runat="server" Text='<%# Bind("Tactic_Description")%>'></asp:TextBox></td>
                            <asp:RequiredFieldValidator ID="reqTacticDesc" runat="server"  ErrorMessage="Invalid Description!" ControlToValidate="txtTacticDesc"></asp:RequiredFieldValidator>
                        </tr>
                        <tr>
                            <td><label>Status</label></td> 
                            <td>                                
                                <pinso:RadiobuttonValueList ID="Tactic_Status" runat="server" BorderStyle="None" RepeatDirection="Horizontal" SelectedVal='<%# Eval("Status.Status_ID") %>'>
                                    <asp:ListItem Text="Completed" Value="1"></asp:ListItem> 
                                    <asp:ListItem Text="In-Progress" Value="2"></asp:ListItem> 
                                    <asp:ListItem Text="Not Yet Started" Value="3"></asp:ListItem> 
                                    <asp:ListItem Text="Other" Value="4"></asp:ListItem>                                     
                                </pinso:RadiobuttonValueList>                           
                            </td>                       
                        </tr>
                        <tr>
                            <td><label>If other please specify-</label></td>
                            <td><asp:TextBox ID="txtTacticsStatusOther" runat="server" CssClass="textBox" Text='<%# Bind("Tactic_Status_Other")%>'></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td><label>Completion Date</label></td>
                            <td>
                                <asp:TextBox ID="txtTacticsComplDate" runat="server"  Text='<%# Bind("Tactic_Completion_Date","{0:d}")%>' MaxLength="10"></asp:TextBox>                  
                                <ajax:CalendarExtender runat="server" ID="calTacticsComplDate" TargetControlID="txtTacticsComplDate" />
                                <asp:CompareValidator ID="cvalTacticsDate" runat="server"  ErrorMessage="Invalid Date!" ControlToValidate="txtTacticsComplDate" 
                                Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                             </td>                        
                        </tr>
                    </table> 
                    <br />

                    <div class="modalFormButtons">
                        <pinso:CustomButton ID="btnUpdateTactic" runat="server" Text="Save" CommandName="Update"/>                              
                    </div>               
                </EditItemTemplate>
                
                <InsertItemTemplate>             
                     
                    <table class="genBusinessPlanning">
                        
                        <tr>
                            <td><label>Tactic Description</label></td> 
                            <td><asp:TextBox ID="txtTacticsDesc" runat="server" Text='<%# Bind("Tactic_Description")%>' CssClass="textBox"></asp:TextBox></td>
                            <asp:RequiredFieldValidator ID="reqTacticDesc" runat="server"  ErrorMessage="Invalid Description!" ControlToValidate="txtTacticsDesc"></asp:RequiredFieldValidator>
                        </tr>
                        <tr>
                            <td><label>Status</label></td> 
                            <td>                               
                                 <pinso:RadiobuttonValueList ID="Tactic_Status" runat="server" BorderStyle="None" RepeatDirection="Horizontal" SelectedVal="3">
                                    <asp:ListItem Text="Completed" Value="1"></asp:ListItem> 
                                    <asp:ListItem Text="In-Progress" Value="2"></asp:ListItem> 
                                    <asp:ListItem Text="Not Yet Started" Value="3"></asp:ListItem> 
                                    <asp:ListItem Text="Other" Value="4"></asp:ListItem>                                     
                                </pinso:RadiobuttonValueList>
                            </td>                       
                        </tr>
                        <tr>
                            <td><label>If other please specify-</label></td>
                            <td><asp:TextBox ID="txtTacticsStatusOther" runat="server" Text='<%# Bind("Tactic_Status_Other")%>'></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td><label>Completion Date</label></td>
                            <td>
                                <asp:TextBox ID="txtTacticsComplDate" runat="server"  Text='<%# Bind("Tactic_Completion_Date","{0:d}")%>' CssClass="textBox" MaxLength="10"></asp:TextBox>                  
                                <ajax:CalendarExtender runat="server" ID="calTacticsComplDate" TargetControlID="txtTacticsComplDate" />
                                <asp:CompareValidator ID="cvalTacticsDate" runat="server"  ErrorMessage="Invalid Date!" ControlToValidate="txtTacticsComplDate" 
                                Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                             </td>                        
                        </tr>
                    </table>
                    <br />

                     <div class="modalFormButtons">
                          <pinso:CustomButton ID="btnNewTactics" runat="server" Text="Save" CommandName="Insert"/>                               
                    </div>            
                </InsertItemTemplate>
                </asp:FormView>  
            </div>   
        </div>
    



    </div>
    <div class="clearAll"></div>
    </div>
    <asp:EntityDataSource ID="dsGoals" runat="server" 
        ConnectionString="name=PathfinderReckittEntities" 
        DefaultContainerName="PathfinderReckittEntities" 
        EntitySetName="BusinessPlanGoalsViewSet" 
        AutoGenerateWhereClause="true">
             <WhereParameters>
                <asp:QueryStringParameter QueryStringField="Business_Plan_ID" Name="Business_Plan_ID" Type="Int32"/>
            </WhereParameters>            
    </asp:EntityDataSource>
    
    <asp:EntityDataSource ID="dsGoal" runat="server" 
        ConnectionString="name=PathfinderReckittEntities" 
        DefaultContainerName="PathfinderReckittEntities" 
        ContextTypeName="Pinsonault.Application.Reckitt.PathfinderReckittEntities, Pinsonault.Application.Reckitt" 
        EntitySetName="BusinessPlanGoalSet" Include="Status"
        EnableInsert="true" OnInserting="AddGoal" 
        EnableUpdate="true" OnUpdating="UpdateGoal" 
        AutoGenerateWhereClause="true">
             <WhereParameters>
                <asp:QueryStringParameter QueryStringField="Business_Plan_ID" Name="BusinessPlan.Business_Plan_ID" Type="Int32"/>
                <asp:ControlParameter ControlID ="hdnGoal_ID" Name="Goal_ID" Type="Int32"/>
            </WhereParameters>
                        
    </asp:EntityDataSource>
    
    <asp:EntityDataSource ID="dsTactics" runat="server" 
        ConnectionString="name=PathfinderReckittEntities" 
        DefaultContainerName="PathfinderReckittEntities" 
        EntitySetName="BusinessPlanTacticsViewSet" AutoGenerateWhereClause="true">
             <WhereParameters>                
                <asp:ControlParameter ControlID ="hdnGoal_ID" Name="Goal_ID" Type="Int32" />                 
            </WhereParameters>
    </asp:EntityDataSource>
    
     <asp:EntityDataSource ID="dsTactic" runat="server" 
        ConnectionString="name=PathfinderReckittEntities" 
        DefaultContainerName="PathfinderReckittEntities" 
        ContextTypeName="Pinsonault.Application.Reckitt.PathfinderReckittEntities, Pinsonault.Application.Reckitt" 
        EntitySetName="BusinessPlanTacticSet" Include="Status"
        EnableInsert="true" OnInserting="AddTactic" 
        EnableUpdate="true" OnUpdating="UpdateTactic"        
        AutoGenerateWhereClause="true">
             <WhereParameters>
                <asp:ControlParameter ControlID ="hdnGoal_ID" Name="BusinessPlanGoal.Goal_ID" Type="Int32"/>
                <asp:ControlParameter ControlID ="hdnTactic_ID" Name="Tactic_ID" Type="Int32"/>
            </WhereParameters>           
    </asp:EntityDataSource>
    <pinso:RadGridWrapper runat="server" ID="radGridWrapper" Target="rgTactics" AutoUpdate="true" RequiresFilter="true" DrillDownLevel="1"  UtcDateColumns="Tactic_Completion_Date"/>

   <telerik:radwindowmanager EnableEmbeddedSkins="false" Skin="pathfinder" id="RadWindowManager1" runat="server" DestroyOnClose="true" Modal="true" 
           Behaviors="Close" VisibleTitlebar="false">    
    </telerik:radwindowmanager>  

</asp:Content>

