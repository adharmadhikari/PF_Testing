<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SingleSection.master" AutoEventWireup="true" CodeFile="MedicalPolicy.aspx.cs" Inherits="custom_merz_businessplanning_all_MedicalPolicy" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="scriptContainer" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Tile3Title" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Tile3Tools" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Tile3" Runat="Server">
<telerik:RadGrid runat="server" ID="gridMedicalPolicy" SkinID="radTable"  AutoGenerateColumns="false" PageSize="5" EnableEmbeddedSkins="false" Skin="pathfinder">
    <MasterTableView PageSize="5" >
        <Columns>
            <telerik:GridBoundColumn DataField="Plan_Name" UniqueName="Plan_Name" HeaderText="Account Name"   />
            <telerik:GridBoundColumn DataField="Medical_Policy_Name" UniqueName="Medical_Policy_Name" HeaderText="Medical Policy Name"  />
            <telerik:GridBoundColumn DataField="Upload_DT" UniqueName="Upload_DT" HeaderText="Uploaded Date"  DataFormatString="{0:d}"/>            
            <telerik:GridBoundColumn DataField="Upload_BY" UniqueName="Upload_BY" HeaderText="Uploaded By"  />
            <telerik:GridBoundColumn DataField="Medical_Policy_ID" UniqueName="Medical_Policy_ID" HeaderText="Medical Policy ID"  />
        </Columns>
    </MasterTableView>
    <ClientSettings>
        <DataBinding Location="~/custom/merz/businessplanning/services/merzdataservice.svc" DataService-TableName="MedicalPolicySet"  />
        <Selecting AllowRowSelect="true" />
    </ClientSettings>
</telerik:RadGrid>
<pinso:RadGridWrapper runat="server" ID="gridWrapper" Target="gridMedicalPolicy" MergeRows="false"  RequiresFilter ="false" AutoLoad="true" ShowNumberOfRecords="false" />

</asp:Content>

