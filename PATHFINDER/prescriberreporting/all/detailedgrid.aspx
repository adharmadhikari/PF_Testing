<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/PartialPage.master" AutoEventWireup="true" CodeFile="detailedgrid.aspx.cs" Inherits="prescriberreporting_all_detailedgrid" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="scriptContainer" Runat="Server">

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="partialPage" Runat="Server">
    <asp:GridView runat="server" OnDataBound="detailedGrid_DataBound" GridLines="None"
        OnPreRender="detailedGrid_PreRender" ID="detailedGrid" CssClass="grid gv" 
        HeaderStyle-CssClass="detailedHeader gvh" RowStyle-CssClass="gvr"
    AutoGenerateColumns="False" >
<RowStyle CssClass="gvr"></RowStyle>
        <Columns>
            <asp:BoundField DataField="Region_Name" HeaderText="Region" />
            <asp:BoundField DataField="District_Name" HeaderText="District" />
            <asp:BoundField DataField="Territory_Name" HeaderText="Territory" />
            <asp:BoundField DataField="Physician_Name" HeaderText="Prescriber Name" />      
            <asp:BoundField DataField="Physician_ID" HeaderText="ME Number" />                 
            <%--<asp:BoundField DataField="Product_Name" HeaderText="Products" />--%>
            <asp:TemplateField HeaderText="Drug Name">
                <ItemTemplate>
                    <asp:Literal ID="Literal1" runat="server" Text='<%# Eval("Product_Name") %>' />                   
                    <input type="hidden" id="dataKey" value='<%# String.Format("{0}~{1}",Eval("Physician_ID"), Eval("Product_ID")) %>' />                
                </ItemTemplate>
            </asp:TemplateField>            
        </Columns>

<HeaderStyle CssClass="detailedHeader gvh"></HeaderStyle>
    </asp:GridView>
    <asp:Label ID="gridCount" runat="server" CssClass="grid gridCount" style="display:none;"></asp:Label>    

</asp:Content>

