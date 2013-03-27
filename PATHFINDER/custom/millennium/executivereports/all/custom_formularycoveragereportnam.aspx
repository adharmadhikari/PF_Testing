<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SingleSection.master" AutoEventWireup="true" CodeFile="custom_formularycoveragereportnam.aspx.cs" Inherits="custom_millennium_executivereports_custom_formularycoveragereportnam" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Tile3Title" Runat="Server">
Formulary Coverage Report By NAM
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Tile3Tools" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Tile3" Runat="Server">
    <iframe id="reportviewerframe" 
        src="custom/millennium/executivereports/all/ReportViewer.aspx?reportname=MillenniumExecutiveReports&report=FormularyCoverageRptNAM"  
        frameborder="0" width="100%" height="100%" scrolling="auto">
    </iframe>       
</asp:Content>