<%@ Page Title="PowerPlanRx - My Campaigns" Language="C#" MasterPageFile="~/powerplanrx/MasterPages/MasterPage.master" Theme="impact" AutoEventWireup="true" CodeFile="mycampaigns_current.aspx.cs" Inherits="mycampaigns_current" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="main" Runat="Server">
<%--<style>
 .RadGrid a:hover {COLOR: Blue}
</style>--%>
<div class="tileContainerHeader">
<div class="CampaignInfo">My Campaigns</div>


</div>

<telerik:RadGrid runat="server" ID="gridMyCampaigns" EnableEmbeddedSkins="false" SkinID="table1" AutoGenerateColumns="false" PageSize="10" 
    DataSourceID="dsMyCampaigns" Width="100%" AllowPaging="true">
    
    <MasterTableView PageSize="10" AllowSorting="true" AllowMultiColumnSorting="true">
        <Columns>
            <telerik:GridBoundColumn HeaderText="AM" DataField="AM" SortExpression="User_L_Name" />
            <telerik:GridBoundColumn HeaderText="ID" DataField="Campaign_ID" SortExpression="Campaign_ID" /> 
            <telerik:GridHyperLinkColumn HeaderText="Campaign Name" DataTextField="Campaign_Name" DataTextFormatString="{0}" 
                DataNavigateUrlFields="Campaign_ID" DataNavigateUrlFormatString="createcampaign_step1_profile.aspx?id={0}" 
                SortExpression="Campaign_Name" />            
            <telerik:GridBoundColumn HeaderText="Status" DataField="Status_Name" SortExpression="Status_Name" />
            <telerik:GridBoundColumn HeaderText="Next Phase" DataField="Next_Step_Name" SortExpression="Next_Step_Name" />
            <telerik:GridBoundColumn HeaderText="Date Created" DataField="Created_DT" DataFormatString="{0:d}" SortExpression="Created_DT" />
        </Columns>        
    </MasterTableView>
    <ClientSettings>
    <Selecting AllowRowSelect="true" />
            <Scrolling AllowScroll="true" UseStaticHeaders="false" />
    </ClientSettings>
</telerik:RadGrid> 


<asp:SqlDataSource ID="dsMyCampaigns" runat="server"  ConnectionString="<%$ ConnectionStrings:PathfinderClientDB_Format %>" 
 SelectCommand="pprx.usp_GetMyCurrentCampaignInfo" SelectCommandType="StoredProcedure" OnSelecting="OnSelecting"> 
</asp:SqlDataSource>

</asp:Content>

