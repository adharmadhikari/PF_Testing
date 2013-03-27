<%@ Page Language="C#" AutoEventWireup="true" CodeFile="error.aspx.cs" Inherits="Default2" %>
<%@ OutputCache Duration="1" NoStore="true" VaryByParam="None" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="scriptManager" EnablePartialRendering="false">
        <Scripts>
            <asp:ScriptReference Path="https://ajax.microsoft.com/ajax/jquery/jquery-1.4.2.min.js" />
            <asp:ScriptReference Path="~/content/scripts/ui.js" />
            <asp:ScriptReference Path="~/content/scripts/clientmanager.js" />
         </Scripts>
        <%--<Services>
            <asp:ServiceReference Path="~/services/securityservice.svc" />
        </Services>--%>
    </asp:ScriptManager>    
    <div>
        An error has occurred.  If this problem persists please contact customer support.  <asp:HyperLink runat="server" ID="supportEmail" Text='Email' />
    </div>
    <div id="closeLink" style="display:none;padding:25px;text-align:center;">
        <a href="javascript:$closeWindow()">Close</a>
    </div>
    </form>
    <script type="text/javascript">

        if (window.top != window)
            document.getElementById("closeLink").style.display = "block";

    </script>    
</body>
</html>
