<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TierCoverageChartImage.aspx.cs" Inherits="standardreports_controls_TierCoverageChartImage" %>
<%@ Register src="tiercoveragechart.ascx" tagname="tiercoveragechart" tagprefix="pinso" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title></title>
    </head>
    <body>
        <form id="form1" runat="server">            
            <pinso:tiercoveragechart ID="tiercoveragechart1" runat="server" />                        
        </form>
    </body>
</html>