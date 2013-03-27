<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GridTemplate.ascx.cs" Inherits="prescriberreporting_controls_GridTemplate" %>
<asp:Panel runat="server" id="drillDownContainer" CssClass="drillDownContainer">
    <asp:GridView runat="server" ID="gridTemplate" OnDataBound="drillDownContainer_DataBound" OnPreRender="drillDownContainer_PreRender" GridLines="None"
        AutoGenerateColumns="False" HeaderStyle-CssClass="detailedHeader" CssClass="grid gv" RowStyle-CssClass="gvr">
        <Columns>             
            <asp:BoundField DataField="Product_ID" HeaderText="&nbsp;" ItemStyle-Width="10px" ControlStyle-Width="10px" HeaderStyle-Width="10px"    />          
            <asp:TemplateField HeaderText="Drug Name">  
                <ItemTemplate>
                    <asp:Literal runat="server" Text='<%# Eval("Product_Name") %>' />
                    <input type="hidden" id="dataKey" value='<%# Eval("Product_ID") %>' />
                </ItemTemplate>
            </asp:TemplateField> 
        </Columns>
    </asp:GridView>
</asp:Panel>
<pinso:ThinGrid ID="thinGridTemplate"  Target="drillDownContainer" runat="server" StaticHeader="true" OnClick="gridEvent" LoadSelector=".grid"  AutoLoad="true" />
