<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SSPreviewControl1MedD.ascx.cs" Inherits="custom_controls_SSPreviewControl1MedD" %>
    <div class="geographyLabel"><asp:Label ID="geolbl" runat="server" /></div>
    <div style="position:absolute; top: 150px; left: 37px; width: 490px">
        <div class="rnd1" style="margin: 0 5px;"></div> 
        <div class="rnd1" style="margin: 0 3px;"></div> 
        <div class="rnd1" style="margin: 0 2px;"></div> 
        <div class="rnd1" style="margin: 0 1px;"></div>
    
    <asp:ListView runat="server" ID="ReviewPlansListViewMedD" 
        DataSourceID="dsSellSheetReviewPlansListMedD" GroupItemCount="10" 
        DataKeyNames="Segment_Name" OnItemDataBound="ReviewPlanListView_OnItemDataBound">
        <LayoutTemplate>
         <table cellpadding="2" runat="server" id="tblSegment" border="1" class="reviewPlansMedD" >
            <tr runat="server" id="groupPlaceholder">
            </tr>
         </table>
        </LayoutTemplate>
        <GroupTemplate>
          <tr runat="server" id="itemPlaceholder">
          </tr>
        </GroupTemplate>
        <ItemTemplate>
          <tr id="Tr1" runat="server" align="left" >
              <th id="Th2" runat="server" align="left" style="border-left-width:3px;">
                <asp:Label ID="Label1" runat="server" Text='<%#Eval("Segment_Name") %>' /> Plan
              </th>
              <th id="Livesheader1" runat="server"  runat="server" >
                Pharmacy Lives
              </th>
              <th id="Tierheader1" runat="server"  runat="server" Visible='<%# (bool)Eval("Tier_Indicator") %>'>
                ADCIRCA PA/QL/ST
              </th>
              <th id="CopayHeader1" runat="server" runat="server"  Visible='<%# (bool)Eval("Copay_Indicator") %>' style="border-right-width: 3px;">
                ADCIRCA Co-pay Range
              </th> 
         </tr>
         <tr id="Tr2" runat="server"  class='<%#Eval("HighlightIndicator") %>'>
           <td class="PlanNMHighlight content" style="border-left-width: 3px;">  
             <b><asp:Label ID="PlanNamelbl" runat="server" Text='<%#Eval("Plan_Name") %>' /></b>
           </td>           
           <td id ="Lives1Data" class="content alignCenter">  
             <asp:Label ID="Lives1lbl" runat="server" Text='<%#Eval("Plan_Pharmacy_Lives", "{0:N0}") %>' />
           </td>    
           <td id="Tier1Data" runat="server" Visible='<%# (bool)Eval("Tier_Indicator") %>' class="content alignCenter">  
             <asp:Label ID="Tier2lbl" runat="server" Text='<%#Eval("SecondTierName") %>' />&nbsp;
           </td>
           <td id="Copay1Data" runat="server"  Visible='<%# (bool)Eval("Copay_Indicator") %>' class="content alignCenter" style="border-right-width: 3px;">  
             <asp:Label ID="Copay2lbl" runat="server" Text='<%#Eval("SecondCopay") %>' />&nbsp;
           </td>           
         </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
        <tr id="Tr1" runat="server" align="left" >
              <th id="Th2" runat="server" align="left" style="border-left-width: 3px;">
                <asp:Label ID="Label1" runat="server" Text='<%#Eval("Segment_Name") %>' /> Plan
              </th>
              <th id="Livesheader1" runat="server"  runat="server" >
                Pharmacy Lives
              </th>
              <th id="Tierheader1" runat="server"  runat="server" Visible='<%# (bool)Eval("Tier_Indicator") %>'>
                ADCIRCA PA/QL/ST
              </th>
              <th id="CopayHeader1" runat="server" runat="server"  Visible='<%# (bool)Eval("Copay_Indicator") %>' style="border-right-width: 3px;">
                ADCIRCA Co-pay Range
              </th> 
         </tr>
         <tr id="Tr2" runat="server"  class='<%#Eval("HighlightIndicator") %>'>
           <td class="PlanNMHighlight content" bgcolor="#fff1e6" style="border-left-width: 3px;">  
             <b><asp:Label ID="PlanNamelbl" runat="server" Text='<%#Eval("Plan_Name") %>' /></b>
           </td>           
           <td id ="Lives1Data" bgcolor="#fff1e6" class="content alignCenter">  
             <asp:Label ID="Lives1lbl" runat="server" Text='<%#Eval("Plan_Pharmacy_Lives", "{0:N0}") %>' />
           </td>    
           <td id="Tier1Data" runat="server" bgcolor="#fff1e6" Visible='<%# (bool)Eval("Tier_Indicator") %>' class="content alignCenter">  
             <asp:Label ID="Tier2lbl" runat="server" Text='<%#Eval("SecondTierName") %>' />&nbsp;
           </td>
           <td id="Copay1Data" runat="server" bgcolor="#fff1e6" Visible='<%# (bool)Eval("Copay_Indicator") %>' class="content alignCenter" style="border-right-width: 3px;">  
             <asp:Label ID="Copay2lbl" runat="server" Text='<%#Eval("SecondCopay") %>' />&nbsp;
           </td>           
         </tr>
        </AlternatingItemTemplate>
    </asp:ListView>
    
        <div class="rnd1" style="margin: 0 1px;"></div> 
        <div class="rnd1" style="margin: 0 2px;"></div> 
        <div class="rnd1" style="margin: 0 3px;"></div> 
        <div class="rnd1" style="margin: 0 5px;"></div>
    </div> 
    
    <div class="geographyLabel"><asp:Label ID="Label2" runat="server" /></div>
    <div style="position:absolute; top: 150px; left: 585px; width: 430px">
        <div class="rnd1" style="margin: 0 5px;"></div> 
        <div class="rnd1" style="margin: 0 3px;"></div> 
        <div class="rnd1" style="margin: 0 2px;"></div> 
        <div class="rnd1" style="margin: 0 1px;"></div>
    <asp:ListView runat="server" ID="ReviewPlansListViewMedicaid" 
        DataSourceID="dsSellSheetReviewPlansListMedicaid" GroupItemCount="10" 
        DataKeyNames="Segment_Name" OnItemDataBound="ReviewPlanListView_OnItemDataBound">
        <LayoutTemplate>
         <table cellpadding="2" runat="server" id="tblSegment" border="1" class="reviewPlansMedD" style="width: 430px" >
            <tr runat="server" id="groupPlaceholder">
            </tr>
         </table>
        </LayoutTemplate>
        <GroupTemplate>
          <tr runat="server" id="itemPlaceholder">
          </tr>
        </GroupTemplate>
        <EmptyDataTemplate>
            Data Not Available
        </EmptyDataTemplate>
        <ItemTemplate>
          <tr id="Tr1" runat="server" align="left" >
              <th id="Th2" runat="server" align="left" style="border-left-width: 3px;">
                Medicaid Plan
              </th>
              <th id="Livesheader1" runat="server"  runat="server" >
                Pharmacy Lives
              </th>
              <th id="Tierheader1" runat="server"  runat="server" Visible='<%# (bool)Eval("Tier_Indicator") %>'>
                ADCIRCA PA/QL/ST
              </th>
              <th id="CopayHeader1" runat="server" runat="server"  Visible='<%# (bool)Eval("Copay_Indicator") %>' style="border-right-width: 3px;">
                ADCIRCA Co-pay Range
              </th> 
         </tr>
         <tr id="Tr2" runat="server"  class='<%#Eval("HighlightIndicator") %>'>
           <td class="PlanNMHighlight content" style="border-left-width: 3px;">  
             <b><asp:Label ID="PlanNamelbl" runat="server" Text='<%#Eval("Plan_Name") %>' /></b>
           </td>           
           <td id ="Lives1Data" class="content alignCenter">  
             <asp:Label ID="Lives1lbl" runat="server" Text='<%#Eval("Plan_Pharmacy_Lives", "{0:N0}") %>' />
           </td>    
           <td id="Tier1Data" runat="server" Visible='<%# (bool)Eval("Tier_Indicator") %>' class="content alignCenter">  
             <asp:Label ID="Tier2lbl" runat="server" Text='<%#Eval("SecondTierName") %>' />&nbsp;
           </td>
           <td id="Copay1Data" runat="server"  Visible='<%# (bool)Eval("Copay_Indicator") %>' class="content alignCenter" style="border-right-width: 3px;">  
             <asp:Label ID="Copay2lbl" runat="server" Text='<%#Eval("SecondCopay") %>' />&nbsp;
           </td>           
         </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
        <tr id="Tr1" runat="server" align="left" >
              <th id="Th2" runat="server" align="left" style="border-left-width: 3px;">
                <asp:Label ID="Label1" runat="server" Text='<%#Eval("Segment_Name") %>' /> Plan
              </th>
              <th id="Livesheader1" runat="server"  runat="server" >
                Pharmacy Lives
              </th>
              <th id="Tierheader1" runat="server"  runat="server" Visible='<%# (bool)Eval("Tier_Indicator") %>'>
                ADCIRCA PA/QL/ST
              </th>
              <th id="CopayHeader1" runat="server" runat="server"  Visible='<%# (bool)Eval("Copay_Indicator") %>' style="border-right-width: 3px;">
                ADCIRCA Co-pay Range
              </th> 
         </tr>
         <tr id="Tr2" runat="server"  class='<%#Eval("HighlightIndicator") %>'>
           <td class="PlanNMHighlight content" bgcolor="#fff1e6" style="border-left-width: 3px;">  
             <b><asp:Label ID="PlanNamelbl" runat="server" Text='<%#Eval("Plan_Name") %>' /></b>
           </td>           
           <td id ="Lives1Data" bgcolor="#fff1e6" class="content alignCenter">  
             <asp:Label ID="Lives1lbl" runat="server" Text='<%#Eval("Plan_Pharmacy_Lives", "{0:N0}") %>' />
           </td>    
           <td id="Tier1Data" runat="server" bgcolor="#fff1e6" Visible='<%# (bool)Eval("Tier_Indicator") %>' class="content alignCenter">  
             <asp:Label ID="Tier2lbl" runat="server" Text='<%#Eval("SecondTierName") %>' />&nbsp;
           </td>
           <td id="Copay1Data" runat="server" bgcolor="#fff1e6" Visible='<%# (bool)Eval("Copay_Indicator") %>' class="content alignCenter">  
             <asp:Label ID="Copay2lbl" runat="server" Text='<%#Eval("SecondCopay") %>' style="border-right-width: 3px;"/>&nbsp;
           </td>           
         </tr>
        </AlternatingItemTemplate>
    </asp:ListView>
    
     <div class="rnd1" style="margin: 0 1px;"></div> 
        <div class="rnd1" style="margin: 0 2px;"></div> 
        <div class="rnd1" style="margin: 0 3px;"></div> 
        <div class="rnd1" style="margin: 0 5px;"></div>
    </div> 
    
    <asp:Panel id="pnlFooter" runat="server" CssClass="pnlFooter" style="position:absolute; top: 655px; left: 38px;">
        Please note that the displayed information is an aggregation of state or national averages. Coverage policies and tier levels may not apply to all insurance products or particular plan designs<br />
        and may vary by patient.  Data accessed from Today's Accounts&reg; on <asp:Label ID="footerdatelbl" runat="server" />, Pinsonault Associates - Today's Accounts&reg;.  Plan information will vary and are subject to change without notice.  <br />
        Please check directly with the health plan to determine the most up to date formulary information.
    </asp:Panel>        
  
    <asp:SqlDataSource ID="dsSellSheetReviewPlansListMedD" runat="server" 
    SelectCommand="usp_SellSheet_ReviewPlanSelectionList"
    SelectCommandType="StoredProcedure"  FilterExpression="Segment_ID = 2">

        <SelectParameters>
            <asp:QueryStringParameter  Name="Sell_Sheet_ID"  QueryStringField="Sell_Sheet_ID" DbType ="Int32"  />
            <asp:SessionParameter Name="User_ID" SessionField="UserID" />              
        </SelectParameters>  
    </asp:SqlDataSource> 
    <asp:SqlDataSource ID="dsSellSheetReviewPlansListMedicaid" runat="server" 
    SelectCommand="usp_SellSheet_ReviewPlanSelectionList"
    SelectCommandType="StoredProcedure"  FilterExpression="Segment_ID = 3">

        <SelectParameters>
            <asp:QueryStringParameter  Name="Sell_Sheet_ID"  QueryStringField="Sell_Sheet_ID" DbType ="Int32"  />
            <asp:SessionParameter Name="User_ID" SessionField="UserID" />              
        </SelectParameters>  
    </asp:SqlDataSource>            

