<%@ Page Title="" Language="C#" MasterPageFile="~/custom/MasterPages/CustomerContactReport.master" AutoEventWireup="true" CodeFile="customercontactreport.aspx.cs" Inherits="custom_auxilium_customercontactreports_all_customercontactreport" %>
<%@ Register Src="~/custom/auxilium/customercontactreports/controls/CCRGridView.ascx" TagName="CCRGridList" TagPrefix="pinso" %>
<%@ Register Src="~/custom/auxilium/customercontactreports/controls/BusinessPlansGrid.ascx" TagName="CCBusinessDocument" TagPrefix="pinso" %>
<%@ Register Src="~/custom/auxilium/customercontactreports/controls/CCRPlanGridView.ascx" TagName="CCPlanGrid" TagPrefix="pinso" %>
<%@ Register Src="~/custom/auxilium/customercontactreports/controls/CCReportScripts.ascx" TagName="CCPlanScript" TagPrefix="pinso" %>

<asp:Content ID="content8" ContentPlaceHolderID="scrptContainer" runat="server">
   <pinso:CCPlanScript ID="ccplanscript1" runat="server" />
</asp:Content>
<asp:Content ID="content7" ContentPlaceHolderID="Tile8Title" runat="server">
      Plan Select
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="Tile8Tools" runat="server">
    <a id="AddCCRLnk" class='reqsel' href="javascript:OpenCCR('AddCCR','');">Add New Meeting</a>
    <span class="reqsel">|</span>
    <a href='javascript:resetSectionPlans()'>Reset</a>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Tile8" Runat="Server">
    <pinso:CCPlanGrid ID="CCPlan1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Tile6Title" Runat="Server">
    Meetings
</asp:Content>
<asp:Content ContentPlaceHolderID="Tile6Tools" runat="server">
<a id="A3" class="reqsel" href="javascript:viewCCR();">View</a>
    <span class="reqsel">|</span>
    <a id="A4" class="reqsel" href="javascript:OpenDeleteCCR();">Delete</a>
    </asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Tile6" Runat="Server">
    <pinso:CCRGridList ID ="CCRGridList1" runat ="server" />
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="Tile7Title" Runat="Server">
    Documents
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
