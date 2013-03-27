<%@ Page Language="C#" MasterPageFile="~/MasterPages/Print.master" AutoEventWireup="true" CodeFile="PlanInformationSMPrint.aspx.cs" Inherits="todaysaccounts_all_PlanInformationSMPrint"
    Theme="pathfinderPrint" EnableTheming="true" %>
<%@ Register src="~/todaysaccounts/controls/PlanInfoDetailsSM.ascx" tagname="PlanInfoDetails" tagprefix="pinso" %>
<%@ Register src="~/todaysaccounts/controls/PlanInfoAddress.ascx" tagname="PlanInfoAddress" tagprefix="pinso" %>

<%-- Today's Accounts - Commercial -  Plan Information --%>

<asp:Content ID="printTitleContent" ContentPlaceHolderID="PrintTitle" Runat="Server">
    Plan Information
</asp:Content>
<asp:Content ID="printContentsContent" ContentPlaceHolderID="PrintContents" Runat="Server">
    <div class="blockTitle">Plan Details</div>
    <pinso:PlanInfoDetails ID="PlanInfoDetails" runat="server" />
    
    <div class="blockTitle">Address</div>
    <pinso:PlanInfoAddress ID="PlanInfoAddress" runat="server" /> 
</asp:Content>
