<%@ Page Language="C#" MasterPageFile="~/powerplanrx/MasterPages/MasterPage.master" AutoEventWireup="true" 
  Theme="impact" CodeFile="createcampaign_step2_messages.aspx.cs" Title="PowerPlanRx - Step 2 Messages"   
 Inherits="createcampaign_step2_messages" %>
<%@ Register Src="~/powerplanrx/controls/Messages.ascx" TagName="Messages" TagPrefix="pinso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="main" Runat="Server">
<script type="text/javascript">


    function ValidateAll() {

        if (($(".MessageAdd :checked").length) == 0) {
            alert("Please select at least 1 message!");
            return false;
        }
    }

    function ConfirmDelete() {
       var PhaseId;

       PhaseId = document.getElementById('<%=hPhaseID.ClientID %>').value;

       //alert(PhaseId);

       if (PhaseId >= 7) {

           if ($find("<%= rgMessage.ClientID %>").get_masterTableView().get_element().tBodies[0].rows.length <= 1) {
               alert('You cannot delete the last selected Message.');
               return false;
           }

           else {
               var answer; answer = window.confirm('Delete selected Message?');
               if (answer == true) {
                   return true;
               }
               else {
                   return false;
               }
           }
       }

    }
    


</script>
<%-- 
<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rgMessage">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgMessage" />
                    <telerik:AjaxUpdatedControl ControlID="rgMessageAdd" />
                   
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
<table width="100%" ><tr><td valign="top" style="width:50%">
<div id="MessageAdd" runat="server" >
<div class="tileSubHeader">
    <div class="labelSubHeader">
        Available List
     </div>

 <div class="toolTipControls">
        <pinso:CustomButton runat="server" ID="Button1" Text="Add Message" OnClientClick="return ValidateAll();" CssClass="button" OnClick="AddMessage" /></SPAN></SPAN></SPAN>
</div>
<div class="clearAll"></div>
</div>   

    <telerik:RadGrid EnableEmbeddedSkins="false" SkinID="table1" runat="server" ID="rgMessageAdd" AutoGenerateColumns="false" DataSourceID="dsMessageAvailable"
               AllowSorting="true" AllowPaging="false" >
        <MasterTableView AutoGenerateColumns="false" ClientDataKeyNames="Message_ID" DataKeyNames="Message_ID" >
            <Columns>
                <telerik:GridTemplateColumn HeaderStyle-Width="5%">
                    <ItemTemplate>
                        <asp:CheckBox ID="chkMessage" CssClass="MessageAdd" runat="server" Text='' Checked="false"  />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
               
                <telerik:GridBoundColumn DataField="Message_Name" HeaderText="Name" />
            </Columns>
        </MasterTableView>
        <ClientSettings Scrolling-AllowScroll="true" Scrolling-UseStaticHeaders="false" Selecting-AllowRowSelect="false" />
    </telerik:RadGrid>
    <asp:SqlDataSource runat="server" ID="dsMessageAvailable" ConnectionString='<%$ConnectionStrings:PathfinderClientDB_Format %>' 
      SelectCommand="pprx.usp_Campaign_Messages_AvailableList" SelectCommandType="StoredProcedure" >
        <SelectParameters>
            <asp:QueryStringParameter Name="Campaign_ID" QueryStringField="id" DefaultValue="" />
           
        </SelectParameters>
    </asp:SqlDataSource>    
    <div class="centerBtn" >
    <SPAN class=coreBtn><SPAN class=bg><SPAN class=bg2>  <asp:Button runat="server" ID="btnAdd" Text="Add Message" OnClientClick="return ValidateAll();" CssClass="button" OnClick="AddMessage" /></SPAN></SPAN></SPAN>
</div>
  
</div>

</td><td valign="top" style="width:50%">
<!-- Edit/Delete -->
<div id="MessageDsp" runat="server" >
<div class="tileSubHeader"> Selected List</div>
<div id="divMsg" runat="server" style="float: none;">
                </div>    


<asp:HiddenField ID="hPhaseID" runat="server" Value="" />
<telerik:RadGrid EnableEmbeddedSkins="false" SkinID="table1" runat="server" ID="rgMessage" AutoGenerateColumns="false" DataSourceID="dsMessageSelected" 
OnDeleteCommand="rgMessage_DeleteCommand" AllowSorting="true" AllowPaging="false"  
 AllowAutomaticUpdates="false" AllowAutomaticDeletes="false" >
    <MasterTableView DataKeyNames="Campaign_ID, Message_ID">
        <Columns>
            <telerik:GridTemplateColumn HeaderStyle-Width="5%">
                <ItemTemplate>
                    <asp:LinkButton ID="linkDelete" OnClientClick="return ConfirmDelete();" runat="server"
                        CommandName="Delete">Delete</asp:LinkButton>
                </ItemTemplate>
            </telerik:GridTemplateColumn>
     
                <telerik:GridBoundColumn DataField="Message_Name" HeaderText="Name" UniqueName="Message_Name" ReadOnly="true"  />

            </Columns>
        </MasterTableView>
       <ClientSettings Scrolling-AllowScroll="true" Scrolling-UseStaticHeaders="false" Selecting-AllowRowSelect="false" />
 </telerik:RadGrid>
    
<asp:SqlDataSource runat="server" ID="dsMessageSelected" ConnectionString='<%$ ConnectionStrings:PathfinderClientDB_Format %>' 
      SelectCommand="pprx.usp_Campaign_Messages_SelectedList" 
        SelectCommandType="StoredProcedure" ProviderName="System.Data.SqlClient" 
        
        UpdateCommand="pprx.usp_Campaign_UpdateMessages" 
        UpdateCommandType="StoredProcedure" 
        DeleteCommand="pprx.usp_Campaign_Messages_SelectedList_Delete" 
        DeleteCommandType="StoredProcedure" >
        <SelectParameters>
            <asp:QueryStringParameter Name="Campaign_ID" QueryStringField="id" DefaultValue="" />
         </SelectParameters>
         
        <DeleteParameters>
            <asp:Parameter Name="Campaign_ID" Type="Int32" />
            <asp:Parameter Name="Message_ID" Type="Int32" />
        </DeleteParameters>
        
        <UpdateParameters>
            <asp:Parameter Name="Campaign_ID" Type="Int32" />
            <asp:Parameter Name="Message_ID" Type="Int32" />
            <asp:Parameter Name="Modified_By" Type="String" />
        </UpdateParameters>
         
    </asp:SqlDataSource>    
</div>

</td></tr></table>
</asp:Panel>


<!-- ReadOnly -->
<asp:Panel ID="pnlReadOnly" runat="server" Visible="true">
    <pinso:Messages runat="server" ID="messages" />
</asp:Panel>

</asp:Content>

