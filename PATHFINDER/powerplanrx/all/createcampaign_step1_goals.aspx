<%@ Page Language="C#" Theme="impact" Title="PowerPlanRx - Step 1 Goals" MasterPageFile="~/powerplanrx/MasterPages/MasterPageGoal.master" AutoEventWireup="true" CodeFile="createcampaign_step1_goals.aspx.cs" Inherits="createcampaign_step1_goals" %>
<%@ Register Src="~/powerplanrx/controls/Goals.ascx" TagName="Goals" TagPrefix="pinso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="goalButton" Runat="Server">

<div class="impactBtns" id="divButtons" runat="server" > 
           <div id="divEditBtns" runat="server" >
            <span class="coreBtn"><span class="bg"><span class="bg2">
                <asp:Button ID="btnEdit" runat="server" Text="Edit" onclick="btn_edit_Click"  />
            </span></span></span></div>         
           <div id="divSubmitBtns" runat="server" visible="false">
              <span class="coreBtn"><span class="bg"><span class="bg2">
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" onclick="btn_submit_Click" 
                               UseSubmitBehavior="false" OnClientClick="javascript: if (submitValidate(this)) {this.disabled=true; this.value='Submitting'; usesubmitbehavior='false';} else { return false; };"   />
            </span></span></span>
            <span class="coreBtn"><span class="bg"><span class="bg2">
                <%--<asp:Button ID="Button1" runat="server" Text="Cancel" CausesValidation="false" />
            --%>
             <input type="reset" name="reset" id="reset" value="Reset" onclick="window.location.reload();">
	       
            
            </span></span></span></div>
        </div>



</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="main" Runat="Server">

    
    <style>
        #ctl00_main_goals_gridViewDistricts_ctl00_Pager
        {
        	padding-right:0px !important;
        }
    </style>
<script  type="text/javascript">
    $(document).ready(GoalsLoaded);
</script>


    <asp:HiddenField runat="server" ID="targetedDistricts" />
  
    <pinso:Goals runat="server" ID="goals" ShowResults="false" />

<%--    <div style="font-size:9pt;font-weight:bold;">
        <div style="width:135px;float:left;">Targeted Volume:</div><div style="float:left;width:65px;text-align:right;" id="targetedTrxPercent"></div>
        <div class="clearAll"></div>
        <div style="width:135px;float:left;">Non-Targeted Volume:</div><div style="float:left;width:65px;text-align:right;" id="notTargetedTrxPercent"></div>
        <div class="clearAll"></div>
     </div>--%>
</asp:Content>

  
  
      
      
        
        
        
        
        






