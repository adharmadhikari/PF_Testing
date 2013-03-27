<%@ Page Language="C#" Theme="pathfinder" MasterPageFile="~/MasterPages/Modal.master" AutoEventWireup="true" CodeFile="forgotpassword.aspx.cs" Inherits="content_forgotpassword" %>

<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="main">
    <div class="modalForm">
    <asp:UpdatePanel runat="server" ID="updatePanel">
        <ContentTemplate>
            <asp:Panel runat="server" ID="requestEntry">
                <div class="instructions">
                    <asp:Literal runat="server" ID="instructions" Text='<%$ Resources:Resource, ForgotPassword_Instructions %>' />
                </div>

                <span class="coreTextBox">
                                    <span class="bg">
                                        <span class="bg2">
                                    <asp:TextBox CssClass="textBox" runat="server" ID="emailAddress" AutoCompleteType="Disabled" ></asp:TextBox>
                                        </span>
                                    </span>
                                </span>
                
                <%--Dummy textbox so form submits properly - Pressing enter within emailAddress textbox won't hit OnSubmitRequest function in postback unless there is more than one input control on the page. --%>
                <input type="text" style="display:none;" />
                                
                <asp:Button SkinID="formButton" runat="server" ID="submit" Text="Submit" OnCommand="OnSubmitRequest" />
                <div>
                    <asp:RequiredFieldValidator runat="server" ID="requiredField1" ErrorMessage='<%$ Resources:Resource, ForgotPassword_Validation_EmailRequired %>' ControlToValidate="emailAddress" Display="Dynamic" />
                    <asp:CustomValidator runat="server" ID="emailError" ErrorMessage='<%$ Resources:Resource, ForgotPassword_InvalidEmailError %>' EnableClientScript="false" Display="Dynamic" />
                    <asp:CustomValidator runat="server" ID="invalidUser" ErrorMessage='<%$ Resources:Resource, ForgotPassword_ServerError %>' EnableClientScript="false" Display="Dynamic" />
                </div>
            </asp:Panel>
                    
            <asp:Panel runat="server" ID="requestSubmitted" Visible="false">
                <asp:Literal runat="server" ID="submitNotification" Visible="true" Text='<%$ Resources:Resource, ForgotPassword_Submitted %>' />
                <asp:Literal runat="server" ID="submitNotification_auto" Visible="false" Text='<%$ Resources:Resource, ForgotPassword_Submitted_Auto %>' />
            
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
   </div>
</asp:Content>
