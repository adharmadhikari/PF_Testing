<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormularyComparison.aspx.cs" Inherits="custom_pinso_formularyhistoryreporting_all_FormularyComparison" %>
<%@ Register src="~/custom/pinso/formularyhistoryreporting/controls/formularycomparison.ascx" tagname="formularycomparison" tagprefix="pinso" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title></title>
    </head>
    <body>
        <form id="form1" runat="server"> 
           <%-- <asp:ScriptManager runat="server" ID="scriptManager"></asp:ScriptManager>      --%>              
            <pinso:formularycomparison ID="formularycomparison" runat="server" />
        </form>
    </body>
</html>
