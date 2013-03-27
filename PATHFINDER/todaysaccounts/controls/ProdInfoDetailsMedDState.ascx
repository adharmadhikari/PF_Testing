<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProdInfoDetailsMedDState.ascx.cs" Inherits="todaysaccounts_controls_ProdInfoDetailsMedDState" %>
<div id="lfView">
    <asp:FormView runat="server" ID="formView" DataSourceID="dsProdInfoDetails" CellPadding="0" CellSpacing="0" Width="100%">
        <ItemTemplate>
   
            <table class="genTable" cellpadding="0" cellspacing="0" border="0">           
                <tr>
                    <td><asp:Literal ID="ltAffiliation" runat="server" Text='<%$ Resources:Resource, Label_Affiliation %>' /></td>
                    <td class="rn"><%# (Request.QueryString["Plan_Name"])%></td>
                </tr>    
                <tr>
                    <td><asp:Literal ID="ltProdStateEnrollment" runat="server" Text='<%$ Resources:Resource, Label_Product_State_Enrollment %>' /></td>
                    <td class="rn alignRight"><%# Eval("ProdStateEnrollment", "{0:n0}")%>&nbsp;</td>
                </tr>   
                <tr>
                    <td><asp:Literal ID="ltProdNationalEnrollment" runat="server" Text='<%$ Resources:Resource, Label_ProductNational_Enrollment %>' /></td>
                    <td class="rn alignRight"><%# Eval("ProdNationalEnrollment", "{0:n0}")%>&nbsp;</td>
                </tr>   
                <tr>
                    <td><asp:Literal ID="ltTotalMedDStateEnrollment" runat="server" Text='<%$ Resources:Resource, Label_Total_MedD_State_Enrollment %>' /></td>
                    <td class="rn alignRight"><%# Eval("TotalStateMedicaidEnrollment", "{0:n0}")%>&nbsp;</td>
                </tr> 
                <tr>
                    <td colspan="2" class="bn rn" style="height:100px">&nbsp;</td>
                </tr>                               
            </table>                 
        </ItemTemplate>
    </asp:FormView> 
     
     
</div> 
<asp:SqlDataSource ID="dsProdInfoDetails" runat="server" ConnectionString="<%$ ConnectionStrings:Pathfinder %>"
    SelectCommand="usp_MedD_GetEnrollment" SelectCommandType="StoredProcedure">      
    <SelectParameters>
        <asp:QueryStringParameter QueryStringField="Prod_ID" Name="Prod_ID" Type="Int32" />
        <asp:QueryStringParameter QueryStringField="Prod_State" Name="Prod_State" Type="string" />
    </SelectParameters>
</asp:SqlDataSource> 
    
     