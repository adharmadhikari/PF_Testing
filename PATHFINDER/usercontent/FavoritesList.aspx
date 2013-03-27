<%@ Page Theme="pathfinder" Language="C#" AutoEventWireup="true" CodeFile="FavoritesList.aspx.cs" Inherits="usercontent_FavoritesList" %>
<%@ Register src="../Controls/favoritesList.ascx" tagname="favoritesList" tagprefix="pinso" %>
<%@ OutputCache NoStore="true" Duration="1" VaryByParam="None" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" ID="scriptManager" EnablePartialRendering="false" />
        <pinso:favoritesList ID="favoritesList1" runat="server" SelectFavoritesFunctionFormat="clientManager.selectFavorite({0}, {1})" EnableDelete="false" />   
    </form>
</body>
</html>
