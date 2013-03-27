<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Modal.master" AutoEventWireup="true" CodeFile="RemoveMedPolicyDoc.aspx.cs" Inherits="custom_merz_businessplanning_all_RemoveMedPolicyDoc" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     <script type="text/javascript">
        //Refreshes Medical Policy Documents grid and closes the popup window.
         function RefreshMedPolicyDocList()
         {
             getMedPolicyGrid().control.get_masterTableView().rebind();

             window.setTimeout(CloseWin, 4000);
         }

         function CloseWin()
         {
             var manager = window.top.GetRadWindowManager();

             var window1 = manager.getWindowByName("RemoveMPPopupwindow");
             if (window1 != null)
             { window1.close(); }
         }
         
         function getMedPolicyGrid()
         {
          return window.top.$get("ctl00_Tile3_MedicalPolicy1_RadGridbppOLICY");
         }
    </script> 
</asp:Content>
<asp:Content runat="server" ID="title" ContentPlaceHolderID="title">
    <asp:Label ID="titleText" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="tools" Runat="Server">
</asp:Content>

<asp:Content runat="server" ID="main" ContentPlaceHolderID="main">
 
<%-- This page displays readonly view of data being deleted and a confirmation message when clicked on Delete
     link from Med Policy Documents grid--%>
     <div id="RemoveMPDoc" class="modalFormButtons" runat="server">
<%--    <asp:FormView runat="server" ID="formRemoveMedPolicyDoc" DefaultMode="ReadOnly"   
           DataSourceID="dsMPDoc" CellPadding="0" CellSpacing="0" Width="100%" DataKeyNames="Medical_Policy_ID">
        <ItemTemplate>--%>
        <br />
                    <table width="100%" align="center" >
                     <tr align="center">
                        <td width="7%" align="right">
                            <pinso:CustomButton ID="Yesbtn" runat="server" Text="Yes" width="70px" Visible="true" OnClick="Yesbtn_Click" />
                        </td>
                        <td width="7%" align="left">
                            <pinso:CustomButton ID="Nobtn" width="70px" runat="server" Text="No" OnClientClick="$closeWindow();return true;"/>
                        </td>

                     </tr>    
                    </table>
<%--         </ItemTemplate> 
         </asp:FormView>--%>
        </div>

    <%--<asp:EntityDataSource ID="dsMPDoc" runat="server" ConnectionString="name=PathfinderMerzEntities" DefaultContainerName="PathfinderMerzEntities" 
        EntitySetName="BusinessPlanMedicalPolicyDocSet" EnableUpdate="true" AutoGenerateWhereClause="true"
        ContextTypeName="PathfinderClientModel.PathfinderMerzEntities, PathfinderClientModel">
    <WhereParameters>       
        <asp:QueryStringParameter QueryStringField="MP_ID" Name="Medical_Policy_ID" Type="Int32"/>
    </WhereParameters>
    </asp:EntityDataSource>--%> 

    <asp:Label ID="Msglbl" runat="server" Text="" Visible="false"></asp:Label>
</asp:Content>