<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterAccountType.ascx.cs" Inherits="standardreports_controls_FilterAccountType" %>
<div id="filterAccountType">
    <div class="filterGeo">
    <asp:Literal runat="server" ID="filterLabel" Text='Account Type' />
    </div>
    <telerik:RadComboBox DataSourceID="dsAccountTypes" DataTextField="Account_Type_Name" DataValueField="Plan_Classification_ID" runat="server" ID="Class_Partition" Skin="pathfinder" EnableEmbeddedSkins="false" MaxHeight="200px">
               
    </telerik:RadComboBox> 
    
    <asp:EntityDataSource ID="dsAccountTypes" runat="server" EntitySetName="AccountTypesSet" ConnectionString="name=PathfinderEntities" DefaultContainerName="PathfinderEntities" OrderBy="it.Plan_Classification_ID"
        Where="it.Plan_Classification_ID not in {26,27} and it.Section_ID = @Section_ID">
        <WhereParameters>
            <asp:QueryStringParameter QueryStringField="Channel" Name="Section_ID" Type="Int32" ConvertEmptyStringToNull="true"/>            
        </WhereParameters>   
    </asp:EntityDataSource>

</div>
<div id="filterAccountSubType">
    <div class="filterGeo">
    <asp:Literal runat="server" ID="Literal1" Text='Account Sub Type' />
    </div>
    <telerik:RadComboBox runat="server" ID="Segment_ID" Skin="pathfinder" EnableEmbeddedSkins="false" MaxHeight="200px">
        <Items>
            <telerik:RadComboBoxItem runat="server" Value="2" Text="Total Medicare Part-D" />
            <telerik:RadComboBoxItem runat="server" Value="8" Text="Low Income Subsidy (LIS)" />
        </Items>        
    </telerik:RadComboBox>  
</div>