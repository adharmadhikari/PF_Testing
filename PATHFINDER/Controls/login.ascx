<%@ Control Language="C#" AutoEventWireup="true" CodeFile="login.ascx.cs" Inherits="Controls_login" %>
<div id="loginPage" class="signIn">
        <asp:Image ID="Image1" SkinID="topImage" runat="server" />
        <div class="inside">
            <div class="logos">
                <asp:Image SkinID="loginLogo" ID="PFLogo" runat="server" />                
            </div>
            <img class="userIcon" runat="server" src="~/content/images/user.jpg" alt="User Icon" />
            <div class="userInfo">
                <asp:UpdatePanel runat="server" ID="updatePanel">
                    <ContentTemplate>
                        <asp:Login ID="Login1" runat="server" SkinID="loginBox" DisplayRememberMe="false"
                            DestinationPageUrl="~/dashboard.aspx">
                            <LayoutTemplate>
                                <span class="coreTextBox">
                                    <span class="bg">
                                        <span class="bg2">
                                    <asp:TextBox TabIndex="1" CssClass="textBox" ID="UserName" runat="server" EnableViewState="false"
                                        AutoCompleteType="Disabled"></asp:TextBox>
                                        </span>
                                    </span>
                                </span>
                                <span class="rightCol">
                                    <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                        Text="*"></asp:RequiredFieldValidator><asp:Label ID="LabelUserName" Text="Username" runat="server" />
                                </span> 
                                

                                <div class="clearAll"></div>
                                <span class="coreTextBox">
                                    <span class="bg">
                                        <span class="bg2">
                                            <asp:TextBox TabIndex="2" CssClass="textBox" ID="Password" runat="server" TextMode="Password"
                                                    EnableViewState="false"></asp:TextBox>
                                        </span>
                                    </span>
                                </span>
                                <span class="rightCol">
                                    <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                        Text="*"></asp:RequiredFieldValidator></span><asp:Label ID="LabelPassword" Text="Password" runat="server" />
                                <div class="clearAll"></div>
                                <asp:Button ID="Login" SkinID="formButton" CommandName="Login" Text="Sign In" runat="server" />                                
                                <div>
                                    <p class="error">
                                        <asp:Literal ID="FailureText" runat="server"></asp:Literal>
                                    </p> 
                                    <div id="spanProgress"><asp:Literal ID="Failure" runat="server" Text='<%$ Resources:Resource, Message_VerifyingUser %>' /></div>
                                </div>
                                <div runat="server" id="additionalOptions">
                                    <a href="javascript:forgotPassword()">Forgot your password?</a> &bull; <a href="javascript:resetUserName()">
                                        Sign in with a different account</a>
                                </div>
                                
                            </LayoutTemplate>
                        </asp:Login>
                    </ContentTemplate>
                </asp:UpdatePanel>                
            </div>
            <div class="clearAll"></div>
        </div>
        <asp:Image ID="Image2" SkinID="btmImage" runat="server" />
        </div>