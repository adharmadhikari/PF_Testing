<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterAccountTypeMedD.ascx.cs" Inherits="standardreports_controls_FilterAccountTypeMedD" %>
<div id="filterAccountType">
    <div class="filterGeo">
    <asp:Literal runat="server" ID="filterLabel" Text='Account Type' />
    </div>
    <telerik:RadComboBox DataSourceID="dsAccountTypes" DataTextField="Account_Type_Name" DataValueField="Plan_Classification_ID" runat="server" ID="Class_Partition" Skin="pathfinder" EnableEmbeddedSkins="false" MaxHeight="200px">
        <%--<Items>
            <telerik:RadComboBoxItem runat="server" Value="1" Text="Parent" />
            <telerik:RadComboBoxItem runat="server" Value="2" Text="Regional" />
        </Items>--%>        
    </telerik:RadComboBox>
    
    <asp:EntityDataSource ID="dsAccountTypes" runat="server" EntitySetName="AccountTypesSet" ConnectionString="name=PathfinderEntities" DefaultContainerName="PathfinderEntities" OrderBy="it.Plan_Classification_ID"
        AutoGenerateWhereClause="true">
        <WhereParameters>
            <asp:QueryStringParameter QueryStringField="Channel" Name="Section_ID" Type="Int32" ConvertEmptyStringToNull="true"/>
        </WhereParameters>   
    </asp:EntityDataSource>

</div>