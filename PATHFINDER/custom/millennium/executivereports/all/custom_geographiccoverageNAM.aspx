﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SingleSection.master" AutoEventWireup="true" CodeFile="custom_geographiccoverageNAM.aspx.cs" Inherits="custom_millennium_executivereports_all_custom_geographiccoverageNAM" %>

<%-- Executive Reports - Geographic Coverage NAM --%>

<asp:Content ID="Content2" ContentPlaceHolderID="Tile3Title" Runat="Server">
    Geographic Coverage Report By NAM
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Tile3Tools" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Tile3" Runat="Server">
    <iframe id="reportviewerframe" src="custom/millennium/executivereports/all/ReportViewer.aspx?reportname=MillenniumExecutiveReports&report=GeographicCoverageNam"  frameborder="0" width="100%" height="100%"></iframe>       
</asp:Content>
