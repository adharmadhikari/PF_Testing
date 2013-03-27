<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Modal.master" AutoEventWireup="true" CodeFile="RemoveSellSheet.aspx.cs" Inherits="custom_pinso_all_RemoveSellSheet" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     <script type="text/javascript">
        //Refreshes sellsheet grids and closes the popup window.
         function RefreshMySSList()
         {
             getDraftGrid().get_masterTableView().rebind();
             getCompletedGrid().get_masterTableView().rebind();

             window.parent.$get("NewOrder").style.display = "none";
             window.parent.$get("PreviousOrders").style.display = "none";
                         
             window.setTimeout(CloseWin, 4000);  
         }

         //Gets reference for draftedsellsheets grid.
         function getDraftGrid() {
             return window.top.$find("ctl00_Tile3_DraftedSellSheets1_gridDraftedSellSheets");
         }

         //Gets reference for draftedsellsheets grid.
         function getCompletedGrid() {
             return window.top.$find("ctl00_Tile3_CompletedSellSheets1_gridCompletedSellSheets");
         }
    
         //To close the popup window.
         function CloseWin()
         {
             var manager = window.top.GetRadWindowManager();

             var window1 = manager.getWindowByName("RemoveWnd");
             window1.close();
         }
    </script> 
</asp:Content>
<asp:Content runat="server" ID="title" ContentPlaceHolderID="title">
    <asp:Label ID="titleText" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="tools" Runat="Server">
</asp:Content>

<asp:Content runat="server" ID="main" ContentPlaceHolderID="main">
 
<%-- This page displays readonly view of data being deleted and a confirmation message when clicked on Remove
     link from Drafted/Completed formulary sell sheet grids--%>
     <div id="RemoveSellSheets" runat="server" class="ssModalContainer sellsheets">
    <%--<asp:FormView runat="server" ID="formRemoveSellSheet" DefaultMode="ReadOnly"   
           DataSourceID="dsSellSheets" CellPadding="0" CellSpacing="0" Width="100%" DataKeyNames="Sell_Sheet_ID">
        <ItemTemplate>
        <br />--%>
                    <table width="100%" align="center" >
                    <%--<tr>
                        <td class="style1">Sell Sheet Name: </td>
                        <td><%# Eval("Sell_Sheet_Name")%></td>
                    </tr> 
                    <table> 
                    <tr>
                         <td class="style1" colspan="2"><br />Are you sure you want to remove the selected sell sheet?<br /></td>
                    </tr>--%>

                    <tr align="center">
                        <td width="7%" align="right">
                            <pinso:CustomButton ID="Yesbtn" runat="server" Text="Yes" width="70px" Visible="true" OnClick="Yesbtn_Click" />
                        </td>
                        <td width="7%" align="left">
                            <pinso:CustomButton ID="Nobtn" width="70px" runat="server" Text="No" OnClientClick="javascript:CloseWin(); return true;"/>
                        </td>

                    </tr>    
                    </table>
                    <%--</div>--%>
         <%--</ItemTemplate> 
         </asp:FormView>--%>
        </div>

    <%--<asp:EntityDataSource ID="dsSellSheets" runat="server" EntitySetName="SellSheetSet" DefaultContainerName="PathfinderClientEntities" EnableUpdate="true"
        AutoGenerateWhereClause="true">
    <WhereParameters>       
        <asp:QueryStringParameter QueryStringField="Sell_Sheet_ID" Name="Sell_Sheet_ID" Type="Int32"/>
    </WhereParameters>
    </asp:EntityDataSource>--%> 

    <asp:Label ID="Msglbl" runat="server" Text="" Visible="false"></asp:Label>
</asp:Content>