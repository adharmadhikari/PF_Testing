<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DeletePlan.ascx.cs" Inherits="custom_Millennium_customercontactreports_controls_deleteplan" %>
<div id="RemovePlanDocument">
 
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

</div>
<asp:Label ID="Msglbl" runat="server" Text="" Visible="false"></asp:Label>