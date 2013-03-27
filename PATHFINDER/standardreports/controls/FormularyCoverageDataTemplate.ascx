<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FormularyCoverageDataTemplate.ascx.cs" Inherits="standardreports_controls_FormularyCoverageDataTemplate" %>
<asp:Label ID="geoName" runat="server" Text="" CssClass="reportDataTitle" ></asp:Label>

<%--<div id="tile4TCSummary">--%>
<telerik:RadGrid SkinID="radTable" runat="server" 
 ID="gridformularycoverage" AllowSorting="False"  AllowFilteringByColumn="false" AutoGenerateColumns="False"
 AllowPaging="false" EnableEmbeddedSkins="False" GridLines="None">
    <MasterTableView AutoGenerateColumns="false" AllowSorting="false" AllowPaging="false" ClientDataKeyNames="Geography_ID, Drug_ID" >
        <Columns>            
            <telerik:GridBoundColumn DataField="Drug_Name" HeaderStyle-Width="50px" UniqueName="Drug_Name"
                HeaderText='<%$ Resources:Resource, Label_Drug_Name  %>' />
                
              <telerik:GridBoundColumn DataField="Formulary_Lives" HeaderStyle-Width="50px" HeaderText='Pharmacy Lives' SortExpression="Formulary_Lives" UniqueName="Formulary_Lives" DataFormatString="{0:n0}" ItemStyle-CssClass="alignRight pharmacyLives"/>         
          
        </Columns>
    </MasterTableView>
    <ClientSettings >
        <Scrolling AllowScroll="true" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="1"    />        
    </ClientSettings>
    
</telerik:RadGrid>
<%--</div>--%>