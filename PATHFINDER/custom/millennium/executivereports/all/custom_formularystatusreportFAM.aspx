<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SingleSection.master" AutoEventWireup="true" CodeFile="custom_formularystatusreportFAM.aspx.cs" Inherits="custom_millennium_executivereports_all_custom_formularystatusreportFAM" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Tile3Title" Runat="Server">
Formulary Status Report By FAM
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Tile3Tools" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Tile3" Runat="Server">
    <iframe id="reportviewerframe" 
        src="custom/millennium/executivereports/all/ReportViewer.aspx?reportname=MillenniumExecutiveReports&report=FormularyStatusReportFAM&parameter=NAM_FAM&parametervalue=FAM"  
        frameborder="0" width="100%" height="100%">
    </iframe>       
</asp:Content>
