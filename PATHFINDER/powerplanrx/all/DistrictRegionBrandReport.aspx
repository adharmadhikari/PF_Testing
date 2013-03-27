<%@ Page Title="Region/District Drill Down Report" Language="C#" MasterPageFile="~/powerplanrx/MasterPages/Report.master" Theme="impact" AutoEventWireup="true"
    CodeFile="DistrictRegionBrandReport.aspx.cs" Inherits="DistrictRegionBrandReport" %>
    

<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
   <iframe id="districtregioniframetable" src="DistrictRegionBrandReportIFrame.aspx?reporttype=1&brandid=0&segment=1&regionid=0&dist=0"  frameborder="0" width="100%" height="100%" ></iframe>

</asp:Content>