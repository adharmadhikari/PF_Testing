<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/PartialPage.master" AutoEventWireup="true" CodeFile="detailedgrid_affiliation.aspx.cs" Inherits="marketplaceanalytics_all_detailedgrid_affiliation" %>

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
            <asp:BoundField DataField="Geography_Name" HeaderText="Geography" />
            <asp:BoundField DataField="Plan_Name" HeaderText="Account Name" />                    
            <asp:BoundField DataField="Total_Covered" HeaderText="Lives" ItemStyle-CssClass="alignRight" DataFormatString="{0:N0}"/>          
            <asp:BoundField DataField="Segment_Name" HeaderText="Segment" /> 
            <asp:BoundField DataField="Product_Name" HeaderText="Drug Name" />
            <asp:BoundField DataField="Tier_Name" HeaderText="Tier" />
            <asp:BoundField DataField="Co_Pay" HeaderText="Co-Pay" ItemStyle-CssClass="alignRight" />
            <asp:TemplateField HeaderText="Restrictions">
                <ItemTemplate>
                    <input type="hidden" id="dataKey" value='<%# String.Format("{0}~{1}~{2}",Eval("Plan_ID"), Eval("Product_ID"), Eval("Segment_ID")) %>' />
                     <asp:Literal ID="lblPA" runat="server" Text='<%# CheckRestriction((object)DataBinder.Eval(Container.DataItem, "PA"), (object)DataBinder.Eval(Container.DataItem, "QL"), (object)DataBinder.Eval(Container.DataItem, "ST")) %>'/>               
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>

<HeaderStyle CssClass="detailedHeader gvh"></HeaderStyle>
    </asp:GridView>
    <asp:Label ID="gridCount" runat="server" CssClass="grid gridCount" style="display:none;"></asp:Label>    

</asp:Content>

