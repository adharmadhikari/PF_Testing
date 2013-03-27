<%@ Control Language="C#" AutoEventWireup="true" CodeFile="KeyContacts.ascx.cs" Inherits="custom_reckitt_businessplanning_controls_KeyContacts" %>
<div id="kcView">
       <asp:GridView ID="grvKeyContacts" runat="server" AutoGenerateColumns="False" GridLines="Both" Width="100%"
        DataSourceID="dsBP_KeyContacts">
        <Columns>
            <asp:BoundField DataField="KC_F_Name" HeaderText="First Name" ReadOnly="True" />
            <asp:BoundField DataField="KC_L_Name" HeaderText="Last Name" ReadOnly="True" />
            <asp:BoundField DataField="KC_Title_Name" HeaderText="Designation" ReadOnly="True" />
            <asp:BoundField DataField="KC_Email" HeaderText="E-Mail" ReadOnly="True" />
            <asp:BoundField DataField="KC_Phone" HeaderText="Phone" ReadOnly="True" />                  
        </Columns>
        <EmptyDataTemplate>No records present.</EmptyDataTemplate>
        </asp:GridView>        
</div>

<asp:EntityDataSource ID="dsBP_KeyContacts" runat="server" 
    ConnectionString="name=PathfinderReckittEntities" 
    DefaultContainerName="PathfinderReckittEntities" 
    EntitySetName="BusinessPlanningKeyContactsSet"
    AutoGenerateWhereClause="true">
         <WhereParameters>
            <asp:QueryStringParameter QueryStringField="Plan_ID" Name="Plan_ID" Type="Int32"/>
            <asp:QueryStringParameter QueryStringField="Business_Plan_ID" Name="Business_Plan_ID" Type="Int32"/>
        </WhereParameters>
</asp:EntityDataSource>
