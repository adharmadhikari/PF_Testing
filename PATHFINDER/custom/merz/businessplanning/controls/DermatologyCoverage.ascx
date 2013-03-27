%@ Control Language="C#" AutoEventWireup="true" CodeFile="DermatologyCoverage.ascx.cs" Inherits="custom_merz_businessplanning_controls_DermatologyCoverage" %>
 <asp:GridView ID="grdvwDermCoverageRead" runat="server" AutoGenerateColumns="False" DataSourceID="dsPlanDrugFormulary"
    Width="100%" GridLines="Both" BorderStyle="None" CssClass="staticTable" DataKeyNames="Product_ID" >
    <Columns>
        <asp:BoundField DataField="Drug_Name" HeaderText="Drug Name" Visible="true"  HeaderStyle-CssClass="firstCol" ItemStyle-CssClass="firstCol"/>
        <asp:BoundField DataField="Benefit_Plan_Product" HeaderText="Benefit/Plan Product" Visible="true" />
        <asp:BoundField DataField="Formulary_Lives" HeaderText="Covered Lives" Visible="true" DataFormatString="{0:n0}"  ItemStyle-CssClass="alignRight" />
        <asp:BoundField DataField="Percentage_Lives" HeaderText="% Covered Lives" Visible="true" DataFormatString="{0:n0}"  ItemStyle-CssClass="alignRight" />
        <asp:BoundField DataField="Tier_Name" HeaderText="Tier" Visible="true" />
        <asp:BoundField DataField="Formulary_Status_Name" HeaderText="Formulary Status" Visible="true" />
        <asp:BoundField DataField="PA" HeaderText="PA" Visible="true" />
        <asp:BoundField DataField="QL" HeaderText="QL" Visible="true" />
        <asp:BoundField DataField="ST" HeaderText="ST" Visible="true" />
        <asp:BoundField DataField="Co_Pay" HeaderText="Co-Pay" Visible="true" ItemStyle-CssClass="alignRight" />
        <asp:BoundField DataField="Restrictions_Comments" HeaderText="Comments" Visible="true" />
    </Columns>
    <EmptyDataTemplate>
        <asp:Label ID="Label1" text="No records present." runat="server"></asp:Label>
    </EmptyDataTemplate>
</asp:GridView>
<asp:EntityDataSource ID="dsPlanDrugFormulary" runat="server" ConnectionString="name=PathfinderMerzEntities"
DefaultContainerName="PathfinderMerzEntities" EntitySetName="PlanDrugFormularySet"
AutoGenerateWhereClause="true" OrderBy="it.[Drug_Name]">
<WhereParameters>
    <asp:QueryStringParameter QueryStringField="Plan_ID" Name="Plan_ID" Type="Int32" ConvertEmptyStringToNull="true"/>
    <%--<asp:QueryStringParameter QueryStringField="Section_ID" Name="Section_ID" Type="Int32" ConvertEmptyStringToNull="true"/>--%>
</WhereParameters>          
</asp:EntityDataSource>