<%@ Page Title="" Language="C#" MasterPageFile="~/custom/MasterPages/CustomerContactReport.master" AutoEventWireup="true" CodeFile="customercontactreport.aspx.cs" Inherits="custom_jazz_customercontactreports_all_customercontactreport" %>
<%@ Register Src="~/custom/jazz/customercontactreports/controls/CCRGridView.ascx" TagName="CCRGridList" TagPrefix="pinso" %>
<%@ Register Src="~/custom/jazz/customercontactreports/controls/BusinessPlansGrid.ascx" TagName="CCBusinessDocument" TagPrefix="pinso" %>
<%@ Register Src="~/custom/jazz/customercontactreports/controls/CCRPlanGridView.ascx" TagName="CCPlanGrid" TagPrefix="pinso" %>
<%@ Register Src="~/custom/jazz/customercontactreports/controls/CCReportScripts.ascx" TagName="CCPlanScript" TagPrefix="pinso" %>

<asp:Content ID="content8" ContentPlaceHolderID="scrptContainer" runat="server">
    <!--[if IE 7]>
        <style type="text/css">
        #ctl00_ctl00_Tile3_Tile8_CCPlan1_gridPlans_GridHeader
        {
            width: 100%!important;
        }
        </style>
    <![endif]-->   
    <!--[if IE 6]>
        <style type="text/css">
        #ctl00_ctl00_Tile3_Tile8_CCPlan1_gridPlans_GridHeader
        {
            margin-right: 0px!important;
        }
        </style>
    <![endif]-->  
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
<asp:Content ID="Content5" ContentPlaceHolderID="Tile7Title" Runat="Server">
    Business Plans
</asp:Content>
<asp:Content ContentPlaceHolderID="Tile7Tools" runat="server">
    <a id="A1" href="javascript:OpenDocUpload('AddCCR');">Upload</a> 
    <span class='reqsel'>|</span> 
    <a id="A2" class="reqsel" href="javascript:viewDocument();">View</a>
    <span class="reqsel">|</span>
    <a id="DeleteCCRLnk" class="reqsel" href="javascript:OpenDeleteDoc();">Delete</a>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="Tile7" Runat="Server">
    <pinso:CCBusinessDocument ID ="BusinessDocument1" runat="server" />
</asp:Content>