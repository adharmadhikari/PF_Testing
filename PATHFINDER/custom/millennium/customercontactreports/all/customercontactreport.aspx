<%@ Page Title="" Language="C#" MasterPageFile="~/custom/MasterPages/CustomerContactReport.master" AutoEventWireup="true" CodeFile="customercontactreport.aspx.cs" Inherits="custom_pinso_customercontactreports_all_customercontactreport" %>
<%@ Register Src="~/custom/millennium/customercontactreports/controls/CCRGridView.ascx" TagName="CCRGridList" TagPrefix="pinso" %>
<%@ Register Src="~/custom/millennium/customercontactreports/controls/CCRPlanGridView.ascx" TagName="CCPlanGrid" TagPrefix="pinso" %>
<%@ Register Src="~/custom/millennium/customercontactreports/controls/CCReportScripts.ascx" TagName="CCPlanScript" TagPrefix="pinso" %>

<asp:Content ID="content8" ContentPlaceHolderID="scrptContainer" runat="server">
   
   <pinso:CCPlanScript ID="ccplanscript1" runat="server" />
</asp:Content>
<asp:Content ID="content7" ContentPlaceHolderID="Tile8Title" runat="server">
      Plan Select
</asp:Content>
<asp:Content ContentPlaceHolderID="Tile8Tools" runat="server">
    <a id="MorePlans" href='javascript:MorePlans()'>More Plans</a>
    <span>|</span>
    <a id="AddCCRLnk" class='reqsel' href="javascript:OpenCCR('AddCCR','');">Add New Meeting</a>
    <span class="reqsel">|</span>
    <a href='javascript:resetSectionPlans()'>Reset</a>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="Tile8" Runat="Server">
    <asp:Label CssClass="userid" style="display:none" runat="server" ID="lblUserID"></asp:Label>
    <pinso:CCPlanGrid ID="CCPlan1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Tile6Title" Runat="Server">
    Meetings
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Tile6" Runat="Server">
    <pinso:CCRGridList ID ="CCRGridList1" runat ="server" />
</asp:Content>
