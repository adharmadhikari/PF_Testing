<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Modal.master" AutoEventWireup="true" CodeFile="AddAccount.aspx.cs" Inherits="custom_pinso_sellsheets_AddAccount"  EnableViewState="true"  %>

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

         var window1 = manager.getWindowByName("AddAcc");
         if (window1 != null)
         { window1.close(); }
     }
 </script>  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="title" Runat="Server">
Add an Account
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="tools" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="main" Runat="Server">
<div id="frmDiv" runat="server" visible="true" class="ssModalContainer sellsheets">
<asp:HiddenField ID="hdnSelectedSegment" Value ="" runat="server" />
<asp:HiddenField ID="hdnSelectedPlan" Value ="" runat="server" />
<table>
    <tr>
        <td align="left" style="padding-bottom: 5px;">Market Segment:</td>
        <td>
             <telerik:RadComboBox ID="rdcmbMktSegment" runat="server" DropDownWidth="250px"  Height="100px" EnableEmbeddedSkins="false" SkinID="planInfoCombo" Skin="pathfinder" AppendDataBoundItems="true" DataTextField="Name" DataValueField="ID"  AutoPostBack="true"  OnSelectedIndexChanged="rdcmbMktSegment_SelectedIndexChanged">
            </telerik:RadComboBox>
        </td>
    </tr>
    <tr>
        <td>
            <br />
        </td>
    </tr>
    <tr>
        <td align="left" style="padding-bottom: 5px;">
        <asp:Label ID="planlbl" runat="server" Visible="true" Text="Select Plan:"></asp:Label>  
        </td>
        <td>
             <telerik:RadComboBox ID="rdcmbPlans" runat="server" DropDownWidth="250px" EnableEmbeddedSkins="false" SkinID="planInfoCombo" Skin="pathfinder" AppendDataBoundItems="true" DataSourceID="dsPlans" DataTextField="Plan_Name" DataValueField="Plan_ID" OnSelectedIndexChanged="rdcmbPlans_SelectedIndexChanged" OnDataBound="rdcmbPlans_OnDataBound" Visible="true" AutoPostBack="true">
            </telerik:RadComboBox>
        </td>
    </tr>
    <tr>
    <td colspan="2" align="center">
        <asp:Label ID="noplanlbl" runat="server" Visible="false" Text="No plans available for selected market segment." style="color:Red;font-weight:bold;"></asp:Label>  
    </td>
    </tr>
</table>
<br />
 <asp:SqlDataSource ID="dsPlans" runat="server"  
  ConnectionString="<%$ ConnectionStrings:PathfinderClientDB_Format %>" 
  SelectCommand="usp_SellSheet_AdditionalPlans"
   SelectCommandType="StoredProcedure" EnableCaching="false">
 <SelectParameters>
     <asp:QueryStringParameter  Name="Sell_Sheet_ID"  QueryStringField="Sell_Sheet_ID" DbType ="Int32"  />
     <asp:SessionParameter Name="User_ID" SessionField="UserID" />  
     <asp:ControlParameter  ControlID="rdcmbMktSegment" PropertyName="SelectedValue"  Name="ID" DefaultValue="1"  Type = "Int32" /> 
 </SelectParameters>  
 </asp:SqlDataSource>   
 

<pinso:CustomButton  ID="btnSubmit" runat="server" Text="Submit" CssClass="postback validate"  OnClick="btnSubmit_Click"/>
<pinso:CustomButton  id="btnCancel" runat="server" Text="Cancel" OnClientClick="javascript:CloseWin(); return true;" />
</div>
 <asp:Label ID="Msglbl" runat="server" Text="" Visible="false"></asp:Label>
</asp:Content>

