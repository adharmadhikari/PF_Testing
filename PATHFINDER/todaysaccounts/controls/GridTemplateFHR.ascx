<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GridTemplateFHR.ascx.cs" Inherits="todaysaccounts_controls_GridTemplate" %>
    
    <asp:Panel runat="server" id="drillDownContainer" CssClass="drillDownContainer" Height="347px">
    <asp:GridView runat="server" ID="gridTemplate" OnPreRender="drillDownContainer_PreRender" OnRowDataBound="drillDownContainer_RowDataBound" GridLines="None"
        AutoGenerateColumns="False" HeaderStyle-CssClass="detailedHeader" CssClass="grid gv" RowStyle-CssClass="gvr" AlternatingRowStyle-BackColor="#eaf2ff">
        <Columns>             
            <%--<asp:BoundField DataField="Product_ID" HeaderText="&nbsp;" ItemStyle-Width="10px" ControlStyle-Width="10px" HeaderStyle-Width="10px"     />         --%>
            <asp:BoundField DataField="TierChanged" HeaderText="TierChanged" ItemStyle-CssClass="hiddenGrid" HeaderStyle-CssClass="hiddenGrid" />
            <asp:BoundField DataField="PAChanged" HeaderText="PAChanged" ItemStyle-CssClass="hiddenGrid" HeaderStyle-CssClass="hiddenGrid"/>
            <asp:BoundField DataField="QLChanged" HeaderText="QLChanged" ItemStyle-CssClass="hiddenGrid" HeaderStyle-CssClass="hiddenGrid"/>
            <asp:BoundField DataField="STChanged" HeaderText="STChanged" ItemStyle-CssClass="hiddenGrid" HeaderStyle-CssClass="hiddenGrid"/>
            <asp:BoundField DataField="CopayChanged" HeaderText="CopayChanged" ItemStyle-CssClass="hiddenGrid" HeaderStyle-CssClass="hiddenGrid"/>
            <asp:BoundField DataField="FSChanged" HeaderText="FSChanged" ItemStyle-CssClass="hiddenGrid" HeaderStyle-CssClass="hiddenGrid"/>
            <asp:TemplateField HeaderText="Drug Name" HeaderStyle-Width="100px">  
                <ItemTemplate>
                    <asp:Literal runat="server" Text='<%# Eval("Drug_Name") %>' />
                    <input type="hidden" id="dataKey" value='<%# Eval("Drug_ID") %>' />
                </ItemTemplate>
            </asp:TemplateField> 
            
        </Columns>
    </asp:GridView>
    <div class="fhrNote"><div class="fhrNoteBox"></div><asp:Label runat="server" ID="lblNote"/></div>
    </asp:Panel>
    
    <%--<pinso:ThinGrid ID="thinGridTemplate"  Target="drillDownContainer" runat="server" StaticHeader="true" LoadSelector=".grid"  AutoLoad="true" />
--%>