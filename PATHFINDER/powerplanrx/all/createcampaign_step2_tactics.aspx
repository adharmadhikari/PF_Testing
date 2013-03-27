<%@ Page  Language="C#"  MasterPageFile="~/powerplanrx/MasterPages/MasterPage.master" AutoEventWireup="true" Theme="impact" 
  CodeFile="createcampaign_step2_tactics.aspx.cs" Inherits="createcampaign_step2_tactics" 
   Title="PowerPlanRx - Step 2 Tactics" %>
<%@ Register Src="~/powerplanrx/controls/Tactics.ascx" TagName="Tactics" TagPrefix="pinso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="main" Runat="Server">
<script type="text/javascript">

   function ConfirmDelete() {
       var rowCount = 0;
       var PhaseId;

       PhaseId = document.getElementById('<%=hPhaseID.ClientID %>').value;

       //alert(PhaseId);

       if (PhaseId >= 6) {

           $(".QtyNeededEdit").each(
                function() {

                    if ($.trim(this.value))
                        rowCount++;
                }

            );

           if (rowCount <= 1) {
               alert('You cannot delete the last selected Tactic.');
               return false;
           }
           else {
               var answer; answer = window.confirm('Delete selected Tactic?');
               if (answer == true) {
                   return true;
               }
               else {
                   return false;
               }
           }
       }
   }

   function cleanInput(o)
   {
       if (o)
       {
           o.value = o.value.replace(/\,/ig, "");
       }
   }
   
</script>

<%-- 
<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rgTactics">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgTactics" />
                    <telerik:AjaxUpdatedControl ControlID="rgTacticsAdd" />
                   
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
--%>
<div class="tileContainerHeader">
    <div id="divCampaignName" class="CampaignInfo" runat="server" style="float: none;"></div>
</div>

<asp:Panel ID="pnlEdit" runat="server" Visible="false"> 
<!-- Add -->
<table ><tr><td valign="top" style="width:50%">
<div id="TacticsAdd" runat="server" >

<div class="tileSubHeader">
    <div class="labelSubHeader">
        Available List...
    </div>    
    <div class="toolTipControls">
        <pinso:CustomButton runat="server" ID="Button2" Text="Add Tactics" CssClass="button" OnClick="AddTactics" />
    </div>
    <div class="clearAll"></div>
</div>   

</div>
 <telerik:RadGrid runat="server" EnableEmbeddedSkins="false" SkinID="table1"  ID="rgTacticsAdd" AutoGenerateColumns="false" DataSourceID="dsTacticsAvailable" 
   AllowSorting="true" AllowPaging="false">
        
        <MasterTableView AutoGenerateColumns="false" ClientDataKeyNames="Tactic_ID" DataKeyNames="Tactic_ID" >
            <Columns>  
 
     <%-- Thumbnail --%>          
               <telerik:GridTemplateColumn HeaderText="Thumbnail" HeaderStyle-Width="16%" >
                 <ItemTemplate>
                    <asp:HyperLink ID="hypThumbReadOnly" runat="server" ImageUrl='<%# Eval("Tactics_Thumb_Filename","~/powerplanrx/content/images/{0}") %>'
                      NavigateUrl='<%# Eval("Tactics_Thumb_Filename","~/powerplanrx/content/images/{0}") %>' Target="_blank" >
                    </asp:HyperLink>
              </ItemTemplate>
               </telerik:GridTemplateColumn>
            <%-- Thumbnail End  --%> 
               
                <telerik:GridBoundColumn DataField="Tactic_Name" HeaderText="Tactic Name"  HeaderStyle-Width="16%"   />
                <telerik:GridBoundColumn DataField="Tactic_Description" HeaderText="Tactic or Program Description"  HeaderStyle-Width="50%"   />
                <telerik:GridTemplateColumn HeaderText="Qty Needed"  HeaderStyle-Width="16%" ItemStyle-CssClass="alignRight" >
                    <ItemTemplate>
                        <telerik:RadTextBox ID="rtxtQtyAdd" runat="server" Text=''  Width="50px" onblur="cleanInput(this)"
                            CssClass="txtBox QtyNeededAdd alignRight">
                        </telerik:RadTextBox>
                        <%-- 
                        <asp:RangeValidator ID="QtyRangeValidatorAdd" runat="server" ControlToValidate="rtxtQtyAdd"
                            ErrorMessage="Qty: 1 ~ 9999 Integer" MinimumValue="1" MaximumValue="9999" Type="Integer"></asp:RangeValidator>
                    --%>
                    <asp:CompareValidator runat="server" ControlToValidate="rtxtQtyAdd" Type="Integer" Operator="DataTypeCheck" 
                     ErrorMessage="Invalid Number" SetFocusOnError="true" ></asp:CompareValidator>
                    
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
   
         
            </Columns>
        </MasterTableView>
        <ClientSettings Scrolling-AllowScroll="true"  Scrolling-UseStaticHeaders="false" Selecting-AllowRowSelect="false" />
 </telerik:RadGrid>
 
<asp:SqlDataSource runat="server" ID="dsTacticsAvailable" ConnectionString='<%$ConnectionStrings:PathfinderClientDB_Format %>' 
      SelectCommand="pprx.usp_Campaign_Tactics_AvailableList" SelectCommandType="StoredProcedure" >
        <SelectParameters>
            <asp:QueryStringParameter Name="Campaign_ID" QueryStringField="id" DefaultValue="" />
          
        </SelectParameters>
    </asp:SqlDataSource>    

   </div>


<div id="btn" class="centerBtn" >
    <pinso:CustomButton runat="server" ID="btnAdd" Text="Add Tactics" CssClass="button" OnClick="AddTactics" />
</div>

</td>

<td valign="top" style="width:50%">

<!-- Edit/Delete -->
<div id="TacticsDsp" runat="server" >

    <div class="tileSubHeader"> 
        <div class="labelSubHeader">
            Selected List ... 
        </div>
        <div class="toolTipControls">
            <pinso:CustomButton runat="server" ID="Button3" Text="Update Qty" CssClass="button" OnClick="EditQty" />
        </div>
        <div class="clearAll"></div>    
    </div>
               

<asp:HiddenField ID="hPhaseID" runat="server" Value="" />

<telerik:RadGrid runat="server" EnableEmbeddedSkins="false" SkinID="table1" ID="rgTactics" AutoGenerateColumns="false" DataSourceID="dsTacticsSelected" 
OnDeleteCommand="rgTactics_DeleteCommand" AllowSorting="true" AllowPaging="false"  
  AllowAutomaticUpdates="false" AllowAutomaticDeletes="false" >
    <MasterTableView DataKeyNames="Campaign_ID, Tactic_ID" >
        <Columns>
            <telerik:GridTemplateColumn>
                <ItemTemplate>
                    <asp:LinkButton ID="linkDelete" OnClientClick="return ConfirmDelete();" runat="server"
                        CommandName="Delete">Delete</asp:LinkButton>
                </ItemTemplate>
            </telerik:GridTemplateColumn>
  
               <%-- Thumbnail --%>          
               <telerik:GridTemplateColumn HeaderText="Thumbnail">
                 <ItemTemplate>
                    <asp:HyperLink ID="hypThumbReadOnly" runat="server" ImageUrl='<%# Eval("Tactics_Thumb_Filename","~/powerplanrx/content/images/{0}") %>'
                      NavigateUrl='<%# Eval("Tactics_Thumb_Filename","~/powerplanrx/content/images/{0}") %>' Target="_blank" >
                    </asp:HyperLink>
              </ItemTemplate>
               </telerik:GridTemplateColumn>
            <%-- Thumbnail End  --%> 
                     
                <telerik:GridBoundColumn DataField="Tactic_Name" HeaderText="Tactic Name" UniqueName="Tactic_Name" ReadOnly="true"   />
                <telerik:GridBoundColumn DataField="Tactic_Description" HeaderText="Tactic or Program Description" UniqueName="Tactic_Description" ReadOnly="true"   />
              
                <telerik:GridTemplateColumn HeaderText="Qty Needed" ItemStyle-CssClass="alignRight">
                     <ItemTemplate>
                     
                        <telerik:RadTextBox ID="rtxtQtyEdit" runat="server" Text='<%# Bind("Quantity") %>'  Width="50px" onblur="cleanInput(this)"
                          CssClass="txtBox QtyNeededEdit alignRight" ></telerik:RadTextBox>
                      
                      
                      <asp:RequiredFieldValidator ID="QtyRequiredValidator" runat="server" Display="Dynamic" ControlToValidate="rtxtQtyEdit"
                       ErrorMessage="Required!"></asp:RequiredFieldValidator>
                      
                      <%-- 
                       <asp:RangeValidator ID="QtyRangeValidator" runat="server" ControlToValidate="rtxtQtyEdit"  
                            ErrorMessage="Qty: 1 ~ 9999 Integer" MinimumValue="1" MaximumValue="9999" Type="Integer" ></asp:RangeValidator>
                      --%> 
                      <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="rtxtQtyEdit" Type="Integer" Operator="DataTypeCheck" 
                     ErrorMessage="Invalid Number" SetFocusOnError="true"  ></asp:CompareValidator>
                        
                      </ItemTemplate>
               
                </telerik:GridTemplateColumn>
    
            </Columns>
        </MasterTableView>
        <ClientSettings Scrolling-AllowScroll="true" Scrolling-UseStaticHeaders="false" Selecting-AllowRowSelect="false" />
 </telerik:RadGrid>
    
<asp:SqlDataSource runat="server" ID="dsTacticsSelected" ConnectionString='<%$ ConnectionStrings:PathfinderClientDB_Format %>' 
      SelectCommand="pprx.usp_Campaign_Tactics_SelectedList" 
        SelectCommandType="StoredProcedure" ProviderName="System.Data.SqlClient" 
        
        UpdateCommand="pprx.usp_Campaign_UpdateTactics" 
        UpdateCommandType="StoredProcedure" 
        DeleteCommand="pprx.usp_Campaign_Tactics_SelectedList_Delete" 
        DeleteCommandType="StoredProcedure" >
        <SelectParameters>
            <asp:QueryStringParameter Name="Campaign_ID" QueryStringField="id" DefaultValue="" />
         </SelectParameters>
         
        <DeleteParameters>
            <asp:Parameter Name="Campaign_ID" Type="Int32" />
            <asp:Parameter Name="Tactic_ID" Type="Int32" />
        </DeleteParameters>
        
        <UpdateParameters>
            <asp:Parameter Name="Campaign_ID" Type="Int32" />
            <asp:Parameter Name="Tactic_ID" Type="Int32" />
            <asp:Parameter Name="Quantity" Type="Int32" />
            <asp:Parameter Name="Modified_By" Type="String" />
        </UpdateParameters>
         
    </asp:SqlDataSource>   
   

<div class="centerBtn" >
    <pinso:CustomButton runat="server" ID="btnEditQty" Text="Update Qty" CssClass="button" OnClick="EditQty" />
</div>    
 
</div>
<div id="divMsg" runat="server" style="float: none;color:#f00"></div>
</td></tr></table>
</asp:Panel>

<!-- ReadOnly -->
    <asp:Panel ID="pnlReadOnly" runat="server" Visible="true">
        <pinso:Tactics runat="server" ID="tactics" />
    </asp:Panel>


</asp:Content>

