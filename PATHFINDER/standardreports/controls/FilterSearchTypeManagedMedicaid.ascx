<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterSearchTypeManagedMedicaid.ascx.cs" Inherits="standardreports_controls_FilterSearchTypeManagedMedicaid" %>

<asp:PlaceHolder runat="server" ID="placeholder">
    <div class="filterGeo">
        <asp:Literal runat="server" ID="filterLabel" Text='Report Type' />
    </div>
    <telerik:RadComboBox runat="server" ID="Search_Type" CssClass="notfilter queryExt" EnableEmbeddedSkins="false" Skin="pathfinder"
        MaxHeight="300px" >
        <Items>
            <telerik:RadComboBoxItem Text="All Accounts" Value="2" />
            <telerik:RadComboBoxItem Text="Account Name" Value="1" />            
            <%--<telerik:RadComboBoxItem Text="Account Manager" Value="3" />--%>
            <%--<telerik:RadComboBoxItem Text="Region" Value="4" Visible="false" />--%>
            <telerik:RadComboBoxItem Text="Top 10" Value="5" />
            <telerik:RadComboBoxItem Text="Top Accounts" Value="6" />
        </Items>
    </telerik:RadComboBox>
    <telerik:RadComboBox runat="server" ID="Plan_ID" Skin="pathfinder" EnableEmbeddedSkins="false" DropDownWidth="375px" >
    </telerik:RadComboBox>
    <pinso:ClientValidator runat="server" id="validator1" target="Plan_ID" DataField="Plan_ID" Required="true" Text='Please Select at least one account.' />
    <%--<telerik:RadComboBox runat="server" ID="Geography_ID" Skin="pathfinder" CssClass="string"  EnableEmbeddedSkins="false" MaxHeight="200px" /> --%>
    
    <!-- control below is hidden - value is set in javascript - if values are changed, javascript in codebehind MUST be modified -->
    <telerik:RadComboBox runat="server" ID="Rank" CssClass="queryExt" EnableEmbeddedSkins="false" Skin="pathfinder"
        MaxHeight="300px" AppendDataBoundItems="true" >
        <Items>
            <telerik:RadComboBoxItem Text="--All--" Value="999999" />
            <telerik:RadComboBoxItem Text="Top 10 Plans" Value="10" />
            <telerik:RadComboBoxItem Text="Top 1000 Plans" Value="1000" />
        </Items>
    </telerik:RadComboBox>
</asp:PlaceHolder>
