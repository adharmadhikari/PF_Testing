<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/PartialPage.master" AutoEventWireup="true" CodeFile="PlanInfoTip.aspx.cs" Inherits="todaysaccounts_all_PlanInfoTip" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="scriptContainer" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="partialPage" Runat="Server">
    <asp:FormView runat="server" ID="formView" DataSourceID="dsPlanInfoDetails" CellPadding="0" CellSpacing="0" Width="100%">  
        <%--<ItemTemplate>
            <%# Eval("Last_Update_DT", Resources.Resource.Label_Section_Last_Updated)%>
        </ItemTemplate>--%>
    </asp:FormView>
    <asp:EntityDataSource ID="dsPlanInfoDetails" runat="server" EntitySetName="PlanInfoSet" ConnectionString="name=PathfinderEntities" DefaultContainerName="PathfinderEntities" 
        AutoGenerateWhereClause="true">
        <WhereParameters>       
            <asp:QueryStringParameter QueryStringField="Plan_ID" Name="Plan_ID" Type="Int32" />
        </WhereParameters>
    </asp:EntityDataSource>    
</asp:Content>

