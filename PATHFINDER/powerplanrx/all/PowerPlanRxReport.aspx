<%@ Page Title="PowerPlanRx Report" Language="C#" MasterPageFile="~/powerplanrx/MasterPages/Report.master" Theme="impact" AutoEventWireup="true"
    CodeFile="PowerPlanRxReport.aspx.cs" Inherits="powerplanrx_all_PowerPlanRxReport" %>
    

<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
   <iframe id="pprxiframetable" src="PowerPlanRxReportIFrame.aspx"  frameborder="0" width="100%" height="100%" ></iframe>

</asp:Content>
