<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/PartialPage.master" AutoEventWireup="true" CodeFile="filtertimeframecomparison.aspx.cs" Inherits="custom_warner_formularyhistoryreporting_all_filtertimeframecomparison" %>
<%@ Register src="~/custom/warner/formularyhistoryreporting/controls/FilterTimeFrameComparison.ascx" tagname="FilterTimeFrame" tagprefix="pinso" %>
<%@ Register src="~/custom/warner/formularyhistoryreporting/controls/FilterTimeFrameComparisonScript.ascx" tagname="FilterTimeFrameScript" tagprefix="pinso" %>

<asp:Content runat="server" ID="scriptSection" ContentPlaceHolderID="scriptContainer">
    <pinso:FilterTimeFrameScript runat="server" ID="filterTimeFrameComparisonScript" />        
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="partialPage" Runat="Server">
    <pinso:FilterTimeFrame ID="filterTimeComparisonFrame" runat="server" />            
</asp:Content>
