<%@ Page Title="" Language="C#" Theme="pathfinder" MasterPageFile="~/MasterPages/Modal.master" AutoEventWireup="true" CodeFile="reauthenticate.aspx.cs" Inherits="content_reauthenticate" %>
<%@ Register Src="~/Controls/login.ascx" TagName="login" TagPrefix="pinso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="title" Runat="Server">Your session has expired
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="tools" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="main" Runat="Server">
    <asp:ScriptManagerProxy runat="server" ID="scriptManagerProxy">
        <Scripts>
            <asp:ScriptReference Path="~/content/scripts/jquery-ui-1.7.2.custom.min.js" />
            <asp:ScriptReference Path="~/content/scripts/login.js" />
        </Scripts>
    </asp:ScriptManagerProxy>
    
    <div class="wrapper">
        <pinso:login runat="server" ID="loginCtrl" LoginPageCheck="false" />
    </div>
</asp:Content>

