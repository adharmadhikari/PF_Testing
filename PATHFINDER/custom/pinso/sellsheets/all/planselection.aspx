<%@ Page Title="" Language="C#" MasterPageFile="~/custom/MasterPages/SellSheetStep.master" AutoEventWireup="true" CodeFile="planselection.aspx.cs" Inherits="custom_pinso_sellsheets_planselection" %>
<%@ MasterType VirtualPath="~/custom/MasterPages/SellSheetStep.master" %>
<%@ Register src="~/custom/pinso/sellsheets/controls/PlanSelectionScript.ascx" tagname="PlanSelectionScript" tagprefix="pinso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="scriptContainer" Runat="Server">
    <pinso:PlanSelectionScript runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="StepBody" Runat="Server">
  <div>
   <asp:Label ID="msglbl" runat="server" Visible="false" Text="Please select between 1 and 10 plans" style="color:Red;"></asp:Label>
   <br />
   <div class="AddAcct"><span class="ssBold"><a id="AddAcctLnk" href="javascript:OpenAddAccount();">Add an Account</a></span></div> 
    <asp:HiddenField ID="hdnPlansSelected" runat="server"/>    
    <pinso:ClientValidator ID="vldPlansSelected" Required ="true" Target="hdnPlansSelected" Text="Please select between 1 and 10 plans" runat="server" />  
    
    <asp:HiddenField ID="hdnSelectedGeo" runat="server" Value="" />    
    
    <asp:FormView  runat="server" ID="formSSPlans" DefaultMode="ReadOnly"   
           DataSourceID="dsSellSheetMast" CellPadding="0" CellSpacing="0" Width="100%">
    <ItemTemplate>
            <asp:HiddenField ID="hdnTypeID" runat="server" Value='<%# Eval("Type_ID") %>' />    
            <asp:HiddenField ID="hdnTypeName" runat="server" Value='<%# Eval("Type_Name") %>' />        
            <asp:HiddenField ID="hdnCopay" runat="server" Value='<%# Eval("Use_Copay") %>' />        
    </ItemTemplate> 
    </asp:FormView> 
    <div id="planSelectContainer">
    <telerik:RadGrid SkinID="radTable"  ClientSettings-Selecting-AllowRowSelect="false" runat="server" ID="gridPlanSelectionList" AllowSorting="true"  AllowFilteringByColumn="false" EnableEmbeddedSkins="false" DataSourceID="dsSellSheetPlansList" AllowPaging="false" >        
        <MasterTableView AllowSorting="true" autogeneratecolumns="false" AllowPaging="false" PagerStyle-CssClass="postback" PageSize="10" CssClass="reviewPlans">
           <Columns >
                <telerik:GridTemplateColumn UniqueName="CheckboxTemplate" HeaderStyle-CssClass="postback planSelectHeader" HeaderStyle-Width="3%"  FooterStyle-CssClass="bottom">
                    <ItemTemplate>
                        <span id='<%# String.Format("{0}_{1}_{2}",Eval("Plan_ID"),Eval("Formulary_ID"),Eval("Product_ID")) %>'>
                        <asp:CheckBox ID="chkSelectedPlan" runat="server" Checked='<%# Eval("Is_Selected") %>' onclick='<%# string.Format("PlanSelectionChanged(this,{0},{1},{2})", Eval("Plan_ID"), Eval("Formulary_ID"), Eval("Product_ID")) %>' />
                        </span>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridBoundColumn UniqueName="Plan_ID" ReadOnly="True" Display="False" DataField="Plan_ID" />
                <telerik:GridBoundColumn UniqueName="Segment_ID" ReadOnly="True" Display="False" DataField="Segment_ID" />
                <telerik:GridBoundColumn UniqueName="Product_ID" ReadOnly="True" Display="False" DataField="Product_ID" />
                <telerik:GridBoundColumn UniqueName="Formulary_ID" ReadOnly="True" Display="False" DataField="Formulary_ID" />
                <telerik:GridBoundColumn DataField="Plan_Name" UniqueName="Plan_Name" HeaderText="Account Name" HeaderStyle-Width="9%" HeaderStyle-CssClass="rgHeader planSelectHeader postback" HeaderStyle-Font-Bold="true" FooterStyle-CssClass="bottom"/>
                <telerik:GridBoundColumn DataField="Formulary_Name" UniqueName="Formulary_Name" HeaderText="Formulary Name" HeaderStyle-Width="9%" HeaderStyle-CssClass="rgHeader planSelectHeader postback" HeaderStyle-Font-Bold="true" FooterStyle-CssClass="bottom"/>
                <telerik:GridBoundColumn DataField="Segment_Name" UniqueName="Segment_Name" HeaderText="Market Segment" HeaderStyle-Width="9%" HeaderStyle-CssClass="rgHeader planSelectHeader postback" HeaderStyle-Font-Bold="true" FooterStyle-CssClass="bottom"/>
                <telerik:GridBoundColumn DataField="Formulary_Lives" HeaderText="Formulary Lives" UniqueName="Formulary_Lives" HeaderStyle-Width="9%" DataFormatString="{0:n0}"  ItemStyle-CssClass="alignRight"  HeaderStyle-CssClass="rgHeader planSelectHeader postback" HeaderStyle-Font-Bold="true" FooterStyle-CssClass="bottom"/>
                <telerik:GridBoundColumn DataField="Plan_Total_Lives" HeaderText="Total Lives" UniqueName="Plan_Total_Lives" HeaderStyle-Width="9%" DataFormatString="{0:n0}"  ItemStyle-CssClass="alignRight" HeaderStyle-CssClass="rgHeader planSelectHeader postback" HeaderStyle-Font-Bold="true" FooterStyle-CssClass="bottom"/>
                <telerik:GridBoundColumn DataField="FirstTierName" UniqueName="FirstTierName" HeaderText="Tier" HeaderStyle-Width="7%" HeaderStyle-CssClass="rgHeader planSelectHeader postback " HeaderStyle-Font-Bold="true" FooterStyle-CssClass="bottom"/>
                <telerik:GridBoundColumn DataField="FirstCoverageStatus" UniqueName="FirstCoverageStatus" HeaderText="Coverage Status" HeaderStyle-Width="7%" HeaderStyle-CssClass="rgHeader planSelectHeader postback " HeaderStyle-Font-Bold="true" FooterStyle-CssClass="bottom"/>
                <telerik:GridBoundColumn DataField="FirstFormularyStatus" UniqueName="FirstFormularyStatus" HeaderText="Status" HeaderStyle-CssClass="rgHeader planSelectHeader postback " HeaderStyle-Font-Bold="true" FooterStyle-CssClass="bottom"/>
                <telerik:GridBoundColumn DataField="FirstCopay" UniqueName="FirstCopay" HeaderText="Co-Pay" HeaderStyle-Width="7%" HeaderStyle-CssClass="rgHeader planSelectHeader postback" ItemStyle-CssClass="alignRight" HeaderStyle-Font-Bold="true" FooterStyle-CssClass="bottom"/>
                <telerik:GridBoundColumn DataField="SecondTierName" UniqueName="SecondTierName" HeaderText="Tier" HeaderStyle-Width="7%" HeaderStyle-CssClass="rgHeader planSelectHeader postback"  HeaderStyle-Font-Bold="true" FooterStyle-CssClass="bottom"/>
                <telerik:GridBoundColumn DataField="SecondCoverageStatus" UniqueName="SecondCoverageStatus" HeaderText="Coverage Status" HeaderStyle-Width="7%" HeaderStyle-CssClass="rgHeader planSelectHeader postback" HeaderStyle-Font-Bold="true" FooterStyle-CssClass="bottom"/>
                <telerik:GridBoundColumn DataField="SecondFormularyStatus" UniqueName="SecondFormularyStatus" HeaderText="Status" HeaderStyle-CssClass="rgHeader planSelectHeader postback " HeaderStyle-Font-Bold="true" FooterStyle-CssClass="bottom"/>
                <telerik:GridBoundColumn DataField="SecondCopay" UniqueName="SecondCopay" HeaderText="Co-Pay" HeaderStyle-Width="7%" HeaderStyle-CssClass="rgHeader planSelectHeader postback" ItemStyle-CssClass="alignRight" HeaderStyle-Font-Bold="true" FooterStyle-CssClass="bottom"/>
                <telerik:GridBoundColumn DataField="ThirdTierName" UniqueName="ThirdTierName" HeaderText="Tier" HeaderStyle-Width="7%" HeaderStyle-CssClass="rgHeader planSelectHeader postback" HeaderStyle-Font-Bold="true" FooterStyle-CssClass="bottom"/>
                <telerik:GridBoundColumn DataField="ThirdCoverageStatus" UniqueName="ThirdCoverageStatus" HeaderText="Coverage Status" HeaderStyle-Width="7%" HeaderStyle-CssClass="rgHeader planSelectHeader postback" HeaderStyle-Font-Bold="true" FooterStyle-CssClass="bottom"/>
                <telerik:GridBoundColumn DataField="ThirdFormularyStatus" UniqueName="ThirdFormularyStatus" HeaderText="Status" HeaderStyle-CssClass="rgHeader planSelectHeader postback " HeaderStyle-Font-Bold="true" FooterStyle-CssClass="bottom"/>
                <telerik:GridBoundColumn DataField="ThirdCopay" UniqueName="ThirdCopay" HeaderText="Co-Pay" HeaderStyle-Width="7%"  HeaderStyle-CssClass="rgHeader planSelectHeader postback" ItemStyle-CssClass="alignRight" HeaderStyle-Font-Bold="true" FooterStyle-CssClass="bottom"/>
            </Columns>  
        <SortExpressions>
            <telerik:GridSortExpression FieldName ="Formulary_Lives" SortOrder ="Descending" /> 
        </SortExpressions> 
        </MasterTableView>   
        <ClientSettings EnablePostBackOnRowClick="false" Selecting-AllowRowSelect="false" >
              <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="220px"  />
              <Selecting AllowRowSelect="false" />
              
        </ClientSettings>
        
    </telerik:RadGrid>
    </div>
    <br />    

  
  <asp:EntityDataSource ID="dsSellSheetMast" EntitySetName="SellSheetMastSet" runat="server" ConnectionString="name=PathfinderClientEntities" DefaultContainerName="PathfinderClientEntities" 
        AutoGenerateWhereClause="true">
        <WhereParameters>       
            <asp:QueryStringParameter QueryStringField="Sell_Sheet_ID" Name="Sell_Sheet_ID" Type="Int32" ConvertEmptyStringToNull="true"/>
        </WhereParameters>
  </asp:EntityDataSource>    
    
  <asp:SqlDataSource ID="dsSellSheetPlansList" runat="server"  
  
  SelectCommand="usp_SellSheet_PlanSelectionList"
   SelectCommandType="StoredProcedure">
 <SelectParameters>
     <asp:QueryStringParameter  Name="Sell_Sheet_ID"  QueryStringField="Sell_Sheet_ID" DbType ="Int32"  />
     <asp:SessionParameter Name="User_ID" SessionField="UserID" />  
 </SelectParameters>  
 </asp:SqlDataSource>   
 
 <input type="button" class="btnPrev"
        onclick="clientManager.get_ApplicationManager().back(clientManager)"  
        value="Back" />
    <asp:Button ID="btnNextStep" runat="server" Text="Next" CssClass="postback validate btnNext" 
        onclick="btnNextStep_Click" />
  
  </div>  
</asp:Content>

  
  