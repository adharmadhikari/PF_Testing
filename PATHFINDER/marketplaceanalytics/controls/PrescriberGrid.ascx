<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PrescriberGrid.ascx.cs"
    Inherits="marketplaceanalytics_controls_PrescriberGrid" %>
<%--<div id="physGrid" style="width:650px;height:450px;overflow:auto">--%>
    <asp:GridView runat="server" GridLines="None" ID="popupGrid" AutoGenerateColumns="False"
        CssClass="grid gv" OnPreRender="popupGrid_PreRender" 
    OnDataBound="popupGrid_DataBound" onrowdatabound="popupGrid_RowDataBound">
        <RowStyle CssClass="gvr"></RowStyle>
        <HeaderStyle CssClass="detailedHeader gvh"></HeaderStyle>
        <Columns>
        <asp:BoundField DataField="Region_Name" HeaderText="" />
            <asp:BoundField DataField="District_Name" HeaderText="" />
            <asp:BoundField DataField="Territory_Name" HeaderText="" />
            <asp:BoundField DataField="Physician_Name" HeaderText="Prescriber Name" />
            <asp:BoundField DataField="Physician_ID" HeaderText="ME Number" />
           <%-- <asp:BoundField DataField="Region_Name" HeaderText="" />
            <asp:BoundField DataField="District_Name" HeaderText="" />
            <asp:BoundField DataField="Territory_Name" HeaderText="" />--%>
            <asp:BoundField DataField="Product_Name" HeaderText="Product" />
        </Columns>
        
        <EmptyDataTemplate>
            There are no records to display.
        </EmptyDataTemplate>
    </asp:GridView>
    <asp:Label ID="gridCount" runat="server" CssClass="grid gridCount" Style="display: block;"></asp:Label>
<%--</div>
<pinso:ThinGrid ID="ThinGrid1" runat="server" AutoLoad="false" StaticHeader="true"
    ContainerID="infoPopup" Target="physGrid" LoadSelector=".grid" Url="~/marketplaceanalytics/all/PrescriberGrid.aspx" />
--%>