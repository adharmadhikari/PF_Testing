<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PlanInfoDetailsSM.ascx.cs" Inherits="todaysaccounts_controls_PlanInfoSM" %>
<div id="lfView">
    <asp:FormView runat="server" ID="formView" DataSourceID="dsPlanInfoDetails" CellPadding="0" CellSpacing="0" Width="100%">
        <ItemTemplate>
    
            <table class="genTable" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td><asp:Literal runat="server" Text='<%$ Resources:Resource, Label_Website %>' /></td>
                    <td class="rn"><%# Pinsonault.Web.Support.ParseWebsiteLink(Eval("Plan_WebSite") as string, 50) %>&nbsp;</td>
                </tr>
                <tr>
                    <td><asp:Literal ID="Literal4" runat="server" Text='<%$ Resources:Resource, Label_MMISFAS %>' /></td>
                    <td class="rn"><%# Eval("MMISFAS")%>&nbsp;</td>
                </tr>                
                <tr>
                    <td><asp:Literal ID="Literal5" runat="server" Text='<%$ Resources:Resource, Label_SMPBM %>' /></td>
                    <td class="rn"><%# Eval("SMPBM")%>&nbsp;</td>
                </tr>                
                <tr>
                    <td><asp:Literal ID="Literal6" runat="server" Text='<%$ Resources:Resource, Label_SMHCM %>' /></td>
                    <td class="rn"><%# Eval("SMHCM")%>&nbsp;</td>
                </tr>                
                <tr>
                    <td><asp:Literal ID="Literal7" runat="server" Text='<%$ Resources:Resource, Label_Preferred_Drug_List %>' /></td>
                    <td class="rn"><a target="_blank" href='usercontent/downloadpdlform.ashx?planID=<%# Eval("Plan_ID")%>'><%# Eval("SM_Preferred_Drug_List")%>&nbsp;</a></td>
                </tr>                                                
                <tr>
                    <td><asp:Literal ID="Literal8" runat="server" Text='<%$ Resources:Resource, Label_Drug_Utilization_Review %>' /></td>
                    <td class="rn"><%# Eval("Drug_Utilization_Review")%>&nbsp;</td>
                </tr>  
               <%-- <tr>
                    <td colspan="2" class="sectionDisclaimer"><%# Eval("Last_Update_DT", Resources.Resource.Label_Section_Last_Updated)%></td>
                </tr>   --%>              
            </table>                  
        </ItemTemplate>
    </asp:FormView>
</div>
    <asp:EntityDataSource ID="dsPlanInfoDetails" runat="server" EntitySetName="PlanInfoStateMedicaidSet" ConnectionString="name=PathfinderEntities" DefaultContainerName="PathfinderEntities" 
        AutoGenerateWhereClause="true">
        <WhereParameters>       
            <asp:QueryStringParameter QueryStringField="Plan_ID" Name="Plan_ID" Type="Int32" />
        </WhereParameters>
    </asp:EntityDataSource>  
    