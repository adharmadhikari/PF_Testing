<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Modal.master" AutoEventWireup="true" CodeFile="DocumentUpload.aspx.cs" Inherits="custom_pinso_customercontactreports_all_DocumentUpload" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script type="text/javascript">
    function ClearForm() 
    {
        $("#AddCCRMain input[type != submit]").val("");
        $("TEXTAREA").val("");
    }

    function RefreshGrid()
    {
        return window.top.$find("ctl00_ctl00_Tile3_Tile7_BusinessDocument1_gridCCDocuments");
    }

    function RefreshDDRs()
    {
        RefreshGrid().get_masterTableView().rebind();
        var oManager = window.top.GetRadWindowManager();   
        window.setTimeout(CloseWin, 1000);
    }
    
    function ConfirmMsg() 
    {
        window.setTimeout(CloseWin, 1000);
    }
    
    function CloseWin() 
    {
        var manager = window.top.GetRadWindowManager();

        var window1 = manager.getWindowByName("AddCCR");
        if (window1 != null)
            window1.close();
    }
    </script>  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="title" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="tools" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="main" Runat="Server">

<table style="width:100%; padding-top: 5px;"  align="center">
    <tr>
        <td style="width:50%;" align="left">
            <table>
                <tr>
                    <td align="left" >
                        <label style="text-align:left; font-family: Arial; font-size: 11px; font-weight: bold; padding-top: 3px;">
                            Document Upload</label>
                    </td>
                    <td id="bpsFile" align="right" valign="middle">
                        <asp:FileUpload ID="FileUpload1" runat="server" style="height: 20px;">
                        </asp:FileUpload>
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
        </td>
        <td style="width:50%;" align="left">
            <table>
                <tr>
                    <td align ="left">
                        <label style="font-family: Arial; font-size: 11px; font-weight: bold">
                            Document Name</label>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="bpsName" runat="server" style="height: 16px" />
                    </td>
                    <td>
                        <asp:Button ID="btnUpload" runat="server" Text="Upload" onclick="btnUpload_Click" style="padding-left: 2px; padding-right: 2px" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:RequiredFieldValidator ControlToValidate="bpsName" runat="server" ErrorMessage='<%$ Resources:Resource, Label_Upload_Document_Name_Validator %>' Display="None" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="FileUpload1" runat="server" ErrorMessage='<%$ Resources:Resource, Label_File_Upload_Validator %>'
    Display="None" />   
            <asp:Label ID = "hdnLbl" runat ="server" Text="" Visible ="false"></asp:Label>
        </td>
    </tr>
</table>
<div>
    <asp:ValidationSummary runat="server" ID="validationSummary" DisplayMode="BulletList" />
</div>

</asp:Content>

