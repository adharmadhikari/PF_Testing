<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SingleSection.master" AutoEventWireup="true" CodeFile="businessplansreport.aspx.cs" Inherits="custom_merz_businessplanning_all_businessplansreport" EnableViewState="true" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="scriptContainer" Runat="Server">
    <script type="text/javascript" >
        clientManager.add_pageLoaded(BP_Report_pageInitialized, "bpPlanInfo");
        clientManager.add_pageUnloaded(BP_Report_pageUnloaded, "bpPlanInfo");
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Tile3Title" Runat="Server">
    <asp:Literal runat="server" ID="Literal1" Text='<%$ Resources:Resource, SectionTitle_BusinessPlanReport %>' />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Tile3Tools" Runat="Server">

</asp:Content> 
 
<asp:Content ID="Content5" ContentPlaceHolderID="Tile3" Runat="Server">
<telerik:RadGrid runat="server" ID="gridReport" SkinID="radTable" AutoGenerateColumns="false" 
    EnableEmbeddedSkins="false" Skin="pathfinder">
    <MasterTableView PageSize="10">
        <Columns>
            <telerik:GridBoundColumn DataField="Section_Name" UniqueName="Section_Name" HeaderText="Account Type"   />
            <telerik:GridBoundColumn DataField="Plan_Name" UniqueName="Plan_Name" HeaderText="Account Name"   />
            <telerik:GridBoundColumn DataField="Thera_Name" UniqueName="Thera_Name" HeaderText="Therapeutic Area"  />
            <telerik:GridBoundColumn DataField="Created_DT" UniqueName="Created_DT" HeaderText="Date Created"  DataFormatString="{0:d}"/>            
            <telerik:GridBoundColumn DataField="FullName" UniqueName="FullName" HeaderText="Account Manager"  />
        </Columns>
    </MasterTableView>
    <ClientSettings ClientEvents-OnRowSelecting="gridReport_OnRowSelected" ClientEvents-OnDataBound="gridReport_OnDataBound">
        <DataBinding Location="~/custom/merz/businessplanning/services/merzdataservice.svc" DataService-TableName="BusinessPlansMerzSet" SelectCountMethod="GetBPReportCount" />
        <Selecting AllowRowSelect="true" />
    </ClientSettings>
</telerik:RadGrid>
<pinso:RadGridWrapper runat="server" ID="gridWrapper" Target="gridReport" PagingSelector="#tile3 .pagination:first"  CustomPaging="false" MergeRows="false"  RequiresFilter ="false" AutoLoad="false" ShowNumberOfRecords="true" />


<div id="bpPlanInfo">

</div>
</asp:Content>


