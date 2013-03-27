<%@ Control Language="C#" AutoEventWireup="true" CodeFile="subtab.ascx.cs" Inherits="controls_subtab" %>
  <div id="subTab" class="impactModules">

      <!--Status Box -->
    <div runat="server" id="statusBox" class="impactStatusBox" visible="false">
    <div id="impactStatus" class="impactStatusBox">
        <div class='impactStatusTextBox'>
            <div class='impactStatusHdr'>Current Campaign Status</div>
            <div class='impactStatusLast'><%= string.Format("Last Task Completed - {0}", PreviousStep > 0 ? string.Format("Step {0}: {1}", PreviousStep, PreviousPhaseName) : PreviousPhaseName) %></div>
            <div class='impactStatusCurrent'><%= string.Format("Next Task - Step {0}: {1}", CurrentStep, CurrentPhaseName) %></div>
        </div>
        <div id="impactStatusImg" class='<%= string.Format("impactStatusImgBox impactStatusImg_step{0}", CurrentStep) %>'></div>                
    </div>
    </div>

    <ul class="ui-tabs-nav" id="tabCampaigns" runat="server">
      <li id="c1" runat="server"><span><a href="campaigns_current.aspx">All Current Campaigns</a></span></li>
      <li id="c2" runat="server"><span><a href="campaigns_archived.aspx">All Archived Campaigns</a></span></li>
    </ul>
    <ul class="ui-tabs-nav" id="tabMyCampaigns" runat="server">
      <li id="mc1" runat="server"><span><a href="mycampaigns_current.aspx">Current Campaigns</a></span></li>
      <li id="mc2" runat="server"><span><a href="mycampaigns_opportunities.aspx">Campaign Opportunities</a></span></li>
    </ul>    
    
    <ul class="ui-tabs-nav" id="tabSteps" runat="server">
        <li id="step0" runat="server"><span><a href="mycampaigns_opportunities.aspx">Opportunity<br />Assessment<br />&nbsp;</a></span></li>
      <li id="step1" runat="server"><span><a href="createcampaign_step1_profile.aspx?id=<%=Request.QueryString["id"] %>"> <br />Planning<br />&nbsp;</a></span></li>
      <li id="step2" runat="server"><span><a href="createcampaign_step2_tactics.aspx?id=<%=Request.QueryString["id"] %>"> <br />Communication<br />&nbsp;</a></span></li>
      <li id="step3" runat="server"><span><a href="createcampaign_step3_approval.aspx?id=<%=Request.QueryString["id"] %>"> <br />Execution<br />&nbsp;</a></span></li>
      <li id="step4" runat="server"><span><a href="createcampaign_step4_results.aspx?id=<%=Request.QueryString["id"] %>"> <br />Results<br />&nbsp;</a></span></li>
    </ul>        
    
    <div class="clearAll"></div>

</div>
    <div id="impactSubmodules">
        <ul class="ui-tabs-nav" id="step1Tabs" runat="server" visible="false">
          <li id="s1Profile" runat="server" class="default"><span><a href="createcampaign_step1_profile.aspx?id=<%=Request.QueryString["id"] %>">Plan Profile</a></span></li>
          <li id="s1Timeline" runat="server" class="default"><span><a href="createcampaign_step1_timeline.aspx?id=<%=Request.QueryString["id"] %>">Timeline</a></span></li>
          <li id="s1Goals" runat="server" class="default"><span><a href="createcampaign_step1_goals.aspx?id=<%=Request.QueryString["id"] %>">Campaign Goals</a></span></li>
          <li id="s1Team" runat="server" class="default"><span><a href="createcampaign_step1_team.aspx?id=<%=Request.QueryString["id"] %>">Campaign Team</a></span></li>
        </ul>        

        <ul class="ui-tabs-nav" id="step2Tabs" runat="server" visible="false">
          <li id="s2Tactics" runat="server" class="default"><span><a href="createcampaign_step2_tactics.aspx?id=<%=Request.QueryString["id"] %>">Tactics</a></span></li>
          <li id="s2Messages" runat="server" class="default"><span><a href="createcampaign_step2_messages.aspx?id=<%=Request.QueryString["id"] %>">Messages</a></span></li>
           <li id="s2Summary" runat="server" class="default"><span><a href="createcampaign_step2_summary.aspx?id=<%=Request.QueryString["id"] %>">Campaign Summary</a></span></li>
        </ul>        
        
        <ul class="ui-tabs-nav" id="step3Tabs" runat="server" visible="false">
          <li id="s3Approval" runat="server" class="default"><span><a href="createcampaign_step3_approval.aspx?id=<%=Request.QueryString["id"] %>">Campaign Approval</a></span></li>
        </ul>        
        
        <ul class="ui-tabs-nav" id="step4Tabs" runat="server" visible="false">
          <li id="s4Results" runat="server" class="default"><span><a href="createcampaign_step4_results.aspx?id=<%=Request.QueryString["id"] %>">Campaign Results</a></span></li>
          <li id="s4BestPractices" runat="server" class="default"><span><a href="createcampaign_step4_bestpractices.aspx?id=<%=Request.QueryString["id"] %>">Best Practices</a></span></li>
        </ul>  
              

    </div>

