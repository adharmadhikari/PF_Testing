<%@ Page Language="C#" AutoEventWireup="true" Theme="pathfinder" MasterPageFile="~/MasterPages/Modal.master" EnableViewState="true" CodeFile="OpenDelMyKCs.aspx.cs" Inherits="todaysaccounts_all_OpenDelMyKCs" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>

<asp:Content runat="server" ContentPlaceHolderID="head">
    <script type="text/javascript" src="../../content/scripts/KeyContacts.js"></script>
     <script type="text/javascript">
         function RefreshMyKCs()
         {
             getMyKCGrid().control.get_masterTableView().rebind();

             window.setTimeout(CloseWin, 4000);  
         }

         function getMyKCGrid()
         {
             return window.top.$get("ctl00_Tile4_MyKeyContactsList1_gridMyContacts");
         }

         function CloseWin()
         {
             var manager = window.top.GetRadWindowManager();

             var window1 = manager.getWindowByName("DelKC");
             window1.close();
         }
    </script> 
</asp:Content>

<asp:Content runat="server" ID="title" ContentPlaceHolderID="title">
    <asp:Literal runat="server" id="titleText" />
</asp:Content>

<asp:Content runat="server" ID="main" ContentPlaceHolderID="main">
 
<%-- This page is displays readonly view of data being deleted and a confirmation message when clicked on Delete 
     link from my key contacts grid--%>
    <asp:HiddenField ID="PlanNameHdn" runat="server" Value="" Visible="false" /> 

    <div id="DelKCMain">
    <asp:FormView runat="server" ID="formDelKC" DefaultMode="ReadOnly"   
           DataSourceID="dsKeyContacts" CellPadding="0" CellSpacing="0" Width="100%" DataKeyNames="KC_ID">
        <ItemTemplate>
                    <table style="color:Black;width:90%">
                    <tr>
                        <td class="style1">Prefix </td>
                        <td><%# Eval("KC_Prefix") %>&nbsp;</td>
                        <td width="15%">First Name </td>
                        <td width="25%"><%# Eval("KC_F_Name") %>&nbsp;</td>
                        <td width="10%">Last Name </td>
                        <td width="25%"><%# Eval("KC_L_Name") %>&nbsp;</td>
                        <td width="10%">Suffix </td>
                        <td width="25%"><%# Eval("KC_Suffix") %>&nbsp;</td>
                    </tr> 
                    <tr>
                        <td  width="5%">Designation </td>
                        <td width="20%" colspan="3"><%# Eval("KC_Title_Name")%>&nbsp;</td>
                        <td>Title </td>
                        <td colspan="3"><%# Eval("KC_Role")%>&nbsp;</td>
                    </tr> 
                    <tr>
                        <td  width="5%">Address1 </td>
                        <td colspan="3"><%# Eval("KC_Address1") %>&nbsp;</td>
                        <td>Address2 </td>
                        <td colspan="3"><%# Eval("KC_Address2") %>&nbsp;</td>
                    </tr> 
                    <tr>
                        <td width="5%">City </td>
                        <td colspan="3"><%# Eval("KC_City") %>&nbsp;</td>
                        <td>State </td>
                        <td><%# Eval("KC_State") %>&nbsp;</td>
                        <td>Zip </td>
                        <td><%# Eval("KC_Zip") %>&nbsp;</td>
                    </tr> 
                    <tr>
                        <td width="5%">Email 1 </td>
                        <td colspan="3"><%# Eval("KC_Email") %>&nbsp;</td>
                        <td></td>
                        <td colspan="3">&nbsp;</td>
                    </tr> 
                    <tr>
                        <td class="style1">Phone 1 </td>
                        <td><%# Eval("KC_Phone") %>&nbsp;</td>
                        <td width="15%"></td>
                        <td>&nbsp;</td>
                        <td>Mobile </td>
                        <td><%# Eval("KC_Mobile") %>&nbsp;</td>
                        <td>Fax </td>
                        <td><%# Eval("KC_Fax") %>&nbsp;</td>
                    </tr> 
                    </table>
                    </div>

                    <div id="Div3" class="tileContainerHeader" align="left" >
                    <asp:Literal runat="server" ID="Literal2" Text='Assistant Details...' />
                    </div>
                    <div id="Div2" class="todaysAccounts2">
                    <table style="color:Black;width:100%">
                    <tr>
                        <td>Name</td>
                        <td colspan="3"><%# Eval("KC_Admin_Name") %>&nbsp;</td>
                        <td></td>
                        <td>&nbsp;</td>
                        <td></td>
                        <td>&nbsp;</td>
                    </tr> 
                    <tr>
                        <td>Office Phone </td>
                        <td><%# Eval("KC_Admin_PH") %>&nbsp;</td>
                        <td></td>
                        <td>&nbsp;</td>
                        <td id="blank"></td>
                        <td>Email </td>
                        <td colspan="2"><%# Eval("KC_Admin_Email") %>&nbsp;</td>
                    </tr> 
                    <tr>
                        <td>comments </td>
                        <td colspan="7"><%# Eval("KC_Comments") %>&nbsp;</td>
                    </tr> 
                    </table>
                    <br />
                    <div>
                    <table> 
                    <tr>
                         <td class="style1" colspan="2" style="color:Red;">Are you sure you want to delete this key contact?</td>
                    </tr>

                    <tr>
                        <td width="7%" align="right">
                            <asp:Button ID="Yesbtn" runat="server" Text="Yes" width="50px" Visible="true" OnClick="Yesbtn_Click" />
                        </td>
                        <td width="7%" align="left">
                            <asp:Button ID="Nobtn" width="50px" runat="server" Text="No" OnClientClick="javascript:CloseWin(); return true;"/>
                        </td>

                    </tr>    
                    </table>
                    </div>
         </ItemTemplate> 
         </asp:FormView>
        </div>

    <asp:EntityDataSource ID="dsKeyContacts" runat="server" EntitySetName="KeyContactSet" ConnectionString="name=PathfinderClientEntities" DefaultContainerName="PathfinderClientEntities" 
        AutoGenerateWhereClause="true">
        <WhereParameters>       
            <asp:QueryStringParameter QueryStringField="Plan_ID" Name="Plan_ID" Type="Int32" ConvertEmptyStringToNull="true"/>
            <asp:QueryStringParameter QueryStringField="KCID" Name="KC_ID" Type="Int32" ConvertEmptyStringToNull="true" />
        </WhereParameters>
    </asp:EntityDataSource>    

    <asp:Label ID="Msglbl" runat="server" Text="" Visible="false"></asp:Label>
</asp:Content>