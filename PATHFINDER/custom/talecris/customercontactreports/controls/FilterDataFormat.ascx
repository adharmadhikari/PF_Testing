<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterDataFormat.ascx.cs" Inherits="custom_controls_FilterDataFormat" %>
<%@ Register Namespace="Pathfinder" TagPrefix="pinso" %>
<div class="filterGeo">
    <asp:Literal runat="server" ID="filterLabel" Text="Data Format"  />
</div>
<div> &nbsp;<asp:RadioButton ID ="rdbtn1" value="1" GroupName="dataFormat" Text="Percentage" runat ="server" CssClass="chkBoxDiv"/> 
      &nbsp;<asp:RadioButton ID ="rdbtn2" value="2" GroupName="dataFormat" Text="Numeric Value" runat="server" CssClass="chkBoxDiv" checked/>
</div>