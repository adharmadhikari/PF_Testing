<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CCR_RemoveBusinessDocument.ascx.cs" Inherits="custom_controls_CCR_RemoveBusinessDocument" %>
<div id="RemovePlanDocument">
 <%--<asp:FormView runat="server" ID="formRemoveDocument" DefaultMode="ReadOnly" DataSourceID="dsPlanDocument" CellPadding="0" CellSpacing="0" Width="100%" DataKeyNames="Document_ID">
   <ItemTemplate>--%>
      <table align="center">
        <tr>
           <td>
             <div class="modalFormButtons">              
                 <pinso:CustomButton ID="Yesbtn" runat="server" Text="Yes" width="70px" Visible="true" OnClick="Yesbtn_Click" />                 
             </div>
          </td>
          <td>
             <div class="modalFormButtons">                
                 <pinso:CustomButton ID="Nobtn" width="70px" runat="server" Text="No" OnClientClick="javascript:CloseWin(); return true;"/>                  
            </div>
         </td>
      </tr>   
    </table>
<%-- </ItemTemplate> 
</asp:FormView>--%>
</div>
<asp:Label ID="Msglbl" runat="server" Text="" Visible="false"></asp:Label>