<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PlanInfoDetailsMAC.ascx.cs" Inherits="todaysaccounts_controls_PlanInfoMAC" %>

    <asp:FormView runat="server" ID="formView" DataSourceID="dsPlanInfoDetails" CellPadding="0" CellSpacing="0" Width="100%">
        <ItemTemplate>

            <table class="genTable" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td><asp:Literal ID="Literal1" runat="server" Text='<%$ Resources:Resource, Label_Website %>' /></td>
                    <td class="rn"><%# Pinsonault.Web.Support.ParseWebsiteLink(Eval("Plan_WebSite") as string) %>&nbsp;</td>
                </tr>                
                <tr>
                    <td><asp:Literal ID="Literal3" runat="server" Text='<%$ Resources:Resource, Label_Total_Covered_Lives %>' /></td>
                    <td class="rn"><%# Eval("Total_Covered", "{0:n0}")%>&nbsp;</td>
                </tr>
                <tr>
                    <td><asp:Literal ID="Literal4" runat="server" Text='<%$ Resources:Resource, Label_MAC_Jurisdictions %>' /></td>
                    <td class="rn"><%# Eval("MAC_Jurisdictions")%>&nbsp;</td>
                </tr>                                
                <tr>
                    <td><asp:Literal ID="Literal2" runat="server" Text='<%$ Resources:Resource, Label_States_Serviced %>' /></td>
                    <td class="rn"><%# Eval("States_Serviced")%>&nbsp;</td>
                </tr>
               <%-- <tr>
                    <td colspan="2" class="sectionDisclaimer"><%# Eval("Last_Update_DT", Resources.Resource.Label_Section_Last_Updated)%></td>
                </tr> --%> 
                <tr>
                    <td colspan="2" class="bn rn">&nbsp;</td>
                </tr>
            </table>                
        </ItemTemplate>
    </asp:FormView>

    <asp:EntityDataSource ID="dsPlanInfoDetails" runat="server" EntitySetName="PlanInfoSet" ConnectionString="name=PathfinderEntities" DefaultContainerName="PathfinderEntities" 
        AutoGenerateWhereClause="true">
        <WhereParameters>       
            <asp:QueryStringParameter QueryStringField="Plan_ID" Name="Plan_ID" Type="Int32" />
        </WhereParameters>
    </asp:EntityDataSource>  