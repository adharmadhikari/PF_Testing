<%@ Page Title="PowerPlanRx - Step 4 Results" Language="C#" MasterPageFile="~/powerplanrx/MasterPages/MasterPage.master" Theme="impact" 
    AutoEventWireup="true" CodeFile="createcampaign_step4_results.aspx.cs" Inherits="createcampaign_step4_results" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="main" Runat="Server">
<!--<iframe id="reportviewerframe" src="ReportViewer.aspx?reportname=PPRX_Reports&report=RegionResult&Campaign_<% =Request.QueryString.ToString().ToUpper() %>" frameborder="0" width="100%" height="100%">
    </iframe>-->
    <script type="text/javascript">
        var url = "ReportViewer.aspx?Client_Key=<%=Pinsonault.Web.Session.ClientKey %>&reportname=PPRX_Reports&report=CampaignResult&Campaign_<% =Request.QueryString.ToString().ToUpper() %>";
        var div = ".contentArea";
        var height = "100%";
        insertIframe(div, url, height);
    </script>
</asp:Content>
