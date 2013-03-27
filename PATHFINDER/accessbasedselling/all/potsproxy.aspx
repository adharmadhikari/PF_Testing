<%@ Page Language="C#" AutoEventWireup="true" CodeFile="potsproxy.aspx.cs" Inherits="accessbasedselling_all_potsproxy" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server" action="https://pinsonault-ots.com/Intranet/index.asp" method="post">
    <div style="visibility:hidden">
        <input value="administrator@pinsonault.com" id="email" name="email" />
        <input value="pinsotest" id="password" name="password" />
        <input type="submit" value="Post" />
    </div>
    </form>
    <script type="text/javascript">
        form1.submit();
    </script>
</body>
</html>
