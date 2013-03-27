<%@ Control Language="C#" AutoEventWireup="true" CodeFile="maintab.ascx.cs" Inherits="controls_maintab" %>
  <div id="mainTab">
    <ul class="ui-tabs-nav">
      <li id="A1" runat="server" class="selected"><span><a href="home.aspx">Home</a></span></li>
      <li id="A2" runat="server" class="default"><span><a href="campaigns_current.aspx">Campaigns</a></span></li>
      <li id="A3" runat="server" class="default"><span><a href="mycampaigns_current.aspx">My Campaigns</a></span></li>
      <li id="A4" runat="server" class="default"><span><a href="reports.aspx">Dashboard</a></span></li>
      
      <%--<li id="A5" runat="server" class="default" visible="false"><span><a href="UpdateGoalsTargets.aspx">Admin</a></span></li>--%>
    </ul>
</div>