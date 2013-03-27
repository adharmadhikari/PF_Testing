 <%@ Control Language="C#" AutoEventWireup="true" CodeFile="Messages.ascx.cs" Inherits="controls_Messages" %>
<div id="MessageReadOnly" runat="server"  >
<div class="tileSubHeader">Messages</div>

<telerik:RadGrid EnableEmbeddedSkins="false" SkinID="table1" runat="server" ID="rgMessageReadOnly" AutoGenerateColumns="false" DataSourceID="dsMessageSelectedReadOnly" 
  AllowSorting="true" AllowPaging="false" >
        <MasterTableView DataKeyNames="Campaign_ID, Message_ID" Width="100%" >
              <Columns>  
                <telerik:GridBoundColumn DataField="Message_Name" HeaderText="Name" UniqueName="Message_Name" ReadOnly="true"  />
    
            </Columns>
        </MasterTableView>
        <ClientSettings Scrolling-AllowScroll="true" Scrolling-UseStaticHeaders="true" Selecting-AllowRowSelect="false" />
 </telerik:RadGrid>
 
 </div>
 
 <asp:SqlDataSource runat="server" ID="dsMessageSelectedReadOnly" ConnectionString='<%$ ConnectionStrings:PathfinderClientDB_Format %>' 
      SelectCommand="pprx.usp_Campaign_Messages_SelectedList" 
        SelectCommandType="StoredProcedure" ProviderName="System.Data.SqlClient" >
        
        <SelectParameters>
            <asp:QueryStringParameter Name="Campaign_ID" QueryStringField="id" DefaultValue="" />
         </SelectParameters>
    </asp:SqlDataSource>    
