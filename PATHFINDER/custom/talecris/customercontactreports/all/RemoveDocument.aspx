<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Modal.master" AutoEventWireup="true" CodeFile="RemoveDocument.aspx.cs" Inherits="custom_pinso_customercontactreports_all_RemoveDocument" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
  <script type="text/javascript">
      function RefreshDocumentList() {
          window.setTimeout(CloseWin, 4000);
      }

      //To close the popup window.
      function CloseWin()
      {
          $(".ccrBusinessPlans .tools .reqsel").hide();
          var manager = window.top.GetRadWindowManager();
          var window1 = manager.getWindowByName("RemoveWnd");
          window1.close();
      }

      function RefreshBusinessDocs()
      {
          window.top.$find("ctl00_ctl00_Tile3_Tile7_BusinessDocument1_gridCCDocuments").get_masterTableView().rebind();
          var oManager = window.top.GetRadWindowManager();
          window.setTimeout(CloseWin, 1000);
      }
  </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="title" Runat="Server">
  <asp:Label ID="titleText" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="tools" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="main" Runat="Server">
<div id="RemovePlanDocument">
 <asp:FormView runat="server" ID="formRemoveDocument" DefaultMode="ReadOnly" DataSourceID="dsPlanDocument" CellPadding="0" CellSpacing="0" Width="100%" DataKeyNames="Document_ID">
   <ItemTemplate>
      <table align="center">
        <tr>
           <td>
             <div class="modalFormButtons">
               <span class="coreBtn">
                 <span class="bg">
                   <span class="bg2">
                       <asp:Button ID="Yesbtn" runat="server" Text="Yes" width="70px" Visible="true" OnClick="Yesbtn_Click" />
                   </span>
                 </span>
               </span>
             </div>
          </td>
          <td>
             <div class="modalFormButtons">
                <span class="coreBtn">
                   <span class="bg">
                     <span class="bg2">
                        <asp:Button ID="Nobtn" width="70px" runat="server" Text="No" OnClientClick="javascript:CloseWin(); return true;"/>
                     </span>
                   </span>
                </span>
            </div>
         </td>
      </tr> 
      
              
    </table>
 </ItemTemplate> 
</asp:FormView>
</div>
<asp:Label ID="Msglbl" runat="server" Text="" Visible="false"></asp:Label>
    <asp:EntityDataSource ID="dsPlanDocument" runat="server" 
        EntitySetName="PlanDocumentsSet" DefaultContainerName="PathfinderClientEntities"
        AutoGenerateWhereClause="True" 
        ConnectionString="name=PathfinderClientEntities">
    <WhereParameters>       
        <asp:QueryStringParameter QueryStringField="Document_ID" Name="Document_ID" Type="Int32"/>
    </WhereParameters>
    </asp:EntityDataSource> 
</asp:Content>


