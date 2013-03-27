<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Modal.master" AutoEventWireup="true" CodeFile="SaveSellSheet.aspx.cs" Inherits="custom_unitedthera_sellsheets_SaveSellSheet"  EnableViewState="true"  %>
<%@ OutputCache NoStore="true" Duration="1" VaryByParam="None" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <script type="text/javascript">
     function RefreshPage()
     {
         window.top.clientManager.set_Module("mysellsheets");
         window.setTimeout(CloseWin, 4000);
     }
     
     function CloseWin()
     {
         window.parent.$("#iframeTable").show();
     
         var manager = window.top.GetRadWindowManager();

         var window1 = manager.getWindowByName("SaveSellSheet");
         
         if (window1 != null)
            window1.close();
     }
 </script>  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="title" Runat="Server">
Save
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="tools" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="main" Runat="Server">
<div id="frmDiv" runat="server" class="sellsheets" visible="true">
    <br />
    <asp:Label ID="MainHeader" Text="Save Formulary Sell Sheet To My Sell Sheets" runat="server" align="left" Font-Bold="true" ></asp:Label>
    <table align="center" >
        <tr>
            <td>Please provide a name to save the sell sheet:</td>
        </tr>
        <tr>
            <td >
                 <asp:TextBox ID="txtSSName" Text ="" MaxLength ="255" runat="server" Width="200px" ></asp:TextBox>
            </td>
        </tr>
    </table>
    <pinso:CustomButton ID="btnSave" runat="server" Text="Save" CssClass="postback validate"  OnClick="btnSave_Click"/>
    <pinso:CustomButton id="btnCancel" runat="server" Text="Cancel" OnClientClick="javascript:CloseWin(); return true;" />
</div>
<br />
<asp:Label ID="Msglbl" runat="server" Text="" Visible="false"></asp:Label>
</asp:Content>

