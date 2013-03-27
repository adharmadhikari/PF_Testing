<%@ Control Language="C#" AutoEventWireup="true" CodeFile="KeyContacts.ascx.cs" Inherits="custom_merz_businessplanning_controls_KeyContacts" %>

  
 <asp:GridView ID="GridviewKC" runat="server" AutoGenerateColumns="False" SkinID="basic" Width="100%">
    <Columns>       
        <asp:BoundField DataField="KC_F_Name" HeaderText="First Name"  ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="firstCol" ItemStyle-CssClass="firstCol"
            ReadOnly="True" SortExpression="KC_F_Name">
        </asp:BoundField>
        <asp:BoundField DataField="KC_L_Name" HeaderText="Last Name" ItemStyle-HorizontalAlign="Left"
            ReadOnly="True" SortExpression="KC_L_Name" >
        </asp:BoundField>       
        <asp:BoundField DataField="KC_Role" HeaderText="Designation" ItemStyle-HorizontalAlign="Left"
            ReadOnly="True" SortExpression="KC_Role" >
        </asp:BoundField>
         <asp:BoundField DataField="KC_Title_Name" HeaderText="Title" ItemStyle-HorizontalAlign="Left"
            ReadOnly="True" SortExpression="KC_Title_Name" >
        </asp:BoundField>
        <asp:BoundField DataField="KC_Phone" HeaderText="Phone" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="80"
            ReadOnly="True" SortExpression="KC_Phone" >
        </asp:BoundField>
        <asp:BoundField DataField="KC_Email" HeaderText="Email" ItemStyle-HorizontalAlign="Left"
            ReadOnly="True" SortExpression="KC_Email">
        </asp:BoundField>
        <asp:BoundField DataField="KC_Admin_Name" HeaderText="Assistant Name" ItemStyle-HorizontalAlign="Left"
            ReadOnly="True" SortExpression="KC_Admin_Name" >
        </asp:BoundField>
        <asp:BoundField DataField="KC_Admin_PH" HeaderText="Assistant Phone" ItemStyle-HorizontalAlign="Left"
            ReadOnly="True" SortExpression="KC_Admin_PH" >
        </asp:BoundField>
    </Columns>
    <EmptyDataTemplate>
        <asp:Label ID="Label1" text="No records present." runat="server"></asp:Label>
    </EmptyDataTemplate>
</asp:GridView>
 <%-- <asp:EntityDataSource ID="AllKCEntity" EntitySetName="KeyContactSearchSet" runat="server" ConnectionString="name=PathfinderClientEntities" DefaultContainerName="PathfinderClientEntities" 
        AutoGenerateWhereClause="true">
         <WhereParameters>       
            <asp:QueryStringParameter QueryStringField="Plan_ID" Name="Plan_ID" Type="Int32" />
        </WhereParameters>
  </asp:EntityDataSource>    --%>
    
