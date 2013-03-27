<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PlanInfoDetailsVAR.ascx.cs" Inherits="custom_millennium_todaysaccounts_controls_PlanInfoDetails" %>
 <div id="lfView">

    <asp:FormView runat="server" ID="formView" DataSourceID="dsPlanInfoDetails" CellPadding="0" CellSpacing="0" Width="100%">
        <ItemTemplate>
            <table style="vertical-align: top;" class="genTable" cellpadding="0" cellspacing="0"
                border="0">
                <tr>
                    <td>
                        <asp:Literal ID="Literal15" runat="server" Text="RAM/FAM/NAE" />
                    </td>
                    <td class="rn">
                        <%# Eval("User_Name")%>&nbsp;
                    </td>
                </tr>
              <tr>
              <tr>
                    <td>
                        <asp:Literal ID="Literal1" runat="server" Text="VA Facility Name" />
                    </td>
                    <td class="rn">
                        <%# Eval("Plan_Name")%>&nbsp;
                    </td>
                </tr>
               <tr>
                    <td>
                        <asp:Literal ID="Literal14" runat="server" Text="Assigned VA VISN" />
                    </td>
                    <td class="rn">
                        <%# Eval("VA_VISN")%>&nbsp;
                    </td>
                </tr>
               <tr>
                    <td>
                        <asp:Literal ID="Literal5" runat="server" Text="Website" />
                    </td>
                    <td class="rn">
                        <%# Pinsonault.Web.Support.ParseWebsiteLink(Eval("WebSite") as string) %>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="ltAddress1" runat="server" Text="Address 1" />
                    </td>
                    <td class="rn">
                        <%# Eval("Address1") %>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="Literal4" runat="server" Text="Address  2" />
                    </td>
                    <td class="rn">
                        <%# Eval("Address2") %>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="ltCity" runat="server" Text="City" />
                    </td>
                    <td class="rn">
                        <%# Eval("City")%>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="ltState" runat="server" Text="State" />
                    </td>
                    <td class="rn">
                        <%# Eval("State") %>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="Literal8" runat="server" Text="Zip" />
                    </td>
                    <td class="rn">
                        <%# Eval("Zip") %>&nbsp;
                        </td>
                        </tr>
                <tr>
                    <td>
                        <asp:Literal ID="Literal9" runat="server" Text="Zip+4" />
                    </td>
                    <td class="rn">
                        <%# Eval("Zip_4")%>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="Literal10" runat="server" Text="Phone" />
                    </td>
                    <td class="rn">
                        <%# Eval("Phone") %>&nbsp;
                        </td>
                        </tr>
                <tr>
                    <td>
                        <asp:Literal ID="Literal12" runat="server" Text="Email" />
                    </td>
                    <td class="rn">
                        <%# Eval("Email")%>&nbsp;
                    </td>
                </tr>
                
                <tr>
                <%--<tr>
                    <td>
                        <asp:Literal ID="Literal2" runat="server" Text="In Office Dispensing" />
                    </td>
                    <td class="rn">
                        <%# (Boolean.Parse(Eval("In_Office_Dispensing").ToString()))?"Yes" : "No"%>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="Literal3" runat="server" Text="Revlimid" />
                    </td>
                    <td class="rn">
                        <%# (Boolean.Parse(Eval("Revlimid").ToString())) ? "Yes" : "No"%>&nbsp;
                    </td>
                </tr>--%>

                <tr>
                    <td>
                        <asp:Literal ID="Literal6" runat="server" Text="Class of Trade" />
                    </td>
                    <td class="rn">
                        <%# Eval("Class_Name")%>&nbsp;
                    </td>
                </tr>
                <tr>
                <tr>
                    <td>
                        <asp:Literal ID="Literal11" runat="server" Text="Customer Master ID" />
                    </td>
                    <td class="rn">
                        <%# FormatCustomerMasterID(Convert.ToString(Eval("Customer_Master_ID")))%>&nbsp;
                    </td>
                </tr>
                
                <tr>
                    <td>
                        <asp:Literal ID="Literal13" runat="server" Text="Territory Alignment" />
                    </td>
                    <td class="rn">
                        <%# Eval("Territory_Name")%>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="Literal7" runat="server" Text="Affiliated Practice" />
                    </td>
                    <td class="rn">
                        <%# Eval("Affiliated_Practice")%>&nbsp;
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
    <asp:EntityDataSource ID="dsPlanInfoDetails" runat="server" EntitySetName="PlansClientViewSet" ConnectionString="name=PathfinderMillenniumEntities" DefaultContainerName="PathfinderMillenniumEntities" 
        AutoGenerateWhereClause="true">
        <WhereParameters>       
            <asp:QueryStringParameter QueryStringField="Plan_ID" Name="Plan_ID" Type="Int32" />
            <%--<asp:Parameter Name="Status" DefaultValue="true" Type="Boolean" />--%>
        </WhereParameters>
    </asp:EntityDataSource>    