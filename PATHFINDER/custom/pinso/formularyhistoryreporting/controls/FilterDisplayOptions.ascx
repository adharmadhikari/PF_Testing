<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterDisplayOptions.ascx.cs" Inherits="custom_pinso_formularyhistoryreporting_controls_FilterDisplayOptions" %>
<div class="filterGeo">
    <asp:Literal runat="server" ID="filterLabel" Text="Change Display Options" />
</div>
    
<telerik:RadComboBox runat="server" ID="Display_ID" DataTextField="Name" DataValueField="ID" 
      AppendDataBoundItems="true" Skin="pathfinder" EnableEmbeddedSkins="false" MaxHeight="300px" OnClientLoad="LoadDisplayOptions"></telerik:RadComboBox>

 <div class="filterGeo">
        <asp:Literal runat="server" ID="Literal1" Text='Benefit Design' />
    </div>
    <telerik:RadComboBox runat="server" ID="Is_Predominant" EnableEmbeddedSkins="false" Skin="pathfinder"
        MaxHeight="300px">
        <Items>
            <telerik:RadComboBoxItem Text="--All--" Value="" />
            <telerik:RadComboBoxItem Text="Predominant" Value="1" />           
        </Items>
    </telerik:RadComboBox>
    
<div class="filterGeo">
    <asp:CheckBox ID="chk_ViewChangesOnly" runat="server" Checked="false" CssClass="queryExt" Text=" View changes only" />
     
</div>

<%--<div class="filterGeo">
    <asp:CheckBox ID="Is_Predominant" runat="server" Checked="false" Text=" Predominant Benefit Design only " />     
</div>--%>


      
