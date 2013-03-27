<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Modal.master" AutoEventWireup="true" CodeFile="removegoal.aspx.cs" Inherits="custom_reckitt_businessplanning_all_RemoveGoal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script type="text/javascript">

     function CloseWin() {
         $closeWindow();
     }
     function RefreshGoalList() {

         window.top.$(".RadWindow iframe").each(function() {
             this.contentWindow.location = this.contentWindow.location;
         });
         window.setTimeout(CloseWin, 4000);
     }    
     
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="title" Runat="Server">
    <asp:Label ID="titleText" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="tools" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="main" Runat="Server">
 <div id="RemoveGoal" runat="server" class="exportConfirmText">
    <asp:FormView runat="server" ID="FormRemoveGoal" DefaultMode="ReadOnly"   
           DataSourceID="dsGoal" CellPadding="0" CellSpacing="0" Width="100%" DataKeyNames="Goal_ID" Visible="false">
        <ItemTemplate>
       
                    <table width="80%" align="center" >
                    <tr>
                        <td class="style1">Goal Description: </td>
                        <td align="left"><%# Eval("Goal_Description")%></td>
                    </tr>
                    
                    <tr align="center">
                        <td width="7%" align="right">
                            <asp:Button ID="Yesbtn" runat="server" Text="Yes" width="70px" Visible="true" OnClick="Yesbtn_Click" />
                        </td>
                        <td width="7%" align="left">
                            <asp:Button ID="Nobtn" width="70px" runat="server" Text="No" OnClientClick="javascript:CloseWin(); return true;"/>
                        </td>

                    </tr>    
                    </table>
                    </div>
         </ItemTemplate> 
         </asp:FormView>
    <asp:FormView runat="server" ID="FormRemoveTactic" DefaultMode="ReadOnly"   
           DataSourceID="dsTactic" CellPadding="0" CellSpacing="0" Width="100%" DataKeyNames="Tactic_ID" Visible="false">
        <ItemTemplate>
        <br />
                    <table width="80%" align="center" >
                    <tr>
                        <td class="style1">Tactic Description: </td>
                        <td align="left"><%# Eval("Tactic_Description")%></td>
                    </tr>
                    
                    <tr align="center">
                        <td width="7%" align="right">
                            <asp:Button ID="YesTacticbtn" runat="server" Text="Yes" width="70px" Visible="true" OnClick="YesTacticbtn" />
                        </td>
                        <td width="7%" align="left">
                            <asp:Button ID="NoTacticbtn" width="70px" runat="server" Text="No" OnClientClick="javascript:CloseWin(); return true;"/>
                        </td>

                    </tr>    
                    </table>
                    </div>
         </ItemTemplate> 
         </asp:FormView>
        </div>
   <asp:EntityDataSource ID="dsGoal" runat="server" 
        ConnectionString="name=PathfinderReckittEntities" 
        DefaultContainerName="PathfinderReckittEntities" 
        ContextTypeName="Pinsonault.Application.Reckitt.PathfinderReckittEntities, Pinsonault.Application.Reckitt" 
        EntitySetName="BusinessPlanGoalSet" Include="Status"      
        AutoGenerateWhereClause="true">
             <WhereParameters>
                <asp:QueryStringParameter QueryStringField="Business_Plan_ID" Name="BusinessPlan.Business_Plan_ID" Type="Int32"/>
                <asp:QueryStringParameter QueryStringField ="Goal_ID" Name="Goal_ID" Type="Int32"/>
            </WhereParameters>
                        
    </asp:EntityDataSource>
     <asp:EntityDataSource ID="dsTactic" runat="server" 
        ConnectionString="name=PathfinderReckittEntities" 
        DefaultContainerName="PathfinderReckittEntities" 
        ContextTypeName="Pinsonault.Application.Reckitt.PathfinderReckittEntities, Pinsonault.Application.Reckitt" 
        EntitySetName="BusinessPlanTacticSet" Include="Status"             
        AutoGenerateWhereClause="true">
             <WhereParameters>
                
                 <asp:QueryStringParameter QueryStringField ="Tactic_ID" Name="Tactic_ID" Type="Int32"/>              
            </WhereParameters>           
    </asp:EntityDataSource>
    <asp:Label ID="Msglbl" runat="server" Text="" Visible="false"></asp:Label>
</asp:Content>

