<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/PartialPage.master" AutoEventWireup="true" CodeFile="detailgrid.aspx.cs" Inherits="custom_warner_formularyhistoryreporting_all_detailgrid" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="scriptContainer" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="partialPage" Runat="Server">
  <asp:GridView runat="server" 
    OnDataBound="detailedGrid_DataBound" 
    OnPreRender="detailedGrid_PreRender" 
    GridLines="None"  
    ID="detailedGrid" 
    CssClass="grid" 
    HeaderStyle-CssClass="detailedHeader"
    AutoGenerateColumns="False" >
        <Columns>
            <asp:BoundField DataField="Plan_Name" HeaderText="Account Name" />               
           <%-- <asp:BoundField DataField="Drug_Name" HeaderText="Drug Name" />--%>
            <asp:BoundField DataField="Product_Name" HeaderText="Product" />
            <asp:BoundField DataField="Formulary_Name" HeaderText="Benefit Design" />
            <asp:BoundField DataField="Formulary_Lives" HeaderText="Formulary Lives" DataFormatString="{0:N0}" ItemStyle-CssClass="alignRight" />
           
        </Columns>
    </asp:GridView>
    <asp:GridView runat="server" 
        OnDataBound="detailedGridTrx_DataBound" 
        OnPreRender="detailedGridTrx_PreRender" 
        GridLines="None"  
        ID="detailedGridTrx" 
        CssClass="grid" 
        HeaderStyle-CssClass="detailedHeader"
        AutoGenerateColumns="False" >
         <Columns>
            <asp:BoundField DataField="Geography_Name" HeaderText="Geography" />
            <asp:BoundField DataField="Plan_Name" HeaderText="Account Name" />                            
            <asp:BoundField DataField="Product_Name" HeaderText="Product" /> 
            <asp:BoundField DataField="Formulary_Name" HeaderText="Benefit Design" /> 
            <asp:BoundField DataField="Formulary_Lives" HeaderText="Formulary Lives" ItemStyle-CssClass="alignRight" DataFormatString="{0:N0}"/>
            <asp:BoundField DataField="Tier_Name0" HeaderText="Tier1" />
            <asp:BoundField DataField="Tier_Name1" HeaderText="Tier2" />
            <asp:BoundField DataField="Co_Pay0" HeaderText="Co_Pay1" />
            <asp:BoundField DataField="Co_Pay1" HeaderText="Co_Pay2" />    
             <asp:BoundField DataField="Restrictions0" HeaderText="Restrictions1" />
            <asp:BoundField DataField="Restrictions1" HeaderText="Restrictions2" />        
        </Columns>
    </asp:GridView>
    <asp:Label ID="gridCount" runat="server" CssClass="grid gridCount" style="display:none;"></asp:Label>  
</asp:Content>

