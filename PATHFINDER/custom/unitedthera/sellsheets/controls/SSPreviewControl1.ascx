<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SSPreviewControl1.ascx.cs" Inherits="custom_controls_SSPreviewControl1" %>
    
    <div class="geographyLabel"><asp:Label ID="geolbl" runat="server" /></div>    
   <div style="position:absolute; top: 120px; left: 440px; width: 580px">
    <div class="rnd1" style="margin: 0 5px;"></div> 
    <div class="rnd1" style="margin: 0 3px;"></div> 
    <div class="rnd1" style="margin: 0 2px;"></div> 
    <div class="rnd1" style="margin: 0 1px;"></div>
  
     <asp:ListView runat="server" ID="ReviewPlansListView" 
        DataSourceID="dsSellSheetReviewPlansList" GroupItemCount="10" 
        DataKeyNames="Segment_Name" OnItemDataBound="ReviewPlanListView_OnItemDataBound">
        <LayoutTemplate>
         <table cellpadding="2" runat="server" id="tblSegment" border="1" class="reviewPlans">
             <tr runat="server" id="groupPlaceholder"></tr>
         </table>
        </LayoutTemplate>
        <GroupTemplate>
            
          <tr runat="server" id="itemPlaceholder">
          </tr>
        </GroupTemplate>
        <ItemTemplate>
          <tr id="Tr1" runat="server" align="left" >
              <th id="Planheader1" runat="server" align="left"  style="width: 204px; border-left-width: 3px;" >
                <asp:Label ID="Label1" runat="server" Text='<%#Eval("Segment_Name") %>' /> Plan
              </th>
              <th id="Livesheader1" runat="server"  runat="server" style="width: 117px">
                Pharmacy Lives
              </th>
              <th id="Tierheader1" runat="server"  runat="server" style="width: 118px" Visible='<%# (bool)Eval("Tier_Indicator") %>'>
                ADCIRCA PA/QL/ST
              </th>
              <th id="CopayHeader1" runat="server" runat="server" style="width: 117px" Visible='<%# (bool)Eval("Copay_Indicator") %>'>
                ADCIRCA Co-pay Range
              </th> 
              <th id="CopayCard1" runat="server" runat="server" bgcolor="#84d1d1" class="toprightcomm" style="width: 118px; border-right-width: 3px;" Visible='<%# (bool)Eval("Copay_Indicator") %>'>
                ADCIRCA Co-pay with Card
              </th>               
         </tr>
         <tr id="Tr2" runat="server"  class='<%#Eval("HighlightIndicator") %>'>
           <td id="Plan1Data" runat="server" class="PlanNMHighlight content" style="border-left-width: 3px;">  
             <b><asp:Label ID="PlanNamelbl" runat="server" Text='<%#Eval("Plan_Name") %>' /></b>
           </td>           
           <td id ="Lives1Data" class="content alignCenter">  
             <asp:Label ID="Lives1lbl" runat="server" Text='<%#Eval("Plan_Pharmacy_Lives", "{0:N0}") %>' />
           </td>    
           <td id="Tier1Data" runat="server" Visible='<%# (bool)Eval("Tier_Indicator") %>' class="content alignCenter">  
             <asp:Label ID="Tier2lbl" runat="server" Text='<%#Eval("SecondTierName") %>' />&nbsp;
           </td>
           <td id="Copay1Data" runat="server"  Visible='<%# (bool)Eval("Copay_Indicator") %>' class="content alignCenter">  
             <asp:Label ID="Copay2lbl" runat="server" Text='<%#Eval("SecondCopay") %>' />&nbsp;
           </td>           
           <td id="CopayCard1Data" runat="server" bgcolor="#f4f8f8" class="content alignCenter" style="border-right-width: 3px;" >  
             <div align="center" style="color: #00aba7;">$20</div>
           </td> 
         </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
        <tr id="Tr1" runat="server" align="left" >
              <th id="Planheader1" runat="server" align="left" style="border-left-width: 3px;" >
                <asp:Label ID="Label1" runat="server" Text='<%#Eval("Segment_Name") %>' /> Plan
              </th>
              <th id="Livesheader1" runat="server"  runat="server" >
                Pharmacy Lives
              </th>
              <th id="Tierheader1" runat="server"  runat="server" Visible='<%# (bool)Eval("Tier_Indicator") %>'>
                ADCIRCA PA/QL/ST
              </th>
              <th id="CopayHeader1" runat="server" runat="server"  Visible='<%# (bool)Eval("Copay_Indicator") %>'>
                ADCIRCA Co-pay Range
              </th> 
              <th id="CopayCard1" runat="server" runat="server" Visible='<%# (bool)Eval("Copay_Indicator") %>' style="border-right-width: 3px;">
                ADCIRCA Co-pay with Card
              </th>               
         </tr>
         <tr id="Tr2" runat="server"  class='<%#Eval("HighlightIndicator") %>'>
           <td class="PlanNMHighlight content" bgcolor="#fff1e6" style="border-left-width: 3px;" >  
             <b><asp:Label ID="PlanNamelbl" runat="server" Text='<%#Eval("Plan_Name") %>' /></b>
           </td>           
           <td id ="Lives1Data" bgcolor="#fff1e6" class="content alignCenter">  
             <asp:Label ID="Lives1lbl" runat="server" Text='<%#Eval("Plan_Pharmacy_Lives", "{0:N0}") %>' />
           </td>    
           <td id="Tier1Data" runat="server" bgcolor="#fff1e6" Visible='<%# (bool)Eval("Tier_Indicator") %>' class="content alignCenter">  
             <asp:Label ID="Tier2lbl" runat="server" Text='<%#Eval("SecondTierName") %>' />&nbsp;
           </td>
           <td id="Copay1Data" runat="server" bgcolor="#fff1e6" Visible='<%# (bool)Eval("Copay_Indicator") %>' class="content alignCenter">  
             <asp:Label ID="Copay2lbl" runat="server" Text='<%#Eval("SecondCopay") %>' />&nbsp;
           </td>           
           <td id="CopayCard1Data" runat="server" bgcolor="#ceebeb" class="content alignCenter" style="border-right-width: 3px;">  
             <div align="center" style="color: #00aba7">$20</div>
           </td> 
         </tr>
         
        </AlternatingItemTemplate>
    </asp:ListView>
     <div class="rnd1" style="margin: 0 1px;"></div> 
    <div class="rnd1" style="margin: 0 2px;"></div> 
    <div class="rnd1" style="margin: 0 3px;"></div> 
    <div class="rnd1" style="margin: 0 5px;"></div>
    </div> 
  
   
   
    <asp:Panel id="pnlFooter" runat="server" CssClass="pnlFooter" style="position:absolute; top: 700px; left: 40px;">
        Please note that the displayed information is an aggregation of state or national averages.<br />
        Coverage policies and tier levels may not apply to all insurance products or particular plan <br />
        designs and may vary by patient.  Data accessed from Today's Accounts&reg; on <asp:Label ID="footerdatelbl" runat="server" />, <br />
        Pinsonault Associates - Today's Accounts&reg;.  Plan information will vary and are subject to <br />
        change without notice.  Please check directly with the health plan to determine the most <br />
        up to date formulary information.
    </asp:Panel>        
  
    <asp:SqlDataSource ID="dsSellSheetReviewPlansList" runat="server" 
    SelectCommand="usp_SellSheet_ReviewPlanSelectionList"
    SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:QueryStringParameter  Name="Sell_Sheet_ID"  QueryStringField="Sell_Sheet_ID" DbType ="Int32"  />
            <asp:SessionParameter Name="User_ID" SessionField="UserID" />  
        </SelectParameters>  
    </asp:SqlDataSource>       

