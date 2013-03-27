﻿<%@ Page Title="PowerPlanRx - Campaigns" Language="C#" MasterPageFile="~/powerplanrx/MasterPages/MasterPage.master" Theme="impact" AutoEventWireup="true" CodeFile="campaigns_current.aspx.cs" Inherits="campaigns_current" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="main" Runat="Server"> 
<%--<style>
 .RadGrid a:hover {COLOR: Blue}
</style>--%>
<div class="tileContainerHeader">
<div class="CampaignInfo">Campaign Opportunity Assessment For</div>
</div>
<telerik:RadGrid runat="server" ID="gridCampaigns" SkinID="table1" EnableEmbeddedSkins="false"  AutoGenerateColumns="false" DataSourceID="dsCampaigns"  
    Width="100%" AllowPaging="true">
    
    <MasterTableView AllowSorting="true" AllowMultiColumnSorting="true" PagerStyle-Position="Top" PagerStyle-CssClass="pagerImpact">    
        <Columns>
            <telerik:GridBoundColumn HeaderText="ID" DataField="Campaign_ID" SortExpression="Campaign_ID" />
            <telerik:GridBoundColumn HeaderText="Region" DataField="Region_Name" SortExpression="Region_Name" />
            <telerik:GridBoundColumn HeaderText="District" DataField="District_Name" SortExpression="District_Name" />   
            <telerik:GridBoundColumn HeaderText="Territory" DataField="Territory_Name" SortExpression="Territory_Name"/> 
            <telerik:GridBoundColumn HeaderText="AM" DataField="Full_Name" SortExpression="User_L_Name" /> 
            <telerik:GridHyperLinkColumn HeaderText="Campaign" DataTextField="Campaign_Name" DataTextFormatString="{0}" 
                DataNavigateUrlFields="Campaign_ID" DataNavigateUrlFormatString="createcampaign_step1_profile.aspx?id={0}" 
                SortExpression="Campaign_Name" />               
            <telerik:GridBoundColumn HeaderText="Status" DataField="Status_Name" SortExpression="Status_Name" />
            <telerik:GridBoundColumn HeaderText="Next Phase" DataField="Next_Step_Name" SortExpression="Next_Step_Name" />
            <telerik:GridBoundColumn HeaderText="Date Created" DataField="Created_DT" SortExpression="Created_DT" DataFormatString="{0:d}" ItemStyle-CssClass="lastCol" />
        </Columns>        
    </MasterTableView>
    <ClientSettings>
    <Selecting AllowRowSelect="true" />
            <Scrolling AllowScroll="true" UseStaticHeaders="false" />
    </ClientSettings>
</telerik:RadGrid> 

<asp:SqlDataSource ID="dsCampaigns" runat="server"  ConnectionString="<%$ ConnectionStrings:PathfinderClientDB_Format %>" 
 SelectCommand="pprx.usp_GetCampaignInfo"  SelectCommandType="StoredProcedure">
</asp:SqlDataSource>
</asp:Content>

