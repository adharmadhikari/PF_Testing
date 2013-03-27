<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Modal.master" AutoEventWireup="true" CodeFile="EditAddress.aspx.cs" Inherits="custom_dey_sellsheets_EditAddress" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">
     function RefreshPlanSelection()
     {
         window.top.clientManager.set_SelectionData(window.top.clientManager.get_SelectionData());
         //window.setTimeout(CloseWin, 4000);
         CloseWin();
     }
     
     function CloseWin()
     {
         var manager = window.top.GetRadWindowManager();

         var window1 = manager.getWindowByName("EditAddress");
         if (window1 != null)
         { window1.close(); }
     }
 </script>  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="title" Runat="Server">
    Edit Address
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="main" Runat="Server">
<iframe>
<div id="AddressDiv" runat="server" class="ssModalContainer sellsheets">
    <table cellspacing="10" cellpadding="10" border="0">
        <tr>
        <td colspan="2">
            <asp:Label ID="msglbl" runat="server" 
            Text="By editing the shipping address, PathfinderRx can’t validate the shipping information is correct." style="color:Red;"></asp:Label>
        </td>
        </tr>
       <tr>
            <td width="40%" align="right"><span class="ssBold">Address 1</span><span style="color:Red;">*</span></td>
            <td width="60%" align="left"><asp:TextBox ID="Addr1" runat="server" Text="" MaxLength="50" Width="150px" ></asp:TextBox></td>
        </tr>
        <tr>
            <td align="right">Address 2</td>
            <td align="left"><asp:TextBox ID="Addr2" runat="server" Text="" MaxLength="50" Width="150px" ></asp:TextBox></td>
        </tr>
        <tr>
            <td align="right">City<span style="color:Red;">*</span></td>
            <td align="left"><asp:TextBox ID="City" runat="server" Text="" MaxLength="50" Width="150px" ></asp:TextBox></td>
         </tr>
         <tr>
            <td align="right">State<span style="color:Red;">*</span></td>
            <td align="left" style="padding-left:5px">
               <%-- <telerik:RadComboBox ID="rdcmbState" runat="server" Width="155px" AppendDataBoundItems="true" DropDownWidth="155px" EnableEmbeddedSkins="false" SkinID="planInfoCombo" Skin="pathfinder" DataSourceID="dsLkpStates" DataTextField="Name" DataValueField="ID"  >
                    <Items>
                        <telerik:RadComboBoxItem Value="" Text="[Select a State]" Selected="true" />  
                    </Items>
                </telerik:RadComboBox>--%>
            </td>
         </tr>
         <tr>
            <td align="right"><span>Zip</span><span style="color:Red;">*</span></td>
            <td align="left"><asp:TextBox ID="Zip" runat="server" Text="" MaxLength="50" Width="30px" ></asp:TextBox></td>
         </tr>
         <tr>
            <td colspan="2">
                <pinso:CustomButton  ID="btn_submit" runat="server" Text="Submit" CausesValidation="true" CssClass="postback validate"  OnClick="btnSubmit_Click"/>
                <pinso:CustomButton  id="btn_cancel" runat="server" Text="Cancel" OnClientClick="javascript:CloseWin(); return true;" />
            </td>
         </tr>
    </table>
    
    <%--<pinso:ClientValidator ID="ClientValidator3" Target="Addr1" Text ='Please enter address1' Required="true" runat="server"  RegExp="[0-9a-zA-Z]" />  
    <%--<pinso:ClientValidator ID="ClientValidator5" Target="Addr2" Text ='Please enter address2' Required="true" runat="server" RegExp="[0-9a-zA-Z]"/>
    <pinso:ClientValidator ID="ClientValidator6" Target="City" Text ='Please enter the city.' Required="true" runat="server" RegExp="[a-zA-Z]"/>  
    <pinso:ClientValidator ID="ClientValidator7" Target="rdcmbState" Text ='Please select the State' Required="true" runat="server" />  
    <pinso:ClientValidator ID="ClientValidator8" Target="Zip" Text ='Please enter the zipcode.' Required="true" runat="server"  RegExp="[0-9]"/>  --%>
</div>
<asp:EntityDataSource ID="dsLkpStates" runat="server" EntitySetName="StateSet" DefaultContainerName="PathfinderClientEntities" 
         AutoGenerateWhereClause="true">
</asp:EntityDataSource>
</iframe>
</asp:Content>

