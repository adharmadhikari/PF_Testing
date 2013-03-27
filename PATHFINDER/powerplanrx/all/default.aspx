<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SingleSection.master" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="powerplanrx_all_default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="scriptContainer" Runat="Server">
    <script type="text/javascript">
//        var w = window.open("http://beta.pinsonault.com/mms_tademo/contents/pt_opportunities.asp", "PPRx");
//        w.focus();
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Tile3Title" Runat="Server">
    PowerPlanRx
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Tile3Tools" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Tile3" Runat="Server">
    <iframe frameborder="0" style="width:100%;height:100%" src="http://www.powerplanrx.com/home.aspx"></iframe>
</asp:Content>

