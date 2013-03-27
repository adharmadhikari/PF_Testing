<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Modal.master" AutoEventWireup="true" CodeFile="OpenTierCoveragePieChart.aspx.cs" Inherits="standardreports_all_OpenTierCoveragePieChart" %>
<%@ Register src="../controls/TierCoveragePieChart.ascx" tagname="TierCoveragePieChart" tagprefix="pinso" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script type="text/javascript">
     function CloseWin()
     {
         var manager = window.top.GetRadWindowManager();

         var window1 = manager.getWindowByName("PieChart");
         if (window1 != null)
         { window1.close(); }
     }

     window.onload = function() {
         $('object').removeAttr("style");
     }

 </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="title" Runat="Server">
Tier Coverage Pie Chart
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="tools" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="main" Runat="Server">
<div id="divpiechart">
      <pinso:TierCoveragePieChart ID="TierCoveragePieChart1" runat="server"  Thumbnail="true" />
</div>
</asp:Content>

