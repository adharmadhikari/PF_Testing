<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormularyStatusChartImage.aspx.cs" Inherits="standardreports_controls_FormularyStatusChartImage" %>
<%@ Register src="FormularyStatusChart.ascx" tagname="formularystatuschart" tagprefix="pinso" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1" runat="server">
        <title></title>
    </head>
    <body>
        <form id="form2" runat="server">
           
                <pinso:formularystatuschart ID="formularystatuschart1" runat="server" Thumbnail="true" GeographicCoverage="true"  />
           
        </form>
    </body>
</html>
