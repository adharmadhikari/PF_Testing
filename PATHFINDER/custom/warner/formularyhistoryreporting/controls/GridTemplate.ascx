<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GridTemplate.ascx.cs" Inherits="custom_warner_formularyhistoryreporting_controls_GridTemplate" %>
<asp:Panel runat="server" id="drillDownContainer" CssClass="drillDownContainer">
    
    <telerik:RadGrid SkinID="radTable" runat="server" 
    ID="gridTemplate" AllowSorting="false" AutoGenerateColumns="false"
     AllowPaging="false" AllowFilteringByColumn="false" 
     EnableEmbeddedSkins="false" 
     MasterTableView-AllowMultiColumnSorting="false">
     <MasterTableView AutoGenerateColumns="false">
        <Columns>

        </Columns>
         <PagerStyle Visible="false" />
     </MasterTableView>        
    <ClientSettings>    
        <Scrolling AllowScroll="True" UseStaticHeaders="True" />
    </ClientSettings>
    
</telerik:RadGrid>
</asp:Panel>
