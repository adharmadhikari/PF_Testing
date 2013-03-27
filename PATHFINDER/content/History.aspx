<%@ Page Language="C#" AutoEventWireup="true" CodeFile="History.aspx.cs" Inherits="content_History" %>
<%@ OutputCache VaryByParam="None" NoStore="false" Location="Client" Duration="100000" %>
<%@ Register Src="~/controls/googleanalytics.ascx" TagName="analytics" TagPrefix="google" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Pathfinder</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     
    </div>
    </form>
    <google:analytics runat="server" ID="googleScript" />
</body>
</html>
