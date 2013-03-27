<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PlanInfoDetailsMedD.ascx.cs" Inherits="todaysaccounts_controls_PlanInfoMedD" %>
          
    <asp:FormView runat="server" ID="formView" DataSourceID="dsPlanInfoDetails" CellPadding="0" CellSpacing="0" Width="100%">
        <ItemTemplate>
   
            <table class="genTable" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td>Website</td>
                    <td class="rn"><%# Pinsonault.Web.Support.ParseWebsiteLink(Eval("Plan_WebSite") as string) %>&nbsp;</td>
                </tr>     
               <%-- <tr>
                    <td colspan="2" class="sectionDisclaimer"><%# Eval("Last_Update_DT", Resources.Resource.Label_Section_Last_Updated)%></td>
                </tr> --%>  
                <tr>
                    <td colspan="2" class="bn rn" style="height:20px">&nbsp;</td>
                </tr>                               
            </table>                 
        </ItemTemplate>
    </asp:FormView>  
  <div ID="BDHeader2" class="areaHeader" runat="server">Product Information</div>
  
    <asp:FormView runat="server" ID="formView1" DataSourceID="dsProdInfoDetails" CellPadding="0" CellSpacing="0" Width="100%">
        <ItemTemplate>
   
            <table class="genTable" cellpadding="0" cellspacing="0" border="0">
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
                <%--<tr>
                    <td colspan="2" class="bn rn" style="height:100px">&nbsp;</td>
                </tr>--%>                               
            </table>                 
        </ItemTemplate>
    </asp:FormView> 
   
    <asp:EntityDataSource ID="dsPlanInfoDetails" runat="server" EntitySetName="PlanInfoSet" ConnectionString="name=PathfinderEntities" DefaultContainerName="PathfinderEntities" 
        AutoGenerateWhereClause="true">
        <WhereParameters>       
            <asp:QueryStringParameter QueryStringField="Plan_ID" Name="Plan_ID" Type="Int32" />
        </WhereParameters>
    </asp:EntityDataSource>    
    
    <asp:SqlDataSource ID="dsProdInfoDetails" runat="server" ConnectionString="<%$ ConnectionStrings:Pathfinder %>"
        SelectCommand="usp_MedD_GetEnrollment" SelectCommandType="StoredProcedure">      
        <SelectParameters>
            <asp:QueryStringParameter QueryStringField="Prod_ID" Name="Prod_ID" Type="Int32" />
            <asp:QueryStringParameter QueryStringField="Plan_State" Name="Prod_State" Type="string" />
        </SelectParameters>
    </asp:SqlDataSource> 
    