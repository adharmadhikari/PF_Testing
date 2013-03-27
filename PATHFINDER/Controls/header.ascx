<%@ Control Language="C#" AutoEventWireup="true" CodeFile="header.ascx.cs" Inherits="controls_header" %>
<div class="welcomeMenu">
    <div style="float:left">
        <span style="text-transform:uppercase; padding-left:3px;">Welcome <%= Session["FirstName"] %></span>&nbsp;&nbsp;
        &bull;
        <asp:HyperLink ID="linkHome" runat="server" NavigateUrl="~/home.aspx" Text='<%$ Resources:Resource, Label_Home %>'></asp:HyperLink>

        <asp:Literal runat="server" Text="&bull;" ID="homeBullet" />
    </div>    
    <telerik:RadMenu runat="server" EnableAutoScroll="true" EnableEmbeddedSkins="false" SkinID="helpMenu" ID="helpMenu" OnClientItemClicked="onHelpMenuItemClicked" ClickToOpen="true"  >
        <Items>
            <telerik:RadMenuItem Text="Help">
                <Items>
                    <%-- Value property should match a javascript function name to trigger the action. --%>
                    <telerik:RadMenuItem Text="Change Password" Value="changePassword" />
                    <%--<telerik:RadMenuItem Text="Customer Support" Value="customerSupport" />--%>
                    <telerik:RadMenuItem Text="Support" Value="about" />
                </Items>
            </telerik:RadMenuItem>
        </Items>
    </telerik:RadMenu>
    <div style="float:left">
    &bull;
    <asp:HyperLink ID="HyperLink4" runat="server" NavigateUrl="~/content/signout.aspx" Text='<%$ Resources:Resource, Label_Sign_Out %>'></asp:HyperLink></div>
    </div>
<div class="logo">
    <asp:Image SkinID="mainLogo" ID="PFLogo" runat="server" /> <h1 class="tagLine" id="tagLine" runat="server"><!-- TAG LINE HERE--></h1>
</div>
<div class="clearAll">
</div>
