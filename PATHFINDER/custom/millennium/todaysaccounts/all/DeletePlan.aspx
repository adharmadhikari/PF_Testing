<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Modal.master" AutoEventWireup="true" CodeFile="DeletePlan.aspx.cs" Inherits="custom_Millennium_customercontactreports_all_DeletePlan" %>

<%@ Register src="../controls/DeletePlan.ascx" tagname="DeletePlan" tagprefix="pinso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script type="text/javascript">
   function CloseWin() 
    {
        var manager = window.top.GetRadWindowManager();
        var window1 = manager.getWindowByName("DelPlan");
        window1.close();
    }

     function RefreshPlan() 
    {

        window.setTimeout(CloseWin, 4000);
        return window.top.$find("ctl00_main_planInfo_gridPlanInfo").get_masterTableView().rebind();
    }
     </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="title" Runat="Server">
  <asp:Label ID="titleText" runat="server" Text="Delete Selected Plan"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="tools" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="main" Runat="Server">
    <pinso:DeletePlan ID="RemoveCCR" runat="server" />
</asp:Content>