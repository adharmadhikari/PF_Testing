<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PlanInfoAddress.ascx.cs" Inherits="controls_PlanInfoAddress" %>
    <div id="rtView">
<asp:ListView runat="server" ID="formViewPlanInfo" DataSourceID="dsPlanInfoAddress" ItemPlaceholderID="itemPlaceholder">
    
    <LayoutTemplate>
        <asp:PlaceHolder runat="server" ID="itemPlaceholder" />        
    </LayoutTemplate>
    <%--<GroupTemplate>        
        <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
    </GroupTemplate>--%>
    <ItemTemplate>
    
        <table class="genTable" cellpadding="0" cellspacing="0" border="0">
            <tr runat="server" style="background-color:#afafaf;" visible="<%# Container.DisplayIndex > 0 %>">
                <td><asp:Literal runat="server" Text='<%$ Resources:Resource, Label_Address_Type %>' /></td>
                <td class="rn"><%# Eval("Address_Type") %></td>
            </tr>
            <tr>
                <td><asp:Literal runat="server" Text='<%$ Resources:Resource, Label_Address_Line1 %>' /></td>
                <td class="rn"><%# Eval("Address1") %>&nbsp;</td>
            </tr>
            <tr>
                <td><asp:Literal runat="server" Text='<%$ Resources:Resource, Label_Address_Line2 %>' /></td>
                <td class="rn"><%# Eval("Address2")%>&nbsp;</td>
            </tr>
            <tr>
                <td><asp:Literal runat="server" Text='<%$ Resources:Resource, Label_City %>' /></td>
                <td class="rn"><%# Eval("City")%>&nbsp;</td>
            </tr>
            <tr>
                <td><asp:Literal runat="server" Text='<%$ Resources:Resource, Label_State %>' /></td>
                <td class="rn"><%# Eval("State")%>&nbsp;</td>
            </tr>   
            <tr>
                <td><asp:Literal runat="server" Text='<%$ Resources:Resource, Label_ZIP %>' /></td>
                <td class="rn"><%# Eval("ZIP") %>&nbsp;</td>
            </tr>   
            <tr>
                <td><asp:Literal runat="server" Text='<%$ Resources:Resource, Label_Phone %>' /></td>
                <td class="rn"><%# Eval("Phone")%>&nbsp;</td>
            </tr>  
            <tr>
                <td><asp:Literal runat="server" Text='<%$ Resources:Resource, Label_Fax %>' /></td>
                <td class="rn"><%# Eval("Fax")%>&nbsp;</td>
            </tr>   
            <tr>
                <td><asp:Literal ID="Literal1" runat="server" Text='<%$ Resources:Resource, Label_Plan_Comments %>' /></td>
                <td class="rn"><%# Eval("Plan_Comments")%>&nbsp;</td>
            </tr>   
           <%-- <tr runat="server" visible='<%# ShowSectionDisclaimer%>'>
                <td class="sectionDisclaimer" colspan="2"><%# Eval("Last_Update_DT", Resources.Resource.Label_Section_Last_Updated) %>&nbsp;</td>
            </tr>   --%>                
        </table>
        <%--<br />--%>
    </ItemTemplate>
    <EmptyDataTemplate>
        <asp:Literal runat="server" ID="lblNoData" Text='<%$ Resources:Resource, Message_No_Addresses %>' />
    </EmptyDataTemplate>

</asp:ListView>
    </div>  

    <asp:EntityDataSource ID="dsPlanInfoAddress" runat="server" EntitySetName="PlanAddressCommentSet" ConnectionString="name=PathfinderEntities" DefaultContainerName="PathfinderEntities" 
        AutoGenerateWhereClause="true" AutoGenerateOrderByClause="false" OrderBy="it.Address_Type_ID">
        <WhereParameters>       
            <asp:QueryStringParameter QueryStringField="plan_ID" Name="Plan_ID" Type="Int32" />
            <asp:Parameter Name="Status" DefaultValue="true" Type="Boolean" />
        </WhereParameters>
    </asp:EntityDataSource>