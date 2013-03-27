<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterFHRTimeFrame.ascx.cs" Inherits="marketplaceanalytics_controls_FilterFHRTimeFrame" %>
<div> 
    <div class="filterGeo">
    <asp:Literal runat="server" ID="filterLabel" Text="Timeframe" />
    </div>
    <telerik:RadComboBox runat="server" ID="Timeframe"  DataTextField="Name" DataValueField="ID" AppendDataBoundItems="true" Skin="pathfinder" EnableEmbeddedSkins="false" DropDownWidth="175px" ></telerik:RadComboBox>
    <%--<pinso:ClientValidator id="valTimeframe" runat="server" Target="Timeframe" Required="true" Text="Please select a time frame." DataField="Timeframe"/> --%>  
</div>