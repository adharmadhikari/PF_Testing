<%@ Page Language="C#" AutoEventWireup="true" CodeFile="map.aspx.cs" Inherits="standardreports_all_map" %>
<%@ Register Src="~/controls/Map.ascx" TagName="Map" TagPrefix="pinso" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <pinso:Map runat="server" ID="map" />    
    </form>
</body>
</html>
