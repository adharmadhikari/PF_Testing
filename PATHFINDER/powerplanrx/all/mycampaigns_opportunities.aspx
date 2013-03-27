<%@ Page Title="PowerPlanRx - My Campaign Opportunities" Language="C#"  MasterPageFile="~/powerplanrx/MasterPages/MasterPage.master"
    EnableViewState="true"  Theme="impact" AutoEventWireup="true" CodeFile="mycampaigns_opportunities.aspx.cs" 
    Inherits="mycampaigns_opportunities" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="main" Runat="Server">
<iframe id="iframetable" src="mycampaigns_opportunitiesReal.aspx"  frameborder="0" width="100%" height="100%" ></iframe>
   
</asp:Content>
 