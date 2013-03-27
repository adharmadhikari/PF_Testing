<%@ Control Language="C#" AutoEventWireup="true" CodeFile="header.ascx.cs" Inherits="controls_header" %>
<div class="welcomeMenu">
    <div style="float:left">
        <span style="text-transform:uppercase; padding-left:3px;">Welcome <%= Session["FirstName"] %></span>&nbsp;&nbsp;
        &bull;
        <asp:HyperLink ID="linkHome" runat="server" NavigateUrl="~/dashboard.aspx" Text='Pathfinder Home'></asp:HyperLink>

        <!--<asp:Literal runat="server" Text="&bull;" ID="homeBullet" />-->
    </div>    

    <div style="float:left">
    &bull;
    <asp:HyperLink ID="HyperLink4" runat="server" NavigateUrl="~/content/signout.aspx" Text='<%$ Resources:Resource, Label_Sign_Out %>'></asp:HyperLink></div>
    </div>
<div class="logo">
    <asp:Image SkinID="mainLogo" ID="PFLogo" runat="server" /> <h1 class="tagLine" id="tagLine" runat="server"><asp:Image SkinID="mainLogo2" ID="Image1" runat="server" style="margin-top:-6px" /></h1>
</div>
<div class="clearAll">
</div>
