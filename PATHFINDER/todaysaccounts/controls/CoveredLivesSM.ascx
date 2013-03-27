<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CoveredLivesSM.ascx.cs" Inherits="controls_CoveredLivesSM" %>
<div id="lfView">
    

     <asp:FormView runat="server" ID="formView" DataSourceID="dsCoveredLives" CellPadding="0" CellSpacing="0" Width="100%">
        <ItemTemplate>
    
            <table class="genTable" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td><asp:Literal ID="Literal1" runat="server" Text='<%$ Resources:Resource, Label_Medicaid_Enrollment %>' /></td>
                    <td class="rn alignRight"><%# Eval("Medicaid_Enrollment", "{0:n0}")%>&nbsp;</td>
                </tr>
                <tr>
                    <td><asp:Literal ID="Literal2" runat="server" Text='<%$ Resources:Resource, Label_Medicaid_Managed_Care_Enrollment %>' /></td>
                    <td class="rn alignRight"><%# Eval("Medicaid_Mcare_Enrollment", "{0:n0}")%>&nbsp;</td>
                </tr>
                <tr>
                    <td><asp:Literal ID="Literal3" runat="server" Text='<%$ Resources:Resource, Label_Percentage_In_Managed_Care %>' /></td>
                    <td class="rn alignRight"><%# Eval("Percent_ManagedCare")%>% &nbsp;</td>
                </tr>
                 <tr>
                    <td><asp:Literal ID="Literal4" runat="server" Text='<%$ Resources:Resource, Label_SM_FFS_Lives %>' /></td>
                 <td class="rn alignRight"><%#string.Format("{0:n0}",(System.Convert.ToInt32(Eval("Medicaid_Enrollment")) - System.Convert.ToInt32(Eval("Medicaid_Mcare_Enrollment"))))%>&nbsp;</td>
                </tr> 
                 </table>        
           <%-- <table  class="genTable" cellpadding="0" cellspacing="0" border="0" >
                <tr id="Tr1" runat="server" visible='<%# ShowSectionDisclaimer%>'>
                    <td class="sectionDisclaimer"><%# Eval("Last_Update_DT", Resources.Resource.Label_Section_Last_Updated)%></td>
                </tr>                
            </table>  --%>                         
        </ItemTemplate>
    </asp:FormView>  
   
</div>
 
  
    <asp:EntityDataSource ID="dsCoveredLives" runat="server" EntitySetName="PlanInfoStateMedicaidSet" ConnectionString="name=PathfinderEntities" DefaultContainerName="PathfinderEntities" 
        AutoGenerateWhereClause="true" AutoGenerateOrderByClause="false"> 
        <WhereParameters>       
            <asp:QueryStringParameter QueryStringField="plan_ID" Name="Plan_ID" Type="Int32" />
        </WhereParameters>
    </asp:EntityDataSource>    
     
   