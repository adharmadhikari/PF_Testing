<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GoalTactics.ascx.cs" Inherits="custom_reckitt_businessplanning_controls_GoalTactics" %>
<%@ Register src="Tactics.ascx" tagname="Tactics" tagprefix="uc1" %>

<div class="leftTile">
 <asp:GridView ID="grvGoals" runat="server" AutoGenerateColumns="False" 
    DataSourceID="dsGoals" Width="100%">   
     <Columns>         
         <asp:TemplateField HeaderText="Goals and Tactics"  
             SortExpression="Goal_Description" HeaderStyle-CssClass="headerList" ItemStyle-CssClass="itemList" ItemStyle-HorizontalAlign="Left" >
             <ItemTemplate>
                 
                 <asp:Label ID="Label1" runat="server" Text='<%# string.Format("Goal Description: {0} ",Eval("Goal_Description")) %>' CssClass="headerList" ></asp:Label><br />
                 
                 <asp:Label ID="lblGoalStatus" runat="server" Text="Status: "></asp:Label>
                 <asp:Label ID="Label2" runat="server" Text='<%# Eval("Goal_Status") %>'></asp:Label><br />
                 
                 <asp:Label ID="lblComplDate" runat="server" Text="Completion Date: "></asp:Label>
                 <asp:Label ID="Label3" runat="server" Text='<%# Eval("Goal_Completion_DT","{0:d}") %>'></asp:Label>
                 
                 <uc1:Tactics ID="Tactics1" runat="server" GoalID='<%# Eval("Goal_ID")%>'/>
             </ItemTemplate>       
         </asp:TemplateField>        
     </Columns>
</asp:GridView>
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
 