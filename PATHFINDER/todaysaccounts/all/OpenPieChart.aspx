<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Modal.master" AutoEventWireup="true" CodeFile="OpenPieChart.aspx.cs" Inherits="todaysaccounts_all_OpenPieChart" %>
<%@ Register src="../controls/PieChart.ascx" tagname="PieChart" tagprefix="pinso" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">



</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="title" Runat="Server">
Pie Chart
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="tools" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="main" Runat="Server">
<script type="text/javascript">
    //Hack to fix width/height issue
    $(document).ready(function()
    {
        $('object').removeAttr('style');
        
        //For Chrome and Firefox
        $('embed').removeAttr('style');
    });
</script>
<div id="divpiechart">
      <pinso:PieChart ID="PieChart1" runat="server"  Thumbnail="true" />
</div>
</asp:Content>

