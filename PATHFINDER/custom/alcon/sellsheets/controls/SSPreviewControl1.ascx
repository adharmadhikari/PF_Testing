<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SSPreviewControl1.ascx.cs" Inherits="custom_controls_SSPreviewControl1" %>
<div style="height:320px;">
</div>
 <div align="center">
    <table width="85%" border="0" cellpadding="0" cellspacing="0" >
        <tr align="right">
            <td style="font-weight:bold;"><h2><asp:Label runat="server" ID="geolbl"></asp:Label></h2></td>
        </tr>
        <tr>
            <td width="100%">&nbsp;</td>
        </tr>
        <tr align="center">
            <td style="font-family:Helvetica Neu Lt Std Condensed; font-size:23px;"><asp:Label runat="server" ID="msglbl"></asp:Label></td>
        </tr>
    </table>
    <div style="height:15px;"></div>
    <asp:ListView runat="server" ID="ReviewPlansListView" 
        DataSourceID="dsSellSheetReviewPlansList" GroupItemCount="13" 
        DataKeyNames="Segment_Name" 
         OnItemDataBound="ReviewPlanListView_OnItemDataBound" 
         ondatabound="ReviewPlansListView_DataBound">
        <LayoutTemplate>
         <table cellpadding="2" width="95%" runat="server" id="tblSegment" border="1" class="reviewPlans" style="font-family:Helvetica Neu Lt Std Condensed; font-size:13px; font-weight:bold ">
            <tr runat="server" id="groupPlaceholder">
                <td>
                    test test
                </td>
            </tr>
         </table>
        </LayoutTemplate>
        <GroupTemplate>
         <tr runat="server" id="plansRow" class="gridHdr" style="font-size: 14px;"> 
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
            <th id="Header5" runat="server" width="5%" colspan="1" >
                Competitor3
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
              <td id="Footer5" runat="server" width="5%" colspan="1"  class="bottom" >
              
              </td>
          </tr>
        </GroupTemplate>
        <ItemTemplate>
          <tr id="Tr1" runat="server" align="left">
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
              <th id="Tierheader4" runat="server"  width="7%" runat="server" Visible='<%# (bool)Eval("Tier_Indicator") %>' >
                Tier
              </th>
              <th id="CovStatusHeader4" runat="server"  width="7%" runat="server"  Visible='<%# (bool)Eval("Coverage_Indicator") %>'>
                Coverage Status
              </th>
              <th id="FormularyStatusHeader4" runat="server"  width="7%" runat="server"  Visible='<%# (bool)Eval("Formulary_Indicator") %>' >
                 Status
              </th>
              <th id="CopayHeader4" runat="server"  width="7%" runat="server"  Visible='<%# (bool)Eval("Copay_Indicator") %>'>
                Co-Pay
              </th>
         </tr>
         <tr id="Tr2" runat="server"  class='<%#Eval("HighlightIndicator") %>'>
           <td width="15%" class="PlanNMHighlight content">  
             <asp:Label ID="PlanNamelbl" runat="server" Text='<%# ConvertSpecialCharsToHTML(Convert.ToString(Eval("Plan_Name")))%>' UseMnemonic="true"/>
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
             <asp:Label ID="Formulary2lbl" runat="server"  Text='<%#Eval("SecondFormularyStatus") %>'/>&nbsp;
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
             <asp:Label ID="Formulary3lbl" runat="server"  Text='<%#Eval("ThirdFormularyStatus") %>'/>&nbsp;
           </td>
           <td width="7%" id="Copay3Data" runat="server"  Visible='<%# (bool)Eval("Copay_Indicator") %>' border="1" class="content alignCenter">  
             <asp:Label ID="Copay3lbl" runat="server" Text='<%#Eval("ThirdCopay") %>' />&nbsp;
           </td>
           <td width="7%" id="Tier4Data" runat="server" Visible='<%# (bool)Eval("Tier_Indicator") %>' border="1" class="content alignCenter">  
             <asp:Label ID="Tier4lbl" runat="server" Text='<%#Eval("FourthTierName") %>' />&nbsp;
           </td>
           <td width="7%" id="Cov4Data" runat="server"  Visible='<%# (bool)Eval("Coverage_Indicator") %>' border="1" class="content alignCenter">  
             <asp:Label ID="Cov4lbl" runat="server" Text='<%#Eval("FourthCoverageStatus") %>' />&nbsp;
           </td>
           <td width="7%" id="Formulary4Data" runat="server" Visible='<%# (bool)Eval("Formulary_Indicator") %>' class="content alignCenter">  
             <asp:Label ID="Formulary4lbl" runat="server"  Text='<%#Eval("FourthFormularyStatus") %>'/>&nbsp;
           </td>
           <td width="7%" id="Copay4Data" runat="server"  Visible='<%# (bool)Eval("Copay_Indicator") %>' border="1" class="content alignCenter">  
             <asp:Label ID="Copay4lbl" runat="server" Text='<%#Eval("FourthCopay") %>' />&nbsp;
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
                    <asp:Label ID="restriction_lbl" runat="server" /> 
<%--                    Plan formularies will vary and are subject to change without notice. Please check directly with the health plan to determine the most up to date formulary information.    
--%>               </asp:Panel>    
            </td>
        </tr>
    </table>
  
    <asp:SqlDataSource ID="dsSellSheetReviewPlansList" runat="server" 
    SelectCommand="usp_SellSheet_PreviewPlanSelectionList"
    SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:QueryStringParameter  Name="Sell_Sheet_ID"  QueryStringField="Sell_Sheet_ID" DbType ="Int32"  />
            <asp:SessionParameter Name="User_ID" SessionField="UserID" />  
        </SelectParameters>  
    </asp:SqlDataSource>       
</div> 
