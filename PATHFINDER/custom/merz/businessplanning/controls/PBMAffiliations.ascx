<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PBMAffiliations.ascx.cs" Inherits="custom_merz_businessplanning_controls_PBMAffiliations" %>
 <asp:GridView ID="grdvwPBMAffiliations" runat="server" AutoGenerateColumns="False" DataSourceID="dsPBMAffiliations"
    Width="100%" GridLines="none" BorderStyle="None" CssClass="staticTable">
    <Columns>
        <asp:BoundField DataField="Plan_Name" HeaderText="PBM Name" Visible="true"  HeaderStyle-CssClass="firstCol firstGeneric" ItemStyle-CssClass="firstCol firstGeneric" />
        <asp:BoundField DataField="PBM_Function" HeaderText="PBM Function" Visible="true" HeaderStyle-CssClass="firstGeneric" ItemStyle-CssClass="firstGeneric" />
    </Columns>
    <EmptyDataTemplate>
            <asp:Label ID="Label1" text="No records present for PBM." runat="server"></asp:Label>
    </EmptyDataTemplate>
</asp:GridView>
<asp:EntityDataSource ID="dsPBMAffiliations" runat="server" ConnectionString="name=PathfinderMerzEntities"
    DefaultContainerName="PathfinderMerzEntities" EntitySetName="PlanAffiliationsForPBMSet"
    AutoGenerateWhereClause="true" OrderBy="it.[Plan_Name]">
    <WhereParameters>
        <asp:QueryStringParameter QueryStringField="Plan_ID" Name="Child_ID" Type="Int32" ConvertEmptyStringToNull="true"/>
    </WhereParameters>            
</asp:EntityDataSource>