<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Tactics.ascx.cs" Inherits="custom_reckitt_businessplanning_controls_Tactics" %>

<asp:HiddenField ID = "hdnGoalID" runat="server" Value='<%# GoalID %>' />

<asp:GridView ID="grvTactics" runat="server" AutoGenerateColumns="False" 
    DataSourceID="dsTactics" Width="100%" HeaderStyle-CssClass="headerList">   
    <Columns>       
        <asp:BoundField DataField="Tactic_Description" HeaderText="Tactic_Description" 
            ReadOnly="True" ItemStyle-CssClass="itemList"/>
        <asp:BoundField DataField="Tactic_Status" HeaderText="Tactic_Status" 
            ReadOnly="True" ItemStyle-CssClass="itemList"/>
        <asp:BoundField DataField="Tactic_Status_Other" 
            HeaderText="Status (Other)" ReadOnly="True" 
            ItemStyle-CssClass="itemList"/>
        <asp:BoundField DataField="Tactic_Completion_Date" 
            HeaderText="Completion Date" ReadOnly="True" 
            DataFormatString="{0:d}" ItemStyle-CssClass="alignRight"/>
    </Columns>
    <EmptyDataTemplate>No Tactics found.</EmptyDataTemplate>
</asp:GridView>

<asp:EntityDataSource ID="dsTactics" runat="server" 
    ConnectionString="name=PathfinderReckittEntities" 
    DefaultContainerName="PathfinderReckittEntities" 
    EntitySetName="BusinessPlanTacticsViewSet" AutoGenerateWhereClause="true" >
    <WhereParameters>
        <asp:ControlParameter ControlID="hdnGoalID" Name="Goal_ID" Type="Int32" />        
    </WhereParameters>
   
</asp:EntityDataSource>