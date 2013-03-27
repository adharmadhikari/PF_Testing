<%@ Page Title="" Language="C#" MasterPageFile="~/custom/MasterPages/SellSheetStep.master" AutoEventWireup="true" CodeFile="reviewplanselection.aspx.cs" Inherits="custom_pinso_sellsheets_reviewplanselection"  EnableViewState="true"  %>
<%@ MasterType VirtualPath="~/custom/MasterPages/SellSheetStep.master" %>
<%@ Register src="~/custom/pinso/sellsheets/controls/ReviewPlanSelectionScript.ascx" tagname="ReviewPlanSelectionScript" tagprefix="pinso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="scriptContainer" Runat="Server">
    <pinso:ReviewPlanSelectionScript ID="ReviewPlanSelectionScript1" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="StepBody" Runat="Server">
  <div id="reviewPlansContainer">
    <asp:HiddenField ID="hdnPlansHighlighted" runat="server" Value="" />    

    <asp:FormView  runat="server" ID="formSSPlans" DefaultMode="ReadOnly"   
           DataSourceID="dsSellSheetMast" CellPadding="0" CellSpacing="0" Width="100%">
    <ItemTemplate>
            <asp:HiddenField ID="hdnTypeID" runat="server" Value='<%# Eval("Type_ID") %>' />    
            <asp:HiddenField ID="hdnTypeName" runat="server" Value='<%# Eval("Type_Name") %>' />        
            <asp:HiddenField ID="hdnCopay" runat="server" Value='<%# Eval("Use_Copay") %>' />        
    </ItemTemplate> 
    </asp:FormView> 
       
    <asp:ListView runat="server" ID="ReviewPlansListView" 
        DataSourceID="dsSellSheetReviewPlansList" GroupItemCount="10" 
        DataKeyNames="Segment_Name" OnItemDataBound="ReviewPlanListView_OnItemDataBound" >
        <LayoutTemplate>
        <div id="SSReviewPlanList"  runat="server" style="height:280px; overflow:auto;">
         <table cellpadding="2" runat="server" id="tblSegment" border="1" class="reviewPlans" width="99%">
            <tr runat="server" id="groupPlaceholder">
            </tr>
         </table>
        </div>
        </LayoutTemplate>
        <GroupTemplate>
         <tr runat="server" id="plansRow" class="gridHdr" > 
            <th id="Header1" runat="server" width="5%" colspan="2" class="left" >
              </th>
              <th id="Header2" runat="server" width="5%" colspan="2" >
                Your Product
              </th>
              <th id="Header3" runat="server" width="5%" colspan="2"  >
                Competitor1
              </th>
              <th id="Header4" runat="server" width="5%" colspan="2"  >
                Competitor2
              </th>
          </tr>
          <tr runat="server" id="itemPlaceholder">
          </tr>
          <tr runat="server" id="plansFooter">
            <td id="Footer1" runat="server" width="5%" colspan="2" class="bottom">
              </td>
              <td id="Footer2" runat="server" width="5%" colspan="2" class="bottom">
              </td>
              <td id="Footer3" runat="server" width="5%" colspan="2"  class="bottom">
              </td>
              <td id="Footer4" runat="server" width="5%" colspan="2"  class="bottom">
              </td>
          </tr>
        </GroupTemplate>
        <ItemTemplate>
          <tr id="Tr1" runat="server" align="left">
              <th id="Th1" runat="server" width="2%">
                Bold
              </th>
              <th id="Th2" runat="server" width="15%" align="left">
                <asp:Label ID="Label1" runat="server" Text='<%#Eval("Segment_Name") %>' />
              </th>
              <th id="Tierheader1" runat="server" width="7%" runat="server"  Visible='<%# (bool)Eval("Tier_Indicator") %>' >
                Tier
              </th>
              <th id="CovStatusHeader1" runat="server"  width="7%" runat="server"  Visible='<%# (bool)Eval("Coverage_Indicator") %>' >
                Coverage Status
              </th>
              <th id="CopayHeader1" runat="server" width="7%" runat="server"  Visible='<%# (bool)Eval("Copay_Indicator") %>' >
                Co-Pay
              </th>
              <th id="Tierheader2" runat="server"  width="7%" runat="server"  Visible='<%# (bool)Eval("Tier_Indicator") %>' >
                Tier
              </th>
              <th id="CovStatusHeader2" runat="server"  width="7%" runat="server"  Visible='<%# (bool)Eval("Coverage_Indicator") %>'>
                Coverage Status
              </th>
              <th id="CopayHeader2" runat="server"  width="7%" runat="server"  Visible='<%# (bool)Eval("Copay_Indicator") %>' >
                Co-Pay
              </th>
              <th id="Tierheader3" runat="server"  width="7%" runat="server" Visible='<%# (bool)Eval("Tier_Indicator") %>' >
                Tier
              </th>
              <th id="CovStatusHeader3" runat="server"  width="7%" runat="server"  Visible='<%# (bool)Eval("Coverage_Indicator") %>'>
                Coverage Status
              </th>
              <th id="CopayHeader3" runat="server"  width="7%" runat="server"  Visible='<%# (bool)Eval("Copay_Indicator") %>' >
                Co-Pay
              </th>
         </tr>
         <tr id="Tr2" runat="server"  class='<%#Eval("HighlightIndicator") %>' >
           <td width="2%">
             <asp:CheckBox ID="chkIsHighlighted" runat="server" Checked='<%#Eval("Is_Highlighted") %>' 
             onclick='<%# string.Format("PlansHighlightChanged(this,{0},{1},{2})", Eval("Plan_ID"), Eval("Formulary_ID"), Eval("Product_ID")) %>' />
           </td>
           <td width="15%" class="PlanNMHighlight">  
             <asp:Label ID="PlanNamelbl" runat="server" Text='<%#Eval("Plan_Name") %>' />
           </td>
           <td width="7%" id="Tier1Data" runat="server" Visible='<%# (bool)Eval("Tier_Indicator") %>' >  
             <asp:Label ID="Tier1lbl" runat="server" Text='<%#Eval("FirstTierName") %>' />
           </td>
           <td width="7%" id="Cov1Data" runat="server" Visible='<%# (bool)Eval("Coverage_Indicator") %>' >  
             <asp:Label ID="Cov1lbl" runat="server" Text='<%#Eval("FirstCoverageStatus") %>'/>
           </td>
           <td width="7%" id="Copay1Data" runat="server" align="right" Visible='<%# (bool)Eval("Copay_Indicator") %>' class="right">  
             <asp:Label ID="Copay1lbl" runat="server" Text='<%#Eval("FirstCopay") %>' />
           </td>
           <td width="7%" id="Tier2Data" runat="server" Visible='<%# (bool)Eval("Tier_Indicator") %>'>  
             <asp:Label ID="Tier2lbl" runat="server" Text='<%#Eval("SecondTierName") %>' />
           </td>
           <td width="7%" id="Cov2Data" runat="server"  Visible='<%# (bool)Eval("Coverage_Indicator") %>'>  
             <asp:Label ID="Cov2lbl" runat="server" Text='<%#Eval("SecondCoverageStatus") %>' />
           </td>
           <td width="7%" id="Copay2Data" runat="server" align="right" Visible='<%# (bool)Eval("Copay_Indicator") %>' class="right">  
             <asp:Label ID="Copay2lbl" runat="server" Text='<%#Eval("SecondCopay") %>' />
           </td>
           <td width="7%" id="Tier3Data" runat="server" Visible='<%# (bool)Eval("Tier_Indicator") %>'>  
             <asp:Label ID="Tier3lbl" runat="server" Text='<%#Eval("ThirdTierName") %>' />
           </td>
           <td width="7%" id="Cov3Data" runat="server"  Visible='<%# (bool)Eval("Coverage_Indicator") %>'>  
             <asp:Label ID="Cov3lbl" runat="server" Text='<%#Eval("ThirdCoverageStatus") %>' />
           </td>
           <td width="7%" id="Copay3Data" runat="server" align="right" Visible='<%# (bool)Eval("Copay_Indicator") %>' class="right">  
             <asp:Label ID="Copay3lbl" runat="server" Text='<%#Eval("ThirdCopay") %>' />
           </td>
         </tr>
        </ItemTemplate>
    </asp:ListView>
  
  <asp:EntityDataSource ID="dsSellSheetMast" EntitySetName="SellSheetMastSet" runat="server" ConnectionString="name=PathfinderClientEntities" DefaultContainerName="PathfinderClientEntities" 
        AutoGenerateWhereClause="true">
        <WhereParameters>       
            <asp:QueryStringParameter QueryStringField="Sell_Sheet_ID" Name="Sell_Sheet_ID" Type="Int32" ConvertEmptyStringToNull="true"/>
        </WhereParameters>
  </asp:EntityDataSource>    
    
  <asp:SqlDataSource ID="dsSellSheetReviewPlansList" runat="server"  
  
  SelectCommand="usp_SellSheet_ReviewPlanSelectionList"
   SelectCommandType="StoredProcedure">
 <SelectParameters>
     <asp:QueryStringParameter  Name="Sell_Sheet_ID"  QueryStringField="Sell_Sheet_ID" DbType ="Int32"  />
     <asp:SessionParameter Name="User_ID" SessionField="UserID" />  
 </SelectParameters>  
 </asp:SqlDataSource>   

 <input type="button" class="btnPrev"
        onclick="clientManager.get_ApplicationManager().back(clientManager)"  
        value="Back" />
    <asp:button ID="btnNextStep" runat="server" Text="Next" CssClass="postback validate btnNext" 
        onclick="btnNextStep_Click" />
  
  </div>  
</asp:Content>

  
  
