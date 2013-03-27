<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PlanInfoDetails.ascx.cs" Inherits="custom_reckitt_todaysaccounts_controls_PlanInfoDetails" %>
 <div id="lfView">

    <asp:FormView runat="server" ID="formView" DataSourceID="dsPlanInfoDetails" CellPadding="0" CellSpacing="0" Width="100%">
        <ItemTemplate>
            <table style="vertical-align: top;" class="genTable" cellpadding="0" cellspacing="0"
                border="0">
                <tr>
                    <td>
                        <asp:Literal ID="ltPlantype" runat="server" Text='<%$ Resources:Resource, Label_Plan_Type %>' />
                    </td>
                    <td class="rn">
                        <%# Eval("Plan_Type_Name") %>&nbsp;
                    </td>
                </tr>
               
                <tr>
                    <td>
                        <asp:Literal ID="ltAddress1" runat="server" Text='<%$ Resources:Resource, Label_Address %>' />
                    </td>
                    <td class="rn">
                        <%# Eval("Address1") %>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="ltCity" runat="server" Text='<%$ Resources:Resource, Label_City %>' />
                    </td>
                    <td class="rn">
                        <%# Eval("City")%>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="ltState" runat="server" Text='<%$ Resources:Resource, Label_State %>' />
                    </td>
                    <td class="rn">
                        <%# Eval("Plan_State") %>&nbsp;
                    </td>
                </tr>

                <tr>
                    <td colspan="2" class="bn rn">
                        &nbsp;
                    </td>
                </tr>
            </table>
        </ItemTemplate>
        
    </asp:FormView>
</div>
    <asp:EntityDataSource ID="dsPlanInfoDetails" runat="server" EntitySetName="PlansClientSet" ConnectionString="name=PathfinderReckittEntities" DefaultContainerName="PathfinderReckittEntities" 
        AutoGenerateWhereClause="true">
        <WhereParameters>       
            <asp:QueryStringParameter QueryStringField="Plan_ID" Name="Plan_ID" Type="Int32" />
            <asp:Parameter Name="Status" DefaultValue="true" Type="Boolean" />
        </WhereParameters>
    </asp:EntityDataSource>    