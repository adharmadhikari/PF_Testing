<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ccrChartImage.aspx.cs" Inherits="custom_controls_ccrChartImage" %>
<%@ Register src="ccrChart.ascx" tagname="ccrChart" tagprefix="pinso" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title></title>
    </head>
    <body>
        <form id="form1" runat="server">            
            <pinso:ccrChart ID="ccrMeetingActivityChart1" runat="server" />                        
        </form>
    </body>
</html>
