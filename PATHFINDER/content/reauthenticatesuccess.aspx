<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Modal.master" Theme="pathfinder" AutoEventWireup="true" CodeFile="reauthenticatesuccess.aspx.cs" Inherits="content_reauthenticatesuccess" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="main" Runat="Server">
    <%--<asp:ScriptManagerProxy runat="server" ID="scriptManagerProxy">
        <Scripts>
            <asp:ScriptReference Path="~/content/scripts/clientmanager.js" />
        </Scripts>
    </asp:ScriptManagerProxy>--%>
    
    <script type="text/javascript">
        window.top.clientManager.restoreView();
    </script>
    <br />
    <h2>You have successfully logged in.  </h2>
    <br />
    <br />
    <a href="javascript:$closeWindow()">Close</a>
</asp:Content>

