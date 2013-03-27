<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Popup.master" AutoEventWireup="true"
    CodeFile="OpenRestrictionCriteria.aspx.cs" Inherits="todaysaccounts_all_OpenRestrictionCriteria" %>

<asp:Content ID="Content1" ContentPlaceHolderID="scriptContainer" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="title" runat="Server">
 <asp:Literal runat="server" id="titleText" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="tools" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="main" runat="Server">
    <div id="main">
        <telerik:RadGrid runat="server" ID="rgCriteriaDetails" SkinID="radTable" Width="100%"
            DataSourceID="dsCriteriaDetails" AllowPaging="false" AllowFilteringByColumn="false"
            EnableEmbeddedSkins="false" AllowSorting="true">
            <MasterTableView AutoGenerateColumns="false" Width="100%" AllowSorting="true">
                <Columns>
                    <telerik:GridBoundColumn DataField="Criteria_Name" HeaderText="QL Restriction Criteria" UniqueName="Criteria_Name"
                        ItemStyle-CssClass="firstCol">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Criteria_Description" HeaderText="QL Details"
                        UniqueName="Criteria_Description">
                    </telerik:GridBoundColumn>
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>
        <asp:EntityDataSource ID="dsCriteriaDetails" runat="server" EntitySetName="RestrictionCriteriaDetailsSet"
            ConnectionString="name=PathfinderEntities" DefaultContainerName="PathfinderEntities"
            AutoGenerateWhereClause="true" OrderBy="it.Criteria_Name">
            <WhereParameters>
                <asp:QueryStringParameter QueryStringField="PlanID" Name="Plan_ID" Type="Int32" />
                <asp:QueryStringParameter QueryStringField="DrugID" Name="Drug_ID" Type="Int32" />
                <asp:QueryStringParameter QueryStringField="FormularyID" Name="Formulary_ID" Type="Int32" />
                <asp:QueryStringParameter QueryStringField="SegmentID" Name="Segment_ID" Type="Int32" />
                <asp:QueryStringParameter QueryStringField="ProductID" Name="Product_ID" Type="Int32" />
                <asp:QueryStringParameter QueryStringField="RestrictionID" Name="Restriction_ID"
                    Type="String" />
            </WhereParameters>
        </asp:EntityDataSource>
    </div>
</asp:Content>
