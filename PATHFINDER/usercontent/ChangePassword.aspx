<%@ Page Title="" Theme="pathfinder" Language="C#" MasterPageFile="~/MasterPages/Modal.master" AutoEventWireup="true" CodeFile="ChangePassword.aspx.cs" Inherits="management_ChangePassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content runat="server" ID="title1" ContentPlaceHolderID="title">
    <asp:Literal runat="server" ID="titleText" Text='<%$ Resources:Resource, DialogTitle_ChangePassword %>' />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="main" Runat="Server"   >
    <asp:HyperLink ID="info" runat="server" NavigateUrl="javascript:openPasswordChangeGuide()">Password Change Guidelines</asp:HyperLink>   
    <asp:UpdatePanel runat="server" ID="updatePanel" >
        <ContentTemplate>
            <div class="changePassword" >
                <asp:ChangePassword runat="server" ID="changePassword" InstructionText="" ChangePasswordTitleText="" 
                    ContinueDestinationPageUrl="javascript:$closeWindow();" ContinueButtonStyle-CssClass="button" 
                    CancelButtonStyle-CssClass="button" ChangePasswordButtonStyle-CssClass="button" TextBoxStyle-Width="170px" 
                    ChangePasswordFailureText="Password incorrect or New Password invalid."
                    CancelDestinationPageUrl="javascript:$closeWindow();" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>    
</asp:Content>

