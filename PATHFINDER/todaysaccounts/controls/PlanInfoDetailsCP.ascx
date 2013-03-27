<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PlanInfoDetailsCP.ascx.cs" Inherits="todaysaccounts_controls_PlanInfoDetailsCP" %>
    <div id="lfView">

    <asp:FormView runat="server" ID="formView" DataSourceID="dsPlanInfoDetails" CellPadding="0" CellSpacing="0" Width="100%">
        <ItemTemplate>
                <table style="vertical-align:top;" class="genTable" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td><asp:Literal ID="ltWebsite" runat="server" Text='<%$ Resources:Resource, Label_Website %>' /></td>
                        <td class="rn"><%# Pinsonault.Web.Support.ParseWebsiteLink(Eval("Plan_WebSite") as string) %>&nbsp;</td>
                    </tr>                
                    <tr>
                        <td><asp:Literal ID="ltParentPlan" runat="server" Text='<%$ Resources:Resource, Label_ParentName %>' /></td>
                        <td class="rn"><%# Eval("Parent_Plan_Name") %>&nbsp;</td>
                    </tr>
                    <tr>
                        <td><asp:Literal ID="ltPlanClassification" runat="server" Text='<%$ Resources:Resource, Label_Plan_Classification %>' /></td>
                        <td class="rn"><%# Eval("Plan_Classification_Name") %>&nbsp;</td>
                    </tr>
                    <tr>
                        <td><asp:Literal ID="ltPlantype" runat="server" Text='<%$ Resources:Resource, Label_Plan_Type %>' /></td>
                        <td class="rn"><%# Eval("Plan_Type_Name") %>&nbsp;</td>
                    </tr>
                    <tr>
                        <td><asp:Literal ID="ltAccreditation" runat="server" Text='<%$ Resources:Resource, Label_Plan_AccreditationStatus %>' /></td>
                        <td class="rn"><%# Eval("Plan_Accreditation") %>&nbsp;</td>
                    </tr>           
                    <tr>
                        <td colspan="2" class="subTable">
                            <asp:GridView runat="server" ID="gridViewPBMs" SkinID="basic" Width="100%" DataSourceID="dsPBMs" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:BoundField DataField="Plan_Name" HeaderText='<%$ Resources:Resource, Label_PBM %>' ItemStyle-CssClass="firstCol" />
                                    <asp:BoundField DataField="PBM_Service" HeaderText='<%$ Resources:Resource, Label_PBM_Service %>' />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>     
                    <tr>
                        <td colspan="2" class="subTable">
                            <asp:GridView runat="server" ID="gridViewSPPs" SkinID="basic" Width="100%" DataSourceID="dsSPPs" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:BoundField DataField="Plan_Name" HeaderText='<%$ Resources:Resource, Label_SPP %>' />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>                         
                   <%-- <tr>
                        <td colspan="2" class="sectionDisclaimer"><%# Eval("Last_Update_DT", Resources.Resource.Label_Section_Last_Updated)%></td>
                    </tr>--%>
                    <tr>
                        <td colspan="2" class="bn rn">&nbsp;</td>
                    </tr>
                </table>       
        </ItemTemplate>
        
    </asp:FormView>
</div>
    <asp:EntityDataSource ID="dsPlanInfoDetails" runat="server" EntitySetName="PlanInfoSet" ConnectionString="name=PathfinderEntities" DefaultContainerName="PathfinderEntities" 
        AutoGenerateWhereClause="true">
        <WhereParameters>       
            <asp:QueryStringParameter QueryStringField="Plan_ID" Name="Plan_ID" Type="Int32" />
        </WhereParameters>
    </asp:EntityDataSource>    
    
    <asp:EntityDataSource ID="dsPBMs" runat="server" EntitySetName="PlanParentAffiliationListViewSet" ConnectionString="name=PathfinderEntities" DefaultContainerName="PathfinderEntities" AutoGenerateWhereClause="true" OrderBy="it.Plan_Name">
        <WhereParameters>
            <asp:QueryStringParameter QueryStringField="Plan_ID" Name="Plan_ID" Type="Int32" />
            <asp:Parameter Name="Affil_Type_ID" DefaultValue="3" Type="Int32" />
        </WhereParameters>
    </asp:EntityDataSource>
    <asp:EntityDataSource ID="dsSPPs" runat="server" EntitySetName="PlanParentAffiliationListViewSet" ConnectionString="name=PathfinderEntities" DefaultContainerName="PathfinderEntities" AutoGenerateWhereClause="true" OrderBy="it.Plan_Name">
        <WhereParameters>
            <asp:QueryStringParameter QueryStringField="Plan_ID" Name="Plan_ID" Type="Int32" />
            <asp:Parameter Name="Affil_Type_ID" DefaultValue="2" Type="Int32" />
        </WhereParameters>        
    </asp:EntityDataSource>