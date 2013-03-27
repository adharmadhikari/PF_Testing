<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SSPreviewControl1MedD.ascx.cs" Inherits="custom_dey_sellsheets_controls_SSPreviewControl1MedD" %>
<%--   <div style="height:120px;">
</div> --%>
<div align="center">
    <table width="85%" border="0" cellpadding="0" cellspacing="0">
        <tr align="right">
            <td style="font-weight:bold;"><h2><asp:Label runat="server" ID="geolbl"></asp:Label></h2></td>
        </tr>
        <tr>
            <td width="100%">&nbsp;</td>
        </tr>
        <tr align="right">
            <td style="font-weight:bold;"><h3><asp:Label runat="server" ID="msglbl"></asp:Label></h3></td>
        </tr>
    </table>
    <asp:ListView runat="server" ID="ReviewPlansListView" 
        DataSourceID="dsSellSheetReviewPlansList" GroupItemCount="10" 
        DataKeyNames="Segment_Name" OnItemDataBound="ReviewPlanListView_OnItemDataBound">
        <LayoutTemplate>
         <table cellpadding="2" runat="server" id="tblSegment" border="1" width="85%" class="reviewPlans">
            <tr runat="server" id="groupPlaceholder">
            </tr>
         </table>
        </LayoutTemplate>
        <GroupTemplate>
            
         <tr runat="server" id="plansRow" class="gridHdr"> 
            <th id="Header1" runat="server" width="5%" colspan="1" align="left">
                &nbsp;
            </th>
            <th id="Header2" runat="server" width="5%" colspan="1" >
                Your Product
            </th>
            <th id="Header3" runat="server" width="5%" colspan="1" >
                Competitor1
            </th>
            <th id="Header4" runat="server" width="5%" colspan="1" >
                Competitor2
            </th>
          </tr>
          <tr runat="server" id="itemPlaceholder">
          </tr>
          <tr runat="server" id="plansFooter" class="gridFooter">
            <td id="Footer1" runat="server" width="5%" colspan="1" class="bottom" >
              </td>
              <td id="Footer2" runat="server" width="5%" colspan="1" class="bottom" >

              </td>
              <td id="Footer3" runat="server" width="5%" colspan="1"  class="bottom" >
              </td>
              <td id="Footer4" runat="server" width="5%" colspan="1"  class="bottom" >
              
              </td>
          </tr>
        </GroupTemplate>
            <ItemTemplate>
          <tr id="Tr1" runat="server" align="left" >
              <th id="Th2" runat="server" width="15%" align="left">
                <asp:Label ID="Label1" runat="server" Text='<%#Eval("Segment_Name") %>' />
              </th>
              <th id="Tierheader1" runat="server" width="7%" runat="server"  Visible='<%# (bool)Eval("Tier_Indicator") %>'>
                Tier
              </th>
              <th id="CovStatusHeader1" runat="server"  width="7%" runat="server"  Visible='<%# (bool)Eval("Coverage_Indicator") %>'>
                Coverage Status
              </th>
              <th id="FormularyStatusHeader1" runat="server"  width="7%" runat="server"  Visible='<%# (bool)Eval("Formulary_Indicator") %>' >
                 Status
              </th>
              <th id="CopayHeader1" runat="server"  width="7%" runat="server"  Visible='<%# (bool)Eval("Copay_Indicator") %>'>
                Co-Pay
              </th>
              <th id="Tierheader2" runat="server"  width="7%" runat="server"  Visible='<%# (bool)Eval("Tier_Indicator") %>'>
                Tier
              </th>
              <th id="CovStatusHeader2" runat="server"  width="7%" runat="server"  Visible='<%# (bool)Eval("Coverage_Indicator") %>'>
                Coverage Status
              </th>
              <th id="FormularyStatusHeader2" runat="server"  width="7%" runat="server"  Visible='<%# (bool)Eval("Formulary_Indicator") %>' >
                 Status
              </th>
              <th id="CopayHeader2" runat="server"  width="7%" runat="server"  Visible='<%# (bool)Eval("Copay_Indicator") %>'>
                Co-Pay
              </th>
              <th id="Tierheader3" runat="server"  width="7%" runat="server" Visible='<%# (bool)Eval("Tier_Indicator") %>' >
                Tier
              </th>
              <th id="CovStatusHeader3" runat="server"  width="7%" runat="server"  Visible='<%# (bool)Eval("Coverage_Indicator") %>'>
                Coverage Status
              </th>
              <th id="FormularyStatusHeader3" runat="server"  width="7%" runat="server"  Visible='<%# (bool)Eval("Formulary_Indicator") %>' >
                 Status
              </th>
              <th id="CopayHeader3" runat="server"  width="7%" runat="server"  Visible='<%# (bool)Eval("Copay_Indicator") %>'>
                Co-Pay
              </th>
         </tr>
         <tr id="Tr2" runat="server"  class='<%#Eval("HighlightIndicator") %>'>
           <td width="15%" class="PlanNMHighlight content">  
             <asp:Label ID="PlanNamelbl" runat="server" Text='<%#Eval("Plan_Name") %>' />
           </td>
           <td width="7%" id="Tier1Data" runat="server" Visible='<%# (bool)Eval("Tier_Indicator") %>' class="content alignCenter">  
             <asp:Label ID="Tier1lbl" runat="server" Text='<%#Eval("FirstTierName") %>' />&nbsp;
           </td>
           <td width="7%" id="Cov1Data" runat="server" Visible='<%# (bool)Eval("Coverage_Indicator") %>' class="content alignCenter">  
             <asp:Label ID="Cov1lbl" runat="server" Text='<%#Eval("FirstCoverageStatus") %>'/>&nbsp;
           </td>
            <td width="7%" id="Formulary1Data" runat="server" Visible='<%# (bool)Eval("Formulary_Indicator") %>' class="content alignCenter">  
             <asp:Label ID="Formulary1lbl" runat="server" Text='<%#Eval("FirstFormularyStatus") %>'/>&nbsp;
           </td>
           <td width="7%" id="Copay1Data" runat="server"  Visible='<%# (bool)Eval("Copay_Indicator") %>' class="content alignCenter">  
             <asp:Label ID="Copay1lbl" runat="server" Text='<%#Eval("FirstCopay") %>' />&nbsp;
           </td>
           <td width="7%" id="Tier2Data" runat="server" Visible='<%# (bool)Eval("Tier_Indicator") %>' class="content alignCenter">  
             <asp:Label ID="Tier2lbl" runat="server" Text='<%#Eval("SecondTierName") %>' />&nbsp;
           </td>
           <td width="7%" id="Cov2Data" runat="server"  Visible='<%# (bool)Eval("Coverage_Indicator") %>' class="content alignCenter">  
             <asp:Label ID="Cov2lbl" runat="server" Text='<%#Eval("SecondCoverageStatus") %>' />&nbsp;
           </td>
            <td width="7%" id="Formulary2Data" runat="server" Visible='<%# (bool)Eval("Formulary_Indicator") %>' class="content alignCenter">  
             <asp:Label ID="Formulary2lbl" runat="server" Text='<%#Eval("SecondFormularyStatus") %>'/>&nbsp;
           </td>
           <td width="7%" id="Copay2Data" runat="server"  Visible='<%# (bool)Eval("Copay_Indicator") %>' class="content alignCenter">  
             <asp:Label ID="Copay2lbl" runat="server" Text='<%#Eval("SecondCopay") %>' />&nbsp;
           </td>
           <td width="7%" id="Tier3Data" runat="server" Visible='<%# (bool)Eval("Tier_Indicator") %>' border="1" class="content alignCenter">  
             <asp:Label ID="Tier3lbl" runat="server" Text='<%#Eval("ThirdTierName") %>' />&nbsp;
           </td>
           <td width="7%" id="Cov3Data" runat="server"  Visible='<%# (bool)Eval("Coverage_Indicator") %>' border="1" class="content alignCenter">  
             <asp:Label ID="Cov3lbl" runat="server" Text='<%#Eval("ThirdCoverageStatus") %>' />&nbsp;
           </td>
           <td width="7%" id="Formulary3Data" runat="server" Visible='<%# (bool)Eval("Formulary_Indicator") %>' class="content alignCenter">  
             <asp:Label ID="Formulary3lbl" runat="server" Text='<%#Eval("ThirdFormularyStatus") %>'/>&nbsp;
           </td>
           <td width="7%" id="Copay3Data" runat="server"  Visible='<%# (bool)Eval("Copay_Indicator") %>' border="1" class="content alignCenter">  
             <asp:Label ID="Copay3lbl" runat="server" Text='<%#Eval("ThirdCopay") %>' />&nbsp;
           </td>
         </tr>
        </ItemTemplate>
    </asp:ListView>
    <table width="85%" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <asp:Panel id="pnlFooter" runat="server" CssClass="pnlFooter">
                    Formulary data provided by Pinsonault Associates, LLC, <asp:Label ID="footerdatelbl" runat="server" Text="March 2010." ></asp:Label>
                    <br />
                    <br />
                    Plan formularies will vary and are subject to change without notice. Please check directly with the health plan to determine the most up to date formulary information.    
                </asp:Panel>    
            </td>
        </tr>
    </table>
  
    <asp:SqlDataSource ID="dsSellSheetReviewPlansList" runat="server" 
    SelectCommand="usp_SellSheet_ReviewPlanSelectionList"
    SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:QueryStringParameter  Name="Sell_Sheet_ID"  QueryStringField="Sell_Sheet_ID" DbType ="Int32"  />
            <asp:SessionParameter Name="User_ID" SessionField="UserID" />  
        </SelectParameters>  
    </asp:SqlDataSource>       
</div>