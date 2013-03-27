<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="ChangePassword_Forced.aspx.cs" Inherits="usercontent_ChangePassword_Forced" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Change Password</title> <link id="Link2" runat="server" href="~/content/styles/main.css" rel="stylesheet" type="text/css" />
</head>
 
<body>
    <form id="form1" runat="server">
     <asp:ScriptManager runat="server" ID="scriptManager">
        <Scripts>
            <asp:ScriptReference Path="https://ajax.microsoft.com/ajax/jquery/jquery-1.4.2.min.js" />
            <asp:ScriptReference Path="~/content/scripts/login.js" />
            <asp:ScriptReference Path="~/content/scripts/ui.js" />
            <asp:ScriptReference Path="~/content/scripts/components.js" />     
            <asp:ScriptReference Path="~/content/scripts/clientmanager.js" />
        </Scripts>
     </asp:ScriptManager>
    <div id="loginPage" class="signIn">
        <asp:Image ID="Image1" SkinID="topImage" runat="server" />
        <div class="inside">
            <div class="logos">
                <asp:Image SkinID="loginLogo" ID="PFLogo" runat="server" />                
            </div>
            <img id="Img1" class="userIcon" runat="server" src="~/content/images/user.jpg" alt="User Icon" />
            <div class="userInfo">
                <asp:HyperLink ID="info" runat="server" NavigateUrl="javascript:openPasswordChangeGuide()">Password Change Guidelines</asp:HyperLink>   
                <asp:UpdatePanel runat="server" ID="updatePanel" >
                    <ContentTemplate>
                        <div class="changePassword" >
                            <asp:ChangePassword runat="server" ID="changePassword" InstructionText="" ChangePasswordTitleText="" 
                                ContinueDestinationPageUrl="~/dashboard.aspx" 
                                ContinueButtonStyle-CssClass="button" CancelButtonStyle-CssClass="button" 
                                ChangePasswordButtonStyle-CssClass="button" TextBoxStyle-Width="170px" 
                                CancelDestinationPageUrl="~/login.aspx" 
                                ChangePasswordFailureText="Password incorrect or New Password invalid.">
                                <CancelButtonStyle CssClass="button" />
                                <ContinueButtonStyle CssClass="button" />
                                <ChangePasswordButtonStyle CssClass="button" />
                                <TextBoxStyle Width="170px" />
                            </asp:ChangePassword>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="clearAll"></div>
        </div>
        <asp:Image ID="Image2" SkinID="btmImage" runat="server" />
    </div>
    </form> 
</body>
</html> 

