<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PlanInfoDetailsPBM.ascx.cs" Inherits="todaysaccounts_controls_PlanInfoDetailsPBM" %>
    <asp:FormView runat="server" ID="formViewPlanInfo" DataSourceID="dsPlanInfoDetails" CellPadding="0" CellSpacing="0" Width="100%">
        <ItemTemplate>

            <table class="genTable" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td>Website</td>
                    <td class="rn"><%# Pinsonault.Web.Support.ParseWebsiteLink(Eval("Plan_WebSite") as string) %>&nbsp;</td>
                </tr>          
                <%--<tr>
                    <td colspan="2" class="sectionDisclaimer"><%# Eval("Last_Update_DT", Resources.Resource.Label_Section_Last_Updated)%></td>
                </tr>  --%>
            </table>                   
        </ItemTemplate>
    </asp:FormView>

    <asp:EntityDataSource ID="dsPlanInfoDetails" runat="server" EntitySetName="PlanInfoSet" ConnectionString="name=PathfinderEntities" DefaultContainerName="PathfinderEntities" 
        AutoGenerateWhereClause="true">
        <WhereParameters>       
            <asp:QueryStringParameter QueryStringField="Plan_ID" Name="Plan_ID" Type="Int32" />
        </WhereParameters>
    </asp:EntityDataSource>   