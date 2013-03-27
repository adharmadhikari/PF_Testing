﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/PartialPage.master" AutoEventWireup="true" CodeFile="filtertimeframe.aspx.cs" Inherits="prescriberreporting_all_filtertimeframe" %>
<%@ Register src="~/prescriberreporting/controls/FilterTimeFrame.ascx" tagname="FilterTimeFrame" tagprefix="pinso" %>
<%@ Register src="~/prescriberreporting/controls/FilterTimeFrameScript.ascx" tagname="FilterTimeFrameScript" tagprefix="pinso" %>

<asp:Content runat="server" ID="scriptSection" ContentPlaceHolderID="scriptContainer">
    <pinso:FilterTimeFrameScript runat="server" ID="filterTimeFrameScript" />        
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="partialPage" Runat="Server">
    <pinso:FilterTimeFrame ID="filterTimeFrame" runat="server" />            
</asp:Content>
