<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Modal.master" AutoEventWireup="true" CodeFile="EmailSellSheet.aspx.cs" Inherits="custom_unitedthera_sellsheets_all_EmailSellSheet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script type="text/javascript">
    function CloseWinDelay()
    {
        window.setTimeout(CloseWin, 4000);
    }

    function CloseWin()
    {
        var manager = window.top.GetRadWindowManager();

        var window1 = manager.getWindowByName("EmailSSWnd");
        if (window1 != null)
        { window1.close(); }
    }

    function ValidatePage()
    {
        var data = $getContainerData("emailContainer");

        var isValid = $validateContainerData("emailContainer", data, "", null, true);

        return isValid;
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="title" Runat="Server">
Email Sell Sheet
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="tools" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="main" Runat="Server">
<div id="emailContainer" class="ssModalContainer sellsheets">
    <div runat="server" id="divEmail">
    <table>    
        <tr>
            <td align="left"><span class="ssBold">E-mail Address:</span></td>
            <td align="left">
                <asp:TextBox ID="txtEmail" runat="server" Width="250px" />
                <pinso:ClientValidator ID="vldEmail" runat="server" Text="Invalid - Email address is required." Target="txtEmail" Required="true"  RegExp="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
            </td>
        </tr>
        <tr>
            <td>
                <br />
            </td>
        </tr>
        <tr>
            <td align="left"><span class="ssBold">Subject:</span></td>
            <td align="left">
                <asp:TextBox ID="txtSubject" runat="server" Width="250px"/>
                <pinso:ClientValidator ID="vldSubject" runat="server" Text="Invalid - Subject is required." Target="txtSubject" Required="true" />
            </td>
        </tr>
        <tr>
            <td>
                <br />
            </td>
        </tr>
        <tr>
            <td align="left"><span class="ssBold">Comments:</span></td>
            <td align="left">
                <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" Width="250px" Height="100px"></asp:TextBox>
                <pinso:ClientValidator ID="vldComments" runat="server" Text="Invalid - Comment is required." Target="txtComments" Required="true" />
            </td>
        </tr>
    </table>

    <pinso:CustomButton ID="btnSubmit" runat="server" Text="Submit" CssClass="postback" OnClick="btnSubmit_Click" OnClientClick="return ValidatePage();"/>
    <pinso:CustomButton id="btnCancel" runat="server" Text="Cancel" OnClientClick="javascript:CloseWin(); return true;" />
    </div>
</div>

<asp:Label ID="lblMessage" runat="server" Visible="false"></asp:Label>
</asp:Content>

