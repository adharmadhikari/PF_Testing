<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RestrictionsReportDataTemplate.ascx.cs" Inherits="restrictionsreport_controls_MedicalPharmacyCoverageDataTemplate" %>

<asp:Label ID="geoName" runat="server" Text="" CssClass="reportDataTitle" ></asp:Label>

<div id="tile4TCSummary">
<telerik:RadGrid SkinID="radTable" runat="server" 
 ID="gridRestrictionsReport" AllowSorting="False"  AllowFilteringByColumn="false" AutoGenerateColumns="False"
 AllowPaging="false" EnableEmbeddedSkins="False" GridLines="None" OnPreRender="gridRestrictionsReport_PreRender">
    <MasterTableView AutoGenerateColumns="false" AllowSorting="false" AllowPaging="false" ClientDataKeyNames="Geography_ID, Section_ID, Drug_ID">
        <Columns>
            <telerik:GridBoundColumn DataField="Drug_Name" HeaderStyle-Width="70px" UniqueName="Drug_Name"
                HeaderText='<%$ Resources:Resource, Label_Drug_Name  %>' />
            <telerik:GridBoundColumn DataField="Section_Name" HeaderStyle-Width="70px" UniqueName="Section_Name"
                HeaderText='Section' />
        </Columns>
    </MasterTableView>
    <ClientSettings >
        <Scrolling AllowScroll="true" UseStaticHeaders="True" SaveScrollPosition="true"  />        
    </ClientSettings>
    
</telerik:RadGrid>
</div>