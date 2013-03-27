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
                <td class="rn"><%# Eval("Geography.ID")%>&nbsp;</td>
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
                              
        </table>
        <%--<br />--%>
    </ItemTemplate>
    <EmptyDataTemplate>
        <asp:Literal runat="server" ID="lblNoData" Text='<%$ Resources:Resource, Message_No_Addresses %>' />
    </EmptyDataTemplate>
</asp:ListView>
    </div>  
<asp:EntityDataSource ID="dsPlanInfoAddress" runat="server" ConnectionString="name=PathfinderEntities" DefaultContainerName="PathfinderEntities" 
     CommandText="select VALUE address from PlanAddressSet as address where address.Plan_ID=@Plan_ID order by address.Address_Type_ID limit(1)" AutoGenerateWhereClause="false"  AutoGenerateOrderByClause="false" 
    >
    <CommandParameters>
        <asp:QueryStringParameter QueryStringField="plan_ID" Name="Plan_ID" Type="Int32" />            
    </CommandParameters>
    <%--<WhereParameters>       
        <asp:QueryStringParameter QueryStringField="plan_ID" Name="Plan_ID" Type="Int32" />            
    </WhereParameters>--%>
</asp:EntityDataSource>

    
     