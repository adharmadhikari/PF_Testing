<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" Theme="pathfinder" AutoEventWireup="true" CodeFile="AccountManagement.aspx.cs" Inherits="management_AccountManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">
        function openPasswordChange()
        {
            $openWindow('changepassword.aspx',null, null, 350, 225)
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="main" Runat="Server">
    <telerik:RadWindowManager runat="server" ID="radWindowManager" EnableEmbeddedSkins="false" Skin="pathfinder" DestroyOnClose="true" Modal="true" Behaviors="Close" VisibleTitlebar="false"/>

    <a href="javascript:void(0)" onclick="openPasswordChange()">Change Password</a>
</asp:Content>


