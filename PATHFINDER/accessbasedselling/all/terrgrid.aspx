<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/PartialPage.master" AutoEventWireup="true" CodeFile="terrgrid.aspx.cs" Inherits="accessbasedselling_all_terrgrid" %>
<%@ OutputCache NoStore="true" Duration="1" VaryByParam="None" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="scriptContainer" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="partialPage" Runat="Server">
    <telerik:RadGrid runat="server" ID="gridTerrs" SkinID="radTable" DataSourceID="dsData" AutoGenerateColumns="false" PageSize="5" EnableEmbeddedSkins="false" Skin="pathfinder">
        <MasterTableView PageSize="5">
            <Columns>
                <telerik:GridBoundColumn HeaderText="Geography" DataField="Geography" ItemStyle-CssClass="firstCol" />
                <telerik:GridBoundColumn HeaderText="User" DataField="User" />
                <telerik:GridBoundColumn HeaderText="Product1 MKT TRX" DataField="SRI_MKT_TRX"  DataFormatString="{0:n0}" HeaderStyle-CssClass="alignRight" ItemStyle-CssClass="alignRight" />
                <telerik:GridBoundColumn HeaderText="Product1 TRX" DataField="LEXAPRO_TRX" DataFormatString="{0:n0}" HeaderStyle-CssClass="alignRight" ItemStyle-CssClass="alignRight" />
                <telerik:GridBoundColumn HeaderText="Product1 Share" DataField="LX_Share" DataFormatString="{0:p}" HeaderStyle-CssClass="alignRight" ItemStyle-CssClass="alignRight" />
                <telerik:GridBoundColumn HeaderText="MKT TRX" DataField="SNRI_MKT_TRX"  DataFormatString="{0:n0}" HeaderStyle-CssClass="alignRight" ItemStyle-CssClass="alignRight" />
                <telerik:GridBoundColumn HeaderText="Share" DataField="SNRI_Share"  DataFormatString="{0:p}"  HeaderStyle-CssClass="alignRight" ItemStyle-CssClass="alignRight"/>
            </Columns>
        </MasterTableView>
        <ClientSettings>
            <Selecting AllowRowSelect="true" />
            <%--<DataBinding Location="~/services/PathfinderClientService.svc" DataService-TableName="ABSSummaryViewSet" />--%>
        </ClientSettings>
    </telerik:RadGrid>
    <asp:EntityDataSource runat="server" ID="dsData" EntitySetName="ABSSummaryViewSet" DefaultContainerName="PathfinderClientEntities" AutoGenerateWhereClause="true">
        <WhereParameters>
            <asp:SessionParameter Name="User_ID" SessionField="UserID" Type="Int32" />
        </WhereParameters>
    </asp:EntityDataSource>
    <%--<pinso:RadGridWrapper runat="server" ID="gridWrapper1" Target="gridTerrs" RequiresFilter="false" AutoLoad="true" />--%>
</asp:Content>

